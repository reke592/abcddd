using Castle.Windsor;

namespace Payroll.Test.UnitTest
{
  public class TestContainer : BootstrapInjectTestBase
  {
    public IWindsorContainer GetContainer => _container;
  }
}