using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
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
    public class SaleReturnNoteDAL
    {
        AppEntities db = new AppEntities();
        StockDAL stockDAL = new StockDAL();
        // Sale Return From Walking Customer
        public string AddSaleReturnNet(string objMasterItem, string objChilds, long userID)
        {
            long SaleRetrunId = 0;
            long orgId = 0;

            string violationMessage = "";
            SaleReturnNote obj = new SaleReturnNote();
            SaleReturnNoteDetail objChild = new SaleReturnNoteDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<SaleReturnNote> Parentitem = JsonConvert.DeserializeObject<List<SaleReturnNote>>(objMasterItem);
                    List<SaleReturnNoteDetail> Childitems = JsonConvert.DeserializeObject<List<SaleReturnNoteDetail>>(objChilds);
                    foreach (var SOR in Parentitem)
                    {
                        obj.OrganizationID = SOR.OrganizationID;
                        obj.SaleInvoicID = SOR.SaleInvoicID;
                        obj.AccoutID = SOR.AccoutID;
                        obj.MainReason = SOR.MainReason;
                        obj.SaleReturnType = SaleInvoiceTypeEnum.NetSaleInvoice;
                        obj.SaleReturnDate = SOR.SaleReturnDate;
                        obj.SaleReturnCode = SOR.SaleReturnCode;
                        obj.CID = SOR.OrganizationID;
                        obj.IsActive = true;
                        obj.IsDeleted = false;
                        obj.CreatedBy = userID;
                        obj.CreatedDate = DateTime.Now;
                        db.SaleReturnNotes.Add(obj);
                        db.SaveChanges();
                        SaleRetrunId = obj.SaleReturnId;
                        orgId = Convert.ToInt64(obj.OrganizationID);


                    }


                    foreach (var item in Childitems)
                    {
                        var Message = stockDAL.AddorUpdateStockSaleResturn(Convert.ToInt64(item.StockID), Convert.ToInt64(item.ReturnQunatity));

                        objChild.SaleReturnID = SaleRetrunId;
                        objChild.StockID = item.StockID;
                        objChild.ProductID = item.ProductID;
                        objChild.ReturnQunatity = item.ReturnQunatity;
                        objChild.Quantity = item.ReturnQunatity;
                        objChild.SaleInvoiceDetailsId = item.SaleInvoiceDetailsId;
                        objChild.Reason = item.Reason;
                        objChild.Active = true;
                        db.SaleReturnNoteDetails.Add(objChild);
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
        public string UpdateSaleReturnNet(string objMasterItem, string objChilds, long userID)
        {
            long SaleRetrunId = 0;
            long orgId = 0;

            string violationMessage = "";
            SaleReturnNote obj = new SaleReturnNote();
            SaleReturnNoteDetail objChild = new SaleReturnNoteDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<SaleReturnNote> Parentitem = JsonConvert.DeserializeObject<List<SaleReturnNote>>(objMasterItem);
                    List<SaleReturnNoteDetail> Childitems = JsonConvert.DeserializeObject<List<SaleReturnNoteDetail>>(objChilds);
                    foreach (var SOR in Parentitem)
                    {
                        obj = db.SaleReturnNotes.Find(SOR.SaleReturnId);
                        obj.OrganizationID = SOR.OrganizationID;
                        obj.SaleInvoicID = SOR.SaleInvoicID;
                        obj.AccoutID = SOR.AccoutID;
                        obj.MainReason = SOR.MainReason;
                        obj.SaleReturnType = SaleInvoiceTypeEnum.NetSaleInvoice;
                        obj.SaleReturnDate = SOR.SaleReturnDate;
                        obj.SaleReturnCode = SOR.SaleReturnCode;
                        obj.CID = SOR.OrganizationID;
                        obj.IsActive = true;
                        obj.IsDeleted = false;
                        obj.UpdatedBy = userID;
                        obj.UpdatedDate = DateTime.Now;
                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                        SaleRetrunId = obj.SaleReturnId;
                        orgId = Convert.ToInt64(obj.OrganizationID);


                    }


                    foreach (var item in Childitems)
                    {
                        objChild = db.SaleReturnNoteDetails.Where(a => a.SaleReturnDetailsId == item.SaleReturnDetailsId).FirstOrDefault();
                        long finalQty = 0;
                        long alreadyReturn = Convert.ToInt64(objChild.Quantity);
                        long nowReturn = Convert.ToInt64(item.ReturnQunatity);
                        finalQty = nowReturn - alreadyReturn;
                        var Message = stockDAL.AddorUpdateStockSaleResturn(Convert.ToInt64(item.StockID), Convert.ToInt64(finalQty));

                        objChild.SaleReturnID = SaleRetrunId;
                        objChild.StockID = item.StockID;
                        objChild.ProductID = item.ProductID;
                        objChild.ReturnQunatity = item.ReturnQunatity;
                        objChild.Quantity = item.ReturnQunatity;
                        objChild.SaleInvoiceDetailsId = item.SaleInvoiceDetailsId;
                        objChild.Reason = item.Reason;
                        objChild.Active = true;
                        db.Entry(objChild).State = EntityState.Modified;
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
        public string DeleteSaleReturnNet(SaleReturnNote saleReturnNote, long userID)
        {
            long SaleRetrunId = 0;
            long orgId = 0;

            string violationMessage = "";
            SaleReturnNote obj = new SaleReturnNote();
            SaleReturnNoteDetail objChild = new SaleReturnNoteDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    obj = db.SaleReturnNotes.Find(saleReturnNote.SaleReturnId);
                    obj.IsDeleted = true;
                    obj.DeletedBy = userID;
                    obj.DeletedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    SaleRetrunId = obj.SaleReturnId;
                    orgId = Convert.ToInt64(obj.OrganizationID);
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
        // Sale Return From Sale Order
        public string AddSaleReturn(string objMasterItem, string objChilds, long userID)
        {
            long SaleRetrunId = 0;
            long orgId = 0;

            string violationMessage = "";
            SaleReturnNote obj = new SaleReturnNote();
            SaleReturnNoteDetail objChild = new SaleReturnNoteDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<SaleReturnNote> Parentitem = JsonConvert.DeserializeObject<List<SaleReturnNote>>(objMasterItem);
                    List<SaleReturnNoteDetail> Childitems = JsonConvert.DeserializeObject<List<SaleReturnNoteDetail>>(objChilds);
                    foreach (var SOR in Parentitem)
                    {
                        obj.OrganizationID = SOR.OrganizationID;
                        obj.SaleInvoicID = SOR.SaleInvoicID;
                        obj.AccoutID = SOR.AccoutID;
                        obj.MainReason = SOR.MainReason;
                        obj.SaleReturnType = SaleInvoiceTypeEnum.SaleInvoice;
                        obj.SaleReturnDate = SOR.SaleReturnDate;
                        obj.CID = SOR.OrganizationID;
                        obj.IsActive = true;
                        obj.IsDeleted = false;
                        obj.CreatedBy = userID;
                        obj.CreatedDate = DateTime.Now;
                        db.SaleReturnNotes.Add(obj);
                        db.SaveChanges();
                        SaleRetrunId = obj.SaleReturnId;
                        orgId = Convert.ToInt64(obj.OrganizationID);


                    }


                    foreach (var item in Childitems)
                    {
                        var Message = stockDAL.AddorUpdateStockSaleResturn(Convert.ToInt64(item.StockID), Convert.ToInt64(item.ReturnQunatity));

                        objChild.SaleReturnID = SaleRetrunId;
                        objChild.StockID = item.StockID;
                        objChild.ProductID = item.ProductID;
                        objChild.ReturnQunatity = item.ReturnQunatity;
                        objChild.Quantity = item.ReturnQunatity;
                        objChild.SaleInvoiceDetailsId = item.SaleInvoiceDetailsId;
                        objChild.Reason = item.Reason;
                        objChild.Active = true;
                        db.SaleReturnNoteDetails.Add(objChild);
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
        public string UpdateSaleReturn(string objMasterItem, string objChilds, long userID)
        {
            long SaleRetrunId = 0;
            long orgId = 0;
            string violationMessage = "";
            SaleReturnNote obj = new SaleReturnNote();
            SaleReturnNoteDetail objChild = new SaleReturnNoteDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<SaleReturnNote> Parentitem = JsonConvert.DeserializeObject<List<SaleReturnNote>>(objMasterItem);
                    List<SaleReturnNoteDetail> Childitems = JsonConvert.DeserializeObject<List<SaleReturnNoteDetail>>(objChilds);
                    foreach (var SOR in Parentitem)
                    {
                        obj = db.SaleReturnNotes.Find(SOR.SaleReturnId);
                        obj.OrganizationID = SOR.OrganizationID;
                        obj.SaleInvoicID = SOR.SaleInvoicID;
                        obj.AccoutID = SOR.AccoutID;
                        obj.MainReason = SOR.MainReason;
                        obj.SaleReturnType = SaleInvoiceTypeEnum.SaleInvoice;
                        obj.SaleReturnDate = SOR.SaleReturnDate;
                        obj.SaleReturnCode = SOR.SaleReturnCode;
                        obj.CID = SOR.OrganizationID;
                        obj.IsActive = true;
                        obj.IsDeleted = false;
                        obj.UpdatedBy = userID;
                        obj.UpdatedDate = DateTime.Now;
                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                        SaleRetrunId = obj.SaleReturnId;
                        orgId = Convert.ToInt64(obj.OrganizationID);
                    }


                    foreach (var item in Childitems)
                    {
                        objChild = db.SaleReturnNoteDetails.Where(a => a.SaleReturnDetailsId == item.SaleReturnDetailsId).FirstOrDefault();
                        long finalQty = 0;
                        long alreadyReturn = Convert.ToInt64(objChild.Quantity);
                        long nowReturn = Convert.ToInt64(item.ReturnQunatity);
                        finalQty = nowReturn - alreadyReturn;
                        var Message = stockDAL.AddorUpdateStockSaleResturn(Convert.ToInt64(item.StockID), Convert.ToInt64(finalQty));

                        objChild.SaleReturnID = SaleRetrunId;
                        objChild.StockID = item.StockID;
                        objChild.ProductID = item.ProductID;
                        objChild.ReturnQunatity = item.ReturnQunatity;
                        objChild.Quantity = item.ReturnQunatity;
                        objChild.SaleInvoiceDetailsId = item.SaleInvoiceDetailsId;
                        objChild.Reason = item.Reason;
                        objChild.Active = true;
                        db.Entry(objChild).State = EntityState.Modified;
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
        public string DeleteSaleReturn(SaleReturnNote saleReturnNote, long userID)
        {
            long SaleRetrunId = 0;
            long orgId = 0;
            string violationMessage = "";
            SaleReturnNote obj = new SaleReturnNote();
            SaleReturnNoteDetail objChild = new SaleReturnNoteDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    obj = db.SaleReturnNotes.Find(saleReturnNote.SaleReturnId);
                    obj.IsDeleted = true;
                    obj.DeletedBy = userID;
                    obj.DeletedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    SaleRetrunId = obj.SaleReturnId;
                    orgId = Convert.ToInt64(obj.OrganizationID);


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