using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.AppDAL
{
    public class PaymentMasterDAL
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL autoGenerateCodeDAL = new AutoGenerateCodeDAL();
        public List<InstallmentsPaymentsScheduler> GetInstallmentsPayments(long Id)
        {
            
                List<InstallmentsPaymentsScheduler> _listObj = new List<InstallmentsPaymentsScheduler>();
                _listObj = db.InstallmentsPaymentsSchedulers.Where(a => a.PaymentMasterId == Id).OrderBy(a => a.SrNo).ToList();
                return _listObj;
            
        }

        public string AddInstallmentsPayments(InstallmentsPaymentsSchedulerViewModel postModel, long userID)
        {
            string violationMessage = "";
            long masterId = 0;
            decimal AmountPaid = 0;
            long AccountCustomerId = 0;
            long AccountSaleId = 0;
            decimal v_TotalAmount = 0;
            ;
            VouchersHeadViewModel vouchersHeadView = new VouchersHeadViewModel();
            List<VouchersDetailViewModel> ListObjVoucherDetails = new List<VouchersDetailViewModel>();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (postModel.IsUpdateCheck == false)
                    {
                        var findObj = db.InstallmentsPaymentsSchedulers.Where(a => a.Id == postModel.Id).FirstOrDefault();
                        if (findObj != null)
                        {
                            Guid transactionkey = Guid.NewGuid();
                            //This Is For Voucher Head
                            vouchersHeadView.TransactionKey = transactionkey;
                            #region CommonRegionForAll
                            var findSaleAccount = db.AccountMapping.Where(a => a.Account.IsActive == true && a.Account.IsDeleted == false && a.OrganizationId == findObj.OrganizationId && a.BranchId == findObj.PaymentMasters.BranchId && a.AccountDefaultType == AccountDefaultType.Sales).FirstOrDefault();
                            
                            if (findSaleAccount != null)
                            {
                                AccountSaleId = findSaleAccount.Account.AccountId;
                            }
                            var findfinancialBookYear = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == findObj.OrganizationId).FirstOrDefault();
                            if (findfinancialBookYear != null)
                            {
                                vouchersHeadView.FinancialBookYearId = findfinancialBookYear.Id;

                            }
                            else
                            {
                                vouchersHeadView.FinancialBookYearId = null;

                            }
                            #endregion

                            AccountCustomerId = Convert.ToInt64(findObj.PaymentMasters.SaleOrder.AccountID);
                            v_TotalAmount = Convert.ToDecimal(postModel.PaidAmount);
                            findObj.OrganizationId = postModel.OrganizationId;
                            findObj.PaidAmount = postModel.PaidAmount;
                            findObj.ExtraCharges = postModel.ExtraCharges;
                            findObj.ReceivedDate = postModel.ReceivedDate;
                            findObj.Notes = postModel.Notes;
                            findObj.PaymentStatus = PaymentStatus.Paid;
                            findObj.TransactionKey = transactionkey;
                            findObj.UpdatedBy = userID;
                            findObj.UpdatedDate = DateTime.Now;
                            db.Entry(findObj).State = EntityState.Modified;
                            db.SaveChanges();
                            masterId = Convert.ToInt64(findObj.PaymentMasterId);
                            AmountPaid = Convert.ToDecimal(findObj.PaidAmount);

                            #region VoucherDetailsRegion

                            //Row Debit
                            VouchersDetailViewModel detailViewModelDebit = new VouchersDetailViewModel();
                            detailViewModelDebit.AccountId = AccountCustomerId;
                            //detailViewModelDebit.ClosingBalance = GetClosingBalanceByAccount(Orgid, Branchid, AccountCustomerId, Convert.ToInt64(vouchersHeadView.FinancialBookYearId)); ;
                            detailViewModelDebit.Narration = "Paid Installments Payments";
                            detailViewModelDebit.Debit = findObj.PaidAmount;
                            detailViewModelDebit.Credit = 0;
                            ListObjVoucherDetails.Add(detailViewModelDebit);
                            //Row Cridet

                            VouchersDetailViewModel detailViewModelCredit = new VouchersDetailViewModel();
                            detailViewModelCredit.AccountId = AccountSaleId;
                            //detailViewModelCredit.ClosingBalance = GetClosingBalanceByAccount(Orgid, Branchid, AccountSaleId, Convert.ToInt64(vouchersHeadView.FinancialBookYearId));
                            detailViewModelCredit.Narration = "Paid Installments Payments";
                            detailViewModelCredit.Debit = 0;
                            detailViewModelCredit.Credit = findObj.PaidAmount;
                            ListObjVoucherDetails.Add(detailViewModelCredit);

                            #endregion
                        }
                        #region Balance Job Run
                        RunBalanceCalculator(masterId, AmountPaid);
                        #endregion
                        #region VoucherHeadRegion

                        vouchersHeadView.SaleId = findObj.PaymentMasters.SaleOrder.SaleOrdeID;
                        vouchersHeadView.OrganizationID = findObj.OrganizationId;
                        vouchersHeadView.BranchId = findObj.PaymentMasters.BranchId;
                        vouchersHeadView.VoucherDate = DateTime.Now;
                        vouchersHeadView.PaymentType = PaymentType.SaleOrder;
                        vouchersHeadView.Description = "Sale Order";
                        vouchersHeadView.CreatedBy = userID;
                        #endregion
                        #region SaveVoucherAdd
                        vouchersHeadView.TotalAmount = v_TotalAmount;
                        
                        vouchersHeadView.VouchersDetailViewList = ListObjVoucherDetails;
                        AddAutoCashReceivedVoucher(vouchersHeadView);
                        #endregion
                    }
                    else if (postModel.IsUpdateCheck == true)
                    {
                        var findObj = db.InstallmentsPaymentsSchedulers.Where(a => a.Id == postModel.Id).FirstOrDefault();
                        if (findObj != null)
                        {
                            #region CommonRegionForAll
                            var findSaleAccount = db.AccountMapping.Where(a => a.Account.IsActive == true && a.Account.IsDeleted == false && a.OrganizationId == findObj.OrganizationId && a.BranchId == findObj.PaymentMasters.BranchId && a.AccountDefaultType == AccountDefaultType.Sales).FirstOrDefault();
                            
                            if (findSaleAccount != null)
                            {
                                AccountSaleId = findSaleAccount.Account.AccountId;
                            }
                            var findfinancialBookYear = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == findObj.OrganizationId ).FirstOrDefault();
                            if (findfinancialBookYear != null)
                            {
                                vouchersHeadView.FinancialBookYearId = findfinancialBookYear.Id;

                            }
                            else
                            {
                                vouchersHeadView.FinancialBookYearId = null;

                            }
                            #endregion

                            AccountCustomerId = Convert.ToInt64(findObj.PaymentMasters.SaleOrder.AccountID);
                            v_TotalAmount = Convert.ToDecimal(postModel.PaidAmount);
                            findObj.OrganizationId = postModel.OrganizationId;
                            findObj.PaidAmount = postModel.PaidAmount;
                            findObj.ExtraCharges = postModel.ExtraCharges;
                            findObj.ReceivedDate = postModel.ReceivedDate;
                            findObj.Notes = postModel.Notes;
                            findObj.PaymentStatus = PaymentStatus.Paid;
                            findObj.UpdatedBy = userID;
                            findObj.UpdatedDate = DateTime.Now;
                            db.Entry(findObj).State = EntityState.Modified;
                            db.SaveChanges();
                            masterId = Convert.ToInt64(findObj.PaymentMasterId);
                            AmountPaid = Convert.ToDecimal(findObj.PaidAmount);

                            #region VoucherDetailsRegion

                            //Row Debit
                            VouchersDetailViewModel detailViewModelDebit = new VouchersDetailViewModel();
                            detailViewModelDebit.AccountId = AccountCustomerId;
                            //detailViewModelDebit.ClosingBalance = GetClosingBalanceByAccount(Orgid, Branchid, AccountCustomerId, Convert.ToInt64(vouchersHeadView.FinancialBookYearId)); ;
                            detailViewModelDebit.Narration = "Paid Installments Payments";
                            detailViewModelDebit.Debit = findObj.PaidAmount;
                            detailViewModelDebit.Credit = 0;
                            ListObjVoucherDetails.Add(detailViewModelDebit);
                            //Row Cridet

                            VouchersDetailViewModel detailViewModelCredit = new VouchersDetailViewModel();
                            detailViewModelCredit.AccountId = AccountSaleId;
                            //detailViewModelCredit.ClosingBalance = GetClosingBalanceByAccount(Orgid, Branchid, AccountSaleId, Convert.ToInt64(vouchersHeadView.FinancialBookYearId));
                            detailViewModelCredit.Narration = "Paid Installments Payments";
                            detailViewModelCredit.Debit = 0;
                            detailViewModelCredit.Credit = findObj.PaidAmount;
                            ListObjVoucherDetails.Add(detailViewModelCredit);

                            #endregion
                        }
                        #region Balance Job Run
                        RunBalanceCalculator(masterId, AmountPaid);
                        #endregion
                        #region VoucherHeadRegion

                        vouchersHeadView.SaleId = findObj.PaymentMasters.SaleOrder.SaleOrdeID;
                        vouchersHeadView.OrganizationID = findObj.OrganizationId;
                        vouchersHeadView.BranchId = findObj.PaymentMasters.BranchId;
                        vouchersHeadView.VoucherDate = DateTime.Now;
                        vouchersHeadView.PaymentType = PaymentType.SaleOrder;
                        vouchersHeadView.Description = "Sale Order";
                        vouchersHeadView.CreatedBy = userID;
                        #endregion
                        #region SaveVoucherAdd
                        vouchersHeadView.TotalAmount = v_TotalAmount;
                        vouchersHeadView.TransactionKey = findObj.TransactionKey;
                        vouchersHeadView.VouchersDetailViewList = ListObjVoucherDetails;
                        UpdateAutoCashReceivedVoucher(vouchersHeadView);
                        #endregion
                    }


                    transaction.Commit();
                }
                catch (DbUpdateException ex)
                {
                    transaction.Rollback();
                    var message = ex.Message;
                    var innerException = ex.InnerException;
                    while (innerException != null)
                    {
                        message = innerException.Message;
                        innerException = innerException.InnerException;
                    }
                    bool PrimaryKey = message.Contains("Violation of PRIMARY KEY");
                    bool ForginKey = message.Contains("REFERENCE");
                    bool UniqueKey = message.Contains("UNIQUE KEY");
                    if (PrimaryKey || UniqueKey)
                    {
                        violationMessage = "This Record is already added in Database.";
                    }
                    else
                    {
                        string[] arr = message.Split('.');
                        if (arr.Length > 0)
                        {
                            violationMessage = arr[0];
                        }
                    }
                }
            }

            return violationMessage;
        }

        public void RunBalanceCalculator(long masterId, decimal amountPaid)
        {
            var findObj = db.PaymentMasters.Where(a => a.Id == masterId).FirstOrDefault();
            var _ListObj = db.InstallmentsPaymentsSchedulers.Where(a => a.PaymentMasterId == masterId && a.PaymentStatus == PaymentStatus.Paid).ToList();

            decimal totalBalance = 0;
            decimal totalpaidAmount = 0;
            decimal totalExtra = 0;
            decimal totalIntallment = 0;
            decimal remainAmount = 0;
            foreach (var item in _ListObj)
            {
                totalpaidAmount += Convert.ToDecimal(item.PaidAmount);
                totalExtra += Convert.ToDecimal(item.ExtraCharges);
                totalIntallment += Convert.ToDecimal(item.PerMonthAmount);
            }

            totalBalance = totalIntallment - (totalpaidAmount + totalExtra);

            findObj.BalanceAmount = totalBalance;

            remainAmount = Convert.ToDecimal(findObj.RemainingBalanceAmount) - amountPaid;
            findObj.RemainingBalanceAmount = remainAmount;
            db.Entry(findObj).State = EntityState.Modified;
            db.SaveChanges();

        }

        public void AddAutoCashReceivedVoucher(VouchersHeadViewModel viewModel)
        {
            long id = 0;
            string violationMessage = "";
            VouchersHead obj = new VouchersHead();


            try
            {

                obj.OrganizationID = viewModel.OrganizationID;
                obj.BranchId = viewModel.BranchId;
                obj.TransactionKey = viewModel.TransactionKey;
                obj.VoucherCode = autoGenerateCodeDAL.AutoGenerateVouchersCRVCode(Convert.ToInt64(viewModel.OrganizationID), Convert.ToInt64(viewModel.BranchId));
                obj.VoucherDate = viewModel.VoucherDate;
                obj.VoucherType = CommonEnums.VoucherType.CRV;
                obj.PaymentType = viewModel.PaymentType;
                obj.SaleId = viewModel.SaleId;
                obj.SheetNo = viewModel.SheetNo;
                obj.FinancialBookYearId = viewModel.FinancialBookYearId;
                obj.TerminalNo = viewModel.TerminalNo;
                obj.ChequeNo = viewModel.ChequeNo;
                obj.TotalAmount = viewModel.TotalAmount;
                obj.Posted = viewModel.Posted;
                obj.Description = viewModel.Description;

                obj.IsActive = true;
                obj.IsDeleted = false;
                obj.CreatedBy = viewModel.CreatedBy;
                obj.CreatedDate = DateTime.Now;
                db.VouchersHeads.Add(obj);
                db.SaveChanges();
                id = obj.VoucherID;


                foreach (var item in viewModel.VouchersDetailViewList)
                {
                    VouchersDetail objChild = new VouchersDetail();
                    objChild.VoucherId = id;
                    objChild.AccountId = item.AccountId;                    
                    objChild.ClosingBalance = GetClosingBalanceByAccount(viewModel.OrganizationID, viewModel.BranchId, item.AccountId, viewModel.FinancialBookYearId);
                    objChild.Narration = item.Narration;
                    objChild.Debit = item.Debit;
                    objChild.Credit = item.Credit;
                    db.VouchersDetails.Add(objChild);
                    db.SaveChanges();
                }

            }
            catch (DbUpdateException ex)
            {

                var message = ex.Message;
                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    message = innerException.Message;
                    innerException = innerException.InnerException;
                }
                bool PrimaryKey = message.Contains("Violation of PRIMARY KEY");
                bool ForginKey = message.Contains("REFERENCE");
                bool UniqueKey = message.Contains("UNIQUE KEY");
                if (PrimaryKey || UniqueKey)
                {
                    violationMessage = "This Record is already added in Database.";
                }
                else
                {
                    string[] arr = message.Split('.');
                    if (arr.Length > 0)
                    {
                        violationMessage = arr[0];
                    }
                }
            }
        }
        public void UpdateAutoCashReceivedVoucher(VouchersHeadViewModel viewModel)
        {
            long id = 0;
            string violationMessage = "";
            VouchersHead obj = new VouchersHead();


            try
            {
                var findObj = db.VouchersHeads.Where(a => a.SaleId == viewModel.SaleId && a.VoucherType == VoucherType.CRV && a.TransactionKey== viewModel.TransactionKey).FirstOrDefault();

                if (findObj.Posted == false || findObj.Posted == null)
                {
                    findObj.OrganizationID = viewModel.OrganizationID;
                    findObj.BranchId = viewModel.BranchId;
                    findObj.VoucherDate = viewModel.VoucherDate;
                    findObj.VoucherType = CommonEnums.VoucherType.CRV;
                    findObj.PaymentType = viewModel.PaymentType;
                    findObj.SaleId = viewModel.SaleId;
                    findObj.SheetNo = viewModel.SheetNo;
                    findObj.FinancialBookYearId = viewModel.FinancialBookYearId;
                    findObj.TerminalNo = viewModel.TerminalNo;
                    findObj.ChequeNo = viewModel.ChequeNo;
                    findObj.TotalAmount = viewModel.TotalAmount;
                    findObj.Posted = viewModel.Posted;
                    findObj.Description = viewModel.Description;

                    findObj.IsActive = true;
                    findObj.IsDeleted = false;
                    findObj.UpdatedBy = viewModel.CreatedBy;
                    findObj.UpdatedDate = DateTime.Now;

                    db.Entry(findObj).State = EntityState.Modified;
                    db.SaveChanges();
                    id = findObj.VoucherID;

                    var RemoveListObj = db.VouchersDetails.Where(a => a.VoucherId == id).ToList();
                    #region RemoveVouchersDetailsRegion
                    db.VouchersDetails.RemoveRange(RemoveListObj);
                    db.SaveChanges();
                    #endregion


                    foreach (var item in viewModel.VouchersDetailViewList)
                    {
                        VouchersDetail objChild = new VouchersDetail();
                        objChild.VoucherId = id;
                        objChild.AccountId = item.AccountId;
                        objChild.ClosingBalance = item.ClosingBalance;
                        objChild.ClosingBalance = GetClosingBalanceByAccount(viewModel.OrganizationID, viewModel.BranchId, item.AccountId, viewModel.FinancialBookYearId);
                        objChild.Narration = item.Narration;
                        objChild.Debit = item.Debit;
                        objChild.Credit = item.Credit;
                        db.VouchersDetails.Add(objChild);
                        db.SaveChanges();
                    }
                }
            }
            catch (DbUpdateException ex)
            {

                var message = ex.Message;
                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    message = innerException.Message;
                    innerException = innerException.InnerException;
                }
                bool PrimaryKey = message.Contains("Violation of PRIMARY KEY");
                bool ForginKey = message.Contains("REFERENCE");
                bool UniqueKey = message.Contains("UNIQUE KEY");
                if (PrimaryKey || UniqueKey)
                {
                    violationMessage = "This Record is already added in Database.";
                }
                else
                {
                    string[] arr = message.Split('.');
                    if (arr.Length > 0)
                    {
                        violationMessage = arr[0];
                    }
                }
            }
        }
        public Nullable<decimal> GetClosingBalanceByAccount(long? OrgId, long? branchId, long? accountId, long? FBYId)
        {
            decimal? ClosingBalance = 0;
            var findSPObj = db.ProcGetClosingBalanceByAccount(OrgId, branchId, accountId, FBYId);
            if (findSPObj !=null && findSPObj.ClosingBalance !=null)
            {
                ClosingBalance = findSPObj.ClosingBalance;
            }
            
            return ClosingBalance;

        }
    }
}