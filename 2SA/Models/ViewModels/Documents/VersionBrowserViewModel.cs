using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eArchiver.Helpers;
using eArchiver.Models.Repositories.Scans;

namespace eArchiver.Models.ViewModels.Documents
{
    public class VersionBrowserViewModel
    {
        private ScansRepository _repository;
        public VersionBrowserViewModel()
        {
            _repository = new ScansRepository();
            
        }

        public VersionBrowserViewModel(Guid scanGuid)
        {
            _repository = new ScansRepository();
            Scan = new ScansRepository().GetScan(scanGuid);
        }

        private Scan _scan;
        public Scan Scan {
            get
            {
                return _scan;
            }
            set
            {
                _scan = value;
                _repository.GetPrevAndNext(_scan.ScanID);
            } 
        }
        public bool HasPrevious
        {
            get
            {
                return PreviousScanID != null;
            }
        }

        public bool HasNext
        {
            get
            {
                return NextScanID != null;
            }
        }

        public Guid? PreviousScanID
        {
            get
            {
                return _repository.GetPreviousVersion();
            }
        }
        
        public Guid? NextScanID
        {
            get
            {
                return _repository.GetNextVersion();
            }
        }

        public bool HasVersions 
        { 
            get
            {
                if (Scan != null)
                    return ScansHelper.HasVersions(Scan.ScanID);
                else
                    return false;
            }
        }
    }
}
