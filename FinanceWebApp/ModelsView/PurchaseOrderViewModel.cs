using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.ModelsView
{
    public class PurchaseOrderViewModel
    {
        public long PurchaseOrderId { get; set; }
        public string PurchaseOrderNO { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public string OrganizationName { get; set; }    
        public Nullable<DateTime> PurchaseOrderDate { get; set; }
     

    }
}