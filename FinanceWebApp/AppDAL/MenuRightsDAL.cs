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
    public class MenuRightsDAL
    {
        AppEntities db = new AppEntities();
        public string AddorUpdateMenuRights(string JsonMenusObj, long userID)
        {

            string violationMessage = "";



            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {



                    List<UsersBaseMenusRight> Parentitem = JsonConvert.DeserializeObject<List<UsersBaseMenusRight>>(JsonMenusObj);

                    foreach (var item in Parentitem)
                    {

                        if (item.OPCreate == true || item.OPUpdate == true || item.OPView == true || item.OPPrint == true || item.OPApproval == true)
                        {
                            var findObj = db.UsersBaseMenusRights.Where(a => a.MenuID == item.MenuID && a.UserId == item.UserId && a.OrganizationID == item.OrganizationID).FirstOrDefault();
                            if (findObj != null)
                            {
                                findObj.OrganizationID = item.OrganizationID;
                                findObj.CID = item.OrganizationID;
                                findObj.UserId = item.UserId;
                                findObj.MenuID = item.MenuID;
                                findObj.OPCreate = item.OPCreate;
                                findObj.OPUpdate = item.OPUpdate;
                                findObj.OPView = item.OPView;
                                findObj.OPDelete = item.OPDelete;
                                findObj.OPPrint = item.OPPrint;
                                findObj.OPApproval = item.OPApproval;
                                findObj.IsDeleted = false;
                                findObj.UpdatedBy = userID;
                                findObj.UpdatedDate = DateTime.Now;
                                db.Entry(findObj).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                UsersBaseMenusRight obj = new UsersBaseMenusRight();
                                obj.Id = Guid.NewGuid();
                                obj.OrganizationID = item.OrganizationID;
                                obj.CID = item.OrganizationID;
                                obj.UserId = item.UserId;
                                obj.MenuID = item.MenuID;
                                obj.OPCreate = item.OPCreate;
                                obj.OPUpdate = item.OPUpdate;
                                obj.OPView = item.OPView;
                                obj.OPDelete = item.OPDelete;
                                obj.OPPrint = item.OPPrint;
                                obj.OPApproval = item.OPApproval;
                                obj.IsDeleted = false;
                                obj.CreatedBy = userID;
                                obj.CreatedDate = DateTime.Now;
                                db.UsersBaseMenusRights.Add(obj);
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            var findObjForRemove = db.UsersBaseMenusRights.Where(a => a.MenuID == item.MenuID && a.UserId == item.UserId && a.OrganizationID == item.OrganizationID).FirstOrDefault();

                            if (findObjForRemove != null)
                            {
                                db.UsersBaseMenusRights.Remove(findObjForRemove);
                                db.SaveChanges();
                            }
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
        public string AddorRemoveMenuRightsForSuperAdmin(long userID)
        {

            string violationMessage = "";



            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<UsersBaseMenusRight> usersBaseMenusRightsList = new List<UsersBaseMenusRight>();
                    var findResultList = db.UsersBaseMenusRights.Where(a => a.UserId == userID).ToList();
                    db.UsersBaseMenusRights.RemoveRange(findResultList);
                    db.SaveChanges();

                    var findMenuObjList = db.Menu.Where(a => a.IsMenu == true).ToList();
                    foreach (var item in findMenuObjList)
                    {
                        UsersBaseMenusRight obj = new UsersBaseMenusRight();
                        obj.Id = Guid.NewGuid();
                        obj.UserId = userID;
                        obj.MenuID = item.MenuID;
                        obj.OPCreate = true;
                        obj.OPUpdate = true;
                        obj.OPView = true;
                        obj.OPDelete = true;
                        obj.OPPrint = true;
                        obj.OPApproval = true;
                        obj.IsDeleted = false;
                        obj.CreatedBy = userID;
                        obj.CreatedDate = DateTime.Now;
                        usersBaseMenusRightsList.Add(obj);
                    }
                    db.UsersBaseMenusRights.AddRange(usersBaseMenusRightsList);
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