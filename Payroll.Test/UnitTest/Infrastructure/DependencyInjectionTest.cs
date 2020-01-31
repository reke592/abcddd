using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Payroll.Application;
using Payroll.EventSourcing;
using Payroll.Infrastructure;
using Xunit;
using Users = Payroll.Domain.Users.Events.V1;
using BusinessYear = Payroll.Domain.BusinessYears.Events.V1;
using Employees = Payroll.Domain.Employees.Events.V1;
using Deductions = Payroll.Domain.Deductions.Events.V1;
using SalaryGrades = Payroll.Domain.SalaryGrades.Events.V1;
using PayrollPeriods = Payroll.Domain.PayrollPeriods.Events.V1;
using Payroll.Application.Users.Projections;
using Payroll.Application.Employees.Projections;
using Payroll.Application.BusinessYears.Projections;
using Payroll.Application.SalaryGrades.Projections;
using Payroll.Application.BusinessYears;
using Payroll.Application.Deductions;
using Payroll.Application.Employees;
using Payroll.Application.PayrollPeriods;
using Payroll.Application.SalaryGrades;
using Payroll.Application.Users;
using Payroll.EventSourcing.Serialization.YAML;
using Payroll.Application.Deductions.Projections;

namespace Payroll.Test.UnitTest.Infrastructure
{
  public class DependencyInjectionTest
  {
    [Fact]
    public void CanResolveServices()
    {
      var container = new WindsorContainer();

      container.Register(Component.For<ITypeMapper>()
        .ImplementedBy<TypeMapper>()
        .LifestyleSingleton());

      container.Register(Component.For<IYAMLSerializer>()
        .ImplementedBy<YAMLSerializer>()
        .LifestyleSingleton());

      container.Register(Component.For<IEncryptionProvider>()
        .ImplementedBy<BCryptEncryptionProvider>()
        .LifestyleSingleton());

      container.Register(Component.For<IEventStore>()
        .ImplementedBy<MemoryEventStore>()
        .LifestyleSingleton());
      
      container.Register(Component.For<ICacheStore>()
        .ImplementedBy<MemoryCacheStore>()
        .LifestyleSingleton());

      container.Register(Component.For<IAccessTokenProvider>()
        .ImplementedBy<TokenProvider>()
        .LifestyleSingleton());
      
      container.Register(Component.For<IProjectionManager>()
        .ImplementedBy<ProjectionManager>()
        .LifestyleSingleton());
      
      // app services -----------

      container.Register(Component.For<IPayrollApplicationServices>()
        .ImplementedBy<PayrollApplicationService>()
        .LifestyleSingleton());

      container.Register(Component.For<IBusinessYearAppService>()
        .ImplementedBy<BusinessYearAppService>()
        .LifestyleTransient());
      
      container.Register(Component.For<IDeductionAppService>()
        .ImplementedBy<DeductionAppService>()
        .LifestyleTransient());

      container.Register(Component.For<IEmployeeAppService>()
        .ImplementedBy<EmployeeAppService>()
        .LifestyleTransient());
      
      container.Register(Component.For<IPayrollPeriodAppService>()
        .ImplementedBy<PayrollPeriodAppService>()
        .LifestyleTransient());
      
      container.Register(Component.For<ISalaryGradeAppService>()
        .ImplementedBy<SalaryGradeAppService>()
        .LifestyleTransient());
      
      container.Register(Component.For<IUserAppService>()
        .ImplementedBy<AuthService>()
        .LifestyleTransient());
      
      var app = container.Resolve<IPayrollApplicationServices>();
      var eventStore = container.Resolve<IEventStore>();
      var cacheStore = container.Resolve<ICacheStore>();
      var projections = container.Resolve<IProjectionManager>();
      var mapper = container.Resolve<ITypeMapper>();
      var serializer = container.Resolve<IYAMLSerializer>();
    
      projections.Register(new ActiveUsersProjection());
      projections.Register(new PassHashProjection());
      projections.Register(new ActiveEmployeesProjection());
      projections.Register(new SeparatedEmployeesProjection());
      projections.Register(new EmployeesOnLeaveProjection());
      projections.Register(new BusinessYearHistoryProjection());
      projections.Register(new SalaryGradeHistoryProjection());
      projections.Register(new MandatoryDeductionProjection());
      projections.Register(new NonMandatoryDeductionProjection());
      projections.Register(CurrentBusinessYearProjection.Instance);

      mapper
        .Map<Users.UserCreated>("User Created")
        .Map<Users.UserPasswordChanged>("User Password Changed")
        .Map<Users.UserUpdateAttemptFailed>("User Update Attempt Failed")
        .Map<BusinessYear.BusinessYearCreated>("BusinessYear Created")
        .Map<BusinessYear.BusinessYearStarted>("BusinessYear Started")
        .Map<BusinessYear.BusinessYearEnded>("BusinessYear Ended")
        .Map<BusinessYear.BusinessYearConsigneeCreated>("BusinessYear Consignee Created")
        .Map<BusinessYear.BusinessYearConsigneeUpdated>("BusinessYear Consignee Updated")
        .Map<BusinessYear.BusinessYearUpdateAttemptFailed>("BusinessYear Update Attempt Failed")
        .Map<Employees.EmployeeCreated>("Employee Created")
        .Map<Employees.EmployeeBioDataUpdated>("Employee BioData Updated")
        .Map<Employees.EmployeeSalaryGradeUpdated>("Employee SalaryGrade Updated")
        // .Map<Employees.EmployeeStatusChanged>("Employee Status Changed")
        .Map<Employees.EmployeeStatusEmployed>("Employee Employed")
        .Map<Employees.EmployeeStatusSeparated>("Employee Separated")
        .Map<Employees.EmployeeLeaveGranted>("Employee Leave Granted")
        .Map<Employees.EmployeeLeaveRevoked>("Employee Leave Revoked")
        .Map<Employees.EmployeeLeaveEnded>("Employee Leave Ended")
        .Map<Employees.EmployeeUpdateAttemptFailed>("Employee Update Attempt Failed")
        .Map<Deductions.MandatoryDeductionCreated>("Mandatory Deduction Created")
        .Map<Deductions.NonMandatoryDeductionCreated>("NonMandatory Deduction Created")
        // .Map<Deductions.DeductionAmountSettled>("Deduction Amount Settled")
        .Map<Deductions.DeductionScheduleSettled>("Deduction Schedule Settled")
        .Map<Deductions.DeductionPaymentCreated>("Deduction Payment Created")
        .Map<Deductions.DeductionPaymentCompleted>("Deduction Payment Completed")
        .Map<Deductions.DeductionUpdateAttemptFailed>("Deduction Attempt Failed")
        .Map<SalaryGrades.SalaryGradeCreated>("SalaryGrade Created")
        .Map<SalaryGrades.SalaryGradeGrossUpdated>("SalaryGrade Gross Updated")
        .Map<SalaryGrades.SalaryGradeUpdateAttemptFailed>("SalaryGrade Update Attempt Failed")
        .Map<PayrollPeriods.PayrollPeriodCreated>("PayrollPeriod Created")
        .Map<PayrollPeriods.PayrollPeriodApplicableMonthSettled>("PayrollPeriod Applicable Month Settled")
        .Map<PayrollPeriods.PayrollPeriodConsigneeIncluded>("PayrollPeriod Consignee Included")
        .Map<PayrollPeriods.PayrollPeriodConsigneeRemoved>("PayrollPeriod Consignee Removed")
        .Map<PayrollPeriods.PayrollPeriodEmployeeIncluded>("PayrollPeriod Employee Included")
        .Map<PayrollPeriods.PayrollPeriodEmployeeExcluded>("PayrollPeriod Employee Excluded")
        .Map<PayrollPeriods.PayrollPeriodDeductionPaymentAdjusted>("PayrollPeriod Deduction Payment Adjusted")
        .Map<PayrollPeriods.PayrollPeriodEmployeeSalaryReceived>("PayrollPeriod Employee Salary Received")
        .Map<PayrollPeriods.PayrollPeriodUpdateAttemptFailed>("PayrollPeriod Update Attempt Failed");
    }
  }
}