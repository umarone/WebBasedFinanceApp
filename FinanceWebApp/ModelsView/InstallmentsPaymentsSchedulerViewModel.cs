using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.ModelsView
{
    public class InstallmentsPaymentsSchedulerViewModel
    {
        public Guid Id { get; set; }
        public Nullable<long> OrganizationId { get; set; }
        public Nullable<long> PaymentMasterId { get; set; }
        public Nullable<int> SrNo { get; set; }
        public Nullable<DateTime> PaymentDueDate { get; set; }
        public Nullable<decimal> PerMonthAmount { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }
        public Nullable<decimal> ExtraCharges { get; set; }
        public Nullable<DateTime> ReceivedDate { get; set; }
        public Nullable<PaymentStatus> PaymentStatus { get; set; }
        public string Notes { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }

        public bool IsUpdateCheck { get; set; }
    }
}