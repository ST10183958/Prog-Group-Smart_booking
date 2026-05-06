<img width="1536" height="1024" alt="MedWebLogo" src="https://github.com/user-attachments/assets/81e219dd-335b-44e1-b6ab-b55210dd720b" />


# BookingSystem
Doctor gmail/password
sipho.nkosi@medmail.co.za,12345
thandi.mokoena@medmail.co.za, 12345
lebo.khumalo@medmail.co.za, 12345

Admin login/password/passkey
Ahilya, 10285098, 1551
Amilie, 10456326, 1551
Njabulo, 10442968, 1551
Thivar, 10271490, 1551

NOTE: when you create patient and login in as patient, remember which doctor you book with and login in with that doctors email


# 🏥 BookingSystem Medical MVC Application

A full-stack ASP.NET Core MVC hospital/clinic booking system with:
- Patient & Doctor management
- Appointment scheduling
- Prescription system
- Admin statistics dashboard
- Email notifications (Gmail SMTP API)
- Real-time doctor updates (SignalR)
- REST API integration
- Docker containerisation for deployment anywhere

---

# 🚀 Features

## 👤 User Roles
- **Patient**
  - Register/Login
  - Book appointments
  - View prescriptions

- **Doctor**
  - View patient appointments
  - Confirm or decline appointments
  - Assign prescriptions
  - Select medication from stock
  - Send email notifications to patients

- **Admin**
  - View system statistics
  - Manage medicine stock
  - View overall system data

---

## 📅 Appointment System
- Patients can book appointments with doctors
- Doctors can:
  - Confirm appointments
  - Decline appointments
  - Add consultation notes
  - Assign medication

---

## 💊 Prescription System
- Doctors assign prescriptions during appointment confirmation
- Patients can view:
  - Medication name
  - Dosage
  - Quantity
  - Doctor details

---

## 📊 Admin Dashboard
Displays:
- Total patients
- Total doctors
- Total appointments
- Total admins
- System statistics stored in Azure SQL

---

## 📡 REST API Integration
The system exposes RESTful endpoints for:
- Patients
- Appointments
- Prescriptions
- Medicines

Used for external integration (mobile apps / future systems).

---

## 📡 SignalR Real-Time Updates
Real-time features include:
- Live appointment updates for doctors
- Instant notifications when patients book appointments
- Dashboard updates without refresh

---

## 📧 Email Notification System (Gmail API)
When a doctor confirms an appointment:
- Patient receives email notification via Gmail SMTP
- Includes:
  - Doctor confirmation
  - Medication assigned
  - Symptoms recorded

---

## 🐳 Docker Support (Containerisation)

The application is fully containerised using Docker.

### 📦 Build Image
```bash
docker build -t bookingsystem-app .

🚀 Run Container
- docker run -d -p 8080:8080 bookingsystem-app
- http://localhost:8080

🛠 Tech Stack
- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- Azure SQL Database
- SignalR (Real-time communication)
- Gmail SMTP API (Email notifications)
- RESTful API architecture
- Docker (Containerisation)
- Bootstrap (Frontend UI)

1. Clone Repository
git clone https://github.com/yourusername/BookingSystem.git

2. Restore Packages
dotnet restore

3. Update Database
dotnet ef database update

4. Run Application
dotnet run

