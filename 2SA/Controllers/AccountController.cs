using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

using eArchiver.Models.Repositories.Account;
using eArchiver.Models.ViewModels.Account;
using eArchiver.Models;
using eArchiver.Constants;
using eArchiver.Models.Repositories.Shared;
using eArchiver.Attributes;
using System.IO;


namespace eArchiver.Controllers
{
   
    [HandleError]
    public class AccountController : Controller
    {



        public IFormsAuthentication FormsAuth { get; private set; }
        public IMembershipService MembershipService { get; private set; }
        public AccountRepository _repository { get; private set; }

        public AccountController() : this(null, null)
        {
        }

        public AccountController(IFormsAuthentication formsAuth, IMembershipService service)
        {
            FormsAuth = formsAuth ?? new FormsAuthenticationService();
            MembershipService = service ?? new AccountMembershipService();
            _repository = new AccountRepository();
        }

        [ContextAuthorize(Roles = RoleNames.Administrator)]
        public string GetPrefixForClient(int clientID)
        {
         var p =  new ClientsRepository().GetClient(clientID).ClientPrefix;
         return p;
        }

        [ContextAuthorize(Roles = RoleNames.Administrator)]
        public ActionResult Index()
        {
            return View();
        }

        
        [ContextAuthorize(Roles = RoleNames.Administrator)]
        public ActionResult Groups()
        {
            GroupsViewModel viewModel = new GroupsViewModel()
            {
                Groups = _repository.GetGroups().ToList()
            };
            
            return View(viewModel);
        }

        [ContextAuthorize(Roles = RoleNames.Administrator)]
        public ActionResult Users()
        {
            string clientName = string.Empty;

            IList<MembershipUser> users;
            if (AppContext.IsClientAdmin() && !AppContext.IsClientAdminButInOtherContext())
            {
                users = MembershipService.GetAllUsers();
            }
            else
            {
                clientName = new ClientsRepository().GetClientName(AppContext.GetCID());
                users = MembershipService.GetClientUsers(AppContext.GetCID());
            }

           

            UsersViewModel viewModel = new UsersViewModel()
            {
                Users=users,
                ClientName = clientName
            };
            return View(viewModel);
        }

        [ContextAuthorize(Roles = RoleNames.Administrator)]
        public ActionResult EditGroup(Guid id)
        {
           // Guid guid = new Guid((string)id);
            string[] allRoles = Roles.GetAllRoles();
            List<string> tmp = new List<string>(allRoles);
            tmp.Remove(RoleNames.RemoteScansImport);
            if(!AppContext.IsClientAdmin())
            { tmp.Remove(RoleNames.ClientAdministrator); }
            allRoles = tmp.ToArray();

            List<string> roles = new List<string>();

            List<MembershipUser> allUsers;
            if (AppContext.IsClientAdmin())
                allUsers = MembershipService.GetAllUsers();
            else
                allUsers = MembershipService.GetClientUsers(AppContext.GetCID());

            EditGroupViewModel viewModel = new EditGroupViewModel()
            {
                Group = _repository.GetGroup(id),
                AllRoles = allRoles,
                Clients = new ClientsRepository().GetUserClients(AppContext.GetUserGuid()),
                GroupRoles = _repository.GetGroupRoles(id),
                AllUsers = allUsers,
                GroupUsers = MembershipService.GetMembershipUsersByGuids(_repository.GetGroupUsers(id)),

            };
            return View(viewModel);
        }

        [ContextAuthorize(Roles = RoleNames.Administrator)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditGroup(FormCollection formValues, Guid id)
        {
            if(formValues["Submit"].Equals("Zapisz", StringComparison.InvariantCultureIgnoreCase))
            {
                Group group = _repository.GetGroup(id);
                UpdateModel<Group>(group);
                
                //group.ClientID = AppContext.GetCID();

                #region Add/Remove Roles
                List<string> allRoles = new List<string>(Roles.GetAllRoles());  
                allRoles.Remove(RoleNames.RemoteScansImport);
                if (!AppContext.IsClientAdmin())
                {
                    allRoles.Remove(RoleNames.ClientAdministrator);
                }
                List<string> groupRoles = _repository.GetGroupRoles(id);

                List<string> rolesToAdd = new List<string>();
                List<string> rolesToRemove = new List<string>();

                foreach (string role in allRoles)
                {
                    if (formValues[role].Equals("false"))
                    {
                        if (groupRoles.Remove(role))
                        {
                            rolesToRemove.Add(role);
                        }
                    }
                    else
                    {
                        groupRoles.Add(role);
                        rolesToAdd.Add(role);
                    }
                }

                _repository.AddRolesToGroup(rolesToAdd, id);
                _repository.RemoveRolesFromGroup(rolesToRemove, id);

                #endregion

                #region Add/Remove Users

                List<string> groupUsersIds;
                if (formValues["GroupUsers"] != null)
                {   
                    //lista użytkowników w grupie po zmianie
                    groupUsersIds = new List<string>(formValues["GroupUsers"].Split(new char[] { ',' }));
                }
                else
                {
                    groupUsersIds = new List<string>();
                }
                //lista użytkowników w grupie przed zmianą
                List<Guid> groupUsers = _repository.GetGroupUsers(id);

                List<Guid> usersToAdd = new List<Guid>();
                List<Guid> usersToRemove = new List<Guid>();

                foreach (Guid userId in groupUsers)
                {
                    if (!groupUsersIds.Contains(userId.ToString()))
                        usersToRemove.Add(userId);
                }
                foreach (string userId in groupUsersIds)
                {
                    Guid g = new Guid(userId);
                    if (!groupUsers.Contains(g))
                        usersToAdd.Add(g);
                }

                _repository.RemoveUsersFromGroup(usersToRemove, id);
                _repository.AddUsersToGroup(usersToAdd, id);
                
                #endregion

                _repository.SubmitChanges();
            }

            return RedirectToAction("Groups");
        }

        [ImportModelStateFromTempData]
        [ContextAuthorize(Roles = RoleNames.Administrator)]
        public ActionResult NewGroup()
        {
            return View(GetNewGroupViewModel());
        }

        [ExportModelStateToTempData]
        [ContextAuthorize(Roles = RoleNames.Administrator)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult NewGroup(FormCollection formValues)
        {
            if (formValues["Submit"].Equals("Zapisz", StringComparison.InvariantCultureIgnoreCase))
            {
                if (!string.IsNullOrEmpty(formValues["GroupName"]))
                {
                    Guid id = _repository.AddGroup(formValues["GroupName"], int.Parse(formValues["ClientID"]));
                    Group group = _repository.GetGroup(id);
                    UpdateModel<Group>(group);


                    //group.ClientID = AppContext.GetCID();

                    #region Add/Remove Roles
                    List<string> allRoles = new List<string>(Roles.GetAllRoles());
                    allRoles.Remove(RoleNames.RemoteScansImport);
                    if (!AppContext.IsClientAdmin())
                    { allRoles.Remove(RoleNames.ClientAdministrator); }

                    List<string> groupRoles = _repository.GetGroupRoles(id);

                    List<string> rolesToAdd = new List<string>();
                    List<string> rolesToRemove = new List<string>();

                    foreach (string role in allRoles)
                    {
                        if (formValues[role].Equals("false"))
                        {
                            if (groupRoles.Remove(role))
                            {
                                rolesToRemove.Add(role);
                            }
                        }
                        else
                        {
                            groupRoles.Add(role);
                            rolesToAdd.Add(role);
                        }
                    }

                    _repository.AddRolesToGroup(rolesToAdd, id);
                    _repository.RemoveRolesFromGroup(rolesToRemove, id);

                    #endregion

                    #region Add/Remove Users

                    List<string> groupUsersIds;
                    if (formValues["GroupUsers"] != null)
                    {
                        //lista użytkowników w grupie po zmianie
                        groupUsersIds = new List<string>(formValues["GroupUsers"].Split(new char[] { ',' }));
                    }
                    else
                    {
                        groupUsersIds = new List<string>();
                    }
                    //lista użytkowników w grupie przed zmianą
                    List<Guid> groupUsers = _repository.GetGroupUsers(id);

                    List<Guid> usersToAdd = new List<Guid>();
                    List<Guid> usersToRemove = new List<Guid>();

                    foreach (Guid userId in groupUsers)
                    {
                        if (!groupUsersIds.Contains(userId.ToString()))
                            usersToRemove.Add(userId);
                    }
                    foreach (string userId in groupUsersIds)
                    {
                        Guid g = new Guid(userId);
                        if (!groupUsers.Contains(g))
                            usersToAdd.Add(g);
                    }

                    _repository.RemoveUsersFromGroup(usersToRemove, id);
                    _repository.AddUsersToGroup(usersToAdd, id);

                    #endregion

                    _repository.SubmitChanges();
                }
                else
                {
                    ModelState.AddModelError("userNameTextBoxName", "Nazwa grupy nie może być pusta.");
                    //return View("NewGroup", GetNewGroupViewModel());
                    return RedirectToAction("NewGroup");

                }
                    
            }

            return RedirectToAction("Groups");
        }

        private EditGroupViewModel GetNewGroupViewModel()
        {
            string[] allRoles = Roles.GetAllRoles();
            List<string> tmp = new List<string>(allRoles);
            tmp.Remove(RoleNames.RemoteScansImport);
            if (!AppContext.IsClientAdmin())
            { tmp.Remove(RoleNames.ClientAdministrator); }
            allRoles = tmp.ToArray();

            List<MembershipUser> allUsers;
            if (AppContext.IsClientAdmin())
                allUsers = MembershipService.GetAllUsers();
            else
                allUsers = MembershipService.GetClientUsers(AppContext.GetCID());

            EditGroupViewModel viewModel = new EditGroupViewModel()
            {
                Group = new Group(),
                AllRoles = allRoles,
                Clients = new ClientsRepository().GetUserClients(AppContext.GetUserGuid()),
                GroupRoles = new List<string>(),
                AllUsers = allUsers,
                GroupUsers = new List<MembershipUser>()
            };

            return viewModel;
        }

        [ContextAuthorize(Roles = RoleNames.Administrator)]
        public ActionResult EditUser(object id)
        {
            Guid guid = new Guid((string)id);

            EditUserViewModel viewModel = new EditUserViewModel()
            {
                User = Membership.GetUser(guid),
                AllGroups = _repository.GetGroups().ToList(),
                UserGroups = _repository.GetUserGroups(guid).ToList(),
            };

            return View(viewModel);
        }

        [ContextAuthorize(Roles = RoleNames.Administrator)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditUser(FormCollection formValues, Guid id)
        {
            if (formValues["Submit"].Equals("Zapisz", StringComparison.InvariantCultureIgnoreCase))
            {
                MembershipUser user = Membership.GetUser(id);
                user.Comment = formValues["Name"];
                user.IsApproved = !formValues["Active"].Equals("false");
                user.Email = formValues["Email"];
                Membership.UpdateUser(user);

                #region Add/Remove Groups

                List<string> userGroups;
                if (formValues["UserGroups"] != null)
                {   
                    //lista grup uzytkownika po zmianie
                    userGroups = new List<string>(formValues["UserGroups"].Split(new char[] { ',' }));
                }
                else
                {
                    userGroups = new List<string>();
                }

                List<Guid> userGroupsBefore = _repository.GetUserGroups(id).Select(g=>g.GroupId).ToList();

                List<Guid> groupsToAdd = new List<Guid>();
                List<Guid> groupsToRemove = new List<Guid>();

                foreach (Guid groupId in userGroupsBefore)
                {
                    if (!userGroups.Contains(groupId.ToString()))
                        groupsToRemove.Add(groupId);
                }
                foreach (string groupId in userGroups)
                {
                    Guid g = new Guid(groupId);
                    if (!userGroupsBefore.Contains(g))
                        groupsToAdd.Add(g);
                }

                _repository.RemoveUserFromGroups(id, groupsToRemove);
                _repository.AddUserToGroups(id, groupsToAdd);

                #endregion

                _repository.SubmitChanges();
            }

            return RedirectToAction("Users");
        }

        [ContextAuthorize(Roles = RoleNames.Administrator)]
        public ActionResult NewUser()
        {
            EditUserViewModel viewModel = new EditUserViewModel()
            {
                AllGroups = _repository.GetGroups().ToList(),
                UserGroups = new List<Group>()
            };

            return View(viewModel);
        }

        [ContextAuthorize(Roles = RoleNames.Administrator)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult NewUser(FormCollection formValues, string Login, string Name, string Email)
        {
            bool validationFailed = false;
            if (formValues["Submit"].Equals("Zapisz", StringComparison.InvariantCultureIgnoreCase))
            {
                string userName = formValues["Login"];
                string password = formValues["Password"];
                string repeatPassword = formValues["RepeatPassword"];
                string email = formValues["UserEmail"];
                bool isActive = !formValues["Active"].Equals("false");


                if (ValidateRegistration(userName, password, repeatPassword))
                {
                    MembershipCreateStatus createStatus;
                    MembershipUser user = MembershipService.CreateUser(userName, password, email, isActive, out createStatus);

                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        user.Comment = formValues["Name"];
                        Membership.UpdateUser(user);
                        Guid id = (Guid)user.ProviderUserKey;
                        new ClientsRepository().AssignUserToClient(id, AppContext.GetCID());
                        
                        #region Add/Remove Groups

                        List<string> userGroups;
                        if (formValues["UserGroups"] != null)
                        {
                            //lista grup uzytkownika po zmianie
                            userGroups = new List<string>(formValues["UserGroups"].Split(new char[] { ',' }));
                        }
                        else
                        {
                            userGroups = new List<string>();
                        }

                        List<Guid> userGroupsBefore = _repository.GetUserGroups(id).Select(g => g.GroupId).ToList();

                        List<Guid> groupsToAdd = new List<Guid>();
                        List<Guid> groupsToRemove = new List<Guid>();

                        foreach (Guid groupId in userGroupsBefore)
                        {
                            if (!userGroups.Contains(groupId.ToString()))
                                groupsToRemove.Add(groupId);
                        }
                        foreach (string groupId in userGroups)
                        {
                            Guid g = new Guid(groupId);
                            if (!userGroupsBefore.Contains(g))
                                groupsToAdd.Add(g);
                        }

                        _repository.RemoveUserFromGroups(id, groupsToRemove);
                        _repository.AddUserToGroups(id, groupsToAdd);

                        #endregion

                        _repository.SubmitChanges();

                    }
                    else
                    {
                        ModelState.AddModelError("_FORM", ErrorCodeToString(createStatus));
                        validationFailed = true;
                    }
                }
                else
                    validationFailed = true;
            }

            if (validationFailed)
            {
                EditUserViewModel viewModel = new EditUserViewModel()
                {
                    AllGroups = _repository.GetGroups().ToList(),
                    UserGroups = new List<Group>()
                };
                return View(viewModel);
            }

            return RedirectToAction("Users");
        }

        [ContextAuthorize(Roles = RoleNames.Administrator)]
        public ActionResult ResetPassword(Guid id)
        {
            MembershipUser user = Membership.GetUser(id);
            string newPassword = user.ResetPassword();

            TempData["NewPassword"] = newPassword;
            return RedirectToAction("EditUser", new { id = id });
        }

        public ActionResult LogOn()
        {

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult LogOn(string userName, string password, bool rememberMe, string returnUrl)
        {

            if (!ValidateLogOn(userName, password))
            {
                return View();
            }

            FormsAuth.SignIn(userName, rememberMe);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult LogOff()
        {

            FormsAuth.SignOut();

            return RedirectToAction("Index", "Home");
        }

        [ContextAuthorize(Roles = RoleNames.Administrator)]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SetRemoteScansUserPassword()
        {
            MembershipUser rsUser;
            
            string clientRSUserName =string.Format("{0}RSUser",AppContext.GetClientPrefix());


            if (Membership.FindUsersByName(clientRSUserName).Count == 0)
            {
                rsUser = Membership.CreateUser(clientRSUserName, "password");
                if (!Roles.IsUserInRole(clientRSUserName, RoleNames.RemoteScansImport))
                {
                    Roles.AddUserToRole(clientRSUserName, RoleNames.RemoteScansImport);
                }
                new ClientsRepository().AssignUserToClient((Guid)rsUser.ProviderUserKey, AppContext.GetCID());
            }

            rsUser = Membership.GetUser(clientRSUserName);
            if (rsUser.IsLockedOut)
                rsUser.UnlockUser();
            return View();

        }

        [ContextAuthorize(Roles = RoleNames.Administrator)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SetRemoteScansUserPassword(FormCollection formValues)
        {
     
            MembershipUser rsUser;

            string clientRSUserName = string.Format("{0}RSUser", AppContext.GetClientPrefix());

            if (Membership.FindUsersByName(clientRSUserName).Count == 0)
            {
                rsUser = Membership.CreateUser(clientRSUserName, "password");
                if (!Roles.IsUserInRole(clientRSUserName, RoleNames.RemoteScansImport))
                {
                    Roles.AddUserToRole(clientRSUserName, RoleNames.RemoteScansImport);
                }
             
            } 


            rsUser = Membership.GetUser(clientRSUserName);
            TempData["rsUserPwd"] = rsUser.ResetPassword();
            return View();

        }

        #region client's actions
        [ContextAuthorize(Roles=RoleNames.ClientAdministrator)]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Clients()
        {
            List<Client> clients = new ClientsRepository().GetAllClients().ToList();
            return View(clients);
        }

        [ContextAuthorize(Roles = RoleNames.ClientAdministrator)]
        public ActionResult NewClient()
        {
            return View();
        }

        [ContextAuthorize(Roles = RoleNames.ClientAdministrator)]

        //QUOTA [Quota(QuotaType=QuotaTypes.Clients)]
        [AcceptVerbs(HttpVerbs.Post)]
        [ContextAuthorize(Roles = RoleNames.ClientAdministrator)]
        public ActionResult NewClient(FormCollection formValues)
        {
            
            if (formValues["Submit"].Equals("Zapisz", StringComparison.InvariantCultureIgnoreCase))
            {

              
                    ClientsRepository cr = new ClientsRepository();
                    Client client = new Client();
                    UpdateModel<Client>(client);
                    if (ValidateClient(client.ClientName, client.ClientPrefix))
                    {
                        try
                        {
                            cr.CreateClient(client);
                            int newClientID = cr.GetAllClients().Where(c => c.ClientName == client.ClientName).First().ClientID;

                            string newUserName = string.Format("{0}{1}", client.ClientPrefix, "Admin");
                            string newAdminGroupName = string.Format("{0}{1}", client.ClientPrefix, "Administratorzy");
                            string newUsersGroupName = string.Format("{0}{1}", client.ClientPrefix, "Użytkownicy");


                            MembershipUser newUser = Membership.CreateUser(newUserName, newUserName);

                            cr.AssignUserToClient((Guid)newUser.ProviderUserKey, newClientID);

                            Guid newGroupGuid = _repository.AddGroup(newUsersGroupName, newClientID);
                            _repository.GetGroup(newGroupGuid).ClientID = newClientID;

                            newGroupGuid = _repository.AddGroup(newAdminGroupName, newClientID);
                            _repository.GetGroup(newGroupGuid).ClientID = newClientID;
                           

                           // _repository.SubmitChanges();


                            _repository.AddUserToGroup((Guid)newUser.ProviderUserKey, newGroupGuid);
                            _repository.AddRoleToGroup(RoleNames.Administrator, newGroupGuid);

                            _repository.AddUserToGroup((Guid)Membership.GetUser().ProviderUserKey, newGroupGuid);
                            string scansDir = Path.Combine(Properties.Settings.Default.ScansRootDirectory, client.ClientPrefix + "scans");
                            if (!Directory.Exists(scansDir))
                                Directory.CreateDirectory(scansDir);

                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("AllClient", ex.Message);
                            return View();
                        }
                    }
                    else
                    {
                        return View();
                    }
                
            }
            
            return RedirectToAction("Clients");
        }

        [ContextAuthorize(Roles = RoleNames.ClientAdministrator)]
        public ActionResult EditClient(int clientID)
        {
            return View(new ClientsRepository().GetClient(clientID));
        }

        [ContextAuthorize(Roles = RoleNames.ClientAdministrator)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditClient(FormCollection formValues, int clientID)
        {
            if (formValues["Submit"].Equals("Zapisz", StringComparison.InvariantCultureIgnoreCase))
            {
                ClientsRepository clientRepo = new ClientsRepository();
                Client client = clientRepo.GetClient(clientID);
                UpdateModel<Client>(client);
                if (ValidateClient(client.ClientName, client.ClientPrefix))
                {
                    clientRepo.SubmitChanges();
                }
                else
                {
                    return View(client);
                }
            }

            return RedirectToAction("Clients");
        }

        #endregion

        #region Commented - register, change password code
        //public ActionResult Register()
        //{

        //    ViewData["PasswordLength"] = MembershipService.MinPasswordLength;

        //    return View();
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Register(string userName, string email, string password, string confirmPassword)
        //{

        //    ViewData["PasswordLength"] = MembershipService.MinPasswordLength;

        //    if (ValidateRegistration(userName, password, confirmPassword))
        //    {
        //        // Attempt to register the user
        //        MembershipCreateStatus createStatus = MembershipService.CreateUser(userName, password, email);

        //        if (createStatus == MembershipCreateStatus.Success)
        //        {
        //            FormsAuth.SignIn(userName, false /* createPersistentCookie */);
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("_FORM", ErrorCodeToString(createStatus));
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View();
        //}


        [Authorize]
        public ActionResult ChangePassword()
        {

            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;

            return View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Exceptions result in password not being changed.")]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {

            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;

            if (!ValidateChangePassword(currentPassword, newPassword, confirmPassword))
            {
                return View();
            }

            try
            {
                if (MembershipService.ChangePassword(User.Identity.Name, currentPassword, newPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("_FORM", "Błąd.");
                    return View();
                }
            }
            catch
            {
                ModelState.AddModelError("_FORM", "Błąd.");
                return View();
            }
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
        #endregion

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity is WindowsIdentity)
            {
                throw new InvalidOperationException("Windows authentication is not supported.");
            }
        }

        #region Validation Methods

        private bool ValidateChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (String.IsNullOrEmpty(currentPassword))
            {
                ModelState.AddModelError("currentPassword", "Musisz podać obecne hasło.");
            }
            if (newPassword == null || newPassword.Length < MembershipService.MinPasswordLength)
            {
                ModelState.AddModelError("newPassword",
                    String.Format(CultureInfo.CurrentCulture,
                         "Hasło musi zawierać {0} lub więcej znaków.",
                         MembershipService.MinPasswordLength));
            }

            if (!String.Equals(newPassword, confirmPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("_FORM", "Powtórz poprawnie nowe hasło.");
            }

            return ModelState.IsValid;
        }

        private bool ValidateLogOn(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", "Podaj login.");
            }
            if (String.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("password", "Podaj hasło.");
            }
            if (!MembershipService.ValidateUser(userName, password))
            {
                ModelState.AddModelError("_FORM", "Login lub hasło niepoprawne.");
            }

            return ModelState.IsValid;
        }

        private bool ValidateClient(string clientName, string clientPrefix)
        {
            if (string.IsNullOrEmpty(clientName))
                ModelState.AddModelError("ClientName", "Podaj nazwę klienta");
            if(string.IsNullOrEmpty(clientPrefix))
                ModelState.AddModelError("ClientPrefix", "Podaj prefiks klienta");

            return ModelState.IsValid;
        }

        private bool ValidateRegistration(string userName,string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(userName.Replace(AppContext.GetClientPrefix()+"_",string.Empty)))
            {
                ModelState.AddModelError("_FORM", "Podaj login.");
            }
            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("_FORM", "Podaj hasło");
            }
            if (!string.Equals(password, confirmPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("_FORM", "Powtórz poprawnie hasło.");
            }
            return ModelState.IsValid;
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://msdn.microsoft.com/en-us/library/system.web.security.membershipcreatestatus.aspx for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Użytkownik o takiej nazwie juz istnieje. Proszę podać inną nazwę użytkownika.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Użytkownik o takim adresie email już istnieje. Proszę podać inny adres email.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Podane hasło jest nieprawidłowe. Proszę podać poprawne hasło.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Podany adres email jest nieprawidłowy. Proszę poprawić wartość i spróbować ponownie.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Podana nazwa użytkownika jest nieprawidłowa. Proszę poprawić wartość i spróbować ponownie.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
