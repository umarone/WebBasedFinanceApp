using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MudasirRehmanAlp.Models
{
    public class EmployeeEducationDetails
    {
        [Key]
        public long Id { get; set; }
        public Nullable<long> OrganizationID { get; set; }
        public Nullable<long> EmployeeId { get; set; }
        public Nullable<long> DegreeId { get; set; }
        public Nullable<long> InstituteId { get; set; }
        public string GPAMarks { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public Nullable<bool> IsCurrent { get; set; }
        public string Notes { get; set; }
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
        public virtual EmployeePersonalDetails EmployeePersonalDetails  { get; set; }
        [ForeignKey("DegreeId")]
        public virtual EducationDegree EducationDegree  { get; set; }
        [ForeignKey("InstituteId")]
        public virtual EducationInstitute EducationInstitute  { get; set; }
    }
}