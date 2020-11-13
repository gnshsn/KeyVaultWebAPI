using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyVaultWeb.Models;
using KeyVaultWebAPI.Model;
using KeyVaultWebAPI.Model.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KeyVaultWebAPI.Services
{
    public class KeyvaultSqlRepository : IKeyvaultRepository, IDisposable
    {
        private readonly KeyvaultContext db;
        public KeyvaultSqlRepository(KeyvaultContext db)
        {
            this.db = db;
        }

        bool disposed = false;
        public async Task AddKey(Key key)
        {
            try
            {
                db.Keys.Add(key);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteKey(string id)
        {
            try
            {
                Key key = await db.Keys.FindAsync(id);
                db.Keys.Remove(key);
                await db.SaveChangesAsync();
            }
            catch(Exception e)
            {
                throw e;
            }
        }


        private bool KeyExists(string id)
        {
            return db.Keys.Count(e => e.Id == id) > 0;
        }

        ~KeyvaultSqlRepository()
        {
            if (!disposed)
            {
                disposed = true;
                Dispose(true);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        public async Task<Key> GetKey(string id)
        {
            try
            {
                Key key = await db.Keys.FindAsync(id);
                if (key == null)
                {
                    return null;
                }
                return key;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<Key>> GetKeys(string userid)
        {
            try
            {
                var keys = await db.Keys.Where(x => x.UserId == userid).ToListAsync();
                return keys.AsQueryable();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task ShareKey(Key key)
        {
            try
            {
                db.Entry(key).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task UpdateKey(Key key)
        {
            try
            {
                db.Entry(key).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<UserListViewModel> GetUsers()
        {
            try
            {
                IList<UserListViewModel> list = new List<UserListViewModel>(); 
                UserListViewModel model = new UserListViewModel();
                var users = db.Users.Select(x => new { x.Id, x.UserName }).ToList();
                foreach (var item in users)
                {
                    model.UserId = item.Id;
                    model.UserName = item.UserName;
                    list.Add(model);
                }
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string GetUserID(string username)
        {
            try
            {
                var userid = db.Users.FirstOrDefault(x => x.Email == username);
                return userid.Id;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<UserLogViewModel> GetUserLog(string userid)
        {
            try
            {
                UserLogViewModel model = new UserLogViewModel();
                model =  db.UserLog.OrderByDescending(x => x.LoginTime).Skip(1).FirstOrDefault(x => x.UserId == userid);
                
                if (model == null)
                {
                    return null;
                }
                return model;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task SetUserLog(UserLogViewModel model)
        {
            try
            {
                db.UserLog.Add(model);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
