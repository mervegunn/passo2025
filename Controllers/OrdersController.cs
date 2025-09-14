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
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;

        public OrdersController(ApplicationDbContext context, IAuthService authService)
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

        [HttpPost]
        public async Task<ActionResult<OrderResponse>> CreateOrder(CreateOrderRequest request)
        {
            var userId = GetCurrentUserId();
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Total = request.Total
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderItems = request.OrderItems.Select(oi => new OrderItem
            {
                OrderId = order.OrderId,
                ProductId = oi.ProductId,
                Quantity = oi.Quantity,
                Price = oi.Price
            }).ToList();

            _context.OrderItems.AddRange(orderItems);
            await _context.SaveChangesAsync();

            var orderWithItems = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.Category)
                .FirstAsync(o => o.OrderId == order.OrderId);

            var response = new OrderResponse
            {
                OrderId = orderWithItems.OrderId,
                UserId = orderWithItems.UserId,
                OrderDate = orderWithItems.OrderDate,
                Total = orderWithItems.Total,
                OrderItems = orderWithItems.OrderItems.Select(oi => new OrderItemResponse
                {
                    OrderItemId = oi.OrderItemId,
                    OrderId = oi.OrderId,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    Product = new ProductResponse
                    {
                        ProductId = oi.Product.ProductId,
                        CategoryId = oi.Product.CategoryId,
                        Name = oi.Product.Name,
                        Price = oi.Product.Price,
                        Category = new CategoryResponse
                        {
                            CategoryId = oi.Product.Category.CategoryId,
                            Name = oi.Product.Category.Name
                        }
                    }
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderResponse>>> GetUserOrders()
        {
            var userId = GetCurrentUserId();
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.Category)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var response = orders.Select(o => new OrderResponse
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                OrderDate = o.OrderDate,
                Total = o.Total,
                OrderItems = o.OrderItems.Select(oi => new OrderItemResponse
                {
                    OrderItemId = oi.OrderItemId,
                    OrderId = oi.OrderId,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    Product = new ProductResponse
                    {
                        ProductId = oi.Product.ProductId,
                        CategoryId = oi.Product.CategoryId,
                        Name = oi.Product.Name,
                        Price = oi.Product.Price,
                        Category = new CategoryResponse
                        {
                            CategoryId = oi.Product.Category.CategoryId,
                            Name = oi.Product.Category.Name
                        }
                    }
                }).ToList()
            }).ToList();

            return Ok(response);
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<OrderResponse>> GetOrder(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                return NotFound();

            var response = new OrderResponse
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Total = order.Total,
                OrderItems = order.OrderItems.Select(oi => new OrderItemResponse
                {
                    OrderItemId = oi.OrderItemId,
                    OrderId = oi.OrderId,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    Product = new ProductResponse
                    {
                        ProductId = oi.Product.ProductId,
                        CategoryId = oi.Product.CategoryId,
                        Name = oi.Product.Name,
                        Price = oi.Product.Price,
                        Category = new CategoryResponse
                        {
                            CategoryId = oi.Product.Category.CategoryId,
                            Name = oi.Product.Category.Name
                        }
                    }
                }).ToList()
            };

            return Ok(response);
        }
    }
}