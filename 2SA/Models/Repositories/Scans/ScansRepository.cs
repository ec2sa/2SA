using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eArchiver.Controllers;

namespace eArchiver.Models.Repositories.Scans
{
    public class ScansRepository
    {
        EArchiverDataContext _context = new EArchiverDataContext();

        public IQueryable<Scan> GetScans()
        {
            return _context.Scans.Where(s=>s.ClientID==AppContext.GetCID()).OrderByDescending(s => s.importDate);
        }

        public Scan GetScan(Guid scanID)
        {
            return GetScans().Where(s => s.ScanID == scanID).Where(s => s.ClientID == AppContext.GetCID()).Single();
        }

        public IQueryable<AvailableScan> GetAvailableScans()
        {
            return _context.AvailableScans.Where(s => s.ClientID == AppContext.GetCID()).OrderByDescending(s => s.ImportDate);
        }

        public void AddScanToRecycleBin(Guid scanID)
        {
            _context.AddScanToRecycleBin(scanID);
        }

        public void RestoreScanFromRecycleBin(Guid scanID)
        {
            _context.RestoreScanFromRecycleBin(scanID);
        }

        public void DeleteScanFromRecycleBin(Guid scanID)
        {
            _context.DeleteScanFromRecycleBin(scanID);
        }

    

        public List<Guid> GetBinScansList()
        {
            return _context.Scans.Where(s => s.isActive == false).Where(s => s.ClientID == AppContext.GetCID()).Select(s => s.ScanID).ToList();
        }

        public void SubmitChanges()
        {
            _context.SubmitChanges();
        }

        public void InsertScan(Scan scan)
        {

            _context.Scans.InsertOnSubmit(scan);
        }

        public void SaveTextContent(Guid scanID, string textContent)
        {
            _context.OCRSaveTextContent(scanID, textContent);
        }

        private List<pGetScanSiblingsResult> ScanPrevAndNext;

        public void GetPrevAndNext(Guid scanID)
        {
            ScanPrevAndNext = _context.GetScanSiblings(scanID).ToList();
        }
        
        public Guid? GetPreviousVersion()
        {
            foreach (var row in ScanPrevAndNext)
            {
                if (row.scanType.ToString().Contains("P"))
                    return row.scanID;
            }
            return null;
        }

        public Guid? GetNextVersion()
        {
            foreach (var row in ScanPrevAndNext)
            {
                if (row.scanType.ToString().Contains("N"))
                    return row.scanID;
            }
            return null;
        }
    }
}
