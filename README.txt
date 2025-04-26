Project Setup (localhost)

git clone https://github.com/suresh-03/product-inventory-system.git


dotnet build


dotnet ef migrations add InitialCreate
dotnet ef database update
