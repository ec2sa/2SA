using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eArchiver.Models.Entities;

namespace eArchiver.Models.ViewModels.Documents
{
    public class DocumentScansViewModel
    {
        public int ScansCount { get; set; }
        public Guid ScanGuid { get; set; }
        public Scan Scan { get; set; }
        public int CurrentScanIndex { get; set; }

        public DocumentDetails Details{ get; set; }
    }
}
