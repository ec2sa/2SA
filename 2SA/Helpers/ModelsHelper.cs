using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eArchiver.Models.Entities;
using eArchiver.Models.SearchCriterias;

namespace eArchiver.Helpers
{
    public static class ModelsHelper
    {
        public static List<DocumentDetails> SortDocuments(this List<DocumentDetails> list, DocumentsSortCriteria sortCriteria)
        {
            IQueryable<DocumentDetails> result = list.AsQueryable<DocumentDetails>();
           

            switch (sortCriteria.SortType)
            {
                case DocumentSortType.DocumentNumber:
                    result = result.OrderBy(s => s.InfoTypeOne.DocumentNumber);
                    break;
                case DocumentSortType.CaseNumber:
                    result = result.OrderBy(s => s.InfoTypeOne.CaseNumber);
                    break;
                case DocumentSortType.Date:
                    result = result.OrderBy(s => s.InfoTypeOne.Date);
                    break;
                case DocumentSortType.Category:
                    //jesli Category ID jest null, sortowanie nic nie zwraca, wiec najpierw dodaje te z nullami, a potem sortuje te ktore maja wartosci
                    List<DocumentDetails> tmpC = result.Where(c => c.InfoTypeOne.CategoryID == null).ToList();
                    tmpC.AddRange(result.Where(c=>c.InfoTypeOne.CategoryID!=null).OrderBy(s => s.InfoTypeOne.Category.Name));
                    result = tmpC.AsQueryable<DocumentDetails>();
                    break;
                case DocumentSortType.Type:
                    List<DocumentDetails> tmpT = result.Where(c => c.InfoTypeOne.TypeID == null).ToList();
                    tmpT.AddRange(result.Where(c => c.InfoTypeOne.TypeID != null).OrderBy(s => s.InfoTypeOne.Type.Name));
                    result = tmpT.AsQueryable<DocumentDetails>();
                    break;
                //case DocumentSortType.Sender:
                //    List<DocumentDetails> tmpS = result.Where(c => c.InfoTypeTwo.SenderID == null).ToList();
                //    tmpS.AddRange(result.Where(c => c.InfoTypeTwo.SenderID != null).OrderBy(s => s.InfoTypeTwo.Sender.FullName));
                //    result = tmpS.AsQueryable<DocumentDetails>();
                //    break;
                case DocumentSortType.Description:
                    result = result.OrderBy(s => s.InfoTypeTwo.Description);
                    break;
                case DocumentSortType.CreateDate:
                    result = result.OrderBy(s => s.Document.CreateDate);
                    break;
                default:
                    break;
            }

            if (sortCriteria.SortDirection == SortDirection.Descending)
                result = result.Reverse();

            return result.ToList();
        }
    }
}
