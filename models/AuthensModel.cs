using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MacMarketGroupApi.Models;

public class Login
{
    [BsonElement("email")]
    public string Email { get; set; } = null!;

    [BsonElement("password")]
    public string Password { get; set; } = null!;
}

public class Options
{
    [BsonElement("id")]
    public string Id { get; set; } = null!;
}

public class RequestForgotPassword
{
    [BsonElement("email")]
    public string Email { get; set; } = null!;
}
