using LeaveManagement.Data;
using LeaveManagement.Models.LeaveTypes;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Services
{
    public class LeaveTypesService : ILeaveTypeService
    {
        ApplicationDbContext _context;
        public LeaveTypesService(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<List<LeaveTypeViewModel>> GetAllLeaveTypes()
        {
            var leaveTypes = await _context.LeaveTypes
                .Select(q => new LeaveTypeViewModel
                {
                    Id = q.Id,
                    Name = q.Name,
                    NumberOfDays = q.NumberOfDays
                }).ToListAsync();
            return leaveTypes;
        }

        public async Task<LeaveTypeViewModel?> GetDetailsById(int? id)
        {
            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (leaveType == null)
            {
                return null;
            }
            return new LeaveTypeViewModel
            {
                Id = leaveType.Id,
                Name = leaveType.Name,
                NumberOfDays = leaveType.NumberOfDays
            };
        }


        public async Task Remove(int id)
        {
            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveType == null)
            {
                return;
            }
            _context.LeaveTypes.Remove(leaveType);
            await _context.SaveChangesAsync();
        }


        public async Task<bool> Edit(int id, EditLeaveTypeViewModel editLeaveTypeViewModel)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType == null)
            {
                return false;
            }
            leaveType.Name = editLeaveTypeViewModel.Name;
            leaveType.NumberOfDays = editLeaveTypeViewModel.NumberOfDays;
            _context.Update(leaveType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task Create(CreateLeaveTypeViewModel createleaveType)
        {
            var leaveType = new LeaveType
            {
                Name = createleaveType.Name,
                NumberOfDays = createleaveType.NumberOfDays
            };
            _context.LeaveTypes.Add(leaveType);
            await _context.SaveChangesAsync();
        }

    }
}