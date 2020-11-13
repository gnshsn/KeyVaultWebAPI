using KeyVaultWeb.Models;
using KeyVaultWebAPI.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyVaultWebAPI.Services
{
    public interface IKeyvaultRepository
    {
        Task AddKey(Key key);
        Task UpdateKey(Key key);
        Task DeleteKey(string id);
        Task<Key> GetKey(string id);
        Task<IEnumerable<Key>> GetKeys(string userid);
        Task ShareKey(Key key);
        IEnumerable<UserListViewModel> GetUsers();
        string GetUserID(string username);
        Task<UserLogViewModel> GetUserLog(string id);
        Task SetUserLog(UserLogViewModel model);
    }
}
