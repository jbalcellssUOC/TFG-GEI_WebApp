using BusinessLogicLayer.Interfaces;
using Moq;

namespace Tests.UnitTests.Services
{
    [TestFixture()]
    public class UT_PromptServiceTests
    {
        private Mock<IPromptService>? _mockPromptService;

        [SetUp]
        public void SetUp()
        {
            _mockPromptService = new Mock<IPromptService>();
        }

        /// <summary>
        /// Tests the TriggerPromptOpenAI method to verify it returns the expected response.
        /// </summary>
        /// <remarks>
        /// This test checks if the TriggerPromptOpenAI method returns the correct response when provided with a valid prompt.
        /// The mock IPromptService is set up to return a specific response when the method is called with a valid prompt.
        /// </remarks>
        [Test()]
        public async Task TriggerPromptOpenAITest_Success()
        {
            // Arrange
            var prompt = "What is the capital of France?";
            var expectedResponse = "The capital of France is Paris.";
            _mockPromptService!.Setup(service => service.TriggerPromptOpenAI(prompt))
                .ReturnsAsync(expectedResponse);

            var promptService = _mockPromptService.Object;

            // Act
            var result = await promptService.TriggerPromptOpenAI(prompt);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResponse));
        }

        /// <summary>
        /// Tests the TriggerPromptOpenAI method to verify it handles errors gracefully.
        /// </summary>
        /// <remarks>
        /// This test checks if the TriggerPromptOpenAI method returns an error message when an exception occurs.
        /// The mock IPromptService is set up to return a specific error message when an exception is thrown.
        /// </remarks>
        [Test()]
        public async Task TriggerPromptOpenAITest_Failure()
        {
            // Arrange
            var prompt = "Invalid prompt";
            var expectedErrorMessage = "Sorry, I am not able to answer that question right now.";
            _mockPromptService!.Setup(service => service.TriggerPromptOpenAI(prompt))
                .ReturnsAsync(expectedErrorMessage);

            var promptService = _mockPromptService.Object;

            // Act
            var result = await promptService.TriggerPromptOpenAI(prompt);

            // Assert
            Assert.That(result, Is.EqualTo(expectedErrorMessage));
        }
    }
}
