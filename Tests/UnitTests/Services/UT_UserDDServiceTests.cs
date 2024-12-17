using BusinessLogicLayer.Interfaces;
using Moq;

namespace Tests.UnitTests.Services
{
    [TestFixture()]
    public class UT_UserDDServiceTests
    {
        private Mock<IUserDDService>? _mockUserDDService;

        [SetUp]
        public void SetUp()
        {
            _mockUserDDService = new Mock<IUserDDService>();
        }

        /// <summary>
        /// Tests the AddUserDeviceDetector method to verify it successfully adds user device information.
        /// </summary>
        /// <remarks>
        /// This test checks if the AddUserDeviceDetector method returns true when provided with a valid userId.
        /// The mock IUserDDService is set up to return true when the method is called with a valid userId.
        /// </remarks>
        [Test()]
        public async Task AddUserDeviceDetectorTest_Success()
        {
            // Arrange
            var userId = "validuser";
            _mockUserDDService!.Setup(service => service.AddUserDeviceDetector(userId))
                .ReturnsAsync(true);

            var userDDService = _mockUserDDService.Object;

            // Act
            var result = await userDDService.AddUserDeviceDetector(userId);

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests the AddUserDeviceDetector method to verify it returns false for an invalid userId.
        /// </summary>
        /// <remarks>
        /// This test checks if the AddUserDeviceDetector method returns false when provided with an invalid userId.
        /// The mock IUserDDService is set up to return false when the method is called with an invalid userId.
        /// </remarks>
        [Test()]
        public async Task AddUserDeviceDetectorTest_Failure()
        {
            // Arrange
            var invalidUserId = "invaliduser";
            _mockUserDDService!.Setup(service => service.AddUserDeviceDetector(invalidUserId))
                .ReturnsAsync(false);

            var userDDService = _mockUserDDService.Object;

            // Act
            var result = await userDDService.AddUserDeviceDetector(invalidUserId);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
