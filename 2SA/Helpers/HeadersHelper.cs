using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eArchiver.Models.Repositories.Headers;
using System.Web.Mvc;

namespace eArchiver.Helpers
{
    public static class HeadersHelper
    {
        private static HeadersRepository _repository = new HeadersRepository();

        public static void Refresh()
        {
            _repository.Refresh();
            _repository = new HeadersRepository();
        }

        public static HeaderNames Headers(this HtmlHelper helper)
        {
            return new HeaderNames()
            {
                Skan = _repository["skan"]
                ,
                Skanu = _repository["skanu"]
                ,
                Skany = _repository["skany"]
                ,
                Skanow=_repository["skanow"]
                ,
                Dokument = _repository["dokument"]
                ,
                Dokumentu = _repository["dokumentu"]
                ,
                Dokumenty = _repository["dokumenty"]
                ,
                NumerDokumentu =_repository["numerDokumentu"]
                ,
                NumerSprawy=_repository["numerSprawy"]
                ,
                Data = _repository["data"]
                ,
                Opis=_repository["opis"]
                ,
                Kategoria = _repository["kategoria"]
                ,
                Kategorii = _repository["kategorii"]
                ,
                Rodzaj = _repository["rodzaj"]
                ,
                Rodzajow = _repository["rodzajow"]
                ,
                Nadawca = _repository["nadawca"]
                ,
                Nadawcy = _repository["nadawcy"]
                ,
                Typ = _repository["typ"]
                ,
                Typow = _repository["typow"]
                ,
                Tagi =_repository["tagi"]
                ,
                Firma=_repository["firma"]
                ,
                Flaga1 = _repository["flaga1"]
                ,
                Edytor = _repository["edytor"]
            };
        }
    }

    public class HeaderNames{
        public string Skan { get;  set; }
        public string Skanu { get;  set; }
        public string Skany { get;  set; }
        public string Skanow { get; set; }
        public string Dokument { get;  set; }
        public string Dokumentu { get;  set; }
        public string Dokumenty { get;  set; }
        public string Edytor { get; set; }
        public string NumerDokumentu { get;  set; }
        public string NumerSprawy { get;  set; }
        public string Data { get;  set; }
        public string Opis { get; set; }
        public string Kategoria { get;  set; }
        public string Kategorii { get;  set; }
        public string Rodzaj { get;  set; }
        public string Rodzajow { get;  set; }
        public string Nadawca { get;  set; }
        public string Nadawcy { get;  set; }
        public string Typ { get; set; }
        public string Typow { get; set; }
        public string Tagi{get;set;}
        public string Firma { get; set; }
        public string Flaga1 { get; set; }

    }
}
