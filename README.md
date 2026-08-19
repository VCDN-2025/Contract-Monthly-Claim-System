# CMCS — Contractor Monthly Claim System

CMCS is a **production-grade ASP.NET Core 8 MVC web application** built to streamline, secure, and automate the independent contractor claim and approval process within an academic institution. It replaces manual, error-prone claim handling with a fully role-based digital workflow — from claim submission to policy-enforced approval and HR reporting.

---

## Overview

The **Part 3** release focused on achieving **enterprise readiness**, layering in a robust security model and automating key business rules on top of the earlier prototype.

### Key Architectural Shifts (Part 2 → Part 3)

| Area | Part 2 | Part 3 |
| :--- | :--- | :--- |
| **Data Persistence** | Encrypted JSON File Storage | SQL Server via Entity Framework Core |
| **Security** | Basic access control | Full ASP.NET Identity (Authentication + RBAC) |
| **Automation** | Manual calculation | Client-side + server-side rate enforcement & policy checks |

---

## Features

### Two-Stage Approval Workflow
- Claims move through a defined chain: **Lecturer → Programme Co-ordinator → Academic Manager**.
- Each stage has its own role-scoped dashboard and actions.

### Automated Rate Enforcement & Calculation
- **Rate Enforcement**: A lecturer's `ContractHourlyRate` is pulled directly from HR-managed database records and used for every claim calculation — user input is never trusted for rate values.
- **Live Auto-Calculation**: jQuery calculates the `ClaimAmount` client-side as hours are entered, giving lecturers instant feedback.

### Policy Verification
- The Programme Co-ordinator's `Verify` action automatically rejects claims that breach the `MAX_HOURS_PER_MONTH` policy (e.g., 150 hours), preventing non-compliant claims from progressing.

### Role-Based Access Control (RBAC)
- Every controller is protected with `[Authorize(Roles="...")]`, ensuring users only ever see dashboards and data relevant to their role.

### HR Reporting
- Complex LINQ-driven, multi-filter reports let HR query claims by status, date range, and amount for data analysis.

---

## Tech Stack

| Category | Technology |
| :--- | :--- |
| **Framework** | ASP.NET Core MVC (.NET 8) |
| **Language** | C# |
| **ORM** | Entity Framework Core (Code-First / Migrations) |
| **Database** | SQL Server |
| **Frontend** | Razor Views + jQuery |
| **Authentication** | ASP.NET Identity |
| **Architecture** | MVC + Service Layer abstraction |
| **Testing** | MSTest + Moq |

---

## Project Structure

```
CMCS/
├── Controllers/      # MVC controllers, protected via [Authorize(Roles="...")]
├── Models/           # EF Core entities (ClaimModel, LecturerModel) & ViewModels
├── Services/         # Core logic (FileUploadService.cs) and utility methods
├── Data/             # CmcsDbContext.cs (EF Core/Identity) + DataRepository.cs
├── DataSeeding/       # DbSeeder.cs — seeds initial Roles and HR Admin user
└── Migrations/        # EF Core–generated SQL schema migrations
```

---

## Automation Feature Map

| Feature | Location | Description |
| :--- | :--- | :--- |
| **User Management** | `HRController` | HR creates accounts and assigns roles via `UserManager` |
| **Rate Enforcement** | `HR/LecturerInfo`, `ClaimStatusController` | HR-set hourly rate overrides any user-entered value |
| **Auto-Calculation** | `ClaimStatus/Create.cshtml` | jQuery computes `ClaimAmount` live as hours are typed |
| **Policy Verification** | `ProgrammeCoOrdinatorController` | Auto-rejects claims exceeding the max-hours policy |
| **Reports** | `HRController/Reports` | Complex LINQ queries filter claims by status, date, amount |

---

## Getting Started

### Prerequisites
- Visual Studio 2022 (v17.14 or later)
- .NET 8.0 SDK
- SQL Server LocalDB (or any local SQL Server instance)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/VCDN-2025/prog6212-poe-part-3-WandileSimamane.git
   ```

2. **Open the solution**
   Navigate to the cloned folder and open `CMCS.sln` in Visual Studio.

3. **Configure the connection string**
   Ensure `appsettings.json` contains a valid SQL Server connection string for `DefaultConnection`.

4. **Run EF Core migrations**
   Open the Package Manager Console (PMC) and run:
   ```powershell
   Update-Database
   ```

5. **Build and run**
   Press `F5` or click **Start**. Initial roles and the HR Admin account are seeded automatically on first run.

### Default Test Credentials

| Field | Value |
| :--- | :--- |
| **Role** | HR Admin (seeded) |
| **Email** | `hr@cmcs.com` |
| **Password** | `HRPassword123!` |
| **Access** | System Setup, Reports, User Management |

---

## Configuration

### Data Persistence
Data is persisted to SQL Server (`CMCS_Part3_DB`) via Entity Framework Core, configured through `appsettings.json`.

### Security & Sessions
ASP.NET Identity manages authentication, authorization, and role assignment. Session state tracks the logged-in user to enforce security across requests.

---

## Dependencies

- `Microsoft.EntityFrameworkCore.SqlServer` — database provider
- `Microsoft.EntityFrameworkCore.Tools` — migration commands
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` — security framework

---

## Testing

Unit tests are written with **MSTest** and **Moq**, covering controller logic, separation of concerns, and automated rule enforcement.

---

## Demo

📺 **YouTube Walkthrough**: [Watch the demo](https://youtu.be/CTD5cTRetiA)

> No lecturer feedback was carried over — the previous assignment submission received 100%.

---

## Screenshots

<p align="center">
<img width="900" alt="CMCS Dashboard 1" src="https://github.com/user-attachments/assets/f976dc16-2c8b-4f35-8d71-f4b71bf2a91a" />
<img width="900" alt="CMCS Dashboard 2" src="https://github.com/user-attachments/assets/678b5f55-3bd7-43d3-8c1d-3e665ecbed96" />
<img width="900" alt="CMCS Dashboard 3" src="https://github.com/user-attachments/assets/061ac804-9e53-4c12-9abf-6be53fcd4c5a" />
<img width="900" alt="CMCS Dashboard 4" src="https://github.com/user-attachments/assets/fc72a8d8-1920-42cd-90c5-756695fccb6e" />
<img width="900" alt="CMCS Dashboard 5" src="https://github.com/user-attachments/assets/7be101da-81a2-4dd3-b09e-0f407c7b8e57" />
<img width="900" alt="CMCS Dashboard 6" src="https://github.com/user-attachments/assets/b3d0703e-2870-494d-8067-146104efb3a6" />
<img width="900" alt="CMCS Dashboard 7" src="https://github.com/user-attachments/assets/e40ae18d-273f-4569-8f74-bdb777920e67" />
<img width="900" alt="CMCS Dashboard 8" src="https://github.com/user-attachments/assets/f591af07-ea6e-42d5-9027-6e9bb8816ff8" />
<img width="900" alt="CMCS Dashboard 9" src="https://github.com/user-attachments/assets/96874732-da73-4369-b970-567a461a6c1b" />
<img width="900" alt="CMCS Dashboard 10" src="https://github.com/user-attachments/assets/39ab0027-8d86-4974-af66-b33e16f1e986" />
</p>

---

## Acknowledgements & References

| # | Source | Category | Link |
| :--- | :--- | :--- | :--- |
| 1 | Richard Nwonah — RBAC in C# and ASP.NET Core | Security / RBAC | [Medium](https://medium.com/@nwonahr/role-based-access-control-rbac-in-c-and-asp-net-core-the-security-backbone-of-modern-apps-dea1204a0870) |
| 2 | Microsoft Learn — ASP.NET Core Identity | Security / Identity | [Docs](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-10.0&tabs=visual-studio) |
| 3 | Microsoft Learn — Identity Role Seeding | Database / Seeding | [Docs](https://learn.microsoft.com/en-us/answers/questions/1529111/how-to-set-asp-net-core-identity-role-automatically) |
| 4 | Microsoft Learn — Calculated Fields | Automation / Client-Side | [Docs](https://learn.microsoft.com/en-us/power-apps/maker/data-platform/define-calculated-fields) |
| 5 | Microsoft MSDN — Design Patterns for Data Persistence | Architecture / EF Core | [MSDN](https://learn.microsoft.com/en-us/archive/msdn-magazine/2009/april/design-patterns-for-data-persistence) |
| 6 | Richard Nwonah — Sessions and Cookies in ASP.NET Core | Security / Sessions | [Medium](https://medium.com/@nwonahr/working-with-sessions-and-cookies-in-asp-net-core-013b24037d91) |
| 7 | ChatGPT — Login POST Framework | Development Tool | [Chat](https://chatgpt.com/share/6920927b-cc48-8000-9795-a3496f12211d) |
| 8 | ChatGPT — Claims Report Generation | Development Tool | [Chat](https://chatgpt.com/share/6920963d-4678-8000-a637-939e514d1df5) |
| 9 | Class Repository — Core Concepts & Architecture | Academic | [GitHub](https://github.com/fb-shaik/PROG6221-Group2-2025/tree/main) |
| 10 | ChatGPT — Adding Comments | Development Tool | [Chat](https://chatgpt.com/share/6920bad4-41e4-8000-bbab-433e76af47c1) |

---

## Author

**Wandile Simamane**
