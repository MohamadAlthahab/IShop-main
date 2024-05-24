using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IShop.Data
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }
        public Product Product { get; set; }

    }
}
