using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace CODL.Models;

public class AppUser
{
    private readonly Guid _id = Guid.NewGuid();
    [Key]   
    public Guid Id
    {
        get { return _id;}
        private set { }
    }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public byte[] PasswordHash { get; set; }
    [Required]
    public byte[] PasswordSalt { get; set; }
    public readonly DateTime _createdAt = DateTime.Now;
    public DateTime CreatedAt { get { return _createdAt; }}

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class AppUserResponse
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class AppUserRegister
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string ConfirmPassword { get; set; }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class AppUserLogin
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

