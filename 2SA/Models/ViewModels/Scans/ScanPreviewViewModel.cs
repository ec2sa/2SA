using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eArchiver.Models.ViewModels.Scans
{
    public class ScanPreviewViewModel
    {
        public Scan Scan { get; set; }
        public int Rotation { get; set; }

        public string Random()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
