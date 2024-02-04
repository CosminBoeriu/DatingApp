using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[AllowAnonymous]
public class UsersController : BaseApiController
{
    private readonly UserRepository _userRepository;
    private readonly IMapper _mapper;
    
    public UsersController(UserRepository userRepo, IMapper mapper)
    {
        _userRepository = userRepo;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
    {
        var users = await _userRepository.GetUsersAsync();

        var usersToReturn = _mapper.Map<IEnumerable<MemberDTO>>(users);
        return Ok(usersToReturn);
    }
    
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDTO>> GetUserByUsername(string username)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);

        return _mapper.Map<MemberDTO>(user);
    }

    [HttpGet("likes/{username}")]
    public async Task<ActionResult<List<string>>> GetLikes(string username)
    {
        var liked = await _userRepository.GetLikedUsers(username);
        return Ok(liked);
    }
    
    [HttpPut("addlike")]
    public async Task<ActionResult> AddUserLike(MemberLikesDTO memberLikesDto)
    {
        var user = await _userRepository.GetUserByUsernameAsync(memberLikesDto.UserName);
        var likedUser = await _userRepository.GetUserByUsernameAsync(memberLikesDto.LikedUserName);
        if (user == null || likedUser == null) 
            return NotFound();
        user.Likes.Add(likedUser);
        if (await _userRepository.SaveAllAsync())
            return NoContent();
        return BadRequest("Failed to update");
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDto)
    {
        var user = await _userRepository.GetUserByUsernameAsync(memberUpdateDto.UserName);
        if (user == null) 
            return NotFound();
        _mapper.Map(memberUpdateDto, user);
        if (await _userRepository.SaveAllAsync())
            return NoContent();
        return BadRequest("Failed to update");
    }
}