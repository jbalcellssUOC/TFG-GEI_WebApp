using BusinessLogicLayer.Interfaces;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using NLog;
using System.Diagnostics;
using System.Net.Mime;

namespace BusinessLogicLayer.Services
{
    /// <summary>
    /// NotificationService class
    /// </summary>
    /// <param name="Configuration"></param>
    public class NotificationService(IConfiguration Configuration) : INotificationService
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// OnMessageSent event fired when a new smpt event occurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMessageSent(object sender, MessageSentEventArgs e)
        {
            var responseCode = e.Response.Split(' ')[0];
            if (!responseCode.StartsWith('2'))
            {
                // SAve error to custom log
            }
        }

        /// <summary>
        /// Generate email notification
        /// </summary>
        /// <param name="ParEmp"></param>
        /// <param name="ParamTo"></param>
        /// <param name="ParamBcc"></param>
        /// <param name="ParamSubject"></param>
        /// <param name="ParamBody"></param>
        /// <param name="Attachments"></param>
        /// <returns></returns>

        public async Task<IActionResult> EmailNotification(string ParEmp, string ParamTo, string ParamBcc, string ParamSubject, string ParamBody, string Attachments)
        {
            var resultProcess = false;

            try
            {
                if (ParamTo != null && ParamTo != "")
                {
                    using var smtpClient = new SmtpClient();
                    smtpClient.Timeout = 10000;
                    smtpClient.Connect(Configuration["EmailSettings:smtpServer"], Convert.ToInt16(Configuration["EmailSettings:smtpPort"]), SecureSocketOptions.SslOnConnect);
                    // Outlook 365 : SecureSocketOptions.StartTls
                    smtpClient.Authenticate(Configuration["EmailSettings:smtpUser"], Configuration["EmailSettings:smtpPass"]);

                    // Create email message
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Codis365 Support", Configuration["EmailSettings:smtpUser"]));

                    // To: first contact
                    List<string> ToList = [];
                    ToList = [.. ParamTo.Split(",")];
                    message.To.Add(new MailboxAddress(ToList[0].ToString(), ToList[0].ToString()));

                    // Parse Bcc contacts list
                    List<string> BccList = [];
                    // To: others contacts
                    for (int i = 1; i < ToList.Count; i++)
                    {
                        if (ToList[i] != null && ToList[i] != "")
                        {
                            message.To.Add(new MailboxAddress(ToList[i].ToString(), ToList[i].ToString()));
                        }
                    }

                    // Bcc contacts
                    if (ParamBcc != null)
                    {
                        BccList = [.. ParamBcc.Split(",")];
                        for (int i = 0; i < BccList.Count; i++)
                        {
                            if (BccList[i] != null && BccList[i] != "")
                            {
                                message.Bcc.Add(new MailboxAddress(BccList[i].ToString(), BccList[i].ToString()));
                            }
                        }
                    }

                    // Attachments management
                    if (Attachments != null && Attachments != "")
                    {
                        var AttachList = Attachments.Split("|").ToList();
                        if (AttachList.Count > 0)
                        {
                            foreach (var file in AttachList)
                            {
                                // Create the file attachment for this email message
                                var attachment = new MimePart(MediaTypeNames.Application.Octet)
                                {
                                    Content = new MimeContent(File.OpenRead(file), ContentEncoding.Default),
                                    ContentDisposition = new MimeKit.ContentDisposition
                                    {
                                        CreationDate = File.GetCreationTime(file),
                                        ModificationDate = File.GetLastWriteTime(file),
                                        ReadDate = File.GetLastAccessTime(file)
                                    },
                                    ContentTransferEncoding = ContentEncoding.Base64,
                                    FileName = Path.GetFileName(file)
                                };
                            }
                        }
                    }

                    // Set the subject and body text of the email message
                    message.Subject = ParamSubject;
                    message.Body = new TextPart(TextFormat.Html) { Text = ParamBody };

                    try
                    {
                        // Send email
                        smtpClient.MessageSent += OnMessageSent!;
                        await smtpClient.SendAsync(message);
                        resultProcess = true;
                    }
                    catch (Exception ex)
                    {
                        if (!Debugger.IsAttached)
                            Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? ""}[StackT]: {ex.StackTrace ?? ""}[HLink]: {ex.HelpLink ?? ""}[HResult]: {ex.HResult}[Source]: {ex.Source ?? ""}{(ex.Data?.Count > 0 ? ex.Data : "")}[InnerE]: {ex.InnerException?.Message ?? ""}");
                        resultProcess = false;              // Save error to custom log
                    }
                    finally
                    {
                        smtpClient.Disconnect(true);            
                    }
                }
            }
            catch (Exception ex)
            {
                if (!Debugger.IsAttached)
                    Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? ""}[StackT]: {ex.StackTrace ?? ""}[HLink]: {ex.HelpLink ?? ""}[HResult]: {ex.HResult}[Source]: {ex.Source ?? ""}{(ex.Data?.Count > 0 ? ex.Data : "")}[InnerE]: {ex.InnerException?.Message ?? ""}");
                resultProcess = false;
            }

            if (resultProcess == true)
            {
                return new OkObjectResult(null);
            }
            else
            {
                return new BadRequestResult();
            }
        }
    }
}
