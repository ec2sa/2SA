using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eArchiver.Models.SearchCriterias;
using eArchiver.Models.Entities;

namespace eArchiver.Models.ViewModels.Documents
{
    public class DocumentSearchViewModel
    {
        public DocumentsSearchCriteria SearchCriteria { get; set; }
        public DocumentsSortCriteria SortCriteria { get; set; }
        public PaginatedList<DocumentDetails> SearchResults { get; set; }
        public List<DocumentDetails> SearchResultDetails { get; set; }

        public bool IsSearchPerformed { get; set; }

        public List<Category> Categories { get; set; }
        public List<Type> Types { get; set; }
        public List<Type2> Types2 { get; set; }
        public List<Sender> Senders { get; set; }


    }

}
