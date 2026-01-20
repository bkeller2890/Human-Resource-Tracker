using HRTracker.Data;
using HRTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRTracker.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly HRDbContext _context;

        public EmployeesController(HRDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(int? departmentId, string? searchString, int page = 1, int pageSize = 10)
        {
            var query = _context.Employees
                .Include(e => e.Department)
                .AsQueryable();

            if (departmentId.HasValue)
            {
                query = query.Where(e => e.DepartmentId == departmentId.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var s = searchString.Trim();
                query = query.Where(e => e.FirstName.Contains(s) || e.LastName.Contains(s) || e.Email.Contains(s));
            }

            var total = await query.CountAsync();
            var items = await query
                .OrderBy(e => e.EmployeeId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // prepare department filter select list
            var departments = await _context.Departments
                .OrderBy(d => d.Name)
                .Select(d => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = d.DepartmentId.ToString(), Text = d.Name })
                .ToListAsync();

            ViewData["DepartmentFilter"] = departments;
            ViewData["SelectedDepartment"] = departmentId;
            ViewData["SearchString"] = searchString ?? string.Empty;

            var result = new HRTracker.Models.PagedResult<Employee>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = total
            };

            return View(result);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name");
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name", employee.DepartmentId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name", employee.DepartmentId);
            return View(employee);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("~/Views/Shared/NotFound.cshtml");
            }

            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.EmployeeId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Employees.Any(e => e.EmployeeId == id))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name", employee.DepartmentId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee!);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
