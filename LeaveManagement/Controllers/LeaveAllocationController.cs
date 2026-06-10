using LeaveManagement.Models.LeaveAllocations;
using LeaveManagement.Services.LeaveAllocations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Controllers
{
    [Authorize]
    public class LeaveAllocationController(ILeaveAllocationsService leaveAllocationsService) : Controller
    {
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var employeesVm = await leaveAllocationsService.GetEmployees();
            return View(employeesVm);
        }

        public async Task<IActionResult> Details(string? userId)
        {
            var employeeVm = await leaveAllocationsService.GetEmployeeAllocations(userId);
            return View(employeeVm);
        }

        public async Task<IActionResult> AllocateLeave(string employeeId)
        {
            await leaveAllocationsService.AllocateLeave(employeeId);
            return RedirectToAction(nameof(Details), new { userId = employeeId });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditAllocation(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allocationEditVm = await leaveAllocationsService.GetEmployeeAllocation(id);
            if (allocationEditVm == null)
            {
                return NotFound();
            }
            return View(allocationEditVm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAllocation(LeaveAllocationEditViewModel allocationEditViewModel)
        {
            if (ModelState.IsValid)
            {
                var isEdited = await leaveAllocationsService.EditAllocation(allocationEditViewModel);
                if (isEdited)
                {
                    return RedirectToAction(nameof(Details), new { userId = allocationEditViewModel.Employee.Id });
                }
            }


            ModelState.AddModelError("NumberOfDays", "The number of days exceeds the maximum allowed for this leave type.");
            var days = allocationEditViewModel.NumberOfDays;
            allocationEditViewModel = await leaveAllocationsService.GetEmployeeAllocation(allocationEditViewModel.Id);
            return View(allocationEditViewModel);
        }
    }
}