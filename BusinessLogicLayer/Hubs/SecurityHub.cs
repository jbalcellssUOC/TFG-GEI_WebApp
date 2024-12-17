using System.Diagnostics;
using System.Security.Claims;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using NLog;

namespace SecurityHubs.Hubs
{
    /// <summary>
    /// SignalR Hub for Security operations
    /// </summary>
    /// <param name="ChatService"></param>
    /// <param name="PromptService"></param>
    /// <param name="ContextAccessor"></param>
    public class SecurityHub(
        IChatService ChatService,
        IPromptService PromptService,
        IHttpContextAccessor ContextAccessor
        ) : Hub
    {
        private static int userCount = 0;

        //########################################//
        //############## Security hub ############//
        //########################################//

        // ############# Overridable hub methods #############  
        public override async Task OnConnectedAsync()
        {

            // ... Connection Logic ... //

            userCount++;
            var ConnectionId = Context.ConnectionId;
            //var Agent = httpContextAccessor!.HttpContext!.Request.Headers.UserAgent;
            //var ConnectionToken = httpContextAccessor.HttpContext!.Request.Query["tokenId"].ToString();

            await Clients.Client(ConnectionId).SendAsync("SetSessionTokenId", ConnectionId); // Execute method "SetSessionTokenId" through client method "SetSessionTokenId"

            // Execute the existing logic into the original, non-overwritten method.
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            userCount--;

            // ... Disconnection Logic ... //

            // Execute the existing logic into the original, non-overwritten method.
            await base.OnDisconnectedAsync(exception);
        }

        // #############  Public HUB methods  #############  
        // *Use these methods from other .CS in same project or invoking from clients browsers .JS files 
        public void ClientDisconnectAsync() {  
            Task.Run(async () => { await OnDisconnectedAsync(null!); }); 
        }

        /// <summary>
        /// Get the connection ID of the current user
        /// </summary>
        /// <returns></returns>
        public string GetConnectionId() => Context.ConnectionId;

        /// <summary>
        /// Get the number of connected users
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Needs access to instance-level Context property provided by SignalR Hub.")]
        public int GetConnectedUsers() { 
            return userCount; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SRConnectionToken"></param>
        /// <param name="ProgramaHistory"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Needs access to instance-level Context property provided by SignalR Hub.")]
        public async Task SetProgramHistory( string SRConnectionToken, string ProgramaHistory) {
            await Task.Run(() => {
                // dummy
                string srconn = SRConnectionToken;
                string programhst = ProgramaHistory;
            });
        }

        /// <summary>
        /// Send a chat message to the user
        /// </summary>
        /// <param name="chatUserS"></param>
        /// <param name="chatUserD"></param>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task SendChatMessage(string chatUserS, string chatUserD, bool source, string message, DateTime dateTime)
        {
            try
            {
                var ClaimsIdentity = ContextAccessor.HttpContext!.User.Identity as ClaimsIdentity;
                var Claim_UserId = ClaimsIdentity!.FindFirst("UserId")!.Value;

                await ChatService.AddUserChatMessage(Claim_UserId, chatUserS, source, message, dateTime);                   // Save message from user in Chat Database
                string promptResult = await PromptService.TriggerPromptOpenAI(message);                                     // Use AI ChatGT Prompt
                DateTime promptResultTime = DateTime.Now;
                await ChatService.AddUserChatMessage(Claim_UserId, chatUserD, false, promptResult, promptResultTime);       // Save message from AI prompt in Chat Database
                await Clients.Caller.SendAsync("ReceiveChatMessage", chatUserD, false, promptResult, promptResultTime);  // Returns AI Prompt result to user
            }
            catch (Exception ex)
            {
                Logger Logger = LogManager.GetCurrentClassLogger();
                if (!Debugger.IsAttached)
                {
                    Logger.Error(System.Reflection.MethodBase.GetCurrentMethod()!.Name + "[M]: " + ex.Message ?? "" + "[StackT]: " + ex.StackTrace ?? "" + "[HLink]: " + ex.HelpLink ?? "" + "[HResult]: " + ex.HResult ?? "" + "[Source]: " + ex.Source ?? "" + ex.Data ?? "" + "[InnerE]: " + ex.InnerException!.Message ?? "");
                }

            }
        }


        // ############# Public CLIENT methods  ############# 
        // *Use these methods from client .JS files to receive actions or messages from the HUB

        /// <summary>
        /// Set the session token ID for the user
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        [HubMethodName("SetSessionTokenId")] // Alias to be used in client rather than method name in Hub definition
        public async Task SetSessionTokenId(string connectionId) { await Clients.Caller.SendAsync(connectionId); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HubMethodName("ReceiveMessage")] // Alias to be used in client rather than method name in Hub definition
        public async Task ReceiveMessage(string user, string message) { await Clients.All.SendAsync(user, message); }

        /// <summary>
        /// Get the progress status of the user screen
        /// </summary>
        /// <param name="type"></param>
        /// <param name="status"></param>
        /// <param name="message1"></param>
        /// <param name="message2"></param>
        /// <returns></returns>
        [HubMethodName("GetProgressStatus")] // Alias to be used in client rather than method name in Hub definition
        public async Task GetProgressStatus(string type, string status, string message1, string message2) { await Clients.All.SendAsync(type, status, message1, message2); }
    }
}
