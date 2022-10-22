using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Extensions.Logging;
using MudasirRehmanAlp.AppDAL;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;
using Newtonsoft.Json;
using PagedList;

namespace MudasirRehmanAlp.AppDefinitions
{
    public class SupplierDefinitionsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        
        // GET: SupplierDefinitions
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<SupplierDefinition> listobj = new List<SupplierDefinition>(); 
            listobj = db.SupplierDefinitions.Where(a=>a.IsDeleted==false && a.OrganizationID== OrganizationID).OrderByDescending(a=>a.Id).ToList();
            PagedList<SupplierDefinition> model = new PagedList<SupplierDefinition>(listobj, page, pageSize);
            return View(model);
         
        }

        // GET: SupplierDefinitions/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierDefinition supplierDefinition = db.SupplierDefinitions.Find(id);
            if (supplierDefinition == null)
            {
                return HttpNotFound();
            }
            return View(supplierDefinition);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierDefinition supplierDefinition = db.SupplierDefinitions.Find(id);
            if (supplierDefinition == null)
            {
                return HttpNotFound();
            }
            return View(supplierDefinition);
        }
        // GET: SupplierDefinitions/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            SupplierDefinition obj = new SupplierDefinition();
            obj.SupplierCode = dALCode.AutoGenerateSupplierCode(OrganizationID/*, BranchId*/);
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
           
            return View(obj);
        }

        // POST: SupplierDefinitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SupplierDefinition supplierDefinition, HttpPostedFileBase fileSupplierImage)
        {
            byte[] Imagebytes;
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            if (ModelState.IsValid)
            {
                if (fileSupplierImage != null)
                {
                    using (BinaryReader br = new BinaryReader(fileSupplierImage.InputStream))
                    {
                        Imagebytes = br.ReadBytes(fileSupplierImage.ContentLength);
                        supplierDefinition.SupplierImage = Imagebytes;
                    }
                }
                supplierDefinition.SupplierCode = dALCode.AutoGenerateSupplierCode(OrganizationID/*, BranchId*/);
                supplierDefinition.IsDeleted = false;
                supplierDefinition.CreatedBy = UserID;
                supplierDefinition.CreatedDate = DateTime.Now;
                db.SupplierDefinitions.Add(supplierDefinition);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Supplier has been saved successfully";
                return RedirectToAction("Index");
            }
            return View(supplierDefinition);
        }

        // GET: SupplierDefinitions/Edit/5
        public ActionResult Edit(long? id)
        {
            
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierDefinition supplierDefinition = db.SupplierDefinitions.Find(id);
            if (supplierDefinition == null)
            {
                return HttpNotFound();
            }
            return View(supplierDefinition);
        }

        // POST: SupplierDefinitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SupplierDefinition supplierDefinition, HttpPostedFileBase fileSupplierImage)
        {
            byte[] Imagebytes;
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            if (fileSupplierImage !=null)
            {
                using (BinaryReader br = new BinaryReader(fileSupplierImage.InputStream))
                {
                    Imagebytes = br.ReadBytes(fileSupplierImage.ContentLength);
                    supplierDefinition.SupplierImage = Imagebytes;
                }
            }
           
            if (ModelState.IsValid)
            {
                supplierDefinition.IsDeleted = false;
                supplierDefinition.UpdatedBy = UserID;
                supplierDefinition.UpdatedDate = DateTime.Now;
                db.Entry(supplierDefinition).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Supplier has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(supplierDefinition);
        }

        // GET: SupplierDefinitions/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierDefinition supplierDefinition = db.SupplierDefinitions.Find(id);
            if (supplierDefinition == null)
            {
                return HttpNotFound();
            }
           
            supplierDefinition.DeletedBy = UserID;
            supplierDefinition.DeletedDate = DateTime.Now;
            db.Entry(supplierDefinition).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Supplier has been deleted successfully";
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
        //--------------- Json Function
        public JsonResult LoadArea(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }
            var result = (from p in db.AreaDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)

                          where p.CityId == id
                          select new
                          {
                              value = p.Id,
                              label = p.AreaName
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AccountCheckInSupplier(string ID)
        {
            long id = 0;
            string Message = "";
            if (!String.IsNullOrEmpty(ID))
            {
                id = Convert.ToInt64(ID);
            }

            var Result = db.SupplierDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false && a.AccountID == id).FirstOrDefault();
            if (Result !=null)
            {
                Message = "Yes";
            }
            else
            {
                Message = "No";
            }
            return Json(Message, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult Search(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            SupplierDefinition _searchObject = JsonConvert.DeserializeObject<SupplierDefinition>(searchModel);
            List<SupplierDefinition> _listObject = new List<SupplierDefinition>();
            _listObject = db.SupplierDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId==BranchId).ToList();
            if (!String.IsNullOrEmpty(_searchObject.SupplierCode) && !String.IsNullOrEmpty(_searchObject.SupplierName))
            {
                _listObject = _listObject.Where(r => r.SupplierCode != null && r.SupplierCode.ToString().ToLower().Contains(_searchObject.SupplierCode) || r.SupplierName.ToString().ToLower().Contains(_searchObject.SupplierName)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.SupplierCode))
            {
                _listObject = _listObject.Where(r => r.SupplierCode != null && r.SupplierCode.ToString().ToLower().Contains(_searchObject.SupplierCode)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.SupplierName))
            {
                _listObject = _listObject.Where(r => r.SupplierName != null && r.SupplierName.ToString().ToLower().Contains(_searchObject.SupplierName)).ToList();
            }
            return PartialView("_Search", _listObject);
        }
    }
}
