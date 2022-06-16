using AllTracker.Models;
using AllTracker.Controllers.Requests;
using AllTracker.Requests.Responses;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AllTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public LoginController(IConfiguration configuration,
                                SignInManager<User> signInManager,
                                UserManager<User> userManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet("Status")]
        public IActionResult Status()
        {
            var token = HttpContext.Request.Cookies.FirstOrDefault(x => x.Key == "AuthToken").Value;
            return token != null ? Ok(token) : BadRequest("JWT Stamp not found");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var foundUser = _userManager.Users.FirstOrDefault(u => u.NormalizedUserName == login.Identifier.ToUpper());
            var result = await _signInManager.PasswordSignInAsync(
                userName: foundUser?.UserName ?? "", 
                password: login.Password, 
                isPersistent: login.RememberMe, 
                lockoutOnFailure: false
            );
            if (!result.Succeeded) return BadRequest("Invalid username / password");
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, foundUser.UserName),
                new Claim(JwtClaimTypes.Id, foundUser.Id),
                new Claim(JwtClaimTypes.JwtId, foundUser.JwtStamp)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiryDate = DateTime.Now.AddDays(1000);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityToken(
                    _configuration["JwtIssuer"],
                    _configuration["JwtAudience"],
                    claims,
                    expires: expiryDate,
                    signingCredentials: creds
                )
            );

            HttpContext.Response.Cookies.Append(
                _configuration["AuthenticationCookieName"],
                tokenString,
                new CookieOptions
                {
                    HttpOnly = true
                }
            );

            return Ok(new LoginResponse { Id = foundUser.Id, Username = foundUser.UserName, Token = tokenString });
        }

        [HttpPost("RegisterUser")]
        public async Task<ActionResult> RegisterUser([FromBody] LoginRequest request)
        {
            if (request.Identifier.Trim().Length == 0) return BadRequest("Username is required");
            if (request.Password.Trim().Length == 0) return BadRequest("Password is required");

            User newUser = new User
            {
                UserName = request.Identifier,
                JwtStamp = Guid.NewGuid().ToString(),
                
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(errors);
            }

            return Ok(newUser);
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete(
                _configuration["AuthenticationCookieName"]
            );
            return Ok();
        }
    }
}
