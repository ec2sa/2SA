using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using eArchiver.Models.Repositories.Shared;
using eArchiver.Controllers;

namespace eArchiver.Models.Repositories.Account
{
    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
        MembershipUser CreateUser(string userName, string password, string email, bool isActive, out MembershipCreateStatus membershipCreateStatus);
        List<MembershipUser> GetAllUsers();
        List<MembershipUser> GetMembershipUsersByGuids(List<Guid> guids);
        List<MembershipUser> GetClientUsers(int clientID);
    }

    public class AccountMembershipService : IMembershipService
    {
        private MembershipProvider _provider;

        public int FindUsersByEmail(string email)
        { 
            int a = 0;
            _provider.GetUser(null, true);
            return _provider.FindUsersByEmail(email, 1, 10, out a).Count;
        }

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        public MembershipUser CreateUser(string userName, string password, string email, bool isActive, out MembershipCreateStatus membershipCreateStatus)
        {
            return _provider.CreateUser(userName, password, email, null, null, isActive, null, out membershipCreateStatus);
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
            return currentUser.ChangePassword(oldPassword, newPassword);
        }

        public List<MembershipUser> GetAllUsers()
        {
            return Membership.GetAllUsers().OfType<MembershipUser>().ToList();
        }

        public List<MembershipUser> GetMembershipUsersByGuids(List<Guid> guids)
        {
            List<MembershipUser> membershipUsers = new List<MembershipUser>();
            foreach (var guid in guids)
            {
                membershipUsers.Add(Membership.GetUser(guid));
            }
            
            return membershipUsers;
        }

        public List<MembershipUser> GetClientUsers(int clientID)
        {
            ClientsRepository clientsRepository = new ClientsRepository();
            var client = clientsRepository.GetClient(clientID);

            //all users from current client
            List<MembershipUser> allUsers;

            if (!string.IsNullOrEmpty(client.ClientPrefix))
            {
                allUsers = GetAllUsers()
                  .Where(u => client.UsersInClients.Where(uc => uc.UserID == (Guid)u.ProviderUserKey).Count() > 0
                      && u.UserName.StartsWith(client.ClientPrefix)).ToList();
            }
            else
            {
                allUsers = GetAllUsers();
                  //.Where(u => client.UsersInClients.Where(uc => uc.UserID == (Guid)u.ProviderUserKey).Count() > 0).ToList();
            }

            return allUsers;
        }
    }
}
