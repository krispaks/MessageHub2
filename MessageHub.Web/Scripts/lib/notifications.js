
$(function () {
    // proxy to reference the hub
    var notifications = $.connection.notificationHub;

    // receives the list of notifications from the hub
    notifications.client.notificationList = function (notifications) {
        // empties the list of notifications
        $('#notification-list').html("");

        // for each of the notifications, generates an entry in the list
        for (var i = 0; i < notifications.length; i++) {
            // different entry for messages and for comments
            if(notifications[i][4] == "message"){
                publishNotification(i, notifications[i][1], "New post published ", notifications[i][2], notifications[i][3], notifications[i][0], '<span class="glyphicon glyphicon-file" aria-hidden="true"></span>');
            } else if (notifications[i][4] == "comment") {
                publishNotification(i, notifications[i][1], "New comment on ", notifications[i][2], notifications[i][3], notifications[i][0], '<span class="glyphicon glyphicon-comment" aria-hidden="true"></span>');
            }
        }
    };

    publishNotification = function (position, name, text, title, id, date, icon) {
        // format the date to be shown in the entry
        var dateFormat = new Date(date);
        var dateOptions = { year: 'numeric', month: 'short', day: 'numeric' };

        // generates the entry for each one of the notifications
        $('#notification-list').append(''

            //+ '<a href="/Message/Detail/' + id + '" class="list-group-item" id="row' + position + '">'                
            + '<a class="list-group-item" id="row' + position + '">'
                + '<div class="row">'
                    + '<div class="col-sm-2" align="right">'
                        + '<h4 class="list-group-item-title"><font color=BBBBBB>'
                        + icon
                        + '</font></h4>'
                    + '</div>'
                    + '<div class="col-sm-10">'
                        + '<h6 class="list-group-item-title" style="margin-top: 5px; margin-bottom: 5px;" align="right">'
                        + '<font color=999999>'
                        + htmlEncode(text)
                        + '</font>'
                        + htmlEncode(title.toUpperCase())
                        + '</h6>'
                        + '<hr style="margin-top: 0px; margin-bottom: 0px;"/>'
                        + '<h6 class="list-group-item-title" style="margin-top: 5px; margin-bottom: 0px;" align="right"><font color=999999>'
                        + htmlEncode(name)
                        + ' - '
                        + '<i>' + htmlEncode(dateFormat.toLocaleString('en-US', dateOptions)) + '</i>'
                        + '</font></h6>'
                    + '</div>'
                + '</div>'
            + '</a>'
        + '')
    };

    $("#foo0").click(function () {
        console.log("click");
        var url = $(this).data('url');
        // do something with the url client side variable, for example redirect
        window.location.href = url;
    });

    function myFunction() {
        console.log("my function");
    }

    notifications.client.updateNotificationList = function (notifications) {
        // hides the last element in the list
        $("#row4").slideToggle("slow");
        
        // empties the list of notifications
        $('#notification-list').html("");

        // for each of the notifications, generates an entry in the list
        for (var i = 0; i < notifications.length; i++) {
            // different entry for messages and for comments
            if (notifications[i][4] == "message") {
                publishNotification(i, notifications[i][1], "New post published ", notifications[i][2], notifications[i][3], notifications[i][0], '<span class="glyphicon glyphicon-file" aria-hidden="true"></span>');
            } else if (notifications[i][4] == "comment") {
                publishNotification(i, notifications[i][1], "New comment on ", notifications[i][2], notifications[i][3], notifications[i][0], '<span class="glyphicon glyphicon-comment" aria-hidden="true"></span>');
            }
        }
        
        $('#row0').hide();
        $("#row0").slideToggle("slow");
    }

    // start the connection.
    $.connection.hub.start().done(function () { });
});
