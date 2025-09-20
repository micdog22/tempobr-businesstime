
using Microsoft.AspNetCore.Mvc;
using TempoBR.BusinessTime.Core;

namespace TempoBR.BusinessTime.Api.Controllers;

[ApiController]
[Route("api/business-hour")]
public class BusinessHourController : ControllerBase
{
    [HttpPost("add")]
    public ActionResult<BusinessHourAddResponse> Add([FromBody] BusinessHourAddRequest req)
    {
        var cal = new BusinessCalendar(req.Uf, req.City, req.IncludeBlackConsciousness);
        var end = cal.AddBusinessHours(req.Start, req.Hours, req.WorkStartHour, req.WorkEndHour);
        return Ok(new BusinessHourAddResponse(end));
    }
}
