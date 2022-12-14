using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class ProductModel
    {
        [Key]
        public long Id { get; set; }
        public Nullable<long> OrganizationId { get; set; }
        public Nullable<long> CategoryId { get; set; }
        public Nullable<long> ManufactureId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<long> BranchId { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> DeletedBy { get; set; }

        public Nullable<DateTime> DeletedDate { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }
        [ForeignKey("BranchId")]
        public virtual BranchDefinition BranchDefinition { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CategoryDefinition CategoryDefinition  { get; set; }
        [ForeignKey("ManufactureId")]
        public virtual ManufactureDefinition ManufactureDefinition  { get; set; }
    }
}