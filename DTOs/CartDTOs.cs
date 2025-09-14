using System.ComponentModel.DataAnnotations;

namespace SportsEcommerceAPI.DTOs
{
    public class AddToCartRequest
    {
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

    public class UpdateCartItemRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

    public class CartItemResponse
    {
        public int CartItemId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public ProductResponse Product { get; set; } = null!;
    }

    public class CartResponse
    {
        public List<CartItemResponse> Items { get; set; } = new List<CartItemResponse>();
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
    }
}