using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.ModelsView
{
    public class GoodsReceivedViewModel
    {
        public long GoodReceivedNoteId { get; set; }
        public string GRNNO { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public Nullable<DateTime> GoodsReceivedDate { get; set; }
    }
    public class GoodReceivedPostViewModel
    {
        public long GoodReceivedNoteId { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> PurchaseOrderID { get; set; }
        public Nullable<long> AdjustmentNoteID { get; set; }
        public Nullable<long> StoreTransferNoteId { get; set; }

        public Nullable<DateTime> GRNDate { get; set; }
        public string GRNNO { get; set; }
        public string GRNType { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        public Nullable<long> BranchId { get; set; }

        public Nullable<long> BranchIdForSTN { get; set; }
    }
}