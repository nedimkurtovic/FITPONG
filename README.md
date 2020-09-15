# FITPONG


FITPONG - application for creating and managing table tennis tournaments with options to create and to participate in tournaments as a player. Run by ASP .NET CORE in backend for web application and Web API. FITPONG consists of following projects 

  - Web application
  - Web API
  - Xamarin (mobile)
  - Windows forms


### Tech

FITPONG uses a number of projects to work properly:

* [Bootstrap] - Build fast, responsive sites with Bootstrap!
* [Microsoft.AspNetCore.SignalR] - real time communcations
* [Microsoft Identity] - user and role management
* [Microsoft.EntityFrameWorkCore] - ORM and database communcation
* [ReflectionIT.Mvc.Paging] - paging on webapp
* [jquery.mentionsInput.js] - tagging like on social media -> @name
* [Google Authenticator] - 2FA on web application
* [MailKit] - sending emails for registration, password recovery etc..
* [Swashbuckle.AspNetCore.Swagger] - accessing and testing endpoints
* [jQuery]

### Installation

If you wish to run the database locally, you will have to provide the correct connection string which is defined in appsettings.json file and configure it in Startup.cs for both web application and web api. 
```sh
Server=.;Database=NAME_OF_DATABASE;Trusted_Connection=True;MultipleActiveResultSets=True;
```
Aftewards you are expected to migrate the database using one of the following commands:
Connection string looks something like this if you are using windows authentication: 
```sh
Update-Database -Project "FITPONG.Database" -StartupProject "FITPONG.WebAPI"
```
or
```sh
Update-Database -Project "FITPONG.Database" -StartupProject "FITPONG.WebApp"
```

The migrations have the default and additional seeds of data so yo can test out some of the functionalities on the web, mobile or windows forms applications.

### Docker
FITPONG is easy to install and deploy in a Docker container.

By default, the Docker will expose ports 4260 for web application and 5766 for WebAPI as it is defined in docker-compose file. When ready, simply use the Dockerfile to build the image or docker-compose to build both web application and web api projects. Xamarin and windows forms will use the ports provided in docker-compose file, any changes regarding ports should be also updated in following files : 
* FITPONG.Mobile/APIServices <- change the urls in all classes present in the folder 
* FITPONG.Winforms - change the url in properties -> resources -> apiurl.


in root folder run:
```sh
docker-compose build
```
and then: 
```sh
docker-compose up
```
Verify the deployment by navigating to your server address in your preferred browser.

for web application
```sh
localhost:4260
```
for web api
```sh
localhost:5766
```
### Credentials

 By default, the migrations folder has default and additional seed data which will be run when you apply the migrations. 
 Some of the default accounts : 
 ```
   - Name: testni + (any number from 1 to 12); examples : testni1, testni2, testni3 
   - Password : test123.
```
Also
```
- Name: mobile
- Password: test
   
- Name: desktop
- Password: test
```


