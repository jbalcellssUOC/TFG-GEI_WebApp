using System.Net;

namespace AuthServiceIntegrationTests
{
    /// <summary>
    /// Integration tests for the BarcodesController.
    /// </summary>
    public class BarcodesControllerIntegrationTests : BaseIntegrationTests
    {
        /// <summary>
        /// Test to verify that the StaticBarcodes GET request returns success.
        /// </summary>
        [Test]
        public async Task StaticBarcodes_Get_ReturnsSuccess()
        {
            // Act
            var response = await Client!.GetAsync("/Barcodes/StaticBarcodes");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        /// <summary>
        /// Test to verify that the DynamicBarcodes GET request returns success.
        /// </summary>
        [Test]
        public async Task DynamicBarcodes_Get_ReturnsSuccess()
        {
            // Act
            var response = await Client!.GetAsync("/Barcodes/DynamicBarcodes");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        /// <summary>
        /// Test to verify that the Management GET request returns success.
        /// </summary>
        [Test]
        public async Task Management_Get_ReturnsSuccess()
        {
            // Act
            var response = await Client!.GetAsync("/Barcodes/Management");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
