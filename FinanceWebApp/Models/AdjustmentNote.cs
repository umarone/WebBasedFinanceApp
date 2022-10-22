using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class AdjustmentNote
    {
        [Key]
        public long AdjustmentNoteId { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> BranchId { get; set; }
        public string AdjustmentNoteNO { get; set; }     
        public Nullable<DateTime> Date { get; set; }
        public string Description { get; set; }
        
        
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        public Nullable<decimal> SubTotalWithOutSaleTax { get; set; }
        public Nullable<decimal> SubTotalWithSaleTax { get; set; }
        public Nullable<long> FreightCharges { get; set; }
        public Nullable<decimal> NetTotal { get; set; }
        public string AmountInWord { get; set; }
    

        [ForeignKey("OrganizationID")]
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }
        [ForeignKey("BranchId")]
        public virtual BranchDefinition BranchDefinition { get; set; }

    }
}