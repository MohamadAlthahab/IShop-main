using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using 
    Microsoft.EntityFrameworkCore;

namespace IShop.Data
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {    }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Favorite> Favorite { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<Rate> Rate { get; set; }
        public DbSet<Street> Street { get; set; }
        public DbSet<CompanyDetails> CompanyDetails { get; set; }
        public DbSet<Favorite_Product> Favorite_Product { get; set; }
        public DbSet<Cart_Product> Cart_Products { get; set; }
        public DbSet<Log_Product> Log_Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ConfirmCode> ConfirmCode { get; set; }
        public DbSet<Reset_Password> ResetPassword { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Street>().HasData(
                new Street
                {
                    Id = 1,
                    Name = "دمشق",
                    Price = 15000,
                },
                new Street
                {
                    Id = 2,
                    Name = "ريف دمشق",
                    Price = 17500,
                },
                new Street
                {
                    Id = 3,
                    Name = "حمص",
                    Price = 12000,
                },
                new Street
                {
                    Id = 4,
                    Name = "حماه",
                    Price = 12500,
                },
                new Street
                {
                    Id = 5,
                    Name = "طرطوس",
                    Price = 10000,
                },
                new Street
                {
                    Id = 6,
                    Name = "اللاذقية",
                    Price = 15000,
                },
                new Street
                {
                    Id = 7,
                    Name = "حلب",
                    Price = 20000,
                },
                new Street
                {
                    Id = 8,
                    Name = "درعا",
                    Price = 15000,
                },
                new Street
                {
                    Id = 9,
                    Name = "السويداء",
                    Price = 18000,
                },
                new Street
                {
                    Id = 10,
                    Name = "دير الزور",
                    Price = 20000,
                },
                new Street
                {
                    Id = 11,
                    Name = "الحسكة",
                    Price = 8000,
                },
                new Street
                {
                    Id = 12,
                    Name = "الرقة",
                    Price = 9000,
                },
                new Street
                {
                    Id = 13,
                    Name = "القنيطرة",
                    Price = 11000,
                },
                new Street
                {
                    Id = 14,
                    Name = "ادلب",
                    Price = 14000,
                });
            

            builder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "ألبسة",
                },
                new Category
                {
                    Id = 2,
                    Name = "منظفات",
                },
                new Category
                {
                    Id = 3,
                    Name = "الاكترونيات",
                },
                new Category
                {
                    Id = 4,
                    Name = "عطورات",
                },
                new Category
                {
                    Id = 5,
                    Name = "مكياجات",
                },
                new Category
                {
                    Id = 6,
                    Name = "أحذية",
                },
                new Category
                {
                    Id = 7,
                    Name = "كهربائيات",
                }
                );

            builder.ApplyConfiguration(new Role());
        }
    }
}
