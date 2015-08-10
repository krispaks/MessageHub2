//define([function () {
//    'use strict';
//    console.log('chat script!');
//}]);

var users = [[]];
var username = null;
var chat = null;
var layerIndex = 10;

$(function () {
    // Reference the auto-generated proxy for the hub.

    console.log("1st CALL - USERNAME: "+username);

    chat = $.connection.chatHub;
    console.log('connected to chat from chat.js');

    // Create a function that the hub can call back to display messages.
    chat.client.addNewMessageToPage = function (name, message) {
        // Add the message to the page.
        $('#discussion').append('<li><strong>' + htmlEncode(name) + '</strong>: ' + htmlEncode(message) + '</li>');
    };

    chat.client.addMessage = function (message, strFrom, strTo) {
        console.log("NEW MESSAGE: " + message + " [" + strFrom + " - " + strTo + "]");

        // checks if the window is already been creted
        if (username != strFrom) {
            if (document.getElementById("" + strFrom) != null) {
                // yeah, its been created
                console.log(strFrom + " EXISTE");
                if (document.getElementById("" + strFrom).style.visibility == "visible") {
                    // we dont do anything cause the user might not want that window to be popping up all the time
                } else {
                    // the window has been closed but still exists. lets open it again
                    console.log(strFrom + " INVISIBLE");
                    document.getElementById("" + strFrom).style.visibility = "visible";
                    document.getElementById("" + strFrom).style.zIndex = layerIndex++;
                }

                addMsg(message, strFrom, strTo);
            } else {
                // the window doesnt exist. we need to create it
                console.log(strFrom + " NO EXISTE");

                // create a clone of the base chat window
                var div = document.getElementById('FloatingChat'),
                clone = div.cloneNode(true);
                clone.id = "" + strFrom;
                document.body.appendChild(clone);
                // sets its properties
                document.getElementById(clone.id).style.visibility = "visible";
                document.getElementById(clone.id).style.zIndex = layerIndex++;
                var targetDiv = document.getElementById(clone.id).getElementsByClassName("chatTitle")[0];
                targetDiv.innerHTML = "Chat with " + strFrom;
                // adds the text area
                var textdiv = document.createElement('div');
                //textdiv.innerHTML = "<input type=\"text\" id=\"cosa\" value=\"\"/><input type=\"button\" id=\"sendmessage\" value=\"Send\" onClick=\"sendMessage(document.getElementById('cosa').value, this.parentNode.parentNode.parentNode.parentNode.parentNode.id);\" />";
                textdiv.innerHTML = "" +
                "<form onsubmit=\"sendMessage(document.getElementById('msg_" + strFrom + "').value, this.parentNode.parentNode.parentNode.parentNode.parentNode.id); document.getElementById('msg_" + strFrom + "').value = ''; return false;\">" +
                    "<div class=\"row\">" +
                        "<div class=\"col-sm-9\"><input type=\"text\" class=\"form-control\" id=\"msg_" + strFrom + "\" value=\"\"/></div>" +
                        "<div class=\"col-sm-2\"><input type=\"button\" class=\"btn btn-primary\" id=\"sendmessage\" value=\"Send\" onClick=\"sendMessage(document.getElementById('msg_" + strFrom + "').value, this.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.id); document.getElementById('msg_" + strFrom + "').value = '';\" /></div>" +
                    "</div>" +
                "</form>";
                document.getElementById(clone.id).getElementsByClassName("text-box")[0].appendChild(textdiv);

                // loads the previous messages from the db
                getPreviousChatController(username, strFrom, message);
            }
        } else {
            addMsg(message, strFrom, strTo);
        }
    };

    // start the connection
    $.connection.hub.start().done(function () {
        // checks online users
        chat.server.getUsers();

        // sends a new message to be broadcasted (its still general chat)
        $('#sendmessage').click(function () {
            // Call the Send method on the hub. 
            chat.server.send($('#displayname').val(), $('#message').val());
            // Clear text box and reset focus for next comment. 
            $('#message').val('').focus();
        });
    });

    // loads the username
    username = $('#username').val();
    // function to be called by the hub when a user connects and in the load of the page
    chat.client.userConnects = function (name, connection) {
        console.log(name + " is connected");

        var userPos = -1;
        // checks if the user is already in the list
        for(var i=0; i<users.length; i++){
            if (users[i][0] == name) {
                // the user is already in the list
                userPos = i;
            }
        }
        // if the user is already in the list, we update the connection id
        if (userPos != -1) {
            users[userPos][1] = connection;
        } else {
            // the user is added to the list of active users (username + connection info)
            users[users.length] = [name, connection];
            console.log("users[" + (users.length - 1) + "] = " + users[(users.length - 1)][0] + " - " + users[(users.length - 1)][1]);
        }

        // the user list is updated
        $('#connected-users').html("");
        if (users.length <= 2) {
            $('#connected-users').append('<a class="list-group-item">No users connected</a>');
            if(users.length > 0)
                console.log("connected user " + 1 + " name = " + users[1][0] + " [" + users[1][1] + "]");
        } else {
            for (var i = 1; i < users.length; i++) {
                console.log("connected user " + i + " name = " + users[i][0] + " [" + users[i][1] + "]");
                if (users[i][0] != username)
                    $('#connected-users').append('<a href="#" onClick="openChatWindow(\'' + users[i][0] + '\'); return false;" class="list-group-item">' + htmlEncode(users[i][0]) + '</a>');
            }
        }
    };
});

function addMsg(message, strFrom, strTo) {
    var messageList;
    var color;
    if (strFrom != username) {
        // creates a new div that contains the new message and appends it to the parent node
        messageList = document.getElementById("" + strFrom).getElementsByClassName("message-list")[0];
        color = "67a0e6";
    } else {
        // creates a new div that contains the new message and appends it to the parent node
        messageList = document.getElementById("" + strTo).getElementsByClassName("message-list")[0];
        color = "9acc3d";
    }
    // fills the div with its content
    console.log("content = " + messageList.textContent);
    var newcontent = document.createElement('div');
    //message = "hello, this is a very long message only for testing, it doesnt have absolutely no other purpose. it might bother you, but i really dont care :)"
    newcontent.innerHTML = "<p>"+ splitMsg(message, color) + "</p>";  // friend
    messageList.appendChild(newcontent.firstChild);
    // scrolls the div to the bottom (so the new messages are seen)
    messageList.parentNode.scrollTop = messageList.parentNode.scrollHeight;

    console.log(" * ADDEED A MESSAGE");
}

function splitMsg(message, color) {
    var msgReturn = '';
    var msgAux = '';
    var msgSplit = message.split(" ");

    // splits the message to be shown in different lines if its too long
    for (i = 0; i < msgSplit.length; i++) {
        if ((msgAux.length + msgSplit[i].length) >= 40) {
            msgReturn += "<span class='label label-default' style='font-size: 100%; font-weight: normal; background-color: #" + color + "';>" + msgAux + "</span><br>";
            msgAux = msgSplit[i] + " ";
        } else {
            msgAux += msgSplit[i] + " ";
        }

        if (i == (msgSplit.length - 1))
            if (msgAux != '')
                msgReturn += "<span class='label label-default' style='font-size: 100%; font-weight: normal; background-color: #" + color + "';>" + msgAux + "</span><br>";
    }
    return msgReturn;
}

// click on a user to start a chat room
function openChatWindow(param) {
    console.log("open chat with " + param);

    // checks if the window is already been creted
    if (document.getElementById("" + param) != null) {
        // yeah, its been created
        console.log(param + " EXISTE");
        if (document.getElementById("" + param).style.visibility == "visible") {
            // the window is already open on the screen. just put it on top of the others
            console.log(param + " VISIBLE");
            document.getElementById("" + param).style.zIndex = layerIndex++;
        } else {
            // the window has been closed but still exists. lets open it again
            console.log(param + " INVISIBLE");
            document.getElementById("" + param).style.visibility = "visible";
            document.getElementById("" + param).style.zIndex = layerIndex++;
        }
    } else {
        // the window doesnt exist. we need to create it
        console.log(param + " NO EXISTE");
        
        // create a clone of the base chat window
        var div = document.getElementById('FloatingChat');
        clone = div.cloneNode(true);
        clone.id = "" + param;
        document.body.appendChild(clone);

        // sets its properties
        document.getElementById(clone.id).style.visibility = "visible";
        document.getElementById(clone.id).style.zIndex = layerIndex++;
        var targetDiv = document.getElementById(clone.id).getElementsByClassName("chatTitle")[0];
        targetDiv.innerHTML = "Chat with " + param;
        var textdiv = document.createElement('div');
        textdiv.innerHTML = "" +
            "<form onsubmit=\"sendMessage(document.getElementById('msg_" + param + "').value, this.parentNode.parentNode.parentNode.parentNode.parentNode.id); document.getElementById('msg_" + param + "').value = ''; return false;\">" +
                "<div class=\"row\">" +
                    "<div class=\"col-sm-9\"><input type=\"text\" class=\"form-control\" id=\"msg_" + param + "\" value=\"\"/></div>" +
                    "<div class=\"col-sm-2\"><input type=\"button\" class=\"btn btn-primary\" id=\"sendmessage\" value=\"Send\" onClick=\"sendMessage(document.getElementById('msg_" + param + "').value, this.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.id); document.getElementById('msg_" + param + "').value = '';\" /></div>" +
                "</div>" +
            "</form>";
        document.getElementById(clone.id).getElementsByClassName("text-box")[0].appendChild(textdiv);

        // loads the previous messages from the db
        getPreviousChatController(username, param, null);

        // create the one-to-one group in the server
        var res = chat.server.createGroup(username, param);
    }

    return false;
}

// click to send a message to a user
function sendMessage(strChatText, strTo) {
    console.log("message=" + strChatText + ", to=" + strTo);
    // connects to the hub to send the message
    if (strChatText != '') {
        //var strGroupName = $(this).parent().attr('groupname');
        if (typeof strTo !== 'undefined' && strTo !== false)
            chat.server.send(username + ': ' + strChatText, username, strTo);
    }

    // connects to the controller to store the message in the db
    storeChatController(username, strTo, strChatText);

    return false;
}

function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}

// store the chat message in the db
function storeChatController(from, to, content) {
    var chatMsg = [from, to, content];

    jQuery.ajax({
        type: 'POST',
        url: '/api/ChatMessageApi/SaveChatMsg',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(chatMsg),
        success: function (data) {
            console.log("succesfully stored chat message");
        },
        failure: function (errMsg) {
            console.log("error storing chat message");
        }
    });
}

// get the previous chats from the db
function getPreviousChatController(from, to, message) {
    console.log(" >>> connects to the db to get "+from+" and "+to+"'s messages");
    var users = [from, to];

    jQuery.ajax({
        type: 'GET',
        url: '/api/ChatMessageApi/GetChatMsgList',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: {from: from, to: to},
        success: function (data) {
            console.log("succesfully loaded message list");
            fillChatBox(data, to);
            // if its suposed to show a new message too
            if(message != null)
                addMsg(message, to, from);
        },
        failure: function (errMsg) {
            console.log("error loading message list");
        }
    });

    console.log(" * FINISH LOADING MESSAGES");
}

// places the conversation loaded from the db in the chat box
function fillChatBox(content, id) {
    console.log("fill chat box with this: " + JSON.stringify(content));

    // prepares the div for inserting the new content
    var messageList = document.getElementById("" + id).getElementsByClassName("message-list")[0];

    // process each entry of the json
    jQuery.each(content, function (i, val) {
        var printLine = val.from + ": " + val.content;
        // if the chain is too long, split it in lines
        if (printLine.length >= 20) {

        }
        // create a new globe for the message
        var newcontent = document.createElement('div');
        var color = val.from == username ? "c9d4b0" : "b0bdcf";
        newcontent.innerHTML = "<p>" + splitMsg(printLine, color) + "</p>";
        messageList.appendChild(newcontent.firstChild);

    });
    
    // scrolls the div to the bottom (so the new messages are seen)
    messageList.parentNode.scrollTop = messageList.parentNode.scrollHeight;
}

// drag functionality for the chat window
var chatdrag = function () {
    return {
        move: function (divid, xpos, ypos) {
            divid.style.left = xpos + 'px';
            divid.style.top = ypos + 'px';
        },
        startMoving: function (divid, container, evt) {
            divid = divid.parentNode;   // chapucilla to move the whole chat window dragging the upper banner
            divid.style.zIndex = layerIndex++;
            evt = evt || window.event;
            var posX = evt.clientX,
                posY = evt.clientY,
            divTop = divid.style.top,
            divLeft = divid.style.left,
            eWi = parseInt(divid.style.width),
            eHe = parseInt(divid.style.height),
            cWi = parseInt(document.getElementById(container).style.width),
            cHe = parseInt(document.getElementById(container).style.height);
            document.getElementById(container).style.cursor = 'move';
            divTop = divTop.replace('px', '');
            divLeft = divLeft.replace('px', '');
            var diffX = posX - divLeft,
                diffY = posY - divTop;
            document.onmousemove = function (evt) {
                evt = evt || window.event;
                var posX = evt.clientX,
                    posY = evt.clientY,
                    aX = posX - diffX,
                    aY = posY - diffY;
                if (aX < 0) aX = 0;
                if (aY < 0) aY = 0;
                if (aX + eWi > cWi) aX = cWi - eWi;
                if (aY + eHe > cHe) aY = cHe - eHe;
                chatdrag.move(divid, aX, aY);
            }
        },
        stopMoving: function (container) {
            var a = document.createElement('script');
            document.getElementById(container).style.cursor = 'default';
            document.onmousemove = function () { }
        },
    }
}();
