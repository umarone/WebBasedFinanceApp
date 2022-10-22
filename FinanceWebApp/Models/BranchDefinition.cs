using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class BranchDefinition
    {    [Key]
        public long Id { get; set; }
        public Nullable<long> OrganizationId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string ContactNo { get; set; }
        public string MobileNo { get; set; }
        
        public string Email { get; set; }
        public string Address { get; set; }
       
        public Nullable<long> CountryId { get; set; }
        public Nullable<long> ProvinceId { get; set; }
        public Nullable<long> RegionId { get; set; }
        public Nullable<long> CityId { get; set; }
        public Nullable<long> AreaId { get; set; }
      
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual OrganizationDefinition OrganizationDefinition  { get; set; }

        [ForeignKey("CountryId")]
        public virtual CountryDefinition CountryDefinition { get; set; }

        [ForeignKey("ProvinceId")]
        public virtual ProvinceDefinition ProvinceDefinition { get; set; }
        [ForeignKey("RegionId")]
        public virtual RegionDefinition RegionDefinition { get; set; }
        [ForeignKey("CityId")]
        public virtual CityDefinition CityDefinition { get; set; }
        [ForeignKey("AreaId")]
        public virtual AreaDefinition AreaDefinition { get; set; }
    }
}