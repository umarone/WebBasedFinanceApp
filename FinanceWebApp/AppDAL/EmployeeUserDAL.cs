using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Helpers;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.AppDAL
{
    public class EmployeeUserDAL
    {
        AppEntities db = new AppEntities();

        public string AddUser(EmployeeUsersViewModel viewModel, long userID)
        {

            long userId = 0;
            string violationMessage = "";
            Users obj = new Users();
            UsersRole objRole = new UsersRole();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    obj.OrganizationID = viewModel.OrganizationID;
                    obj.EmployeeID = viewModel.EmployeeID;
                    obj.FullName = viewModel.FullName;
                    obj.UserName = viewModel.UserName;
                    obj.Password = viewModel.Password;
                    obj.Password = PasswordStorage.CreateHash(viewModel.Password);
                    obj.IsActive = viewModel.IsActive;
                   
                    obj.IsDeleted = false;
                    obj.CreatedBy = userID;
                    obj.CreatedDate = DateTime.Now;

                    db.Users.Add(obj);
                    db.SaveChanges();
                    userId = obj.UserId;

                    objRole.UserID = userId;
                    objRole.RoleID = viewModel.RoleID;
                    db.UsersRole.Add(objRole);
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
        public string UpdateUser(EmployeeUsersViewModel usersViewModel, long userID)
        {
            long userId = 0;
            string violationMessage = "";
            Users obj = new Users();
            UsersRole objRole = new UsersRole();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    obj = db.Users.Find(usersViewModel.UserId);
                    obj.OrganizationID = usersViewModel.OrganizationID;
                    obj.EmployeeID = usersViewModel.EmployeeID;
                    obj.FullName = usersViewModel.FullName;
                    obj.UserName = usersViewModel.UserName;

                    obj.Password = PasswordStorage.CreateHash(usersViewModel.Password);
                    obj.IsActive = usersViewModel.IsActive;
                    
                    obj.IsDeleted = false;
                    obj.UpdatedBy = userID;
                    obj.UpdatedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    userId = obj.UserId;

                    objRole = db.UsersRole.Find(usersViewModel.UserRoleID);
                    objRole.UserID = userId;
                    objRole.RoleID = usersViewModel.RoleID;
                    db.Entry(objRole).State = EntityState.Modified;
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
        public string DeleteUser(Users users, long userID)
        {
            long userId = 0;
            string violationMessage = "";
            Users obj = new Users();
            UsersRole objRole = new UsersRole();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    obj = db.Users.Find(users.UserId);

                    obj.IsDeleted = true;
                    obj.DeletedBy = userID;
                    obj.DeletedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    userId = obj.UserId;
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


        public string AddUserWBranch(string userJsonObj, string checkListJsonObj, long userID)
        {

            long userId = 0;
            string violationMessage = "";
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    Users obj = new Users();
                    UsersRole objRole = new UsersRole();
                    EmployeeUsersViewModel viewModel = JsonConvert.DeserializeObject<EmployeeUsersViewModel>(userJsonObj);
                    List<BranchesRights> Parentitem = JsonConvert.DeserializeObject<List<BranchesRights>>(checkListJsonObj);


                    obj.OrganizationID = viewModel.OrganizationID;
                    obj.EmployeeID = viewModel.EmployeeID;
                    obj.FullName = viewModel.FullName;
                    obj.UserName = viewModel.UserName;
                    obj.Password = viewModel.Password;
                    obj.Password = PasswordStorage.CreateHash(viewModel.Password);
                    obj.IsActive = viewModel.IsActive;
                   
                    obj.IsDeleted = false;
                    obj.CreatedBy = userID;
                    obj.CreatedDate = DateTime.Now;

                    db.Users.Add(obj);
                    db.SaveChanges();
                    userId = obj.UserId;

                    objRole.UserID = userId;
                    objRole.RoleID = viewModel.RoleID;
                    db.UsersRole.Add(objRole);
                    db.SaveChanges();


                    foreach (var item in Parentitem)
                    {
                        var findObj = db.BranchesRights.Where(a => a.EmployeeId == item.EmployeeId && a.OrganizationId == item.OrganizationId && a.BranchId == item.BranchId).FirstOrDefault();
                        if (findObj != null)
                        {
                            findObj.OrganizationId = item.OrganizationId;
                            findObj.EmployeeId = item.EmployeeId;
                            findObj.BranchId = item.BranchId;
                            findObj.IsSelected = item.IsSelected;
                            findObj.IsActive = true;
                            findObj.IsDeleted = false;
                            findObj.UpdatedBy = userID;
                            findObj.UpdatedDate = DateTime.Now;
                            db.Entry(findObj).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            BranchesRights objRight = new BranchesRights();
                            objRight.OrganizationId = item.OrganizationId;
                            objRight.OrganizationId = item.OrganizationId;
                            objRight.EmployeeId = item.EmployeeId;
                            objRight.BranchId = item.BranchId;
                            objRight.IsSelected = item.IsSelected;
                            objRight.IsActive = true;
                            objRight.IsDeleted = false;
                            objRight.CreatedBy = userID;
                            objRight.CreatedDate = DateTime.Now;
                            db.BranchesRights.Add(objRight);
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
        public string UpdateUserBranch(string userJsonObj, string checkListJsonObj, long userID)
        {
            long userId = 0;
            string violationMessage = "";
            Users obj = new Users();
            UsersRole objRole = new UsersRole();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    EmployeeUsersViewModel usersViewModel = JsonConvert.DeserializeObject<EmployeeUsersViewModel>(userJsonObj);
                    List<BranchesRights> Parentitem = JsonConvert.DeserializeObject<List<BranchesRights>>(checkListJsonObj);

                    obj = db.Users.Find(usersViewModel.UserId);
                    obj.OrganizationID = usersViewModel.OrganizationID;
                    obj.EmployeeID = usersViewModel.EmployeeID;
                    obj.FullName = usersViewModel.FullName;
                    obj.UserName = usersViewModel.UserName;

                    obj.Password = PasswordStorage.CreateHash(usersViewModel.Password);
                    obj.IsActive = usersViewModel.IsActive;
                   
                    obj.IsDeleted = false;
                    obj.UpdatedBy = userID;
                    obj.UpdatedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    userId = obj.UserId;

                    objRole = db.UsersRole.Find(usersViewModel.UserRoleID);
                    objRole.UserID = userId;
                    objRole.RoleID = usersViewModel.RoleID;
                    db.Entry(objRole).State = EntityState.Modified;
                    db.SaveChanges();

                    foreach (var item in Parentitem)
                    {
                        var findObj = db.BranchesRights.Where(a => a.EmployeeId == item.EmployeeId && a.OrganizationId == item.OrganizationId && a.BranchId == item.BranchId).FirstOrDefault();
                        if (findObj != null)
                        {
                            findObj.OrganizationId = item.OrganizationId;
                            findObj.EmployeeId = item.EmployeeId;
                            findObj.BranchId = item.BranchId;
                            findObj.IsSelected = item.IsSelected;
                            findObj.IsActive = true;
                            findObj.IsDeleted = false;
                            findObj.UpdatedBy = userID;
                            findObj.UpdatedDate = DateTime.Now;
                            db.Entry(findObj).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            BranchesRights objRight = new BranchesRights();
                            objRight.OrganizationId = item.OrganizationId;
                            objRight.OrganizationId = item.OrganizationId;
                            objRight.EmployeeId = item.EmployeeId;
                            objRight.BranchId = item.BranchId;
                            objRight.IsSelected = item.IsSelected;
                            objRight.IsActive = true;
                            objRight.IsDeleted = false;
                            objRight.IsDeleted = false;
                            objRight.CreatedBy = userID;
                            objRight.CreatedDate = DateTime.Now;
                            db.BranchesRights.Add(objRight);
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
    }
}