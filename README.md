# Band Tracker
## Created by Olena Kuchko

### Description
Web application that helps user to manage a database of bands and venues. User is able to add, update and delete bands and venues.

### Installation
1. Download or clone the repository from https://github.com/LenaKuchko/BandTracker.git
2. Using PowerShell (for Windows), navigate to the directory in which you downloaded project.
3. To recreate a database use SSMS or another similar application.
4. In PowerShell run the next command: sqlcmd -S "(localdb)\mssqllocaldb" or sqlcmd -S (particular name of SQLServer instance on your PC).
5. To recreate database open SSMS, Select File > Open > File and select .sql file in folder that you have cloned. To run script click Execute.
6. In PowerShell then run dnu restore and dnx kestrel.
7. In your Web-browser enter - localhost:5004.
8. Using IDE of your choice to open code.

### Known Bugs.

### Specs

| Behavior | Input | Output |
|----------|-------|--------|
|User adds a venue | "Moda Center" | App displays new venue with name "Mode Center" |  
|User can view information about venue | Click on venue's name | App displays information about venue including all bands which belong to this venue |
|User add a band | "Adel" | App all bands including new one |
|User is able to update venue. Change name "Moda Center" | "Music Hall" | App displays updated information "Music Hall" |
|User is able to delete venue | Click on "delete venue" button | App displays confirmation about deleting venue|
|User is able to delete all venue | Click on "delete all" button| App displays confirmation about deleting All venues|

## Technologies used
* Boostrap
* C#
* HTML
* JSON
* Nancy
* Razor
* SQL
* xUnit

#### License
This project is licensed under the MIT License.
Copyright (c) 2017  Olena Kuchko
