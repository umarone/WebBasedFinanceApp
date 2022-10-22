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
using MudasirRehmanAlp.Helpers;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;
using Newtonsoft.Json;
using PagedList;

namespace MudasirRehmanAlp.AppDefinitions
{
    public class ProductDefinitionsController : Controller
    {
        private AppEntities db = new AppEntities();

        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        
        // GET: ProductDefinitions
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<ProductDefinition> listobj = new List<ProductDefinition>();
            listobj = db.ProductDefinitions.Where(a=>a.IsDeleted==false && a.OrganizationID==OrganizationID).OrderByDescending(a=>a.ProductId).ToList();
            PagedList<ProductDefinition> model = new PagedList<ProductDefinition>(listobj, page, pageSize);
            return View(model);

        }

        // GET: ProductDefinitions/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDefinition productDefinition = db.ProductDefinitions.Find(id);
            if (productDefinition == null)
            {
                return HttpNotFound();
            }
            return View(productDefinition);
        }
        public ActionResult GenerateCode(long? id)
        {
            HtmlHelperCodeGenerate qRCodeHtmlHelper = new HtmlHelperCodeGenerate();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDefinition productDefinition = db.ProductDefinitions.Find(id);
            if (!String.IsNullOrEmpty(productDefinition.ProductBarCode))
            {
                ViewBag.QRCode = qRCodeHtmlHelper.GenerateQRCode(productDefinition.ProductBarCode);
                ViewBag.BarCode = qRCodeHtmlHelper.GenerateBarCode(productDefinition.ProductBarCode);
            }
          
            if (productDefinition == null)
            {
                return HttpNotFound();
            }
            return View(productDefinition);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDefinition productDefinition = db.ProductDefinitions.Find(id);
            if (productDefinition == null)
            {
                return HttpNotFound();
            }
            return View(productDefinition);
        }
        // GET: ProductDefinitions/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            ProductDefinition obj = new ProductDefinition();
           
            obj.ProductCode = dALCode.AutoGenerateProductCode(OrganizationID/*, BranchId*/);

            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
          
            return View(obj);
        }

        // POST: ProductDefinitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductDefinition productDefinition, HttpPostedFileBase fileProductImage)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            byte[] Imagebytes;
            if (ModelState.IsValid)
            {
                if (fileProductImage != null)
                {
                    using (BinaryReader br = new BinaryReader(fileProductImage.InputStream))
                    {
                        Imagebytes = br.ReadBytes(fileProductImage.ContentLength);
                        productDefinition.ProductImage = Imagebytes;
                    }

                }
                productDefinition.ProductCode= dALCode.AutoGenerateProductCode(OrganizationID/*, BranchId*/);
                productDefinition.IsDeleted = false;
                productDefinition.CreatedBy = UserID;
                productDefinition.CreatedDate = DateTime.Now;

                db.ProductDefinitions.Add(productDefinition);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Product has been saved successfully";
                return RedirectToAction("Index");
            }

            return View(productDefinition);
        }

        // GET: ProductDefinitions/Edit/5
        public ActionResult Edit(long? id)
        {

            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDefinition productDefinition = db.ProductDefinitions.Find(id);
            if (productDefinition == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationUnitName = productDefinition.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = productDefinition.BranchDefinition.Name;
            return View(productDefinition);
        }

        // POST: ProductDefinitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductDefinition productDefinition, HttpPostedFileBase fileProductImage)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            byte[] Imagebytes;
            if (ModelState.IsValid)
            {
                if (fileProductImage != null)
                {
                    using (BinaryReader br = new BinaryReader(fileProductImage.InputStream))
                    {
                        Imagebytes = br.ReadBytes(fileProductImage.ContentLength);
                        productDefinition.ProductImage = Imagebytes;
                    }
                }
             
                productDefinition.IsDeleted = false;
                productDefinition.UpdatedBy = UserID;
                productDefinition.UpdatedDate = DateTime.Now;
                db.Entry(productDefinition).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Product has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(productDefinition);
        }

        // GET: ProductDefinitions/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDefinition productDefinition = db.ProductDefinitions.Find(id);
            if (productDefinition == null)
            {
                return HttpNotFound();
            }
            
            productDefinition.IsDeleted = true;
            productDefinition.DeletedBy = UserID;
            productDefinition.DeletedDate = DateTime.Now;
            db.Entry(productDefinition).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Product has been deleted successfully";
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
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            ProductDefinition _searchObject = JsonConvert.DeserializeObject<ProductDefinition>(searchModel);
            List<ProductDefinition> _listObject = new List<ProductDefinition>();
            _listObject = db.ProductDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId== BranchId).ToList();
            if (!String.IsNullOrEmpty(_searchObject.ProductCode) && !String.IsNullOrEmpty(_searchObject.ProductName))
            {
                _listObject = _listObject.Where(r => r.ProductCode != null && r.ProductCode.ToString().ToLower().Contains(_searchObject.ProductCode) || r.ProductName.ToString().ToLower().Contains(_searchObject.ProductName)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.ProductCode))
            {
                _listObject = _listObject.Where(r => r.ProductCode != null && r.ProductCode.ToString().ToLower().Contains(_searchObject.ProductCode)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.ProductName))
            {

                _listObject = _listObject.Where(r => r.ProductName != null && r.ProductName.ToString().ToLower().Contains(_searchObject.ProductName)).ToList();

            }
            return PartialView("_Search", _listObject);
        }
    }
}
