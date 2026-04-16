# Employee CRM System

A full-stack, multi-page Employee CRM (Customer Relationship Management) system built with **ASP.NET Core 8**, featuring a Web API backend and an MVC frontend.

---

## Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Prerequisites](#prerequisites)
- [Quick Start (Automated)](#quick-start-automated)
- [Manual Setup](#manual-setup)
- [Default Login Credentials](#default-login-credentials)
- [Application URLs](#application-urls)
- [Project Structure](#project-structure)
- [Features](#features)
- [Role Permissions](#role-permissions)

---

## Overview

This system allows administrators and managers to manage employees, assign clients, track tasks, monitor performance, and generate reports — all from a clean, modern web interface.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Frontend | ASP.NET Core 8 MVC (Razor Views) |
| Backend | ASP.NET Core 8 Web API |
| Database | SQL Server (LocalDB for development) |
| ORM | Entity Framework Core 8 (Code-First) |
| Authentication | JWT Bearer Tokens |
| UI Libraries | Bootstrap 5.3, Font Awesome 6, Chart.js |

---

## Architecture

```
EmployeeCRM.MVC  (port 7136)          EmployeeCRM.API  (port 7001)
┌─────────────────────┐               ┌──────────────────────────┐
│  Razor Views (.cshtml)│              │  Controllers (REST API)  │
│  MVC Controllers     │──── HTTP ───▶│  Services                │
│  API Service Layer   │              │  Repositories            │
└─────────────────────┘               │  Entity Framework Core   │
                                       │  SQL Server (LocalDB)    │
                                       └──────────────────────────┘

EmployeeCRM.Shared  (Class Library)
└── DTOs, Enums — shared between API and MVC
```

---

## Prerequisites

Before running this project, make sure you have the following installed:

### 1. .NET 8 SDK
Download from: https://dotnet.microsoft.com/en-us/download/dotnet/8.0

Verify installation:
```
dotnet --version
```
Expected output: `8.x.x`

### 2. SQL Server LocalDB
Comes bundled with **Visual Studio 2022** or install standalone:

- **Option A** — Download SQL Server Express (includes LocalDB):  
  https://www.microsoft.com/en-us/sql-server/sql-server-downloads  
  *(Choose "Express" → in the installer select "LocalDB" feature)*

- **Option B** — Install Visual Studio 2022 Community (free):  
  https://visualstudio.microsoft.com/  
  *(LocalDB is included automatically with the ASP.NET workload)*

Verify installation:
```
sqllocaldb info
```
Expected output: `MSSQLLocalDB` (or similar instance names listed)

### 3. Git
Download from: https://git-scm.com/downloads

---

## Quick Start (Automated)

> **Recommended for most users**

1. Clone the repository:
   ```
   git clone <your-github-repo-url>
   cd <repo-folder>
   ```

2. Double-click `setup.bat` (or right-click → **Run as Administrator** if you face permission issues).

The script will automatically:
- Verify your .NET 8 SDK is installed
- Check for SQL Server LocalDB (and attempt to install it via winget if missing)
- Install the EF Core CLI tool if not present
- Restore all NuGet packages
- Build the solution
- Start both the API and MVC app in separate windows
- Open the browser at the web app URL

> **First-run note:** The database is created and seeded automatically when the API starts for the first time. This takes about 5-10 seconds.

---

## Manual Setup

If you prefer to run things yourself:

### Step 1 — Clone and restore packages
```bash
git clone <your-github-repo-url>
cd <repo-folder>
dotnet restore EmployeeCRM.sln
```

### Step 2 — Start the API
Open a terminal and run:
```bash
cd src/EmployeeCRM.API
dotnet run --launch-profile https
```
The API will start on `https://localhost:7001`. On first run, it automatically creates the SQL Server database and seeds sample data.

### Step 3 — Start the MVC app
Open a **second** terminal and run:
```bash
cd src/EmployeeCRM.MVC
dotnet run --launch-profile https
```
The web app will start on `https://localhost:7136`.

### Step 4 — Open the browser
Navigate to: `https://localhost:7136`

> If you see an SSL certificate warning in the browser, click "Advanced" → "Proceed". This is expected for local development certificates. You can also trust the dev cert by running `dotnet dev-certs https --trust`.

---

## Default Login Credentials

The database is seeded with these accounts on first run:

| Role | Username | Password |
|---|---|---|
| **Admin** | `admin` | `Admin@123` |
| **Manager** | `ravi.kumar` | `Manager@123` |
| **Employee** | `pavan.kalyan` | `Employee@123` |
| **Employee** | `arjun.reddy` | `Employee@123` |

---

## Application URLs

| Service | URL |
|---|---|
| **Web App (MVC)** | https://localhost:7136 |
| **API (Swagger UI)** | https://localhost:7001/swagger |

---

## Project Structure

```
EmployeeCRM.sln
│
├── src/
│   ├── EmployeeCRM.API/              ← Web API (backend)
│   │   ├── Controllers/              ← REST endpoints
│   │   ├── Services/                 ← Business logic
│   │   ├── Repositories/             ← Data access (EF Core)
│   │   ├── Models/                   ← EF entity classes
│   │   ├── Data/                     ← DbContext + seed data
│   │   └── Migrations/               ← EF Core migrations
│   │
│   ├── EmployeeCRM.MVC/              ← MVC App (frontend)
│   │   ├── Controllers/              ← Page controllers
│   │   ├── Services/                 ← HTTP clients for the API
│   │   ├── Views/                    ← Razor pages (.cshtml)
│   │   └── wwwroot/                  ← Static files (CSS, JS)
│   │
│   └── EmployeeCRM.Shared/           ← Shared class library
│       ├── DTOs/                     ← Data Transfer Objects
│       └── Enums/                    ← Shared enumerations
│
├── setup.bat                         ← One-click setup script
└── README.md
```

---

## Features

| Module | What you can do |
|---|---|
| **Dashboard** | Overview stats, department chart, task status chart |
| **Employee Management** | Add, edit, deactivate employees; search and filter by name/department/status |
| **Client Management** | Add, edit, delete clients; assign to employees; view assignment history |
| **Task Management** | Create and assign tasks; track Pending / In Progress / Completed status; view deadlines |
| **Performance Reviews** | Add star-rated reviews per employee; view history; Admin can delete |
| **Reports** | Employee summary matrix, client engagement metrics, task analysis with overdue alerts |
| **Authentication** | JWT-based login/logout; Admin can register new users |

---

## Role Permissions

| Action | Admin | Manager | Employee |
|---|---|---|---|
| View all employees/clients/tasks | ✅ | ✅ | Own only |
| Create employees / clients | ✅ | ✅ | ❌ |
| Edit employees / clients | ✅ | ✅ | ❌ |
| Delete employees / clients | ✅ | ❌ | ❌ |
| Assign tasks | ✅ | ✅ | ❌ |
| Update task status | ✅ | ✅ | Own tasks only |
| Add performance reviews | ✅ | ✅ | ❌ |
| Delete performance reviews | ✅ | ❌ | ❌ |
| View reports | ✅ | ✅ | ❌ |
| Register new users | ✅ | ❌ | ❌ |

---

## Troubleshooting

**SSL certificate error in browser**
```bash
dotnet dev-certs https --trust
```
Restart the browser after running this.

**"A connection was forcibly closed" or API unreachable**  
Make sure the API is running first (check the API terminal window). The MVC app depends on the API being up.

**Database errors on first run**  
Ensure SQL Server LocalDB is installed and the `MSSQLLocalDB` instance is running:
```bash
sqllocaldb start MSSQLLocalDB
```

**Port already in use**  
Another process is using port 7001 or 7136. Stop the conflicting process or change the port in `Properties/launchSettings.json`.
