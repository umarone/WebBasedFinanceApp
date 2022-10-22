using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MudasirRehmanAlp.AppDAL;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using Newtonsoft.Json;
using PagedList;

namespace MudasirRehmanAlp.PurchaseSetup.BasicSettings
{
    public class PaymentAgingsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();

        // GET: PaymentAgings
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<PaymentAging> listobj = new List<PaymentAging>();
            listobj = db.PaymentAgings.Where(a => a.IsDeleted == false && a.OrganizationId == OrganizationID).OrderByDescending(a=>a.Id).ToList();
            PagedList<PaymentAging> model = new PagedList<PaymentAging>(listobj, page, pageSize);
            return View(model);

        }

        // GET: PaymentAgings/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentAging paymentAging = db.PaymentAgings.Find(id);
            if (paymentAging == null)
            {
                return HttpNotFound();
            }
            return View(paymentAging);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentAging paymentAging = db.PaymentAgings.Find(id);
            if (paymentAging == null)
            {
                return HttpNotFound();
            }
            return View(paymentAging);
        }

        // GET: PaymentAgings/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            PaymentAging obj = new PaymentAging();
            obj.Code = dALCode.AutoPaymentAgingsCode(OrganizationID/*, BranchId*/);
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationId = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
            return View(obj);
        }

        // POST: PaymentAgings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PaymentAging paymentAging)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            if (ModelState.IsValid)
            {
                paymentAging.Code = dALCode.AutoPaymentAgingsCode(OrganizationID/*, BranchId*/);
                paymentAging.IsDeleted = false;
                paymentAging.CreatedBy = UserID;
                paymentAging.CreatedDate = DateTime.Now;
                db.PaymentAgings.Add(paymentAging);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Payment Aging has been saved successfully";

                return RedirectToAction("Index");
            }

            return View(paymentAging);
        }
        public ActionResult JsonCreate(string model)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            PaymentAging paymentAging = JsonConvert.DeserializeObject<PaymentAging>(model);
            if (ModelState.IsValid)
            {
                paymentAging.OrganizationId = OrganizationID;
                paymentAging.BranchId = BranchId;
                paymentAging.Code = dALCode.AutoPaymentAgingsCode(OrganizationID/*, BranchId*/);
                paymentAging.IsActive = true;
                paymentAging.IsDeleted = false;
                paymentAging.CreatedBy = UserID;
                paymentAging.CreatedDate = DateTime.Now;
                db.PaymentAgings.Add(paymentAging);
                db.SaveChanges();
                return Json("OK", JsonRequestBehavior.AllowGet);
            }

            return View(paymentAging);
        }

        // GET: PaymentAgings/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentAging paymentAging = db.PaymentAgings.Find(id);
            if (paymentAging == null)
            {
                return HttpNotFound();
            }
            return View(paymentAging);
        }

        // POST: PaymentAgings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PaymentAging paymentAging)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);

            if (ModelState.IsValid)
            {

                paymentAging.IsDeleted = false;
                paymentAging.UpdatedBy = UserID;
                paymentAging.UpdatedDate = DateTime.Now;
                db.Entry(paymentAging).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Payment Aging has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(paymentAging);
        }

        // GET: PaymentAgings/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentAging paymentAging = db.PaymentAgings.Find(id);
            if (paymentAging == null)
            {
                return HttpNotFound();
            }

            paymentAging.IsDeleted = true;
            paymentAging.DeletedBy = UserID;
            paymentAging.DeletedDate = DateTime.Now;
            db.Entry(paymentAging).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Payment Aging has been deleted successfully";
            return RedirectToAction("Index");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //------- Json Functions
        public ActionResult Search(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BrancheId = Convert.ToInt64(Session["BranchId"]);
            PaymentAging _searchObject = JsonConvert.DeserializeObject<PaymentAging>(searchModel);
            List<PaymentAging> _listObject = new List<PaymentAging>();
            _listObject = db.PaymentAgings.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationId == OrganizationID && a.BranchId == BrancheId).ToList();
            if (!String.IsNullOrEmpty(_searchObject.Code) && !String.IsNullOrEmpty(_searchObject.Name))
            {
                _listObject = _listObject.Where(r => r.Code != null && r.Code.ToString().ToLower().Contains(_searchObject.Code) || r.Name.ToString().ToLower().Contains(_searchObject.Name)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.Code))
            {
                _listObject = _listObject.Where(r => r.Code != null && r.Code.ToString().ToLower().Contains(_searchObject.Code)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.Name))
            {

                _listObject = _listObject.Where(r => r.Name != null && r.Name.ToString().ToLower().Contains(_searchObject.Name)).ToList();

            }
            return PartialView("_Search", _listObject);
        }
    }
}
