# 🌉 TaskBridge API (.NET 9)

A scalable and production-ready Freelance Marketplace REST API built with **ASP.NET Core Web API (.NET 9)**, following **Clean Architecture** and **CQRS** principles.

---

## 🚀 Features

### 👤 Authentication & User Management
- JWT Authentication for secure and stateless authorization
- Role-Based Authorization (**Client / Freelancer**)
- Secure password hashing with BCrypt
- Register and Login with full validation

### 🛠 Marketplace & Task Management
- Full CRUD operations for tasks/projects
- Freelancers can apply to tasks
- Clients can manage posted tasks
- Clean separation between business logic and presentation layer

### ⚡ CQRS Pattern with MediatR
- Commands and Queries fully separated
- Pipeline Behaviors for automatic validation
- Every operation has its own Handler
- Clean and maintainable structure

### 🔴 Redis Caching
- GetAllTasks cached with 10 minute expiry
- GetAllApplications cached
- Cache invalidated on Create/Update/Delete

### ✅ Validation & Error Handling
- FluentValidation via Pipeline Behavior
- Global Exception Handling Middleware
- Custom ApiException with standardized responses
- Validation runs automatically before every Handler

### 🧪 Unit Testing
- xUnit + Moq
- InMemory Database for Handler tests
- Separate Validator tests
- Full coverage: Create, Read, Update, Delete, Auth

### 🐳 Docker
- Docker Compose setup
- SQL Server, Redis containers
- Auto migration on startup
- GitHub Actions CI/CD

---

## 🛠 Tech Stack

| Technology | Usage |
|------------|-------|
| ASP.NET Core Web API (.NET 9) | Backend Framework |
| Entity Framework Core | ORM & Database Access |
| SQL Server | Database |
| JWT Authentication | Secure API Authorization |
| MediatR | CQRS Pattern |
| FluentValidation | Pipeline Validation |
| Redis | Caching |
| Docker | Containerization |
| xUnit | Unit Testing |
| Moq | Mocking Dependencies |
| Clean Architecture | Project Structure |
| GitHub Actions | CI/CD |

---

## 🏗 Architecture

```
TaskBridge.Domain
└── Entities, Enums, Errors

TaskBridge.Application
├── Commands/       ← Write operations
├── Queries/        ← Read operations
├── Behaviors/      ← ValidationBehavior Pipeline
├── DTOs/
├── Interfaces/
└── Validators/

TaskBridge.Infrastructure
├── Data/           ← AppDbContext
└── Consumers/

TaskBridge.API
├── Controller/
├── Middleware/
└── Program.cs

xUnit_Test_Project
├── Handlers/       ← Handler tests
└── Validators/     ← Validator tests
```

---

## 🔒 Security
- JWT Bearer Authentication
- Role-Based Authorization
- Protected API Endpoints
- BCrypt Password Hashing
- Automatic request validation via Pipeline

---

## 📈 Future Improvements
- Refresh Tokens
- Real-time Notifications with SignalR
- File Upload Support
- API Versioning
- Integration Testing

---

## 👨‍💻 Author
**Saba Khubutia**  
GitHub: https://github.com/sabakhubutia03/TaskBridge
