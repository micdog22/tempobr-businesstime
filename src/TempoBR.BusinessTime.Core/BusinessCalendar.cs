
namespace TempoBR.BusinessTime.Core;

public class BusinessCalendar
{
    private readonly string? _uf;
    private readonly string? _city;
    private readonly bool _includeBlack;

    public BusinessCalendar(string? uf = null, string? city = null, bool includeBlackConsciousness = false)
    {
        _uf = uf;
        _city = city;
        _includeBlack = includeBlackConsciousness;
    }

    public bool IsHoliday(DateOnly date)
    {
        var holidays = HolidayCalculator.AllHolidays(date.Year, _uf, _city, _includeBlack);
        return holidays.Any(h => h.Date == date);
    }

    public bool IsWeekend(DateOnly date) => date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    public bool IsBusinessDay(DateOnly date) => !IsWeekend(date) && !IsHoliday(date);

    public DateOnly NextBusinessDay(DateOnly date)
    {
        var d = date.AddDays(1);
        while (!IsBusinessDay(d)) d = d.AddDays(1);
        return d;
    }

    public DateOnly AddBusinessDays(DateOnly start, int days)
    {
        if (days == 0) return start;
        int step = days > 0 ? 1 : -1;
        int remaining = Math.Abs(days);
        var d = start;
        while (remaining > 0)
        {
            d = d.AddDays(step);
            if (IsBusinessDay(d)) remaining--;
        }
        return d;
    }

    public DateTime AddBusinessHours(DateTime start, double hours, int workStartHour = 9, int workEndHour = 18)
    {
        if (hours == 0) return start;
        if (workEndHour <= workStartHour) throw new ArgumentException("workEndHour deve ser > workStartHour");

        var current = start;
        double remaining = Math.Abs(hours);
        int dir = hours >= 0 ? 1 : -1;

        while (remaining > 0)
        {
            var date = DateOnly.FromDateTime(current);
            if (!IsBusinessDay(date))
            {
                var next = dir > 0 ? NextBusinessDay(date) : PrevBusinessDay(date);
                current = new DateTime(next.Year, next.Month, next.Day,
                    dir > 0 ? workStartHour : workEndHour, 0, 0, current.Kind);
                continue;
            }

            var workStart = new DateTime(date.Year, date.Month, date.Day, workStartHour, 0, 0, current.Kind);
            var workEnd = new DateTime(date.Year, date.Month, date.Day, workEndHour, 0, 0, current.Kind);

            if (dir > 0)
            {
                if (current < workStart) current = workStart;
                if (current >= workEnd)
                {
                    var next = NextBusinessDay(date);
                    current = new DateTime(next.Year, next.Month, next.Day, workStartHour, 0, 0, current.Kind);
                    continue;
                }

                var available = (workEnd - current).TotalHours;
                if (remaining <= available)
                {
                    return current.AddHours(remaining);
                }
                else
                {
                    remaining -= available;
                    var next = NextBusinessDay(date);
                    current = new DateTime(next.Year, next.Month, next.Day, workStartHour, 0, 0, current.Kind);
                }
            }
            else
            {
                if (current > workEnd) current = workEnd;
                if (current <= workStart)
                {
                    var prev = PrevBusinessDay(date);
                    current = new DateTime(prev.Year, prev.Month, prev.Day, workEndHour, 0, 0, current.Kind);
                    continue;
                }

                var available = (current - workStart).TotalHours;
                if (remaining <= available)
                {
                    return current.AddHours(-remaining);
                }
                else
                {
                    remaining -= available;
                    var prev = PrevBusinessDay(date);
                    current = new DateTime(prev.Year, prev.Month, prev.Day, workEndHour, 0, 0, current.Kind);
                }
            }
        }

        return current;
    }

    private DateOnly PrevBusinessDay(DateOnly date)
    {
        var d = date.AddDays(-1);
        while (!IsBusinessDay(d)) d = d.AddDays(-1);
        return d;
    }
}
