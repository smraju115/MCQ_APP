# 🎓 Coaching Management System

A complete **Coaching Management System** built using **ASP.NET Core 8.0** with **MVC Razor Pages**, **Entity Framework Core (Code First)**, and **SQL Server 2022**. Developed in **Visual Studio 2022**, this system includes a modern public website for students and a secure admin panel for coaching administrators.

---

## 🔧 Technologies Used

- ✅ ASP.NET Core 8.0 (Razor Pages - MVC Pattern)
- ✅ Entity Framework Core (Code-First)
- ✅ SQL Server 2022
- ✅ Visual Studio 2022
- ✅ Bootstrap 5 (Responsive Design)
- ✅ ASP.NET Core Identity (Authentication & Authorization)

---

## 🌐 Public Website Features

Visitors and students can interact with the platform without registration:

- 🏫 View coaching center information and services
- 📝 Participate in **free online quiz exams**
- 📊 Instantly view **quiz results**
- 🔍 Browse coaching-related announcements or notices

---

## 🔐 Admin Panel Features

A separate secured admin dashboard allows registered admins to manage the coaching system:

- 📈 Dashboard showing total number of quiz participants
- 🧑‍🎓 View and manage all quiz takers and results
- 🧪 Create, update, and delete quiz questions
- 📝 View complete reports and quiz summaries
- 🔒 Login system with email and password (only registered admins can access)

---

## 🗂️ Project Structure
├── Pages/ → Razor Pages (Admin & Public UI)

    ├── Models/ → Entity Framework Core Models

      ├── Data/ → DbContext & Migrations

        ├── wwwroot/ → Static Files (CSS, JS, Images)

          ├── Controller/ → Business Logic (e.g., quiz calculation)

            ├── appsettings.json → Configuration & DB Connection


## 🛠️ How to Run the Project

1. **Clone the repository:**
   ```bash
   git clone https://github.com/your-username/CoachingManagementSystem.git

2. Open the project using Visual Studio 2022

3. Update database connection string in appsettings.json:
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=CoachingDB;Trusted_Connection=True;"
}
4. Run the following commands in the Package Manager Console:
    Add-Migration InitialCreate -- optional
    Update-Database
5. Run the project (F5) and explore both public and admin sections.
   
Note: You can change this by updating seed data or using ASP.NET Identity UI.

## 📸 Screenshots:



## 📃 License
This project is licensed under the MIT License.

## 🙋 Developer Info
Developer: S. M. SHAHAJALAL RAJU
Email: smraju115@gmail.com




