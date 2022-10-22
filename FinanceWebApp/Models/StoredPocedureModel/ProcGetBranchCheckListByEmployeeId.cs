using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models.StoredPocedureModel
{
    public class ProcGetBranchCheckListByEmployeeId
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}