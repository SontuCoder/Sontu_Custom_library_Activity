# 📘 Sontu.Activities

<div align="center">

<br/>

[![UiPath](https://img.shields.io/badge/UiPath-2021.10+-orange?style=for-the-badge&logo=uipath)](https://www.uipath.com/)
[![.NET Framework](https://img.shields.io/badge/.NET_Framework-4.6.1+-blueviolet?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-Custom_Activities-blue?style=for-the-badge&logo=csharp)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![FastAPI](https://img.shields.io/badge/Backend-FastAPI-009688?style=for-the-badge&logo=fastapi)](https://github.com/SontuCoder/Library_FastAPI)
[![NuGet](https://img.shields.io/badge/NuGet-Packaged-004880?style=for-the-badge&logo=nuget)](https://www.nuget.org/)

<br/>

**A professional custom UiPath activity library that automates a Library Management System through clean, drag-and-drop activities powered by a FastAPI backend.**

</div>

---

## 📖 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Project Structure](#-project-structure)
- [Activity Modules](#-activity-modules)
- [Getting Started](#-getting-started)
- [Backend Server](#-backend-server)
- [Activity Template](#-activity-template)
- [Documentation](#-documentation)
- [Design Principles](#-design-principles)
- [Author](#-author)

---

## 🔍 Overview

**Sontu.Activities** is a custom-built UiPath activity library designed to automate a **Library Management System** using REST API communication. Instead of writing repetitive HTTP requests in every workflow, this library converts all API calls into clean, reusable drag-and-drop UiPath activities.

> Built as part of an internship project using the official UiPath Community Activity Template.

---

## ✨ Features

| Feature | Description |
|---|---|
| 🔐 **Auth Scope** | Session-based authentication with CookieContainer management |
| ♻️ **Token Refresh** | Silent token renewal without breaking the workflow |
| 📚 **Book Management** | Add, edit, delete, and query books via API |
| 👨‍💼 **Admin Operations** | Full admin control — approvals, student & book management |
| 🎓 **Student Requests** | Book requests, renewals, and returns |
| 📦 **NuGet Ready** | Packaged and deployable as a NuGet library |
| 🛡️ **Safe Error Handling** | Non-throwing errors returned as `OutArgument<string>` |
| 🏗️ **Model + ViewModel** | Clean separation between API models and workflow outputs |

---

## 📁 Project Structure

```
Sontu.Activities
│
├── 📁 Admin           → Admin-level API activity classes
├── 📁 Auth            → Authentication scope and token management
├── 📁 Book            → Book retrieval activities
├── 📁 Student         → Student operations and requests
├── 📁 Helpers         → ActivityContextExtensions (session storage)
├── 📁 Models          → Backend API response data models
├── 📁 ViewModels      → Structured output models for UiPath
│    ├── Admin
│    ├── Auth
│    ├── Book
│    └── Student
├── 📁 Resources       → Embedded resources
│
├── 📦 Sontu.Activities.Packaging   → NuGet build project
└── 🧪 Sontu.Activities.Tests       → Unit testing project
```

---

## 🧩 Activity Modules

### 🔐 Auth Activities
| Activity | Description |
|---|---|
| `AuthScope.cs` | Creates authenticated session, stores cookie in context |
| `NoAuthScope.cs` | Executes activities without authentication |
| `GetMe.cs` | Gets current logged-in user details |
| `RefreshToken.cs` | Refreshes expired token silently |

### 👨‍💼 Admin Activities
| Activity | Description |
|---|---|
| `AddNewBook.cs` | Add a new book to the library |
| `EditBookDetails.cs` | Update book metadata |
| `DeleteBook.cs` | Remove a book from the catalogue |
| `ApproveRequest.cs` | Approve a student borrow request |
| `ApproveReturnBook.cs` | Confirm a book return |
| `ApproveRenewBooks.cs` | Approve a renewal request |
| `GetAllAdmins.cs` | List all admin accounts |
| `GetAllStudents.cs` | List all registered students |
| `GetAllIssuedBooks.cs` | Get all currently issued books |
| `GetAllRenewReturnReqBooks.cs` | Get all renewal/return requests |
| `GetAllReqBooks.cs` | Get all pending borrow requests |
| `GetStudentDetailsById.cs` | Get a specific student's details |

### 📚 Book Activities
| Activity | Description |
|---|---|
| `GetAllBooks.cs` | Fetch the full book catalogue |
| `GetBook.cs` | Fetch a single book by ID |

### 🎓 Student Activities
| Activity | Description |
|---|---|
| `RequestBook.cs` | Submit a borrow request |
| `DeleteIssue.cs` | Cancel an issued book record |
| `GetAllRequests.cs` | List all student requests |
| `BookReturnRenewRequest.cs` | Submit a return or renewal request |

---

## 🚀 Getting Started

### Prerequisites

- Windows 10 / 11
- Visual Studio 2019 or 2022
- .NET Framework 4.6.1 or above
- UiPath Studio 2021.10 or above
- NuGet CLI

### Step 1 — Clone this repository

```bash
git clone https://github.com/SontuCoder/Sontu.Activities.git
cd Sontu.Activities
```

### Step 2 — Open in Visual Studio

Open `Sontu.Activities.sln` in Visual Studio.

### Step 3 — Restore NuGet packages

```bash
nuget restore
```

### Step 4 — Start the backend server

See [Backend Server](#-backend-server) section below.

### Step 5 — Build the NuGet package

Build the `Sontu.Activities.Packaging` project:

```bash
msbuild Sontu.Activities.Packaging/Sontu.Activities.Packaging.csproj /p:Configuration=Release
```

### Step 6 — Install in UiPath Studio

1. Open UiPath Studio
2. Go to **Manage Packages → Settings → Add local feed**
3. Point to the folder containing the generated `.nupkg` file
4. Search for **Sontu.Activities** and install

### Step 7 — Use in your workflow

```
AuthScope
  └── GetAllBooks        → outputs: List<GetAllBookViewModel>
  └── AddNewBook         → outputs: AddNewBookViewModel, Error
  └── GetAllStudents     → outputs: List<GetAllStudentsViewModel>
```

---

## 🖥️ Backend Server

This activity library communicates with a **FastAPI backend** that implements all Library Management System API endpoints.

🔗 **[Library_FastAPI — GitHub Repository](https://github.com/SontuCoder/Library_FastAPI)**

### Quick Setup

```bash
# Clone the backend
git clone https://github.com/SontuCoder/Library_FastAPI.git
cd Library_FastAPI

# Install dependencies
pip install -r requirements.txt

# Run the server
uvicorn main:app --reload
```

| URL | Description |
|---|---|
| `http://localhost:8000` | API base URL |
| `http://localhost:8000/docs` | Interactive Swagger UI |
| `http://localhost:8000/redoc` | ReDoc documentation |

> Make sure the backend server is running before using Sontu.Activities in your UiPath workflows.

---

## 🛠️ Activity Template

This project was built using the official **UiPath Community Activity Template** for Visual Studio.

🔗 **[Download the Template](https://github.com/UiPath/Community.Activities/tree/develop/Activities/Templates/UiPath.Activities.Template/VisualStudio)**

Use this template to build your own custom UiPath activity library:

1. Clone the template repository
2. Open the `VisualStudio` folder in Visual Studio
3. Rename namespaces to match your project
4. Implement your activities
5. Build and package with the Packaging project
6. Install in UiPath Studio

---

## 🏗️ Design Principles

- **Modular Architecture** — Each domain (Auth, Admin, Book, Student) is fully isolated
- **Separation of Concerns** — Models for deserialization, ViewModels for workflow output
- **Scoped Authentication** — AuthScope centralizes session management for all child activities
- **Single Active Session** — `AuthCookiePropertyName` ensures one cookie at a time
- **Non-Throwing Errors** — All errors returned as `OutArgument<string>`, never as exceptions
- **Reusable Components** — NuGet packaging for one-click install in any UiPath project

---

## ⚙️ Error Handling Pattern

```csharp
// On success:
Result = new GetAllStudentsViewModel { ... }
Error  = ""

// On failure:
Result = null
Error  = "401 Unauthorized — Token expired"
```

All activities follow this consistent pattern, making error handling in workflows straightforward.

---

## 📄 Documentation

Full book-style PDF documentation is available:

📥 **[Download Sontu_Activities_Documentation.pdf](./Sontu_Activities_Documentation.pdf)**

The documentation covers:
- Activity Classification (Scope vs Non-Scope)
- Complete folder structure with screenshots
- All activity modules with descriptions
- Execution flow diagrams
- Error handling strategy
- Design principles

---

## 🖼️ Preview

| Cover Page | Table of Contents |
|---|---|
| ![Page 1](page_1.png) | ![Page 2](page_2.png) |

---

## 👤 Author

**Subhadip**

- GitHub: [@SontuCoder](https://github.com/SontuCoder)
- Backend: [Library_FastAPI](https://github.com/SontuCoder/Library_FastAPI)

---

## 🙏 Acknowledgements

- [UiPath Community](https://github.com/UiPath/Community.Activities) for the activity template
- [FastAPI](https://fastapi.tiangolo.com/) for the blazing fast backend framework
- [ReportLab](https://www.reportlab.com/) for PDF generation

---

<div align="center">

Made with ❤️ by Subhadip &nbsp;|&nbsp; Internship Project 2026

</div>
