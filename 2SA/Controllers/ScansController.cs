using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using ScanManager;
using eArchiver.Controllers.Factories;
using eArchiver.Models;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using eArchiver.Models.ViewModels.Scans;
using eArchiver.Models.Repositories.Scans;
using eArchiver.Models.Repositories.Dictionaries;
using eArchiver.Properties;
using eArchiver.Helpers;
using eArchiver.Attributes;
using eArchiver.Constants;
using System.Web.Security;
using Winnovative.PdfCreator;
using System.Transactions;

namespace eArchiver.Controllers
{

    public class ScansController : BaseController
    {
        private ScansRepository _repository = new ScansRepository();

        public ActionResult Crash()
        {
            throw new Exception("CRASHTEST!");
        }

        public ActionResult SenderSearch(string term)
        {
            DictionaryRepository dictRepository = new DictionaryRepository();
            var result = dictRepository.GetSenders()
                .Where(s => s.LastName.Contains(term) || s.FirstName.Contains(term) || s.Company.Contains(term))
                .Select(s => new { id = s.SenderID, value = s.LastName ?? "" + " " + s.FirstName ?? "" + (" (" + s.Company + ")") ?? "" })
                .ToList();
            // List<object> items = new List<object>();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SplitFile(Guid? scanGuid)
        {
            if (scanGuid.HasValue)
            {
                ScanBrowser browser = ScanBrowserFactory.Create();
                ScansRepository scanRepository = new ScansRepository();
                Scan scan = scanRepository.GetScan(scanGuid.Value);

                byte[] fileContent = scan.ScanContent.ToArray();
                string[] fileName = scan.FileName.Split('.');
                string mimeType = scan.MimeType;

                MemoryStream stream = new MemoryStream();
                stream.Write(fileContent, 0, fileContent.Length);


                LicensingManager.LicenseKey = "lL+mtKWhtKOnrbSjuqS0p6W6paa6ra2trQ==";

                Winnovative.PdfCreator.Document sourcedoc = new Winnovative.PdfCreator.Document(stream);
                stream.Close();

                using (TransactionScope transaction = new TransactionScope())
                {
                    if (sourcedoc != null && sourcedoc.Pages.Count > 1)
                    {
                        for (int i = 0; i < sourcedoc.Pages.Count; i++)
                        {
                            Winnovative.PdfCreator.Document newdoc = SplitPdf.ExtractPages(sourcedoc, i, 1);

                            string newFileName = string.Join(string.Format("_{0}.", (i + 1)), fileName);
                            ScanInfo newscan = browser.GetScanFromFile(newdoc.Save(), newFileName);
                            ScansHelper.AddScan(newscan);
                        }

                        scanRepository.DeleteScanFromRecycleBin(scan.ScanID);


                    }

                    transaction.Complete();
                }

            }


            return RedirectToAction("Index");
        }

        // [Authorize]
        public ActionResult Index(Guid? documentID)
        {
            DictionaryRepository dictRepository = new DictionaryRepository();

            ScanBrowser scans = ScanBrowserFactory.Create();
            ScansViewModel viewModel = new ScansViewModel();
            viewModel.AllScansCount = scans.GetScansCount(true);
            viewModel.KnownScansCount = scans.GetScansCount(false);
            viewModel.AvailableScans = _repository.GetAvailableScans().ToList();//Context.AvailableScans.OrderByDescending(s=>s.ImportDate).ToList();
            viewModel.Categories = dictRepository.GetCategories().ToList();
            viewModel.Types2 = dictRepository.GetTypes2().ToList();
            viewModel.Senders = dictRepository.GetSenders().OrderBy(s => s.LastName).ThenBy(s => s.FirstName).ToList();

            if (documentID.HasValue)
                viewModel.DocumentID = documentID;

            return View(viewModel);
        }

        //QUOTA [Quota(QuotaType = QuotaTypes.ScansTotalSize)]
        [Authorize]
        public ActionResult GetScans()
        {
            ScansHelper.GetScansFromFolder();

            return RedirectToAction("Index");
        }

        //QUOTA [Quota(QuotaType = QuotaTypes.ScansTotalSize)]
        [Authorize]
        public ActionResult GetScansFromFolder()
        {
            ScansHelper.GetScansFromFolder();

            return Json(true, "text/plain", JsonRequestBehavior.AllowGet);
        }

        //QUOTA [Quota(QuotaType = QuotaTypes.ScansTotalSize)]
        [Authorize]
        public ActionResult UploadScan()
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                if (file != null)
                {
                    ScanBrowser browser = ScanBrowserFactory.Create();
                    byte[] content = new byte[file.ContentLength];
                    file.InputStream.Read(content, 0, file.ContentLength);
                    ScanInfo info = browser.GetScanFromFile(content, file.FileName);

                    Guid scanId = ScansHelper.AddScan(info);
                }
            }
            return RedirectToAction("Index");
        }

        //QUOTA [Quota(QuotaType = QuotaTypes.ScansTotalSize)]
        public ActionResult FlashUpload(string token, HttpPostedFileBase FileData)
        {
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(token);
            if (ticket != null)
            {
                var identity = new FormsIdentity(ticket);
                if (identity.IsAuthenticated)
                {
                    HttpPostedFileBase file = FileData;
                    if (file != null)
                    {
                        ScanBrowser browser = ScanBrowserFactory.Create();
                        byte[] content = new byte[file.ContentLength];
                        file.InputStream.Read(content, 0, file.ContentLength);
                        ScanInfo info = browser.GetScanFromFile(content, file.FileName);

                        Guid scanId = ScansHelper.AddScan(info);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult ScansBin()
        {
            ScansRepository repository = new ScansRepository();
            ScansBinViewModel viewModel = new ScansBinViewModel()
            {
                ScansList = repository.GetBinScansList()
            };

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Preview(Guid scanGuid)
        {
            var sc = Context.Scans.Where(s => s.ScanID == scanGuid).FirstOrDefault();
            var sp = sc.ScanPreviews.FirstOrDefault();

            byte[] previewContent = null;

            if (sp == null || sp.ScanPreviewContent.Length == 0)
                previewContent = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/images/noPreview.png"));
            else
                previewContent = PutOverlayIcon(sp.ScanPreviewContent.ToArray(), sc.MimeType);

            return File(previewContent, "image/png");
        }

        [Authorize]
        public ActionResult Download(Guid scanGuid)
        {
            var sp = Context.Scans.Where(s => s.ScanID == scanGuid).FirstOrDefault();
            if (sp == null)
                return new EmptyResult();
            return File(sp.ScanContent.ToArray(), sp.MimeType, sp.FileName);
        }

        [Authorize]
        public ActionResult DownloadMarked(Guid scanGuid)
        {
            var sp = Context.Scans.Where(s => s.ScanID == scanGuid).FirstOrDefault();
            if (sp == null)
                return new EmptyResult();
            byte[] content = sp.ScanContent.ToArray();
            if (!sp.DocumentID.HasValue || sp.MimeType.ToLower() != "application/pdf")
                return File(content, sp.MimeType, sp.FileName);
            try
            {
                string markText = sp.Document.InfoTypeOnes.First().DocumentNumber;
                byte[] markedContent = WatermarkGenerator.AddWatermarkToPDF(content, markText, Settings.Default.MarkFontSize, Color.Black, Settings.Default.MarkDistanceFromRight, Settings.Default.MarkDistanceFromTop);
                return File(markedContent, sp.MimeType, sp.FileName);
            }
            catch
            {
                return File(content, sp.MimeType, sp.FileName);
            }

        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RenderPreview(Guid scanGuid, int? rotation)
        {
            ScanPreviewViewModel viewModel = new ScanPreviewViewModel()
            {
                Scan = _repository.GetScan(scanGuid),
                Rotation = rotation ?? 0
            };

            return PartialView("ScanPreviewUserControl", viewModel);
        }

        [Authorize]
        public ActionResult SaveRotated(Guid scanGuid, int? rotation)
        {
            if (rotation.HasValue && rotation % 4 == 0)
                return new EmptyResult();
            var sp = Context.Scans.Where(s => s.ScanID == scanGuid).FirstOrDefault();
            if (sp == null)
                return new EmptyResult();

            ScanTypeInfo si = ScansHelper.GetTypeInfo(sp.MimeType);
            if (!(si.IsImage || si.IsPdfDocument))
                return new EmptyResult();

            int rotationType = rotation.Value % 4;
            RotateFlipType rft = RotateFlipType.RotateNoneFlipNone;

            switch (rotationType)
            {
                case 0:
                    rft = RotateFlipType.RotateNoneFlipNone;
                    break;
                case 1:
                    rft = RotateFlipType.Rotate90FlipNone;
                    break;
                case 2:
                    rft = RotateFlipType.Rotate180FlipNone;
                    break;
                case 3:
                    rft = RotateFlipType.Rotate270FlipNone;
                    break;
            }


            using (MemoryStream ms = new MemoryStream(sp.ScanZoom.ToArray()))
            {
                Image img = Image.FromStream(ms);
                img.RotateFlip(rft);
                using (MemoryStream oms = new MemoryStream())
                {
                    img.Save(oms, si.Format);
                    sp.ScanZoom = oms.ToArray();
                }
            }

            var scanPreview = sp.ScanPreviews.FirstOrDefault();
            if (scanPreview != null)
            {
                using (MemoryStream ms = new MemoryStream(scanPreview.ScanPreviewContent.ToArray()))
                {
                    Image img = Image.FromStream(ms);
                    img.RotateFlip(rft);
                    using (MemoryStream oms = new MemoryStream())
                    {
                        img.Save(oms, ImageFormat.Png);
                        scanPreview.ScanPreviewContent = oms.ToArray();
                    }
                }
            }
            Context.SubmitChanges();
            return new EmptyResult();
        }

        [Authorize]
        public ActionResult Zoom(Guid scanGuid, int? rotation)
        {
            var sp = Context.ScanPreviews.Where(s => s.ScanID == scanGuid).FirstOrDefault();
            byte[] previewContent = null;

            if (sp == null)
                previewContent = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/images/noPreview.png"));
            else
            {
                previewContent = getZoomedPreview(scanGuid, rotation ?? 0);
            }
            return File(previewContent, "image/png");
        }

        private byte[] getZoomedPreview(Guid scanGuid, int rotation)
        {
            int rotationType = rotation % 4;
            RotateFlipType rft = RotateFlipType.RotateNoneFlipNone;

            switch (rotationType)
            {
                case 0:
                    rft = RotateFlipType.RotateNoneFlipNone;
                    break;
                case 1:
                    rft = RotateFlipType.Rotate90FlipNone;
                    break;
                case 2:
                    rft = RotateFlipType.Rotate180FlipNone;
                    break;
                case 3:
                    rft = RotateFlipType.Rotate270FlipNone;
                    break;
            }

            var sp = Context.Scans.Where(s => s.ScanID == scanGuid).FirstOrDefault();
            using (MemoryStream ms = new MemoryStream(sp.ScanZoom.ToArray()))
            {
                Image img = Image.FromStream(ms);

                using (MemoryStream mso = new MemoryStream())
                {
                    img.RotateFlip(rft);
                    img.Save(mso, ImageFormat.Png);
                    return mso.ToArray();
                }

            }

        }

        private byte[] PutOverlayIcon(byte[] previewContent, string mimeType)
        {
            //decimal ratio = 640.0M / (decimal)img.Width;

            //Image zoomed = new Bitmap((int)(ratio * img.Width), (int)(ratio * img.Height));
            //using (Graphics g = Graphics.FromImage(zoomed))
            //{
            //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //    g.DrawImage(img, new Rectangle(Point.Empty, zoomed.Size), new Rectangle(Point.Empty, img.Size), GraphicsUnit.Pixel);
            //}
            using (MemoryStream ms = new MemoryStream(previewContent))
            {
                Image overlayed = Image.FromStream(ms);
                bool isHorizontal = overlayed.Width > overlayed.Height;

                string overlayPath = "~/Content/images/";


                ScanTypeInfo sti = ScansHelper.GetTypeInfo(mimeType);

                if (sti.IsPdfDocument)
                    overlayPath = string.Format("{0}{1}_{2}.png", overlayPath, "pdf", isHorizontal ? "h" : "v");
                else if (sti.IsImage)
                    overlayPath = string.Format("{0}{1}_{2}.png", overlayPath, "img", isHorizontal ? "h" : "v");
                else if (sti.IsWordDocument)
                    overlayPath = string.Format("{0}{1}_{2}.png", overlayPath, "doc", isHorizontal ? "h" : "v");
                else
                    overlayPath = string.Format("{0}{1}_{2}.png", overlayPath, "inne", isHorizontal ? "h" : "v");

                Image overlay = Image.FromFile(Server.MapPath(overlayPath));

                using (Graphics g = Graphics.FromImage(overlayed))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(overlay, new Rectangle(0, 0, overlayed.Width, overlayed.Height));
                }
                using (MemoryStream mso = new MemoryStream())
                {

                    overlayed.Save(mso, ImageFormat.Png);
                    return mso.ToArray();
                }
            }

        }
    }
}
