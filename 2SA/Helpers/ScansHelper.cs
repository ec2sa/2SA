using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScanManager;
using System.IO;
using eArchiver.Properties;
using eArchiver.Models;
using eArchiver.Controllers.Factories;
using eArchiver.Models.Repositories.Scans;
using System.Drawing.Imaging;
using eArchiver.Models.Repositories.Documents;
using eArchiver.Controllers;
//using EPocalipse.IFilter;

namespace eArchiver.Helpers
{
    public static class ScansHelper
    {
        public static bool IsProcessing { get; private set; }

        private static ScanBrowser scans = ScanBrowserFactory.Create();

        private static ScansRepository _repository = new ScansRepository();

        public static void GetScansFromFolder()
        {
            scans = ScanBrowserFactory.Create();
            if (IsProcessing)
                return;
            IsProcessing = true;
            try
            {
                IList<ScanInfo> scansToProcess = scans.GetScans(true);

                foreach (ScanInfo scan in scansToProcess)
                {
                    AddScan(scan);
                    scans.RemoveScan(scan.FullFileName);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to process scan: " + ex.Message + "/" + ((ex.InnerException != null) ? ex.InnerException.Message : ""));
                throw;
            }
            finally
            {
                IsProcessing = false;
            }
        }

        public static Guid AddScan(ScanInfo scan)
        {
            string extension = Path.GetExtension(scan.FileName).ToLower().Substring(1, 3);

            bool isKnown = Settings.Default.ScansSearchPattern.Contains(extension);

            Scan scanToSave = new Scan();
            ScanTypeInfo sti=ScansHelper.GetTypeInfo(scan.MimeType);
            scanToSave.ScanID = new Guid();
            scanToSave.FileName = scan.FileName;
            scanToSave.MimeType = scan.MimeType;                
            scanToSave.ScanContent = scan.Scan;
            scanToSave.ClientID = AppContext.GetCID();
            scanToSave.PageCount = scan.PageCount;
            

            if (scan.ScanZoom != null)
                scanToSave.ScanZoom = scan.ScanZoom;
            if (sti.IsPreviewable)
            {
                scanToSave.ScanPreviews.Add(new ScanPreview()
                {
                    ScanPreviewID = new Guid(),
                    MimeType = "image/png",
                    ScanPreviewContent = scan.ScanPreview
                });
            }
            _repository.InsertScan(scanToSave);
            _repository.SubmitChanges();

          //  readScanTextContent(scan.FileName, scan.Scan,scanToSave.ScanID);
            return scanToSave.ScanID;
        }

        private static void readScanTextContent(string path, byte[] content,Guid scanID)
        {
            
        }

        public static bool HasVersions(Guid scanID)
        {
            return new DocumentRepository().HasVersion(scanID);
        }

        public static ScanTypeInfo GetTypeInfo(string mimeType)
        {
            return new ScanTypeInfo(mimeType);
        }
    }

    public class ScanTypeInfo
    {
        private string mimeType;

        public ImageFormat Format
        {
            get
            {
                if (!IsImage && !IsPdfDocument)
                    return null;
                if (IsJpegImage)
                    return ImageFormat.Jpeg;
                if (IsGifImage)
                    return ImageFormat.Gif;
                if (IsPngImage || IsPdfDocument)
                    return ImageFormat.Png;
                if (IsTiffImage)
                    return ImageFormat.Tiff;
                if (IsBmpImage)
                    return ImageFormat.Bmp;

                return null;
            }
        }

        public ScanTypeInfo(string mimeType)
        {
            this.mimeType = mimeType.ToLower();
        }

        public bool IsPdfDocument
        {
            get { return mimeType.Contains("application") && mimeType.Contains("pdf"); }
        }

        public bool IsWordDocument
        {
            get { return mimeType.Contains("application") && mimeType.Contains("word"); }
        }

        public bool IsImage
        {
            get { return mimeType.Contains("image"); }
        }

        public bool IsTiffImage
        {
            get { return mimeType.Contains("image") && mimeType.Contains("tif"); }
        }

        public bool IsJpegImage
        {
            get { return mimeType.Contains("image") && (mimeType.Contains("jpg") || mimeType.Contains("jpeg")); }
        }

        public bool IsGifImage
        {
            get { return mimeType.Contains("image") && mimeType.Contains("gif"); }
        }

        public bool IsPngImage
        {
            get { return mimeType.Contains("image") && mimeType.Contains("png"); }
        }

        public bool IsBmpImage
        {
            get { return mimeType.Contains("image") && mimeType.Contains("bmp"); }
        }

        public bool IsPreviewable
        {
            get
            {
                return IsImage || IsPdfDocument;
            }
        }
    }
}
