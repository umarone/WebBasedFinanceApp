using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.ModelsView
{
    public class CommonDashboardJsonViewModel
    {
        public long SaleOrderTotalCount { get; set; }
        public long PurchaseOrderTotalCount { get; set; }
        public long CustomerTotalCount { get; set; }
        public decimal SumOfIncome { get; set; }
        public decimal SumOfExpense { get; set; }
        public long SaleVoucherTotalCount { get; set; }
        public long PurchaseVoucherTotalCount { get; set; }
    }
}