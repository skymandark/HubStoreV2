using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.ViewModels.StockInViewModels;

namespace Core.ViewModels.DirectStockInViewModels
{
    public class DirectStockInRequestDto
    {
        [Required]
        public DateTime DocDate { get; set; }

        [Required]
        public int BranchId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [MaxLength(50)]
        public string InvoiceNo { get; set; }

        [MaxLength(1000)]
        public string Remarks { get; set; }

        public List<StockInViewModels.SupplierDto> Suppliers { get; set; } = new();
        public List<StockInViewModels.BranchDto> Branches { get; set; } = new();
        public List<StockInViewModels.ItemDto> Items { get; set; } = new();
        public List<DirectStockInDetailDto> StockInDetails { get; set; } = new();
    }

    public class DirectStockInDetailDto
    {
        [Required]
        public int ItemId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "الكمية يجب أن تكون أكبر من صفر")]
        public decimal Qty { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "السعر يجب أن يكون صفر أو أكبر")]
        public decimal Price { get; set; }

        [Range(0, 100, ErrorMessage = "الخصم يجب أن يكون بين 0 و 100")]
        public decimal Discount { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "القيمة الإجمالية يجب أن تكون أكبر من صفر")]
        public decimal TotalValue { get; set; }

        [MaxLength(50)]
        public string BatchNo { get; set; }

        public DateTime? ExpiryDate { get; set; }

        // Navigation properties for UI
        public string ItemName { get; set; }
    }

    public class DirectStockInListDto
    {
        public int StockInId { get; set; }
        public string DocCode { get; set; }
        public string SupplierName { get; set; }
        public string BranchName { get; set; }
        public DateTime DocDate { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public decimal TotalValue { get; set; }
    }

}