using KeyVaultWeb.Models;
using KeyVaultWebAPI.Authentication;
using KeyVaultWebAPI.Model.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyVaultWebAPI.Model
{
    public class KeyvaultContext : IdentityDbContext
    {
        public KeyvaultContext(DbContextOptions<KeyvaultContext> options)
            : base(options)
        {
        }
        public KeyvaultContext()
        {

        }
        public virtual DbSet<Key> Keys { get; set; }
        public virtual DbSet<UserLogViewModel> UserLog { get; set; }
    }
}
