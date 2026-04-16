//Setup up database and seed it with starting data.
var database = new Database();
database.Initialize();
database.Seed();

//Program loop the menu has the options to kill the environment, so it will run until the user selects that option.
while (true)
{
    var menu = new MenuService(database);
    menu.DisplayMain();
}