using MongoDB.Driver;

namespace MongoDB.CSharpDriver.Extensions.Templating.Tests.Utilities;

public abstract class DatabaseFixtureBase
{
    private const string MongoServer = "localhost:27017";

    private static readonly MongoClientSettings __mongoClientSettings = new() {Server = MongoServerAddress.Parse(MongoServer)};
    private static readonly MongoClient __mongoClient = new(__mongoClientSettings);

    private readonly IMongoDatabase _database;

    protected DatabaseFixtureBase(string databaseName)
    {
        _database = MongoClient.GetDatabase(databaseName);
    }
    
    public IMongoClient MongoClient => __mongoClient;

    public IMongoDatabase MongoDatabase => _database;
}
