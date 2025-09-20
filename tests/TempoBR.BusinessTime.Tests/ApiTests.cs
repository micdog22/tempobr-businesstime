
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using TempoBR.BusinessTime.Core;
using Xunit;
using FluentAssertions;

namespace TempoBR.BusinessTime.Tests;

public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public ApiTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task Holidays_Should_Return_List()
    {
        var client = _factory.CreateClient();
        var res = await client.GetAsync("/api/holidays/2025");
        res.EnsureSuccessStatusCode();
        var items = await res.Content.ReadFromJsonAsync<List<Holiday>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
    }

    [Fact]
    public async Task BusinessHour_Add_Should_Return_End()
    {
        var client = _factory.CreateClient();
        var req = new BusinessHourAddRequest(new DateTime(2025,9,19,16,30,0), 4.5, 9, 18, null, null, false);
        var res = await client.PostAsJsonAsync("/api/business-hour/add", req);
        res.EnsureSuccessStatusCode();
        var body = await res.Content.ReadFromJsonAsync<BusinessHourAddResponse>();
        body.Should().NotBeNull();
        body!.End.Should().Be(new DateTime(2025, 9, 22, 12, 0, 0));
    }
}
