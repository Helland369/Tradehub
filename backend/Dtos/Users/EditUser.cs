using System.Text.Json.Serialization;
using Backend.Models;

namespace Backend.DTO.Users;

public class EditAddress
{
    [JsonPropertyName("city")]
    public string? City { get; set; } = "";
    
    [JsonPropertyName("country")]
    public string? Country { get; set; } = "";
    
    [JsonPropertyName("street")]
    public string? Street { get; set; } = "";
    
    [JsonPropertyName("zip")]
    public int? Zip { get; set; }
}

public class EditUser
{
    [JsonPropertyName("fname")]
    public string? Fname { get; set; } = "";
    
    [JsonPropertyName("lname")]
    public string? Lname { get; set; } = "";
    
    [JsonPropertyName("email")]
    public string? Email { get; set; } = "";
    
    [JsonPropertyName("userName")]
    public string? UserName { get; set; } = "";
    
    [JsonPropertyName("phone")]
    public string? Phone { get; set; } = "";
    
    [JsonPropertyName("password")]
    public string? Password { get; set; } = "";
    
    [JsonPropertyName("address")]
    public EditAddress? Address { get; set; }
}