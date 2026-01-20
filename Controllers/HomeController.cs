using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HRTracker.Models;
using HRTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace HRTracker.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HRDbContext _db;

    public HomeController(ILogger<HomeController> logger, HRDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    // API endpoint: counts of leave requests grouped by status
    [HttpGet("/Home/Api/LeaveStatusCounts")]
    public async Task<IActionResult> LeaveStatusCounts()
    {
        var counts = await _db.LeaveRequests
            .GroupBy(l => l.Status)
            .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
            .ToListAsync();

        return Json(counts);
    }

    // API endpoint: counts of employees per department
    [HttpGet("/Home/Api/EmployeeCountsByDepartment")]
    public async Task<IActionResult> EmployeeCountsByDepartment()
    {
        var counts = await _db.Departments
            .Select(d => new { Department = d.Name, Count = (d.Employees == null ? 0 : d.Employees.Count) })
            .ToListAsync();

        return Json(counts);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
