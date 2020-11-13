using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyVaultWebAPI.Model;
using KeyVaultWebAPI.Model.ViewModels;
using MongoDB.Driver;

namespace KeyVaultWebAPI.Services
{
    public class KeyvaultMangoDbRepository : IKeyvaultRepository
    {
        MangoDBContext db = new MangoDBContext();
        public async Task AddKey(Key key)
        {
            try
            {
                await db.Key.InsertOneAsync(key);
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteKey(string id)
        {
            try
            {
                FilterDefinition<Key> data = Builders<Key>.Filter.Eq("Id", id);
                await db.Key.DeleteOneAsync(data);
            }
            catch
            {
                throw;
            }
        }

        public async Task<Key> GetKey(string id)
        {
            try
            {
                FilterDefinition<Key> filter = Builders<Key>.Filter.Eq("Id", id);
                return await db.Key.Find(filter).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Key>> GetKeys(string userid)
        {
            try
            {
                return await db.Key.Find(_ => true).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task ShareKey(Key key)
        {
            try
            {
                await db.Key.InsertOneAsync(key);
            }
            catch
            {
                throw;
            }
        }

        public async Task UpdateKey(Key key)
        {
            try
            {
                await db.Key.ReplaceOneAsync(filter: g => g.Id == key.Id, replacement: key);
            }
            catch
            {
                throw;
            }
        }
        public IEnumerable<UserListViewModel> GetUsers()
        {
            try
            {
                IList<UserListViewModel> List = null;
                return List;
            }
            catch
            {
                throw;
            }
        }

        public string GetUserID(string username)
        {
            try
            {
                return username;
            }
            catch
            {
                throw;
            }
        }
        public async Task<UserLogViewModel> GetUserLog(string userid)
        {
            try
            {
                UserLogViewModel model = null;
                return model;
            }
            catch
            {
                throw;
            }
        }
        public async Task SetUserLog(UserLogViewModel model)
        {
            try
            {
                //await db.UserLog.InsertOneAsync(model);
                return;
            }
            catch
            {
                throw;
            }
        }
    }
}
