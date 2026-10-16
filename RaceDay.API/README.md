# RaceDay API - Part 2

## Project Description

RaceDay is a web-based event management system for South African road running, walking and cycling events. This is Part 2 of the project - the RESTful API built in C# using ASP.NET Core Web API.

## User Roles

### Organiser
- Create, edit and delete events
- Manage event categories
- View all event enrolments
- Capture participant results

### Participant
- Create account and log in
- Browse available events
- Enter an event and select a category
- View their own enrolments
- Track their own race results

## Technology Stack

- **Framework:** ASP.NET Core 8.0 Web API
- **Database:** SQL Server with Entity Framework Core (Code-First)
- **Authentication:** JWT (JSON Web Tokens)
- **Password Hashing:** BCrypt
- **API Documentation:** Swagger
- **Testing:** xUnit
- **CI/CD:** GitHub Actions

## Setup Instructions

### Prerequisites
- Visual Studio 2022
- .NET 8.0 SDK
- SQL Server (localhost)

### Steps to Run

1. Clone the repository:

git clone https://github.com/evy-dev-2723/RaceDay-API.git



2. Navigate to the project folder:
cd RaceDay.API


3. Restore packages:
dotnet restore


4. Update the database:
dotnet ef database update


5. Run the API:
$env:ASPNETCORE_ENVIRONMENT="Development"; dotnet run



6. Open Swagger in your browser:
http://localhost:5000/swagger



## API Endpoints

| Resource | Endpoints | Description |
|----------|-----------|-------------|
| Authentication | POST /api/auth/register, POST /api/auth/login | Register and login |
| Users | GET /api/users/profile | View profile |
| Events | GET, POST, PUT, DELETE /api/events | Event management |
| Categories | GET, POST, PUT, DELETE /api/categories | Category management |
| Enrolments | GET, POST, DELETE /api/enrolments | Event enrolments |
| Results | GET, POST, PUT /api/results | Race results |

## Authentication

The API uses JWT tokens. To authenticate:

1. Register or login via the auth endpoints
2. Copy the token from the response
3. In Swagger, click "Authorize" and paste: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJqb2huQHJhY2VkYXkuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZ2l2ZW5uYW1lIjoiSm9obiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL3N1cm5hbWUiOiJTbWl0aCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Ik9yZ2FuaXNlciIsImV4cCI6MTc5MjEyNzQ3MCwiaXNzIjoiUmFjZURheUFQSSIsImF1ZCI6IlJhY2VEYXlDbGllbnQifQ.xgd_GOnawfyoc0iqkSbHrWdJdRyTLX6gzho56RiARyc"
4. Now you can access protected endpoints

## Role-Based Access

- **Organiser** can: create/edit/delete events, manage categories, capture results
- **Participant** can: view events, enrol in events, view own results

## CI/CD Status

![CI Build](https://github.com/evy-dev-2723/RaceDay-API/actions/workflows/part2-ci.yml/badge.svg)

## Database Schema

The database matches the Part 1 ERD with 8 tables:
1. Roles
2. Users
3. Events
4. Categories
5. EventCategories
6. Enrolments
7. Results
8. WeatherInfo

## Running Tests

To run the unit tests:

dotnet test


## Video Presentation

Watch my Part 2 walkthrough here: [YouTube Link](https://youtu.be/YOUR-VIDEO-ID)

## Author

Evelyne Clementine Filloi - ST10492340
PROG6212 - Programming 2B
The Independent Institute of Education

## Date

16th of October 2026