using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
{
    // override protected void OnModelCreating(ModelBuilder builder)
    // {
    //     base.OnModelCreating(builder);
    //     builder.Entity<IdentityRole>().HasData(
    //         new IdentityRole { Id = "1e1e51ce-4da1-4d07-9e64-0533a6b3e75c", Name = "Employee", NormalizedName = "EMPLOYEE" },
    //         new IdentityRole { Id = "f2bd4794-fd58-4fbf-a4d3-88be7818419b", Name = "Administrator", NormalizedName = "ADMINISTRATOR" },
    //         new IdentityRole { Id = "c9849be9-b4dd-4992-85df-be260885fdb5", Name = "Supervisor", NormalizedName = "SUPERVISOR" }
    //     );

    //     // var hasher = new PasswordHasher<User>();
    //     // var hash = hasher.HashPassword(null, "Password@123");
    //     // Console.WriteLine(hash);

    //     builder.Entity<User>().HasData(
    //         new User
    //         {
    //             Id = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    //             UserName = "admin@gmail.com",
    //             NormalizedUserName = "ADMIN@GMAIL.COM",
    //             Email = "admin@gmail.com",
    //             NormalizedEmail = "ADMIN@GMAIL.COM",
    //             EmailConfirmed = true,
    //             PasswordHash = "AQAAAAIAAYagAAAAEIeziMu6FDgqfXoaoWJPybrIRKTHhdjYdHB5jC12ZFFwMCHDa+NhMQkmafD01v6pgg=="
    //         }
    //     );

    //     builder.Entity<UserRole<string>>().HasData(
    //         new UserRole<string>
    //         {
    //             UserId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    //             RoleId = "f2bd4794-fd58-4fbf-a4d3-88be7818419b"
    //         }
    //     );
    // }

    public DbSet<LeaveType> LeaveTypes { get; set; }
}
