For a quick demo : https://mementocoding.be/home

(
DEMO credentials : for Admin Role use these credentials 
	login: admin 
	& password: admin
)


Prerequisites:

    Operating System: Ensure that you have a compatible operating system (e.g., Windows, macOS, or Linux) installed on your machine.

    Software Requirements: Make sure you have the following software installed:
        .NET 6 SDK (6.0.400)
        Node.js  (18.x for Angular)
        Angular CLI (install using npm: npm install -g @angular/cli)

    Database: You'll need a database server such as SQL Server installed and running.

Installation Steps:

    Clone the Repository:

    git clone https://github.com/AjrAli/MementoCoding.git


Navigate to the Project Directory:

    cd MementoCoding

Backend Setup:

a. Database Configuration:

    Configure your database connection string in the appsettings.json file.

b. Migrations:

    Run the database migrations to create the required tables.
    There is a file named "DBCommands.txt" in MementoCoding\api, run these commands lines in Package Manager Console


c. Run the Backend:

	dotnet restore
	dotnet build
	dotnet test
	dotnet run

d. Update launchSettings.json:

    Open the launchSettings.json file and add the following configuration:
	
	{
	  "profiles": {
	    "DevProfile": {
	      "commandName": "Project",
	      "launchBrowser": true,
	      "launchUrl": "swagger",
	      "environmentVariables": {
	        "ASPNETCORE_ENVIRONMENT": "Development"
	      },
	      "applicationUrl": "http://localhost:5000"
	    }
	  }
	}

Frontend Setup:

a. Navigate to the Client Directory:

	cd MementoCoding\app

b. Install Dependencies:

	npm ci

c. Run the Frontend:

    ng build

    ng serve

    Access the Application:

    a. Open a web browser and navigate to http://localhost:4200 to access the MementoCoding app Angular.

    User Roles:
        As an admin, you can log in with your admin credentials to modify data.
        Guest users can use the search engine features to access school information.

Note: These installation instructions are a general guideline. Depending on your specific project structure and configurations, you may need to adjust the steps accordingly.

Enjoy using the MementoCoding Project!



---------MementoCoding---------

- .Net Architecture Test using

    -.Net 6.0
	
    -ASP.NET Web APIs
	
    -Entity Framework (EF) Core
	
    -ASP.NET Core Identity
	
    -Swagger
	
    -MediatR pattern
	
    -CQRS pattern
	
    -Unit test...

 - UI app 
 
    -Angular 15
