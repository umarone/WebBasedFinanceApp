using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.Models
{
    public class AccountMapping
    {
        [Key]
        public long Id { get; set; }
        public Nullable<long> OrganizationId { get; set; }
        public Nullable<long> AccountId { get; set; }
        public Nullable<long> BranchId { get; set; }
        public Nullable<AccountDefaultType> AccountDefaultType { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }
        [ForeignKey("BranchId")]
        public virtual BranchDefinition BranchDefinition { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }
}