using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eArchiver.Models.Entities
{
    public class DocumentDetails
    {
        public Document Document { get; set; }
        public InfoTypeOne InfoTypeOne { get; set; }
        public InfoTypeTwo InfoTypeTwo { get; set; }
        public IList<Guid> ScanIDs { get; set; }
        public string ScanContentInfo { get; set; }
        public List<Sender> Senders { get; set; }
        public string SenderIDs
        {
            get
            {
                return string.Join(",", (from s in Senders select s.SenderID.ToString()).ToArray());
            }
        }
        public string DocumentSenders { get; set; }
    }
}
