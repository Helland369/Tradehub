using Backend.Models;

namespace Backend.DTO.Users;

public class EditAddress
{
    public string City { get; set; } = "";
    public string Country { get; set; } = "";
    public string Street { get; set; } = "";
    public int Zip { get; set; }
}

public class EditUser
{
    public string Fname { get; set; } = "";
    public string Lname { get; set; } = "";
    public string Email { get; set; } = "";
    public string UserName { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Password { get; set; } = "";
    public EditAddress Address { get; set; }
}