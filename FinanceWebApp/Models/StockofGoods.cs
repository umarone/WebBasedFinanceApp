using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class StockofGoods
    {
        [Key]
        public long StockID { get; set; }
        public string StockCode { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> BranchId { get; set; }
        public Nullable<long> ProductID { get; set; }
        public Nullable<long> Quantity { get; set; }
        public Nullable<decimal> LastPurchaseUnitRate { get; set; }
        public Nullable<bool> Active { get; set; }

        public Nullable<DateTime> InDate { get; set; }
        public Nullable<DateTime> OutDate { get; set; }

        [ForeignKey("OrganizationID")]
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }
        [ForeignKey("BranchId")]
        public virtual BranchDefinition BranchDefinition { get; set; }
        [ForeignKey("ProductID")]
        public virtual ProductDefinition ProductDefinition { get; set; }
    }
}