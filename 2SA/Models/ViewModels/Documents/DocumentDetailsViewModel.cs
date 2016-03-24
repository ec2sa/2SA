using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eArchiver.Models.Entities;
using eArchiver.Models.Repositories.Documents;
using eArchiver.Helpers;

namespace eArchiver.Models.ViewModels.Documents
{
    public class DocumentDetailsViewModel
    {
        public bool AllowDocumentDisplay { get; set; }
        public string DenyMessage { get; set; }
        
        public DocumentDetails Details { get; set; }

        public bool HasVersions(Guid scanId)
        {
            return ScansHelper.HasVersions(scanId);
        }
    }
}
