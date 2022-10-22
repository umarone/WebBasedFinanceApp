using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.Models
{
    public class SaleInvoice
    {
        [Key]
        public long SaleInvoiceID { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> SaleOrderID { get; set; }
        public Nullable<long> AccountID { get; set; }
        public string SaleInvoiceNo { get; set; }
        public Nullable<SaleInvoiceTypeEnum> SaleInvoiceType { get; set; }
        public Nullable<DateTime> SaleInvoiceDate { get; set; }
        public string TransactionType { get; set; }
        public Nullable<long> CustomerID { get; set; }      
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

        public Nullable<long> CID { get; set; }

        [ForeignKey("OrganizationID")]
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }
        [ForeignKey("SaleOrderID")]
        public virtual SaleOrder SaleOrder  { get; set; }
        [ForeignKey("AccountID")]
        public virtual Account Account { get; set; }
        [ForeignKey("CustomerID")]
        public virtual Customer Customer  { get; set; }

    }
}