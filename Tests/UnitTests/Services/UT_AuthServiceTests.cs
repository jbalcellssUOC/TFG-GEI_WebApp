using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Interfaces;
using Entities.DTOs;
using Entities.Models;
using Moq;

namespace Tests.UnitTests.Services
{
    /// <summary>
    /// UT for AuthService
    /// </summary>
    [TestFixture()]
    public class UT_AuthServiceTests
    {
        /// <summary>
        /// Tests the CheckUserAuth method with invalid credentials.
        /// </summary>
        /// <remarks>
        /// This test verifies that the CheckUserAuth method returns false when provided with an invalid 
        /// password. It sets up the mock IUserRepository to return a specific user and configures 
        /// the IEncryptionService mock to return false when the incorrect password is checked against 
        /// the stored hash.
        /// </remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        
        [Test]
        public async Task CheckUserAuthTest_KO()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.GetUserByEmail("test@codis365.cat"))
                .Returns(new AppUser { Login = "test@codis365.cat", Password = "$2a$11$CSNAnu2ZWlYqnHstR5SWA.snlXhwTpsmUWk/EopLvfPsDsxL/Cg0G" });

            var mockEncryptionService = new Mock<IEncryptionService>();
            mockEncryptionService.Setup(enc => enc.BCrypt_CheckPassword("fakepassword", "$2a$11$CSNAnu2ZWlYqnHstR5SWA.snlXhwTpsmUWk/EopLvfPsDsxL/Cg0G")).Returns(false);

            var mockUserDDService = new Mock<IUserDDService>();
            var mockNotificationService = new Mock<INotificationService>();
            var mockHelperService = new Mock<IHelpersService>();

            var service = new AuthService(mockUserRepository.Object, mockEncryptionService.Object, mockUserDDService.Object, mockNotificationService.Object, mockHelperService.Object);
            var result = await service.CheckUserAuth(new LoginUserDTO() { Username = "test@codis365.cat", Password = "fakepassword" });

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests the CheckUserAuth method with valid credentials.
        /// </summary>
        /// <remarks>
        /// This test verifies that the CheckUserAuth method returns true when provided with valid 
        /// username and password. It sets up the mock IUserRepository to return a specific user 
        /// and configures the IEncryptionService mock to return true when the password is checked 
        /// against the stored hash.
        /// </remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        
        [Test()]
		public async Task CheckUserAuthTest_OK()
		{
			var mockUserRepository = new Mock<IUserRepository>();
			mockUserRepository.Setup(repo => repo.GetUserByEmail("test@codis365.cat"))
				.Returns(new AppUser { Login = "test@codis365.cat", Password = "$2a$11$CSNAnu2ZWlYqnHstR5SWA.snlXhwTpsmUWk/EopLvfPsDsxL/Cg0G" });

            var mockEncryptionService = new Mock<IEncryptionService>();
            mockEncryptionService.Setup(enc => enc.BCrypt_CheckPassword("*Codis365demo2024", "$2a$11$CSNAnu2ZWlYqnHstR5SWA.snlXhwTpsmUWk/EopLvfPsDsxL/Cg0G")).Returns(true);

            var mockAuthService = new Mock<IAuthService>();
            var mockNotificationService = new Mock<INotificationService>();
            var mockHelperService = new Mock<IHelpersService>();
            var mockUserDDService = new Mock<IUserDDService>();

            mockAuthService.Setup(service => service.CheckUserAuth(It.IsAny<LoginUserDTO>())).Returns(Task.FromResult(true));
            var service = new AuthService(mockUserRepository.Object, mockEncryptionService.Object, mockUserDDService.Object, mockNotificationService.Object, mockHelperService.Object);
            var result = await service.CheckUserAuth(new LoginUserDTO() { Username = "test@codis365.cat", Password = "*Codis365demo2024" });

			Assert.That(result, Is.True);
		}
    }
}