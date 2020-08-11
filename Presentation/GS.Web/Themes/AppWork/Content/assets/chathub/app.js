"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

//doi tuong tra ve la 1 danh sach item chua doc
connection.on("ReceiveMessage", function (msgs) {
    //console.log(message);
    if (msgs.length == 0)
        return;
    $(".notifyTotalNew").html(msgs.length);
    LoadNotificationContent(msgs);
});
//tao html tu du lieu tra ve
function LoadNotificationContent(items) {
    //su dung kendo template
    //template dc luu trong phan _Shared/_Notification.cshtml
    var template = kendo.template($("#notification-template").html());
    var itemPreviewHtmls = "";
    for (var i = 0; i < items.length; i++) {
        var itemPreviewHtml = template(items[i]);
        itemPreviewHtmls = itemPreviewHtmls + itemPreviewHtml;
    }    
    $("#notifyContent").html(itemPreviewHtmls);
}
connection.start()
    .then(function () {
        console.log('connection started');
        //tao subcrible cho user dang dang nhap
        connection.invoke("subscribe", $("#hidCustomerId").val())
    })
    .catch(function (err) {
    return console.error(err.toString());
    });
