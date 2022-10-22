using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.ModelsView
{
    public class InstallmentsSchedulerViewModel
    {
        public int SrNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public string MonthName { get; set; }
        public decimal PerMonthInstallment { get; set; }
        public decimal InstallmentPaid { get; set; }
        public decimal ExtraCharges { get; set; }
        public Nullable<DateTime> DateReceived { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string Notes { get; set; }
        public int CurrentMonthStatus { get; set; }

    }
}