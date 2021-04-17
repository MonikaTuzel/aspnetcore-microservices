using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserApi.Data;
using UserApi.IServices;
using UserApi.Models;

namespace UserApi.Sevices
{
    public class UserService : IUserService
    {
        ApplicationDbContext dbContext;
        private UserManager<IdentityUser> userManager { get; }
        private RoleManager<IdentityRole> roleManager { get; }
        private readonly IConfiguration configuration;

        public UserService(ApplicationDbContext _db, UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _roleManager,
            IConfiguration _configuration)
        {
            dbContext = _db;
            userManager = _userManager;
            roleManager = _roleManager;
            configuration = _configuration;
        }
        /// <summary>
        /// Metoda zwraCAJĄCA WSZYSTkich użytkowników
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<IdentityUser>> GetUsers()
        {
            return await dbContext.Users.ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">Obietk użyt</param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<string> AddUser(IdentityUser user, string password)
        {
            return await Register(user, password);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IdentityUser> UpdateUser(ExpandoObject user)
        {
            string id = user.Where(u => u.Key == "idUser").FirstOrDefault().Value.ToString();
            var newUser = userManager.FindByIdAsync(id).Result;

            newUser.UserName = user.Where(u => u.Key == "userName").FirstOrDefault().Value.ToString();
            newUser.Email = user.Where(u => u.Key == "email").FirstOrDefault().Value.ToString();

            await userManager.UpdateAsync(newUser);
            return await userManager.FindByIdAsync(id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>W</returns>
        public async Task<string> DeleteUser(string id)
        {
            var user = dbContext.Users.FirstOrDefault(x => x.Id == id);
            dbContext.Entry(user).State = EntityState.Deleted;
            dbContext.SaveChanges();
            return await Task.FromResult("");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IdentityUser> GetUserById(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        private async Task<string> Register(IdentityUser user, string password)
        {

            //user = new IdentityUser();
            //user.UserName = "TestUser";
            //user.Email = "TestUser@test.com";

            string massage = "";
            try
            {
                var checkUserExistance = await userManager.FindByNameAsync(user.UserName);
                if (checkUserExistance == null)
                {
                    var result = await userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                        massage = "OK";
                }
                else
                {
                    massage = "User not available";
                }

            }
            catch (Exception ex)
            {
                massage = ex.Message;
            }

            return await Task.FromResult(massage);
        }
    }
}



