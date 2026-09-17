# Global Exception Handling Practice

A beginner-friendly ASP.NET Core Web API project demonstrating global exception handling using custom middleware, custom exceptions, logging, HTTP status codes, and ProblemDetails.

## Overview

This project demonstrates how to handle exceptions centrally in an ASP.NET Core Web API instead of handling them separately inside every controller action.

The middleware catches unhandled exceptions, logs them, maps them to appropriate HTTP status codes, and returns a consistent JSON error response using `ProblemDetails`.

## Technologies

* C#
* ASP.NET Core Web API
* Custom Middleware
* Custom Exceptions
* ProblemDetails
* ILogger
* Swagger / OpenAPI

## Project Structure

```text
GlobalExceptionPractice
│
├── Controllers
│   └── ProductsController.cs
│
├── Exceptions
│   ├── NotFoundException.cs
│   └── BadRequestException.cs
│
├── Middleware
│   └── ExceptionHandlingMiddleware.cs
│
├── Program.cs
└── README.md
```

## Key Concepts

### 1. Custom Middleware

`ExceptionHandlingMiddleware` is responsible for handling exceptions centrally.

Instead of writing `try/catch` blocks inside every controller, exceptions are caught by the middleware.

```text
Request
   ↓
ExceptionHandlingMiddleware
   ↓
Controller
   ↓
Exception
   ↓
Middleware catches exception
   ↓
Create error response
   ↓
Client
```

### 2. RequestDelegate

The middleware receives a `RequestDelegate` called `_next`.

```csharp
await _next(context);
```

This passes the request to the next component in the ASP.NET Core request pipeline.

The middleware wraps this call inside a `try/catch` so exceptions thrown by downstream components can be handled centrally.

### 3. Custom Exceptions

The project contains custom exceptions for specific application errors:

* `NotFoundException` → HTTP 404
* `BadRequestException` → HTTP 400

This allows the middleware to distinguish between different types of errors.

### 4. Exception Logging

Unhandled exceptions are logged using `ILogger`.

```csharp
_logger.LogError(
    ex,
    "An unhandled exception has occurred while executing the request."
);
```

The actual exception is recorded in the application logs while unexpected internal details are hidden from the API client.

### 5. ProblemDetails

The project uses ASP.NET Core's `ProblemDetails` model to return a consistent structure for API errors.

Example:

```json
{
  "type": null,
  "title": "Product with ID 10 was not found.",
  "status": 404,
  "detail": "Product with ID 10 was not found.",
  "instance": "/api/Products/10"
}
```

The main properties used are:

* `Status` — HTTP status code
* `Title` — short description of the problem
* `Detail` — details about the specific occurrence
* `Instance` — the request path where the problem occurred

## Exception Mapping

| Exception             | HTTP Status | Response             |
| --------------------- | ----------: | -------------------- |
| `NotFoundException`   |         404 | Product not found    |
| `BadRequestException` |         400 | Bad request          |
| Other `Exception`     |         500 | Generic server error |

## API Endpoints

### Get Existing Product

```http
GET /api/Products/1
```

Response:

```http
200 OK
```

Example:

```json
{
  "id": 1,
  "name": "Laptop",
  "price": 25000
}
```

### Get Non-Existing Product

```http
GET /api/Products/10
```

Response:

```http
404 Not Found
```

### Test Bad Request

```http
GET /api/Products/badRequest
```

Response:

```http
400 Bad Request
```

### Test Unexpected Exception

```http
GET /api/Products/error
```

Response:

```http
500 Internal Server Error
```

The API returns a generic message instead of exposing internal exception details.

## Configuration

The middleware is registered in `Program.cs`:

```csharp
app.UseMiddleware<ExceptionHandlingMiddleware>();
```

ProblemDetails services are registered with:

```csharp
builder.Services.AddProblemDetails();
```

## How to Run

### 1. Clone the repository

```bash
git clone https://github.com/ToqaHatem1/GlobalExceptionPractice.git
```

### 2. Navigate to the project

```bash
cd GlobalExceptionPractice
```

### 3. Restore dependencies

```bash
dotnet restore
```

### 4. Run the application

```bash
dotnet run
```

### 5. Open Swagger

After running the application, open the Swagger URL shown in the terminal, for example:

```text
https://localhost:<port>/swagger
```

Use Swagger to test the available endpoints and observe the different HTTP responses.

## What I Learned

* How ASP.NET Core middleware works
* How the HTTP request pipeline works
* How `RequestDelegate` and `_next` work
* How to handle exceptions globally
* How to create custom exceptions
* How to map exceptions to HTTP status codes
* How to use `ILogger` for exception logging
* How to return consistent API error responses
* How to use `ProblemDetails` for Web API errors
* Why unexpected exception details should not be exposed to API clients
