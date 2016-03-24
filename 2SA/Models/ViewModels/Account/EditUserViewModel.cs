using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace eArchiver.Models.ViewModels.Account
{
    public class EditUserViewModel
    {
        public MembershipUser User { get; set; }
        public List<Group> AllGroups { get; set; }
        public List<Group> UserGroups { get; set; }
        public List<Group> NotUserGroups
        {
            get
            {
                List<Group> result = new List<Group>(AllGroups);
                foreach (var group in UserGroupsNames)
                {
                    try
                    {
                        Group g = result.Single(m => m.GroupName == group);
                        if (g != null)
                            result.Remove(g);
                    }
                    catch (InvalidOperationException)
                    { }
                }
                return result;
            }
        }
        
        private string[] UserGroupsNames
        {
            get
            {
                if(UserGroups != null)
                    return UserGroups.Select(g => g.GroupName).ToArray();
                
                return new string[]{};
            }
        }
    }
}
