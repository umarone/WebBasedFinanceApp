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
    public class ColoursController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();

        // GET: Colours
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<Colour> listobj = new List<Colour>();
            listobj = db.Colours.Where(a => a.IsDeleted == false && a.OrganizationId == OrganizationID).OrderByDescending(a=>a.Id).ToList();
            PagedList<Colour> model = new PagedList<Colour>(listobj, page, pageSize);
            return View(model);

        }

        // GET: Colours/Details/5
        public ActionResult Details(long? id)
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

        // GET: Colours/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            Colour obj = new Colour();
            obj.Code = dALCode.AutoColoursCode(OrganizationID/*, BranchId*/);
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationId = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
            return View(obj);
        }

        // POST: Colours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Colour colour)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            long UserID = Convert.ToInt64(Session["UserID"]);

            if (ModelState.IsValid)
            {
                colour.Code = dALCode.AutoColoursCode(OrganizationID/*, BranchId*/);
                colour.IsDeleted = false;
                colour.CreatedBy = UserID;
                colour.CreatedDate = DateTime.Now;
                db.Colours.Add(colour);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Colour has been saved successfully";

                return RedirectToAction("Index");
            }

            return View(colour);
        }
        public ActionResult JsonCreate(string model)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            Colour colour = JsonConvert.DeserializeObject<Colour>(model);
            if (ModelState.IsValid)
            {
                colour.OrganizationId = OrganizationID;
                colour.BranchId = BranchId;
                colour.Code = dALCode.AutoColoursCode(OrganizationID/*, BranchId*/);
                colour.IsActive = true;
                colour.IsDeleted = false;
                colour.CreatedBy = UserID;
                colour.CreatedDate = DateTime.Now;
                db.Colours.Add(colour);
                db.SaveChanges();
                return Json("OK",JsonRequestBehavior.AllowGet);
            }

            return View(colour);
        }

        // GET: Colours/Edit/5
        public ActionResult Edit(long? id)
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

        // POST: Colours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Colour colour)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);

            if (ModelState.IsValid)
            {

                colour.IsDeleted = false;
                colour.UpdatedBy = UserID;
                colour.UpdatedDate = DateTime.Now;
                db.Entry(colour).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Colour has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(colour);
        }

        // GET: Colours/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colour colour = db.Colours.Find(id);
            if (colour == null)
            {
                return HttpNotFound();
            }

            colour.IsDeleted = true;
            colour.DeletedBy = UserID;
            colour.DeletedDate = DateTime.Now;
            db.Entry(colour).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Colour has been deleted successfully";
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
            Colour _searchObject = JsonConvert.DeserializeObject<Colour>(searchModel);
            List<Colour> _listObject = new List<Colour>();
            _listObject = db.Colours.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationId == OrganizationID && a.BranchId == BrancheId).ToList();
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
