using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Data
{
    public class ShopContext:IdentityDbContext<User,Role,int>
    {
        public ShopContext(DbContextOptions<ShopContext> options):base(options)
        {
            
        }

         public DbSet<Product> Products{ get; set; }

         public DbSet<Customer> Customers { get; set; }


         public DbSet<Address> Addresses { get; set; }

    public DbSet<ProductCategory> ProductCategories { get; set; }
         public DbSet<Supplier> Suppliers { get; set; }
        
         public DbSet<Category> Categories { get; set; }
         public DbSet<Image> Images { get; set; }

          protected override void OnModelCreating(ModelBuilder builder) 
        {
          base.OnModelCreating(builder);
         

            builder.Entity<ProductCategory>()
                .HasKey(k=> new {k.ProductId,k.CategoryId});

            builder.Entity<ProductCategory>()
                  .HasOne(pc=>pc.Product)
                  .WithMany(p=>p.ProductCategories)
                  .HasForeignKey(pc=>pc.ProductId);

                  builder.Entity<ProductCategory>()
                  .HasOne(pc=>pc.Category)
                  .WithMany(c=>c.ProductCategories)
                  .HasForeignKey(pc=>pc.CategoryId);

             builder.Entity<Product>()
            .HasOne(p => p.Supplier)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.SupplierId);
                  


            
        }
 

    }
}