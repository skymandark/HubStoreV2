// ApprovalService.cs
using Microsoft.EntityFrameworkCore;
using Core.ViewModels.ApprovalViewModels;
using Core.Domin;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Core.Services.ApprovalServices;
using Infrastructure.Data;

namespace Infrastructure.Services
{
    public class ApprovalService : IApprovalService
    {
        private readonly ApplicationDbContext _context;
        private readonly IApprovalWorkflowService _workflowService;
        private readonly ILogger<ApprovalService> _logger;
        private readonly UserManager<AppUser> _userManager;

        public ApprovalService(ApplicationDbContext context,
            IApprovalWorkflowService workflowService,
            ILogger<ApprovalService> logger,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _workflowService = workflowService;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<ServiceResult> ApproveDocumentAsync(int documentId, string documentType,
            string userId, string note)
        {
            try
            {
                // التحقق من الصلاحيات
                if (!await _workflowService.CheckApprovalPermissionsAsync(userId, documentType, "Approve", documentId))
                    return ServiceResult.Fail("لا تملك صلاحية الاعتماد");

                // التحقق من الملاحظة الإلزامية
                if (!ValidateMandatoryNote(note))
                    return ServiceResult.Fail("الملاحظة إلزامية للاعتماد");

                // تحديث حالة المستند
                var updateResult = await UpdateDocumentStatusAsync(documentId, documentType, "Approved");
                if (!updateResult.IsSuccess)
                    return updateResult;

                // تسجيل في سجل الموافقات
                await CreateApprovalHistoryAsync(documentId, documentType, "Approved", userId, note, true);

                // تحديث خطوة الموافقة التالية
                await MoveToNextApprovalStepAsync(documentId, documentType);

                // إرسال إشعارات إذا لزم الأمر
                await SendNotificationAsync(documentId, documentType, "Approved", userId);

                return ServiceResult.Success("تم اعتماد المستند بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في اعتماد المستند {DocumentId}", documentId);
                return ServiceResult.Fail("حدث خطأ أثناء الاعتماد", new List<string> { ex.Message });
            }
        }

        public async Task<ServiceResult> RejectDocumentAsync(int documentId, string documentType,
            string userId, string note)
        {
            try
            {
                if (!await _workflowService.CheckApprovalPermissionsAsync(userId, documentType, "Reject", documentId))
                    return ServiceResult.Fail("لا تملك صلاحية الرفض");

                if (!ValidateMandatoryNote(note))
                    return ServiceResult.Fail("الملاحظة إلزامية للرفض");

                var updateResult = await UpdateDocumentStatusAsync(documentId, documentType, "Rejected");
                if (!updateResult.IsSuccess)
                    return updateResult;

                await CreateApprovalHistoryAsync(documentId, documentType, "Rejected", userId, note, true);
                await SendNotificationAsync(documentId, documentType, "Rejected", userId);

                return ServiceResult.Success("تم رفض المستند بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في رفض المستند {DocumentId}", documentId);
                return ServiceResult.Fail("حدث خطأ أثناء الرفض", new List<string> { ex.Message });
            }
        }

        public async Task<ServiceResult> ReturnForEditAsync(int documentId, string documentType,
            string userId, string note)
        {
            try
            {
                if (!await _workflowService.CheckApprovalPermissionsAsync(userId, documentType, "Return", documentId))
                    return ServiceResult.Fail("لا تملك صلاحية الإرجاع للتعديل");

                if (!ValidateMandatoryNote(note))
                    return ServiceResult.Fail("الملاحظة إلزامية للإرجاع للتعديل");

                var updateResult = await UpdateDocumentStatusAsync(documentId, documentType, "PendingEdit");
                if (!updateResult.IsSuccess)
                    return updateResult;

                await CreateApprovalHistoryAsync(documentId, documentType, "ReturnedForEdit", userId, note, true);
                await SendNotificationAsync(documentId, documentType, "ReturnedForEdit", userId);

                return ServiceResult.Success("تم إرجاع المستند للتعديل بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في إرجاع المستند {DocumentId}", documentId);
                return ServiceResult.Fail("حدث خطأ أثناء الإرجاع للتعديل", new List<string> { ex.Message });
            }
        }

        public async Task<ServiceResult> PartialApproveAsync(int documentId, List<int> linesApproved,
            string userId, string note, string documentType)
        {
            try
            {
                if (!await _workflowService.CheckApprovalPermissionsAsync(userId, documentType, "PartialApprove", documentId))
                    return ServiceResult.Fail("لا تملك صلاحية الاعتماد الجزئي");

                if (!ValidateMandatoryNote(note))
                    return ServiceResult.Fail("الملاحظة إلزامية للاعتماد الجزئي");

                // تنفيذ الاعتماد الجزئي حسب نوع المستند
                if (documentType == "Order")
                {
                    await PartialApproveOrderLinesAsync(documentId, linesApproved);
                }
                else if (documentType == "Movement")
                {
                    await PartialApproveMovementLinesAsync(documentId, linesApproved);
                }

                await CreateApprovalHistoryAsync(documentId, documentType, "PartiallyApproved", userId, note, true);

                // التحقق إذا كانت جميع الخطوط معتمدة
                if (await AreAllLinesApprovedAsync(documentId, documentType))
                {
                    await ApproveDocumentAsync(documentId, documentType, userId, note);
                }

                return ServiceResult.Success("تم الاعتماد الجزئي بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في الاعتماد الجزئي للمستند {DocumentId}", documentId);
                return ServiceResult.Fail("حدث خطأ أثناء الاعتماد الجزئي", new List<string> { ex.Message });
            }
        }

        public async Task<List<PendingApprovalViewModel>> GetPendingApprovalsAsync(string userId,
            ApprovalFilterViewModel filters)
        {
            var pendingApprovals = new List<PendingApprovalViewModel>();

            try
            {
                // جلب أدوار المستخدم
                var userRoles = await _context.UserRoles
                    .Where(ur => ur.UserId == userId)
                    .Select(ur => ur.RoleId)
                    .ToListAsync();

                // التحقق من كل نوع مستند
                if (string.IsNullOrEmpty(filters?.DocumentType) || filters.DocumentType == "Movement")
                {
                    var pendingMovements = await GetPendingMovementsAsync(userRoles, filters);
                    pendingApprovals.AddRange(pendingMovements);
                }

                if (string.IsNullOrEmpty(filters?.DocumentType) || filters.DocumentType == "Order")
                {
                    var pendingOrders = await GetPendingOrdersAsync(userRoles, filters);
                    pendingApprovals.AddRange(pendingOrders);
                }

                // تطبيق الفلاتر الإضافية
                if (filters != null)
                {
                    if (filters.PriorityFrom.HasValue)
                        pendingApprovals = pendingApprovals.Where(p => p.PriorityScore >= filters.PriorityFrom.Value).ToList();

                    if (filters.PriorityTo.HasValue)
                        pendingApprovals = pendingApprovals.Where(p => p.PriorityScore <= filters.PriorityTo.Value).ToList();
                }

                return pendingApprovals.OrderByDescending(p => p.PriorityScore).ThenBy(p => p.RequestedDate).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في جلب الموافقات المعلقة");
                return pendingApprovals;
            }
        }

        public async Task<List<ApprovalHistoryViewModel>> GetApprovalHistoryAsync(int documentId, string documentType)
        {
            var history = new List<ApprovalHistoryViewModel>();

            try
            {
                var docType = await _context.DocumentTypes
                    .FirstOrDefaultAsync(dt => dt.Name == documentType);

                if (docType == null)
                    return history;

                var approvalHistories = await _context.ApprovalHistories
                    .Where(ah => ah.DocumentId == documentId && ah.DocumentTypeId == docType.DocumentTypeId)
                    .Include(ah => ah.ApprovalStatus)
                    .Include(ah => ah.ActionByUser)
                    .OrderByDescending(ah => ah.ActionTimestamp)
                    .ToListAsync();

                history = approvalHistories.Select(ah => new ApprovalHistoryViewModel
                {
                    ApprovalHistoryId = ah.ApprovalHistoryId,
                    DocumentId = ah.DocumentId,
                    DocumentType = documentType,
                    ApprovalStatus = ah.ApprovalStatus?.Name ?? "غير معروف",
                    ActionByUserId = ah.ActionByUserId,
                    ActionByUserName = ah.ActionByUser?.UserName ?? "غير معروف",
                    ActionRoleId = ah.ActionRoleId,
                    ActionTimestamp = ah.ActionTimestamp,
                    Note = ah.Note,
                    IsMandatoryNoteProvided = ah.IsMandatoryNoteProvided
                }).ToList();

                return history;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في جلب تاريخ الموافقات");
                return history;
            }
        }

        public bool ValidateMandatoryNote(string note, bool isMandatoryAction = true)
        {
            if (!isMandatoryAction)
                return true;

            return !string.IsNullOrWhiteSpace(note) && note.Trim().Length >= 10;
        }

        public async Task<ServiceResult> BulkApproveAsync(List<int> documentIds, string userId,
            string note, string documentType)
        {
            var results = new List<string>();
            var successCount = 0;

            try
            {
                foreach (var documentId in documentIds)
                {
                    var result = await ApproveDocumentAsync(documentId, documentType, userId, note);
                    if (result.IsSuccess)
                        successCount++;
                    else
                        results.Add($"المستند {documentId}: {result.Message}");
                }

                return ServiceResult.Success($"تم معالجة {successCount} من أصل {documentIds.Count} مستند",
                    new { SuccessCount = successCount, Errors = results });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في الموافقة الجماعية");
                return ServiceResult.Fail("حدث خطأ أثناء الموافقة الجماعية", new List<string> { ex.Message });
            }
        }

        #region Private Helper Methods

        private async Task<ServiceResult> UpdateDocumentStatusAsync(int documentId, string documentType, string statusCode)
        {
            try
            {
                var status = await _context.ApprovalStatuses
                    .FirstOrDefaultAsync(s => s.Code == statusCode);

                if (status == null)
                    return ServiceResult.Fail("حالة الموافقة غير موجودة");

                if (documentType == "Movement")
                {
                    var movement = await _context.Movements
                        .Include(m => m.ApprovalStatus)
                        .FirstOrDefaultAsync(m => m.MovementId == documentId);

                    if (movement == null)
                        return ServiceResult.Fail("الحركة غير موجودة");

                    movement.ApprovalStatusId = status.ApprovalStatusId;

                    // التحقق من وجود خاصية LastModifiedDate وإضافتها إذا كانت مفقودة
                    var movementType = movement.GetType();
                    var lastModifiedProperty = movementType.GetProperty("LastModifiedDate");
                    if (lastModifiedProperty != null && lastModifiedProperty.CanWrite)
                    {
                        lastModifiedProperty.SetValue(movement, DateTime.UtcNow);
                    }
                }
                else if (documentType == "Order")
                {
                    var order = await _context.Orders
                        .Include(o => o.ApprovalStatus)
                        .FirstOrDefaultAsync(o => o.OrderId == documentId);

                    if (order == null)
                        return ServiceResult.Fail("الطلب غير موجود");

                    order.ApprovalStatusId = status.ApprovalStatusId;

                    // التحقق من وجود خاصية LastModifiedDate وإضافتها إذا كانت مفقودة
                    var orderType = order.GetType();
                    var lastModifiedProperty = orderType.GetProperty("LastModifiedDate");
                    if (lastModifiedProperty != null && lastModifiedProperty.CanWrite)
                    {
                        lastModifiedProperty.SetValue(order, DateTime.UtcNow);
                    }
                }
                else
                {
                    return ServiceResult.Fail("نوع المستند غير مدعوم");
                }

                await _context.SaveChangesAsync();
                return ServiceResult.Success("تم تحديث حالة المستند بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في تحديث حالة المستند");
                return ServiceResult.Fail("حدث خطأ في تحديث الحالة", new List<string> { ex.Message });
            }
        }

        private async Task CreateApprovalHistoryAsync(int documentId, string documentType,
            string statusCode, string userId, string note, bool isMandatoryNoteProvided)
        {
            var docType = await _context.DocumentTypes
                .FirstOrDefaultAsync(dt => dt.Name == documentType);

            var status = await _context.ApprovalStatuses
                .FirstOrDefaultAsync(s => s.Code == statusCode);

            // جلب دور المستخدم
            var userRoles = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .ToListAsync();

            var userRoleId = userRoles.FirstOrDefault()?.RoleId;

            var approvalHistory = new ApprovalHistory
            {
                DocumentId = documentId,
                DocumentTypeId = docType?.DocumentTypeId ?? 0,
                ApprovalStatusId = status?.ApprovalStatusId ?? 0,
                ActionByUserId = userId,
                ActionTimestamp = DateTime.UtcNow,
                Note = note,
                IsMandatoryNoteProvided = isMandatoryNoteProvided,
                ActionRoleId = userRoleId
            };

            _context.ApprovalHistories.Add(approvalHistory);
            await _context.SaveChangesAsync();
        }

        private async Task MoveToNextApprovalStepAsync(int documentId, string documentType)
        {
            // منطق التحرك للخطوة التالية في سلسلة الموافقات
            // يمكن حفظها في جدول منفصل للمستندات قيد المعالجة
            await Task.CompletedTask;
        }

        private async Task SendNotificationAsync(int documentId, string documentType,
            string action, string userId)
        {
            // إرسال إشعارات للمستخدمين المعنيين
            // يمكن استخدام خدمة الإشعارات هنا
            await Task.CompletedTask;
        }

        private async Task<bool> AreAllLinesApprovedAsync(int documentId, string documentType)
        {
            if (documentType == "Order")
            {
                var order = await _context.Orders
                    .Include(o => o.OrderLines)
                    .FirstOrDefaultAsync(o => o.OrderId == documentId);

                // التحقق من وجود خاصية IsApproved في OrderLine
                if (order?.OrderLines != null)
                {
                    var orderLineType = typeof(OrderLine);
                    var isApprovedProperty = orderLineType.GetProperty("IsApproved");

                    if (isApprovedProperty != null)
                    {
                        return order.OrderLines.All(ol =>
                            (bool?)isApprovedProperty.GetValue(ol) == true);
                    }
                }
            }

            return true;
        }

        private async Task PartialApproveOrderLinesAsync(int orderId, List<int> lineIds)
        {
            var orderLines = await _context.OrderLines
                .Where(ol => ol.OrderId == orderId && lineIds.Contains(ol.OrderLineId))
                .ToListAsync();

            var orderLineType = typeof(OrderLine);
            var isApprovedProperty = orderLineType.GetProperty("IsApproved");
            var approvedDateProperty = orderLineType.GetProperty("ApprovedDate");

            foreach (var line in orderLines)
            {
                if (isApprovedProperty != null && isApprovedProperty.CanWrite)
                {
                    isApprovedProperty.SetValue(line, true);
                }

                if (approvedDateProperty != null && approvedDateProperty.CanWrite)
                {
                    approvedDateProperty.SetValue(line, DateTime.UtcNow);
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task PartialApproveMovementLinesAsync(int movementId, List<int> lineIds)
        {
            var movementLines = await _context.MovementLines
                .Where(ml => ml.MovementId == movementId && lineIds.Contains(ml.MovementLineId))
                .ToListAsync();

            var movementLineType = typeof(MovementLine);
            var isApprovedProperty = movementLineType.GetProperty("IsApproved");
            var approvedDateProperty = movementLineType.GetProperty("ApprovedDate");

            foreach (var line in movementLines)
            {
                if (isApprovedProperty != null && isApprovedProperty.CanWrite)
                {
                    isApprovedProperty.SetValue(line, true);
                }

                if (approvedDateProperty != null && approvedDateProperty.CanWrite)
                {
                    approvedDateProperty.SetValue(line, DateTime.UtcNow);
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task<List<PendingApprovalViewModel>> GetPendingMovementsAsync(List<string> userRoles, ApprovalFilterViewModel filters)
        {
            var query = _context.Movements
                .Include(m => m.ApprovalStatus)
                .Include(m => m.CreatedByUser)
                .Where(m => m.ApprovalStatus.Code == "Pending")
                .AsQueryable();

            // تطبيق الفلاتر التاريخية
            if (filters?.DateFrom.HasValue == true && filters.DateTo.HasValue == true)
            {
                query = query.Where(m => m.CreatedDate >= filters.DateFrom.Value &&
                                         m.CreatedDate <= filters.DateTo.Value);
            }

            var pendingMovements = await query.ToListAsync();
            var result = new List<PendingApprovalViewModel>();

            foreach (var movement in pendingMovements)
            {
                // حساب درجة الأولوية
                var priorityScore = _workflowService.CalculatePriorityScore(movement);

                result.Add(new PendingApprovalViewModel
                {
                    DocumentId = movement.MovementId,
                    DocumentCode = movement.MovementCode ?? $"MOV-{movement.MovementId}",
                    DocumentType = "Movement",
                    RequestedDate = movement.CreatedDate,
                    PriorityScore = priorityScore,
                    CurrentStatus = movement.ApprovalStatus?.Name ?? "معلق"
                });
            }

            return result;
        }

        private async Task<List<PendingApprovalViewModel>> GetPendingOrdersAsync(List<string> userRoles, ApprovalFilterViewModel filters)
        {
            var query = _context.Orders
                .Include(o => o.ApprovalStatus)
                .Include(o => o.RequestedByUser)
                .Where(o => o.ApprovalStatus.Code == "Pending")
                .AsQueryable();

            // تطبيق الفلاتر التاريخية
            if (filters?.DateFrom.HasValue == true && filters.DateTo.HasValue == true)
            {
                query = query.Where(o => o.CreatedDate >= filters.DateFrom.Value &&
                                         o.CreatedDate <= filters.DateTo.Value);
            }

            var pendingOrders = await query.ToListAsync();
            var result = new List<PendingApprovalViewModel>();

            foreach (var order in pendingOrders)
            {
                var priorityScore = _workflowService.CalculatePriorityScore(order);

                result.Add(new PendingApprovalViewModel
                {
                    DocumentId = order.OrderId,
                    DocumentCode = order.OrderCode ?? $"ORD-{order.OrderId}",
                    DocumentType = "Order",
                    RequestedDate = order.CreatedDate,
                    PriorityScore = priorityScore,
                    CreatedByUserName = order.RequestedByUser?.UserName ?? "غير معروف",
                    CurrentStatus = order.ApprovalStatus?.Name ?? "معلق"
                });
            }

            return result;
        }

        #endregion
    }
}