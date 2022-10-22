using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.AppDAL
{
    public class VouchersDAL
    {
        AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        //--------- Cash Payment Voucher
        public string AddCashPaymentVoucher(string ObjMasterItem, string ObjChildItem, long userID)
        {
            long id = 0;
            string violationMessage = "";
            VouchersHead obj = new VouchersHead();
            VouchersDetail objChild = new VouchersDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    VouchersHead _Object = JsonConvert.DeserializeObject<VouchersHead>(ObjMasterItem);
                    List<VouchersDetail> Childitems = JsonConvert.DeserializeObject<List<VouchersDetail>>(ObjChildItem);

                    obj.OrganizationID = _Object.OrganizationID;
                    obj.BranchId = _Object.BranchId;
                    obj.TransactionKey = Guid.NewGuid();
                    obj.VoucherCode = _Object.VoucherCode;
                    obj.VoucherDate = _Object.VoucherDate;
                    obj.VoucherType = CommonEnums.VoucherType.CPV;
                    obj.PaymentType = _Object.PaymentType;
                    obj.PurchaseId = _Object.PurchaseId;
                    obj.SheetNo = _Object.SheetNo;
                    obj.FinancialBookYearId = _Object.FinancialBookYearId;
                    obj.TerminalNo = _Object.TerminalNo;
                    obj.ChequeNo = _Object.ChequeNo;
                    obj.TotalAmount = _Object.TotalAmount;
                    obj.Posted = _Object.Posted;
                    obj.Description = _Object.Description;

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.CreatedBy = userID;
                    obj.CreatedDate = DateTime.Now;
                    db.VouchersHeads.Add(obj);
                    db.SaveChanges();
                    id = obj.VoucherID;

                    foreach (var item in Childitems)
                    {
                        objChild.VoucherId = id;
                        objChild.AccountId = item.AccountId;
                        objChild.ClosingBalance = item.ClosingBalance;
                        objChild.Narration = item.Narration;
                        objChild.Debit = item.Debit;
                        objChild.Credit = item.Credit;

                        db.VouchersDetails.Add(objChild);
                        db.SaveChanges();
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
        public string UpdateCashPaymentVoucher(string ObjMasterItem, string ObjChildItem, long userID)
        {
            long id = 0;
            string violationMessage = "";
            VouchersHead obj = new VouchersHead();
            VouchersDetail objChild = new VouchersDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    VouchersHead _Object = JsonConvert.DeserializeObject<VouchersHead>(ObjMasterItem);
                    List<VouchersDetail> Childitems = JsonConvert.DeserializeObject<List<VouchersDetail>>(ObjChildItem);

                    obj = db.VouchersHeads.Find(_Object.VoucherID);

                    obj.OrganizationID = _Object.OrganizationID;
                    obj.BranchId = _Object.BranchId;
                    obj.VoucherCode = _Object.VoucherCode;
                    obj.VoucherDate = _Object.VoucherDate;
                    obj.VoucherType = CommonEnums.VoucherType.CPV;
                    // obj.PaymentType = _Object.PaymentType;
                    // obj.PurchaseId = _Object.PurchaseId;
                    obj.SheetNo = _Object.SheetNo;
                    obj.FinancialBookYearId = _Object.FinancialBookYearId;
                    obj.TerminalNo = _Object.TerminalNo;
                    obj.ChequeNo = _Object.ChequeNo;
                    obj.TotalAmount = _Object.TotalAmount;
                    obj.Posted = _Object.Posted;
                    obj.Description = _Object.Description;

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.UpdatedBy = userID;
                    obj.UpdatedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    id = obj.VoucherID;

                    foreach (var item in Childitems)
                    {
                        var findObj = db.VouchersDetails.Where(a => a.VoucherDetailID == item.VoucherDetailID).FirstOrDefault();
                        if (findObj != null)
                        {
                            findObj.VoucherId = id;
                            findObj.AccountId = item.AccountId;
                            findObj.ClosingBalance = item.ClosingBalance;
                            findObj.Narration = item.Narration;
                            findObj.Debit = item.Debit;
                            findObj.Credit = item.Credit;
                            db.Entry(findObj).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            objChild.VoucherId = id;
                            objChild.AccountId = item.AccountId;
                            objChild.ClosingBalance = item.ClosingBalance;
                            objChild.Narration = item.Narration;
                            objChild.Debit = item.Debit;
                            objChild.Credit = item.Credit;

                            db.VouchersDetails.Add(objChild);
                            db.SaveChanges();
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
        public string DeleteCashPaymentVoucher(VouchersHead vouchersHead, long userID)
        {
            long id = 0;
            string violationMessage = "";
            VouchersHead obj = new VouchersHead();
            VouchersDetail objChild = new VouchersDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {


                    obj = db.VouchersHeads.Find(vouchersHead.VoucherID);
                    obj.IsDeleted = true;
                    obj.DeletedBy = userID;
                    obj.DeletedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    id = obj.VoucherID;

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
        //--------- Cash Received Voucher
        public string AddCashReceivedVoucher(string ObjMasterItem, string ObjChildItem, long userID)
        {
            long vId = 0;
            string violationMessage = "";
            VouchersHead obj = new VouchersHead();
            VouchersDetail objChild = new VouchersDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    VouchersHead _Object = JsonConvert.DeserializeObject<VouchersHead>(ObjMasterItem);
                    List<VouchersDetail> Childitems = JsonConvert.DeserializeObject<List<VouchersDetail>>(ObjChildItem);

                    obj.OrganizationID = _Object.OrganizationID;
                    obj.BranchId = _Object.BranchId;
                    obj.VoucherCode = _Object.VoucherCode;
                    obj.TransactionKey = Guid.NewGuid();
                    obj.VoucherDate = _Object.VoucherDate;
                    obj.VoucherType = CommonEnums.VoucherType.CRV;
                    obj.PaymentType = _Object.PaymentType;
                    obj.SaleId = _Object.SaleId;
                    obj.SheetNo = _Object.SheetNo;
                    obj.FinancialBookYearId = _Object.FinancialBookYearId;
                    obj.TerminalNo = _Object.TerminalNo;
                    obj.ChequeNo = _Object.ChequeNo;
                    obj.TotalAmount = _Object.TotalAmount;
                    obj.Posted = _Object.Posted;
                    obj.Description = _Object.Description;

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.CreatedBy = userID;
                    obj.CreatedDate = DateTime.Now;
                    db.VouchersHeads.Add(obj);
                    db.SaveChanges();
                    vId = obj.VoucherID;

                    foreach (var item in Childitems)
                    {
                        objChild.VoucherId = vId;
                        objChild.AccountId = item.AccountId;
                        objChild.ClosingBalance = item.ClosingBalance;
                        objChild.Narration = item.Narration;
                        objChild.Debit = item.Debit;
                        objChild.Credit = item.Credit;
                        db.VouchersDetails.Add(objChild);
                        db.SaveChanges();
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

        public string UpdateCashReceivedVoucher(string ObjMasterItem, string ObjChildItem, long userID)
        {
            long id = 0;
            string violationMessage = "";
            VouchersHead obj = new VouchersHead();
            VouchersDetail objChild = new VouchersDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    VouchersHead _Object = JsonConvert.DeserializeObject<VouchersHead>(ObjMasterItem);
                    List<VouchersDetail> Childitems = JsonConvert.DeserializeObject<List<VouchersDetail>>(ObjChildItem);

                    obj = db.VouchersHeads.Find(_Object.VoucherID);

                    obj.OrganizationID = _Object.OrganizationID;
                    obj.BranchId = _Object.BranchId;
                    //obj.VoucherCode = _Object.VoucherCode;
                    obj.VoucherDate = _Object.VoucherDate;
                    obj.VoucherType = CommonEnums.VoucherType.CRV;
                    //obj.PaymentType = _Object.PaymentType;
                    //obj.SaleId = _Object.SaleId;
                    obj.SheetNo = _Object.SheetNo;
                    obj.FinancialBookYearId = _Object.FinancialBookYearId;
                    obj.TerminalNo = _Object.TerminalNo;
                    obj.ChequeNo = _Object.ChequeNo;
                    obj.TotalAmount = _Object.TotalAmount;
                    obj.Posted = _Object.Posted;
                    obj.Description = _Object.Description;

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.UpdatedBy = userID;
                    obj.UpdatedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    id = obj.VoucherID;
                    foreach (var item in Childitems)
                    {
                        var findObj = db.VouchersDetails.Where(a => a.VoucherDetailID == item.VoucherDetailID).FirstOrDefault();
                        if (findObj != null)
                        {
                            findObj.VoucherId = id;
                            findObj.AccountId = item.AccountId;
                            findObj.ClosingBalance = item.ClosingBalance;
                            findObj.Narration = item.Narration;
                            findObj.Debit = item.Debit;
                            findObj.Credit = item.Credit;
                            db.Entry(findObj).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            objChild.VoucherId = id;
                            objChild.AccountId = item.AccountId;
                            objChild.ClosingBalance = item.ClosingBalance;
                            objChild.Narration = item.Narration;
                            objChild.Debit = item.Debit;
                            objChild.Credit = item.Credit;

                            db.VouchersDetails.Add(objChild);
                            db.SaveChanges();
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
        public string DeleteCashReceivedVoucher(VouchersHead vouchersHead, long userID)
        {
            long id = 0;
            string violationMessage = "";
            VouchersHead obj = new VouchersHead();
            VouchersDetail objChild = new VouchersDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {


                    obj = db.VouchersHeads.Find(vouchersHead.VoucherID);
                    obj.IsDeleted = true;
                    obj.DeletedBy = userID;
                    obj.DeletedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    id = obj.VoucherID;

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
        //--------- Bank Payment Voucher
        public string AddBankPaymentVoucher(string ObjMasterItem, string ObjChildItem, long userID)
        {
            long id = 0;
            string violationMessage = "";
            VouchersHead obj = new VouchersHead();
            VouchersDetail objChild = new VouchersDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    VouchersHead _Object = JsonConvert.DeserializeObject<VouchersHead>(ObjMasterItem);
                    List<VouchersDetail> Childitems = JsonConvert.DeserializeObject<List<VouchersDetail>>(ObjChildItem);

                    obj.OrganizationID = _Object.OrganizationID;
                    obj.BranchId = _Object.BranchId;
                    obj.VoucherCode = _Object.VoucherCode;
                    obj.TransactionKey = Guid.NewGuid();
                    obj.VoucherDate = _Object.VoucherDate;
                    obj.VoucherType = CommonEnums.VoucherType.BPV;
                    obj.PaymentType = _Object.PaymentType;
                    obj.PurchaseId = _Object.PurchaseId;
                    obj.SheetNo = _Object.SheetNo;
                    obj.FinancialBookYearId = _Object.FinancialBookYearId;
                    obj.TerminalNo = _Object.TerminalNo;
                    obj.ChequeNo = _Object.ChequeNo;
                    obj.TotalAmount = _Object.TotalAmount;
                    obj.Posted = _Object.Posted;
                    obj.Description = _Object.Description;

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.CreatedBy = userID;
                    obj.CreatedDate = DateTime.Now;
                    db.VouchersHeads.Add(obj);
                    db.SaveChanges();
                    id = obj.VoucherID;
                    foreach (var item in Childitems)
                    {
                        objChild.VoucherId = id;
                        objChild.AccountId = item.AccountId;
                        objChild.ClosingBalance = item.ClosingBalance;
                        objChild.Narration = item.Narration;
                        objChild.Debit = item.Debit;
                        objChild.Credit = item.Credit;

                        db.VouchersDetails.Add(objChild);
                        db.SaveChanges();
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
        public string UpdateBankPaymentVoucher(string ObjMasterItem, string ObjChildItem, long userID)
        {
            long id = 0;
            string violationMessage = "";
            VouchersHead obj = new VouchersHead();
            VouchersDetail objChild = new VouchersDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    VouchersHead _Object = JsonConvert.DeserializeObject<VouchersHead>(ObjMasterItem);
                    List<VouchersDetail> Childitems = JsonConvert.DeserializeObject<List<VouchersDetail>>(ObjChildItem);

                    obj = db.VouchersHeads.Find(_Object.VoucherID);

                    obj.OrganizationID = _Object.OrganizationID;
                    obj.VoucherCode = _Object.VoucherCode;
                    obj.VoucherDate = _Object.VoucherDate;
                    obj.VoucherType = CommonEnums.VoucherType.BPV;
                    //obj.PaymentType = _Object.PaymentType;
                    //obj.PurchaseId = _Object.PurchaseId;
                    obj.SheetNo = _Object.SheetNo;
                    obj.FinancialBookYearId = _Object.FinancialBookYearId;
                    obj.TerminalNo = _Object.TerminalNo;
                    obj.ChequeNo = _Object.ChequeNo;
                    obj.TotalAmount = _Object.TotalAmount;
                    obj.Posted = _Object.Posted;
                    obj.Description = _Object.Description;

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.UpdatedBy = userID;
                    obj.UpdatedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    id = obj.VoucherID;

                    foreach (var item in Childitems)
                    {
                        var findObj = db.VouchersDetails.Where(a => a.VoucherDetailID == item.VoucherDetailID).FirstOrDefault();
                        if (findObj != null)
                        {
                            findObj.VoucherId = id;
                            findObj.AccountId = item.AccountId;
                            findObj.ClosingBalance = item.ClosingBalance;
                            findObj.Narration = item.Narration;
                            findObj.Debit = item.Debit;
                            findObj.Credit = item.Credit;
                            db.Entry(findObj).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            objChild.VoucherId = id;
                            objChild.AccountId = item.AccountId;
                            objChild.ClosingBalance = item.ClosingBalance;
                            objChild.Narration = item.Narration;
                            objChild.Debit = item.Debit;
                            objChild.Credit = item.Credit;

                            db.VouchersDetails.Add(objChild);
                            db.SaveChanges();
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
        public string DeleteBankPaymentVoucher(VouchersHead vouchersHead, long userID)
        {
            long id = 0;
            string violationMessage = "";
            VouchersHead obj = new VouchersHead();
            VouchersDetail objChild = new VouchersDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {


                    obj = db.VouchersHeads.Find(vouchersHead.VoucherID);
                    obj.IsDeleted = true;
                    obj.DeletedBy = userID;
                    obj.DeletedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    id = obj.VoucherID;

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
        //--------- Bank Received Voucher
        public string AddBankReceivedVoucher(string ObjMasterItem, string ObjChildItem, long userID)
        {
            long id = 0;
            string violationMessage = "";
            VouchersHead obj = new VouchersHead();
            VouchersDetail objChild = new VouchersDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    VouchersHead _Object = JsonConvert.DeserializeObject<VouchersHead>(ObjMasterItem);
                    List<VouchersDetail> Childitems = JsonConvert.DeserializeObject<List<VouchersDetail>>(ObjChildItem);


                    obj.OrganizationID = _Object.OrganizationID;
                    obj.BranchId = _Object.BranchId;
                    obj.VoucherCode = _Object.VoucherCode;
                    obj.TransactionKey = Guid.NewGuid();
                    obj.VoucherDate = _Object.VoucherDate;
                    obj.VoucherType = CommonEnums.VoucherType.BRV;
                    obj.PaymentType = _Object.PaymentType;
                    obj.SaleId = _Object.SaleId;
                    obj.SheetNo = _Object.SheetNo;
                    obj.FinancialBookYearId = _Object.FinancialBookYearId;
                    obj.TerminalNo = _Object.TerminalNo;
                    obj.ChequeNo = _Object.ChequeNo;
                    obj.TotalAmount = _Object.TotalAmount;
                    obj.Posted = _Object.Posted;
                    obj.Description = _Object.Description;

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.CreatedBy = userID;
                    obj.CreatedDate = DateTime.Now;
                    db.VouchersHeads.Add(obj);
                    db.SaveChanges();
                    id = obj.VoucherID;

                    foreach (var item in Childitems)
                    {
                        objChild.VoucherId = id;
                        objChild.AccountId = item.AccountId;
                        objChild.ClosingBalance = item.ClosingBalance;
                        objChild.Narration = item.Narration;
                        objChild.Debit = item.Debit;
                        objChild.Credit = item.Credit;
                        db.VouchersDetails.Add(objChild);
                        db.SaveChanges();
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
        public string UpdateBankReceivedVoucher(string ObjMasterItem, string ObjChildItem, long userID)
        {
            long id = 0;
            string violationMessage = "";
            VouchersHead obj = new VouchersHead();
            VouchersDetail objChild = new VouchersDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    VouchersHead _Object = JsonConvert.DeserializeObject<VouchersHead>(ObjMasterItem);
                    obj = db.VouchersHeads.Find(_Object.VoucherID);

                    obj.OrganizationID = _Object.OrganizationID;
                    //obj.VoucherCode = _Object.VoucherCode;
                    obj.VoucherDate = _Object.VoucherDate;
                    obj.VoucherType = CommonEnums.VoucherType.BRV;
                    // obj.PaymentType = _Object.PaymentType;
                    //obj.SaleId = _Object.SaleId;
                    obj.SheetNo = _Object.SheetNo;
                    obj.FinancialBookYearId = _Object.FinancialBookYearId;
                    obj.TerminalNo = _Object.TerminalNo;
                    obj.ChequeNo = _Object.ChequeNo;
                    obj.TotalAmount = _Object.TotalAmount;
                    obj.Posted = _Object.Posted;
                    obj.Description = _Object.Description;

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.UpdatedBy = userID;
                    obj.UpdatedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    id = obj.VoucherID;

                    List<VouchersDetail> Childitems = JsonConvert.DeserializeObject<List<VouchersDetail>>(ObjChildItem);

                    foreach (var item in Childitems)
                    {
                        var findObj = db.VouchersDetails.Where(a => a.VoucherDetailID == item.VoucherDetailID).FirstOrDefault();
                        if (findObj != null)
                        {
                            findObj.VoucherId = id;
                            findObj.AccountId = item.AccountId;
                            findObj.ClosingBalance = item.ClosingBalance;
                            findObj.Narration = item.Narration;
                            findObj.Debit = item.Debit;
                            findObj.Credit = item.Credit;
                            db.Entry(findObj).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            objChild.VoucherId = id;
                            objChild.AccountId = item.AccountId;
                            objChild.ClosingBalance = item.ClosingBalance;
                            objChild.Narration = item.Narration;
                            objChild.Debit = item.Debit;
                            objChild.Credit = item.Credit;

                            db.VouchersDetails.Add(objChild);
                            db.SaveChanges();
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
        public string DeleteBankReceivedVoucher(VouchersHead vouchersHead, long userID)
        {
            long id = 0;
            string violationMessage = "";
            VouchersHead obj = new VouchersHead();
            VouchersDetail objChild = new VouchersDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {


                    obj = db.VouchersHeads.Find(vouchersHead.VoucherID);
                    obj.IsDeleted = true;
                    obj.DeletedBy = userID;
                    obj.DeletedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    id = obj.VoucherID;

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
        //--------- Journal Voucher
        public string AddJournalVoucher(string ObjMasterItem, string ObjChildItem, long userID)
        {
            long id = 0;
            string violationMessage = "";
            VouchersHead obj = new VouchersHead();
            VouchersDetail objChild = new VouchersDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    VouchersHead _Object = JsonConvert.DeserializeObject<VouchersHead>(ObjMasterItem);

                    obj.OrganizationID = _Object.OrganizationID;
                    obj.BranchId = _Object.BranchId;
                    obj.VoucherCode = _Object.VoucherCode;
                    obj.TransactionKey = Guid.NewGuid();
                    obj.VoucherDate = _Object.VoucherDate;
                    obj.VoucherType = CommonEnums.VoucherType.JV;
                    obj.PaymentType = _Object.PaymentType;
                    obj.SaleId = _Object.SaleId;
                    obj.PurchaseId = _Object.PurchaseId;
                    obj.SheetNo = _Object.SheetNo;
                    obj.FinancialBookYearId = _Object.FinancialBookYearId;
                    obj.TerminalNo = _Object.TerminalNo;
                    obj.ChequeNo = _Object.ChequeNo;
                    obj.TotalAmount = _Object.TotalAmount;
                    obj.Posted = _Object.Posted;
                    obj.Description = _Object.Description;

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.CreatedBy = userID;
                    obj.CreatedDate = DateTime.Now;
                    db.VouchersHeads.Add(obj);
                    db.SaveChanges();
                    id = obj.VoucherID;

                    List<VouchersDetail> Childitems = JsonConvert.DeserializeObject<List<VouchersDetail>>(ObjChildItem);

                    foreach (var item in Childitems)
                    {
                        objChild.VoucherId = id;
                        objChild.AccountId = item.AccountId;
                        objChild.ClosingBalance = item.ClosingBalance;
                        objChild.Narration = item.Narration;
                        objChild.Debit = item.Debit;
                        objChild.Credit = item.Credit;
                        db.VouchersDetails.Add(objChild);
                        db.SaveChanges();
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
        public string UpdateJournalVoucher(string ObjMasterItem, string ObjChildItem, long userID)
        {
            long id = 0;
            string violationMessage = "";
            VouchersHead obj = new VouchersHead();
            VouchersDetail objChild = new VouchersDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    VouchersHead _Object = JsonConvert.DeserializeObject<VouchersHead>(ObjMasterItem);


                    obj = db.VouchersHeads.Find(_Object.VoucherID);

                    obj.OrganizationID = _Object.OrganizationID;
                    obj.BranchId = _Object.BranchId;
                    obj.VoucherCode = _Object.VoucherCode;
                    obj.VoucherDate = _Object.VoucherDate;
                    obj.VoucherType = CommonEnums.VoucherType.JV;
                    //obj.PaymentType = _Object.PaymentType;
                    //obj.SaleId = _Object.SaleId;
                    //obj.PurchaseId = _Object.PurchaseId;
                    obj.SheetNo = _Object.SheetNo;
                    obj.FinancialBookYearId = _Object.FinancialBookYearId;
                    obj.TerminalNo = _Object.TerminalNo;
                    obj.ChequeNo = _Object.ChequeNo;
                    obj.TotalAmount = _Object.TotalAmount;
                    obj.Posted = _Object.Posted;
                    obj.Description = _Object.Description;

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.UpdatedBy = userID;
                    obj.UpdatedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    id = obj.VoucherID;

                    List<VouchersDetail> Childitems = JsonConvert.DeserializeObject<List<VouchersDetail>>(ObjChildItem);

                    foreach (var item in Childitems)
                    {
                        var findObj = db.VouchersDetails.Where(a => a.VoucherDetailID == item.VoucherDetailID).FirstOrDefault();
                        if (findObj != null)
                        {
                            findObj.VoucherId = id;
                            findObj.AccountId = item.AccountId;
                            findObj.ClosingBalance = item.ClosingBalance;
                            findObj.Narration = item.Narration;
                            findObj.Debit = item.Debit;
                            findObj.Credit = item.Credit;
                            db.Entry(findObj).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            objChild.VoucherId = id;
                            objChild.AccountId = item.AccountId;
                            objChild.ClosingBalance = item.ClosingBalance;
                            objChild.Narration = item.Narration;
                            objChild.Debit = item.Debit;
                            objChild.Credit = item.Credit;

                            db.VouchersDetails.Add(objChild);
                            db.SaveChanges();
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
        public string DeleteJournalVoucher(VouchersHead vouchersHead, long userID)
        {
            long id = 0;
            string violationMessage = "";
            VouchersHead obj = new VouchersHead();
            VouchersDetail objChild = new VouchersDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {


                    obj = db.VouchersHeads.Find(vouchersHead.VoucherID);
                    obj.IsDeleted = true;
                    obj.DeletedBy = userID;
                    obj.DeletedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    id = obj.VoucherID;

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
    }
}