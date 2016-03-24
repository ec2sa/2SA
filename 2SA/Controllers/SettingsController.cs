using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using eArchiver.Models.ViewModels.Settings;
using eArchiver.Models.Repositories.Dictionaries;
using eArchiver.Models;
using eArchiver.Models.Repositories.Scans;
using eArchiver.Models.Repositories.Headers;
using eArchiver.Helpers;
using eArchiver.Constants;
using eArchiver.Attributes;
using System.ServiceProcess;
using eArchiver.Models.Repositories.Documents;

namespace eArchiver.Controllers
{
    public class SettingsController : Controller
    {
        DictionaryRepository _repository = new DictionaryRepository();

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [ContextAuthorize(Roles = RoleNames.Administrator)]
        public ActionResult Types()
        {
            TypesDictViewModel viewModel = new TypesDictViewModel()
            {
                Categories = _repository.GetCategories().ToList()
                ,
                Types = new List<eArchiver.Models.Type>()
            };

            return View(viewModel);
        }

        [ContextAuthorize(Roles = RoleNames.Administrator)]
        public ActionResult Categories()
        {
            List<Category> categories = _repository.GetCategories().ToList();

            return View(categories);
        }

        //QUOTA[Quota(QuotaType = QuotaTypes.DocumentCategories)]
        [ContextAuthorize(Roles = RoleNames.Administrator)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddCategory(FormCollection formValues)
        {
            if (!string.IsNullOrEmpty(formValues["CategoryName"]) && !_repository.CategoryExists(formValues["CategoryName"]))
            {
                _repository.CreateCategory(formValues["CategoryName"]);
                _repository.SubmitChanges();
            }
            
            return PartialView("CategoriesUserControl", _repository.GetCategories().ToList());
        }

        [ContextAuthorize(Roles = RoleNames.Administrator)]
        public ActionResult Types2()
        {
            List<Type2> types = _repository.GetTypes2().ToList();

            return View(types);
        }

        //QUOTA[Quota(QuotaType = QuotaTypes.DocumentCategories)]
        [ContextAuthorize(Roles = RoleNames.Administrator)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddType2(FormCollection formValues)
        {
            if (!string.IsNullOrEmpty(formValues["Type2Name"]) && !_repository.Type2Exists(formValues["Type2Name"]))
            {
                _repository.CreateType2(formValues["Type2Name"]);
                _repository.SubmitChanges();
            }

            return PartialView("Types2UserControl", _repository.GetTypes2().ToList());
        }

        //QUOTA [Quota(QuotaType = QuotaTypes.DocumentTypes)]
        [ContextAuthorize(Roles = RoleNames.Administrator)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddType(FormCollection formValues )
        {
            string TypeName = formValues["TypeName"];
            string sCategoryID = formValues["DictCategoryID"];
            
            int? DictCategoryID = null;
            int id;
            if (int.TryParse(sCategoryID, out id))
                DictCategoryID = id;

            List<eArchiver.Models.Type> types;
            if (DictCategoryID.HasValue)
            {
                int categoryID = DictCategoryID.Value;
                if (!string.IsNullOrEmpty(TypeName) && !_repository.TypeExists(TypeName, categoryID))
                {
                    _repository.CreateType(TypeName, categoryID);
                    _repository.SubmitChanges();
                }
                else
                {
                    TempData["Message"] = "Taki typ juz istnieje!";
                }
                types = _repository.GetTypes(DictCategoryID.Value).ToList();
            }
            else
            {
                types = new List<eArchiver.Models.Type>();
            }
            TypesDictViewModel viewModel = new TypesDictViewModel()
            {
                Categories = _repository.GetCategories().ToList(),
                Types = types,
                SelectedCategory = DictCategoryID
            };

            return PartialView("TypesUserControl", viewModel);
        }

        [Authorize]
        public ActionResult Senders()
        {
            return View(_repository.GetSenders());
        }

        [ContextAuthorize(Roles = RoleNames.Level2Full)]
        public ActionResult NewSender()
        {
            return View();
        }

        [ContextAuthorize(Roles = RoleNames.Level2Full)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult NewSender(FormCollection formValues)
        {
            if (formValues["Submit"] == Resources.Strings.S64)
            {
                Sender sender = new Sender();
                UpdateModel<Sender>(sender);
                sender.ClientID = AppContext.GetCID();
                if (ValidateSender(sender))
                {
                    _repository.CreateSender(sender);
                    _repository.SubmitChanges();
                    return RedirectToAction("Senders");
                }
                else
                {
                    return View();
                }
            }
            else
                return RedirectToAction("Senders");
        }

        [ContextAuthorize(Roles = RoleNames.Level2Full)]
        public ActionResult EditSender(int senderID)
        {
            Sender sender = _repository.GetSender(senderID);

            return View(sender);
        }

        [ContextAuthorize(Roles = RoleNames.Level2Full)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSender(FormCollection formValues)
        {
            int senderID;

            if (formValues["Submit"] == Resources.Strings.S64 && int.TryParse(formValues["SenderID"], out senderID))
            {
                Sender sender = _repository.GetSender(senderID);
                UpdateModel<Sender>(sender);
                if (ValidateSender(sender))
                {
                    _repository.SubmitChanges();
                    return RedirectToAction("Senders");
                }
                else
                    return View(sender);
            }
            else
                return RedirectToAction("Senders");
        }

        [ContextAuthorize(Roles = RoleNames.Administrator)]
        public ActionResult Headers()
        {
            HeadersRepository repository = new HeadersRepository();
            var model=repository.GetAllHeaders();
            return View(model);
        }

        [ContextAuthorize(Roles = RoleNames.Administrator)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveHeaders(IList<Header> headers)
        {
            HeadersRepository repository = new HeadersRepository();

            foreach (Header header in headers)
            {
                repository.UpdateHeader(header.HeaderKey, header.HeaderValue);
            }

            HeadersHelper.Refresh();
            TempData["message"] = "Zmiany zosta³y zapisane";
            return RedirectToAction("Headers");
        }

        [Authorize]
        public ActionResult OCRService()
        {
            ServiceController sc = new ServiceController("2saOCR");

            try
            {
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    ViewData["serviceStatus"] = "Us³uga OCR jest uruchomiona.";
                    ViewData["statusClass"] = "ocrRunning";
                }
                else
                {
                    ViewData["serviceStatus"] = "Us³uga OCR nie jest uruchomiona.";
                    ViewData["statusClass"] = "ocrStopped";
                }
            }
            catch (Exception)
            {
                ViewData["serviceStatus"] = "Nie uda³o siê odnaleŸæ us³ugi OCR";
                ViewData["statusClass"] = "ocrError";
            }
        

            pOCRGetConfigurationResult r = TempData["entity"] as pOCRGetConfigurationResult;
            return View(r??_repository.GetOCRConfiguration());
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult OCRService(pOCRGetConfigurationResult ocrSettings,bool ocrEnabled)
        {
            TimeSpan sh;
            if (ocrSettings.OCRStartHour.Length!=5 || ocrSettings.OCRStartHour.IndexOf(':')!=2 || !TimeSpan.TryParse(ocrSettings.OCRStartHour, out sh))
            {
                ModelState.AddModelError("OCRStartHour", "godzina musi byæ w formacie gg:mm !");
                TempData["entity"] = ocrSettings;
            }
            if (ocrSettings.OCREndHour.Length != 5 || ocrSettings.OCREndHour.IndexOf(':') != 2 || !TimeSpan.TryParse(ocrSettings.OCREndHour, out sh))
            {
                ModelState.AddModelError("OCREndHour", "godzina musi byæ w formacie gg:mm !");
                TempData["entity"] = ocrSettings;
            }

            if (ModelState.IsValid)
            {
                _repository.SaveOCRConfiguration(ocrSettings.OCRStartHour, ocrSettings.OCREndHour,ocrEnabled);
                return RedirectToAction("OCRService");
            }
            return View(ocrSettings);
        }

        [Authorize]
        public ActionResult DocumentHistory(Guid documentID)
        {
            return View(new DocumentRepository().GetDocumentHistory(documentID));
        }

        private bool ValidateSender(Sender sender)
        {
            if(string.IsNullOrEmpty(sender.Company))
            {
                if (string.IsNullOrEmpty(sender.FirstName) || string.IsNullOrEmpty(sender.LastName))
                {
                    ModelState.AddModelError("FirstName", "");
                    ModelState.AddModelError("LastName", "");
                    ModelState.AddModelError("Company", "");

                    ModelState.AddModelError("_FORM", "Podaj imiê i nazwisko lub nazwê firmy.");
                }
            }

            return ModelState.IsValid;
        }
    }
}
