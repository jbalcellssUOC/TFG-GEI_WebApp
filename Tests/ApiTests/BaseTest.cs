using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Net;
using Tests.ApiTests;

namespace ApiTests
{
    /// <summary>
    /// Base class for all API tests
    /// </summary>
    public class BaseTest
    {
        private IHost? APIHost;                      // TestServer for the API
        protected IConfiguration? Configuration;     // Configuration for the test
        protected HttpClient? Client;                // HttpClient for API requests

        protected string? UserToken;                // User token for authentication
        protected string? APIUsernameTest;          // API username for testing    
        protected string? APIUsernameTestPass;      // API username password for testing

        protected ApiEndpoints Endpoints = new();

        public static readonly List<string> createdStaticBarcodes = [];     // List to store created statics barcode GUIDs
        public static readonly List<string> createdDynamicBarcodes = [];    // List to store created dynamics barcode GUIDs
        public static readonly List<string> createdProducts = [];           // List to store created products GUIDs

        /// <summary>
        /// Setup the test
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Load the appsettings.json file
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets<ExternalAPI.Startup>()
                .Build();

            // Setup TestServer with the WebApplicationFactory
            APIHost = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .UseStartup<ExternalAPI.Startup>()
                        .UseEnvironment("Development")
                        .UseConfiguration(Configuration);
                })
            .Start();
    
            Client = APIHost.GetTestServer().CreateClient();                // Create a new HttpClient
            Client!.BaseAddress = new Uri("https://localhost:7155");         // Set the base address

            APIUsernameTest = Configuration["APIUsernameTest"]!;            // Get the API username from Secrets
            APIUsernameTestPass = Configuration["APIUsernameTestPass"]!;    // Get the API password from Secrets
        }

        /// <summary>
        /// One time tear down
        /// </summary>
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Task.Run(async () => await Cleanup()).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Tear down the test
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            Client?.Dispose();  // Dispose the HttpClient
            APIHost?.Dispose(); // Dispose the TestServer
        }

        /// <summary>
        /// Dispose the test
        /// </summary>
        void Dispose()
        {
            Client?.Dispose();  // Dispose the HttpClient
            APIHost?.Dispose(); // Dispose the TestServer
        }

        /// <summary>
        /// Shared method by all tests tha needs to authenticate a Codis365 user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>

        protected async Task AuthenticateUserAsync(string username, string password)
        {
            var loginDetails = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password } 
            };
            var content = new MultipartFormDataContent();
            foreach (var keyValuePair in loginDetails)
                content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
            var response = await Client!.PostAsync("/v1/Authentication/authenticate", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(responseBody)!;

            // Assertions
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Response status should be OK");
            Assert.That(result.token, Is.Not.Null, "Authentication token should not be null");

            UserToken = result.token;   // Set the user token globally to the tests
        }

        /// <summary>
        /// Delete all created rows and other objects in test methods
        /// </summary>
        /// <returns></returns>
        async Task Cleanup()
        {
            if (!Client!.DefaultRequestHeaders.Contains("Authorization"))
            {
                Client!.DefaultRequestHeaders.Add("Authorization", $"Bearer {UserToken}");
            }

            foreach (var barcodeId in createdStaticBarcodes)
            {
                var response = await Client!.DeleteAsync($"{Endpoints.Barcodes_endpoint}{Endpoints.Barcodes_s_delete_endpoint}/{barcodeId}");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Static Barcode {barcodeId} should be deleted successfully");
            }

            foreach (var barcodeId in createdDynamicBarcodes)
            {
                var response = await Client!.DeleteAsync($"{Endpoints.Barcodes_endpoint}{Endpoints.Barcodes_d_delete_endpoint}/{barcodeId}");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Dynamic Barcode {barcodeId} should be deleted successfully");
            }

            foreach (var barcodeId in createdStaticBarcodes)
            {
                var response = await Client!.DeleteAsync($"{Endpoints.Barcodes_endpoint}{Endpoints.Products_delete_endpoint}/{barcodeId}");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Product {barcodeId} should be deleted successfully");
            }
        }
    }
}
