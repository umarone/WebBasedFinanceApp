using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.Models
{
    public class CustomerStatement
    {
        [Key]

        public long Id { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> BranchId { get; set; }
        public Nullable<long> AccountID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string CAFNo { get; set; }
        public string CNICNo { get; set; }
        public string Age { get; set; }
        
        public string Address { get; set; }
        public Nullable<OwnershipType> CurrentOwnership { get; set; }
        public string CurrentNoOfYear { get; set; }
        public string PermanentAddress { get; set; }
        public Nullable<OwnershipType> PermanentOwnership { get; set; }
        public string PermanentNoOfYear { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
       
        public string Email { get; set; }
        public string GuardianName { get; set; }

        public string NTNNo { get; set; }


        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string BusinessAddress { get; set; }
        public Nullable<DateTime> AppointmentDate { get; set; }
        public Nullable<DateTime> RetirementDate { get; set; }
        public Nullable<decimal> GrossSalary { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountNo { get; set; }

        public Nullable<long> CityId { get; set; }
        public string Remarks { get; set; }
        public Nullable<long> AreaId { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
      
        public Nullable<long> DeletedBy { get; set; }
        public byte[] Image { get; set; }
       
       
        public string FilePath { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        [ForeignKey("OrganizationID")]
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }
        [ForeignKey("AccountID")]
        public virtual Account Account { get; set; }
        [ForeignKey("CityId")]
        public virtual CityDefinition CityDefinition { get; set; }
        [ForeignKey("AreaId")]
        public virtual AreaDefinition AreaDefinition { get; set; }
        [ForeignKey("BranchId")]
        public virtual BranchDefinition BranchDefinition { get; set; }
    }
}