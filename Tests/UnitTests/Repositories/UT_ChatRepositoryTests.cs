using DataAccessLayer.Repositories;
using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.UnitTests.Repositories
{
    [TestFixture()]
    public class UT_ChatRepositoryTests
    {
        private Mock<BBDDContext>? _mockContext;
        private ChatRepository? _chatRepository;

        [SetUp]
        public void SetUp()
        {
            _mockContext = new Mock<BBDDContext>();
            _chatRepository = new ChatRepository(_mockContext.Object);
        }

        /// <summary>
        /// Tests the AddChatMessage method to verify it successfully adds a new chat message.
        /// </summary>
        /// <remarks>
        /// This test checks if the AddChatMessage method returns true when provided with valid parameters.
        /// </remarks>
        [Test()]
        public async Task AddChatMessageTest_Success()
        {
            // Arrange
            var userId = "validuser";
            var userChat = "userChat";
            var source = true;
            var message = "Test message";
            var datetime = DateTime.UtcNow;

            var user = new AppUser { Login = userId, UserId = Guid.NewGuid() };
            var mockDbSetUsers = new Mock<DbSet<AppUser>>();
            mockDbSetUsers.As<IQueryable<AppUser>>().Setup(m => m.Provider).Returns(new List<AppUser> { user }.AsQueryable().Provider);
            mockDbSetUsers.As<IQueryable<AppUser>>().Setup(m => m.Expression).Returns(new List<AppUser> { user }.AsQueryable().Expression);
            mockDbSetUsers.As<IQueryable<AppUser>>().Setup(m => m.ElementType).Returns(new List<AppUser> { user }.AsQueryable().ElementType);
            mockDbSetUsers.As<IQueryable<AppUser>>().Setup(m => m.GetEnumerator()).Returns(new List<AppUser> { user }.AsQueryable().GetEnumerator());

            var mockDbSetChats = new Mock<DbSet<AppChat>>();
            _mockContext!.Setup(c => c.AppUsers).Returns(mockDbSetUsers.Object);
            _mockContext!.Setup(c => c.AppChats).Returns(mockDbSetChats.Object);

            _mockContext!.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _chatRepository!.AddChatMessage(userId, userChat, source, message, datetime);

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests the GetUserChatMessagesAsync method to verify it returns all chat messages for a user.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetUserChatMessagesAsync method returns a list of all chat messages when provided with a valid userId.
        /// </remarks>
        [Test()]
        public void GetUserChatMessagesAsyncTest_Success()
        {
            // Arrange
            var userId = "validuser";
            var userGuid = Guid.NewGuid();
            var expectedMessages = new List<AppChat>
            {
                new AppChat { UserId = userGuid, UserName = "userChat1", Source = true, Message = "Test message 1", Datetime = DateTime.UtcNow },
                new AppChat { UserId = userGuid, UserName = "userChat2", Source = false, Message = "Test message 2", Datetime = DateTime.UtcNow }
            };

            var user = new AppUser { Login = userId, UserId = userGuid };
            var mockDbSetUsers = new Mock<DbSet<AppUser>>();
            mockDbSetUsers.As<IQueryable<AppUser>>().Setup(m => m.Provider).Returns(new List<AppUser> { user }.AsQueryable().Provider);
            mockDbSetUsers.As<IQueryable<AppUser>>().Setup(m => m.Expression).Returns(new List<AppUser> { user }.AsQueryable().Expression);
            mockDbSetUsers.As<IQueryable<AppUser>>().Setup(m => m.ElementType).Returns(new List<AppUser> { user }.AsQueryable().ElementType);
            mockDbSetUsers.As<IQueryable<AppUser>>().Setup(m => m.GetEnumerator()).Returns(new List<AppUser> { user }.AsQueryable().GetEnumerator());

            var mockDbSetChats = new Mock<DbSet<AppChat>>();
            mockDbSetChats.As<IQueryable<AppChat>>().Setup(m => m.Provider).Returns(expectedMessages.AsQueryable().Provider);
            mockDbSetChats.As<IQueryable<AppChat>>().Setup(m => m.Expression).Returns(expectedMessages.AsQueryable().Expression);
            mockDbSetChats.As<IQueryable<AppChat>>().Setup(m => m.ElementType).Returns(expectedMessages.AsQueryable().ElementType);
            mockDbSetChats.As<IQueryable<AppChat>>().Setup(m => m.GetEnumerator()).Returns(expectedMessages.AsQueryable().GetEnumerator());

            _mockContext!.Setup(c => c.AppUsers).Returns(mockDbSetUsers.Object);
            _mockContext.Setup(c => c.AppChats).Returns(mockDbSetChats.Object);

            // Act
            var result = _chatRepository!.GetUserChatMessagesAsync(userId);

            // Assert
            Assert.That(result.Count, Is.EqualTo(expectedMessages.Count));
            Assert.That(result[0].Message, Is.EqualTo(expectedMessages[0].Message));
            Assert.That(result[1].Message, Is.EqualTo(expectedMessages[1].Message));
        }
    }
}
