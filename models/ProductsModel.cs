using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MacMarketGroupApi.Models;
public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("code")]
    public string Code { get; set; } = "";

    [BsonElement("userId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;

    [BsonElement("categoryId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CategoryId { get; set; } = null!;

    [BsonElement("category")]
    public Category? Category { get; set; }

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
    public virtual DateTime CreatedAt { get; set; }

    [BsonElement("updatedAt")]
    public virtual DateTime UpdatedAt { get; set; }
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

public class RequestCreateProduct
{

    [BsonElement("data")]
    [ModelBinder(BinderType = typeof(AuthorEntityBinder))]
    public Product Data { get; set; } = null!;

    [BsonElement("images")]
    public List<IFormFile>? Images { get; set; }
}

public class RequestUpdateProduct : RequestCreateProduct { }

