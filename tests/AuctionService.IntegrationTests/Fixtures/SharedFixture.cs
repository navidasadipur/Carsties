using AuctionService.IntegrationTests.Fixtures;

namespace AuctionService.IntegrationTests;

[CollectionDefinition("Shared collection")]
public class SharedFixture : ICollectionFixture<CustomWebAppFactory>
{

}
