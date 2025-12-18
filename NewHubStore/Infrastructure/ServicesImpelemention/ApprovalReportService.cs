using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Services.ReportingServices;
using Core.ViewModels.ReportViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ServicesImpelemention
{
    public class ApprovalReportService : IApprovalReportService
    {
        private readonly ApplicationDbContext _context;
        private DateTime currentDate;

        public ApprovalReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApprovalMetricsViewModel> GetApprovalMetrics(DateTime? dateFrom, DateTime? dateTo)
        {
            var query = _context.ApprovalHistories
                .Include(ah => ah.ApprovalStatus)
                .Include(ah => ah.DocumentType)
                .AsQueryable();

            if (dateFrom.HasValue)
                query = query.Where(ah => ah.ActionTimestamp >= dateFrom.Value); // ActionTimestamp بدلاً من ActionDate

            if (dateTo.HasValue)
                query = query.Where(ah => ah.ActionTimestamp <= dateTo.Value); // ActionTimestamp بدلاً من ActionDate

            var histories = await query.ToListAsync();

            // تجميع حسب المستند للحصول على الحالة النهائية
            var documentStatuses = histories
                .GroupBy(ah => new { ah.DocumentId, ah.DocumentTypeId })
                .Select(g => g.OrderByDescending(ah => ah.ActionTimestamp).First()) // ActionTimestamp
                .ToList();

            var totalDocuments = documentStatuses.Count;
            var totalApproved = documentStatuses.Count(d => d.ApprovalStatus.Name == "Approved");
            var totalRejected = documentStatuses.Count(d => d.ApprovalStatus.Name == "Rejected");
            var totalPending = documentStatuses.Count(d => d.ApprovalStatus.Name == "Pending");

            // لحساب متوسط وقت الموافقة، نحتاج معرفة وقت إنشاء المستند
            // يجب أن يكون هناك طريقة للربط مع المستند الأصلي
            double averageApprovalTime = 0;

            // يمكنك إضافة منطق لحساب وقت الإنشاء من المستندات المرتبطة
            // مثلاً: Movements أو Orders أو غيرها حسب DocumentTypeId

            // تنفيذ مبسط:
            var approvedDocuments = documentStatuses.Where(d => d.ApprovalStatus.Name == "Approved").ToList();
            if (approvedDocuments.Any())
            {
                // يمكنك ربط هذا بجدول المستندات الفعلي
                averageApprovalTime = 0; // قم بتنفيذ المنطق المناسب
            }

            double approvalRate = totalDocuments > 0 ?
                ((double)totalApproved / totalDocuments) * 100 : 0;

            return new ApprovalMetricsViewModel
            {
                TotalDocumentsSubmitted = totalDocuments,
                TotalApproved = totalApproved,
                TotalRejected = totalRejected,
                TotalPending = totalPending,
                AverageApprovalTime = Math.Round(averageApprovalTime, 2),
                ApprovalRate = Math.Round(approvalRate, 2)
            };
        }

        public async Task<ApprovalBottlenecksViewModel> GetApprovalBottlenecks(
            ReportFilterViewModel filters)
        {
            // هناك خطأ: PendingApproval ليس كيان في DbContext، إنه ViewModel
            // يجب الحصول على البيانات من ApprovalHistory والتركيز على المستندات المعلقة

            // اقتراح بديل: الحصول على المستندات المعلقة من جداول المستندات الفعلية
            var currentDate = DateTime.UtcNow;

            // 1. الحصول على المستندات المعلقة من Movements
            var pendingMovements = await _context.Movements
                .Include(m => m.ApprovalStatus)
                .Include(m => m.MovementType)
                .Where(m => m.ApprovalStatus.Name == "Pending")
                .ToListAsync();

            // 2. الحصول على المستندات المعلقة من Orders
            var pendingOrders = await _context.Orders
                .Include(o => o.ApprovalStatus)
                .Include(o => o.OrderType)
                .Where(o => o.ApprovalStatus.Name == "Pending")
                .ToListAsync();

            var bottlenecks = new List<BottleneckItemViewModel>();

            // تجميع حسب نوع المستند والخطوة الحالية
            if (pendingMovements.Any())
            {
                // يمكنك تجميع حسب نوع الحركة أو الفرع
                bottlenecks.Add(new BottleneckItemViewModel
                {
                    ApprovalRole = "Movement Approvers",
                    PendingDocuments = pendingMovements.Count,
                    AverageWaitingTime = Math.Round(pendingMovements
                        .Average(m => (currentDate - m.CreatedDate).TotalHours), 2),
                    CriticalItems = string.Join(", ", pendingMovements
                        .OrderByDescending(m => (currentDate - m.CreatedDate).TotalHours)
                        .Take(5)
                        .Select(m => $"حركة #{m.MovementId}"))
                });
            }

            if (pendingOrders.Any())
            {
                bottlenecks.Add(new BottleneckItemViewModel
                {
                    ApprovalRole = "Order Approvers",
                    PendingDocuments = pendingOrders.Count,
                    AverageWaitingTime = Math.Round(pendingOrders
                        .Average(o => (currentDate - o.CreatedDate).TotalHours), 2),
                    CriticalItems = string.Join(", ", pendingOrders
                        .OrderByDescending(o => (currentDate - o.CreatedDate).TotalHours)
                        .Take(5)
                        .Select(o => $"طلب #{o.OrderId}"))
                });
            }

            return new ApprovalBottlenecksViewModel
            {
                Bottlenecks = bottlenecks.OrderByDescending(b => b.PendingDocuments).ToList()
            };
        }

        public async Task<UserApprovalActivityViewModel> GetUserApprovalActivity(
            string userId, DateTime? dateFrom, DateTime? dateTo)
        {
            var query = _context.ApprovalHistories
                .Include(ah => ah.ApprovalStatus)
                .Include(ah => ah.ActionByUser)
                .Where(ah => ah.ActionByUserId == userId) // ActionByUserId بدلاً من ActionBy
                .AsQueryable();

            if (dateFrom.HasValue)
                query = query.Where(ah => ah.ActionTimestamp >= dateFrom.Value); // ActionTimestamp

            if (dateTo.HasValue)
                query = query.Where(ah => ah.ActionTimestamp <= dateTo.Value); // ActionTimestamp

            var userActions = await query.ToListAsync();
            var user = await _context.Users.FindAsync(userId);

            var approvedCount = userActions.Count(ah => ah.ApprovalStatus.Name == "Approved");
            var rejectedCount = userActions.Count(ah => ah.ApprovalStatus.Name == "Rejected");

            // لحساب متوسط وقت الموافقة، نحتاج وقت إنشاء المستند
            // هذا يتطلب ربط ApprovalHistory بالمستند الأصلي
            double averageTime = 0;
            var approvalActions = userActions
                .Where(ah => ah.ApprovalStatus.Name == "Approved" || ah.ApprovalStatus.Name == "Rejected")
                .ToList();

            if (approvalActions.Any())
            {
                // تنفيذ مبسط - يمكنك تحسينه بربط المستندات
                averageTime = approvalActions
                    .Average(ah => (currentDate - ah.ActionTimestamp).TotalHours); // استخدام ActionTimestamp
            }

            var lastApprovalDate = userActions
                .Where(ah => ah.ApprovalStatus.Name == "Approved")
                .OrderByDescending(ah => ah.ActionTimestamp) // ActionTimestamp
                .FirstOrDefault()?.ActionTimestamp ?? DateTime.MinValue; // ActionTimestamp

            return new UserApprovalActivityViewModel
            {
                UserId = userId,
                UserName = user?.UserName,
                DocumentsApproved = approvedCount,
                DocumentsRejected = rejectedCount,
                AverageTimePerApproval = Math.Round(averageTime, 2),
                LastApprovalDate = lastApprovalDate
            };
        }

        // دالة مساعدة للحصول على وقت إنشاء المستند حسب نوعه
        private async Task<DateTime?> GetDocumentCreatedDate(int documentId, int documentTypeId)
        {
            // يمكنك إضافة المنطق المناسب حسب نوع المستند
            switch (documentTypeId)
            {
                case 1: // فرضاً: Movement
                    var movement = await _context.Movements.FindAsync(documentId);
                    return movement?.CreatedDate;

                case 2: // فرضاً: Order
                    var order = await _context.Orders.FindAsync(documentId);
                    return order?.CreatedDate;

                default:
                    return null;
            }
        }
    }
}