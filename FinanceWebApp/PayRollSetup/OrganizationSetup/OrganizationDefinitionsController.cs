using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MudasirRehmanAlp.AppDAL;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;
using PagedList;

namespace MudasirRehmanAlp.PayRollSetup.OrganizationSetup
{
    public class OrganizationDefinitionsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        // GET: OrganizationDefinitions
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            List<OrganizationDefinition> listobj = new List<OrganizationDefinition>();
            listobj = db.OrganizationDefinitions.Where(a => a.IsDeleted == false && a.Id == OrganizationID).ToList();
            PagedList<OrganizationDefinition> model = new PagedList<OrganizationDefinition>(listobj, page, pageSize);
            return View(model);

        }

        // GET: OrganizationDefinitions/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrganizationDefinition organizationDefinition = db.OrganizationDefinitions.Find(id);
            if (organizationDefinition == null)
            {
                return HttpNotFound();
            }
            return View(organizationDefinition);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrganizationDefinition organizationDefinition = db.OrganizationDefinitions.Find(id);
            if (organizationDefinition == null)
            {
                return HttpNotFound();
            }
            return View(organizationDefinition);
        }
        // GET: OrganizationDefinitions/Create
        public ActionResult Create()
        {
            OrganizationDefinition obj = new OrganizationDefinition();
            obj.OrganizationUnitCode = dALCode.AutoGenerateOrganizationCode();
            ViewBag.CountryList = db.CountryDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.CountryName).ToList();
            return View(obj);
        }

        // POST: OrganizationDefinitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrganizationDefinition organizationDefinition, HttpPostedFileBase fileOrganizationLogo, HttpPostedFileBase fileOrganizationLogoSingle)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            byte[] Logobytes;
            byte[] LogoSinglebytes;

            if (ModelState.IsValid)
            {
                if (fileOrganizationLogo != null)
                {
                    using (BinaryReader br = new BinaryReader(fileOrganizationLogo.InputStream))
                    {
                        Logobytes = br.ReadBytes(fileOrganizationLogo.ContentLength);
                        //findObj.OrganizationLogo = Logobytes;
                        Stream stream = new MemoryStream(Logobytes);
                        organizationDefinition.OrganizationLogo = CommonFunctions.ResizeImage(stream, 213, 38);
                    }
                }

                if (fileOrganizationLogoSingle != null)
                {
                    using (BinaryReader br = new BinaryReader(fileOrganizationLogoSingle.InputStream))
                    {
                        LogoSinglebytes = br.ReadBytes(fileOrganizationLogoSingle.ContentLength);
                        Stream stream = new MemoryStream(LogoSinglebytes);
                        //findObj.OrganizationLogoSingle = LogoSinglebytes;
                        organizationDefinition.OrganizationLogoSingle = CommonFunctions.ResizeImage(stream, 50, 50);
                    }
                }
                organizationDefinition.IsDeleted = false;
                organizationDefinition.CreatedBy = UserID;
                organizationDefinition.CreatedDate = DateTime.Now;
                db.OrganizationDefinitions.Add(organizationDefinition);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Organization has been saved successfully";

                return RedirectToAction("Index");
            }

            return View(organizationDefinition);
        }

        // GET: OrganizationDefinitions/Edit/5
        public ActionResult Edit(long? id)
        {
            ViewBag.CountryList = db.CountryDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.CountryName).ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrganizationDefinition organizationDefinition = db.OrganizationDefinitions.Find(id);
            if (organizationDefinition == null)
            {
                return HttpNotFound();
            }
            return View(organizationDefinition);
        }

        // POST: OrganizationDefinitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrganizationDefinition organizationDefinition, HttpPostedFileBase fileOrganizationLogo, HttpPostedFileBase fileOrganizationLogoSingle)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            byte[] Logobytes;
            byte[] LogoSinglebytes;
            if (ModelState.IsValid)
            {

                var findObj = db.OrganizationDefinitions.Find(organizationDefinition.Id);

                findObj.OrganizationUnitName = organizationDefinition.OrganizationUnitName;
                findObj.OrganizationUnitCode = organizationDefinition.OrganizationUnitCode;
                findObj.Website = organizationDefinition.Website;

                findObj.ContactNo = organizationDefinition.ContactNo;
                findObj.MobileNo = organizationDefinition.MobileNo;
                findObj.FaxNo = organizationDefinition.FaxNo;
                findObj.Email = organizationDefinition.Address1;
                findObj.Address1 = organizationDefinition.Address1;
                findObj.Address2 = organizationDefinition.Address2;
                findObj.CountryId = organizationDefinition.CountryId;
                findObj.ProvinceId = organizationDefinition.ProvinceId;
                findObj.RegionId = organizationDefinition.RegionId;
                findObj.CityId = organizationDefinition.CityId;
                findObj.AreaId = organizationDefinition.AreaId;
                findObj.OrganizationPincode = organizationDefinition.OrganizationPincode;
                findObj.NTN = organizationDefinition.NTN;
                findObj.GSTNo = organizationDefinition.GSTNo;
                findObj.OrganizationTitle = organizationDefinition.OrganizationTitle;
                if (fileOrganizationLogo != null)
                {
                    using (BinaryReader br = new BinaryReader(fileOrganizationLogo.InputStream))
                    {
                        Logobytes = br.ReadBytes(fileOrganizationLogo.ContentLength);
                        //findObj.OrganizationLogo = Logobytes;
                        Stream stream = new MemoryStream(Logobytes);
                        findObj.OrganizationLogo = CommonFunctions.ResizeImage(stream, 213, 38);
                    }
                }

                if (fileOrganizationLogoSingle != null)
                {
                    using (BinaryReader br = new BinaryReader(fileOrganizationLogoSingle.InputStream))
                    {
                        LogoSinglebytes = br.ReadBytes(fileOrganizationLogoSingle.ContentLength);
                        Stream stream = new MemoryStream(LogoSinglebytes);
                        //findObj.OrganizationLogoSingle = LogoSinglebytes;
                        findObj.OrganizationLogoSingle = CommonFunctions.ResizeImage(stream, 50, 50);
                    }
                }


                findObj.IsDeleted = false;
                findObj.UpdatedBy = UserID;
                findObj.UpdatedDate = DateTime.Now;
                db.Entry(findObj).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Organization has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(organizationDefinition);
        }

        // GET: OrganizationDefinitions/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrganizationDefinition organizationDefinition = db.OrganizationDefinitions.Find(id);
            if (organizationDefinition == null)
            {
                return HttpNotFound();
            }
            organizationDefinition.IsDeleted = true;
            organizationDefinition.DeletedBy = UserID;
            organizationDefinition.DeletedDate = DateTime.Now;
            db.Entry(organizationDefinition).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Organization has been deleted successfully";
            return RedirectToAction("Index");
        }

        // GET: OrganizationDefinitions/CreateAccountList/5
        public ActionResult CreateAccountList(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrganizationDefinition organizationDefinition = db.OrganizationDefinitions.Find(id);
            if (organizationDefinition == null)
            {
                return HttpNotFound();
            }

            var objList = db.AccountSystemList.ToList();
            foreach (var item in objList)
            {
                Account obj = new Account();
                obj.OrganizationID = organizationDefinition.Id;
                obj.ParentID = item.ParentID;
                obj.AccountNo = item.AccountNo;
                obj.AccountName = item.AccountName;
                obj.AccountType = item.AccountType;
                obj.LevelID = item.LevelID;
                obj.HeadID = item.HeadID;
                obj.Description = item.Description;
                obj.IsActive = obj.IsActive;
               
                obj.IsDeleted = false;
                obj.IsActive = true;
                obj.IsSystemAccount = false;
                obj.CreatedBy = UserID;
                obj.CreatedDate = DateTime.Now;
                db.Accounts.Add(obj);
                db.SaveChanges();
            }
            organizationDefinition.IsChartOfAccountCreated = true;
            db.Entry(organizationDefinition).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Organization Accounts List has been created successfully";
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
        public JsonResult LoadProvince(string ID)
        {
            long countryid = 0;
            if (ID != "")
            {
                countryid = Convert.ToInt64(ID);
            }
            var result = (from p in db.ProvinceDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)

                          where p.CountryId == countryid
                          select new
                          {
                              value = p.Id,
                              label = p.ProvinceName
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadRegion(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }
            var result = (from p in db.RegionDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)

                          where p.ProvinceId == id
                          select new
                          {
                              value = p.Id,
                              label = p.RegionName
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadCity(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }
            var result = (from p in db.CityDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)

                          where p.RegionId == id
                          select new
                          {
                              value = p.CityId,
                              label = p.CityName
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
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
        //------------ This is Code For Super Admin
        // GET: OrganizationDefinitions For Super Admin
        public ActionResult IndexSuperAdmin(int page = 1, int pageSize = 15)
        {
            List<OrganizationDefinition> listobj = new List<OrganizationDefinition>();
            listobj = db.OrganizationDefinitions.Where(a => a.IsDeleted == false).ToList();
            PagedList<OrganizationDefinition> model = new PagedList<OrganizationDefinition>(listobj, page, pageSize);
            return View(model);
        }
        // GET: OrganizationDefinitions/Details/5
        public ActionResult DetailsSuperAdmin(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrganizationDefinition organizationDefinition = db.OrganizationDefinitions.Find(id);
            if (organizationDefinition == null)
            {
                return HttpNotFound();
            }
            return View(organizationDefinition);
        }
        public ActionResult PrintSuperAdmin(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrganizationDefinition organizationDefinition = db.OrganizationDefinitions.Find(id);
            if (organizationDefinition == null)
            {
                return HttpNotFound();
            }
            return View(organizationDefinition);
        }
        // GET: OrganizationDefinitions/Create
        public ActionResult CreateSuperAdmin()
        {
            OrganizationDefinition obj = new OrganizationDefinition();
            obj.OrganizationUnitCode = dALCode.AutoGenerateOrganizationCode();
            ViewBag.CountryList = db.CountryDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.CountryName).ToList();
            return View(obj);
        }

        // POST: OrganizationDefinitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSuperAdmin(OrganizationDefinition organizationDefinition, HttpPostedFileBase fileOrganizationLogo, HttpPostedFileBase fileOrganizationLogoSingle)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            byte[] Logobytes;
            byte[] LogoSinglebytes;

            if (ModelState.IsValid)
            {
                if (fileOrganizationLogo != null)
                {
                    using (BinaryReader br = new BinaryReader(fileOrganizationLogo.InputStream))
                    {
                        Logobytes = br.ReadBytes(fileOrganizationLogo.ContentLength);
                        //findObj.OrganizationLogo = Logobytes;
                        Stream stream = new MemoryStream(Logobytes);
                        organizationDefinition.OrganizationLogo = CommonFunctions.ResizeImage(stream, 213, 38);
                    }
                }

                if (fileOrganizationLogoSingle != null)
                {
                    using (BinaryReader br = new BinaryReader(fileOrganizationLogoSingle.InputStream))
                    {
                        LogoSinglebytes = br.ReadBytes(fileOrganizationLogoSingle.ContentLength);
                        Stream stream = new MemoryStream(LogoSinglebytes);
                        //findObj.OrganizationLogoSingle = LogoSinglebytes;
                        organizationDefinition.OrganizationLogoSingle = CommonFunctions.ResizeImage(stream, 50, 50);
                    }
                }
                organizationDefinition.IsDeleted = false;
                organizationDefinition.CreatedBy = UserID;
                organizationDefinition.CreatedDate = DateTime.Now;
                db.OrganizationDefinitions.Add(organizationDefinition);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Organization has been saved successfully";

                return RedirectToAction("IndexSuperAdmin");
            }

            return View(organizationDefinition);
        }

        // GET: OrganizationDefinitions/Edit/5
        public ActionResult EditSuperAdmin(long? id)
        {
            ViewBag.CountryList = db.CountryDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.CountryName).ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrganizationDefinition organizationDefinition = db.OrganizationDefinitions.Find(id);
            if (organizationDefinition == null)
            {
                return HttpNotFound();
            }
            return View(organizationDefinition);
        }

        // POST: OrganizationDefinitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSuperAdmin(OrganizationDefinition organizationDefinition, HttpPostedFileBase fileOrganizationLogo, HttpPostedFileBase fileOrganizationLogoSingle)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            byte[] Logobytes;
            byte[] LogoSinglebytes;
            if (ModelState.IsValid)
            {

                var findObj = db.OrganizationDefinitions.Find(organizationDefinition.Id);

                findObj.OrganizationUnitName = organizationDefinition.OrganizationUnitName;
                findObj.OrganizationUnitCode = organizationDefinition.OrganizationUnitCode;
                findObj.Website = organizationDefinition.Website;

                findObj.ContactNo = organizationDefinition.ContactNo;
                findObj.MobileNo = organizationDefinition.MobileNo;
                findObj.FaxNo = organizationDefinition.FaxNo;
                findObj.Email = organizationDefinition.Address1;
                findObj.Address1 = organizationDefinition.Address1;
                findObj.Address2 = organizationDefinition.Address2;
                findObj.CountryId = organizationDefinition.CountryId;
                findObj.ProvinceId = organizationDefinition.ProvinceId;
                findObj.RegionId = organizationDefinition.RegionId;
                findObj.CityId = organizationDefinition.CityId;
                findObj.AreaId = organizationDefinition.AreaId;
                findObj.OrganizationPincode = organizationDefinition.OrganizationPincode;
                findObj.NTN = organizationDefinition.NTN;
                findObj.GSTNo = organizationDefinition.GSTNo;
                findObj.OrganizationTitle = organizationDefinition.OrganizationTitle;
                if (fileOrganizationLogo != null)
                {
                    using (BinaryReader br = new BinaryReader(fileOrganizationLogo.InputStream))
                    {
                        Logobytes = br.ReadBytes(fileOrganizationLogo.ContentLength);
                        //findObj.OrganizationLogo = Logobytes;
                        Stream stream = new MemoryStream(Logobytes);
                        findObj.OrganizationLogo = CommonFunctions.ResizeImage(stream, 213, 38);
                    }
                }

                if (fileOrganizationLogoSingle != null)
                {
                    using (BinaryReader br = new BinaryReader(fileOrganizationLogoSingle.InputStream))
                    {
                        LogoSinglebytes = br.ReadBytes(fileOrganizationLogoSingle.ContentLength);
                        Stream stream = new MemoryStream(LogoSinglebytes);
                        //findObj.OrganizationLogoSingle = LogoSinglebytes;
                        findObj.OrganizationLogoSingle = CommonFunctions.ResizeImage(stream, 50, 50);
                    }
                }


                findObj.IsDeleted = false;
                findObj.UpdatedBy = UserID;
                findObj.UpdatedDate = DateTime.Now;
                db.Entry(findObj).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Organization has been updated successfully";
                return RedirectToAction("IndexSuperAdmin");
            }
            return View(organizationDefinition);
        }

        // GET: OrganizationDefinitions/Delete/5
        public ActionResult DeleteSuperAdmin(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrganizationDefinition organizationDefinition = db.OrganizationDefinitions.Find(id);
            if (organizationDefinition == null)
            {
                return HttpNotFound();
            }
            organizationDefinition.IsDeleted = true;
            organizationDefinition.DeletedBy = UserID;
            organizationDefinition.DeletedDate = DateTime.Now;
            db.Entry(organizationDefinition).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Organization has been deleted successfully";
            return RedirectToAction("IndexSuperAdmin");
        }
        public ActionResult CreateAccountListSuperAdmin(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrganizationDefinition organizationDefinition = db.OrganizationDefinitions.Find(id);
            if (organizationDefinition == null)
            {
                return HttpNotFound();
            }

            var objList = db.AccountSystemList.ToList();
            foreach (var item in objList)
            {
                Account obj = new Account();
                obj.OrganizationID = organizationDefinition.Id;
                obj.ParentID = item.ParentID;
                obj.AccountNo = item.AccountNo;
                obj.AccountName = item.AccountName;
                obj.AccountType = item.AccountType;
                obj.LevelID = item.LevelID;
                obj.HeadID = item.HeadID;
                obj.Description = item.Description;
                obj.IsActive = obj.IsActive;
             
                obj.IsDeleted = false;
                obj.IsActive = true;
                obj.IsSystemAccount = false;
                obj.CreatedBy = UserID;
                obj.CreatedDate = DateTime.Now;
                db.Accounts.Add(obj);
                db.SaveChanges();
            }
            organizationDefinition.IsChartOfAccountCreated = true;
            db.Entry(organizationDefinition).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Organization Accounts List has been created successfully";
            return RedirectToAction("IndexSuperAdmin");
        }
    }
}
