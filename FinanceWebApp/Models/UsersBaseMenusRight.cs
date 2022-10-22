using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class UsersBaseMenusRight
    {
        [Key]
        public Guid Id { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> UserId { get; set; }
        public Nullable<long> MenuID { get; set; }
        public Nullable<bool> OPCreate { get; set; }
        public Nullable<bool> OPUpdate { get; set; }
        public Nullable<bool> OPView { get; set; }
        public Nullable<bool> OPDelete { get; set; }
        public Nullable<bool> OPPrint { get; set; }
        public Nullable<bool> OPApproval { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        public Nullable<long> CID { get; set; }
        [ForeignKey("OrganizationID")]
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }
        [ForeignKey("UserId")]
        public virtual Users Users { get; set; }
        [ForeignKey("MenuID")]
        public virtual Menu Menu { get; set; }
    }
}