using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KeyVaultWeb.Models;
using KeyVaultWebAPI.Model;
using KeyVaultWebAPI.Model.ViewModels;
using KeyVaultWebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KeyVaultWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeyController : ControllerBase
    {
        private readonly KeyvaultContext _db;
        private readonly StrategyContext mangodb = new StrategyContext(new KeyvaultMangoDbRepository());
        private readonly StrategyContext sql;

        public KeyController(KeyvaultContext db )
        {
            _db = db;
            sql = new StrategyContext(new KeyvaultSqlRepository(_db));
        }

        [HttpPost]
        [Route("CreateAsync")]
        public async Task CreateAsync([FromBody]Key model)
        {
            //var userId = _userManager.FindByNameAsync("hasangenis1907@hotmail.com").Result;
            if (ModelState.IsValid && _db != null)
            {
                model.Id = Guid.NewGuid().ToString();
                model.CreateDate = DateTime.Now;
                
                await sql.AddKey(model);
                //await  mangodb.AddKey(model);
            }
        }

        [HttpGet]
        [Route("GetKeys/{userid}")]
        public async Task<IEnumerable<Key>> GetKeys(string userid)
        {
            // return await mangodb.GetKeys();
            return await sql.GetKeys(userid);
        }

        [HttpDelete]
        [Route("DeleteKey/{id}")]
        public async Task DeleteKey(string id)
        {
             //await mangodb.DeleteKey(id);
             await sql.DeleteKey(id);
        }
        [HttpPost]
        [Route("ShareKey")]
        public async Task ShareKey([FromBody]Key key)
        {
            //await mangodb.ShareKey(key);
            await sql.ShareKey(key);
        }
        [HttpPost]
        [Route("UpdateKey")]
        public async Task UpdateKey([FromBody]Key key)
        {
            //await mangodb.UpdateKey(key);
            try
            {
                await sql.UpdateKey(key);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        [HttpGet]
        [Route("GetKey/{id}")]
        public async Task<Key> GetKey(string id)
        {
            //wait mangodb.GetKey(id);
            return await sql.GetKey(id);
        }
        [HttpGet]
        [Route("GetUsers")]
        public IEnumerable<UserListViewModel> GetUsers()
        {
            //wait mangodb.GetKey(id);
            return  sql.GetUsers();
        }
        
        [HttpGet]
        [Route("GetUserLog/{userid}")]
        public async Task<UserLogViewModel> GetUserLog(string userid)
        {
            //wait mangodb.GetUserID(username);
            return await sql.GetUserLog(userid);
        }
    }
}