using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.ModelsView
{
    public class CustomerLedgerReportViewModel
    {
        public string ProductsName { get; set; }
        public string UnitRate { get; set; }
        public string DownPayment { get; set; }
        public string Receivable { get; set; }
        public string NoOfMonths { get; set; }
        public string InstallmentPerMonth { get; set; }
    }
}