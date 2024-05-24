using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace IShop.Data
{
    public class CompanyDetails
    {
        [Key]
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public string CommercialNumber { get; set; }

        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
    }
}
