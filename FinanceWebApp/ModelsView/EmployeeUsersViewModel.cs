using MudasirRehmanAlp.Models.StoredPocedureModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.ModelsView
{
    public class EmployeeUsersViewModel
    {
        public long UserId { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> EmployeeID { get; set; }
        public Nullable<long> RoleID { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirsName { get; set; }
        public string LastName { get; set; }
        public string OrganizationName { get; set; }
        public string RoleName { get; set; }
        public Nullable<long> UserRoleID { get; set; }
        public Nullable<bool> IsActive { get; set; }

       public List<ProcGetBranchCheckListByEmployeeId> procGetBranchCheckListByEmployeeIds { get; set; }
    }
}