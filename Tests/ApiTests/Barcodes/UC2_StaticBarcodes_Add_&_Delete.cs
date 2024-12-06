using ApiTests;
using Newtonsoft.Json;
using System.Net;

namespace Tests.ApiTests.Barcodes
{
    /// <summary>
    /// Use cases API Tests for Barcodes
    /// </summary>
    [TestFixture]
    [Category("API Tests")]
    [Description("UC2 - Authenticate User and Create Static Barcode")]

    internal class UC2_CreateStaticBarcode : BaseTest
    {
        /****************************************************************************/
        /********* UC2 **************************************************************/
        /****************************************************************************/
        //Use Case:         Authenticate User and Create Static Barcode via Codis365 API
        //Use Case Name:    Authenticate User and Create Static Barcode
        //Description:      This use case describes the steps required for a user to authenticate via the Codis365 API and subsequently create a static barcode associated with their account.
        //Actors:
        //  Primary Actor: User (Client Application)
        //  System: Codis365 API
        //Preconditions:
        //  The user has valid login credentials (username and password).
        //  The Codis365 API is accessible and running.
        //Postconditions:
        //  The user is authenticated successfully.
        //  The user creates a new static barcode associated with their account.
        //Main Success Scenario:
        //  User Requests Authentication:
        //      The user sends an authentication request to the Codis365 API with their username and password.
        //  System Validates Credentials:
        //      The Codis365 API validates the provided credentials.
        //  System Returns Authentication Token:
        //      Upon successful validation, the Codis365 API returns an authentication token to the user.
        //  User Requests Barcode Creation:
        //      The user sends a request to create a static barcode, including the authentication token in the request header.
        //  System Creates Static Barcode:
        //      The Codis365 API creates the static barcode and returns its details to the authenticated user.
        //Extensions (Alternate Flows):
        //  Invalid Credentials:
        //      If the user provides invalid credentials, the Codis365 API returns an authentication error message (401, "Unauthorized").
        //      The user can not create a barcode.
        //  Token Expired or Invalid:
        //      If the authentication token is expired or invalid, the Codis365 API returns an authorization error message (401, "Unauthorized").
        //      The user can not create a barcode.
        //Special Requirements:
        //  Secure handling of user credentials.
        //  Proper error handling and messaging for authentication and authorization failures.

        /// <summary>
        /// Authenticate the user Ok
        /// </summary>
        [Test]
        [Category("UC2 - Create Static Barcode")]
        [Description("UC2 - Positive Test to verify user authentication. Returns 200. Ok")]
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
        [Category("UC2 - Create Static Barcode")]
        [Description("UC2 - Positive Test to verify user authentication error. Returns 401. Unauthorized")]
        public async Task AuthenticateUser_Ko()
        {
            var loginDetails = new Dictionary<string, string>
            {
                { "username", "sdfsdfsdfd" },
                { "password", "*sdfsdfsdf" }
            };
            var content = new MultipartFormDataContent();
            foreach (var keyValuePair in loginDetails)
                content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
            var response = await Client!.PostAsync(Endpoints.Auth_endpoint, content);
            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(responseBody)!;

            // Assertions
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized), "Response status should be Ko with 401");
        }

        /// <summary>
        /// Authenticate the user Ok and then try to create a static barcode with a fake token
        /// </summary>
        [Test]
        [Category("UC2 - Create Static Barcode")]
        [Description("UC2 - Positive Test to verify user authentication and token error.")]
        public async Task AuthenticateUser_Ok_TokenErr()
        {
            await AuthenticateUserAsync(APIUsernameTest!, APIUsernameTestPass!);
            Client!.DefaultRequestHeaders.Add("Authorization", $"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImRlbW9hcGlAcXJmeS5lcyIsIm5iZiI6MTcxNzg5MDE2NSwiZXgwIjoxNzE4NDkwMTA1LCJpYXQiOjE3MTc4OTAxNjV9.FPAlaN0GZlnGk0CDhIzSWlsUBMRw8MR-L7KK0rQtu-2");
            var response = await Client!.GetAsync($"{Endpoints.Barcodes_endpoint}{Endpoints.Barcodes_s_getall_endpoint}?pageIndex=1&pageSize=5");

            // Assertions
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized), "Response status should be 401. Unauthorized");
        }

        /// <summary>
        /// Create a static barcode Ok
        /// </summary>
        [Test]
        [Category("UC2 - Create Static Barcode")]
        [Description("UC2 - Test to verify if a static barcode can be created successfully after authenticating the user.")]
        public async Task AddStaticBarcode_Ok()
        {
            await AuthenticateUserAsync(APIUsernameTest!, APIUsernameTestPass!);
            Client!.DefaultRequestHeaders.Add("Authorization", $"Bearer {UserToken}");

            var barcodeDetails = new Dictionary<string, string>
            {
                { "Description", "Add static QR barcode from the API Test - " + DateTime.Now.ToString() },
                { "CBType", "QR" },
                { "CBValue", "https://www.uoc.edu/barcode-" + Guid.NewGuid().ToString() }
            };
            var content = new MultipartFormDataContent();
            foreach (var keyValuePair in barcodeDetails)
                content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
            var response = await Client!.PostAsync($"{Endpoints.Barcodes_endpoint}{Endpoints.Barcodes_s_add_endpoint}", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiGenericResponse>(responseBody);

            // Assertions
            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Response status should be OK");
                Assert.That(result, Is.Not.Null, "Response body should not be null");
                Assert.That(result, Is.InstanceOf<ApiGenericResponse>(), "Response body should be of type ApiGenericResponse");
                Assert.That(result!.Data, Is.Not.Null.And.Not.Empty, "Response body data should contain the new added product guid: ");
            });

            createdStaticBarcodes.Add(result!.Data!);  // Save the created barcode ID for later cleanup
        }

        /// <summary>
        /// Create a static barcode Ok
        /// </summary>
        [Test]
        [Category("UC2 - Create Static Barcode")]
        [Description("UC2 - Test to verify if a static barcode can be created successfully after authenticating the user.")]
        public async Task AddStaticBarcode_Ko()
        {
            await AuthenticateUserAsync(APIUsernameTest!, APIUsernameTestPass!);
            Client!.DefaultRequestHeaders.Add("Authorization", $"Bearer {UserToken}");

            var barcodeDetails = new Dictionary<string, string>
            {
                { "Description", "Add static QR barcode from the API Test" },
            };
            var content = new MultipartFormDataContent();
            foreach (var keyValuePair in barcodeDetails)
                content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
            var response = await Client!.PostAsync($"{Endpoints.Barcodes_endpoint}{Endpoints.Barcodes_s_add_endpoint}", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiGenericResponse>(responseBody);

            // Assertions
            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), "Response status should be BadRequest");
            });
        }

    }
}
