using AutoMapper;
using hr.core.application.Employees;
using hr.core.application.commons;
using hr.core.domain.Employees;
using hr.core.domain.commons;

namespace hr.infrastracture.context {
    public class EmployeeMappingProfile : Profile {
        public EmployeeMappingProfile() {
            CreateMap<Employee, EmployeeDTO>();
            CreateMap<EmployeeDTO, Employee>();
                // .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            
            CreateMap<Bio, BioDTO>();
            CreateMap<BioDTO, Bio>();

            CreateMap<Address, AddressDTO>();
            CreateMap<AddressDTO, Address>();
            
            CreateMap<WorkSchedule, WorkScheduleDTO>();
            CreateMap<WorkScheduleDTO, WorkSchedule>();
            
            CreateMap<TimeValue, TimeValueDTO>();
            CreateMap<TimeValueDTO, TimeValue>();

            CreateMap<MonetaryValue, MonetaryValueDTO>();
            CreateMap<MonetaryValueDTO, MonetaryValue>();

            CreateMap<SalaryGrade, SalaryGradeDTO>(); // ???
            CreateMap<SalaryGradeDTO, SalaryGrade>(); // ???
        }
    }
}