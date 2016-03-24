using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScanManager;
using eArchiver.Properties;
using System.IO;

namespace eArchiver.Controllers.Factories
{
    public static class ScanBrowserFactory
    {
        public static ScanBrowser Create(){
            string tempPath = Path.GetTempPath();// HttpContext.Current.Server.MapPath("~/temp");
            string prefix=AppContext.GetClientPrefix();
            string scansDirectory=null;

            if (string.IsNullOrEmpty(prefix))
              scansDirectory = Properties.Settings.Default.ScansRootDirectory;
            else
                scansDirectory = Path.Combine(Properties.Settings.Default.ScansRootDirectory, prefix + "scans");
            string searchPattern = Settings.Default.ScansSearchPattern;

            return new ScanBrowser()
            {
                ScanDirectory = scansDirectory
                ,
                SearchPattern = searchPattern
                ,
                TempDirectory = tempPath
            };
        }

     
    }
}
