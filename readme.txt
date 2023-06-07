##### API with ASP.NET Core + MongoDB #####

Install or update a package
$ dotnet add package <PACKAGE_NAME>

Packages
Install-Package BCrypt.Net

Install a specific version of a package
$ dotnet add package <PACKAGE_NAME> -v <VERSION>
$ dotnet add package Newtonsoft.Json --version 12.0.1

List package references
$ dotnet list package

Remove a package
$ dotnet remove package <PACKAGE_NAME>
$ dotnet remove package Newtonsoft.Json


# Convert object to string
var options = new JsonSerializerOptions { WriteIndented = true };
var jsonString = JsonSerializer.Serialize(<JSON_OBJECT>, options);

# Convert string to object
var jsonObject = JsonSerializer.Deserialize<Product>(<JSON_STRING>);

