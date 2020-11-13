using KeyVaultWebAPI.Model.ViewModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyVaultWebAPI.Model
{
    public class MangoDBContext
    {
        private readonly IMongoDatabase _mongoDb;
        public MangoDBContext()
        {
            var client = new MongoClient("mongodb+srv://hgenis:<Lj5neH8mhoR537F7>@cluster0.l8xof.mongodb.net/<dbname>?retryWrites=true&w=majority");
            var database = client.GetDatabase("keyvault");
        }
        public IMongoCollection<Key> Key
        {
            get
            {
                return _mongoDb.GetCollection<Key>("keys");
            }
        }
    }
}
