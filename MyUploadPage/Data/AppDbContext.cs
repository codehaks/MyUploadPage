using Microsoft.EntityFrameworkCore;
using MyUploadPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyUploadPage.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doc>()
                .HasIndex(c=>c.Id).IsUnique();
        }

        public DbSet<Doc> Docs { get; set; }
    }
}
