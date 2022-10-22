using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class VouchersDetail
    {

        [Key]
        public long VoucherDetailID { get; set; }
        public Nullable<long> VoucherId { get; set; }

        public Nullable<long> AccountId { get; set; }
       
        public string Narration { get; set; }
        public Nullable<decimal> ClosingBalance { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }

        [ForeignKey("VoucherId")]
        public virtual VouchersHead VouchersHead { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }
}