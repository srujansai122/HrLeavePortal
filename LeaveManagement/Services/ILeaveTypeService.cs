using LeaveManagement.Models.LeaveTypes;

namespace LeaveManagement.Services
{
    public interface ILeaveTypeService
    {
        Task<List<LeaveTypeViewModel>> GetAllLeaveTypes();
        Task<LeaveTypeViewModel?> GetDetailsById(int? id);
        Task Remove(int id);
        Task<bool> Edit(int id, EditLeaveTypeViewModel editLeaveTypeViewModel);
        Task Create(CreateLeaveTypeViewModel createLeaveType);

    }
}