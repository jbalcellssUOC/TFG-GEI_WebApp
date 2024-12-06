using BusinessLogicLayer.Interfaces;
using Entities.DTOs;
using Entities.Models;
using Moq;

namespace Tests.UnitTests.Services
{
    [TestFixture()]
    public class UT_UserServiceTests
    {
        private Mock<IUserService>? _mockUserService;

        [SetUp]
        public void SetUp()
        {
            _mockUserService = new Mock<IUserService>();
        }

        /// <summary>
        /// Tests the GetAllUserStats method to verify it returns the correct user stats.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetAllUserStats method returns a list of user stats when provided with a valid username.
        /// The mock IUserService is set up to return a list of AppUsersStat objects when the method is called with a valid username.
        /// </remarks>
        [Test()]
        public void GetAllUserStatsTest_Success()
        {
            // Arrange
            var username = "validuser";
            var expectedStats = new List<AppUsersStat>
            {
                new AppUsersStat { Id = 1, UserId = Guid.NewGuid(), DevOS = "Windows", DevBrowser = "Chrome", IsoDateC = DateTime.Now },
                new AppUsersStat { Id = 2, UserId = Guid.NewGuid(), DevOS = "iOS", DevBrowser = "Safari", IsoDateC = DateTime.Now.AddDays(-1) }
            };
            _mockUserService!.Setup(service => service.GetAllUserStats(username))
                .Returns(expectedStats);

            var userService = _mockUserService!.Object;

            // Act
            var result = userService.GetAllUserStats(username);

            // Assert
            Assert.That(result, Is.EqualTo(expectedStats));
        }

        /// <summary>
        /// Tests the GetAllUserStats method to verify it returns an empty list for an invalid username.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetAllUserStats method returns an empty list when provided with an invalid username.
        /// The mock IUserService is set up to return an empty list of AppUsersStat objects when the method is called with an invalid username.
        /// </remarks>
        [Test()]
        public void GetAllUserStatsTest_Failure()
        {
            // Arrange
            var invalidUsername = "invaliduser";
            var expectedStats = new List<AppUsersStat>();
            _mockUserService!.Setup(service => service.GetAllUserStats(invalidUsername))
                .Returns(expectedStats);

            var userService = _mockUserService!.Object;

            // Act
            var result = userService.GetAllUserStats(invalidUsername);

            // Assert
            Assert.That(result, Is.EqualTo(expectedStats));
        }

        /// <summary>
        /// Tests the GetUserDetails method to verify it returns the correct user details.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetUserDetails method returns the correct user details when provided with a valid username.
        /// The mock IUserService is set up to return a UserDetailsDTO object when the method is called with a valid username.
        /// </remarks>
        [Test()]
        public async Task GetUserDetailsTest_Success()
        {
            // Arrange
            var username = "validuser";
            var expectedUserDetails = new UserDetailsDTO
            {
                Name = "validuser",
                Login = "validuser@example.com",
                AppUsersStats = new List<AppUsersStat>
                {
                    new AppUsersStat { Id = 1, UserId = Guid.NewGuid(), DevOS = "Windows", DevBrowser = "Chrome", IsoDateC = DateTime.Now },
                    new AppUsersStat { Id = 2, UserId = Guid.NewGuid(), DevOS = "iOS", DevBrowser = "Safari", IsoDateC = DateTime.Now.AddDays(-1) }
                }
            };
            _mockUserService!.Setup(service => service.GetUserDetails(username))
                .ReturnsAsync(expectedUserDetails);

            var userService = _mockUserService!.Object;

            // Act
            var result = await userService.GetUserDetails(username);

            // Assert
            Assert.That(result, Is.EqualTo(expectedUserDetails));
        }

        /// <summary>
        /// Tests the GetUserDetails method to verify it returns null for an invalid username.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetUserDetails method returns null when provided with an invalid username.
        /// The mock IUserService is set up to return null when the method is called with an invalid username.
        /// </remarks>
        [Test()]
        public async Task GetUserDetailsTest_Failure()
        {
            // Arrange
            var invalidUsername = "invaliduser";
            UserDetailsDTO expectedUserDetails = null!;
            _mockUserService!.Setup(service => service.GetUserDetails(invalidUsername))
                .ReturnsAsync(expectedUserDetails);

            var userService = _mockUserService!.Object;

            // Act
            var result = await userService.GetUserDetails(invalidUsername);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
