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
    [Description("UC8 - Authenticate User and Create Product")]

    internal class UC8_CreateProduct : BaseTest
    {
        /****************************************************************************/
        /********* UC8 **************************************************************/
        /****************************************************************************/
        //Use Case:         Authenticate User and Create Product via Codis365 API
        //Use Case Name:    Authenticate User and Create Product
        //Description:      This use case describes the steps required for a user to authenticate via the Codis365 API and subsequently create a product associated with their account.
        //Actors:
        //  Primary Actor: User (Client Application)
        //  System: Codis365 API
        //Preconditions:
        //  The user has valid login credentials (username and password).
        //  The Codis365 API is accessible and running.
        //Postconditions:
        //  The user is authenticated successfully.
        //  The user creates a new product associated with their account.
        //Main Success Scenario:
        //  User Requests Authentication:
        //      The user sends an authentication request to the Codis365 API with their username and password.
        //  System Validates Credentials:
        //      The Codis365 API validates the provided credentials.
        //  System Returns Authentication Token:
        //      Upon successful validation, the Codis365 API returns an authentication token to the user.
        //  User Requests Product Creation:
        //      The user sends a request to create a product, including the authentication token in the request header.
        //  System Creates Product:
        //      The Codis365 API creates the product and returns its details to the authenticated user.
        //Extensions (Alternate Flows):
        //  Invalid Credentials:
        //      If the user provides invalid credentials, the Codis365 API returns an authentication error message (401, "Unauthorized").
        //      The user can not create a product.
        //  Token Expired or Invalid:
        //      If the authentication token is expired or invalid, the Codis365 API returns an authorization error message (401, "Unauthorized").
        //      The user can not create a product.
        //Special Requirements:
        //  Secure handling of user credentials.
        //  Proper error handling and messaging for authentication and authorization failures.

        /// <summary>
        /// Authenticate the user Ok
        /// </summary>
        [Test]
        [Category("UC8 - Products")]
        [Description("UC8 - Positive Test to verify user authentication. Returns 200. Ok")]
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
        [Category("UC8 - Products")]
        [Description("UC8 - Positive Test to verify user authentication error. Returns 401. Unauthorized")]
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
            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(responseBody)!;

            // Assertions
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized), "Response status should be Ko with 401");
        }

        /// <summary>
        /// Authenticate the user Ok and then try to create a product with a fake token
        /// </summary>
        [Test]
        [Category("UC8 - Products")]
        [Description("UC8 - Positive Test to verify user authentication and token error.")]
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
        /// Create a product Ok
        /// </summary>
        [Test]
        [Category("UC8 - Products")]
        [Description("UC8 - Test to verify if a product can be created successfully after authenticating the user.")]
        public async Task AddProduct_Ok()
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
            var response = await Client!.PostAsync($"{Endpoints.Products_endpoint}{Endpoints.Products_add_endpoint}", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiGenericResponse>(responseBody);

            // Assertions
            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Response status should be OK");
                Assert.That(result, Is.Not.Null, "Response body should not be null");
                Assert.That(result, Is.InstanceOf<ApiGenericResponse>(), "Response body should be of type ApiGenericResponse");
                Assert.That(result!.Data, Is.Not.Null.And.Not.Empty, "Response body data should contain the new added product ID: ");
            });

            createdProducts.Add(result!.Data!);  // Save the created product ID for later cleanup
        }

        /// <summary>
        /// Create a product with missing fields Ko
        /// </summary>
        [Test]
        [Category("UC8 - Products")]
        [Description("UC8 - Test to verify if fails creating a product with missing fields.")]
        public async Task AddProduct_Ko()
        {
            await AuthenticateUserAsync(APIUsernameTest!, APIUsernameTestPass!);
            Client!.DefaultRequestHeaders.Add("Authorization", $"Bearer {UserToken}");

            var productDetails = new Dictionary<string, string>
            {
                { "Name", "Product from API Test" }
            };
            var content = new MultipartFormDataContent();
            foreach (var keyValuePair in productDetails)
                content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
            var response = await Client!.PostAsync($"{Endpoints.Products_endpoint}{Endpoints.Products_add_endpoint}", content);
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
