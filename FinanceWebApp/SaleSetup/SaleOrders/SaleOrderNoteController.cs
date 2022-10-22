using MudasirRehmanAlp.AppDAL;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MudasirRehmanAlp.SaleSetup.SaleOrders
{
    public class SaleOrderNoteController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        SaleOrderDAL saleOrderDAL = new SaleOrderDAL();
        // GET: SaleOrderNote
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<SaleOrder> listobj = new List<SaleOrder>();
            listobj = db.SaleOrders.Where(a => a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId== BranchId).OrderByDescending(a=>a.SaleOrdeID).ToList();
            PagedList<SaleOrder> model = new PagedList<SaleOrder>(listobj, page, pageSize);
            return View(model);
        }
        // GET: SaleOrderNote/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            SaleViewModel obj = new SaleViewModel();
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
           
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
            ViewBag.CustomerStatementCode = dALCode.AutoGenerateCustomerStatementCode(OrganizationID, BranchId);
            ViewBag.SaleOrderNo = dALCode.AutoGenerateSaleOrderCode(OrganizationID, BranchId);
            var findGeneralSettings = db.GeneralSettings.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId == BranchId).FirstOrDefault();

            if (findGeneralSettings != null)
            {
                ViewBag.GuarantorMaxNeed = findGeneralSettings.GuarantorMaxNeed;
            }
            else
            {
                TempData["Validation"] = "warning";
                TempData["ErrorMessage"] = "Please add Guarantor Max Need befor you add Customer Statements.";
            }
            return View(obj);
        }
    }
}