using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace MacMarketGroupApi.Models;
public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("userId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;

    [BsonElement("categoryId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CategoryId { get; set; } = null!;

    [BsonElement("name")]
    public string Name { get; set; } = "";

    [BsonElement("images")]
    public string[] Images { get; set; } = Array.Empty<string>();

    [BsonElement("price")]
    public decimal Price { get; set; } = 0;

    [BsonElement("condition")]
    public Condition Condition { get; set; } = Condition.NEW;

    [BsonElement("description")]
    public string Description { get; set; } = "";

    [BsonElement("location")]
    public string Location { get; set; } = "";

    [BsonElement("isPublic")]
    public bool IsPublic { get; set; } = true;

    [BsonElement("isActive")]
    public bool IsActive { get; set; } = true;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}

public enum Condition
{
    NEW,
    LIKE_NEW,
    GOOD,
    FAIR
}

public class ProductRequest
{

    [BsonElement("data")]
    [ModelBinder(BinderType = typeof(AuthorEntityBinder))]
    public Product Data { get; set; } = null!;

    [BsonElement("images")]
    public List<IFormFile> Images { get; set; } = null!;
}

