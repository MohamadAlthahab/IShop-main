using System.ComponentModel.DataAnnotations;

namespace IShop.Models
{
    public class CreateRoleDTO
    {
        [Required]
        public string Name { get; set; }
    }

    public class RoleDTO : CreateRoleDTO
    {
        [Key]
        public int Id { get; set; }
    }
}
