using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KeyVaultWeb.Models
{
    public class UsernameViewModel
    {
        public List<SelectListItem> allUsers { set; get; }  // These 2 properties for the dropdown
        [Required]
        [DisplayName("Select a user")]
        public string SelectedUser { set; get; }
        public string keyId { set; get; }
    }
}
