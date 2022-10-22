using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.Models.StoredPocedureModel;
using MudasirRehmanAlp.ModelsView;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.AppDAL
{

    public class ExcelPackageDAL
    {
        CustomerLedgerDAL ledgerDAL = new CustomerLedgerDAL();
        public ExcelPackage CustomerLedger_CreateExcelPackage(List<VouchersHead> _listObject, BranchDefinition _object)
        {
            var package = new ExcelPackage();
            Color colorbg = System.Drawing.ColorTranslator.FromHtml("#979797");
            Color colorbgPay = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            Color colFrombg = System.Drawing.ColorTranslator.FromHtml("#D9D9D9");
            package.Workbook.Properties.Title = "Customer Ledger Report";
            package.Workbook.Properties.Author = _object.OrganizationDefinition.OrganizationUnitName;
            package.Workbook.Properties.Subject = "Customer Ledger Report";
            package.Workbook.Properties.Keywords = "Customer Ledger";


            var worksheetRent = package.Workbook.Worksheets.Add("Customer Ledgers");

            worksheetRent.Cells[1, 1].Value = _object.OrganizationDefinition.OrganizationUnitName + " " + _object.Name;
            worksheetRent.Cells[2, 1].Value = "Customer Ledgers";
            //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
            //Merge the coumns
            worksheetRent.Cells[1, 1, 1, 14].Merge = true;
            worksheetRent.Cells[2, 1, 2, 14].Merge = true;
            //center algin the text
            worksheetRent.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheetRent.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


            worksheetRent.Cells["A1:L1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheetRent.Cells["A1:L1"].Style.Fill.BackgroundColor.SetColor(colFrombg);
            worksheetRent.Cells["B1:B1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheetRent.Cells["B1:B1"].Style.Fill.BackgroundColor.SetColor(colFrombg);
            worksheetRent.Cells["A1:L1"].Style.Font.Name = "Arial";
            worksheetRent.Cells["A1:L1"].Style.Font.Size = 15;
            worksheetRent.Cells["A2:L2"].Style.Font.Name = "Arial";
            worksheetRent.Cells["A2:L2"].Style.Font.Size = 15;



            //First add the headers
            worksheetRent.Cells[3, 1].Value = "Sr#";
            worksheetRent.Cells[3, 2].Value = "Account No";
            worksheetRent.Cells[3, 3].Value = "Customer Name";
            worksheetRent.Cells[3, 4].Value = "Mobile No";
            worksheetRent.Cells[3, 5].Value = "Sale Order Date";
            worksheetRent.Cells[3, 6].Value = "Department";
            worksheetRent.Cells[3, 7].Value = "Produts";
            worksheetRent.Cells[3, 8].Value = "Selling Price";
            worksheetRent.Cells[3, 9].Value = "Advance Received";
            worksheetRent.Cells[3, 10].Value = "Receivable";
            worksheetRent.Cells[3, 11].Value = "Plan";
            worksheetRent.Cells[3, 12].Value = "PMI";
            worksheetRent.Cells[3, 13].Value = "Cash Receive No";
            worksheetRent.Cells[3, 14].Value = "Amount";

            worksheetRent.Cells["A3:L3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheetRent.Cells["A3:L3"].Style.Fill.BackgroundColor.SetColor(colorbg);

            worksheetRent.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheetRent.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheetRent.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheetRent.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            worksheetRent.Cells["A3:N3"].Style.Font.Color.SetColor(Color.White);

            int counter = 4;
            int srNo = 0;
            foreach (var item in _listObject.OrderBy(a => a.VoucherID))
            {
                CustomerLedgerReportViewModel viewModel = new CustomerLedgerReportViewModel();
                srNo = srNo + 1;
                viewModel = ledgerDAL.GetDetailsOfCustomerLedgers(item.SaleId);

                worksheetRent.Cells[counter, 1].Value = srNo;
                if (item.PaymentType == CommonEnums.PaymentType.SaleOrder && item.SaleId != null)
                {
                    worksheetRent.Cells[counter, 2].Value = item.SaleOrder.Account.AccountNo;
                }
                else
                {
                    worksheetRent.Cells[counter, 2].Value = "";
                }
                if (item.PaymentType == CommonEnums.PaymentType.SaleOrder && item.SaleId != null)
                {
                    worksheetRent.Cells[counter, 3].Value = item.SaleOrder.CustomerStatement.Name;
                }
                else
                {
                    worksheetRent.Cells[counter, 3].Value = "";
                }
                if (item.PaymentType == CommonEnums.PaymentType.SaleOrder && item.SaleId != null)
                {
                    worksheetRent.Cells[counter, 4].Value = item.SaleOrder.CustomerStatement.MobileNo;
                }
                else
                {
                    worksheetRent.Cells[counter, 4].Value = "";
                }
                if (item.PaymentType == CommonEnums.PaymentType.SaleOrder && item.SaleId != null)
                {
                    worksheetRent.Cells[counter, 5].Value = item.SaleOrder.SaleOrderDate;
                }
                else
                {
                    worksheetRent.Cells[counter, 5].Value = "";
                }
                if (item.PaymentType == CommonEnums.PaymentType.SaleOrder && item.SaleId != null)
                {
                    worksheetRent.Cells[counter, 6].Value = item.SaleOrder.CustomerStatement.DepartmentName;
                }
                else
                {
                    worksheetRent.Cells[counter, 6].Value = "";
                }

                worksheetRent.Cells[counter, 7].Value = viewModel.ProductsName;
                worksheetRent.Cells[counter, 8].Value = viewModel.UnitRate;
                worksheetRent.Cells[counter, 9].Value = viewModel.DownPayment;
                worksheetRent.Cells[counter, 10].Value = viewModel.Receivable;
                worksheetRent.Cells[counter, 11].Value = viewModel.NoOfMonths;
                worksheetRent.Cells[counter, 12].Value = viewModel.InstallmentPerMonth;
                worksheetRent.Cells[counter, 13].Value = item.VoucherCode;
                worksheetRent.Cells[counter, 14].Value = "";
                counter++;
            }

            worksheetRent.Cells.AutoFitColumns();


            return package;
        }
        public ExcelPackage CashReceipt_CreateExcelPackage(List<VouchersHead> _listObject, BranchDefinition _object)
        {
            var package = new ExcelPackage();
            Color colorbg = System.Drawing.ColorTranslator.FromHtml("#979797");
            Color colorbgPay = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            Color colFrombg = System.Drawing.ColorTranslator.FromHtml("#D9D9D9");
            package.Workbook.Properties.Title = "Cash Receipts Recovery";
            package.Workbook.Properties.Author = _object.OrganizationDefinition.OrganizationUnitName;
            package.Workbook.Properties.Subject = "Cash Receipts Recovery";
            package.Workbook.Properties.Keywords = "Cash Receipts";


            var worksheetRent = package.Workbook.Worksheets.Add("Cash Receipts Recovery");

            worksheetRent.Cells[1, 1].Value = _object.OrganizationDefinition.OrganizationUnitName + " " + _object.Name;
            worksheetRent.Cells[2, 1].Value = "Cash Receipts Recovery";
            //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
            //Merge the coumns
            worksheetRent.Cells[1, 1, 1, 7].Merge = true;
            worksheetRent.Cells[2, 1, 2, 7].Merge = true;
            //center algin the text
            worksheetRent.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheetRent.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


            worksheetRent.Cells["A1:G1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheetRent.Cells["A1:G1"].Style.Fill.BackgroundColor.SetColor(colFrombg);
            worksheetRent.Cells["B1:B1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheetRent.Cells["B1:B1"].Style.Fill.BackgroundColor.SetColor(colFrombg);
            worksheetRent.Cells["A1:G1"].Style.Font.Name = "Arial";
            worksheetRent.Cells["A1:G1"].Style.Font.Size = 15;
            worksheetRent.Cells["A2:G2"].Style.Font.Name = "Arial";
            worksheetRent.Cells["A2:G2"].Style.Font.Size = 15;



            //First add the headers
            worksheetRent.Cells[3, 1].Value = "Sr#";
            worksheetRent.Cells[3, 2].Value = "Date";
            worksheetRent.Cells[3, 3].Value = "Account No";
            worksheetRent.Cells[3, 4].Value = "Customer Name";
            worksheetRent.Cells[3, 5].Value = "Cash Receive No";
            worksheetRent.Cells[3, 6].Value = "Amount";



            worksheetRent.Cells["A3:G3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheetRent.Cells["A3:G3"].Style.Fill.BackgroundColor.SetColor(colorbg);

            worksheetRent.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheetRent.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheetRent.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheetRent.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            worksheetRent.Cells["A3:N3"].Style.Font.Color.SetColor(Color.White);

            int counter = 4;
            int srNo = 0;
            foreach (var item in _listObject.OrderBy(a => a.VoucherID))
            {
                CustomerLedgerReportViewModel viewModel = new CustomerLedgerReportViewModel();
                srNo = srNo + 1;
                viewModel = ledgerDAL.GetDetailsOfCustomerLedgers(item.SaleId);

                worksheetRent.Cells[counter, 1].Value = srNo;
                worksheetRent.Cells[counter, 2].Value = item.VoucherDate;
                if (item.PaymentType == CommonEnums.PaymentType.SaleOrder && item.SaleId != null)
                {
                    worksheetRent.Cells[counter, 3].Value = item.SaleOrder.Account.AccountNo;
                }
                else
                {
                    worksheetRent.Cells[counter, 3].Value = "";
                }
                if (item.PaymentType == CommonEnums.PaymentType.SaleOrder && item.SaleId != null)
                {
                    worksheetRent.Cells[counter, 4].Value = item.SaleOrder.CustomerStatement.Name;
                }
                else
                {
                    worksheetRent.Cells[counter, 4].Value = "";
                }
                worksheetRent.Cells[counter, 5].Value = item.VoucherCode;
                worksheetRent.Cells[counter, 6].Value = item.TotalAmount;
                counter++;
            }

            worksheetRent.Cells.AutoFitColumns();


            return package;
        }
        public ExcelPackage CustomerPayment_CreateExcelPackage(List<InstallmentsPaymentsScheduler> _listObject, BranchDefinition _object)
        {
            var package = new ExcelPackage();
            Color colorbg = System.Drawing.ColorTranslator.FromHtml("#979797");
            Color colorbgPay = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            Color colFrombg = System.Drawing.ColorTranslator.FromHtml("#D9D9D9");
            package.Workbook.Properties.Title = "Customer Payment Recovery";
            package.Workbook.Properties.Author = _object.OrganizationDefinition.OrganizationUnitName;
            package.Workbook.Properties.Subject = "Customer Payment Recovery";
            package.Workbook.Properties.Keywords = "Customer Payment";


            var worksheetRent = package.Workbook.Worksheets.Add("Customer Payment Recovery");

            worksheetRent.Cells[1, 1].Value = _object.OrganizationDefinition.OrganizationUnitName + " " + _object.Name;
            worksheetRent.Cells[2, 1].Value = "Customer Payment Recovery";
            //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
            //Merge the coumns
            worksheetRent.Cells[1, 1, 1, 9].Merge = true;
            worksheetRent.Cells[2, 1, 2, 9].Merge = true;
            //center algin the text
            worksheetRent.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheetRent.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


            worksheetRent.Cells["A1:G1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheetRent.Cells["A1:G1"].Style.Fill.BackgroundColor.SetColor(colFrombg);
            worksheetRent.Cells["B1:B1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheetRent.Cells["B1:B1"].Style.Fill.BackgroundColor.SetColor(colFrombg);
            worksheetRent.Cells["A1:G1"].Style.Font.Name = "Arial";
            worksheetRent.Cells["A1:G1"].Style.Font.Size = 15;
            worksheetRent.Cells["A2:G2"].Style.Font.Name = "Arial";
            worksheetRent.Cells["A2:G2"].Style.Font.Size = 15;



            //First add the headers
            worksheetRent.Cells[3, 1].Value = "Sr#";
            worksheetRent.Cells[3, 2].Value = "Code";
            worksheetRent.Cells[3, 3].Value = "Date";
            worksheetRent.Cells[3, 4].Value = "Customer Name";
            worksheetRent.Cells[3, 5].Value = "Installment Amount";
            worksheetRent.Cells[3, 6].Value = "Due Amount";
            worksheetRent.Cells[3, 7].Value = "Balance Amount";
            worksheetRent.Cells[3, 8].Value = "Mobile No";

            worksheetRent.Cells["A3:H3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheetRent.Cells["A3:H3"].Style.Fill.BackgroundColor.SetColor(colorbg);

            worksheetRent.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheetRent.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheetRent.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheetRent.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            worksheetRent.Cells["A3:N3"].Style.Font.Color.SetColor(Color.White);

            int counter = 4;
            int srNo = 0;
            foreach (var item in _listObject)
            {
                CustomerLedgerReportViewModel viewModel = new CustomerLedgerReportViewModel();
                srNo = srNo + 1;

                worksheetRent.Cells[counter, 1].Value = srNo;
                worksheetRent.Cells[counter, 2].Value = item.PaymentMasters.SaleOrder.CustomerStatement.Code;
                worksheetRent.Cells[counter, 3].Value = item.PaymentDueDate;
                worksheetRent.Cells[counter, 4].Value = item.PaymentMasters.SaleOrder.CustomerStatement.Name;

                worksheetRent.Cells[counter, 5].Value = item.PerMonthAmount;
                worksheetRent.Cells[counter, 6].Value = item.PerMonthAmount;
                worksheetRent.Cells[counter, 7].Value = item.PaymentMasters.RemainingBalanceAmount;
                worksheetRent.Cells[counter, 8].Value = item.PaymentMasters.SaleOrder.CustomerStatement.MobileNo;
                counter++;
            }

            worksheetRent.Cells.AutoFitColumns();


            return package;
        }
        public ExcelPackage IncomeStatement_CreateExcelPackage(List<ProcIncomeStatement> _listObject, BranchDefinition _object)
        {
            var package = new ExcelPackage();
            Color colorbg = System.Drawing.ColorTranslator.FromHtml("#979797");
            Color colorbgPay = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            Color colFrombg = System.Drawing.ColorTranslator.FromHtml("#D9D9D9");
            package.Workbook.Properties.Title = "Income Statement Report";
            package.Workbook.Properties.Author = _object.OrganizationDefinition.OrganizationUnitName;
            package.Workbook.Properties.Subject = "Income Statement Report";
            package.Workbook.Properties.Keywords = "Income Statement";


            var worksheetRent = package.Workbook.Worksheets.Add("Income Statement");

            worksheetRent.Cells[1, 1].Value = _object.OrganizationDefinition.OrganizationUnitName + " " + _object.Name;
            worksheetRent.Cells[2, 1].Value = "Income Statement";
            //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
            //Merge the coumns
            worksheetRent.Cells[1, 1, 1, 7].Merge = true;
            worksheetRent.Cells[2, 1, 2, 7].Merge = true;
            //center algin the text
            worksheetRent.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheetRent.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


            worksheetRent.Cells["A1:L1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheetRent.Cells["A1:L1"].Style.Fill.BackgroundColor.SetColor(colFrombg);
            worksheetRent.Cells["B1:B1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheetRent.Cells["B1:B1"].Style.Fill.BackgroundColor.SetColor(colFrombg);
            worksheetRent.Cells["A1:L1"].Style.Font.Name = "Arial";
            worksheetRent.Cells["A1:L1"].Style.Font.Size = 15;
            worksheetRent.Cells["A2:L2"].Style.Font.Name = "Arial";
            worksheetRent.Cells["A2:L2"].Style.Font.Size = 15;



            //First add the headers
            worksheetRent.Cells[3, 1].Value = "Sr#";
            worksheetRent.Cells[3, 2].Value = "Account Name";
            worksheetRent.Cells[3, 3].Value = "Amount";


            worksheetRent.Cells["A3:G3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheetRent.Cells["A3:G3"].Style.Fill.BackgroundColor.SetColor(colorbg);

            worksheetRent.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheetRent.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheetRent.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheetRent.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            worksheetRent.Cells["A3:G3"].Style.Font.Color.SetColor(Color.White);

            int counter = 4;
            int srNo = 0;
            foreach (var item in _listObject)
            {

                srNo = srNo + 1;
                worksheetRent.Cells[counter, 1].Value = item.SerialNo;
                worksheetRent.Cells[counter, 2].Value = item.AccountName;
                worksheetRent.Cells[counter, 3].Value = item.Amount;
                counter++;
            }

            worksheetRent.Cells.AutoFitColumns();


            return package;
        }
        public ExcelPackage IncomeStatementBranch_CreateExcelPackage(List<ProcBranchIncomeStatement> _listObject, BranchDefinition _object)
        {
            var package = new ExcelPackage();
            Color colorbg = System.Drawing.ColorTranslator.FromHtml("#979797");
            Color colorbgPay = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            Color colFrombg = System.Drawing.ColorTranslator.FromHtml("#D9D9D9");
            package.Workbook.Properties.Title = "Branch Income Statement Report";
            package.Workbook.Properties.Author = _object.OrganizationDefinition.OrganizationUnitName;
            package.Workbook.Properties.Subject = "Branch Income Statement Report";
            package.Workbook.Properties.Keywords = "Branch Income Statement";


            var worksheetRent = package.Workbook.Worksheets.Add("Branch Income Statement");

            worksheetRent.Cells[1, 1].Value = _object.OrganizationDefinition.OrganizationUnitName + " " + _object.Name;
            worksheetRent.Cells[2, 1].Value = "Branch Income Statement";
            //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
            //Merge the coumns
            worksheetRent.Cells[1, 1, 1, 7].Merge = true;
            worksheetRent.Cells[2, 1, 2, 7].Merge = true;
            //center algin the text
            worksheetRent.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheetRent.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


            worksheetRent.Cells["A1:L1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheetRent.Cells["A1:L1"].Style.Fill.BackgroundColor.SetColor(colFrombg);
            worksheetRent.Cells["B1:B1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheetRent.Cells["B1:B1"].Style.Fill.BackgroundColor.SetColor(colFrombg);
            worksheetRent.Cells["A1:L1"].Style.Font.Name = "Arial";
            worksheetRent.Cells["A1:L1"].Style.Font.Size = 15;
            worksheetRent.Cells["A2:L2"].Style.Font.Name = "Arial";
            worksheetRent.Cells["A2:L2"].Style.Font.Size = 15;



            //First add the headers
            worksheetRent.Cells[3, 1].Value = "Sr#";
            worksheetRent.Cells[3, 2].Value = "Account Name";
            worksheetRent.Cells[3, 3].Value = "Amount";


            worksheetRent.Cells["A3:G3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheetRent.Cells["A3:G3"].Style.Fill.BackgroundColor.SetColor(colorbg);

            worksheetRent.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheetRent.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheetRent.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheetRent.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            worksheetRent.Cells["A3:G3"].Style.Font.Color.SetColor(Color.White);

            int counter = 4;
            int srNo = 0;
            foreach (var item in _listObject)
            {

                srNo = srNo + 1;
                worksheetRent.Cells[counter, 1].Value = item.SerialNo;
                worksheetRent.Cells[counter, 2].Value = item.AccountName;
                worksheetRent.Cells[counter, 3].Value = item.Amount;
                counter++;
            }

            worksheetRent.Cells.AutoFitColumns();


            return package;
        }
    }
}