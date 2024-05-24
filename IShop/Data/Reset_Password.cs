using System.ComponentModel.DataAnnotations;

namespace IShop.Data
{
    public class Reset_Password
    {
        [Key]
        public int Id { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public int Code { get; set; }
        public string Token { get; set; }
    }
}
