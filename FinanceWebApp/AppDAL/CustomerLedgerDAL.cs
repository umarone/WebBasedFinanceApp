using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.ModelsView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.AppDAL
{
    public class CustomerLedgerDAL
    {
        AppEntities db = new AppEntities();
        public CustomerLedgerReportViewModel GetDetailsOfCustomerLedgers(long? SaleId)
        {
            CustomerLedgerReportViewModel viewModel = new CustomerLedgerReportViewModel();

            var ObjMasterPayment = db.PaymentMasters.Where(a => a.SaleOrderId == SaleId).ToList();

            foreach (var item in ObjMasterPayment)
            {
                var sumOfReceived = db.InstallmentsPaymentsSchedulers.Where(a => a.PaymentMasterId == item.Id && a.PaymentStatus == PaymentStatus.Paid).ToList().Sum(a => a.PaidAmount);

                if (ObjMasterPayment.Count > 1)
                {
                    viewModel.ProductsName += "," + item.ProductDefinition.ProductName;
                    viewModel.UnitRate += "," + item.SaleOrderDetail.UnitRate;
                    viewModel.DownPayment += "," + item.SaleOrderDetail.DownPayment;
                    viewModel.NoOfMonths += "," + item.SaleOrderDetail.NoOfMonths;
                    viewModel.InstallmentPerMonth += "," + item.SaleOrderDetail.InstallmentPerMonth;
                    viewModel.Receivable += "," + sumOfReceived;
                }
                else
                {
                    viewModel.ProductsName = item.ProductDefinition.ProductName;
                    viewModel.UnitRate += item.SaleOrderDetail.UnitRate;
                    viewModel.DownPayment += item.SaleOrderDetail.DownPayment;
                    viewModel.NoOfMonths += item.SaleOrderDetail.NoOfMonths;
                    viewModel.InstallmentPerMonth += item.SaleOrderDetail.InstallmentPerMonth;
                    viewModel.Receivable += sumOfReceived;
                }

            }


            return viewModel;

        }
    }
}