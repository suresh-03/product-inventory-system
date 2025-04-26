<h1>Product Inventory System</h1>
<br>
<h2>A simple ASP.NET Core MVC application for managing product inventory.
Project Setup (Localhost)</h2>
<br>
<h3>Follow the steps below to set up and run the project on your local machine:</h3>

<h4>1.Clone the Repository:</h4>
    <pre><code>git clone https://github.com/suresh-03/product-inventory-system.git</code></pre>
    <pre><code>cd product-inventory-system</code></pre>
    
<h4>2.Build the Project:</h4>
    <pre><code>dotnet build</code></pre>

<h4>3.Install EF Core CLI Tools:</h4>
    <pre><code>dotnet tool install --global dotnet-ef</code></pre>
    <p><b>(You only need to install this once on your machine.)</b></p>

<h4>4.Configure the Database:</h4>   
    <p>Open appsettings.json and update the "DefaultConnection" string according to your local SQL Server configuration.</p>
        
<h4>5.Run Migrations:</h4>
    <p><b>To create the initial migration:</b></p>
        <pre><code>dotnet ef migrations add ProductTableCreation</code></pre>
    <p><b>Then update the database:</b></p>
        <pre><code>dotnet ef database update</code></pre>

<h4>6.Run the Application:</h6>
    <pre><code>dotnet run</code></pre>

You are all set! Start managing products.
