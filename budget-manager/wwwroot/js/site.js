//Eventos para el sidebar responsive
$(".sidebar ul li").on('click', function () {
    $(".sidebar ul li.active").removeClass('active');
    $(this).addClass('active');
});

$('.open-btn').on('click', function () {
    $('.sidebar').addClass('active');

});


$('.close-btn').on('click', function () {
    $('.sidebar').removeClass('active');

})
//Activa la seccion actual
/*$(document).ready(function () {
    $('#ul_sidebar li').click(function () {
        $('#ul_sidebar li').removeClass('active');
        $(this).addClass('active');
    });
});*/