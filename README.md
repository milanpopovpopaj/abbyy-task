# Developing a Module on ASP.Net Core
 
## Goal
Develop a simple web application on ASP.Net Core that includes basic elements such as authentication, authorization, and database operations.
 
## Requirements
 
1. Create a new project:
	- Create a new ASP.Net Core web application.
	- Configure the project to use Entity Framework Core with a SQL Server database.


2. Data Model:
	- Create a data model to manage users, roles, and products.
	- Example model:
     ```csharp
     public class Product
     {
         public int Id { get; set; }
         public string Name { get; set; }
         public decimal Price { get; set; }
         public string Description { get; set; }
     }
     ```
 
4. Authentication and Authorization:
	- Implement user registration and login system using Identity.
	- Set up user roles (e.g., Admin, User) and ensure access to different parts of the application based on roles.
 
5. CRUD Operations:
	- Implement CRUD (Create, Read, Update, Delete) operations for the Product model.
	- Ensure that only users with the Admin role can create, edit, and delete products.
 
6. API:
	- Create an API to manage products.
	- The API should allow retrieving a list of products, getting details of a specific product, creating, updating, and deleting products.
 
7. Frontend:
	- Create a simple page to display the list of products and details of a specific product.
	- Implement forms for adding and editing products.
 
8. Testing:
	- Write unit tests for the main components of the application.
	- Ensure your tests cover the main usage scenarios.

# Implementation and instructions:

*For this task, I've decided to use Angular for the FE part, instead of simple razor views.*

## Versions used:

- Visual Studio **2019**
- nodejs **v14.15.0**
- npm **v6.14.8**
- angular/cli **v11.0.1**

## Guide on how to prepare, run and use the application

1. Run `npm install` from the following path: **.\Abbyy-task\ClientApp**

2. Open Microsoft SQL Server Management Studio and add a database called **AbbyyTask**.

3. After that expand the Security folder, right-click on the Logins subfolder and choose New Login.
	- In the modal window, set the login name to 'AbbyyTask'
	- From the radio button list, select 'SQL Server Authentication'
	- Set a password 'Pa$$w0rd'
	- Disable the 'User must change the password at next login' option
	- Set the user's default database to 'AbbyyTask'
	- Click 'OK'
	- Double-click the AbbyyTask login name again.
	- Switch to the 'User Mapping' tab.
	- Click on the checkbox to the left of the 'AbbyyTask' database
	- In the 'Database role membership for' box, assign the 'db_owner' membership role.

4. Update ConnectionStrings/DefaultConnection in appsettings.json with your SQL Server name.

> [!NOTE]
> Data for the database comes from the following file: **\Abbyy-task\Data\Source\Products.xlsx** (https://www.mockaroo.com/ used for generating mock data)

5. To run migration:
	- Open PowerShell Command Prompt and navigate to the root of the app (where the Abbyy-task.sln file is)
	- Install the dotnet-ef command-line tool globally:
		- **dotnet tool install --global dotnet-ef**
	- Execute migrations:
		- **dotnet ef database update**
	- Open the following URLs to import products and create default users in the database:
		- https://localhost:44382/api/Seed/Import
		- https://localhost:44382/api/Seed/CreateDefaultUsers


> [!NOTE]
> AbbyyTask DB backup is also included (located in the root folder), just in case.

When you run the application, you will see:
- The product table with filtering, pagination, sorting... (accessed by the 'Products' link in the navigation)
- Registration and login/logout functionality (accessed by the 'Register' and 'Login/Logout' links in the navigation)
- A form for editing a product (accessed by clicking the product name link in the table)
- A form for adding a new product (accessed by clicking the 'Add a new product' button)
- Only users with 'Administrator' rights can add/edit products. You can use the following credentials to login as Administrator: admin@email.com/Pa$$w0rd
- There are also regular users that you can use with emails 'milan@email.com' and 'popov@email.com'. Their password is also 'Pa$$w0rd'.
