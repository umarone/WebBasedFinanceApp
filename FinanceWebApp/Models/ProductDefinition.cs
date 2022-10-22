using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class ProductDefinition
    {
        [Key]
        public long ProductId { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> CategoryID { get; set; }
        public Nullable<long> ManufacturerID { get; set; }
        public Nullable<long> ColourId { get; set; }
        public Nullable<long> ModelId { get; set; }
        public Nullable<long> SizeId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductBarCode { get; set; }
        public string UniqueCode { get; set; }        
        public string Specification1 { get; set; }
        public string Specification2 { get; set; }
        public string Specification3 { get; set; }
        public string Specification4 { get; set; }
        public string Specification5 { get; set; }
        public string MainSpecification { get; set; }        
        public string Remarks { get; set; }             
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsBarCode { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        public Nullable<long> BranchId { get; set; }
        public byte[] ProductImage { get; set; }
        [ForeignKey("OrganizationID")]
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }
        [ForeignKey("CategoryID")]
        public virtual CategoryDefinition CategoryDefinition  { get; set; }
        [ForeignKey("ManufacturerID")]
        public virtual ManufactureDefinition ManufactureDefinition { get; set; }
        [ForeignKey("BranchId")]
        public virtual BranchDefinition BranchDefinition  { get; set; }

        [ForeignKey("ColourId")]
        public virtual Colour Colour  { get; set; }
        [ForeignKey("ModelId")]
        public virtual ProductModel ProductModel { get; set; }
        [ForeignKey("SizeId")]
        public virtual SizeDefinition SizeDefinition  { get; set; }


    }
}