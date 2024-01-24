using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly AccountRepository _accountRepository;

    public AccountController(AccountRepository accountRepo)
    {
        _accountRepository = accountRepo;
    }

    [HttpPost("register")] // POST: api/account/register
    public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
    {
        if (await _accountRepository.UserExists(registerDto.Username)) 
            return BadRequest("Username is taken");

        return await _accountRepository.Register(registerDto);  // Method also adds user to DB!
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
    {
        var user = await _accountRepository.Login(loginDto);
        if (user == null) 
            return Unauthorized("Either user or password is invalid");
        return user;
    }
    
}