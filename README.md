# 🌉 TaskBridge API (.NET 9)

A scalable and production-oriented Freelance Marketplace REST API built with **ASP.NET Core Web API (.NET 9)**, following **Clean Architecture** principles and modern backend development practices.

The project connects clients and freelancers through a secure and maintainable system focused on clean code, separation of concerns, validation, and reliable business logic.

---

## 🚀 Features

### 👤 Authentication & User Management

- JWT Authentication for secure and stateless authorization
- Role-Based Authorization (**Client / Freelancer**)
- ASP.NET Core Identity integration
- User profile management using DTOs
- Secure password hashing and authentication flow

---

### 🛠 Marketplace & Task Management

- Full CRUD operations for tasks/projects
- Freelancers can apply to tasks
- Clients can manage posted tasks
- Clean separation between business logic and presentation layer
- Repository Pattern implementation

---

### ✅ Validation & Error Handling

- FluentValidation integration for request validation
- Global Exception Handling Middleware
- Custom Domain Exceptions
- Standardized API error responses
- Strong input validation and data consistency

---

### 🧪 Unit Testing

Comprehensive unit testing implemented using:

- xUnit
- Moq

#### Test Coverage Includes:

- Successful service operations
- Validation failures
- NotFound scenarios
- Mocked DbContext and repositories
- Business logic isolation

---

## 🛠 Tech Stack

| Technology | Usage |
|------------|-------|
| ASP.NET Core Web API (.NET 9) | Backend Framework |
| Entity Framework Core | ORM & Database Access |
| SQL Server | Database |
| ASP.NET Identity | Authentication & User Management |
| JWT Authentication | Secure API Authorization |
| FluentValidation | Request Validation |
| xUnit | Unit Testing |
| Moq | Mocking Dependencies |
| Clean Architecture | Project Structure |
| Dependency Injection | Service Management |

---

## 🏗 Architecture

The project follows **Clean Architecture** to ensure maintainability, scalability, and separation of concerns.

### 📁 Project Structure

#### 📂 TaskBridge.Domain

Contains:

- Core Entities
- Domain Exceptions
- Business Rules

#### 📂 TaskBridge.Application

Contains:

- Service Interfaces
- DTOs
- Validators
- Application Logic

#### 📂 TaskBridge.Infrastructure

Contains:

- ApplicationDbContext
- Repository Implementations
- Identity & JWT Services
- Database Configuration

#### 📂 TaskBridge.API

Contains:

- Controllers
- Middleware
- Authentication Configuration
- Dependency Injection Setup

#### 📂 xUnit_Test_Project

Contains:

- Unit Tests
- Mocked Services
- Validation and Business Logic Testing

---

## 🔒 Security

- JWT Bearer Authentication
- Role-Based Authorization
- Protected API Endpoints
- Secure Password Handling
- Validation against invalid requests

---

## 📌 Highlights

- Clean Architecture Implementation
- Repository Pattern
- DTO-Based Communication
- Middleware-Based Exception Handling
- SOLID Principles
- Unit Testing Best Practices
- Scalable and Maintainable Structure

---

## 📈 Future Improvements

- Refresh Tokens
- Real-time Notifications
- File Upload Support
- Docker Support
- API Versioning
- Integration Testing
- CI/CD Pipeline

---

## 👨‍💻 Author

**Saba Khubutia**

GitHub Repository:  
https://github.com/sabakhubutia03/TaskBridge
