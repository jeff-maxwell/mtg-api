# mtg-api
Code for the .NET Core API talk for OKC C# user group talk.

This talk uses the very nice starting example project/talk done by Jack Kinsey which he gave in August 2019.

This project takes an existing .NET Core WebAPI project and adds [OData](https://www.odata.org) features to it without affecting the current API.

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
    routeBuilder.Select()
                .Count()
                .Expand()
                .OrderBy()
                .Filter();
});
```
3. For each API verb (GET, POST, PUT, DELETE...) that you want OData available you have to modify that controller and add the [EnableQuery()] attribute:
```cs
[HttpGet]
[EnableQuery()]
public ActionResult<IEnumerable<Card>> Get()   
```