using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eArchiver.Models.Repositories.Headers
{
    public class HeadersRepository
    {
        private  EArchiverDataContext _context = new EArchiverDataContext();

        private List<Header> _headers;

        protected List<Header> headers {
            get
            {
                if (_headers == null)
                {
                    if (HttpContext.Current.Cache["headers"] == null)
                    {
                        HttpContext.Current.Cache["headers"] = _context.Headers.ToList();
                    }
                    _headers = HttpContext.Current.Cache["headers"] as List<Header>;
                }
                return _headers;
            }
        }

        public string this[string key]
        {
            get
            {
                return headers.FirstOrDefault(h => h.HeaderKey.Equals(key, StringComparison.InvariantCultureIgnoreCase)).HeaderValue;
            }
        }

        public void UpdateHeader(string headerKey, string headerValue)
        {
            _context.Headers.Single(h => h.HeaderKey == headerKey).HeaderValue = headerValue;
            _context.SubmitChanges();
        }

        public List<Header> GetAllHeaders()
        {
            return _context.Headers.ToList();
        }

        public void Refresh()
        {
            _headers = null;
            HttpContext.Current.Cache.Remove("headers");
        }

    }
}
