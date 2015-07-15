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
        if(username != strFrom)
            if (document.getElementById("" + strFrom) != null) {
                // yeah, its been created
                console.log(strFrom + " EXISTE");
                if (document.getElementById("" + strFrom).style.visibility == "visible") {
                    // the window is already open on the screen. just put it on top of the others
                    //console.log(param + " VISIBLE");
                    //document.getElementById("" + param).style.zIndex = layerIndex++;
                } else {
                    // the window has been closed but still exists. lets open it again
                    console.log(strFrom + " INVISIBLE");
                    document.getElementById("" + strFrom).style.visibility = "visible";
                    document.getElementById("" + strFrom).style.zIndex = layerIndex++;
                }
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
            }

        // adds the message to the window
        if (strFrom != username) {
            // creates a new div that contains the new message and appends it to the parent node
            var messageList = document.getElementById("" + strFrom).getElementsByClassName("message-list")[0];
            console.log("content = " + messageList.textContent);
            var newcontent = document.createElement('div');
            newcontent.innerHTML = "<p><span class='label label-info' style='font-size: 100%; font-weight: normal';>" + message + "</span></p>";
            messageList.appendChild(newcontent.firstChild);
            // scrolls the div to the bottom (so the new messages are seen)
            messageList.parentNode.scrollTop = messageList.parentNode.scrollHeight;
        } else {
            // creates a new div that contains the new message and appends it to the parent node
            var messageList = document.getElementById("" + strTo).getElementsByClassName("message-list")[0];
            console.log("content = " + messageList.textContent);
            var newcontent = document.createElement('div');
            newcontent.innerHTML = "<p><span class='label label-success' style='font-size: 100%; font-weight: normal;'>" + message + "</span></p>";
            messageList.appendChild(newcontent.firstChild);
            // scrolls the div to the bottom (so the new messages are seen)
            messageList.parentNode.scrollTop = messageList.parentNode.scrollHeight;
        }
    };

    // Get the user name and store it to prepend to messages.
    //$('#displayname').val(prompt('Enter your name:', ''));

    // Start the connection.
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
    //chat.client.userConnects = function (name, connection) {
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
        } else {
            for (var i = 1; i < users.length; i++) {
                console.log("connected users. user " + i + " name = " + users[i][0]);
                if (users[i][0] != username)
                    $('#connected-users').append('<a href="#" onClick="openChatWindow(\'' + users[i][0] + '\'); return false;" class="list-group-item">' + htmlEncode(users[i][0]) + '</a>');
            }
        }
    };
});

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
        //var textBox = document.getElementById(clone.id).getElementsByClassName("text-box")[0];
        //textBox.innerHTML = "<input type=\"text\" id=\"message\" value=\"caca\"/><input type=\"button\" onClick=\"console.log('HEY: '+document.getElementById('message').value); sendMessage('HOLA', this.parentNode.parentNode.parentNode.parentNode.id)\"; id=\"sendmessage\" value=\"Send\" />";
        //$('#' + clone.id + ' .text-box').append("HOLA");
        var textdiv = document.createElement('div');
        //textdiv.innerHTML = "<input type=\"text\" id=\"msg_" + param + "\" value=\"\"/><input type=\"button\" id=\"sendmessage\" value=\"Send\" onClick=\"sendMessage(document.getElementById('msg_" + param + "').value, this.parentNode.parentNode.parentNode.parentNode.parentNode.id); document.getElementById('msg_" + param + "').value = '';\" />";
        textdiv.innerHTML = "" +
            "<form onsubmit=\"sendMessage(document.getElementById('msg_" + param + "').value, this.parentNode.parentNode.parentNode.parentNode.parentNode.id); document.getElementById('msg_" + param + "').value = ''; return false;\">" +
                "<div class=\"row\">" +
                    "<div class=\"col-sm-9\"><input type=\"text\" class=\"form-control\" id=\"msg_" + param + "\" value=\"\"/></div>" +
                    "<div class=\"col-sm-2\"><input type=\"button\" class=\"btn btn-primary\" id=\"sendmessage\" value=\"Send\" onClick=\"sendMessage(document.getElementById('msg_" + param + "').value, this.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.id); document.getElementById('msg_" + param + "').value = '';\" /></div>" +
                "</div>" +
            "</form>";
        document.getElementById(clone.id).getElementsByClassName("text-box")[0].appendChild(textdiv);

        // create the one-to-one group in the server
        var res = chat.server.createGroup(username, param);
    }

    return false;
}

// click to send a message to a user
function sendMessage(strChatText, strTo) {
    console.log("message="+strChatText+", to="+strTo);
    if (strChatText != '') {
        //var strGroupName = $(this).parent().attr('groupname');
        if (typeof strTo !== 'undefined' && strTo !== false)
            chat.server.send(username + ': ' + strChatText, username, strTo);
    }
    return false;
}

function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
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
