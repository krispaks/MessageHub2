﻿
var users = [[]];
var username = null;
var chat = null;
var layerIndex = 10;

$(function () {
    chat = $.connection.chatHub;

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

                //targetDiv.innerHTML = "Chat with " + param;
                var targetDiv = document.getElementById(clone.id).getElementsByClassName("chatTitle")[0];

                jQuery.ajax({
                    type: 'GET',
                    url: '/api/UserInfoApi/GetUserRealName',
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    data: { email: strFrom },
                    success: function (data) {
                        console.log("succesfully got the name '" + data[1] + " " + data[2] + "' for '" + data[0] + "'");
                        targetDiv.innerHTML = "Chat with " + data[1] + " " + data[2];
                    },
                    failure: function (errMsg) {
                        console.log("error getting the name");
                        targetDiv.innerHTML = "Chat with " + param + "(ERROR)";
                    }
                });

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
    });

    // loads the username
    chat.client.whoami = function (iam) {
        // sets the user name
        username = iam;
        console.log("who am i = " + username);

        // attaches the users real name to the chat module
        jQuery.ajax({
            type: 'GET',
            url: '/api/UserInfoApi/GetUserRealName',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: { email: username },
            success: function (data) {
                console.log("succesfully got the name '" + data[1] + " " + data[2] + "' for '" + data[0] + "'");
                $('#user-status').append("You're logged in as <strong>"+data[1]+" "+data[2]+"</strong>.<br><br>");
            },
            failure: function (errMsg) {
                console.log("error getting the name");
                $('#user-status').append("You're logged in as <strong>" + username + "</strong>.<br><br>");
            }
        });
    }

    // function to be called by the hub to reset the list of connected users
    chat.client.resetUsers = function () {
        users = [[]];
    }

    // function to be called by the hub when a user connects and in the load of the page
    chat.client.userConnects = function (email, connection) {
        console.log(email + " is connected");

        var userPos = -1;

        // checks if the user is already in the list
        for(var i=0; i<users.length; i++){
            if (users[i][0] == email) {
                // the user is already in the list
                userPos = i;
            }
        }

        // if the user is already in the list, we update the connection id
        if (userPos != -1) {
            users[userPos][1] = connection;
        } else {
            // the user is added to the list of active users (username + connection info)
            users[users.length] = [email, connection];
            console.log("users[" + (users.length - 1) + "] = " + users[(users.length - 1)][0] + " - " + users[(users.length - 1)][1]);
        }

        // the user list is updated
        $('#connected-users').html("");
        if (users.length <= 2) {
            $('#connected-users').append('<a class="list-group-item">No users connected</a>');
            if(users.length > 0)
                console.log("connected user " + 1 + " email = " + users[1][0] + " [" + users[1][1] + "]");
        } else {
            // aux list for storing the users
            var auxList = [[]];
            var wait = true;
            // fill the list with the content of the db
            for (var i = 1; i < users.length; i++) {
                console.log("connected user " + i + " email = " + users[i][0] + " [" + users[i][1] + "]");
                if (users[i][0] != username) {
                    console.log("I'm gonna ask for the info for: "+users[i][0]);
                    jQuery.ajax({
                        type: 'GET',
                        url: '/api/UserInfoApi/GetUserRealName',
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        data: { email: users[i][0] },
                        success: function (data) {
                            console.log("succesfully got the name '" + data[1] + " " + data[2] + "' for '" + data[0]+"'");
                            // include the user in the aux list to be able to sort
                            var alreadyIn = false;
                            for(var k = 0; k < auxList.length; k++){
                                if(auxList[k][0] == data[0])
                                    alreadyIn = true;
                            }

                            if(alreadyIn == false)
                                auxList[auxList.length] = ["" + data[0], "" + data[1], "" + data[2]];

                            // if the user list has been completed
                            if (i >= users.length) {
                                $('#connected-users').html("");
                                // sort the auxList by name
                                auxList.sort(function (a, b) {
                                    return (a[1] + " " + a[2] < b[1] + " " + b[2] ? -1 : (a[1] + " " + a[2] > b[1] + " " + b[2] ? 1 : 0));
                                });
                                // fill the chat box with the sorted list
                                for (var j = 0; j < auxList.length; j++) {
                                    if (auxList[j][0] != undefined) {
                                        console.log("auxList pos " + j + ": " + auxList[j][0] + " - " + auxList[j][1] + " - " + auxList[j][2]);
                                        $('#connected-users').append('<a href="#" onClick="openChatWindow(\'' + auxList[j][0] + '\'); return false;" class="list-group-item">'
                                                + '<h5 class="list-group-item-title" style="padding: 0px 0px; margin-top: -5px; margin-bottom: 0px;">'
                                                + htmlEncode(auxList[j][1] + " " + auxList[j][2])
                                                + '</h5>'
                                                + '<h6 class="list-group-item-title" style="padding: 0px 0px; margin-top: 5px; margin-bottom: -5px; color: #999999;">'
                                                + htmlEncode(auxList[j][0])
                                                + '</h6>'
                                            + '</a>');
                                    }
                                }
                            }
                        },
                        failure: function (errMsg) {
                            console.log("error getting the name");
                            $('#connected-users').append('<a href="#" onClick="openChatWindow(\'' + users[i][0] + '\'); return false;" class="list-group-item">' + htmlEncode("NULL") + '</a>');
                        }
                    });
                }
            }
        }
    };
});

// a user sends a message to another user
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
}

// splits the message to fit the chat window
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
        //targetDiv.innerHTML = "Chat with " + param;
        var targetDiv = document.getElementById(clone.id).getElementsByClassName("chatTitle")[0];
        
        jQuery.ajax({
            type: 'GET',
            url: '/api/UserInfoApi/GetUserRealName',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: { email: param },
            success: function (data) {
                console.log("succesfully got the name '" + data[1] + " " + data[2] + "' for '" + data[0] + "'");
                targetDiv.innerHTML = "Chat with " + data[1] + " " + data[2];
            },
            failure: function (errMsg) {
                console.log("error getting the name");
                targetDiv.innerHTML = "Chat with " + param;
            }
        });

        // lower part of the window (text input, send button)
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
            chat.server.send(strChatText, username, strTo);
            //chat.server.send(username + ': ' + strChatText, username, strTo);
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

// get a user's real name (first and last) by his username
function getUserRealName(mail) {
    // (not used, but kept to be used where needed)
    var returnVal = "";
    jQuery.ajax({
        type: 'GET',
        url: '/api/UserInfoApi/GetUserRealName',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: { email: mail },
        success: function (data) {
            console.log("succesfully got the name '" + data[1] + " " + data[2] + "' for '" + data[0] + "'");
            returnVal = data[1] + " " + data[2];
        },
        failure: function (errMsg) {
            console.log("error getting the name");
            returnVal = "NULL";
        }
    });
    return returnVal;
}

// place the conversation loaded from the db in the chat box
function fillChatBox(content, id) {
    console.log("fill chat box with this: " + JSON.stringify(content));

    // prepares the div for inserting the new content
    var messageList = document.getElementById("" + id).getElementsByClassName("message-list")[0];

    // process each entry of the json
    jQuery.each(content, function (i, val) {
        var printLine = val.content;

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
