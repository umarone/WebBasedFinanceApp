using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.ModelsView
{
    public class SaleOrderViewModel
    {
        public long SaleOrdeID { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> DistributorID { get; set; }
        public Nullable<long> CustomerStatementID { get; set; }
        public string CustomerStatementName { get; set; }
        public string DistributorName { get; set; }
        public string SaleOrderNo { get; set; }
        public string SaleOrderType { get; set; }
        public Nullable<SaleOrderCustomerType> SaleOrderCustomerType { get; set; }
        public Nullable<DateTime> SaleOrderDate { get; set; }
        public Nullable<TransactionType> TransactionType { get; set; }
        public string PaymentTerms { get; set; }

        public Nullable<bool> ApprovalStatus { get; set; }
        public Nullable<long> ApprovalBy { get; set; }
        public Nullable<long> CID { get; set; }
        public Nullable<long> CurrencyID { get; set; }

        public Nullable<decimal> SubTotalWithOutSaleTax { get; set; }
        public Nullable<decimal> SubTotalWithSaleTax { get; set; }
        public Nullable<decimal> FreightCharges { get; set; }
        public Nullable<decimal> NetTotal { get; set; }
        public string AmountInWord { get; set; }
        public string TermAndCondition { get; set; }
        public string AccountNo { get; set; }
        public Nullable<long> AccountID { get; set; }
    }
}