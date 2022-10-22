using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.Models
{
    public class VouchersHead
    {
        [Key]
        public long VoucherID { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> BranchId { get; set; }
        public Nullable<Guid> TransactionKey { get; set; }
        public string VoucherCode { get; set; }
        public Nullable<VoucherType> VoucherType { get; set; }
        public Nullable<DateTime> VoucherDate { get; set; }
        public Nullable<PaymentType> PaymentType { get; set; }
        public Nullable<long> PurchaseId { get; set; }
        public Nullable<long> SaleId { get; set; }
        public Nullable<long> SheetNo { get; set; }
        public Nullable<long> FinancialBookYearId { get; set; }
        public Nullable<long> TerminalNo { get; set; }
        public string ChequeNo { get; set; }
        public string EmpCode { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<bool> Posted { get; set; }
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

        [ForeignKey("BranchId")]
        public virtual BranchDefinition BranchDefinition  { get; set; }
        [ForeignKey("PurchaseId")]
        public virtual PurchaseOrder PurchaseOrder  { get; set; }
        [ForeignKey("SaleId")]
        public virtual SaleOrder SaleOrder   { get; set; }
        [ForeignKey("FinancialBookYearId")]
        public virtual FinancialBookYear FinancialBookYear { get; set; }
    }
}