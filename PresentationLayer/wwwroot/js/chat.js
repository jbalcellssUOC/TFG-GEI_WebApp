document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('chatSupport').addEventListener('click', function (e) {
        const chatBox = document.querySelector('.small-chat-box');
        if (isChatBoxActive()) {
            ClearChatMessages();
        }
        else {
        }
    });

    document.getElementById('messageText').addEventListener('keypress', function (event) {
        if (event.key === 'Enter') {
            event.preventDefault();     // Prevent the default action to avoid submitting the form
            sendChatMessage();          // Call the sendChatMessage function
        }
    });

    document.getElementById('sendMessageBtn').addEventListener('click', function (e) {
        sendChatMessage();              // Call the sendChatMessage function
    });
});

document.addEventListener('DOMContentLoaded', function () {
    ClearChatMessages();
    PreviousChatMessagesInitial();
});

var randomStaff = Math.floor(Math.random() * 10) + 1
var userChatD = "Support Staff #" + randomStaff;

function sendChatMessageFromInput() {
    sendChatMessage();
}

function scrollToBottom() {
    var chatContent = document.getElementById('chatContent');
    if (chatContent) {
        chatContent.scrollTop = chatContent.scrollHeight;
        const scrollBar = document.querySelector('.slimScrollBar');
        if (scrollBar) scrollBar.style.setProperty("top", "310px"); // Set the top property to the height of the container
    }
}

function createLoadingIndicator() {
    var existingIndicator = document.getElementById('loadingIndicator');
    if (!existingIndicator) {
        var indicator = document.createElement('div');
        indicator.id = 'loadingIndicator';
        indicator.className = 'loading-dots';
        indicator.style.display = 'none';                               // Initially hidden
        indicator.innerHTML = '<div></div><div></div><div></div>';
        return indicator;
    }
    return existingIndicator;
}

function showLoadingIndicator() {
    let chatContent = document.getElementById('chatContent');
    let indicator = document.getElementById('loadingIndicator');

    // Check if the indicator already exists
    if (!indicator) {
        indicator = document.createElement('div');
        indicator.id = 'loadingIndicator';
        indicator.className = 'loading-dots';
        indicator.innerHTML = '<div></div><div></div><div></div>';
        chatContent.appendChild(indicator);                             // Append it as the last child
    }

    indicator.style.display = 'flex';  // Show the indicator

    // Set a timeout to automatically hide and remove the indicator after 30 seconds
    setTimeout(function () {
        hideLoadingIndicator();     // Call hideLoadingIndicator to remove the indicator from DOM
    }, 60000);                      // 60000 milliseconds equals 60 seconds
}

function hideLoadingIndicator() {
    let indicator = document.getElementById('loadingIndicator');
    if (indicator) {
        indicator.parentNode.removeChild(indicator);  // Remove the indicator from the DOM
    }
}

function AddChatMessage(author, source, messageText, datetime, active) {
    var newMessage = document.createElement('div');
    if (source)
        newMessage.classList.add('right');
    else newMessage.classList.add('left');
    newMessage.innerHTML = `
                <div class="author-name">
                    ${author} <small class="chat-date">${datetime}</small>
                </div>
                <div class="chat-message ${active}">
                    ${messageText}
                </div>
                `;
    var chatContent = document.getElementById('chatContent');
    chatContent.appendChild(newMessage);
    scrollToBottom(); // Scroll to the bottom
}

function DateToLocalString(currentDate) {
    return currentDate.toLocaleDateString('es-ES', {
        year: 'numeric', month: '2-digit', day: '2-digit',
        hour: '2-digit', minute: '2-digit', second: '2-digit', hour12: false
    }).replace(/\//g, '.');
}

function WelcomeChatMessage() {
    var currentDate = new Date();
    var currentDateString = DateToLocalString(currentDate);
    AddChatMessage(userChatD + " wrote on ", false, "How can we assist you today with Codis365 services?", currentDateString, "active");
}

function isChatBoxActive() {
    const chatBox = document.querySelector('.small-chat-box');
    return chatBox.classList.contains('active');
}

function InsertPreviousConversationsBtn() {
    var chatContent = document.getElementById('chatContent');
    var buttonHTML = `
        <div id="PreviousChatMessages">
            <div class="center">
                <button id="PreviousChatMessagesBtn" class="btn btn-secondary">
                    Previous conversations
                </button>
            </div>
        </div>
    `;

    chatContent.innerHTML = buttonHTML;     // Reinsert the button HTML

    if (document.getElementById('PreviousChatMessagesBtn')) {
        document.getElementById('PreviousChatMessagesBtn').addEventListener('click', function () {
            PreviousChatMessages();
        });
    }
}

function ClearChatMessages() {
    hideLoadingIndicator()
    scrollToBottom();                   // Scroll to the bottom
}

function PreviousChatMessagesInit() {
    var _Data = {};
    $.ajax({
        url: "/SupportChat/GetAllUserChatMessages",
        type: "POST",
        async: true,
        data: _Data,
        datatype: "json",
        success: function (data) {
            if (data != null) {
                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        var message = data[i];
                        var currentDate = new Date(message.datetime);
                        var currentDateString = DateToLocalString(currentDate);
                        AddChatMessage(message.userName, message.source, message.message, currentDateString, "");
                    }
                    WelcomeChatMessage();
                    scrollToBottom()
                }
                else {
                    WelcomeChatMessage();
                    scrollToBottom()
                }
                UpdateChatMessagesUnread(data.length);
            }
        },
        always: function (data) { },
        error: function (data) { }
    });

}

var messages = null;

function GetPreviousMessages() {
    var _Data = {};
    $.ajax({
        url: "/SupportChat/GetAllUserChatMessages",
        type: "POST",
        async: false,
        data: _Data,
        datatype: "json",
        success: function (data) {
            if (data != null) {
                messages = data;
            }
        },
        always: function (data) { },
        error: function (data) { }
    });
}

function PreviousChatMessagesInitial() {
    GetPreviousMessages();
    if (messages != null) {
        UpdateChatMessagesUnread(messages.length);
        if (messages.length > 0) InsertPreviousConversationsBtn();
    }
        
    WelcomeChatMessage();
}

function PreviousChatMessages() {
    chatContent.innerHTML = '';         // Clear the chat content

    var _Data = { };
    $.ajax({
        url: "/SupportChat/GetAllUserChatMessages",
        type: "POST",
        async: false,
        data: _Data,
        datatype: "json",
        success: function (data) {
            if (data != null) {
                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        var message = data[i];
                        var currentDate = new Date(message.datetime);
                        var currentDateString = DateToLocalString(currentDate);
                        AddChatMessage(message.userName, message.source, message.message, currentDateString, "");
                    }
                    WelcomeChatMessage();
                    scrollToBottom()
                }
                else {
                    WelcomeChatMessage();
                    scrollToBottom()
                }
                UpdateChatMessagesUnread(data.length);
            }
        },
        always: function (data) { },
        error: function (data) { }
    });
}

function UpdateChatMessagesUnread(messagesNumber) {
    var badge = document.getElementById('badgeNumber');
    badge.textContent = messagesNumber;  
    badge.style.display = messagesNumber > 0 ? 'inline-block' : 'none';
}

function sendChatMessage() {
    var messageText = document.getElementById('messageText').value;
    var currentDate = new Date();
    var currentDateString = DateToLocalString(currentDate);
    if (!messageText.trim()) return;  // Ignore empty messages
    var userChatS = "User";
    AddChatMessage(userChatS + " wrote on ", true, messageText, currentDateString, "");
    document.getElementById('messageText').value = ''; // Clear input field
    connection.invoke('SendChatMessage', userChatS, userChatD, true, messageText, currentDate).catch(err => console.error("Chat Error: ", err.toString()));
    playReceivedMessageSound();
    showLoadingIndicator();
}

function playReceivedMessageSound() {
    var audio = document.getElementById('messageChatSoundIn');
    audio.play();
}