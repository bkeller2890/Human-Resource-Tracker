using HRTracker.Models;

namespace HRTracker.Data
{
    public static class DbSeeder
    {
        public static void Seed(HRDbContext context)
        {
            context.Database.EnsureCreated();

            // Ensure departments exist
            if (!context.Departments.Any())
            {
                context.Departments.AddRange(new List<HRTracker.Models.Department>
                {
                    new HRTracker.Models.Department { DepartmentId = 1, Name = "Engineering" },
                    new HRTracker.Models.Department { DepartmentId = 2, Name = "Management" },
                    new HRTracker.Models.Department { DepartmentId = 3, Name = "Human Resources" }
                });
                context.SaveChanges();
            }

            if (context.Employees.Any()) return;

            var sample = new List<Employee>
            {
                new Employee { FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@example.com", Position = "Engineer", DepartmentId = 1 },
                new Employee { FirstName = "Bob", LastName = "Smith", Email = "bob.smith@example.com", Position = "Manager", DepartmentId = 2 },
                new Employee { FirstName = "Carol", LastName = "Lee", Email = "carol.lee@example.com", Position = "HR", DepartmentId = 3 },
                new Employee { FirstName = "David", LastName = "Ng", Email = "david.ng@example.com", Position = "Engineer", DepartmentId = 1 },
                new Employee { FirstName = "Eva", LastName = "Martinez", Email = "eva.martinez@example.com", Position = "Engineer", DepartmentId = 1 },
                new Employee { FirstName = "Frank", LastName = "O'Neill", Email = "frank.oneill@example.com", Position = "Manager", DepartmentId = 2 },
                new Employee { FirstName = "Grace", LastName = "Kim", Email = "grace.kim@example.com", Position = "HR", DepartmentId = 3 }
            };

            context.Employees.AddRange(sample);
            context.SaveChanges();

            // Seed some sample leave requests if none exist
            if (!context.LeaveRequests.Any())
            {
                var leaves = new List<LeaveRequest>
                {
                    new LeaveRequest { EmployeeId = 1, StartDate = DateTime.Today.AddDays(7), EndDate = DateTime.Today.AddDays(10), LeaveType = "Vacation", Status = LeaveStatus.Pending },
                    new LeaveRequest { EmployeeId = 2, StartDate = DateTime.Today.AddDays(3), EndDate = DateTime.Today.AddDays(4), LeaveType = "Sick", Status = LeaveStatus.Approved },
                    new LeaveRequest { EmployeeId = 3, StartDate = DateTime.Today.AddDays(30), EndDate = DateTime.Today.AddDays(33), LeaveType = "Vacation", Status = LeaveStatus.Pending },
                    new LeaveRequest { EmployeeId = 4, StartDate = DateTime.Today.AddDays(-10), EndDate = DateTime.Today.AddDays(-8), LeaveType = "Sick", Status = LeaveStatus.Approved },
                    new LeaveRequest { EmployeeId = 5, StartDate = DateTime.Today.AddDays(14), EndDate = DateTime.Today.AddDays(16), LeaveType = "Vacation", Status = LeaveStatus.Pending },
                    new LeaveRequest { EmployeeId = 6, StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(1), LeaveType = "Personal", Status = LeaveStatus.Rejected },
                    new LeaveRequest { EmployeeId = 7, StartDate = DateTime.Today.AddDays(21), EndDate = DateTime.Today.AddDays(23), LeaveType = "Vacation", Status = LeaveStatus.Pending }
                };

                context.LeaveRequests.AddRange(leaves);
                context.SaveChanges();
            }
        }
    }
}
