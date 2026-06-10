using AutoMapper;
using LeaveManagement.Models.LeaveAllocations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Services.LeaveAllocations
{
    public class LeaveAllocationsService(ApplicationDbContext _context, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, IMapper _mapper) : ILeaveAllocationsService
    {

        public async Task AllocateLeave(string employeeId)
        {


            // var leaveTypes = await _context.LeaveTypes.ToListAsync();
            // can write above code in a more efficient way by filtering leave types that have already been allocated to the employee for the current period
            var leaveTypes = await _context.LeaveTypes.Where(q => !q.LeaveAllocations.Any(x => x.EmployeeId == employeeId)).ToListAsync();

            var period = await _context.Periods.FirstOrDefaultAsync(q => q.endDate.Year == DateTime.Now.Year);

            if (period == null)
            {
                return;
            }

            if (!leaveTypes.Any())
            {
                return;
            }

            if (period != null && leaveTypes.Any())
            {
                var monthsRemaining = period.endDate.Month - DateTime.Now.Month;
                foreach (var leaveType in leaveTypes)
                {
                    // var allocationExists = await _context.LeaveAllocations
                    //     .AnyAsync(q => q.EmployeeId == employeeId
                    //                 && q.LeaveTypeId == leaveType.Id
                    //                 && q.PeriodId == period.Id);

                    // if (allocationExists)
                    //     continue;

                    var leavesPerMonth = decimal.Divide(leaveType.NumberOfDays, 12);

                    var allocation = new LeaveAllocation
                    {
                        EmployeeId = employeeId,
                        LeaveTypeId = leaveType.Id,
                        DaysAllocated = (int)Math.Ceiling(leavesPerMonth * monthsRemaining),
                        PeriodId = period.Id
                    };

                    _context.Add(allocation);
                }
                await _context.SaveChangesAsync();
            }
        }


        public async Task<List<LeaveAllocation>> getAllocations(string? userId)
        {
            string employeeId = string.Empty;
            if (string.IsNullOrEmpty(userId))
            {
                var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext?.User);
                employeeId = user.Id;
            }
            else
            {
                employeeId = await userManager.Users.Where(q => q.Id == userId).Select(q => q.Id).SingleAsync();
            }

            // var username = httpContextAccessor.HttpContext?.User?.Identity?.Name;
            // var employeeId = httpContextAccessor.HttpContext?.User?.FindFirst("EmployeeId")?.Value;

            var currentDate = DateTime.Now;
            var leaveAllocations = await _context.LeaveAllocations.Include(q => q.LeaveType).Include(q => q.Period).Include(q => q.Employee).Where(q => q.EmployeeId == employeeId && q.Period.endDate.Year == currentDate.Year).ToListAsync();
            return leaveAllocations;
        }

        public async Task<EmployeeAllocationViewModel> GetEmployeeAllocations(string? userId)
        {
            var user = string.IsNullOrEmpty(userId) ? await userManager.GetUserAsync(httpContextAccessor.HttpContext?.User)
                                                  : await userManager.Users.FirstOrDefaultAsync(q => q.Id == userId);

            var allocations = await getAllocations(user.Id);

            var allocationViewModelList = _mapper.Map<List<LeaveAllocationViewModel>>(allocations);


            var leaveTypes = await _context.LeaveTypes.ToListAsync();
            var countOfLeaveTypes = leaveTypes.Count;

            var employeeViewModel = new EmployeeAllocationViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                IsCompletedAllocation = allocationViewModelList.Count == countOfLeaveTypes,
                DateOfBirth = user.dateOfBirth,
                LeaveAllocations = allocationViewModelList
            };

            return employeeViewModel;
        }

        public async Task<List<EmployeesListViewModel>> GetEmployees()
        {
            var users = await userManager.GetUsersInRoleAsync("Employee");
            return _mapper.Map<List<EmployeesListViewModel>>(users.ToList());
        }


        public async Task<LeaveAllocationEditViewModel> GetEmployeeAllocation(int id)
        {
            var allocation = await _context.LeaveAllocations.Include(q => q.LeaveType).Include(q => q.Period).Include(q => q.Employee).FirstOrDefaultAsync(q => q.Id == id);
            if (allocation == null)
            {
                return null;
            }
            return _mapper.Map<LeaveAllocationEditViewModel>(allocation);
        }

        public async Task<bool> EditAllocation(LeaveAllocationEditViewModel allocationEditViewModel)
        {
            var allocation = await _context.LeaveAllocations.FirstOrDefaultAsync(q => q.Id == allocationEditViewModel.Id);
            if (allocation == null)
            {
                return false;
            }
            if (await DaysExceedMaximum(allocation.LeaveTypeId, allocationEditViewModel.NumberOfDays))
            {
                return false;
            }
            allocation.DaysAllocated = allocationEditViewModel.NumberOfDays;
            _context.Update(allocation);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DaysExceedMaximum(int leaveTypeId, int days)
        {
            var leaveType = await _context.LeaveTypes.FirstOrDefaultAsync(q => q.Id == leaveTypeId);
            if (leaveType == null)
            {
                return false;
            }
            return days > leaveType.NumberOfDays;
        }
    }
}