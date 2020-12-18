var toastr = {
    error: function (content, title) {
        notify(content,"bottom", "right", "", "inverse", "animated fadeInRight", "animated fadeOutRight","error");
    },
    success: function (content, title) {
        //console.log(title);
        //console.log(content);
        notify(content, "bottom", "right", "", "inverse", "animated fadeInRight", "animated fadeOutRight","success");
    },
}

function CKupdate() {
    var instance;
    for (instance in CKEDITOR.instances)
        CKEDITOR.instances[instance].updateElement();

}
function FlushCache() {
    CmsFunc.AjaxGet("/admin/CacheManager/Flush", function (data) {
        console.log(data);
    });
}
function dropOnChange(e) {
    var text = $(e).html(),
        val = $(e).data("value");
    $("#ddl-custom").find("button").html(text).attr("data-status",val);
}
function dropOnChange2(e) {
    var text = $(e).html(),
        val = $(e).data("value");
    $(e).closest("[data-dropdown='true']").find("button").html(text).attr("data-val", val);
}
function onSearch() {
    reloadGrid();
}

var CmsFunc = {};

CmsFunc.formatDate = function (date) {
    if (date == null || date == "" || typeof (date) == "undefined") return "";
    return moment(date).format("DD-MM-YYYY, h:mm:ss a");
}
CmsFunc.AjaxGet = function (url, callback) {
    $.ajax({
        url: url,
        type: "GET",
        success: function (res) {
            callback(res);
            //_lwfunc.bindingControl();
        },
        error: function (jqXhr, errorCode, error) {
            toastr.error("Lỗi không xác định " + jqXhr.status + " " + jqXhr.statusText, "Message!");
            closeLoading();
        }
    });
}
CmsFunc.AjaxPost = function (url, data, callback) {
    jQuery.ajax({
        url: url,
        data: data,
        type: "POST",
        success: function (data) {
            callback(data);
        },
        error: function (jqXhr, errorCode, error) {
            toastr.error("Lỗi không xác định " + jqXhr.status + " " + jqXhr.statusText, "Message!");
            closeLoading();
        }
    });
}
CmsFunc.AjaxNoprocessData = function (url, data, callback) {

    jQuery.ajax({
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
            toastr.error("Lỗi không xác định " + jqXhr.status + " " + jqXhr.statusText, "Message!");
            closeLoading();
        }
    });
}

CmsFunc.LogOut = function (url) {
    CmsFunc.AjaxPost(url, {}, function (res) {
        if (res.isSuccess === true) {
            toastr.success(res.mess, "Message!");

            if (window.location.pathname =="/admin/Account/AccessDenied") {
                window.location.href = "/admin";
            } else {
                window.location.reload();
            }
        } else {
            toastr.error(res.mess, "Message!");
        }
    });
}
CmsFunc.Thumb = function (value , w , h) {
    if (value == null || value == "" ) 
        return "";
    return ["/api/thumb?w=", w, "&h=", h, "&path=", value].join('');
}
function showLoading() {
    $('#ModalLoading').bPopup({
        modalClose: false,
        opacity: 0.6,
        positionStyle: 'fixed'
    });

}
function closeLoading() {
    $('#ModalLoading').bPopup().close();

}
function showModal(url) {
    showLoading();
    CmsFunc.AjaxGet(url, function (res) {
        closeLoading();
        $("#CreateModal").html(res).modal();
        CmsFunc.bindingControl();
    });
}
function changeStatus(url, id, status, message) {
    var temp = "<div class=\"modal-dialog\" role=\"document\" \"> <div class=\"modal-content\"> <div class=\"modal-header\"> <h5 class=\"modal-title\">Xác nhận</h5> <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\"> <span aria-hidden=\"true\">×</span> </button> </div> <div class=\"modal-body p-b-0\"> <div class=\"form-group row\"> <label class=\"col-sm-12 col-form-label\">{{message}}</label> </div> </div> <div class=\"modal-footer\"> <button type=\"button\" onclick=\"confirmChange('{{url}}' , '{{id}}' , '{{status}}')\" class=\"btn btn-primary\">Đồng ý</button> <button type=\"button\" class=\"btn btn-danger\" data-dismiss=\"modal\">Hủy</button> </div> </div> </div>"
        .replace("{{id}}", id)
        .replace("{{status}}", status)
        .replace("{{message}}", message)
        .replace("{{url}}", url);
    $("#CreateModal").html(temp).modal();
}
function confirmChange(url, id, status) {
    var data = {
        id: id,
        status: status
    }
    CmsFunc.AjaxPost(url, data, function (res) {
        if (res.isSuccess === true) {
            reloadGrid(); toastr.success(res.mess, "Message!");
            FlushCache();
        } else {
            toastr.error(res.mess, "Message!");
        }
        $("#CreateModal").modal('hide');
    });
}
function reloadGrid() {
    var oDataTable = $("#dt-ajax-array").dataTable();
    oDataTable.fnDraw(false);
}

function sendCreate(url) {
    var form = $('form#frCreate');

    //validate
    if (!form.valid()) {
        form.validate().focusInvalid();
        return;
    }
    CKupdate();

    //submit
    CmsFunc.AjaxPost(url, form.serialize(), function (data) {
        if (data.isSuccess === true) {
            $("#CreateModal").modal('hide');
            reloadGrid(); toastr.success(data.mess, "Message!");
            FlushCache();
        } else {
            toastr.error(data.mess, "Message!");
        }
    });
}
function sendDelete(url) {
    var form = $('form#frDelete');

    //submit
    CmsFunc.AjaxPost(url, form.serialize(), function (data) {
        if (data.isSuccess === true) {
            $("#CreateModal").modal('hide');
            reloadGrid(); toastr.success(data.mess, "Message!");
            FlushCache();
        } else {
            toastr.error(data.mess, "Message!");
        }
    });
}
CmsFunc.replaceAll = function (find, replace) {
    var str = this;
    return str.replace(new RegExp(find.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&'), 'g'), replace);
};
String.prototype.replaceAll = function (find, replace) {
    return this.replace(new RegExp(find.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&'), 'g'), replace);
};
//Lấy text hiển thị tương ứng giá trị của constant
CmsFunc.getConstantDisplay = function (constantName, value) {
    return typeof value !== 'undefined' && value !== null ? Resource.Constant[[constantName, value].join('_')] : "";
}

//Đọc dữ liệu của form data để submit
function getFormSubmitData(form , fdAppend) {
    var f = $(form);
    var fd = new FormData(form);

    //xử lý tình huống nhập số
    for (var pair of fd.entries()) {
        var control = f.find('#' + pair[0].replace(".", "_"));
        if (control.hasClass(numberControlCls)) {
            fd.set(pair[0], pair[1].replaceAll(numberSep, '').replaceAll(numberDec, _decimalSeperator));
            fdAppend.append(pair[0], pair[1].replaceAll(numberSep, '').replaceAll(numberDec, _decimalSeperator));
        } else {
            fdAppend.append(pair[0], pair[1]);
        }
        
    }

    return fdAppend;
}
function loadJs(jsLink, callback) {
    var head = document.getElementsByTagName('head')[0];
    var scriptEl = document.createElement("script");
    scriptEl.type = "text/javascript";
    if (typeof callback == "function") {
        scriptEl.onreadystatechange = scriptEl.onload = function () {
            callback();
        };
    }
    scriptEl.src = jsLink;
    head.appendChild(scriptEl);
}
//resource
var Resource = {
    //Các hằng số mapping text
    Constant: {
        RecordStatus_1: 'Nháp',
        RecordStatus_2: 'Chờ duyệt',
        RecordStatus_3: 'Hoạt động',
        RecordStatus_4: 'Ngừng hoạt động',
        RecordStatus_5: 'Khóa',
    },
    //Cấu hình format
    NumberConfig: {
        groupSeperator: '.',
        decimalSeperator: ',',
    },
    DataTableVnLang: {
        "sLengthMenu": "Hiển thị _MENU_ bản ghi trên 1 trang",
        "sZeroRecords": "Không tìm thấy dữ liệu",
        "sInfo": "Hiển thị _START_ tới _END_ của _TOTAL_ bản ghi",
        "sInfoEmpty": "Không có bản ghi nào",
        "sInfoFiltered": "(filtered from _MAX_ total records)",
        "oPaginate": {
            "sFirst": "<<",
            "sLast": ">>",
            "sNext": ">",
            "sPrevious": "<"
        }
    }
}

var numberDec = Resource.NumberConfig.decimalSeperator;
var numberSep = Resource.NumberConfig.groupSeperator;
var numberControlCls = "autonumber";
var amountDecimalPrecision = 4;

CmsFunc.bindingControl = function () {
    $('.ckeditor').each(function () {
        CKEDITOR.replace($(this).attr('Id'));
    });
    $('.int.' + numberControlCls).autoNumeric('init', { aDec: numberDec, aSep: numberSep, mDec: 0 });
}

$(function () {
    $(document).on("click", ".avatar-light-box", function () {
        var src = $(this).data("src");
        var img = ["<div class='modal-dialog modal-lg' role='document'><div class='modal-content'><div class='modal-body' style='display:flex;justify-content: center;'><img style='max-width:100%' src='", src, "'", " /></div></div></div>"].join('')
        $("#CreateModal").html(img).modal();
    });
});