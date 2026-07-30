# Book API

API for managing users and books built with ASP.NET Core.

The project was created to learn the fundamentals of backend development.

## Features

- JWT authentication
- Role-based authorization
- CRUD operations for books
- User management

## Technologies

- C#
- ASP.NET
- SQL Server
- Entity Framework Core
- JWT

## Endpoints

POST /Auth/login
POST /Auth/register

GET /Book
POST /Book
GET /Book/{id}
PUT /Book/{id}
DELETE /Book/{id}

GET /User
GET /User/{id}
DELETE /User/{id}
PATCH /User/{id}/role

## Test administrator account

**Login**

```text
admin
```

**Password**

```text
Admin123!
```

## How to run

1. Clone the repository.
2. Open the solution in Visual Studio.
3. Build and run the application.
4. On the first launch the database is created automatically.
5. Use the administrator account to sign in.
6. Copy JWT token after login and paste in "Authorize".

## Author

Yehor Radykop