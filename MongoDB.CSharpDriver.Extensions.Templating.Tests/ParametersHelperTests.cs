using System.Collections.Generic;
using MongoDB.Bson;
using Xunit;

namespace MongoDB.CSharpDriver.Extensions.Templating.Tests;

public class ParametersHelperTests
{
    [Theory]
    [MemberData(nameof(TestCases))]
    public void ToBsonDocumentTests(string source, object parameters, string expected)
    {
        var result = ParametersHelper.ToBsonDocument(source, parameters);
            
        Assert.NotNull(result);
        var expectedDocument = BsonDocument.Parse(expected);
        Assert.Equivalent(expectedDocument, result);
    }
        
    public static IEnumerable<object[]> TestCases => new[]
    {
        new object[] { "{a:12}", null, "{a:12}"},
        new object[] { "{a:@p1}", new { p1 = 12 }, "{a:12}"},
        new object[] { "{a:\"@p1\"}", new { p1 = 12 }, "{a:'@p1'}"},
        new object[] { "{a:'@p1'}", new { p1 = 12 }, "{a:'@p1'}"},
        new object[] { "{a:'test@p1.com'}", new { p1 = 12 }, "{a:'test@p1.com'}"},
        new object[] { "{a:[@p1]}", new { p1 = 12 }, "{a:[12]}"},
        new object[] { "{a:[@p1, 10]}", new { p1 = 12 }, "{a:[12,10]}"},
        new object[] { "{a:[10, @p1]}", new { p1 = 12 }, "{a:[10,12]}"},
        new object[] { "{a:[10, @p1, '@text']}", new { p1 = 12 }, "{a:[10,12,'@text']}"},
        new object[] { "{a:[10, @p1, 'some@text']}", new { p1 = 12 }, "{a:[10,12,'some@text']}"},
        new object[] { "{a:@str1}", new { str1 = "my string" }, "{a:'my string'}"},
        new object[] { "{a:@p1, b: 4, c: 'test', d: @str1}", new { p1 = 12, str1 = "my string" }, "{a:12,b:4,c:'test',d:'my string'}"},
        new object[] { "{$addFields: { name: @FirstName } }", new {FirstName = "John"}, "{$addFields: { name: 'John' } }"},
        new object[] { "{$addFields: { tags: @Tags } }", new {Tags = new[] {"a", "b"}}, "{$addFields: { tags: ['a', 'b'] } }"},
    };
}