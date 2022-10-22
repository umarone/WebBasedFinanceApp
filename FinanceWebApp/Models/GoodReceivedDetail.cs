using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class GoodReceivedDetail
    {
        [Key]

        public long GoodReceivedNoteDetailID { get; set; }
        public Nullable<long> GoodReceivedNoteID { get; set; }
        public Nullable<long> PurchaseOrderDetailID { get; set; }
        public Nullable<long> AdjustmentNoteDetailID { get; set; }
        public Nullable<long> STNDetailsId { get; set; }
        public string GRNType { get; set; }
        public Nullable<long> ProductID { get; set; }
        public Nullable<long> StockID { get; set; }
        public Nullable<long> OrderQuantity { get; set; }
        public Nullable<long> ReceiveQuantity { get; set; }
        public Nullable<long> BalanceQuantity { get; set; }
        public Nullable<long> AlreadyReceivedQuantity { get; set; }
        public Nullable<long> FirstReceiveQuantity { get; set; }      
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> AvgUnitPrice { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<long> CID { get; set; }

        [ForeignKey("GoodReceivedNoteID")]
        public virtual GoodReceived GoodReceived  { get; set; }
        [ForeignKey("PurchaseOrderDetailID")]
        public virtual PurchaseOrderDetail PurchaseOrderDetail  { get; set; }
        [ForeignKey("AdjustmentNoteDetailID")]
        public virtual AdjustmentNoteDetail AdjustmentNoteDetail  { get; set; }
        [ForeignKey("STNDetailsId")]
        public virtual StoreTransferNoteDetail StoreTransferNoteDetail  { get; set; }

        [ForeignKey("ProductID")]
        public virtual ProductDefinition ProductDefinition   { get; set; }
        [ForeignKey("StockID")]
        public virtual StockofGoods StockofGoods  { get; set; }


    }
}