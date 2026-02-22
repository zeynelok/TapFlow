using Microsoft.EntityFrameworkCore;
using TapFlow.API.Models;

namespace TapFlow.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Pour> Pours { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pour>(entity =>
        {
            entity.HasKey(e => e.EventId);
            entity.Property(e => e.DeviceId).HasMaxLength(50);

            entity.Property(e => e.LocationId).HasMaxLength(100);

            entity.Property(e => e.ProductId).HasMaxLength(50);
        });
    }
}
