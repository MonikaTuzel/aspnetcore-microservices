using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using UserApi.IServices;
using UserApi.Models;
using UserApi.Sevices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService userService { get; }
        private UserManager<IdentityUser> userManager { get; }
        private RoleManager<IdentityRole> roleManager { get; }
        private readonly IConfiguration configuration;

        public UserController(IUserService _userService, RoleManager<IdentityRole> _roleManager, UserManager<IdentityUser> _userManager,
            IConfiguration _configuration)
        {
            userService = _userService;
            userManager = _userManager;
            roleManager = _roleManager;
            configuration = _configuration;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: api/<UserController>
        [HttpGet]
        public async Task<IEnumerable<IdentityUser>> Get()
        {
            return await userService.GetUsers();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<IDictionary<string, object>> GetUserByIdAsync(string id)
        {
            return await userService.GetUserById(id);
        }

        // POST api/<UserController>
        [HttpPost]
        [Route("register")]
        public async Task<string> Post([FromBody] IdentityUser user, string password)
        {
            return await userService.AddUser(user, password);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    id = user.Id,
                    role = userRoles[0]
                });
            }
            return Unauthorized();
        }


        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public Task<IdentityUser> Put([FromBody] ExpandoObject user)
        {
            return userService.UpdateUser(user);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<string> Delete(string id)
        {
            return await userService.DeleteUser(id);
        }
    }
}
