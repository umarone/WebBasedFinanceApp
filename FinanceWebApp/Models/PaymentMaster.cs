using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class PaymentMaster
    {
        [Key]
        public long Id { get; set; }
        public Nullable<long> OrganizationId { get; set; }
        public Nullable<long> BranchId { get; set; }
        public Nullable<long> SaleOrderId { get; set; }
        public Nullable<long> SaleOrderDetailId { get; set; }
        public Nullable<long> ProductId { get; set; }
        public Nullable<decimal> BalanceAmount { get; set; }
        public Nullable<decimal> RemainingBalanceAmount { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }
        [ForeignKey("BranchId")]
        public virtual BranchDefinition BranchDefinition { get; set; }
        [ForeignKey("SaleOrderId")]
        public virtual SaleOrder SaleOrder  { get; set; }
        [ForeignKey("SaleOrderDetailId")]
        public virtual SaleOrderDetail SaleOrderDetail  { get; set; }
        [ForeignKey("ProductId")]
        public virtual ProductDefinition ProductDefinition { get; set; }

    }
}