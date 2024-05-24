using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace IShop.Data
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }

        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public User user { get; set; }
    }
}
