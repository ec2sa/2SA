using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eArchiver.Models.Entities;

namespace eArchiver.Models.ViewModels.Documents
{
    public class DocumentSearchResult
    {
        public int Count { get; set; }
        public bool IsSearchPerformed { get; set; }

        public List<DocumentDetails> Documents { get; set; }
    }
}
