namespace LeaveManagement.Models.LeaveRequests
{
    public class EmployeeLeaveRequestListViewModel
    {
        public int Id { get; set; }
        public int TotalRequests { get; set; }

        public int ApprovedRequests { get; set; }

        public int PendingRequests { get; set; }

        public int RejectedRequests { get; set; }

        public List<LeaveRequestListViewModel> LeaveRequests { get; set; } = new List<LeaveRequestListViewModel>();

    }
}