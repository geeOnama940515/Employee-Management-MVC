﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeCRUD.Data;
using EmployeeCRUD.Models;

namespace EmployeeCRUD.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            HeaderCrumbs();


            return View(await _context.Employees.ToListAsync());
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeModel = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeModel == null)
            {
                HeaderCrumbs();
                return NotFound();
            }
            HeaderCrumbs();
            return View(employeeModel);
        }

        // GET: Employee/Create
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

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeModel employeeModel)
        {
            HeaderCrumbs();
            employeeModel.CreatedBy = "GeeRax94";
            if (ModelState.IsValid)
            {
                _context.Add(employeeModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeModel);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            HeaderCrumbs();
            if (id == null)
            {
                return NotFound();
            }

            var employeeModel = await _context.Employees.FindAsync(id);
            employeeModel.DateUpdated = DateTime.Now;
            employeeModel.UpdateBy = "GeeRax94";
            if (employeeModel == null)
            {
                return NotFound();
            }
            return View(employeeModel);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmpNumber,FirstName,MiddleName,LastName,PhoneNumber,DateOfBirth,DateHired,Department,Designation,Email,Address,DateCreated,DateUpdated,UpdateBy,CreatedBy")] EmployeeModel employeeModel)
        {
            HeaderCrumbs();
            if (id != employeeModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeeModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeModelExists(employeeModel.Id))
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
            return View(employeeModel);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            HeaderCrumbs();
            if (id == null)
            {
                return NotFound();
            }

            var employeeModel = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeModel == null)
            {
                return NotFound();
            }

            return View(employeeModel);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HeaderCrumbs();
            var employeeModel = await _context.Employees.FindAsync(id);
            if (employeeModel != null)
            {
                _context.Employees.Remove(employeeModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeModelExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
        private void HeaderCrumbs()
        {
            // Set dynamic data
            ViewBag.PageTitle = "Employees";
            ViewBag.Breadcrumbs = new List<Breadcrumb>
        {
            new Breadcrumb { Name = "Home", Link = Url.Action("Index", "Home") },
            new Breadcrumb { Name = "Employees List", Link = null } // Current page
        };
        }
    }
}
