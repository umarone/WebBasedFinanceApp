using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class AccountSystemList
    {
        [Key]
        public long AccountId { get; set; }
        public Nullable<long> ParentID { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public Nullable<int> LevelID { get; set; }
        public Nullable<int> HeadID { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsSystemAccount { get; set; }
       
    }
}