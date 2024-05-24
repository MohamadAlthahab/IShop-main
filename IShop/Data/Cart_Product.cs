using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IShop.Data
{
    public class Cart_Product
    {
        [Key]
        public int Id { get; set; }
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public Cart cart { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product product { get; set; }
    }
}
