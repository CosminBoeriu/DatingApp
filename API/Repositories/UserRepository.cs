using API.Data;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class UserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context, IMapper mapper)
    {
        _context = context;
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        var users = await _context.Users.Include(p => p.Photos).ToListAsync();
        await SaveAllAsync();
        return users;
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
    
    public async Task<AppUser> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        return user;
    }
    
    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
        var user = await _context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
        return user;
    }

    public async Task<List<AppUser>> GetLikedUsers(string username)
    {
        var user = await _context.Users.Include(x => x.Likes)
            .FirstOrDefaultAsync(x => x.UserName == username);
        if(user != null)
            return user.Likes.ToList();
        return new List<AppUser>();
    }

    public void Update(AppUser user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }
}