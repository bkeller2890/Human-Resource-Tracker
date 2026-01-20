using System.ComponentModel.DataAnnotations;

namespace HRTracker.Models
{
    public class LeaveRequest
    {
    public int LeaveRequestId { get; set; }

    [Required]
    [Display(Name = "Employee ID")]
    public int EmployeeId { get; set; }

    public Employee? Employee { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "Leave Type")]
    public string LeaveType { get; set; } = string.Empty;
        // e.g. Vacation, Sick, Personal

        [StringLength(500)]
        public string? Reason { get; set; }

        [Required]
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
