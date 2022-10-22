using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.Models
{
    public class PurchaseOrder
    {
        [Key]
        public long PurchaseOrderId { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> SupplierID { get; set; }
        public Nullable<long> AccountID { get; set; }
        public string PurchaseOrderNO { get; set; }
        public Nullable <PuchaseOrderType> PuchaseOrderType { get; set; }
        public Nullable<DateTime> PurchaseOrderDate { get; set; }
        public Nullable<TransactionType> TransactionType { get; set; }
        public string PaymentTerms { get; set; }
        public Nullable<DateTime> DeliveryDate { get; set; }
        public Nullable<bool> ApprovalStatus { get; set; }
        public Nullable<long> ApprovalBy { get; set; }
        public Nullable<long> BranchId { get; set; }
        public Nullable<long> CurrencyID { get; set; }
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
        public string TermAndCondition { get; set; }
        public string FilePath { get; set; }
        public Nullable<bool> IsCompleted { get; set; }
        public Nullable<long> CompletedBy { get; set; }
        public Nullable<DateTime> CompletedDate { get; set; }
        public byte[] PurchaseOrderImage { get; set; }

        [ForeignKey("OrganizationID")]
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }
        [ForeignKey("SupplierID")]
        public virtual SupplierDefinition SupplierDefinition { get; set; }
        [ForeignKey("CurrencyID")]
        public virtual CurrencyDefinition CurrencyDefinition { get; set; }
        [ForeignKey("AccountID")]
        public virtual Account Account { get; set; }
     
        [ForeignKey("BranchId")]
        public virtual BranchDefinition BranchDefinition  { get; set; }

        
    }
}