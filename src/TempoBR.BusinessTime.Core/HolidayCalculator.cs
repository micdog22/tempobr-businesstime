
namespace TempoBR.BusinessTime.Core;

public static class HolidayCalculator
{
    // Algoritmo de Meeus/Jones/Butcher para Páscoa
    public static DateOnly EasterSunday(int year)
    {
        int a = year % 19;
        int b = year / 100;
        int c = year % 100;
        int d = b / 4;
        int e = b % 4;
        int f = (b + 8) / 25;
        int g = (b - f + 1) / 3;
        int h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int l = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 22 * l) / 451;
        int month = (h + l - 7 * m + 114) / 31;
        int day = ((h + l - 7 * m + 114) % 31) + 1;
        return new DateOnly(year, month, day);
    }

    public static List<Holiday> NationalHolidays(int year, bool includeBlackConsciousness = false)
    {
        var easter = EasterSunday(year);
        var carnivalMonday = easter.AddDays(-48);
        var carnivalTuesday = easter.AddDays(-47);
        var goodFriday = easter.AddDays(-2);
        var corpusChristi = easter.AddDays(60);

        var list = new List<Holiday>
        {
            new("Confraternização Universal", new DateOnly(year,1,1)),
            new("Carnaval (Segunda)", carnivalMonday),
            new("Carnaval (Terça)", carnivalTuesday),
            new("Sexta-feira Santa", goodFriday),
            new("Tiradentes", new DateOnly(year,4,21)),
            new("Dia do Trabalho", new DateOnly(year,5,1)),
            new("Corpus Christi", corpusChristi),
            new("Independência do Brasil", new DateOnly(year,9,7)),
            new("Nossa Senhora Aparecida", new DateOnly(year,10,12)),
            new("Finados", new DateOnly(year,11,2)),
            new("Proclamação da República", new DateOnly(year,11,15)),
            new("Natal", new DateOnly(year,12,25))
        };

        if (includeBlackConsciousness)
            list.Add(new("Dia da Consciência Negra", new DateOnly(year,11,20)));

        return list.OrderBy(h => h.Date).ToList();
    }

    public static IEnumerable<Holiday> AllHolidays(int year, string? uf = null, string? city = null, bool includeBlackConsciousness = false)
    {
        // Nesta versão: nacionais + opção de Consciência Negra.
        // Pontos de extensão: acrescentar feriados estaduais/municipais conforme UF/cidade.
        return NationalHolidays(year, includeBlackConsciousness);
    }
}
