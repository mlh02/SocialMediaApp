using System;
using Microsoft.EntityFrameworkCore;

namespace SocialMedia_ApplicationV1.Models
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

    }
}
