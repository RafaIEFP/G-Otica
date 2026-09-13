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
    public DbSet<StockMovement> StockMovements { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<PurchaseItem> PurchaseItems { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Treatment> Treatments { get; set; }
    public DbSet<ItemLens> ItemLens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // CK == Composite Key
        ConfigureUserOpticalStoreCK(modelBuilder);
        ConfigureItemLensTreatmentCK(modelBuilder);

        // 1:1 relationship between ItemLens and SaleItem
        modelBuilder.Entity<ItemLens>()
            .HasOne(itemLens => itemLens.SaleItem)
            .WithOne()
            .HasForeignKey<ItemLens>(itemLens => itemLens.SaleItemId);
    }

    private static void ConfigureUserOpticalStoreCK(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserOpticalStore>()
            .HasKey(uos => new { uos.UserId, uos.OpticalStoreId });

        modelBuilder.Entity<UserOpticalStore>()
            .HasOne(uos => uos.User)
            .WithMany()
            .HasForeignKey(uos => uos.UserId);

        modelBuilder.Entity<UserOpticalStore>()
            .HasOne(uos => uos.OpticalStore)
            .WithMany()
            .HasForeignKey(uos => uos.OpticalStoreId);
    }

    private static void ConfigureItemLensTreatmentCK(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ItemLensTreatment>()
            .HasKey(itemLensTreatment => new
            {
                itemLensTreatment.ItemLensId,
                itemLensTreatment.TreatmentId
            });

        modelBuilder.Entity<ItemLensTreatment>()
            .HasOne(itemLensTreatment => itemLensTreatment.ItemLens)
            .WithMany(itemLens => itemLens.Treatments)
            .HasForeignKey(itemLensTreatment => itemLensTreatment.ItemLensId);

        modelBuilder.Entity<ItemLensTreatment>()
            .HasOne(itemLensTreatment => itemLensTreatment.Treatment)
            .WithMany(treatment => treatment.ItemLensTreatments)
            .HasForeignKey(itemLensTreatment => itemLensTreatment.TreatmentId);
    }
}
