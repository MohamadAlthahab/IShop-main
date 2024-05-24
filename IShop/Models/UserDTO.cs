using IShop.Data;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IShop.Models
{

    public class LoginUserDTO
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class UserDTO : LoginUserDTO
    {
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(10)]
        public string Phone { get; set; }

        public int CartId { get; set; }
        public int FavoriteId { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
