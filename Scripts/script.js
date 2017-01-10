$(document).ready(function () {
    $(window).scroll(function () {
        console.log($(window).scrollTop());
        if ($(window).scrollTop() > 0) {
            $('#nav_bar').addClass('navbar-fixed');
        }
        if ($(window).scrollTop() < 1) {
            $('#nav_bar').removeClass('navbar-fixed');
        }
    });
    $('#start').click(function () {
        $('#titleTop').attr('top', '20px');
    });
});