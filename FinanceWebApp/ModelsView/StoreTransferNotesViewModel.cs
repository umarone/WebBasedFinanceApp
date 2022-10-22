using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.ModelsView
{
    public class StoreTransferNotesViewModel
    {
        public long Id { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string Code { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public string Description { get; set; }
        public Nullable<long> FromBranchId { get; set; }
        public Nullable<long> ToBranchId { get; set; }
    }
}