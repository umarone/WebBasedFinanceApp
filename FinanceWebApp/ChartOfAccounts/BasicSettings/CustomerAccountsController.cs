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
using MudasirRehmanAlp.ModelsView;
using Newtonsoft.Json;
using PagedList;

namespace MudasirRehmanAlp.ChartOfCustomerAccounts.BasicSettings
{
    public class CustomerAccountsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        ChartAccountDAL chartAccountDAL = new ChartAccountDAL();

        // GET: CustomerAccounts

        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<Account> listobj = new List<Account>();
            listobj = db.Accounts.Where(a => a.ParentID != null && a.LevelID == 4 && a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId == BranchId && a.IsCustomer == true).OrderByDescending(a=>a.AccountId).ToList();
            PagedList<Account> model = new PagedList<Account>(listobj, page, pageSize);
            return View(model);

        }
        // GET: CustomerAccounts/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            var FindLevelThree = db.Accounts.Where(a => a.AccountId == account.ParentID).FirstOrDefault();
            var FindLevelTwo = db.Accounts.Where(a => a.AccountId == FindLevelThree.ParentID).FirstOrDefault();
            var FindLevelOne = db.Accounts.Where(a => a.AccountId == FindLevelTwo.ParentID).FirstOrDefault();

            ViewBag.LevelThreeName = FindLevelThree.AccountName;
            ViewBag.levelThreeNo = FindLevelThree.AccountNo;
            ViewBag.levelThreeAccountID = FindLevelThree.AccountId;

            ViewBag.LevelTwoName = FindLevelTwo.AccountName;
            ViewBag.levelTwoNo = FindLevelTwo.AccountNo;
            ViewBag.levelTwoAccountID = FindLevelTwo.AccountId;

            ViewBag.LevelOneName = FindLevelOne.AccountName;
            ViewBag.levelOneNo = FindLevelOne.AccountNo;
            ViewBag.levelOneAccountID = FindLevelOne.AccountId;
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            var FindLevelThree = db.Accounts.Where(a => a.AccountId == account.ParentID).FirstOrDefault();
            var FindLevelTwo = db.Accounts.Where(a => a.AccountId == FindLevelThree.ParentID).FirstOrDefault();
            var FindLevelOne = db.Accounts.Where(a => a.AccountId == FindLevelTwo.ParentID).FirstOrDefault();

            ViewBag.LevelThreeName = FindLevelThree.AccountName;
            ViewBag.levelThreeNo = FindLevelThree.AccountNo;
            ViewBag.levelThreeAccountID = FindLevelThree.AccountId;

            ViewBag.LevelTwoName = FindLevelTwo.AccountName;
            ViewBag.levelTwoNo = FindLevelTwo.AccountNo;
            ViewBag.levelTwoAccountID = FindLevelTwo.AccountId;

            ViewBag.LevelOneName = FindLevelOne.AccountName;
            ViewBag.levelOneNo = FindLevelOne.AccountNo;
            ViewBag.levelOneAccountID = FindLevelOne.AccountId;
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }
        // GET: CustomerAccounts/Create
        public ActionResult Create()
        {
            AccountsViewModel obj = new AccountsViewModel();
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);

            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
           
            var FindLevelOne = db.Accounts.Where(a => a.AccountId == 1).FirstOrDefault();
            var FindLevelTwo = db.Accounts.Where(a => a.AccountId == 6).FirstOrDefault();
            var FindLevelThree = db.Accounts.Where(a => a.AccountId == 14).FirstOrDefault();

            obj.LevelOneName = FindLevelOne.AccountName;
            obj.HeadID =Convert.ToInt32(FindLevelOne.AccountNo);
            obj.levelOneAccountID = Convert.ToInt64(FindLevelOne.AccountId);


            obj.LevelTwoName = FindLevelTwo.AccountName;
            obj.levelTwoNo = FindLevelTwo.AccountNo;
            obj.levelTwoAccountID = Convert.ToInt32(FindLevelTwo.AccountId);

            obj.LevelThreeName = FindLevelThree.AccountName;
            obj.levelThreeNo = FindLevelThree.AccountNo;
            obj.ParentID = Convert.ToInt64(FindLevelThree.AccountId);
            obj.AccountType= FindLevelThree.AccountName;

            var findfinancialBookYear = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == OrganizationID).FirstOrDefault();
            if (findfinancialBookYear != null)
            {
                ViewBag.FinancialBookYearsNo = findfinancialBookYear.YearCode;
            }
            else
            {
                TempData["Validation"] = "warning";
                TempData["ErrorMessage"] = "Please add financial book year befor you add vouchers.";
            }
            return View(obj);
        }

        // POST: CustomerAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(AccountsViewModel account)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
                var getmessage = chartAccountDAL.AddCustomerAccounts(account, UserID);
                if (getmessage != "")
                {
                    TempData["Validation"] = false;
                    TempData["ErrorMessage"] = getmessage;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Validation"] = true;
                    TempData["ErrorMessage"] = "Customer Accounts has been saved successfully";
                    return RedirectToAction("Index");
                }

            }
            return View(account);
        }

        // GET: CustomerAccounts/Edit/5
        public ActionResult Edit(long? id)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            AccountsViewModel findObj = ConvertDbModelToViewModel(account);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(findObj);
        }

        // POST: CustomerAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit(AccountsViewModel account)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
                var getmessage = chartAccountDAL.UpdateCustomerAccounts(account, UserID);
                if (getmessage != "")
                {
                    TempData["Validation"] = false;
                    TempData["ErrorMessage"] = getmessage;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Validation"] = true;
                    TempData["ErrorMessage"] = "Customer Accounts has been updated successfully";
                    return RedirectToAction("Index");
                }
            }

            return View(account);
        }

        // GET: CustomerAccounts/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }

            var getmessage = chartAccountDAL.DeleteAccount(account, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Account has been deleted successfully";
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
        //------------ Inner functions
        public AccountsViewModel ConvertDbModelToViewModel(Account account)
        {
            var FindLevelThree = db.Accounts.Where(a => a.AccountId == account.ParentID).FirstOrDefault();
            var FindLevelTwo = db.Accounts.Where(a => a.AccountId == FindLevelThree.ParentID).FirstOrDefault();
            var FindLevelOne = db.Accounts.Where(a => a.AccountId == FindLevelTwo.ParentID).FirstOrDefault();
            var findOpeningBalance = db.OpeningBalance.Where(a => a.AccountId == account.AccountId).FirstOrDefault();

            AccountsViewModel obj = new AccountsViewModel();
            obj.AccountId = account.AccountId;
            obj.OrganizationID = account.OrganizationID;
            obj.BranchId = account.BranchId;
            obj.BranchName = account.BranchDefinition.Name;
            obj.ParentID = account.ParentID;
            obj.AccountNo = account.AccountNo;
            obj.AccountName = account.AccountName;
            obj.AccountType = account.AccountType;
            obj.LevelID = account.LevelID;
            obj.HeadID = account.HeadID;
            obj.Description = account.Description;
            obj.CreatedBy = account.CreatedBy;
            obj.CreatedDate = account.CreatedDate;
            obj.UpdatedBy = account.UpdatedBy;
            obj.UpdatedDate = account.UpdatedDate;
            obj.IsSystemAccount = account.IsSystemAccount;
            obj.IsDeleted = account.IsDeleted;
            obj.IsActive = account.IsActive;
            obj.DeletedBy = account.DeletedBy;
            obj.DeletedDate = account.DeletedDate;
            obj.LevelThreeName = FindLevelThree.AccountName;
            obj.levelThreeNo = FindLevelThree.AccountNo;
            obj.levelThreeAccountID = FindLevelThree.AccountId;
            obj.LevelTwoName = FindLevelTwo.AccountName;
            obj.levelTwoNo = FindLevelTwo.AccountNo;
            obj.levelTwoAccountID = FindLevelTwo.AccountId;
            obj.LevelOneName = FindLevelOne.AccountName;
            obj.levelOneNo = FindLevelOne.AccountNo;
            obj.levelOneAccountID = FindLevelOne.AccountId;
            obj.OrganizationUnitName = account.OrganizationDefinition.OrganizationUnitName;
            if (findOpeningBalance != null)
            {
                obj.OpeningBalance = findOpeningBalance.TotalAmount;
            }

            return obj;
        }
        //------------------- Json Function
       
        public JsonResult LoadMaxAccountNo(string LevelOneId, string LevelTwoId, string LevelThreeId, string IDParent)
        {
            int idONe = 0;
            int idTwo = 0;
            int idThree = 0;
            long idParent = 0;
            int idLevel = 4;
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            if (!String.IsNullOrEmpty(LevelOneId) && !String.IsNullOrEmpty(LevelTwoId) && !String.IsNullOrEmpty(LevelThreeId) && !String.IsNullOrEmpty(IDParent))
            {
                idONe = Convert.ToInt32(LevelOneId);
                idTwo = Convert.ToInt32(LevelTwoId);
                idThree = Convert.ToInt32(LevelThreeId);
                idParent = Convert.ToInt32(IDParent);
            }
            var MaxAccount = db.ProcGetMaxAccountNo(OrganizationID, BranchId, idParent, idONe, idTwo, idThree, idLevel);
            return Json(MaxAccount, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            Account _searchObject = JsonConvert.DeserializeObject<Account>(searchModel);
            List<Account> _listObject = new List<Account>();
            _listObject = db.Accounts.Where(a => a.IsActive == true && a.IsDeleted == false && a.ParentID != null && a.LevelID == 4 && a.OrganizationID == OrganizationID).ToList();
            if (!String.IsNullOrEmpty(_searchObject.AccountNo) && !String.IsNullOrEmpty(_searchObject.AccountName))
            {
                _listObject = _listObject.Where(r => r.AccountNo != null && r.AccountNo.ToString().ToLower().Contains(_searchObject.AccountNo) || r.AccountName.ToString().ToLower().Contains(_searchObject.AccountName)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.AccountNo))
            {
                _listObject = _listObject.Where(r => r.AccountNo != null && r.AccountNo.ToString().ToLower().Contains(_searchObject.AccountNo)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.AccountName))
            {
                _listObject = _listObject.Where(r => r.AccountName != null && r.AccountName.ToString().ToLower().Contains(_searchObject.AccountName)).ToList();
            }
            return PartialView("_Search", _listObject);
        }
    }
}
