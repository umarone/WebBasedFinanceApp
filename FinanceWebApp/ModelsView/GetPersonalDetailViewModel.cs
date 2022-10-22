using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.ModelsView
{
    public class GetPersonalDetailViewModel
    {
        public long EmployeeId { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public string EmployeeCode { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public Nullable<DateTime> DateofBirth { get; set; }
        public string CNICNo { get; set; }
        public string Gender { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string MaritalStatus { get; set; }
        public Nullable<long> CityID { get; set; }
        public string PermanentAddress { get; set; }
        public string TemporaryAddress { get; set; }
        public byte[] CNICFront { get; set; }
        public byte[] CNICBack { get; set; }
        public byte[] EmployeePicture { get; set; }
        public byte[] EmployeeProfilePic { get; set; }

        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsUser { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        public Nullable<long> CID { get; set; }
    }
}