namespace Backend.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

public class Address
{
    [BsonElement("street")]
    [JsonPropertyName("street")]
    public string Street { get; set; } = "";

    [BsonElement("city")]
    [JsonPropertyName("city")]
    public string City { get; set; } = "";

    [BsonElement("country")]
    [JsonPropertyName("coutry")]
    public string Country { get; set; } = "";

    [BsonElement("zip")]
    [JsonPropertyName("zip")]
    public int zip { get; set; }
}

public class User
{
    [BsonId]
    [BsonElement("_id")]
    public ObjectId ID { get; set; }

    [BsonElement("fname")]
    [JsonPropertyName("fname")]
    public string Fname { get; set; } = "";

    [BsonElement("lname")]
    [JsonPropertyName("lname")]
    public string Lname { get; set; } = "";

    [BsonElement("email")]
    [JsonPropertyName("email")]
    public string Email { get; set; } = "";

    [BsonElement("userName")]
    [JsonPropertyName("userName")]
    public string UserName { get; set; } = "";

    [BsonElement("address")]
    [JsonPropertyName("address")]
    public Address Address { get; set; } = new();

    [BsonElement("phone")]
    [JsonPropertyName("phone")]
    public string Phone { get; set; } = "";

    // we dont send password to client
    [BsonElement("passwordHash")]
    [JsonIgnore]
    public string PasswordHash { get; set; } = "";

    [BsonElement("selling")]
    [JsonPropertyName("selling")]
    public List<ObjectId> Selling { get; set; } = new();

    [BsonElement("purchases")]
    [JsonPropertyName("purchases")]
    public List<ObjectId> Purchases { get; set; } = new();

    [BsonElement("createdAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("role")]
    [JsonPropertyName("role")]
    public string Role { get; set; } = "user";
}

