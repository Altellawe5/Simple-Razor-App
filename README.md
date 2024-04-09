# update dotnet ef tool - use in VSCode terminal

dotnet tool install -g dotnet-ef

dotnet tool update -g dotnet-ef

# Aanmaken SQLite db

Voer uit in directory EmpAPI1:

dotnet ef migrations add InitialCreate -p ..\..\Libraries\EmpAPI1.Infrastructure
dotnet ef database update#

# Login in Razor
to LOGIN 
firstname : Ja 
Lastname : Morant 
