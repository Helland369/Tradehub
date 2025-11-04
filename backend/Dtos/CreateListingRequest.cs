namespace backend.Dtos;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

public class CreateListingRequest
{
    [BsonElement("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; } = "";

    [BsonElement("description")]
    [JsonPropertyName("description")]
    public string Description { get; set; } = "";

    [BsonElement("category")]
    [JsonPropertyName("category")]
    public string Category { get; set; } = "";

    [BsonElement("condition")]
    [JsonPropertyName("condition")]
    public string Condition { get; set; } = "";

    [BsonElement("startingPrice")]
    [JsonPropertyName("startingPrice")]
    public double StartingPrice { get; set; }

    [BsonElement("buyPrice")]
    [JsonPropertyName("buyPrice")]
    public double? BuyPrice { get; set; }

    [BsonElement("isAuction")]
    [JsonPropertyName("isAuction")]
    public bool IsAuction { get; set; }
    
    [BsonElement("endTime")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [JsonPropertyName("endTime")]
    public DateTime EndTime { get; set; }

    [BsonElement("location")]
    [JsonPropertyName("location")]
    public string Location { get; set; } = "";
    
    [BsonElement("images")]
    [JsonPropertyName("images")]
    public List<IFormFile?> Images { get; set; }
}