using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IShop.Models
{
    public class SelectStreetDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

    }

    public class StreetDTO : SelectStreetDTO
    {
        [Key]
        public int Id { get; set; }
        [Column("Price")]
        public double Price { get; set; }
    }
}
