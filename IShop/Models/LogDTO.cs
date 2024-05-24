using IShop.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace IShop.Models
{
    public class CreateLogDTO
    {
        
    }

    public class LogDTO : CreateLogDTO
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public Cart Cart { get; set; }
        public int UserId { get; set; }
        public UserDTO user { get; set; }
        
        
        
    }
}
