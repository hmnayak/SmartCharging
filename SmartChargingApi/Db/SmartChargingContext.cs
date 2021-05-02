using Microsoft.EntityFrameworkCore;
using SmartChargingApi.Models.Domain;

namespace SmartChargingApi.Db
{
    public class SmartChargingContext : DbContext
    {

        public virtual DbSet<ChargeStation> ChargeStations { get; set; }

        public virtual DbSet<Connector> Connectors { get; set; }

        public virtual DbSet<Group> Groups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("SmartChargingApi");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Connector>()
                .HasOne(c => c.ChargeStation)
                .WithMany(cs => cs.Connectors)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChargeStation>()
                .HasOne(cs => cs.Group)
                .WithMany(g => g.ChargeStations)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Group>()
                .HasMany(g => g.ChargeStations)
                .WithOne(cs => cs.Group);
        }
    }
}
