using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace IShop.Data
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quntity { get; set; }
        public string Size { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }
        public string ImageProduct { get; set; }
        public byte[] Image { get; set; }

    }
}
