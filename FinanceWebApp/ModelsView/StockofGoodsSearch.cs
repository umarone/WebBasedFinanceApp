using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.ModelsView
{
    public class StockofGoodsSearch
    {
        public string ProductId { get; set; }
        public Nullable<StockType> StockType { get; set; }
    }
}