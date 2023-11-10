using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDB.CSharpDriver.Extensions.Templating.Tests.Utilities;

public class SampleGuidesFixture : TemporaryDatabaseFixture
{
    private readonly IMongoCollection<Planet> _planetCollection;
    
    public SampleGuidesFixture()
    {
        _planetCollection = MongoDatabase.GetCollection<Planet>("planets");
        _planetCollection.BulkWrite(__planetData.Select(p => new InsertOneModel<Planet>(p)));
    }

    public IMongoCollection<Planet> PlanetCollection => _planetCollection;
    
    private static readonly Planet[] __planetData =
    {
        new()
        {
            Id = ObjectId.Parse("621ff30d2a3e781873fcb65c"),
            Name = "Mercury",
            OrderFromSun = 1,
            HasRings = false,
            MainAtmosphere = Array.Empty<string>()
        },
        new()
        {
            Id = ObjectId.Parse("621ff30d2a3e781873fcb662"),
            Name = "Venus",
            OrderFromSun = 2,
            HasRings = false,
            MainAtmosphere = new[] {"CO2", "N"}
        },
        new()
        {
            Id = ObjectId.Parse("621ff30d2a3e781873fcb661"),
            Name = "Earth",
            OrderFromSun = 3,
            HasRings = false,
            MainAtmosphere = new[] {"N", "O2", "Ar"}
        },
        new()
        {
            Id = ObjectId.Parse("621ff30d2a3e781873fcb65e"),
            Name = "Mars",
            OrderFromSun = 4,
            HasRings = false,
            MainAtmosphere = new[] {"CO2", "Ar", "N"}
        },
        new()
        {
            Id = ObjectId.Parse("621ff30d2a3e781873fcb660"),
            Name = "Jupiter",
            OrderFromSun = 5,
            HasRings = true,
            MainAtmosphere = new[] {"H2", "He", "CH4"}
        },
        new()
        {
            Id = ObjectId.Parse("621ff30d2a3e781873fcb663"),
            Name = "Saturn",
            OrderFromSun = 6,
            HasRings = true,
            MainAtmosphere = new[] {"H2", "He", "CH4"}
        },
        new()
        {
            Id = ObjectId.Parse("621ff30d2a3e781873fcb65d"),
            Name = "Uranus",
            OrderFromSun = 7,
            HasRings = true,
            MainAtmosphere = new[] {"H2", "He", "CH4"}
        },
        new()
        {
            Id = ObjectId.Parse("621ff30d2a3e781873fcb65f"),
            Name = "Neptune",
            OrderFromSun = 8,
            HasRings = true,
            MainAtmosphere = new[] {"H2", "He", "CH4"}
        }
    };
}

public class Planet
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public int OrderFromSun { get; set; }
    public bool HasRings { get; set; }
    public string[] MainAtmosphere { get; set; }
}