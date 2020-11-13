using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KeyVaultWebAPI.Model.ViewModels
{
    public class Key
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Username")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "ExpirationDate")]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
