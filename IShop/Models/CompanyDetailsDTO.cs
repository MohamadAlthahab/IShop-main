using IShop.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace IShop.Models
{
    public class CreateCompanyDetailsDTO
    {
        [Required]
        [StringLength(50)]
        public string CompanyName { get; set; }
        public string CommercialNumber { get; set; }
    }

    public class CompanyDetailsDTO : CreateCompanyDetailsDTO
    {
        [Key]
        public int ID { get; set; }
        public UserDTO User { get; set; }
        public int UserID { get; set; }
    }
}
