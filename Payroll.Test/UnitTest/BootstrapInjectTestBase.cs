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
using Payroll.EventSourcing;

namespace Payroll.Test.UnitTest
{
  public abstract class BootstrapInjectTestBase : InjectTestBase
  {
    public BootstrapInjectTestBase()
    {
      var eventStore = _container.Resolve<IEventStore>();
      var projections = _container.Resolve<IProjectionManager>();
      var mapper = _container.Resolve<ITypeMapper>();
      
      eventStore.AfterSave(projections.UpdateProjections);
      eventStore.AfterDBReload(projections.UpdateProjections);

      projections.Register(new ActiveUsersProjection());
      projections.Register(new PassHashProjection());
      projections.Register(new ActiveEmployeesProjection());
      projections.Register(new SeparatedEmployeesProjection());
      projections.Register(new EmployeesOnLeaveProjection());
      projections.Register(new BusinessYearHistoryProjection());
      projections.Register(new SalaryGradeHistoryProjection());
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
        .Map<Deductions.DeductionCreated>("Deduction Created")
        .Map<Deductions.DeductionAmountSettled>("Deduction Amount Settled")
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