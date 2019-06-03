
//////////////
//pckages
    dotnet add package Newtonsoft.Json --version 12.0.2
    dotnet add package Autofac.Extensions.DependencyInjection --version 4.4.0
    dotnet add package AutoMapper --version 8.1.0
    dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 6.1.0
//js packages
    npm install --save-dev react react-dom
    npm install --save-dev gulp gulp-babel
    npm install --save-dev webpack webpack-dev-server webpack-cli webpack-stream html-webpack-plugin clean-webpack-plugin
    npm install --save-dev @babel/core @babel/cli @babel/plugin-proposal-class-properties @babel/preset-env @babel/preset-react @babel/plugin-transform-arrow-functions @babel/plugin-transform-classes @babel/plugin-proposal-function-bind

//////////////
//MVC WebApi Fodlers, routing and URLs:
Folders:
    //scaffolded vews for MVC and WebApi
        Areas/Scaffolded
    //conventional structure but not name
        Areas/TestArea/FolderControllers/Homecontroller.cs
    //custom controller placement
        Areas/TestArea/NewHomecontroller.cs
    
    //controller to check JS bundles
        Areas/TestArea/FolderControllers/JScheckController.cs
    //view
        Areas/TestArea/Views/JScgeck/CheckAppOne.cshtml

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
        BlogController ->
            //hardcoded string blog collection
                https://localhost:5001/api/blog
            //blog object
                http://localhost:5000/api/blog/{id}
            //get Newtonsoft Jsonized string
                http://localhost:5000/api/blog/GetString/{id}
            

            //POST
            Posts add,delete, posts and blog by person, get post by blog
            http://localhost:5000/api/blog/AddPostJSON
                {"PersonId":"81A130D2-502F-4CF1-A376-63EDEB000E9F",
                "BlogId":"1",
                "Title":"PostTitle","Content":"PostContent"	}

            http://localhost:5000/api/blog/GetPostsByPerson
                {"PersonId":"81A130D2-502F-4CF1-A376-63EDEB000E9F"}
        
            http://localhost:5000/api/blog/GetPostsByBlog
                {"BlogId":"1"}
                        
            http://localhost:5000/api/blog/GetBlogsByPerson
                {"PersonId":"81A130D2-502F-4CF1-A376-63EDEB000E9F"}
            
            //PUT
            http://localhost:5000/api/blog/UpdatePost
                {
                "PersonId":"81A130D2-502F-4CF1-A376-63EDEB000E9F"
                ,"Post":{"PostId":"1","Title":"UpdatedTitle","Content":"UpdatedContent"}
                }

            //POST
            http://localhost:5000/api/blog/DeletePost
                {
                "PersonId":"81A130D2-502F-4CF1-A376-63EDEB000E9F"
                ,"PostId":"1"
                }
        
        JScheckController ->
            //check Events in AppOne
            http://localhost:5000/TestArea/JScheck/CheckAppOne

API:
    http://localhost:5000/api/blog/AddPost -> returns Ok(result)
    http://localhost:5000/api/blog/AddPostJSON -> retorns Json(result)
    
    Body:
    {
	    "PersonId":"81A130D2-502F-4CF1-A376-63EDEB000E9F",
	    "BlogId":"1",	
		"Title":"PostTitle","Content":"PostContent"
    }

//////////////
attribute vs named area routing
OR -> controller routing attribute
	[Area("TestArea")]
	public class NewHomeController : Controller

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
//Configs

//////////////
//startup.cs
custom default MVC Area location folder in API/Areas
in startup.cs rerouted through  RazorViewEngineOptions

Autofac container registration added

AutoMapper service added, 
one coniguration 
two types of initialization - static and instance API

AutoFact to Automapper registration added

AutofacServiceProvider returned from ConfigureServices


//////////////
//Program.cs 
Added http instead of https routing for Fiddler test to:
    .UseUrls("http://localhost:5000")


//////////////
//webpack for webpack conf
webpack.config.js
//custom webpack to run from gulp
webpack.custom.js
//gulp default and webpack via gulp 
gulpfile.js


//////////////
//EF initial migration cmds
dotnet ef migrations add InitialCreate
dotnet ef database update

//////////////
//npm
npx webpack


//////////////
//DDD decomposition->
    API:
        MVC, WebApi, Controllers
        Uses View Models
    Infrastructure:
        ORMs contexts : [EF];
        Repo and UOW realizations;
        Application logic:[Checkers];

        Repo:{
            contains EF context;
            EF repo uses DAL concrete classes
        }

        UOW:{
            Contains IRepository<ConcreteRealization>, maps DAL to BLL, returns View models
        }

    Domain:
        Entity interfaces and Models For layers :[DAL,BLL,View];
        IRepo,IUOW interfaces;

//////////////
//DDD layers relation directions
    API->Infrastructure
    API->Domain
    Infrastructure->Domain
