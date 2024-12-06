using ApiTests;
using Newtonsoft.Json;
using System.Net;

namespace Tests.ApiTests.Products
{
    /// <summary>
    /// Use cases API Tests for Products
    /// </summary>
    [TestFixture]
    [Category("API Tests")]
    [Description("UC9 - Authenticate User and Update Product")]

    internal class UC9_UpdateProduct : BaseTest
    {
        private readonly ApiEndpoints ApiEndPoints = new ApiEndpoints();

        /****************************************************************************/
        /********* UC9 **************************************************************/
        /****************************************************************************/
        //Use Case:         Authenticate User and Update Product via Codis365 API
        //Use Case Name:    Authenticate User and Update Product
        //Description:      This use case describes the steps required for a user to authenticate via the Codis365 API and subsequently update a product associated with their account.
        //Actors:
        //  Primary Actor: User (Client Application)
        //  System: Codis365 API
        //Preconditions:
        //  The user has valid login credentials (username and password).
        //  The Codis365 API is accessible and running.
        //Postconditions:
        //  The user is authenticated successfully.
        //  The user updates a product associated with their account.
        //Main Success Scenario:
        //  User Requests Authentication:
        //      The user sends an authentication request to the Codis365 API with their username and password.
        //  System Validates Credentials:
        //      The Codis365 API validates the provided credentials.
        //  System Returns Authentication Token:
        //      Upon successful validation, the Codis365 API returns an authentication token to the user.
        //  User Requests Product Update:
        //      The user sends a request to update a product, including the authentication token in the request header.
        //  System Updates Product:
        //      The Codis365 API updates the product and returns its details to the authenticated user.
        //Extensions (Alternate Flows):
        //  Invalid Credentials:
        //      If the user provides invalid credentials, the Codis365 API returns an authentication error message (401, "Unauthorized").
        //      The user can not update a product.
        //  Token Expired or Invalid:
        //      If the authentication token is expired or invalid, the Codis365 API returns an authorization error message (401, "Unauthorized").
        //      The user can not update a product.
        //Special Requirements:
        //  Secure handling of user credentials.
        //  Proper error handling and messaging for authentication and authorization failures.

        /// <summary>
        /// Authenticate the user Ok
        /// </summary>
        [Test]
        [Category("UC9 - Products")]
        [Description("UC9 - Positive Test to verify user authentication. Returns 200. Ok")]
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
            var response = await Client!.PostAsync(ApiEndPoints.Auth_endpoint, content);
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
        [Category("UC9 - Products")]
        [Description("UC9 - Positive Test to verify user authentication error. Returns 401. Unauthorized")]
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
            var response = await Client!.PostAsync(ApiEndPoints.Auth_endpoint, content);
            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(responseBody)!;

            // Assertions
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized), "Response status should be Ko with 401");
        }

        /// <summary>
        /// Authenticate the user Ok and then try to update a product with a fake token
        /// </summary>
        [Test]
        [Category("UC9 - Products")]
        [Description("UC9 - Positive Test to verify user authentication and token error.")]
        public async Task AuthenticateUser_Ok_TokenErr()
        {
            await AuthenticateUserAsync(APIUsernameTest!, APIUsernameTestPass!);
            Client!.DefaultRequestHeaders.Add("Authorization", $"Bearer fakeToken");
            var response = await Client!.GetAsync($"{Endpoints.Products_endpoint}{Endpoints.Products_getall_endpoint}?pageIndex=1&pageSize=5");

            // Assertions
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized), "Response status should be 401. Unauthorized");
        }

        static int CalculateEAN13CheckDigit(string ean)
        {
            int sum = 0;
            for (int i = 0; i < ean.Length; i++)
            {
                int digit = int.Parse(ean[i].ToString());
                if (i % 2 == 0) // even index (0-based)
                {
                    sum += digit;
                }
                else // odd index
                {
                    sum += digit * 3;
                }
            }
            int remainder = sum % 10;
            return (10 - remainder) % 10;  // returns the check digit
        }

        /// <summary>
        /// Update a product Ok
        /// </summary>
        [Test]
        [Category("UC9 - Products")]
        [Description("UC9 - Test to verify if a product can be updated successfully after authenticating the user.")]
        public async Task UpdateProduct_Ok()
        {
            await AuthenticateUserAsync(APIUsernameTest!, APIUsernameTestPass!);
            Client!.DefaultRequestHeaders.Add("Authorization", $"Bearer {UserToken}");

            // Create a product

            // Generate a random 4-digit value
            Random random = new Random();
            int randomNum = random.Next(1000, 10000);  // Generates a number between 1000 and 9999
            string randomValue = randomNum.ToString();
            // Generate a random EAN-13 number beginning with 84
            string randomEAN = "84" + random.Next(100000000, 1000000000).ToString();  // Generates 9 random digits
            // Calculate the EAN-13 check digit
            randomEAN += CalculateEAN13CheckDigit(randomEAN);

            var productDetails = new Dictionary<string, string>
            {
                { "Reference", $"REFAPI-{randomNum}" },
                { "Category", "Fashion" },
                { "Description", "Add product from the API Test " + DateTime.Now.ToString() },
                { "Price", "10,99" },
                { "CBType", "EAN13" },
                { "CBValue", $"{randomEAN}" }
            };
            var content = new MultipartFormDataContent();
            foreach (var keyValuePair in productDetails)
                content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
            var createResponse = await Client!.PostAsync($"{ApiEndPoints.Products_endpoint}{ApiEndPoints.Products_add_endpoint}", content);
            var createResponseBody = await createResponse.Content.ReadAsStringAsync();
            var createResult = JsonConvert.DeserializeObject<ApiGenericResponse>(createResponseBody);

            Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Response status should be OK");
            Assert.That(createResult, Is.Not.Null, "Response body should not be null");
            Assert.That(createResult!.Data, Is.Not.Null.And.Not.Empty, "Response body data should contain the new added product ID");

            var productId = createResult!.Data!;

            // Update the created product
            var updateProductDetails = new Dictionary<string, string>
            {
                { "Description", "*** UPDATED PRODUCT FROM API TEST ***" },
                { "Price", "25.50" },
                { "Category", "Updated Test Category" }
            };
            var updateContent = new MultipartFormDataContent();
            foreach (var keyValuePair in updateProductDetails)
                updateContent.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
            var updateResponse = await Client!.PutAsync($"{ApiEndPoints.Products_endpoint}{ApiEndPoints.Products_update_endpoint}/{productId}", updateContent);
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
