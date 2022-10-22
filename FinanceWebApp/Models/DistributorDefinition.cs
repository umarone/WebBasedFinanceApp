using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class DistributorDefinition
    {
        [Key]

        public long Id { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> AccountID { get; set; }
        public string DistributorName { get; set; }
        public string DistributorCode { get; set; }
        public string DistributorAddrss { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonMobileNo { get; set; }
        public Nullable<long> CityId { get; set; }
        public string DistributorRemarks { get; set; }
        public Nullable<long> AreaId { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> BranchId { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public byte[] DistributorImage { get; set; }
        public string NTN { get; set; }
        public string GSTNo { get; set; }
        public string Website { get; set; }

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
        public virtual BranchDefinition BranchDefinition  { get; set; }
    }
}