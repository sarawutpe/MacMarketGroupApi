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

Numeric Types:

int: Represents a 32-bit signed integer.
long: Represents a 64-bit signed integer.
float: Represents a single-precision floating-point number.
double: Represents a double-precision floating-point number.
decimal: Represents a decimal number with high precision.
Boolean Type:

bool: Represents a Boolean value (true or false).
Character Types:

char: Represents a single Unicode character.
String Type:

string: Represents a sequence of characters.
DateTime Type:

DateTime: Represents a date and time value.
Collections:

Array: Represents a fixed-size collection of elements of the same type.
List<T>: Represents a dynamic-size collection of elements of type T.
Dictionary<TKey, TValue>: Represents a collection of key-value pairs.
HashSet<T>: Represents a collection of unique elements.
Object Type:

object: Represents any type of object.
Nullable Types:

int?, bool?, etc.: Represents a value type that can also be null.

