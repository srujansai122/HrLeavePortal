using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeaveManagement.Data;

namespace LeaveManagement.Controllers
{
    public class LeaveRequestStatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveRequestStatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LeaveRequestStatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.LeaveRequestStatuses.ToListAsync());
        }

        // GET: LeaveRequestStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveRequestStatus = await _context.LeaveRequestStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveRequestStatus == null)
            {
                return NotFound();
            }

            return View(leaveRequestStatus);
        }

        // GET: LeaveRequestStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaveRequestStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] LeaveRequestStatus leaveRequestStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leaveRequestStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leaveRequestStatus);
        }

        // GET: LeaveRequestStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveRequestStatus = await _context.LeaveRequestStatuses.FindAsync(id);
            if (leaveRequestStatus == null)
            {
                return NotFound();
            }
            return View(leaveRequestStatus);
        }

        // POST: LeaveRequestStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] LeaveRequestStatus leaveRequestStatus)
        {
            if (id != leaveRequestStatus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaveRequestStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveRequestStatusExists(leaveRequestStatus.Id))
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
            return View(leaveRequestStatus);
        }

        // GET: LeaveRequestStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveRequestStatus = await _context.LeaveRequestStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveRequestStatus == null)
            {
                return NotFound();
            }

            return View(leaveRequestStatus);
        }

        // POST: LeaveRequestStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveRequestStatus = await _context.LeaveRequestStatuses.FindAsync(id);
            if (leaveRequestStatus != null)
            {
                _context.LeaveRequestStatuses.Remove(leaveRequestStatus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveRequestStatusExists(int id)
        {
            return _context.LeaveRequestStatuses.Any(e => e.Id == id);
        }
    }
}
