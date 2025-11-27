using Application.Commands.Donuts.Create;
using Application.Queries.Donuts.Dtos;
using Domain.Entities;
using Host.Abstractions;
using IntegrationTests.Abstractions;
using IntegrationTests.Services;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace IntegrationTests;

public class DonutsTests(CustomWebAppFactory factory) : BaseTest(factory)
{
    private const string BaseRoute = "api/donuts";
    private readonly List<int> _toDelete = [];

    [Fact]
    public async Task Create()
    {
        var command = new CreateDonutCommand
        {
            Name = "Frambuesa",
            Description = "La mejor dona de todas",
            Price = 19.99m
        };

        var client = Factory.CreateClient();
        var response = await client.PostAsJsonAsync(BaseRoute, command);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Response<int>>();
        Assert.NotNull(result);

        bool saved = await DbContext.Donuts
            .AnyAsync(x => x.Id == result.Value);

        Assert.True(saved, "La dona no fue guardada en la base de datos");
        _toDelete.Add(result.Value);
    }

    [Fact]
    public async Task GetList()
    {
        var donuts = new List<Donut>();

        for (int i = 0; i < 3; i++)
        {
            donuts.Add(new Donut
            {
                Name = $"Dona {i}",
                Description = "Dona de prueba",
                Price = 10 + i
            });
        }

        DbContext.Donuts.AddRange(donuts);
        await DbContext.SaveChangesAsync();
        _toDelete.AddRange(donuts.Select(x => x.Id));
        var client = Factory.CreateClient();
        var response = await client.GetAsync(BaseRoute);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Response<List<DonutDto>>>();
        Assert.NotNull(result?.Value);
        Assert.True(result.Value!.Count >= 3, "No se obtuvieron las donas esperadas");
    }

    protected override void Cleanup()
    {
        if (_toDelete.Count > 0)
        {
            DbContext.Donuts
                .Where(x => _toDelete.Contains(x.Id))
                .ExecuteDelete();
        }
    }
}
