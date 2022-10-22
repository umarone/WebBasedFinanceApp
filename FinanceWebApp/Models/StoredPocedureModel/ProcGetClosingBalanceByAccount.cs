using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models.StoredPocedureModel
{
    public class ProcGetClosingBalanceByAccount
    {
        public Nullable<decimal> ClosingBalance { get; set; }
        public string BalanceType { get; set; }
    }
}