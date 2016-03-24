using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eArchiver.Controllers;

namespace eArchiver.Models.Repositories.Account
{
    public class AccountRepository
    {
        EArchiverDataContext _context = new EArchiverDataContext();

        public IQueryable<Group> GetGroups()
        {
            var results=_context.Groups.AsQueryable();
            if(!AppContext.IsClientAdmin())
                results=results.Where(g=>AppContext.GetCIDs().Contains(g.ClientID));
    
            return results.OrderBy(g => g.GroupName).OrderBy(g => g.Client.ClientName);
        }

        public Group GetGroup(Guid id)
        {
            return GetGroups().Single(g => g.GroupId == id);
        }

        public List<string> GetGroupRoles(Guid groupId)
        {
            List<string> result = new List<string>();
            foreach (pGetGroupRolesResult row in _context.GetGroupRoles(groupId))
            {
                result.Add(row.RoleName);
            }

            return result;
        }

        public Guid AddGroup(string groupName,int? clientID)
        {
            Guid? groupId = null;
            _context.CreateGroup(groupName,clientID, ref groupId);
            return groupId.Value;
        }

        public List<Guid> GetGroupUsers(Guid groupId)
        {
            IQueryable<UsersInGroup> usersInGroup = _context.UsersInGroups.Where(u => u.GroupId == groupId);

            List<Guid> guids = new List<Guid>();
            foreach (var item in usersInGroup)
            {
                guids.Add(item.UserId);
            }

            return guids;
        }

        public void AddUsersToGroup(List<Guid> usersIds, Guid groupId)
        {
            foreach (Guid userId in usersIds)
            {
                AddUserToGroup(userId, groupId);
            }
        }

        public void AddUserToGroups(Guid userId, List<Guid> groupsIds)
        {
            foreach (Guid groupId in groupsIds)
            {
                AddUserToGroup(userId, groupId);
            }
        }
        
        public void AddUserToGroup(Guid userId, Guid groupId)
        {
            _context.AddUserToGroup(userId, groupId);
        }

        public void AddRolesToGroup(List<string> rolesNames, Guid groupId)
        {
            foreach (string roleName in rolesNames)
            {
                AddRoleToGroup(roleName, groupId);
            }
        }

        public void AddRoleToGroup(string roleName, Guid groupId)
        {
            _context.AddRoleToGroup(groupId, roleName);
        }

        public void RemoveUsersFromGroup(List<Guid> usersIds, Guid groupId)
        {
            foreach (Guid userId in usersIds)
            {
                RemoveUserFromGroup(userId, groupId);
            }
        }

        public void RemoveUserFromGroups(Guid userId, List<Guid> groupsIds)
        {
            foreach (Guid groupId in groupsIds)
            {
                RemoveUserFromGroup(userId, groupId);
            }
        }

        public void RemoveUserFromGroup(Guid userId, Guid groupId)
        {
            _context.RemoveUserFromGroup(userId, groupId);
        }

        public void RemoveRoleFromGroup(string roleName, Guid groupId)
        {
            _context.RemoveRoleFromGroup(roleName, groupId);
        }

        public void RemoveRolesFromGroup(List<string> rolesNames, Guid groupId)
        {
            foreach (string roleName in rolesNames)
            {
                RemoveRoleFromGroup(roleName, groupId);
            }
        }

        public IQueryable<Group> GetUserGroups(Guid id)
        {
            return from g in _context.UsersInGroups
                   where g.UserId == id
                   select g.Group;
        }
        
        public void SubmitChanges()
        {
            _context.SubmitChanges();
        }
    }
}
