
using Microsoft.AspNetCore.Mvc;
using TempoBR.BusinessTime.Core;

namespace TempoBR.BusinessTime.Api.Controllers;

[ApiController]
[Route("api/holidays")]
public class HolidaysController : ControllerBase
{
    [HttpGet("{year:int}")]
    public ActionResult<IEnumerable<Holiday>> Get(int year, [FromQuery] string? uf = null, [FromQuery] string? city = null, [FromQuery] bool includeBlackConsciousness = false)
    {
        var list = HolidayCalculator.AllHolidays(year, uf, city, includeBlackConsciousness);
        return Ok(list);
    }
}
