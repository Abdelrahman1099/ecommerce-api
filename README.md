# E-Commerce RESTful Web API

A scalable E-Commerce Web API built with ASP.NET Core (.NET 8) and Clean Architecture principles. It features Redis caching, JWT authentication, and Stripe payment integration.

---

## Architecture and Core Concepts

This project follows Clean Architecture (Onion Architecture) to ensure maintainability, testability, and a clear separation of concerns:

- **Domain Layer (E_Commerce.Domain):** Core business entities, specifications, and domain interfaces.
- **Application Layer (E_Commerce.Application):** Business logic, DTOs, service abstractions, and mapping profiles.
- **Infrastructure Layer (E_Commerce.Infrastructure):** Database context, EF Core configurations, Redis implementations, Identity, and external service integrations (Stripe).
- **API Layer (E_Commerce.API):** Controllers, middlewares, dependency injection setup, and endpoint configuration.

### Key Patterns and Principles

- Repository and Unit of Work patterns for data access abstraction and transactional operations.
- Specification pattern for dynamic query building, filtering, and pagination.
- Dependency Injection for loose coupling and easy extensibility.

---

## Key Features

- **Authentication and Authorization:** JWT token authentication powered by ASP.NET Core Identity.
- **Product Management:** CRUD operations with pagination, sorting, and filtering by brand and type.
- **Basket and Caching:** Shopping cart management backed by Redis.
- **Payment Integration:** Checkout process using the Stripe SDK.
- **Order Management:** Order lifecycle handling, including status updates and user order history.
- **Global Error Handling:** Custom middleware that handles exceptions with a standard response structure.

---

## Tech Stack and Tooling

- **Framework:** .NET 8 (ASP.NET Core Web API)
- **Language:** C#
- **Database:** Microsoft SQL Server
- **ORM:** Entity Framework Core (Code First and Fluent API)
- **Caching:** Redis
- **Security:** ASP.NET Core Identity, JWT Bearer Tokens
- **Payments:** Stripe API SDK
- **Documentation:** Swagger UI / OpenAPI

---

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server
- Redis (running via WSL, Docker, or as a native Windows service)
- A Stripe account (test mode is enough)

### Installation and Setup

1. **Clone the repository:**

   ```bash
   git clone https://github.com/Abdelrahman1099/ecommerce-api.git
   cd ecommerce-api
   ```

2. **Configure the connection strings** in `appsettings.json` inside the API project:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=ECommerceDb;Trusted_Connection=True;TrustServerCertificate=True;",
     "Redis": "localhost:6379"
   }
   ```

3. **Configure the Stripe secrets** (see the next section).

4. **Apply the database migrations:**

   ```bash
   dotnet ef database update --project E_Commerce.Infrastructure --startup-project E_Commerce.API
   ```

5. **Run the application:**

   ```bash
   dotnet run --project E_Commerce.API
   ```

   Then open `https://localhost:[port]/swagger` to explore the endpoints through Swagger UI.

---

## Secrets Configuration

Sensitive values are **not** stored in this repository. The `appsettings.json` file contains placeholders only.

Use [.NET User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) for local development:

```bash
cd E_Commerce.API
dotnet user-secrets init
dotnet user-secrets set "Stripe:Secretkey" "your_stripe_secret_key"
dotnet user-secrets set "Stripe:WebhookSecret" "your_stripe_webhook_secret"
```

In Visual Studio, you can also right-click the API project and choose **Manage User Secrets**.

You can get your test keys from the [Stripe Dashboard](https://dashboard.stripe.com/test/apikeys).

---

## License

This project is open-source and available under the MIT License.