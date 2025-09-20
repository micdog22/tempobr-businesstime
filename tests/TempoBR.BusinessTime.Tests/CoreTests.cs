
using TempoBR.BusinessTime.Core;
using Xunit;
using FluentAssertions;

namespace TempoBR.BusinessTime.Tests;

public class CoreTests
{
    [Fact]
    public void NextBusinessDay_Should_Skip_Weekend_And_Holidays()
    {
        // Exemplo: Tiradentes 2025-04-21 é segunda, feriado nacional
        var cal = new BusinessCalendar();
        var next = cal.NextBusinessDay(new DateOnly(2025, 4, 21));
        next.Should().Be(new DateOnly(2025, 4, 22));
    }

    [Fact]
    public void AddBusinessHours_Should_Skip_Outside_Working_Range_And_Weekend()
    {
        var cal = new BusinessCalendar();
        var start = new DateTime(2025, 9, 19, 16, 30, 0); // sexta 16:30
        var end = cal.AddBusinessHours(start, 4.5, 9, 18); // 1.5h sexta + 3h segunda = 12:00 de segunda
        end.Should().Be(new DateTime(2025, 9, 22, 12, 0, 0));
    }
}
