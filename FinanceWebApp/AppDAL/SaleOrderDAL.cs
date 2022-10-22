using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Infrastructure.AppServices;
using MudasirRehmanAlp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using MudasirRehmanAlp.ModelsView;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.AppDAL
{
    public class SaleOrderDAL
    {
        AppEntities db = new AppEntities();
        FilesUploadService filesUploadService = new FilesUploadService();
        AutoGenerateCodeDAL autoGenerateCodeDAL = new AutoGenerateCodeDAL();
        StockDAL stockDAL = new StockDAL();
        SystemDAL systemDAL = new SystemDAL();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        public string AddSaleOrder(string objMasterItem, string objChilds, HttpPostedFileBase sOfile, long userID)
        {
            long SOid = 0;
            long Orgid = 0;
            long Branchid = 0;
            long AccountCustomerId = 0;
            long AccountSaleId = 0;
            //decimal v_TotalAmount = 0;
            byte[] Imagebytes;
            string violationMessage = "";
            string fileName = "SO_";
            string folderName = "SaleOrders";
            SaleOrder obj = new SaleOrder();
            VouchersHeadViewModel vouchersHeadView = new VouchersHeadViewModel();
            PaymentMasterViewModel paymentMasterView = new PaymentMasterViewModel();
            List<VouchersDetailViewModel> ListObjVoucherDetails = new List<VouchersDetailViewModel>();
            TransactionsViewModel transactionsViewModel = new TransactionsViewModel();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {




                    #region SaleOrderRegion
                    SaleOrder _Object = JsonConvert.DeserializeObject<SaleOrder>(objMasterItem);
                    List<SaleOrderDetail> Childitems = JsonConvert.DeserializeObject<List<SaleOrderDetail>>(objChilds);

                    if (sOfile != null)
                    {
                        using (BinaryReader br = new BinaryReader(sOfile.InputStream))
                        {
                            Imagebytes = br.ReadBytes(sOfile.ContentLength);
                            obj.SaleOrderImage = Imagebytes;
                        }
                        Stream stream = new MemoryStream(Imagebytes);
                        string extension = Path.GetExtension(sOfile.FileName);
                        var filePath = filesUploadService.UploadImages(fileName + _Object.SaleOrderNo + "_", stream, folderName, extension);
                        obj.FilePath = filePath.ToString();
                    }

                    obj.OrganizationID = _Object.OrganizationID;
                    obj.BranchId = _Object.BranchId;
                    obj.CustomerStatementID = _Object.CustomerStatementID;
                    obj.DistributorID = _Object.DistributorID;
                    obj.SaleOrderCustomerType = _Object.SaleOrderCustomerType;
                    obj.AccountID = _Object.AccountID;
                    obj.EmployeeId = _Object.EmployeeId;
                    obj.SaleOrderNo = dALCode.AutoGenerateSaleOrderCode(Convert.ToInt64(_Object.OrganizationID), Convert.ToInt64(_Object.BranchId));
                    
                    obj.SaleOrderDate = _Object.SaleOrderDate;
                    obj.TransactionType = _Object.TransactionType;
                    obj.PaymentTerms = _Object.PaymentTerms;



                    obj.FreightCharges = _Object.FreightCharges;
                    obj.NetTotal = _Object.NetTotal;
                    obj.AmountInWord = _Object.AmountInWord;
                    obj.TermAndCondition = _Object.TermAndCondition;

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.CreatedBy = userID;
                    obj.CreatedDate = DateTime.Now;
                    db.SaleOrders.Add(obj);
                    db.SaveChanges();
                    SOid = obj.SaleOrdeID;
                    Orgid = Convert.ToInt64(obj.OrganizationID);
                    Branchid = Convert.ToInt64(obj.BranchId);
                    AccountCustomerId = Convert.ToInt64(obj.AccountID);

                    #endregion

                    #region CommonRegionForAll
                    var findSaleAccount = db.AccountMapping.Where(a => a.Account.IsActive == true && a.Account.IsDeleted == false && a.OrganizationId == Orgid && a.BranchId == Branchid && a.AccountDefaultType == AccountDefaultType.Sales).FirstOrDefault();
                    //var findSaleAccount = db.Accounts.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == Orgid && a.BranchId == Branchid && a.AccountDefaultType == AccountDefaultType.Sales).FirstOrDefault();
                    if (findSaleAccount != null)
                    {
                        AccountSaleId = findSaleAccount.Account.AccountId;
                    }
                    var findfinancialBookYear = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == Orgid && a.BranchId == Branchid).FirstOrDefault();
                    if (findfinancialBookYear != null)
                    {
                        //vouchersHeadView.FinancialBookYearId = findfinancialBookYear.Id;
                        transactionsViewModel.FinancialBookYearId = findfinancialBookYear.Id;
                    }
                    else
                    {
                        //vouchersHeadView.FinancialBookYearId = null;
                        transactionsViewModel.FinancialBookYearId = null;
                    }
                    #endregion
                    #region TransactionsRegion
                    transactionsViewModel.SaleId = SOid;
                    transactionsViewModel.OrganizationID = Orgid;
                    transactionsViewModel.BranchId = Branchid;
                    transactionsViewModel.DefaultSaleAccountId = AccountSaleId;
                    transactionsViewModel.AccountId = AccountCustomerId;
                    transactionsViewModel.PaymentType = PaymentType.SaleOrder;
                    transactionsViewModel.NetTotal = obj.NetTotal;
                    AddorUpdateTransactions(transactionsViewModel);
                    #endregion




                    #region PaymentMastRegion
                    paymentMasterView.OrganizationId = Convert.ToInt64(obj.OrganizationID);
                    paymentMasterView.BranchId = Convert.ToInt64(obj.BranchId);
                    paymentMasterView.SaleOrderId = Convert.ToInt64(obj.SaleOrdeID);
                    paymentMasterView.userID = userID;
                    paymentMasterView.SaleOrderDate = obj.SaleOrderDate;
                    #endregion


                    //#region VoucherHeadRegion

                    //vouchersHeadView.SaleId = SOid;
                    //vouchersHeadView.OrganizationID = Orgid;
                    //vouchersHeadView.BranchId = Branchid;
                    //vouchersHeadView.VoucherDate = DateTime.Now;

                    //vouchersHeadView.PaymentType = PaymentType.SaleOrder;
                    //vouchersHeadView.Description = "Sale Order";
                    //vouchersHeadView.CreatedBy = userID;
                    //#endregion

                    foreach (var item in Childitems)
                    {
                        SaleOrderDetail objChild = new SaleOrderDetail();
                        long StockId = 0;
                        if (item.ProductID != null && item.ProductID != 0)
                        {
                            var StockCode = stockDAL.AutoGenerateStockCode(Convert.ToInt64(item.ProductID), Branchid);
                            StockId = stockDAL.GetStockIDByStockCode(StockCode);
                           
                        }
                        else
                        {
                            violationMessage = "Stock Not Available";
                        }
                        objChild.SaleOrderID = SOid;
                        objChild.StockId = StockId;
                        objChild.ProductID = item.ProductID;
                        objChild.Quantity = item.Quantity;
                        objChild.BalanceQuantity = item.Quantity;
                        objChild.UnitRate = item.UnitRate;
                        objChild.RemainingBalanceAmount = item.RemainingBalanceAmount;
                        objChild.MarkupPercentage = item.MarkupPercentage;
                        objChild.Total = item.Total;
                        objChild.MarkupAmount = item.MarkupAmount;
                        objChild.ProcessingFee = item.ProcessingFee;
                        objChild.DownPayment = item.DownPayment;
                        objChild.NoOfMonths = item.NoOfMonths;
                        objChild.InstallmentPerMonth = item.InstallmentPerMonth;
                        objChild.Formula = item.Formula;
                        objChild.Active = true;
                        db.SaleOrderDetails.Add(objChild);
                        db.SaveChanges();
                        #region StockUpdate_Region
                        violationMessage = stockDAL.RemoveorUpdateStockSaleOrder(StockId, Convert.ToInt64(item.Quantity));
                        #endregion

                        paymentMasterView.SaleOrderDetailId = objChild.SaleOrderDetailID;
                        if (objChild.Formula == "TotalAmount")
                        {
                            paymentMasterView.RemainingBalanceAmount = objChild.Total - objChild.DownPayment;
                        }
                        else if (objChild.Formula == "RemainingAmount")
                        {
                            paymentMasterView.RemainingBalanceAmount = objChild.RemainingBalanceAmount;
                        }

                        paymentMasterView.ProductId = objChild.ProductID;
                        paymentMasterView.DownPayment = objChild.DownPayment;
                        paymentMasterView.NoOfMonths = objChild.NoOfMonths;
                        paymentMasterView.InstallmentPerMonth = objChild.InstallmentPerMonth;
                        paymentMasterView.BalanceAmount = objChild.InstallmentPerMonth - objChild.DownPayment;
                        #region PaymentMasterSection
                        AddPaymentMaster(paymentMasterView);
                        #endregion
                        //#region VoucherDetailsRegion
                        //v_TotalAmount += Convert.ToDecimal(objChild.DownPayment);
                        ////Row Debit
                        //VouchersDetailViewModel detailViewModelDebit = new VouchersDetailViewModel();
                        //detailViewModelDebit.AccountId = AccountCustomerId;
                        ////detailViewModelDebit.ClosingBalance = GetClosingBalanceByAccount(Orgid, Branchid, AccountCustomerId, Convert.ToInt64(vouchersHeadView.FinancialBookYearId)); ;
                        //detailViewModelDebit.Narration = "Down Payment";
                        //detailViewModelDebit.Debit = objChild.DownPayment;
                        //detailViewModelDebit.Credit = 0;
                        //ListObjVoucherDetails.Add(detailViewModelDebit);
                        ////Row Cridet

                        //VouchersDetailViewModel detailViewModelCredit = new VouchersDetailViewModel();
                        //detailViewModelCredit.AccountId = AccountSaleId;
                        ////detailViewModelCredit.ClosingBalance = GetClosingBalanceByAccount(Orgid, Branchid, AccountSaleId, Convert.ToInt64(vouchersHeadView.FinancialBookYearId));
                        //detailViewModelCredit.Narration = "Down Payment";
                        //detailViewModelCredit.Debit = 0;
                        //detailViewModelCredit.Credit = objChild.DownPayment;
                        //ListObjVoucherDetails.Add(detailViewModelCredit);

                        //#endregion



                    }

                    //#region SaveVoucherAdd
                    //vouchersHeadView.TotalAmount = v_TotalAmount;
                    //vouchersHeadView.VouchersDetailViewList = ListObjVoucherDetails;
                    //AddAutoCashReceivedVoucher(vouchersHeadView);
                    //#endregion
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
        public string UpdateSaleOrder(string objMasterItem, string objChilds, HttpPostedFileBase sOfile, long userID)
        {
            long SOid = 0;
            long Orgid = 0;
            long Branchid = 0;
            long AccountCustomerId = 0;
            long AccountSaleId = 0;
            string violationMessage = "";
            string fileName = "SO_";
            string folderName = "SaleOrders";
            SaleOrder obj = new SaleOrder();
            PaymentMasterViewModel paymentMasterView = new PaymentMasterViewModel();
            TransactionsViewModel transactionsViewModel = new TransactionsViewModel();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    byte[] Imagebytes;


                    SaleOrder _Object = JsonConvert.DeserializeObject<SaleOrder>(objMasterItem);
                    List<SaleOrderDetail> Childitems = JsonConvert.DeserializeObject<List<SaleOrderDetail>>(objChilds);

                    obj = db.SaleOrders.Find(_Object.SaleOrdeID);
                    if (sOfile != null)
                    {
                        using (BinaryReader br = new BinaryReader(sOfile.InputStream))
                        {
                            Imagebytes = br.ReadBytes(sOfile.ContentLength);
                            obj.SaleOrderImage = Imagebytes;
                        }
                        Stream stream = new MemoryStream(Imagebytes);
                        string extension = Path.GetExtension(sOfile.FileName);
                        var filePath = filesUploadService.UploadImages(fileName + _Object.SaleOrderNo + "_", stream, folderName, extension);
                        obj.FilePath = filePath.ToString();
                    }



                    obj.OrganizationID = _Object.OrganizationID;
                    obj.BranchId = _Object.BranchId;
                    obj.DistributorID = _Object.DistributorID;
                    obj.AccountID = _Object.AccountID;

                    obj.EmployeeId = _Object.EmployeeId;
                    obj.SaleOrderNo = _Object.SaleOrderNo;

                    obj.SaleOrderDate = _Object.SaleOrderDate;
                    obj.TransactionType = _Object.TransactionType;
                    obj.PaymentTerms = _Object.PaymentTerms;

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.UpdatedBy = userID;
                    obj.UpdatedDate = DateTime.Now;

                    obj.FreightCharges = _Object.FreightCharges;
                    obj.NetTotal = _Object.NetTotal;
                    obj.AmountInWord = _Object.AmountInWord;
                    obj.TermAndCondition = _Object.TermAndCondition;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    SOid = obj.SaleOrdeID;
                    Orgid = Convert.ToInt64(obj.OrganizationID);
                    Branchid = Convert.ToInt64(obj.BranchId);
                    AccountCustomerId = Convert.ToInt64(obj.AccountID);

                    #region CommonRegionForAll
                    var findSaleAccount = db.AccountMapping.Where(a => a.Account.IsActive == true && a.Account.IsDeleted == false && a.OrganizationId == Orgid && a.BranchId == Branchid && a.AccountDefaultType == AccountDefaultType.Sales).FirstOrDefault();
                    if (findSaleAccount != null)
                    {
                        AccountSaleId = findSaleAccount.Account.AccountId;
                    }
                    var findfinancialBookYear = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == Orgid && a.BranchId == Branchid).FirstOrDefault();
                    if (findfinancialBookYear != null)
                    {

                        transactionsViewModel.FinancialBookYearId = findfinancialBookYear.Id;
                    }
                    else
                    {

                        transactionsViewModel.FinancialBookYearId = null;
                    }
                    #endregion
                    #region TransactionsRegion
                    transactionsViewModel.SaleId = SOid;
                    transactionsViewModel.OrganizationID = Orgid;
                    transactionsViewModel.BranchId = Branchid;
                    transactionsViewModel.DefaultSaleAccountId = AccountSaleId;
                    transactionsViewModel.AccountId = AccountCustomerId;
                    transactionsViewModel.PaymentType = PaymentType.SaleOrder;
                    transactionsViewModel.NetTotal = obj.NetTotal;
                    AddorUpdateTransactions(transactionsViewModel);
                    #endregion

                    #region PaymentMastRegion
                    paymentMasterView.OrganizationId = Convert.ToInt64(obj.OrganizationID);
                    paymentMasterView.BranchId = Convert.ToInt64(obj.BranchId);
                    paymentMasterView.SaleOrderId = Convert.ToInt64(obj.SaleOrdeID);
                    paymentMasterView.userID = userID;
                    paymentMasterView.SaleOrderDate = obj.SaleOrderDate;
                    #endregion

                    foreach (var item in Childitems)
                    {

                        var findResult = db.SaleOrderDetails.Find(item.SaleOrderDetailID);
                        if (findResult != null)
                        {
                            long StockId = 0;
                            if (item.ProductID != null && item.ProductID != 0)
                            {
                                long? differenceQTY = item.Quantity - findResult.Quantity;
                                var StockCode = stockDAL.AutoGenerateStockCode(Convert.ToInt64(item.ProductID), Branchid);
                                StockId = stockDAL.GetStockIDByStockCode(StockCode);

                                if (differenceQTY != 0)
                                {
                                    var Message = stockDAL.RemoveorUpdateStockSaleOrder(StockId, Convert.ToInt64(differenceQTY));
                                }

                            }
                            else
                            {
                                violationMessage = "Stock Not Available";
                            }
                            findResult.SaleOrderID = SOid;
                            findResult.StockId = StockId;
                            findResult.ProductID = item.ProductID;
                            findResult.Quantity = item.Quantity;
                            findResult.BalanceQuantity = item.Quantity;
                            findResult.UnitRate = item.UnitRate;
                            findResult.Total = item.Total;
                            findResult.MarkupAmount = item.MarkupAmount;
                            findResult.RemainingBalanceAmount = item.RemainingBalanceAmount;
                            findResult.MarkupPercentage = item.MarkupPercentage;
                            findResult.ProcessingFee = item.ProcessingFee;
                            findResult.DownPayment = item.DownPayment;
                            findResult.NoOfMonths = item.NoOfMonths;
                            findResult.InstallmentPerMonth = item.InstallmentPerMonth;
                            findResult.Formula = item.Formula;
                            findResult.Active = true;
                            db.Entry(findResult).State = EntityState.Modified;
                            db.SaveChanges();

                            paymentMasterView.SaleOrderDetailId = findResult.SaleOrderDetailID;
                            if (findResult.Formula == "TotalAmount")
                            {
                                paymentMasterView.RemainingBalanceAmount = findResult.Total - findResult.DownPayment;
                            }
                            else if (findResult.Formula == "RemainingAmount")
                            {
                                paymentMasterView.RemainingBalanceAmount = findResult.RemainingBalanceAmount;
                            }

                            paymentMasterView.ProductId = findResult.ProductID;
                            paymentMasterView.DownPayment = findResult.DownPayment;
                            paymentMasterView.NoOfMonths = findResult.NoOfMonths;
                            paymentMasterView.InstallmentPerMonth = findResult.InstallmentPerMonth;
                            paymentMasterView.BalanceAmount = findResult.InstallmentPerMonth - findResult.DownPayment;
                            #region UpdatePaymentMasterSection
                            UpdatePaymentMaster(paymentMasterView);
                            #endregion
                        }
                        else
                        {


                            SaleOrderDetail objChild = new SaleOrderDetail();
                            long StockId = 0;
                            if (item.ProductID != null && item.ProductID != 0)
                            {
                                var StockCode = stockDAL.AutoGenerateStockCode(Convert.ToInt64(item.ProductID), Branchid);
                                StockId = stockDAL.GetStockIDByStockCode(StockCode);
                                var Message = stockDAL.RemoveorUpdateStockSaleOrder(StockId, Convert.ToInt64(item.Quantity));
                            }
                            else
                            {
                                violationMessage = "Stock Not Available";
                            }
                            objChild.SaleOrderID = SOid;
                            objChild.StockId = StockId;
                            objChild.ProductID = item.ProductID;
                            objChild.Quantity = item.Quantity;
                            objChild.BalanceQuantity = item.Quantity;
                            objChild.UnitRate = item.UnitRate;
                            objChild.Total = item.Total;
                            objChild.RemainingBalanceAmount = item.RemainingBalanceAmount;
                            objChild.MarkupPercentage = item.MarkupPercentage;
                            objChild.MarkupAmount = item.MarkupAmount;
                            objChild.ProcessingFee = item.ProcessingFee;
                            objChild.DownPayment = item.DownPayment;
                            objChild.NoOfMonths = item.NoOfMonths;
                            objChild.InstallmentPerMonth = item.InstallmentPerMonth;
                            objChild.Formula = item.Formula;
                            objChild.Active = true;
                            db.SaleOrderDetails.Add(objChild);
                            db.SaveChanges();

                            paymentMasterView.SaleOrderDetailId = objChild.SaleOrderDetailID;
                            if (objChild.Formula == "TotalAmount")
                            {
                                paymentMasterView.RemainingBalanceAmount = objChild.Total - objChild.DownPayment;
                            }
                            else if (objChild.Formula == "RemainingAmount")
                            {
                                paymentMasterView.RemainingBalanceAmount = objChild.RemainingBalanceAmount;
                            }

                            paymentMasterView.ProductId = objChild.ProductID;
                            paymentMasterView.DownPayment = objChild.DownPayment;
                            paymentMasterView.NoOfMonths = objChild.NoOfMonths;
                            paymentMasterView.InstallmentPerMonth = objChild.InstallmentPerMonth;
                            paymentMasterView.BalanceAmount = objChild.InstallmentPerMonth - objChild.DownPayment;
                            #region PaymentMasterSection
                            AddPaymentMaster(paymentMasterView);
                            #endregion



                        }
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
        public string DeleteSaleOrder(SaleOrder saleOrder, long userID)
        {
            long SOid = 0;
            string violationMessage = "";
            SaleOrder obj = new SaleOrder();
            SaleOrderDetail objChild = new SaleOrderDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    obj = db.SaleOrders.Find(saleOrder.SaleOrdeID);
                    obj.IsDeleted = true;
                    obj.DeletedBy = userID;
                    obj.DeletedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    SOid = obj.SaleOrdeID;
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
        public string DeleteSaleOrderDetails(string iD)
        {
            long id = 0;
            string violationMessage = "";

            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (iD != "")
                    {
                        id = Convert.ToInt64(iD);
                    }

                    var findResult = db.SaleOrderDetails.Find(id);
                    var findObj = db.SaleOrders.Find(findResult.SaleOrderID);
                    db.SaleOrderDetails.Remove(findResult);
                    db.SaveChanges();
                    RunCalculateNetTotal(findObj);
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

        public void RunCalculateNetTotal(SaleOrder classObj)
        {
            var findListDetails = db.SaleOrderDetails.Where(a => a.SaleOrderID == classObj.SaleOrdeID).ToList();

            decimal SubTotal = 0;
            foreach (var item in findListDetails)
            {
                SubTotal += Convert.ToDecimal(item.Total);
            }
            classObj.NetTotal = SubTotal + classObj.FreightCharges;
            string AmountInWord = systemDAL.NumberToWords(Convert.ToInt64(classObj.NetTotal));
            AmountInWord = AmountInWord + " Only";
            classObj.AmountInWord = AmountInWord;

            db.Entry(classObj).State = EntityState.Modified;
            db.SaveChanges();

        }

        public SaleOrder GetSaleOrderById(long Id)
        {
            SaleOrder obj = new SaleOrder();

            obj = db.SaleOrders.Where(a => a.SaleOrdeID == Id).FirstOrDefault();

            return obj;
        }
        //-----------------------------------------------
        //=========== Payment Master=============
        //------------------------------------------------
        public void AddPaymentMaster(PaymentMasterViewModel viewModel)
        {
            try
            {
                PaymentMaster Obj = new PaymentMaster();
                long masterId = 0;
                int srno = 0;

                Obj.OrganizationId = viewModel.OrganizationId;
                Obj.BranchId = viewModel.BranchId;
                Obj.SaleOrderId = viewModel.SaleOrderId;
                Obj.ProductId = viewModel.ProductId;
                Obj.BalanceAmount = viewModel.BalanceAmount;
                Obj.SaleOrderDetailId = viewModel.SaleOrderDetailId;
                Obj.RemainingBalanceAmount = viewModel.RemainingBalanceAmount;
                db.PaymentMasters.Add(Obj);
                db.SaveChanges();
                masterId = Obj.Id;

                List<InstallmentsPaymentsScheduler> _listObject = new List<InstallmentsPaymentsScheduler>();
                for (int i = 0; i < viewModel.NoOfMonths; i++)
                {
                    srno = srno + 1;
                    InstallmentsPaymentsScheduler _Object = new InstallmentsPaymentsScheduler();
                    _Object.Id = Guid.NewGuid();
                    _Object.SrNo = srno;
                    _Object.OrganizationId = viewModel.OrganizationId;
                    _Object.PaymentMasterId = masterId;
                    _Object.PaymentDueDate = Convert.ToDateTime(viewModel.SaleOrderDate).AddMonths(i);
                    _Object.PerMonthAmount = viewModel.InstallmentPerMonth;
                    if (srno == 1 && viewModel.DownPayment != null && viewModel.DownPayment != 0)
                    {
                        _Object.PaidAmount = viewModel.DownPayment;
                        _Object.ReceivedDate = DateTime.Now;
                        _Object.PaymentStatus = PaymentStatus.UnPaid;
                        _Object.Notes = "Down Payment";
                    }
                    else
                    {
                        _Object.PaidAmount = 0;
                        _Object.ReceivedDate = null;
                        _Object.PaymentStatus = PaymentStatus.UnPaid;
                        _Object.Notes = "";
                    }

                    _Object.ExtraCharges = 0;
                    _Object.CreatedBy = viewModel.userID;
                    _Object.CreatedDate = DateTime.Now;

                    _listObject.Add(_Object);

                }
                db.InstallmentsPaymentsSchedulers.AddRange(_listObject);
                db.SaveChanges();


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public void UpdatePaymentMaster(PaymentMasterViewModel viewModel)
        {
            try
            {
                PaymentMaster Obj = db.PaymentMasters.Where(a => a.SaleOrderId == viewModel.SaleOrderId && a.SaleOrderDetailId == viewModel.SaleOrderDetailId).FirstOrDefault();
                long masterId = 0;
                int srno = 0;

                Obj.OrganizationId = viewModel.OrganizationId;
                Obj.BranchId = viewModel.BranchId;
                Obj.SaleOrderId = viewModel.SaleOrderId;
                Obj.ProductId = viewModel.ProductId;
                Obj.BalanceAmount = viewModel.BalanceAmount;
                Obj.SaleOrderDetailId = viewModel.SaleOrderDetailId;
                Obj.RemainingBalanceAmount = viewModel.RemainingBalanceAmount;
                db.Entry(Obj).State = EntityState.Modified;
                db.SaveChanges();
                masterId = Obj.Id;

                var findCountObjCount = db.InstallmentsPaymentsSchedulers.Where(a => a.PaymentMasterId == masterId && a.PaymentStatus == PaymentStatus.Paid).ToList().Count();
                if (findCountObjCount == 0)
                {
                    #region RemoveInstallmentsPaymentsRegion
                    var findListForRemove= db.InstallmentsPaymentsSchedulers.Where(a => a.PaymentMasterId == masterId).ToList();
                    db.InstallmentsPaymentsSchedulers.RemoveRange(findListForRemove);
                    db.SaveChanges();
                    #endregion


                    List<InstallmentsPaymentsScheduler> _listObject = new List<InstallmentsPaymentsScheduler>();
                    for (int i = 0; i < viewModel.NoOfMonths; i++)
                    {
                        srno = srno + 1;
                        InstallmentsPaymentsScheduler _Object = new InstallmentsPaymentsScheduler();
                        _Object.Id = Guid.NewGuid();
                        _Object.SrNo = srno;
                        _Object.OrganizationId = viewModel.OrganizationId;
                        _Object.PaymentMasterId = masterId;
                        _Object.PaymentDueDate = Convert.ToDateTime(viewModel.SaleOrderDate).AddMonths(i);
                        _Object.PerMonthAmount = viewModel.InstallmentPerMonth;
                        if (srno == 1 && viewModel.DownPayment != null && viewModel.DownPayment != 0)
                        {
                            _Object.PaidAmount = viewModel.DownPayment;
                            _Object.ReceivedDate = DateTime.Now;
                            _Object.PaymentStatus = PaymentStatus.UnPaid;
                            _Object.Notes = "Down Payment";
                        }
                        else
                        {
                            _Object.PaidAmount = 0;
                            _Object.ReceivedDate = null;
                            _Object.PaymentStatus = PaymentStatus.UnPaid;
                            _Object.Notes = "";
                        }

                        _Object.ExtraCharges = 0;
                        _Object.CreatedBy = viewModel.userID;
                        _Object.CreatedDate = DateTime.Now;

                        _listObject.Add(_Object);

                    }
                    db.InstallmentsPaymentsSchedulers.AddRange(_listObject);
                    db.SaveChanges();
                }
                else
                {
                    var findObj = db.InstallmentsPaymentsSchedulers.Where(a => a.PaymentMasterId == masterId && a.SrNo == 1).FirstOrDefault();
                    
                    if (viewModel.DownPayment != null && viewModel.DownPayment != 0)
                    {
                        findObj.PaidAmount = viewModel.DownPayment;
                        findObj.ReceivedDate = DateTime.Now;
                        findObj.Notes = "Down Payment Update";
                    }

                    findObj.UpdatedBy = viewModel.userID;
                    findObj.UpdatedDate = DateTime.Now;

                    db.Entry(findObj).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

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
                    objChild.ClosingBalance = item.ClosingBalance;
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

        public Nullable<decimal> GetClosingBalanceByAccount(long? OrgId, long? branchId, long? accountId, long? FBYId)
        {
            decimal? ClosingBalance = 0;
            var findSPObj = db.ProcGetClosingBalanceByAccount(OrgId, branchId, accountId, FBYId);
            ClosingBalance = findSPObj.ClosingBalance;
            return ClosingBalance;

        }
        public void AddorUpdateTransactions(TransactionsViewModel classObj)
        {
            string violationMessage = "";
            TransactionsDetail objFirst = new TransactionsDetail();
            TransactionsDetail objSecond = new TransactionsDetail();
            try
            {
                var findAccounts = db.AccountMapping.Where(a => a.Account.IsActive == true && a.Account.IsDeleted == false && a.OrganizationId == classObj.OrganizationID && a.AccountDefaultType == CommonEnums.AccountDefaultType.Sales).FirstOrDefault();

                var findObjFirst = db.TransactionsDetails.Where(a => a.SaleId == classObj.SaleId && a.PaymentType == PaymentType.SaleOrder).FirstOrDefault();
                var findObjLast = db.TransactionsDetails.Where(a => a.SaleId == classObj.SaleId && a.PaymentType == PaymentType.SaleOrder).OrderByDescending(a => a.TransactionDetailID).FirstOrDefault();

                if (findObjFirst != null && findObjLast != null)
                {
                    // First Object Save for Debit 
                    findObjFirst.OrganizationID = classObj.OrganizationID;
                    findObjFirst.BranchId = classObj.BranchId;
                    findObjFirst.PaymentType = classObj.PaymentType;//
                    findObjFirst.AccountId = classObj.AccountId;//
                    findObjFirst.SaleId = classObj.SaleId;
                    findObjFirst.FinancialBookYearId = classObj.FinancialBookYearId;
                    findObjFirst.SeqNo = 0;
                    findObjFirst.Narration = string.Empty;
                    findObjFirst.Debit = classObj.NetTotal;
                    findObjFirst.Credit = 0;
                    findObjFirst.TransactionMode = null;
                    findObjFirst.TrackNumber = 0;
                    findObjFirst.Posted = true;
                    findObjFirst.IsDeleted = false;
                    findObjFirst.UpdatedDate = DateTime.Now;
                    db.Entry(findObjFirst).State = EntityState.Modified;
                    db.SaveChanges();

                    // Second Object Save for Credit
                    findObjLast.OrganizationID = classObj.OrganizationID;
                    findObjLast.BranchId = classObj.BranchId;
                    findObjLast.PaymentType = PaymentType.SaleOrder;//
                    //Static Account Id Of General Sales Account Id=39

                    findObjLast.AccountId = classObj.DefaultSaleAccountId;
                    findObjLast.SaleId = classObj.SaleId;
                    findObjLast.FinancialBookYearId = classObj.FinancialBookYearId;

                    findObjLast.SeqNo = 0;
                    findObjLast.Narration = string.Empty;
                    findObjLast.Debit = 0;
                    findObjLast.Credit = classObj.NetTotal;
                    findObjLast.TransactionMode = null;
                    findObjLast.TrackNumber = 0;
                    findObjLast.Posted = true;
                    findObjLast.IsDeleted = false;
                    findObjLast.UpdatedDate = DateTime.Now;
                    db.Entry(findObjLast).State = EntityState.Modified;
                    db.SaveChanges();

                }
                else
                {
                    // First Object Save for Debit 
                    objFirst.OrganizationID = classObj.OrganizationID;
                    objFirst.BranchId = classObj.BranchId;
                    objFirst.PaymentType = classObj.PaymentType;//
                    objFirst.AccountId = classObj.AccountId;//
                    objFirst.SaleId = classObj.SaleId;

                    objFirst.FinancialBookYearId = classObj.FinancialBookYearId;

                    objFirst.SeqNo = 0;
                    objFirst.Narration = string.Empty;
                    objFirst.Debit = classObj.NetTotal;
                    objFirst.Credit = 0;
                    objFirst.TransactionMode = null;
                    objFirst.TrackNumber = 0;
                    objFirst.Posted = true;
                    objFirst.IsDeleted = false;
                    objFirst.CreatedDate = DateTime.Now;
                    db.TransactionsDetails.Add(objFirst);
                    db.SaveChanges();

                    // Second Object Save for Credit
                    objFirst.OrganizationID = classObj.OrganizationID;
                    objFirst.BranchId = classObj.BranchId;
                    objFirst.PaymentType = classObj.PaymentType;//
                                                                //Static Account Id Of General Sales Account Id=39

                    objFirst.AccountId = classObj.DefaultSaleAccountId;//

                    objFirst.SaleId = classObj.SaleId;

                    objFirst.FinancialBookYearId = classObj.FinancialBookYearId;

                    objFirst.SeqNo = 0;
                    objFirst.Narration = string.Empty;
                    objFirst.Debit = 0;
                    objFirst.Credit = classObj.NetTotal;
                    objFirst.TransactionMode = null;
                    objFirst.TrackNumber = 0;
                    objFirst.Posted = true;
                    objFirst.IsDeleted = false;
                    objFirst.CreatedDate = DateTime.Now;
                    db.TransactionsDetails.Add(objFirst);
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
    }
}