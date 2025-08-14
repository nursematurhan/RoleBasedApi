# RoleBasedApi

RoleBasedApi is an ASP.NET Core Web API project that implements **JWT-based authentication** and **role-based authorization** using **Keycloak**.  
Users can log in with different roles (`Customer`, `Seller`, `Admin`) and access API endpoints according to their assigned permissions.

## 🚀 Features
- **JWT Authentication** with Keycloak integration
- **Role-Based Authorization** using `[Authorize(Roles = "...")]`
- Data access with **Entity Framework Core**
- **SQL Server** database support
- **CORS** configuration for development and cross-origin requests
- API documentation with **Swagger UI** and Bearer token support
- Sample endpoints for:
  - Product management (`ProductsController`)
  - Order management (`OrdersController`)
  - Report viewing with a custom policy (`ReportsController`)

## 🛠 Technologies
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Keycloak (via Docker)
- Docker
- Swagger / Swashbuckle

## 📌 Roles and Permissions
| Role       | Permissions |
|------------|-------------|
| **Customer** | Create orders, list products |
| **Seller**   | Add, update, delete products they own |
| **Admin**    | Full access to all operations, including reports |


## 🔐 Keycloak Setup (Docker)
1. **Run Keycloak in Docker**
```bash
docker run -d --name keycloak -p 8080:8080 \
  -e KEYCLOAK_ADMIN=admin \
  -e KEYCLOAK_ADMIN_PASSWORD=admin \
  quay.io/keycloak/keycloak:latest start-dev

