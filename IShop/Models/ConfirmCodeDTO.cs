using System.ComponentModel.DataAnnotations;

namespace IShop.Models
{
    public class ConfirmCodeDTO
    {
        [Required]
        public int Number { get; set; }
        public string Email { get; set; }
    }
}
