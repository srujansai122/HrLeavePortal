using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagement.Models.LeaveRequests
{
    public class LeaveRequestCreateViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        public int LeaveTypeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly EndDate { get; set; }


        [StringLength(100)]
        public string? RequestComments { get; set; }

        public SelectList? LeaveTypes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate > EndDate)
            {
                yield return new ValidationResult("Start date cannot be after end date.", new[] { nameof(StartDate), nameof(EndDate) });
            }
        }
    }
}