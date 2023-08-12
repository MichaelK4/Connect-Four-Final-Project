# Connect Four Final Project

This README document provides an overview of the Connect Four Game Project that utilizes ASP.NET for the website server-side and Windows Forms for the client-side game GUI.

## Project Description
This project aims to create an interactive and engaging gaming experience where users can play the classic Connect Four game through a user-friendly Windows Forms GUI. The project is divided into two main components: The Server-side and the Client-side.

## Server-Side
The project's server-side is implemented using ASP.NET Core Web Application, which provides a robust framework for handling web applications and APIs. The server is responsible for:

* Managing user accounts: When a client wants to log in to the game he enters his username the client-side asks the server if the user exists in the server's database.
* Game data storage: Basic game data is stored on the server to keep track of ongoing games.
* User statistics: The server updates the user's statistics when a game is completed, incrementing the total number of games played.

## Client-Side
The client-side of the project is implemented using Windows Forms, providing a graphical user interface for players to interact with the game. The client-side GUI offers the following features:

* Play: Users can make moves in the Connect Four game by selecting columns on the game board.
* Replay: Users have the ability to replay previously played games.
* Resume: If a game was paused, users can resume the game from where they left off.
* Restart: Users can restart the current game without saving the previous state.
* New Game: Users can start a new game from scratch.
* View Game History: Users can view their previous game history stored on the client-side database.

## Website Features
The website component of the project provides additional functionalities accessible through a web interface:
* Edit users: Users can edit their information.
* Delete User: You have the option to delete your user, including all your game data.
* View User Details: Detailed information about users, including their game statistics, can be viewed.
* View Game Details: Information about individual games, such as outcome and duration, can be accessed.
* Statistics Page: The website offers a statistics page featuring various queries and views to showcase different data from the user and game tables.


## Getting Started
To run the Connect Four Game Project locally, follow these steps:

1. Open the ConnectFourServer.sln on Visual Studio
2. In the upper part of Visual Studio select 'Tools' -> 'NuGet Package Manager' -> 'Package Manager Console'.
3. Write 'Update-Database' in the console and run.
4. Now you can run the solution itself.
5. A website will open on your main browser.
6. Because the database is new on your computer you won't see any previous data, it will be empty.
7. Register yourself.
8. Now open the ConnectFourClient.sln on Visual Studion.
9. In both Program.cs and DatabaseBox.cs(right mouse click -> 'view code') change the path of the 'connStr' to the path to the 'Database.mdf' file in the 'ConnectFourClient' Folder.
10. Run the client solution.
11. Enter your username and login, if you didn't enter a username before you can press the sign-up button and it will take you to the register page(only if the server side is running).
12. Enjoy the game.
13. If you want to save your game to the database you can press either Pause/New game/ End game. If you finished the game and you Won/Lost or made a draw the game won't be saved to the database.


# Conclusion
The Connect Four Game Project, completed as part of a .NET course, demonstrates the implementation of the classic game. The project showcases the use of ASP.NET for server-side components and Windows Forms for the client-side interface. The integration of user accounts, game storage, and statistics tracking enhances the user experience and provides a well-rounded gaming platform.

### Project Team:
* Michael Kogan
* Itay Ifrach











   
