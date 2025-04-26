Product Inventory System

A simple ASP.NET Core MVC application for managing product inventory.
Project Setup (Localhost)

Follow the steps below to set up and run the project on your local machine:

    1.Clone the Repository:

        $git clone https://github.com/suresh-03/product-inventory-system.git

        $cd product-inventory-system

    2.Build the Project:

        $dotnet build

    3.Install EF Core CLI Tools:

        $dotnet tool install --global dotnet-ef

        (You only need to install this once on your machine.)

    4.Configure the Database:

        Open appsettings.json and update the "DefaultConnection" string according to your local SQL Server configuration.

    5.Run Migrations:

        To create the initial migration:

            $dotnet ef migrations add ProductTableCreation

        Then update the database:

            $dotnet ef database update

    6.Run the Application:

        $dotnet run

You are all set! Start managing products.