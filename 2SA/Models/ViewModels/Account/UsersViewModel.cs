using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace eArchiver.Models.ViewModels.Account
{
    public class UsersViewModel
    {
        public IList<MembershipUser> Users { get; set; }
        public string ClientName { get; set; }
    }
}
