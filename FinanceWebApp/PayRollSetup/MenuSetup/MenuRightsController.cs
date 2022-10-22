using MudasirRehmanAlp.AppDAL;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.ModelsView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MudasirRehmanAlp.PayRollSetup.MenuSetup
{
    public class MenuRightsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        MenuRightsDAL employeesDAL = new MenuRightsDAL();
        // GET: MenuRights
        public ActionResult Index()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            var OrganizationObj = dALCode.GetOrganizationDefinition(OrganizationID);
            ViewBag.OrganizationUnitName = OrganizationObj.OrganizationUnitName;

            var ResultList = (from a in db.Users.Where(a => a.IsActive == true && a.IsDeleted == false)
                              where a.OrganizationID == OrganizationID
                              select new EmployeeViewModel
                              {
                                  UserID = a.UserId,
                                  FullName = a.EmployeePersonalDetails.EmployeeCode + "-" + a.FullName,
                              }).OrderByDescending(a => a.UserID).ToList();
            ViewBag.ObjectList = ResultList;
            ViewBag.OrganizationID = OrganizationID;
            return View();
        }
        [HttpPost]
        public ActionResult Create(string MenuJsonObj)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);

            var getmessage = employeesDAL.AddorUpdateMenuRights(MenuJsonObj, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "User Menu Rights has been saved successfully";

                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUserRolesByUserID(string ID)
        {
            long id = 0;

            if (!String.IsNullOrEmpty(ID))
            {
                id = Convert.ToInt64(ID);
            }

            string RoleName = "";
            var findResult = db.UsersRole.Where(a => a.UserID == id).FirstOrDefault();
            if (findResult != null)
            {
                RoleName = findResult.Roles.RoleName;
            }


            return Json(RoleName, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadMenuRightsByUserId(string userID, string levelID, string menuName)
        {
            long id = 0;

            if (!String.IsNullOrEmpty(userID))
            {
                id = Convert.ToInt64(userID);
            }
            if (String.IsNullOrEmpty(levelID))
            {
                levelID = "1";
            }

            var resultObjectList = db.ProcGetUsersBaseMenusRight(id, levelID, menuName).ToList();

            return PartialView("_PartialViewMenus", resultObjectList);
        }
        //----------- For Super Admin
        public ActionResult IndexSuperAdmin()
        {

            return View();
        }
        [HttpPost]
        public ActionResult CreateSuperAdmin(string MenuJsonObj)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);

            var getmessage = employeesDAL.AddorUpdateMenuRights(MenuJsonObj, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "User Menu Rights has been saved successfully";

                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
    }
}