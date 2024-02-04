using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUser(DataContext context)
    {
        if(await context.Users.CountAsync() > 1)
            return;
        
        var userData = await File.ReadAllTextAsync("Data/UserSeedFile.json");
        var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, option);

        foreach (var user in users)
        {
            using var hmac = new HMACSHA512();

            user.UserName = user.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$wOrd"));

            user.PasswordSalt = hmac.Key;
            context.Users.Add(user);
        }

        await context.SaveChangesAsync();
    }
}