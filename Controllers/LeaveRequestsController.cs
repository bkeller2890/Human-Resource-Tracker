using HRTracker.Data;
using HRTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRTracker.Controllers
{
    public class LeaveRequestsController : Controller
    {
        private readonly HRDbContext _context;

        public LeaveRequestsController(HRDbContext context)
        {
            _context = context;
        }

        // GET: LeaveRequests
        // Displays the list. .Include(l => l.Employee) is vital for showing the name.
        public async Task<IActionResult> Index()
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(l => l.Employee)
                .ToListAsync();
            return View(leaveRequests);
        }

        // GET: LeaveRequests/Create
        public IActionResult Create()
        {
            // Using ViewBag.Employees to match your Create.cshtml
            ViewBag.Employees = new SelectList(_context.Employees, "EmployeeId", "LastName");
            return View();
        }

        // POST: LeaveRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequest leaveRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leaveRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If we got here, something failed validation. 
            // We must reload the dropdown list before returning the view.
            ViewBag.Employees = new SelectList(_context.Employees, "EmployeeId", "LastName", leaveRequest.EmployeeId);
            return View(leaveRequest);
        }

        // GET: LeaveRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest == null) return NotFound();

            ViewBag.Employees = new SelectList(_context.Employees, "EmployeeId", "LastName", leaveRequest.EmployeeId);
            return View(leaveRequest);
        }

        // GET: LeaveRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var leaveRequest = await _context.LeaveRequests
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(m => m.LeaveRequestId == id);

            if (leaveRequest == null)
            {
                Response.StatusCode = 404;
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return View(leaveRequest);
        }

        // POST: LeaveRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveRequest leaveRequest)
        {
            if (id != leaveRequest.LeaveRequestId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaveRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.LeaveRequests.Any(e => e.LeaveRequestId == id))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Employees = new SelectList(_context.Employees, "EmployeeId", "LastName", leaveRequest.EmployeeId);
            return View(leaveRequest);
        }

        // GET: LeaveRequests/Delete/5
        // This shows the "Are you sure you want to delete this?" page.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var leaveRequest = await _context.LeaveRequests
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(m => m.LeaveRequestId == id);

            if (leaveRequest == null) return NotFound();

            return View(leaveRequest);
        }

        // POST: LeaveRequests/Delete/5
        // This actually removes the record from the database.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest != null)
            {
                _context.LeaveRequests.Remove(leaveRequest);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}