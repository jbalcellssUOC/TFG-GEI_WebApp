using BusinessLogicLayer.Interfaces;
using Moq;

namespace Tests.UnitTests.Services
{
    [TestFixture()]
    public class UT_ClaimsServiceTests
    {
        private Mock<IClaimsService>? _mockClaimsService;

        [SetUp]
        public void SetUp()
        {
            _mockClaimsService = new Mock<IClaimsService>();
        }

        /// <summary>
        /// Tests the GetClaimValue method to verify it returns the correct claim value.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetClaimValue method returns the correct value for a given 
        /// claim type. The mock IClaimsService is set up to return a specific claim value 
        /// when the method is called with a particular claim type.
        /// </remarks>
        [Test()]
        public void GetClaimValueTest_Success()
        {
            // Arrange
            var claimType = "email";
            var expectedClaimValue = "user@example.com";
            _mockClaimsService!.Setup(service => service.GetClaimValue(claimType))
                .Returns(expectedClaimValue);

            var claimsService = _mockClaimsService.Object;

            // Act
            var result = claimsService.GetClaimValue(claimType);

            // Assert
            Assert.That(result, Is.EqualTo(expectedClaimValue));
        }

        /// <summary>
        /// Tests the GetClaimValue method to verify it returns null for an invalid claim type.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetClaimValue method returns null when provided with an 
        /// invalid claim type. The mock IClaimsService is set up to return null when the 
        /// method is called with an invalid claim type.
        /// </remarks>
        [Test()]
        public void GetClaimValueTest_Failure()
        {
            // Arrange
            var invalidClaimType = "invalid_claim";
            _mockClaimsService!.Setup(service => service.GetClaimValue(invalidClaimType))
                .Returns((string)null!);

            var claimsService = _mockClaimsService.Object;

            // Act
            var result = claimsService.GetClaimValue(invalidClaimType);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
