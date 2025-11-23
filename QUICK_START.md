# Payment System - Quick Start Guide

## ✅ Current Status

- ✅ Node.js v24.11.0 installed
- ✅ npm v11.6.1 installed
- ✅ Frontend dependencies installed
- ⏳ Waiting: .NET SDK 8.0 installation

---

## 🚀 Next Steps

### 1. Install .NET SDK 8.0 (REQUIRED)
```
Download: https://dotnet.microsoft.com/download/dotnet/8.0
Select: Windows x64 installer
After install: Restart PowerShell/Terminal
Verify: dotnet --version
```

### 2. Start Frontend (After .NET is installed)
```powershell
cd c:\projects\PaymentSystem\paymentsystemui
npm run dev
```
This starts the React app on: http://localhost:5173

### 3. Start Backend (After .NET is installed)
```powershell
cd c:\projects\PaymentSystem\paymentsystem-apis
dotnet restore
dotnet run --project src/Solidaridad.API/Solidaridad.API.csproj
```
This starts the API on: http://localhost:5000

### 4. Access the Application
- Frontend: http://localhost:5173
- API Docs: http://localhost:5000/swagger

---

## 📝 Test Credentials

- **Admin:** basicuser / 123Pa$$word!
- **Initiator:** LoanManager / Password123!
- **Reviewer:** reviewer2@mail.com / Password123!
- **Approver:** approver@mail.com / Password123!

---

## 🗄️ Database Setup

The backend uses PostgreSQL. You have two options:

### Option A: Use Remote Database (Already configured)
- Host: 167.71.101.244
- Database: sdpay_prod
- (Connection string in appsettings.json)

### Option B: Use Docker (Recommended for local dev)
```powershell
docker run --name payment-db -e POSTGRES_PASSWORD=password -p 5432:5432 postgres:14
```

Then update connection string in `appsettings.Development.json`:
```json
"DefaultConnection": "Host=localhost;Port=5432;Database=payment_dev;Username=postgres;Password=password"
```

---

## 📂 Project Structure

```
PaymentSystem/
├── paymentsystem-apis/          (Backend - .NET)
│   ├── src/
│   │   ├── Solidaridad.API/     (REST Controllers)
│   │   ├── Solidaridad.Application/  (Business Logic)
│   │   ├── Solidaridad.Core/    (Domain Models)
│   │   ├── Solidaridad.DataAccess/   (Repositories)
│   │   └── Solidaridad.Shared/  (Utilities)
│   └── tests/
│
└── paymentsystemui/             (Frontend - React)
    ├── src/
    │   ├── app/
    │   │   ├── modules/         (Feature modules)
    │   │   ├── pages/           (Page components)
    │   │   └── routing/         (Routes)
    │   ├── services/            (API calls)
    │   ├── _redux/              (State management)
    │   └── _metronic/           (UI theme)
    └── package.json
```

---

## 🛠️ Common Commands

### Frontend
```powershell
npm run dev       # Start dev server
npm run build     # Build for production
npm run lint      # Check code quality
npm run preview   # Preview production build
```

### Backend
```powershell
dotnet restore                    # Install dependencies
dotnet build                      # Compile
dotnet run                        # Run API
dotnet test                       # Run tests
dotnet ef migrations add NAME     # Create migration
dotnet ef database update         # Apply migrations
```

---

## 🐛 Troubleshooting

### "dotnet: command not found"
- Install .NET SDK 8.0 from https://dotnet.microsoft.com/download/dotnet/8.0
- Restart your terminal after installation

### "npm: cannot be loaded"
- Run: `Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser -Force`
- Restart PowerShell

### Database connection errors
- Verify PostgreSQL is running
- Check connection string in appsettings.json
- Or use Docker to run PostgreSQL

### Port already in use
- Frontend (5173): Kill process on port 5173
- Backend (5000): Kill process on port 5000
- Or change ports in configuration

---

## 📚 What We Can Build Together

- ✅ New payment features
- ✅ User management pages
- ✅ Loan processing workflows
- ✅ Reports and analytics
- ✅ API endpoints
- ✅ Database optimizations
- ✅ Bug fixes
- ✅ Tests

---

## 💡 Ready to Code?

Once you have .NET SDK installed and both servers running, let me know what you want to build!

Examples:
- "Add a new payment status page"
- "Create a loan calculator feature"
- "Fix the user login bug"
- "Add email notifications"
- "Create a dashboard report"

