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
    public class SaleInvoiceDAL
    {
        AppEntities db = new AppEntities();
        StockDAL stockDAL = new StockDAL();
        FilesUploadService filesUploadService = new FilesUploadService();
        //Sale Invoice from Sale Orders
        public string AddSaleInvoice(string objMasterItem, string objChilds,long userID)
        {
            long SIid = 0;

            string violationMessage = "";
            SaleInvoice obj = new SaleInvoice();
            SaleInvoiceDetail objChild = new SaleInvoiceDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {



                    List<SaleInvoice> Parentitem = JsonConvert.DeserializeObject<List<SaleInvoice>>(objMasterItem);
                    List<SaleInvoiceDetail> Childitems = JsonConvert.DeserializeObject<List<SaleInvoiceDetail>>(objChilds);
                    foreach (var SO in Parentitem)
                    {


                        obj.OrganizationID = SO.OrganizationID;
                        obj.SaleOrderID = SO.SaleOrderID;
                        obj.AccountID = SO.AccountID;
                        obj.SaleInvoiceNo = SO.SaleInvoiceNo;
                        obj.SaleInvoiceType = SaleInvoiceTypeEnum.SaleInvoice;
                        obj.SaleInvoiceDate = SO.SaleInvoiceDate;
                        obj.TransactionType = SO.TransactionType;
                        obj.SubTotalWithOutSaleTax = SO.SubTotalWithOutSaleTax;
                        obj.SubTotalWithSaleTax = SO.SubTotalWithSaleTax;
                        obj.FreightCharges = SO.FreightCharges;
                        obj.NetTotal = SO.NetTotal;
                        obj.AmountInWord = SO.AmountInWord;
                        obj.TermAndCondition = SO.TermAndCondition;
                        obj.CID = SO.OrganizationID;
                        obj.IsActive = true;
                        obj.IsDeleted = false;
                        obj.CreatedBy = userID;
                        obj.CreatedDate = DateTime.Now;
                        db.SaleInvoices.Add(obj);
                        db.SaveChanges();
                        SIid = obj.SaleInvoiceID;
                        violationMessage=AddorUpdateTransactions(obj);
                    }


                    foreach (var item in Childitems)
                    {
                        objChild.SaleInvoiceID = SIid;
                        objChild.SaleOrderDetailsID = item.SaleOrderDetailsID;
                        if (item.StockID != null && item.StockID != 0)
                        {
                            objChild.StockID = item.StockID;
                            var Message = stockDAL.RemoveorUpdateStockSaleOrder(Convert.ToInt64(item.StockID), Convert.ToInt64(item.Quantity));
                        }
                        else
                        {
                            violationMessage = "Stock Not Available";
                        }

                        objChild.ProductID = item.ProductID;
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
                        db.SaleInvoiceDetails.Add(objChild);
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
        public string UpdateSaleInvoice(string objMasterItem, string objChilds,long userID)
        {
            long SIid = 0;
            string violationMessage = "";
            SaleInvoice obj = new SaleInvoice();
            SaleInvoiceDetail objChild = new SaleInvoiceDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {


                    List<SaleInvoice> Parentitem = JsonConvert.DeserializeObject<List<SaleInvoice>>(objMasterItem);
                    List<SaleInvoiceDetail> Childitems = JsonConvert.DeserializeObject<List<SaleInvoiceDetail>>(objChilds);
                    foreach (var SO in Parentitem)
                    {
                        obj = db.SaleInvoices.Find(SO.SaleInvoiceID);
                        obj.OrganizationID = SO.OrganizationID;
                        obj.SaleOrderID = SO.SaleOrderID;
                        obj.AccountID = SO.AccountID;
                        obj.SaleInvoiceNo = SO.SaleInvoiceNo;
                        obj.SaleInvoiceType = SaleInvoiceTypeEnum.SaleInvoice;
                        obj.SaleInvoiceDate = SO.SaleInvoiceDate;
                        obj.TransactionType = SO.TransactionType;

                        obj.SubTotalWithOutSaleTax = SO.SubTotalWithOutSaleTax;
                        obj.SubTotalWithSaleTax = SO.SubTotalWithSaleTax;
                        obj.FreightCharges = SO.FreightCharges;
                        obj.NetTotal = SO.NetTotal;
                        obj.AmountInWord = SO.AmountInWord;
                        obj.TermAndCondition = SO.TermAndCondition;

                        obj.CID = SO.OrganizationID;
                        obj.IsActive = true;
                        obj.IsDeleted = false;
                        obj.UpdatedBy = userID;
                        obj.UpdatedDate = DateTime.Now;

                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                        SIid = obj.SaleInvoiceID;
                        violationMessage = AddorUpdateTransactions(obj);
                    }


                    foreach (var item in Childitems)
                    {
                        var findResult = db.SaleInvoiceDetails.Find(item.SaleInvoiceDetailID);
                        if (findResult != null)
                        {
                            if (item.StockID != null && item.StockID != 0)
                            {
                                objChild.StockID = item.StockID;
                                var Message = stockDAL.RemoveorUpdateStockSaleOrder(Convert.ToInt64(item.StockID), Convert.ToInt64(item.Quantity));
                            }
                            else
                            {
                                violationMessage = "Stock Not Available";
                            }
                            findResult.SaleInvoiceID = SIid;
                            findResult.StockID = item.StockID;
                            findResult.SaleOrderDetailsID = item.SaleOrderDetailsID;
                            findResult.ProductID = item.ProductID;
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
                        else
                        {
                            if (item.StockID != null && item.StockID != 0)
                            {
                                objChild.StockID = item.StockID;
                                var Message = stockDAL.RemoveorUpdateStockSaleOrder(Convert.ToInt64(item.StockID), Convert.ToInt64(item.Quantity));
                            }
                            else
                            {
                                violationMessage = "Stock Not Available";
                            }
                            objChild.SaleInvoiceID = SIid;
                            objChild.StockID = item.StockID;
                            objChild.SaleOrderDetailsID = item.SaleOrderDetailsID;
                            objChild.ProductID = item.ProductID;
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
                            db.SaleInvoiceDetails.Add(objChild);
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
        public string DeleteSaleInvoice(SaleInvoice saleInvoice, long userID)
        {
            long SIid = 0;
            string violationMessage = "";
            SaleInvoice obj = new SaleInvoice();
            SaleInvoiceDetail objChild = new SaleInvoiceDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                        obj = db.SaleInvoices.Find(saleInvoice.SaleInvoiceID);
                        obj.IsActive = true;
                        obj.IsDeleted = true;
                        obj.DeletedBy = userID;
                        obj.DeletedDate = DateTime.Now;

                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                        SIid = obj.SaleInvoiceID;
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
        //Sale Invoice Net For Walking Customer
        public string AddSaleInvoiceNet(string objMasterItem, string objChilds, string objCustomers, HttpPostedFileBase cusFile, long userID)
        {
            long SIid = 0;
            long Cusid = 0;
            byte[] Imagebytes;
            string violationMessage = "";
            string fileName = "Cus_";
            string folderName = "Customers";
            SaleInvoice obj = new SaleInvoice();
            SaleInvoiceDetail objChild = new SaleInvoiceDetail();
            Customer objCustomer = new Customer();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {



                    List<SaleInvoice> Parentitem = JsonConvert.DeserializeObject<List<SaleInvoice>>(objMasterItem);
                    List<SaleInvoiceDetail> Childitems = JsonConvert.DeserializeObject<List<SaleInvoiceDetail>>(objChilds);
                    List<Customer> Customeritems = JsonConvert.DeserializeObject<List<Customer>>(objCustomers);
                    foreach (var cus in Customeritems)
                    {
                        if (cusFile != null)
                        {
                            using (BinaryReader br = new BinaryReader(cusFile.InputStream))
                            {
                                Imagebytes = br.ReadBytes(cusFile.ContentLength);
                                objCustomer.Picture = Imagebytes;
                            }
                            Stream stream = new MemoryStream(Imagebytes);
                            string extension = Path.GetExtension(cusFile.FileName);
                            var filePath = filesUploadService.UploadImages(fileName + cus.CustomerCode + "_", stream, folderName, extension);
                            objCustomer.PictureFilePath = filePath.ToString();
                        }

                        objCustomer.OrganizationID = cus.OrganizationID;
                        objCustomer.CustomerCode = cus.CustomerCode;
                        objCustomer.Title = cus.Title;
                        objCustomer.FirstName = cus.FirstName;
                        objCustomer.LastName = cus.LastName;
                        objCustomer.GuardianName = cus.GuardianName;
                        objCustomer.MobileNo = cus.MobileNo;
                        objCustomer.PhoneNo = cus.PhoneNo;
                        objCustomer.Email = cus.Email;
                        objCustomer.CNIC = cus.CNIC;
                        objCustomer.Address = cus.Address;
                        objCustomer.IsActive = true;
                        objCustomer.IsDeleted = false;
                        objCustomer.CreatedBy = userID;
                        objCustomer.CreatedDate = DateTime.Now;
                        objCustomer.CID = cus.OrganizationID;
                        db.Customers.Add(objCustomer);
                        db.SaveChanges();
                        Cusid = objCustomer.CustomerId;
                    }
                    foreach (var SO in Parentitem)
                    {
                        obj.OrganizationID = SO.OrganizationID;
                        obj.CustomerID = Cusid;
                        obj.AccountID = SO.AccountID;
                        obj.SaleInvoiceNo = SO.SaleInvoiceNo;
                        obj.SaleInvoiceDate = SO.SaleInvoiceDate;
                        obj.SaleInvoiceType = SaleInvoiceTypeEnum.NetSaleInvoice;
                        obj.TransactionType = SO.TransactionType;
                        obj.SubTotalWithOutSaleTax = SO.SubTotalWithOutSaleTax;
                        obj.SubTotalWithSaleTax = SO.SubTotalWithSaleTax;
                        obj.FreightCharges = SO.FreightCharges;
                        obj.NetTotal = SO.NetTotal;
                        obj.AmountInWord = SO.AmountInWord;
                        obj.TermAndCondition = SO.TermAndCondition;
                        obj.CID = SO.OrganizationID;
                        obj.IsActive = true;
                        obj.IsDeleted = false;
                        obj.CreatedBy = userID;
                        obj.CreatedDate = DateTime.Now;
                        db.SaleInvoices.Add(obj);
                        db.SaveChanges();
                        SIid = obj.SaleInvoiceID;
                        violationMessage = AddorUpdateTransactions(obj);
                    }


                    //foreach (var item in Childitems)
                    //{
                    //    //string StockCode = stockDAL.AutoGenerateStockCode(Convert.ToInt64(item.ProductID),);

                    //    if (!String.IsNullOrEmpty(StockCode))
                    //    {
                    //        long StockID = stockDAL.GetStockIDByStockCode(StockCode);
                    //        objChild.StockID = StockID;
                    //        var Message = stockDAL.RemoveorUpdateStockSaleOrder(StockID, Convert.ToInt64(item.Quantity));
                    //    }
                    //    else
                    //    {
                    //        violationMessage = "Stock Not Available";
                    //    }
                    //    objChild.SaleInvoiceID = SIid;
                    //    objChild.ProductID = item.ProductID;
                    //    objChild.Quantity = item.Quantity;
                    //    objChild.BalanceQuantity = item.Quantity;
                    //    objChild.UnitRate = item.UnitRate;
                    //    objChild.TotalWithOutSaleTax = item.TotalWithOutSaleTax;
                    //    objChild.DiscountPercentage = item.DiscountPercentage;
                    //    objChild.DiscountAmount = item.DiscountAmount;
                    //    objChild.DiscountedUnitRate = item.DiscountedUnitRate;
                    //    objChild.TotalAfterDiscount = item.TotalAfterDiscount;
                    //    objChild.SaleTaxPercentage = item.SaleTaxPercentage;
                    //    objChild.SaleTaxAmount = item.SaleTaxAmount;
                    //    objChild.TotalTaxInclusive = item.TotalTaxInclusive;
                    //    objChild.Active = true;
                    //    db.SaleInvoiceDetails.Add(objChild);
                    //    db.SaveChanges();
                    //}


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
        public string UpdateSaleInvoiceNet(string objMasterItem, string objChilds, string objCustomers, HttpPostedFileBase cusFile, long userID)
        {
            long SIid = 0;
            long Cusid = 0;
            byte[] Imagebytes;
            string violationMessage = "";
            string fileName = "Cus_";
            string folderName = "Customers";
            SaleInvoice obj = new SaleInvoice();
            SaleInvoiceDetail objChild = new SaleInvoiceDetail();
            Customer objCustomer = new Customer();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<SaleInvoice> Parentitem = JsonConvert.DeserializeObject<List<SaleInvoice>>(objMasterItem);
                    List<SaleInvoiceDetail> Childitems = JsonConvert.DeserializeObject<List<SaleInvoiceDetail>>(objChilds);
                    List<Customer> Customeritems = JsonConvert.DeserializeObject<List<Customer>>(objCustomers);
                    foreach (var cus in Customeritems)
                    {
                        objCustomer = db.Customers.Where(a => a.CustomerId == cus.CustomerId).FirstOrDefault();

                        if (cusFile != null)
                        {
                            using (BinaryReader br = new BinaryReader(cusFile.InputStream))
                            {
                                Imagebytes = br.ReadBytes(cusFile.ContentLength);
                                objCustomer.Picture = Imagebytes;
                            }
                            Stream stream = new MemoryStream(Imagebytes);
                            string extension = Path.GetExtension(cusFile.FileName);
                            var filePath = filesUploadService.UploadImages(fileName + cus.CustomerCode + "_", stream, folderName, extension);
                            objCustomer.PictureFilePath = filePath.ToString();
                        }

                        objCustomer.OrganizationID = cus.OrganizationID;
                        objCustomer.CustomerCode = cus.CustomerCode;
                        objCustomer.Title = cus.Title;
                        objCustomer.FirstName = cus.FirstName;
                        objCustomer.LastName = cus.LastName;
                        objCustomer.GuardianName = cus.GuardianName;
                        objCustomer.MobileNo = cus.MobileNo;
                        objCustomer.PhoneNo = cus.PhoneNo;
                        objCustomer.Email = cus.Email;
                        objCustomer.CNIC = cus.CNIC;
                        objCustomer.Address = cus.Address;
                        objCustomer.IsActive = true;
                        objCustomer.IsDeleted = false;
                        objCustomer.CreatedBy = userID;
                        objCustomer.CreatedDate = DateTime.Now;
                        objCustomer.CID = cus.OrganizationID;
                        db.Entry(objCustomer).State = EntityState.Modified;
                        db.SaveChanges();
                        Cusid = objCustomer.CustomerId;
                      
                    }
                    foreach (var SO in Parentitem)
                    {
                        obj = db.SaleInvoices.Where(a => a.SaleInvoiceID == SO.SaleInvoiceID).FirstOrDefault();
                        obj.OrganizationID = SO.OrganizationID;
                        obj.CustomerID = Cusid;
                        obj.AccountID = SO.AccountID;
                        obj.SaleInvoiceNo = SO.SaleInvoiceNo;
                        obj.SaleInvoiceDate = SO.SaleInvoiceDate;
                        obj.SaleInvoiceType = SaleInvoiceTypeEnum.NetSaleInvoice;
                        obj.TransactionType = SO.TransactionType;
                        obj.SubTotalWithOutSaleTax = SO.SubTotalWithOutSaleTax;
                        obj.SubTotalWithSaleTax = SO.SubTotalWithSaleTax;
                        obj.FreightCharges = SO.FreightCharges;
                        obj.NetTotal = SO.NetTotal;
                        obj.AmountInWord = SO.AmountInWord;
                        obj.TermAndCondition = SO.TermAndCondition;
                        obj.CID = SO.OrganizationID;
                        obj.IsActive = true;
                        obj.IsDeleted = false;
                        obj.UpdatedBy = userID;
                        obj.UpdatedDate = DateTime.Now;
                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                        SIid = obj.SaleInvoiceID;
                        violationMessage = AddorUpdateTransactions(obj);
                    }


                    //foreach (var item in Childitems)
                    //{
                    //    var findResult = db.SaleInvoiceDetails.Where(a => a.SaleInvoiceDetailID == item.SaleInvoiceDetailID).FirstOrDefault();
                    //    if (findResult != null)
                    //    {
                    //        long finalQty = 0;
                    //        long alreadyreceived = Convert.ToInt64(findResult.Quantity);
                    //        long nowreceived = Convert.ToInt64(item.Quantity);
                    //        finalQty = nowreceived - alreadyreceived;
                    //        string StockCode = stockDAL.AutoGenerateStockCode(Convert.ToInt64(item.ProductID));
                    //        if (!String.IsNullOrEmpty(StockCode))
                    //        {
                    //            long StockID = stockDAL.GetStockIDByStockCode(StockCode);
                    //            findResult.StockID = StockID;
                    //            var Message = stockDAL.RemoveorUpdateStockSaleOrder(StockID, Convert.ToInt64(finalQty));
                    //        }
                    //        else
                    //        {
                    //            violationMessage = "Stock Not Available";
                    //        }
                    //        findResult.SaleInvoiceID = SIid;
                    //        findResult.ProductID = item.ProductID;
                    //        findResult.Quantity = item.Quantity;
                    //        findResult.BalanceQuantity = item.Quantity;
                    //        findResult.UnitRate = item.UnitRate;
                    //        findResult.TotalWithOutSaleTax = item.TotalWithOutSaleTax;
                    //        findResult.DiscountPercentage = item.DiscountPercentage;
                    //        findResult.DiscountAmount = item.DiscountAmount;
                    //        findResult.DiscountedUnitRate = item.DiscountedUnitRate;
                    //        findResult.TotalAfterDiscount = item.TotalAfterDiscount;
                    //        findResult.SaleTaxPercentage = item.SaleTaxPercentage;
                    //        findResult.SaleTaxAmount = item.SaleTaxAmount;
                    //        findResult.TotalTaxInclusive = item.TotalTaxInclusive;
                    //        findResult.Active = true;
                    //        db.Entry(findResult).State = EntityState.Modified;
                    //        db.SaveChanges();
                    //    }
                    //    else
                    //    {
                    //        string StockCode = stockDAL.AutoGenerateStockCode(Convert.ToInt64(item.ProductID));

                    //        if (!String.IsNullOrEmpty(StockCode))
                    //        {
                    //            long StockID = stockDAL.GetStockIDByStockCode(StockCode);
                    //            objChild.StockID = StockID;
                    //            var Message = stockDAL.RemoveorUpdateStockSaleOrder(StockID, Convert.ToInt64(item.Quantity));
                    //        }
                    //        else
                    //        {
                    //            violationMessage = "Stock Not Available";
                    //        }
                    //        objChild.SaleInvoiceID = SIid;
                    //        objChild.ProductID = item.ProductID;
                    //        objChild.Quantity = item.Quantity;
                    //        objChild.BalanceQuantity = item.Quantity;
                    //        objChild.UnitRate = item.UnitRate;
                    //        objChild.TotalWithOutSaleTax = item.TotalWithOutSaleTax;
                    //        objChild.DiscountPercentage = item.DiscountPercentage;
                    //        objChild.DiscountAmount = item.DiscountAmount;
                    //        objChild.DiscountedUnitRate = item.DiscountedUnitRate;
                    //        objChild.TotalAfterDiscount = item.TotalAfterDiscount;
                    //        objChild.SaleTaxPercentage = item.SaleTaxPercentage;
                    //        objChild.SaleTaxAmount = item.SaleTaxAmount;
                    //        objChild.TotalTaxInclusive = item.TotalTaxInclusive;
                    //        objChild.Active = true;
                    //        db.SaleInvoiceDetails.Add(objChild);
                    //        db.SaveChanges();
                    //    }

                    //}

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
        public string DeleteSaleInvoiceNet(SaleInvoice saleInvoice, long userID)
        {
            long SIid = 0;
            
            string violationMessage = "";
            SaleInvoice obj = new SaleInvoice();
            SaleInvoiceDetail objChild = new SaleInvoiceDetail();
            Customer objCustomer = new Customer();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                   
                        obj = db.SaleInvoices.Where(a => a.SaleInvoiceID == saleInvoice.SaleInvoiceID).FirstOrDefault();                       
                        obj.IsDeleted = true;
                        obj.DeletedBy = userID;
                        obj.DeletedDate = DateTime.Now;
                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                        SIid = obj.SaleInvoiceID;

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
        public string DeleteSaleInvoiceDetails(string iD)
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

                    var findResult = db.SaleInvoiceDetails.Find(id);
                    db.SaleInvoiceDetails.Remove(findResult);
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
        public string AddorUpdateTransactions(SaleInvoice classObj)
        {
            string violationMessage = "";
            TransactionsDetail objFirst = new TransactionsDetail();
            TransactionsDetail objSecond = new TransactionsDetail();
            try
            {
                var findAccounts = db.AccountMapping.Where(a => a.Account.IsActive == true && a.Account.IsDeleted == false && a.OrganizationId == classObj.OrganizationID && a.AccountDefaultType == CommonEnums.AccountDefaultType.Sales).FirstOrDefault();
                
                var findfinancialBookYear = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == classObj.OrganizationID).FirstOrDefault();
                var findObjFirst = db.TransactionsDetails.Where(a => a.SaleId == classObj.SaleInvoiceID && a.PaymentType == PaymentType.SaleOrder).FirstOrDefault();
                var findObjLast = db.TransactionsDetails.Where(a => a.SaleId == classObj.SaleInvoiceID && a.PaymentType == PaymentType.SaleOrder).OrderByDescending(a => a.TransactionDetailID).FirstOrDefault();

                if (findObjFirst != null && findObjLast != null)
                {
                    // First Object Save for Debit 
                    findObjFirst.OrganizationID = classObj.OrganizationID;
                    findObjFirst.PaymentType = PaymentType.SaleOrder;//
                    findObjFirst.AccountId = classObj.AccountID;//
                    findObjFirst.SaleId = classObj.SaleInvoiceID;
                    if (findfinancialBookYear != null)
                    {
                        findObjFirst.FinancialBookYearId = findfinancialBookYear.Id;
                    }

                    findObjFirst.SeqNo = 0;
                    findObjFirst.Narration = string.Empty;
                    findObjFirst.Debit = classObj.NetTotal;
                    findObjFirst.Credit =0;
                    findObjFirst.TransactionMode = null;
                    findObjFirst.TrackNumber = 0;
                    findObjFirst.Posted = true;
                    findObjFirst.IsDeleted = false;
                    findObjFirst.UpdatedDate = DateTime.Now;
                    db.Entry(findObjFirst).State = EntityState.Modified;
                    db.SaveChanges();

                    // Second Object Save for Credit
                    findObjLast.OrganizationID = classObj.OrganizationID;
                    findObjLast.PaymentType = PaymentType.SaleOrder;//
                    //Static Account Id Of General Sales Account Id=39
                    if (findAccounts !=null)
                    {
                        findObjLast.AccountId = findAccounts.AccountId;//
                    }
                    
                    findObjLast.SaleId = classObj.SaleInvoiceID;
                    if (findfinancialBookYear != null)
                    {
                        findObjLast.FinancialBookYearId = findfinancialBookYear.Id;
                    }

                    findObjLast.SeqNo = 0;
                    findObjLast.Narration = string.Empty;
                    findObjLast.Debit =0;
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
                    objFirst.PaymentType = PaymentType.SaleOrder;//
                    objFirst.AccountId = classObj.AccountID;//
                    objFirst.SaleId = classObj.SaleInvoiceID;
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

                    // Second Object Save for Credit
                    objFirst.OrganizationID = classObj.OrganizationID;
                    objFirst.PaymentType = PaymentType.SaleOrder;//
                    //Static Account Id Of General Sales Account Id=39
                    if (findAccounts != null)
                    {
                        objFirst.AccountId = findAccounts.AccountId;//
                    }
                    objFirst.SaleId = classObj.SaleInvoiceID;
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


    }
}