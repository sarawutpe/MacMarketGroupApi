using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MacMarketGroupApi.Models;

public class Login
{
    [BsonElement("username")]
    public string Email { get; set; } = null!;

    [BsonElement("password")]
    public string Password { get; set; } = null!;
}

public class RequestForgotPassword
{
    [BsonElement("email")]
    public string Email { get; set; } = null!;
}
