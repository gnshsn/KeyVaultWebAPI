using MongoDB.Driver.Core.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyVaultWeb.Utility
{
    public class UtilityClass
    {
        public string Username;

        public string getUsername()
        {
            return this.Username;
        }
        public void setUsername(string Username)
        {
            this.Username = Username;
        }
    }
}
