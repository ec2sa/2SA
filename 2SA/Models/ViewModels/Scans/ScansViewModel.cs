using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eArchiver.Models;

namespace eArchiver.Models.ViewModels.Scans
{

    public class ScansViewModel
    {
        /// <summary>
        /// Count of available scans in known format (tiff, pdf...)
        /// </summary>
        public int KnownScansCount { get; set; }

        /// <summary>
        /// Count of all available scans (*.*)
        /// </summary>
        public int AllScansCount { get; set; }

        /// <summary>
        /// List of available (already processed) scans
        /// </summary>
        public List<AvailableScan> AvailableScans { get; set; }

        /// <summary>
        /// Created document ID
        /// </summary>
        public Guid? DocumentID { get; set; }

        public List<Category> Categories { get; set; }
        public List<Sender> Senders { get; set; }

        public List<Type2> Types2 { get; set; }

    }
}
