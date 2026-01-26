using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Domin;

namespace Core.ViewModels.PurchaseOrderViewModels
{
    public class PurchaseOrderRequestDto
    {
        public int PurchaseOrderId { get; set; }
        
        [Required(ErrorMessage = "يجب اختيار نوع الإيصال")]
        public ReceiptType ReceiptType { get; set; }
        
        [StringLength(50, ErrorMessage = "كود طلب الشراء يجب ألا يزيد عن 50 حرفاً")]
        public string PurchaseOrderCode { get; set; }
        
        [Required(ErrorMessage = "تاريخ المستند مطلوب")]
        [DataType(DataType.Date, ErrorMessage = "صيغة التاريخ غير صحيحة")]
        [Display(Name = "تاريخ المستند")]
        public DateTime DocDate { get; set; }
        
        [DataType(DataType.Date, ErrorMessage = "صيغة التاريخ غير صحيحة")]
        [Display(Name = "تاريخ الإدخال")]
        public DateTime EntryDate { get; set; }
        
        [Required(ErrorMessage = "يجب اختيار المورد")]
        [Range(1, int.MaxValue, ErrorMessage = "يجب اختيار مورد صحيح")]
        [Display(Name = "المورد")]
        public int SupplierId { get; set; }
        
        [StringLength(200, ErrorMessage = "اسم المورد يجب ألا يزيد عن 200 حرف")]
        public string SupplierName { get; set; }
        
        [StringLength(100, ErrorMessage = "اسم مندوب المورد يجب ألا يزيد عن 100 حرف")]
        [Display(Name = "مندوب المورد")]
        public string SupplierDelegate { get; set; }
        
        [StringLength(50, ErrorMessage = "رقم الفاتورة يجب ألا يزيد عن 50 حرف")]
        [Display(Name = "رقم الفاتورة")]
        public string InvoiceId { get; set; }
        
        [Required(ErrorMessage = "يجب اختيار الفرع")]
        [Range(1, int.MaxValue, ErrorMessage = "يجب اختيار فرع صحيح")]
        [Display(Name = "الفرع")]
        public int BranchId { get; set; }
        
        [StringLength(200, ErrorMessage = "اسم الفرع يجب ألا يزيد عن 200 حرف")]
        public string BranchName { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "يجب اختيار مستودع صحيح")]
        [Display(Name = "المستودع")]
        public int? BranchStockId { get; set; }
        
        [StringLength(100, ErrorMessage = "المرجع يجب ألا يزيد عن 100 حرف")]
        [Display(Name = "المرجع")]
        public string Reference { get; set; }
        
        [DataType(DataType.Date, ErrorMessage = "صيغة التاريخ غير صحيحة")]
        [Display(Name = "تاريخ التسليم")]
        public DateTime? DeliveryDate { get; set; }
        
        [Display(Name = "سداد آجل")]
        public bool CreditPayment { get; set; }
        
        [DataType(DataType.Date, ErrorMessage = "صيغة التاريخ غير صحيحة")]
        [Display(Name = "تاريخ الاستحقاق")]
        public DateTime? DueDate { get; set; }
        
        [Range(0, 365, ErrorMessage = "مدة السداد يجب أن تكون بين 0 و 365 يوم")]
        [Display(Name = "مدة السداد (أيام)")]
        public int? PaymentPeriodDays { get; set; }
        
        [StringLength(1000, ErrorMessage = "الملاحظات يجب ألا تزيد عن 1000 حرف")]
        [Display(Name = "ملاحظات")]
        public string Remarks { get; set; }
        
        [StringLength(50, ErrorMessage = "الحالة يجب ألا تزيد عن 50 حرف")]
        public string Status { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "معرف الحالة غير صحيح")]
        [Display(Name = "نظام الموافقات")]
        public int? StatusId { get; set; } // For auto approval: 1 = auto approve, 2 = manual approval, 3 = conditional approval
        
        [Range(0, double.MaxValue, ErrorMessage = "القيمة الإجمالية يجب أن تكون قيمة موجبة")]
        [Display(Name = "القيمة الإجمالية")]
        public decimal TotalValue { get; set; }
        
        [Required(ErrorMessage = "يجب إضافة صنف واحد على الأقل")]
        [MinLength(1, ErrorMessage = "يجب إضافة صنف واحد على الأقل")]
        public List<PurchaseOrderDetailDto> PurchaseOrderDetails { get; set; }
        // Dropdown data
        public List<SupplierDto> Suppliers { get; set; }
        public List<BranchDto> Branches { get; set; }
        public List<ItemDto> Items { get; set; }
        public List<ItemUnitDto> ItemPackages { get; set; }
        public List<ApprovalSystemOptionDto> ApprovalSystemOptions { get; set; }
    }

    public class PurchaseOrderDetailDto
    {
        public int PurchaseOrderDetailId { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "رقم التسلسل يجب أن يكون قيمة موجبة")]
        [Display(Name = "رقم التسلسل")]
        public int LineSerialNumber { get; set; }
        
        [Required(ErrorMessage = "يجب اختيار الصنف")]
        [Range(1, int.MaxValue, ErrorMessage = "يجب اختيار صنف صحيح")]
        [Display(Name = "الصنف")]
        public int ItemId { get; set; }
        
        [StringLength(200, ErrorMessage = "اسم الصنف يجب ألا يزيد عن 200 حرف")]
        public string ItemName { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "يجب اختيار عبوة صحيحة")]
        [Display(Name = "عبوة الصنف")]
        public int? ItemPackageId { get; set; }
        
        [Required(ErrorMessage = "الكمية مطلوبة")]
        [Range(0.001, double.MaxValue, ErrorMessage = "الكمية يجب أن تكون أكبر من صفر")]
        [Display(Name = "الكمية")]
        public decimal Quantity { get; set; }
        
        [Required(ErrorMessage = "السعر مطلوب")]
        [Range(0.01, double.MaxValue, ErrorMessage = "السعر يجب أن يكون أكبر من صفر")]
        [Display(Name = "السعر")]
        public decimal Price { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "القيمة الإجمالية يجب أن تكون قيمة موجبة")]
        [Display(Name = "القيمة الإجمالية")]
        public decimal TotalValue { get; set; }
        
        [Range(0, 100, ErrorMessage = "الخصم الرئيسي يجب أن يكون بين 0 و 100%")]
        [Display(Name = "الخصم الرئيسي (%)")]
        public decimal MainDiscountPercent { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "قيمة الخصم الرئيسي يجب أن تكون قيمة موجبة")]
        [Display(Name = "قيمة الخصم الرئيسي")]
        public decimal MainDiscountValue { get; set; }
        
        [Range(0, 100, ErrorMessage = "الخصم الإضافي يجب أن يكون بين 0 و 100%")]
        [Display(Name = "الخصم الإضافي (%)")]
        public decimal AddedDiscountPercent { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "قيمة الخصم الإضافي يجب أن تكون قيمة موجبة")]
        [Display(Name = "قيمة الخصم الإضافي")]
        public decimal AddedDiscountValue { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "الكمية الإضافية يجب أن تكون قيمة موجبة")]
        [Display(Name = "الكمية الإضافية")]
        public decimal BonusQuantity { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "الكمية المستلمة يجب أن تكون قيمة موجبة")]
        [Display(Name = "الكمية المستلمة")]
        public decimal ReceivedQuantity { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "قيمة الضريبة يجب أن تكون قيمة موجبة")]
        [Display(Name = "قيمة الضريبة")]
        public decimal VatValue { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "القيمة الصافية يجب أن تكون قيمة موجبة")]
        [Display(Name = "القيمة الصافية")]
        public decimal NetValue { get; set; }

        public decimal AvailableStock { get; set; }
        
        [StringLength(500, ErrorMessage = "ملاحظات الصنف يجب ألا تزيد عن 500 حرف")]
        [Display(Name = "ملاحظات الصنف")]
        public string Remarks { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "سعر التكلفة يجب أن يكون قيمة موجبة")]
        [Display(Name = "سعر التكلفة")]
        public decimal CostPrice { get; set; }
    }

    public class SupplierDto
    {
        public int SupplierId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class BranchDto
    {
        public int BranchId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class ItemDto
    {
        public int ItemId { get; set; }
        public int BranchId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameArab { get; set; }
    }

    public class ItemUnitDto
    {
        public int ItemUnitId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }


    // For listing
    public class PurchaseOrderListDto
    {
        public int PurchaseOrderId { get; set; }
        public string PurchaseOrderCode { get; set; }
        public string Reference { get; set; }
        public string SupplierName { get; set; }
        public string BranchName { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime DocDate { get; set; }
        public string Stage { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public decimal TotalValue { get; set; }
        public int? StatusId { get; set; } // Approval system type
    }

    // Approval system options
    public class ApprovalSystemOptionDto
    {
        public int StatusId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool RequiresApproval { get; set; }
    }
}