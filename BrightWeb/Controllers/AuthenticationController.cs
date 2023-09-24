using AutoMapper;
using BrightWeb_BAL.Contracts;
using BrightWeb_BAL.DTO;
using BrightWeb_DAL.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BrightWeb.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<Student> _userManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        //private readonly IEmailService _emailService;
        public AuthenticationController(
            UserManager<Student> userManager, IAuthService authService, IMapper mapper)
            //, IEmailService emailService)
        {
            _userManager = userManager;
            _authService = authService;
            _mapper = mapper;
            //_emailService = emailService;
        }
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] StudentForRegisterDto userForRegistration)
        {
            var user = _mapper.Map<Student>(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password!);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            await _userManager.AddToRoleAsync(user, "Student");
            var student = await _userManager.FindByNameAsync(user.UserName!);
            return Ok(
                new
                {
                    UserId = await _userManager.GetUserIdAsync(student!)
                }
                );
        }
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserForLoginDto user)
        {
            if (!await _authService.ValidateUser(user))
                return Unauthorized();
            var student = await _userManager.FindByEmailAsync(user.Email!);
            var token = await _authService.CreateToken();
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(20),
                Secure = true,
                SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append("StudentId", student!.Id, cookieOptions);
            Response.Cookies.Append("Token", token, cookieOptions);
            return Ok(
            new
            {
                Token = token,
                UserId = await _userManager.GetUserIdAsync(student!)
            }
            );
        }
    }
}
