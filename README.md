# Advanced C# Sample
Example how to use Dependency Injection, Serialization, Interfaces, Auto update etc.

Client is Console App, Web service is ASP.NET MVC 5

Sample app requirements: 
- Auto updates its list of plugins from a web server and then executes all identified plugins
- Plugins all implement a simple interface ISimplePlugin with one method: void Print()
- The app will be written to be as close to production-ready as possible, with proper design principles (separation
of concern), professional naming and coding conventions and with proper exception handling
- The application will connect to a web server and will request the "last update" value for each of plugin DLLs
- The server will derive this last update value by examining the dates of DLLs that it is managing
- The response to the request will be a JSON-serialized response containing an array of objects that have two
fields: DLL name and Date Created for that DLL
- The console app will examine the list, compare the list of DLLs with its own and will then download the NEW and
the UPDATED DLLs
- Once the update is complete, the app will load all DLLs, find all plugins in them, and call each discovered plugin's
Print method
- Each plugin will just print out a simple message on the console screen
- The console app will output detailed diagnostic information to the console screen during its execution so that
the user knows what's going on
- The console's Main class will use Dependency Injection to load other classes it needs (classes
for getting the versions from the server, the loader class, the executor class, etc...)


