using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class AreaDefinition
    {
        [Key]

        
        public long Id { get; set; }
        public Nullable<long> CityId { get; set; }
        public string AreaName { get; set; }
        public string AreaCode { get; set; }
       
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        [ForeignKey("CityId")]
        public virtual CityDefinition CityDefinition  { get; set; }
    }
}
