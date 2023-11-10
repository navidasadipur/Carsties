using System.Net;
using System.Net.Http.Json;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.IntegrationTests.Fixtures;
using AuctionService.IntegrationTests.Util;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionService.IntegrationTests;

[Collection("Shared collection")]
public class AuctionControllerTests : IAsyncLifetime
{
    private readonly CustomWebAppFactory _factory;
    private readonly HttpClient _httpClient;
    private const string _gT_ID = "afbee524-5972-4075-8800-7d1f9d7b0a0c";

    public AuctionControllerTests(CustomWebAppFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task GetAuctions_ShouldReturn3Auctions()
    {
        //arrang?

        //act
        var response = await _httpClient.GetFromJsonAsync<List<AuctionDto>>("api/auctions");

        //assert
        Assert.Equal(3, response.Count);
    }

    [Fact]
    public async Task GetAuctionById_WithValidId_ShouldReturnAuction()
    {
        //arrang?

        //act
        var response = await _httpClient.GetFromJsonAsync<AuctionDto>($"api/auctions/{_gT_ID}");

        //assert
        Assert.Equal("GT", response.Model);
    }

    [Fact]
    public async Task GetAuctionById_WithInvalidId_ShouldReturn404()
    {
        //arrang?

        //act
        var response = await _httpClient.GetAsync($"api/auctions/{Guid.NewGuid()}");

        //assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetAuctionById_WithInvalidGuid_ShouldReturn400()
    {
        //arrang?

        //act
        var response = await _httpClient.GetAsync($"api/auctions/notAGuid");

        //assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateAuction_WithNoAuth_ShouldReturn401()
    {
        //arrang?
        var auction = new CreateAuctionDto{Make = "test"};

        //act
        var response = await _httpClient.PostAsJsonAsync($"api/auctions", auction);

        //assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateAuction_WithAuth_ShouldReturn201()
    {
        //arrang?
        var auction = GetAuctionForCreate();
        _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

        //act
        var response = await _httpClient.PostAsJsonAsync($"api/auctions", auction);

        //assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdAuction = await response.Content.ReadFromJsonAsync<AuctionDto>();
        Assert.Equal("bob", createdAuction.Seller);
    }

    [Fact]
    public async Task CreateAuction_WithInvalidCreateAuctionDto_ShouldReturn400()
    {
        //arrang?
        var auction = GetAuctionForCreate();
        auction.Make = null;
        _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

        //act
        var response = await _httpClient.PostAsJsonAsync($"api/auctions", auction);

        //assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateAuction_WithValidUpdateDtoAndUser_ShouldReturn200()
    {
        //arrang
        var auction = new UpdateAuctionDto{Make = "updated"};
        _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

        //act
        var response = await _httpClient.PutAsJsonAsync($"api/auctions/{_gT_ID}", auction);

        //assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updatedAuction = await _httpClient.GetFromJsonAsync<AuctionDto>($"api/auctions/{_gT_ID}");
        Assert.Equal("bob", updatedAuction.Seller);
        Assert.Equal(auction.Make, updatedAuction.Make);
    }

    [Fact]
    public async Task UpdateAuction_WithValidUpdateDtoAndInvalidUser_ShouldReturn403()
    {
        // arrange 
        var auction = new UpdateAuctionDto{Make = "updated"};
        _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("alice"));

        //act
        var response = await _httpClient.PutAsJsonAsync($"api/auctions/{_gT_ID}", auction);

        //assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
        DbHelper.ReinitDbForTests(db);
        return Task.CompletedTask;
    }

    private static CreateAuctionDto GetAuctionForCreate()
    {
        return new CreateAuctionDto
        {
            Make = "test",
            Model = "testModel",
            ImageUrl = "test",
            Color = "test",
            Mileage = 10,
            Year = 10,
            ReservePrice = 10,
        };
    }
}
