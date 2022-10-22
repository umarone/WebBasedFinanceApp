using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.Models
{
    public class SaleOrder
    {
        [Key]
        public long SaleOrdeID { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> DistributorID { get; set; }
        public Nullable<long> CustomerStatementID { get; set; }
        public Nullable<long> BranchId { get; set; }
        public Nullable<long> AccountID { get; set; }
        public string SaleOrderNo { get; set; }
        
        public Nullable<SaleOrderCustomerType> SaleOrderCustomerType { get; set; }
        public Nullable<DateTime> SaleOrderDate { get; set; }
        public Nullable<TransactionType > TransactionType { get; set; }
        public string PaymentTerms { get; set; }

        public Nullable<bool> ApprovalStatus { get; set; }
        public Nullable<long> ApprovalBy { get; set; }
      
     
        public Nullable<long> EmployeeId { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        public Nullable<decimal> FreightCharges { get; set; }
        public Nullable<decimal> NetTotal { get; set; }
        public string AmountInWord { get; set; }
        public string TermAndCondition { get; set; }

        public byte[] SaleOrderImage { get; set; }
        public string FilePath { get; set; }

        [ForeignKey("OrganizationID")]
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }
        [ForeignKey("DistributorID")]
        public virtual DistributorDefinition DistributorDefinition { get; set; }
      
        [ForeignKey("AccountID")]
        public virtual Account Account { get; set; }
        [ForeignKey("CustomerStatementID")]
        public virtual CustomerStatement CustomerStatement { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual EmployeePersonalDetails EmployeePersonalDetails { get; set; }
        [ForeignKey("BranchId")]
        public virtual BranchDefinition BranchDefinition { get; set; }
    }
}