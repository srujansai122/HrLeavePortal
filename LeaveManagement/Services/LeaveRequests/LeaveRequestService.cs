using LeaveManagement.Services.LeaveRequests;
using LeaveManagement.Models.LeaveRequests;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LeaveManagement.Models.LeaveAllocations;
using Microsoft.AspNetCore.Http.HttpResults;

public class LeaveRequestService : ILeaveRequestService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LeaveRequestService(
        ApplicationDbContext context,
        IMapper mapper,
        UserManager<User> userManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task CancelLeaveRequest(int leaveRequestId)
    {
        var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);

        if (leaveRequest == null)
            throw new InvalidOperationException("Leave request not found.");

        var cancelledStatus = await _context.LeaveRequestStatuses
            .FirstOrDefaultAsync(x => x.Name == "Cancelled");

        if (cancelledStatus == null)
            throw new InvalidOperationException("Cancelled status not found.");

        leaveRequest.LeaveRequestStatusId = cancelledStatus.Id;


        var currentDate = DateTime.Now;
        var period = await _context.Periods.SingleAsync(q => q.endDate.Year == currentDate.Year);
        var allocation = await _context.LeaveAllocations.FirstOrDefaultAsync(x => x.EmployeeId == leaveRequest.EmployeeId && x.LeaveTypeId == leaveRequest.LeaveTypeId && x.PeriodId == period.Id);
        if (allocation != null)
        {
            allocation.DaysAllocated += (leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber) + 1;
        }

        await _context.SaveChangesAsync();
    }


    public async Task<bool> CheckRequestExceedsMaximumDays(int leaveTypeId, DateOnly startDate, DateOnly endDate)
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        var currentDate = DateTime.Now;
        var period = await _context.Periods.SingleAsync(q => q.endDate.Year == currentDate.Year);
        var numberOfDays = (endDate.DayNumber - startDate.DayNumber) + 1;
        var allocation = _context.LeaveAllocations.FirstOrDefault(x => x.EmployeeId == user.Id && x.LeaveTypeId == leaveTypeId && x.PeriodId == period.Id);
        return allocation != null && numberOfDays <= allocation.DaysAllocated;
    }

    public async Task CreateLeaveRequest(LeaveRequestCreateViewModel model)
    {
        var leaveRequest = _mapper.Map<LeaveRequest>(model);

        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        leaveRequest.EmployeeId = user.Id;

        var pendingStatus = _context.LeaveRequestStatuses.FirstOrDefault(x => x.Name == "Pending");
        if (pendingStatus == null)
            throw new InvalidOperationException("Pending status not found in database.");

        leaveRequest.LeaveRequestStatusId = pendingStatus.Id;

        _context.LeaveRequests.Add(leaveRequest);


        var currentDate = DateTime.Now;
        var period = await _context.Periods.SingleAsync(q => q.endDate.Year == currentDate.Year);
        var numberOfDays = (leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber) + 1;
        var allocation = _context.LeaveAllocations.FirstOrDefault(x => x.EmployeeId == user.Id && x.LeaveTypeId == leaveRequest.LeaveTypeId && x.PeriodId == period.Id);
        if (allocation == null)
            throw new InvalidOperationException("No leave allocation found for this user and leave type.");

        if (numberOfDays > allocation.DaysAllocated)
            throw new InvalidOperationException("You do not have enough leave days allocated for this request.");

        allocation.DaysAllocated -= numberOfDays;


        await _context.SaveChangesAsync();

    }

    public async Task<EmployeeLeaveRequestListViewModel> AdminViewGetAllLeaveRequests()
    {
        var leaveRequests = await _context.LeaveRequests
            .Include(x => x.LeaveType)
            .Include(x => x.LeaveRequestStatus)
            .Include(x => x.Employee)
            .ToListAsync();

        var model = new EmployeeLeaveRequestListViewModel
        {
            TotalRequests = leaveRequests.Count,
            ApprovedRequests = leaveRequests.Count(r => r.LeaveRequestStatus != null && r.LeaveRequestStatus.Name == "Approved"),
            PendingRequests = leaveRequests.Count(r => r.LeaveRequestStatus != null && r.LeaveRequestStatus.Name == "Pending"),
            RejectedRequests = leaveRequests.Count(r => r.LeaveRequestStatus != null && r.LeaveRequestStatus.Name == "Denied"),
            LeaveRequests = leaveRequests.Select(r => new LeaveRequestListViewModel
            {
                Id = r.Id,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                NumberOfDays = (r.EndDate.DayNumber - r.StartDate.DayNumber) + 1,
                LeaveType = r.LeaveType != null ? r.LeaveType.Name : "Unknown",
                LeaveRequestStatus = r.LeaveRequestStatus != null ? r.LeaveRequestStatus.Name : "Unknown"
            }).ToList()
        };

        return model;
    }

    public async Task<List<LeaveRequestListViewModel>> GetEmployeeLeaveRequests()
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        var leaveRequests = await _context.LeaveRequests.Include(x => x.LeaveType).Include(x => x.LeaveRequestStatus).Where(x => x.EmployeeId == user.Id).ToListAsync();
        var model = leaveRequests.Select(x => new LeaveRequestListViewModel
        {
            Id = x.Id,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            NumberOfDays = (x.EndDate.DayNumber - x.StartDate.DayNumber) + 1,
            LeaveType = x.LeaveType != null ? x.LeaveType.Name : "Unknown",
            LeaveRequestStatus = x.LeaveRequestStatus != null ? x.LeaveRequestStatus.Name : "Unknown"
        }).ToList();
        return model;
    }


    public async Task<ReviewLeaveRequestViewModel> GetLeaveRequestForReview(int id)
    {
        var leaveRequest = await _context.LeaveRequests.Include(q => q.LeaveType).Include(q => q.Employee).Include(q => q.LeaveType).Include(q => q.LeaveRequestStatus).FirstAsync(x => x.Id == id);

        var model = new ReviewLeaveRequestViewModel
        {
            StartDate = leaveRequest.StartDate,
            EndDate = leaveRequest.EndDate,
            NumberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber + 1,
            LeaveRequestStatus = leaveRequest.LeaveRequestStatus.Name,
            RequestComments = leaveRequest.RequestComments,
            Id = leaveRequest.Id,
            LeaveType = leaveRequest.LeaveType.Name,
            employeesListViewModel = new EmployeesListViewModel
            {
                LastName = leaveRequest.Employee.LastName,
                FirstName = leaveRequest.Employee.FirstName,
                dateOfBirth = leaveRequest.Employee.dateOfBirth,
                Email = leaveRequest.Employee.Email,
                Id = leaveRequest.Employee.Id
            }
        };
        return model;
    }


    public async Task<bool> Reject(int id)
    {
        var leaveRequest = await _context.LeaveRequests
            .Include(r => r.LeaveType)
            .Include(r => r.Employee)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (leaveRequest == null)
            return false;

        var rejectedStatus = await _context.LeaveRequestStatuses
            .FirstOrDefaultAsync(s => s.Name == "Denied");

        if (rejectedStatus == null)
            return false;

        // Update status
        leaveRequest.LeaveRequestStatusId = rejectedStatus.Id;

        // Return days back to allocation



        var currentDate = DateTime.Now;
        var period = await _context.Periods.SingleAsync(q => q.endDate.Year == currentDate.Year);
        var allocation = await _context.LeaveAllocations
            .FirstOrDefaultAsync(a => a.EmployeeId == leaveRequest.Employee.Id
                                   && a.LeaveTypeId == leaveRequest.LeaveType.Id
                                   && a.PeriodId == period.Id
                                   );

        if (allocation != null)
        {
            allocation.DaysAllocated += (leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber + 1);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Approve(int id)
    {
        var leaveRequest = await _context.LeaveRequests.FindAsync(id);
        if (leaveRequest == null)
            return false;

        var approvedStatus = await _context.LeaveRequestStatuses
            .FirstOrDefaultAsync(s => s.Name == "Approved");

        if (approvedStatus == null)
            return false;

        // Update status only
        leaveRequest.LeaveRequestStatusId = approvedStatus.Id;

        await _context.SaveChangesAsync();
        return true;
    }


}
