using System.Linq;
using System.Threading.Tasks;
using MongoDB.CSharpDriver.Extensions.Templating.Tests.Utilities;
using MongoDB.Driver;
using Xunit;

namespace MongoDB.CSharpDriver.Extensions.Templating.Tests;

public class FindAsyncExtensionTests : IntegrationTestBase<SampleGuidesFixture>
{
    public FindAsyncExtensionTests(SampleGuidesFixture fixture)
        : base(fixture)
    {}

    [Theory]
    [InlineData(true, 4)]
    [InlineData(false, 4)]
    public async Task FindAsync_with_bool_parameter(bool hasRings, int expectedCount)
    {
        var collection = Fixture.PlanetCollection;

        var resultsCursor = await collection.FindAsync("{ HasRings : @hasRings }", new { hasRings });
        var resultsList = await resultsCursor.ToListAsync();
        
        Assert.Equal(expectedCount, resultsList.Count);
        Assert.All(resultsList, p => Assert.Equal(hasRings, p.HasRings));
    }
    
    [Theory]
    [InlineData(3, "Earth")]
    [InlineData(5, "Jupiter")]
    public async Task FindAsync_with_int_parameter(int orderNum, string expectedName)
    {
        var collection = Fixture.PlanetCollection;

        var resultsCursor = await collection.FindAsync("{ OrderFromSun : @orderNum }", new { orderNum });
        var resultsList = await resultsCursor.ToListAsync();
        
        Assert.Single(resultsList);
        Assert.Equal(expectedName, resultsList.Single().Name);
    }
    
    [Theory]
    [InlineData(3, 2)]
    public async Task FindAsync_lt_with_int_parameter(int orderNum, int expectedCount)
    {
        var collection = Fixture.PlanetCollection;

        var resultsCursor = await collection.FindAsync("{ OrderFromSun: { $lt: @orderNum } }", new { orderNum });
        var resultsList = await resultsCursor.ToListAsync();
        
        Assert.Equal(expectedCount, resultsList.Count);
    }
    
    [Fact]
    public async Task FindAsync_lt_with_array_parameter()
    {
        var collection = Fixture.PlanetCollection;

        var resultsCursor = await collection.FindAsync("{ $expr: { $in: ['$OrderFromSun', @orderNums] } }", new { orderNums = new[] {3, 6} });
        var resultsList = await resultsCursor.ToListAsync();
        
        Assert.Equal(2, resultsList.Count);
    }
    
    [Fact]
    public async Task FindAsync_lt_with_array_parameter2()
    {
        var collection = Fixture.PlanetCollection;

        var resultsCursor = await collection.FindAsync("{ $expr: { $in: ['$OrderFromSun', [@earthNum, @saturnNum]] } }", new { earthNum = 3, saturnNum = 6 });
        var resultsList = await resultsCursor.ToListAsync();
        
        Assert.Equal(2, resultsList.Count);
    }
}