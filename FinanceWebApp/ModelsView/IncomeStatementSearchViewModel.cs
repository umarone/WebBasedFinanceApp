using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.ModelsView
{
    public class IncomeStatementSearchViewModel: BaseSearchViewModel
    {
        public Nullable<long> FinancialBookYearId { get; set; }
    }
}