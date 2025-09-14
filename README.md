# Sports E-commerce API

A simple sports e-commerce API built with .NET 9.0, PostgreSQL, and Docker.

## Features

- **Products Management**: Browse and filter sports products by category
- **Shopping Cart**: Add, update, and remove items from cart
- **Order Management**: Create and view orders
- **User Authentication**: JWT-based authentication
- **Categories**: Football, Basketball, and Volleyball products

## Quick Start

### Prerequisites

- Docker and Docker Compose installed on your machine

### Running the Application

1. Start the application:
   ```bash
   docker-compose up --build
   ```

2. The API will be available at:
   - **API**: http://localhost:8080
   - **Swagger UI**: http://localhost:8080/swagger

That's it! The database will be automatically created and seeded with sample data.

## Mobile App Integration

This API is designed to work with mobile applications and includes:

- **CORS Support**: Configured to allow requests from mobile apps
- **RESTful API**: Standard HTTP methods for all operations
- **JSON Responses**: Mobile-friendly data format
- **JWT Authentication**: Secure token-based authentication for mobile users
- **Error Handling**: Proper HTTP status codes and error messages

## API Endpoints

### Authentication (Public)
- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login user
- `POST /api/auth/validate` - Validate JWT token

### Products (Public)
- `GET /api/products` - Get all products (with filtering)
- `GET /api/products/{id}` - Get product by ID
- `GET /api/products/categories` - Get all categories

### Cart (Protected - Requires JWT Token)
- `GET /api/cart` - Get current user's cart
- `POST /api/cart` - Add item to cart
- `PUT /api/cart/{cartItemId}` - Update cart item quantity
- `DELETE /api/cart/{cartItemId}` - Remove item from cart

### Orders (Protected - Requires JWT Token)
- `POST /api/orders` - Create new order
- `GET /api/orders` - Get current user's orders
- `GET /api/orders/order/{orderId}` - Get specific order

## Authentication

The API uses JWT (JSON Web Tokens) for authentication. Some endpoints are public (like viewing products), while others require authentication (like cart and orders).

### How to Use Authentication

1. **Register a new user:**
   ```bash
   curl -X POST http://localhost:8080/api/auth/register \
     -H "Content-Type: application/json" \
     -d '{"fullName": "Michael Johnson", "email": "michael@example.com", "password": "password123"}'
   ```

2. **Login:**
   ```bash
   curl -X POST http://localhost:8080/api/auth/login \
     -H "Content-Type: application/json" \
     -d '{"email": "michael@example.com", "password": "password123"}'
   ```

3. **Use the token in protected endpoints:**
   ```bash
   curl -H "Authorization: Bearer YOUR_JWT_TOKEN" \
     http://localhost:8080/api/cart
   ```

### Protected vs Public Endpoints

- **Public Endpoints** (no authentication required):
  - All `/api/products/*` endpoints
  - All `/api/auth/*` endpoints

- **Protected Endpoints** (JWT token required):
  - All `/api/cart/*` endpoints
  - All `/api/orders/*` endpoints

## Sample Data

The application comes pre-loaded with:

### Users
- Sarah Williams (sarah@example.com)
- David Brown (david@example.com)
- Jessica Davis (jessica@example.com)
- Christopher Wilson (chris@example.com)
- Amanda Taylor (amanda@example.com)

### Categories
- Football
- Basketball
- Volleyball

### Products
- Football Ball, Soccer Shoes, Goalkeeper Gloves
- Basketball Ball, Basketball Shoes, Hoop Shooting Set
- Volleyball Ball, Knee Pads, Volleyball Jersey

## Technology Stack

- **.NET 9.0** - Web API framework
- **PostgreSQL** - Database
- **Entity Framework Core** - ORM
- **Docker** - Containerization
- **JWT** - Authentication
- **Swagger** - API documentation

## Database Schema

The application uses a simplified schema with the following entities:
- **Users**: UserId, FullName, Email, Password
- **Categories**: CategoryId, Name
- **Products**: ProductId, CategoryId, Name, Price
- **CartItems**: CartItemId, UserId, ProductId, Quantity
- **Orders**: OrderId, UserId, OrderDate, Total
- **OrderItems**: OrderItemId, OrderId, ProductId, Quantity, Price

## Development

The application automatically:
1. Creates the database on startup
2. Seeds it with sample data
3. Handles all database migrations

No manual setup required!