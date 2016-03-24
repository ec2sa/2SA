using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Web.Security;
using eArchiver.Constants;
using eArchiver.Controllers;

namespace eArchiver
{
    /// <summary>
    /// Summary description for rs
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class rs : System.Web.Services.WebService
    {

        private string prefix;

        [WebMethod]
        public bool SaveScan(string username, string password,string filename,byte[] content){
            if(!validateUser(username,password))
                return false;
            return processFile(filename, content);
             
        }

        private bool processFile(string filename, byte[] content)
        {
            
            string scansDirectory = null;

            if (string.IsNullOrEmpty(prefix))
                scansDirectory = Properties.Settings.Default.ScansRootDirectory;
            else
                scansDirectory = Path.Combine(Properties.Settings.Default.ScansRootDirectory, prefix + "scans");

            string newScanFileName = Path.Combine(scansDirectory, filename);
            try
            {
                File.WriteAllBytes(newScanFileName, content);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool validateUser(string username, string password)
        {
         //   if (username.ToLower() != "rsuser")
         //       return false;
            bool validated = Membership.ValidateUser(username, password);
            MembershipUser u = Membership.GetUser(username);

            prefix = AppContext.GetClientPrefix(AppContext.GetUserCID((Guid)u.ProviderUserKey));

            return (validated
                &&
                Roles.IsUserInRole(username, RoleNames.RemoteScansImport)
                );
            
        }
        
    }
}
