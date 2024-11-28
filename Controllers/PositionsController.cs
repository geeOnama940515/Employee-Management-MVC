using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeCRUD.Data;
using EmployeeCRUD.Models;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeCRUD.Controllers
{
    public class PositionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PositionsController(ApplicationDbContext context)
        {
            _context = context;
        }
        private void HeaderCrumbs() 
        {
            // Set dynamic data
            ViewBag.PageTitle = "Positions";
            ViewBag.Breadcrumbs = new List<Breadcrumb>
        {
            new Breadcrumb { Name = "Home", Link = Url.Action("Index", "Home") },
            new Breadcrumb { Name = "Positions List", Link = null } // Current page
        };
        }
        // GET: Positions
        public async Task<IActionResult> Index()
        {
            HeaderCrumbs();

            var applicationDbContext = _context.Positions.Include(p => p.Department);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Positions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var positionsModel = await _context.Positions
                .Include(p => p.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (positionsModel == null)
            {
                return NotFound();
            }

            return View(positionsModel);
        }

        // GET: Positions/Create
        public IActionResult Create()
        {

            // Create a list of anonymous objects with DepartmentId and formatted display string
            var departments = _context.Departments
                .Select(d => new
                {
                    d.Id,
                    DepartmentNameAndHead = $"{d.DepartmentName} - {d.DepartmentHead}"
                })
                .ToList();  // Convert to list to avoid IQueryable<string>

            // Pass the list to SelectList, using Id as the value and DepartmentNameAndHead as the display text
            ViewData["DepartmentId"] = new SelectList(departments, "Id", "DepartmentNameAndHead");

            HeaderCrumbs();

            return View();
        }

        [HttpGet, ActionName("GetPositionsById")]
        public JsonResult GetPositionsById(int DepartmentId)
        {
            List<PositionsModel> positionsList = new List<PositionsModel>();
            positionsList = _context.Positions.Where(s => s.DepartmentId.Equals(DepartmentId)).ToList();
            return Json(positionsList);
        }

        // POST: Positions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PositionsModel positionsModel)
        {
            HeaderCrumbs();
            if (ModelState.IsValid)
            {
                positionsModel.CreatedBy = "Myoi Mina";
                positionsModel.DateCreated = DateTime.Now;
                _context.Add(positionsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", positionsModel.DepartmentId);
            return View(positionsModel);
        }

        // GET: Positions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var positionsModel = await _context.Positions.FindAsync(id);
            if (positionsModel == null)
            {
                return NotFound();
            }
            HeaderCrumbs();
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", positionsModel.DepartmentId);
            return View(positionsModel);
        }

        // POST: Positions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PositionName,DepartmentId,DateCreated,DateUpdated,UpdateBy,CreatedBy")] PositionsModel positionsModel)
        {
            if (id != positionsModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(positionsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PositionsModelExists(positionsModel.Id))
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
            HeaderCrumbs();
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", positionsModel.DepartmentId);
            return View(positionsModel);
        }

        // GET: Positions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var positionsModel = await _context.Positions
                .Include(p => p.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (positionsModel == null)
            {
                return NotFound();
            }
            HeaderCrumbs();
            return View(positionsModel);
        }

        // POST: Positions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var positionsModel = await _context.Positions.FindAsync(id);
            if (positionsModel != null)
            {
                _context.Positions.Remove(positionsModel);
            }
            HeaderCrumbs();
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PositionsModelExists(int id)
        {
            return _context.Positions.Any(e => e.Id == id);
        }
    }
}
