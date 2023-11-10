using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.CSharpDriver.Extensions.Templating.Tests.Utilities;
using MongoDB.Driver;
using Xunit;

namespace MongoDB.CSharpDriver.Extensions.Templating.Tests;

public class AggregateFluentExtensionsTests : IntegrationTestBase<SampleGuidesFixture>
{
    public AggregateFluentExtensionsTests(SampleGuidesFixture fixture)
        : base(fixture)
    {}

    [Fact]
    public async Task AppendStageMultipleTests()
    {
        var collection = Fixture.PlanetCollection;
        var hasRings = true;
        var aggregateFluent = collection.Aggregate()
            .AppendStage("{ $match: { HasRings : @hasRings } }", new { hasRings })
            .AppendStage("{ $sort: { OrderFromSun : 1 } }")
            .AppendStage("{ $limit: 1 }");
        
        var result = await aggregateFluent.SingleAsync();
        
        Assert.Equal("Jupiter", result.Name);
    }
    
    [Fact]
    public async Task AppendStageCombineWithOtherTests()
    {
        var collection = Fixture.PlanetCollection;
        var aggregateFluent = collection.Aggregate()
            .AppendStage("{ $match: { HasRings : @hasRings } }", new { hasRings = true })
            .SortBy(p => p.OrderFromSun)
            .Limit(1);
        
        var result = await aggregateFluent.SingleAsync();
        
        Assert.Equal("Jupiter", result.Name);
    }
    
    [Theory]
    [MemberData(nameof(AppendStagesTestCases))]
    public async Task AppendStageTests(string stage, object parameters, int expectedCount)
    {
        var collection = Fixture.PlanetCollection;
        var aggregateFluent = collection.Aggregate()
            .AppendStage(stage, parameters);

        var results = await aggregateFluent.ToListAsync();
        
        Assert.Equal(expectedCount, results.Count);
    }
    

    public static IEnumerable<object[]> AppendStagesTestCases()
    {
        yield return new object[] { "{ $match: { HasRings: @hasRings } }", new { hasRings = true }, 4 };

        yield return new object[] { "{ $match: { OrderFromSun: { $lt: @orderNum } } }", new { orderNum = 3 }, 2 };
    }
}