using ChessServer.Controllers;
using ChessServer.Interfaces;
using ChessServer.Models.Entities;
using ChessServer.Models.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ChessServer.Tests
{
    public class ChessGameControllerTest
    {
        private readonly Mock<IChessGameRepository> _chessGameRepository;
        private readonly Mock<IUserRepository> _userRepository;
        
        public ChessGameControllerTest()
        {
            _chessGameRepository = new Mock<IChessGameRepository>();
            _userRepository = new Mock<IUserRepository>();
        }
        [Fact]
        public void ChessGameController_GetChessGameById_Success()
        {
            var chessGame = new ChessGame(1, "a", 2, "b", new DateTime(1, 1, 1), new DateTime(1, 1, 1), "", 10, 5, "", "", 0, "");
            var chessGameResponse = new ChessGameResponse(chessGame);
            _chessGameRepository.Setup(x => x.GetChessGameById(1)).Returns(chessGame);
            var chessGameController = new ChessGameController(_chessGameRepository.Object, _userRepository.Object);

            var actionResult = chessGameController.GetChessGameById(1);


            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var chessGameResponseResult = Assert.IsType<ChessGameResponse>(okResult.Value);

            Assert.NotNull(chessGameResponseResult);
            chessGameResponse.Should().BeEquivalentTo(chessGameResponseResult);
        }
        [Fact]
        public void ChessGameController_GetChessGamesByUsername_Success()
        {
            var chessGame1 = new ChessGame(1, "a", 2, "b", new DateTime(1, 1, 1), new DateTime(1, 1, 1), "", 10, 5, "", "", 0, "");
            var chessGame2 = new ChessGame(2, "b", 1, "a", new DateTime(1, 1, 1), new DateTime(2, 2, 2), "", 10, 5, "", "", 0, "");
            var chessGameResponse1 = new ChessGameResponse(chessGame1);
            var chessGameResponse2 = new ChessGameResponse(chessGame2);
            var chessGames = new List<ChessGame> { chessGame1, chessGame2 };
            _chessGameRepository.Setup(x => x.GetChessGamesByUsername("a")).Returns(chessGames);
            var chessGameController = new ChessGameController(_chessGameRepository.Object, _userRepository.Object);

            var actionResult = chessGameController.GetChessGamesByUsername("a");


            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var chessGameResponsesResult = Assert.IsType<List<ChessGameResponse>>(okResult.Value);
            Assert.NotNull(chessGameResponsesResult);
            chessGameResponse1.Should().BeEquivalentTo(chessGameResponsesResult[0]);
            chessGameResponse2.Should().BeEquivalentTo(chessGameResponsesResult[1]);
        }
    }
}