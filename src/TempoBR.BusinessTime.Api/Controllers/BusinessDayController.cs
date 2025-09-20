
using Microsoft.AspNetCore.Mvc;
using TempoBR.BusinessTime.Core;

namespace TempoBR.BusinessTime.Api.Controllers;

[ApiController]
[Route("api/business-day")]
public class BusinessDayController : ControllerBase
{
    [HttpGet("next")]
    public ActionResult<object> Next([FromQuery] DateOnly date, [FromQuery] string? uf = null, [FromQuery] string? city = null, [FromQuery] bool includeBlackConsciousness = false)
    {
        var cal = new BusinessCalendar(uf, city, includeBlackConsciousness);
        var next = cal.NextBusinessDay(date);
        return Ok(new { date = next });
    }

    [HttpGet("add")]
    public ActionResult<object> Add([FromQuery] DateOnly start, [FromQuery] int days, [FromQuery] string? uf = null, [FromQuery] string? city = null, [FromQuery] bool includeBlackConsciousness = false)
    {
        var cal = new BusinessCalendar(uf, city, includeBlackConsciousness);
        var end = cal.AddBusinessDays(start, days);
        return Ok(new { end });
    }
}
