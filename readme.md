
//////////////pckages
dotnet add package Newtonsoft.Json --version 12.0.2
dotnet add package Autofac.Extensions.DependencyInjection --version 4.4.0

//////////////
MVC WebApi Fodlers, routing and URLs:
Folders:
    //scaffolded vews for MVC and WebApi
        Areas/Scaffolded
    //conventional structure but not name
        Areas/TestArea/FolderControllers/Homecontroller.cs
    //custom controller placement
        Areas/TestArea/NewHomecontroller.cs
    //conventional views
        Areas/TestArea/Views/Home/Index.cshtml
        Areas/TestArea/Views/NewHome/Index.cshtml
Routes:
    Added http routing for Fiddler test to:
        program.cs -> IWebHostBuilder CreateWebHostBuilder -> UseUrls("http://localhost:5000")

    scaffolded controllers:
        HomeControllers ->
            https://localhost:5001/Scaffolded/home/index
        ValuesController ->
            https://localhost:5001/api/values
    added controllers:
        HomeController -> 
            //conventional view
                https://localhost:5001/TestArea/Home/
            //another folder view
                https://localhost:5001/TestArea/Home/NewHomeIndex
        NewHomeController ->
            //conventional view
                https://localhost:5001/TestArea/NewHome/
            //another folder view
                https://localhost:5001/TestArea/NewHome/OldHomeIndex
        blogcontroller ->
            //blog value controller
                https://localhost:5001/api/blog

//////////////
attribute vs named area routing
OR -> controller routing attribute
	[Area("TestArea")]
	public class NewHomeController : Controller
...

	with template route 
	routes.MapRoute(
		name:"areas",
		template:"{area}/{controller}/{action}"
	);
		
OR -> NAMED routing in startup.cs wihtout attributes

        routes.MapAreaRoute(
        name: "TestArea",
        areaName: "TestArea",
        template: "TestArea/{controller=Home}/{action=Index}");


//////////////
Default Views folder rename
http://jackhiston.com/2017/10/24/extending-the-razor-view-engine-with-view-location-expanders/
View -> ViewsNew
CustomViewLocations.cs
    
    registered in statup.cs =>

    services.Configure<RazorViewEngineOptions>(
        options => options.ViewLocationExpanders.Add(
    new CustomViewLocation()));


//////////////
//startup.cs
custom default MVC Area location folder in API/Areas
in startup.cs rerouted through  RazorViewEngineOptions


//////////////
//Program.cs 
Added http routing for Fiddler test to:
    .UseUrls("http://localhost:5000")


//////////////
//EF initial migration cmds
dotnet ef migrations add InitialCreate
dotnet ef database update
