using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models.StoredPocedureModel
{
    public class ProcGetMaxAccountNo
    {
        public long AccountId { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> ParentID { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public Nullable<int> LevelID { get; set; }
        public Nullable<int> HeadID { get; set; }
        public string Description { get; set; }
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
    }
}