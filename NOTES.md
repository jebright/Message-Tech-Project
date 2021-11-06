# Notes for the Message Tech Project
I added two main sections here: Instructions and Change Log.  The Change Log provides a brief summary of the major changes
to the application whereas the Instructions section gives you the information you need to run this program.  Before reading 
those sections, a brief summary of how I tackled the project is provided below.

## My Approach to the API Dependencies
The DailyWatchlistEmails depends on two APIs to function.  
1. Articles API
2. Mail Champ API

The original project provided only a MailChap_swagger.json file.  I used that to create a new ASP.NET CORE solution by loading this swagger
file into the online swagger editor and invoking its feature to generate the server code.  This was a fast and easy way to get a Mail Champ API
up and running that met the API definitions provided.

The original project however did not have a swagger file for the Articles API.  But it did provide a good data model as well as a sample API
call.  This gave enough information so that I could build my own ASP.NET CORE solution for this API.  To do so I utilized Swashbuckle and
coded a sample response for 6 articles, each with one instrument.  The articles were for: Amazon, Netflix, Google, Roku, Alcoa, and Advance Auto Parts. 

## Significant Code Changes to DailyWatchlistEmails
Generally speaking, the approach taken here was to aggregate together the email subscriptions and instruments to make them 
easier/faster to process.  This removed the need to go out to the database for each user in a loop.  Example data is:
```
JeanLivingston@email.com : 1,3,5
```
as opposed to

```
JeanLivingston@email.com	1
JeanLivingston@email.com	3
JeanLivingston@email.com	5
```

The second big change was to batch up the sending of emails to the MailChamp API as opposed to sending one record to the API at a time.
See the new implemenation of sendEmails as to how this was done using parallel async tasks.

## Future Improvements and Conclusion
I wanted to keep this around 8 hours per the project 'goal'.  This does leave a few things left for improvement.  But I do feel
that I was able to make some major improvements to the app and showcase my abilities along the way.  It was also a fun project to tackle!

A few improvements that were left undone:
* unit testing.  I would have liked to have spent time here but doing so would have meant far exceedinghe 8 hours as unit testing
generally speaking takes 2x the amount of work as coding in order to get even decent coverage.
* parameter checking.  Many of the methods dont go far enough to ensuring parameters are valid.
* error handling.  Although I did implement basic error handling and logging.  It is simple and has room for improvement.
* prepare templates and xml creation.  There was room for improvement in this method but other than refactoring it to use the more
modern httpclient, I left it mostly untouched.  I would for example use StringBuffer over string in this code.
* The WatchlistEmailService breaks a few best practices - for example the single responsibility principle.  I had intended to break
this out into multiple services but I left that to the end and ultimately ran out of time.  On further review though, this being such
a small project it is pretty manageable as is.
* The EF code that pulls data from the view is pretty efficient as is but if the number of records were to get into the tens or hundreds
of thousands, I would suggest a minor refactor to page the data from the database as opposed to grabbing it all at once.

# Instructions

## Step 1. Make sure you have the repos
To begin with, ensure you have the following repositories cloned.  Repository locations were provided via email.
1. Message-Tech-Project
2. ArticlesApi
3. MailChampApi

## Step 2. Database preparation.
Next, you should create the new database view in the Fool database.  The SQL definition can be found in the 
root folder (next to the .sln file).  It is named fool-database-add-views.sql and provides a view definition compatible with
SQLS 2017 and later as well as SQLS 2016 and earlier depending on your installed version.  Make sure to execute the right one.

And finally, you should also take a look at the connection string for the SQL Server database in appsettings.json.
It may need to be adjusted for your SQLS instance:
```
"Fool": "Server=localhost;Initial Catalog=Fool;Integrated Security=true;"
```

## Step 3. Build and run the APIs
Each API needs to be up and running as the DailyWatchlistEmails program is configured to connect to both of the APIs.
1. Build and run Articles API.  This is found in the ArticleApi repository (ArticleApi.sln).  
Note the port number as you will need it when you configure the DailyWatchlistEmails program connections.
2. Build and run the MailChamp API.  This is found in the MailChampApi repository (IO.Swagger.sln).
Note the port number as you will need it when you configure the DailyWatchlistEmails program connections.
3. Open the DailyWatchlistEmails.sln and locate the appsettings.json file.  Modify the ports for the corresponding
APIs that you collected in step 1 and step 2 above.

Example:
```
  "MailChampUri": "http://localhost:50352/",
  "ArticleApiUri": "https://localhost:44386/api/",
```
4. You are now ready to give the program a try.  Build and run from within Visual Studio.  The program, when finished, 
will write details of its execution to a log file in the debug folder (right beside the application exe). You will want
to open this file and examine its contents.  Hopefully you will see at the bottom a log entry as follows:
```
[INF] ************************ Execution completed successfully ************************
```

### Troubleshooting
If you experience problems runing the app please let me know.  Here are some things to consider.
* This project now targets .NET 5.  A framework install may be needed depending on your environment.
* Numerous nuget packages were added.  It is necessary to download and restore these nuget packages.
* Ensure the appsettings.json is being copied to the output folder.  Otherwise the program cannot read the necessary config values.
* Check the configuration values for accuracy in the appsettings.json file.

# Change Log
* DB Connection in App.config switched to localhost
* Migrated to .NET 5
* Switched MailChampApiUri in the App.Config from https://localhost:44319/ to http://localhost:50352/.
  This allowed me to work with a ASP.NET CORE WEBAPI project that was built based on the MaiChamp_swagger.json file that was provided with the original project.
* Switched ArticleApiUri in the App.Config from https://localhost:44305/ to https://localhost:44386/api/
  This allowed me to work with a ASP.NET CORE WEBAPI project (ArticlesApi) that was built based on the Article models that were provided with the original project.
* Added a models folder and moved models to that folder.  Also created the DbContext FoolContext.  In a bigger project it would be a better decision to place these in a separate project.
* Configured app to use appsettings.json instead of App.config
* Setup dependency injection
* Created a DB view named vw_subscriptions which aggregates the subscriber/instrument data together for easier client side processing.
* Created the FoolRepository to obtain data via EF.
* Refactored WatchlistEmailServices to use the new EF repository.
* Refactored WebRequest code to use IHttpClientFactory.
* Additional refactoring on WatchlistEmailService (making members and methods private where appropriate)
* Modified how emails are sent.  According to the API docs on '/Mailings/{mailingId}/send', we can send up to 1024 records per request.
* Added error handling and moved the execution of the mulitiple steps in this program out of Program.cs into the service (see new Execute method)
* Added in Serilog
