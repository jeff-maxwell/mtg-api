# Adding OData to existing .NET Core WebAPI
Code for the .NET Core API lightning talk for OKC C# user group talk I gave on November 4th, 2019.

This talk uses the very nice starting example project/talk done by Jack Kinsey which he gave in August 2019 about how to build a .NET Core Web API.

# Original Talk by Jack Kinsey: 
[Github](https://github.com/jackwkinsey/mtg-api)
[Youtube](https://www.youtube.com/watch?v=60dkQSY0e6g&list=PLdW0ayjzW_LAhrIp9cP5Cthirbte2-Euq&index=2)

This project takes an existing .NET Core WebAPI project and adds [OData](https://www.odata.org) features to it without affecting the current API.

To use OData you have to install the NuGet Package:
```
dotnet add package Microsoft.AspNetCore.OData
```

There are 3 changes that you need to make:
1. First you have to add the OData Middleware to the services.  In Startup.cs you have to add the OData Middleware in the ConfigureServices method:
```cs
services.AddOData();
```
2. Next you need to Enable Dependency Injection into the MVC router so you can still use your current API and add in the OData features.  Then you need to turn on the features that you want to be available (Select, Count, Expand, OrderBy, Filter....).  In Startup.cs you have to make a change in the Configure Method:
```cs
// Change from
app.UseMvc();

// To
app.UseMvc(routeBuilder => 
{ 
    // Enable Dependency Injection in MVC Route Builder so you can add OData
    routeBuilder.EnableDependencyInjection();
    // Tell the Route Builder what OData features you want to Enable.
    routeBuilder.Select()      // Allows limiting what fields are returned
                .OrderBy()     // Will allow ordering the data returned by one or more fileds
                .Filter()      // Allows search filtering on the data.
                .SkipToken()   // Allows Paging of data
                .MaxTop(4)    // Sets the Maximum number of records to return by default
                .Expand();     // Will load related Objects/Data
});
```
3. For each API verb (GET, POST, PUT, DELETE...) that you want OData available you have to modify that controller and add the [EnableQuery()] attribute:
```cs
[HttpGet]
[EnableQuery()]
public ActionResult<IEnumerable<Card>> Get()   
```