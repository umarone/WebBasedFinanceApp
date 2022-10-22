using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.Models
{
    public class TransactionsDetail
    {
        [Key]
        public long TransactionDetailID { get; set; }

        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> BranchId { get; set; }
        public Nullable<long> PurchaseId { get; set; }
        public Nullable<long> SaleId { get; set; }
        public Nullable<long> AccountId { get; set; }
        public Nullable<long> FinancialBookYearId { get; set; }
        public Nullable<int> SeqNo { get; set; }
        public Nullable<PaymentType> PaymentType { get; set; }
        public string Narration { get; set; }
        public string TransactionMode { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<int> TrackNumber { get; set; }
        public Nullable<bool> Posted { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }

        [ForeignKey("OrganizationID")]
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }

        [ForeignKey("BranchId")]
        public virtual BranchDefinition BranchDefinition { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
        [ForeignKey("PurchaseId")]
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        [ForeignKey("SaleId")]
        public virtual SaleOrder SaleOrder { get; set; }
        [ForeignKey("FinancialBookYearId")]
        public virtual FinancialBookYear FinancialBookYear { get; set; }
    }
}