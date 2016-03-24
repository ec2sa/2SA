using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using eArchiver.Helpers;
using eArchiver.Models.Repositories.Documents;
using eArchiver.Models;
using eArchiver.Models.ViewModels.Documents;
using eArchiver.Models.Repositories.Dictionaries;
using eArchiver.Models.SearchCriterias;
using eArchiver.Models.ViewModels.Scans;
using eArchiver.Models.Repositories.Scans;
using eArchiver.Models.Entities;
using ScanManager;
using eArchiver.Controllers.Factories;
using eArchiver.Constants;
using eArchiver.Attributes;

namespace eArchiver.Controllers
{

    public class DocumentsController : BaseController
    {
        DocumentRepository _repository = new DocumentRepository();

        DictionaryRepository _dictRepository = new DictionaryRepository();

        [Authorize]
        public ActionResult Index(bool? isBack)
        {
            if (isBack != null && isBack.Value == true && Session["doc_sc"] != null)
            {

                int ItemsCount = (int)Session["doc_ic"];
                int? p = (int?)Session["doc_p"];
                return Index(null, ItemsCount, p);
            }


            DocumentsSortCriteria sortCriteria = new DocumentsSortCriteria()
            {
                SortDirection = SortDirection.Descending,
                SortType = DocumentSortType.CreateDate
            };

            DocumentsViewModel viewModel = new DocumentsViewModel()
            {
                DocumentSearchModel = new DocumentSearchViewModel()
                {
                    SortCriteria = sortCriteria,
                    SearchCriteria = new DocumentsSearchCriteria() { ItemsCount = 10 },
                    IsSearchPerformed = false,
                    Categories = _dictRepository.GetCategories().ToList(),
                    Senders = _dictRepository.GetSenders().OrderBy(s => s.LastName).ThenBy(s => s.FirstName).ToList(),
                    Types2 = _dictRepository.GetTypes2().ToList()
                }
            };

            return View(viewModel);
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(FormCollection formValues, int ItemsCount, int? p)
        {
            int? page = p;
            int itemsOnPage = ItemsCount;

            if (formValues == null)
                formValues = new FormCollection();


            DocumentsSearchCriteria searchCriteria = new DocumentsSearchCriteria();
            if (!TryUpdateModel<DocumentsSearchCriteria>(searchCriteria, formValues.AllKeys))
                searchCriteria = Session["doc_sc"] as DocumentsSearchCriteria;
            if (searchCriteria == null)
                searchCriteria = new DocumentsSearchCriteria();

            DocumentsSortCriteria sortCriteria = new DocumentsSortCriteria();
          if(!TryUpdateModel<DocumentsSortCriteria>(sortCriteria, formValues.AllKeys))
                sortCriteria = Session["doc_soc"] as DocumentsSortCriteria;

          if (sortCriteria == null)
              sortCriteria = new DocumentsSortCriteria();

            DocumentsViewModel viewModel = new DocumentsViewModel()
            {
                DocumentSearchModel = new DocumentSearchViewModel()
                {
                    SearchCriteria = searchCriteria,
                    SortCriteria = sortCriteria,
                    SearchResults = new PaginatedList<DocumentDetails>(_repository.SearchDocuments(searchCriteria).SortDocuments(sortCriteria).AsQueryable<DocumentDetails>(), page ?? 0, itemsOnPage),
                    IsSearchPerformed = true,
                    Categories = _dictRepository.GetCategories().ToList(),
                    Senders = _dictRepository.GetSenders().ToList(),
                    Types2 = _dictRepository.GetTypes2().ToList()
                }
            };


            Session["doc_sc"] = searchCriteria;
            Session["doc_soc"] = sortCriteria;
            Session["doc_ic"] = ItemsCount;
            Session["doc_p"] = p;


            if (searchCriteria.CategoryID.HasValue)
            {
                viewModel.DocumentSearchModel.Types = _dictRepository.GetTypes(searchCriteria.CategoryID.Value).ToList();
            }

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Search(bool? isBack)
        {
            if (isBack != null && isBack.Value == true && Session["doc_sc"] != null)
            {
                int ItemsCount = (int)Session["doc_ic"];
                int? p = (int?)Session["doc_p"];
                return Search(null, ItemsCount, p);
            }


            //DocumentsSortCriteria sortCriteria = new DocumentsSortCriteria()
            //{
            //    SortDirection = SortDirection.Descending,
            //    SortType = DocumentSortType.CreateDate
            //};

            DocumentSearchViewModel viewModel = new DocumentSearchViewModel()
            {
                //SortCriteria = sortCriteria,
                SearchCriteria = new DocumentsSearchCriteria() { ItemsCount = 10 },
                IsSearchPerformed = false,
                Categories = _dictRepository.GetCategories().ToList(),
                Senders = _dictRepository.GetSenders().ToList(),
                Types2 = _dictRepository.GetTypes2().ToList()
            };

            return View(viewModel);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(FormCollection formValues, int ItemsCount, int? p)
        {
            int? page = p;
            int itemsOnPage = ItemsCount;

            DocumentsSearchCriteria searchCriteria = new DocumentsSearchCriteria();
            if (formValues != null)
                UpdateModel<DocumentsSearchCriteria>(searchCriteria, formValues.AllKeys);
            else
                searchCriteria = Session["doc_sc"] as DocumentsSearchCriteria;

            // olewamy sortowanie bo zmniejsza wydajnoœæ
            //DocumentsSortCriteria sortCriteria = new DocumentsSortCriteria();
            //if (formValues != null)
            //    UpdateModel<DocumentsSortCriteria>(sortCriteria, formValues.AllKeys);
            //else
            //    sortCriteria = Session["doc_soc"] as DocumentsSortCriteria;


            DocumentSearchViewModel viewModel = new DocumentSearchViewModel()
            {
                SearchCriteria = searchCriteria,
                //SortCriteria = sortCriteria,
                //SearchResults = new PaginatedList<DocumentDetails>(_repository.SearchDocuments(searchCriteria).SortDocuments(sortCriteria).AsQueryable<DocumentDetails>(), page ?? 0, itemsOnPage),
                IsSearchPerformed = true,
                Categories = _dictRepository.GetCategories().ToList(),
                Senders = _dictRepository.GetSenders().ToList(),
                Types2 = _dictRepository.GetTypes2().ToList()
            };


            Session["doc_sc"] = searchCriteria;
            //Session["doc_soc"] = sortCriteria;
            Session["doc_ic"] = ItemsCount;
            Session["doc_p"] = p;


            if (searchCriteria.CategoryID.HasValue)
            {
                viewModel.Types = _dictRepository.GetTypes(searchCriteria.CategoryID.Value).ToList();
            }

            return View(viewModel);
        }


        //QUOTA [Quota(QuotaType = QuotaTypes.Documents)]
        [Authorize]
        public ActionResult NewDocument(FormCollection formValues)
        {
            Guid userGuid = AppContext.GetUserGuid();
            if (userGuid == Guid.Empty)
                return new HttpUnauthorizedResult();
            Guid documentID = _repository.CreateDocument(userGuid);

            InfoTypeOne infoTypeOne = new InfoTypeOne() { DocumentID = documentID };
            InfoTypeTwo infoTypeTwo = new InfoTypeTwo() { DocumentID = documentID };

            if (AppContext.AllowInfoTypeOneWrite())
                UpdateModel<InfoTypeOne>(infoTypeOne);

            if (AppContext.AllowInfoTypeTwoWrite())
                UpdateModel<InfoTypeTwo>(infoTypeTwo);

            if (AppContext.AllowScansWrite())
            {
                string[] scansGuids = formValues["selectedScans"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<Guid> guidsList = new List<Guid>();
                foreach (var guid in scansGuids)
                {
                    guidsList.Add(new Guid(guid));
                }
                _repository.SetDocumentScans(guidsList, documentID);
            }

            string[] senderIDs = formValues["sendersIDs"].Split(',');
            foreach (string sid in senderIDs)
            {
                int tmp;
                if (!int.TryParse(sid, out tmp))
                    continue;
                _repository.AddSender(documentID, tmp);
            }

            //poza ifami poniewa¿ i tak tworzymy "wydmuszki" sekcji
            _repository.CreateInfoTypeOne(infoTypeOne);
            _repository.CreateInfoTypeTwo(infoTypeTwo);

            _repository.SubmitChanges();
            return RedirectToAction("Index", "Scans", new { documentID = documentID });
        }

        [Authorize]
        public ActionResult Details(Guid documentID)
        {
            DocumentDetailsViewModel viewModel = new DocumentDetailsViewModel()
            {
                AllowDocumentDisplay = true,
                Details = _repository.GetDocumentDetails(documentID)
            };

            if (viewModel.Details.Document == null)
            {
                viewModel.AllowDocumentDisplay = false;
                viewModel.DenyMessage = "Dokument nie istnieje.";
            }
            else if (!AppContext.GetCIDs().Contains(viewModel.Details.Document.ClientID))
            {
                viewModel.AllowDocumentDisplay = false;
                viewModel.DenyMessage = "Nie masz uprawnieñ do tego dokumentu.";
            }
            else
            {
                if (viewModel.Details.Document.ClientID != AppContext.GetCID())
                {
                    AppContext.ChangeClient(viewModel.Details.Document.ClientID);
                }
            }

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Edit(Guid documentID)
        {
            ScansRepository scansRepository = new ScansRepository();

            ScanBrowser scans = ScanBrowserFactory.Create();

            DocumentEditViewModel viewModel = new DocumentEditViewModel()
            {
                AllowDocumentDisplay = true,
                Details = _repository.GetDocumentDetails(documentID),
                Categories = _dictRepository.GetCategories().ToList(),
                Senders = _dictRepository.GetSenders().ToList(),
                Types2 = _dictRepository.GetTypes2().ToList(),
                ScanSelectorModel = new ScanSelectorViewModel()
                {
                    AvailableScans = scansRepository.GetAvailableScans().ToList(),
                    AllScansCount = scans.GetScansCount(true),
                    KnownScansCount = scans.GetScansCount(false)
                }
            };

            #region Check if user can display this document
            if (viewModel.Details.Document == null)
            {
                viewModel.AllowDocumentDisplay = false;
                viewModel.DenyMessage = "Dokument nie istnieje.";
            }
            else if (!AppContext.GetCIDs().Contains(viewModel.Details.Document.ClientID))
            {
                viewModel.AllowDocumentDisplay = false;
                viewModel.DenyMessage = "Nie masz uprawnieñ do tego dokumentu.";
            }
            else
            {
                if (viewModel.Details.Document.ClientID != AppContext.GetCID())
                {
                    AppContext.ChangeClient(viewModel.Details.Document.ClientID);
                }
            }
            #endregion

            if (viewModel.Details.InfoTypeOne != null && viewModel.Details.InfoTypeOne.CategoryID.HasValue)
                viewModel.Types = _dictRepository.GetTypes(viewModel.Details.InfoTypeOne.CategoryID.Value).ToList();

            return View(viewModel);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Guid documentID, string selectedScans, string DocumentNumber, string CaseNumber,
            DateTime? Date, int? CategoryID, int? TypeID, string sendersIDs, string Description, string Submit, int? Type2ID, string Tags, bool? Flag1)
        {
            if (Submit.Equals("Zapisz"))
            {
                DocumentDetails details = _repository.GetDocumentDetails(documentID);
                Document document = _repository.GetDocument(documentID);

                if (AppContext.AllowScansWrite())
                {
                    ScansRepository scansRepository = new ScansRepository();

                    #region Add/Remove scans
                    List<Guid> scansIDs = new List<Guid>();
                    List<Guid> scansToAdd = new List<Guid>();
                    List<Guid> scansToRemove = new List<Guid>();

                    foreach (string g in selectedScans.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        Guid guid;
                        try
                        {
                            guid = new Guid(g);
                            if (!details.ScanIDs.Contains(guid))
                                scansToAdd.Add(guid);
                        }
                        catch (FormatException)
                        {
                            break;
                        }
                        scansIDs.Add(guid);
                    }

                    foreach (Guid guid in details.ScanIDs)
                    {
                        if (!scansIDs.Contains(guid))
                            scansToRemove.Add(guid);
                    }

                    _repository.SetDocumentScans(scansToAdd, documentID);
                    _repository.RemoveDocumentScans(scansToRemove);
                    #endregion
                }

                if (AppContext.AllowInfoTypeOneWrite())
                {
                    details.InfoTypeOne.DocumentNumber = DocumentNumber;
                    details.InfoTypeOne.CaseNumber = CaseNumber;
                    details.InfoTypeOne.Date = Date;
                    details.InfoTypeOne.CategoryID = CategoryID;
                    details.InfoTypeOne.TypeID = TypeID;
                    details.InfoTypeOne.Type2ID = Type2ID;
                    details.InfoTypeOne.Tags = Tags;
                }

                if (AppContext.AllowInfoTypeTwoWrite())
                {

                    string[] senderIDs = sendersIDs.Split(',');
                    _repository.RemoveSenders(documentID);
                    foreach (string sid in senderIDs)
                    {
                        int tmp;
                        if (!int.TryParse(sid, out tmp))
                            continue;
                        _repository.AddSender(documentID, tmp);
                    }

                    details.InfoTypeTwo.Description = Description;
                    details.InfoTypeTwo.Flag1 = Flag1 ?? false;
                }

                document.Editor = AppContext.GetUserGuid();

                _repository.SubmitChanges();

            }

            return RedirectToAction("Details", new { documentID = documentID });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadScanVersion(Guid originalID, Guid documentID, HttpPostedFileBase file)
        {
            if (file != null)
            {
                ScansRepository scanRepo = new ScansRepository();
                Scan visibleScan = scanRepo.GetScan(originalID);

                //jeœli skan jest dodany do dokumentu podczas tej edycji, to nie mam jeszcze przypisanego ID dokumentu
                if (!visibleScan.DocumentID.HasValue)
                {
                    _repository.SetDocumentScan(visibleScan.ScanID, documentID);
                    _repository.SubmitChanges();
                }

                if (visibleScan.OriginalScanID.HasValue)
                    originalID = visibleScan.OriginalScanID.Value;

                ScanBrowser browser = ScanBrowserFactory.Create();
                byte[] content = new byte[file.ContentLength];
                file.InputStream.Read(content, 0, file.ContentLength);
                ScanInfo info = browser.GetScanFromFile(content, file.FileName);
                try
                {
                    _repository.AddScanVersion(originalID, info.FileName, info.MimeType, info.Scan, info.ScanPreview ?? new byte[0], "image/png", info.ScanZoom ?? new byte[0], AppContext.GetCID());
                }
                catch { }
            }

            return RedirectToAction("Edit", new { documentID = documentID });// (result);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadScan(Guid documentID, HttpPostedFileBase file)
        {
            if (file != null)
            {
                ScanBrowser browser = ScanBrowserFactory.Create();
                byte[] content = new byte[file.ContentLength];
                file.InputStream.Read(content, 0, file.ContentLength);
                ScanInfo info = browser.GetScanFromFile(content, file.FileName);

                Guid scanId = ScansHelper.AddScan(info);

                _repository.SetDocumentScan(scanId, documentID);
                _repository.SubmitChanges();
            }

            return RedirectToAction("Edit", new { documentID = documentID });// (result);
        }

        [Authorize]
        public ActionResult GetScans(Guid documentID)
        {
            ScansHelper.GetScansFromFolder();

            return RedirectToAction("Edit", "Documents", new { documentID = documentID, d = 1 });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RenderVersionsBrowser(Guid scanGuid)
        {
            VersionBrowserViewModel viewModel = new VersionBrowserViewModel()
            {
                Scan = new ScansRepository().GetScan(scanGuid)
            };

            return PartialView("VersionBrowserUserControl", viewModel);
        }

        [Authorize]
        public ActionResult DocumentScans(Guid d, int s)
        {
            DocumentScansViewModel viewModel = new DocumentScansViewModel()
            {
                Details = _repository.GetDocumentDetails(d),
                CurrentScanIndex = s
            };

            return View(viewModel);
        }

    }
}
