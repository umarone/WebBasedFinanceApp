using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class POTermsAndCondition
    {
        [Key]

        public long POTermsID { get; set; }
        public long PurchaseOrderID { get; set; }
        public string POTermDescription { get; set; }
        public Nullable<long> OragnizationID { get; set; }
        
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}