# CLAUDE.md - Meeting Room Booking System

## Project Context
A meeting room booking system with strict concurrency control and real-time updates. Multiple users can view and book fixed time slots for resources (meeting rooms) simultaneously without double-booking.

## Tech Stack & Infrastructure
- **Backend:** ASP.NET Core 8.0+ (Web API)
- **Database:** Azure SQL Database & Entity Framework Core
- **Real-Time:** Azure SignalR Service
- **Deployment:** Azure Web Apps
- **Frontend:** React

## Architecture & Crucial Design Decisions

### 1. Concurrency Control
- **Strategy:** Optimistic Concurrency with Versioning (using `[Timestamp]` or `uint Version` row tracking in EF Core) OR Explicit Transactional Locking (e.g., `SERIALIZABLE` isolation level / Row-level locking).
- **Rule:** A naive "check then insert" is strictly forbidden. The system must explicitly handle database exceptions (e.g., `DbUpdateConcurrencyException`) and return a explicit, user-friendly conflict response (HTTP 409 Conflict). Never throw server errors (HTTP 500) or silently overwrite data.

### 2. Real-Time Sync
- Integrate **Azure SignalR Service**.
- Whenever a slot booking status changes, instantly broadcast the event to all active clients viewing that specific resource schedule.

### 3. Role-Based Security
- **Regular User:** Can view free/booked slots and book available resources.
- **Admin:** Can manage resources (Create, Edit, Delete) and monitor all system bookings.

## Core Commands

### Backend (.NET)
- Build solution: `dotnet build`
- Run backend locally: `dotnet run --project src/BookingSystem.Api`
- Add EF Core migration: `dotnet ef migrations add <MigrationName> --project src/BookingSystem.Infrastructure --startup-project src/BookingSystem.Api`
- Update database: `dotnet ef database update --project src/BookingSystem.Infrastructure --startup-project src/BookingSystem.Api`

### Concurrency & Automated Tests
- Run concurrency integration test: `dotnet test --filter Category=Concurrency`

### Frontend (Adjust based on your choice)
- Install packages: `npm install`
- Run dev environment: `npm run dev`
- Build frontend: `npm run build`

## Code Style & Development Guidelines

### Git & Commit Rules
- Commits must be strictly **atomic**.
- Write a short, meaningful description of *what* changed and *why* inside each commit.

### C# / ASP.NET Core Standards
- Use Clean Architecture (API, Domain, Application, Infrastructure).
- Leverage modern C# features (primary constructors, file-scoped namespaces, records for DTOs).
- Enforce strict validation via FluentValidation or DataAnnotations before processing bookings.
- Handle global exceptions gracefully via custom middleware or `IExceptionHandler`.
- Write XML docstrings or concise comments for complex logic, especially around transaction handling and concurrency resolution.
