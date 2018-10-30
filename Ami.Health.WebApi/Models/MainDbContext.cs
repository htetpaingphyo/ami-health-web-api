using Ami.Health.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ami.Health.WebApi.Models
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions options) : base(options)
        {
            //...
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ClaimsProposal> ClaimsProposals { get; set; }
        public DbSet<EventLog> EventLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(UserConfiguration);
            modelBuilder.Entity<ClaimsProposal>(ClaimsProposalConfiguration);
            modelBuilder.Entity<EventLog>(EventLogConfiguration);

            base.OnModelCreating(modelBuilder);
        }

        protected void UserConfiguration(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.Email).IsUnique();
            builder.HasIndex(e => e.UserCode).IsUnique();
        }

        protected void ClaimsProposalConfiguration(EntityTypeBuilder<ClaimsProposal> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.PolicyNo).IsUnique();
            builder.HasIndex(e => e.NIC).IsUnique();
        }

        protected void EventLogConfiguration(EntityTypeBuilder<EventLog> builder)
        {
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.EventId).HasColumnName("EventID");
            builder.Property(e => e.LogLevel).HasMaxLength(50);
            builder.Property(e => e.Message).HasMaxLength(4000);
        }
    }
}
