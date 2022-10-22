using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class PurchaseOrderReturnDetail
    {
        [Key]
        public long PurchaseOrderReturnDetailId { get; set; }
        public Nullable<long> PurchaseOrderReturnID { get; set; }
        public Nullable<long> GRNReturnDetailID { get; set; }
        public Nullable<long> PurchaseOrderDetailID { get; set; }
        public Nullable<long> ProductID { get; set; }
        
        public Nullable<long> Quantity { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<bool> Active { get; set; }

        [ForeignKey("PurchaseOrderReturnID")]
        public virtual PurchaseOrderReturn PurchaseOrderReturn  { get; set; }

        [ForeignKey("GRNReturnDetailID")]
        public virtual GoodReceivedReturnDetail GoodReceivedReturnDetail  { get; set; }

        [ForeignKey("PurchaseOrderDetailID")]
        public virtual PurchaseOrderDetail PurchaseOrderDetail { get; set; }

        [ForeignKey("ProductID")]
        public virtual ProductDefinition ProductDefinition   { get; set; }




    }
}