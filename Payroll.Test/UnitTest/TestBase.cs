using Payroll.Application;
using Payroll.EventSourcing;
using Users = Payroll.Domain.Users.Events.V1;
using BusinessYear = Payroll.Domain.BusinessYears.Events.V1;
using Employees = Payroll.Domain.Employees.Events.V1;
using Deductions = Payroll.Domain.Deductions.Events.V1;
using SalaryGrades = Payroll.Domain.SalaryGrades.Events.V1;
using PayrollPeriods = Payroll.Domain.PayrollPeriods.Events.V1;
using Payroll.Infrastructure;
using Payroll.Application.Users.Projections;
using System;
using Payroll.Domain.Users;
using static Payroll.Application.Users.Contracts.V1;
using Payroll.Application.Employees.Projections;

namespace Payroll.Test.UnitTest
{
  public abstract class TestBase : IDisposable
  {
    protected ITypeMapper _typeMapper;
    protected IEventStore _eventStore;
    protected ISnapshotStore _snapshots;
    protected IProjectionManager _projections;
    protected IEncryptionProvider _enc;
    protected PayrollApplicationService _app;
    protected AuthService _auth;
    protected IAccessTokenProvider _tokenProvider;
    protected Guid _rootId;
    protected string _accessTokenStub;

    public TestBase()
    {
      _typeMapper = new TypeMapper()
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
      
      // initialize
      _eventStore = new MemoryEventStore(_typeMapper);
      _snapshots = new MemorySnapshotStore();
      _projections = new ProjectionManager(_snapshots);
      _tokenProvider = new TokenProvider("some secret", _snapshots);
      _enc = new BCryptEncryptionProvider();
      _app = new PayrollApplicationService(_tokenProvider, _eventStore);
      _auth = new AuthService(_eventStore, _tokenProvider, _snapshots, _enc);

      // register projections
      _projections.Register(new ActiveUsersProjection());
      _projections.Register(new PassHashProjection());
      _projections.Register(new ActiveEmployeesProjection());
      _projections.Register(new SeparatedEmployeesProjection());
      _projections.Register(new EmployeesOnLeaveProjection());

      // hook projection updates
      _eventStore.AfterDBReload(_projections.UpdateProjections);
      _eventStore.AfterSave(_projections.UpdateProjections);

      _rootId = Guid.NewGuid();
      var root_event = new Users.UserCreated {
        Id = _rootId,
        Username = "test",
        PassHash = "$2a$11$VQiq2RnPyec5V9D3bGX1CufYgOK8jtKwZwD0nHqkQAQKQ7kWKIPYK",
        CreatedBy = _rootId,
        CreatedAt = DateTimeOffset.Now
      };
      var stubRoot = new User();
      stubRoot.Apply(root_event);
      _eventStore.Save(stubRoot);

      _accessTokenStub = _auth.Handle(new PasswordLogin {
        Username = "test",
        Password = "p4ssw0d"
      });
    }

    public void Dispose() {
      _typeMapper = null;
      _eventStore = null;
      _snapshots = null;
      _projections = null;
      _enc = null;
      _app = null;
      _auth = null;
    }
  }
}