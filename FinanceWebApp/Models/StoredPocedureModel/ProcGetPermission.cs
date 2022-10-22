using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models.StoredPocedureModel
{
    public class ProcGetPermission
    {
        public bool OPCreate { get; set; }
        public bool OPUpdate { get; set; }
        public bool OPView { get; set; }
        public bool OPDelete { get; set; }
        public bool OPPrint { get; set; }
        public bool OPApproval { get; set; }
        public string UserName { get; set; }
        public string MenuName { get; set; }
        public long UserID { get; set; }
    }
}