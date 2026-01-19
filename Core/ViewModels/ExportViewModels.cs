using System;
using System.Collections.Generic;

namespace Core.ViewModels.ExportViewModels
{
    public class ExportResultViewModel
    {
        public string FileReference { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public int RowCount { get; set; }
        public DateTime ExportDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string DownloadUrl { get; set; }
    }
}
