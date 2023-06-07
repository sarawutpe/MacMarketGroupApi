using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MacMarketGroupApi;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [BsonElement("email")]
    public string Email { get; set; } = "";

    [BsonElement("password")]
    public string Password { get; set; } = "";

    [BsonElement("phoneNumber")]
    public string PhoneNumber { get; set; } = "";

    [BsonElement("isActive")]
    public virtual bool IsActive { get; set; } = true;

    [BsonElement("isVerified")]
    public bool IsVerified { get; set; } = false;

    [BsonElement("lastAccessed")]
    public DateTime LastAccessed { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}