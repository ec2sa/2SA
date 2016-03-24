using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eArchiver.Models.ViewModels.Scans
{
    public class ScanSelectorViewModel
    {
        /// <summary>
        /// List of available (already processed) scans
        /// </summary>
        public List<AvailableScan> AvailableScans { get; set; }

        /// <summary>
        /// Count of available scans in known format (tiff, pdf...)
        /// </summary>
        public int KnownScansCount { get; set; }

        /// <summary>
        /// Count of all available scans (*.*)
        /// </summary>
        public int AllScansCount { get; set; }
    }
}
