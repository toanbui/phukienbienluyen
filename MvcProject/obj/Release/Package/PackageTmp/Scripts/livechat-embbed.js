var docTitle = "";
function handleDocMsg(e) {
    if (e.data.action === "setHeight") {
        $('#LiveChat').css("height", e.data.height);
        $('#LiveChat').css("width", 330);
    } else if (e.data.action === "setWidth") {
        $('#LiveChat').css("width", e.data.width);
        $('#LiveChat').css("height", e.data.height);
    } else if (e.data.action === "blink") {
        blinkTitleStop();
        blinkTitle(e.data.content, docTitle, 1000);
    } else if (e.data.action === "stop") {
        blinkTitleStop();
        document.title = docTitle;
    }
}
// assign message handler
if (window.addEventListener) {
    window.addEventListener('message', handleDocMsg, false);
} else if (window.attachEvent) { // ie8
    window.attachEvent('onmessage', handleDocMsg);
}

function LiveChatInit(domain) {
    docTitle = document.title;

    if (typeof (blinkTitle) == "undefined") {
        loadJs(domain + "/Scripts/Admin/blinkTitle.js");
    }
    var iframe = "<iframe ID='LiveChat' width='330' height='46' src='" + domain + "/live-chat.html' frameborder='0' scrolling='no'></iframe>";
    var style = "<style>#LiveChat {position:fixed;right:0;bottom:0;z-index:1111111111111}</style>";
    $('body').append(iframe).append(style);
}

