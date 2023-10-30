using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NET_MVC_Razor.Data;
using NET_MVC_Razor.Models.Domain;
using OfficeOpenXml;

namespace NET_MVC_Razor.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder.IsNullOrEmpty() ? "name_desc" : "";
            ViewData["SalarySortParm"] = sortOrder == "Salary" ? "salary_desc" : "Salary";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            var employees = from emp in _context.Employee
                            select emp;
            if (!string.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(x => x.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(x => x.Name);
                    break;
                case "salary_desc":
                    employees = employees.OrderByDescending(x => x.Salary);
                    break;
                case "Salary":
                    employees = employees.OrderBy(x => x.Salary);
                    break;
                default:
                    employees = employees.OrderBy(x => x.Name);
                    break;
            }

            int pageSize = 2;
            return View(await PaginatedList<Employee>.CreateAsync(employees, pageNumber ?? 1, pageSize));
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Salary,Email")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.Id = Guid.NewGuid();
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Salary,Email")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Employee == null)
            {
                return Problem("Entity set 'AppDbContext.Employee'  is null.");
            }
            var employee = await _context.Employee.FindAsync(id);
            if (employee != null)
            {
                _context.Employee.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(Guid id)
        {
            return (_context.Employee?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        public ActionResult UploadExcel(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ||
          !file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("file", "Invalid file type. Please upload an Excel file (.xlsx).");
                    return View("Create");
                }
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                        {
                            var employee = new Employee
                            {
                                Name = worksheet.Cells[row, 1].Value.ToString()!,
                                Salary = double.Parse(worksheet.Cells[row, 2].Value.ToString()!),
                                Email = worksheet.Cells[row, 3].Value.ToString()!
                            };
                            _context.Employee.Add(employee);
                            _context.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("file", "Please select a file to upload");
            return View("Create");
        }
    }
}
