namespace LeaveManagement.Data
{
    public class LeaveAllocation
    {
        public int Id { get; set; }

        public LeaveType? LeaveType { get; set; }
        public int LeaveTypeId { get; set; }

        public User? Employee { get; set; }
        public string EmployeeId { get; set; }

        public Period? Period { get; set; }
        public int PeriodId { get; set; }

        public int DaysAllocated { get; set; }

    }
}