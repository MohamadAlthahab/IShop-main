using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IShop.Data
{
    public class Log_Product
    {
        [Key]
        public int Id { get; set; }
        public int LogId { get; set; }
        [ForeignKey("LogId")]
        public Log log { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product product { get; set; }

    }
}
