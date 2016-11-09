"use strict";

var MESSAGE_TEMPLATE = "<div class='message'><div><span class='sender'>{USER_NAME}</span> <span class='time'>{SENT}</span><div/><div class='message-content'>{MESSAGE}</div></div>";
var SYSTEM_MESSAGE_TEMPLATE = "<div class='system-message'><span class='time'>{SENT}</span> {MESSAGE}</div>";

var UserId = 0;

function SetUserId(id) {
    UserId = id;
}

function ShowMessage(id, userId, userName, sent, message, scroll, animate) {
    var isSystemMessage = !id;
    var isMineMessage = false;

    if (!isSystemMessage && UserId === userId) {
        userName = "Вы";
        isMineMessage = true;
    }

    var messageHtml = !isSystemMessage
        ? MESSAGE_TEMPLATE.replace(/{USER_NAME}/gi, userName || "").replace(/{SENT}/gi, sent).replace(/{MESSAGE}/gi, message)
        : SYSTEM_MESSAGE_TEMPLATE.replace(/{SENT}/gi, sent).replace(/{MESSAGE}/gi, message);

    var $section = $("<section/>");
    $section.attr("data-id", id).attr("data-user-id", userId);

    $section.append($(messageHtml));

    if (isMineMessage) {
        $section.addClass("mine");
    }
 
    $("body").append($section);

    animate = !isSystemMessage && animate;
    
    if (scroll) {
        if (animate) {
            $section.addClass("animate");
            $section.css("visibility", "hidden");
        }
        ScrollToBottom(animate ? 100 : 0, function() {
            if (animate) {
                $section.css("visibility", "").hide().fadeIn(500, function () {
                    $section.removeClass("animate");
                });
            }
        });
    }
}

function ScrollToBottom(time, complete) {
    time = time || 0;
    complete = complete || function () { };

    $("html").animate({ scrollTop: $(document).height() - $(window).height() }, time, complete);
}

function RemoveOldMessages(count) {
    while ($("section").length > count) {
        $("section").first().remove();
    }
}