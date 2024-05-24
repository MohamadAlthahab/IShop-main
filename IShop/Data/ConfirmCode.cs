using System.ComponentModel.DataAnnotations.Schema;

namespace IShop.Data
{
    public class ConfirmCode
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
