using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class GoodReceived
    {
        [Key]
        public long GoodReceivedNoteId { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> PurchaseOrderID { get; set; }
        public Nullable<long> AdjustmentNoteID { get; set; }
        public Nullable<long> StoreTransferNoteId { get; set; }
        
        public Nullable<DateTime> GRNDate { get; set; }
        public string GRNNO { get; set; }
        public string GRNType { get; set; }  
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        public Nullable<long> BranchId { get; set; }

        [ForeignKey("OrganizationID")]
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }
        [ForeignKey("BranchId")]
        public virtual BranchDefinition BranchDefinition  { get; set; }
        [ForeignKey("PurchaseOrderID")]
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        [ForeignKey("AdjustmentNoteID")]
        public virtual AdjustmentNote AdjustmentNote  { get; set; }
        [ForeignKey("StoreTransferNoteId")]
        public virtual StoreTransferNote StoreTransferNote  { get; set; }

    }
}