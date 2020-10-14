var video = '';

$('input[name="start_tubwell"]').keyup(function () {
    var value = $(this).val();

    if (parseInt(value) === 1) {
        video = $('#start-tubewell');
        video.get(0).currentTime = 0;
        video.addClass('show');
        video.get(0).play();

        $('.main-panel-lights .green').removeClass('off');
        $('.main-panel-lights .red').addClass('off');

        $('.small-panel-lights .green').removeClass('off');
        $('.small-panel-lights .red').addClass('off');

        $('.flow-meter').addClass('op-1');
        $('.pressure-transmitter').addClass('op-1');

    } else {
        video = $('#start-tubewell');
        video.get(0).currentTime = 0;
        video.removeClass('show');

        video = $('#tubewell-seamless');
        video.get(0).currentTime = 0;
        video.removeClass('show');

        $('.main-panel-lights .green').addClass('off');
        $('.main-panel-lights .red').removeClass('off');

        $('.small-panel-lights .green').addClass('off');
        $('.small-panel-lights .red').removeClass('off');

        $('.flow-meter').removeClass('op-1');
        $('.pressure-transmitter').removeClass('op-1');

        video.get(0).pause();
    }


});



//$('input[name="empty_chlorine_tank"]').keyup(function () {
//    var is_filled = $(this).val();

//    if (parseInt(is_filled) === 0) {
//        $('.empty-chlorine-tank').addClass('show');
//    } else {
//        $('.empty-chlorine-tank').removeClass('show');
//    }

//});

//$('input[name="empty_priming_tank"]').keyup(function () {
//    var is_filled = $(this).val();

//    if (parseInt(is_filled) === 0) {
//        $('.empty-priming-tank').addClass('show');
//    } else {
//        $('.empty-priming-tank').removeClass('show');
//    }

//});

//$('input[name="room_exaust"]').keyup(function () {
//    var is_running = $(this).val();

//    if (parseInt(is_running) === 0) {
//        $('.room-exaust').addClass('show');
//    } else {
//        $('.room-exaust').removeClass('show');
//    }

//});

//$('input[name="no_water_supply"]').keyup(function () {
//    var is_running = $(this).val();

//    if (parseInt(is_running) === 0) {
//        $('.no-water-supply').addClass('show');
//    } else {
//        $('.no-water-supply').removeClass('show');
//    }

//});


// First angle value of equal 5 parts reading
var points_gap = 45;

// Starting angle where value should be zero
var starting_val = 45;

// Ending angle where maximum value will be 5
var ending_val = 315;

// Current value for meter reading
var current_reading = 0;

// Current reading from server (1 to 5)
var server_input = 2.34;


changeFlowMeterValue = (value) => {
    $('.flow-meter .flow-meter-inner .meter-nib')
        .css('transform', 'rotate(' + value + 'deg)');
};

changePressureTransmitterValue = (value) => {
    $('.pressure-transmitter .pressure-transmitter-inner .meter-nib')
        .css('transform', 'rotate(' + value + 'deg)');
};


// Flow-meter controls
// ------------------------------------------------------------------

// First flow meter reading
if (server_input <= 1) {
    current_reading = server_input * points_gap + points_gap;
    console.log(current_reading);
    changeFlowMeterValue(current_reading);
}

// Second flow meter reading
else if (server_input <= 2) {
    current_reading = server_input * points_gap + points_gap;
    console.log(current_reading);
    changeFlowMeterValue(current_reading);
}

// Third flow meter reading
else if (server_input <= 3) {
    current_reading = server_input * points_gap + points_gap;
    console.log(current_reading);
    changeFlowMeterValue(current_reading);
}

// Forth flow meter reading
else if (server_input <= 4) {
    current_reading = server_input * points_gap + points_gap;
    console.log(current_reading);
    changeFlowMeterValue(current_reading);
}

// Fifth flow meter reading
else if (server_input <= 5) {
    current_reading = server_input * points_gap + points_gap;
    console.log(current_reading);
    changeFlowMeterValue(current_reading);
}