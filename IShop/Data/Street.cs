using System.ComponentModel.DataAnnotations;

namespace IShop.Data
{
    public class Street
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
