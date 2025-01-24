# Chess Server

This Chess Server API is built using ASP.NET Core Web API and provides endpoints for managing chess games, live chess games, and players. This project makes use of clean architecture and SOLID principles by dividing the application into different layers and making use of dependency injection. 

The API was deployed to Azure using Azure App Service and Azure SQL Database.

You can test out endpoints using this URL: https://chessservernathan.azurewebsites.net

## Features

Game Management: Create, retrieve, update, and delete chess games.

Live Chess Games: Start new game with opponent, Make move, Resign, Check game state, Get valid moves, Store game into game history

Player Management: Register, login, retrieve player information, and update player profiles.

Authentication and Authorization: Secure endpoints using JWT (JSON Web Tokens) for authentication 

## Endpoints

### Auth controller

POST /register\
Example:
{
  "username": "user",
  "email": "user@example.com",
  "password": "password"
}

POST /login\
Example: 
{
  "username": "user",
  "password": "password"
}

### ChessGame controller

Endpoints for retrieving game history or creating a new chess game record.

GET /api/ChessGame/myGames

GET /api/ChessGame/user/{username}/whiteGames

GET /api/ChessGame/user/{username}/blackGames

GET /api/ChessGame/user/{username}/games

GET /api/ChessGame/{id}

POST /api/ChessGame/createGame\
Example: 
{
  "whitePlayerUsername": "user1",
  "blackPlayerUsername": "user2",
  "dateStarted": "2024-05-08",
  "dateFinished": "2024-05-08",
  "result": "1-0",
  "gameEndReason": "White wins by resignation",
  "moves": 1,
  "startTime": 90,
  "increment": 30,
  "pgn": "1. e4 e5"
}

### LiveChessGame controller
Endpoints for creating new Live Chess Game or playing moves on a current game.

GET /api/LiveChessGame/getGame/{id}

GET /api/LiveChessGame/getCurrentGame\
Info: Returns the current game state information including the current position and legal moves.

POST /api/LiveChessGame/makeMove?move={e4}

POST /api/LiveChessGame/resignGame

POST /api/LiveChessGame/newGame\
Example:
{
  "colour": "White",
  "opponentUsername": "user",
  "startTime": 90,
  "increment": 30
}\
Info: Creates a new game with opponent username unless opponent user is already in a game.

### User controller


GET /profile

GET /getStats/{username}

