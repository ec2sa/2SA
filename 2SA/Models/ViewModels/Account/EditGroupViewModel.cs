using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace eArchiver.Models.ViewModels.Account
{
    public class EditGroupViewModel
    {
        public Group Group { get; set; }
        public string[] AllRoles { get; set; }
        public List<string> GroupRoles { get; set; }
        public List<Client> Clients { get; set; }
        public List<MembershipUser> AllUsers { get; set; }
        public List<MembershipUser> GroupUsers { get; set; }
        public List<MembershipUser> NotInGroupUsers
        {
            get
            {
                List<MembershipUser> result = new List<MembershipUser>(AllUsers);
                foreach (var user in GroupUsers.Select(m=>m.UserName))
                {
                    try
                    {
                        MembershipUser mu = result.Single(m => m.UserName == user);
                        if (mu != null)
                            result.Remove(mu);
                    }
                    catch (InvalidOperationException) { }
                }
                return result;
            }
        }

        public bool IsGroupRole(string roleName)
        {
            return GroupRoles.Contains(roleName);
        }
    }
}
