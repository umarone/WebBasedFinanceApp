using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MudasirRehmanAlp.AppDAL;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Helper;
using MudasirRehmanAlp.Helpers;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;


namespace MudasirRehmanAlp.PayRollSetup.UsersSetup
{
    public class AccountController : Controller
    {
        private AppEntities db = new AppEntities();
        WebProvider webProvider = new WebProvider();
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            Users user = CheckCookies();
            if (user == null)
            {
                return View();
            }
            else
            {
                UsersAccountViewModel usersAccountViewModel = new UsersAccountViewModel();
                usersAccountViewModel.UserName = user.UserName;
                usersAccountViewModel.Password = user.Password;
                var restult = LoginFuntion(usersAccountViewModel);

                if (restult == "OK")
                {

                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    return HttpNotFound();
                }
            }


        }
        [HttpPost]
        public ActionResult Login(UsersAccountViewModel usersAccountViewModel)
        {

            var restult = LoginFuntion(usersAccountViewModel);

            if (restult == "OK")
            {

                if (usersAccountViewModel.RemorememberMe)
                {
                    HttpCookie chUserName = new HttpCookie("UserName");
                    chUserName.Expires = DateTime.Now.AddSeconds(3600);
                    chUserName.Value = usersAccountViewModel.UserName;
                    Response.Cookies.Add(chUserName);

                    HttpCookie chPassword = new HttpCookie("Password");
                    chPassword.Expires = DateTime.Now.AddSeconds(3600);
                    chPassword.Value = usersAccountViewModel.Password;
                    Response.Cookies.Add(chPassword);
                }
                return RedirectToAction("Dashboard", "Home");
            }
            else if (restult == "NOTACTIVE")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = "User are inactive or deleted";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = "You have entered an invalid username or password";
                return RedirectToAction("Login");
            }
        }
        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Remove("UserName");
            Session.Remove("UserFirstName");
            Session.Remove("UserID");
            Session.Remove("OrganizationID");
            Session.Remove("OrganizationTitle");
            Session.Remove("RoleName");
            Session.Remove("OrganizationLogo");
            Session.Remove("OrganizationLogoSingle");
            #region Notifications_Session_Region
            Session.Remove("isNotificationsJobRun");
            #endregion

            if (Response.Cookies["UserName"] != null)
            {

                HttpCookie chUserName = new HttpCookie("UserName");
                chUserName.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(chUserName);
            }
            if (Response.Cookies["Password"] != null)
            {

                HttpCookie chPassword = new HttpCookie("Password");
                chPassword.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(chPassword);
            }


            return RedirectToAction("Login");
        }
        public Users CheckCookies()
        {
            Users user = null;
            string UserName = string.Empty;
            string Password = string.Empty;

            if (Request.Cookies["UserName"] != null)
            {
                UserName = Request.Cookies["UserName"].Value;
            }
            if (Request.Cookies["Password"] != null)
            {
                Password = Request.Cookies["Password"].Value;
            }
            if (!String.IsNullOrEmpty(UserName) && !String.IsNullOrEmpty(Password))
            {
                user = new Users();
                user.UserName = UserName;
                user.Password = Password;
            }

            return user;

        }

        [HttpGet]
        public ActionResult RedirectFromBranch(long id)
        {
           
            UsersAccountViewModel usersAccountViewModel = new UsersAccountViewModel();
            usersAccountViewModel.UserName =Convert.ToString(Session["UserName"]);
            usersAccountViewModel.Password = Convert.ToString(Session["UserPassword"]);
            usersAccountViewModel.BranchId = id;
            var restult = LoginFuntion(usersAccountViewModel);
            if (restult == "OK")
            {

                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Lock()
        {
            UsersAccountViewModel viewModel = new UsersAccountViewModel();
            viewModel.UserName = Convert.ToString(Session["UserName"]);
            if (!String.IsNullOrEmpty(viewModel.UserName))
            {
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Login");
            }

        }
        public ActionResult UnLock(UsersAccountViewModel usersAccountViewModel)
        {

            var restult = LoginFuntion(usersAccountViewModel);

            if (restult == "OK")
            {

                if (usersAccountViewModel.RemorememberMe)
                {
                    HttpCookie chUserName = new HttpCookie("UserName");
                    chUserName.Expires = DateTime.Now.AddSeconds(3600);
                    chUserName.Value = usersAccountViewModel.UserName;
                    Response.Cookies.Add(chUserName);

                    HttpCookie chPassword = new HttpCookie("Password");
                    chPassword.Expires = DateTime.Now.AddSeconds(3600);
                    chPassword.Value = usersAccountViewModel.Password;
                    Response.Cookies.Add(chPassword);
                }
                return RedirectToAction("Dashboard", "Home");
            }
            else if (restult == "NOTACTIVE")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = "User are inactive or deleted";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = "You have entered an invalid username or password";
                return RedirectToAction("Lock");
            }
        }

        public string LoginFuntion(UsersAccountViewModel usersAccountViewModel)
        {
            string Message = "";
            List<Menu> ListObj = new List<Menu>();
            List<BranchesRights> objBranchesList = new List<BranchesRights>();
            
            MailSetting mailSettingObj = new MailSetting();
            BranchesRights branchesRights = new BranchesRights();
            MenuRightsDAL menuRightsDAL = new MenuRightsDAL();
            if (!String.IsNullOrEmpty(usersAccountViewModel.UserName) && !String.IsNullOrEmpty(usersAccountViewModel.Password))
            {
                var model = db.Users.Where(a => a.UserName == usersAccountViewModel.UserName.ToLower().ToString()).FirstOrDefault();
                if (model == null)
                {
                    return Message = "NO";
                }
                else
                {
                    if (model.IsDeleted == true || model.IsActive == false)
                    {
                        return Message = "NOTACTIVE";
                    }
                }
                bool resultPassword = PasswordStorage.VerifyPassword(usersAccountViewModel.Password, model.Password);
                if (resultPassword)
                {
                    var roleResult = webProvider.GetRolesForUser(model.UserName);
                    var RoleName = roleResult[0];
                    if (RoleName == "Super Admin")
                    {
                        var getmessage = menuRightsDAL.AddorRemoveMenuRightsForSuperAdmin(model.UserId);
                    }

                    if (model.EmployeePersonalDetails != null && model.OrganizationDefinition != null)
                    {
                        if (usersAccountViewModel.BranchId != null)
                        {
                            branchesRights = db.BranchesRights.Where(a => a.EmployeeId == model.EmployeeID && a.OrganizationId == model.OrganizationID && a.IsActive == true && a.IsSelected == true && a.BranchId == usersAccountViewModel.BranchId).FirstOrDefault();

                        }
                        else
                        {
                            branchesRights = db.BranchesRights.Where(a => a.EmployeeId == model.EmployeeID && a.OrganizationId == model.OrganizationID && a.IsActive == true && a.IsSelected==true).FirstOrDefault();

                        }
                        objBranchesList = db.BranchesRights.Where(a => a.EmployeeId == model.EmployeeID && a.OrganizationId == model.OrganizationID && a.IsActive == true && a.IsSelected==true).ToList();

                        Session["UserFirstName"] = model.EmployeePersonalDetails.FirstName;
                        Session["UserName"] = model.UserName;
                        Session["UserPassword"] = usersAccountViewModel.Password;
                        Session["UserID"] = model.UserId;
                        Session["EmployeeId"] = model.EmployeeID;
                        Session["OrganizationID"] = model.OrganizationID;
                        Session["OrganizationTitle"] = model.OrganizationDefinition.OrganizationTitle;
                        Session["RoleName"] = RoleName;

                        Session["OrganizationID"] = model.OrganizationID;
                        Session["BranchId"] = branchesRights.BranchId;
                        Session["BranchesList"] = objBranchesList;
                        Session["isNotificationsJobRun"] = false;

                        var UserPic = model.EmployeePersonalDetails.EmployeeProfilePic;

                        if (UserPic != null)
                        {
                            Session["EmployeeProfilePic"] = UserPic;
                        }
                        var Logo = model.OrganizationDefinition.OrganizationLogo;
                        if (Logo != null)
                        {
                            Session["OrganizationLogo"] = Logo;
                        }
                        var LogoSingle = model.OrganizationDefinition.OrganizationLogoSingle;
                        if (LogoSingle != null)
                        {
                            Session["OrganizationLogoSingle"] = LogoSingle;
                        }
                        ListObj = (from urb in db.UsersBaseMenusRights
                                   join m in db.Menu on urb.MenuID equals m.MenuID
                                   where urb.UserId == model.UserId && urb.IsDeleted == false
                                   select m).ToList();
                        Session["loadUserBasedMenuListAtLoginTime"] = ListObj;
                    }
                    else
                    {
                        Session["UserName"] = model.UserName;
                        Session["UserID"] = model.UserId;
                        Session["RoleName"] = RoleName;
                        ListObj = (from urb in db.UsersBaseMenusRights
                                   join m in db.Menu on urb.MenuID equals m.MenuID
                                   where urb.UserId == model.UserId && urb.IsDeleted == false
                                   select m).Distinct().ToList();
                        Session["loadUserBasedMenuListAtLoginTime"] = ListObj;
                    }

                    return Message = "OK";
                }
                else
                {
                    return Message = "NO";
                }
            }
            return Message;
        }
    }
}