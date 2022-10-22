using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;

namespace MudasirRehmanAlp.Helper
{
    public class WebProvider : RoleProvider
    {
        private AppEntities db = new AppEntities();
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            
                var result = (from user in db.Users
                              join userrole in db.UsersRole on user.UserId equals userrole.UserID
                              join role in db.Roles on userrole.RoleID equals role.RoleId
                              where user.UserName == username
                              select role.RoleName).ToArray();
                return result;
           
        }
      
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}