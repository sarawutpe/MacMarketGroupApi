using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

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
    public List<string> Images { get; set; } = new List<string>();

    [BsonElement("price")]
    public decimal Price { get; set; } = 0;

    [BsonElement("condition")]
    [EnumDataType(typeof(Condition))]
    public string Condition { get; set; } = "";

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
    [Display(Name = "NEW")]
    NEW,

    [Display(Name = "LIKE_NEW")]
    LIKE_NEW,

    [Display(Name = "GOOD")]
    GOOD,

    [Display(Name = "FAIR")]
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

