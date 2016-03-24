using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eArchiver.Models
{
    public partial class Sender
    {
        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(this.Company))
                    return string.Concat(this.FirstName, " ", this.LastName);
                else
                {
                    if(string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName))
                        return string.Format("[{0}]", Company);
                    else
                        return string.Format("{2} {1} - [{0}]", Company, FirstName, LastName);
                }
            }
        }
    }
}
