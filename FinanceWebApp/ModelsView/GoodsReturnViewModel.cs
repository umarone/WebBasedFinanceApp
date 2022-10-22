using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.ModelsView
{
    public class GoodsReturnViewModel
    {
        public long GoodsReceivedReturnID { get; set; }
        public string GRReturnNO { get; set; }
        public long PurchaseOrderId { get; set; }
        public string PurchaseOrderNO { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public Nullable<PuchaseOrderType> PuchaseOrderType { get; set; }
        public Nullable<DateTime> PurchaseOrderDate { get; set; }
        public Nullable<TransactionType> TransactionType { get; set; }
        public string PaymentTerms { get; set; }
        public string SupplierName { get; set; }
        public Nullable<long> SupplierID { get; set; }
        public string AccountNo { get; set; }
        public Nullable<long> AccountID { get; set; }
    }
}