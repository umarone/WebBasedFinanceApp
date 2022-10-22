using MudasirRehmanAlp.AppDAL;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Infrastructure.AppServices;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MudasirRehmanAlp.AppDefinitions.BasicSettings
{
    public class JsonJobController : Controller
    {
        private AppEntities db = new AppEntities();
        SystemDAL systemDAL = new SystemDAL();
        AutoGenerateCodeDAL autoGenerateCodeDAl = new AutoGenerateCodeDAL();
        NotificationService _service = new NotificationService();
        // GET: JsonJob
        public ActionResult Index()
        {
            return View();
        }
        #region Notifications_Job_Region
        public JsonResult NotificationsAddJob()
        {
            try
            {
                bool isNotificationsJobRun = Convert.ToBoolean(Session["isNotificationsJobRun"]);
                if (isNotificationsJobRun == false)
                {
                    NotificationsGenralSettings();
                    NotificationsLowStock();
                    Session["isNotificationsJobRun"] = true;
                }


                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
                throw ex;
            }
        }
        public void NotificationsGenralSettings()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            long UserID = Convert.ToInt64(Session["UserID"]);
            List<Notification> _ObjList = new List<Notification>();
            var findSaleAccount = db.AccountMapping.Where(a => a.Account.IsActive == true && a.Account.IsDeleted == false && a.Account.OrganizationID == OrganizationID && a.BranchId == BranchId && a.AccountDefaultType == CommonEnums.AccountDefaultType.Sales).FirstOrDefault();
            var findPurcaseAccount = db.AccountMapping.Where(a => a.Account.IsActive == true && a.Account.IsDeleted == false && a.OrganizationId == OrganizationID && a.BranchId == BranchId && a.AccountDefaultType == CommonEnums.AccountDefaultType.Purchases).FirstOrDefault();
            var findfinancialBookYear = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == OrganizationID).FirstOrDefault();
            var findCurrency = db.CurrencyDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == OrganizationID).FirstOrDefault();
            var findGeneralSettings = db.GeneralSettings.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID).FirstOrDefault();
            if (findSaleAccount == null)
            {
                var findObj = db.Notifications.Where(a => a.OrganizationId == OrganizationID && a.BranchId == BranchId && a.IsRead == false && a.GeneralCode == CommonEnums.NotificationGeneralCode.SalesAccount.ToString()).FirstOrDefault();
                if (findObj == null)
                {
                    Notification Obj = new Notification();
                    Obj.OrganizationId = OrganizationID;
                    Obj.BranchId = BranchId;
                    Obj.Type = CommonEnums.NotificationType.General;
                    Obj.Priority = CommonEnums.NotificationPriority.High;
                    Obj.GeneralCode = CommonEnums.NotificationGeneralCode.SalesAccount.ToString();
                    Obj.Title = "Sale Account";
                    Obj.Description = "Please set one account for sales.";
                    Obj.IsRead = false;
                    Obj.IsDeleted = false;
                    Obj.CreatedBy = UserID;
                    Obj.CreatedDate = DateTime.Now;
                    _ObjList.Add(Obj);
                }

            }
            if (findPurcaseAccount == null)
            {
                var findObj = db.Notifications.Where(a => a.OrganizationId == OrganizationID && a.BranchId == BranchId && a.IsRead == false && a.GeneralCode == CommonEnums.NotificationGeneralCode.PurchasesAccount.ToString()).FirstOrDefault();
                if (findObj == null)
                {
                    Notification Obj = new Notification();
                    Obj.OrganizationId = OrganizationID;
                    Obj.BranchId = BranchId;
                    Obj.Type = CommonEnums.NotificationType.General;
                    Obj.Priority = CommonEnums.NotificationPriority.High;
                    Obj.GeneralCode = CommonEnums.NotificationGeneralCode.PurchasesAccount.ToString();
                    Obj.Title = "Purchase Account";
                    Obj.Description = "Please set one account for purchases.";
                    Obj.IsRead = false;
                    Obj.IsDeleted = false;
                    Obj.CreatedBy = UserID;
                    Obj.CreatedDate = DateTime.Now;
                    _ObjList.Add(Obj);
                }

            }
            if (findfinancialBookYear == null)
            {
                var findObj = db.Notifications.Where(a => a.OrganizationId == OrganizationID && a.BranchId == BranchId && a.IsRead == false && a.GeneralCode == CommonEnums.NotificationGeneralCode.FinancialYear.ToString()).FirstOrDefault();
                if (findObj == null)
                {
                    Notification Obj = new Notification();
                    Obj.OrganizationId = OrganizationID;
                    Obj.BranchId = BranchId;
                    Obj.Type = CommonEnums.NotificationType.General;
                    Obj.Priority = CommonEnums.NotificationPriority.High;
                    Obj.GeneralCode = CommonEnums.NotificationGeneralCode.FinancialYear.ToString();
                    Obj.Title = "Financial Book Year";
                    Obj.Description = "Please add financial book year befor you add vouchers.";
                    Obj.IsRead = false;
                    Obj.IsDeleted = false;
                    Obj.CreatedBy = UserID;
                    Obj.CreatedDate = DateTime.Now;
                    _ObjList.Add(Obj);
                }
            }
            if (findCurrency == null)
            {
                var findObj = db.Notifications.Where(a => a.OrganizationId == OrganizationID && a.BranchId == BranchId && a.IsRead == false && a.GeneralCode == CommonEnums.NotificationGeneralCode.Currency.ToString()).FirstOrDefault();
                if (findObj == null)
                {
                    Notification Obj = new Notification();
                    Obj.OrganizationId = OrganizationID;
                    Obj.BranchId = BranchId;
                    Obj.Type = CommonEnums.NotificationType.General;
                    Obj.Priority = CommonEnums.NotificationPriority.High;
                    Obj.GeneralCode = CommonEnums.NotificationGeneralCode.Currency.ToString();
                    Obj.Title = "Currency";
                    Obj.Description = "Please add currency and set is default.";
                    Obj.IsRead = false;
                    Obj.IsDeleted = false;
                    Obj.CreatedBy = UserID;
                    Obj.CreatedDate = DateTime.Now;
                    _ObjList.Add(Obj);
                }
            }
            if (findGeneralSettings == null || findGeneralSettings.StockMinimumQuantity == 0)
            {
                var findObj = db.Notifications.Where(a => a.OrganizationId == OrganizationID && a.BranchId == BranchId && a.IsRead == false && a.GeneralCode == CommonEnums.NotificationGeneralCode.GeneralStockMinQty.ToString()).FirstOrDefault();
                if (findObj == null)
                {
                    Notification Obj = new Notification();
                    Obj.OrganizationId = OrganizationID;
                    Obj.BranchId = BranchId;
                    Obj.Type = CommonEnums.NotificationType.General;
                    Obj.Priority = CommonEnums.NotificationPriority.High;
                    Obj.GeneralCode = CommonEnums.NotificationGeneralCode.GeneralStockMinQty.ToString();
                    Obj.Title = "Stock Minimum Quantity";
                    Obj.Description = "Please set stock minimum quantity in general settings.";
                    Obj.IsRead = false;
                    Obj.IsDeleted = false;
                    Obj.CreatedBy = UserID;
                    Obj.CreatedDate = DateTime.Now;
                    _ObjList.Add(Obj);
                }
            }

            if (_ObjList.Count > 0)
            {
                db.Notifications.AddRange(_ObjList);
                db.SaveChanges();
            }
        }
        public void NotificationsLowStock()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            long UserID = Convert.ToInt64(Session["UserID"]);
            List<Notification> _ObjList = new List<Notification>();
            var findGeneralSettings = db.GeneralSettings.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID).FirstOrDefault();
           
            if (findGeneralSettings != null || findGeneralSettings.StockMinimumQuantity > 0)
            {
                var _listOfLowStock = db.StockofGoods.Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId && a.Quantity < findGeneralSettings.StockMinimumQuantity).ToList();

                foreach (var item in _listOfLowStock)
                {
                    var findObj = db.Notifications.Where(a => a.OrganizationId == OrganizationID && a.BranchId == BranchId && a.IsRead == false && a.GeneralCode == CommonEnums.NotificationGeneralCode.LowStock.ToString()+ item.StockCode).FirstOrDefault();
                    if (findObj == null)
                    {
                        Notification Obj = new Notification();
                        Obj.OrganizationId = OrganizationID;
                        Obj.BranchId = BranchId;
                        Obj.Type = CommonEnums.NotificationType.General;
                        Obj.Priority = CommonEnums.NotificationPriority.High;
                        Obj.GeneralCode = CommonEnums.NotificationGeneralCode.LowStock.ToString()+ item.StockCode;
                        Obj.Title = "Low Stock";
                        Obj.Description ="This " + item.ProductDefinition.ProductBarCode+" "+ item.ProductDefinition.ProductName+" have low stock.";
                        Obj.IsRead = false;
                        Obj.IsDeleted = false;
                        Obj.CreatedBy = UserID;
                        Obj.CreatedDate = DateTime.Now;
                        _ObjList.Add(Obj);
                    }
                }
                
                
            }

            if (_ObjList.Count > 0)
            {
                db.Notifications.AddRange(_ObjList);
                db.SaveChanges();
            }
        }
        public JsonResult GetNotificationJob()
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<NotificationViewModel> dataList = new List<NotificationViewModel>();
            dataList = _service.GetNotificationsAC(OrganizationID, BranchId);
            return Json(dataList, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}