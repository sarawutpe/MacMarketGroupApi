using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MacMarketGroupApi.Models;

public class HttpException : Exception
{
    public int StatusCode { get; } = 500;

    public HttpException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}

public class Response
{
    [BsonElement("success")]
    public bool Success { get; set; }

    [BsonElement("data")]
    [BsonIgnoreIfNull]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Data { get; set; }

    [BsonElement("error")]
    [BsonIgnoreIfNull]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Error { get; set; }
}

