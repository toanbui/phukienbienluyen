$(function () {

    //Set language for select2
    $.fn.select2.defaults.set('language', {
        errorLoading: function () {
            return "Lỗi";
        },
        inputTooLong: function (args) {
            return "Dữ liệu nhập quá dài";
        },
        inputTooShort: function (args) {
            return "Dữ liệu nhập quá ngắn";
        },
        loadingMore: function () {
            return "Xem thêm";
        },
        maximumSelected: function (args) {
            return "Chọn quá giới hạn";
        },
        noResults: function () {
            return "Không tìm thấy dữ liệu";
        },
        searching: function () {
            return "Đang tìm kiếm";
        }
    });

    //<select class="col-sm-12" data-controller="Product" data-delete="DeleteProps" data-add="AddProps" data-id="@Model.Product.Id" data-selected="@(Json.Encode(Model.PropsOfProductIds))" data-create="@ViewBag.Create" data-hdinput="hdProps" data-placeholder="Chọn thuộc tính" init-select2="true" multiple="multiple">
    $("[init-select2='true']").each(function () {
        $(this).removeAttr("init-select2");

        //modal container
        var $container = $(this).closest("form");

        var data = $(this).data("selected");

        //params for add and delete middle table
        var params = {
            controller: $(this).data("controller"),
            delete: $(this).data("delete"),
            add: $(this).data("add"),
            id: $(this).data("id"),
            create: $(this).data("create"),
            hdinput: $(this).data("hdinput"),
            placeholder: $(this).data("placeholder"),
            select: this
        };

        if (typeof (params.placeholder) == "undefined")
            params.placeholder = "Nhấp để chọn";
        //init and set source
        $(this).select2({
            dropdownParent: $container,
            placeholder: params.placeholder,
        });
        $(this).val(data).trigger("change");

        //handler event when remove item
        $(this).on('select2:unselect', function (e) {
            var data = e.params.data;
            OnUnselect.call(this, data, params);
        });

        //handler event when add item
        $(this).on('select2:select', function (e) {
            var data = e.params.data;
            OnSelect.call(this, data, params);
        });
    });

    //Validate init 
    $.extend($.validator.messages, {
        required: "Trường này bắt buộc có",
    });

    //remove disable with input disable='false'
    $("input[disabled='false']").each(function () {
        $(this).removeAttr("disabled");
    });
});
function OnUnselect(data, params) {

    //for select2 normal no handler event
    if (typeof (params.id) == "undefined")
        return false;

    var url = ["/admin/", params.controller, "/", params.delete, "?id=" + params.id, "&deleteId=" + data.id].join('');
    if (params.create != "True") {
        //When edit item
        CmsFunc.AjaxGet(url, function (res) {
            console.log(res);
        });
    } else {
        var hdInputs = document.querySelectorAll("[id=" + params.hdinput + "]");
        if (hdInputs.length > 0) {
            for (var i = 0; i < hdInputs.length; i++) {
                if (hdInputs[i].value == data.id) {
                    $(hdInputs[i]).remove();
                }
            }
        }
    }

}
function OnSelect(data, params) {

    //for select2 normal no handler event
    if (typeof (params.id) == "undefined")
        return false;

    var url = ["/admin/", params.controller, "/", params.add, "?id=" + params.id, "&addId=" + data.id].join('');

    if (params.create != "True") {
        //When edit item
        CmsFunc.AjaxGet(url, function (res) {
            console.log(res);
        });
    } else {
        var $container = $(params.select).closest("form");
        var hdInputs = document.querySelectorAll("[id=" + params.hdinput + "]");
        if (hdInputs.length > 0) {
            var hasValue = false;
            for (var i = 0; i < hdInputs.length; i++) {
                if (hdInputs[i].value == data.id) {
                    hasValue = true;
                    break;
                }
            }
            if (!hasValue) {
                var addInput = "<input type='hidden' Id='{id}' name='{id}' value='{value}' />".replace(/{id}/g, params.hdinput).replace(/{value}/g, data.id);
                $container.append(addInput);
            }
        } else {
            var addInput = "<input type='hidden' Id='{id}' name='{id}' value='{value}' />".replace(/{id}/g, params.hdinput).replace(/{value}/g, data.id);
            $container.append(addInput);
        }
    }
}
