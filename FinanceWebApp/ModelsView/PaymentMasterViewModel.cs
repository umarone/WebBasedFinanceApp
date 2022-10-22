using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.ModelsView
{
    public class PaymentMasterViewModel
    {
        public Nullable<long> OrganizationId { get; set; }
        public Nullable<long> BranchId { get; set; }
        public Nullable<long> SaleOrderId { get; set; }
        public Nullable<long> SaleOrderDetailId { get; set; }
        public Nullable<decimal> RemainingBalanceAmount { get; set; }
        public Nullable<DateTime> SaleOrderDate { get; set; }
        public Nullable<long> ProductId { get; set; }
        public Nullable<decimal> BalanceAmount { get; set; }

        public Nullable<decimal> InstallmentPerMonth { get; set; }
        public Nullable<decimal> NoOfMonths { get; set; }
        
        public Nullable<decimal> DownPayment { get; set; }
        public Nullable<long> userID { get; set; }
    }
    public class PaymentMasterSearchViewModel
    {
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public string SaleOrderNo { get; set; }
        public string CustomerName { get; set; }
        public string AccountNo { get; set; }
        public string MobileNo { get; set; }
        public string RecoveryOfficerName { get; set; }

    }
    
}