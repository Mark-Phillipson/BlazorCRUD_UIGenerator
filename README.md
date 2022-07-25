# Blazor CRUD UI Generator

![](DynamicCRUD/wwwroot/images/DynamicCRUD.png)

This project can be used to create the classes and Blazor components to provide rudimentary CRUD functionality for any table in a SQL Server.

The classes are all created within the project under a folder called AutoGenClasses, with the intention of moving them into your project.

## Quick Start — How to Use

1. Edit the appsettings.json file in the root of the project. To make the DefaultConnection point to your database in SQL Server.

2. Run the application (Do not run watch).

3. Enter a search term to filter down to a particular table, then select the table in the drop-down list.

4. Then click on Populate Columns button.

5. Then enter the destination project, this is used to build the namespace for the classes.

6. Enter a singular model name and a plural name. Note the model should already exist in the destination project and be entered in the DB Context.

7. The list of columns should now be displayed.  Check the filter checkboxes for each column that you wish to filter on. Also check each sort column that you wish to sort by.

8. If the primary key is set up correctly the PK Override should not be necessary.  However if the primary key has not been set up correctly it may be necessary to use the override.

9. If the table has a parent, you can select a foreign key (One only).

10. Then click Generate C# Classes button.

11. The classes should now exist in the AutoGenClasses folder.  Noted they will not compile in this project.

12. Copy the code that is displayed red in the webpage into your project. 

13. Move each file to the required location in your project.  Taking care to make sure the namespaces are correct

## Dependencies

1. Automapper will need to be set up in your project.
2. Blazored Modal and Blazored Toast will need to be set up.
3. Ardalis.GuardClauses NuGet Package will also need to be installed.
4. When deleting a record created classes will try to call a Blazor component called BlazoredModalConfirmDialog. This can be found in the project under Shared folder.
5. Each model will need to exist in your project along with the DB context with the DbSet already added.
6. The Blazor components are set up to use Bootstrap 5.

## Files Created

* Model DTO
* Interface and Repository Class
* Interface and Service Class
* Model Table Blazor Page Component and Code Behind
* Model Add Edit Blazor Component and Code Behind

## T4 Templates

In order to make any enhancements or changes, there are T4 Templates for each of the files listed above.

## Please Note

Only tested with:
* Blazor Server (Not Blazor Client)
* SQL Server Database
* .net 6



