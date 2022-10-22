using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using MudasirRehmanAlp.AppDAL;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;
using Newtonsoft.Json;
using PagedList;

namespace MudasirRehmanAlp.SaleSetup.PaymentSetup
{
    public class PaymentMastersController : Controller
    {
        private AppEntities db = new AppEntities();
        PaymentMasterDAL paymentMasterDAL = new PaymentMasterDAL();

        // GET: PaymentMasters       
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<SaleOrder> listobj = new List<SaleOrder>();
            listobj = db.SaleOrders.Where(a => a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId == BranchId).OrderByDescending(a => a.SaleOrdeID).ToList();
            PagedList<SaleOrder> model = new PagedList<SaleOrder>(listobj, page, pageSize);
            return View(model);
        }

        // GET: PaymentMasters/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentMaster paymentMaster = db.PaymentMasters.Find(id);
            if (paymentMaster == null)
            {
                return HttpNotFound();
            }
            return View(paymentMaster);
        }
        // GET: PaymentMasters/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.SaleOrderId = id;
            List<PaymentMaster> _listObj = db.PaymentMasters.Where(a => a.SaleOrderId == id).ToList();

            return View(_listObj);
        }
        public ActionResult JsonCreate(string model)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            InstallmentsPaymentsSchedulerViewModel postModel = JsonConvert.DeserializeObject<InstallmentsPaymentsSchedulerViewModel>(model);
            postModel.OrganizationId = OrganizationID;

            var getmessage = paymentMasterDAL.AddInstallmentsPayments(postModel, UserID);
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Payment has been saved successfully";

            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(string searchModel)
        {
            try
            {
                long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
                long BranchId = Convert.ToInt64(Session["BranchId"]);
                PaymentMasterSearchViewModel _searchObject = JsonConvert.DeserializeObject<PaymentMasterSearchViewModel>(searchModel);
                List<SaleOrder> _listObject = new List<SaleOrder>();
                Expression<Func<SaleOrder, bool>> WhereClouse = null;
                WhereClouse = x => x.SaleOrderNo.ToLower().Contains(_searchObject.SaleOrderNo.ToLower())
                || x.CustomerStatement.Name.ToLower().Contains(_searchObject.CustomerName.ToLower())
                || x.Account.AccountNo.ToLower().Contains(_searchObject.AccountNo.ToLower())
                || x.CustomerStatement.MobileNo.ToLower().Contains(_searchObject.MobileNo.ToLower())
                || x.EmployeePersonalDetails.FirstName.ToLower().Contains(_searchObject.RecoveryOfficerName.ToLower())
                || x.EmployeePersonalDetails.LastName.ToLower().Contains(_searchObject.RecoveryOfficerName.ToLower())
                ;

               
                if (_searchObject.StartDate != null && _searchObject.EndDate != null && _searchObject.SaleOrderNo == "" && _searchObject.CustomerName == "" && _searchObject.AccountNo == "" && _searchObject.MobileNo == "" && _searchObject.RecoveryOfficerName == "")
                {
                    _listObject = db.SaleOrders.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId).Where(a => DbFunctions.TruncateTime(a.CreatedDate) >= DbFunctions.TruncateTime(_searchObject.StartDate) && DbFunctions.TruncateTime(a.CreatedDate) <= DbFunctions.TruncateTime(_searchObject.EndDate)).ToList();
                }
               else if (_searchObject.StartDate != null && _searchObject.EndDate != null)
                {
                    _listObject = db.SaleOrders.Where(WhereClouse).Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId).Where(a => DbFunctions.TruncateTime(a.CreatedDate) >= DbFunctions.TruncateTime(_searchObject.StartDate) && DbFunctions.TruncateTime(a.CreatedDate) <= DbFunctions.TruncateTime(_searchObject.EndDate)).ToList();
                }
                else
                {
                    _listObject = db.SaleOrders.Where(WhereClouse).Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId).ToList();
                }
                return PartialView("_Search", _listObject);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
