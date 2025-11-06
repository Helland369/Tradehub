namespace Backend.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

public class Bid
{
    [BsonId]
    [BsonElement("_id")]
    public ObjectId ID { get; set; }

    [BsonElement("userId")]
    public ObjectId userID { get; set; }

    [BsonElement("amount")]
    [JsonPropertyName("amount")]
    public double Amount { get; set; }

    [BsonElement("timeStamp")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [JsonPropertyName("timeStamp")]
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
}

public enum ListingStatus
{
    Active,
    Sold,
    Expired,
    Draft
}

public class Listing
{
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ID { get; set; } = string.Empty;

    [BsonElement("userId")]
    [JsonPropertyName("userId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserID { get; set; } = string.Empty;

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

    [BsonElement("images")]
    [JsonPropertyName("images")]
    public List<string?> Images { get; set; } = new();

    [BsonElement("startingPrice")]
    [JsonPropertyName("startingPrice")]
    public double? StartingPrice { get; set; }

    [BsonElement("buyPrice")]
    [JsonPropertyName("buyPrice")]
    public double? BuyPrice { get; set; }

    [BsonElement("currentBid")]
    [JsonPropertyName("currentBid")]
    public double? CurrentBid { get; set; }

    [BsonElement("sellerId")]
    [JsonPropertyName("sellerId")]
    public ObjectId SellerID { get; set; }

    [BsonElement("bids")]
    [JsonPropertyName("bids")]
    public List<Bid> Bids { get; set; } = new();

    [BsonElement("isAuction")]
    [JsonPropertyName("isAuction")]
    public bool IsAuction { get; set; }

    [BsonElement("createdAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("endTime")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [JsonPropertyName("endTime")]
    public DateTime EndTime { get; set; }

    [BsonElement("status")]
    [BsonRepresentation(BsonType.String)]
    [JsonPropertyName("status")]
    public ListingStatus Status { get; set; } = ListingStatus.Active;

    [BsonElement("watchers")]
    [JsonPropertyName("watchers")]
    public List<ObjectId> Watchers { get; set; } = new();

    [BsonElement("location")]
    [JsonPropertyName("location")]
    public string Location { get; set; } = "";
}
