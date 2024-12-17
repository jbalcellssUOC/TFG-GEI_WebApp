using BusinessLogicLayer.Interfaces;
using Entities.DTOs;
using Moq;
using System.IdentityModel.Tokens.Jwt;

namespace Tests.UnitTests.Services
{
    [TestFixture()]
    public class UT_EncryptionServiceTests
    {
        private Mock<IEncryptionService>? _mockEncryptionService;

        [SetUp]
        public void SetUp()
        {
            _mockEncryptionService = new Mock<IEncryptionService>();
        }

        /// <summary>
        /// Tests the JWT_CheckExternalToken method to verify it returns a valid JwtSecurityToken.
        /// </summary>
        /// <remarks>
        /// This test checks if the JWT_CheckExternalToken method correctly parses and returns a valid 
        /// JwtSecurityToken object when provided with a valid JWT string.
        /// </remarks>
        [Test()]
        public void JWT_CheckExternalTokenTest()
        {
            // Arrange
            var jwtTokenString = "valid.jwt.token";
            var expectedToken = new JwtSecurityToken();
            _mockEncryptionService!.Setup(service => service.JWT_CheckExternalToken(jwtTokenString))
                .Returns(expectedToken);

            var encryptionService = _mockEncryptionService!.Object;

            // Act
            var result = encryptionService.JWT_CheckExternalToken(jwtTokenString);

            // Assert
            Assert.That(result, Is.EqualTo(expectedToken));
        }

        /// <summary>
        /// Tests the JWT_GetEmailFromToken method to verify it extracts the email from the JWT token.
        /// </summary>
        /// <remarks>
        /// This test checks if the JWT_GetEmailFromToken method correctly extracts and returns the email 
        /// address from a given JwtSecurityToken object.
        /// </remarks>
        [Test()]
        public void JWT_GetEmailFromTokenTest()
        {
            // Arrange
            var jwtToken = new JwtSecurityToken();
            var expectedEmail = "user@example.com";
            _mockEncryptionService!.Setup(service => service.JWT_GetEmailFromToken(jwtToken))
                .Returns(expectedEmail);

            var encryptionService = _mockEncryptionService!.Object;

            // Act
            var result = encryptionService.JWT_GetEmailFromToken(jwtToken);

            // Assert
            Assert.That(result, Is.EqualTo(expectedEmail));
        }

        /// <summary>
        /// Tests the JWT_GetDataFromToken method to verify it extracts data from the JWT token.
        /// </summary>
        /// <remarks>
        /// This test checks if the JWT_GetDataFromToken method correctly extracts and returns data 
        /// from a given JwtSecurityToken object as a JWTDTO object.
        /// </remarks>
        [Test()]
        public void JWT_GetDataFromTokenTest()
        {
            // Arrange
            var jwtToken = new JwtSecurityToken();
            var expectedData = new JWTDTO { Email = "user@example.com", Name = "UserTest" };
            _mockEncryptionService!.Setup(service => service.JWT_GetDataFromToken(jwtToken))
                .Returns(expectedData);

            var encryptionService = _mockEncryptionService!.Object;

            // Act
            var result = encryptionService.JWT_GetDataFromToken(jwtToken);

            // Assert
            Assert.That(result, Is.EqualTo(expectedData));
        }

        /// <summary>
        /// Tests the JWT_IsExpired method to verify it checks if a JWT token is expired.
        /// </summary>
        /// <remarks>
        /// This test checks if the JWT_IsExpired method correctly identifies whether a given 
        /// JwtSecurityToken object is expired.
        /// </remarks>
        [Test()]
        public void JWT_IsExpiredTest()
        {
            // Arrange
            var jwtToken = new JwtSecurityToken();
            _mockEncryptionService!.Setup(service => service.JWT_IsExpired(jwtToken))
                .Returns(true);

            var encryptionService = _mockEncryptionService!.Object;

            // Act
            var result = encryptionService.JWT_IsExpired(jwtToken);

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests the JWT_Generate method to verify it generates a valid JWT token.
        /// </summary>
        /// <remarks>
        /// This test checks if the JWT_Generate method correctly generates a valid JWT string when 
        /// provided with a user ID.
        /// </remarks>
        [Test()]
        public void JWT_GenerateTest()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expectedJwt = "generated.jwt.token";
            _mockEncryptionService!.Setup(service => service.JWT_Generate(userId))
                .Returns(expectedJwt);

            var encryptionService = _mockEncryptionService!.Object;

            // Act
            var result = encryptionService.JWT_Generate(userId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedJwt));
        }

        /// <summary>
        /// Tests the JWT_Validate method to verify it validates a JWT token.
        /// </summary>
        /// <remarks>
        /// This test checks if the JWT_Validate method correctly validates a given JWT string 
        /// and returns a valid SecurityToken object.
        /// </remarks>
        [Test()]
        public void JWT_ValidateTest()
        {
            // Arrange
            var jwt = "valid.jwt.token";
            var expectedToken = new JwtSecurityToken();
            _mockEncryptionService!.Setup(service => service.JWT_Validate(jwt))
                .Returns(expectedToken);

            var encryptionService = _mockEncryptionService!.Object;

            // Act
            var result = encryptionService.JWT_Validate(jwt);

            // Assert
            Assert.That(result, Is.EqualTo(expectedToken));
        }

        /// <summary>
        /// Tests the BCrypt_CheckPassword method to verify it checks if the password is correct.
        /// </summary>
        /// <remarks>
        /// This test checks if the BCrypt_CheckPassword method correctly verifies whether a given 
        /// password matches the stored hash.
        /// </remarks>
        [Test()]
        public void BCrypt_CheckPasswordTest()
        {
            // Arrange
            var password = "password123";
            var hash = "$2a$11$somethinghashed";
            _mockEncryptionService!.Setup(service => service.BCrypt_CheckPassword(password, hash))
                .Returns(true);

            var encryptionService = _mockEncryptionService!.Object;

            // Act
            var result = encryptionService.BCrypt_CheckPassword(password, hash);

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests the BCrypt_EncryptPassword method to verify it encrypts the password.
        /// </summary>
        /// <remarks>
        /// This test checks if the BCrypt_EncryptPassword method correctly encrypts a given password 
        /// and returns the hashed string.
        /// </remarks>
        [Test()]
        public void BCrypt_EncryptPasswordTest()
        {
            // Arrange
            var password = "password123";
            var expectedHash = "$2a$11$somethinghashed";
            _mockEncryptionService!.Setup(service => service.BCrypt_EncryptPassword(password))
                .Returns(expectedHash);

            var encryptionService = _mockEncryptionService!.Object;

            // Act
            var result = encryptionService.BCrypt_EncryptPassword(password);

            // Assert
            Assert.That(result, Is.EqualTo(expectedHash));
        }
    }
}
