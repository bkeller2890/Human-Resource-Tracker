using System.ComponentModel.DataAnnotations;

namespace HRTracker.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(100)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Position { get; set; }

        [Required]
    [Display(Name = "Department ID")]
    public int DepartmentId { get; set; }

        // FIX: Added '?' to make it nullable for validation
        public Department? Department { get; set; }

        // FIX: Added '?' here as well
        public ICollection<LeaveRequest>? LeaveRequests { get; set; } = new List<LeaveRequest>();
    }
}