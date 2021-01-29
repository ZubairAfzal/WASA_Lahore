

var loopStartTime = 15000;
var video_loop = document.querySelector('#tubvelVideo-loop');


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

var timeoutVar;

startPump = () => {
    $('#startvideo').html('<img id="tubvelVideo" src="assets/videos/tubvel-animation.webp" alt="">');
    $('#lightsOff').addClass('lightsOn');
    $('.flow-meter').css('opacity', '1');

    $('.main-panel-lights span.green').removeClass('off');
    $('.main-panel-lights span.red').addClass('off');

    $('.small-panel-lights span.green').removeClass('off');
    $('.small-panel-lights span.red').addClass('off');
    
    $('.pressure-transmitter').css('opacity', '1');
    $('.btn-chlorine-drops-magnify').css('display', 'block');
    setTimeout(function(){
        if($('#startvideo').html() != ''){
            $('#tubvelVideo').remove();
            video_loop.style.display = 'block';
        }
    }, loopStartTime);
};

stopPump = () => {
    
    $('.main-panel-lights span.green').addClass('off');
    $('.main-panel-lights span.red').removeClass('off');

    $('.small-panel-lights span.green').addClass('off');
    $('.small-panel-lights span.red').removeClass('off');

    $('#lightsOff').removeClass('lightsOn');
    $('#startvideo').html("");
    $('.flow-meter').css('opacity', '0.3');
    $('.pressure-transmitter').css('opacity', '0.3');
    $('.btn-chlorine-drops-magnify').css('display', 'none');
    setTimeout(function(){
        $('#tubvelVideo').remove();
    }, 300);
    video_loop.style.display = 'none';
}

changeFlowMeterValue = (value) => {
    $('.flow-meter .flow-meter-inner .meter-nib')
        .css('transform', 'rotate(' + value + 'deg)');
}

changePressureTransmitterValue = (value) => {
    $('.pressure-transmitter .pressure-transmitter-inner .meter-nib')
        .css('transform', 'rotate(' + value + 'deg)');
}

$(document).ready(function(){

    $('.btn-chlorine-drops-magnify').mouseover(function(){
        $('#chlorine-drops').addClass('show');
    });

    $('.btn-chlorine-drops-magnify').mouseout(function(){
        $('#chlorine-drops').removeClass('show');
    });

    // Flow-meter controls
    // ------------------------------------------------------------------
    
    // First flow meter reading
    if( server_input <= 1 ) {
        current_reading = server_input * points_gap + points_gap;
        console.log(current_reading);
        changeFlowMeterValue(current_reading);
    } 
    
    // Second flow meter reading
    else if( server_input <= 2 ){
        current_reading = server_input * points_gap + points_gap;
        console.log(current_reading);
        changeFlowMeterValue(current_reading);
    }

    // Third flow meter reading
    else if( server_input <= 3 ){
        current_reading = server_input * points_gap + points_gap;
        console.log(current_reading);
        changeFlowMeterValue(current_reading);
    }

    // Forth flow meter reading
    else if( server_input <= 4 ){
        current_reading = server_input * points_gap + points_gap;
        console.log(current_reading);
        changeFlowMeterValue(current_reading);
    }

    // Fifth flow meter reading
    else if( server_input <= 5 ){
        current_reading = server_input * points_gap + points_gap;
        console.log(current_reading);
        changeFlowMeterValue(current_reading);
    }


    $('#btn-pump-start').click(function(){
        startPump();
    });

    $('#btn-pump-stop').click(function(){
        stopPump();
    });

    $('input[name="flow_meter_range"]').on( 'input', function(){
        changeFlowMeterValue($(this).val());
    });

    $('input[name="pressure_transmitter_range"]').on( 'input', function(){
        changePressureTransmitterValue($(this).val());
    });

    $('input[name="flow_meter_check"]').on('change', function(){
        if( $(this).is(":checked") ){
            $('.flow-meter-inner').addClass('show');
        }else{
            $('.flow-meter-inner').removeClass('show');
        }
    });

    $('input[name="pressure_transmitter_check"]').on('change', function(){
        if( $(this).is(":checked") ){
            $('.pressure-transmitter-inner').addClass('show');
        }else{
            $('.pressure-transmitter-inner').removeClass('show');
        }
    });


    /**
     * Priming tank status control
     */
    $('input[name="priming_tank_status"]').on('change', function(){
        if( $(this).is(":checked") ){
            $('#priming-tank-status').addClass('show');
        }else{
            $('#priming-tank-status').removeClass('show')
        }
    });

    /**
     * Chlorine tank status control
     */
    $('input[name="chlorine_tank_status"]').on('change', function(){
        if( $(this).is(":checked") ){
            $('#chlorine-tank-status').addClass('show');
        }else{
            $('#chlorine-tank-status').removeClass('show')
        }
    });

    /**
     * Room exaust status control
     */
    $('input[name="room_exaust_status"]').on('change', function(){
        if( $(this).is(":checked") ){
            $('#room-exaust-status').addClass('show');
        }else{
            $('#room-exaust-status').removeClass('show')
        }
    });

    /**
     * Motor fan status control
     */
    $('input[name="motor_status"]').on('change', function(){
        if( $(this).is(":checked") ){
            $('#motor-status-with-fan').addClass('show');
        }else{
            $('#motor-status-with-fan').removeClass('show')
        }
    });




});
