using System.ComponentModel.DataAnnotations;

namespace LeaveManagement.Models.LeaveAllocations
{
    public class EmployeesListViewModel
    {
        public string Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Date Of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateOnly dateOfBirth { get; set; }


        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

    }
}