# UrbanHub - Parking Management System

A comprehensive ASP.NET Core MVC application for managing urban parking spaces, bookings, and payments. UrbanHub provides a platform for parking space owners to list their spaces and for users to browse, book, and manage parking reservations.

## 📋 Table of Contents
- [Overview](#overview)
- [Technology Stack](#technology-stack)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Features](#features)
- [Getting Started](#getting-started)
- [Database Setup](#database-setup)
- [Project Modules](#project-modules)
- [Controllers Guide](#controllers-guide)
- [Contributing](#contributing)

## 🎯 Overview

UrbanHub is an urban parking management platform that bridges the gap between parking space owners and users seeking convenient parking solutions. The application provides real-time booking capabilities, wallet management, transaction tracking, and comprehensive admin panels for system management.

**Key Stakeholders:**
- **Parking Space Owners**: Can list, manage, and monetize their parking spaces
- **Renters/Users**: Can browse, book, and manage parking reservations
- **Administrators**: Can manage users, monitor transactions, and maintain system logs

## 🛠 Technology Stack

### Backend
- **Framework**: ASP.NET Core 9.0 (.NET 9)
- **ORM**: Entity Framework Core 9.0.15
- **Database**: SQL Server (LocalDB for development)
- **Mapping**: AutoMapper 12.0.0
- **Real-time Communication**: SignalR
- **Email Service**: MailKit 4.16.0
- **Geospatial**: NetTopologySuite (for location-based queries)
- **Security**: Custom authentication with cookie-based sessions

### Frontend
- **UI Framework**: Bootstrap 5
- **Icons**: Font Awesome
- **Client-side Scripting**: jQuery with AJAX

### Development
- **Language**: C# with nullable reference types enabled
- **IDE**: Visual Studio / Visual Studio Code
- **Package Manager**: NuGet

## 🏗 Architecture

The application follows a **layered architecture** with clear separation of concerns:

```
UrbanHub.web (Presentation Layer)
    ├── Controllers
    ├── Views
    └── Custom Services

        ↓

UrbanhubAuth.repo (Business Logic Layer)
    ├── Authentication Services
    ├── Booking Management
    ├── Payment Processing
    └── Admin Operations

        ↓

UrbanHub.DTO (Data Transfer Objects)
    └── DTOs for API contracts

        ↓

Urbanhub.Entities (Domain Model)
    └── Entity Framework Models

        ↓

UrbanHub.Data (Data Access Layer)
    └── DbContext & Migrations

        ↓

SQL Server Database
```

## 📁 Project Structure

```
Urbanhub/
├── UrbanHub/                          # Main Web Application (ASP.NET Core MVC)
│   ├── Controllers/                   # API & MVC Controllers
│   │   ├── AdminController.cs
│   │   ├── AdminLogsController.cs
│   │   ├── AdminTransactionsController.cs
│   │   ├── AdminUserManagementController.cs
│   │   ├── HomeController.cs
│   │   ├── login_regisration.cs
│   │   ├── NotificationsController.cs
│   │   ├── ParkINBookings.cs
│   │   ├── ParkINDetails.cs
│   │   ├── ParkingSpaceController.cs
│   │   ├── ParkINHome.cs
│   │   ├── ParkINManageBookings.cs
│   │   ├── ParkINMyspace.cs
│   │   ├── ParkINPayment.cs
│   │   └── ParkINWallet.cs
│   ├── Views/                         # Razor Templates
│   │   ├── Admin/                     # Admin Dashboard Pages
│   │   ├── AdminLogs/
│   │   ├── AdminTransactions/
│   │   ├── AdminUserManagement/
│   │   ├── Home/
│   │   ├── login_regisration/
│   │   ├── ParkINBookings/
│   │   ├── ParkINHome/
│   │   ├── ParkINWallet/
│   │   ├── Shared/                    # Shared Layouts & Partials
│   │   └── _ViewStart.cshtml
│   ├── custom services/               # Business Logic Services
│   ├── Models/                        # View Models
│   ├── wwwroot/                       # Static Assets (CSS, JS, Images)
│   ├── Program.cs                     # Application Startup Configuration
│   ├── appsettings.json               # Configuration Settings
│   └── UrbanHub.web.csproj            # Project File
│
├── UrbanHub.DTO/                      # Data Transfer Objects
│   ├── LoginDTO.cs
│   ├── RegistrationDTO.cs
│   ├── ParkingSpaceDTO.cs
│   ├── ParkingBookingDTO.cs
│   ├── UserDTO.cs
│   └── UrbanHub.DTO.csproj
│
├── Urbanhub.Entities/                 # Entity Framework Domain Models
│   ├── User.cs
│   ├── ParkingSpace.cs
│   ├── ParkingBooking.cs
│   ├── Wallet.cs
│   ├── PlatformWallet.cs
│   ├── Withdrawals.cs
│   ├── Notification.cs
│   ├── Log.cs
│   ├── Email.cs
│   ├── Registration.cs
│   └── UrbanHub.Entities.csproj
│
├── Urbanhub.Data/                     # Data Access Layer
│   ├── UrbanHubDbContext.cs
│   ├── Migrations/
│   └── UrbanHub.Data.csproj
│
├── UrbanhubAuth.repo/                 # Business Logic & Repository
│   ├── auth.cs                        # Authentication Logic
│   ├── ParkinHome.cs                  # Parking Home Service
│   ├── ParkinViewDetails.cs           # Parking Details Service
│   ├── ManageBookings.cs              # Booking Management
│   ├── MySpace.cs                     # Space Management
│   ├── Payment.cs                     # Payment Processing
│   ├── ParkinWallet.cs                # Wallet Management
│   ├── Notifications.cs               # Notification Service
│   ├── AdminUserManagement.cs
│   ├── AdminTransactions.cs
│   ├── AdminLogs.cs
│   ├── HeversineFormula.cs            # Geospatial Calculations
│   └── UrbanHubManagement.repo.csproj
│
├── UrbanHub.shared/                   # Shared Models & Mappers
│   ├── mapper.cs                      # AutoMapper Configuration
│   ├── ParkingDetailsModel.cs
│   ├── ParkInBrowseModel.cs
│   ├── UserCard.cs
│   ├── WithdrawalModel.cs
│   └── UrbanHub.shared.csproj
│
├── UrbahHubMail/                      # Email Service Module
│   ├── send_mail.cs                   # Email Sending Logic
│   └── UrbahHubMail.csproj
│
├── UrbanHubNotification/              # Notification Module (SignalR)
│   └── UrbanHubNotification.csproj
│
├── project docs/                      # Project Documentation
│   ├── Core Db Usage.txt
│   └── urbanhubDb.bak                 # Database Backup
│
└── README.md                          # This file
```

## ✨ Features

### User Features
- **User Authentication & Authorization**
  - Secure registration with password validation (8+ chars, uppercase, lowercase, number, special char)
  - Cookie-based authentication with 5-hour session timeout
  - Email verification

- **Parking Space Discovery**
  - Browse available parking spaces with filtering
  - View detailed parking information
  - Location-based search using geospatial queries (Haversine Formula)
  - Real-time availability status

- **Booking Management**
  - Create and manage parking bookings
  - View booking history
  - Track booking status (Pending, Confirmed, Completed, Cancelled)

- **Wallet System**
  - Digital wallet for transactions
  - Add funds to wallet
  - Track wallet balance
  - Withdrawal requests

- **Notifications**
  - Real-time notifications via SignalR
  - Booking confirmations
  - Payment notifications
  - System alerts

- **Reviews & Ratings**
  - Leave reviews on parking spaces
  - Rate experiences
  - View community feedback

### Parking Space Owner Features
- **Space Management**
  - List new parking spaces
  - Edit space details and pricing
  - Manage availability schedules
  - View booking history
  - Track earnings

### Admin Features
- **User Management**
  - View all registered users
  - Search and filter users
  - Ban/Unban users
  - Verify user accounts
  - Track user join dates and status

- **Transaction Monitoring**
  - View all transactions
  - Track payment history
  - Monitor withdrawal requests
  - Generate transaction reports

- **System Logs**
  - Access detailed system logs
  - Track admin actions
  - Monitor user activities
  - Audit trail

- **Dashboard Analytics**
  - Total users count
  - Transaction statistics
  - System health monitoring

## 🚀 Getting Started

### Prerequisites
- .NET 9 SDK or later
- SQL Server 2019 or later (LocalDB for development)
- Visual Studio 2022 or Visual Studio Code
- Git

### Installation Steps

1. **Clone the Repository**
   ```bash
   git clone https://github.com/mubin25-dodu/Urbanhub.git
   cd Urbanhub
   ```

2. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

3. **Update Connection String**
   - Open `UrbanHub/appsettings.json`
   - Modify the connection string if needed:
   ```json
   {
     "ConnectionStrings": {
       "UrbanHubDB": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=UrbanHubDb;TrustServerCertificate=True;Integrated Security=True;"
     }
   }
   ```

4. **Create Database**
   ```bash
   cd UrbanHub
   dotnet ef database update
   ```

5. **Run the Application**
   ```bash
   dotnet run
   ```

6. **Access the Application**
   - Open browser and navigate to: `https://localhost:7xxx` (check terminal for exact port)

## 💾 Database Setup

### Database Initialization

The application uses Entity Framework Core Code-First migrations. The database will be created automatically on first run.

### Database Restore (Alternative Method)

If you have the backup file `urbanhubDb.bak`:

1. Open SQL Server Management Studio
2. Right-click on **Databases** → **Restore Database**
3. Select device and browse to `project docs/urbanhubDb.bak`
4. Click OK to restore

### Key Tables
- **User**: Stores user account information
- **ParkingSpace**: Parking space listings with geolocation
- **ParkingBooking**: Booking records and history
- **Wallet**: User wallet balances
- **PlatformWallet**: Platform's aggregate wallet
- **Withdrawals**: Withdrawal request tracking
- **Notification**: System notifications
- **Log**: Audit logs
- **Registration**: Registration data
- **Email**: Email history

## 📦 Project Modules

### 1. **UrbanHub.web** - Main Web Application
Handles HTTP requests, user interface, and view rendering. Contains MVC controllers, Razor views, and custom middleware.

### 2. **UrbanhubAuth.repo** - Business Logic Layer
Implements core business logic for:
- User authentication
- Parking space management
- Booking operations
- Payment processing
- Wallet management
- Notifications
- Admin operations

### 3. **UrbanHub.DTO** - Data Transfer Objects
Defines contracts for:
- Login/Registration
- Parking spaces
- Bookings
- User profiles
- Search filters

### 4. **Urbanhub.Entities** - Domain Models
Entity Framework entities mapped to database tables.

### 5. **UrbanHub.Data** - Data Access Layer
Entity Framework DbContext and migrations.

### 6. **UrbanHub.shared** - Shared Utilities
- AutoMapper configuration
- Shared models (ParkInBrowseModel, UserCard, etc.)
- Reusable view models

### 7. **UrbahHubMail** - Email Service
Handles email sending using MailKit for notifications and communications.

### 8. **UrbanHubNotification** - SignalR Hub
Real-time notification service using SignalR for live updates.

## 🎮 Controllers Guide

### Authentication Controllers
- **login_regisration**: User registration and login

### Admin Controllers
- **AdminController**: Admin dashboard and home
- **AdminUserManagementController**: User management operations
- **AdminTransactionsController**: Transaction tracking
- **AdminLogsController**: System logs and audit trail

### Parking Space Controllers
- **ParkingSpaceController**: Parking space CRUD operations
- **ParkINHome**: Parking browsing and discovery
- **ParkINDetails**: Detailed parking information view

### Booking Controllers
- **ParkINBookings**: User booking management
- **ParkINManageBookings**: Space owner booking management

### Payment & Wallet Controllers
- **ParkINPayment**: Payment processing
- **ParkINWallet**: Wallet management (balance, transactions, withdrawals)

### Additional Controllers
- **HomeController**: General home page
- **NotificationsController**: Notification management

## 🔐 Authentication & Security

- **Cookie-Based Authentication**: Uses "UrbanAuth" cookie scheme
- **Session Management**: 5-hour session timeout
- **Password Requirements**: 
  - Minimum 8 characters
  - At least one uppercase letter
  - At least one lowercase letter
  - At least one digit
  - At least one special character (@$!%*?&)
- **Email Verification**: Tracks verified status via Vid field

## 🗺️ Geospatial Features

The application uses **NetTopologySuite** for geospatial operations:
- Location-based parking search
- Distance calculations using Haversine Formula
- Geographic point storage and querying

## 📨 Email & Notifications

### Email Service (UrbahHubMail)
- Uses MailKit library
- Sends booking confirmations
- Payment notifications
- System alerts

### Real-time Notifications (SignalR)
- Hub: `/signalrNotification`
- Instant booking updates
- Live notification delivery
- User-specific messages

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/YourFeature`)
3. Commit your changes (`git commit -m 'Add YourFeature'`)
4. Push to the branch (`git push origin feature/YourFeature`)
5. Open a Pull Request

### Naming Conventions
- **Controllers**: `[Feature]Controller.cs` (e.g., `ParkINBookings.cs`)
- **Services**: `[Service]Service.cs` or `[Feature].cs`
- **Views**: `[Action].cshtml` in `Views/[Controller]` directory
- **DTOs**: `[Entity]DTO.cs`
- **Entities**: `[EntityName].cs`

## 📝 Project Notes

### Current Status
- **Branch**: `mubin-simplecruds`
- **Default Branch**: `Main`
- **Framework**: .NET 9.0

### Database
- **Location**: SQL Server LocalDB
- **Name**: UrbanHubDb
- **Authentication**: Windows Integrated Security

### Environment Files
- **Development**: `appsettings.Development.json`
- **Production**: `appsettings.json`

## 📞 Support & Documentation

For additional documentation, refer to:
- `project docs/Core Db Usage.txt` - Database usage documentation
- `project docs/urbanhubDb.bak` - Database backup

## 📄 License

This project is developed as part of AIUB 10th Semester .NET coursework.

## 👥 Team

**Developer**: Mubin (mubin25-dodu)

---

**Last Updated**: May 2026
**Version**: 1.0