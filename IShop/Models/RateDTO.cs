using IShop.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IShop.Models
{
    public class CreateRateDTO
    {
        [Required]
        [Range(1,5)]
        public double Ratee { get; set; }  
    }

    public class RateDTO : CreateRateDTO
    {
        [Key]
        public int Id { get; set; }
        public UserDTO user { get; set; }
        public ProductDTO product { get; set; }
    }
}
