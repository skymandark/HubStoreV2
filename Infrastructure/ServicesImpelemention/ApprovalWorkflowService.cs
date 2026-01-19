// ApprovalWorkflowService.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Core.ViewModels.ApprovalViewModels;
using Core.Domin;
using Core.Services.ApprovalServices;
using Infrastructure.Data;

namespace Infrastructure.Services
{
    public class ApprovalWorkflowService : IApprovalWorkflowService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<ApprovalWorkflowService> _logger;

        public ApprovalWorkflowService(ApplicationDbContext context,
            UserManager<AppUser> userManager,
            ILogger<ApprovalWorkflowService> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<ApprovalChainViewModel> GetApprovalChainAsync(string documentType)
        {
            try
            {
                var docType = await _context.DocumentTypes
                    .FirstOrDefaultAsync(dt => dt.Name == documentType);

                if (docType == null)
                    return null;

                var approvalChain = await _context.ApprovalChains
                    .Include(ac => ac.Role)
                    .Where(ac => ac.DocumentTypeId == docType.DocumentTypeId)
                    .OrderBy(ac => ac.StepNumber)
                    .ToListAsync();

                var model = new ApprovalChainViewModel
                {
                    DocumentType = documentType,
                    ApprovalSteps = approvalChain.Select(ac => new ApprovalStepViewModel
                    {
                        StepNumber = ac.StepNumber,
                        RoleName = ac.Role?.Name ?? "غير محدد",
                        IsMandatory = ac.IsMandatory,
                        AllowPartialApproval = ac.AllowPartialApproval
                    }).ToList()
                };

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في جلب سلسلة الموافقات");
                return null;
            }
        }

        public async Task<bool> CheckApprovalPermissionsAsync(string userId, string documentType,
            string action, int? documentId = null)
        {
            try
            {
                // جلب أدوار المستخدم
                var userRoles = await _context.UserRoles
                    .Where(ur => ur.UserId == userId)
                    .Select(ur => ur.RoleId)
                    .ToListAsync();

                if (!userRoles.Any())
                    return false;

                // جلب نوع المستند
                var docType = await _context.DocumentTypes
                    .FirstOrDefaultAsync(dt => dt.Name == documentType);

                if (docType == null)
                    return false;

                // جلب سلسلة الموافقات لهذا النوع
                var ApprovalChains = await _context.ApprovalChains
                    .Where(ac => ac.DocumentTypeId == docType.DocumentTypeId)
                    .ToListAsync();

                if (!ApprovalChains.Any())
                    return false;

                // التحقق إذا كان المستخدم في الخطوة الحالية من سلسلة الموافقات
                if (documentId.HasValue)
                {
                    var currentStep = await GetCurrentApprovalStepAsync(documentId.Value, documentType);
                    var currentChain = ApprovalChains.FirstOrDefault(ac => ac.StepNumber == currentStep);

                    if (currentChain != null && userRoles.Contains(currentChain.RoleId))
                        return true;
                }
                else
                {
                    // إذا لم يتم توفير documentId، التحقق بشكل عام
                    return ApprovalChains.Any(ac => userRoles.Contains(ac.RoleId));
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في التحقق من الصلاحيات");
                return false;
            }
        }

        public async Task<ServiceResult> EscalateApprovalAsync(int documentId, string documentType,
            string reason, string userId)
        {
            try
            {
                // التحقق من الصلاحيات
                var user = await _userManager.FindByIdAsync(userId);
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

                if (!isAdmin)
                    return ServiceResult.Fail("لا تملك صلاحية التصعيد");

                // تسجيل التصعيد في سجل الموافقات
                var docType = await _context.DocumentTypes
                    .FirstOrDefaultAsync(dt => dt.Name == documentType);

                var escalatedStatus = await _context.ApprovalStatuses
                    .FirstOrDefaultAsync(s => s.Code == "Escalated");

                if (docType == null || escalatedStatus == null)
                    return ServiceResult.Fail("بيانات غير مكتملة");

                var escalationHistory = new ApprovalHistory
                {
                    DocumentId = documentId,
                    DocumentTypeId = docType.DocumentTypeId,
                    ApprovalStatusId = escalatedStatus.ApprovalStatusId,
                    ActionByUserId = userId,
                    ActionTimestamp = DateTime.UtcNow,
                    Note = $"تصعيد: {reason}",
                    IsMandatoryNoteProvided = true
                };

                _context.ApprovalHistories.Add(escalationHistory);

                // إرسال إشعار للمسؤولين
                await SendEscalationNotificationAsync(documentId, documentType, reason, userId);

                await _context.SaveChangesAsync();

                return ServiceResult.Success("تم تصعيد الموافقة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في تصعيد الموافقة");
                return ServiceResult.Fail("حدث خطأ أثناء التصعيد", new List<string> { ex.Message });
            }
        }

        public int CalculatePriorityScore(object documentDto)
        {
            int score = 0;

            try
            {
                if (documentDto is Movement movement)
                {
                    // حساب الأولوية بناءً على قيمة الحركة وتاريخها
                    score += (int)(movement.TotalAmount / 1000); // كل 1000 وحدة = 1 نقطة

                    // تحويل PriorityFlag إلى bool إذا لزم الأمر
                    if (movement.IsPriority)
                        score += 20;

                    // نقص النقاط مع مرور الوقت (كل يوم = -1 نقطة)
                    var daysPassed = (DateTime.UtcNow - movement.CreatedDate).Days;
                    score = Math.Max(0, score - daysPassed);
                }
                else if (documentDto is Order order)
                {
                    // استخدام TotalValueCost بدلاً من TotalAmount
                    score += (int)(order.TotalValueCost / 1000);

                    // استخدام PriorityFlag بدلاً من IsPriority
                    if (order.PriorityFlag > 0)
                        score += 20;

                    // استخدام CreatedDate
                    var daysPassed = (DateTime.UtcNow - order.CreatedDate).Days;
                    score = Math.Max(0, score - daysPassed);
                }

                return Math.Min(100, score); // حد أقصى 100 نقطة
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في حساب درجة الأولوية");
                return 0;
            }
        }

        public async Task<bool> IsUserInApprovalChainAsync(string userId, string documentType)
        {
            try
            {
                var userRoles = await _context.UserRoles
                    .Where(ur => ur.UserId == userId)
                    .Select(ur => ur.RoleId)
                    .ToListAsync();

                if (!userRoles.Any())
                    return false;

                var docType = await _context.DocumentTypes
                    .FirstOrDefaultAsync(dt => dt.Name == documentType);

                if (docType == null)
                    return false;

                return await _context.ApprovalChains
                    .AnyAsync(ac => ac.DocumentTypeId == docType.DocumentTypeId &&
                                   userRoles.Contains(ac.RoleId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في التحقق من وجود المستخدم في سلسلة الموافقات");
                return false;
            }
        }

        public async Task<int> GetCurrentApprovalStepAsync(int documentId, string documentType)
        {
            try
            {
                var docType = await _context.DocumentTypes
                    .FirstOrDefaultAsync(dt => dt.Name == documentType);

                if (docType == null)
                    return 0;

                // جلب آخر موافقة للمستند
                var lastApproval = await _context.ApprovalHistories
                    .Include(ah => ah.ApprovalStatus)
                    .Where(ah => ah.DocumentId == documentId &&
                                ah.DocumentTypeId == docType.DocumentTypeId &&
                                (ah.ApprovalStatus.Code == "Approved" ||
                                 ah.ApprovalStatus.Code == "PartiallyApproved"))
                    .OrderByDescending(ah => ah.ActionTimestamp)
                    .FirstOrDefaultAsync();

                if (lastApproval == null)
                    return 1; // الخطوة الأولى إذا لم تكن هناك موافقات سابقة

                // جلب سلسلة الموافقات لهذا النوع
                var approvalChain = await _context.ApprovalChains
                    .Where(ac => ac.DocumentTypeId == docType.DocumentTypeId)
                    .OrderBy(ac => ac.StepNumber)
                    .ToListAsync();

                // إيجاد الخطوة الحالية بناءً على آخر موافقة
                var currentStep = approvalChain
                    .FirstOrDefault(ac => ac.RoleId == lastApproval.ActionRoleId)?.StepNumber ?? 0;

                return currentStep + 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في جلب خطوة الموافقة الحالية");
                return 1;
            }
        }

        private async Task SendEscalationNotificationAsync(int documentId, string documentType,
            string reason, string userId)
        {
            try
            {
                // تنفيذ إرسال الإشعارات للمسؤولين
                var notification = new Notification
                {
                    UserId = userId,
                    Title = $"تصعيد موافقة - {documentType}",
                    Message = $"تم تصعيد موافقة المستند {documentId} بسبب: {reason}",
                    CreatedDate = DateTime.UtcNow,
                    IsRead = false
                };

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في إرسال إشعار التصعيد");
            }
        }
    }
}