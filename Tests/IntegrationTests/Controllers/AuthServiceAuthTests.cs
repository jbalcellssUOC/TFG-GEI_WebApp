using Entities.DTOs;
using System.Net;

namespace AuthServiceIntegrationTests
{
    /// <summary>
    /// Integration tests for the SignInController.
    /// </summary>
    public class SignInControllerTests : BaseIntegrationTests
    {
        /// <summary>
        /// Test to verify that the Login GET request returns success.
        /// </summary>
        [Test]
        public async Task Login_Get_ReturnsSuccess()
        {
            // Act
            var response = await Client!.GetAsync("/SignIn/Login");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        /// <summary>
        /// Test to verify that the Login POST request with valid credentials returns a redirect response.
        /// </summary>
        [Test]
        public async Task Login_Post_WithValidCredentials_ReturnsRedirect()
        {
            // Arrange
            var loginUserDTO = new LoginUserDTO
            {
                Username = "demo@codis365.cat",
                Password = "*Demo2024",
                KeepSigned = true
            };
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Username", loginUserDTO.Username),
                new KeyValuePair<string, string>("Password", loginUserDTO.Password),
                new KeyValuePair<string, string>("KeepSigned", loginUserDTO.KeepSigned.ToString())
            });

            // Act
            var response = await Client!.PostAsync("/SignIn/Login", content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        /// <summary>
        /// Test to verify that the Login POST request with invalid credentials returns the login view with an error message.
        /// </summary>
        [Test]
        public async Task Login_Post_WithInvalidCredentials_ReturnsLoginView()
        {
            // Arrange
            var loginUserDTO = new LoginUserDTO
            {
                Username = "demo@codis365.cat",
                Password = "fakepassword",
            };
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Username", loginUserDTO.Username),
                new KeyValuePair<string, string>("Password", loginUserDTO.Password)
            });

            // Act
            var response = await Client!.PostAsync("/SignIn/Login", content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        /// <summary>
        /// Test to verify that the Logout GET request returns a redirect response.
        /// </summary>
        [Test]
        public async Task Logout_Get_ReturnsRedirect()
        {
            // Arrange
            var loginUserDTO = new LoginUserDTO
            {
                Username = "demo@codis365.cat",
                Password = "*Demo2024",
                KeepSigned = true
            };
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Username", loginUserDTO.Username),
                new KeyValuePair<string, string>("Password", loginUserDTO.Password),
                new KeyValuePair<string, string>("KeepSigned", loginUserDTO.KeepSigned.ToString())
            });

            // Act
            var response1 = await Client!.PostAsync("/SignIn/Login", content);

            // Assert
            Assert.That(response1.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Act
            var response2 = await Client!.GetAsync("/SignIn/Logout");

            // Assert
            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
