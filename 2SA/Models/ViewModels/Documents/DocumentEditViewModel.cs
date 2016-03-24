using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eArchiver.Models.Entities;
using eArchiver.Models.ViewModels.Scans;
using eArchiver.Models.Repositories.Documents;

namespace eArchiver.Models.ViewModels.Documents
{
    public class DocumentEditViewModel
    {
        public bool AllowDocumentDisplay { get; set; }
        public string DenyMessage { get; set; }

        public DocumentDetails Details { get; set; }
        public List<Category> Categories { get; set; }
        public List<Type> Types { get; set; }
        public List<Sender> Senders { get; set; }
        public ScanSelectorViewModel ScanSelectorModel { get; set; }
        public List<Type2> Types2 { get; set; }
        public List<Sender> FilteredSenders
        {
            get
            {
                return Senders.Where(s => !Details.Senders.Select(s2 => s2.SenderID).Contains(s.SenderID)).ToList();
            }
        }
        
        
    }
}
