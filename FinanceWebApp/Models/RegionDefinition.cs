using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class RegionDefinition
    {
        [Key]

        public long Id { get; set; }
        public Nullable<long> CountryId { get; set; }
        public Nullable<long> ProvinceId { get; set; }
        public string RegionName { get; set; }
        public string RegionCode { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        [ForeignKey("ProvinceId")]
        public virtual ProvinceDefinition ProvinceDefinition { get; set; }
        [ForeignKey("CountryId")]
        public virtual CountryDefinition CountryDefinition { get; set; }

    }
}