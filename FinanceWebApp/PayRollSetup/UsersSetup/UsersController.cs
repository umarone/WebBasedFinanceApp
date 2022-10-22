using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Extensions.Logging;
using MudasirRehmanAlp.AppDAL;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Infrastructure;
using MudasirRehmanAlp.Infrastructure.AppServices;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;
using Newtonsoft.Json;
using PagedList;

namespace MudasirRehmanAlp.PayRollSetup.UsersSetup
{
    public class UsersController : Controller
    {
        public UsersController() { }
        private AppEntities db = new AppEntities();
        EmployeeUserDAL employeeUserDAL = new EmployeeUserDAL();
        private readonly ILogger _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index(int page = 1, int pageSize = 15)
        {

            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            List<Users> listobj = new List<Users>();
            listobj = db.Users.Where(a => a.IsDeleted == false && a.OrganizationID == OrganizationID).ToList();
            PagedList<Users> model = new PagedList<Users>(listobj, page, pageSize);
            return View(model);
        }

        // GET: Users/Details/5
        public ActionResult Details(long? id)
        {
            EmailkitService emailkitService = new EmailkitService();
            MailSettingViewModel mailSetting = new MailSettingViewModel();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            EmployeeUsersViewModel obj = new EmployeeUsersViewModel();
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            var listObj = (from e in db.EmployeePersonalDetails.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsUser == true && a.OrganizationID == OrganizationID)
                           where !db.Users.Any(f => f.EmployeeID == e.EmployeeId && f.IsActive == true && f.IsDeleted == false)
                           select e).OrderBy(a => a.FirstName).ToList();

            ViewBag.EmployeeList = listObj;
            //Not including 
            ViewBag.RoleList = db.Roles.Where(a => a.isActive == true && a.RoleId != 3).OrderBy(a => a.RoleName).ToList();
            return View(obj);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeUsersViewModel usersViewModel)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = employeeUserDAL.AddUser(usersViewModel, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Users has been saved successfully";
                return RedirectToAction("Index");
            }
        }
        public ActionResult JsonCreate(string userJsonObj, string checkListJsonObj)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = employeeUserDAL.AddUserWBranch(userJsonObj, checkListJsonObj, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Users has been saved successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Users/Edit/5
        public ActionResult Edit(long? id)
        {
            EmployeeUsersViewModel viewModel = new EmployeeUsersViewModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            UsersRole usersrole = db.UsersRole.Where(a => a.UserID == users.UserId).FirstOrDefault();
            viewModel.UserId = users.UserId;
            viewModel.OrganizationID = users.OrganizationID;
            viewModel.OrganizationName = users.OrganizationDefinition.OrganizationUnitName;
            viewModel.EmployeeID = users.EmployeeID;
            viewModel.RoleID = usersrole.RoleID;
            viewModel.RoleName = usersrole.Roles.RoleName;
            viewModel.UserRoleID = usersrole.UserRoleId;
            viewModel.FullName = users.FullName;
            viewModel.FirsName = users.EmployeePersonalDetails.FirstName;
            viewModel.LastName = users.EmployeePersonalDetails.LastName;
            viewModel.UserName = users.UserName;
            viewModel.IsActive = users.IsActive;
            var resultObjectList = db.ProcGetBranchCheckListByEmployeeId(users.EmployeeID, users.OrganizationID).ToList();
            viewModel.procGetBranchCheckListByEmployeeIds = resultObjectList;
            if (users == null)
            {
                return HttpNotFound();
            }

            //Not including 
            ViewBag.RoleList = db.Roles.Where(a => a.isActive == true && a.RoleId != 3).OrderBy(a => a.RoleName).ToList();

            return View(viewModel);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeUsersViewModel usersViewModel)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = employeeUserDAL.UpdateUser(usersViewModel, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return RedirectToAction("Index");
            }
            else
            {


                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "User has been updated successfully";
                return RedirectToAction("Index");
            }
            
        }
        public ActionResult JsonEdit(string userJsonObj, string checkListJsonObj)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = employeeUserDAL.UpdateUserBranch(userJsonObj,checkListJsonObj, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {


                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "User has been updated successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }
        // GET: Users/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = employeeUserDAL.DeleteUser(users, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Users has been deleted successfully";
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
        //FUnction
        public JsonResult EmployeeDetails(string ID)
        {
            long id = 0;
            if (!String.IsNullOrEmpty(ID))
            {
                id = Convert.ToInt64(ID);
            }
            var result = (from e in db.EmployeePersonalDetails.Where(a => a.EmployeeId == id)

                          select new EmployeeViewModel
                          {
                              EmployeeID = e.EmployeeId,
                              FullName = e.FirstName + " " + e.LastName,
                              OrganiztionID = e.OrganizationID,
                              OrganiztionName = e.OrganizationDefinition.OrganizationUnitName
                          }).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GeneratorUserName(string ID)
        {
            long id = 0;
            string Username = "";

            if (!String.IsNullOrEmpty(ID))
            {
                id = Convert.ToInt64(ID);
            }
            var result = db.EmployeePersonalDetails.Where(a => a.EmployeeId == id).FirstOrDefault();
            if (result != null)
            {
                if (!String.IsNullOrEmpty(result.Email))
                {
                    Username = result.Email;

                }
                else
                {
                    Username = result.FirstName + result.LastName + "-" + result.EmployeeCode;

                }
            }
            Username.Trim();
            Username.ToLower();
            return Json(Username, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            Users _searchObject = JsonConvert.DeserializeObject<Users>(searchModel);
            List<Users> _listObject = new List<Users>();
            _listObject = db.Users.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID).ToList();
            if (!String.IsNullOrEmpty(_searchObject.UserName))
            {
                _listObject = _listObject.Where(r => r.UserName != null && r.UserName.ToString().ToLower().Contains(_searchObject.UserName.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.UserName))
            {
                _listObject = _listObject.Where(r => r.UserName != null && r.UserName.ToString().ToLower().Contains(_searchObject.UserName.ToLower())).ToList();
            }

            return PartialView("_Search", _listObject);
        }
        public ActionResult LoadBranchCheckList(string Id)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long id = 0;

            if (!String.IsNullOrEmpty(Id))
            {
                id = Convert.ToInt64(Id);
            }
            var resultObjectList = db.ProcGetBranchCheckListByEmployeeId(id, OrganizationID).ToList();


            return PartialView("_BranchCheckList", resultObjectList);
        }
        //---------- For Super Admin
        public ActionResult IndexSuperAdmin(int page = 1, int pageSize = 15)
        {


            List<Users> listobj = new List<Users>();
            listobj = db.Users.Where(a => a.IsDeleted == false).ToList();
            PagedList<Users> model = new PagedList<Users>(listobj, page, pageSize);
            return View(model);
        }

        // GET: Users/Details/5
        public ActionResult DetailsSuperAdmin(long? id)
        {
            EmailkitService emailkitService = new EmailkitService();
            MailSettingViewModel mailSetting = new MailSettingViewModel();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        public ActionResult CreateSuperAdmin()
        {
            EmployeeUsersViewModel obj = new EmployeeUsersViewModel();
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            var listObj = (from e in db.EmployeePersonalDetails.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsUser == true && a.OrganizationID == OrganizationID)
                           where !db.Users.Any(f => f.EmployeeID == e.EmployeeId)
                           select e).OrderBy(a => a.FirstName).ToList();

            ViewBag.EmployeeList = listObj;
            //Not including 
            ViewBag.RoleList = db.Roles.Where(a => a.isActive == true && a.RoleId != 3).OrderBy(a => a.RoleName).ToList();
            return View(obj);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSuperAdmin(EmployeeUsersViewModel usersViewModel)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = employeeUserDAL.AddUser(usersViewModel, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return RedirectToAction("IndexSuperAdmin");
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Users has been saved successfully";
                return RedirectToAction("IndexSuperAdmin");
            }
        }

        // GET: Users/Edit/5
        public ActionResult EditSuperAdmin(long? id)
        {
            EmployeeUsersViewModel viewModel = new EmployeeUsersViewModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            UsersRole usersrole = db.UsersRole.Where(a => a.UserID == users.UserId).FirstOrDefault();
            viewModel.UserId = users.UserId;
            viewModel.OrganizationID = users.OrganizationID;
            viewModel.OrganizationName = users.OrganizationDefinition.OrganizationUnitName;
            viewModel.EmployeeID = users.EmployeeID;
            viewModel.RoleID = usersrole.RoleID;
            viewModel.RoleName = usersrole.Roles.RoleName;
            viewModel.UserRoleID = usersrole.UserRoleId;
            viewModel.FullName = users.FullName;
            viewModel.FirsName = users.EmployeePersonalDetails.FirstName;
            viewModel.LastName = users.EmployeePersonalDetails.LastName;
            viewModel.UserName = users.UserName;
            viewModel.IsActive = users.IsActive;

            if (users == null)
            {
                return HttpNotFound();
            }

            //Not including 
            ViewBag.RoleList = db.Roles.Where(a => a.isActive == true && a.RoleId != 3).OrderBy(a => a.RoleName).ToList();

            return View(viewModel);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSuperAdmin(EmployeeUsersViewModel usersViewModel)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = employeeUserDAL.UpdateUser(usersViewModel, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return RedirectToAction("IndexSuperAdmin");
            }
            else
            {


                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "User has been updated successfully";
                return RedirectToAction("IndexSuperAdmin");
            }
        }

        // GET: Users/Delete/5
        public ActionResult DeleteSuperAdmin(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = employeeUserDAL.DeleteUser(users, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return RedirectToAction("IndexSuperAdmin");
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Users has been deleted successfully";
                return RedirectToAction("IndexSuperAdmin");
            }
        }
    }
}
