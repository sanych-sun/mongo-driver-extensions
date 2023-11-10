using System;
using System.Runtime.CompilerServices;
using System.Threading;
using MongoDB.Driver;

namespace MongoDB.CSharpDriver.Extensions.Templating.Tests.Utilities;

public class TemporaryDatabaseFixture: DatabaseFixtureBase, IDisposable
{
    public const string TestDatabasePrefix = "MongoDBExtensions-";

    private static readonly string __timeStamp = DateTime.Now.ToString("s").Replace(':', '-');
    private static int __count;

    public TemporaryDatabaseFixture()
        : base($"{TestDatabasePrefix}{__timeStamp}-{Interlocked.Increment(ref __count)}")
    {
    }

    public void Dispose()
    {
        MongoClient.DropDatabase(MongoDatabase.DatabaseNamespace.DatabaseName);
    }
    
    public IMongoCollection<T> CreateTemporaryCollection<T>([CallerMemberName] string? name = null)
    {
        MongoDatabase.CreateCollection(name);
        return MongoDatabase.GetCollection<T>(name);
    }
}