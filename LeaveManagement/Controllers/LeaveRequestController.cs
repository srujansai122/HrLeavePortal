using LeaveManagement.Models.LeaveRequests;
using LeaveManagement.Services;
using LeaveManagement.Services.LeaveRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagement.Controllers
{
    [Authorize]
    public class LeaveRequestController(ILeaveTypeService leaveTypesService, ILeaveRequestService leaveRequestService) : Controller
    {
        // LeaveRequests Employee made
        public async Task<IActionResult> Index()
        {
            var leaveRequestsListVm = await leaveRequestService.GetEmployeeLeaveRequests();
            return View(leaveRequestsListVm);
        }

        public async Task<IActionResult> Create(int? leaveTypeId)
        {

            var leaveTypes = await leaveTypesService.GetAllLeaveTypes();
            var leaveTypesList = new SelectList(leaveTypes, "Id", "Name", leaveTypeId);
            var viewModel = new LeaveRequestCreateViewModel
            {
                LeaveTypes = leaveTypesList,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequestCreateViewModel leaveRequestCreateViewModel)
        {
            if (!await leaveRequestService.CheckRequestExceedsMaximumDays(leaveRequestCreateViewModel.LeaveTypeId, leaveRequestCreateViewModel.StartDate, leaveRequestCreateViewModel.EndDate))
            {
                ModelState.AddModelError(nameof(leaveRequestCreateViewModel.EndDate), "You do not have enough leave days allocated for this request.");
            }

            if (ModelState.IsValid)
            {
                await leaveRequestService.CreateLeaveRequest(leaveRequestCreateViewModel);
                return RedirectToAction("Index");
            }

            var leaveTypes = await leaveTypesService.GetAllLeaveTypes();
            var leaveTypesList = new SelectList(leaveTypes, "Id", "Name");
            leaveRequestCreateViewModel.LeaveTypes = leaveTypesList;
            return View(leaveRequestCreateViewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            await leaveRequestService.CancelLeaveRequest(id);
            return RedirectToAction("Index");
        }

        //      Admin view of all leave requests
        [Authorize(Policy = "AdminSupervisorOnly")]
        public async Task<IActionResult> ListRequests()
        {
            var model = await leaveRequestService.AdminViewGetAllLeaveRequests();
            return View(model);
        }

        public async Task<IActionResult> Review(int id)
        {
            var reviewLeaveRequestViewModel = await leaveRequestService.GetLeaveRequestForReview(id);
            return View(reviewLeaveRequestViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var isApproved = await leaveRequestService.Approve(id);

            if (!isApproved)
            {
                ModelState.AddModelError("", "Unable to approve the leave request.");
            }
            return RedirectToAction("ListRequests");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var isRejected = await leaveRequestService.Reject(id);

            if (!isRejected)
            {
                ModelState.AddModelError("", "Unable to reject the leave request.");
            }

            return RedirectToAction("ListRequests");
        }

    }
}