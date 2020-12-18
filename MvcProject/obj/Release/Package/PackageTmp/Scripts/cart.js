var $CartModal = $("#modalAddComplete");
var $QuickViewModal = $("#productQuickView");
var MyCart = {
    Init: function (options) {
        $(document).on("click", ".btnAddToCart", function () {
            var Id = $(this).closest(".product-item").data("id");
            $CartModal.html("<div id='loading'> <div></div> </div>").show();
            FeFunc.AjaxGet("front/Cart/Create?Id=" + Id, function (res) {
                $CartModal.html(res);
                $(".currency-format:not('.loaded')").each(function () {
                    var number = $(this).html();
                    $(this).html(formatCurrency(number)).addClass("loaded");
                });

                FeFunc.AjaxGet("/front/Cart/UpdateView", function (data) {
                    if (data.isSuccess) {
                        if (data.topCart != "") {
                            $("#view-top-cart").replaceWith(data.topCart);
                        }
                        if (data.topCartMobile != "") {
                            $("#view-top-cart-mobile").replaceWith(data.topCartMobile);
                        }
                        $(".currency-format:not('.loaded')").each(function () {
                            var number = $(this).html();
                            $(this).html(formatCurrency(number)).addClass("loaded");
                        });
                    } else {
                        console.log("có lỗi " + data.mess);
                    }
                });
            });
        });
        $(document).on("click", ".btnAddToCardQuickView", function () {
            var Id = $(this).closest(".product-info").data("id"),
                Number = $(this).closest(".product-info").find(".js-qty__num").val();
            $(this).closest(".product-info").find(".js-qty__num").val("1");
            FeFunc.AjaxGet("front/Cart/CreateNumber?Id=" + Id + "&Number=" + Number, function (res) {
                alert("Thêm vào giỏ hàng thành công !");
            });
        });
        $(document).on("click", ".btnQuickView", function () {
            var Id = $(this).closest(".product-item").data("id");
            $QuickViewModal.html("<div id='loading'> <div></div> </div>").show();
            FeFunc.AjaxGet("front/Cart/QuickView?Id=" + Id, function (res) {
                $QuickViewModal.html(res);
                $(".currency-format:not('.loaded')").each(function () {
                    var number = $(this).html();
                    $(this).html(formatCurrency(number)).addClass("loaded");
                });
                var owl = $('#p-sliderproduct .owl-carousel');
                owl.owlCarousel({
                    items: 3,
                    navigation: true,
                    navigationText: ["<svg class='svg-inline--fa fa-angle-left fa-w-8' aria-hidden='true' data-prefix='fa' data-icon='angle-left' role='img' xmlns='http://www.w3.org/2000/svg' viewBox='0 0 256 512' data-fa-i2svg=''><path fill='currentColor' d='M31.7 239l136-136c9.4-9.4 24.6-9.4 33.9 0l22.6 22.6c9.4 9.4 9.4 24.6 0 33.9L127.9 256l96.4 96.4c9.4 9.4 9.4 24.6 0 33.9L201.7 409c-9.4 9.4-24.6 9.4-33.9 0l-136-136c-9.5-9.4-9.5-24.6-.1-34z'></path></svg>", "<svg class='svg-inline--fa fa-angle-right fa-w-8' aria-hidden='true' data-prefix='fa' data-icon='angle-right' role='img' xmlns='http://www.w3.org/2000/svg' viewBox='0 0 256 512' data-fa-i2svg=''><path fill='currentColor' d='M224.3 273l-136 136c-9.4 9.4-24.6 9.4-33.9 0l-22.6-22.6c-9.4-9.4-9.4-24.6 0-33.9l96.4-96.4-96.4-96.4c-9.4-9.4-9.4-24.6 0-33.9L54.3 103c9.4-9.4 24.6-9.4 33.9 0l136 136c9.5 9.4 9.5 24.6.1 34z'></path></svg>"]
                });

                $('#p-sliderproduct .owl-carousel .owl-item').first().children('.item').addClass('active');
            });
        });
        $(document).on("click", "#modalAddComplete-close , .modalAddComplete-close", function () {
            $("#modalAddComplete").hide();
        });
        $(document).on("click", "#productQuickView .close", function () {
            $("#productQuickView").hide();
        });

    }
}
function updateCart(url) {
    var form = $('form#frCreate');

    //submit
    FeFunc.AjaxPost(url, form.serialize(), function (data) {
        if (data.isSuccess) {
            alert(data.mess);
            $("#cartTotal").html(data.data.TotalProductInCart);
            $("#total-incart").html(formatCurrency(data.data.TotalPriceInCart));
            for (var i = 0; i < data.data.ProductInCarts.length; i++) {
                $("[data-prd-id='" + data.data.ProductInCarts[i].Id + "']").find("#total-item").html(formatCurrency(data.data.ProductInCarts[i].TotalPrice));
            }
        } else {
            console.log("có lỗi " + data.data.mess);
        }
    });
}
function updateCartMutiView(url, formId) {
    var form = $(formId);

    //submit
    FeFunc.AjaxPost(url, form.serialize(), function (data) {
        if (data.isSuccess) {
            if (data.topCart != "") {
                $("#view-top-cart").replaceWith(data.topCart);
            }
            if (data.topCartMobile != "") {
                $("#view-top-cart-mobile").replaceWith(data.topCartMobile);
            }
            if (data.cartPage != "") {
                $(".cart-page-container").replaceWith(data.cartPage);
            }
            $(".currency-format:not('.loaded')").each(function () {
                var number = $(this).html();
                $(this).html(formatCurrency(number)).addClass("loaded");
            });
        } else {
            console.log("có lỗi " + data.mess);
        }
    });
}

function removeCart(url,that) {
    var Id = $(that).closest(".data-prd-id").data("prd-id");

    //submit
    FeFunc.AjaxPost(url, {Id : Id}, function (res) {
        $("#modalAddComplete").html(res);
        $(".currency-format:not('.loaded')").each(function () {
            var number = $(this).html();
            $(this).html(formatCurrency(number)).addClass("loaded");
        });
    });
}

function removeCart2(url, that) {
    var Id = $(that).closest(".cart-item").data("id");

    //submit
    FeFunc.AjaxPost(url, { Id: Id }, function (data) {
        if (data.isSuccess) {
            if (data.topCart != "") {
                $("#view-top-cart").replaceWith(data.topCart);
            }
            if (data.topCartMobile != "") {
                $("#view-top-cart-mobile").replaceWith(data.topCartMobile);
            }
            //if (data.cartPage != "") {
            //    $(".cart-page-container").replaceWith(data.cartPage);
            //}
            $('#top-cart').show("slow");
            $(".currency-format:not('.loaded')").each(function () {
                var number = $(this).html();
                $(this).html(formatCurrency(number)).addClass("loaded");
            });
        } else {
            console.log("có lỗi " + data.mess);
        }
    });
}
function changeQuantity(that,method,parent,input) {
    var $ele = $(that).closest(parent).find(input),
        quantity = parseInt($ele.val());
    switch (method) {
        case "add": {
            quantity = quantity + 1;
            break;
        }
        default: {
            if (quantity == 1) {
                break;
            }
            quantity = quantity - 1;
            break;
        }
    }
    $ele.val(quantity);
}

$(function () {
    MyCart.Init();
});