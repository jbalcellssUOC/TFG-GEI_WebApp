window.addEventListener('load', OnLoadSecurity);

let connection;
var timeInactivity;

// !Important to avoid errores on unload page
window.onbeforeunload = function () {
    if (this.connection !== null && this.connection !== this.undefined) {
        connection.invoke("ClientDisconnectAsync");
        this.connection.stop();
    }
}; 

/***** onload() ******/
function OnLoadSecurity() {
    AuthResetTimer();
    AuthInactivityTime(); // Check for user inactivity

    // SignalR Developer & Production URL, Important!
    var baseUrl = `${window.location.protocol}//${window.location.host}`; // Get the base URL using protocol and host
    var url = `${baseUrl}/securityhub`;
    var baseurl = `${baseUrl}/securityhub`;
    var token = GetTokenId();   // Append the pathname and the specific endpoint
    console.log("Codis365. SignalR Session Token", token);
    if (token !== "" && token !== null && token !== undefined) {
        url = url + "?tokenId=" + token;
    }
    // Create Hub connection
    try {
        console.log('Codis365. Starting SignalR Hub.');
        try {
            connection = new signalR
                .HubConnectionBuilder()
                .withUrl(baseurl)
                .configureLogging(signalR.LogLevel.Error)  // Error, Warning, Information, Trace
                .build();

            connection.serverTimeoutInMilliseconds = 60000; // TimeOut 60 sec.
            connection.setTimeout = 60000;

            // ############################################################################################## //
            // ######## Receive methods from the Hub "connection.on" always before start() !Important  ###### //
            // ############################################################################################## //

            connection.on("SetSessionTokenId", (connectionId) => { SetTokenId(connection, connectionId); });
            connection.on("ReceiveMessage", (user, message) => { console.log(user, message); });
            connection.on("GetProgressStatus", (type, status, message1, message2) => { SetProgressStatus(type, status, message1, message2); });


            function DateToLocalString(currentDate) {
                const date = new Date(currentDate);
                var dateTimeString = date.toLocaleString('en-US', {
                    year: 'numeric', month: '2-digit', day: '2-digit',
                    hour: '2-digit', minute: '2-digit', second: '2-digit', hour12: false
                }).replace(/\//g, '.');
                return dateTimeString;
            }

            if (connection) {
                connection.on("ReceiveChatMessage", (user, source, message, datetime) => {
                    var currentDate = DateToLocalString(datetime);
                    AddChatMessage(user + " wrote on ", source, message, currentDate, "active");
                    playReceivedMessageSound();
                    playReceivedMessageSound();
                    hideLoadingIndicator()
                });
            }

            start(connection, url);
            connection.onclose(async (error) => { console.log("Codis365. SignalR OnClose: Trying to reconnect to SecurityHub..."); await start(connection, url); });  // Always try reconnection

            // ############################################################################################## //
            // ######## Example to Invoque medthods in the Hub from the client                         ###### //
            // ############################################################################################## //

            //connection.invoke("SendMessage", user, message).catch(err => console.error(err.toString()));
            //connection.invoke("SendChatMessage", "User", "Test Message", null).catch(err => console.error(err.toString()));

        }
        catch (error) {
            console.error("Codis365. SignalR is not installed. Cannot start. ", error);   // Handle errors that occur during the instantiation of the connection.
        } 
    }
    catch (error) {
        console.log('Codis365. SignalR is not installed. ', error);
    }
}

// ################################################################ //
// ################## COMMON FUNCTIONS AND START ################## //
// ################################################################ //
async function start(connection, url) {
    try {
        if (connection !== undefined) {
            await connection.start({ transport: ['serverSentEvents', 'foreverFrame', 'longPolling'] });
            connection.invoke('GetConnectionId')
                .then(function (connectionId){
                    SetTokenId(connection, connectionId);
                    console.log("Codis365. SignalR Connected to SecurityHub with Token: [" + GetTokenId() + "] & ConnectionId: [" + connectionId + "]");
                });
            connection.invoke('SetProgramHistory', GetTokenId(), window.location.href);
        } else {
            connection = new signalR.HubConnectionBuilder()
                .withUrl(url)
                .configureLogging(signalR.LogLevel.Error)   // Error, Warning, Information, Trace
                .build();
            connection.serverTimeoutInMilliseconds = 60000; // TimeOut 60 sec.
            connection.setTimeout = 60000;
            start(connection, url);
        }
    } catch (err) {
        console.log("Codis365. SignalR Exception -> ", err.toString());
        setTimeout(() => start(connection, url), 20000);
    }
}

function GetTokenId() { return window.sessionStorage.tokenId; }

function SetTokenId(connection, connectionId) {
    var sessionId = window.sessionStorage.tokenId;
    if (!sessionId) { window.sessionStorage.tokenId = connectionId; }
}

function AuthLogout() {
    var iLabSpinner = Rats.UI.LoadAnimation.start();
    location.href = '/SignIn/Login';
    $.ajax({
        url: "/SignIn/MakeLogout",
        async: true,
        type: "POST",
        data: null,
        datatype: "json",
        processData: false,
        contentType: false,
        success: function (data, textStatus, XmlHttpRequest) {
            location.href = '/SignIn/Login';
        },
        always: function (data) { },
        error: function (data) { }
    });
}

function AuthResetTimer() {
    //console.log("Codis365. Resetting inativity Time:", timeInactivity);
    clearTimeout(timeInactivity);
    timeInactivity = setTimeout(AuthLogout, 300000);    // 5 Minutes

    //timeInactivity = setTimeout(AuthLogout, 1800000);   // 30 Minutes
    //timeInactivity = setTimeout(AuthLogout, 15000);     // 15 Secs
}

function AuthInactivityTime(){
    // DOM Events
    document.onmousemove = AuthResetTimer;                      // on mouse move
    document.onkeydown = AuthResetTimer;                        // on onkeydown
    document.onkeyup = AuthResetTimer;                          // on onkeyup
    document.ontouchstart = AuthResetTimer;                     // on touchpad clicks
    document.onclick = AuthResetTimer;                          // Reset timer    
    document.onkeydown = AuthResetTimer;                        // on onkeydown
    document.onkeyup = AuthResetTimer;                          // on onkeyup
    document.addEventListener('scroll', AuthResetTimer, true);  // on scroll, improved
}
