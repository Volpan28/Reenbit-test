# Reenbit Meeting Room Booking System

A full-stack meeting room booking system developed for the Reenbit test task. The application guarantees absolute protection against double-booking through optimistic concurrency and provides real-time schedule updates using SignalR.

## Features

*   **Role-Based Access Control (RBAC):**
    *   **Admin:** The first registered user automatically receives Admin privileges. Admins can manage rooms, add/delete time slots, and view all bookings across the system.
    *   **Regular User:** Can view available room schedules and book open slots.
*   **Real-time Updates:** Integrated with SignalR. When a user books a slot or an Admin deletes one, all users viewing that specific room's schedule see the update instantly without page reloads.
*   **Strict Concurrency Control:** Prevents double-booking when multiple users attempt to book the exact same slot simultaneously.

## Tech Stack

*   **Backend:** ASP.NET Core 8, Entity Framework Core, Dapper, MediatR (CQRS Pattern), FluentValidation, SignalR.
*   **Frontend:** React, TypeScript, Vite, Tailwind CSS, Axios.
*   **Database:** SQL Server (LocalDB / Azure SQL).
*   **Testing:** xUnit, WebApplicationFactory, Testcontainers (Docker).

## Concurrency Control (Design Decision)

Handling simultaneous booking requests is a core requirement of this project. The system uses **Optimistic Concurrency Control with Row Versioning**.

1.  **Mechanism:** A `RowVersion` (byte array) column is added to the `Slots` table.
2.  **Execution:** When a booking request is made, the system fetches the slot. Before saving the new `Booked` status, EF Core checks if the `RowVersion` in the database matches the one fetched.
3.  **Conflict Resolution:** If two users request the same slot at the exact same millisecond, the database processes one first. When the second request attempts to save, the `RowVersion` will have changed. EF Core throws a `DbUpdateConcurrencyException`, which is caught and mapped to a clean **HTTP 409 Conflict** response. 
4.  **Why this approach?** It avoids expensive database locks (pessimistic concurrency) that degrade performance, ensuring high throughput while maintaining 100% data integrity.

## Local Setup

### Prerequisites
*   .NET 8 SDK
*   Node.js (v18+)
*   SQL Server Express LocalDB (or standard SQL Server)
*   Docker Desktop (required only for running Integration Tests)

### Backend Setup
1. Navigate to the API folder:
   ```bash
   cd ReenbitBooking.Api
   dotnet run
   ```
   
### Frontend Setup
1. Open a new terminal and navigate to the client folder:
  ```bash
  cd ReenbitBooking.Client
  ```

2. install dependencies and start the Vite dev server:
   ```bash
   npm install
   npm run dev
   ```

## Automated Testing
The repository includes an automated integration test verifying the concurrency logic. It simulates multiple concurrent requests to the same slot and asserts that exactly one succeeds while others receive a 409 Conflict.

To run the tests (requires Docker for Testcontainers):
  ```bash
  cd ReenbitBooking.IntegrationTests
  dotnet test
  ```

## AI Collaboration
This project was developed with the active assistance of Claude Code and Gemini, adhering to the development process requirements. A CLAUDE.md configuration file is included in the repository.

## Admin Login
**Admin - reenbittest@gmail.com | Password123! **
