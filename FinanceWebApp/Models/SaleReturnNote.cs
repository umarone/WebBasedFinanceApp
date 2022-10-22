using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.Models
{
    public class SaleReturnNote
    {
        [Key]
        public long SaleReturnId { get; set; }
        public string SaleReturnCode { get; set; }
        public Nullable<long> SaleInvoicID { get; set; }
        public Nullable<long> OrganizationID { get; set; }

        public Nullable<long> AccoutID { get; set; }
        public Nullable<DateTime> SaleReturnDate { get; set; }
        public SaleInvoiceTypeEnum SaleReturnType { get; set; }
        public string MainReason { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        public Nullable<long> CID { get; set; }
        [ForeignKey("OrganizationID")]
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }
        [ForeignKey("SaleInvoicID")]
        public virtual SaleInvoice SaleInvoice { get; set; }
        [ForeignKey("AccoutID")]
        public virtual Account Account { get; set; }

    }
}