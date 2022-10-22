using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class Roles
    {
        [Key]
        public long RoleId { get; set; }      
        public string RoleName { get; set; }
        public Nullable<bool> isActive { get; set; }
    }
}