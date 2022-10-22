using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.ModelsView
{
    public class MailSettingViewModel
    {
        public long Id { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public string MailCode { get; set; }
        public string FromEmail { get; set; }
        public string HostEmail { get; set; }
        public int PortNo { get; set; }
        public bool EnableSSL { get; set; }
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
        public Nullable<long> CID { get; set; }
    }
}