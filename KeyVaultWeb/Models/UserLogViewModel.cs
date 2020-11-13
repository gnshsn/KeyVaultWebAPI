using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyVaultWeb.Models
{
    public class UserLogViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
