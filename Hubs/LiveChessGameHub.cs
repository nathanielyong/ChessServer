using Microsoft.AspNetCore.SignalR;
using ChessServer.Interfaces;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;

namespace ChessServer.Hubs
{
    [Authorize]
    public class LiveChessGameHub : Hub
    {
        private readonly ILiveChessGameService _liveChessGameService;
        private readonly IUserRepository _userRepository;
        private readonly ILiveChessGameRepository _liveChessGameRepository;
        private static readonly ConcurrentDictionary<string, string> _connections = new ConcurrentDictionary<string, string>();

        public LiveChessGameHub(ILiveChessGameService liveChessGameService, IUserRepository userRepository, ILiveChessGameRepository liveChessGameRepository)
        {
            _liveChessGameService = liveChessGameService;
            _userRepository = userRepository;
            _liveChessGameRepository = liveChessGameRepository;
        }

        public async Task JoinGame(string gameId)
        {
            string username = Context.User.Identity.Name;
            Console.WriteLine(Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            _connections[Context.ConnectionId] = gameId;
            await Clients.Group(gameId).SendAsync("JoinGame", $"{username} has joined game: {gameId}");
        }

        public async Task MakeMove(string move)
        {
            if (move == null)
            {
                await Clients.Caller.SendAsync("MakeMoveError", "Error: move cannot be null");
                return;
            }

            var username = Context.User.Identity.Name;
            var response = _liveChessGameService.MakeMove(username, move);
            if (response == null)
            {
                await Clients.Caller.SendAsync("MakeMoveError", "Error: Illegal move");
            }
            else
            {
                await Clients.Group(_connections[Context.ConnectionId]).SendAsync("MakeMove", response);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string username = Context.User.Identity.Name;
            if (_connections.TryRemove(Context.ConnectionId, out var groupId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
                await Clients.Group(groupId).SendAsync("UserDisconnected", $"User {username} disconnected from game: {groupId}");
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}