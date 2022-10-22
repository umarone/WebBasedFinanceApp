using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.ModelsView
{
    public class EmployeeViewModel
    {
        public long EmployeeID { get; set; }
        public string FullName { get; set; }
        public Nullable<long> OrganiztionID { get; set; }
        public string OrganiztionName { get; set; }

        public long UserID { get; set; }
       
    }
}