using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AccountApplication.Areas.Identity.Data;
using System.Threading.Tasks;
using AccountApplication.DTOs.Auth.Request;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using BackEnd.dtos.Auth.Request;
using Microsoft.EntityFrameworkCore;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase 
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

   [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        if (ModelState.IsValid)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return BadRequest(new { success = false, message = "Email đã tồn tại" });
            }
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Status = 1
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { success = true, message = "Đăng ký thành công" });
            }

            return BadRequest(new { success = false, message = "Đăng ký thất bại" });
        }

        return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ" });
    }

    private JwtSecurityToken CreateToken(List<Claim> authClaims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            authClaims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: signIn);

        return token;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = CreateToken(authClaims);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
        // return not found with message not found
        return NotFound(new { message = "Invalsid credentials" });
    }

    [HttpPost("google")]
    public async Task<IActionResult> Google([FromBody] GoogleModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            // user = new ApplicationUser 
            // { 
            //     UserName = model.Email, 
            //     Email = model.Email,
            //     FirstName = model.FirstName,
            //     LastName = model.LastName,
            //     PhoneNumber = model.PhoneNumber,
            //     Address = model.Address,
            //     Status = 1
            // };

            // var result = await _userManager.CreateAsync(user);
            // if (!result.Succeeded)
            // {
            //     return BadRequest(new { errors = result.Errors });
            // }
            // console hehe
            return BadRequest(new { message = "User not found" });
        }

        // var authClaims = new List<Claim>
        // {
        //     new Claim(ClaimTypes.Name, user.UserName),
        //     new Claim(ClaimTypes.Email, user.Email),
        //     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        // };

        // var token = CreateToken(authClaims);

        // return found with mesage found only 
        return Ok(new { message = "User found" });
    }
}