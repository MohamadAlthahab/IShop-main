using IShop.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IShop.Models
{
    public class CreateFavoriteDTO
    {
        [Required]
        public int UserId { get; set; }
    }

    public class FavoriteDTO : CreateFavoriteDTO
    {
        [Key]
        public int Id { get; set; }
        public UserDTO User { get; set; }
    }
}
