using System.Net;

namespace AuthServiceIntegrationTests
{
    /// <summary>
    /// Integration tests for the Support Chat functionality.
    /// </summary>
    public class SupportChatIntegrationTests : BaseIntegrationTests
    {
        /// <summary>
        /// Test to verify that the GetAllUserChatMessages GET request returns success.
        /// </summary>
        [Test]
        public async Task GetAllUserChatMessages_Get_ReturnsSuccess()
        {
            // Act
            var response = await Client!.GetAsync("/SupportChat/GetAllUserChatMessages");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
