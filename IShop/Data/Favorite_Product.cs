using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IShop.Data
{
    public class Favorite_Product
    {
        [Key]
        public int Id { get; set; }

        public int FavoriteId { get; set; }
        [ForeignKey("FavoriteId")]
        public Favorite Favorite { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product product { get; set; }
    }
}
