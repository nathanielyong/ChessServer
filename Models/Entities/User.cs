﻿using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace ChessServer.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public DateTime DateJoined { get; set; }
        public int NumGamesPlayed
        {
            get
            {
                int whiteGamesCount = WhiteChessGames == null ? 0 : WhiteChessGames.Count;
                int blackGamesCount = BlackChessGames == null ? 0 : BlackChessGames.Count;

                return whiteGamesCount + blackGamesCount;
            }
        }
        public int Wins {  get; set; }
        public int Losses {  get; set; }
        public int Draws {  get; set; }
        public int Rating { get; set; }
        public ICollection<ChessGame> WhiteChessGames { get; set; }
        public ICollection<ChessGame> BlackChessGames { get; set; } 
        public User()
        {
        }
        public User(string username, string email, string hashedPassword)
        {
            Username = username;
            Email = email;
            HashedPassword = hashedPassword;
            WhiteChessGames = new List<ChessGame>();
            BlackChessGames = new List<ChessGame>();
            DateJoined = DateTime.Now;
            Wins = 0;
            Losses = 0;
            Draws = 0;
            Rating = 1200;
        }
    }
}