using GOtica.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess;

internal class GOticaDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<OpticalStore> OpticalStores { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserOpticalStore> UserOpticalStores { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Invite> Invites { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configura a chave composta de UserOpticalStore
        modelBuilder.Entity<UserOpticalStore>()
            .HasKey(uos => new { uos.UserId, uos.OpticalStoreId });

        // Configura os relacionamentos
        modelBuilder.Entity<UserOpticalStore>()
            .HasOne(uos => uos.User)
            .WithMany()
            .HasForeignKey(uos => uos.UserId);

        modelBuilder.Entity<UserOpticalStore>()
            .HasOne(uos => uos.OpticalStore)
            .WithMany()
            .HasForeignKey(uos => uos.OpticalStoreId);
    }
}
