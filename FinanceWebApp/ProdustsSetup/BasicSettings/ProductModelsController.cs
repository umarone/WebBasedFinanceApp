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

namespace MudasirRehmanAlp.ProdustsSetup.BasicSettings
{
    public class ProductModelsController : Controller
    {
        private AppEntities db = new AppEntities();

        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();

        // GET: ProductModels
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<ProductModel> listobj = new List<ProductModel>();
            listobj = db.ProductModels.Where(a => a.IsDeleted == false && a.OrganizationId == OrganizationID).OrderByDescending(a=>a.Id).ToList();
            PagedList<ProductModel> model = new PagedList<ProductModel>(listobj, page, pageSize);
            return View(model);

        }

        // GET: ProductModels/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductModel ProductModel = db.ProductModels.Find(id);
            if (ProductModel == null)
            {
                return HttpNotFound();
            }
            return View(ProductModel);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colour colour = db.Colours.Find(id);
            if (colour == null)
            {
                return HttpNotFound();
            }
            return View(colour);
        }
        // GET: ProductModels/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            ProductModel obj = new ProductModel();
            obj.Code = dALCode.AutoProductModelsCode(OrganizationID/*, BranchId*/);
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationId = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
            return View(obj);
        }

        // POST: ProductModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductModel productModel)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            if (ModelState.IsValid)
            {
                productModel.Code = dALCode.AutoProductModelsCode(OrganizationID/*, BranchId*/);
                productModel.IsDeleted = false;
                productModel.CreatedBy = UserID;
                productModel.CreatedDate = DateTime.Now;
                db.ProductModels.Add(productModel);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Product Model has been saved successfully";

                return RedirectToAction("Index");
            }

            return View(productModel);
        }
        public ActionResult JsonCreate(string model)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            ProductModel productModel = JsonConvert.DeserializeObject<ProductModel>(model);

            if (ModelState.IsValid)
            {
                productModel.OrganizationId = OrganizationID;
                productModel.BranchId = BranchId;
                productModel.Code = dALCode.AutoProductModelsCode(OrganizationID/*, BranchId*/);
                productModel.IsActive = true;
                productModel.IsDeleted = false;
                productModel.CreatedBy = UserID;
                productModel.CreatedDate = DateTime.Now;
                db.ProductModels.Add(productModel);
                db.SaveChanges();
                return Json("OK", JsonRequestBehavior.AllowGet);
            }

            return View(productModel);
        }
        // GET: ProductModels/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductModel productModel = db.ProductModels.Find(id);
            if (productModel == null)
            {
                return HttpNotFound();
            }
            return View(productModel);
        }

        // POST: ProductModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductModel productModel)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);

            if (ModelState.IsValid)
            {

                productModel.IsDeleted = false;
                productModel.UpdatedBy = UserID;
                productModel.UpdatedDate = DateTime.Now;
                db.Entry(productModel).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Product Model has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(productModel);
        }

        // GET: ProductModels/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductModel productModel = db.ProductModels.Find(id);
            if (productModel == null)
            {
                return HttpNotFound();
            }

            productModel.IsDeleted = true;
            productModel.DeletedBy = UserID;
            productModel.DeletedDate = DateTime.Now;
            db.Entry(productModel).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Product Model has been deleted successfully";
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
            ProductModel _searchObject = JsonConvert.DeserializeObject<ProductModel>(searchModel);
            List<ProductModel> _listObject = new List<ProductModel>();
            _listObject = db.ProductModels.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationId == OrganizationID && a.BranchId == BrancheId).ToList();
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
