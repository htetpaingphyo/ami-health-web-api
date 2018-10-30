using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ami.Health.Core.Entities;
using Ami.Health.WebApi.Admin.Models;

namespace Ami.Health.WebApi.Admin.Models
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions options) : base(options)
        {
            //...
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Logger> Logger { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Model configuration
            modelBuilder.Entity<User>(UserConfiguration);
            modelBuilder.Entity<Admin>(AdminConfiguration);
            modelBuilder.Entity<Logger>(LoggerConfiguration);

            base.OnModelCreating(modelBuilder);
        }

        protected void UserConfiguration(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.Email).IsUnique();
            builder.HasIndex(e => e.UserCode).IsUnique();
        }

        protected void AdminConfiguration(EntityTypeBuilder<Admin> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.Email).IsUnique();
        }

        protected void LoggerConfiguration(EntityTypeBuilder<Logger> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.LogInfo).IsUnicode(true);
        }        
    }
}