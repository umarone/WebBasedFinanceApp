using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class SaleReturnNoteDetail
    {
        [Key]
        public long SaleReturnDetailsId { get; set; }
        public Nullable<long> StockID { get; set; }
        public Nullable<long> SaleReturnID { get; set; }
        public Nullable<long> SaleInvoiceDetailsId { get; set; }

        public Nullable<long> ProductID { get; set; }
        public Nullable<long> ReturnQunatity { get; set; }

        public Nullable<long> Quantity { get; set; }
       
        public String Reason { get; set; }
        
        public Nullable<bool> Active { get; set; }
        [ForeignKey("SaleReturnID")]
        public virtual SaleReturnNote SaleReturnNote { get; set; }
        [ForeignKey("ProductID")]
        public virtual ProductDefinition ProductDefinition { get; set; }
        [ForeignKey("SaleInvoiceDetailsId")]
        public virtual SaleInvoiceDetail SaleInvoiceDetail { get; set; }
        [ForeignKey("StockID")]
        public virtual StockofGoods StockofGoods { get; set; }

    }
}