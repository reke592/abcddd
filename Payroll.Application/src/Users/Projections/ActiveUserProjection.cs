using System.Collections.Generic;
using Payroll.Domain.Users;

namespace Payroll.Application.Users.Projections
{
  public class ActiveUserRecord
  {
    public UserId Id { get; set; }
    public string Username { get; set; }
    public ISet<string> Claims { get; set; } = new HashSet<string>();
  }

  public class ActiveUserProjection
  {

  }
}