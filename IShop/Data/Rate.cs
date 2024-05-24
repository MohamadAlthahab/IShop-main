using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IShop.Data
{
    public class Rate
    {
        [Key]
        public int Id { get; set; }
        public double Ratee { get; set; }

        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public User user { get; set; }

        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product product { get; set; }
    }
}
