using MudasirRehmanAlp.AppModels;
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

namespace MudasirRehmanAlp.AppDAL
{
    public class GoodReceivedDAL
    {
        AppEntities db = new AppEntities();
        PurchaseOrderDAL purchaseOrderDAL = new PurchaseOrderDAL();
        AdjustmentNotesDAL adjustmentNotesDAL = new AdjustmentNotesDAL();
        StoreTransferNotesDAL storeTransferNotesDAL = new StoreTransferNotesDAL();
        StockDAL stockDAL = new StockDAL();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();

        public string AddGoodsReceived(string ObjMasterItem, string ObjChildItem, long userID)
        {
            long GRNId = 0;
            long orgId = 0;
            long brchId = 0;
            //long StnbrchId = 0;
            long? POId = 0;
            string Type = "";
            string violationMessage = "";
            GoodReceived obj = new GoodReceived();
            GoodReceivedDetail objChild = new GoodReceivedDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    GoodReceivedPostViewModel GRNObj = JsonConvert.DeserializeObject<GoodReceivedPostViewModel>(ObjMasterItem);
                    List<GoodReceivedDetail> Childitems = JsonConvert.DeserializeObject<List<GoodReceivedDetail>>(ObjChildItem);
                    Type = GRNObj.GRNType;
                    if (Type == "PO")
                    {
                        obj.PurchaseOrderID = GRNObj.PurchaseOrderID;
                        POId= GRNObj.PurchaseOrderID;
                    }
                    else if(Type == "ADJ")
                    {
                        obj.AdjustmentNoteID = GRNObj.AdjustmentNoteID;

                    }
                    else if (Type == "STN")
                    {
                        obj.StoreTransferNoteId = GRNObj.StoreTransferNoteId;
                        //StnbrchId =Convert.ToInt64(GRNObj.BranchIdForSTN);

                    }
                    obj.OrganizationID = GRNObj.OrganizationID;
                    obj.BranchId = GRNObj.BranchId;
                   
                    obj.GRNNO = dALCode.AutoGenerateGoodReceivedsCode(Convert.ToInt64(GRNObj.OrganizationID), Convert.ToInt64(GRNObj.BranchId));
                    obj.GRNDate = GRNObj.GRNDate;
                    obj.GRNType = GRNObj.GRNType;
                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.CreatedBy = userID;
                    obj.CreatedDate = DateTime.Now;
                    db.GoodReceiveds.Add(obj);
                    db.SaveChanges();
                    GRNId = obj.GoodReceivedNoteId;
                    orgId = Convert.ToInt64(obj.OrganizationID);
                    brchId = Convert.ToInt64(obj.BranchId);
                    



                    foreach (var item in Childitems)
                    {
                        string StockCode = "";
                        // For Udpate of Purchase Order Details and get A Total Amount which Qunatity Received and also get a Avg Unit rate
                        if (Type == "PO")
                        {
                            
                            purchaseOrderDAL.UpdateQuantityFromGoodsReceived(item.PurchaseOrderDetailID, item.ReceiveQuantity);
                          
                        }
                        else if (Type == "ADJ")
                        {
                           
                            adjustmentNotesDAL.UpdateQuantityFromGoodsReceived(item.AdjustmentNoteDetailID, item.ReceiveQuantity);

                        }
                        else if (Type == "STN")
                        {
                           
                            storeTransferNotesDAL.UpdateQuantityFromGoodsReceived(item.STNDetailsId, item.ReceiveQuantity);
                        }
                        decimal TotalAmount = TotalAmountGRN(Convert.ToInt64(item.ReceiveQuantity), Convert.ToDecimal(item.UnitPrice));
                        decimal AvgUnit = AvrUnitRateGRN(Convert.ToInt64(item.ReceiveQuantity), TotalAmount);
                        //Get Stock Code and Id
                        StockCode = stockDAL.AutoGenerateStockCode(Convert.ToInt64(item.ProductID), brchId);
                        long StockID = stockDAL.AddorUpdateStock(StockCode, Convert.ToInt64(item.ReceiveQuantity), Convert.ToDecimal(item.UnitPrice), orgId, brchId,Convert.ToInt64(item.ProductID));

                        objChild.StockID = StockID;

                        objChild.GoodReceivedNoteID = GRNId;
                        objChild.GRNType = Type;
                        if (Type == "PO")
                        {
                            objChild.PurchaseOrderDetailID = item.PurchaseOrderDetailID;
                        }
                        else if (Type == "ADJ")
                        {
                            objChild.AdjustmentNoteDetailID = item.AdjustmentNoteDetailID;
                        }
                        else if (Type == "STN")
                        {
                            objChild.STNDetailsId = item.STNDetailsId;
                        }


                        objChild.ProductID = item.ProductID;
                        objChild.OrderQuantity = item.OrderQuantity;
                        objChild.ReceiveQuantity = item.ReceiveQuantity;
                        objChild.AlreadyReceivedQuantity = item.ReceiveQuantity;
                        objChild.FirstReceiveQuantity = item.ReceiveQuantity;
                        objChild.BalanceQuantity = item.ReceiveQuantity;
                        objChild.UnitPrice = item.UnitPrice;
                        objChild.TotalPrice = TotalAmount;
                        objChild.AvgUnitPrice = AvgUnit;
                        objChild.Active = true;
                        db.GoodReceivedDetails.Add(objChild);
                        db.SaveChanges();
                    }
                    //this is the BackEnd Job For Change Status For PO and Others
                    if (Type == "PO")
                    {
                        purchaseOrderDAL.IsCompletedJob(POId, userID);
                    }
                    else
                    {
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

        public string UpdateGoodsReceived(string objMasterItem, string objChilds, long userID)
        {
            long GRNId = 0;
            long orgId = 0;
            long brchId = 0;
            string Type = "";
            long? POId = 0;
            string violationMessage = "";
            GoodReceived obj = new GoodReceived();
            GoodReceivedDetail objChild = new GoodReceivedDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    GoodReceived GRNObj = JsonConvert.DeserializeObject<GoodReceived>(objMasterItem);
                    List<GoodReceivedDetail> Childitems = JsonConvert.DeserializeObject<List<GoodReceivedDetail>>(objChilds);


                    obj = db.GoodReceiveds.Where(a => a.GoodReceivedNoteId == GRNObj.GoodReceivedNoteId).FirstOrDefault();
                    Type = obj.GRNType;
                    if (Type == "PO")
                    {
                        obj.PurchaseOrderID = GRNObj.PurchaseOrderID;
                        POId= GRNObj.PurchaseOrderID;
                    }
                    else if (Type == "ADJ")
                    {
                        obj.AdjustmentNoteID = GRNObj.AdjustmentNoteID;

                    }
                    else if (Type == "STN")
                    {
                        obj.StoreTransferNoteId = GRNObj.StoreTransferNoteId;

                    }
                    obj.OrganizationID = GRNObj.OrganizationID;
                    obj.BranchId = GRNObj.BranchId;
                    obj.GRNNO = GRNObj.GRNNO;
                    obj.GRNDate = GRNObj.GRNDate;
                    obj.GRNType = GRNObj.GRNType;

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.UpdatedBy = userID;
                    obj.UpdatedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();

                    GRNId = obj.GoodReceivedNoteId;
                    orgId = Convert.ToInt64(obj.OrganizationID);
                    brchId = Convert.ToInt64(obj.BranchId);
                    
                    foreach (var item in Childitems)
                    {



                        var findResult = db.GoodReceivedDetails.Where(a => a.GoodReceivedNoteDetailID == item.GoodReceivedNoteDetailID).FirstOrDefault();

                        if (findResult != null)
                        {
                            var AlRecQty = findResult.ReceiveQuantity;
                            var nowRecQty = item.ReceiveQuantity;
                            var resultRecQty = AlRecQty + nowRecQty;
                            // For Udpate of Purchase Order Details and get A Total Amount which Qunatity Received and also get a Avg Unit rate

                            if (Type == "PO" && item.ReceiveQuantity !=null)
                            {
                                purchaseOrderDAL.UpdateQuantityFromGoodsReceived(item.PurchaseOrderDetailID, item.ReceiveQuantity);
                            }
                            else if (Type == "ADJ" && item.ReceiveQuantity != null)
                            {
                                adjustmentNotesDAL.UpdateQuantityFromGoodsReceived(item.AdjustmentNoteDetailID, item.ReceiveQuantity);

                            }
                            else if (Type == "STN" && item.ReceiveQuantity != null)
                            {
                                storeTransferNotesDAL.UpdateQuantityFromGoodsReceived(item.STNDetailsId, item.ReceiveQuantity);

                            }
                            decimal TotalAmount = TotalAmountGRN(Convert.ToInt64(resultRecQty), Convert.ToDecimal(item.UnitPrice));
                            decimal TotalAmountStock = TotalAmountGRN(Convert.ToInt64(item.ReceiveQuantity), Convert.ToDecimal(item.UnitPrice));
                            decimal AvgUnit = AvrUnitRateGRN(Convert.ToInt64(resultRecQty), TotalAmount);
                            //Get Stock Code
                            string StockCode = stockDAL.AutoGenerateStockCode(Convert.ToInt64(item.ProductID), brchId);
                            long StockID = stockDAL.AddorUpdateStock(StockCode, Convert.ToInt64(item.ReceiveQuantity), Convert.ToDecimal(item.UnitPrice), orgId, brchId, Convert.ToInt64(item.ProductID));

                            //---------
                            findResult.StockID = StockID;
                            findResult.GoodReceivedNoteID = GRNId;
                            findResult.PurchaseOrderDetailID = item.PurchaseOrderDetailID;
                            findResult.ProductID = item.ProductID;
                            findResult.OrderQuantity = item.OrderQuantity;
                            findResult.ReceiveQuantity = resultRecQty;
                            objChild.BalanceQuantity = resultRecQty;
                            findResult.AlreadyReceivedQuantity = resultRecQty;

                            findResult.UnitPrice = item.UnitPrice;
                            findResult.TotalPrice = TotalAmount;
                            findResult.AvgUnitPrice = AvgUnit;
                            findResult.Active = true;
                            if (Type == "PO")
                            {
                                findResult.PurchaseOrderDetailID = item.PurchaseOrderDetailID;
                            }
                            else if (Type == "ADJ")
                            {
                                findResult.AdjustmentNoteDetailID = item.AdjustmentNoteDetailID;
                            }
                            else if (Type == "STN")
                            {
                                findResult.STNDetailsId = item.STNDetailsId;
                            }
                            db.Entry(findResult).State = EntityState.Modified;
                            db.SaveChanges();
                        }



                    }
                    if (Type == "PO")
                    {
                        purchaseOrderDAL.IsCompletedJob(POId, userID);
                    }
                    else
                    {
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
        public string DeleteGoodsReceived(GoodReceived goodReceived, long userID)
        {
            long GRNId = 0;

            string violationMessage = "";
            GoodReceived obj = new GoodReceived();
            GoodReceivedDetail objChild = new GoodReceivedDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    obj = db.GoodReceiveds.Where(a => a.GoodReceivedNoteId == goodReceived.GoodReceivedNoteId).FirstOrDefault();
                    obj.IsDeleted = true;
                    obj.DeletedBy = userID;
                    obj.DeletedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    GRNId = obj.GoodReceivedNoteId;
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
        public decimal TotalAmountGRN(long Quantity, decimal UnitRate)
        {
            decimal totalAmt = 0;
            if (Quantity != 0 && UnitRate != 0)
            {
                totalAmt = Quantity * UnitRate;
            }

            return totalAmt;
        }
        public decimal AvrUnitRateGRN(long Quantity, decimal TotalAmt)
        {
            decimal avgUnit = 0;
            if (Quantity != 0 && TotalAmt != 0)
            {
                avgUnit = TotalAmt / Quantity;

            }
            return avgUnit;
        }
        public void UpdateQuantityFromGoodsReturn(long? GoodsDetailsID, long? Quantity)
        {
            long balQty = 0;
            long grtnQty = 0;
            long resultQty = 0;
            if (Quantity != null)
            {
                grtnQty = Convert.ToInt64(Quantity);
            }
            try
            {
                var findResult = db.GoodReceivedDetails.Where(a => a.GoodReceivedNoteDetailID == GoodsDetailsID).FirstOrDefault();
                if (findResult != null)
                {
                    if (findResult.BalanceQuantity != 0)
                    {
                        if (grtnQty <= findResult.BalanceQuantity)
                        {
                            balQty = Convert.ToInt64(findResult.BalanceQuantity);
                            resultQty = balQty - grtnQty;
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
    }
}