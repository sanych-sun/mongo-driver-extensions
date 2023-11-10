using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.CSharpDriver.Extensions.Templating.Tests.Utilities;
using MongoDB.Driver;
using Xunit;

namespace MongoDB.CSharpDriver.Extensions.Templating.Tests;

public class AggregateAsyncExtensionTests : IntegrationTestBase<SampleGuidesFixture>
{
    public AggregateAsyncExtensionTests(SampleGuidesFixture fixture)
        : base(fixture)
    {}

    [Theory]
    [MemberData(nameof(AggregateTestCases))]
    public async Task AggregateMatchAsync(string aggregate, object parameters, int expectedCount)
    {
        var collection = Fixture.PlanetCollection;

        var resultsCursor = await collection.AggregateAsync(aggregate, parameters);
        var resultsList = await resultsCursor.ToListAsync();
        
        Assert.Equal(expectedCount, resultsList.Count);
    }

    public static IEnumerable<object[]> AggregateTestCases()
    {
        yield return new object[] { "[{ $match: { HasRings: @hasRings } }]", new { hasRings = true }, 4 };

        yield return new object[] { "[{ $match: { OrderFromSun: { $lt: @orderNum } } }]", new { orderNum = 3 }, 2 };
    }
}