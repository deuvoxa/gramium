using Gramium.Framework.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gramium.Framework.Database;

public class GramiumDbContext(DbContextOptions<GramiumDbContext> options) : DbContext(options)
{
    public DbSet<TelegramUser> Users { get; set; } = null!;
    public DbSet<UserState> States { get; set; } = null!;
    public DbSet<UserMetadata> Metadata { get; set; } = null!;
    public DbSet<PayloadEntity> Payloads { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=gramium.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TelegramUser>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.TelegramId).IsUnique();
        });

        modelBuilder.Entity<UserState>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.Key }).IsUnique();

            entity.HasOne(e => e.User)
                .WithMany(e => e.States)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserMetadata>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.Key }).IsUnique();

            entity.HasOne(e => e.User)
                .WithMany(e => e.Metadata)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PayloadEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
    }
}