using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace MacMarketGroupApi;

public class Category
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("code")]
    public string Code { get; set; } = "";

    [BsonElement("name")]
    public string Name { get; set; } = "";

    [BsonElement("isActive")]
    public virtual bool IsActive { get; set; } = true;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}

public class CategoryResponse : Category
{
    [BsonIgnore]
    [JsonIgnore]
    public override bool IsActive { get; set; }
}

public class UpdateCategoryResponse : Category
{
    [BsonIgnore]
    [JsonIgnore]
    public override bool IsActive { get; set; }
}