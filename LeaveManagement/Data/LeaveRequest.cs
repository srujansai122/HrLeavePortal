namespace LeaveManagement.Data
{
    public class LeaveRequest
    {
        public int Id { get; set; }

        public LeaveType? LeaveType { get; set; }
        public int LeaveTypeId { get; set; }


        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }


        public LeaveRequestStatus? LeaveRequestStatus { get; set; }
        public int LeaveRequestStatusId { get; set; }

        public string? RequestComments { get; set; }

        public User? Employee { get; set; }
        public string EmployeeId { get; set; } = string.Empty;

        public User? ReviewedBy { get; set; }
        public string? ReviewedById { get; set; }

    }
}