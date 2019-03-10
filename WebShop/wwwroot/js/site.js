$(document).ready(function () {
    var scrollTop = 0;
    $(window).scroll(function () {
        scrollTop = $(window).scrollTop();
        if (scrollTop >= 30) {
            $('.navbar-fixed-top').addClass('scrolled');
        } else if (scrollTop < 30) {
            $('.navbar-fixed-top').removeClass('scrolled');
        }
    });
});
