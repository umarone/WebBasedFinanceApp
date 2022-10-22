using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models.StoredPocedureModel
{
    public class ProcBranchIncomeStatement
    {
        public string AccountName { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public int SerialNo { get; set; }
    }
}