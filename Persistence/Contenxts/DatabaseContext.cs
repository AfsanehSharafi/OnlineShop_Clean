using Application.Interfaces.Contexts;
using Domain.Attributes;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Contenxts
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        //public DbSet<CatalogBrand> CatalogBrands { get; set; }
        //public DbSet<CatalogType> CatalogTypes { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var EntityType in builder.Model.GetEntityTypes())
            {
                if (EntityType.ClrType.GetCustomAttributes(typeof(AuditableAttribute), true).Length > 0)
                {
                    builder.Entity(EntityType.Name).Property<DateTime>("InsertTime");
                    builder.Entity(EntityType.Name).Property<DateTime?>("UpdateTime");
                    builder.Entity(EntityType.Name).Property<DateTime?>("RemoveTime");
                    builder.Entity(EntityType.Name).Property<bool>("IsRemove");

                }
            }
            //    foreach (var relationship in builder.Model.GetEntityTypes().
            //   SelectMany(e => e.GetForeignKeys()))
            //    {
            //        relationship.DeleteBehavior = DeleteBehavior.Restrict;
            //    }
            //    builder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
            //    builder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
            //    DatabaseContextSeed.CatalogSeed(builder);

            //DataBaseContextSeed.CatalogSeed(builder);

            base.OnModelCreating(builder);
        }



        public override int SaveChanges()
        {
            var ModifiedEntries = ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Modified ||
                p.State == EntityState.Added ||
                p.State == EntityState.Deleted
                );
            foreach (var item in ModifiedEntries)
            {
                var entityType = item.Context.Model.FindEntityType(item.Entity.GetType());
                var inserted = entityType.FindProperty("InsertTime");
                var updated = entityType.FindProperty("UpdateTime");
                var removeTime = entityType.FindProperty("RemoveTime");
                var isRemoved = entityType.FindProperty("IsRemoved");
                if (item.State == EntityState.Added && inserted != null)
                {
                    item.Property("InsertTime").CurrentValue = DateTime.Now;
                }

                if (item.State == EntityState.Modified && updated != null)
                {
                    item.Property("UpdateTime").CurrentValue = DateTime.Now;
                }

                if (item.State == EntityState.Deleted && removeTime != null)
                {
                    item.Property("RemoveTime").CurrentValue = DateTime.Now;
                    item.Property("IsRemoved").CurrentValue = true;
                    item.State = EntityState.Modified;
                }





            }
            return base.SaveChanges();

        }

    }

}