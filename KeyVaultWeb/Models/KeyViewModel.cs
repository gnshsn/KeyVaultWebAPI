using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KeyVaultWeb.Models
{
    public class KeyViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "ExpirationDate")]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
