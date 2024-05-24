using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace IShop.Data
{
    public class Role : IEntityTypeConfiguration<IdentityRole>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

         public void Configure(EntityTypeBuilder<IdentityRole> builder)
         {
             builder.HasData(
                 new IdentityRole
                 {
                     Name = "Customer",
                     NormalizedName = "Customer"
                 },
                 new IdentityRole
                 {
                     Name = "Seller",
                     NormalizedName = "Seller"
                 });
         }
    }
}
