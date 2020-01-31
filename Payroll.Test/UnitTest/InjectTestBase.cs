using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Payroll.Application;
using Payroll.EventSourcing;
using Payroll.Infrastructure;
using Payroll.Application.BusinessYears;
using Payroll.Application.Deductions;
using Payroll.Application.Employees;
using Payroll.Application.PayrollPeriods;
using Payroll.Application.SalaryGrades;
using Payroll.Application.Users;
using Payroll.EventSourcing.Serialization.YAML;

namespace Payroll.Test.UnitTest
{
  public abstract class InjectTestBase : IDisposable
  {
    protected IWindsorContainer _container;

    public InjectTestBase()
    {
      var container = new WindsorContainer();

      container.Register(Component.For<ITypeMapper>()
        .ImplementedBy<TypeMapper>()
        .LifestyleSingleton());
      
      container.Register(Component.For<IYAMLSerializer>()
        .ImplementedBy<YAMLSerializer>()
        .LifestyleSingleton());
      
      container.Register(Component.For<ISerializer>()
        .ImplementedBy<Payroll.Test.UnitTest.Impl.JSONSerializer>()
        .LifestyleSingleton());

      container.Register(Component.For<IEventStore>()
        .ImplementedBy<Payroll.Test.UnitTest.Impl.UseEventStore>()
        .LifestyleSingleton());
      // container.Register(Component.For<IEventStore>()
      //   .ImplementedBy<MemoryEventStore>()
      //   .LifestyleSingleton());

      container.Register(Component.For<IEncryptionProvider>()
        .ImplementedBy<BCryptEncryptionProvider>()
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
      
      _container = container;
    }

    public void Dispose() {
      _container.Dispose();
      _container = null;
    }
  }
}