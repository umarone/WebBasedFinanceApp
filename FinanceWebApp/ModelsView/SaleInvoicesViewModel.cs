using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.ModelsView
{
    public class SaleInvoicesViewModel
    {
        public long SaleInvoiceID { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> SaleOrderID { get; set; }
        public Nullable<long> AccountID { get; set; }
        public string SaleInvoiceNo { get; set; }
        public Nullable<SaleInvoiceTypeEnum> SaleInvoiceType { get; set; }
        public Nullable<DateTime> SaleInvoiceDate { get; set; }
        public string TransactionType { get; set; }
        public Nullable<long> CustomerID { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        public Nullable<decimal> SubTotalWithOutSaleTax { get; set; }
        public Nullable<decimal> SubTotalWithSaleTax { get; set; }
        public Nullable<long> FreightCharges { get; set; }
        public Nullable<decimal> NetTotal { get; set; }
        public string AmountInWord { get; set; }
        public string TermAndCondition { get; set; }

        public string OrganizationName { get; set; }
        public long CustomerId { get; set; }

        public string CustomerCode { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GuardianName { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string CNIC { get; set; }
        public string Address { get; set; }
        public string AccountNo { get; set; }
        public string DistributorName { get; set; }
    }
}