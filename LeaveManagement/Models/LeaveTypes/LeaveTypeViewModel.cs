using System.ComponentModel.DataAnnotations;

namespace LeaveManagement.Models.LeaveTypes
{
    public class LeaveTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Maximum Number of Days")]
        public int NumberOfDays { get; set; }


    }

    public class CreateLeaveTypeViewModel
    {
        [Required]
        [Length(4, 100, ErrorMessage = "Name should be at between 4 and 100 characters long")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, 90, ErrorMessage = "Number of days should be between 1 and 90")]
        [Display(Name = "Maximum Number of Days")]
        public int NumberOfDays { get; set; }
    }

    public class EditLeaveTypeViewModel
    {

        public int Id { get; set; }

        [Required]
        [Length(4, 100, ErrorMessage = "Name should be at between 4 and 100 characters long")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, 90, ErrorMessage = "Number of days should be between 1 and 90")]
        [Display(Name = "Maximum Number of Days")]
        public int NumberOfDays { get; set; }
    }
}