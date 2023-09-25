using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class ClientDbContext : DbContext
    {
        public ClientDbContext(DbContextOptions<ClientDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<AuditEntry> AuditEntries { get; set; } // Added for auditing

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Addresses)
                .WithOne(a => a.Client)
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Client>()
                .Property(c => c.ClientId)
                .ValueGeneratedNever();
        }

        public override int SaveChanges()
        {
            var changedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntities)
            {
                var entityType = entry.Entity.GetType().Name;
                var primaryKey = FindPrimaryKey(entry);
                var changeType = entry.State.ToString();

                var changes = new List<string>();
                foreach (var property in entry.OriginalValues.Properties)
                {
                    var original = entry.OriginalValues[property];
                    var current = entry.CurrentValues[property];
                    if (!object.Equals(original, current))
                    {
                        changes.Add($"Property {property.Name} changed from {original} to {current}");
                    }
                }

                var auditEntry = new AuditEntry
                {
                    EntityType = entityType,
                    PrimaryKey = primaryKey.ToString(),
                    ChangeType = changeType,
                    Changes = string.Join(", ", changes),
                    ChangeDate = DateTime.Now,
                };

                AuditEntries.Add(auditEntry);
            }

            return base.SaveChanges();
        }

        private object FindPrimaryKey(EntityEntry entry)
        {
            var keyName = this.Model.FindEntityType(entry.Entity.GetType()).FindPrimaryKey().Properties.Select(x => x.Name).Single();
            return entry.Property(keyName).CurrentValue;
        }
    }

    public class AuditEntry
    {
        public int AuditEntryId { get; set; }
        public string EntityType { get; set; }
        public string PrimaryKey { get; set; }
        public string ChangeType { get; set; }
        public string Changes { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
