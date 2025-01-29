🩺 Patient Registration System
A RESTful API built using ASP.NET Core & SQL Server for managing patient records.




🚀 Features

🏥 Register patients with details like name, age, diagnosis, and assigned physician.
📋 Retrieve patient records with filtering options.
✏️ Update existing patient details.
🗑️ Delete patient records securely.
🔄 Uses Entity Framework Core & SQL Server for persistent storage.


🛠️ Tech Stack
Backend: C#, .NET 8, ASP.NET Core Web API
Database: SQL Server, Entity Framework Core
Tools: Postman, Swagger UI
Version Control: Git, GitHub


📸 API Screenshots
![image](https://github.com/user-attachments/assets/3a880729-5fb1-40c6-af2d-73c4720e486e)
![image](https://github.com/user-attachments/assets/b6612f05-7716-4393-8ca8-974a2b05b391)

📦 Setup & Installation
1️⃣ Clone the Repository
sh
Copy
Edit
git clone https://github.com/Pawanneetkaur/PatientRegistrationNew.git
cd PatientRegistrationNew


2️⃣ Setup Database
Install SQL Server & SQL Server Management Studio (SSMS).
Restore PatientDB.bak in SSMS.
Update appsettings.json with your SQL Server connection string.


3️⃣ Run the Application
sh
Copy
Edit
dotnet restore
dotnet build
dotnet run
Open Swagger UI at: http://localhost:5240/swagger/index.html
Test APIs using Postman.


📄 API Endpoints
HTTP Method	Endpoint	Description
GET	/api/patient	Get all patients
GET	/api/patient/{id}	Get patient by ID
POST	/api/patient	Register new patient
PUT	/api/patient/{id}	Update patient details
DELETE	/api/patient/{id}	Delete patient
