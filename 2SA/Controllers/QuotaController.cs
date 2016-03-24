using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using eArchiver.Constants;
using eArchiver.Models.ViewModels.Quota;

namespace eArchiver.Controllers
{
 
    public class QuotaController : BaseController
    {
        public ActionResult Index()
        {
            QuotaTypes quotaType = (QuotaTypes)TempData["quotaType"];
            string currentValue = TempData["currentValue"].ToString();
            string maxValue = TempData["maxValue"].ToString();
            
            string message = string.Empty;

            switch (quotaType)
            {
                case QuotaTypes.Documents:
                    message = string.Concat("Przekroczona maksymalna liczba (", maxValue, ") dokumentów.");
                    break;
                case QuotaTypes.DocumentCategories:
                    message = string.Concat("Przekroczona maksymalna liczba (", maxValue, ") kategorii.");
                    break;
                case QuotaTypes.DocumentTypes:
                    message = string.Concat("Przekroczona maksymalna liczba (", maxValue, ") typów.");
                    break;
                case QuotaTypes.ScansInDocument:
                    message = string.Concat("Przekroczona maksymalna liczba (", maxValue, ") skanów w dokumencie.");
                    break;
                case QuotaTypes.ScansTotalSize:
                    message = string.Concat("Przekroczony maksymalny rozmiar(", maxValue, "MB) skanów w systemie.");
                    break;
                case QuotaTypes.Clients:
                    message = string.Concat("Przekroczony maksymalny liczba(", maxValue, ") klientów w systemie.");
                    break;
                default:
                    break;
            }

            QuotaViewModel viewModel = new QuotaViewModel() { Message = message };

            if ((bool)TempData["isAjax"])
                return PartialView("QuotaInfoControl", viewModel);
            else
                return View(viewModel);
        }

    }
}
