using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class UsersRole
    {
        [Key]
        public long UserRoleId { get; set; }
        public Nullable<long> UserID { get; set; }
        public Nullable<long> RoleID { get; set; }

        [ForeignKey("UserID")]
        public virtual Users Users  { get; set; }
        [ForeignKey("RoleID")]
        public virtual Roles Roles  { get; set; }
    }
}