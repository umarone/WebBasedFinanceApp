using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class PurchaseOrderDetail
    {
        [Key]

        public long PurchaseOrderDetailID { get; set; }
        public Nullable<long> PurchaseOrderID { get; set; }
        public Nullable<long> ProductID { get; set; }
        public Nullable<long> Quantity { get; set; }
        public Nullable<decimal> UnitRate { get; set; }
        public Nullable<decimal> TotalWithOutSaleTax { get; set; }
        public Nullable<bool> Active { get; set; }   
        public Nullable<decimal> DiscountPercentage { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> DiscountedUnitRate { get; set; }
        public Nullable<decimal> TotalAfterDiscount { get; set; }
        public Nullable<decimal> SaleTaxPercentage { get; set; }
        public Nullable<decimal> SaleTaxAmount { get; set; }
        public Nullable<decimal> TotalTaxInclusive { get; set; }
        
        public Nullable<long> BalanceQuantity { get; set; }
        public Nullable<long> AgingId { get; set; }
      
        [ForeignKey("PurchaseOrderID")]
        public virtual PurchaseOrder PurchaseOrder  { get; set; }
        [ForeignKey("ProductID")]
        public virtual ProductDefinition ProductDefinition  { get; set; }
        [ForeignKey("AgingId")]
        public virtual PaymentAging PaymentAging  { get; set; }


    }
}