using Moq;
using ChessServer.Controllers;
using ChessServer.Repository;
using ChessServer.Interfaces;
using Microsoft.Extensions.Configuration;
using ChessServer.Models.Requests;
using ChessServer.Models.Entities;
using ChessServer.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ChessServer.Tests
{

    public class AuthControllerTest
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IAuthService> _authService;
        public AuthControllerTest()
        { 
            _userRepository = new Mock<IUserRepository>();
            _authService = new Mock<IAuthService>();
        }
        [Fact]
        public void AuthController_Login_Success()
        {
            User user = new User("a", "a@a.com", "password");
            LoginRequest loginRequest = new LoginRequest("a", "password");
            _userRepository.Setup(x => x.GetUserByUsername("a")).Returns(user);
            _authService.Setup(x => x.Login(user)).Returns("token");
            var authController = new AuthController(_userRepository.Object, _authService.Object);

            var actionResult = authController.Login(loginRequest);

            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var token = Assert.IsType<String>(okResult.Value);
            Assert.NotNull(token);
            Assert.Equal(token, "token");
        }
        [Fact]
        public void AuthController_Register_Success()
        {

        }
    }
}