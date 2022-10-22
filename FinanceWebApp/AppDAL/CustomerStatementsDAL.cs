using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Infrastructure.AppServices;
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
    public class CustomerStatementsDAL
    {
        AppEntities db = new AppEntities();
        FilesUploadService filesUploadService = new FilesUploadService();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        public string Add(string objMasterItem, string objChilds, HttpPostedFileBase cusfile, long userID)
        {
            long Id = 0;
            long OrgId = 0;
            long BrchId = 0;
            byte[] Imagebytes;
            string violationMessage = "";
            string fileName = "CS_";
            string folderName = "CustomerStatements";
            CustomerStatement obj = new CustomerStatement();
            GuarantorDefinition objChild = new GuarantorDefinition();
            //using (DbContextTransaction transaction = db.Database.BeginTransaction())
            //{
            try
            {



                CustomerStatement _objectParent = JsonConvert.DeserializeObject<CustomerStatement>(objMasterItem);
                List<GuarantorDefinition> Childitems = JsonConvert.DeserializeObject<List<GuarantorDefinition>>(objChilds);

                if (cusfile != null)
                {
                    using (BinaryReader br = new BinaryReader(cusfile.InputStream))
                    {
                        Imagebytes = br.ReadBytes(cusfile.ContentLength);
                        _objectParent.Image = Imagebytes;
                    }
                    Stream stream = new MemoryStream(Imagebytes);
                    string extension = Path.GetExtension(cusfile.FileName);
                    var filePath = filesUploadService.UploadImages(fileName + _objectParent.Code + "_", stream, folderName, extension);
                    _objectParent.FilePath = filePath.ToString();
                }

                _objectParent.Code = dALCode.AutoGenerateCustomerStatementCode(Convert.ToInt64(_objectParent.OrganizationID),Convert.ToInt64( _objectParent.BranchId));
                _objectParent.CAFNo = dALCode.AutoGenerateCustomerStatementCAFNo(Convert.ToInt64(_objectParent.OrganizationID), Convert.ToInt64(_objectParent.BranchId));
                _objectParent.IsDeleted = false;
                _objectParent.CreatedBy = userID;
                _objectParent.CreatedDate = DateTime.Now;
                db.CustomerStatements.Add(_objectParent);
                db.SaveChanges();
                Id = _objectParent.Id;
                OrgId = Convert.ToInt64(_objectParent.OrganizationID);
                BrchId = Convert.ToInt64(_objectParent.BranchId);
                foreach (var item in Childitems)
                {
                    objChild.OrganizationID = OrgId;
                    objChild.BranchId = BrchId;
                    objChild.CustomerStatementId = Id;
                    objChild.Code = dALCode.AutoGenerateGuarantorDefinitionsCode(OrgId, BrchId);

                    objChild.FirstName = item.FirstName;
                    objChild.GuardianName = item.GuardianName;
                    objChild.CNICNo = item.CNICNo;
                    objChild.MobileNo = item.MobileNo;
                    objChild.PermanentAddress = item.PermanentAddress;
                    objChild.PermanentOwnership = item.PermanentOwnership;
                    objChild.PermanentNoOfYear = item.PermanentNoOfYear;

                    objChild.OfficeName = item.OfficeName;
                    objChild.DesignationName = item.DesignationName;
                    objChild.GrossSalary = item.GrossSalary;
                    objChild.OfficeAddress = item.OfficeAddress;

                    objChild.OfficePhoneNo = item.OfficePhoneNo;
                    objChild.Email = item.Email;

                    objChild.IsActive = true;
                    objChild.IsDeleted = false;
                    objChild.CreatedBy = userID;
                    objChild.CreatedDate = DateTime.Now;
                    db.GuarantorDefinitions.Add(objChild);
                    db.SaveChanges();
                }


                //transaction.Commit();
            }
            catch (DbUpdateException ex)
            {
                //transaction.Rollback();
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
            //}

            return violationMessage;
        }
        public string Update(string objMasterItem, string objChilds, HttpPostedFileBase cusfile, long userID)
        {
            long Id = 0;
            long OrgId = 0;
            long BrchId = 0;
            byte[] Imagebytes;
            string violationMessage = "";
            string fileName = "CS_";
            string folderName = "CustomerStatements";
            CustomerStatement obj = new CustomerStatement();
            GuarantorDefinition objChild = new GuarantorDefinition();
            //using (DbContextTransaction transaction = db.Database.BeginTransaction())
            //{
            try
            {



                CustomerStatement _objectParent = JsonConvert.DeserializeObject<CustomerStatement>(objMasterItem);
                List<GuarantorDefinition> Childitems = JsonConvert.DeserializeObject<List<GuarantorDefinition>>(objChilds);
                obj = db.CustomerStatements.Where(a => a.Id == _objectParent.Id).FirstOrDefault();

                if (cusfile != null)
                {
                    using (BinaryReader br = new BinaryReader(cusfile.InputStream))
                    {
                        Imagebytes = br.ReadBytes(cusfile.ContentLength);
                        obj.Image = Imagebytes;
                    }
                    Stream stream = new MemoryStream(Imagebytes);
                    string extension = Path.GetExtension(cusfile.FileName);
                    var filePath = filesUploadService.UploadImages(fileName + _objectParent.Code + "_", stream, folderName, extension);
                    obj.FilePath = filePath.ToString();
                }
                obj.Code = _objectParent.Code;
                obj.CAFNo = _objectParent.CAFNo;
                obj.OrganizationID = _objectParent.OrganizationID;
                obj.BranchId = _objectParent.BranchId;
                obj.Name = _objectParent.Name;
                obj.GuardianName = _objectParent.GuardianName;
                obj.CNICNo = _objectParent.CNICNo;
                obj.MobileNo = _objectParent.MobileNo;
                obj.Address = _objectParent.Address;
                obj.CurrentOwnership = _objectParent.CurrentOwnership;
                obj.CurrentNoOfYear = _objectParent.CurrentNoOfYear;
                obj.PermanentAddress = _objectParent.PermanentAddress;
                obj.PermanentOwnership = _objectParent.PermanentOwnership;

                obj.PermanentNoOfYear = _objectParent.PermanentNoOfYear;
                obj.DepartmentName = _objectParent.DepartmentName;
                obj.DesignationName = _objectParent.DesignationName;
                obj.BusinessAddress = _objectParent.BusinessAddress;
                obj.AppointmentDate = _objectParent.AppointmentDate;
                obj.RetirementDate = _objectParent.RetirementDate;

                obj.GrossSalary = _objectParent.GrossSalary;
                obj.Age = _objectParent.Age;
                obj.BankName = _objectParent.BankName;
                obj.BranchName = _objectParent.BranchName;
                obj.AccountNo = _objectParent.AccountNo;

                obj.IsActive = _objectParent.IsActive;
                obj.IsDeleted = false;
                obj.UpdatedBy = userID;
                obj.UpdatedDate = DateTime.Now;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                Id = _objectParent.Id;
                OrgId = Convert.ToInt64(_objectParent.OrganizationID);
                BrchId = Convert.ToInt64(_objectParent.BranchId);
                foreach (var item in Childitems)
                {
                    objChild = db.GuarantorDefinitions.Find(item.Id);
                    objChild.OrganizationID = OrgId;
                    objChild.BranchId = BrchId;
                    objChild.CustomerStatementId = Id;

                    objChild.FirstName = item.FirstName;
                    objChild.GuardianName = item.GuardianName;
                    objChild.CNICNo = item.CNICNo;
                    objChild.MobileNo = item.MobileNo;

                    objChild.PermanentAddress = item.PermanentAddress;
                    objChild.PermanentOwnership = item.PermanentOwnership;
                    objChild.PermanentNoOfYear = item.PermanentNoOfYear;
                    objChild.OfficeName = item.OfficeName;
                    objChild.DesignationName = item.DesignationName;
                    objChild.GrossSalary = item.GrossSalary;
                    objChild.OfficeAddress = item.OfficeAddress;

                    objChild.IsActive = true;
                    objChild.IsDeleted = false;
                    objChild.UpdatedBy = userID;
                    objChild.UpdatedDate = DateTime.Now;

                    db.Entry(objChild).State = EntityState.Modified;
                    db.SaveChanges();
                }


                //transaction.Commit();
            }
            catch (DbUpdateException ex)
            {
                //transaction.Rollback();
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
            //}

            return violationMessage;
        }

       

    }
}