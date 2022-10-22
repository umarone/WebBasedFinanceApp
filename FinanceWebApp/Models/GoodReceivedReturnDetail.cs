using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class GoodReceivedReturnDetail
    {
        [Key]

        public long GoodsReceivedReturnDetailID { get; set; }
        public Nullable<long> GoodsReceivedReturnID { get; set; }
        public Nullable<long> GoodsReceivedDetailID { get; set; }
        public Nullable<long> ProductID { get; set; }
        public Nullable<long> StockID { get; set; }
        public Nullable<long> ReturnQuantity { get; set; }
        public Nullable<long> ReceiveQuantity { get; set; }
        public Nullable<long> AlreadyReturnQuantity { get; set; }
        
        public string Reason { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> AvgUnitPrice { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<long> CID { get; set; }


        [ForeignKey("GoodsReceivedReturnID")]
        public virtual GoodReceivedReturn GoodReceivedReturn  { get; set; }
        [ForeignKey("GoodsReceivedDetailID")]
        public virtual GoodReceivedDetail GoodReceivedDetail  { get; set; }
        [ForeignKey("ProductID")]
        public virtual ProductDefinition ProductDefinition  { get; set; }
        [ForeignKey("StockID")]
        public virtual StockofGoods StockofGoods  { get; set; }



    }
}