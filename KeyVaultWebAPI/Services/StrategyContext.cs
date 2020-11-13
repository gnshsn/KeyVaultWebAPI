using KeyVaultWebAPI.Model.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyVaultWebAPI.Services
{
    public class StrategyContext
    {
        private readonly IKeyvaultRepository repository;

        public StrategyContext(IKeyvaultRepository repository)
        {
            this.repository = repository;
        }

        public async Task AddKey(Key key)
        {
            try
            {
                if (repository != null)
                {
                    await repository.AddKey(key);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public Task<IEnumerable<Key>> GetKeys(string userid)
        {
            try
            {
                return repository.GetKeys(userid);

            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public async Task UpdateKey(Key key)
        {
            try
            {
                await repository.UpdateKey(key);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Key> GetKey(string id)
        {
            try
            {
                return await repository.GetKey(id);

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
                 await repository.DeleteKey(id);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task ShareKey(Key key)
        {
            try
            {
                await repository.ShareKey(key);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<UserListViewModel> GetUsers( ) 
        {
            try
            {
                return repository.GetUsers();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public string GetUserID(string username)
        {
            try
            {
                return repository.GetUserID(username);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<UserLogViewModel> GetUserLog(string userid)
        {
            try
            {
                return await repository.GetUserLog(userid);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task SetUserLog(UserLogViewModel model)
        {
            try
            {
                if (repository != null)
                {
                    await repository.SetUserLog(model);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
