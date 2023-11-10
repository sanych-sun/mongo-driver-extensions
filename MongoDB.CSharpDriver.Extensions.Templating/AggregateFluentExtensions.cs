using MongoDB.Driver;
using MongoDB.Driver.Core.Misc;

namespace MongoDB.CSharpDriver.Extensions.Templating;

public static class AggregateFluentExtensions
{
    /// <summary>
    /// Appends the stage to the pipeline.
    /// </summary>
    /// <typeparam name="TResult">The type of the result of the pipeline.</typeparam>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="stage">The stage template.</param>
    /// <param name="parameters">Parameters object to use in the stage template</param>
    /// <returns>The fluent aggregate interface.</returns>
    public static IAggregateFluent<TResult> AppendStage<TResult>(
        this IAggregateFluent<TResult> aggregate,
        string stage,
        object parameters = null)
        => aggregate.AppendStage<TResult, TResult>(stage, parameters);

    /// <summary>
    /// Appends the stage to the pipeline.
    /// </summary>
    /// <typeparam name="TResult">The type of the result of the pipeline.</typeparam>
    /// <typeparam name="TNewResult">The type of the result of the stage.</typeparam>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="stage">The stage template.</param>
    /// <param name="parameters">Parameters object to use in the stage template</param>
    /// <returns>The fluent aggregate interface.</returns>
    public static IAggregateFluent<TNewResult> AppendStage<TResult, TNewResult>(
        this IAggregateFluent<TResult> aggregate,
        string stage,
        object parameters = null)
    {
        Ensure.IsNotNull(aggregate, nameof(aggregate));
        Ensure.IsNotNullOrEmpty(stage, nameof(stage));

        var stageDoc = ParametersHelper.ToBsonDocument(stage, parameters);

        return aggregate.AppendStage(new BsonDocumentPipelineStageDefinition<TResult, TNewResult>(stageDoc));
    }
}