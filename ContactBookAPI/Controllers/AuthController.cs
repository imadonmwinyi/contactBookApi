using ContactBookAPI.DTOs;
using ContactBookAPI.Lib.Core;
using ContactBookAPI.Lib.Core.Services;
using ContactBookAPI.Lib.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SignInManager<AppUser> _signIn;
        private readonly UserManager<AppUser> _userManager;
        public AuthController(IConfiguration config,
            SignInManager<AppUser> signIn, UserManager<AppUser> userManager)
        {
            _config = config;
            _signIn = signIn;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            var roles = await _userManager.GetRolesAsync(user);
            if(user != null)
            {
                var res = await _signIn.PasswordSignInAsync(user, 
                                       loginDto.Password, loginDto.Rememberme, false);
                // generate logged in token
                var Jwt = new JwtTokenGeneratorClass(_config);
                var token =Jwt.GenerateToken(user.Id, user.Email, roles.ToList());
                if (res.Succeeded)
                    return Ok(token);
            }
            
               
            return BadRequest();
        }
    }
}
