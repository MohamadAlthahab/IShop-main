using System.ComponentModel.DataAnnotations.Schema;

namespace IShop.Data
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

    }
}
