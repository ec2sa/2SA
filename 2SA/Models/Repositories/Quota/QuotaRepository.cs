using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eArchiver.Constants;
using eArchiver.Controllers;

namespace eArchiver.Models.Repositories.Quota
{
    public class QuotaRepository
    {
        EArchiverDataContext _context = new EArchiverDataContext();

        public void SubmitChanges()
        {
            _context.SubmitChanges();
        }

        public bool QuotaExceeded(QuotaTypes quotaType, int categoryID, out int allowedCount, out int currentCount)
        {
            //pCheckQuotaResult result = _context.CheckQuota(quotaType.ToString()).Single();
            
            //allowedCount = result.allowedCount;
            //currentCount = result.currentCount;
            
            //return result.isViolated??true;
            
            bool result = false;
            currentCount = 0;
            allowedCount = 0;
            int cid = AppContext.GetCID();
            switch (quotaType)
            {
                case QuotaTypes.Clients:
                    currentCount = _context.Clients.Count();
                    allowedCount = QuotaLimits.ClientLimit;
                    result = currentCount >= allowedCount;
                    break;

                case QuotaTypes.DocumentCategories:
                    currentCount = _context.Categories.Where(c=>c.ClientID==cid).Count();
                    allowedCount = QuotaLimits.DocumentCategoriesLimit;
                    result = currentCount >= allowedCount;
                    break;

                case QuotaTypes.Documents:
                    currentCount = _context.Documents.Where(d=>d.ClientID==cid).Count();
                    allowedCount = QuotaLimits.DocumentsLimit;
                    result = currentCount >= allowedCount;
                    break;

                case QuotaTypes.DocumentTypes:
                    currentCount = _context.Types.Where(t=>t.ClientID==cid &&(t.CategoryID==categoryID || categoryID==0)).Count();
                    allowedCount = QuotaLimits.DocumentTypesLimit;
                    result = currentCount  >= allowedCount;
                    break;

                case QuotaTypes.ScansTotalSize:
                    var scans = _context.Scans;
                    if(scans.Count()>0)
                        currentCount = scans.Where(s=>s.ClientID==cid).Sum(s => s.ScanContent.Length);
                    else
                    currentCount = 0;
                    allowedCount = QuotaLimits.ScansTotalSizeLimit;
                    result = currentCount >= allowedCount * 1024 * 1024;
                    break;
            }
            return result;
        }
    }

    public static class QuotaLimits
    {
        public static readonly int ClientLimit = 5;
        public static readonly int DocumentsLimit = 30;
        public static readonly int DocumentCategoriesLimit= 5;
        public static readonly int DocumentTypesLimit = 3;
        public static readonly int ScansTotalSizeLimit = 100;//in MB

    }
}
