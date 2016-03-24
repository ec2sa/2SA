using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eArchiver.Models.SearchCriterias
{
    public class DocumentsSearchCriteria
    {
        public string DocumentNumber { get; set; }
        public string CaseNumber { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DateTo { get; set; }
        public int? CategoryID { get; set; }
        public int? TypeID { get; set; }
        public int? SenderID { get; set; }
        public string Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? CreateDateTo { get; set; }
        public string SystemNumber { get; set; }
        public int? ItemsCount { get; set; }
        public string Text { get; set; }
        public bool SearchOCR { get; set; }
        public string Tags { get; set; }
        public int? Type2ID { get; set; }
        public string Company { get; set; }

    }

    public enum DocumentSortType
    {
        DocumentNumber,
        CaseNumber,
        Date,
        Category, 
        Type,
        Sender, 
        Description,
        CreateDate,
        SystemNumber
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }

    public class DocumentsSortCriteria
    {
        public DocumentSortType SortType { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
