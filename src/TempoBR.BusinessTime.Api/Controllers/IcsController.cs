
using Microsoft.AspNetCore.Mvc;
using System.Text;
using TempoBR.BusinessTime.Core;

namespace TempoBR.BusinessTime.Api.Controllers;

[ApiController]
[Route("api/ics")]
public class IcsController : ControllerBase
{
    [HttpGet("holidays/{year:int}")]
    public IActionResult HolidaysIcs(int year, [FromQuery] string? uf = null, [FromQuery] string? city = null, [FromQuery] bool includeBlackConsciousness = false)
    {
        var list = HolidayCalculator.AllHolidays(year, uf, city, includeBlackConsciousness);
        var ics = IcsExporter.HolidaysToIcs(list, $"Feriados Brasil {year}");
        var bytes = Encoding.UTF8.GetBytes(ics);
        return File(bytes, "text/calendar", $"feriados-brasil-{year}.ics");
    }
}
