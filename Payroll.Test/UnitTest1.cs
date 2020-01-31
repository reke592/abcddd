using System;
using Payroll.Domain.Users;
using Xunit;

namespace Payroll.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var x = Guid.NewGuid();
            UserId y = x;
            var r = y != x;
        }
    }
}
