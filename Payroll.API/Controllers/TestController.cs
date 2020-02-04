using Microsoft.AspNetCore.Mvc;
using Payroll.Application;
using Payroll.EventSourcing;
using static Payroll.Application.BusinessYears.Contracts.V1;
using static Payroll.Application.BusinessYears.Projections.CurrentBusinessYearProjection;
using static Payroll.Application.Users.Contracts.V1;

namespace Payroll.API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class TestController : ControllerBase
  {
    private readonly IPayrollApplicationServices _app;
    private readonly ICacheStore _cache;
    private readonly IEventStore _store;

    public TestController(IEventStore store, IPayrollApplicationServices app, ICacheStore cache)
    {
      _app = app;
      _cache = cache;
      _store = store;
    }

    [HttpPost("/business/year/new")]
    public IActionResult CreateBusinessYear([FromHeader] string AccessToken, [FromBody] CreateBusinessYear request)
    {
      object result;
      request.AccessToken = AccessToken;
      _app.BusinessYear.Handle(request);
      if(_cache.GetRecent<CurrentBusinessYearRecord>(out var record))
      {
        result = record;
      }
      else
      {
        result = "no record for current business year";
      }
      return new JsonResult(result);
    }

    [HttpPost("/login")]
    public IActionResult PasswordLogin([FromBody] PasswordLogin request)
    {
      return new JsonResult(_app.User.Handle(request));
    }

    [HttpGet("/business/year")]
    public IActionResult Get()
    {
      object result;
      if(_cache.GetRecent<CurrentBusinessYearRecord>(out var record))
      {
        result = record;
      }
      else
      {
        result = "no record for current business year";
      }
      return new JsonResult(result);
    }
  }
}