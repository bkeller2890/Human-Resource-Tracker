#if DEBUG
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using HRTracker.Data;
using HRTracker.Models;

namespace HRTracker.Controllers
{
    [ApiController]
    [Route("TestData/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TestDataController : ControllerBase
    {
        private readonly HRDbContext _context;
        private readonly IHostEnvironment _env;

        public TestDataController(HRDbContext context, IHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        private bool IsDev() => _env.IsDevelopment();

        [HttpPost]
        public async Task<IActionResult> SeedLeaveRequest()
        {
            if (!IsDev()) return NotFound();

            // create a deterministic test record
            var lr = new LeaveRequest
            {
                EmployeeId = _context.Employees.OrderBy(e => e.EmployeeId).Select(e => e.EmployeeId).FirstOrDefault(),
                StartDate = DateTime.Parse("2025-12-31"),
                EndDate = DateTime.Parse("2026-01-02"),
                LeaveType = "TestSeed",
                Reason = "seeded via TestDataController",
                Status = LeaveStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.LeaveRequests.Add(lr);
            await _context.SaveChangesAsync();
            return Ok(new { id = lr.LeaveRequestId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveTestSeeds()
        {
            if (!IsDev()) return NotFound();

            var seeds = _context.LeaveRequests.Where(l => l.LeaveType == "TestSeed" || l.Reason == "seeded via TestDataController").ToList();
            if (seeds.Count == 0) return Ok(new { removed = 0 });
            _context.LeaveRequests.RemoveRange(seeds);
            await _context.SaveChangesAsync();
            return Ok(new { removed = seeds.Count });
        }

        [HttpGet]
        public IActionResult Info()
        {
            return Ok(new { env = _env.EnvironmentName, dev = IsDev(), routes = new[] { "SeedLeaveRequest (POST)", "RemoveTestSeeds (POST)" } });
        }
    }
}
#endif
