# MoviesServicesAPI

Web API service that allows you to create, read, update, and delete movies and movie categories for an Internet Movies On Demand service.

## Dependencies 📋

* Microsoft VisualStudio Web CodeGeneration Design
* Microsoft Entity Framework Tools
* Microsoft Entity Framework SqlServer
* Microsoft ASP.NET Core Authentication JwtBearer
* Microsoft Identity Model Tokens
* System Identity Model Tokens Jwt
* Serilog Extensions Logging File
* Swashbuckle ASP.NET Core

## Test ⚙️

**LOGIN**

LOGIN /api/Login?UserEmail=userEmail&password=password

CREATE /api/Login/Create



**CATEGORY**

GET /api/Category/Get

ADD /api/Category/Add

UPDATE /api/Category/Update

DELETE /api/Category/Delete?CategoryId={id}



**MOVIE**

GET /api/Movie/Get

ADD /api/Movie/Add

UPDATE /api/Movie/Update

DELETE /api/Movie/Delete?MovieId={id}



**USER**

ADD /api/User/Add

UPDATE /api/User/Update

DELETE /api/User/Delete?UserId={id}
