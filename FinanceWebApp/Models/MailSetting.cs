using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class MailSetting
    {
        [Key]

        public long Id { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public string MailCode { get; set; }
        public string FromEmail { get; set; }
        public string HostEmail { get; set; }
        public Nullable<int> PortNo { get; set; }
        public Nullable<bool> EnableSSL { get; set; }
        public string UserNameEmail { get; set; }
        public string Password { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public Nullable<long> DefaultBy { get; set; }
        public Nullable<DateTime> DefaultDate { get; set; }       
        public Nullable<long> BranchId { get; set; }
        [ForeignKey("OrganizationID")]
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }
        [ForeignKey("BranchId")]
        public virtual BranchDefinition BranchDefinition { get; set; }
    }
}