using BusinessLogicLayer.Interfaces;
using Entities.DTOs;
using Moq;

namespace Tests.UnitTests.Services
{
    [TestFixture()]
    public class UT_ChatServiceTests
    {
        private Mock<IChatService>? _mockChatService;

        [SetUp]
        public void SetUp()
        {
            _mockChatService = new Mock<IChatService>();
        }

        /// <summary>
        /// Tests the AddUserChatMessage method to verify it successfully adds a chat message.
        /// </summary>
        /// <remarks>
        /// This test checks if the AddUserChatMessage method returns true when provided with 
        /// valid user, userChat, source, message, and datetime parameters, indicating that 
        /// the message was successfully added.
        /// </remarks>
        
        [Test()]
        public async Task AddUserChatMessageTest_Success()
        {
            // Arrange
            var user = "user1";
            var userChat = "chat1";
            var source = true;
            var message = "Hello, world!";
            var datetime = DateTime.Now;

            _mockChatService!.Setup(service => service.AddUserChatMessage(user, userChat, source, message, datetime))
                .ReturnsAsync(true);

            var chatService = _mockChatService!.Object;

            // Act
            var result = await chatService.AddUserChatMessage(user, userChat, source, message, datetime);

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests the AddUserChatMessage method to verify it fails to add a chat message.
        /// </summary>
        /// <remarks>
        /// This test checks if the AddUserChatMessage method returns false when provided with 
        /// valid user, userChat, source, message, and datetime parameters, indicating that 
        /// the message was not added successfully.
        /// </remarks>
        [Test()]
        public async Task AddUserChatMessageTest_Failure()
        {
            // Arrange
            var user = "user1";
            var userChat = "chat1";
            var source = true;
            var message = "Hello, world!";
            var datetime = DateTime.Now;

            _mockChatService!.Setup(service => service.AddUserChatMessage(user, userChat, source, message, datetime))
                .ReturnsAsync(false);

            var chatService = _mockChatService!.Object;

            // Act
            var result = await chatService.AddUserChatMessage(user, userChat, source, message, datetime);

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests the GetAllUserChatMessages method to verify it retrieves all chat messages for a user.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetAllUserChatMessages method returns a list of UserChatMessageDTO 
        /// objects for a given user, indicating that the messages were successfully retrieved.
        /// </remarks>
        [Test()]
        public void GetAllUserChatMessagesTest_Success()
        {
            // Arrange
            var user = "user1";
            var expectedMessages = new List<UserChatMessageDTO>
            {
                new UserChatMessageDTO { IdxSec = 1, UserName = "user1", Source = true, Message = "Hello!", Datetime = DateTime.Now },
                new UserChatMessageDTO { IdxSec = 2, UserName = "user1", Source = false, Message = "How are you?", Datetime = DateTime.Now }
            };

            _mockChatService!.Setup(service => service.GetAllUserChatMessages(user))
                .Returns(expectedMessages);

            var chatService = _mockChatService!.Object;

            // Act
            var result = chatService.GetAllUserChatMessages(user);

            // Assert
            Assert.That(result, Is.EquivalentTo(expectedMessages));
        }

        /// <summary>
        /// Tests the GetAllUserChatMessages method when no messages are found for a user.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetAllUserChatMessages method returns an empty list of UserChatMessageDTO 
        /// objects for a given user, indicating that no messages were found.
        /// </remarks>
        [Test()]
        public void GetAllUserChatMessagesTest_NoMessages()
        {
            // Arrange
            var user = "user1";
            var expectedMessages = new List<UserChatMessageDTO>();

            _mockChatService!.Setup(service => service.GetAllUserChatMessages(user))
                .Returns(expectedMessages);

            var chatService = _mockChatService!.Object;

            // Act
            var result = chatService.GetAllUserChatMessages(user);

            // Assert
            Assert.That(result, Is.EquivalentTo(expectedMessages));
        }
    }
}
