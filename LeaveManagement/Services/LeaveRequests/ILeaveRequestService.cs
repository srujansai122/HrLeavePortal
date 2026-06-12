using LeaveManagement.Models.LeaveRequests;

namespace LeaveManagement.Services.LeaveRequests
{
    public interface ILeaveRequestService
    {
        Task CreateLeaveRequest(LeaveRequestCreateViewModel model);
        Task<List<LeaveRequestListViewModel>> GetEmployeeLeaveRequests();

        Task<EmployeeLeaveRequestListViewModel> AdminViewGetAllLeaveRequests();

        Task CancelLeaveRequest(int leaveRequestId);

        Task<bool> Reject(int id);

        Task<bool> Approve(int id);

        Task<bool> CheckRequestExceedsMaximumDays(int leaveTypeId, DateOnly startDate, DateOnly endDate);


        Task<ReviewLeaveRequestViewModel> GetLeaveRequestForReview(int id);

    }
}