# 🎓 UniversityApp: University Management System (API)

Welcome to the **UniversityApp** project, a Web API built with ASP.NET Core 8, designed to manage the key operations of a university institution.

This project uses a modular architecture and follows the principles of Separation of Concerns to ensure maintainability and scalability.

---
## 🚀 Technologies Used

*   **Backend**: ASP.NET Core 8 Web API
*   **Database**: SQL Server (via Entity Framework Core)
*   **ORM**: Entity Framework Core
*   **Authentication**: JWT (JSON Web Tokens)
*   **Logging**: NLog
*   **Mapping**: AutoMapper
## 🏗 Project Architecture

The project is structured into several logical layers, each with a unique and well-defined responsibility. This approach, inspired by the Layered Architecture, facilitates development and unit testing.

| Layer | Project Name | Primary Role | Dependencies |
| :--- | :--- | :--- | :--- |
| **Presentation** | `UniversityApp` | API entry point. Contains the **Controllers** (Endpoints), middleware configuration (JWT, CORS), and dependency injection. | BL, DTOs |
| **Business Logic** | `BL` (Business Logic) | Contains complex business logic via **Managers** (e.g., `AuthManager`, `CourseManager`). Manages JWT token creation and object mapping. | DAL, DTOs, Entities |
| **Data Access** | `DAL` (Data Access Layer) | Defines data access contracts (**Repository** and **UnitOfWork** Interfaces). Manages communication with the DAO layer. | DAO, Entities |
| **Data Access Object** | `DAO` (Data Access Object) | Concrete implementation of data access via **Entity Framework Core**. Contains the `DbContext` and Migrations. | Entities |
| **Data Models** | `Entities` | Contains the classes that represent the database tables (e.g., `User`, `Course`, `Assignment`). | None |
| **Data Transfer** | `DTOs` (Data Transfer Objects) | Contains the data models used for communication between the API and the client (requests and responses). | None |

## 🚀 Getting Started

Follow these steps to set up and run the project on your machine.

### Prerequisites

Make sure you have installed:

*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or higher.
*   A database server compatible with Entity Framework Core (by default, **SQL Server LocalDB** is configured via the connection string).

### 1. Clone the Repository

```bash
git clone [REPOSITORY_URL]
cd UniversityApp
```

### 2. Database Configuration

The project uses Entity Framework Core for database management.

1.  **Check the Connection String**:
    Open `UniversityApp/appsettings.json` and ensure the `DefaultConnection` string is correct for your environment. By default, it points to SQL Server LocalDB:
    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=UniversityDb;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
    ```

2.  **Apply Migrations**:
    Run the following commands in the terminal at the project root (`/UniversityApp`) to create the database and tables:

    ```bash
    dotnet ef database update --project DAO
    ```

### 3. Run the Project

Start the API from the project root directory:

```bash
dotnet run --project UniversityApp/UniversityApp.csproj
```

The API will be launched, usually on `https://localhost:7000` (check `Properties/launchSettings.json` for the exact port).

### 4. Test the API with Swagger

Once the application is launched, open your browser and navigate to the **Swagger UI** interface:

```
https://localhost:[PORT]/swagger
```

#### Authentication (JWT)

1.  Use the `POST /api/auth/register` or `POST /api/auth/login` endpoint to get a JWT token.
2.  Click the **Authorize** button at the top right of the Swagger interface.
3.  Enter the token in the format `Bearer [your_token]` (for example: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`).
4.  You can now test the protected endpoints (those with the padlock icon).

## 🌐 Endpoint Overview (Swagger)

The API uses Swagger for interactive documentation.

### Authentication (`/api/auth`)

| Endpoint | Description |
| :--- | :--- |
| `POST /api/auth/register` | Registers a new user. |
| `POST /api/auth/login` | Logs in the user and returns the JWT. |
| `POST /api/auth/refresh-token` | Refreshes the JWT. |

![Authentication Endpoints](https://private-us-east-1.manuscdn.com/sessionFile/gE5EZQphkU4VsC42mi4igy/sandbox/qGnB3ykjTSFLKILhNhRqsf-images_1763340892601_na1fn_L2hvbWUvdWJ1bnR1L1VuaXZlcnNpdHlBcHAvVW5pdmVyc2l0eUFwcC9kb2NzL2ltYWdlcy9zd2FnZ2VyX2F1dGg.png?Policy=eyJTdGF0ZW1lbnQiOlt7IlJlc291cmNlIjoiaHR0cHM6Ly9wcml2YXRlLXVzLWVhc3QtMS5tYW51c2Nkbi5jb20vc2Vzc2lvbkZpbGUvZ0U1RVpRcGhrVTRWc0M0Mm1pNGlneS9zYW5kYm94L3FHbkIzeWtqVFNGTEtJTGhOaFJxc2YtaW1hZ2VzXzE3NjMzNDA4OTI2MDFfbmExZm5fTDJodmJXVXZkV0oxYm5SMUwxVnVhWFpsY25OcGRIbEJjSEF2Vlc1cGRtVnljMmwwZVVGd2NDOWtiMk56TDJsdFlXZGxjeTl6ZDJGbloyVnlYMkYxZEdnLnBuZyIsIkNvbmRpdGlvbiI6eyJEYXRlTGVzc1RoYW4iOnsiQVdTOkVwb2NoVGltZSI6MTc5ODc2MTYwMH19fV19&Key-Pair-Id=K2HSFNDJXOU9YS&Signature=ZhJ1nea~6liv4-CKnebhrzjwONNeiZr7lIfjlrD9mmHOCyWX-8bfQt0hOYGlzK8~jXsn4OxgXlXOEYv3n908~qfl-CcfWPhoC3F4uuJ9PJEkG9x6QF28BP0Cl1UzGPwRGXWLGJRIwB7DwdctIaYBaqlpNA4T1TILCsPhyzD3kpWXiB4BpZUwgudL1TEHqG4fy4qhMEiANLqydZNo11PuwfnX3y0faQugHJ5sf-0PdU7pbIBDxqS4hIm6sBgq2jIpLJt2NdmCf2XdumvFB3ang2prynC1JYBfR-AlFzJ~9BMBa4KuFe7NROLB0Ge3KvpwIMUokasgtTbUSP1Mh6eEsg__)

### Administration (`/api/admin`)

These endpoints require the **Admin** role.

| Endpoint | Description |
| :--- | :--- |
| `GET /api/admin/departments` | CRUD for departments. |
| `GET /api/admin/courses` | CRUD for courses. |

![Administration Endpoints](https://private-us-east-1.manuscdn.com/sessionFile/gE5EZQphkU4VsC42mi4igy/sandbox/qGnB3ykjTSFLKILhNhRqsf-images_1763340892602_na1fn_L2hvbWUvdWJ1bnR1L1VuaXZlcnNpdHlBcHAvVW5pdmVyc2l0eUFwcC9kb2NzL2ltYWdlcy9zd2FnZ2VyX2FkbWlu.png?Policy=eyJTdGF0ZW1lbnQiOlt7IlJlc291cmNlIjoiaHR0cHM6Ly9wcml2YXRlLXVzLWVhc3QtMS5tYW51c2Nkbi5jb20vc2Vzc2lvbkZpbGUvZ0U1RVpRcGhrVTRWc0M0Mm1pNGlneS9zYW5kYm94L3FHbkIzeWtqVFNGTEtJTGhOaFJxc2YtaW1hZ2VzXzE3NjMzNDA4OTI2MDJfbmExZm5fTDJodmJXVXZkV0oxYm5SMUwxVnVhWFpsY25OcGRIbEJjSEF2Vlc1cGRtVnljMmwwZVVGd2NDOWtiMk56TDJsdFlXZGxjeTl6ZDJGbloyVnlYMkZrYldsdS5wbmciLCJDb25kaXRpb24iOnsiRGF0ZUxlc3NUaGFuIjp7IkFXUzpFcG9jaFRpbWUiOjE3OTg3NjE2MDB9fX1dfQ__&Key-Pair-Id=K2HSFNDJXOU9YS&Signature=cXFgB5aH0viE8Tkj6PVRj6xax~J-UpvD8Qf~WG-k-3gPd4gD1TUVAVKHx7YnmPKshSLICWP1BZScz~yB6rOswrgE0z1~ec2tYqaqYtPlYPn6kHZd0FDMZzxNVjHjscS-J0fx2rEZICoQhTbpwPbu6rfiZzlmjsztJDGnvcVE2fwpnOck6rVbCpMS57CF~G2H9i8h6eJ-E8HViwXJG77jOgwt0jVxHFlsg2PiBcCgiKiRYXrTTani3FPgSNESqF6gWWY5bJwMu9diKKllhMTi5R0nM4d10~pRGITrgDe6mEMchCGl-3ZKv8Q2eS9z3YYmvn7Uz6Gc6Xsm9OyCfdkqxw__)

### Teacher (`/api/teacher`)

These endpoints require the **Teacher** role.

| Endpoint | Description |
| :--- | :--- |
| `GET /api/teacher/classes` | Class management (CRUD, student assignment). |
| `POST /api/teacher/attendance` | Attendance management. |
| `POST /api/teacher/assignments` | Assignment and grade management. |

![Teacher Endpoints](https://private-us-east-1.manuscdn.com/sessionFile/gE5EZQphkU4VsC42mi4igy/sandbox/qGnB3ykjTSFLKILhNhRqsf-images_1763340892603_na1fn_L2hvbWUvdWJ1bnR1L1VuaXZlcnNpdHlBcHAvVW5pdmVyc2l0eUFwcC9kb2NzL2ltYWdlcy9zd2FnZ2VyX3RlYWNoZXI.png?Policy=eyJTdGF0ZW1lbnQiOlt7IlJlc291cmNlIjoiaHR0cHM6Ly9wcml2YXRlLXVzLWVhc3QtMS5tYW51c2Nkbi5jb20vc2Vzc2lvbkZpbGUvZ0U1RVpRcGhrVTRWc0M0Mm1pNGlneS9zYW5kYm94L3FHbkIzeWtqVFNGTEtJTGhOaFJxc2YtaW1hZ2VzXzE3NjMzNDA4OTI2MDNfbmExZm5fTDJodmJXVXZkV0oxYm5SMUwxVnVhWFpsY25OcGRIbEJjSEF2Vlc1cGRtVnljMmwwZVVGd2NDOWtiMk56TDJsdFlXZGxjeTl6ZDJGbloyVnlYM1JsWVdOb1pYSS5wbmciLCJDb25kaXRpb24iOnsiRGF0ZUxlc3NUaGFuIjp7IkFXUzpFcG9jaFRpbWUiOjE3OTg3NjE2MDB9fX1dfQ__&Key-Pair-Id=K2HSFNDJXOU9YS&Signature=ZPNRhXxW0kKpev6iicKICLrIlM1xmOAo-Am74x3p7i6VRoXY6VwW9cZQJGVE5arKBSMV0KkO-4VRKJJnT74aUW9lttVWYm3VwnBv2Y78s4bMLN7a~tE247XU~QSz8g3gIWJnKlIbHrLKUkB4wj5T2JUO-xW7T6RGw~HeCY0qPusrw8z4R5mOoYu1e2YzDN-L2sLyzWdctSkbaHTVFZTL6qNWD-FppUE-VP4MYcAcjpA7DOm12Q4q2HU2TdFSM6mQG~8z5xwU-wcH-fPZ-gNN9Hdytzp~7Uhq3CBk8bEUNCRHTdEB-y7ny2nEsRE-eM4i3CUcWjwKGYcvDURxWN2O3w__)

### Student (`/api/student`)

These endpoints require the **Student** role.

| Endpoint | Description |
| :--- | :--- |
| `GET /api/student/classes` | View classes and attendance. |
| `POST /api/student/assignments/{id}/submit` | Submit assignments. |
| `GET /api/student/grades` | View grades. |

![Student Endpoints](https://private-us-east-1.manuscdn.com/sessionFile/gE5EZQphkU4VsC42mi4igy/sandbox/qGnB3ykjTSFLKILhNhRqsf-images_1763340892603_na1fn_L2hvbWUvdWJ1bnR1L1VuaXZlcnNpdHlBcHAvVW5pdmVyc2l0eUFwcC9kb2NzL2ltYWdlcy9zd2FnZ2VyX3N0dWRlbnQ.png?Policy=eyJTdGF0ZW1lbnQiOlt7IlJlc291cmNlIjoiaHR0cHM6Ly9wcml2YXRlLXVzLWVhc3QtMS5tYW51c2Nkbi5jb20vc2Vzc2lvbkZpbGUvZ0U1RVpRcGhrVTRWc0M0Mm1pNGlneS9zYW5kYm94L3FHbkIzeWtqVFNGTEtJTGhOaFJxc2YtaW1hZ2VzXzE3NjMzNDA4OTI2MDNfbmExZm5fTDJodmJXVXZkV0oxYm5SMUwxVnVhWFpsY25OcGRIbEJjSEF2Vlc1cGRtVnljMmwwZVVGd2NDOWtiMk56TDJsdFlXZGxjeTl6ZDJGbloyVnlYM04wZFdSbGJuUS5wbmciLCJDb25kaXRpb24iOnsiRGF0ZUxlc3NUaGFuIjp7IkFXUzpFcG9jaFRpbWUiOjE3OTg3NjE2MDB9fX1dfQ__&Key-Pair-Id=K2HSFNDJXOU9YS&Signature=gqNruB-011vXG6eCkHhh51MmQNveXx55EUxvc7-rTBgawQh03EawFjscBQJ52hZKlVZD6zjce1-sIN06RnrdZV8HpvpJuyq-9JfYoQ2MHjEk3tLLJWfsweFa9RtW9HXfeLS-D0ETC52cmL7Kou22cwDAUEGym0GVc15WEbigO~7SMkqXJtof-v2R9uj9xijXQod9GpWpWD0N0Qv5bF9n4mK6nvYYo5pI8N46Whdk6Q-5ETZNkXtXAzh3HFFqfQZbYnj-aJcHACsJQnQ9UgBloXz7xEk-wNAofYwUaWOAbNB4aUWNt0GcMpoBwvOvA6AJ4uXUXhl-EofHIS7VZOVhxw__)


## 🎥 Demonstration

Video of Authentication & Admin APIs:
[▶️ Watch the video](Videos/Auth&AdminAPIS.mp4)

Video of Teacher & Student APIs:
[▶️ Watch the video](Videos/Teacher&StudentAPIS.mp4)

## 💡 Implemented Features

In accordance with improvement requirements, the following features have been added or enhanced:

*   **Improved Error Handling**: Implementation of a global *middleware* for centralized exception handling and returning standardized JSON responses.
*   **Logging (NLog)**: Integration of NLog for comprehensive logging of events and errors.
*   **Email Notifications**: Automatic sending of emails to students when:
    *   A submission is graded (Grade Notification).
    *   The student is assigned to a new class (New Class Enrollment Notification).
*   **File Upload API (IFormFile)**: Added an endpoint for assignment submission via file upload (`IFormFile`), with file type and size validation.
*   **In-Memory Caching**: Use of `IMemoryCache` to cache lists of departments and courses.
*   **Pagination and Filtering**: Already present for `Class` and `Assignment`, extended to include user retrieval (`User`).
