using MongoDB.Driver;
using Xunit;

namespace MongoDB.CSharpDriver.Extensions.Templating.Tests.Utilities;

[Trait("Category", "Integration")]
public abstract class IntegrationTestBase<TFixture> : IClassFixture<TFixture>
    where TFixture: DatabaseFixtureBase
{
    protected IntegrationTestBase(TFixture fixture)
    {
        Fixture = fixture;
    }

    protected TFixture Fixture { get; }

    protected IMongoCollection<T> GetCollection<T>(string name)
        => Fixture.MongoDatabase.GetCollection<T>(name);
}

public abstract class IntegrationTestBase: IntegrationTestBase<TemporaryDatabaseFixture>
{
    protected IntegrationTestBase(TemporaryDatabaseFixture fixture)
        : base(fixture)
    {}
}