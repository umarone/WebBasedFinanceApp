using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Infrastructure.AppServices;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.AppDAL
{
    public class PurchaseOrderDAL
    {
        AppEntities db = new AppEntities();
        FilesUploadService filesUploadService = new FilesUploadService();
        SystemDAL systemDAL = new SystemDAL();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        public string AddPurchaseOrder(string ObjMasterItem, string ObjChildItem, HttpPostedFileBase POfile, long userID)
        {
            long poid = 0;
            byte[] Imagebytes;
            string violationMessage = "";
            string fileName = "PO_";
            string folderName = "PurchaseOrders";

            PurchaseOrder obj = new PurchaseOrder();
            PurchaseOrderDetail objChild = new PurchaseOrderDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    PurchaseOrder POObj = JsonConvert.DeserializeObject<PurchaseOrder>(ObjMasterItem);


                    if (POfile != null)
                    {
                        using (BinaryReader br = new BinaryReader(POfile.InputStream))
                        {
                            Imagebytes = br.ReadBytes(POfile.ContentLength);
                            obj.PurchaseOrderImage = Imagebytes;
                        }
                        Stream stream = new MemoryStream(Imagebytes);
                        string extension = Path.GetExtension(POfile.FileName);
                        var filePath = filesUploadService.UploadImages(fileName + POObj.PurchaseOrderNO + "_", stream, folderName, extension);
                        obj.FilePath = filePath.ToString();
                    }
                    obj.OrganizationID = POObj.OrganizationID;
                    obj.BranchId = POObj.BranchId;
                    obj.SupplierID = POObj.SupplierID;
                    obj.AccountID = POObj.AccountID;
                    obj.CurrencyID = POObj.CurrencyID;
                    obj.PurchaseOrderNO = dALCode.AutoGeneratePurchaseOrderCode(Convert.ToInt64(POObj.OrganizationID),Convert.ToInt64(POObj.BranchId));
                    obj.PurchaseOrderNO = POObj.PurchaseOrderNO;
                    obj.PuchaseOrderType = POObj.PuchaseOrderType;
                    obj.PurchaseOrderDate = POObj.PurchaseOrderDate;
                    obj.TransactionType = POObj.TransactionType;
                    obj.PaymentTerms = POObj.PaymentTerms;
                    obj.DeliveryDate = POObj.DeliveryDate;
                    obj.SubTotalWithOutSaleTax = POObj.SubTotalWithOutSaleTax;
                    obj.SubTotalWithSaleTax = POObj.SubTotalWithSaleTax;
                    obj.FreightCharges = POObj.FreightCharges;
                    obj.NetTotal = POObj.NetTotal;
                    obj.AmountInWord = POObj.AmountInWord;
                    obj.TermAndCondition = POObj.TermAndCondition;

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.CreatedBy = userID;
                    obj.CreatedDate = DateTime.Now;

                    db.PurchaseOrders.Add(obj);
                    db.SaveChanges();
                    poid = obj.PurchaseOrderId;
                    violationMessage = AddorUpdateTransactions(obj);


                    List<PurchaseOrderDetail> Childitems = JsonConvert.DeserializeObject<List<PurchaseOrderDetail>>(ObjChildItem);

                    foreach (var item in Childitems)
                    {
                        if (item.ProductID !=null)
                        {
                            objChild.PurchaseOrderID = poid;
                            objChild.ProductID = item.ProductID;
                            objChild.AgingId = item.AgingId;
                            objChild.Quantity = item.Quantity;
                            objChild.BalanceQuantity = item.Quantity;
                            objChild.UnitRate = item.UnitRate;
                            objChild.TotalWithOutSaleTax = item.TotalWithOutSaleTax;
                            objChild.DiscountPercentage = item.DiscountPercentage;
                            objChild.DiscountAmount = item.DiscountAmount;
                            objChild.DiscountedUnitRate = item.DiscountedUnitRate;
                            objChild.TotalAfterDiscount = item.TotalAfterDiscount;
                            objChild.SaleTaxPercentage = item.SaleTaxPercentage;
                            objChild.SaleTaxAmount = item.SaleTaxAmount;
                            objChild.TotalTaxInclusive = item.TotalTaxInclusive;
                            objChild.Active = true;
                            db.PurchaseOrderDetails.Add(objChild);
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
        public string UpdatePurchaseOrder(string objMasterItem, string objChilds, HttpPostedFileBase pOfile, long userID)
        {
            long poid = 0;
            string violationMessage = "";
            string fileName = "PO_";
            string folderName = "PurchaseOrders";
            PurchaseOrder obj = new PurchaseOrder();
            PurchaseOrderDetail objChild = new PurchaseOrderDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    byte[] Imagebytes;

                    PurchaseOrder POObj = JsonConvert.DeserializeObject<PurchaseOrder>(objMasterItem);


                    obj = db.PurchaseOrders.Find(POObj.PurchaseOrderId);
                    if (pOfile != null)
                    {
                        using (BinaryReader br = new BinaryReader(pOfile.InputStream))
                        {
                            Imagebytes = br.ReadBytes(pOfile.ContentLength);
                            obj.PurchaseOrderImage = Imagebytes;
                        }
                        Stream stream = new MemoryStream(Imagebytes);
                        string extension = Path.GetExtension(pOfile.FileName);
                        var filePath = filesUploadService.UploadImages(fileName + POObj.PurchaseOrderNO + "_", stream, folderName, extension);
                        obj.FilePath = filePath.ToString();
                    }
                    obj.OrganizationID = POObj.OrganizationID;
                    obj.BranchId = POObj.BranchId;
                    obj.SupplierID = POObj.SupplierID;
                    obj.AccountID = POObj.AccountID;
                    obj.CurrencyID = POObj.CurrencyID;
                    obj.PurchaseOrderNO = POObj.PurchaseOrderNO;
                    obj.PuchaseOrderType = POObj.PuchaseOrderType;
                    obj.PurchaseOrderDate = POObj.PurchaseOrderDate;
                    obj.TransactionType = POObj.TransactionType;
                    obj.PaymentTerms = POObj.PaymentTerms;
                    obj.DeliveryDate = POObj.DeliveryDate;

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.UpdatedBy = userID;
                    obj.UpdatedDate = DateTime.Now;
                    obj.SubTotalWithOutSaleTax = POObj.SubTotalWithOutSaleTax;
                    obj.SubTotalWithSaleTax = POObj.SubTotalWithSaleTax;
                    obj.FreightCharges = POObj.FreightCharges;
                    obj.NetTotal = POObj.NetTotal;
                    obj.AmountInWord = POObj.AmountInWord;
                    obj.TermAndCondition = POObj.TermAndCondition;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    poid = obj.PurchaseOrderId;
                    violationMessage = AddorUpdateTransactions(obj);

                    List<PurchaseOrderDetail> Childitems = JsonConvert.DeserializeObject<List<PurchaseOrderDetail>>(objChilds);

                    foreach (var item in Childitems)
                    {
                        var findResult = db.PurchaseOrderDetails.Find(item.PurchaseOrderDetailID);
                        if (findResult != null)
                        {
                            if (item.ProductID !=null)
                            {
                                findResult.PurchaseOrderID = poid;
                                findResult.ProductID = item.ProductID;
                                findResult.AgingId = item.AgingId;
                                findResult.Quantity = item.Quantity;
                                findResult.BalanceQuantity = item.Quantity;
                                findResult.UnitRate = item.UnitRate;
                                findResult.TotalWithOutSaleTax = item.TotalWithOutSaleTax;
                                findResult.DiscountPercentage = item.DiscountPercentage;
                                findResult.DiscountAmount = item.DiscountAmount;
                                findResult.DiscountedUnitRate = item.DiscountedUnitRate;
                                findResult.TotalAfterDiscount = item.TotalAfterDiscount;
                                findResult.SaleTaxPercentage = item.SaleTaxPercentage;
                                findResult.SaleTaxAmount = item.SaleTaxAmount;
                                findResult.TotalTaxInclusive = item.TotalTaxInclusive;
                                findResult.Active = true;
                                db.Entry(findResult).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                           
                        }
                        else
                        {
                            if (item.ProductID !=null)
                            {
                                objChild.PurchaseOrderID = poid;
                                objChild.ProductID = item.ProductID;
                                objChild.AgingId = item.AgingId;
                                objChild.Quantity = item.Quantity;
                                objChild.BalanceQuantity = item.Quantity;
                                objChild.UnitRate = item.UnitRate;
                                objChild.TotalWithOutSaleTax = item.TotalWithOutSaleTax;
                                objChild.DiscountPercentage = item.DiscountPercentage;
                                objChild.DiscountAmount = item.DiscountAmount;
                                objChild.DiscountedUnitRate = item.DiscountedUnitRate;
                                objChild.TotalAfterDiscount = item.TotalAfterDiscount;
                                objChild.SaleTaxPercentage = item.SaleTaxPercentage;
                                objChild.SaleTaxAmount = item.SaleTaxAmount;
                                objChild.TotalTaxInclusive = item.TotalTaxInclusive;
                                objChild.Active = true;
                                db.PurchaseOrderDetails.Add(objChild);
                                db.SaveChanges();
                            }
                           
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
        public string DeletePurchaseOrder(PurchaseOrder purchaseOrder, long userID)
        {

            string violationMessage = "";
            PurchaseOrder obj = new PurchaseOrder();
            PurchaseOrderDetail objChild = new PurchaseOrderDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    obj = db.PurchaseOrders.Find(purchaseOrder.PurchaseOrderId);

                    obj.IsDeleted = true;
                    obj.DeletedBy = userID;
                    obj.DeletedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                   
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
        public string DeletePurchaseOrderDetails(string iD)
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
                  
                    var findResult = db.PurchaseOrderDetails.Find(id);
                    var findObj = db.PurchaseOrders.Find(findResult.PurchaseOrderID);
                    db.PurchaseOrderDetails.Remove(findResult);
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
        public void UpdateQuantityFromGoodsReceived(long? PurchaseDetailsID, long? Quantity)
        {
            long balQty = 0;
            long grnQty = 0;
            long resultQty = 0;
            if (Quantity != null)
            {
                grnQty = Convert.ToInt64(Quantity);
            }
            try
            {
                var findResult = db.PurchaseOrderDetails.Where(a => a.PurchaseOrderDetailID == PurchaseDetailsID).FirstOrDefault();
                if (findResult != null)
                {
                    if (findResult.BalanceQuantity != 0)
                    {
                        if (grnQty <= findResult.BalanceQuantity)
                        {
                            balQty = Convert.ToInt64(findResult.BalanceQuantity);
                            resultQty = balQty - grnQty;
                            findResult.BalanceQuantity = resultQty;
                        }

                    }
                }
                db.Entry(findResult).State = EntityState.Modified;
                db.SaveChanges();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void IsCompletedJob(long? Id,long userId)
        {
            var ischeckCompleted = false;
            var findObjList = db.PurchaseOrderDetails.Where(a => a.PurchaseOrderID == Id).ToList();
            var sumBalQTY = findObjList.Sum(a => a.BalanceQuantity);
            if (sumBalQTY==0)
            {
                ischeckCompleted = true;
            }
            if (ischeckCompleted)
            {
                var findObj = db.PurchaseOrders.Find(Id);

                findObj.IsCompleted = true;
                findObj.CompletedBy = userId;
                findObj.CompletedDate = DateTime.Now;

                db.Entry(findObj).State = EntityState.Modified;
                db.SaveChanges();

            }
            
        }
        public string AddorUpdateTransactions(PurchaseOrder classObj)
        {
            string violationMessage = "";
            TransactionsDetail objFirst = new TransactionsDetail();
            TransactionsDetail objSecond = new TransactionsDetail();
            try
            {
                var findAccounts = db.AccountMapping.Where(a => a.Account.IsActive == true && a.Account.IsDeleted == false && a.OrganizationId == classObj.OrganizationID && a.BranchId == classObj.BranchId && a.AccountDefaultType == CommonEnums.AccountDefaultType.Purchases).FirstOrDefault();
                
                var findfinancialBookYear = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == classObj.OrganizationID).FirstOrDefault();

                if (findfinancialBookYear==null)
                {
                   violationMessage = "Please add financial book year befor you add vouchers.";
                }
                 var findObjFirst = db.TransactionsDetails.Where(a => a.PurchaseId == classObj.PurchaseOrderId && a.PaymentType == PaymentType.PurchaseOrder).FirstOrDefault();
                var findObjLast = db.TransactionsDetails.Where(a => a.PurchaseId == classObj.PurchaseOrderId && a.PaymentType == PaymentType.PurchaseOrder).OrderByDescending(a => a.TransactionDetailID).FirstOrDefault();

                if (findObjFirst != null && findObjLast != null)
                {
                    // First Object Save for Credit
                    findObjFirst.OrganizationID = classObj.OrganizationID;
                    findObjFirst.PaymentType = PaymentType.PurchaseOrder;//
                    findObjFirst.AccountId = classObj.AccountID;//
                    findObjFirst.PurchaseId = classObj.PurchaseOrderId;
                    if (findfinancialBookYear != null)
                    {
                        findObjFirst.FinancialBookYearId = findfinancialBookYear.Id;
                    }

                    findObjFirst.SeqNo = 0;
                    findObjFirst.Narration = string.Empty;
                    findObjFirst.Debit = 0;
                    findObjFirst.Credit = classObj.NetTotal;
                    findObjFirst.TransactionMode = null;
                    findObjFirst.TrackNumber = 0;
                    findObjFirst.Posted = true;
                    findObjFirst.IsDeleted = false;
                    findObjFirst.UpdatedDate = DateTime.Now;
                    db.Entry(findObjFirst).State = EntityState.Modified;
                    db.SaveChanges();

                    // Second Object Save for Debit
                    findObjLast.OrganizationID = classObj.OrganizationID;
                    findObjLast.PaymentType = PaymentType.PurchaseOrder;//
                    //Static Account Id Of Goods Purchases
                    if (findAccounts != null)
                    {
                        findObjLast.AccountId = findAccounts.AccountId;//
                    }

                    findObjLast.PurchaseId = classObj.PurchaseOrderId;
                    if (findfinancialBookYear != null)
                    {
                        findObjLast.FinancialBookYearId = findfinancialBookYear.Id;
                    }

                    findObjLast.SeqNo = 0;
                    findObjLast.Narration = string.Empty;
                    findObjLast.Debit = classObj.NetTotal;
                    findObjLast.Credit = 0;
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
                    // First Object Save for Credit
                    objFirst.OrganizationID = classObj.OrganizationID;
                    objFirst.PaymentType = PaymentType.PurchaseOrder;//
                    objFirst.AccountId = classObj.AccountID;//
                    objFirst.PurchaseId = classObj.PurchaseOrderId;
                    if (findfinancialBookYear != null)
                    {
                        objFirst.FinancialBookYearId = findfinancialBookYear.Id;
                    }
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

                    // Second Object Save for Debit
                    objFirst.OrganizationID = classObj.OrganizationID;
                    objFirst.PaymentType = PaymentType.PurchaseOrder;//
                    //Static Account Id Of Goods Purchases Id=44
                    if (findAccounts != null)
                    {
                        objFirst.AccountId = findAccounts.AccountId;//
                    }
                    objFirst.PurchaseId = classObj.PurchaseOrderId;
                    if (findfinancialBookYear != null)
                    {
                        objFirst.FinancialBookYearId = findfinancialBookYear.Id;
                    }
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
            return violationMessage;
        }
        public void RunCalculateNetTotal(PurchaseOrder classObj)
        {
            var findListDetails = db.PurchaseOrderDetails.Where(a => a.PurchaseOrderID == classObj.PurchaseOrderId).ToList();

            decimal SubWithOutSaleTaxTotal = 0;
            decimal SubWithSaleTaxTotal = 0;
            foreach (var item in findListDetails)
            {
                SubWithOutSaleTaxTotal += Convert.ToDecimal(item.TotalWithOutSaleTax);
                SubWithSaleTaxTotal += Convert.ToDecimal(item.TotalTaxInclusive);

            }
            classObj.SubTotalWithOutSaleTax = SubWithOutSaleTaxTotal;
            classObj.SubTotalWithSaleTax = SubWithSaleTaxTotal;

            classObj.NetTotal = SubWithSaleTaxTotal + classObj.FreightCharges;
            string AmountInWord = systemDAL.NumberToWords(Convert.ToInt64(classObj.NetTotal));
            AmountInWord = AmountInWord + " Only";
            classObj.AmountInWord = AmountInWord;

            db.Entry(classObj).State=EntityState.Modified;
            db.SaveChanges();

        }
    }
}