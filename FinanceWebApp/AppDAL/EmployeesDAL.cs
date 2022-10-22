using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.AppDAL
{
    public class EmployeesDAL
    {
        AppEntities db = new AppEntities();
        public string AddEmployee(string employeeDetails, HttpPostedFileBase profilePic, HttpPostedFileBase cnicFrontPic, HttpPostedFileBase cnicBackPic, long userID, out long Id)
        {

            long EmpID = 0;

            byte[] CnicFrontbytes;
            byte[] CnicBackbytes;
            string violationMessage = "";
            EmployeePersonalDetails obj = new EmployeePersonalDetails();
            Users objChild = new Users();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    EmployeePersonalDetails _Object = JsonConvert.DeserializeObject<EmployeePersonalDetails>(employeeDetails);
                    if (profilePic != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {

                            profilePic.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                            obj.EmployeePicture = array;
                            byte[] resizedImage;
                            using (Image orginalImage = Image.FromStream(ms))
                            {
                                ImageFormat orginalImageFormat = orginalImage.RawFormat;
                                int orginalImageWidth = orginalImage.Width;
                                int orginalImageHeight = orginalImage.Height;

                                int resizedImageWidth = 128;
                                int resizedImageHeight = 128;

                                using (Bitmap bitmapResized = new Bitmap(orginalImage, resizedImageWidth, resizedImageHeight))
                                {
                                    using (MemoryStream streamResized = new MemoryStream())
                                    {
                                        bitmapResized.Save(streamResized, orginalImageFormat);
                                        resizedImage = streamResized.ToArray();
                                    }
                                }
                            }
                            obj.EmployeeProfilePic = resizedImage;


                        }

                    }
                    if (cnicFrontPic != null)
                    {
                        using (BinaryReader br = new BinaryReader(cnicFrontPic.InputStream))
                        {
                            CnicFrontbytes = br.ReadBytes(cnicFrontPic.ContentLength);
                            obj.CNICFront = CnicFrontbytes;
                        }

                    }
                    if (cnicBackPic != null)
                    {
                        using (BinaryReader br = new BinaryReader(cnicBackPic.InputStream))
                        {
                            CnicBackbytes = br.ReadBytes(cnicBackPic.ContentLength);
                            obj.CNICBack = CnicBackbytes;
                        }
                    }

                    obj.OrganizationID = _Object.OrganizationID;
                    obj.BranchId = _Object.BranchId;
                    obj.EmployeeCode = _Object.EmployeeCode;
                    obj.Title = _Object.Title;
                    obj.FirstName = _Object.FirstName;
                    obj.LastName = _Object.LastName;
                    obj.FatherName = _Object.FatherName;
                    obj.DateofBirth = _Object.DateofBirth;
                    obj.CNICNo = _Object.CNICNo;
                    obj.Gender = _Object.Gender;
                    obj.MobileNo = _Object.MobileNo;
                    obj.PhoneNo = _Object.PhoneNo;
                    obj.Email = _Object.Email;
                    obj.MaritalStatus = _Object.MaritalStatus;
                    obj.CityID = _Object.CityID;
                    obj.PermanentAddress = _Object.PermanentAddress;
                    obj.TemporaryAddress = _Object.TemporaryAddress;
                    obj.IsActive = _Object.IsActive;
                    obj.IsUser = _Object.IsUser;
                  
                    obj.IsDeleted = false;
                    obj.CreatedBy = userID;
                    obj.CreatedDate = DateTime.Now;
                    db.EmployeePersonalDetails.Add(obj);
                    db.SaveChanges();
                    EmpID = obj.EmployeeId;
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
            Id = EmpID;
            return violationMessage;
        }
        public string UpdateEmployee(string employeeDetails, HttpPostedFileBase profilePic, HttpPostedFileBase cnicFrontPic, HttpPostedFileBase cnicBackPic, long userID, out long Id)
        {

            long EmpID = 0;
            byte[] CnicFrontbytes;
            byte[] CnicBackbytes;
            string violationMessage = "";
            EmployeePersonalDetails obj = new EmployeePersonalDetails();
            Users objChild = new Users();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    EmployeePersonalDetails _Object = JsonConvert.DeserializeObject<EmployeePersonalDetails>(employeeDetails);

                    obj = db.EmployeePersonalDetails.Find(_Object.EmployeeId);
                    if (profilePic != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {

                            profilePic.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                            obj.EmployeePicture = array;
                            byte[] resizedImage;
                            using (Image orginalImage = Image.FromStream(ms))
                            {
                                ImageFormat orginalImageFormat = orginalImage.RawFormat;
                                int orginalImageWidth = orginalImage.Width;
                                int orginalImageHeight = orginalImage.Height;

                                int resizedImageWidth = 128;
                                int resizedImageHeight = 128;

                                using (Bitmap bitmapResized = new Bitmap(orginalImage, resizedImageWidth, resizedImageHeight))
                                {
                                    using (MemoryStream streamResized = new MemoryStream())
                                    {
                                        bitmapResized.Save(streamResized, orginalImageFormat);
                                        resizedImage = streamResized.ToArray();
                                    }
                                }
                            }
                            obj.EmployeeProfilePic = resizedImage;


                        }

                    }
                    if (cnicFrontPic != null)
                    {
                        using (BinaryReader br = new BinaryReader(cnicFrontPic.InputStream))
                        {
                            CnicFrontbytes = br.ReadBytes(cnicFrontPic.ContentLength);
                            obj.CNICFront = CnicFrontbytes;
                        }

                    }
                    if (cnicBackPic != null)
                    {
                        using (BinaryReader br = new BinaryReader(cnicBackPic.InputStream))
                        {
                            CnicBackbytes = br.ReadBytes(cnicBackPic.ContentLength);
                            obj.CNICBack = CnicBackbytes;
                        }
                    }
                    obj.OrganizationID = _Object.OrganizationID;
                    obj.EmployeeCode = _Object.EmployeeCode;
                    obj.Title = _Object.Title;
                    obj.FirstName = _Object.FirstName;
                    obj.LastName = _Object.LastName;
                    obj.FatherName = _Object.FatherName;
                    obj.DateofBirth = _Object.DateofBirth;
                    obj.CNICNo = _Object.CNICNo;
                    obj.Gender = _Object.Gender;
                    obj.MobileNo = _Object.MobileNo;
                    obj.PhoneNo = _Object.PhoneNo;
                    obj.Email = _Object.Email;
                    obj.MaritalStatus = _Object.MaritalStatus;
                    obj.CityID = _Object.CityID;
                    obj.PermanentAddress = _Object.PermanentAddress;
                    obj.TemporaryAddress = _Object.TemporaryAddress;
                    obj.IsActive = _Object.IsActive;
                    obj.IsUser = _Object.IsUser;
                    obj.IsDeleted = false;
                    obj.UpdatedBy = userID;
                    obj.UpdatedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    EmpID = obj.EmployeeId;

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
            Id = EmpID;
            return violationMessage;
        }       
        public string AddorUpdateEmployeeOffical(string officalDetails, long userID, out long Id)
        {

            long EmpOfficalID = 0;

            string violationMessage = "";
            EmployeeOfficialDetails obj = new EmployeeOfficialDetails();
            Users objChild = new Users();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    EmployeeOfficialDetails _Object = JsonConvert.DeserializeObject<EmployeeOfficialDetails>(officalDetails);
                   var findobj = db.EmployeeOfficiallDetails.Where(a => a.Id == _Object.Id && a.EmployeeId == _Object.EmployeeId).FirstOrDefault();
                    if (findobj != null)
                    {
                        findobj.OrganizationID = _Object.OrganizationID;
                        findobj.EmployeeId = _Object.EmployeeId;
                        findobj.DepartmentId = _Object.DepartmentId;
                        findobj.DesignationId = _Object.DesignationId;
                        findobj.JoiningDate = _Object.JoiningDate;
                       
                        findobj.IsDeleted = false;
                        findobj.IsActive = true;
                        findobj.UpdatedBy = userID;
                        findobj.UpdatedDate = DateTime.Now;
                        db.Entry(findobj).State = EntityState.Modified;
                        db.SaveChanges();
                        EmpOfficalID = findobj.Id;
                    }                    
                    else
                    {
                        obj.OrganizationID = _Object.OrganizationID;
                        obj.EmployeeId = _Object.EmployeeId;
                        obj.DepartmentId = _Object.DepartmentId;
                        obj.DesignationId = _Object.DesignationId;
                        obj.JoiningDate = _Object.JoiningDate;
                        
                        obj.IsDeleted = false;
                        obj.IsActive = true;
                        obj.CreatedBy = userID;
                        obj.CreatedDate = DateTime.Now;
                        db.EmployeeOfficiallDetails.Add(obj);
                        db.SaveChanges();
                        EmpOfficalID = obj.Id;
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
            Id = EmpOfficalID;
            return violationMessage;
        }
        public string AddEmployeeEducation(string employeeEducation, long userID)
        {

            long EmpOfficalID = 0;

            string violationMessage = "";
            EmployeeEducationDetails obj = new EmployeeEducationDetails();
            Users objChild = new Users();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<EmployeeEducationDetails> _ObjectList = JsonConvert.DeserializeObject<List<EmployeeEducationDetails>>(employeeEducation);
                    foreach (var item in _ObjectList)
                    {
                        var findObj = db.EmployeeEducationDetails.Where(a => a.EmployeeId == item.EmployeeId && a.Id == item.Id).FirstOrDefault();
                        if (findObj != null)
                        {
                            findObj.OrganizationID = item.OrganizationID;
                            findObj.EmployeeId = item.EmployeeId;
                            findObj.InstituteId = item.InstituteId;
                            findObj.DegreeId = item.DegreeId;
                            findObj.GPAMarks = item.GPAMarks;
                            findObj.StartDate = item.StartDate;
                            findObj.EndDate = item.EndDate;
                            findObj.IsCurrent = item.IsCurrent;
                            findObj.Notes = item.Notes;
                          
                            findObj.IsDeleted = false;
                            findObj.IsActive = true;
                            findObj.UpdatedBy = userID;
                            findObj.UpdatedDate = DateTime.Now;
                            db.Entry(findObj).State = EntityState.Modified;
                            db.SaveChanges();
                            EmpOfficalID = obj.Id;
                        }
                        else
                        {
                            obj.OrganizationID = item.OrganizationID;
                            obj.EmployeeId = item.EmployeeId;
                            obj.InstituteId = item.InstituteId;
                            obj.DegreeId = item.DegreeId;
                            obj.GPAMarks = item.GPAMarks;
                            obj.StartDate = item.StartDate;
                            obj.EndDate = item.EndDate;
                            obj.IsCurrent = item.IsCurrent;
                            obj.Notes = item.Notes;
                            
                            obj.IsDeleted = false;
                            obj.IsActive = true;
                            obj.CreatedBy = userID;
                            obj.CreatedDate = DateTime.Now;
                            db.EmployeeEducationDetails.Add(obj);
                            db.SaveChanges();
                            EmpOfficalID = obj.Id;
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
        public string DeleteEducationDetail(string Id, long userID)
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

                    var findObj = db.EmployeeEducationDetails.Find(id);
                    findObj.IsDeleted = true;
                    findObj.IsActive = true;
                    findObj.DeletedBy = userID;
                    findObj.DeletedDate = DateTime.Now;
                    db.Entry(findObj).State = EntityState.Modified;
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
        public string AddEmployeeExperience(string employeeExperience, long userID)
        {

            long EmpExpID = 0;

            string violationMessage = "";
            EmployeeExperienceDetails obj = new EmployeeExperienceDetails();
            Users objChild = new Users();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<EmployeeExperienceDetails> _ObjectList = JsonConvert.DeserializeObject<List<EmployeeExperienceDetails>>(employeeExperience);
                    foreach (var item in _ObjectList)
                    {
                        var findObj = db.EmployeeExperienceDetails.Where(a => a.EmployeeId == item.EmployeeId && a.Id == item.Id).FirstOrDefault();
                        if (findObj != null)
                        {
                            findObj.OrganizationID = item.OrganizationID;
                            findObj.EmployeeId = item.EmployeeId;
                            findObj.CityId = item.CityId;
                            findObj.JobTitle = item.JobTitle;
                            findObj.CompanyName = item.CompanyName;
                            findObj.StartDate = item.StartDate;
                            findObj.EndDate = item.EndDate;
                            findObj.IsCurrent = item.IsCurrent;
                            findObj.Notes = item.Notes;
                            
                            findObj.IsDeleted = false;
                            findObj.IsActive = true;
                            findObj.UpdatedBy = userID;
                            findObj.UpdatedDate = DateTime.Now;
                            db.Entry(findObj).State = EntityState.Modified;
                            db.SaveChanges();
                            EmpExpID = obj.Id;
                        }
                        else
                        {
                            obj.OrganizationID = item.OrganizationID;
                            obj.EmployeeId = item.EmployeeId;
                            obj.CityId = item.CityId;
                            obj.JobTitle = item.JobTitle;
                            obj.CompanyName = item.CompanyName;
                            obj.StartDate = item.StartDate;
                            obj.EndDate = item.EndDate;
                            obj.IsCurrent = item.IsCurrent;
                            obj.Notes = item.Notes;
                           
                            obj.IsDeleted = false;
                            obj.IsActive = true;
                            obj.CreatedBy = userID;
                            obj.CreatedDate = DateTime.Now;
                            db.EmployeeExperienceDetails.Add(obj);
                            db.SaveChanges();
                            EmpExpID = obj.Id;
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
        public string DeleteExperienceDetail(string Id, long userID)
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

                    var findObj = db.EmployeeExperienceDetails.Find(id);
                    findObj.IsDeleted = true;
                    findObj.IsActive = true;
                    findObj.DeletedBy = userID;
                    findObj.DeletedDate = DateTime.Now;
                    db.Entry(findObj).State = EntityState.Modified;
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
        public string DeleteEmployee(EmployeePersonalDetails employeePersonalDetails, long userID)
        {

            long EmpID = 0;

            string violationMessage = "";
            EmployeePersonalDetails obj = new EmployeePersonalDetails();
            Users objChild = new Users();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {



                    obj = db.EmployeePersonalDetails.Find(employeePersonalDetails.EmployeeId);
                    obj.IsDeleted = true;
                    obj.DeletedBy = userID;
                    obj.DeletedDate = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    EmpID = obj.EmployeeId;
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