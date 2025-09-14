using SportsEcommerceAPI.Data;
using SportsEcommerceAPI.Models;

namespace SportsEcommerceAPI.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;

        public DataSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Seed categories
            if (!_context.Categories.Any())
            {
                _context.Categories.AddRange(
                    new Category { Name = "Football" },
                    new Category { Name = "Basketball" },
                    new Category { Name = "Volleyball" }
                );
                await _context.SaveChangesAsync();
            }

            // Seed products
            if (!_context.Products.Any())
            {
                var footballCategory = _context.Categories.First(c => c.Name == "Football");
                var basketballCategory = _context.Categories.First(c => c.Name == "Basketball");
                var volleyballCategory = _context.Categories.First(c => c.Name == "Volleyball");

                _context.Products.AddRange(
                    new Product
                    {
                        Name = "Football Ball",
                        Price = 50.00m,
                        CategoryId = footballCategory.CategoryId
                    },
                    new Product
                    {
                        Name = "Soccer Shoes",
                        Price = 120.00m,
                        CategoryId = footballCategory.CategoryId
                    },
                    new Product
                    {
                        Name = "Goalkeeper Gloves",
                        Price = 35.00m,
                        CategoryId = footballCategory.CategoryId
                    },
                    new Product
                    {
                        Name = "Basketball Ball",
                        Price = 60.00m,
                        CategoryId = basketballCategory.CategoryId
                    },
                    new Product
                    {
                        Name = "Basketball Shoes",
                        Price = 150.00m,
                        CategoryId = basketballCategory.CategoryId
                    },
                    new Product
                    {
                        Name = "Hoop Shooting Set",
                        Price = 80.00m,
                        CategoryId = basketballCategory.CategoryId
                    },
                    new Product
                    {
                        Name = "Volleyball Ball",
                        Price = 45.00m,
                        CategoryId = volleyballCategory.CategoryId
                    },
                    new Product
                    {
                        Name = "Knee Pads",
                        Price = 30.00m,
                        CategoryId = volleyballCategory.CategoryId
                    },
                    new Product
                    {
                        Name = "Volleyball Jersey",
                        Price = 75.00m,
                        CategoryId = volleyballCategory.CategoryId
                    }
                );

                await _context.SaveChangesAsync();
            }

            // Seed users
            if (!_context.Users.Any())
            {
                _context.Users.AddRange(
                    new User { FullName = "Sarah Williams", Email = "sarah@example.com", Password = "sarah123!" },
                    new User { FullName = "David Brown", Email = "david@example.com", Password = "david456@" },
                    new User { FullName = "Jessica Davis", Email = "jessica@example.com", Password = "jessica789#" },
                    new User { FullName = "Christopher Wilson", Email = "chris@example.com", Password = "chrisABC$" },
                    new User { FullName = "Amanda Taylor", Email = "amanda@example.com", Password = "amandaXYZ%" }
                );

                await _context.SaveChangesAsync();
            }

            // Seed cart items
            if (!_context.CartItems.Any())
            {
                var users = _context.Users.ToList();
                var products = _context.Products.ToList();

                _context.CartItems.AddRange(
                    new CartItem { UserId = users[0].UserId, ProductId = products[1].ProductId, Quantity = 1 }, // Merve - Soccer Shoes
                    new CartItem { UserId = users[1].UserId, ProductId = products[3].ProductId, Quantity = 2 }, // Ersel - Basketball Ball
                    new CartItem { UserId = users[2].UserId, ProductId = products[6].ProductId, Quantity = 1 }, // Ikra - Volleyball Ball
                    new CartItem { UserId = users[3].UserId, ProductId = products[0].ProductId, Quantity = 1 }, // Deniz - Football Ball
                    new CartItem { UserId = users[4].UserId, ProductId = products[4].ProductId, Quantity = 1 }  // Eylul - Basketball Shoes
                );

                await _context.SaveChangesAsync();
            }

            // Seed orders
            if (!_context.Orders.Any())
            {
                var users = _context.Users.ToList();

                _context.Orders.AddRange(
                    new Order { UserId = users[0].UserId, Total = 120.00m }, // Merve
                    new Order { UserId = users[1].UserId, Total = 120.00m }, // Ersel
                    new Order { UserId = users[2].UserId, Total = 45.00m }   // Ikra
                );

                await _context.SaveChangesAsync();
            }

            // Seed order items
            if (!_context.OrderItems.Any())
            {
                var orders = _context.Orders.ToList();
                var products = _context.Products.ToList();

                _context.OrderItems.AddRange(
                    new OrderItem { OrderId = orders[0].OrderId, ProductId = products[1].ProductId, Quantity = 1, Price = 120.00m }, // Merve - Soccer Shoes
                    new OrderItem { OrderId = orders[1].OrderId, ProductId = products[3].ProductId, Quantity = 2, Price = 60.00m },  // Ersel - Basketball Ball
                    new OrderItem { OrderId = orders[2].OrderId, ProductId = products[6].ProductId, Quantity = 1, Price = 45.00m }   // Ikra - Volleyball Ball
                );

                await _context.SaveChangesAsync();
            }
        }
    }
}