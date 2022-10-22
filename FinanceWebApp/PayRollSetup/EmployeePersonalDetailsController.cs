using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

namespace MudasirRehmanAlp.PayRollSetup
{
    public class EmployeePersonalDetailsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        EmployeesDAL employeesDAL = new EmployeesDAL();

        // GET: EmployeePersonalDetails
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            List<EmployeePersonalDetails> listobj = new List<EmployeePersonalDetails>();
            listobj = db.EmployeePersonalDetails.Where(a => a.IsDeleted == false && a.OrganizationID == OrganizationID).ToList();
            PagedList<EmployeePersonalDetails> model = new PagedList<EmployeePersonalDetails>(listobj, page, pageSize);
            return View(model);
        }

        // GET: EmployeePersonalDetails/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeePersonalDetails employeePersonalDetails = db.EmployeePersonalDetails.Find(id);
            if (employeePersonalDetails == null)
            {
                return HttpNotFound();
            }
            return View(employeePersonalDetails);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeePersonalDetails employeePersonalDetails = db.EmployeePersonalDetails.Find(id);
            if (employeePersonalDetails == null)
            {
                return HttpNotFound();
            }
            return View(employeePersonalDetails);
        }
        // GET: EmployeePersonalDetails/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            EmployeePersonalDetails obj = new EmployeePersonalDetails();
            obj.EmployeeCode = dALCode.AutoGenerateEmployeeCode(OrganizationID);
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;

            return View(obj);
        }

        // POST: EmployeePersonalDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult CreatePersonalDetails(string EmployeeDetails, HttpPostedFileBase[] EmpImages, HttpPostedFileBase[] CNICFront, HttpPostedFileBase[] CNICBack)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            HttpPostedFileBase ProfilePic = Request.Files["EmpImages"];
            HttpPostedFileBase CnicFrontPic = Request.Files["CNICFront"];
            HttpPostedFileBase CnicBackPic = Request.Files["CNICBack"];
            long EmployeeId = 0;
            var getmessage = employeesDAL.AddEmployee(EmployeeDetails, ProfilePic, CnicFrontPic, CnicBackPic, UserID, out EmployeeId);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                //TempData["Validation"] = true;
                //TempData["ErrorMessage"] = "Employee has been saved successfully";

                return Json(EmployeeId, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetPersonalDetails(string ID)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long Id = 0;
            if (!String.IsNullOrEmpty(ID))
            {
                Id = Convert.ToInt64(ID);
            }
            EmployeePersonalDetails employeePersonalDetails = db.EmployeePersonalDetails.Find(Id);
            GetPersonalDetailViewModel viewModel = new GetPersonalDetailViewModel();
            viewModel.EmployeeId = employeePersonalDetails.EmployeeId;
            viewModel.OrganizationID = employeePersonalDetails.OrganizationID;
            viewModel.EmployeeCode = employeePersonalDetails.EmployeeCode;
            viewModel.Title = employeePersonalDetails.Title;
            viewModel.FirstName = employeePersonalDetails.FirstName;
            viewModel.LastName = employeePersonalDetails.LastName;
            viewModel.FatherName = employeePersonalDetails.FatherName;
            viewModel.DateofBirth = employeePersonalDetails.DateofBirth;
            viewModel.CNICNo = employeePersonalDetails.CNICNo;
            viewModel.Gender = employeePersonalDetails.Gender;
            viewModel.MobileNo = employeePersonalDetails.MobileNo;
            viewModel.PhoneNo = employeePersonalDetails.PhoneNo;
            viewModel.Email = employeePersonalDetails.Email;
            viewModel.MaritalStatus = employeePersonalDetails.MaritalStatus;
            viewModel.CityID = employeePersonalDetails.CityID;
            viewModel.PermanentAddress = employeePersonalDetails.PermanentAddress;
            viewModel.TemporaryAddress = employeePersonalDetails.TemporaryAddress;
            viewModel.CNICFront = employeePersonalDetails.CNICFront;
            viewModel.CNICBack = employeePersonalDetails.CNICBack;
            viewModel.EmployeePicture = employeePersonalDetails.EmployeePicture;
            viewModel.EmployeeProfilePic = employeePersonalDetails.EmployeeProfilePic;
            viewModel.IsActive = employeePersonalDetails.IsActive;
            viewModel.IsUser = employeePersonalDetails.IsUser;

            return Json(viewModel, JsonRequestBehavior.AllowGet);

        }
        
        [HttpPost]
        public ActionResult CreateorEditOfficalDetails(string EmployeeOffical)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long Id = 0;
            var getmessage = employeesDAL.AddorUpdateEmployeeOffical(EmployeeOffical, UserID, out Id);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(Id, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //TempData["Validation"] = true;
                //TempData["ErrorMessage"] = "Employee has been saved successfully";

                return Json(Id, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CreateEducationDetails(string EmployeeEducation)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);


            var getmessage = employeesDAL.AddEmployeeEducation(EmployeeEducation, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                //TempData["Validation"] = true;
                //TempData["ErrorMessage"] = "Employee has been saved successfully";

                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult CreateExperienceDetails(string EmployeeExperience)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = employeesDAL.AddEmployeeExperience(EmployeeExperience, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                //TempData["Validation"] = true;
                //TempData["ErrorMessage"] = "Employee has been saved successfully";

                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        // GET: EmployeePersonalDetails/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeePersonalDetails employeePersonalDetails = db.EmployeePersonalDetails.Find(id);
            ViewBag.EmployeeOfficiallDetailsObj = db.EmployeeOfficiallDetails.Where(a=>a.EmployeeId==id).FirstOrDefault();
            ViewBag.EmployeeEducationDetailsList = db.EmployeeEducationDetails.Where(a => a.EmployeeId == id).ToList();
            ViewBag.EmployeeExperienceDetailsList = db.EmployeeExperienceDetails.Where(a => a.EmployeeId == id).ToList();
            var objFindOfficial = db.EmployeeOfficiallDetails.Where(a => a.EmployeeId == id).FirstOrDefault();
            if (objFindOfficial !=null)
            {
                ViewBag.OfficialId = objFindOfficial.Id;
            }
            
            if (employeePersonalDetails == null)
            {
                return HttpNotFound();
            }            
            return View(employeePersonalDetails);
        }

        // POST: EmployeePersonalDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult EditPersonalDetails(string EmployeeDetails, HttpPostedFileBase[] EmpImages, HttpPostedFileBase[] CNICFront, HttpPostedFileBase[] CNICBack)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            HttpPostedFileBase ProfilePic = Request.Files["EmpImages"];
            HttpPostedFileBase CnicFrontPic = Request.Files["CNICFront"];
            HttpPostedFileBase CnicBackPic = Request.Files["CNICBack"];
            long EmployeeId = 0;
            var getmessage = employeesDAL.UpdateEmployee(EmployeeDetails, ProfilePic, CnicFrontPic, CnicBackPic, UserID, out EmployeeId);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(EmployeeId, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Employee has been updated successfully";
                return Json(EmployeeId, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteEducationDetailSingle(string ID)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = employeesDAL.DeleteEducationDetail(ID, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Employee education has been deleted successfully";
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteExperienceDetailSingle(string ID)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = employeesDAL.DeleteExperienceDetail(ID, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Employee education has been deleted successfully";
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        // GET: EmployeePersonalDetails/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeePersonalDetails employeePersonalDetails = db.EmployeePersonalDetails.Find(id);
            if (employeePersonalDetails == null)
            {
                return HttpNotFound();
            }
            var getmessage = employeesDAL.DeleteEmployee(employeePersonalDetails, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Employee has been deleted successfully";
                return RedirectToAction("Index");
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
        public ActionResult Search(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            EmployeePersonalDetails _searchObject = JsonConvert.DeserializeObject<EmployeePersonalDetails>(searchModel);
            List<EmployeePersonalDetails> _listObject = new List<EmployeePersonalDetails>();
            _listObject = db.EmployeePersonalDetails.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID).ToList();
            if (!String.IsNullOrEmpty(_searchObject.EmployeeCode) && !String.IsNullOrEmpty(_searchObject.FirstName))
            {
                _listObject = _listObject.Where(r => r.EmployeeCode != null && r.EmployeeCode.ToString().ToLower().Contains(_searchObject.EmployeeCode) || r.FirstName.ToString().ToLower().Contains(_searchObject.FirstName)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.EmployeeCode))
            {
                _listObject = _listObject.Where(r => r.EmployeeCode != null && r.EmployeeCode.ToString().ToLower().Contains(_searchObject.EmployeeCode)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.FirstName))
            {

                _listObject = _listObject.Where(r => r.FirstName != null && r.FirstName.ToString().ToLower().Contains(_searchObject.FirstName)).ToList();

            }
            return PartialView("_Search", _listObject);
        }
        public ActionResult GetEducationDetailsByEmployeeId(string ID)
        {
            long id = 0;
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }
            var Result = db.EmployeeEducationDetails.Where(a => a.EmployeeId == id  && a.IsActive == true && a.IsDeleted == false).ToList();

            return PartialView("_PartialViewEducationDetails", Result);
        }
        public ActionResult GetExperienceDetailsByEmployeeId(string ID)
        {
            long id = 0;
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }
            var Result = db.EmployeeExperienceDetails.Where(a => a.EmployeeId == id &&  a.IsActive == true && a.IsDeleted == false).ToList();

            return PartialView("_PartialViewExperienceDetails", Result);
        }

        //=========================================================================>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        // GET: EmployeePersonalDetails For Super Admin
        public ActionResult IndexSuperAdmin(int page = 1, int pageSize = 15)
        {

            List<EmployeePersonalDetails> listobj = new List<EmployeePersonalDetails>();
            listobj = db.EmployeePersonalDetails.Where(a => a.IsDeleted == false).ToList();
            PagedList<EmployeePersonalDetails> model = new PagedList<EmployeePersonalDetails>(listobj, page, pageSize);
            return View(model);
        }

        // GET: EmployeePersonalDetails/Details/5
        public ActionResult DetailsSuperAdmin(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeePersonalDetails employeePersonalDetails = db.EmployeePersonalDetails.Find(id);
            if (employeePersonalDetails == null)
            {
                return HttpNotFound();
            }
            return View(employeePersonalDetails);
        }
        public ActionResult PrintSuperAdmin(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeePersonalDetails employeePersonalDetails = db.EmployeePersonalDetails.Find(id);
            if (employeePersonalDetails == null)
            {
                return HttpNotFound();
            }
            return View(employeePersonalDetails);
        }
        // GET: EmployeePersonalDetails/Create
        public ActionResult CreateSuperAdmin()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            EmployeePersonalDetails obj = new EmployeePersonalDetails();
            obj.EmployeeCode = dALCode.AutoGenerateEmployeeCode(OrganizationID);
            ViewBag.CityDefinitionsList = db.CityDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.CityName).ToList();
            ViewBag.OrganizationList = db.OrganizationDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.OrganizationUnitName).ToList();
            return View(obj);
        }

        // POST: EmployeePersonalDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult CreateSuperAdmin(string EmployeeDetails, HttpPostedFileBase[] EmpImages, HttpPostedFileBase[] CNICFront, HttpPostedFileBase[] CNICBack)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            HttpPostedFileBase ProfilePic = Request.Files["EmpImages"];
            HttpPostedFileBase CnicFrontPic = Request.Files["CNICFront"];
            HttpPostedFileBase CnicBackPic = Request.Files["CNICBack"];
            long EmployeeId = 0;
            var getmessage = employeesDAL.AddEmployee(EmployeeDetails, ProfilePic, CnicFrontPic, CnicBackPic, UserID, out EmployeeId);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Employee has been saved successfully";

                return Json("", JsonRequestBehavior.AllowGet);
            }
        }


        // GET: EmployeePersonalDetails/Edit/5
        public ActionResult EditSuperAdmin(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeePersonalDetails employeePersonalDetails = db.EmployeePersonalDetails.Find(id);
            if (employeePersonalDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationList = db.OrganizationDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.OrganizationUnitName).ToList();
            ViewBag.CityDefinitionsList = db.CityDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.CityName).ToList();
            return View(employeePersonalDetails);
        }

        // POST: EmployeePersonalDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult EditSuperAdmin(string EmployeeDetails, HttpPostedFileBase[] EmpImages, HttpPostedFileBase[] CNICFront, HttpPostedFileBase[] CNICBack)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            HttpPostedFileBase ProfilePic = Request.Files["EmpImages"];
            HttpPostedFileBase CnicFrontPic = Request.Files["CNICFront"];
            HttpPostedFileBase CnicBackPic = Request.Files["CNICBack"];
            long EmployeeId = 0;
            var getmessage = employeesDAL.UpdateEmployee(EmployeeDetails, ProfilePic, CnicFrontPic, CnicBackPic, UserID, out EmployeeId);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Employee has been updated successfully";
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: EmployeePersonalDetails/Delete/5
        public ActionResult DeleteSuperAdmin(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeePersonalDetails employeePersonalDetails = db.EmployeePersonalDetails.Find(id);
            if (employeePersonalDetails == null)
            {
                return HttpNotFound();
            }
            var getmessage = employeesDAL.DeleteEmployee(employeePersonalDetails, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return RedirectToAction("IndexSuperAdmin");
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Employee has been deleted successfully";
                return RedirectToAction("IndexSuperAdmin");
            }
        }
       
    }
}
