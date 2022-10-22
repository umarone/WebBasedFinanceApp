using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class EmployeeOfficialDetails
    {
        [Key]

        public long Id { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> EmployeeId { get; set; }
        public Nullable<long> DepartmentId { get; set; }
        public Nullable<long> DesignationId { get; set; }
        public Nullable<DateTime> JoiningDate { get; set; }
        public Nullable<bool> ProbationStatus { get; set; }
        public Nullable<DateTime> ProbationStartDate { get; set; }
        public Nullable<DateTime> ProbationEndDate { get; set; }
        public string ProbationaryPeriod { get; set; }
        public Nullable<bool> ResignitionStatus { get; set; }
        public Nullable<DateTime> ResignitionDate { get; set; }
        public string ResignitionReason { get; set; }
        public Nullable<DateTime> WorkingEndDate { get; set; }
        public Nullable<bool> SuspensionStatus { get; set; }
        public Nullable<DateTime> SuspensionFrom { get; set; }
        public Nullable<DateTime> SuspensionTo { get; set; }
        public string SuspensionReason { get; set; }
        public Nullable<bool> TerminationStatus { get; set; }
        public string TerminationType { get; set; }
        public string TerminationReason { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
      
        [ForeignKey("OrganizationID")]
        public virtual OrganizationDefinition OrganizationDefinition { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual EmployeePersonalDetails EmployeePersonalDetails { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual DepartmentDefinition DepartmentDefinition { get; set; }
        [ForeignKey("DesignationId")]
        public virtual DesignationDefinition DesignationDefinition { get; set; }
    }
}