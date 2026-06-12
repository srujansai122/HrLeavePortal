using LeaveManagement.Models.LeaveAllocations;

namespace LeaveManagement.Models.LeaveRequests
{
    public class ReviewLeaveRequestViewModel
    {
        public int Id { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public int NumberOfDays { get; set; }

        public string LeaveType { get; set; } = string.Empty;

        public string LeaveRequestStatus { get; set; } = string.Empty;

        public string? RequestComments { get; set; }

        public EmployeesListViewModel employeesListViewModel { get; set; } = new EmployeesListViewModel();
    }
}