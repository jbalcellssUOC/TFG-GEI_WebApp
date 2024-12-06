using ApiTests;
using Entities.POCO;
using Newtonsoft.Json;
using System.Net;

namespace Tests.ApiTests.Products
{
    /// <summary>
    /// Use cases API Tests for Products
    /// </summary>
    [TestFixture]
    [Category("API Tests")]
    [Description("UC7 - Authenticate User and Retrieve All Products")]

    internal class UC7_GetAllProducts : BaseTest
    {
        /****************************************************************************/
        /********* UC7 **************************************************************/
        /****************************************************************************/
        //Use Case:         Authenticate User and Retrieve All Products via Codis365 API
        //Use Case Name:    Authenticate User and Retrieve All Products
        //Description:      This use case describes the steps required for a user to authenticate via the Codis365 API and subsequently retrieve all products associated with their account.
        //Actors:
        //  Primary Actor: User (Client Application)
        //  System: Codis365 API
        //Preconditions:
        //  The user has valid login credentials (username and password).
        //  The Codis365 API is accessible and running.
        //  The user has an account with the Codis365 service that includes at least one product.
        //Postconditions:
        //  The user is authenticated successfully.
        //  The user receives a list of all products associated with their account.
        //Main Success Scenario:
        //  User Requests Authentication:
        //      The user sends an authentication request to the Codis365 API with their username and password.
        //  System Validates Credentials:
        //      The Codis365 API validates the provided credentials.
        //  System Returns Authentication Token:
        //      Upon successful validation, the Codis365 API returns an authentication token to the user.
        //  User Requests Products:
        //      The user sends a request to retrieve all products, including the authentication token in the request header.
        //  System Returns Products:
        //      The Codis365 API returns a list of all products associated with the authenticated user's account.
        //Extensions (Alternate Flows):
        //  Invalid Credentials:
        //      If the user provides invalid credentials, the Codis365 API returns an authentication error message (401, "Unauthorized").
        //      The user cannot retrieve data from the Codis365 API.
        //  Token Expired or Invalid:
        //      If the authentication token is expired or invalid, the Codis365 API returns an authorization error message (401, "Unauthorized").
        //      The user cannot retrieve data from the Codis365 API.
        //Special Requirements:
        //  Secure handling of user credentials.
        //  Proper error handling and messaging for authentication and authorization failures.

        /// <summary>
        /// Authenticate the user Ok
        /// </summary>
        [Test]
        [Category("UC7 - Products")]
        [Description("UC7 - Positive Test to verify user authentication. Returns 200. Ok")]
        public async Task AuthenticateUser_Ok()
        {
            var loginDetails = new Dictionary<string, string>
            {
                { "username", APIUsernameTest! },
                { "password", APIUsernameTestPass! }
            };
            var content = new MultipartFormDataContent();
            foreach (var keyValuePair in loginDetails)
                content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
            var response = await Client!.PostAsync(Endpoints.Auth_endpoint, content);
            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(responseBody)!;

            // Assertions
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Response status should be OK");
            Assert.That(result.token, Is.Not.Null, "Authentication token should not be null");
        }

        /// <summary>
        /// Authenticate the user with error (Ko)
        /// </summary>
        [Test]
        [Category("UC7 - Products")]
        [Description("UC7 - Positive Test to verify user authentication error. Returns 401. Unauthorized")]
        public async Task AuthenticateUser_Ko()
        {
            var loginDetails = new Dictionary<string, string>
            {
                { "username", "invalidUsername" },
                { "password", "invalidPassword" }
            };
            var content = new MultipartFormDataContent();
            foreach (var keyValuePair in loginDetails)
                content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
            var response = await Client!.PostAsync(Endpoints.Auth_endpoint, content);

            // Assertions
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized), "Response status should be Ko with 401");
        }

        /// <summary>
        /// Authenticate the user Ok and then try to access the endpoint with a fake token
        /// </summary>
        [Test]
        [Category("UC7 - Products")]
        [Description("UC7 - Positive Test to verify user authentication and token error.")]
        public async Task AuthenticateUser_Ok_TokenErr()
        {
            await AuthenticateUserAsync(APIUsernameTest!, APIUsernameTestPass!);
            Client!.DefaultRequestHeaders.Add("Authorization", $"Bearer fakeToken");
            var response = await Client!.GetAsync($"{Endpoints.Products_endpoint}{Endpoints.Products_getall_endpoint}?pageIndex=1&pageSize=5");

            // Assertions
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized), "Response status should be 401. Unauthorized");
        }

        /// <summary>
        /// Get all products Ok
        /// </summary>
        [Test]
        [Category("UC7 - Products")]
        [Description("UC7 - Test to verify if all products can be retrieved successfully after authenticating the user.")]
        public async Task GetAllProducts_Ok()
        {
            await AuthenticateUserAsync(APIUsernameTest!, APIUsernameTestPass!);
            Client!.DefaultRequestHeaders.Add("Authorization", $"Bearer {UserToken}");
            var pageIndex = 1;
            var pageSize = 5;
            var response = await Client!.GetAsync($"{Endpoints.Products_endpoint}{Endpoints.Products_getall_endpoint}?pageIndex={pageIndex}&pageSize={pageSize}");
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiResponseProducts>(responseBody);

            // Assertions
            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Response status should be OK");
                Assert.That(result, Is.Not.Null, "Response body should not be null");
            });
            Assert.That(result, Is.InstanceOf<ApiResponseProducts>(), "Response body should be of type ApiResponseProducts");
            Assert.That(result!.Success, Is.True, "Response.Success has to be true.");
        }

        /// <summary>
        /// Get all products Ko
        /// </summary>
        [Test]
        [Category("UC7 - Products")]
        [Description("UC7 - Test to verify if fails retrieving products with incorrect pageindex or pagesize.")]
        public async Task GetAllProducts_Ko()
        {
            await AuthenticateUserAsync(APIUsernameTest!, APIUsernameTestPass!);
            Client!.DefaultRequestHeaders.Add("Authorization", $"Bearer {UserToken}");
            var pageIndex = 9999;
            var pageSize = 9999;
            var response = await Client!.GetAsync($"{Endpoints.Products_endpoint}{Endpoints.Products_getall_endpoint}?pageIndex={pageIndex}&pageSize={pageSize}");
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiResponseProducts>(responseBody);

            // Assertions
            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Response status should be OK");
                Assert.That(result, Is.Not.Null, "Response body should not be null");
                Assert.That(result, Is.InstanceOf<ApiResponseProducts>(), "Response body should be of type ApiResponseProducts");
                Assert.That(result!.Data.Items, Is.Empty, "Response.Data.Items has to be 0.");
            });
        }
    }
}
