using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AccountApplication.Areas.Identity.Data; 
using System.Threading.Tasks;
using AccountApplication.DTOs.Auth.Request;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase 
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        var user = new ApplicationUser 
        { 
            UserName = model.Email, 
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName 
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        
        if (result.Succeeded)
        {
            return Ok(new { message = "Registration successful" });
        }

        return BadRequest(new { errors = result.Errors });
    }
}