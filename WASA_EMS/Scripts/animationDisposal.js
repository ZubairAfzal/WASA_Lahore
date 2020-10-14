var video = '';
$('input[name="toggle_flow"]').keyup(function(){
    var value = $(this).val();

    if(parseInt(value) === 1 ) {
         video = $('#top-view-seamless');
        video.get(0).currentTime = 0;
        video.addClass('show');
        $('#seamless-stop-flow').removeClass('show');
        video.get(0).play();
    }else {
        video = $('#seamless-stop-flow');
        video.get(0).currentTime = 0;
        video.addClass('show');
        $('#top-view-seamless').removeClass('show');
        video.get(0).play();
    }

});

$('input[name="toggle_hints"]').keyup(function() {
    var value = $(this).val();

    if(parseInt(value) === 1) {
        $('.main-scene-elements').addClass('show');
    }else {
        $('.main-scene-elements').removeClass('show');
    }
});


$('input[name="well_1"]').change(function() {
    var value = $(this).val();

    $('.well-1').html(value);
});


$('input[name="well1_status"]').change(function(){
    if($(this).prop('checked') === false) {
        $('.well-1').addClass('off-state');
    }else {
        $('.well-1').removeClass('off-state');
    }
});