using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace IShop.Data
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public override string Email { get; set; }

        [ForeignKey("CartId")]
        public int CartId { get; set; }
        public Cart Cart { get; set; }

        [ForeignKey("FavoriteId")]
        public int FavoriteId { get; set; }
        public Favorite Favorite { get; set;}
    }
}
