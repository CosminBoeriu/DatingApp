using System.Runtime.InteropServices.JavaScript;
using API.Extensions;
using Microsoft.VisualBasic;

namespace API.Entities;

public class AppUser
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    
    public DateOnly DateOfBirth { get; set; }

    public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    
    public string Gender { get; set; }
    
    public string Adress { get; set; }
    
    public string Description { get; set; }

    public List<Photo> Photos { get; set; } = new();

    public List<AppUser> AppUsers { get; set; } = new();

    public int GetAge()
    {
        return DateOfBirth.CalculateAge();
    }
}