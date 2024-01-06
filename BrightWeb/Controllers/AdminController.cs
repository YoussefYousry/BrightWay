using AutoMapper;
using BrightWeb_BAL.Contracts;
using BrightWeb_BAL.DTO;
using BrightWeb_DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BrightWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userAdminManager;
        private readonly IAuthService _authService;
        private IRepositoryManager _repository;
        public AdminController(
            IMapper mapper,
            UserManager<User> userAdminManager,
        IAuthService authService,
        IRepositoryManager repository)
        {
            _mapper = mapper;
            _userAdminManager = userAdminManager;
            _authService = authService;
            _repository = repository;
        }
        [HttpPost("Registration")]
        public async Task<IActionResult> RegisterUser([FromBody] AdminForRegisterDto userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);
            var result = await _userAdminManager.CreateAsync(user, userForRegistration.Password!);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            await _userAdminManager.AddToRoleAsync(user, "Admin");
            return StatusCode(201);
        }

        [HttpPost("AdminLogin")]
        public async Task<IActionResult> AuthenticateToAdmin([FromBody] UserForLoginDto user)
        {
            if (!await _authService.ValidateUser(user))
            {
                return Unauthorized();
            }
            var admin = await _userAdminManager.FindByEmailAsync(user.Email!);
            var useradmin = await _userAdminManager.IsInRoleAsync(admin!, "Admin");
            var token = await _authService.CreateToken();
            var userId = await _userAdminManager.GetUserIdAsync(admin!);
            //var cookieOptions = new CookieOptions
            //{
            //    Expires = DateTime.Now.AddDays(20),
            //    Secure = true,
            //    SameSite = SameSiteMode.Strict
            //};
            //Response.Cookies.Append("StudentId", userId, cookieOptions);
            //Response.Cookies.Append("Token", token, cookieOptions);
            if (!useradmin)
                return NotFound();
            return Ok(
            new
            {
                Token = token,
                UserId = userId
            }
            );
        }
    }
}
