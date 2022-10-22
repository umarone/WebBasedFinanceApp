using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class SaleOrderDetail
    {
        [Key]
        public long SaleOrderDetailID { get; set; }
        public Nullable<long> SaleOrderID { get; set; }
        public Nullable<long> ProductID { get; set; }
        public Nullable<long> StockId { get; set; }
        public Nullable<long> Quantity { get; set; }
        public Nullable<decimal> UnitRate { get; set; }
        public Nullable<long> BalanceQuantity { get; set; }

        public Nullable<decimal> RemainingBalanceAmount { get; set; }
        public Nullable<decimal> MarkupPercentage { get; set; }

        public Nullable<decimal> Total { get; set; }
        public Nullable<decimal> MarkupAmount { get; set; }
        public Nullable<decimal> ProcessingFee { get; set; }
        public Nullable<decimal> DownPayment { get; set; }
        public Nullable<decimal> NoOfMonths { get; set; }
        public Nullable<decimal> InstallmentPerMonth { get; set; }
        public Nullable<bool> Active { get; set; }
        public string Formula { get; set; }

        [ForeignKey("SaleOrderID")]
        public virtual SaleOrder SaleOrder { get; set; }
        [ForeignKey("ProductID")]
        public virtual ProductDefinition ProductDefinition { get; set; }
        [ForeignKey("StockId")]
        public virtual StockofGoods StockofGoods { get; set; }
    }
}