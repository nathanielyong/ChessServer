using Microsoft.AspNetCore.SignalR;
using ChessServer.Interfaces;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;
using ChessServer.Models.Entities;
using ChessServer.Models.Responses;

namespace ChessServer.Hubs
{
    [Authorize]
    public class LiveChessGameHub : Hub
    {
        private readonly ILiveChessGameService _liveChessGameService;
        private readonly IUserRepository _userRepository;
        private readonly ILiveChessGameRepository _liveChessGameRepository;
        private static readonly ConcurrentDictionary<string, string> _gameConnections = new ConcurrentDictionary<string, string>();
        private static readonly ConcurrentDictionary<string, string> _userConnections = new ConcurrentDictionary<string, string>();

        public LiveChessGameHub(ILiveChessGameService liveChessGameService, IUserRepository userRepository, ILiveChessGameRepository liveChessGameRepository)
        {
            _liveChessGameService = liveChessGameService;
            _userRepository = userRepository;
            _liveChessGameRepository = liveChessGameRepository;
        }

        public async Task JoinGame(string gameId)
        {
            string username = Context.User.Identity.Name;
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            _gameConnections[Context.ConnectionId] = gameId;
            await Clients.Group(gameId).SendAsync("JoinGame", $"{username} has joined game: {gameId}");
        }

        public async Task MakeMove(string move)
        {
            var username = Context.User.Identity.Name;
            var response = _liveChessGameService.MakeMove(username, move);
            if (response == null)
            {
                await Clients.Caller.SendAsync("MakeMoveError", "Error: Illegal move");
            }
            else
            {
                await Clients.Group(_gameConnections[Context.ConnectionId]).SendAsync("MakeMove", response);
            }
        }

        public async Task SendMessage(string message)
        {
            string username = Context.User.Identity.Name;
            await Clients.OthersInGroup(_gameConnections[Context.ConnectionId]).SendAsync("SendMessage", username, message);
        }

        public async Task ResignGame()
        {
            string username = Context.User.Identity.Name;
            var response = _liveChessGameService.ResignGame(username);
            if (response != null)
            {
                await Clients.Group(_gameConnections[Context.ConnectionId]).SendAsync("ResignGame", response, username);
            }

        }

        public async Task DrawOffer()
        {
            string username = Context.User.Identity.Name;
            LiveChessGameResponse game = _liveChessGameService.GetGameState(username);
            if (game != null) {
                await Clients.OthersInGroup(_gameConnections[Context.ConnectionId]).SendAsync("DrawOffer", username);
            }
        }

        public async Task ReplyToDrawOffer(string message)
        {
            string username = Context.User.Identity.Name;
            if (message == "Decline") {
                await Clients.OthersInGroup(_gameConnections[Context.ConnectionId]).SendAsync("DeclineDrawOffer", username);
            } else if (message == "Accept") {
                var response = _liveChessGameService.DrawGame(username);
                if (response != null)
                {
                    await Clients.Group(_gameConnections[Context.ConnectionId]).SendAsync("AcceptDrawOffer", username, response);
                }
            }
        }

        public async Task OnConnectedAsync()
        {
            string username = Context.User.Identity.Name;
            _userConnections[username] = Context.ConnectionId;
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string username = Context.User.Identity.Name;
            if (_gameConnections.TryRemove(Context.ConnectionId, out var gameId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
                await Clients.Group(gameId).SendAsync("UserDisconnected", $"User {username} disconnected from game: {gameId}");
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}