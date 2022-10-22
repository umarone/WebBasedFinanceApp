using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.ReportsSetup.StockReports
{
    public class BranchStocksController : Controller
    {
        private AppEntities db = new AppEntities();
        // GET: BranchStocks
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<StockofGoods> listobj = new List<StockofGoods>();
            listobj = db.StockofGoods.Where(a => a.Active == true && a.OrganizationID== OrganizationID && a.BranchId== BranchId).OrderByDescending(a => a.StockID).ToList();
            PagedList<StockofGoods> model = new PagedList<StockofGoods>(listobj, page, pageSize);
            return View(model);
        }
        //------- Josn Functions
        public ActionResult Search(string searchModel)
        {

            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            StockofGoodsSearch _searchObject = JsonConvert.DeserializeObject<StockofGoodsSearch>(searchModel);
            List<StockofGoods> _listObject = new List<StockofGoods>();
            Expression<Func<StockofGoods, bool>> WhereClouse = null;
            WhereClouse = x => x.ProductID.ToString().Contains(_searchObject.ProductId.ToString())
            ;
            if (_searchObject.ProductId != "" && _searchObject.StockType == StockType.InStock)
            {
                _listObject = db.StockofGoods.Where(WhereClouse).Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && a.Quantity > 0).ToList();
            }
            else if (_searchObject.ProductId != "" && _searchObject.StockType == StockType.OutStock)
            {
                _listObject = db.StockofGoods.Where(WhereClouse).Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && a.Quantity == 0).ToList();
            }
            else
            {
                _listObject = db.StockofGoods.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && a.Quantity > 0).ToList();
            }


            return PartialView("_Search", _listObject);
        }
    }
}