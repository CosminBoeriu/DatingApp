using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class AccountRepository
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;

    public AccountRepository(DataContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
    {
        using var hmac = new HMACSHA512(); //class is disposable => can use 'using'
        var user = new AppUser()
        {
            UserName = registerDto.Username,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return new UserDTO()
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }

    public async Task<UserDTO> Login(LoginDTO loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);
        if (user == null)
            return null;
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
                return null;
        }
        return new UserDTO()
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
        
    }
    
    public async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
    }
}