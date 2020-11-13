using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyVaultWebAPI.Model.ViewModels
{
    public class UserLogViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
