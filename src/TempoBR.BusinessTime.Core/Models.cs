
namespace TempoBR.BusinessTime.Core;

public record Holiday(string Name, DateOnly Date, string Scope = "Nacional");

public record BusinessHourAddRequest(
    DateTime Start,
    double Hours,
    int WorkStartHour = 9,
    int WorkEndHour = 18,
    string? Uf = null,
    string? City = null,
    bool IncludeBlackConsciousness = false
);

public record BusinessHourAddResponse(DateTime End);
