using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class SaleInvoiceDetail
    {
        [Key]
        public long SaleInvoiceDetailID { get; set; }
        public Nullable<long> SaleInvoiceID { get; set; }
        public Nullable<long> SaleOrderDetailsID { get; set; }        
        public Nullable<long> ProductID { get; set; }
        public Nullable<long> StockID { get; set; }
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
        public Nullable<long> CID { get; set; }

        [ForeignKey("SaleInvoiceID")]
        public virtual SaleInvoice SaleInvoice  { get; set; }
        [ForeignKey("SaleOrderDetailsID")]
        public virtual SaleOrderDetail SaleOrderDetail  { get; set; }
        [ForeignKey("ProductID")]
        public virtual ProductDefinition ProductDefinition { get; set; }
        [ForeignKey("StockID")]
        public virtual StockofGoods StockofGoods  { get; set; }
    }
}