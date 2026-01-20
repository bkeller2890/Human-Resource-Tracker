using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRTracker.Models
{
    public class Department
    {
    [Display(Name = "Department ID")]
    public int DepartmentId { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Department Name")]
    public string Name { get; set; } = string.Empty;

        // Navigation - Added '?' to make it nullable
        public ICollection<Employee>? Employees { get; set; }
            = new List<Employee>();
    }
}