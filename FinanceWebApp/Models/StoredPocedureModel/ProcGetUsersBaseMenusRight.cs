using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models.StoredPocedureModel
{
    public class ProcGetUsersBaseMenusRight
    {
        public Nullable<long> MenuID { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsMenu { get; set; }
        public Nullable<bool> OPCreate { get; set; }
        public Nullable<bool> OPUpdate { get; set; }
        public Nullable<bool> OPView { get; set; }
        public Nullable<bool> OPDelete { get; set; }
        public Nullable<bool> OPPrint { get; set; }
        public Nullable<bool> OPApproval { get; set; }
    }
}
