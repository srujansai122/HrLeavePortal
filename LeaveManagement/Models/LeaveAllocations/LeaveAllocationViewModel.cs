using LeaveManagement.Models.LeaveTypes;

namespace LeaveManagement.Models.LeaveAllocations
{
    public class LeaveAllocationViewModel
    {
        public int Id { get; set; }
        public int NumberOfDays { get; set; }
        public PeriodViewModel Period { get; set; } = new PeriodViewModel();

        public LeaveTypeViewModel LeaveType { get; set; } = new LeaveTypeViewModel();

    }
}