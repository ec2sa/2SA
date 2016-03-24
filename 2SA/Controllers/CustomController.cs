using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using eArchiver.Models.Repositories.Scans;
using eArchiver.Models;
using eArchiver.Models.Repositories.Dictionaries;
using ScanManager;
using eArchiver.Controllers.Factories;
using eArchiver.Models.Repositories.Documents;

namespace eArchiver.Controllers
{

    public class CustomController : BaseController
    {
        public JsonResult GetTypeByCategoryID(int categoryID)
        {
            DictionaryRepository _repository = new DictionaryRepository();
            
            var data = _repository.GetTypes(categoryID).ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (eArchiver.Models.Type type in data)
            {
                list.Add(new SelectListItem()
                {
                    Text = type.Name,
                    Value = type.TypeID.ToString()
                });
            }

            return Json(list);
        }

        public JsonResult CreateSender(string firstName, string lastName, string company, string position, 
            string email, string webpage, string phonehome, string phonemobile, string phonework, 
            string faxwork, string postCode,string city,string street,string building,string flat,string post, string notes)
        {
            DictionaryRepository _repository = new DictionaryRepository();

            Sender sender = new Sender()
            {
                FirstName = firstName,
                LastName = lastName,
                Company = company,
                Position = position,
                Email = email,
                Webpage = webpage,
                PhoneHome = phonehome,
                PhoneMobile = phonemobile,
                PhoneWork = phonework,
                FaxWork = faxwork,
                PostCode=postCode,
                Post=post,
                City=city,
                Street=street,
                Building=building,
                Flat=flat,
                Notes = notes,
                ClientID = AppContext.GetCID()
            };

            _repository.CreateSender(sender);
            _repository.SubmitChanges();

            List<Sender> senders = _repository.GetSenders().ToList();
            
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (Sender s in senders)
            {
                bool selected = (s.FirstName.Equals(firstName) && s.LastName.Equals(lastName) && s.Company.Equals(company));
                list.Add(new SelectListItem()
                {
                    Text = s.FullName,
                    Value = s.SenderID.ToString(),
                    Selected = selected
                });
            }

            return Json(list);
        }

        public JsonResult DeleteCategory(int categoryID)
        {
            bool result = true;
            try
            {
                DictionaryRepository _repository = new DictionaryRepository();
                _repository.DeleteCategory(categoryID);
            }
            catch { result = false; }

            return Json(result);
        }

        public JsonResult DeleteType2(int type2ID)
        {
            bool result = true;
            try
            {
                DictionaryRepository _repository = new DictionaryRepository();
                _repository.DeleteType2(type2ID);
            }
            catch { result = false; }

            return Json(result);
        }

        public JsonResult DeleteType(int typeID)
        {
            bool result = true;
            try
            {
                DictionaryRepository _repository = new DictionaryRepository();
                _repository.DeleteType(typeID);
            }
            catch { result = false; }

            return Json(result);
        }

        public JsonResult AddScanToRecycleBin(Guid scanID)
        {
            bool result = true;
            try
            {
                ScansRepository repository = new ScansRepository();
                repository.AddScanToRecycleBin(scanID);
            }
            catch { result = false; }

            return Json(result);
        }

        public JsonResult RestoreScanFromRecycleBin(Guid scanID)
        {
            bool result = true;
            try
            {
                ScansRepository repository = new ScansRepository();
                repository.RestoreScanFromRecycleBin(scanID);
            }
            catch { result = false; }

            return Json(result);
        }

        public JsonResult DeleteScanFromRecycleBin(Guid scanID)
        {
            bool result = true;
            try
            {
                ScansRepository repository = new ScansRepository();
                repository.DeleteScanFromRecycleBin(scanID);
            }
            catch { result = false; }

            return Json(result);
        }

        public JsonResult RefreshAvailableScans()
        {
            ScanBrowser scans = ScanBrowserFactory.Create();

            int[] data = new int[2];

            data[0] = scans.GetScansCount(true);
            data[1] = scans.GetScansCount(false);

            return Json(data);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddScanVersion(Guid originalScanId, Guid versionScanId, Guid documentId)
        {
            try
            {
                DocumentRepository docRepository = new DocumentRepository();
                docRepository.AddScanVersion(originalScanId, versionScanId, documentId);
                docRepository.SubmitChanges();
            }
            catch
            {
            }

            return new EmptyResult();//RedirectToAction("Edit", "Documents", new { documentId = documentId });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public EmptyResult ChangeClient(int clientID)
        {
            AppContext.ChangeClient(clientID);

            return new EmptyResult();
        }
    }
}
