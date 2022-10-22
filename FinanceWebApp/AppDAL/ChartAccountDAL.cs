using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;

namespace MudasirRehmanAlp.AppDAL
{
    public class ChartAccountDAL
    {
        AppEntities db = new AppEntities();

        public string AddAccount(AccountsViewModel accountObj, long userID)
        {

            string violationMessage = "";
            long AccountNo = 0;
            Account obj = new Account();


            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    var FindAccount = db.Accounts.Where(a => a.IsActive == true && a.IsDeleted == false && a.AccountNo == accountObj.AccountNo && a.OrganizationID == accountObj.OrganizationID && a.BranchId == accountObj.BranchId).FirstOrDefault();
                    if (FindAccount != null)
                    {
                        AccountNo = Convert.ToInt64(FindAccount.AccountNo);
                        AccountNo = AccountNo + 1;
                        obj.OrganizationID = accountObj.OrganizationID;
                        obj.BranchId = accountObj.BranchId;
                        obj.ParentID = accountObj.ParentID;
                        obj.AccountNo = AccountNo.ToString();
                        obj.AccountName = accountObj.AccountName;
                        obj.AccountType = accountObj.AccountType;
                        obj.LevelID = accountObj.LevelID;
                        obj.HeadID = accountObj.HeadID;
                        obj.Description = accountObj.Description;
                        obj.IsActive = accountObj.IsActive;

                        obj.IsDeleted = false;
                        obj.IsCustomer = false;
                        obj.CreatedBy = userID;
                        obj.CreatedDate = DateTime.Now;
                        db.Accounts.Add(obj);
                        db.SaveChanges();
                        accountObj.AccountId = obj.AccountId;
                        #region OpeningBalance_Region
                        violationMessage = AddandUpdateOpeningBalance(accountObj, userID);
                        #endregion

                        #region MappingAccount_Region
                        AddandUpdateMappingAccounts(accountObj);
                        #endregion

                    }
                    else
                    {
                        obj.OrganizationID = accountObj.OrganizationID;
                        obj.BranchId = accountObj.BranchId;
                        obj.ParentID = accountObj.ParentID;
                        obj.AccountNo = accountObj.AccountNo;
                        obj.AccountName = accountObj.AccountName;
                        obj.AccountType = accountObj.AccountType;
                        obj.LevelID = accountObj.LevelID;
                        obj.HeadID = accountObj.HeadID;
                        obj.Description = accountObj.Description;
                        obj.IsActive = accountObj.IsActive;
                        obj.IsCustomer = false;
                        obj.IsDeleted = false;
                        obj.IsSystemAccount = false;
                        obj.CreatedBy = userID;
                        obj.CreatedDate = DateTime.Now;
                        db.Accounts.Add(obj);
                        db.SaveChanges();
                        accountObj.AccountId = obj.AccountId;
                        #region OpeningBalance_Region
                        violationMessage = AddandUpdateOpeningBalance(accountObj, userID);
                        #endregion

                        #region MappingAccount_Region
                        AddandUpdateMappingAccounts(accountObj);
                        #endregion

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
        public string UpdateAccount(AccountsViewModel accountObj, long userID)
        {
            string violationMessage = "";

            Account obj = new Account();

            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    var FindAccount = db.Accounts.Where(a => a.AccountId == accountObj.AccountId).FirstOrDefault();
                    if (FindAccount != null)
                    {

                        FindAccount.OrganizationID = accountObj.OrganizationID;
                        FindAccount.BranchId = accountObj.BranchId;
                        //obj.ParentID = accountObj.ParentID;
                        //obj.AccountNo = AccountNo.ToString();
                        FindAccount.AccountName = accountObj.AccountName;
                        //obj.AccountType = accountObj.AccountType;
                        //obj.LevelID = accountObj.LevelID;
                        //obj.HeadID = accountObj.HeadID;
                        FindAccount.Description = accountObj.Description;
                        FindAccount.IsActive = accountObj.IsActive;
                        FindAccount.IsCustomer = false;
                        FindAccount.IsDeleted = false;
                        FindAccount.UpdatedBy = userID;
                        FindAccount.UpdatedDate = DateTime.Now;

                        db.Entry(FindAccount).State = EntityState.Modified;
                        db.SaveChanges();
                        accountObj.AccountId = FindAccount.AccountId;
                      
                        #region OpeningBalance_Region
                        violationMessage = AddandUpdateOpeningBalance(accountObj, userID);
                        #endregion

                        #region MappingAccount_Region
                        AddandUpdateMappingAccounts(accountObj);
                        #endregion
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
        public string DeleteAccount(Account accountObj, long userID)
        {
            string violationMessage = "";

            Account obj = new Account();

            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    var FindAccount = db.Accounts.Where(a => a.AccountId == accountObj.AccountId).FirstOrDefault();
                    if (FindAccount != null)
                    {

                        if (FindAccount.IsSystemAccount == true)
                        {
                            violationMessage = "This account is system create and not be deleted";
                        }
                        else
                        {
                            FindAccount.IsDeleted = true;
                            FindAccount.DeletedBy = userID;
                            FindAccount.DeletedDate = DateTime.Now;

                            db.Entry(FindAccount).State = EntityState.Modified;
                            db.SaveChanges();
                            accountObj.AccountId = FindAccount.AccountId;
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
        //------------- Add Opening Balance-- This is Backend Job
        public string AddandUpdateOpeningBalance(AccountsViewModel classObj, long uerID)
        {

            string violationMessage = "";
            OpeningBalance obj = new OpeningBalance();
            try
            {
                var findfinancialBookYear = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == classObj.OrganizationID && a.BranchId == classObj.BranchId).FirstOrDefault();
                //------ This is The Customer Case
                if (classObj.AccountType == "Accounts Recieveables / Customers")
                {
                    var findObj = db.OpeningBalance.Where(a => a.IsActive == true && a.IsDeleted == false && a.AccountId == classObj.AccountId).FirstOrDefault();
                    if (findObj != null)
                    {

                        findObj.OrganizationID = classObj.OrganizationID;
                        findObj.BranchId = classObj.BranchId;
                        findObj.AccountId = classObj.AccountId;
                        if (findfinancialBookYear != null)
                        {
                            findObj.FinancialBookYearId = findfinancialBookYear.Id;

                        }
                        findObj.OpeningBalanceDate = DateTime.Now;
                        findObj.TransactionMode = String.Empty;
                        findObj.EmpCode = String.Empty;
                        findObj.Description = classObj.Description;
                        findObj.TotalAmount = classObj.OpeningBalance;
                        string NumType = CommonFunctions.CheckNumberPositiveorNegative(Convert.ToDecimal(classObj.OpeningBalance));
                        if (NumType == "+")
                        {
                            findObj.Debit = classObj.OpeningBalance;
                            findObj.Credit = 0;
                        }
                        else if (NumType == "-")
                        {
                            findObj.Credit = classObj.OpeningBalance * (-1);
                            findObj.Debit = 0;

                        }
                        findObj.IsActive = true;
                        findObj.IsDeleted = false;
                        findObj.UpdatedBy = uerID;
                        findObj.UpdatedDate = DateTime.Now;
                        db.Entry(findObj).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        obj.OrganizationID = classObj.OrganizationID;
                        obj.BranchId = classObj.BranchId;
                        obj.AccountId = classObj.AccountId;
                        if (findfinancialBookYear != null)
                        {
                            obj.FinancialBookYearId = findfinancialBookYear.Id;

                        }
                        obj.OpeningBalanceDate = DateTime.Now;
                        obj.TransactionMode = String.Empty;
                        obj.EmpCode = String.Empty;
                        obj.Description = classObj.Description;
                        obj.TotalAmount = classObj.OpeningBalance;
                        string NumType = CommonFunctions.CheckNumberPositiveorNegative(Convert.ToDecimal(classObj.OpeningBalance));
                        if (NumType == "+")
                        {
                            obj.Debit = classObj.OpeningBalance;
                            obj.Credit = 0;
                        }
                        else if (NumType == "-")
                        {
                            obj.Credit = classObj.OpeningBalance * (-1);
                            obj.Debit = 0;
                        }
                        obj.IsActive = true;
                        obj.IsDeleted = false;
                        obj.CreatedBy = uerID;
                        obj.CreatedDate = DateTime.Now;
                        db.OpeningBalance.Add(obj);
                        db.SaveChanges();
                    }
                }
                //------ This is The Supplier Case
                else if (classObj.AccountType == "Accounts Payables")
                {
                    var findObj = db.OpeningBalance.Where(a => a.IsActive == true && a.IsDeleted == false && a.AccountId == classObj.AccountId).FirstOrDefault();
                    if (findObj != null)
                    {

                        findObj.OrganizationID = classObj.OrganizationID;
                        findObj.BranchId = classObj.BranchId;
                        findObj.AccountId = classObj.AccountId;
                        if (findfinancialBookYear != null)
                        {
                            findObj.FinancialBookYearId = findfinancialBookYear.Id;

                        }
                        findObj.OpeningBalanceDate = DateTime.Now;
                        findObj.TransactionMode = String.Empty;
                        findObj.EmpCode = String.Empty;
                        findObj.Description = classObj.Description;
                        findObj.TotalAmount = classObj.OpeningBalance;
                        string NumType = CommonFunctions.CheckNumberPositiveorNegative(Convert.ToDecimal(classObj.OpeningBalance));
                        if (NumType == "+")
                        {
                            findObj.Credit = classObj.OpeningBalance;
                            findObj.Debit = 0;
                        }
                        else if (NumType == "-")
                        {
                            findObj.Debit = classObj.OpeningBalance * (-1);
                            findObj.Credit = 0;

                        }
                        findObj.IsActive = true;
                        findObj.IsDeleted = false;
                        findObj.UpdatedBy = uerID;
                        findObj.UpdatedDate = DateTime.Now;
                        db.Entry(findObj).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        obj.OrganizationID = classObj.OrganizationID;
                        obj.BranchId = classObj.BranchId;
                        obj.AccountId = classObj.AccountId;
                        if (findfinancialBookYear != null)
                        {
                            obj.FinancialBookYearId = findfinancialBookYear.Id;
                        }
                        obj.OpeningBalanceDate = DateTime.Now;
                        obj.TransactionMode = String.Empty;
                        obj.EmpCode = String.Empty;
                        obj.Description = classObj.Description;
                        obj.TotalAmount = classObj.OpeningBalance;
                        string NumType = CommonFunctions.CheckNumberPositiveorNegative(Convert.ToDecimal(classObj.OpeningBalance));
                        if (NumType == "+")
                        {
                            obj.Credit = classObj.OpeningBalance;
                            obj.Debit = 0;
                        }
                        else if (NumType == "-")
                        {
                            obj.Debit = classObj.OpeningBalance * (-1);
                            obj.Credit = 0;

                        }
                        obj.IsActive = true;
                        obj.IsDeleted = false;
                        obj.CreatedBy = uerID;
                        obj.CreatedDate = DateTime.Now;
                        db.OpeningBalance.Add(obj);
                        db.SaveChanges();
                    }
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
        public void AddandUpdateMappingAccounts(AccountsViewModel classObj)
        {

            string violationMessage = "";
            
            try
            {
              var  findObjMap = db.AccountMapping.Where(a => a.AccountId == classObj.AccountId).FirstOrDefault();
                if (findObjMap != null)
                {
                    findObjMap.OrganizationId = classObj.OrganizationID;
                    findObjMap.BranchId = classObj.MappingBranchId;
                    findObjMap.AccountId = classObj.AccountId;
                    findObjMap.AccountDefaultType = classObj.AccountDefaultType;

                    db.Entry(findObjMap).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    AccountMapping objMap = new AccountMapping();
                    objMap.OrganizationId = classObj.OrganizationID;
                    objMap.BranchId = classObj.MappingBranchId;
                    objMap.AccountId = classObj.AccountId;
                    objMap.AccountDefaultType = classObj.AccountDefaultType;

                    db.AccountMapping.Add(objMap);
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
        }

        //====================
        //---- This Is For Customer Accounts----
        //====================
        public string AddCustomerAccounts(AccountsViewModel accountObj, long userID)
        {

            string violationMessage = "";
            long AccountNo = 0;
            Account obj = new Account();

            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    var FindAccount = db.Accounts.Where(a => a.IsActive == true && a.IsDeleted == false && a.AccountNo == accountObj.AccountNo && a.OrganizationID == accountObj.OrganizationID && a.BranchId == accountObj.BranchId).FirstOrDefault();
                    if (FindAccount != null)
                    {
                        AccountNo = Convert.ToInt64(FindAccount.AccountNo);
                        AccountNo = AccountNo + 1;
                        obj.OrganizationID = accountObj.OrganizationID;
                        obj.BranchId = accountObj.BranchId;
                        obj.ParentID = accountObj.ParentID;
                        obj.AccountNo = AccountNo.ToString();
                        obj.AccountName = accountObj.AccountName;
                        obj.AccountType = accountObj.AccountType;
                        obj.LevelID = accountObj.LevelID;
                        obj.HeadID = accountObj.HeadID;
                        obj.Description = accountObj.Description;
                        obj.IsActive = accountObj.IsActive;

                        obj.IsDeleted = false;
                        obj.IsCustomer = true;
                        obj.CreatedBy = userID;
                        obj.CreatedDate = DateTime.Now;
                        db.Accounts.Add(obj);
                        db.SaveChanges();
                        accountObj.AccountId = obj.AccountId;
                        violationMessage = AddandUpdateOpeningBalance(accountObj, userID);
                    }
                    else
                    {
                        obj.OrganizationID = accountObj.OrganizationID;
                        obj.BranchId = accountObj.BranchId;
                        obj.ParentID = accountObj.ParentID;
                        obj.AccountNo = accountObj.AccountNo;
                        obj.AccountName = accountObj.AccountName;
                        obj.AccountType = accountObj.AccountType;
                        obj.LevelID = accountObj.LevelID;
                        obj.HeadID = accountObj.HeadID;
                        obj.Description = accountObj.Description;
                        obj.IsActive = accountObj.IsActive;
                        obj.IsCustomer = true;
                        obj.IsDeleted = false;
                        obj.IsSystemAccount = false;
                        obj.CreatedBy = userID;
                        obj.CreatedDate = DateTime.Now;
                        db.Accounts.Add(obj);
                        db.SaveChanges();
                        accountObj.AccountId = obj.AccountId;
                        violationMessage = AddandUpdateOpeningBalance(accountObj, userID);

                    }
                    if (accountObj.CustomerStatementId != null)
                    {
                        var objFind = db.CustomerStatements.Where(a => a.Id == accountObj.CustomerStatementId).FirstOrDefault();
                        objFind.AccountID = accountObj.AccountId;
                        objFind.UpdatedBy = userID;
                        objFind.UpdatedDate = DateTime.Now;
                        db.Entry(objFind).State = EntityState.Modified;
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
        public string UpdateCustomerAccounts(AccountsViewModel accountObj, long userID)
        {
            string violationMessage = "";

            Account obj = new Account();

            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var FindAccount = db.Accounts.Where(a => a.AccountId == accountObj.AccountId).FirstOrDefault();
                    if (FindAccount != null)
                    {
                        FindAccount.OrganizationID = accountObj.OrganizationID;
                        FindAccount.BranchId = accountObj.BranchId;
                        FindAccount.AccountName = accountObj.AccountName;
                        FindAccount.Description = accountObj.Description;
                        FindAccount.IsActive = accountObj.IsActive;
                        FindAccount.IsCustomer = true;
                        FindAccount.IsDeleted = false;
                        FindAccount.UpdatedBy = userID;
                        FindAccount.UpdatedDate = DateTime.Now;
                        db.Entry(FindAccount).State = EntityState.Modified;
                        db.SaveChanges();
                        accountObj.AccountId = FindAccount.AccountId;
                        violationMessage = AddandUpdateOpeningBalance(accountObj, userID);
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