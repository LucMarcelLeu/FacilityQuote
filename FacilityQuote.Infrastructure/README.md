# FacilityQuote

A modern web application for managing service requests and quotations for cleaning, clearance and gardening services.

**FacilityQuote** was built as a practical full-stack business application for **Leu Gebäudeservice**. Customers can request a service, select a preferred appointment and submit their contact details. Administrators can manage services, availability, customer requests and quotations through a protected administration area.

The project demonstrates a modern **.NET + Angular** application with PostgreSQL, Keycloak authentication and Docker-based development.

---

## ✨ Features

### Public Website

* Modern responsive landing page
* Presentation of available services
* Service descriptions and pricing units
* Contact information
* Customer-oriented quote request workflow

### Quote Request

Customers can:

1. Select a service
2. Enter the location
3. Select an available date
4. Select a morning or afternoon time slot
5. Enter their contact details
6. Submit the request

Inactive services are automatically excluded from the public website and quote request form.

### Availability Management

The application supports configurable availability slots with:

* Available dates
* Morning availability
* Afternoon availability
* Booking status
* Protection against double bookings

Morning and afternoon appointments are treated as separate bookable slots.

### Service Management

Administrators can:

* Create services
* Edit services
* Activate / deactivate services
* Define service categories
* Define units such as `m²`, `hour` or `item`
* Define unit prices
* Add service descriptions

Inactive services remain available in the administration area but are no longer offered to customers.

### Administration

The protected administration area provides functionality for:

* Services
* Availability
* Customers
* Requests
* Quotes

Administrative functionality is protected using **Keycloak roles**.

---

## 🛠️ Technology Stack

| Area              | Technology              |
| ----------------- | ----------------------- |
| Backend           | ASP.NET Core 10         |
| Language          | C#                      |
| ORM               | Entity Framework Core   |
| Database          | PostgreSQL              |
| Frontend          | Angular 20              |
| Language          | TypeScript              |
| Authentication    | Keycloak                |
| Containerization  | Docker / Docker Compose |
| API               | REST                    |
| API Documentation | Swagger / OpenAPI       |
| Testing           | xUnit                   |
| Development       | VS Code                 |

---

## 🏗️ Architecture

FacilityQuote follows a layered architecture with a clear separation between the API, application logic, domain model and infrastructure.

```text
┌─────────────────────────────────────────┐
│              Angular 20                 │
│                                         │
│  Public Website     Administration      │
└────────────────────┬────────────────────┘
                     │ HTTP / REST
                     ▼
┌─────────────────────────────────────────┐
│          ASP.NET Core 10 API            │
│                                         │
│ Controllers                             │
│        │                                │
│        ▼                                │
│ Application Services / Commands         │
│        │                                │
│        ▼                                │
│ Domain                                  │
│        │                                │
│        ▼                                │
│ Repositories                            │
└────────────────────┬────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────┐
│              PostgreSQL                 │
└─────────────────────────────────────────┘

              ┌──────────────┐
              │   Keycloak   │
              │ Authentication│
              │    & Roles   │
              └──────────────┘
```

The application deliberately keeps the domain model independent from infrastructure concerns.

---

## 📁 Project Structure

A simplified project structure looks like this:

```text
FacilityQuote/
│
├── src/
│   ├── FacilityQuote.Api/
│   │   ├── Controllers/
│   │   └── Models/
│   │
│   ├── FacilityQuote.Application/
│   │   ├── Services/
│   │   └── ...
│   │
│   ├── FacilityQuote.Domain/
│   │   ├── Services/
│   │   ├── Requests/
│   │   ├── Availability/
│   │   └── ...
│   │
│   ├── FacilityQuote.Infrastructure/
│   │   ├── Persistence/
│   │   └── ...
│   │
│   └── facilityquote-web/
│       └── src/
│           └── app/
│
├── tests/
│   └── FacilityQuote.Tests/
│
├── docker-compose.yml
└── README.md
```

---

# 🔌 REST API

The backend exposes a REST API consumed by the Angular application.

Examples:

```text
GET    /api/services
GET    /api/services/active
POST   /api/services
PUT    /api/services/{id}

GET    /api/availability
GET    /api/requests/available-dates

POST   /api/requests
```

The service API intentionally distinguishes between administrative and public use cases.

### All Services

```http
GET /api/services
```

Returns all services, including inactive services.

Used by the administration area.

### Active Services

```http
GET /api/services/active
```

Returns only active services.

Used by the public website and quote request workflow.

This keeps the business rule on the backend rather than relying on frontend filtering.

---

# 📅 Availability & Booking

One of the important parts of FacilityQuote is appointment availability.

An availability slot contains:

```text
Date
MorningAvailable
AfternoonAvailable
```

A request references the selected availability slot together with the selected time slot:

```text
AvailabilitySlot
        │
        ├── Morning
        │
        └── Afternoon
```

This allows the business to offer two independent appointments per day.

## Preventing Double Bookings

The database contains a unique constraint on:

```text
AvailabilitySlotId + TimeSlot
```

This means that two requests cannot reserve the same time slot.

For example:

```text
2026-09-15
    Morning    → booked
    Afternoon  → available
```

The Angular frontend uses the availability information to prevent customers from selecting already booked slots.

The database constraint provides an additional layer of protection against race conditions and concurrent requests.

---

# 🧩 Domain Model

The domain contains the core business concepts of the application.

Examples include:

```text
Service
Customer
Request
AvailabilitySlot
Quote
Address
```

### Service

A service contains information such as:

```text
Id
Category
Name
Description
Unit
UnitPrice
IsActive
```

Supported categories currently include:

```text
Cleaning
Clearance
Gardening
```

Services are deactivated instead of deleted.

This is intentional because services can already be referenced by existing requests or quotations.

---

# 🔐 Authentication & Authorization

Authentication and authorization are handled by **Keycloak**.

The Angular application obtains an access token from Keycloak and sends it with protected API requests.

Administrative routes are protected by an Angular route guard.

The backend additionally protects administrative endpoints using role-based authorization.

Conceptually:

```text
User
 │
 ▼
Keycloak
 │
 ├── Authentication
 │
 └── Roles
       │
       ▼
 Angular
       │
       ▼
 ASP.NET Core API
```

This keeps authentication responsibilities separate from the application itself.

---

# 🐳 Docker

FacilityQuote uses Docker for local infrastructure and application development.

Typical infrastructure includes:

```text
┌─────────────────────┐
│      Angular       │
└─────────────────────┘

┌─────────────────────┐
│   ASP.NET Core API │
└─────────────────────┘

┌─────────────────────┐
│     PostgreSQL     │
└─────────────────────┘

┌─────────────────────┐
│      Keycloak      │
└─────────────────────┘
```

Docker Compose is used to simplify local setup and make the development environment reproducible.

---

# 🚀 Getting Started

## Prerequisites

Install the following tools:

* [.NET 10 SDK](https://dotnet.microsoft.com/)
* [Node.js](https://nodejs.org/)
* Angular CLI
* Docker Desktop
* Git

Optional:

* VS Code
* pgAdmin
* A PostgreSQL client

---

## Clone the Repository

```bash
git clone <repository-url>
cd FacilityQuote
```

---

## Start Infrastructure

Start the required containers:

```bash
docker compose up -d
```

Check the running containers:

```bash
docker compose ps
```

---

# 🗄️ Database

FacilityQuote uses PostgreSQL with Entity Framework Core.

The connection string is configured through the application's configuration / environment variables.

Apply database migrations with:

```bash
dotnet ef database update
```

If Entity Framework CLI is not installed:

```bash
dotnet tool install --global dotnet-ef
```

---

# ▶️ Run the Backend

From the API project:

```bash
dotnet run
```

The API exposes Swagger / OpenAPI documentation during development.

Swagger can be used to inspect and test the available endpoints.

---

# 🖥️ Run the Angular Frontend

Navigate to the Angular application:

```bash
cd src/facilityquote-web
```

Install dependencies:

```bash
npm install
```

Start the development server:

```bash
npm start
```

The application is then available locally through the Angular development server.

---

# 👤 Keycloak

Keycloak is used for authentication and administration access.

After starting the infrastructure, configure a Keycloak realm and create the required client.

An administrative user must have the appropriate `admin` role to access the protected administration area.

The exact Keycloak configuration is environment-specific and should not be committed with secrets.

---

# 🧪 Testing

The backend contains automated tests using **xUnit**.

Run all tests from the solution root:

```bash
dotnet test
```

The tests cover important application and domain behavior.

The project follows a pragmatic testing approach: business-critical behavior is tested while keeping the test suite lightweight and maintainable.

---

# 📱 Responsive Design

The public-facing application is designed to work across desktop and mobile devices.

The administration area is primarily optimized for desktop use, while the customer-facing website and quote request workflow are designed with mobile users in mind.

---

# 🔄 Current Workflow

The current customer workflow looks like this:

```text
             Customer
                 │
                 ▼
          Public Website
                 │
                 ▼
        Select Service
                 │
                 ▼
          Enter Location
                 │
                 ▼
       Select Date & Time
                 │
                 ▼
        Enter Contact Data
                 │
                 ▼
        Submit Request
                 │
                 ▼
             Request
                 │
                 ▼
        Administration
                 │
                 ▼
             Quote
```

---

# 🎯 Design Goals

FacilityQuote is intentionally designed around a few principles:

### Keep the domain simple

Business concepts should remain understandable and independent from technical infrastructure.

### Prefer explicit business behavior

Examples:

```csharp
service.Activate();
service.Deactivate();
service.Update(...);
```

instead of manipulating business state from controllers.

### Protect business rules at multiple levels

For example, appointment availability is handled by:

1. Backend availability logic
2. Frontend selection logic
3. Database constraints

### Avoid unnecessary complexity

The application uses straightforward ASP.NET Core, Entity Framework Core and Angular patterns rather than introducing abstractions or frameworks without a concrete benefit.

---

# 📈 Future Improvements

Possible future extensions include:

* Email notification after request submission
* Email notification when a quotation is created
* PDF quotation generation
* Customer self-service
* More detailed quotation line items
* Additional service pricing models
* Improved availability management
* Dashboard with business statistics
* Audit logging
* Production monitoring
* More comprehensive automated integration tests

These features are intentionally not part of the current MVP.

---

# 📌 Project Status

**Current status: MVP / active development**

The core workflow is functional:

* ✅ Public website
* ✅ Service management
* ✅ Customer requests
* ✅ Availability management
* ✅ Morning / afternoon time slots
* ✅ Double-booking protection
* ✅ Customer management
* ✅ Quote management
* ✅ Keycloak authentication
* ✅ Protected administration area
* ✅ PostgreSQL persistence
* ✅ Docker-based development environment
* ✅ Responsive public UI

The project is deployed to a Hetzner server for real-world testing and further development.

---

# 👨‍💻 About the Project

FacilityQuote is a practical portfolio project demonstrating how a modern business application can be designed and implemented using a full-stack Microsoft-oriented technology stack.

The focus is not only on making the application work, but also on demonstrating:

* Clean separation of responsibilities
* Domain-oriented design
* REST API design
* Secure authentication
* Database constraints
* Automated testing
* Responsive frontend development
* Containerized infrastructure
* Incremental development of a real business workflow

---

## License

This project is currently intended as a private / portfolio project.

© Leu Gebäudeservice
