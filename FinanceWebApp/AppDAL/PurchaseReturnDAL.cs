using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;

namespace MudasirRehmanAlp.AppDAL
{
    public class PurchaseReturnDAL
    {
        AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        public string AddPurchaseReturn(string ObjMasterItem, string ObjChildItem, long userID)
        {
            long poRid = 0;

            string violationMessage = "";
            PurchaseOrderReturn obj = new PurchaseOrderReturn();
            PurchaseOrderReturnDetail objChild = new PurchaseOrderReturnDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    PurchaseOrderReturn PORObj = JsonConvert.DeserializeObject<PurchaseOrderReturn>(ObjMasterItem);
                    List<PurchaseOrderReturnDetail> Childitems = JsonConvert.DeserializeObject<List<PurchaseOrderReturnDetail>>(ObjChildItem);



                    obj.OrganizationID = PORObj.OrganizationID;
                    obj.BranchId = PORObj.BranchId;
                    obj.PurchaseOrderReturnNO = PORObj.PurchaseOrderReturnNO;
                    obj.PurchaseOrderReturnNO = dALCode.AutoGeneratePurchaseReturnCode(Convert.ToInt64(PORObj.OrganizationID), Convert.ToInt64(PORObj.BranchId));
                    obj.GRNReturnID = PORObj.GRNReturnID;
                    obj.PurchaseOrderID = PORObj.PurchaseOrderID;
                    obj.SupplierID = PORObj.SupplierID;
                    obj.AccountID = PORObj.AccountID;
                    obj.Date = PORObj.Date;
                    obj.Description = PORObj.Description;

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.CreatedBy = userID;
                    obj.CreatedDate = DateTime.Now;
                    db.PurchaseOrderReturns.Add(obj);
                    db.SaveChanges();
                    poRid = obj.PurchaseOrderReturnId;

                    foreach (var item in Childitems)
                    {
                        objChild.PurchaseOrderReturnID = poRid;
                        objChild.ProductID = item.ProductID;
                        objChild.Quantity = item.Quantity;
                        objChild.GRNReturnDetailID = item.GRNReturnDetailID;
                        objChild.PurchaseOrderDetailID = item.PurchaseOrderDetailID;
                        objChild.UnitPrice = item.UnitPrice;
                        objChild.TotalPrice = item.TotalPrice;

                        objChild.Active = true;
                        db.PurchaseOrderReturnDetails.Add(objChild);
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
        public string UpdatePurchaseReturn(string ObjMasterItem, string ObjChildItem, long userID)
        {
            long poRid = 0;

            string violationMessage = "";
            PurchaseOrderReturn obj = new PurchaseOrderReturn();
            PurchaseOrderReturnDetail objChild = new PurchaseOrderReturnDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    PurchaseOrderReturn PORObj = JsonConvert.DeserializeObject<PurchaseOrderReturn>(ObjMasterItem);
                    List<PurchaseOrderReturnDetail> Childitems = JsonConvert.DeserializeObject<List<PurchaseOrderReturnDetail>>(ObjChildItem);



                    obj = db.PurchaseOrderReturns.Find(PORObj.PurchaseOrderReturnId);

                    obj.OrganizationID = PORObj.OrganizationID;
                    obj.BranchId = PORObj.BranchId;
                    obj.PurchaseOrderReturnNO = PORObj.PurchaseOrderReturnNO;
                    obj.GRNReturnID = PORObj.GRNReturnID;
                    obj.PurchaseOrderID = PORObj.PurchaseOrderID;
                    obj.SupplierID = PORObj.SupplierID;
                    obj.AccountID = PORObj.AccountID;
                    obj.Date = PORObj.Date;
                    obj.Description = PORObj.Description;

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.UpdatedBy = userID;
                    obj.UpdatedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    poRid = obj.PurchaseOrderReturnId;

                    foreach (var item in Childitems)
                    {
                        objChild = db.PurchaseOrderReturnDetails.Find(item.PurchaseOrderReturnDetailId);

                        objChild.PurchaseOrderReturnID = poRid;
                        objChild.ProductID = item.ProductID;
                        objChild.Quantity = item.Quantity;
                        objChild.GRNReturnDetailID = item.GRNReturnDetailID;
                        objChild.PurchaseOrderDetailID = item.PurchaseOrderDetailID;
                        objChild.UnitPrice = item.UnitPrice;
                        objChild.TotalPrice = item.TotalPrice;

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
        public string DeletePurchaseReturn(PurchaseOrderReturn purchaseOrderReturn, long userID)
        {
            long poRid = 0;

            string violationMessage = "";
            PurchaseOrderReturn obj = new PurchaseOrderReturn();
            PurchaseOrderReturnDetail objChild = new PurchaseOrderReturnDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    obj = db.PurchaseOrderReturns.Find(purchaseOrderReturn.PurchaseOrderReturnId);
                    obj.IsDeleted = true;
                    obj.DeletedBy = userID;
                    obj.DeletedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    poRid = obj.PurchaseOrderReturnId;
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