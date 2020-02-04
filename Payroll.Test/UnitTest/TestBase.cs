using System;
using Payroll.Application;
using Payroll.EventSourcing;
using Users = Payroll.Domain.Users.Events.V1;
using BusinessYear = Payroll.Domain.BusinessYears.Events.V1;
using Employees = Payroll.Domain.Employees.Events.V1;
using Deductions = Payroll.Domain.Deductions.Events.V1;
using SalaryGrades = Payroll.Domain.SalaryGrades.Events.V1;
using PayrollPeriods = Payroll.Domain.PayrollPeriods.Events.V1;
using Payroll.Infrastructure;
using Payroll.Domain.Users;
using Payroll.Application.Employees.Projections;
using Payroll.Application.Users.Projections;
using Payroll.Application.BusinessYears.Projections;
using static Payroll.Application.Users.Contracts.V1;
using Payroll.Application.SalaryGrades.Projections;
using Payroll.EventSourcing.Serialization.YAML;
using Payroll.Application.BusinessYears;
using Payroll.Application.Users;

namespace Payroll.Test.UnitTest
{
  public abstract class TestBase : BootstrapInjectTestBase
  {
    protected ITypeMapper _typeMapper;
    protected IYAMLSerializer _serializer;
    protected IEventStore _eventStore;
    protected ICacheStore _cache;
    protected IProjectionManager _projections;
    protected IEncryptionProvider _enc;
    protected IPayrollApplicationServices _app;
    protected IUserAppService _auth;
    protected IAccessTokenProvider _tokenProvider;
    protected Guid _rootId;
    protected string _accessTokenStub;

    public TestBase()
    {
      _typeMapper = _container.Resolve<ITypeMapper>();
      _serializer = _container.Resolve<IYAMLSerializer>();
      _eventStore = _container.Resolve<IEventStore>();
      _cache = _container.Resolve<ICacheStore>();
      _projections = _container.Resolve<IProjectionManager>();
      _enc = _container.Resolve<IEncryptionProvider>();
      _app = _container.Resolve<IPayrollApplicationServices>();
      _auth = _container.Resolve<IUserAppService>();
      _tokenProvider = _container.Resolve<IAccessTokenProvider>();
      
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

      _auth.Handle(new PasswordLogin {
        Username = "test",
        Password = "p4ssw0d"
      }, result => _accessTokenStub = result);
    }

    public new void Dispose() {
      base.Dispose();
      _typeMapper = null;
      _eventStore = null;
      _cache = null;
      _projections = null;
      _enc = null;
      _app = null;
      _auth = null;
    }
  }
}