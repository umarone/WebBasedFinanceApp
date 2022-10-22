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
    public class AdjustmentNotesDAL
    {
        AppEntities db = new AppEntities(); 
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        public string AddAdjustmentNotes(string objMasterItem, string objChilds, long userID)
        {
            long adid = 0;
            string violationMessage = "";
            AdjustmentNote obj = new AdjustmentNote();
            AdjustmentNoteDetail objChild = new AdjustmentNoteDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {



                    AdjustmentNote AdjObj = JsonConvert.DeserializeObject<AdjustmentNote>(objMasterItem);
                    List<AdjustmentNoteDetail> Childitems = JsonConvert.DeserializeObject<List<AdjustmentNoteDetail>>(objChilds);


                    obj.OrganizationID = AdjObj.OrganizationID;
                    obj.BranchId = AdjObj.BranchId;
                    obj.AdjustmentNoteNO = dALCode.AutoGenerateAdjustmentNoteCode(Convert.ToInt64(AdjObj.OrganizationID), Convert.ToInt64(AdjObj.BranchId));
                    obj.Date = AdjObj.Date;
                    obj.Description = AdjObj.Description;
                    obj.SubTotalWithOutSaleTax = AdjObj.SubTotalWithOutSaleTax;
                    obj.SubTotalWithSaleTax = AdjObj.SubTotalWithSaleTax;
                    obj.FreightCharges = AdjObj.FreightCharges;
                    obj.NetTotal = AdjObj.NetTotal;
                    obj.AmountInWord = AdjObj.AmountInWord;

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.CreatedBy = userID;
                    obj.CreatedDate = DateTime.Now;
                    db.AdjustmentNotes.Add(obj);
                    db.SaveChanges();
                    adid = obj.AdjustmentNoteId;

                    foreach (var item in Childitems)
                    {
                        objChild.AdjustmentNoteID = adid;
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
                        db.AdjustmentNoteDetails.Add(objChild);
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
        public string UpdateAdjustmentNotes(string objMasterItem, string objChilds, long userID)
        {
            long adid = 0;
            string violationMessage = "";
            AdjustmentNote obj = new AdjustmentNote();
            AdjustmentNoteDetail objChild = new AdjustmentNoteDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    AdjustmentNote AdjObj = JsonConvert.DeserializeObject<AdjustmentNote>(objMasterItem);
                    List<AdjustmentNoteDetail> Childitems = JsonConvert.DeserializeObject<List<AdjustmentNoteDetail>>(objChilds);


                    obj = db.AdjustmentNotes.Find(AdjObj.AdjustmentNoteId);
                    obj.OrganizationID = AdjObj.OrganizationID;
                    obj.BranchId = AdjObj.BranchId;
                    obj.AdjustmentNoteNO = AdjObj.AdjustmentNoteNO;
                    obj.Date = AdjObj.Date;
                    obj.Description = AdjObj.Description;
                    obj.SubTotalWithOutSaleTax = AdjObj.SubTotalWithOutSaleTax;
                    obj.SubTotalWithSaleTax = AdjObj.SubTotalWithSaleTax;
                    obj.FreightCharges = AdjObj.FreightCharges;
                    obj.NetTotal = AdjObj.NetTotal;
                    obj.AmountInWord = AdjObj.AmountInWord;

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.UpdatedBy = userID;
                    obj.UpdatedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    adid = obj.AdjustmentNoteId;

                    foreach (var item in Childitems)
                    {

                        var findResult = db.AdjustmentNoteDetails.Where(a => a.AdjustmentNoteDetailId == item.AdjustmentNoteDetailId).FirstOrDefault();
                        if (findResult != null)
                        {
                            findResult.AdjustmentNoteID = adid;
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
                            objChild.AdjustmentNoteID = adid;
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
                            db.AdjustmentNoteDetails.Add(objChild);
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
        public string DeleteAdjustmentNotes(AdjustmentNote adjustmentNote, long userID)
        {
            long adid = 0;
            string violationMessage = "";
            AdjustmentNote obj = new AdjustmentNote();
            AdjustmentNoteDetail objChild = new AdjustmentNoteDetail();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    obj = db.AdjustmentNotes.Find(adjustmentNote.AdjustmentNoteId);


                    obj.IsDeleted = true;
                    obj.DeletedBy = userID;
                    obj.DeletedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    adid = obj.AdjustmentNoteId;



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
        public void UpdateQuantityFromGoodsReceived(long? adjustmentNoteDetailID, long? receiveQuantity)
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
                var findResult = db.AdjustmentNoteDetails.Where(a => a.AdjustmentNoteDetailId == adjustmentNoteDetailID).FirstOrDefault();
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


        public string DeleteAdjustmentNotesDetails(string Id)
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

                    var findResult = db.AdjustmentNoteDetails.Find(id);
                    db.AdjustmentNoteDetails.Remove(findResult);
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