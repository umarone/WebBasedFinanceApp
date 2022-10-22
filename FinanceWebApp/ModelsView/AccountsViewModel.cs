using MudasirRehmanAlp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.ModelsView
{
    public class AccountsViewModel
    {
        public long AccountId { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> BranchId { get; set; }
        public string BranchName { get; set; }
        public Nullable<long> ParentID { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public Nullable<int> LevelID { get; set; }
        public Nullable<int> HeadID { get; set; }
        public string Description { get; set; }
        public string AccountNoAndAccountName { get; set; }
        public Nullable<decimal> NetTotal { get; set; }
        public Nullable<decimal> OpeningBalance { get; set; }
        public Nullable<decimal> ClosingBalance { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }

        public Nullable<bool> IsSystemAccount { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        public Nullable<long> CID { get; set; }
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }

        public string LevelOneName { get; set; }
        public string levelOneNo { get; set; }
        public Nullable<long> levelOneAccountID { get; set; }
        public string LevelTwoName { get; set; }
        public string levelTwoNo { get; set; }
        public Nullable<long> levelTwoAccountID { get; set; }
        public string LevelThreeName { get; set; }
        public string levelThreeNo { get; set; }
        public Nullable<long> levelThreeAccountID { get; set; }
        public string OrganizationUnitName { get; set; }

        public Nullable<long> CustomerStatementId { get; set; }


        public Nullable<long> MappingBranchId { get; set; }
        public string MappingBranchName { get; set; }
        public Nullable<AccountDefaultType> AccountDefaultType { get; set; }
        public int DefaultType { get; set; }
    }
}