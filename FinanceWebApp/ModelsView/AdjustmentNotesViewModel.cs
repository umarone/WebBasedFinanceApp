using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.ModelsView
{
    public class AdjustmentNotesViewModel
    {
        public long AdjustmentNoteId { get; set; }
        public string AdjustmentNoteNO { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public Nullable<DateTime> Date { get; set; }
    }
}