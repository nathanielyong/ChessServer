# Chess Arena

Chess Arena is a full-stack chess website built with ASP.NET backend and React.js frontend. This project makes use of clean architecture and SOLID principles to ensure scalability, maintainability, and high-quality code. 

You can check the website out here
[ChessArena](chessservernathan2-hzh6a4hsbjfxabhf.canadacentral-01.azurewebsites.net)

![image](https://github.com/user-attachments/assets/8a336e5e-cec3-495a-87b6-9c5b1bf8caf2)

## Features

- Live Chess Games:
    - Start new game with opponent, Make move, Resign, Offer draw, Accept/Decline draw offer, Live chat with opponent.
    - Check game state, Get valid moves, Finish game and store into game history.
  
- Game Management: Create, retrieve, update, and delete completed chess games.

- Player Management: Register, login, retrieve player information, and update player profiles.

- Authentication and Authorization: Secure endpoints using JWT (JSON Web Tokens) for authentication 

## Installation

1. Install .NET 8.0 SDK https://dotnet.microsoft.com/en-us/download/dotnet/8.0
2. Install Node.js
3. Clone the repository

### Frontend

4. cd into the `client` folder
5. Run `npm install` to install the frontend dependencies
6. Run `npm run build` to build the files for production

### Backend

7. Go back to the root folder
8. Run `dotnet restore` to install the .NET dependencies
9. Run `dotnet run` to build and start up the backend

### Database

10. You may need to provision your own SQL Server database in order to use this application locally.
    - On MacOS, you can install Docker Desktop and run a SQL Server container image https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver16&tabs=cli&pivots=cs1-bash
    - On Windows, you can simply install SQL Server
11. Once you've created a SQL Server database, you can connect it to ASP.NET by going to appsettings.json and replacing the `"DefaultConnection"` string field with your own connection string.
12. In the root folder, run `dotnet ef migrations add InitialCreate` to create a new database migration based on the models.
13. Update the database using `dotnet ef database update`

