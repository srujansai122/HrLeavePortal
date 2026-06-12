using AutoMapper;
using LeaveManagement.Models.LeaveAllocations;
using LeaveManagement.Models.LeaveRequests;
using LeaveManagement.Models.LeaveTypes;

namespace LeaveManagement.ProfileMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<LeaveType, LeaveTypeViewModel>();

            // CreateMap<LeaveAllocation, LeaveAllocationViewModel>();
            CreateMap<LeaveAllocation, LeaveAllocationViewModel>()
            .ForMember(dest => dest.NumberOfDays, opt => opt.MapFrom(src => src.DaysAllocated));

            CreateMap<Period, PeriodViewModel>();

            CreateMap<LeaveAllocation, LeaveAllocationEditViewModel>()
            .ForMember(dest => dest.NumberOfDays, opt => opt.MapFrom(src => src.DaysAllocated));

            CreateMap<User, EmployeesListViewModel>().ReverseMap();

            CreateMap<LeaveRequestCreateViewModel, LeaveRequest>();
        }
    }
}