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
    [Description("UC3 - Update Static Barcode")]

    internal class UC3_UpdateStaticBarcode : BaseTest
    {
        /****************************************************************************/
        /********* UC3 **************************************************************/
        /****************************************************************************/
        //Use Case:         Authenticate User and Update Static Barcode via Codis365 API
        //Use Case Name:    Authenticate User and Update Static Barcode
        //Description:      This use case describes the steps required for a user to authenticate via the Codis365 API and subsequently update a static barcode associated with their account.
        //Actors:
        //  Primary Actor: User (Client Application)
        //  System: Codis365 API
        //Preconditions:
        //  The user has valid login credentials (username and password).
        //  The Codis365 API is accessible and running.
        //Postconditions:
        //  The user is authenticated successfully.
        //  The user updates a static barcode associated with their account.
        //Main Success Scenario:
        //  User Requests Authentication:
        //      The user sends an authentication request to the Codis365 API with their username and password.
        //  System Validates Credentials:
        //      The Codis365 API validates the provided credentials.
        //  System Returns Authentication Token:
        //      Upon successful validation, the Codis365 API returns an authentication token to the user.
        //  User Requests Barcode Update:
        //      The user sends a request to update a static barcode, including the authentication token in the request header.
        //  System Updates Static Barcode:
        //      The Codis365 API updates the static barcode and returns its details to the authenticated user.
        //Extensions (Alternate Flows):
        //  Invalid Credentials:
        //      If the user provides invalid credentials, the Codis365 API returns an authentication error message (401, "Unauthorized").
        //      The user can not update a barcode.
        //  Token Expired or Invalid:
        //      If the authentication token is expired or invalid, the Codis365 API returns an authorization error message (401, "Unauthorized").
        //      The user can not update a barcode.
        //Special Requirements:
        //  Secure handling of user credentials.
        //  Proper error handling and messaging for authentication and authorization failures.

        /// <summary>
        /// Authenticate the user Ok
        /// </summary>
        [Test]
        [Category("UC3 - Update Static Barcode")]
        [Description("UC3 - Positive Test to verify user authentication. Returns 200. Ok")]
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
        [Category("UC3 - Update Static Barcode")]
        [Description("UC3 - Positive Test to verify user authentication error. Returns 401. Unauthorized")]
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
        [Category("UC3 - Update Static Barcode")]
        [Description("UC3 - Positive Test to verify user authentication and token error.")]
        public async Task AuthenticateUser_Ok_TokenErr()
        {
            await AuthenticateUserAsync(APIUsernameTest!, APIUsernameTestPass!);
            Client!.DefaultRequestHeaders.Add("Authorization", $"Bearer fakeToken");
            var response = await Client!.GetAsync($"{Endpoints.Barcodes_endpoint}{Endpoints.Barcodes_s_getall_endpoint}");

            // Assertions
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized), "Response status should be 401. Unauthorized");
        }

        /// <summary>
        /// Update a static barcode Ok
        /// </summary>
        [Test]
        [Category("UC3 - Update Static Barcode")]
        [Description("UC3 - Test to verify if a static barcode can be updated successfully after authenticating the user.")]
        public async Task UpdateStaticBarcode_Ok()
        {
            await AuthenticateUserAsync(APIUsernameTest!, APIUsernameTestPass!);
            Client!.DefaultRequestHeaders.Add("Authorization", $"Bearer {UserToken}");

            // Create a static barcode
            var barcodeDetails = new Dictionary<string, string>
            {
                { "Description", "Add static QR barcode from the API Test" },
                { "CBType", "QR" },
                { "CBValue", "https://www.uoc.edu/barcode-" + Guid.NewGuid().ToString() }
            };
            var content = new MultipartFormDataContent();
            foreach (var keyValuePair in barcodeDetails)
                content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
            var createResponse = await Client!.PostAsync($"{Endpoints.Barcodes_endpoint}{Endpoints.Barcodes_s_add_endpoint}", content);
            var createResponseBody = await createResponse.Content.ReadAsStringAsync();
            var createResult = JsonConvert.DeserializeObject<ApiGenericResponse>(createResponseBody);

            Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Response status should be OK");
            Assert.That(createResult, Is.Not.Null, "Response body should not be null");
            Assert.That(createResult!.Data, Is.Not.Null.And.Not.Empty, "Response body data should contain the new added product guid");

            var barcodeId = createResult!.Data!;

            // Update the created static barcode
            var updateBarcodeDetails = new Dictionary<string, string>
            {
                { "Description", "Updated static QR barcode from the API Test" },
                { "CBType", "QR" },
                { "CBValue", "https://www.uoc.edu/barcode-updated-" + Guid.NewGuid().ToString() }
            };
            var updateContent = new MultipartFormDataContent();
            foreach (var keyValuePair in updateBarcodeDetails)
                updateContent.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
            var updateResponse = await Client!.PutAsync($"{Endpoints.Barcodes_endpoint}{Endpoints.Barcodes_s_update_endpoint}/{barcodeId}", updateContent);
            var updateResponseBody = await updateResponse.Content.ReadAsStringAsync();
            var updateResult = JsonConvert.DeserializeObject<ApiGenericResponse>(updateResponseBody);

            // Assertions
            Assert.Multiple(() =>
            {
                Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Response status should be OK");
                Assert.That(updateResult, Is.Not.Null, "Response body should not be null");
                Assert.That(updateResult, Is.InstanceOf<ApiGenericResponse>(), "Response body should be of type ApiGenericResponse");
                Assert.That(updateResult!.Data, Is.Not.Null.And.Not.Empty, "Response body data should contain the updated product details");
            });
        }
    }
}
