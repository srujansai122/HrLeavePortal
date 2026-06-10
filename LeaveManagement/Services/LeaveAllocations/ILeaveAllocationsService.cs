using LeaveManagement.Models.LeaveAllocations;

namespace LeaveManagement.Services.LeaveAllocations
{
    public interface ILeaveAllocationsService
    {
        Task AllocateLeave(string employeeId);

        Task<List<LeaveAllocation>> getAllocations(string? userId);

        Task<EmployeeAllocationViewModel> GetEmployeeAllocations(string? userId);

        Task<List<EmployeesListViewModel>> GetEmployees();

        Task<LeaveAllocationEditViewModel> GetEmployeeAllocation(int id);

        Task<bool> EditAllocation(LeaveAllocationEditViewModel allocationEditViewModel);

        Task<bool> DaysExceedMaximum(int leaveTypeId, int days);
    }
}