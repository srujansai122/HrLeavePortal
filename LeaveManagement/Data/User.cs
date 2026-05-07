using Microsoft.AspNetCore.Identity;

namespace LeaveManagement.Data
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly dateOfBirth { get; set; }


    }
}