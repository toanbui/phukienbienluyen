$(function () {
    //Validate init 
    $.extend($.validator.messages, {
        required: "Trường này bắt buộc có",
    });

    $("#fileInput").on("change", fileSelected);
});
var chatWith = "";
var chatWithName = "";
var timeToBottom = 800;
// Declare a proxy to reference the hub.
var chat = $.connection.chatHub;
//set status support
chat.client.SetOnline = function (data) {
    //reset status
    $("[support-persion]").find(".direct-chat-status").removeClass("online");
    var users = JSON.parse(data);
    if (users != "") {
        for (var user in users) {
            var obj = JSON.parse(user);
            if (obj.IsSupport) {
                $("[support-persion][data-uname='" + obj.Name + "']").attr("data-online", "true").find(".direct-chat-status").addClass("online");
            }
        }
    }
};

//set info
chat.client.SetInfo = function (data) {
    setMyInfo(data);
};

chat.client.CreateWindow = function (data) {
    if (data != "") {
        var sender = JSON.parse(data.sender),
            recieved = JSON.parse(data.recieved),
            window = data.window,
            group = data.group;

        var myInfo = getMyInfo();

        var $window = $("[box-chatwindow][data-group-name='" + group + "']");
        if ($window.length == 0) {
            $("[box-register]").hide();
            $("[box-support]").show();
            $("[live-chat-display]").prepend(window);
            $window = $("[live-chat-display]").find("[box-chatwindow][data-group-name='" + group + "']");

            //get history
            getHistory(group, 1, 10, function (data) {
                if (data.isSuccess) {
                    if (data.data.length > 0) {
                        data.data.reverse().forEach(function (item) {
                            item.time = moment(item.time).format("YYYY-MM-DD HH:mm:ss");
                            bindMessage($("[box-chatwindow][data-group-name='" + group + "']").find("[message-container]"), item, false);
                        });
                        scrollBottom(group);
                    }
                    setTimeout(function () {
                        $("[box-chatwindow][data-group-name='" + group + "']").attr("data-history-init", true);
                    }, timeToBottom);
                }
            });

            //add event for scroll div
            $window.find("[message-container]")[0].addEventListener("scroll", ScrollDiv, false);

            //add event expand , close for chat window 
            var box = $window.boxWidget({
                animationSpeed: 0,
                collapseTrigger: '[data-widget="collapse"]',
                removeTrigger: '[data-widget="remove"]',
                collapseIcon: 'fa-minus',
                expandIcon: 'fa-plus',
                removeIcon: 'fa-times',
                onCollapse: LiveChatCollapse,
                onRemove: LiveChatRemove
            });
        }

        if (myInfo.UnsignName == sender.UnsignName) {
            //Is sender
            $window.attr("data-sendto", recieved.UnsignName);
            if (recieved.IsSupport) {
                $window.find("[chat-with]").html(recieved.SupportName);
            } else {
                $window.find("[chat-with]").html(recieved.Name);
            }
        } else {
            $window.attr("data-sendto", sender.UnsignName);
            if (sender.IsSupport) {
                $window.find("[chat-with]").html(sender.SupportName);
            } else {
                $window.find("[chat-with]").html(sender.Name);
            }
        }

        //resize iframe embbed
        resize();
    }
};
chat.client.RecivedMessage = function (data) {
    if (data != "") {
        var content = data.content,
            group = data.group,
            sender = typeof (data.sender) == "string" ? JSON.parse(data.sender) : data.sender,
            window = data.window;
        if ($("[box-chatwindow][data-group-name='" + group + "']").length > 0) {
            bindMessage($("[box-chatwindow][data-group-name='" + group + "']").find("[message-container]"), data);
        } else {
            //when window closed , only recieved has case
            $("[live-chat-display]").prepend(window).find("[box-chatwindow]").show();
            $("[live-chat-display]").find(".list-support").show();
            $("[live-chat-display]").find(".register").hide();
            $window = $("[live-chat-display]").find("[box-chatwindow][data-group-name='" + group + "']");
            $window.attr("data-sendto", sender.UnsignName);
            if (sender.IsSupport) {
                $window.find(".box-chat-header").html(sender.SupportName);
            } else {
                $window.find(".box-chat-header").html(sender.Name);
            }

            //get history
            getHistory(group, 1, 10, function (data) {
                if (data.isSuccess) {
                    if (data.data.length > 0) {
                        data.data.reverse().forEach(function (item) {
                            item.time = moment(item.time).format("YYYY-MM-DD HH:mm:ss");
                            bindMessage($("[box-chatwindow][data-group-name='" + group + "']").find("[message-container]"), item , false);
                        });
                        scrollBottom(group);
                    }
                    setTimeout(function () {
                        $("[box-chatwindow][data-group-name='" + group + "']").attr("data-history-init", true);
                    }, timeToBottom);
                }
            });

            //add event for scroll div
            $window.find("[message-container]")[0].addEventListener("scroll", ScrollDiv, false);
            //add event expand , close for chat window 
            $window.boxWidget({
                animationSpeed: 0,
                collapseTrigger: '[data-widget="collapse"]',
                removeTrigger: '[data-widget="remove"]',
                collapseIcon: 'fa-minus',
                expandIcon: 'fa-plus',
                removeIcon: 'fa-times',
                onCollapse: LiveChatCollapse
            });
            resize();
        }
        scrollBottom(group);
    }
};

chat.client.Typing = function (data) {
    if (data != "") {
        bindTyping($("[box-chatwindow][data-group-name='" + data.group + "']").find("[message-container]"), data);
    }
};

$.connection.hub.start().done(function () {
    console.log("Live chat has connected !");
})

function openWindow(e) {
    var $container = $(e).closest("[live-chat-display]");
    var myInfo = getMyInfo($container);
    var sendTo = {
        Online: $(e).attr("data-online"),
        Name: $(e).attr("data-uname"),
        SupportName: $(e).attr("data-supportname")
    }
    if (myInfo.IsSupport == "false") {

        //when click chat with offline support
        if (sendTo.online == "false") {
            alert("Người này đang không online !");
            return false;
        }

        //guest
        if (myInfo.Name == "" || myInfo.Email == "") {

            //set support 
            chatWith = sendTo.Name;
            chatWithName = sendTo.SupportName;
            //guest not regist
            $container.find("[box-support]").hide();
            $container.find("[box-register]").show();
            resize(255);
        } else {

            //guest registed
            chat.server.createwindow(myInfo, sendTo.Name);
        }
    } else {
        if(myInfo.UnsignName != sendTo.Name)
            chat.server.createwindow(myInfo, sendTo.Name); 
    }
}

function sendRegister(url) {
    var form = $('form#frRegister');

    //validate
    if (!form.valid()) {
        form.validate().focusInvalid();
        return;
    }
    if (!validateEmail($("#LiveChatUser_Email").val())) {
        alert("Email không đúng định dạng");
        return;
    }
    //submit 
    $.ajax({
        url: url,
        data: form.serialize(),
        type: "POST",
        success: function (data) {
            if (data.isSuccess) {
                var myInfo = setMyInfo(data.data);

                var time = setInterval(function () {
                    chat.server.createwindow(myInfo, chatWith);
                    if ($("[box-chatwindow]").length > 0) clearInterval(time);
                }, 100);
            }
        },
        error: function (jqXhr, errorCode, error) {

        }
    });
}
$(document).on('keypress', "[input-message]", function (e) {

    var parent = $(this).closest("[box-chatwindow]"),
        group = parent.attr("data-group-name"),
        content = $(this).val(),
        sendTo = parent.attr("data-sendto"),
        attachFile = parent.attr("data-attach"),
        attachFileName = parent.attr("data-attach-fname"),
        myInfo = getMyInfo();

    if ((e.keyCode || e.which) == 13) {
        $(this).val("");
        $("[box-chatwindow][data-group-name='" + group + "']").find(".attach-file-info").html("");
        if (content != "" || attachFile != "") {
            chat.server.chatprivate(content, attachFile, attachFileName, group, myInfo, sendTo);
            $("[box-chatwindow][data-group-name='" + group + "']").attr("data-attach", "");
        } else {
            alert("Nhập nội dung cần gửi !");
        }
    } else {
        //typing
        chat.server.typing(group, myInfo, sendTo);
    }
    stopBlinkParent();
});
function getMyInfo($container) {
    if (typeof ($container) == "undefined") $container = $("[live-chat-display]");
    return {
        Name: $container.attr("data-name"),
        Email: $container.attr("data-email"),
        IsSupport: $container.attr("data-issupport"),
        SupportName: $container.attr("data-supportname"),
        UnsignName: $container.attr("data-unsigname"),
        Avatar: $container.attr("data-avatar"),
        PhoneNumber: $container.attr("data-phone")
    };
}
function validateEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}
function setMyInfo(data) {
    if (typeof (data) == "string")
        data = JSON.parse(data);
    $("[live-chat-display]").attr("data-name", data.Name);
    $("[live-chat-display]").attr("data-supportname", data.SupportName);
    $("[live-chat-display]").attr("data-unsigname", data.UnsignName);
    $("[live-chat-display]").attr("data-issupport", data.IsSupport);
    $("[live-chat-display]").attr("data-email", data.Email);
    $("[live-chat-display]").attr("data-avatar", data.Avatar);
    $("[live-chat-display]").attr("data-phone", data.PhoneNumber);
    return data;
}
function bindMessage(container, data, sound, prepend) {
    sound = typeof (sound) == "undefined" ? true : sound;
    prepend = typeof (prepend) == "undefined" ? false : prepend;
    if (data != "") {
        var sender = typeof (data.sender) == "string" ? JSON.parse(data.sender) : data.sender,
            recievedInfo = typeof (data.recievedInfo) == "string" ? JSON.parse(data.recievedInfo) : data.recievedInfo,
            content = data.content,
            group = data.group,
            time = data.time;
        var recievedTemp = "<div class='direct-chat-msg'> <div class='direct-chat-info clearfix'> <span class='direct-chat-name pull-left'>{name}</span> <span class='direct-chat-timestamp pull-right time-ago' title='{time}'></span> </div> <img class='direct-chat-img' src='{avatar}' alt=''> <div class='direct-chat-text'> {content} </div> </div>";
        var senderTemp = "<div class='direct-chat-msg right'> <div class='direct-chat-info clearfix'> <span class='direct-chat-name pull-right'>{name}</span> <span class='direct-chat-timestamp pull-left time-ago' title='{time}'></span> </div> <img class='direct-chat-img' src='{avatar}' alt=''> <div class='direct-chat-text'> {content} </div> </div>";
        var message = "", myInfo = getMyInfo();
        if (myInfo.UnsignName == sender.UnsignName) {
            message = senderTemp.replace("{content}", content).replace("{time}", time).replace("{avatar}", sender.Avatar).replace("{name}", (sender.IsSupport ? sender.SupportName : sender.Name));
        } else {

            if (sound) {
                blinkParent("Có tin nhắn từ " + (sender.IsSupport ? sender.SupportName : sender.Name));
                playSound("/LiveChat/new_mess.mp3");
            }
                
            
            message = recievedTemp.replace("{avatar}", sender.Avatar)
                .replace("{content}", content)
                .replace("{time}", time)
                .replace("{name}", (sender.IsSupport ? sender.SupportName : sender.Name));
        }
        $(container).find(".direct-chat-msg.typing").remove();

        if (prepend)
            $(container).prepend(message);
        else
            $(container).append(message);
        $(".time-ago").timeago();
    }
}
var timeTyping = null;
function bindTyping(container, data) {
    if (data != "") {
        var sender = typeof (data.sender) == "string" ? JSON.parse(data.sender) : data.sender,
            recieved = data.recieved,
            group = data.group;
        var recievedTemp = "<div class='direct-chat-msg typing'> <div class='direct-chat-info clearfix'> <span class='direct-chat-name pull-left'>{name}</span> <span class='direct-chat-timestamp pull-right'></span> </div> <img class='direct-chat-img' src='{avatar}' alt=''> <div class='direct-chat-text'> <div class='bubble-container'><div class='_5pd7'></div><div class='_5pd7'></div><div class='_5pd7'></div></div> <span class='time time-ago' title=''></span> </div> </div>";
        var message = "", myInfo = getMyInfo();
        message = recievedTemp.replace("{avatar}", sender.Avatar)
            .replace("{name}", data.sender.IsSupport ? data.sender.SupportName : data.sender.Name);
        if ($(container).find(".direct-chat-msg.typing").length == 0) {
            $(container).append(message);
        }
        clearTimeout(timeTyping);
        timeTyping = setTimeout(function () {
            $(container).find(".direct-chat-msg.typing").remove();
        }, 2000);
        scrollBottom(group);
    }
}
function scrollBottom(group) {
    //Kéo xuống cuối
    $("[box-chatwindow][data-group-name='" + group + "']").find("[message-container]").stop().animate({
        scrollTop: $("[box-chatwindow][data-group-name='" + group + "']").find("[message-container]")[0].scrollHeight
    }, timeToBottom);
}


function attachFile(e) {
    var group = $(e).closest("[box-chatwindow]").data("group-name");
    $("#hdGroup").val(group);
    $("#fileInput").trigger("click");
}
function fileSelected(e) {
    var $form = $("form#frmAttach");
    var fdImage = new FormData();
    var fd = getFormSubmitData($form.get(0), fdImage);

    //submit
    AjaxNoprocessData("/front/LiveChat/AttachFile", fd, function (data) {
        if (data.isSuccess) {
            var fileName = data.data.fileName,
                filePath = data.data.filePath,
                group = data.data.group;
            $("[box-chatwindow][data-group-name='" + group + "']").find(".attach-file-info").html(fileName);
            $("[box-chatwindow][data-group-name='" + group + "']").attr("data-attach", filePath);
            $("[box-chatwindow][data-group-name='" + group + "']").attr("data-attach-fname", fileName);
        }
    });
}

function AjaxNoprocessData(url, data, callback) {
    $.ajax({
        url: url,
        data: data,
        type: "POST",
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            callback(data);
        },
        error: function (jqXhr, errorCode, error) {
        }
    });
}
//Đọc dữ liệu của form data để submit
function getFormSubmitData(form, fdAppend) {
    var f = $(form);
    var fd = new FormData(form);

    //xử lý tình huống nhập số
    for (var pair of fd.entries()) {
        var control = f.find('#' + pair[0].replace(".", "_"));
        fdAppend.append(pair[0], pair[1]);
    }

    return fdAppend;
}

function getHistory(group , pageIndex , pageSize , callback) {
    $.ajax({
        url: "/front/LiveChat/History",
        data: {
            group : group,
            pageIndex: pageIndex,
            pageSize: pageSize
        },
        type: "POST",
        success: function (data) {
            if (typeof (callback) == "function") callback(data);
        },
        error: function (jqXhr, errorCode, error) {

        }
    });
}
function playSound(sound) {
    try {
        var beep = new Audio(sound);
        beep.play();
    } catch (e) {
        console.log(e);
    }
}
function ScrollDiv(e) {
    var $self = $(e.target),
        $container = $self.closest("[box-chatwindow]"),
        top = $self.find(".direct-chat-msg").first().position().top,
        group = $container.attr("data-group-name"),
        pageindex = parseInt($container.attr("data-pageindex")),
        pagesize = $container.attr("data-pagesize"),
        historyInit = $container.attr("data-history-init");
    
    if (top >= 0) {
        pageindex++;
        if (!$container.hasClass("loading") && historyInit == "true") {
            !$container.addClass("loading")
            getHistory(group, pageindex, pagesize, function (data) {
                if (data.isSuccess) {
                    if (data.data.length > 0) {
                        data.data.forEach(function (item) {
                            item.time = moment(item.time).format("YYYY-MM-DD HH:mm:ss");
                            bindMessage($("[box-chatwindow][data-group-name='" + group + "']").find("[message-container]"), item, false,true);
                        });
                        $container.attr("data-pageindex", pageindex).removeClass("loading");
                    }
                }
            });
        }
    }

}
function LiveChatCollapse(e) {
    resize();
}
function LiveChatRemove(e) {
    resize();
}
/**************Message when embbed***************/

// send docHeight onload
function sendDocHeightMsg(e) {
    var mess = {
        action: "setHeight",
        height: 46
    }
    parent.postMessage(mess, '*');
}

// assign onload handler 
if (window.addEventListener) {
    window.addEventListener('load', sendDocHeightMsg, false);
} else if (window.attachEvent) { // ie8
    window.attachEvent('onload', sendDocHeightMsg);
    //$('.OpenWindow').attachEvent("click", setIframeSize, false);
}

function resize(h) {

    if (typeof (h) == "undefined") h = 373;
    var w = 330;

    w = ($("[live-chat-display]").find("[box-chatwindow],[box-support]").length) * w;
    var collapsedAll = true;
    
    $("[live-chat-display]").find("*[box-support],*[box-chatwindow]").each(function () {
        if (typeof ($(this).attr("collapsed-box")) == "undefined") collapsedAll = false;
    });
    if (collapsedAll) {
        $("[live-chat-display]").addClass("collapsed-all");
        h = 46;
    } else {
        $("[live-chat-display]").removeClass("collapsed-all");
    }

    var mess = {
        action: "setWidth",
        width: w,
        height: h
    }
    parent.postMessage(mess, '*');
}

function blinkParent(content) {
    var mess = {
        action: "blink",
        content: content
    }
    parent.postMessage(mess, '*');
}
function stopBlinkParent() {
    var mess = {
        action: "stop"
    }
    parent.postMessage(mess, '*');
}