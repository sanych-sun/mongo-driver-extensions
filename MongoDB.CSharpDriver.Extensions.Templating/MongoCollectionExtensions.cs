using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Misc;

namespace MongoDB.CSharpDriver.Extensions.Templating;

public static class MongoCollectionExtensions
{
    /// <summary>
    /// Finds the documents matching the filter.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    /// <param name="collection">The collection.</param>
    /// <param name="filter">The filter template string.</param>
    /// <param name="parameters">Parameters object to use in filter template</param>
    /// <param name="options">The options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task whose result is a cursor.</returns>
    public static Task<IAsyncCursor<TDocument>> FindAsync<TDocument>(
        this IMongoCollection<TDocument> collection,
        string filter,
        object? parameters,
        FindOptions<TDocument, TDocument> options = null,
        CancellationToken cancellationToken = default)
    {
        Ensure.IsNotNull(collection, nameof(collection));
        Ensure.IsNotNullOrEmpty(filter, nameof(filter));

        var filterDoc = ParametersHelper.ToBsonDocument(filter, parameters);

        return collection.FindAsync(new BsonDocumentFilterDefinition<TDocument>(filterDoc), options, cancellationToken);
    }

    /// <summary>
    /// Deletes multiple documents.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    /// <param name="collection">The collection.</param>
    /// <param name="filter">The filter template string.</param>
    /// <param name="parameters">Parameters object to use in filter template</param>
    /// <param name="options">The options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// The result of the delete operation.
    /// </returns>
    public static Task<DeleteResult> DeleteManyAsync<TDocument>(
        this IMongoCollection<TDocument> collection,
        string filter,
        object? parameters,
        DeleteOptions options = null,
        CancellationToken cancellationToken = default)
    {
        Ensure.IsNotNull(collection, nameof(collection));
        Ensure.IsNotNullOrEmpty(filter, nameof(filter));

        var filterDoc = ParametersHelper.ToBsonDocument(filter, parameters);

        return collection.DeleteManyAsync(new BsonDocumentFilterDefinition<TDocument>(filterDoc), options, cancellationToken);
    }


    /// <summary>
    /// Runs an aggregation pipeline.
    /// </summary>
    /// <param name="collection">The collection.</param>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    /// <param name="pipeline">The pipeline template.</param>
    /// <param name="parameters">Parameters object to use in pipeline template</param>
    /// <param name="options">The options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A cursor.</returns>
    public static Task<IAsyncCursor<TDocument>> AggregateAsync<TDocument>(
        this IMongoCollection<TDocument> collection,
        string pipeline,
        object? parameters,
        AggregateOptions options = null,
        CancellationToken cancellationToken = default)
        => collection.AggregateAsync<TDocument, TDocument>(pipeline, parameters, options, cancellationToken);
    
    /// <summary>
    /// Runs an aggregation pipeline.
    /// </summary>
    /// <param name="collection">The collection.</param>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="pipeline">The pipeline template.</param>
    /// <param name="parameters">Parameters object to use in pipeline template</param>
    /// <param name="options">The options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A cursor.</returns>
    public static Task<IAsyncCursor<TResult>> AggregateAsync<TDocument, TResult>(
        this IMongoCollection<TDocument> collection,
        string pipeline,
        object? parameters,
        AggregateOptions options = null,
        CancellationToken cancellationToken = default)
    {
        Ensure.IsNotNull(collection, nameof(collection));
        Ensure.IsNotNullOrEmpty(pipeline, nameof(pipeline));

        var stages = ParametersHelper.ToBsonArray(pipeline, parameters).Select(e => (BsonDocument)e);
        return collection.AggregateAsync(new BsonDocumentStagePipelineDefinition<TDocument, TResult>(stages), options, cancellationToken);
    }
}