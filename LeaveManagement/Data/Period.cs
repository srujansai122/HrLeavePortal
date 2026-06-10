namespace LeaveManagement.Data
{
    public class Period
    {
        public int Id { get; set; }

        public DateOnly startDate { get; set; }

        public DateOnly endDate { get; set; }
    }
}