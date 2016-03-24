using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eArchiver.Models.Repositories.Scans;

namespace eArchiver.Models.ViewModels.Scans
{
    public class ScansBinViewModel
    {
        public List<Guid> ScansList { get; set; }

        public string GetFileName(Guid scanGuid)
        {
            var sp = new ScansRepository().GetScans().Where(s => s.ScanID == scanGuid).FirstOrDefault();
            if (sp == null)
                return string.Empty;
            return sp.FileName;
        }

        public DateTime GetImportDate(Guid scanGuid)
        {
            var sp = new ScansRepository().GetScans().Where(s => s.ScanID == scanGuid).FirstOrDefault();
            if (sp == null || !sp.importDate.HasValue)
                return new DateTime();
            return sp.importDate.Value;
        }
    }
}
