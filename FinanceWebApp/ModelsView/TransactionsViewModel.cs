using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.ModelsView
{
    public class TransactionsViewModel
    {
        public long TransactionDetailID { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> BranchId { get; set; }
        public Nullable<long> PurchaseId { get; set; }
        public Nullable<long> SaleId { get; set; }
        public Nullable<long> AccountId { get; set; }
        public Nullable<long> FinancialBookYearId { get; set; }
        public Nullable<int> SeqNo { get; set; }
        public Nullable<PaymentType> PaymentType { get; set; }
        public string Narration { get; set; }
        public string TransactionMode { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<int> TrackNumber { get; set; }
        public Nullable<bool> Posted { get; set; }
        public Nullable<bool> IsDeleted { get; set; }

        public Nullable<long> DefaultSaleAccountId { get; set; }
        public Nullable<decimal> NetTotal { get; set; }
    }
}