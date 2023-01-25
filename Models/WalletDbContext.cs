
using System;
using Microsoft.EntityFrameworkCore;

namespace Hubtel.Wallets.Api.Models
{ 
        public class WalletDbContext : DbContext
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options)
            : base(options)
        {
        }

        public DbSet<Wallet> Wallets { get; set; }
    }
}