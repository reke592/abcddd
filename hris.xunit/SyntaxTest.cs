using System;
using Xunit;

namespace hris.xunit
{
  public class SyntaxTest
  {
    public class v1 { }
    public class v2 : v1 { }

    [Fact]
    public void SwitchTestInheritance()
    {
      switch(new v2())
      {
        case v1 x:
          Assert.True(true);
          break;
        default:
          Assert.True(false);
          break;
      }
    }

  }
}