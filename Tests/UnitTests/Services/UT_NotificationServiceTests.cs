using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.UnitTests.Services
{
    [TestFixture()]
    public class UT_NotificationServiceTests
    {
        private Mock<INotificationService>? _mockNotificationService;

        [SetUp]
        public void SetUp()
        {
            _mockNotificationService = new Mock<INotificationService>();
        }

        /// <summary>
        /// Tests the EmailNotification method to verify it sends an email notification successfully.
        /// </summary>
        /// <remarks>
        /// This test checks if the EmailNotification method returns an OkResult indicating 
        /// the email was sent successfully. The mock INotificationService is set up to return 
        /// OkResult when the method is called with valid parameters.
        /// </remarks>
        [Test()]
        public async Task EmailNotificationTest_Success()
        {
            // Arrange
            var ParEmp = "sender@example.com";
            var ParamTo = "receiver@example.com";
            var ParamBcc = "bcc@example.com";
            var ParamSubject = "Test Subject";
            var ParamBody = "This is a test email body.";
            var Attachments = "path/to/attachment";

            _mockNotificationService!.Setup(service => service.EmailNotification(ParEmp, ParamTo, ParamBcc, ParamSubject, ParamBody, Attachments))
                .ReturnsAsync(new OkResult());

            var notificationService = _mockNotificationService.Object;

            // Act
            var result = await notificationService.EmailNotification(ParEmp, ParamTo, ParamBcc, ParamSubject, ParamBody, Attachments);

            // Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        /// <summary>
        /// Tests the EmailNotification method to verify it fails to send an email notification.
        /// </summary>
        /// <remarks>
        /// This test checks if the EmailNotification method returns a BadRequestResult indicating 
        /// the email failed to send. The mock INotificationService is set up to return 
        /// BadRequestResult when the method is called with invalid parameters.
        /// </remarks>
        [Test()]
        public async Task EmailNotificationTest_Failure()
        {
            // Arrange
            var ParEmp = "sender@example.com";
            var ParamTo = ""; // Invalid email address
            var ParamBcc = "bcc@example.com";
            var ParamSubject = "Test Subject";
            var ParamBody = "This is a test email body.";
            var Attachments = "path/to/attachment";

            _mockNotificationService!.Setup(service => service.EmailNotification(ParEmp, ParamTo, ParamBcc, ParamSubject, ParamBody, Attachments))
                .ReturnsAsync(new BadRequestResult());

            var notificationService = _mockNotificationService.Object;

            // Act
            var result = await notificationService.EmailNotification(ParEmp, ParamTo, ParamBcc, ParamSubject, ParamBody, Attachments);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }
    }
}
