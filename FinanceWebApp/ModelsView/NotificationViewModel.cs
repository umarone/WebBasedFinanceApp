using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.ModelsView
{
    public class NotificationViewModel
    {
        public long Id { get; set; }
        public Nullable<long> OrganizationId { get; set; }
        public Nullable<long> BranchId { get; set; }
        public Nullable<NotificationType> Type { get; set; }
        public Nullable<NotificationPriority> Priority { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsRead { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
    }
}