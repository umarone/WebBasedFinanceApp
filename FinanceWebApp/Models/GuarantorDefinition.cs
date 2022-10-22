using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.Models
{
    public class GuarantorDefinition
    {
        [Key]

        public long Id { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> BranchId { get; set; }
        public Nullable<long> CustomerStatementId { get; set; }
        public string Code { get; set; }
        
        public string FirstName { get; set; }
        public string GuardianName { get; set; }
        public string CNICNo { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string PermanentAddress { get; set; }
        public Nullable<OwnershipType> PermanentOwnership { get; set; }
        public string PermanentNoOfYear { get; set; }
        
        public string OfficeName { get; set; }
        public string DesignationName { get; set; }
        public Nullable<decimal> GrossSalary { get; set; }
        public string OfficeAddress { get; set; }

        public string OfficePhoneNo { get; set; }

        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
      
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
     
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        [ForeignKey("OrganizationID")]
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }
      
        [ForeignKey("CustomerStatementId")]
        public virtual CustomerStatement CustomerStatement { get; set; }
        [ForeignKey("BranchId")]
        public virtual BranchDefinition BranchDefinition { get; set; }

    }
}