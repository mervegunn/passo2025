using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsEcommerceAPI.Data;
using SportsEcommerceAPI.DTOs;
using SportsEcommerceAPI.Models;
using SportsEcommerceAPI.Services;
using System.Security.Claims;

namespace SportsEcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;

        public CartController(ApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                throw new UnauthorizedAccessException("Invalid user token");
            return userId;
        }

        [HttpGet]
        public async Task<ActionResult<CartResponse>> GetCart()
        {
            var userId = GetCurrentUserId();
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .ThenInclude(p => p.Category)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            var items = cartItems.Select(ci => new CartItemResponse
            {
                CartItemId = ci.CartItemId,
                UserId = ci.UserId,
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                Product = new ProductResponse
                {
                    ProductId = ci.Product.ProductId,
                    CategoryId = ci.Product.CategoryId,
                    Name = ci.Product.Name,
                    Price = ci.Product.Price,
                    Category = new CategoryResponse
                    {
                        CategoryId = ci.Product.Category.CategoryId,
                        Name = ci.Product.Category.Name
                    }
                }
            }).ToList();

            var totalAmount = items.Sum(i => i.Product.Price * i.Quantity);
            var totalItems = items.Sum(i => i.Quantity);

            return Ok(new CartResponse
            {
                Items = items,
                TotalAmount = totalAmount,
                TotalItems = totalItems
            });
        }

        [HttpPost]
        public async Task<ActionResult<CartItemResponse>> AddToCart(AddToCartRequest request)
        {
            var userId = GetCurrentUserId();
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == request.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    UserId = userId,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstAsync(p => p.ProductId == request.ProductId);

            var response = new CartItemResponse
            {
                CartItemId = existingItem?.CartItemId ?? _context.CartItems
                    .First(ci => ci.UserId == userId && ci.ProductId == request.ProductId).CartItemId,
                UserId = userId,
                ProductId = request.ProductId,
                Quantity = existingItem?.Quantity ?? request.Quantity,
                Product = new ProductResponse
                {
                    ProductId = product.ProductId,
                    CategoryId = product.CategoryId,
                    Name = product.Name,
                    Price = product.Price,
                    Category = new CategoryResponse
                    {
                        CategoryId = product.Category.CategoryId,
                        Name = product.Category.Name
                    }
                }
            };

            return Ok(response);
        }

        [HttpPut("{cartItemId}")]
        public async Task<ActionResult<CartItemResponse>> UpdateCartItem(int cartItemId, UpdateCartItemRequest request)
        {
            var cartItem = await _context.CartItems
                .Include(ci => ci.Product)
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);

            if (cartItem == null)
                return NotFound();

            cartItem.Quantity = request.Quantity;
            await _context.SaveChangesAsync();

            var response = new CartItemResponse
            {
                CartItemId = cartItem.CartItemId,
                UserId = cartItem.UserId,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                Product = new ProductResponse
                {
                    ProductId = cartItem.Product.ProductId,
                    CategoryId = cartItem.Product.CategoryId,
                    Name = cartItem.Product.Name,
                    Price = cartItem.Product.Price,
                    Category = new CategoryResponse
                    {
                        CategoryId = cartItem.Product.Category.CategoryId,
                        Name = cartItem.Product.Category.Name
                    }
                }
            };

            return Ok(response);
        }

        [HttpDelete("{cartItemId}")]
        public async Task<ActionResult> RemoveFromCart(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null)
                return NotFound();

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}