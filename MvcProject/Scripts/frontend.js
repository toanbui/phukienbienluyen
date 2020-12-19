var FeFunc = {};
FeFunc.AjaxPost = function (url, data, callback) {
    jQuery.ajax({
        url: url,
        data: data,
        type: "POST",
        success: function (data) {
            callback(data);
        },
        error: function (jqXhr, errorCode, error) {

        }
    });
}
FeFunc.AjaxGet = function (url, callback) {
    $.ajax({
        url: url,
        type: "GET",
        success: function (res) {
            callback(res);
        },
        error: function (jqXhr, errorCode, error) {
        }
    });
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
function numberWithCommas(x) {
    x = x.toString();
    var pattern = /(-?\d+)(\d{3})/;
    while (pattern.test(x))
        x = x.replace(pattern, "$1.$2");
    return x;
}
function formatCurrency(number) {
    if (number == "") return;
    return numberWithCommas(number) + " ₫";
}
function doAnimations(elements) {
    var animationEndEvents = 'webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend';
    elements.each(function () {
        var $this = $(this);
        var $animationDelay = $this.data('delay');
        var $animationType = 'animated ' + $this.data('animation');
        $this.css({
            'animation-delay': $animationDelay,
            '-webkit-animation-delay': $animationDelay
        });
        $this.addClass($animationType).one(animationEndEvents, function () {
            $this.removeClass($animationType);
        });
    });
}
$(function () {
    new WOW().init();
    //format currency
    $(".currency-format:not('.loaded')").each(function () {
        var number = $(this).html();
        $(this).html(formatCurrency(number)).addClass("loaded");
    });

    //slider init 

    $('#hero-slider').on('init', function (e, slick) {
        var $firstAnimatingElements = $('div.hero-slide:first-child').find('[data-animation]');
        doAnimations($firstAnimatingElements);
    });
    $('#hero-slider').on('beforeChange', function (e, slick, currentSlide, nextSlide) {
        var $animatingElements = $('div.hero-slide[data-slick-index="' + nextSlide + '"]').find('[data-animation]');
        doAnimations($animatingElements);
    });
    $('#hero-slider').slick({
        autoplay: true,
        autoplaySpeed: 10000,
        dots: true,
        fade: true
    });

    //slide tin
    $("#owl-home-articles-slider").owlCarousel({
        items: 2,
        itemsDesktop: [1000, 2],
        itemsDesktopSmall: [900, 2],
        itemsTablet: [768, 2],
        itemsMobile: [480, 1],
        navigation: true,
        pagination: false,
        slideSpeed: 1000,
        paginationSpeed: 1000,
        navigationText: ['<i class="fa fa-angle-left" aria-hidden="true"></i>', '<i class="fa fa-angle-right" aria-hidden="true"></i>']
    });

    var offset = 220;
    var duration = 500;
    $(window).scroll(function () {
        if (jQuery(this).scrollTop() > offset) {
            jQuery('#back-to-top').fadeIn(duration);
        } else {
            jQuery('#back-to-top').fadeOut(duration);
        }
    });

    $('#back-to-top').click(function (event) {
        event.preventDefault();
        jQuery('html, body').animate({
            scrollTop: 0
        }, duration);
        return false;
    });

    window.onscroll = changePos;

    document.addEventListener("touchmove", processScroll, false);
    document.addEventListener("scroll", processScroll, false);
});
var pageProduct = 1;
function processScroll() {
    var scrollTop = $(window).scrollTop();
    if ($("#product-load-more").length > 0) {
        var ajaxTop = $("#product-load-more").offset().top;
        if (scrollTop >= (ajaxTop - 768)) {
            var $ajaxContainer = $("#product-ajax");
            if (!$("#product-ajax").hasClass("loading")) {
                $ajaxContainer.addClass("loading");
                pageProduct++;
                $.ajax({
                    url: "/front/Home/AjaxProduct",
                    type: "Post",
                    data: {
                        pageIndex: pageProduct,
                        pageSize: 12
                    },
                    success: function (data) {
                        if (data != "") {
                            if (typeof (data.isSuccess) != "undefined" && !data.isSuccess) {
                                return false;
                            }
                            var html = $ajaxContainer.append(data).removeClass("loading");
                            setTimeout(function () {
                                $(html).find('.fade-out').removeClass('fade-out');
                                $(".currency-format:not('.loaded')").each(function () {
                                    var number = $(this).html();
                                    $(this).html(formatCurrency(number)).addClass("loaded");
                                });
                            }, 200);
                        }
                    },
                    error: function () {
                        pageProduct = pageProduct - 1;
                    }
                });
            }
        }
    }
    if ($("#product-list-load-more").length > 0) {
        var ajaxListTop = $("#product-list-load-more").offset().top;
        var zoneId = $("#ZoneId").val();
        if (scrollTop >= (ajaxListTop - 400)) {
            var $ajaxContainer = $("#product-ajax");
            if (!$("#product-ajax").hasClass("loading")) {
                $ajaxContainer.addClass("loading");
                pageProduct++;
                $.ajax({
                    url: "/front/List/AjaxProduct",
                    type: "Post",
                    data: {
                        pageIndex: pageProduct,
                        pageSize: 12,
                        zoneId : zoneId
                    },
                    success: function (data) {
                        if (data != "") {
                            if (typeof (data.isSuccess) != "undefined" && !data.isSuccess) {
                                return false;
                            }
                            var html = $ajaxContainer.append(data).removeClass("loading");
                            setTimeout(function () {
                                $(html).find('.fade-out').removeClass('fade-out');
                                $(".currency-format:not('.loaded')").each(function () {
                                    var number = $(this).html();
                                    $(this).html(formatCurrency(number)).addClass("loaded");
                                });
                            }, 200);
                        }
                    },
                    error: function () {
                        pageProduct = pageProduct - 1;
                    }
                });
            }
        }
    }
}
function changePos() {
    var header = $("#header");
    var headerheight = $("#header").height();
    if (window.pageYOffset > headerheight) {
        header.addClass('scrolldown');
    } else {
        header.removeClass('scrolldown');
    }
}
function toggleMobileNav() {
    $("body").toggleClass("js-drawer-open")
    $("body").toggleClass("js-drawer-open-right")
}