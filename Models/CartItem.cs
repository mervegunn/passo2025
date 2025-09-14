using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsEcommerceAPI.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;

        [ForeignKey("UserId")]
        public User User { get; set; }
        
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}