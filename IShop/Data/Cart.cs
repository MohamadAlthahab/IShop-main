using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IShop.Data
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int Count { get; set; }
        public double Total { get; set; }

    }
}
