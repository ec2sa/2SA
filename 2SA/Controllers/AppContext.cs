using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using eArchiver.Constants;
using eArchiver.Models.Repositories.Shared;
using eArchiver.Models;
using eArchiver.Models.Repositories.Dictionaries;

namespace eArchiver.Controllers
{
    public static class AppContext
    {
        private static Dictionary<Guid, int> usersClients
        {
            get
            {
                if (HttpContext.Current.Cache["E9FE328C-87D2-45AD-B7E9-71B2AEC30FCE"] == null)
                {
                    HttpContext.Current.Cache.Insert("E9FE328C-87D2-45AD-B7E9-71B2AEC30FCE", new Dictionary<Guid, int>());
                }

                return HttpContext.Current.Cache["E9FE328C-87D2-45AD-B7E9-71B2AEC30FCE"] as Dictionary<Guid, int>;
            }
        }

        #region Roles Methods

        public static bool IsUserInRoleInContext(string RoleName)
        {
            try
            {
                MembershipUser user = Membership.GetUser();
                
                //tego wpuszczamy zawsze
                if (Roles.IsUserInRole(user.UserName, RoleNames.ClientAdministrator))
                    return true;

                EArchiverDataContext ctx = new EArchiverDataContext();
                var roles = ctx.GetUserRolesInClient((Guid)user.ProviderUserKey, AppContext.GetCID());
              
                return (roles.Where(r => r.RoleName == RoleName).Count() > 0);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool AllowScansRead()
        {
            if (IsUserInRoleInContext(RoleNames.ScansRead)
                || IsUserInRoleInContext(RoleNames.ScansFull))
                return true;
            return false;
        }

        public static bool AllowScansWrite()
        {
            return IsUserInRoleInContext(RoleNames.ScansFull);
        }

        public static bool AllowInfoTypeOneRead()
        {
            if (IsUserInRoleInContext(RoleNames.Level1Read)
                || IsUserInRoleInContext(RoleNames.Level1Full))
                return true;
            return false;
        }

        public static bool AllowInfoTypeOneWrite()
        {
            return IsUserInRoleInContext(RoleNames.Level1Full);
        }

        public static bool AllowInfoTypeTwoRead()
        {
            if (IsUserInRoleInContext(RoleNames.Level2Read)
                || IsUserInRoleInContext(RoleNames.Level2Full))
                return true;
            return false;
        }

        public static bool AllowInfoTypeTwoWrite()
        {
            return IsUserInRoleInContext(RoleNames.Level2Full);
        }

        public static bool AllowInfoTypeThreeRead()
        {
            if (IsUserInRoleInContext(RoleNames.Level3Read)
                || IsUserInRoleInContext(RoleNames.Level3Full))
                return true;
            return false;
        }

        public static bool AllowInfoTypeThreeWrite()
        {
            return IsUserInRoleInContext(RoleNames.Level3Full);
        }

        public static bool AllowContent()
        {
            return IsUserInRoleInContext(RoleNames.LevelContent);
        }

        public static bool AllowOCR()
        {
            DictionaryRepository dr = new DictionaryRepository();
            return dr.GetOCRConfiguration().OCREnabled == "1" ? true : false;
        }

        public static bool IsAdmin()
        {
            return IsUserInRoleInContext(RoleNames.Administrator);
        }

        public static bool IsClientAdmin()
        {
            return IsUserInRoleInContext(RoleNames.ClientAdministrator);
        }

        public static bool IsClientAdminButInOtherContext()
        {
            string prefix = new ClientsRepository().GetClient(GetCID()).ClientPrefix;
            return IsUserInRoleInContext(RoleNames.ClientAdministrator)
                   && !string.IsNullOrEmpty(prefix);

        }

        #endregion

        public static string GetUserName()
        {
            return Membership.GetUser().UserName;
        }

        public static Guid GetUserGuid()
        {
            MembershipUser user = Membership.GetUser();
            if (user == null)
                throw new ArgumentException("No user found");
            return new Guid(user.ProviderUserKey.ToString());
        }

        public static int GetUserCID()
        {
            return GetUserCID((Guid)Membership.GetUser().ProviderUserKey);
        }

        public static int GetUserCID(Guid userID)
        {
       
            var userInClient=  new EArchiverDataContext().UsersInClients.FirstOrDefault(u => u.UserID == userID);
            if (userInClient != null)
                return userInClient.ClientID;
            else
                return 0;
                    
        }

        public static string GetClientPrefix()
        {
            int cid = GetCID();
            return new ClientsRepository().GetClient(cid).ClientPrefix;

        }

        public static string GetClientPrefix(int cid)
        {
            
            return new ClientsRepository().GetClient(cid).ClientPrefix;

        }

        public static int GetCID()
        {
            Guid currentUserGuid = GetUserGuid();

            if (usersClients.ContainsKey(currentUserGuid))
            {
                return usersClients[currentUserGuid];
            }
            else
            {
                List<int> clientIDs = new ClientsRepository().GetCID(currentUserGuid);
                if (clientIDs.Count > 0)
                {
                    usersClients.Add(currentUserGuid, clientIDs[0]);
                    return clientIDs[0];
                }
                else
                    return -1;
            }
        }

        public static List<int> GetCIDs()
        {
            Guid currentUserGuid = GetUserGuid();
            return new ClientsRepository().GetCID(currentUserGuid);
         
        }

        public static void ChangeClient(int selectedClientID)
        {
            Guid currentUserGuid = GetUserGuid();

            if (usersClients.ContainsKey(currentUserGuid))
            {
                usersClients[currentUserGuid] = selectedClientID;
            }
            else
            {
                usersClients.Add(currentUserGuid, selectedClientID);
            }
        }
    }
}
