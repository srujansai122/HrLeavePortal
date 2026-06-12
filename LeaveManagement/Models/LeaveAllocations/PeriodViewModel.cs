using System.ComponentModel.DataAnnotations;

namespace LeaveManagement.Models.LeaveAllocations
{
    public class PeriodViewModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly startDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly endDate { get; set; }
    }
}


