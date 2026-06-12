namespace LeaveManagement.Models.LeaveRequests
{

    // EMPLOYEE VIEW MODEL
    public class LeaveRequestListViewModel
    {
        public int Id { get; set; }


        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public int NumberOfDays { get; set; }

        public string LeaveType { get; set; } = string.Empty;

        public string LeaveRequestStatus { get; set; } = string.Empty;

    }
}