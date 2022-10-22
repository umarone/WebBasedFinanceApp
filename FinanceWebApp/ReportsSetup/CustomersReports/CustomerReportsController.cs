using MudasirRehmanAlp.AppDAL;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.ReportsSetup.CustomersReports
{
    public class CustomerReportsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        ExcelPackageDAL excelPackage = new ExcelPackageDAL();
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        // GET: CustomerReports
        public ActionResult Index()
        {
            return View();
        }

        #region CustomerLedger_Region
        public ActionResult CustomerLedger()
        {
            return View();
        }
        public ActionResult CustomerLedgerSearch(string searchModel)
        {
            try
            {
                long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
                long BranchId = Convert.ToInt64(Session["BranchId"]);
                VouchersSearchViewModel _searchObject = JsonConvert.DeserializeObject<VouchersSearchViewModel>(searchModel);

                if (_searchObject.StartDate == null && _searchObject.EndDate == null)
                {
                    _searchObject.StartDate = DateTime.Now.AddMonths(-1);
                    _searchObject.EndDate = DateTime.Now;
                }

                List<VouchersHead> _listObject = new List<VouchersHead>();
                Expression<Func<VouchersHead, bool>> WhereClouse = null;

                WhereClouse = x => x.VoucherType.ToString().Contains(VoucherType.CRV.ToString())
                || x.VoucherType.ToString().Contains(VoucherType.BRV.ToString())
                || x.VoucherType.ToString().Contains(VoucherType.JV.ToString())
                ;

                if (_searchObject.StartDate != null && _searchObject.EndDate != null)
                {
                    _listObject = db.VouchersHeads.Where(WhereClouse).Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId).Where(a => DbFunctions.TruncateTime(a.CreatedDate) >= DbFunctions.TruncateTime(_searchObject.StartDate) && DbFunctions.TruncateTime(a.CreatedDate) <= DbFunctions.TruncateTime(_searchObject.EndDate)).ToList();
                }
                else
                {
                    _listObject = db.VouchersHeads.Where(WhereClouse).Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId).ToList();
                }
                return PartialView("_SearchLedger", _listObject);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ActionResult CustomerLedgerDownloadExcel(DownloadFilesViewModel viewModel)
        {
            try
            {
                byte[] excelReportBytes;
                long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
                long BranchId = Convert.ToInt64(Session["BranchId"]);
                var BrancheObj = dALCode.GetBranchDefinition(BranchId);
                VouchersSearchViewModel _searchObject = JsonConvert.DeserializeObject<VouchersSearchViewModel>(viewModel.SearchValues);

                if (_searchObject.StartDate == null && _searchObject.EndDate == null)
                {
                    _searchObject.StartDate = DateTime.Now.AddMonths(-1);
                    _searchObject.EndDate = DateTime.Now;
                }

                List<VouchersHead> _listObject = new List<VouchersHead>();
                Expression<Func<VouchersHead, bool>> WhereClouse = null;

                WhereClouse = x => x.VoucherType.ToString().Contains(VoucherType.CRV.ToString())
                || x.VoucherType.ToString().Contains(VoucherType.BRV.ToString())
                || x.VoucherType.ToString().Contains(VoucherType.JV.ToString())
                ;

                if (_searchObject.StartDate != null && _searchObject.EndDate != null)
                {
                    _listObject = db.VouchersHeads.Where(WhereClouse).Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId).Where(a => DbFunctions.TruncateTime(a.CreatedDate) >= DbFunctions.TruncateTime(_searchObject.StartDate) && DbFunctions.TruncateTime(a.CreatedDate) <= DbFunctions.TruncateTime(_searchObject.EndDate)).ToList();
                }
                else
                {
                    _listObject = db.VouchersHeads.Where(WhereClouse).Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId).ToList();
                }

                using (var package = excelPackage.CustomerLedger_CreateExcelPackage(_listObject, BrancheObj))
                {
                    excelReportBytes = package.GetAsByteArray();
                }
                return File(excelReportBytes, XlsxContentType, "CustomerLedger_"+DateTime.Now.Ticks+".xlsx");

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
       
        #endregion
        #region CashReceiptRecovery_Region
        public ActionResult CashReceiptRecovery()
        {
            return View();
        }
        public ActionResult CashReceiptRecoverySearch(string searchModel)
        {
            try
            {
                long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
                long BranchId = Convert.ToInt64(Session["BranchId"]);
                VouchersSearchViewModel _searchObject = JsonConvert.DeserializeObject<VouchersSearchViewModel>(searchModel);

                if (_searchObject.StartDate == null && _searchObject.EndDate == null)
                {
                    _searchObject.StartDate = DateTime.Now.AddMonths(-1);
                    _searchObject.EndDate = DateTime.Now;
                }

                List<VouchersHead> _listObject = new List<VouchersHead>();
                Expression<Func<VouchersHead, bool>> WhereClouse = null;

                WhereClouse = x => x.VoucherType.ToString().Contains(VoucherType.CRV.ToString())
                || x.VoucherType.ToString().Contains(VoucherType.BRV.ToString())
                || x.VoucherType.ToString().Contains(VoucherType.JV.ToString())
                ;

                if (_searchObject.StartDate != null && _searchObject.EndDate != null)
                {
                    _listObject = db.VouchersHeads.Where(WhereClouse).Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId).Where(a => DbFunctions.TruncateTime(a.CreatedDate) >= DbFunctions.TruncateTime(_searchObject.StartDate) && DbFunctions.TruncateTime(a.CreatedDate) <= DbFunctions.TruncateTime(_searchObject.EndDate)).ToList();
                }
                else
                {
                    _listObject = db.VouchersHeads.Where(WhereClouse).Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId).ToList();
                }
                return PartialView("_SearchRecovery", _listObject);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ActionResult CashReceiptDownloadExcel(DownloadFilesViewModel viewModel)
        {
            try
            {
                byte[] excelReportBytes;
                long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
                long BranchId = Convert.ToInt64(Session["BranchId"]);
                var BrancheObj = dALCode.GetBranchDefinition(BranchId);
                VouchersSearchViewModel _searchObject = JsonConvert.DeserializeObject<VouchersSearchViewModel>(viewModel.SearchValues);

                if (_searchObject.StartDate == null && _searchObject.EndDate == null)
                {
                    _searchObject.StartDate = DateTime.Now.AddMonths(-1);
                    _searchObject.EndDate = DateTime.Now;
                }

                List<VouchersHead> _listObject = new List<VouchersHead>();
                Expression<Func<VouchersHead, bool>> WhereClouse = null;

                WhereClouse = x => x.VoucherType.ToString().Contains(VoucherType.CRV.ToString())
                || x.VoucherType.ToString().Contains(VoucherType.BRV.ToString())
                || x.VoucherType.ToString().Contains(VoucherType.JV.ToString())
                ;

                if (_searchObject.StartDate != null && _searchObject.EndDate != null)
                {
                    _listObject = db.VouchersHeads.Where(WhereClouse).Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId).Where(a => DbFunctions.TruncateTime(a.CreatedDate) >= DbFunctions.TruncateTime(_searchObject.StartDate) && DbFunctions.TruncateTime(a.CreatedDate) <= DbFunctions.TruncateTime(_searchObject.EndDate)).ToList();
                }
                else
                {
                    _listObject = db.VouchersHeads.Where(WhereClouse).Where(a => a.OrganizationID == OrganizationID && a.BranchId == BranchId).ToList();
                }

                using (var package = excelPackage.CashReceipt_CreateExcelPackage(_listObject, BrancheObj))
                {
                    excelReportBytes = package.GetAsByteArray();
                }
                return File(excelReportBytes, XlsxContentType, "CashReceipts_" + DateTime.Now.Ticks + ".xlsx");

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        #endregion
        #region CustomerPaymentRecovery_Region
        public ActionResult CustomerPaymentRecovery()
        {
            return View();
        }
        public ActionResult CustomerPaymentRecoverySearch(string searchModel)
        {
            try
            {
                long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
                long BranchId = Convert.ToInt64(Session["BranchId"]);
                VouchersSearchViewModel _searchObject = JsonConvert.DeserializeObject<VouchersSearchViewModel>(searchModel);

                if (_searchObject.StartDate == null && _searchObject.EndDate == null)
                {
                    DateTime now = DateTime.Now;
                    _searchObject.StartDate = new DateTime(now.Year, now.Month, 1);
                    _searchObject.EndDate = Convert.ToDateTime(_searchObject.StartDate).AddMonths(1).AddDays(-1);
                }

                List<InstallmentsPaymentsScheduler> _listObject = new List<InstallmentsPaymentsScheduler>();
                Expression<Func<InstallmentsPaymentsScheduler, bool>> WhereClouse = null;

                WhereClouse = x => x.PaymentStatus.ToString().Contains(PaymentStatus.UnPaid.ToString())              
                ;

                if (_searchObject.StartDate != null && _searchObject.EndDate != null)
                {
                    _listObject = db.InstallmentsPaymentsSchedulers.Where(WhereClouse).Where(a => a.PaymentMasters.OrganizationId == OrganizationID && a.PaymentMasters.BranchId == BranchId).Where(a => DbFunctions.TruncateTime(a.PaymentDueDate) >= DbFunctions.TruncateTime(_searchObject.StartDate) && DbFunctions.TruncateTime(a.PaymentDueDate) <= DbFunctions.TruncateTime(_searchObject.EndDate)).ToList();
                }
                else
                {
                    _listObject = db.InstallmentsPaymentsSchedulers.Where(WhereClouse).Where(a => a.PaymentMasters.OrganizationId == OrganizationID && a.PaymentMasters.BranchId == BranchId).ToList();
                }
                return PartialView("_SearchPaymentRecovery", _listObject);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ActionResult CustomerPaymentDownloadExcel(DownloadFilesViewModel viewModel)
        {
            try
            {
                byte[] excelReportBytes;
                long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
                long BranchId = Convert.ToInt64(Session["BranchId"]);
                var BrancheObj = dALCode.GetBranchDefinition(BranchId);
                VouchersSearchViewModel _searchObject = JsonConvert.DeserializeObject<VouchersSearchViewModel>(viewModel.SearchValues);

                if (_searchObject.StartDate == null && _searchObject.EndDate == null)
                {
                    DateTime now = DateTime.Now;
                    _searchObject.StartDate = new DateTime(now.Year, now.Month, 1);
                    _searchObject.EndDate = Convert.ToDateTime(_searchObject.StartDate).AddMonths(1).AddDays(-1);
                }

                List<InstallmentsPaymentsScheduler> _listObject = new List<InstallmentsPaymentsScheduler>();
                Expression<Func<InstallmentsPaymentsScheduler, bool>> WhereClouse = null;

                WhereClouse = x => x.PaymentStatus.ToString().Contains(PaymentStatus.UnPaid.ToString())
                ;

                if (_searchObject.StartDate != null && _searchObject.EndDate != null)
                {
                    _listObject = db.InstallmentsPaymentsSchedulers.Where(WhereClouse).Where(a => a.PaymentMasters.OrganizationId == OrganizationID && a.PaymentMasters.BranchId == BranchId).Where(a => DbFunctions.TruncateTime(a.PaymentDueDate) >= DbFunctions.TruncateTime(_searchObject.StartDate) && DbFunctions.TruncateTime(a.PaymentDueDate) <= DbFunctions.TruncateTime(_searchObject.EndDate)).ToList();
                }
                else
                {
                    _listObject = db.InstallmentsPaymentsSchedulers.Where(WhereClouse).Where(a => a.PaymentMasters.OrganizationId == OrganizationID && a.PaymentMasters.BranchId == BranchId).ToList();
                }

                using (var package = excelPackage.CustomerPayment_CreateExcelPackage(_listObject, BrancheObj))
                {
                    excelReportBytes = package.GetAsByteArray();
                }
                return File(excelReportBytes, XlsxContentType, "CustomerPayment_" + DateTime.Now.Ticks + ".xlsx");

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        #endregion



    }
}