
using LegacyApp;
using Moq;


namespace LegacyAppTests
{
    public class UserServiceTests
    {
        private readonly Mock<IClientRepository> _clientRepositoryMock = new Mock<IClientRepository>();
        private readonly Mock<IUserCreditService> _userCreditServiceMock = new Mock<IUserCreditService>();
        private UserService _service;

        public UserServiceTests()
        {
            _service = new UserService(_clientRepositoryMock.Object, _userCreditServiceMock.Object);
        }

        [Fact]
        public void AddUser_Should_Return_False_When_Missing_FirstName()
        {
            var result = _service.AddUser(null, "Doe", "johndoe@example.com", new DateTime(2000, 1, 1), 1);
            Assert.False(result);
        }
        
        [Fact]
        public void AddUser_Should_Return_False_When_Missing_At_Sign_And_Dot_In_Email()
        {
            var result = _service.AddUser("John", "Doe", "johndoeatexamplecom", new DateTime(2000, 1, 1), 1);
            Assert.False(result);
        }
        
        [Fact]
        public void AddUser_Should_Return_False_When_Younger_Then_21_Years_Old()
        {
            var result = _service.AddUser("John", "Doe", "johndoe@example.com", DateTime.Now.AddYears(-20), 1);
            Assert.False(result);
        }
        
        [Fact]
        public void AddUser_Should_Return_True_When_Very_Important_Client()
        {
            _clientRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns(new Client { Type = "VeryImportantClient" });
            var result = _service.AddUser("John", "Doe", "johndoe@example.com", new DateTime(1980, 1, 1), 1);
            Assert.True(result);
        }
        
        [Fact]
        public void AddUser_Should_Return_False_When_Normal_Client_And_Credit_Limit_Less_Than_500()
        {
            _clientRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns(new Client { Type = "NormalClient" });
            _userCreditServiceMock.Setup(credit => credit.GetCreditLimit(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(300);
            var result = _service.AddUser("John", "Doe", "johndoe@example.com", new DateTime(1980, 1, 1), 1);
            Assert.False(result);
        }
        
    }
}
