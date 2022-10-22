using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;

namespace MudasirRehmanAlp.AppDAL
{
    public class StockDAL
    {
        private AppEntities db = new AppEntities();
        public string AutoGenerateStockCode(long productID,long branchId)
        {
            string Code = "";
            //Organization+Branch+Prodcode+Category
            if (productID != 0)
            {
                var findProdcut = db.ProductDefinitions.Where(a => a.ProductId == productID).FirstOrDefault();
                var findBranch = db.BranchDefinitions.Where(a => a.Id == branchId).FirstOrDefault();
                if (findProdcut != null && findBranch !=null)
                {
                    Code = findProdcut.OrganizationDefinition.OrganizationUnitCode+ findBranch.Code+ findProdcut.ProductCode + findProdcut.CategoryDefinition.CategoryCode;
                }
            }
            return Code;
        }
        public long AddorUpdateStock(string stockCode, long Qunatity, decimal lastPurchaseRate, long OrgId,long BrchId ,long ProductId)
        {
            long StockID = 0;
            long alreadyStockQty = 0;          
            long finalStockQty = 0;
       
            string violationMessage = "";
            StockofGoods obj = new StockofGoods();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var findStockResult = db.StockofGoods.Where(a => a.Active == true && a.StockCode == stockCode && a.OrganizationID== OrgId && a.BranchId== BrchId).FirstOrDefault();
                    if (findStockResult != null)
                    {
                        alreadyStockQty = Convert.ToInt64(findStockResult.Quantity);
                        finalStockQty = alreadyStockQty + Qunatity;
                        findStockResult.Quantity = finalStockQty;
                        findStockResult.LastPurchaseUnitRate = lastPurchaseRate;
                        findStockResult.InDate = DateTime.Now;
                        db.Entry(findStockResult).State = EntityState.Modified;
                        db.SaveChanges();
                        StockID = findStockResult.StockID;
                    }
                    else
                    {
                        obj.StockCode = stockCode;
                        obj.OrganizationID = OrgId;
                        obj.BranchId = BrchId;
                        obj.ProductID = ProductId;
                        obj.Quantity = Qunatity;
                        obj.LastPurchaseUnitRate = lastPurchaseRate;
                        obj.Active = true;
                        obj.InDate = DateTime.Now;
                        db.StockofGoods.Add(obj);
                        db.SaveChanges();
                        StockID = obj.StockID;
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

            return StockID;
        }
        public long GetStockQuantity(string stockCode)
        {
            long QTY = 0;
            //Organization+Prodcut+Category
            if (stockCode != "")
            {
                var findResult = db.StockofGoods.Where(a => a.StockCode == stockCode).FirstOrDefault();
                if (findResult != null)
                {
                    QTY = Convert.ToInt64(findResult.Quantity);
                }
            }
            return QTY;
        }
        public long GetStockIDByStockCode(string stockCode)
        {
            long ID = 0;
            //Organization+Prodcut+Category
            if (stockCode != "")
            {
                var findResult = db.StockofGoods.Where(a => a.StockCode == stockCode).FirstOrDefault();
                if (findResult != null)
                {
                    ID = findResult.StockID;
                }
            }
            return ID;
        }
        public string RemoveorUpdateStock(long StockId,long Qunatity)
        {
           
            long alreadyStockQty = 0;
         
            long finalStockQty = 0;
           
            string violationMessage = "";
            StockofGoods obj = new StockofGoods();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var findStockResult = db.StockofGoods.Where(a => a.Active == true && a.StockID == StockId).FirstOrDefault();
                    if (findStockResult != null)
                    {
                        alreadyStockQty = Convert.ToInt64(findStockResult.Quantity);
                       
                        finalStockQty = alreadyStockQty - Qunatity;
                    
                        findStockResult.Quantity = finalStockQty;
                        findStockResult.OutDate = DateTime.Now;
                        db.Entry(findStockResult).State = EntityState.Modified;
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
       
        public string RemoveorUpdateStockSaleOrder(long StockId, long Qunatity)
        {

            long alreadyStockQty = 0;           
            long finalStockQty = 0;
         
            string violationMessage = "";
            StockofGoods obj = new StockofGoods();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var findStockResult = db.StockofGoods.Where(a => a.Active == true && a.StockID == StockId).FirstOrDefault();
                    if (findStockResult != null)
                    {
                        alreadyStockQty = Convert.ToInt64(findStockResult.Quantity); 
                        finalStockQty = alreadyStockQty - Qunatity;  
                        findStockResult.Quantity = finalStockQty;
                        findStockResult.OutDate = DateTime.Now;

                        db.Entry(findStockResult).State = EntityState.Modified;
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
      
        public string AddorUpdateStockSaleResturn(long StockId, long Qunatity)
        {

            long alreadyStockQty = 0;

            long finalStockQty = 0;

            string violationMessage = "";
            StockofGoods obj = new StockofGoods();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var findStockResult = db.StockofGoods.Where(a => a.Active == true && a.StockID == StockId).FirstOrDefault();
                    if (findStockResult != null)
                    {
                        alreadyStockQty = Convert.ToInt64(findStockResult.Quantity);

                        finalStockQty = alreadyStockQty + Qunatity;

                        findStockResult.Quantity = finalStockQty;
                        findStockResult.InDate = DateTime.Now;
                        db.Entry(findStockResult).State = EntityState.Modified;
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
    }
}