using Domain.Catalogs;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces.Contexts
{
    public interface IDatabaseContext
    {
        DbSet<CatalogBrand> CatalogBrands { get; }
        DbSet<CatalogType> CatalogTypes { get; }
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }

}
