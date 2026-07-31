using GOtica.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess;

internal class GOticaDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<OpticalStore> OpticalStores { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserOpticalStore> UserOpticalStores { get; set; }
}
