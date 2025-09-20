
using System.Text;

namespace TempoBR.BusinessTime.Core;

public static class IcsExporter
{
    public static string HolidaysToIcs(IEnumerable<Holiday> holidays, string calendarName = "Feriados Brasil")
    {
        // Gera um calendário ICS simples com eventos de dia inteiro para cada feriado.
        var sb = new StringBuilder();
        sb.AppendLine("BEGIN:VCALENDAR");
        sb.AppendLine("VERSION:2.0");
        sb.AppendLine("PRODID:-//TempoBR//BusinessTime//PT-BR");
        sb.AppendLine($"X-WR-CALNAME:{calendarName}");

        var now = DateTime.UtcNow.ToString("yyyyMMdd'T'HHmmss'Z'");

        foreach (var h in holidays)
        {
            string dt = h.Date.ToString("yyyyMMdd");
            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine($"UID:{Guid.NewGuid()}@tempobr");
            sb.AppendLine($"DTSTAMP:{now}");
            sb.AppendLine($"DTSTART;VALUE=DATE:{dt}");
            sb.AppendLine($"DTEND;VALUE=DATE:{h.Date.AddDays(1):yyyyMMdd}");
            sb.AppendLine($"SUMMARY:{Escape(h.Name)}");
            sb.AppendLine("END:VEVENT");
        }

        sb.AppendLine("END:VCALENDAR");
        return sb.ToString();
    }

    private static string Escape(string s) => s.Replace(",", "\,").Replace(";", "\;").Replace("\", "\\");
}
