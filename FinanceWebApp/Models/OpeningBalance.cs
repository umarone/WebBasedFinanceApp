using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class OpeningBalance
    {
        [Key]
        public long OpeningBalanceID { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> FinancialBookYearId { get; set; }
        public Nullable<long> AccountId { get; set; }
        public Nullable<DateTime> OpeningBalanceDate { get; set; }
        public string TransactionMode { get; set; }
        public string EmpCode { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public string Description { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        public Nullable<long> BranchId { get; set; }
        [ForeignKey("OrganizationID")]
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }
        [ForeignKey("BranchId")]
        public virtual BranchDefinition BranchDefinition  { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
        [ForeignKey("FinancialBookYearId")]
        public virtual FinancialBookYear FinancialBookYear { get; set; }
    }
}