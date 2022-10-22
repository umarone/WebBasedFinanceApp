using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class PurchaseOrderReturn
    {
        [Key]

        public long PurchaseOrderReturnId { get; set; }        
         public Nullable<long> OrganizationID { get; set; }
        public string PurchaseOrderReturnNO { get; set; }
        public Nullable<long> GRNReturnID { get; set; }
        public Nullable<long> PurchaseOrderID { get; set; }
        public Nullable<long> SupplierID { get; set; }
        public Nullable<long> AccountID { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public string Description { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> BranchId { get; set; }



        [ForeignKey("OrganizationID")]
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }

        [ForeignKey("GRNReturnID")]
        public virtual GoodReceivedReturn GoodReceivedReturn  { get; set; }

        [ForeignKey("PurchaseOrderID")]
        public virtual PurchaseOrder PurchaseOrder  { get; set; }

        [ForeignKey("SupplierID")]
        public virtual SupplierDefinition SupplierDefinition  { get; set; }

        [ForeignKey("AccountID")]
        public virtual Account  Account { get; set; }
        [ForeignKey("BranchId")]
        public virtual BranchDefinition BranchDefinition { get; set; }

    }

   

}