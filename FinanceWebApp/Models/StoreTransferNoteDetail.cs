using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class StoreTransferNoteDetail
    {
        [Key]
        public long Id { get; set; }
        public Nullable<long> STNId { get; set; }
        public Nullable<long> ProductId { get; set; }
        public Nullable<long> StockId { get; set; }
        public Nullable<long> Quantity { get; set; }
        public Nullable<long> BalanceQuantity { get; set; }
        [ForeignKey("STNId")]
        public virtual StoreTransferNote StoreTransferNotes  { get; set; }
        [ForeignKey("ProductId")]
        public virtual ProductDefinition ProductDefinition  { get; set; }
        [ForeignKey("StockId")]
        public virtual StockofGoods StockofGoods { get; set; }
    }
}