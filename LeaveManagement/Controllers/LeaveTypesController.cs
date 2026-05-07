using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeaveManagement.Data;
using LeaveManagement.Models.LeaveTypes;
using LeaveManagement.Services;
using Microsoft.AspNetCore.Authorization;

namespace LeaveManagement.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LeaveTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILeaveTypeService _leaveTypeService;
        public LeaveTypesController(ApplicationDbContext context, ILeaveTypeService leaveTypeService)
        {
            _context = context;
            _leaveTypeService = leaveTypeService;
        }

        // GET: LeaveTypes
        public async Task<IActionResult> Index()
        {
            var leaveTypes = await _leaveTypeService.GetAllLeaveTypes();

            return View(leaveTypes);
        }

        // GET: LeaveTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _leaveTypeService.GetDetailsById(id);

            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // GET: LeaveTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLeaveTypeViewModel createleaveType)
        {

            if (createleaveType.Name.Contains("vacation"))
            {
                ModelState.AddModelError("Name", "Name cannot contain the word 'vacation'");
            }

            bool isExists = await _context.LeaveTypes.AnyAsync(q => q.Name.ToLower().Equals(createleaveType.Name.ToLower()));

            if (isExists)
            {
                ModelState.AddModelError("Name", "A leave type with the same name already exists.");
            }

            if (ModelState.IsValid)
            {
                await _leaveTypeService.Create(createleaveType);
                return RedirectToAction(nameof(Index));
            }
            return View(createleaveType);
        }

        // GET: LeaveTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _context.LeaveTypes.FindAsync(id);

            if (leaveType == null)
            {
                return NotFound();
            }

            var model = new Models.LeaveTypes.EditLeaveTypeViewModel
            {
                Id = leaveType.Id,
                Name = leaveType.Name,
                NumberOfDays = leaveType.NumberOfDays
            };
            return View(model);
        }

        // POST: LeaveTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditLeaveTypeViewModel editLeaveTypeViewModel)
        {
            if (editLeaveTypeViewModel == null)
            {
                return NotFound();
            }

            if (id != editLeaveTypeViewModel.Id)
            {
                return NotFound();
            }

            bool isExists = await _context.LeaveTypes.AnyAsync(q => q.Name.ToLower().Equals(editLeaveTypeViewModel.Name.ToLower())
             && q.Id != editLeaveTypeViewModel.Id);

            if (isExists)
            {
                ModelState.AddModelError("Name", "A leave type with the same name already exists.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _leaveTypeService.Edit(id, editLeaveTypeViewModel);
                    if (!result)
                    {
                        return NotFound();
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveTypeExists(editLeaveTypeViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(editLeaveTypeViewModel);
        }

        // GET: LeaveTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveType == null)
            {
                return NotFound();
            }

            var model = new LeaveTypeViewModel
            {
                Id = leaveType.Id,
                Name = leaveType.Name,
                NumberOfDays = leaveType.NumberOfDays
            };

            return View(model);
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _leaveTypeService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveTypeExists(int id)
        {
            return _context.LeaveTypes.Any(e => e.Id == id);
        }
    }
}
