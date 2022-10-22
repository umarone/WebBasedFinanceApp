using MudasirRehmanAlp.AppDAL;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.ModelsView;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.AppDefinitions.BasicSettings
{
    public class JsonController : Controller
    {
        private AppEntities db = new AppEntities();
        SystemDAL systemDAL = new SystemDAL();
        AutoGenerateCodeDAL autoGenerateCodeDAl = new AutoGenerateCodeDAL();
        // GET: Json
        public ActionResult Index()
        {
            return View();
        }
        //-- Users Rights 
        public JsonResult getPagePermission(string ID)
        {
            long Id = 0;
            if (!String.IsNullOrEmpty(ID))
            {
                Id = Convert.ToInt64(ID);
            }
            var PagePermission = db.ProcGetPermissionByUserID(Id);
            return Json(PagePermission, JsonRequestBehavior.AllowGet);
        }

        // GET: Products List By organization 
        public ActionResult ProductList(string term, string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }
            var result = (from p in db.ProductDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)
                          where p.ProductName.ToString().Contains(term) || p.ProductBarCode.ToString().Contains(term)
                          where p.OrganizationID == id
                          select new
                          {
                              value = p.ProductId,
                              label = p.ProductBarCode + " - " + p.ProductName
                          }
                         ).Take(10).OrderBy(a => a.label).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult LoadProductList(string term)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            var result = (from p in db.ProductDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)
                          where p.ProductName.ToString().Contains(term) || p.ProductBarCode.ToString().Contains(term)
                          where p.OrganizationID == OrganizationID
                          select new
                          {
                              value = p.ProductId,
                              label = p.ProductBarCode + " - " + p.ProductName
                          }
                         ).Take(10).OrderBy(a => a.label).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        // GET: Amount In Word 
        public JsonResult AmountInWord(string label)
        {
            long number = 0;

            if (label != "")
            {

                decimal numberrs = Convert.ToDecimal(label);
                number = Convert.ToInt64(numberrs);
            }
            try
            {
                //string AmountInWord = SysObj.GenerateAmountInWord(label);
                string AmountInWord = systemDAL.NumberToWords(number);
                AmountInWord = AmountInWord + " Only";

                return Json(AmountInWord, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Amount in Word Conversion Failed", JsonRequestBehavior.AllowGet);
            }

        }

        // GET: Load Area by City 
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
        //--------
        // GET: Load AccountNo
        public JsonResult LoadAccounts(string term)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            var result = (from ac in db.Accounts.Where(a => a.IsActive == true && a.IsDeleted == false && a.ParentID != null && a.LevelID == 4)
                          where ac.AccountName.ToString().Contains(term) || ac.AccountNo.ToString().Contains(term)
                          where ac.OrganizationID == OrganizationID
                          select new
                          {
                              value = ac.AccountId,
                              label = ac.AccountNo + " - " + ac.AccountName,
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //--------
        // GET: Load PurchaseOrders For Good Received
        public JsonResult LoadPurchaseOrders(string term)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            var result = (from p in db.PurchaseOrders.Where(a => a.IsActive == true && a.IsDeleted == false)
                          join pd in db.PurchaseOrderDetails on p.PurchaseOrderId equals pd.PurchaseOrderID
                          where (pd.Active == true && pd.BalanceQuantity != 0) && (p.IsCompleted != true || p.IsCompleted == null) && p.OrganizationID == OrganizationID && p.BranchId == BranchId
                          select new JsonViewModel
                          {
                              value = p.PurchaseOrderId,
                              label = p.PurchaseOrderNO
                          }).Distinct().ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: Load Adjustment Notes For Good Received
        public JsonResult LoadAdjustmentNotes(string term)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            var result = (from p in db.AdjustmentNotes.Where(a => a.IsActive == true && a.IsDeleted == false)
                          join pd in db.AdjustmentNoteDetails on p.AdjustmentNoteId equals pd.AdjustmentNoteID
                          where (pd.Active == true && pd.BalanceQuantity != 0) && p.OrganizationID == OrganizationID && p.BranchId == BranchId
                          select new JsonViewModel
                          {
                              value = p.AdjustmentNoteId,
                              label = p.AdjustmentNoteNO
                          }).Distinct().ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: Load StoreTransfer Notes  For Good Received
        public JsonResult LoadStoreTransferNotes(string term)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            var result = (from p in db.StoreTransferNotes.Where(a => a.IsActive == true && a.IsDeleted == false)
                          join pd in db.StoreTransferNoteDetails on p.Id equals pd.STNId
                          where pd.BalanceQuantity != 0 && p.OrganizationID == OrganizationID && p.ToBranchId == BranchId
                          select new JsonViewModel
                          {
                              value = p.Id,
                              label = p.Code
                          }).OrderByDescending(a=>a.value).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: Load GoodReceiveds For Good Received Retrun
        public JsonResult LoadGoodReceiveds(string term)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            var result = (from p in db.GoodReceiveds.Where(a => a.IsActive == true && a.IsDeleted == false)
                          join pd in db.GoodReceivedDetails on p.GoodReceivedNoteId equals pd.GoodReceivedNoteID
                          where (pd.Active == true && pd.BalanceQuantity != 0) && p.OrganizationID == OrganizationID && p.BranchId == BranchId
                          select new JsonViewModel
                          {
                              value = p.GoodReceivedNoteId,
                              label = p.GRNNO
                          }).Distinct().ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: Load Good Received Returns For Purchase Return
        public JsonResult LoadGoodReceivedReturns(string term)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            var result = (from p in db.GoodReceivedReturns.Where(a => a.IsActive == true && a.IsDeleted == false)
                          join pd in db.GoodReceivedReturnDetails on p.GoodsReceivedReturnID equals pd.GoodsReceivedReturnID
                          where (pd.Active == true && pd.ReturnQuantity != 0) && p.OrganizationID == OrganizationID && p.BranchId == BranchId
                          select new JsonViewModel
                          {
                              value = p.GoodsReceivedReturnID,
                              label = p.GRReturnNO
                          }).Distinct().ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
       
        // GET: Load Closing Balance By Account For Vouchers
        public JsonResult loadClosingBalanceByAccount(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            long AccountId = 0;
            if (!String.IsNullOrEmpty(ID))
            {
                AccountId = Convert.ToInt64(ID);
            }
            AccountsViewModel jsonResult = new AccountsViewModel();

            var findfinancialBookYear = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == OrganizationID && a.BranchId == BranchId).FirstOrDefault();
            if (findfinancialBookYear != null)
            {
                var findSPObj = db.ProcGetClosingBalanceByAccount(OrganizationID, BranchId, AccountId, findfinancialBookYear.Id);
                jsonResult.AccountId = AccountId;
                if (findSPObj.ClosingBalance != null)
                {
                    jsonResult.ClosingBalance = findSPObj.ClosingBalance;
                }
                else
                {
                    jsonResult.ClosingBalance = 0;
                }
            }
            return Json(jsonResult, JsonRequestBehavior.AllowGet);

        }
        public AccountsViewModel GetCommonClosingBalanceByAccount(string ID)
        {
            AccountsViewModel jsonResult = new AccountsViewModel();
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            long AccountId = 0;
            if (!String.IsNullOrEmpty(ID))
            {
                AccountId = Convert.ToInt64(ID);
            }


            var findfinancialBookYear = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == OrganizationID && a.BranchId == BranchId).FirstOrDefault();
            if (findfinancialBookYear != null)
            {
                var findSPObj = db.ProcGetClosingBalanceByAccount(OrganizationID, BranchId, AccountId, findfinancialBookYear.Id);
                jsonResult.AccountId = AccountId;
                if (findSPObj.ClosingBalance != null)
                {
                    jsonResult.ClosingBalance = findSPObj.ClosingBalance;
                }
                else
                {
                    jsonResult.ClosingBalance = 0;
                }
            }
            return jsonResult;

        }
       
        //-- For Serach pages dropdowns
        public JsonResult LoadAllCountry(string ID)
        {
            var result = (from p in db.CountryDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)
                          select new
                          {
                              value = p.Id,
                              label = p.CountryName
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadAllProvince(string ID)
        {
            var result = (from p in db.ProvinceDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)
                          select new
                          {
                              value = p.Id,
                              label = p.ProvinceName
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadAllRegion(string ID)
        {
            var result = (from p in db.RegionDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)
                          select new
                          {
                              value = p.Id,
                              label = p.RegionName
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadAllCity(string ID)
        {
            var result = (from p in db.CityDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)
                          select new
                          {
                              value = p.CityId,
                              label = p.CityName
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //--- All Users Get For Super Admin
        public JsonResult LoadAllOrganizationForSuperAdmin(string ID)
        {

            var ResultList = (from a in db.OrganizationDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)

                              select new
                              {
                                  value = a.Id,
                                  label = a.OrganizationUnitCode + "-" + a.OrganizationUnitName,
                              }).OrderBy(a=>a.label).ToList();
            return Json(ResultList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadAllUsersForSuperAdmin(string ID)
        {
            long Id = 0;
            if (!String.IsNullOrEmpty(ID))
            {
                Id = Convert.ToInt64(ID);
            }
            var ResultList = (from a in db.Users.Where(a => a.IsActive == true && a.IsDeleted == false)
                              where a.OrganizationID == Id
                              select new
                              {
                                  value = a.UserId,
                                  label = a.EmployeePersonalDetails.EmployeeCode + "-" + a.FullName,
                              }).OrderBy(a=>a.label).ToList();
            return Json(ResultList, JsonRequestBehavior.AllowGet);
        }

        //-------- This Code For Sale Setup----------
        //------- Load DistributorDefinitions
        public JsonResult LoadDistributorDefinitionsByOrgId(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);

            var result = (from a in db.DistributorDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)
                          where a.OrganizationID == OrganizationID
                          select new JsonViewModel
                          {
                              value = a.Id,
                              label = a.Account.AccountNo + " - " + a.DistributorName
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //------- Load CustomerStatements
        public JsonResult LoadCustomerStatementsByOrgId(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            var result = (from a in db.CustomerStatements.Where(a => a.IsActive == true && a.IsDeleted == false)
                          where a.OrganizationID == OrganizationID && a.BranchId == BranchId
                          select new JsonViewModel
                          {
                              value = a.Id,
                              label = a.Account.AccountNo + " - " + a.Name
                          }
                          ).OrderBy(a=>a.label).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //------- Load CustomerStatements
        public JsonResult LoadCustomerStatementsForAccounts(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);

            var result = (from a in db.CustomerStatements.Where(a => a.IsActive == true && a.IsDeleted == false && a.AccountID == null)
                          where a.OrganizationID == OrganizationID && a.BranchId == BranchId
                          select new JsonViewModel
                          {
                              value = a.Id,
                              label = a.Code + " - " + a.Name
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadProducts(string term)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            var result = (from p in db.ProductDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)
                          where p.ProductName.ToString().Contains(term) || p.ProductBarCode.ToString().Contains(term) || p.ProductModel.Name.ToString().Contains(term) || p.Colour.Name.ToString().Contains(term) || p.SizeDefinition.Name.ToString().Contains(term)
                          where p.OrganizationID == OrganizationID 
                          select new
                          {
                              value = p.ProductId,
                              label = p.ProductBarCode + " " + p.ProductName + " " + p.ProductModel.Name + " " + p.Colour.Name + " " + p.SizeDefinition.Name
                          }
                         ).OrderByDescending(a=>a.label).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ProductLoadFromStock(string term)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            var result = (from p in db.StockofGoods.Where(a => a.Quantity > 0)
                          where p.ProductDefinition.ProductName.ToString().Contains(term) || p.ProductDefinition.ProductBarCode.ToString().Contains(term) || p.ProductDefinition.ProductModel.Name.ToString().Contains(term) || p.ProductDefinition.Colour.Name.ToString().Contains(term) || p.ProductDefinition.SizeDefinition.Name.ToString().Contains(term)
                          where p.OrganizationID == OrganizationID && p.BranchId == BranchId
                          select new
                          {
                              value = p.ProductID,
                              label = p.ProductDefinition.ProductBarCode + " " + p.ProductDefinition.ProductName + " " + p.ProductDefinition.ProductModel.Name + " " + p.ProductDefinition.Colour.Name + " " + p.ProductDefinition.SizeDefinition.Name
                          }
                         ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        // GET: Load Product For Stock
        public JsonResult LoadProductFromStock(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            var result = (from a in db.StockofGoods.Where(a => a.Active == true && a.Quantity > 0)

                          where a.OrganizationID == OrganizationID && a.BranchId == BranchId
                          select new JsonViewModel
                          {
                              value = a.ProductDefinition.ProductId,
                              label = a.ProductDefinition.ProductBarCode + " " + a.ProductDefinition.ProductName + " " + a.ProductDefinition.ProductModel.Name + " " + a.ProductDefinition.Colour.Name + " " + a.ProductDefinition.SizeDefinition.Name
                          }
                          ).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadEmployeeSalePersonCode(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            //var result = (from a in db.EmployeePersonalDetails.Where(a => a.IsActive == true && a.IsDeleted == false)
            //              join o in db.EmployeeOfficiallDetails on a.EmployeeId equals o.EmployeeId
            //              join dp in db.DepartmentDefinitions on o.DepartmentId equals dp.Id
            //              where a.OrganizationID == OrganizationID && a.BranchId == BranchId && dp.Type == DepartmentType.Sale
            //              select new JsonViewModel
            //              {
            //                  value = a.EmployeeId,
            //                  label = a.EmployeeCode + " - " + a.FirstName
            //              }
            //              ).OrderBy(a=>a.label).ToList();

            var result = (from a in db.EmployeePersonalDetails.Where(a => a.IsActive == true && a.IsDeleted == false)
                          join o in db.EmployeeOfficiallDetails on a.EmployeeId equals o.EmployeeId
                          join dp in db.DepartmentDefinitions on o.DepartmentId equals dp.Id
                          join br in db.BranchesRights on o.EmployeeId equals br.EmployeeId
                          where a.OrganizationID == OrganizationID && br.BranchId.ToString().Contains(BranchId.ToString()) && dp.Type == DepartmentType.Sale
                          select new JsonViewModel
                          {
                              value = a.EmployeeId,
                              label = a.EmployeeCode + " - " + a.FirstName
                          }
                         ).Distinct().OrderBy(a => a.label).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //------- Load CurrencyDefinitions
        public JsonResult LoadCurrencyDefinitionsByOrgId(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);

            var result = (from a in db.CurrencyDefinitions.Where(a => a.IsActive == true)
                          where a.OrganizationID == OrganizationID 
                          select new JsonViewModel
                          {
                              value = a.CurrencyID,
                              label = a.CurrencySymbol + " - " + a.CurrencyName
                          }).OrderByDescending(a=>a.label).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //------- Load SupplierDefinitions
        public JsonResult LoadSupplierDefinitions(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);

            var result = (from a in db.SupplierDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)
                          where a.OrganizationID == OrganizationID 
                          select new JsonViewModel
                          {
                              value = a.Id,
                              label = a.Account.AccountNo + " - " + a.SupplierName
                          }).OrderByDescending(a=>a.label).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //------- Load SaleOrderNo For Sale Invoice
        public JsonResult LoadSaleOrder(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            var result = (from p in db.SaleOrders.Where(a => a.IsActive == true && a.IsDeleted == false)
                          join pd in db.SaleOrderDetails on p.SaleOrdeID equals pd.SaleOrderID
                          where (pd.Active == true && pd.BalanceQuantity != 0) && p.OrganizationID == OrganizationID
                          select new JsonViewModel
                          {
                              value = p.SaleOrdeID,
                              label = p.SaleOrderNo
                          }).Distinct().ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //------- Load Load Employee Code For PayRoll Setup
        public JsonResult LoadEmployeeCode(string ID)
        {
            JsonViewModel jsonView = new JsonViewModel();
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            jsonView.code = autoGenerateCodeDAl.AutoGenerateEmployeeCode(OrganizationID);
            return Json(jsonView, JsonRequestBehavior.AllowGet);
        }
        //------- Load Department Definitions For PayRoll Setup
        public JsonResult LoadDepartmentDefinitionsCode(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);

            var result = (from a in db.DepartmentDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)
                          where a.OrganizationID == OrganizationID
                          select new JsonViewModel
                          {
                              value = a.Id,
                              label = a.Code + " - " + a.Name
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //------- Load Department Definitions For PayRoll Setup
        public JsonResult LoadDesignationDefinitionsCode(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);

            var result = (from a in db.DesignationDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)
                          where a.OrganizationID == OrganizationID
                          select new JsonViewModel
                          {
                              value = a.Id,
                              label = a.Code + " - " + a.Name
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //------- Load EducationInstitutes For PayRoll Setup
        public JsonResult LoadEducationInstitutesCode(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);

            var result = (from a in db.EducationInstitutes.Where(a => a.IsActive == true && a.IsDeleted == false)
                          where a.OrganizationID == OrganizationID
                          select new JsonViewModel
                          {
                              value = a.Id,
                              label = a.Code + " - " + a.Name
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //------- Load LoadEducationDegreesCode For PayRoll Setup
        public JsonResult LoadEducationDegreesCode(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);

            var result = (from a in db.EducationDegrees.Where(a => a.IsActive == true && a.IsDeleted == false)
                          where a.OrganizationID == OrganizationID
                          select new JsonViewModel
                          {
                              value = a.Id,
                              label = a.Code + " - " + a.Name
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //-------- Enums Load For Select Dropdown----------
        public JsonResult LoadStockType(string ID)
        {
            var result = from StockType e in Enum.GetValues(typeof(StockType))
                         select new JsonViewModel
                         {
                             value = (int)e,
                             label = e.ToDisplayName()
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadDepartmentType(string ID)
        {
            var result = from DepartmentType e in Enum.GetValues(typeof(DepartmentType))
                         select new JsonViewModel
                         {
                             value = (int)e,
                             label = e.ToDisplayName()
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadSaleOrderCustomerType(string ID)
        {
            var result = from SaleOrderCustomerType e in Enum.GetValues(typeof(SaleOrderCustomerType))
                         select new JsonViewModel
                         {
                             value = (int)e,
                             label = e.ToDisplayName()
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //----- Chart of Account and sale Order Note For All Details
        public JsonResult LoadAccountLevelOne(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            var result = (from a in db.Accounts.Where(a => a.LevelID == 1 && a.OrganizationID == OrganizationID)
                          select new JsonViewModel
                          {
                              value = a.AccountId,
                              label = a.AccountName,

                          }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadTransactionType(string ID)
        {
            var result = from TransactionType e in Enum.GetValues(typeof(TransactionType))
                         select new JsonViewModel
                         {
                             value = (int)e,
                             label = e.ToDisplayName()
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadPuchaseOrderType(string ID)
        {
            var result = from PuchaseOrderType e in Enum.GetValues(typeof(PuchaseOrderType))
                         select new JsonViewModel
                         {
                             value = (int)e,
                             label = e.ToDisplayName()
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //---------------------- 
        public JsonResult LoadCategoryDefinitionsCode(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);

            var result = (from a in db.CategoryDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)
                          where a.OrganizationID == OrganizationID 
                          select new JsonViewModel
                          {
                              value = a.Id,
                              label = a.CategoryName
                          }
                          ).OrderByDescending(a=>a.label).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadManufactureDefinitionsCode(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);

            var result = (from a in db.ManufactureDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)
                          where a.OrganizationID == OrganizationID && a.BranchId == BranchId
                          select new JsonViewModel
                          {
                              value = a.ManufactureId,
                              label = a.ManufactureName
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadManufactureByCategory(string ID)
        {
            long id = 0;
            if (!String.IsNullOrEmpty(ID))
            {
                id = Convert.ToInt64(ID);
            }
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);

            var result = (from a in db.ManufactureDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false && a.CategoryId == id)
                          where a.OrganizationID == OrganizationID
                          select new JsonViewModel
                          {
                              value = a.ManufactureId,
                              label = a.ManufactureName
                          }).OrderByDescending(a=>a.label).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadColourCode(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);

            var result = (from a in db.Colours.Where(a => a.IsActive == true && a.IsDeleted == false)
                          where a.OrganizationId == OrganizationID
                          select new JsonViewModel
                          {
                              value = a.Id,
                              label = a.Name
                          }).OrderByDescending(a=>a.label).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadPaymentAgingsCode(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);

            var result = (from a in db.PaymentAgings.Where(a => a.IsActive == true && a.IsDeleted == false)
                          where a.OrganizationId == OrganizationID
                          select new JsonViewModel
                          {
                              value = a.Id,
                              label = a.Name
                          }
                          ).OrderByDescending(a=>a.label).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadProductModelsCode(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);

            var result = (from a in db.ProductModels.Where(a => a.IsActive == true && a.IsDeleted == false)
                          where a.OrganizationId == OrganizationID && a.BranchId == BranchId
                          select new JsonViewModel
                          {
                              value = a.Id,
                              label = a.Name
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadProductModelsByManufacture(string ID)
        {
            long Id = 0;
            if (!String.IsNullOrEmpty(ID))
            {
                Id = Convert.ToInt64(ID);
            }

            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);


            var result = (from a in db.ProductModels.Where(a => a.IsActive == true && a.IsDeleted == false)
                          where a.OrganizationId == OrganizationID && a.ManufactureId == Id
                          select new JsonViewModel
                          {
                              value = a.Id,
                              label = a.Name
                          }).OrderByDescending(a=>a.label).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadSizeDefinitionsCode(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);

            var result = (from a in db.SizeDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)
                          where a.OrganizationId == OrganizationID 
                          select new JsonViewModel
                          {
                              value = a.Id,
                              label = a.Name
                          }).OrderByDescending(a=>a.label).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadChartOfAccounts(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);

            var result = (from a in db.Accounts.Where(a => a.IsActive == true && a.IsDeleted == false && a.ParentID != null && a.LevelID == 4 && a.IsCustomer==false)
                          where a.OrganizationID == OrganizationID 
                          select new JsonViewModel
                          {
                              value = a.AccountId,
                              label = a.AccountNo + " - " + a.AccountName,
                          }).OrderBy(a=>a.label).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadOwnershipType(string ID)
        {
            var result = from OwnershipType e in Enum.GetValues(typeof(OwnershipType))
                         select new JsonViewModel
                         {
                             value = (int)e,
                             label = e.ToDisplayName()
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        
        public JsonResult LoadBranchesRightsSTN(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long EmployeeId = Convert.ToInt64(Session["EmployeeId"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            var result = (from a in db.BranchesRights.Where(a => a.IsActive == true && a.IsDeleted == false)
                          where a.OrganizationId == OrganizationID && a.EmployeeId == EmployeeId && a.BranchId != BranchId
                          select new JsonViewModel
                          {
                              value = a.BranchDefinition.Id,
                              label = a.BranchDefinition.Name
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadBranchsForAccounts(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long EmployeeId = Convert.ToInt64(Session["EmployeeId"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            var result = (from a in db.BranchesRights.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsSelected==true)
                          where a.OrganizationId == OrganizationID && a.EmployeeId == EmployeeId
                          select new JsonViewModel
                          {
                              value = a.BranchDefinition.Id,
                              label = a.BranchDefinition.Name
                          }).OrderBy(a=>a.label).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadAccountDefaultAs(string ID)
        {
            var result = from AccountDefaultType e in Enum.GetValues(typeof(AccountDefaultType))
                         select new JsonViewModel
                         {
                             value = (int)e,
                             label = e.ToDisplayName()
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BranchsDetailSetAsDefault(string ID)
        {
            long id = 0;
            if (!String.IsNullOrEmpty(ID))
            {
                id = Convert.ToInt64(ID);
            }
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            var result = db.Accounts.Where(a => a.OrganizationID == OrganizationID && a.AccountId == id).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadProductFromStockofGoods(string term)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);

            var result = (from a in db.StockofGoods.Where(a => a.Active == true && a.Quantity > 0)
                          where a.ProductDefinition.ProductName.ToString().Contains(term)
                          where a.OrganizationID == OrganizationID && a.BranchId == BranchId
                          select new JsonViewModel
                          {
                              value = a.StockID,
                              label = a.ProductDefinition.ProductBarCode + " " + a.ProductDefinition.ProductName + " " + a.ProductDefinition.ProductModel.Name + " " + a.ProductDefinition.Colour.Name + " " + a.ProductDefinition.SizeDefinition.Name
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        // GET: Load SaleOrder For Vouchers
        public JsonResult LoadSaleOrders(string term)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            var result = (from p in db.SaleOrders.Where(a => a.IsActive == true && a.IsDeleted == false)

                          where p.OrganizationID == OrganizationID && p.BranchId == BranchId
                          select new JsonViewModel
                          {
                              value = p.SaleOrdeID,
                              label = p.SaleOrderNo
                          }).Distinct().OrderByDescending(a=>a.value).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: Load Sale Order Accounts Details For Vouchers
        public JsonResult loadSaleOrderAccountsDetails(string ID)
        {
            AccountsViewModel jsonResult = new AccountsViewModel();

            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            long id = 0;
            if (!String.IsNullOrEmpty(ID))
            {
                id = Convert.ToInt64(ID);
            }
            var result = db.SaleOrders.Where(a => a.SaleOrdeID == id).FirstOrDefault();
            var findfinancialBookYear = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == OrganizationID).FirstOrDefault();
            if (result != null && findfinancialBookYear != null)
            {
                var findSPObj = db.ProcGetClosingBalanceByAccount(OrganizationID, BranchId, result.AccountID, findfinancialBookYear.Id);
                jsonResult.AccountId = Convert.ToInt64(result.AccountID);
                jsonResult.AccountNoAndAccountName = result.Account.AccountNo + " - " + result.Account.AccountName;
                jsonResult.NetTotal = result.NetTotal;
                if (findSPObj.ClosingBalance != null)
                {
                    jsonResult.ClosingBalance = findSPObj.ClosingBalance;
                }
                else
                {
                    jsonResult.ClosingBalance = 0;
                }
            }

            return Json(jsonResult, JsonRequestBehavior.AllowGet);

        }
        // GET: Load Puchase Order For Vouchers
        public JsonResult LoadPurchaseOrdersForVouchers(string term)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            var result = (from p in db.PurchaseOrders.Where(a => a.IsActive == true && a.IsDeleted == false)
                        
                          where p.IsCompleted == true && p.OrganizationID == OrganizationID && p.BranchId == BranchId
                          select new JsonViewModel
                          {
                              value = p.PurchaseOrderId,
                              label = p.PurchaseOrderNO
                          }).Distinct().OrderByDescending(a=>a.value).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: Load Purchase Order Accounts Details For Vouchers
        public JsonResult loadPurchaseOrderAccountsDetails(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            long id = 0;
            if (!String.IsNullOrEmpty(ID))
            {
                id = Convert.ToInt64(ID);
            }


            AccountsViewModel jsonResult = new AccountsViewModel();

            var result = db.PurchaseOrders.Where(a => a.IsActive == true && a.IsDeleted == false && a.PurchaseOrderId==id).FirstOrDefault();
                          
            var findfinancialBookYear = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == OrganizationID && a.BranchId == BranchId).FirstOrDefault();
            if (result != null && findfinancialBookYear != null)
            {
                var findSPObj = db.ProcGetClosingBalanceByAccount(OrganizationID, BranchId, result.AccountID, findfinancialBookYear.Id);
                jsonResult.AccountId =Convert.ToInt64(result.AccountID);
                jsonResult.AccountNoAndAccountName = result.Account.AccountNo+" - "+ result.Account.AccountName;
                jsonResult.NetTotal = result.NetTotal;
                if (findSPObj.ClosingBalance != null)
                {
                    jsonResult.ClosingBalance = findSPObj.ClosingBalance;
                }
                else
                {
                    jsonResult.ClosingBalance = 0;
                }
            }

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        #region Dashboradjson_Region
        public JsonResult LoadCountsOfRecordsForCommonDashboard(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            DashboardSearchViewModel _searchObject = JsonConvert.DeserializeObject<DashboardSearchViewModel>(searchModel);
            CommonDashboardJsonViewModel jsonViewModel = new CommonDashboardJsonViewModel();
            if (_searchObject.DashboardReportType== DashboardReportType.Today)
            {
                var StartDate = DateTime.Now;
                jsonViewModel.SaleOrderTotalCount = db.SaleOrders.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && DbFunctions.TruncateTime(a.CreatedDate) == DbFunctions.TruncateTime(StartDate)).ToList().Count();
                jsonViewModel.PurchaseOrderTotalCount = db.PurchaseOrders.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && DbFunctions.TruncateTime(a.CreatedDate) == DbFunctions.TruncateTime(StartDate)).ToList().Count();
                jsonViewModel.CustomerTotalCount = db.CustomerStatements.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && DbFunctions.TruncateTime(a.CreatedDate) == DbFunctions.TruncateTime(StartDate)).ToList().Count();

               var SumOfIncome = db.VouchersHeads.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && a.PaymentType == PaymentType.SaleOrder && a.Posted == true && DbFunctions.TruncateTime(a.CreatedDate) == DbFunctions.TruncateTime(StartDate)).ToList().Sum(a => a.TotalAmount);
               var SumOfExpense = db.VouchersHeads.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && a.PaymentType == PaymentType.PurchaseOrder && a.Posted == true && DbFunctions.TruncateTime(a.CreatedDate) == DbFunctions.TruncateTime(StartDate)).ToList().Sum(a => a.TotalAmount);
                jsonViewModel.SumOfIncome =Convert.ToDecimal(SumOfIncome);
                jsonViewModel.SumOfExpense = Convert.ToDecimal(SumOfExpense);
                jsonViewModel.SaleVoucherTotalCount = db.VouchersHeads.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && a.PaymentType == PaymentType.SaleOrder && (a.VoucherType == VoucherType.CRV || a.VoucherType == VoucherType.BRV || a.VoucherType == VoucherType.JV) && a.Posted == true && DbFunctions.TruncateTime(a.CreatedDate) == DbFunctions.TruncateTime(StartDate)).ToList().Count();
                jsonViewModel.PurchaseVoucherTotalCount = db.VouchersHeads.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && a.PaymentType == PaymentType.PurchaseOrder && (a.VoucherType == VoucherType.CPV || a.VoucherType == VoucherType.BPV || a.VoucherType == VoucherType.JV) && a.Posted == true && DbFunctions.TruncateTime(a.CreatedDate) == DbFunctions.TruncateTime(StartDate)).ToList().Count(); ;

            }
           else if (_searchObject.DashboardReportType == DashboardReportType.Weekly)
            {
                var StartDate = DateTime.Now.AddDays(-7);
                var EndDate = DateTime.Now;

                jsonViewModel.SaleOrderTotalCount = db.SaleOrders.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && DbFunctions.TruncateTime(a.CreatedDate) >= DbFunctions.TruncateTime(StartDate) && DbFunctions.TruncateTime(a.CreatedDate) <= DbFunctions.TruncateTime(EndDate)).ToList().Count();
                jsonViewModel.PurchaseOrderTotalCount = db.PurchaseOrders.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && DbFunctions.TruncateTime(a.CreatedDate) >= DbFunctions.TruncateTime(StartDate) && DbFunctions.TruncateTime(a.CreatedDate) <= DbFunctions.TruncateTime(EndDate)).ToList().Count();
                jsonViewModel.CustomerTotalCount = db.CustomerStatements.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && DbFunctions.TruncateTime(a.CreatedDate) >= DbFunctions.TruncateTime(StartDate) && DbFunctions.TruncateTime(a.CreatedDate) <= DbFunctions.TruncateTime(EndDate)).ToList().Count();

                var SumOfIncome = db.VouchersHeads.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && a.PaymentType == PaymentType.SaleOrder && a.Posted == true && DbFunctions.TruncateTime(a.CreatedDate) >= DbFunctions.TruncateTime(StartDate) && DbFunctions.TruncateTime(a.CreatedDate) <= DbFunctions.TruncateTime(EndDate)).ToList().Sum(a => a.TotalAmount);
                var SumOfExpense = db.VouchersHeads.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && a.PaymentType == PaymentType.PurchaseOrder && a.Posted == true && DbFunctions.TruncateTime(a.CreatedDate) >= DbFunctions.TruncateTime(StartDate) && DbFunctions.TruncateTime(a.CreatedDate) <= DbFunctions.TruncateTime(EndDate)).ToList().Sum(a => a.TotalAmount);
                jsonViewModel.SumOfIncome = Convert.ToDecimal(SumOfIncome);
                jsonViewModel.SumOfExpense = Convert.ToDecimal(SumOfExpense);
                jsonViewModel.SaleVoucherTotalCount = db.VouchersHeads.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && a.PaymentType == PaymentType.SaleOrder && (a.VoucherType == VoucherType.CRV || a.VoucherType == VoucherType.BRV || a.VoucherType == VoucherType.JV) && a.Posted == true && DbFunctions.TruncateTime(a.CreatedDate) >= DbFunctions.TruncateTime(StartDate) && DbFunctions.TruncateTime(a.CreatedDate) <= DbFunctions.TruncateTime(EndDate)).ToList().Count();
                jsonViewModel.PurchaseVoucherTotalCount = db.VouchersHeads.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && a.PaymentType == PaymentType.PurchaseOrder && (a.VoucherType == VoucherType.CPV || a.VoucherType == VoucherType.BPV || a.VoucherType == VoucherType.JV) && a.Posted == true && DbFunctions.TruncateTime(a.CreatedDate) >= DbFunctions.TruncateTime(StartDate) && DbFunctions.TruncateTime(a.CreatedDate) <= DbFunctions.TruncateTime(EndDate)).ToList().Count(); ;

            }
            else if (_searchObject.DashboardReportType == DashboardReportType.Monthly)
            {
                var StartDate = DateTime.Now.AddMonths(-1);
                var EndDate = DateTime.Now;

                jsonViewModel.SaleOrderTotalCount = db.SaleOrders.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && DbFunctions.TruncateTime(a.CreatedDate) >= DbFunctions.TruncateTime(StartDate) && DbFunctions.TruncateTime(a.CreatedDate) <= DbFunctions.TruncateTime(EndDate)).ToList().Count();
                jsonViewModel.PurchaseOrderTotalCount = db.PurchaseOrders.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && DbFunctions.TruncateTime(a.CreatedDate) >= DbFunctions.TruncateTime(StartDate) && DbFunctions.TruncateTime(a.CreatedDate) <= DbFunctions.TruncateTime(EndDate)).ToList().Count();
                jsonViewModel.CustomerTotalCount = db.CustomerStatements.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && DbFunctions.TruncateTime(a.CreatedDate) >= DbFunctions.TruncateTime(StartDate) && DbFunctions.TruncateTime(a.CreatedDate) <= DbFunctions.TruncateTime(EndDate)).ToList().Count();

                var SumOfIncome = db.VouchersHeads.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && a.PaymentType == PaymentType.SaleOrder && a.Posted == true && DbFunctions.TruncateTime(a.CreatedDate) >= DbFunctions.TruncateTime(StartDate) && DbFunctions.TruncateTime(a.CreatedDate) <= DbFunctions.TruncateTime(EndDate)).ToList().Sum(a => a.TotalAmount);
                var SumOfExpense = db.VouchersHeads.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && a.PaymentType == PaymentType.PurchaseOrder && a.Posted == true && DbFunctions.TruncateTime(a.CreatedDate) >= DbFunctions.TruncateTime(StartDate) && DbFunctions.TruncateTime(a.CreatedDate) <= DbFunctions.TruncateTime(EndDate)).ToList().Sum(a => a.TotalAmount);
                jsonViewModel.SumOfIncome = Convert.ToDecimal(SumOfIncome);
                jsonViewModel.SumOfExpense = Convert.ToDecimal(SumOfExpense);
                jsonViewModel.SaleVoucherTotalCount = db.VouchersHeads.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && a.PaymentType == PaymentType.SaleOrder && (a.VoucherType == VoucherType.CRV || a.VoucherType == VoucherType.BRV || a.VoucherType == VoucherType.JV) && a.Posted == true && DbFunctions.TruncateTime(a.CreatedDate) >= DbFunctions.TruncateTime(StartDate) && DbFunctions.TruncateTime(a.CreatedDate) <= DbFunctions.TruncateTime(EndDate)).ToList().Count();
                jsonViewModel.PurchaseVoucherTotalCount = db.VouchersHeads.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && a.PaymentType == PaymentType.PurchaseOrder && (a.VoucherType == VoucherType.CPV || a.VoucherType == VoucherType.BPV || a.VoucherType == VoucherType.JV) && a.Posted == true && DbFunctions.TruncateTime(a.CreatedDate) >= DbFunctions.TruncateTime(StartDate) && DbFunctions.TruncateTime(a.CreatedDate) <= DbFunctions.TruncateTime(EndDate)).ToList().Count(); ;

            }
            return Json(jsonViewModel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Notification_Region
        public JsonResult LoadNotificationType(string ID)
        {
            var result = from NotificationType e in Enum.GetValues(typeof(NotificationType))
                         select new JsonViewModel
                         {
                             value = (int)e,
                             label = e.ToDisplayName()
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadNotificationPriority(string ID)
        {
            var result = from NotificationPriority e in Enum.GetValues(typeof(NotificationPriority))
                         select new JsonViewModel
                         {
                             value = (int)e,
                             label = e.ToDisplayName()
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region IncomeStatement_Region
        public JsonResult LoadFinancialBookYearForReport(string ID)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);

            var result = (from a in db.FinancialBookYears.Where(a => a.IsActive == true)
                          where a.OrganizationID == OrganizationID
                          select new JsonViewModel
                          {
                              value = a.Id,
                              label = a.YearCode + " - " + a.YearName
                          }).OrderByDescending(a => a.label).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}