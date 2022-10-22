using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.ModelsView
{
    public class UsersAccountViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RemorememberMe { get; set; }
        public Nullable<long> BranchId { get; set; }
    }
}