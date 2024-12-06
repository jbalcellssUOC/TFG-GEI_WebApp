using System.Net;

namespace AuthServiceIntegrationTests
{
    /// <summary>
    /// Integration tests for the SupportController.
    /// </summary>
    public class SupportControllerIntegrationTests : BaseIntegrationTests
    {
        /// <summary>
        /// Test to verify that the IdxSupport GET request returns success.
        /// </summary>
        [Test]
        public async Task IdxSupport_Get_ReturnsSuccess()
        {
            // Act
            var response = await Client!.GetAsync("/Support/IdxSupport");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
