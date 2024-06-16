using DotNetDeveloperRecruitmentTask.SecurityModels;
using DotNetDeveloperRecruitmentTask.Services;
using DotNetDeveloperRecruitmentTask.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotNetDeveloperRecruitmentTask.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ProductDbContext _context;
		private readonly ITokenService _tokenService;

		public UsersController(
			UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> roleManager,
            ProductDbContext context,
			ITokenService tokenService, ILogger<UsersController> logger
			)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_context = context;
			_tokenService = tokenService;
		}


		[HttpPost]
		[Route("register")]
		public async Task<IActionResult> Register(RegistrationRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var result = await _userManager.CreateAsync(
				new IdentityUser { UserName = request.Username, Email = request.Email },
				request.Password!
			);

			if (result.Succeeded)
			{
				request.Password = "";
				return CreatedAtAction(nameof(Register), new { email = request.Email, role = request.Role }, request);
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(error.Code, error.Description);
			}

			return BadRequest(ModelState);
		}

		




		[HttpPost()]//https://domain.com/api/users/login
		[Route("login")]//https://domain.com/login
		public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var managedUser = await _userManager.FindByEmailAsync(request.Email!);

			if (managedUser == null)
			{
				return BadRequest("Bad credentials");
			}

			var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password!);

			if (!isPasswordValid)
			{
				return BadRequest("Bad credentials");
			}

			var userInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email);

			if (userInDb is null)
			{
				return Unauthorized(request);
			}

			var accessToken = _tokenService.CreateToken((IdentityUser)userInDb);
			await _context.SaveChangesAsync();

			return Ok(new AuthResponse
			{
				Username = userInDb.UserName,
				Email = userInDb.Email,
				Token = accessToken,
			});
		}


		[HttpPost]
		[Route("logout")]
		public async Task<ActionResult> Logout()
		{
			return Ok();
		}
	}
}