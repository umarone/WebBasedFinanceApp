$(function () {
    
    // Proxy created on the fly
    var job = $.connection.notificationsHub;

    // Declare a function on the job hub so the server can invoke it
    job.client.displayCustomer = function () {
        getDataOfNotificationJob();
    };

    // Start the connection
    $.connection.hub.start();
    getDataOfNotificationJob();
});
function getDataOfNotificationJob() {
    $("#divListAppend").html('');
    $.ajax({
        url: $("#hiddenGetNotificationJob").val(),
        type: 'GET',
        datatype: 'json',
        success: function (data) {
            
            var remainNotifi = 0;
            if (data.length > 3) {
                remainNotifi = data.length - 3;
            }
            else {
                remainNotifi = data.length;
            }
            $("#idSpan_TotalNotification").html(data.length);
            $("#idSpan_RemainNotification").html(remainNotifi);

            if (data.length > 0) {
                
                console.log("data" + data.length)
                $.each(data, function (index) {
                    var iConHtml = "";
                    if (data[index].Priority != "" && data[index].Priority == "1") {
                        iConHtml = "<em class='icon-info fa-2x text-info'></em>";
                    }
                    else if (data[index].Priority != "" && data[index].Priority == "2") {
                        iConHtml = "<em class='icon-speech fa-2x text-warning'></em>";
                    }
                    else if (data[index].Priority != "" && data[index].Priority == "3") {
                        iConHtml = "<em class='icon-fire fa-2x text-danger'></em>";
                    }
                    else {
                        iConHtml = "<em class='fa fa-bell fa-2x text-info'></em>";
                    }
                    var liHtml = "";
                    liHtml += "<a href='#' class='list-group-item'>";
                    liHtml += "<div class='media-box'>";
                    liHtml += "<div class='pull-left'>";
                    liHtml += iConHtml;
                    liHtml += "</div>";
                    liHtml += "<div class='media-box-body clearfix'>";
                    liHtml += "<p class='m0'>" + data[index].Title + "</p>";
                    liHtml += "<p class='m0 text-muted'>";
                    if (data[index].Description != "" && data[index].Description.length > 25) {
                        liHtml += "<small>" + data[index].Description.substr(0, 25) + ".....</small>";
                    }
                    else {
                        liHtml += "<small>" + data[index].Description + "</small>";
                    }

                    liHtml += "</p>";
                    liHtml += "</div>";
                    liHtml += "</div>";
                    liHtml += "</a>";

                    $("#divListAppend").append(liHtml);
                    if (index == 2) {
                        return false;
                    }
                });


            }
        }
    });
}