# infoShare Coaches-app
This repository contains the source code for the web application for managing coach list (CRUD actions). The application is written in C# using ASP.NET Core 3.1. It contains of several projects and 2 separate databases (one for coaches, and one for logging the events).

## Requirements
* Visual Studio 2019 with ASP.NET Core 3.1
* SQL Server 2016

## Setup
* Clone the repository
    * ${MainDir} = local path where you put the repository
* Locate the database creation script files:
    * ${MainDir}/Coaches.MainApp/db_scripts/Coaches_create.sql
    * ${MainDir}/Coaches.Tracking/db_scripts/Tracking_create.sql
* In both files replace the value of the @directory variable to the path, where the database file should be located, after it is created
* Run both scripts separately (preferably using SQL Server Management Studio)

## Launching
* Run the Visual Studio
* Go to Solution's properties window
* In the **Startup Project** section choose **Multiple startup projects:**, then below select value **Start** in the **Action** column for projects: **Coaches.MainApp** and **Coaches.Tracking**
* Close the window and run the application
    * Coaches.MainApp should be running locally
    * Coaches.Tracking should be running separately on IIS Express