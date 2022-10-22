using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class StoreTransferNote
    {
        [Key]
        public long Id { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public string Code { get; set; }
        public Nullable<DateTime> STNDate { get; set; }
        public string Description { get; set; }
        public Nullable<long> FromBranchId { get; set; }
        public Nullable<long> ToBranchId { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        [ForeignKey("OrganizationID")]
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }
        [ForeignKey("FromBranchId")]
        public virtual BranchDefinition FromBranchDefinition  { get; set; }
        
    }
}