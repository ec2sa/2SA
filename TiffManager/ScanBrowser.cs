using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace ScanManager
{
    public class ScanBrowser
    {
        public string ScanDirectory { get; set; }
        public string TempDirectory { get; set; }

        /// <summary>
        /// contains search pattern. multiple patterns must be separated with semicolon
        /// </summary>
        public string SearchPattern { get; set; }

        public ScanBrowser()
        {
            SearchPattern = "*.*";
        }

        public IList<ScanInfo> GetScans(bool includeUnknown)
        {
            string[] files = null;
            DirectoryInfo info = new DirectoryInfo(ScanDirectory);

            if (!includeUnknown)
            {
                string[] patterns = SearchPattern.Split(';');

                List<string> filesToAdd = new List<string>();
                foreach (string pattern in patterns)
                {
                    var filtered = info.GetFiles(pattern, SearchOption.TopDirectoryOnly).Where(f => (f.Attributes & FileAttributes.Hidden) == 0).Select(f => f.FullName);

                    filesToAdd.AddRange(filtered.ToList());
                }
                files = filesToAdd.ToArray();
            }
            else
                files = info.GetFiles("*.*", SearchOption.TopDirectoryOnly).Where(f => (f.Attributes & FileAttributes.Hidden) == 0).Select(f => f.FullName).ToArray();

            IPreviewGenerator previewGen;
            List<ScanInfo> items = new List<ScanInfo>();

            foreach (string scanFile in files)
            {

                byte[] scan = File.ReadAllBytes(scanFile);

                if (scan.Length == 0)
                    continue;
                try
                {
                    byte[] preview = null;

                    byte[] zoom = null;
                    int pageCount = 1;

                    string fileExtension = Path.GetExtension(scanFile).ToLower();
                    if (fileExtension.Contains("tif")
                        || fileExtension.Contains("gif")
                        || fileExtension.Contains("jpg")
                        || fileExtension.Contains("jpeg")
                        || fileExtension.Contains("png")
                        || fileExtension.Contains("bmp")
                        || fileExtension.Contains("pdf"))
                    {
                        previewGen = PreviewGeneratorFactory.Create(getMimeType(fileExtension), null, TempDirectory);

                        Image tmpPreview = previewGen.GetPreview(scan);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            tmpPreview.Save(ms, ImageFormat.Png);
                            preview = ms.ToArray();
                        }

                        previewGen.PreviewSize = 640;
                        tmpPreview = previewGen.GetPreview(scan);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            tmpPreview.Save(ms, ImageFormat.Png);
                            zoom = ms.ToArray();

                        }

                        if (fileExtension.Contains("pdf"))
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                ms.Write(scan, 0, scan.Length);
                                pageCount = Winnovative.PdfCreator.Document.GetPageCount(ms);
                            }
                        }
                    }
                    items.Add(new ScanInfo()
                    {
                        FileName = Path.GetFileName(scanFile)
                        ,
                        FullFileName = scanFile
                        ,
                        Scan = scan
                        ,
                        ScanPreview = preview
                        ,
                        ScanZoom = zoom
                        ,
                        MimeType = getMimeType(Path.GetExtension(scanFile))
                        ,
                        PageCount = pageCount
                    });
                }
                catch
                {
                    continue;
                }
            }
            return items;
        }

        public ScanInfo GetScanFromFile(byte[] fileContent, string fileName)
        {
            ScanInfo scan = null;
            byte[] preview = null;
            byte[] zoom = null;
            int pageCount = 1;

            string fileExtension = Path.GetExtension(fileName).ToLower();
            IPreviewGenerator previewGen = PreviewGeneratorFactory.Create(getMimeType(fileExtension), null, TempDirectory);

            if (fileExtension.Contains("tif")
                || fileExtension.Contains("gif")
                || fileExtension.Contains("jpg")
                || fileExtension.Contains("jpeg")
                || fileExtension.Contains("png")
                || fileExtension.Contains("bmp")
                || fileExtension.Contains("pdf"))
            {
                Image tmpPreview = previewGen.GetPreview(fileContent);
                using (MemoryStream ms = new MemoryStream())
                {
                    tmpPreview.Save(ms, ImageFormat.Png);
                    preview = ms.ToArray();
                }
                previewGen.PreviewSize = 640;
                tmpPreview = previewGen.GetPreview(fileContent);
                using (MemoryStream ms = new MemoryStream())
                {
                    tmpPreview.Save(ms, ImageFormat.Png);
                    zoom = ms.ToArray();


                   
                }

                if (fileExtension.Contains("pdf"))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ms.Write(fileContent, 0, fileContent.Length);
                        pageCount = Winnovative.PdfCreator.Document.GetPageCount(ms);
                    }
                }
            }



            scan = new ScanInfo()
             {
                 FileName = Path.GetFileName(fileName)
                 ,
                 FullFileName = fileName
                 ,
                 Scan = fileContent
                 ,
                 ScanPreview = preview
                 ,
                 ScanZoom = zoom
                 ,
                 MimeType = getMimeType(fileExtension)
                 ,
                 PageCount = pageCount
             };
            return scan;
        }

        public int GetScansCount(bool includeUnknown)
        {

            DirectoryInfo info = new DirectoryInfo(ScanDirectory);

            if (includeUnknown)
                return info.GetFiles("*.*", SearchOption.TopDirectoryOnly).Where(f => (f.Attributes & FileAttributes.Hidden) == 0).Count();
            else
            {
                string[] patterns = SearchPattern.Split(';');
                int scanCount = 0;
                foreach (string pattern in patterns)
                    scanCount += info.GetFiles(pattern, SearchOption.TopDirectoryOnly).Where(f => (f.Attributes & FileAttributes.Hidden) == 0).Count();

                return scanCount;
            }
        }

        public bool RemoveScan(string fullPath)
        {
            if (File.Exists(fullPath))
                try
                {
                    File.Delete(fullPath);
                    return true;
                }
                catch
                {
                    return false;
                }
            return false;

        }

        private string getMimeType(string extension)
        {
            extension = extension.ToLower();

            if (extension.Contains("tif"))
                return "image/tiff";
            if (extension.Contains("jpg") || extension.Contains("jpeg"))
                return "image/jpeg";
            if (extension.Contains("png"))
                return "image/png";
            if (extension.Contains("bmp"))
                return "image/bmp";
            if (extension.Contains("gif"))
                return "image/gif";
            if (extension.Contains("pdf"))
                return "application/pdf";
            if (extension.Contains("doc"))
                return "application/msword";
            if (extension.Contains("xls"))
                return "application/msexcel";
            if (extension.Contains("xml"))
                return "text/xml";
            return "text/plain";

        }
    }

    public class ScanInfo
    {
        public string FileName { get; set; }
        public string FullFileName { get; set; }
        public byte[] Scan { get; set; }
        public byte[] ScanZoom { get; set; }
        public byte[] ScanPreview { get; set; }
        public string MimeType { get; set; }
        public int PageCount { get; set; }
    }
}

