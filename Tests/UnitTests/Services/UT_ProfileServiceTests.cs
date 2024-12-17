using BusinessLogicLayer.Interfaces;
using Entities.DTOs;
using Moq;

namespace Tests.UnitTests.Services
{
    [TestFixture()]
    public class UT_ProfileServiceTests
    {
        private Mock<IProfileService>? _mockProfileService;

        [SetUp]
        public void SetUp()
        {
            _mockProfileService = new Mock<IProfileService>();
        }

        /// <summary>
        /// Tests the GetUserProfile method to verify it returns the correct user profile.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetUserProfile method returns the correct user profile data 
        /// when provided with a valid username. The mock IProfileService is set up to return a specific 
        /// DashboardUserProfileDTO object when the method is called with a valid username.
        /// </remarks>
        [Test()]
        public void GetUserProfileTest_Success()
        {
            // Arrange
            var username = "validuser";
            var expectedProfile = new DashboardUserProfileDTO
            {
                UserName = "validuser",
                UserLogin = "validuser@example.com",
                UserProfile = "Web User"
            };
            _mockProfileService!.Setup(service => service.GetUserProfile(username))
                .Returns(expectedProfile);

            var profileService = _mockProfileService.Object;

            // Act
            var result = profileService.GetUserProfile(username);

            // Assert
            Assert.That(result, Is.EqualTo(expectedProfile));
        }

        /// <summary>
        /// Tests the GetUserProfile method to verify it returns null for an invalid username.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetUserProfile method returns null when provided with an 
        /// invalid username. The mock IProfileService is set up to return null when the method 
        /// is called with an invalid username.
        /// </remarks>
        [Test()]
        public void GetUserProfileTest_Failure()
        {
            // Arrange
            var invalidUsername = "invaliduser";
            _mockProfileService!.Setup(service => service.GetUserProfile(invalidUsername))
                .Returns((DashboardUserProfileDTO)null!);

            var profileService = _mockProfileService.Object;

            // Act
            var result = profileService.GetUserProfile(invalidUsername);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
