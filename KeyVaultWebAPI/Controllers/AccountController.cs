using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KeyVaultWebAPI.Authentication;
using KeyVaultWebAPI.Model;
using KeyVaultWebAPI.Model.ViewModels;
using KeyVaultWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;

namespace KeyVaultWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly KeyvaultContext _db;
        private readonly StrategyContext mangodb = new StrategyContext(new KeyvaultMangoDbRepository());
        private readonly StrategyContext sql;
        private string sessionUserid = "userid";
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, KeyvaultContext db) 
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _db = db;
            sql = new StrategyContext(new KeyvaultSqlRepository(_db));
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = new IdentityUser() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                return Ok();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
                
                if (result.Succeeded)
                {
                    return Ok();
                }
                ModelState.AddModelError(string.Empty, "Invalid Username or Password");
                return Ok(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
           
            await signInManager.SignOutAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetUserID/{username}")]
        public string GetUserID(string username)
        {
            //wait mangodb.GetUserID(username);
            return sql.GetUserID(username);
        }

        [HttpPost]
        [Route("setUserLog")]
        public async Task SetUserLog([FromBody] UserLogViewModel model)
        {
            //wait mangodb.GetUserID(username);
            await sql.SetUserLog(model);
        }
    }
}