using MudasirRehmanAlp.AppDAL;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models.StoredPocedureModel;
using MudasirRehmanAlp.ModelsView;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MudasirRehmanAlp.ReportsSetup.FinanceReports
{
    public class BranchIncomeStatementReportController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        ExcelPackageDAL excelPackage = new ExcelPackageDAL();
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        // GET: BranchIncomeStatementReport
        public ActionResult Index()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            var findfinancialBookYear = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == OrganizationID).FirstOrDefault();
            if (findfinancialBookYear != null)
            {
                ViewBag.FinancialBookYearId = findfinancialBookYear.Id;
                ViewBag.FinancialBookYear = findfinancialBookYear.YearCode + " - " + findfinancialBookYear.YearName;
            }
            return View();
        }
        public ActionResult IncomeStatementSearch(string searchModel)
        {
            try
            {
                long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
                long BranchId = Convert.ToInt64(Session["BranchId"]);
                long FinancialBookYearId = 0;

                IncomeStatementSearchViewModel _searchObject = JsonConvert.DeserializeObject<IncomeStatementSearchViewModel>(searchModel);

                if (_searchObject.StartDate == null && _searchObject.EndDate == null)
                {
                    _searchObject.StartDate = DateTime.Now.AddMonths(-1);
                    _searchObject.EndDate = DateTime.Now;
                }
                if (_searchObject.FinancialBookYearId == null)
                {
                    var findfinancialBookYear = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == OrganizationID).FirstOrDefault();
                    FinancialBookYearId = findfinancialBookYear.Id;
                }
                else
                {
                    FinancialBookYearId = Convert.ToInt64(_searchObject.FinancialBookYearId);
                }

                List<ProcBranchIncomeStatement> _listObject = new List<ProcBranchIncomeStatement>();

                if (_searchObject.StartDate != null && _searchObject.EndDate != null && FinancialBookYearId != 0)
                {
                    _listObject = db.ProcBranchIncomeStatements(OrganizationID, BranchId,FinancialBookYearId, _searchObject.StartDate, _searchObject.EndDate);
                }

                return PartialView("_Search", _listObject);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ActionResult IncomeStatementDownloadExcel(DownloadFilesViewModel viewModel)
        {
            try
            {
                byte[] excelReportBytes;
                long FinancialBookYearId = 0;
                long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
                long BranchId = Convert.ToInt64(Session["BranchId"]);

                var BrancheObj = dALCode.GetBranchDefinition(BranchId);
                IncomeStatementSearchViewModel _searchObject = JsonConvert.DeserializeObject<IncomeStatementSearchViewModel>(viewModel.SearchValues);

                if (_searchObject.StartDate == null && _searchObject.EndDate == null)
                {
                    _searchObject.StartDate = DateTime.Now.AddMonths(-1);
                    _searchObject.EndDate = DateTime.Now;
                }
                if (_searchObject.FinancialBookYearId == null)
                {
                    var findfinancialBookYear = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == OrganizationID).FirstOrDefault();
                    FinancialBookYearId = findfinancialBookYear.Id;
                }
                else
                {
                    FinancialBookYearId = Convert.ToInt64(_searchObject.FinancialBookYearId);
                }
                List<ProcBranchIncomeStatement> _listObject = new List<ProcBranchIncomeStatement>();

                if (_searchObject.StartDate != null && _searchObject.EndDate != null && FinancialBookYearId != 0)
                {
                    _listObject = db.ProcBranchIncomeStatements(OrganizationID, BranchId,FinancialBookYearId, _searchObject.StartDate, _searchObject.EndDate);
                }

                using (var package = excelPackage.IncomeStatementBranch_CreateExcelPackage(_listObject, BrancheObj))
                {
                    excelReportBytes = package.GetAsByteArray();
                }
                return File(excelReportBytes, XlsxContentType, "BranchIncomeStatement_" + DateTime.Now.Ticks + ".xlsx");

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
    }
}