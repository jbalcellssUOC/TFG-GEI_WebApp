using Entities.DTOs;
using System.Net;

namespace AuthServiceIntegrationTests
{
    /// <summary>
    /// Integration tests for the Settings functionality.
    /// </summary>
    public class SettingsIntegrationTests : BaseIntegrationTests
    {
        /// <summary>
        /// Test to verify that the IdxSettings GET request returns success.
        /// </summary>
        [Test]
        public async Task IdxSettings_Get_ReturnsSuccess()
        {
            // Act
            var response = await Client!.GetAsync("/Settings/IdxSettings");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
