using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.ModelsView
{
    public class GetSchedulerViewModel
    {
        public  Nullable<DateTime> StartDate { get; set; }
        public Nullable<decimal> NetTotalAmount { get; set; }
        public Nullable<decimal> ProfitPercentage { get; set; }
        public Nullable<decimal> TotalAmountToPaidProfit { get; set; }
        public Nullable<int> NoOfMonths { get; set; }
        public Nullable<decimal> PerMonthAmount { get; set; }
    }
}