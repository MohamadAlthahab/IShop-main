using IShop.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IShop.Models
{
    public class CreateCategoryDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
    }

    public class CategoryDTO : CreateCategoryDTO
    {
        [Key]
        public int Id { get; set; }
        public virtual IList<Product> Products { get; set; }
    }
}
