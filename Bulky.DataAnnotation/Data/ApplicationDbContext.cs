using BulkyBook.Model.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ShopingCart> ShopingCart { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "action", DisplayOrder = 1, },
                new Category { Id = 2, Name = "scifi", DisplayOrder = 2, },
                new Category { Id = 3, Name = "history", DisplayOrder = 3, }
                );
            modelBuilder.Entity<Product>().HasData(
               new Product
               {
                   Id = 1,
                   Title = "Srinivasa Kalyana",
                   Author = "Venkateswara Swamy",
                   Description = "Story of Srinivasa Marriage",
                   ISBN = "ISBN00091",
                   ListPrice = 200,
                   Price = 150,
                   Price50 = 55,
                   Price100 = 60,
                   CategoryId = 2,
                   ImageUrl = ""
               },
                  new Product
                  {
                      Id = 2,
                      Title = "Yogi AatmaKadha",
                      Author = "Yogi Swamy",
                      Description = "Story of Yogi",
                      ISBN = "ISBN00071",
                      ListPrice = 300,
                      Price = 130,
                      Price50 = 75,
                      Price100 = 90,
                      CategoryId = 4,
                      ImageUrl = ""
                  }
               );
        }

    }
}
