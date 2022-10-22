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

namespace MudasirRehmanAlp.AppDAL
{
    public class GoodReturnDAL
    {
        AppEntities db = new AppEntities();
        StockDAL stockDAL = new StockDAL();
        GoodReceivedDAL goodReceivedDAL = new GoodReceivedDAL();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        public string AddGoodsRetrun(string objMasterItem, string objChilds, long userID)
        {
            long GRTNId = 0;
            long orgId = 0;

            string violationMessage = "";
            GoodReceivedReturn obj = new GoodReceivedReturn();
            GoodReceivedReturnDetail objChild = new GoodReceivedReturnDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    GoodReceivedReturn GRTNObj = JsonConvert.DeserializeObject<GoodReceivedReturn>(objMasterItem);
                    List<GoodReceivedReturnDetail> Childitems = JsonConvert.DeserializeObject<List<GoodReceivedReturnDetail>>(objChilds);

                    obj.OrganizationID = GRTNObj.OrganizationID;
                    obj.GoodsReceivedID = GRTNObj.GoodsReceivedID;
                    obj.BranchId = GRTNObj.BranchId;
                    obj.GRReturnNO = dALCode.AutoGenerateGoodReceivedsReturnCode(Convert.ToInt64(GRTNObj.OrganizationID),Convert.ToInt64(GRTNObj.BranchId));
                    obj.MainReason = GRTNObj.MainReason;
                    obj.Date = GRTNObj.Date;
                    
                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.CreatedBy = userID;
                    obj.CreatedDate = DateTime.Now;
                    db.GoodReceivedReturns.Add(obj);
                    db.SaveChanges();
                    GRTNId = obj.GoodsReceivedReturnID;
                    orgId = Convert.ToInt64(obj.OrganizationID);

                    foreach (var item in Childitems)
                    {
                        // For Udpate of Purchase Order Details and get A Total Amount which Qunatity Received and also get a Avg Unit rate
                        goodReceivedDAL.UpdateQuantityFromGoodsReturn(item.GoodsReceivedDetailID, item.ReturnQuantity);
                        decimal TotalAmount = TotalAmountGTRN(Convert.ToInt64(item.ReturnQuantity), Convert.ToDecimal(item.UnitPrice));
                        decimal AvgUnit = AvrUnitRateGRTN(Convert.ToInt64(item.ReturnQuantity), TotalAmount);
                        //Get Stock Code

                        var oK = stockDAL.RemoveorUpdateStock(Convert.ToInt64(item.StockID), Convert.ToInt64(item.ReturnQuantity));

                        objChild.StockID = item.StockID;
                        objChild.GoodsReceivedReturnID = GRTNId;
                        objChild.GoodsReceivedDetailID = item.GoodsReceivedDetailID;
                        objChild.ProductID = item.ProductID;
                        objChild.ReturnQuantity = item.ReturnQuantity;
                        objChild.ReceiveQuantity = item.ReceiveQuantity;
                        objChild.AlreadyReturnQuantity = item.ReturnQuantity;

                        objChild.UnitPrice = item.UnitPrice;
                        objChild.TotalPrice = TotalAmount;
                        objChild.AvgUnitPrice = AvgUnit;
                        objChild.Reason = item.Reason;
                        objChild.Active = true;
                        db.GoodReceivedReturnDetails.Add(objChild);
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
        public string UpdateGoodsRetrun(string objMasterItem, string objChilds, long userID)
        {
            long GRTNId = 0;
            long orgId = 0;

            string violationMessage = "";
            GoodReceivedReturn obj = new GoodReceivedReturn();
            GoodReceivedReturnDetail objChild = new GoodReceivedReturnDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    GoodReceivedReturn GRTNObj = JsonConvert.DeserializeObject<GoodReceivedReturn>(objMasterItem);
                    List<GoodReceivedReturnDetail> Childitems = JsonConvert.DeserializeObject<List<GoodReceivedReturnDetail>>(objChilds);

                    obj = db.GoodReceivedReturns.Where(a => a.GoodsReceivedReturnID == GRTNObj.GoodsReceivedReturnID).FirstOrDefault();
                    obj.OrganizationID = GRTNObj.OrganizationID;
                    obj.GoodsReceivedID = GRTNObj.GoodsReceivedID;
                    obj.GRReturnNO = GRTNObj.GRReturnNO;
                    obj.Date = GRTNObj.Date;
                    obj.MainReason = GRTNObj.MainReason;
                    obj.BranchId = GRTNObj.BranchId;
                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.UpdatedBy = userID;
                    obj.UpdatedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    GRTNId = obj.GoodsReceivedReturnID;
                    orgId = Convert.ToInt64(obj.OrganizationID);


                    foreach (var item in Childitems)
                    {
                        objChild = db.GoodReceivedReturnDetails.Where(a => a.GoodsReceivedReturnDetailID == item.GoodsReceivedReturnDetailID).FirstOrDefault();
                        var AlRetQty = objChild.ReturnQuantity;
                        var nowRetQty = item.ReturnQuantity;
                        var resultRetQty = AlRetQty + nowRetQty;


                        // For Udpate of Goods Recived Details and get A Total Amount which Qunatity Received and also get a Avg Unit rate
                        goodReceivedDAL.UpdateQuantityFromGoodsReturn(item.GoodsReceivedDetailID, item.ReturnQuantity);
                        decimal TotalAmount = TotalAmountGTRN(Convert.ToInt64(resultRetQty), Convert.ToDecimal(item.UnitPrice));
                        decimal AvgUnit = AvrUnitRateGRTN(Convert.ToInt64(resultRetQty), TotalAmount);
                        //Get Stock Code

                        var oK = stockDAL.RemoveorUpdateStock(Convert.ToInt64(item.StockID), Convert.ToInt64(item.ReturnQuantity));

                        objChild.StockID = item.StockID;
                        objChild.GoodsReceivedReturnID = GRTNId;
                        objChild.GoodsReceivedDetailID = item.GoodsReceivedDetailID;
                        objChild.ProductID = item.ProductID;
                        objChild.ReturnQuantity = resultRetQty;
                        objChild.ReceiveQuantity = item.ReceiveQuantity;
                        objChild.AlreadyReturnQuantity = resultRetQty;
                        objChild.UnitPrice = item.UnitPrice;
                        objChild.TotalPrice = TotalAmount;
                        objChild.AvgUnitPrice = AvgUnit;
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
        public string DeleteGoodsRetrun(GoodReceivedReturn goodReceivedReturn, long userID)
        {
            long GRTNId = 0;
            long orgId = 0;

            string violationMessage = "";
            GoodReceivedReturn obj = new GoodReceivedReturn();
            GoodReceivedReturnDetail objChild = new GoodReceivedReturnDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    obj = db.GoodReceivedReturns.Where(a => a.GoodsReceivedReturnID == goodReceivedReturn.GoodsReceivedReturnID).FirstOrDefault();

                    obj.IsDeleted = true;
                    obj.DeletedBy = userID;
                    obj.DeletedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    GRTNId = obj.GoodsReceivedReturnID;
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
        private decimal TotalAmountGTRN(long ReturnQuantity, decimal UnitPrice)
        {

            decimal totalAmt = 0;
            if (ReturnQuantity != 0 && UnitPrice != 0)
            {
                totalAmt = ReturnQuantity * UnitPrice;
            }

            return totalAmt;
        }
        private decimal AvrUnitRateGRTN(long ReturnQuantity, decimal TotalAmount)
        {
            decimal avgUnit = 0;
            if (ReturnQuantity != 0 && TotalAmount != 0)
            {
                avgUnit = TotalAmount / ReturnQuantity;

            }
            return avgUnit;
        }


    }
}