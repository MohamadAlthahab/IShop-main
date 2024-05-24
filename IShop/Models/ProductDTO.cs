using IShop.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace IShop.Models
{
    public class CreateProductDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Quntity { get; set; }
        [Required]
        [StringLength(200)]
        public string Description { get; set; }
        public string Size { get; set; }
        public int CategoryId { get; set; }
        //public string UserId { get; set; }
        [Required]
        public List<IFormFile> ImageProduct { get; set; }



        // public ICollection<string> Category { get; set; }
    }

    public class UpdateProductDTO 
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Quntity { get; set; }
        [Required]
        [StringLength(100)]
        public string Description { get; set; }
    }
    public class DeleteProductDTO 
    {
        public int Id { get; set; }
    }
    public class ProductDTO 
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Quntity { get; set; }
        [Required]
        public string Size { get; set; }
        [Required]
        [StringLength(100)]
        public string Description { get; set; }

       public byte[] Image { get; set; }
        
    }
}
