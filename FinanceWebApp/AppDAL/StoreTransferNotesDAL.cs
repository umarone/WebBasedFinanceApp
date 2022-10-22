using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.AppDAL
{
    public class StoreTransferNotesDAL
    {
        AppEntities db = new AppEntities();
        public string Add(string objMasterItem, string objChilds, long userID)
        {
            long stnid = 0;
            string violationMessage = "";
            StoreTransferNote obj = new StoreTransferNote();
            StoreTransferNoteDetail objChild = new StoreTransferNoteDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    StoreTransferNote STNObj = JsonConvert.DeserializeObject<StoreTransferNote>(objMasterItem);
                    List<StoreTransferNoteDetail> Childitems = JsonConvert.DeserializeObject<List<StoreTransferNoteDetail>>(objChilds);


                    obj.OrganizationID = STNObj.OrganizationID;
                    obj.FromBranchId = STNObj.FromBranchId;
                    obj.ToBranchId = STNObj.ToBranchId;
                    obj.STNDate = STNObj.STNDate;
                    obj.Description = STNObj.Description;
                    obj.Code = STNObj.Code;


                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.CreatedBy = userID;
                    obj.CreatedDate = DateTime.Now;
                    db.StoreTransferNotes.Add(obj);
                    db.SaveChanges();
                    stnid = obj.Id;

                    foreach (var item in Childitems)
                    {
                        objChild.STNId = stnid;
                        objChild.ProductId = item.ProductId;
                        objChild.StockId = item.StockId;
                        objChild.Quantity = item.Quantity;
                        objChild.BalanceQuantity = item.Quantity;                      
                        db.StoreTransferNoteDetails.Add(objChild);
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
        public string Update(string objMasterItem, string objChilds, long userID)
        {
            long stnid = 0;
            string violationMessage = "";
            StoreTransferNote obj = new StoreTransferNote();
            StoreTransferNoteDetail objChild = new StoreTransferNoteDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    StoreTransferNote STNObj = JsonConvert.DeserializeObject<StoreTransferNote>(objMasterItem);
                    List<StoreTransferNoteDetail> Childitems = JsonConvert.DeserializeObject<List<StoreTransferNoteDetail>>(objChilds);


                    obj = db.StoreTransferNotes.Find(STNObj.Id);
                    obj.OrganizationID = STNObj.OrganizationID;
                    obj.FromBranchId = STNObj.FromBranchId;
                    obj.ToBranchId = STNObj.ToBranchId;
                    obj.STNDate = STNObj.STNDate;
                    obj.Description = STNObj.Description;
                    obj.Code = STNObj.Code;

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.UpdatedBy = userID;
                    obj.UpdatedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    stnid = obj.Id;

                    foreach (var item in Childitems)
                    {

                        var findResult = db.StoreTransferNoteDetails.Where(a => a.Id == item.Id).FirstOrDefault();
                        if (findResult != null)
                        {
                            findResult.STNId = stnid;
                            findResult.ProductId = item.ProductId;                            
                            findResult.StockId = item.StockId;
                            findResult.Quantity = item.Quantity;
                            findResult.BalanceQuantity = item.Quantity;
                            db.Entry(findResult).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            objChild.STNId = stnid;
                            objChild.ProductId = item.ProductId;
                            objChild.StockId = item.StockId;
                            objChild.Quantity = item.Quantity;
                            objChild.BalanceQuantity = item.Quantity;
                            db.StoreTransferNoteDetails.Add(objChild);
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
        public string Delete(StoreTransferNote storeTransferNote, long userID)
        {
            
            string violationMessage = "";
            StoreTransferNote obj = new StoreTransferNote();
            StoreTransferNoteDetail objChild = new StoreTransferNoteDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    obj = db.StoreTransferNotes.Find(storeTransferNote.Id);


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
        public void UpdateQuantityFromGoodsReceived(long? sTNDetailID, long? receiveQuantity)
        {
            long balQty = 0;
            long grnQty = 0;
            long resultQty = 0;
            if (receiveQuantity != null)
            {
                grnQty = Convert.ToInt64(receiveQuantity);
            }
            try
            {
                var findResult = db.StoreTransferNoteDetails.Where(a => a.Id == sTNDetailID).FirstOrDefault();
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


        public string DeleteSTNSingleDetails(string Id)
        {
            long id = 0;
            string violationMessage = "";

            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (Id != "")
                    {
                        id = Convert.ToInt64(Id);
                    }

                    var findResult = db.StoreTransferNoteDetails.Find(id);
                    db.StoreTransferNoteDetails.Remove(findResult);
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
    }
}