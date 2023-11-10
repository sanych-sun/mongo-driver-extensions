using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using MongoDB.Bson;

namespace MongoDB.CSharpDriver.Extensions.Templating;

internal static class ParametersHelper
{
    private static readonly Regex __valueReplaceRegex = new Regex(@"(?<qt>[""'])(?:\\.|[^\\])*?\k<qt>|@(?<param>\w*)");
    
    public static BsonDocument ToBsonDocument(string template, object parameters = null)
    {
        if (parameters == null)
        {
            return BsonDocument.Parse(template);
        }
        
        var document = BsonDocument.Parse(PreProcessTemplate(template));
        BindParameters(document, parameters);
        
        return document;
    }

    public static BsonArray ToBsonArray(string template, object parameters = null)
    {
        template = $"{{ '_v': {template} }}";

        var doc = ToBsonDocument(template, parameters);
        return (BsonArray)doc["_v"];
    }

    private static string PreProcessTemplate(string template)
        => __valueReplaceRegex.Replace(template, m =>
        {
            if (!string.IsNullOrEmpty(m.Groups["qt"]?.Value))
            {
                return m.Value;
            }

            if (!string.IsNullOrEmpty(m.Groups["param"]?.Value))
            {
                return $"'__param:{m.Groups["param"].Value}'";
            }
            
            return m.Value;
        });

    private static BsonValue BindParameters(BsonValue bsonValue, object parameters)
    {
        switch (bsonValue)
        {
            case BsonDocument bsonDocument:
                var names = bsonDocument.Names.ToArray();
                foreach (var elementName in names)
                {
                    bsonDocument[elementName] = BindParameters(bsonDocument[elementName], parameters);
                }

                return bsonDocument;
            case BsonArray bsonArray:
                for (var i = 0; i < bsonArray.Count; i++)
                {
                    bsonArray[i] = BindParameters(bsonArray[i], parameters);
                }
                break;
            case BsonString bsonString:
                if (!bsonString.Value.StartsWith("__param:"))
                {
                    return bsonValue;
                }
                
                var param = bsonString.Value.Remove(0, "__param:".Length);
                var type = parameters.GetType();
                var property = type.GetProperty(param, BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                {
                    throw new KeyNotFoundException($"Cannot bind to parameter '{param}': property not found");
                }

                var value = property.GetValue(parameters);
                if(!BsonTypeMapper.TryMapToBsonValue(value, out bsonValue))
                {
                    throw new KeyNotFoundException($"Cannot bind to parameter '{param}': cannot convert value to BsonValue");
                }

                return bsonValue;
        }
        
        return bsonValue;
    }
}