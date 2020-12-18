



var twf = "";
var twf7 = "";
var twf30 = "";
var cr = "";
var pr = "";
var pl = "";
var vm = "";
var twha = "";
var twha7 = "";
var twha30 = "";

twha = "2";
twha7 = "56";
twha30 = "220";
twf = "10";
twf7 = "20";
twf30 = "30";
cr = "0.5";
pr = "0.75";
pl = "0";
vm = "0.3";
// $("#IV").text(e.target.textContent);
$("#CL").html("&#9989");
$("#LS1").html("&#128308");
$("#VT2").html("&#9889");
$("#IT2").html("&#9889");
function toggleClass(elem, className) {
    if (elem.className.indexOf(className) !== -1) {
        elem.className = elem.className.replace(className, '');
    }
    else {
        elem.className = elem.className.replace(/\s+/g, ' ') + ' ' + className;
    }

    return elem;
}

function toggleDisplay(elem) {
    const curDisplayStyle = elem.style.display;

    if (curDisplayStyle === 'none' || curDisplayStyle === '') {
        elem.style.display = 'block';
    }
    else {
        elem.style.display = 'none';
    }

}

function toggleMenuDisplay(e) {
    const dropdown = e.currentTarget.parentNode;
    const menu = dropdown.querySelector('.menu');
    const icon = dropdown.querySelector('.fa-angle-right');

    toggleClass(menu, 'hide');
    toggleClass(icon, 'rotate-90');
}

function handleOptionSelected(e) {
    toggleClass(e.target.parentNode, 'hide');

    const id = e.target.id;
    const newValue = e.target.textContent + ' ';
    const titleElem = document.querySelector('.dropdown .title');
    const icon = document.querySelector('.dropdown .title .fa');


    titleElem.textContent = newValue;
    titleElem.appendChild(icon);

    //trigger custom event
    document.querySelector('.dropdown .title').dispatchEvent(new Event('change'));
    //setTimeout is used so transition is properly shown
    setTimeout(() => toggleClass(icon, 'rotate-90', 0));
}
//    handleTitleChange(e);
// last 30 and 7 current a3
function l30a() {

    //el_up.innerHTML = JSON.stringify(array);

    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("LI30");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetI30",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
function l7a() {
    var el_down = document.getElementById("LI7");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetI7",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
// last 30 and 7 current c2
function l30c() {

    //el_up.innerHTML = JSON.stringify(array);

    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("LI30");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetI30C",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
function l7c() {
    var el_down = document.getElementById("LI7");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetI7C",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}

// last 30 and 7 current d2
function l30d() {

    //el_up.innerHTML = JSON.stringify(array);

    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("LI30");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetI30D",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
function l7d() {
    var el_down = document.getElementById("LI7");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetI7D",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}

// last 30 and 7 current E
function l30e() {

    //el_up.innerHTML = JSON.stringify(array);

    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("LI30");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetI30E",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
function l7e() {
    var el_down = document.getElementById("LI7");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetI7E",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}

// last 30 and 7 current camp
function l30cp() {

    //el_up.innerHTML = JSON.stringify(array);

    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("LI30");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetI30cp",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
function l7cp() {
    var el_down = document.getElementById("LI7");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetI7cp",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}


// last 30 and 7 current F
function l30F() {

    //el_up.innerHTML = JSON.stringify(array);

    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("LI30");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetI30F",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
function l7F() {
    var el_down = document.getElementById("LI7");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetI7F",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}

// last 30 and 7 current J
function l30J() {

    //el_up.innerHTML = JSON.stringify(array);

    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("LI30");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetI30J",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
function l7J() {
    var el_down = document.getElementById("LI7");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetI7J",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}

// LAST 30 AND 7 DAYS VOLATGEBG A3
function V30a() {

    //el_up.innerHTML = JSON.stringify(array);

    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("LV30");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV30",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
function V7a() {

    var el_down = document.getElementById("LV7");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV7",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}

// LAST 30 AND 7 DAYS VOLATGEBG C2
function V30c() {

    //el_up.innerHTML = JSON.stringify(array);

    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("LV30");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV30C",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
function V7c() {

    var el_down = document.getElementById("LV7");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV7C",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}

// LAST 30 AND 7 DAYS VOLATGEBG D
function V30d() {

    //el_up.innerHTML = JSON.stringify(array);

    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("LV30");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV30D",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
function V7d() {

    var el_down = document.getElementById("LV7");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV7D",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}

// LAST 30 AND 7 DAYS VOLATGEBG E
function V30e() {

    //el_up.innerHTML = JSON.stringify(array);

    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("LV30");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV30E",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
function V7e() {

    var el_down = document.getElementById("LV7");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV7E",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}

// LAST 30 AND 7 DAYS VOLATGEBG camp
function V30cp() {

    //el_up.innerHTML = JSON.stringify(array);

    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("LV30");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV30cp",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
function V7cp() {

    var el_down = document.getElementById("LV7");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV7cp",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}

// LAST 30 AND 7 DAYS VOLATGEBG F
function V30F() {

    //el_up.innerHTML = JSON.stringify(array);

    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("LV30");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV30F",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
function V7F() {

    var el_down = document.getElementById("LV7");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV7F",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}

// LAST 30 AND 7 DAYS VOLATGEBG J
function V30J() {

    //el_up.innerHTML = JSON.stringify(array);

    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("LV30");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV30J",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
function V7J() {

    var el_down = document.getElementById("LV7");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV7J",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}

// Today Avg V a3
function TV() {
    var el_down = document.getElementById("LVT");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetVmax",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
// Today Avg I a3
function TI() {
    var el_down = document.getElementById("LIT");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetDataI",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}

// Today Avg V c2
function TVC() {
    var el_down = document.getElementById("LVT");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetVmaxC",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
// Today Avg I c2
function TIC() {
    var el_down = document.getElementById("LIT");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetDataIC",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}

// Today Avg V d2
function TVD() {
    var el_down = document.getElementById("LVT");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetVmaxD",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
// Today Avg I d2
function TID() {
    var el_down = document.getElementById("LIT");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetDataID",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}

// Today Avg V e1
function TVE() {
    var el_down = document.getElementById("LVT");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetVmaxE",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
// Today Avg I e1
function TIE() {
    var el_down = document.getElementById("LIT");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetDataIE",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}

// Today Avg V camp
function TVcp() {
    var el_down = document.getElementById("LVT");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetVmaxcp",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
// Today Avg I camp
function TIcp() {
    var el_down = document.getElementById("LIT");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetDataIcp",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}

// Today Avg V F
function TVF() {
    var el_down = document.getElementById("LVT");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetVmaxF",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
// Today Avg I F
function TIF() {
    var el_down = document.getElementById("LIT");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetDataIF",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}

// Today Avg V J
function TVJ() {
    var el_down = document.getElementById("LVT");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetVmaxJ",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}
// Today Avg I J
function TIJ() {
    var el_down = document.getElementById("LIT");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetDataIJ",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            av = 0;
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            av = total / markers.length;
            el_down.innerHTML = Math.round(av * 10) / 10;

            // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
        }
    });
}

function C2() {
    var F = [];
    var Fre = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetF",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            F = eval(resp.d);
            for (i = 0; i < F.length; i++) {


                const r0 = F[0].y;
                const r1 = F[1].y;
                const r2 = F[2].y;
                const r3 = F[3].y;
                const r4 = F[4].y;
                const r5 = F[5].y;
                const r6 = F[6].y;
                const r7 = F[7].y;
                const r8 = F[8].y;
                const r9 = F[9].y;
                Fre = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKW
    var PKW = [];
    var PKW1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKW",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            PKW = eval(resp.d);
            for (i = 0; i < PKW.length; i++) {


                const r0 = PKW[0].y;
                const r1 = PKW[1].y;
                const r2 = PKW[2].y;
                const r3 = PKW[3].y;
                const r4 = PKW[4].y;
                const r5 = PKW[5].y;
                const r6 = PKW[6].y;
                const r7 = PKW[7].y;
                const r8 = PKW[8].y;
                const r9 = PKW[9].y;
                PKW1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKVA

    var PKVAR = [];
    var PKVAR1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKVAR",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            PKVAR = eval(resp.d);
            for (i = 0; i < PKVAR.length; i++) {


                const r0 = PKVAR[0].y;
                const r1 = PKVAR[1].y;
                const r2 = PKVAR[2].y;
                const r3 = PKVAR[3].y;
                const r4 = PKVAR[4].y;
                const r5 = PKVAR[5].y;
                const r6 = PKVAR[6].y;
                const r7 = PKVAR[7].y;
                const r8 = PKVAR[8].y;
                const r9 = PKVAR[9].y;
                PKVAR1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKVA

    var PKVA = [];
    var PKVA1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKVA",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            //  PKVA = resp.d;
            PKVA = eval(resp.d);
            for (i = 0; i < PKVA.length; i++) {


                const r0 = PKVA[0].y;
                const r1 = PKVA[1].y;
                const r2 = PKVA[2].y;
                const r3 = PKVA[3].y;
                const r4 = PKVA[4].y;
                const r5 = PKVA[5].y;
                const r6 = PKVA[6].y;
                const r7 = PKVA[7].y;
                const r8 = PKVA[8].y;
                const r9 = PKVA[9].y;
                PKVA1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    // Voltage
    var V1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV1",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            V1 = resp.d;
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    var V = [];
    var cur = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetData2",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            V = eval(resp.d);
            for (i = 0; i < V.length; i++) {


                const r0 = V[0].y;
                const r1 = V[1].y;
                const r2 = V[2].y;
                const r3 = V[3].y;
                const r4 = V[4].y;
                const r5 = V[5].y;
                const r6 = V[6].y;
                const r7 = V[7].y;
                const r8 = V[8].y;
                const r9 = V[9].y;
                cur = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(peval(resp.d);
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //


    var markers1 = [];
    var ty = [];
    var tx = [];

    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV1A3",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            // markers = resp.d;
            markers1 = eval(resp.d);

            for (i = 0; i < markers1.length; i++) {
                //ty = markers1[i].x;
                //  ty = new Date(markers1[i].x).toLocaleDateString("en-US");
                const d0 = new Date(markers1[0].x);
                const d1 = new Date(markers1[1].x);
                const d2 = new Date(markers1[2].x);
                const d3 = new Date(markers1[3].x);
                const d4 = new Date(markers1[4].x);
                const d5 = new Date(markers1[5].x);
                const d6 = new Date(markers1[6].x);
                const d7 = new Date(markers1[7].x);
                const d8 = new Date(markers1[8].x);
                const d9 = new Date(markers1[9].x);
                ty = [
                    d9.getHours() + ":" + d9.getMinutes(), d8.getHours() + ":" + d8.getMinutes(), d7.getHours() + ":" + d7.getMinutes(), d6.getHours() + ":" + d6.getMinutes(),
                    d5.getHours() + ":" + d5.getMinutes(), d4.getHours() + ":" + d4.getMinutes(), d3.getHours() + ":" + d3.getMinutes(), d2.getHours() + ":" + d2.getMinutes(),
                    d1.getHours() + ":" + d1.getMinutes(), d0.getHours() + ":" + d0.getMinutes()
                ];

                const t0 = markers1[0].y;
                const t1 = markers1[1].y;
                const t2 = markers1[2].y;
                const t3 = markers1[3].y;
                const t4 = markers1[4].y;
                const t5 = markers1[5].y;
                const t6 = markers1[6].y;
                const t7 = markers1[7].y;
                const t8 = markers1[8].y;
                const t9 = markers1[9].y;
                tx = [t0, t1, t2, t3, t4, t5, t6, t7, t8, t9];
            }



            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }

    });
    console.log(ty);

    var canvas = document.getElementById('canvas1');
    var ctx = canvas.getContext('2d');

    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            //labels: JSON.stringify(dps),
            labels: ty,
            datasets: [{
                label: "V",
                data: tx,
                fill: false,
                backgroundColor: 'green',
                borderColor: 'green'
                // color: "#878BB6",

            }, {
                label: "I",
                data: cur,
                fill: false,
                backgroundColor: 'red',
                borderColor: 'red'
                //color: "#4ACAB4",

            },
            {
                label: "F",
                data: Fre,
                fill: false,
                backgroundColor: '#FF5733',
                borderColor: '#FF5733'
                //color: "#4ACAB4",
            },

            {
                label: "PKW",
                data: PKW1,
                fill: false,
                backgroundColor: '#0066cc',
                borderColor: '#0066cc'
                //color: "#4ACAB4",
            },
            {
                label: "PKVAR",
                data: PKVAR1,
                fill: false,
                backgroundColor: '#993399',
                borderColor: '#993399'
                //color: "#4ACAB4",
            },
            {
                label: "PKVA",
                data: PKVA1,
                fill: false,
                backgroundColor: '#333399',
                borderColor: '#333399'
                //color: "#4ACAB4",
            }
            ]
        },
        /*  options: {

xAxes: [{
type: 'time',
                  time: {
format: "HH:MM:SS",

}

}],


              /* xAxes: [{
type: 'time',
                   time: {

unit: 'second',
tooltipFormat: 'h:mm:ss a'
}
}],*/

        // }
        options: {
            maintainAspectRatio: false,
            responsive: true,
            title: {
                display: true,
                position: 'top',
                text: 'Power Telemetry C-II Block'
            },
            legend: {
                display: true,
                position: 'bottom'
            },
            tooltips: {
                mode: 'index'
            },


            xAxes: [{
                type: 'time',
                time: {
                    unit: 'second',
                    displayFormats: {
                        second: 'h:mm:ss a'
                    },
                    tooltipFormat: 'h:mm:ss a'
                },
                /*type: 'time',
                                        time: {
                        unit: 'second',
                    tooltipFormat: 'h:mm:ss a'
                }*/
            }],

        }

    });
    /*  var F = [];
                  $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetF",
                      data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                      success: function (resp) {
            F = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKW
    var PKW = [];
                  $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetPKW",
                      data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                      success: function (resp) {
            PKW = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKVA
    
    var PKVAR = [];
                  $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetPKVAR",
                      data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                      success: function (resp) {
            PKVAR = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKVA
    
    var PKVA = [];
                  $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetPKVA",
                      data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                      success: function (resp) {
            PKVA = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    // Voltage
    var V1 = [];
                  $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetV1",
                      data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                      success: function (resp) {
            V1 = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    // Current
    
    var p;
    
    
    var markers = [];
                  $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetData2",
                      data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                      success: function (resp) {
            markers = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    
    //console.log(markers);
    var dps = [];
                  $.each(JSON.parse(markers), function (i, item) {
            dps.push({ x: item.X, y: item.y });
        //alert(dps[0]);
        });
        console.log(dps);
                 // var chart = new CanvasJS.Chart("chartContainer1", {
            backgroundColor: "transparent",
        animationEnabled: true,
                      title: {
            text: "Power Telemetry"
    },
                      axisX: {
            //valueFormatString:  "hh:mm TT DD-MM-YYYY"
        },
                      axisY2: {
            title: "Values",
        //refix: "$",
        //suffix: "K"
    },
                      toolTip: {
            shared: true
    },
                      legend: {
            //cursor: "pointer",
            //verticalAlign: "top",
            //horizontalAlign: "center",
            //dockInsidePlotArea: true,
            //itemclick: toogleDataSeries
        },
        data: [
                          {
            type: "line",
        axisYType: "secondary",
        name: "I",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(markers)
    
    },
                          {
            type: "line",
        axisYType: "secondary",
        name: "V",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(V1)
    
    },
                          {
            type: "line",
        axisYType: "secondary",
        name: "PKVA",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKVA)
    
    },
                          {
            type: "line",
        axisYType: "secondary",
        name: "PKVAR",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKVAR)
                              //  [{"x": 1601026800000.0, "y": 90.398857 }, {"x": 1601026740000.0, "y": 90.400246 }, {"x": 1601026680000.0, "y": 90.03231 }, {"x": 1601026560000.0, "y": 89.822952 }, {"x": 1601026500000.0, "y": 90.096573 }, {"x": 1601026440000.0, "y": 89.846809 }, {"x": 1601026380000.0, "y": 89.768143 }, {"x": 1601026320000.0, "y": 89.917648 }, {"x": 1601026260000.0, "y": 89.751648 }, {"x": 1601026140000.0, "y": 89.838318 }]
        //JSON.parse(markers)
                              // [{"x": 1601018460000.0, "y": 85.26503 }, {"x": 1601018400000.0, "y": 85.125191 }, {"x": 1601018340000.0, "y": 85.387268 }, {"x": 1601018280000.0, "y": 85.602837 }, {"x": 1601018160000.0, "y": 85.699348 }, {"x": 1601018100000.0, "y": 85.701126 }, {"x": 1601018040000.0, "y": 85.631393 }, {"x": 1601017980000.0, "y": 85.547073 }, {"x": 1601017920000.0, "y": 84.770912 }, {"x": 1601017800000.0, "y": 83.743973 }]
        //    data: markers
        //}
        //JSON.parse(markers)
                              //[{"x": 1601012100000.0, "y": 0.0 }, {"x": 1601012040000.0, "y": 0.0 }, {"x": 1601011980000.0, "y": 0.0 }, {"x": 1601011920000.0, "y": 0.0 }, {"x": 1601011860000.0, "y": 0.0 }, {"x": 1601011800000.0, "y": 0.0 }, {"x": 1601011680000.0, "y": 0.0 }, {"x": 1601011620000.0, "y": 0.0 }, {"x": 1601011560000.0, "y": 0.0 }, {"x": 1601011500000.0, "y": 0.0 }]
                              //    [{
            //    data: markers
            //}
            //]
        },
                          {
            type: "line",
        axisYType: "secondary",
        name: "PKW",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKW)
    
    },
                          {
            type: "line",
        axisYType: "secondary",
        name: "Frequency",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(F)
    
    }
    ]
    });
    chart.render();*/
}
/// function A3
function C1() {
    var F = [];
    var Fre = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetF3",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            F = eval(resp.d);
            for (i = 0; i < F.length; i++) {


                const r0 = F[0].y;
                const r1 = F[1].y;
                const r2 = F[2].y;
                const r3 = F[3].y;
                const r4 = F[4].y;
                const r5 = F[5].y;
                const r6 = F[6].y;
                const r7 = F[7].y;
                const r8 = F[8].y;
                const r9 = F[9].y;
                Fre = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKW
    var PKW = [];
    var PKW1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKWa3",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            PKW = eval(resp.d);
            for (i = 0; i < PKW.length; i++) {


                const r0 = PKW[0].y;
                const r1 = PKW[1].y;
                const r2 = PKW[2].y;
                const r3 = PKW[3].y;
                const r4 = PKW[4].y;
                const r5 = PKW[5].y;
                const r6 = PKW[6].y;
                const r7 = PKW[7].y;
                const r8 = PKW[8].y;
                const r9 = PKW[9].y;
                PKW1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKVA

    var PKVAR = [];
    var PKVAR1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKVARA3",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            PKVAR = eval(resp.d);
            for (i = 0; i < PKVAR.length; i++) {


                const r0 = PKVAR[0].y;
                const r1 = PKVAR[1].y;
                const r2 = PKVAR[2].y;
                const r3 = PKVAR[3].y;
                const r4 = PKVAR[4].y;
                const r5 = PKVAR[5].y;
                const r6 = PKVAR[6].y;
                const r7 = PKVAR[7].y;
                const r8 = PKVAR[8].y;
                const r9 = PKVAR[9].y;
                PKVAR1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKVA

    var PKVA = [];
    var PKVA1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKVA3",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            //  PKVA = resp.d;
            PKVA = eval(resp.d);
            for (i = 0; i < PKVA.length; i++) {


                const r0 = PKVA[0].y;
                const r1 = PKVA[1].y;
                const r2 = PKVA[2].y;
                const r3 = PKVA[3].y;
                const r4 = PKVA[4].y;
                const r5 = PKVA[5].y;
                const r6 = PKVA[6].y;
                const r7 = PKVA[7].y;
                const r8 = PKVA[8].y;
                const r9 = PKVA[9].y;
                PKVA1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    // Voltage
    var V1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV1A3",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            V1 = resp.d;
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    var V = [];
    var cur = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetData3",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            V = eval(resp.d);
            for (i = 0; i < V.length; i++) {


                const r0 = V[0].y;
                const r1 = V[1].y;
                const r2 = V[2].y;
                const r3 = V[3].y;
                const r4 = V[4].y;
                const r5 = V[5].y;
                const r6 = V[6].y;
                const r7 = V[7].y;
                const r8 = V[8].y;
                const r9 = V[9].y;
                cur = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(peval(resp.d);
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //


    var markers1 = [];
    var ty = [];
    var tx = [];

    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV1A3",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            // markers = resp.d;
            markers1 = eval(resp.d);

            for (i = 0; i < markers1.length; i++) {
                //ty = markers1[i].x;
                //  ty = new Date(markers1[i].x).toLocaleDateString("en-US");
                const d0 = new Date(markers1[0].x);
                const d1 = new Date(markers1[1].x);
                const d2 = new Date(markers1[2].x);
                const d3 = new Date(markers1[3].x);
                const d4 = new Date(markers1[4].x);
                const d5 = new Date(markers1[5].x);
                const d6 = new Date(markers1[6].x);
                const d7 = new Date(markers1[7].x);
                const d8 = new Date(markers1[8].x);
                const d9 = new Date(markers1[9].x);
                ty = [
                    d9.getHours() + ":" + d9.getMinutes(), d8.getHours() + ":" + d8.getMinutes(), d7.getHours() + ":" + d7.getMinutes(), d6.getHours() + ":" + d6.getMinutes(),
                    d5.getHours() + ":" + d5.getMinutes(), d4.getHours() + ":" + d4.getMinutes(), d3.getHours() + ":" + d3.getMinutes(), d2.getHours() + ":" + d2.getMinutes(),
                    d1.getHours() + ":" + d1.getMinutes(), d0.getHours() + ":" + d0.getMinutes()
                ];

                const t0 = markers1[0].y;
                const t1 = markers1[1].y;
                const t2 = markers1[2].y;
                const t3 = markers1[3].y;
                const t4 = markers1[4].y;
                const t5 = markers1[5].y;
                const t6 = markers1[6].y;
                const t7 = markers1[7].y;
                const t8 = markers1[8].y;
                const t9 = markers1[9].y;
                tx = [t0, t1, t2, t3, t4, t5, t6, t7, t8, t9];
            }



            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }

    });
    console.log(ty);

    var canvas = document.getElementById('canvas1');
    var ctx = canvas.getContext('2d');

    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            //labels: JSON.stringify(dps),
            labels: ty,
            datasets: [{
                label: "V",
                data: tx,
                fill: false,
                backgroundColor: 'green',
                borderColor: 'green'
                // color: "#878BB6",

            }, {
                label: "I",
                data: cur,
                fill: false,
                backgroundColor: 'red',
                borderColor: 'red'
                //color: "#4ACAB4",

            },
            {
                label: "F",
                data: Fre,
                fill: false,
                backgroundColor: '#FF5733',
                borderColor: '#FF5733'
                //color: "#4ACAB4",
            },

            {
                label: "PKW",
                data: PKW1,
                fill: false,
                backgroundColor: '#0066cc',
                borderColor: '#0066cc'
                //color: "#4ACAB4",
            },
            {
                label: "PKVAR",
                data: PKVAR1,
                fill: false,
                backgroundColor: '#993399',
                borderColor: '#993399'
                //color: "#4ACAB4",
            },
            {
                label: "PKVA",
                data: PKVA1,
                fill: false,
                backgroundColor: '#333399',
                borderColor: '#333399'
                //color: "#4ACAB4",
            }
            ]
        },
        /*  options: {

xAxes: [{
type: 'time',
                  time: {
format: "HH:MM:SS",

}

}],


              /* xAxes: [{
type: 'time',
                   time: {

unit: 'second',
tooltipFormat: 'h:mm:ss a'
}
}],*/

        // }
        options: {
            maintainAspectRatio: false,
            responsive: true,
            title: {
                display: true,
                position: 'top',
                text: 'Power Telemetry A-III Block'
            },
            legend: {
                display: true,
                position: 'bottom'
            },
            tooltips: {
                mode: 'index'
            },


            xAxes: [{
                type: 'time',
                time: {
                    unit: 'second',
                    displayFormats: {
                        second: 'h:mm:ss a'
                    },
                    tooltipFormat: 'h:mm:ss a'
                },
                /*type: 'time',
                                        time: {
                        unit: 'second',
                    tooltipFormat: 'h:mm:ss a'
                }*/
            }],

        }

    });
    /* var F = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetF3",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            F = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKW
    var PKW = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetPKWa3",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            PKW = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKVA
    
    var PKVAR = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetPKVARA3",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            PKVAR = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKVA
    
    var PKVA = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetPKVA3",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            PKVA = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    //});
    // Voltage
    var V1 = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetV1A3",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            V1 = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    // Current
    
    var p;
    
    
    var markers = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetData3",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            markers = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    
    //console.log(markers);
    var dps = [];
                 $.each(JSON.parse(markers), function (i, item) {
            dps.push({ x: item.X, y: item.y });
        //alert(dps[0]);
        });
        console.log(dps);
                 var chart = new CanvasJS.Chart("chartContainer1", {
            backgroundColor: "transparent",
        animationEnabled: true,
                     title: {
            text: "Power Telemetry"
    },
                     axisX: {
            //valueFormatString:  "hh:mm TT DD-MM-YYYY"
        },
                     axisY2: {
            title: "A-III",
        //refix: "$",
        //suffix: "K"
    },
                     toolTip: {
            shared: true
    },
                     legend: {
            //cursor: "pointer",
            //verticalAlign: "top",
            //horizontalAlign: "center",
            //dockInsidePlotArea: true,
            //itemclick: toogleDataSeries
        },
        data: [
                         {
            type: "line",
        axisYType: "secondary",
        name: "I",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(markers)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "V",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(V1)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "PKVA",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKVA)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "PKVAR",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKVAR)
                             //  [{"x": 1601026800000.0, "y": 90.398857 }, {"x": 1601026740000.0, "y": 90.400246 }, {"x": 1601026680000.0, "y": 90.03231 }, {"x": 1601026560000.0, "y": 89.822952 }, {"x": 1601026500000.0, "y": 90.096573 }, {"x": 1601026440000.0, "y": 89.846809 }, {"x": 1601026380000.0, "y": 89.768143 }, {"x": 1601026320000.0, "y": 89.917648 }, {"x": 1601026260000.0, "y": 89.751648 }, {"x": 1601026140000.0, "y": 89.838318 }]
        //JSON.parse(markers)
                             // [{"x": 1601018460000.0, "y": 85.26503 }, {"x": 1601018400000.0, "y": 85.125191 }, {"x": 1601018340000.0, "y": 85.387268 }, {"x": 1601018280000.0, "y": 85.602837 }, {"x": 1601018160000.0, "y": 85.699348 }, {"x": 1601018100000.0, "y": 85.701126 }, {"x": 1601018040000.0, "y": 85.631393 }, {"x": 1601017980000.0, "y": 85.547073 }, {"x": 1601017920000.0, "y": 84.770912 }, {"x": 1601017800000.0, "y": 83.743973 }]
        //    data: markers
        //}
        //JSON.parse(markers)
                             //[{"x": 1601012100000.0, "y": 0.0 }, {"x": 1601012040000.0, "y": 0.0 }, {"x": 1601011980000.0, "y": 0.0 }, {"x": 1601011920000.0, "y": 0.0 }, {"x": 1601011860000.0, "y": 0.0 }, {"x": 1601011800000.0, "y": 0.0 }, {"x": 1601011680000.0, "y": 0.0 }, {"x": 1601011620000.0, "y": 0.0 }, {"x": 1601011560000.0, "y": 0.0 }, {"x": 1601011500000.0, "y": 0.0 }]
                             //    [{
            //    data: markers
            //}
            //]
        },
                         {
            type: "line",
        axisYType: "secondary",
        name: "PKW",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKW)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "Frequency",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(F)
    
    }
    ]
    });
    chart.render();*/
}
/// function D2
function D2() {
    var F = [];
    var Fre = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetFd2",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            F = eval(resp.d);
            for (i = 0; i < F.length; i++) {


                const r0 = F[0].y;
                const r1 = F[1].y;
                const r2 = F[2].y;
                const r3 = F[3].y;
                const r4 = F[4].y;
                const r5 = F[5].y;
                const r6 = F[6].y;
                const r7 = F[7].y;
                const r8 = F[8].y;
                const r9 = F[9].y;
                Fre = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKW
    var PKW = [];
    var PKW1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKWd2",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            PKW = eval(resp.d);
            for (i = 0; i < PKW.length; i++) {


                const r0 = PKW[0].y;
                const r1 = PKW[1].y;
                const r2 = PKW[2].y;
                const r3 = PKW[3].y;
                const r4 = PKW[4].y;
                const r5 = PKW[5].y;
                const r6 = PKW[6].y;
                const r7 = PKW[7].y;
                const r8 = PKW[8].y;
                const r9 = PKW[9].y;
                PKW1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKVA

    var PKVAR = [];
    var PKVAR1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKVARd2",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            PKVAR = eval(resp.d);
            for (i = 0; i < PKVAR.length; i++) {


                const r0 = PKVAR[0].y;
                const r1 = PKVAR[1].y;
                const r2 = PKVAR[2].y;
                const r3 = PKVAR[3].y;
                const r4 = PKVAR[4].y;
                const r5 = PKVAR[5].y;
                const r6 = PKVAR[6].y;
                const r7 = PKVAR[7].y;
                const r8 = PKVAR[8].y;
                const r9 = PKVAR[9].y;
                PKVAR1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKVA

    var PKVA = [];
    var PKVA1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKVAd2",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            //  PKVA = resp.d;
            PKVA = eval(resp.d);
            for (i = 0; i < PKVA.length; i++) {


                const r0 = PKVA[0].y;
                const r1 = PKVA[1].y;
                const r2 = PKVA[2].y;
                const r3 = PKVA[3].y;
                const r4 = PKVA[4].y;
                const r5 = PKVA[5].y;
                const r6 = PKVA[6].y;
                const r7 = PKVA[7].y;
                const r8 = PKVA[8].y;
                const r9 = PKVA[9].y;
                PKVA1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    // Voltage
    var V1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV1d2",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            V1 = resp.d;
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    var V = [];
    var cur = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetDatad2",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            V = eval(resp.d);
            for (i = 0; i < V.length; i++) {


                const r0 = V[0].y;
                const r1 = V[1].y;
                const r2 = V[2].y;
                const r3 = V[3].y;
                const r4 = V[4].y;
                const r5 = V[5].y;
                const r6 = V[6].y;
                const r7 = V[7].y;
                const r8 = V[8].y;
                const r9 = V[9].y;
                cur = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(peval(resp.d);
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //


    var markers1 = [];
    var ty = [];
    var tx = [];

    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV1d2",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            // markers = resp.d;
            markers1 = eval(resp.d);

            for (i = 0; i < markers1.length; i++) {
                //ty = markers1[i].x;
                //  ty = new Date(markers1[i].x).toLocaleDateString("en-US");
                const d0 = new Date(markers1[0].x);
                const d1 = new Date(markers1[1].x);
                const d2 = new Date(markers1[2].x);
                const d3 = new Date(markers1[3].x);
                const d4 = new Date(markers1[4].x);
                const d5 = new Date(markers1[5].x);
                const d6 = new Date(markers1[6].x);
                const d7 = new Date(markers1[7].x);
                const d8 = new Date(markers1[8].x);
                const d9 = new Date(markers1[9].x);
                ty = [
                    d9.getHours() + ":" + d9.getMinutes(), d8.getHours() + ":" + d8.getMinutes(), d7.getHours() + ":" + d7.getMinutes(), d6.getHours() + ":" + d6.getMinutes(),
                    d5.getHours() + ":" + d5.getMinutes(), d4.getHours() + ":" + d4.getMinutes(), d3.getHours() + ":" + d3.getMinutes(), d2.getHours() + ":" + d2.getMinutes(),
                    d1.getHours() + ":" + d1.getMinutes(), d0.getHours() + ":" + d0.getMinutes()
                ];

                const t0 = markers1[0].y;
                const t1 = markers1[1].y;
                const t2 = markers1[2].y;
                const t3 = markers1[3].y;
                const t4 = markers1[4].y;
                const t5 = markers1[5].y;
                const t6 = markers1[6].y;
                const t7 = markers1[7].y;
                const t8 = markers1[8].y;
                const t9 = markers1[9].y;
                tx = [t0, t1, t2, t3, t4, t5, t6, t7, t8, t9];
            }



            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }

    });
    console.log(ty);

    var canvas = document.getElementById('canvas1');
    var ctx = canvas.getContext('2d');

    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            //labels: JSON.stringify(dps),
            labels: ty,
            datasets: [{
                label: "V",
                data: tx,
                fill: false,
                backgroundColor: 'green',
                borderColor: 'green'
                // color: "#878BB6",

            }, {
                label: "I",
                data: cur,
                fill: false,
                backgroundColor: 'red',
                borderColor: 'red'
                //color: "#4ACAB4",

            },
            {
                label: "F",
                data: Fre,
                fill: false,
                backgroundColor: '#FF5733',
                borderColor: '#FF5733'
                //color: "#4ACAB4",
            },

            {
                label: "PKW",
                data: PKW1,
                fill: false,
                backgroundColor: '#0066cc',
                borderColor: '#0066cc'
                //color: "#4ACAB4",
            },
            {
                label: "PKVAR",
                data: PKVAR1,
                fill: false,
                backgroundColor: '#993399',
                borderColor: '#993399'
                //color: "#4ACAB4",
            },
            {
                label: "PKVA",
                data: PKVA1,
                fill: false,
                backgroundColor: '#333399',
                borderColor: '#333399'
                //color: "#4ACAB4",
            }
            ]
        },
        /*  options: {

xAxes: [{
type: 'time',
                  time: {
format: "HH:MM:SS",

}

}],


              /* xAxes: [{
type: 'time',
                   time: {

unit: 'second',
tooltipFormat: 'h:mm:ss a'
}
}],*/

        // }
        options: {
            maintainAspectRatio: false,
            responsive: true,
            title: {
                display: true,
                position: 'top',
                text: 'Power Telemetry D-II Block'
            },
            legend: {
                display: true,
                position: 'bottom'
            },
            tooltips: {
                mode: 'index'
            },


            xAxes: [{
                type: 'time',
                time: {
                    unit: 'second',
                    displayFormats: {
                        second: 'h:mm:ss a'
                    },
                    tooltipFormat: 'h:mm:ss a'
                },
                /*type: 'time',
                                        time: {
                        unit: 'second',
                    tooltipFormat: 'h:mm:ss a'
                }*/
            }],

        }

    });
    /* var F = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetFd2",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            F = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKW
    var PKW = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetPKWd2",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            PKW = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKVA
    
    var PKVAR = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetPKVARd2",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            PKVAR = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKVA
    
    var PKVA = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetPKVd2",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            PKVA = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        // }
        //});
        // Voltage
        var V1 = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetV1d2",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            V1 = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    // Current
    
    var p;
    
    
    var markers = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetDatad2",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            markers = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    
    //console.log(markers);
    var dps = [];
                 $.each(JSON.parse(markers), function (i, item) {
            dps.push({ x: item.X, y: item.y });
        //alert(dps[0]);
        });
        console.log(dps);
                 var chart = new CanvasJS.Chart("chartContainer1", {
            backgroundColor: "transparent",
        animationEnabled: true,
                     title: {
            text: "Power Telemetry"
    },
                     axisX: {
            //valueFormatString:  "hh:mm TT DD-MM-YYYY"
        },
                     axisY2: {
            title: "D-II",
        //refix: "$",
        //suffix: "K"
    },
                     toolTip: {
            shared: true
    },
                     legend: {
            //cursor: "pointer",
            //verticalAlign: "top",
            //horizontalAlign: "center",
            //dockInsidePlotArea: true,
            //itemclick: toogleDataSeries
        },
        data: [
                         {
            type: "line",
        axisYType: "secondary",
        name: "I",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(markers)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "V",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(V1)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "PKVA",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKVA)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "PKVAR",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKVAR)
                             //  [{"x": 1601026800000.0, "y": 90.398857 }, {"x": 1601026740000.0, "y": 90.400246 }, {"x": 1601026680000.0, "y": 90.03231 }, {"x": 1601026560000.0, "y": 89.822952 }, {"x": 1601026500000.0, "y": 90.096573 }, {"x": 1601026440000.0, "y": 89.846809 }, {"x": 1601026380000.0, "y": 89.768143 }, {"x": 1601026320000.0, "y": 89.917648 }, {"x": 1601026260000.0, "y": 89.751648 }, {"x": 1601026140000.0, "y": 89.838318 }]
        //JSON.parse(markers)
                             // [{"x": 1601018460000.0, "y": 85.26503 }, {"x": 1601018400000.0, "y": 85.125191 }, {"x": 1601018340000.0, "y": 85.387268 }, {"x": 1601018280000.0, "y": 85.602837 }, {"x": 1601018160000.0, "y": 85.699348 }, {"x": 1601018100000.0, "y": 85.701126 }, {"x": 1601018040000.0, "y": 85.631393 }, {"x": 1601017980000.0, "y": 85.547073 }, {"x": 1601017920000.0, "y": 84.770912 }, {"x": 1601017800000.0, "y": 83.743973 }]
        //    data: markers
        //}
        //JSON.parse(markers)
                             //[{"x": 1601012100000.0, "y": 0.0 }, {"x": 1601012040000.0, "y": 0.0 }, {"x": 1601011980000.0, "y": 0.0 }, {"x": 1601011920000.0, "y": 0.0 }, {"x": 1601011860000.0, "y": 0.0 }, {"x": 1601011800000.0, "y": 0.0 }, {"x": 1601011680000.0, "y": 0.0 }, {"x": 1601011620000.0, "y": 0.0 }, {"x": 1601011560000.0, "y": 0.0 }, {"x": 1601011500000.0, "y": 0.0 }]
                             //    [{
            //    data: markers
            //}
            //]
        },
                         {
            type: "line",
        axisYType: "secondary",
        name: "PKW",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKW)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "Frequency",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(F)
    
    }
    ]
    });
    chart.render();*/
}
// function e1
function E1() {
    var F = [];
    var Fre = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetFe1",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            F = eval(resp.d);
            for (i = 0; i < F.length; i++) {


                const r0 = F[0].y;
                const r1 = F[1].y;
                const r2 = F[2].y;
                const r3 = F[3].y;
                const r4 = F[4].y;
                const r5 = F[5].y;
                const r6 = F[6].y;
                const r7 = F[7].y;
                const r8 = F[8].y;
                const r9 = F[9].y;
                Fre = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKW
    var PKW = [];
    var PKW1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKWe1",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            PKW = eval(resp.d);
            for (i = 0; i < PKW.length; i++) {


                const r0 = PKW[0].y;
                const r1 = PKW[1].y;
                const r2 = PKW[2].y;
                const r3 = PKW[3].y;
                const r4 = PKW[4].y;
                const r5 = PKW[5].y;
                const r6 = PKW[6].y;
                const r7 = PKW[7].y;
                const r8 = PKW[8].y;
                const r9 = PKW[9].y;
                PKW1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKVA

    var PKVAR = [];
    var PKVAR1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKVARe1",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            PKVAR = eval(resp.d);
            for (i = 0; i < PKVAR.length; i++) {


                const r0 = PKVAR[0].y;
                const r1 = PKVAR[1].y;
                const r2 = PKVAR[2].y;
                const r3 = PKVAR[3].y;
                const r4 = PKVAR[4].y;
                const r5 = PKVAR[5].y;
                const r6 = PKVAR[6].y;
                const r7 = PKVAR[7].y;
                const r8 = PKVAR[8].y;
                const r9 = PKVAR[9].y;
                PKVAR1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKVA

    var PKVA = [];
    var PKVA1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKVe1",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            //  PKVA = resp.d;
            PKVA = eval(resp.d);
            for (i = 0; i < PKVA.length; i++) {


                const r0 = PKVA[0].y;
                const r1 = PKVA[1].y;
                const r2 = PKVA[2].y;
                const r3 = PKVA[3].y;
                const r4 = PKVA[4].y;
                const r5 = PKVA[5].y;
                const r6 = PKVA[6].y;
                const r7 = PKVA[7].y;
                const r8 = PKVA[8].y;
                const r9 = PKVA[9].y;
                PKVA1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    // Voltage
    var V1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV1e1",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            V1 = resp.d;
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    var V = [];
    var cur = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetDatae1",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            V = eval(resp.d);
            for (i = 0; i < V.length; i++) {


                const r0 = V[0].y;
                const r1 = V[1].y;
                const r2 = V[2].y;
                const r3 = V[3].y;
                const r4 = V[4].y;
                const r5 = V[5].y;
                const r6 = V[6].y;
                const r7 = V[7].y;
                const r8 = V[8].y;
                const r9 = V[9].y;
                cur = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(peval(resp.d);
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //


    var markers1 = [];
    var ty = [];
    var tx = [];

    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV1e1",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            // markers = resp.d;
            markers1 = eval(resp.d);

            for (i = 0; i < markers1.length; i++) {
                //ty = markers1[i].x;
                //  ty = new Date(markers1[i].x).toLocaleDateString("en-US");
                const d0 = new Date(markers1[0].x);
                const d1 = new Date(markers1[1].x);
                const d2 = new Date(markers1[2].x);
                const d3 = new Date(markers1[3].x);
                const d4 = new Date(markers1[4].x);
                const d5 = new Date(markers1[5].x);
                const d6 = new Date(markers1[6].x);
                const d7 = new Date(markers1[7].x);
                const d8 = new Date(markers1[8].x);
                const d9 = new Date(markers1[9].x);
                ty = [
                    d9.getHours() + ":" + d9.getMinutes(), d8.getHours() + ":" + d8.getMinutes(), d7.getHours() + ":" + d7.getMinutes(), d6.getHours() + ":" + d6.getMinutes(),
                    d5.getHours() + ":" + d5.getMinutes(), d4.getHours() + ":" + d4.getMinutes(), d3.getHours() + ":" + d3.getMinutes(), d2.getHours() + ":" + d2.getMinutes(),
                    d1.getHours() + ":" + d1.getMinutes(), d0.getHours() + ":" + d0.getMinutes()
                ];

                const t0 = markers1[0].y;
                const t1 = markers1[1].y;
                const t2 = markers1[2].y;
                const t3 = markers1[3].y;
                const t4 = markers1[4].y;
                const t5 = markers1[5].y;
                const t6 = markers1[6].y;
                const t7 = markers1[7].y;
                const t8 = markers1[8].y;
                const t9 = markers1[9].y;
                tx = [t0, t1, t2, t3, t4, t5, t6, t7, t8, t9];
            }



            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }

    });
    console.log(ty);

    var canvas = document.getElementById('canvas1');
    var ctx = canvas.getContext('2d');

    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            //labels: JSON.stringify(dps),
            labels: ty,
            datasets: [{
                label: "V",
                data: tx,
                fill: false,
                backgroundColor: 'green',
                borderColor: 'green'
                // color: "#878BB6",

            }, {
                label: "I",
                data: cur,
                fill: false,
                backgroundColor: 'red',
                borderColor: 'red'
                //color: "#4ACAB4",

            },
            {
                label: "F",
                data: Fre,
                fill: false,
                backgroundColor: '#FF5733',
                borderColor: '#FF5733'
                //color: "#4ACAB4",
            },

            {
                label: "PKW",
                data: PKW1,
                fill: false,
                backgroundColor: '#0066cc',
                borderColor: '#0066cc'
                //color: "#4ACAB4",
            },
            {
                label: "PKVAR",
                data: PKVAR1,
                fill: false,
                backgroundColor: '#993399',
                borderColor: '#993399'
                //color: "#4ACAB4",
            },
            {
                label: "PKVA",
                data: PKVA1,
                fill: false,
                backgroundColor: '#333399',
                borderColor: '#333399'
                //color: "#4ACAB4",
            }
            ]
        },
        /*  options: {

xAxes: [{
type: 'time',
                  time: {
format: "HH:MM:SS",

}

}],


              /* xAxes: [{
type: 'time',
                   time: {

unit: 'second',
tooltipFormat: 'h:mm:ss a'
}
}],*/

        // }
        options: {
            maintainAspectRatio: false,
            responsive: true,
            title: {
                display: true,
                position: 'top',
                text: 'Power Telemetry E-Block'
            },
            legend: {
                display: true,
                position: 'bottom'
            },
            tooltips: {
                mode: 'index'
            },


            xAxes: [{
                type: 'time',
                time: {
                    unit: 'second',
                    displayFormats: {
                        second: 'h:mm:ss a'
                    },
                    tooltipFormat: 'h:mm:ss a'
                },
                /*type: 'time',
                                        time: {
                        unit: 'second',
                    tooltipFormat: 'h:mm:ss a'
                }*/
            }],

        }

    });
    /* var F = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetFe1",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            F = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKW
    var PKW = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetPKWe1",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            PKW = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKVA
    
    var PKVAR = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetPKVARe1",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            PKVAR = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKVA
    
    var PKVA = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetPKVe1",
                     data: "{}",
    
      //  contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            PKVA = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    // Voltage
    var V1 = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetV1e1",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            V1 = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    // Current
    
    var p;
    
    
    var markers = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetDatae1",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            markers = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    
    //console.log(markers);
    var dps = [];
                 $.each(JSON.parse(markers), function (i, item) {
            dps.push({ x: item.X, y: item.y });
        //alert(dps[0]);
        });
        console.log(dps);
                 var chart = new CanvasJS.Chart("chartContainer1", {
            backgroundColor: "transparent",
        animationEnabled: true,
                     title: {
            text: "Power Telemetry"
    },
                     axisX: {
            //valueFormatString:  "hh:mm TT DD-MM-YYYY"
        },
                     axisY2: {
            title: "E-I",
        //refix: "$",
        //suffix: "K"
    },
                     toolTip: {
            shared: true
    },
                     legend: {
            //cursor: "pointer",
            //verticalAlign: "top",
            //horizontalAlign: "center",
            //dockInsidePlotArea: true,
            //itemclick: toogleDataSeries
        },
        data: [
                         {
            type: "line",
        axisYType: "secondary",
        name: "I",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(markers)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "V",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(V1)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "PKVA",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKVA)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "PKVAR",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKVAR)
                             //  [{"x": 1601026800000.0, "y": 90.398857 }, {"x": 1601026740000.0, "y": 90.400246 }, {"x": 1601026680000.0, "y": 90.03231 }, {"x": 1601026560000.0, "y": 89.822952 }, {"x": 1601026500000.0, "y": 90.096573 }, {"x": 1601026440000.0, "y": 89.846809 }, {"x": 1601026380000.0, "y": 89.768143 }, {"x": 1601026320000.0, "y": 89.917648 }, {"x": 1601026260000.0, "y": 89.751648 }, {"x": 1601026140000.0, "y": 89.838318 }]
        //JSON.parse(markers)
                             // [{"x": 1601018460000.0, "y": 85.26503 }, {"x": 1601018400000.0, "y": 85.125191 }, {"x": 1601018340000.0, "y": 85.387268 }, {"x": 1601018280000.0, "y": 85.602837 }, {"x": 1601018160000.0, "y": 85.699348 }, {"x": 1601018100000.0, "y": 85.701126 }, {"x": 1601018040000.0, "y": 85.631393 }, {"x": 1601017980000.0, "y": 85.547073 }, {"x": 1601017920000.0, "y": 84.770912 }, {"x": 1601017800000.0, "y": 83.743973 }]
        //    data: markers
        //}
        //JSON.parse(markers)
                             //[{"x": 1601012100000.0, "y": 0.0 }, {"x": 1601012040000.0, "y": 0.0 }, {"x": 1601011980000.0, "y": 0.0 }, {"x": 1601011920000.0, "y": 0.0 }, {"x": 1601011860000.0, "y": 0.0 }, {"x": 1601011800000.0, "y": 0.0 }, {"x": 1601011680000.0, "y": 0.0 }, {"x": 1601011620000.0, "y": 0.0 }, {"x": 1601011560000.0, "y": 0.0 }, {"x": 1601011500000.0, "y": 0.0 }]
                             //    [{
            //    data: markers
            //}
            //]
        },
                         {
            type: "line",
        axisYType: "secondary",
        name: "PKW",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKW)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "Frequency",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(F)
    
    }
    ]
    });
    chart.render();*/
}
// function Camp
function Camp() {
    var F = [];
    var Fre = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetFcp",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            F = eval(resp.d);
            for (i = 0; i < F.length; i++) {


                const r0 = F[0].y;
                const r1 = F[1].y;
                const r2 = F[2].y;
                const r3 = F[3].y;
                const r4 = F[4].y;
                const r5 = F[5].y;
                const r6 = F[6].y;
                const r7 = F[7].y;
                const r8 = F[8].y;
                const r9 = F[9].y;
                Fre = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKW
    var PKW = [];
    var PKW1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKWcp",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            PKW = eval(resp.d);
            for (i = 0; i < PKW.length; i++) {


                const r0 = PKW[0].y;
                const r1 = PKW[1].y;
                const r2 = PKW[2].y;
                const r3 = PKW[3].y;
                const r4 = PKW[4].y;
                const r5 = PKW[5].y;
                const r6 = PKW[6].y;
                const r7 = PKW[7].y;
                const r8 = PKW[8].y;
                const r9 = PKW[9].y;
                PKW1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKVA

    var PKVAR = [];
    var PKVAR1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKVARcp",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            PKVAR = eval(resp.d);
            for (i = 0; i < PKVAR.length; i++) {


                const r0 = PKVAR[0].y;
                const r1 = PKVAR[1].y;
                const r2 = PKVAR[2].y;
                const r3 = PKVAR[3].y;
                const r4 = PKVAR[4].y;
                const r5 = PKVAR[5].y;
                const r6 = PKVAR[6].y;
                const r7 = PKVAR[7].y;
                const r8 = PKVAR[8].y;
                const r9 = PKVAR[9].y;
                PKVAR1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKVA

    var PKVA = [];
    var PKVA1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKVAcp",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            //  PKVA = resp.d;
            PKVA = eval(resp.d);
            for (i = 0; i < PKVA.length; i++) {


                const r0 = PKVA[0].y;
                const r1 = PKVA[1].y;
                const r2 = PKVA[2].y;
                const r3 = PKVA[3].y;
                const r4 = PKVA[4].y;
                const r5 = PKVA[5].y;
                const r6 = PKVA[6].y;
                const r7 = PKVA[7].y;
                const r8 = PKVA[8].y;
                const r9 = PKVA[9].y;
                PKVA1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    // Voltage
    var V1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV1cp",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            V1 = resp.d;
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    var V = [];
    var cur = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetDatacp",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            V = eval(resp.d);
            for (i = 0; i < V.length; i++) {


                const r0 = V[0].y;
                const r1 = V[1].y;
                const r2 = V[2].y;
                const r3 = V[3].y;
                const r4 = V[4].y;
                const r5 = V[5].y;
                const r6 = V[6].y;
                const r7 = V[7].y;
                const r8 = V[8].y;
                const r9 = V[9].y;
                cur = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(peval(resp.d);
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //


    var markers1 = [];
    var ty = [];
    var tx = [];

    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV1cp",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            // markers = resp.d;
            markers1 = eval(resp.d);

            for (i = 0; i < markers1.length; i++) {
                //ty = markers1[i].x;
                //  ty = new Date(markers1[i].x).toLocaleDateString("en-US");
                const d0 = new Date(markers1[0].x);
                const d1 = new Date(markers1[1].x);
                const d2 = new Date(markers1[2].x);
                const d3 = new Date(markers1[3].x);
                const d4 = new Date(markers1[4].x);
                const d5 = new Date(markers1[5].x);
                const d6 = new Date(markers1[6].x);
                const d7 = new Date(markers1[7].x);
                const d8 = new Date(markers1[8].x);
                const d9 = new Date(markers1[9].x);
                ty = [
                    d9.getHours() + ":" + d9.getMinutes(), d8.getHours() + ":" + d8.getMinutes(), d7.getHours() + ":" + d7.getMinutes(), d6.getHours() + ":" + d6.getMinutes(),
                    d5.getHours() + ":" + d5.getMinutes(), d4.getHours() + ":" + d4.getMinutes(), d3.getHours() + ":" + d3.getMinutes(), d2.getHours() + ":" + d2.getMinutes(),
                    d1.getHours() + ":" + d1.getMinutes(), d0.getHours() + ":" + d0.getMinutes()
                ];

                const t0 = markers1[0].y;
                const t1 = markers1[1].y;
                const t2 = markers1[2].y;
                const t3 = markers1[3].y;
                const t4 = markers1[4].y;
                const t5 = markers1[5].y;
                const t6 = markers1[6].y;
                const t7 = markers1[7].y;
                const t8 = markers1[8].y;
                const t9 = markers1[9].y;
                tx = [t0, t1, t2, t3, t4, t5, t6, t7, t8, t9];
            }



            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }

    });
    console.log(ty);

    var canvas = document.getElementById('canvas1');
    var ctx = canvas.getContext('2d');

    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            //labels: JSON.stringify(dps),
            labels: ty,
            datasets: [{
                label: "V",
                data: tx,
                fill: false,
                backgroundColor: 'green',
                borderColor: 'green'
                // color: "#878BB6",

            }, {
                label: "I",
                data: cur,
                fill: false,
                backgroundColor: 'red',
                borderColor: 'red'
                //color: "#4ACAB4",

            },
            {
                label: "F",
                data: Fre,
                fill: false,
                backgroundColor: '#FF5733',
                borderColor: '#FF5733'
                //color: "#4ACAB4",
            },

            {
                label: "PKW",
                data: PKW1,
                fill: false,
                backgroundColor: '#0066cc',
                borderColor: '#0066cc'
                //color: "#4ACAB4",
            },
            {
                label: "PKVAR",
                data: PKVAR1,
                fill: false,
                backgroundColor: '#993399',
                borderColor: '#993399'
                //color: "#4ACAB4",
            },
            {
                label: "PKVA",
                data: PKVA1,
                fill: false,
                backgroundColor: '#333399',
                borderColor: '#333399'
                //color: "#4ACAB4",
            }
            ]
        },
        /*  options: {

xAxes: [{
type: 'time',
                  time: {
format: "HH:MM:SS",

}

}],


              /* xAxes: [{
type: 'time',
                   time: {

unit: 'second',
tooltipFormat: 'h:mm:ss a'
}
}],*/

        // }
        options: {
            maintainAspectRatio: false,
            responsive: true,
            title: {
                display: true,
                position: 'top',
                text: 'Power Telemetry Campus View'
            },
            legend: {
                display: true,
                position: 'bottom'
            },
            tooltips: {
                mode: 'index'
            },


            xAxes: [{
                type: 'time',
                time: {
                    unit: 'second',
                    displayFormats: {
                        second: 'h:mm:ss a'
                    },
                    tooltipFormat: 'h:mm:ss a'
                },
                /*type: 'time',
                                        time: {
                        unit: 'second',
                    tooltipFormat: 'h:mm:ss a'
                }*/
            }],

        }

    });
    /* var F = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetFcp",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            F = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKW
    var PKW = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetPKWcp",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            PKW = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKVA
    
    var PKVAR = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetPKVARcp",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            PKVAR = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKVA
    
    var PKVA = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetPKVcp",
                     data: "{}",
    
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            PKVA = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    // Voltage
    var V1 = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetV1cp",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            V1 = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    // Current
    
    var p;
    
    
    var markers = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetDatacp",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            markers = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    
    //console.log(markers);
    var dps = [];
                 $.each(JSON.parse(markers), function (i, item) {
            dps.push({ x: item.X, y: item.y });
        //alert(dps[0]);
        });
        console.log(dps);
                 var chart = new CanvasJS.Chart("chartContainer1", {
            backgroundColor: "transparent",
        animationEnabled: true,
                     title: {
            text: "Power Telemetry"
    },
                     axisX: {
            //valueFormatString:  "hh:mm TT DD-MM-YYYY"
        },
                     axisY2: {
            title: "Campus View",
        //refix: "$",
        //suffix: "K"
    },
                     toolTip: {
            shared: true
    },
                     legend: {
            //cursor: "pointer",
            //verticalAlign: "top",
            //horizontalAlign: "center",
            //dockInsidePlotArea: true,
            //itemclick: toogleDataSeries
        },
        data: [
                         {
            type: "line",
        axisYType: "secondary",
        name: "I",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(markers)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "V",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(V1)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "PKVA",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKVA)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "PKVAR",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKVAR)
                             //  [{"x": 1601026800000.0, "y": 90.398857 }, {"x": 1601026740000.0, "y": 90.400246 }, {"x": 1601026680000.0, "y": 90.03231 }, {"x": 1601026560000.0, "y": 89.822952 }, {"x": 1601026500000.0, "y": 90.096573 }, {"x": 1601026440000.0, "y": 89.846809 }, {"x": 1601026380000.0, "y": 89.768143 }, {"x": 1601026320000.0, "y": 89.917648 }, {"x": 1601026260000.0, "y": 89.751648 }, {"x": 1601026140000.0, "y": 89.838318 }]
        //JSON.parse(markers)
                             // [{"x": 1601018460000.0, "y": 85.26503 }, {"x": 1601018400000.0, "y": 85.125191 }, {"x": 1601018340000.0, "y": 85.387268 }, {"x": 1601018280000.0, "y": 85.602837 }, {"x": 1601018160000.0, "y": 85.699348 }, {"x": 1601018100000.0, "y": 85.701126 }, {"x": 1601018040000.0, "y": 85.631393 }, {"x": 1601017980000.0, "y": 85.547073 }, {"x": 1601017920000.0, "y": 84.770912 }, {"x": 1601017800000.0, "y": 83.743973 }]
        //    data: markers
        //}
        //JSON.parse(markers)
                             //[{"x": 1601012100000.0, "y": 0.0 }, {"x": 1601012040000.0, "y": 0.0 }, {"x": 1601011980000.0, "y": 0.0 }, {"x": 1601011920000.0, "y": 0.0 }, {"x": 1601011860000.0, "y": 0.0 }, {"x": 1601011800000.0, "y": 0.0 }, {"x": 1601011680000.0, "y": 0.0 }, {"x": 1601011620000.0, "y": 0.0 }, {"x": 1601011560000.0, "y": 0.0 }, {"x": 1601011500000.0, "y": 0.0 }]
                             //    [{
            //    data: markers
            //}
            //]
        },
                         {
            type: "line",
        axisYType: "secondary",
        name: "PKW",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKW)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "Frequency",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(F)
    
    }
    ]
    });
    chart.render();*/
}
function F1() {
    var F = [];
    var Fre = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetFj",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            F = eval(resp.d);
            for (i = 0; i < F.length; i++) {


                const r0 = F[0].y;
                const r1 = F[1].y;
                const r2 = F[2].y;
                const r3 = F[3].y;
                const r4 = F[4].y;
                const r5 = F[5].y;
                const r6 = F[6].y;
                const r7 = F[7].y;
                const r8 = F[8].y;
                const r9 = F[9].y;
                Fre = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKW
    var PKW = [];
    var PKW1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKWj",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            PKW = eval(resp.d);
            for (i = 0; i < PKW.length; i++) {


                const r0 = PKW[0].y;
                const r1 = PKW[1].y;
                const r2 = PKW[2].y;
                const r3 = PKW[3].y;
                const r4 = PKW[4].y;
                const r5 = PKW[5].y;
                const r6 = PKW[6].y;
                const r7 = PKW[7].y;
                const r8 = PKW[8].y;
                const r9 = PKW[9].y;
                PKW1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKVA

    var PKVAR = [];
    var PKVAR1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKVARj",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            PKVAR = eval(resp.d);
            for (i = 0; i < PKVAR.length; i++) {


                const r0 = PKVAR[0].y;
                const r1 = PKVAR[1].y;
                const r2 = PKVAR[2].y;
                const r3 = PKVAR[3].y;
                const r4 = PKVAR[4].y;
                const r5 = PKVAR[5].y;
                const r6 = PKVAR[6].y;
                const r7 = PKVAR[7].y;
                const r8 = PKVAR[8].y;
                const r9 = PKVAR[9].y;
                PKVAR1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKVA

    var PKVA = [];
    var PKVA1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKVj",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            //  PKVA = resp.d;
            PKVA = eval(resp.d);
            for (i = 0; i < PKVA.length; i++) {


                const r0 = PKVA[0].y;
                const r1 = PKVA[1].y;
                const r2 = PKVA[2].y;
                const r3 = PKVA[3].y;
                const r4 = PKVA[4].y;
                const r5 = PKVA[5].y;
                const r6 = PKVA[6].y;
                const r7 = PKVA[7].y;
                const r8 = PKVA[8].y;
                const r9 = PKVA[9].y;
                PKVA1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    // Voltage
    var V1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV1j",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            V1 = resp.d;
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    var V = [];
    var cur = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetDataj",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            V = eval(resp.d);
            for (i = 0; i < V.length; i++) {


                const r0 = V[0].y;
                const r1 = V[1].y;
                const r2 = V[2].y;
                const r3 = V[3].y;
                const r4 = V[4].y;
                const r5 = V[5].y;
                const r6 = V[6].y;
                const r7 = V[7].y;
                const r8 = V[8].y;
                const r9 = V[9].y;
                cur = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(peval(resp.d);
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //


    var markers1 = [];
    var ty = [];
    var tx = [];

    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV1j",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            // markers = resp.d;
            markers1 = eval(resp.d);

            for (i = 0; i < markers1.length; i++) {
                //ty = markers1[i].x;
                //  ty = new Date(markers1[i].x).toLocaleDateString("en-US");
                const d0 = new Date(markers1[0].x);
                const d1 = new Date(markers1[1].x);
                const d2 = new Date(markers1[2].x);
                const d3 = new Date(markers1[3].x);
                const d4 = new Date(markers1[4].x);
                const d5 = new Date(markers1[5].x);
                const d6 = new Date(markers1[6].x);
                const d7 = new Date(markers1[7].x);
                const d8 = new Date(markers1[8].x);
                const d9 = new Date(markers1[9].x);
                ty = [
                    d9.getHours() + ":" + d9.getMinutes(), d8.getHours() + ":" + d8.getMinutes(), d7.getHours() + ":" + d7.getMinutes(), d6.getHours() + ":" + d6.getMinutes(),
                    d5.getHours() + ":" + d5.getMinutes(), d4.getHours() + ":" + d4.getMinutes(), d3.getHours() + ":" + d3.getMinutes(), d2.getHours() + ":" + d2.getMinutes(),
                    d1.getHours() + ":" + d1.getMinutes(), d0.getHours() + ":" + d0.getMinutes()
                ];

                const t0 = markers1[0].y;
                const t1 = markers1[1].y;
                const t2 = markers1[2].y;
                const t3 = markers1[3].y;
                const t4 = markers1[4].y;
                const t5 = markers1[5].y;
                const t6 = markers1[6].y;
                const t7 = markers1[7].y;
                const t8 = markers1[8].y;
                const t9 = markers1[9].y;
                tx = [t0, t1, t2, t3, t4, t5, t6, t7, t8, t9];
            }



            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }

    });
    console.log(ty);

    var canvas = document.getElementById('canvas1');
    var ctx = canvas.getContext('2d');

    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            //labels: JSON.stringify(dps),
            labels: ty,
            datasets: [{
                label: "V",
                data: tx,
                fill: false,
                backgroundColor: 'green',
                borderColor: 'green'
                // color: "#878BB6",

            }, {
                label: "I",
                data: cur,
                fill: false,
                backgroundColor: 'red',
                borderColor: 'red'
                //color: "#4ACAB4",

            },
            {
                label: "F",
                data: Fre,
                fill: false,
                backgroundColor: '#FF5733',
                borderColor: '#FF5733'
                //color: "#4ACAB4",
            },

            {
                label: "PKW",
                data: PKW1,
                fill: false,
                backgroundColor: '#0066cc',
                borderColor: '#0066cc'
                //color: "#4ACAB4",
            },
            {
                label: "PKVAR",
                data: PKVAR1,
                fill: false,
                backgroundColor: '#993399',
                borderColor: '#993399'
                //color: "#4ACAB4",
            },
            {
                label: "PKVA",
                data: PKVA1,
                fill: false,
                backgroundColor: '#333399',
                borderColor: '#333399'
                //color: "#4ACAB4",
            }
            ]
        },
        /*  options: {

xAxes: [{
type: 'time',
                  time: {
format: "HH:MM:SS",

}

}],


              /* xAxes: [{
type: 'time',
                   time: {

unit: 'second',
tooltipFormat: 'h:mm:ss a'
}
}],*/

        // }
        options: {
            maintainAspectRatio: false,
            responsive: true,
            title: {
                display: true,
                position: 'top',
                text: 'Power Telemetry F block'
            },
            legend: {
                display: true,
                position: 'bottom'
            },
            tooltips: {
                mode: 'index'
            },


            xAxes: [{
                type: 'time',
                time: {
                    unit: 'second',
                    displayFormats: {
                        second: 'h:mm:ss a'
                    },
                    tooltipFormat: 'h:mm:ss a'
                },
                /*type: 'time',
                                        time: {
                        unit: 'second',
                    tooltipFormat: 'h:mm:ss a'
                }*/
            }],

        }

    });
    /* var F = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetFj",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            F = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKW
    var PKW = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetPKWj",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            PKW = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKVA
    
    var PKVAR = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetPKVARj",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            PKVAR = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKVA
    
    var PKVA = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetPKVj",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
    //     async: false,
                     success: function (resp) {
            PKVA = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    // Voltage
    var V1 = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetV1j",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            V1 = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    // Current
    
    var p;
    
    
    var markers = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetDataj",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            markers = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    
    //console.log(markers);
    var dps = [];
                 $.each(JSON.parse(markers), function (i, item) {
            dps.push({ x: item.X, y: item.y });
        //alert(dps[0]);
        });
        console.log(dps);
                 var chart = new CanvasJS.Chart("chartContainer1", {
            backgroundColor: "transparent",
        animationEnabled: true,
                     title: {
            text: "Power Telemetry"
    },
                     axisX: {
            //valueFormatString:  "hh:mm TT DD-MM-YYYY"
        },
                     axisY2: {
            title: "F-I Block",
        //refix: "$",
        //suffix: "K"
    },
                     toolTip: {
            shared: true
    },
                     legend: {
            //cursor: "pointer",
            //verticalAlign: "top",
            //horizontalAlign: "center",
            //dockInsidePlotArea: true,
            //itemclick: toogleDataSeries
        },
        data: [
                         {
            type: "line",
        axisYType: "secondary",
        name: "I",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(markers)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "V",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(V1)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "PKVA",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKVA)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "PKVAR",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKVAR)
                             //  [{"x": 1601026800000.0, "y": 90.398857 }, {"x": 1601026740000.0, "y": 90.400246 }, {"x": 1601026680000.0, "y": 90.03231 }, {"x": 1601026560000.0, "y": 89.822952 }, {"x": 1601026500000.0, "y": 90.096573 }, {"x": 1601026440000.0, "y": 89.846809 }, {"x": 1601026380000.0, "y": 89.768143 }, {"x": 1601026320000.0, "y": 89.917648 }, {"x": 1601026260000.0, "y": 89.751648 }, {"x": 1601026140000.0, "y": 89.838318 }]
        //JSON.parse(markers)
                             // [{"x": 1601018460000.0, "y": 85.26503 }, {"x": 1601018400000.0, "y": 85.125191 }, {"x": 1601018340000.0, "y": 85.387268 }, {"x": 1601018280000.0, "y": 85.602837 }, {"x": 1601018160000.0, "y": 85.699348 }, {"x": 1601018100000.0, "y": 85.701126 }, {"x": 1601018040000.0, "y": 85.631393 }, {"x": 1601017980000.0, "y": 85.547073 }, {"x": 1601017920000.0, "y": 84.770912 }, {"x": 1601017800000.0, "y": 83.743973 }]
        //    data: markers
        //}
        //JSON.parse(markers)
                             //[{"x": 1601012100000.0, "y": 0.0 }, {"x": 1601012040000.0, "y": 0.0 }, {"x": 1601011980000.0, "y": 0.0 }, {"x": 1601011920000.0, "y": 0.0 }, {"x": 1601011860000.0, "y": 0.0 }, {"x": 1601011800000.0, "y": 0.0 }, {"x": 1601011680000.0, "y": 0.0 }, {"x": 1601011620000.0, "y": 0.0 }, {"x": 1601011560000.0, "y": 0.0 }, {"x": 1601011500000.0, "y": 0.0 }]
                             //    [{
            //    data: markers
            //}
            //]
        },
                         {
            type: "line",
        axisYType: "secondary",
        name: "PKW",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKW)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "Frequency",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(F)
    
    }
    ]
    });
    chart.render();*/
}
//function jubile
function JC() {
    var F = [];
    var Fre = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetFjc",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            F = eval(resp.d);
            for (i = 0; i < F.length; i++) {


                const r0 = F[0].y;
                const r1 = F[1].y;
                const r2 = F[2].y;
                const r3 = F[3].y;
                const r4 = F[4].y;
                const r5 = F[5].y;
                const r6 = F[6].y;
                const r7 = F[7].y;
                const r8 = F[8].y;
                const r9 = F[9].y;
                Fre = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKW
    var PKW = [];
    var PKW1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKWjc",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            PKW = eval(resp.d);
            for (i = 0; i < PKW.length; i++) {


                const r0 = PKW[0].y;
                const r1 = PKW[1].y;
                const r2 = PKW[2].y;
                const r3 = PKW[3].y;
                const r4 = PKW[4].y;
                const r5 = PKW[5].y;
                const r6 = PKW[6].y;
                const r7 = PKW[7].y;
                const r8 = PKW[8].y;
                const r9 = PKW[9].y;
                PKW1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKVA

    var PKVAR = [];
    var PKVAR1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKVARjc",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            PKVAR = eval(resp.d);
            for (i = 0; i < PKVAR.length; i++) {


                const r0 = PKVAR[0].y;
                const r1 = PKVAR[1].y;
                const r2 = PKVAR[2].y;
                const r3 = PKVAR[3].y;
                const r4 = PKVAR[4].y;
                const r5 = PKVAR[5].y;
                const r6 = PKVAR[6].y;
                const r7 = PKVAR[7].y;
                const r8 = PKVAR[8].y;
                const r9 = PKVAR[9].y;
                PKVAR1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //PKVA

    var PKVA = [];
    var PKVA1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetPKVjc",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            //  PKVA = resp.d;
            PKVA = eval(resp.d);
            for (i = 0; i < PKVA.length; i++) {


                const r0 = PKVA[0].y;
                const r1 = PKVA[1].y;
                const r2 = PKVA[2].y;
                const r3 = PKVA[3].y;
                const r4 = PKVA[4].y;
                const r5 = PKVA[5].y;
                const r6 = PKVA[6].y;
                const r7 = PKVA[7].y;
                const r8 = PKVA[8].y;
                const r9 = PKVA[9].y;
                PKVA1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    // Voltage
    var V1 = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV1jc",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            V1 = resp.d;
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    var V = [];
    var cur = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetDatajc",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            V = eval(resp.d);
            for (i = 0; i < V.length; i++) {


                const r0 = V[0].y;
                const r1 = V[1].y;
                const r2 = V[2].y;
                const r3 = V[3].y;
                const r4 = V[4].y;
                const r5 = V[5].y;
                const r6 = V[6].y;
                const r7 = V[7].y;
                const r8 = V[8].y;
                const r9 = V[9].y;
                cur = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
            }
            // markers = eval(resp.d);
            // p = markers.Data12;

            //   alert(peval(resp.d);
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
    //


    var markers1 = [];
    var ty = [];
    var tx = [];

    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetV1jc",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            // markers = resp.d;
            markers1 = eval(resp.d);

            for (i = 0; i < markers1.length; i++) {
                //ty = markers1[i].x;
                //  ty = new Date(markers1[i].x).toLocaleDateString("en-US");
                const d0 = new Date(markers1[0].x);
                const d1 = new Date(markers1[1].x);
                const d2 = new Date(markers1[2].x);
                const d3 = new Date(markers1[3].x);
                const d4 = new Date(markers1[4].x);
                const d5 = new Date(markers1[5].x);
                const d6 = new Date(markers1[6].x);
                const d7 = new Date(markers1[7].x);
                const d8 = new Date(markers1[8].x);
                const d9 = new Date(markers1[9].x);
                ty = [
                    d9.getHours() + ":" + d9.getMinutes(), d8.getHours() + ":" + d8.getMinutes(), d7.getHours() + ":" + d7.getMinutes(), d6.getHours() + ":" + d6.getMinutes(),
                    d5.getHours() + ":" + d5.getMinutes(), d4.getHours() + ":" + d4.getMinutes(), d3.getHours() + ":" + d3.getMinutes(), d2.getHours() + ":" + d2.getMinutes(),
                    d1.getHours() + ":" + d1.getMinutes(), d0.getHours() + ":" + d0.getMinutes()
                ];

                const t0 = markers1[0].y;
                const t1 = markers1[1].y;
                const t2 = markers1[2].y;
                const t3 = markers1[3].y;
                const t4 = markers1[4].y;
                const t5 = markers1[5].y;
                const t6 = markers1[6].y;
                const t7 = markers1[7].y;
                const t8 = markers1[8].y;
                const t9 = markers1[9].y;
                tx = [t0, t1, t2, t3, t4, t5, t6, t7, t8, t9];
            }



            // p = markers.Data12;

            //   alert(p)
            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }

    });
    console.log(ty);

    var canvas = document.getElementById('canvas1');
    var ctx = canvas.getContext('2d');

    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            //labels: JSON.stringify(dps),
            labels: ty,
            datasets: [{
                label: "V",
                data: tx,
                fill: false,
                backgroundColor: 'green',
                borderColor: 'green'
                // color: "#878BB6",

            }, {
                label: "I",
                data: cur,
                fill: false,
                backgroundColor: 'red',
                borderColor: 'red'
                //color: "#4ACAB4",

            },
            {
                label: "F",
                data: Fre,
                fill: false,
                backgroundColor: '#FF5733',
                borderColor: '#FF5733'
                //color: "#4ACAB4",
            },

            {
                label: "PKW",
                data: PKW1,
                fill: false,
                backgroundColor: '#0066cc',
                borderColor: '#0066cc'
                //color: "#4ACAB4",
            },
            {
                label: "PKVAR",
                data: PKVAR1,
                fill: false,
                backgroundColor: '#993399',
                borderColor: '#993399'
                //color: "#4ACAB4",
            },
            {
                label: "PKVA",
                data: PKVA1,
                fill: false,
                backgroundColor: '#333399',
                borderColor: '#333399'
                //color: "#4ACAB4",
            }
            ]
        },
        /*  options: {

xAxes: [{
type: 'time',
                  time: {
format: "HH:MM:SS",

}

}],


              /* xAxes: [{
type: 'time',
                   time: {

unit: 'second',
tooltipFormat: 'h:mm:ss a'
}
}],*/

        // }
        options: {
            maintainAspectRatio: false,
            responsive: true,
            title: {
                display: true,
                position: 'top',
                text: 'Power Telemetry Jubilee Town'
            },
            legend: {
                display: true,
                position: 'bottom'
            },
            tooltips: {
                mode: 'index'
            },


            xAxes: [{
                type: 'time',
                time: {
                    unit: 'second',
                    displayFormats: {
                        second: 'h:mm:ss a'
                    },
                    tooltipFormat: 'h:mm:ss a'
                },
                /*type: 'time',
                                        time: {
                        unit: 'second',
                    tooltipFormat: 'h:mm:ss a'
                }*/
            }],

        }

    });
    /* var F = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetFjc",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            F = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKW
    var PKW = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetPKWjc",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            PKW = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKVA
    
    var PKVAR = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetPKVARjc",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            PKVAR = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    //PKVA
    
    var PKVA = [];
                 $.ajax({
            // type: "POST",
            url: "../savedata.asmx/GetPKVjc",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            PKVA = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    // Voltage
    var V1 = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetV1jc",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            V1 = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    // Current
    
    var p;
    
    
    var markers = [];
                 $.ajax({
            type: "POST",
        url: "../savedata.asmx/GetDatajc",
                     data: "{}",
    
        contentType: "application/json; charset=utf-8",
        dataType: "json",
    
        async: false,
                     success: function (resp) {
            markers = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;
    
        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
        }
    });
    
    //console.log(markers);
    var dps = [];
                 $.each(JSON.parse(markers), function (i, item) {
            dps.push({ x: item.X, y: item.y });
        //alert(dps[0]);
        });
        console.log(dps);
                 var chart = new CanvasJS.Chart("chartContainer1", {
            backgroundColor: "transparent",
        animationEnabled: true,
                     title: {
            text: "Power Telemetry"
    },
                     axisX: {
            //valueFormatString:  "hh:mm TT DD-MM-YYYY"
        },
                     axisY2: {
            title: "Jubilee Town",
        //refix: "$",
        //suffix: "K"
    },
                     toolTip: {
            shared: true
    },
                     legend: {
            //cursor: "pointer",
            //verticalAlign: "top",
            //horizontalAlign: "center",
            //dockInsidePlotArea: true,
            //itemclick: toogleDataSeries
        },
        data: [
                         {
            type: "line",
        axisYType: "secondary",
        name: "I",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(markers)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "V",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(V1)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "PKVA",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKVA)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "PKVAR",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKVAR)
                             //  [{"x": 1601026800000.0, "y": 90.398857 }, {"x": 1601026740000.0, "y": 90.400246 }, {"x": 1601026680000.0, "y": 90.03231 }, {"x": 1601026560000.0, "y": 89.822952 }, {"x": 1601026500000.0, "y": 90.096573 }, {"x": 1601026440000.0, "y": 89.846809 }, {"x": 1601026380000.0, "y": 89.768143 }, {"x": 1601026320000.0, "y": 89.917648 }, {"x": 1601026260000.0, "y": 89.751648 }, {"x": 1601026140000.0, "y": 89.838318 }]
        //JSON.parse(markers)
                             // [{"x": 1601018460000.0, "y": 85.26503 }, {"x": 1601018400000.0, "y": 85.125191 }, {"x": 1601018340000.0, "y": 85.387268 }, {"x": 1601018280000.0, "y": 85.602837 }, {"x": 1601018160000.0, "y": 85.699348 }, {"x": 1601018100000.0, "y": 85.701126 }, {"x": 1601018040000.0, "y": 85.631393 }, {"x": 1601017980000.0, "y": 85.547073 }, {"x": 1601017920000.0, "y": 84.770912 }, {"x": 1601017800000.0, "y": 83.743973 }]
        //    data: markers
        //}
        //JSON.parse(markers)
                             //[{"x": 1601012100000.0, "y": 0.0 }, {"x": 1601012040000.0, "y": 0.0 }, {"x": 1601011980000.0, "y": 0.0 }, {"x": 1601011920000.0, "y": 0.0 }, {"x": 1601011860000.0, "y": 0.0 }, {"x": 1601011800000.0, "y": 0.0 }, {"x": 1601011680000.0, "y": 0.0 }, {"x": 1601011620000.0, "y": 0.0 }, {"x": 1601011560000.0, "y": 0.0 }, {"x": 1601011500000.0, "y": 0.0 }]
                             //    [{
            //    data: markers
            //}
            //]
        },
                         {
            type: "line",
        axisYType: "secondary",
        name: "PKW",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(PKW)
    
    },
                         {
            type: "line",
        axisYType: "secondary",
        name: "Frequency",
        showInLegend: true,
        xValueType: "dateTime",
        xValueFormatString: "hh:mm TT DD-MM-YYYY",
        markerSize: 1,
        dataPoints: JSON.parse(F)
    
    }
    ]
    });
    chart.render();*/
}

// max voltage A3
function maxVA3() {
    // var el_up = document.getElementById("GFG_UP");



    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("MV1");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetVmax",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            el_down.innerHTML =
                Math.max.apply(Math, markers.map(function (o) {
                    return Math.round(o.y * 10) / 10;
                }));
        }
    });
    //el_up.innerHTML = JSON.stringify(array);

}
// max current A3
function maxIA3() {
    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("IV1");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetDataI",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            el_down.innerHTML =
                Math.max.apply(Math, markers.map(function (o) {
                    return Math.round(o.y * 10) / 10;;
                }));
        }
    });
    //el_up.innerHTML = JSON.stringify(array);




}


// max voltageC2
function maxVC() {
    // var el_up = document.getElementById("GFG_UP");



    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("MV1");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetVmaxC",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            el_down.innerHTML =
                Math.max.apply(Math, markers.map(function (o) {
                    return Math.round(o.y * 10) / 10;
                }));
        }
    });
    //el_up.innerHTML = JSON.stringify(array);

}
// max current C2
function maxIC() {
    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("IV1");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetDataIC",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            el_down.innerHTML =
                Math.max.apply(Math, markers.map(function (o) {
                    return Math.round(o.y * 10) / 10;;
                }));
        }
    });
    //el_up.innerHTML = JSON.stringify(array);




}

// max voltageD2
function maxVD() {
    // var el_up = document.getElementById("GFG_UP");



    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("MV1");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetVmaxD",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            el_down.innerHTML =
                Math.max.apply(Math, markers.map(function (o) {
                    return Math.round(o.y * 10) / 10;
                }));
        }
    });
    //el_up.innerHTML = JSON.stringify(array);

}
// max current D2
function maxID() {
    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("IV1");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetDataID",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            el_down.innerHTML =
                Math.max.apply(Math, markers.map(function (o) {
                    return Math.round(o.y * 10) / 10;;
                }));
        }
    });
    //el_up.innerHTML = JSON.stringify(array);




}

// max voltageE
function maxVE() {
    // var el_up = document.getElementById("GFG_UP");



    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("MV1");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetVmaxE",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            el_down.innerHTML =
                Math.max.apply(Math, markers.map(function (o) {
                    return Math.round(o.y * 10) / 10;
                }));
        }
    });
    //el_up.innerHTML = JSON.stringify(array);

}
// max current E
function maxIE() {
    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("IV1");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetDataIE",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            el_down.innerHTML =
                Math.max.apply(Math, markers.map(function (o) {
                    return Math.round(o.y * 10) / 10;;
                }));
        }
    });
    //el_up.innerHTML = JSON.stringify(array);




}


// max voltage campus
function maxVcp() {
    // var el_up = document.getElementById("GFG_UP");



    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("MV1");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetVmaxcp",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            el_down.innerHTML =
                Math.max.apply(Math, markers.map(function (o) {
                    return Math.round(o.y * 10) / 10;
                }));
        }
    });
    //el_up.innerHTML = JSON.stringify(array);

}
// max current E
function maxIcp() {
    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("IV1");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetDataIcp",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            el_down.innerHTML =
                Math.max.apply(Math, markers.map(function (o) {
                    return Math.round(o.y * 10) / 10;;
                }));
        }
    });
    //el_up.innerHTML = JSON.stringify(array);




}

// max voltage F
function maxVF() {
    // var el_up = document.getElementById("GFG_UP");



    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("MV1");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetVmaxF",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            el_down.innerHTML =
                Math.max.apply(Math, markers.map(function (o) {
                    return Math.round(o.y * 10) / 10;
                }));
        }
    });
    //el_up.innerHTML = JSON.stringify(array);

}
// max current F
function maxIF() {
    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("IV1");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetDataIF",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            el_down.innerHTML =
                Math.max.apply(Math, markers.map(function (o) {
                    return Math.round(o.y * 10) / 10;;
                }));
        }
    });
    //el_up.innerHTML = JSON.stringify(array);




}


// max voltage J
function maxVJ() {
    // var el_up = document.getElementById("GFG_UP");



    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("MV1");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetVmaxJ",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            el_down.innerHTML =
                Math.max.apply(Math, markers.map(function (o) {
                    return Math.round(o.y * 10) / 10;
                }));
        }
    });
    //el_up.innerHTML = JSON.stringify(array);

}
// max current J
function maxIJ() {
    // var el_up = document.getElementById("GFG_UP");
    var el_down = document.getElementById("IV1");

    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetDataIJ",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {

            markers = eval(resp.d);
            el_down.innerHTML =
                Math.max.apply(Math, markers.map(function (o) {
                    return Math.round(o.y * 10) / 10;;
                }));
        }
    });
    //el_up.innerHTML = JSON.stringify(array);




}
// Water flow A3
function TWFa() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWat",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf = Math.round(total * 10) / 10;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWF7a() {
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWat7",
        data: "{ }",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf7 = Math.round(total * 10) / 10;
        }
    });
}
function TWF30a() {
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWat30",
        data: "{ }",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf30 = Math.round(total * 10) / 10;
        }
    });
}

// Water flow C
function TWFc() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWatC",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf = Math.round(total * 10) / 10;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWF7c() {
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWat7C",
        data: "{ }",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf7 = Math.round(total * 10) / 10;
        }
    });
}
function TWF30c() {
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWat30C",
        data: "{ }",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf30 = Math.round(total * 10) / 10;
        }
    });
}

// Water flow D
function TWFd() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWatD",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf = Math.round(total * 10) / 10;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWF7d() {
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWat7D",
        data: "{ }",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf7 = Math.round(total * 10) / 10;
        }
    });
}
function TWF30d() {
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWat30D",
        data: "{ }",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf30 = Math.round(total * 10) / 10;
        }
    });
}

// Water flow E
function TWFe() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWatE",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf = Math.round(total * 10) / 10;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWF7e() {
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWat7E",
        data: "{ }",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf7 = Math.round(total * 10) / 10;
        }
    });
}
function TWF30e() {
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWat30E",
        data: "{ }",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf30 = Math.round(total * 10) / 10;
        }
    });
}

// Water flow camp
function TWFcp() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWatcp",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf = Math.round(total * 10) / 10;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWF7cp() {
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWat7cp",
        data: "{ }",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf7 = Math.round(total * 10) / 10;
        }
    });
}
function TWF30cp() {
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWat30cp",
        data: "{ }",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf30 = Math.round(total * 10) / 10;
        }
    });
}

// Water flow F
function TWFF() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWatF",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf = Math.round(total * 10) / 10;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWF7F() {
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWat7F",
        data: "{ }",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf7 = Math.round(total * 10) / 10;
        }
    });
}
function TWF30F() {
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWat30F",
        data: "{ }",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf30 = Math.round(total * 10) / 10;
        }
    });
}

// Water flow J
function TWFJ() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWat6",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf = Math.round(total * 10) / 10;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWF7J() {
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWat7J",
        data: "{ }",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf7 = Math.round(total * 10) / 10;
        }
    });
}
function TWF30J() {
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWat30J",
        data: "{ }",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);
            total = 0
            for (i = 0; i < markers.length; i++) {  //loop through the array
                total += markers[i].y;  //Do the math!
            }
            twf30 = Math.round(total * 10) / 10;
        }
    });
}


// Working hours a3

function TWHa() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWHA3",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWH7a() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWH7A3",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha7 = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWH30a() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWH30A3",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha30 = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
// Working hours c2

function TWHc() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWH",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWH7c() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWH7",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha7 = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWH30c() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWH30",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha30 = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
// Working hours d

function TWHd() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWHD",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWH7d() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWH7D",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha7 = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWH30d() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWH30D",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha30 = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
// Working hours E

function TWHe() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWHE",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWH7e() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWH7E",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha7 = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWH30e() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWH30E",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha30 = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}

// Working hours F

function TWHf() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWHF",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWH7f() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWH7F",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha7 = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWH30f() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWH30F",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha30 = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}

// Working hours Camp

function TWHcp() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWHCp",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWH7cp() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWH7cp",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha7 = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWH30cp() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWH30cp",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha30 = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
// Working hoursJ

function TWHj() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWHJ",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWH7j() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWH7J",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha7 = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function TWH30j() {
    var markers = [];
    $.ajax({
        type: "POST",
        url: "../savedata.asmx/GetWH30J",
        data: "{}",

        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (resp) {
            markers = eval(resp.d);

            twha30 = markers;
            //cra = markers[3].P;

            //   alert(markers[3].P.length);
            // cr = markers[3].P;

            // cr = markers[2].P;

            //   $("#chart").text(markers[3].P);
            //$("#name").text(response[0].name);
            // $("#id").text(response[0].id);
        }
    });
}
function handleTitleChange(e) {

    //   var y = '';
    //  y =  e.target.textContent;
    setInterval(function () {
        var markers = '';
        $.ajax({
            type: "POST",
            url: "../savedata.asmx/GetData121",
            data: "{ }",

            contentType: "application/json; charset=utf-8",
            dataType: "json",

            async: false,
            success: function (resp) {

                // alert(resp.d);
                markers = eval(resp.d);

                //alert(markers[2].p.length);
                //  $("#siteF").text(markers[3].title);
                if (e.target.textContent.indexOf("A-III Block Johar Town") > -1) {
                    TWHa();
                    TWH7a();
                    TWH30a();
                    TWFa();
                    TWF7a();
                    TWF30a();
                    V30a();
                    V7a();
                    l30a();
                    l7a();
                    maxVA3();
                    maxIA3();
                    //hello();
                    C1();
                    // A3();
                    //Modes
                    $("#A1").text(markers[3].Auto);
                    $("#R1").text(markers[3].Remote);
                    $("#T1").text(markers[3].Time);
                    //Pressure Gauge
                    cr = markers[3].P;

                    //Power factor
                    pr = markers[3].PF;

                    //Primming Level
                    // pl = markers[3].PL;
                    if (markers[3].PL == "0") {
                        $("#PL").text("Low");
                    }
                    else {

                        $("#PL").text("High");
                    }

                    // Vibration meter
                    vm = markers[3].VM;

                    //  if (e.target.textContent == "A-III Block Johar Town") {
                    //  document.getElementById("IV").innerHTML = "whatever";
                    ///  $("#IV").text(markers[6].A + markers[3].A + markers[2].A + markers[4].A);

                    //Chlorine
                    if (markers[3].CL == "0") {
                        $("#CL").html("&#10062");
                    }
                    else {

                        $("#CL").html("&#9989");
                    }
                    //Light
                    if (markers[3].LS == "0") {
                        $("#LS1").html("&#128308");
                    }
                    else {
                        $("#LS1").html("&#128994");
                    }
                    // Voltage trip
                    if (markers[3].VT == "1") {
                        $("#VT1").html("&#9888");

                    }
                    else {
                        $("#VT2").html("&#9889");

                    }
                    //Curent Trip
                    if (markers[3].IT == "1") {
                        $("#IT1").html("&#9888");

                    }
                    else {

                        $("#IT2").html("&#9889");
                    }

                }
                else if (e.target.textContent.indexOf("D-II Block Johar Town") > -1) {
                    TWHd();
                    TWH7d();
                    TWH30d();
                    TWFd();
                    TWF7d();
                    TWF30d();
                    V30d();
                    V7d();
                    l30d();
                    l7d();
                    TVD();
                    TID();
                    maxVD();
                    maxID();
                    D2();
                    //Modes
                    $("#A1").text(markers[4].Auto);
                    $("#R1").text(markers[4].Remote);
                    $("#T1").text(markers[4].Time);
                    //Power factor
                    pr = markers[4].PF;
                    cr = markers[4].P;
                    //Primming Level
                    //  pl = markers[4].PL;
                    if (markers[4].PL == "0") {
                        $("#PL").text("Low");
                    }
                    else {

                        $("#PL").text("High");
                    }
                    // Vibration meter
                    vm = markers[4].VM;
                    //$("#IV").text(markers[4].title);

                    if (markers[4].CL == "0") {
                        $("#CL").html("&#10062");
                    }
                    else {

                        $("#CL").html("&#9989");
                    }
                    //Light
                    if (markers[4].LS == "0") {
                        $("#LS1").html("&#128308");
                    }
                    else {
                        $("#LS1").html("&#128994");
                    }
                    // Voltage trip
                    if (markers[4].VT == "1") {
                        $("#VT1").html("&#9888");

                    }
                    else {

                        $("#VT2").html("&#9889");
                    }
                    //Curent Trip
                    if (markers[4].IT == "1") {
                        $("#IT1").html("&#9888");

                    }
                    else {

                        $("#IT2").html("&#9889");
                    }
                }
                else if (e.target.textContent.indexOf("E Block Johar Town") > -1) {

                    TWHe();
                    TWH7e();
                    TWH30e();
                    TWFe();
                    TWF7e();
                    TWF30e();
                    V30e();
                    V7e();
                    l30e();
                    l7e();
                    TVE();
                    TIE();
                    maxVE();
                    maxIE();
                    E1();
                    //Modes
                    $("#A1").text(markers[5].Auto);
                    $("#R1").text(markers[5].Remote);
                    $("#T1").text(markers[5].Time);

                    cr = markers[5].P;

                    //Power factor
                    pr = markers[5].PF;
                    //Primming Level
                    if (markers[5].PL == "0") {
                        $("#PL").text("Low");
                    }
                    else {

                        $("#PL").text("High");
                    }
                    // pl = markers[5].PL;
                    // Vibration meter
                    vm = markers[5].VM;
                    //$("#IV").text(markers[5].title);
                    if (markers[5].CL == "0") {
                        $("#CL").html("&#10062");
                    }
                    else {

                        $("#CL").html("&#9989");
                    }
                    //Light
                    if (markers[5].LS == "0") {
                        $("#LS1").html("&#128308");
                    }
                    else {
                        $("#LS1").html("&#128994");
                    }
                    // Voltage trip
                    if (markers[5].VT == "1") {
                        $("#VT1").html("&#9888");

                    }
                    else {

                        $("#VT2").html("&#9889");
                    }
                    //Curent Trip
                    if (markers[5].IT == "1") {
                        $("#IT1").html("&#9888");

                    }
                    else {

                        $("#IT2").html("&#9889");
                    }

                }
                else if (e.target.textContent.indexOf("Campus View Johar Town") > -1) {
                    TWHcp();
                    TWH7cp();
                    TWH30cp();
                    TWFcp();
                    TWF7cp();
                    TWF30cp();
                    V30cp();
                    V7cp();
                    l30cp();
                    l7cp();
                    TVcp();
                    TIcp();
                    maxVcp();
                    maxIcp();
                    Camp();
                    //Modes
                    $("#A1").text(markers[6].Auto);
                    $("#R1").text(markers[6].Remote);
                    $("#T1").text(markers[6].Time);
                    cr = markers[6].P;
                    //Power factor
                    pr = markers[6].PF;
                    //Primming Level
                    if (markers[6].PL == "0") {
                        $("#PL").text("Low");
                    }
                    else {

                        $("#PL").text("High");
                    }
                    // pl = markers[6].PL;
                    // Vibration meter
                    vm = markers[6].VM;
                    //$("#IV").text(markers[6].title);
                    if (markers[6].CL == "0") {
                        $("#CL").html("&#10062");
                    }
                    else {

                        $("#CL").html("&#9989");
                    }
                    //Light
                    if (markers[6].LS == "0") {
                        $("#LS1").html("&#128308");
                    }
                    else {
                        $("#LS1").html("&#128994");
                    }
                    // Voltage trip
                    if (markers[6].VT == "1") {
                        $("#VT1").html("&#9888");

                    }
                    else {

                        $("#VT2").html("&#9889");
                    }
                    //Curent Trip
                    if (markers[6].IT == "1") {
                        $("#IT1").html("&#9888");

                    }
                    else {

                        $("#IT2").html("&#9889");
                    }
                }
                else if (e.target.textContent.indexOf("F-I Block Johar Town") > -1) {
                    TWH7f();
                    TWH30f();
                    TWHf();
                    TWFF()
                    TWF7F();
                    TWF30F();
                    V30F();
                    V7F();
                    l30F();
                    l7F();
                    TVF();
                    TIF();
                    maxVF();
                    maxIF();
                    F1();
                    //Modes
                    $("#A1").text(markers[7].Auto);
                    $("#R1").text(markers[7].Remote);
                    $("#T1").text(markers[7].Time);
                    cr = markers[7].P;
                    //Power factor
                    pr = markers[7].PF;
                    //Primming Level
                    pl = markers[7].PL;
                    // Vibration meter
                    vm = markers[7].VM;
                    //$("#IV").text(markers[7].title);
                    if (markers[7].CL == "0") {
                        $("#CL").html("&#10062");
                    }
                    else {

                        $("#CL").html("&#9989");
                    }
                    //Light
                    if (markers[7].LS == "0") {
                        $("#LS1").html("&#128308");
                    }
                    else {
                        $("#LS1").html("&#128994");
                    }
                    // Voltage trip
                    if (markers[7].VT == "1") {
                        $("#VT1").html("&#9888");

                    }
                    else {

                        $("#VT2").html("&#9889");
                    }
                    //Curent Trip
                    if (markers[7].IT == "1") {
                        $("#IT1").html("&#9888");

                    }
                    else {

                        $("#IT2").html("&#9889");
                    }
                }
                else if (e.target.textContent.indexOf("C Block Jubilee Town") > -1) {

                    TWHj();
                    TWH7j();
                    TWH30j();
                    TWFJ();
                    TWF7J();
                    TWF30J();
                    V30J();
                    V7J();
                    l30J();
                    l7J();
                    TVJ();
                    TIJ();
                    maxVJ();
                    maxIJ()
                    JC();
                    //Modes
                    $("#A1").text(markers[8].Auto);
                    $("#R1").text(markers[8].Remote);
                    $("#T1").text(markers[8].Time);
                    cr = markers[8].P;
                    //Power factor
                    pr = markers[8].PF;
                    //Primming Level
                    if (markers[8].PL == "0") {
                        $("#PL").text("Low");
                    }
                    else {

                        $("#PL").text("High");
                    }
                    //pl = markers[8].PL;
                    // Vibration meter
                    vm = markers[8].VM;
                    //  $("#IV").text(markers[8].title);
                    if (markers[8].CL == "0") {
                        $("#CL").html("&#10062");
                    }
                    else {

                        $("#CL").html("&#9989");
                    }
                    //Light
                    if (markers[8].LS == "0") {
                        $("#LS1").html("&#128308");
                    }
                    else {
                        $("#LS1").html("&#128994");
                    }
                    // Voltage trip
                    if (markers[8].VT == "1") {
                        $("#VT1").html("&#9888");

                    }
                    else {

                        $("#VT2").html("&#9889");
                    }
                    //Curent Trip
                    if (markers[8].IT == "1") {
                        $("#IT1").html("&#9888");

                    }
                    else {

                        $("#IT2").html("&#9889");
                    }
                }

                else if (e.target.textContent.indexOf("C-II Block Johar Town") > -1) {
                    TWHc();
                    TWH7c();
                    TWH30c();
                    TWFc()
                    TWF7c();
                    TWF30c();
                    V30c();
                    V7c();
                    l30c();
                    l7c();
                    TVC();
                    TIC();
                    maxVC();
                    maxIC();
                    // A3();
                    C2();
                    //Modes
                    $("#A1").text(markers[2].Auto);
                    $("#R1").text(markers[2].Remote);
                    $("#T1").text(markers[2].Time);
                    cr = markers[2].P;
                    //Power factor
                    pr = markers[2].PF;
                    //Primming Level
                    pl = markers[2].PL;
                    // Vibration meter
                    vm = markers[8].VM;
                    //$("#IV").text(markers[2].Dtae);
                    if (markers[2].CL == "0") {
                        $("#CL").html("&#10062");
                    }
                    else {

                        $("#CL").html("&#9989");
                    }
                    //Light
                    if (markers[2].LS == "0") {
                        $("#LS1").html("&#128308");
                    }
                    else {
                        $("#LS1").html("&#128994");
                    }
                    // Voltage trip
                    if (markers[2].VT == "1") {
                        $("#VT1").html("&#9888");

                    }
                    else {

                        $("#VT2").html("&#9889");
                    }
                    //Curent Trip
                    if (markers[2].IT == "1") {
                        $("#IT1").html("&#9888");

                    }
                    else {

                        $("#IT2").html("&#9889");
                    }
                }
            }
        });
        //  setTimeout(handleTitleChange, 5000);
    }, 5000);

    // const result = document.getElementById('result');

    //result.innerHTML = 'The result is: ' + e.target.textContent;
}
/*$(document).ready(function () {
setTimeout(handleTitleChange, 5000);
});*/
//get elements
var dropdownTitle = document.querySelector('.dropdown .title');
const dropdownOptions = document.querySelectorAll('.dropdown .option');

//bind listeners to these elements
if (dropdownTitle) {

    dropdownTitle.addEventListener('click', toggleMenuDisplay);
}

dropdownOptions.forEach(option => option.addEventListener('click', handleOptionSelected));

var htc = document.querySelector('.dropdown .title');
if (htc) {
    htc.addEventListener('change', handleTitleChange);
}




//Modes*@








// Tubewell Status







var d = new Date(); // for now
d.getHours(); // => 9
d.getMinutes(); // =>  30
d.getSeconds(); // => 51

var chart121 = new CanvasJS.Chart("chartContainer1", {
        backgroundColor: "transparent",
        animationEnabled: true,
        title: {
            text: "Power Telemetry"
        },
        axisX: {
            interval: 0
        },
        data: [
            {
                type: "line",
                dataPoints: [
                    { x: 12, y: 21 },
                    { x: 1, y: 25 },
                    { x: 2, y: 2 },
                    { x: 3, y: 25 },
                    { x: 4, y: 27 },
                    { x: 5, y: 28 },
                    { x: 6, y: 28 },
                    { x: 7, y: 24 },
                    { x: 8, y: 26 }

                ]
            },
            {
                type: "line",
                dataPoints: [
                    { x: 12, y: 31 },
                    { x: 1, y: 35 },
                    { x: 2, y: 30 },
                    { x: 3, y: 35 },
                    { x: 4, y: 35 },
                    { x: 5, y: 38 },
                    { x: 6, y: 38 },
                    { x: 7, y: 34 },
                    { x: 8, y: 44 }

                ]
            },
            {
                type: "line",
                dataPoints: [
                    { x: 12, y: 45 },
                    { x: 1, y: 50 },
                    { x: 2, y: 40 },
                    { x: 3, y: 45 },
                    { x: 4, y: 45 },
                    { x: 5, y: 48 },
                    { x: 6, y: 43 },
                    { x: 7, y: 41 },
                    { x: 8, y: 28 }

                ]
            },
            {
                type: "line",
                dataPoints: [
                    { x: 12, y: 71 },
                    { x: 1, y: 55 },
                    { x: 2, y: 50 },
                    { x: 3, y: 65 },
                    { x: 4, y: 95 },
                    { x: 5, y: 68 },
                    { x: 6, y: 28 },
                    { x: 7, y: 34 },
                    { x: 8, y: 14 }

                ]
            }
        ]
    }
);

chart121.render();


//
var F = [];
$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetF",
    data: "{ }",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {
        F = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;

        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
    }
});
//PKW
var PKW = [];
$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetPKW",
    data: "{ }",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {
        PKW = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;

        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
    }
});
//PKVA

var PKVAR = [];
$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetPKVAR",
    data: "{ }",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {
        PKVAR = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;

        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
    }
});
//PKVA

var PKVA = [];
$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetPKVA",
    data: "{ }",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {
        PKVA = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;

        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
    }
});
// Voltage
var V1 = [];
$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetV1",
    data: "{ }",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {
        V1 = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;

        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
    }
});
// Current

var p;


var markers = [];
$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetData2",
    data: "{ }",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {
        markers = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;

        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
    }
});

//console.log(markers);
var dps = [];
$.each(JSON.parse(markers), function (i, item) {
    dps.push({ x: item.X, y: item.y });
    //alert(dps[0]);
});
console.log(dps);
var chart = new CanvasJS.Chart("chartContainer1", {
    backgroundColor: "transparent",
    animationEnabled: true,
    title: {
        text: "Power Telemetry"
    },
    axisX: {
        //valueFormatString:  "hh:mm TT DD-MM-YYYY"
    },
    axisY2: {
        title: "Values",
        //refix: "$",
        //suffix: "K"
    },
    toolTip: {
        shared: true
    },
    legend: {
        //cursor: "pointer",
        //verticalAlign: "top",
        //horizontalAlign: "center",
        //dockInsidePlotArea: true,
        //itemclick: toogleDataSeries
    },
    data: [
        {
            type: "line",
            axisYType: "secondary",
            name: "I",
            showInLegend: true,
            xValueType: "dateTime",
            xValueFormatString: "hh:mm TT DD-MM-YYYY",
            markerSize: 1,
            dataPoints: JSON.parse(markers)

        },
        {
            type: "line",
            axisYType: "secondary",
            name: "V",
            showInLegend: true,
            xValueType: "dateTime",
            xValueFormatString: "hh:mm TT DD-MM-YYYY",
            markerSize: 1,
            dataPoints: JSON.parse(V1)

        },
        {
            type: "line",
            axisYType: "secondary",
            name: "PKVA",
            showInLegend: true,
            xValueType: "dateTime",
            xValueFormatString: "hh:mm TT DD-MM-YYYY",
            markerSize: 1,
            dataPoints: JSON.parse(PKVA)

        },
        {
            type: "line",
            axisYType: "secondary",
            name: "PKVAR",
            showInLegend: true,
            xValueType: "dateTime",
            xValueFormatString: "hh:mm TT DD-MM-YYYY",
            markerSize: 1,
            dataPoints: JSON.parse(PKVAR)
            //  [{ "x": 1601026800000.0, "y": 90.398857 }, { "x": 1601026740000.0, "y": 90.400246 }, { "x": 1601026680000.0, "y": 90.03231 }, { "x": 1601026560000.0, "y": 89.822952 }, { "x": 1601026500000.0, "y": 90.096573 }, { "x": 1601026440000.0, "y": 89.846809 }, { "x": 1601026380000.0, "y": 89.768143 }, { "x": 1601026320000.0, "y": 89.917648 }, { "x": 1601026260000.0, "y": 89.751648 }, { "x": 1601026140000.0, "y": 89.838318 }]
            //JSON.parse(markers)
            // [{ "x": 1601018460000.0, "y": 85.26503 }, { "x": 1601018400000.0, "y": 85.125191 }, { "x": 1601018340000.0, "y": 85.387268 }, { "x": 1601018280000.0, "y": 85.602837 }, { "x": 1601018160000.0, "y": 85.699348 }, { "x": 1601018100000.0, "y": 85.701126 }, { "x": 1601018040000.0, "y": 85.631393 }, { "x": 1601017980000.0, "y": 85.547073 }, { "x": 1601017920000.0, "y": 84.770912 }, { "x": 1601017800000.0, "y": 83.743973 }]
            //    data: markers
            //}
            //JSON.parse(markers)
            //[{ "x": 1601012100000.0, "y": 0.0 }, { "x": 1601012040000.0, "y": 0.0 }, { "x": 1601011980000.0, "y": 0.0 }, { "x": 1601011920000.0, "y": 0.0 }, { "x": 1601011860000.0, "y": 0.0 }, { "x": 1601011800000.0, "y": 0.0 }, { "x": 1601011680000.0, "y": 0.0 }, { "x": 1601011620000.0, "y": 0.0 }, { "x": 1601011560000.0, "y": 0.0 }, { "x": 1601011500000.0, "y": 0.0 }]
            //    [{
            //    data: markers
            //}
            //]
        },
        {
            type: "line",
            axisYType: "secondary",
            name: "PKW",
            showInLegend: true,
            xValueType: "dateTime",
            xValueFormatString: "hh:mm TT DD-MM-YYYY",
            markerSize: 1,
            dataPoints: JSON.parse(PKW)

        },
        {
            type: "line",
            axisYType: "secondary",
            name: "Frequency",
            showInLegend: true,
            xValueType: "dateTime",
            xValueFormatString: "hh:mm TT DD-MM-YYYY",
            markerSize: 1,
            dataPoints: JSON.parse(F)

        }
    ]
});
chart.render();
// alert(markers.Datau)




var F = [];
var Fre = [];
$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetF3",
    data: "{}",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {
        F = eval(resp.d);
        for (i = 0; i < F.length; i++) {


            const r0 = F[0].y;
            const r1 = F[1].y;
            const r2 = F[2].y;
            const r3 = F[3].y;
            const r4 = F[4].y;
            const r5 = F[5].y;
            const r6 = F[6].y;
            const r7 = F[7].y;
            const r8 = F[8].y;
            const r9 = F[9].y;
            Fre = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
        }
        // markers = eval(resp.d);
        // p = markers.Data12;

        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
    }
});
//PKW
var PKW = [];
var PKW1 = [];
$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetPKWa3",
    data: "{}",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {
        PKW = eval(resp.d);
        for (i = 0; i < PKW.length; i++) {


            const r0 = PKW[0].y;
            const r1 = PKW[1].y;
            const r2 = PKW[2].y;
            const r3 = PKW[3].y;
            const r4 = PKW[4].y;
            const r5 = PKW[5].y;
            const r6 = PKW[6].y;
            const r7 = PKW[7].y;
            const r8 = PKW[8].y;
            const r9 = PKW[9].y;
            PKW1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
        }
        // markers = eval(resp.d);
        // p = markers.Data12;

        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
    }
});
//PKVA

var PKVAR = [];
var PKVAR1 = [];
$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetPKVARA3",
    data: "{}",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {

        PKVAR = eval(resp.d);
        for (i = 0; i < PKVAR.length; i++) {


            const r0 = PKVAR[0].y;
            const r1 = PKVAR[1].y;
            const r2 = PKVAR[2].y;
            const r3 = PKVAR[3].y;
            const r4 = PKVAR[4].y;
            const r5 = PKVAR[5].y;
            const r6 = PKVAR[6].y;
            const r7 = PKVAR[7].y;
            const r8 = PKVAR[8].y;
            const r9 = PKVAR[9].y;
            PKVAR1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
        }
        // markers = eval(resp.d);
        // p = markers.Data12;

        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
    }
});
//PKVA

var PKVA = [];
var PKVA1 = [];
$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetPKVA3",
    data: "{}",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {
        //  PKVA = resp.d;
        PKVA = eval(resp.d);
        for (i = 0; i < PKVA.length; i++) {


            const r0 = PKVA[0].y;
            const r1 = PKVA[1].y;
            const r2 = PKVA[2].y;
            const r3 = PKVA[3].y;
            const r4 = PKVA[4].y;
            const r5 = PKVA[5].y;
            const r6 = PKVA[6].y;
            const r7 = PKVA[7].y;
            const r8 = PKVA[8].y;
            const r9 = PKVA[9].y;
            PKVA1 = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
        }
        // markers = eval(resp.d);
        // p = markers.Data12;

        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
    }
});
// Voltage
var V1 = [];
$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetV1A3",
    data: "{}",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {
        V1 = resp.d;
        // markers = eval(resp.d);
        // p = markers.Data12;

        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
    }
});
var V = [];
var cur = [];
$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetData3",
    data: "{}",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {
        V = eval(resp.d);
        for (i = 0; i < V.length; i++) {


            const r0 = V[0].y;
            const r1 = V[1].y;
            const r2 = V[2].y;
            const r3 = V[3].y;
            const r4 = V[4].y;
            const r5 = V[5].y;
            const r6 = V[6].y;
            const r7 = V[7].y;
            const r8 = V[8].y;
            const r9 = V[9].y;
            cur = [r0, r1, r2, r3, r4, r5, r6, r7, r8, r9];
        }
        // markers = eval(resp.d);
        // p = markers.Data12;

        //   alert(peval(resp.d);
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
    }
});
//


var markers1 = [];
var ty = [];
var tx = [];

$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetV1A3",
    data: "{}",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {
        // markers = resp.d;
        markers1 = eval(resp.d);

        for (i = 0; i < markers1.length; i++) {
            //ty = markers1[i].x;
            //  ty = new Date(markers1[i].x).toLocaleDateString("en-US");
            const d0 = new Date(markers1[0].x);
            const d1 = new Date(markers1[1].x);
            const d2 = new Date(markers1[2].x);
            const d3 = new Date(markers1[3].x);
            const d4 = new Date(markers1[4].x);
            const d5 = new Date(markers1[5].x);
            const d6 = new Date(markers1[6].x);
            const d7 = new Date(markers1[7].x);
            const d8 = new Date(markers1[8].x);
            const d9 = new Date(markers1[9].x);
            ty = [
                d9.getHours() + ":" + d9.getMinutes(), d8.getHours() + ":" + d8.getMinutes(), d7.getHours() + ":" + d7.getMinutes(), d6.getHours() + ":" + d6.getMinutes(),
                d5.getHours() + ":" + d5.getMinutes(), d4.getHours() + ":" + d4.getMinutes(), d3.getHours() + ":" + d3.getMinutes(), d2.getHours() + ":" + d2.getMinutes(),
                d1.getHours() + ":" + d1.getMinutes(), d0.getHours() + ":" + d0.getMinutes()
            ];

            const t0 = markers1[0].y;
            const t1 = markers1[1].y;
            const t2 = markers1[2].y;
            const t3 = markers1[3].y;
            const t4 = markers1[4].y;
            const t5 = markers1[5].y;
            const t6 = markers1[6].y;
            const t7 = markers1[7].y;
            const t8 = markers1[8].y;
            const t9 = markers1[9].y;
            tx = [t0, t1, t2, t3, t4, t5, t6, t7, t8, t9];
        }



        // p = markers.Data12;

        //   alert(p)
        //   $("#chart").text(markers[3].P);
        //$("#name").text(response[0].name);
        // $("#id").text(response[0].id);
    }

});
console.log(ty);

var canvas = document.getElementById('canvas1');
var ctx = canvas.getContext('2d');

var myChart = new Chart(ctx, {
    type: 'line',
    data: {
        //labels: JSON.stringify(dps),
        labels: ty,
        datasets: [{
            label: "V",
            data: tx,
            fill: false,
            backgroundColor: 'green',
            borderColor: 'green'
            // color: "#878BB6",

        }, {
            label: "I",
            data: cur,
            fill: false,
            backgroundColor: 'red',
            borderColor: 'red'
            //color: "#4ACAB4",

        },
        {
            label: "F",
            data: Fre,
            fill: false,
            backgroundColor: '#FF5733',
            borderColor: '#FF5733'
            //color: "#4ACAB4",
        },

        {
            label: "PKW",
            data: PKW1,
            fill: false,
            backgroundColor: '#0066cc',
            borderColor: '#0066cc'
            //color: "#4ACAB4",
        },
        {
            label: "PKVAR",
            data: PKVAR1,
            fill: false,
            backgroundColor: '#993399',
            borderColor: '#993399'
            //color: "#4ACAB4",
        },
        {
            label: "PKVA",
            data: PKVA1,
            fill: false,
            backgroundColor: '#333399',
            borderColor: '#333399'
            //color: "#4ACAB4",
        }
        ]
    },
    /*  options: {

    xAxes: [{
    type: 'time',
              time: {
    format: "HH:MM:SS",

}

}],


          /* xAxes: [{
    type: 'time',
               time: {

    unit: 'second',
tooltipFormat: 'h:mm:ss a'
}
}],*/

    // }
    options: {
        maintainAspectRatio: false,
        responsive: true,
        title: {
            display: true,
            position: 'top',
            text: 'Power Telemetry A-III Block'
        },
        legend: {
            display: true,
            position: 'bottom'
        },
        tooltips: {
            mode: 'index'
        },


        xAxes: [{
            type: 'time',
            time: {
                unit: 'second',
                displayFormats: {
                    second: 'h:mm:ss a'
                },
                tooltipFormat: 'h:mm:ss a'
            },
            /*type: 'time',
                                time: {
                        unit: 'second',
                    tooltipFormat: 'h:mm:ss a'
                }*/
        }],

    }

});

// last 30 Wh*@


google.charts.load('current', { 'packages': ['gauge'] });
google.charts.setOnLoadCallback(drawChart);
function resizeHandler() {
    chart.draw(data, options);
}
if (window.addEventListener) {
    window.addEventListener('resize', resizeHandler, false);
}
else if (window.attachEvent) {
    window.attachEvent('onresize', resizeHandler);
}
//  var d = 67;
// cr = markers[3].P;
function drawChart() {
    // var i = cr;
    //  var integer = parseFloat(cr);
    var data = google.visualization.arrayToDataTable([
        ['Label', 'Value'],
        ['Hours', parseFloat(twha30)],


    ]);

    var options = {
        //width: 900, height: 150,
        max: 720,
        redFrom: 650, redTo: 720,
        yellowFrom: 300, yellowTo: 650,
        greenFrom: 0, greenTo: 300,
        minorTicks: 5
    };

    var chart = new google.visualization.Gauge(document.getElementById('chartl30WH'));

    chart.draw(data, options);

    setInterval(function () {
        //var integer = parseFloat(cr);
        //data.setValue(0, 1, 40 + Math.round(60 * Math.random()));
        data.setValue(0, 1, parseFloat(twha30));
        chart.draw(data, options);
    }, 2000);

}



var chart = new Gauge("chartl30WH", {

    title: { text: "Last 30 Days Working Hours" },
    maximum: 720,
    data: { y: 0 } //gauge value change it

});
chart.render();
updateData(chart);  //Comment it out if you'r data will not change



// For dynamic data
function updateData(chart) {

    setInterval(function () {
        chart.gauge.data.y = twha30;
        chart.updateGauge();

    }, 3000);
}




//Function for gauge
function Gauge(containerId, gauge) {

    //Caluculation of remaining parameters to render gauge with the help of doughnut
    gauge.unoccupied = {

        y: gauge.maximum - gauge.data.y,
        color: "#DEDEDE",
        toolTipContent: null,
        highlightEnabled: false,
        click: function () { gauge.unoccupied.exploded = true; }

    };

    gauge.data.click = function () { gauge.data.exploded = true; };

    if (!gauge.data.color)
        gauge.data.color = "#F17925";

    gauge.valueText = { text: gauge.data.y.toString(), verticalAlign: "center", dockInsidePlotArea: true };

    var chart = new CanvasJS.Chart(containerId, {
        backgroundColor: "transparent",

        subtitles: [gauge.valueText],

        data: [{
            type: "doughnut",
            dataPoints: [
                { y: gauge.maximum, color: "transparent", toolTipContent: null },
                gauge.data,
                gauge.unoccupied
            ]
        }]

    });

    if (gauge.title)
        chart.options.title = gauge.title;

    chart.gauge = gauge;
    //For updating
    chart.updateGauge = function () {
        this.gauge.unoccupied.y = this.gauge.maximum - this.gauge.data.y;
        this.gauge.valueText.text = this.gauge.data.y.toString();

        this.render();
    }

    return chart;
}


// last 7 days WH*@


google.charts.load('current', { 'packages': ['gauge'] });
google.charts.setOnLoadCallback(drawChart);
function resizeHandler() {
    chart.draw(data, options);
}
if (window.addEventListener) {
    window.addEventListener('resize', resizeHandler, false);
}
else if (window.attachEvent) {
    window.attachEvent('onresize', resizeHandler);
}
//  var d = 67;
// cr = markers[3].P;
function drawChart() {
    // var i = cr;
    //  var integer = parseFloat(cr);
    var data = google.visualization.arrayToDataTable([
        ['Label', 'Value'],
        ['Hours', parseFloat(twha7)],


    ]);

    var options = {
        //width: 900, height: 150,
        max: 168,
        redFrom: 130, redTo: 168,
        yellowFrom: 70, yellowTo: 130,
        greenFrom: 0, greenTo: 70,
        minorTicks: 5
    };

    var chart = new google.visualization.Gauge(document.getElementById('chartl7WH'));

    chart.draw(data, options);

    setInterval(function () {
        //var integer = parseFloat(cr);
        //data.setValue(0, 1, 40 + Math.round(60 * Math.random()));
        data.setValue(0, 1, parseFloat(twha7));
        chart.draw(data, options);
    }, 2000);

}



var chart = new Gauge("chartl7WH", {

    title: { text: "Last 7 days Working Hours" },
    maximum: 168,
    data: { y: 0 } //gauge value change it

});
chart.render();
updateData(chart);  //Comment it out if you'r data will not change



// For dynamic data
function updateData(chart) {

    setInterval(function () {
        chart.gauge.data.y = twha7;
        chart.updateGauge();

    }, 3000);
}




//Function for gauge
function Gauge(containerId, gauge) {

    //Caluculation of remaining parameters to render gauge with the help of doughnut
    gauge.unoccupied = {

        y: gauge.maximum - gauge.data.y,
        color: "#DEDEDE",
        toolTipContent: null,
        highlightEnabled: false,
        click: function () { gauge.unoccupied.exploded = true; }

    };

    gauge.data.click = function () { gauge.data.exploded = true; };

    if (!gauge.data.color)
        gauge.data.color = "#ccaa14";

    gauge.valueText = { text: gauge.data.y.toString(), verticalAlign: "center", dockInsidePlotArea: true };

    var chart = new CanvasJS.Chart(containerId, {
        backgroundColor: "transparent",

        subtitles: [gauge.valueText],

        data: [{
            type: "doughnut",
            dataPoints: [
                { y: gauge.maximum, color: "transparent", toolTipContent: null },
                gauge.data,
                gauge.unoccupied
            ]
        }]

    });

    if (gauge.title)
        chart.options.title = gauge.title;

    chart.gauge = gauge;
    //For updating
    chart.updateGauge = function () {
        this.gauge.unoccupied.y = this.gauge.maximum - this.gauge.data.y;
        this.gauge.valueText.text = this.gauge.data.y.toString();

        this.render();
    }

    return chart;
}


// Today Working HOur*@


google.charts.load('current', { 'packages': ['gauge'] });
google.charts.setOnLoadCallback(drawChart);
function resizeHandler() {
    chart.draw(data, options);
}
if (window.addEventListener) {
    window.addEventListener('resize', resizeHandler, false);
}
else if (window.attachEvent) {
    window.attachEvent('onresize', resizeHandler);
}
//  var d = 67;
// cr = markers[3].P;
function drawChart() {
    // var i = cr;
    //  var integer = parseFloat(cr);
    var data = google.visualization.arrayToDataTable([
        ['Label', 'Value'],
        ['Hours', parseFloat(twha)],


    ]);

    var options = {
        //width: 900, height: 150,
        max: 24,
        redFrom: 20, redTo: 24,
        yellowFrom: 10, yellowTo: 20,
        greenFrom: 0, greenTo: 10,
        minorTicks: 5
    };

    var chart = new google.visualization.Gauge(document.getElementById('chartTWH'));

    chart.draw(data, options);

    setInterval(function () {
        //var integer = parseFloat(cr);
        //data.setValue(0, 1, 40 + Math.round(60 * Math.random()));
        data.setValue(0, 1, parseFloat(twha));
        chart.draw(data, options);
    }, 2000);

}



var chart = new Gauge("chartTWH", {

    title: { text: "Today Working Hours" },
    maximum: 24,
    data: { y: 0 } //gauge value change it

});
chart.render();
updateData(chart);  //Comment it out if you'r data will not change



// For dynamic data
function updateData(chart) {

    setInterval(function () {
        chart.gauge.data.y = twha;
        chart.updateGauge();

    }, 3000);
}




//Function for gauge
function Gauge(containerId, gauge) {

    //Caluculation of remaining parameters to render gauge with the help of doughnut
    gauge.unoccupied = {

        y: gauge.maximum - gauge.data.y,
        color: "#DEDEDE",
        toolTipContent: null,
        highlightEnabled: false,
        click: function () { gauge.unoccupied.exploded = true; }

    };

    gauge.data.click = function () { gauge.data.exploded = true; };

    if (!gauge.data.color)
        gauge.data.color = "#235658";

    gauge.valueText = { text: gauge.data.y.toString(), verticalAlign: "center", dockInsidePlotArea: true };

    var chart = new CanvasJS.Chart(containerId, {
        backgroundColor: "transparent",

        subtitles: [gauge.valueText],

        data: [{
            type: "doughnut",
            dataPoints: [
                { y: gauge.maximum, color: "transparent", toolTipContent: null },
                gauge.data,
                gauge.unoccupied
            ]
        }]

    });

    if (gauge.title)
        chart.options.title = gauge.title;

    chart.gauge = gauge;
    //For updating
    chart.updateGauge = function () {
        this.gauge.unoccupied.y = this.gauge.maximum - this.gauge.data.y;
        this.gauge.valueText.text = this.gauge.data.y.toString();

        this.render();
    }

    return chart;
}


// Last 30 day WF*@


google.charts.load('current', { 'packages': ['gauge'] });
google.charts.setOnLoadCallback(drawChart);
function resizeHandler() {
    chart.draw(data, options);
}
if (window.addEventListener) {
    window.addEventListener('resize', resizeHandler, false);
}
else if (window.attachEvent) {
    window.attachEvent('onresize', resizeHandler);
}
//  var d = 67;
// cr = markers[3].P;
function drawChart() {
    // var i = cr;
    //  var integer = parseFloat(cr);
    var data = google.visualization.arrayToDataTable([
        ['Label', 'Value'],
        ['Water Flow', parseFloat(twf30)],


    ]);

    var options = {
        //width: 900, height: 150,
        max: 900000,
        redFrom: 700000, redTo: 900000,
        yellowFrom: 600000, yellowTo: 700000,
        greenFrom: 0, greenTo: 600000,
        minorTicks: 5
    };

    var chart = new google.visualization.Gauge(document.getElementById('chartl30WF'));

    chart.draw(data, options);

    setInterval(function () {
        //var integer = parseFloat(cr);
        //data.setValue(0, 1, 40 + Math.round(60 * Math.random()));
        data.setValue(0, 1, parseFloat(twf30));
        chart.draw(data, options);
    }, 2000);

}



var chart = new Gauge("chartl30WF", {

    title: { text: "Last 30 Days Water Flow" },
    maximum: 900000,
    data: { y: 0 } //gauge value change it

});
chart.render();
updateData(chart);  //Comment it out if you'r data will not change



// For dynamic data
function updateData(chart) {

    setInterval(function () {


        chart.gauge.data.y = twf30;
        chart.updateGauge();
    }, 3000);
}




//Function for gauge
function Gauge(containerId, gauge) {

    //Caluculation of remaining parameters to render gauge with the help of doughnut
    gauge.unoccupied = {

        y: gauge.maximum - gauge.data.y,
        color: "#DEDEDE",
        toolTipContent: null,
        highlightEnabled: false,
        click: function () { gauge.unoccupied.exploded = true; }

    };

    gauge.data.click = function () { gauge.data.exploded = true; };

    if (!gauge.data.color)
        gauge.data.color = "#F17925";

    gauge.valueText = { text: gauge.data.y.toString(), verticalAlign: "center", dockInsidePlotArea: true };

    var chart = new CanvasJS.Chart(containerId, {
        backgroundColor: "transparent",

        subtitles: [gauge.valueText],

        data: [{
            type: "doughnut",
            dataPoints: [
                { y: gauge.maximum, color: "transparent", toolTipContent: null },
                gauge.data,
                gauge.unoccupied
            ]
        }]

    });

    if (gauge.title)
        chart.options.title = gauge.title;

    chart.gauge = gauge;
    //For updating
    chart.updateGauge = function () {
        this.gauge.unoccupied.y = this.gauge.maximum - this.gauge.data.y;
        this.gauge.valueText.text = this.gauge.data.y.toString();

        this.render();
    }

    return chart;
}


// Last 7 days WF*@


google.charts.load('current', { 'packages': ['gauge'] });
google.charts.setOnLoadCallback(drawChart);
function resizeHandler() {
    chart.draw(data, options);
}
if (window.addEventListener) {
    window.addEventListener('resize', resizeHandler, false);
}
else if (window.attachEvent) {
    window.attachEvent('onresize', resizeHandler);
}
//  var d = 67;
// cr = markers[3].P;
function drawChart() {
    // var i = cr;
    //  var integer = parseFloat(cr);
    var data = google.visualization.arrayToDataTable([
        ['Label', 'Value'],
        ['Water Flow', parseFloat(twf7)],

    ]);

    var options = {
        //  width: 900, height: 150,
        max: 800000,
        redFrom: 700000, redTo: 800000,
        yellowFrom: 600000, yellowTo: 700000,
        greenFrom: 0, greenTo: 600000,
        minorTicks: 5
    };

    var chart = new google.visualization.Gauge(document.getElementById('chartl7WF'));

    chart.draw(data, options);

    setInterval(function () {
        //var integer = parseFloat(cr);
        //data.setValue(0, 1, 40 + Math.round(60 * Math.random()));
        data.setValue(0, 1, parseFloat(twf7));
        chart.draw(data, options);
    }, 2000);

}



var chart = new Gauge("chartl7WF", {

    title: { text: "Last 7 Days Water Flow" },
    maximum: 800000,
    data: { y: 0 } //gauge value change it

});
chart.render();
updateData(chart);  //Comment it out if you'r data will not change



// For dynamic data
function updateData(chart) {

    setInterval(function () {

        //  console.log(twf7);
        chart.gauge.data.y = twf7;
        chart.updateGauge();

    }, 3000);
}




//Function for gauge
function Gauge(containerId, gauge) {

    //Caluculation of remaining parameters to render gauge with the help of doughnut
    gauge.unoccupied = {

        y: gauge.maximum - gauge.data.y,
        color: "#DEDEDE",
        toolTipContent: null,
        highlightEnabled: false,
        click: function () { gauge.unoccupied.exploded = true; }

    };

    gauge.data.click = function () { gauge.data.exploded = true; };

    if (!gauge.data.color)
        gauge.data.color = "#ccaa14";

    gauge.valueText = { text: gauge.data.y.toString(), verticalAlign: "center", dockInsidePlotArea: true };

    var chart = new CanvasJS.Chart(containerId, {
        backgroundColor: "transparent",

        subtitles: [gauge.valueText],

        data: [{
            type: "doughnut",
            dataPoints: [
                { y: gauge.maximum, color: "transparent", toolTipContent: null },
                gauge.data,
                gauge.unoccupied
            ]
        }]

    });

    if (gauge.title)
        chart.options.title = gauge.title;

    chart.gauge = gauge;
    //For updating
    chart.updateGauge = function () {
        this.gauge.unoccupied.y = this.gauge.maximum - this.gauge.data.y;
        this.gauge.valueText.text = this.gauge.data.y.toString();

        this.render();
    }

    return chart;
}


// Todat Water Flow*@


google.charts.load('current', { 'packages': ['gauge'] });
google.charts.setOnLoadCallback(drawChart);
function resizeHandler() {
    chart.draw(data, options);
}
if (window.addEventListener) {
    window.addEventListener('resize', resizeHandler, false);
}
else if (window.attachEvent) {
    window.attachEvent('onresize', resizeHandler);
}
//  var d = 67;
// cr = markers[3].P;
function drawChart() {
    // var i = cr;
    //  var integer = parseFloat(cr);
    var data = google.visualization.arrayToDataTable([
        ['Label', 'Value'],
        ['Water Flow', parseFloat(twf)],

    ]);

    var options = {
        width: 1700, height: 150,
        max: 120000,
        redFrom: 100000, redTo: 120000,
        yellowFrom: 70000, yellowTo: 100000,
        greenFrom: 0, greenTo: 70000,
        minorTicks: 5
    };

    var chart = new google.visualization.Gauge(document.getElementById('chartTWF'));

    chart.draw(data, options);

    setInterval(function () {
        //var integer = parseFloat(cr);
        //data.setValue(0, 1, 40 + Math.round(60 * Math.random()));
        data.setValue(0, 1, parseFloat(twf));
        chart.draw(data, options);
    }, 2000);

}



var chart = new Gauge("chartTWF", {

    title: { text: "Today Water Flow" },
    maximum: 120000,
    data: { y: 0 } //gauge value change it

});
chart.render();
updateData(chart);  //Comment it out if you'r data will not change



// For dynamic data
function updateData(chart) {

    setInterval(function () {

        chart.gauge.data.y = twf;
        chart.updateGauge();

    }, 3000);
}




//Function for gauge
function Gauge(containerId, gauge) {

    //Caluculation of remaining parameters to render gauge with the help of doughnut
    gauge.unoccupied = {

        y: gauge.maximum - gauge.data.y,
        color: "#DEDEDE",
        toolTipContent: null,
        highlightEnabled: false,
        click: function () { gauge.unoccupied.exploded = true; }

    };

    gauge.data.click = function () { gauge.data.exploded = true; };

    if (!gauge.data.color)
        gauge.data.color = "#235658";

    gauge.valueText = { text: gauge.data.y.toString(), verticalAlign: "center", dockInsidePlotArea: true };

    var chart = new CanvasJS.Chart(containerId, {
        backgroundColor: "transparent",

        subtitles: [gauge.valueText],

        data: [{
            type: "doughnut",
            dataPoints: [
                { y: gauge.maximum, color: "transparent", toolTipContent: null },
                gauge.data,
                gauge.unoccupied
            ]
        }]

    });

    if (gauge.title)
        chart.options.title = gauge.title;

    chart.gauge = gauge;
    //For updating
    chart.updateGauge = function () {
        this.gauge.unoccupied.y = this.gauge.maximum - this.gauge.data.y;
        this.gauge.valueText.text = this.gauge.data.y.toString();

        this.render();
    }

    return chart;
}




var chart = new Gauge("chart", {

    title: { text: "Pressure Guage" },
    maximum: 4,

    data: { y: 0 } //gauge value change it

});
chart.render();
updateData(chart);  //Comment it out if you'r data will not change



// For dynamic data
function updateData(chart) {

    setInterval(function () {

        chart.gauge.data.y = cr;


        chart.updateGauge();

    }, 3000);
}




//Function for gauge
function Gauge(containerId, gauge) {

    //Caluculation of remaining parameters to render gauge with the help of doughnut
    gauge.unoccupied = {

        y: gauge.maximum - gauge.data.y,
        color: "#DEDEDE",
        toolTipContent: null,
        highlightEnabled: false,
        click: function () { gauge.unoccupied.exploded = true; }

    };

    gauge.data.click = function () { gauge.data.exploded = true; };

    if (!gauge.data.color)
        gauge.data.color = "#F17925";

    gauge.valueText = { text: gauge.data.y.toString(), verticalAlign: "center", dockInsidePlotArea: true };

    var chart = new CanvasJS.Chart(containerId, {
        backgroundColor: "transparent",

        subtitles: [gauge.valueText],

        data: [{
            type: "doughnut",
            dataPoints: [
                { y: gauge.maximum, color: "transparent", toolTipContent: null },
                gauge.data,
                gauge.unoccupied
            ]
        }]

    });

    if (gauge.title)
        chart.options.title = gauge.title;

    chart.gauge = gauge;
    //For updating
    chart.updateGauge = function () {
        this.gauge.unoccupied.y = this.gauge.maximum - this.gauge.data.y;
        this.gauge.valueText.text = this.gauge.data.y.toString();

        this.render();
    }

    return chart;
}




google.charts.load('current', { 'packages': ['gauge'] });
google.charts.setOnLoadCallback(drawChart);
//  var d = 67;
// cr = markers[3].P;
function drawChart() {
    // var i = cr;
    //  var integer = parseFloat(cr);
    var data = google.visualization.arrayToDataTable([
        ['Label', 'Value'],
        ['Pressure', parseFloat(cr)],

    ]);

    var options = {
        width: 600, height: 150,
        max: 5,
        redFrom: 2, redTo: 5,
        yellowFrom: 1, yellowTo: 2,
        greenFrom: 0, greenTo: 1,
        minorTicks: 5
    };

    var chart = new google.visualization.Gauge(document.getElementById('chart'));

    chart.draw(data, options);

    setInterval(function () {
        //var integer = parseFloat(cr);
        //data.setValue(0, 1, 40 + Math.round(60 * Math.random()));
        data.setValue(0, 1, parseFloat(cr));
        chart.draw(data, options);
    }, 2000);

}

// PF*@


google.charts.load('current', { 'packages': ['gauge'] });
google.charts.setOnLoadCallback(drawChart);
//  var d = 67;
// cr = markers[3].P;
function drawChart() {
    // var i = cr;
    //  var integer = parseFloat(cr);
    var data = google.visualization.arrayToDataTable([
        ['Label', 'Value'],
        ['PF', parseFloat(pr)],

    ]);

    var options = {
        title: 'Power Factor',
        width: 600, height: 150,
        max: 1,
        redFrom: 0.1, redTo: 0.5,
        yellowFrom: 0.5, yellowTo: 0.75,
        greenFrom: 0.75, greenTo: 1,
        minorTicks: 5
    };

    var chart = new google.visualization.Gauge(document.getElementById('chartPF'));

    chart.draw(data, options);

    setInterval(function () {
        //var integer = parseFloat(cr);
        //data.setValue(0, 1, 40 + Math.round(60 * Math.random()));
        data.setValue(0, 1, parseFloat(pr));
        chart.draw(data, options);
    }, 2000);

}



var chart = new Gauge("chartPF", {

    title: { text: "Power Factor" },
    maximum: 1,
    data: { y: 0 } //gauge value change it

});
chart.render();
updateData(chart);  //Comment it out if you'r data will not change



// For dynamic data
function updateData(chart) {

    setInterval(function () {

        // chart.gauge.data.y = Math.ceil(Math.random() * chart.gauge.maximum);
        chart.gauge.data.y = pr;
        chart.updateGauge();

    }, 4000);
}




//Function for gauge
function Gauge(containerId, gauge) {

    //Caluculation of remaining parameters to render gauge with the help of doughnut
    gauge.unoccupied = {

        y: gauge.maximum - gauge.data.y,
        color: "#DEDEDE",
        toolTipContent: null,
        highlightEnabled: false,
        click: function () { gauge.unoccupied.exploded = true; }

    };

    gauge.data.click = function () { gauge.data.exploded = true; };

    if (!gauge.data.color)
        gauge.data.color = "#ccaa14";

    gauge.valueText = { text: gauge.data.y.toString(), verticalAlign: "center", dockInsidePlotArea: true };

    var chart = new CanvasJS.Chart(containerId, {
        backgroundColor: "transparent",

        subtitles: [gauge.valueText],

        data: [{
            type: "doughnut",
            dataPoints: [
                { y: gauge.maximum, color: "transparent", toolTipContent: null },
                gauge.data,
                gauge.unoccupied
            ]
        }]

    });

    if (gauge.title)
        chart.options.title = gauge.title;

    chart.gauge = gauge;
    //For updating
    chart.updateGauge = function () {
        this.gauge.unoccupied.y = this.gauge.maximum - this.gauge.data.y;
        this.gauge.valueText.text = this.gauge.data.y.toString();

        this.render();
    }

    return chart;
}


//VM*@


google.charts.load('current', { 'packages': ['gauge'] });
google.charts.setOnLoadCallback(drawChart);
//  var d = 67;
// cr = markers[3].P;
function drawChart() {
    // var i = cr;
    //  var integer = parseFloat(cr);
    var data = google.visualization.arrayToDataTable([
        ['Label', 'Value'],
        ['Vibration', parseFloat(vm)],

    ]);

    var options = {
        title: 'chartTitle',
        width: 600, height: 150,
        max: 1,
        redFrom: 0.6, redTo: 1,
        yellowFrom: 0.3, yellowTo: 0.6,
        greenFrom: 0, greenTo: 0.3,
        minorTicks: 5
    };

    var chart = new google.visualization.Gauge(document.getElementById('chartVM'));
    google.visualization.events.addListener(chart, 'ready', function () {
        var svgNS = $('#chart svg')[0].namespaceURI;
        var chartTitle = $('#chart text').filter(':contains("chartTitle")')[0];
        $(chartTitle).text('');

        var textStyle = document.createElementNS(svgNS, 'tspan');
        $(textStyle).attr('fill', '#ff0000');
        $(textStyle).attr('font-weight', 'bold');
        $(textStyle).text('Chart ');
        $(chartTitle).append(textStyle);

        var textStyle = document.createElementNS(svgNS, 'tspan');
        $(textStyle).attr('fill', '#0000ff');
        $(textStyle).attr('font-weight', 'normal');
        $(textStyle).text('Title');
        $(chartTitle).append(textStyle);
    });

    chart.draw(data, options);

    setInterval(function () {
        //var integer = parseFloat(cr);
        //data.setValue(0, 1, 40 + Math.round(60 * Math.random()));
        data.setValue(0, 1, parseFloat(vm));
        chart.draw(data, options);
    }, 2000);

}




var chart = new Gauge("chartVM", {

    title: { text: "Vibration Meter" },
    maximum: -100,
    data: { y: 0 } //gauge value change it

});
chart.render();
updateData(chart);  //Comment it out if you'r data will not change



// For dynamic data
function updateData(chart) {

    setInterval(function () {

        chart.gauge.data.y = vm;
        chart.updateGauge();

    }, 5000);
}




//Function for gauge
function Gauge(containerId, gauge) {

    //Caluculation of remaining parameters to render gauge with the help of doughnut
    gauge.unoccupied = {

        y: gauge.maximum - gauge.data.y,
        color: "#DEDEDE",
        toolTipContent: null,
        highlightEnabled: false,
        click: function () { gauge.unoccupied.exploded = true; }

    };

    gauge.data.click = function () { gauge.data.exploded = true; };

    if (!gauge.data.color)
        gauge.data.color = "#235658";

    gauge.valueText = { text: gauge.data.y.toString(), verticalAlign: "center", dockInsidePlotArea: true };

    var chart = new CanvasJS.Chart(containerId, {
        backgroundColor: "transparent",

        subtitles: [gauge.valueText],

        data: [{
            type: "doughnut",
            dataPoints: [
                { y: gauge.maximum, color: "transparent", toolTipContent: null },
                gauge.data,
                gauge.unoccupied
            ]
        }]

    });

    if (gauge.title)
        chart.options.title = gauge.title;

    chart.gauge = gauge;
    //For updating
    chart.updateGauge = function () {
        this.gauge.unoccupied.y = this.gauge.maximum - this.gauge.data.y;
        this.gauge.valueText.text = this.gauge.data.y.toString();

        this.render();
    }

    return chart;
}




var chart = new Gauge("chartPL", {

    title: { text: "Primping Level" },
    maximum: 1,
    data: { y: 0 } //gauge value change it

});
chart.render();
updateData(chart);  //Comment it out if you'r data will not change



// For dynamic data
function updateData(chart) {

    setInterval(function () {



        chart.gauge.data.y = pl;
        chart.updateGauge();

    }, 6000);
}




//Function for gauge
function Gauge(containerId, gauge) {

    //Caluculation of remaining parameters to render gauge with the help of doughnut
    gauge.unoccupied = {

        y: gauge.maximum - gauge.data.y,
        color: "#DEDEDE",
        toolTipContent: null,
        highlightEnabled: false,
        click: function () { gauge.unoccupied.exploded = true; }

    };

    gauge.data.click = function () { gauge.data.exploded = true; };

    if (!gauge.data.color)
        gauge.data.color = "#F17925";

    gauge.valueText = { text: gauge.data.y.toString(), verticalAlign: "center", dockInsidePlotArea: true };

    var chart = new CanvasJS.Chart(containerId, {
        backgroundColor: "transparent",

        subtitles: [gauge.valueText],

        data: [{
            type: "doughnut",
            dataPoints: [
                { y: gauge.maximum, color: "transparent", toolTipContent: null },
                gauge.data,
                gauge.unoccupied
            ]
        }]

    });

    if (gauge.title)
        chart.options.title = gauge.title;

    chart.gauge = gauge;
    //For updating
    chart.updateGauge = function () {
        this.gauge.unoccupied.y = this.gauge.maximum - this.gauge.data.y;
        this.gauge.valueText.text = this.gauge.data.y.toString();

        this.render();
    }

    return chart;
}


// Max Current*@

// var el_up = document.getElementById("GFG_UP");
var el_down = document.getElementById("IV1");

var markers = [];
$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetDataI",
    data: "{}",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {

        markers = eval(resp.d);
        el_down.innerHTML =
            Math.max.apply(Math, markers.map(function (o) {
                return Math.round(o.y * 10) / 10;;
            }));
    }
});
//el_up.innerHTML = JSON.stringify(array);




// var el_up = document.getElementById("GFG_UP");
var el_down = document.getElementById("MV1");

var markers = [];
$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetVmax",
    data: "{}",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {

        markers = eval(resp.d);
        el_down.innerHTML =
            Math.max.apply(Math, markers.map(function (o) {
                return Math.round(o.y * 10) / 10;
            }));
    }
});
//el_up.innerHTML = JSON.stringify(array);




//Today Avg. v*@

// var el_up = document.getElementById("GFG_UP");
var el_down = document.getElementById("LVT");

var markers = [];
$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetVmax",
    data: "{}",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {

        markers = eval(resp.d);
        av = 0;
        total = 0
        for (i = 0; i < markers.length; i++) {  //loop through the array
            total += markers[i].y;  //Do the math!
        }
        av = total / markers.length;
        el_down.innerHTML = Math.round(av * 10) / 10;

        // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
    }
});
//el_up.innerHTML = JSON.stringify(array);

//Today Avg.I*@

// var el_up = document.getElementById("GFG_UP");
var el_down = document.getElementById("LIT");

var markers = [];
$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetDataI",
    data: "{}",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {

        markers = eval(resp.d);
        av = 0;
        total = 0
        for (i = 0; i < markers.length; i++) {  //loop through the array
            total += markers[i].y;  //Do the math!
        }
        av = total / markers.length;
        el_down.innerHTML = Math.round(av * 10) / 10;

        // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
    }
});
//el_up.innerHTML = JSON.stringify(array);


// var el_up = document.getElementById("GFG_UP");
var el_down = document.getElementById("LI7");

var markers = [];
$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetI7",
    data: "{}",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {

        markers = eval(resp.d);
        av = 0;
        total = 0
        for (i = 0; i < markers.length; i++) {  //loop through the array
            total += markers[i].y;  //Do the math!
        }
        av = total / markers.length;
        el_down.innerHTML = Math.round(av * 10) / 10;

        // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
    }
});
//el_up.innerHTML = JSON.stringify(array);


// var el_up = document.getElementById("GFG_UP");
var el_down = document.getElementById("LI30");

var markers = [];
$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetI30",
    data: "{}",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {

        markers = eval(resp.d);
        av = 0;
        total = 0
        for (i = 0; i < markers.length; i++) {  //loop through the array
            total += markers[i].y;  //Do the math!
        }
        av = total / markers.length;
        el_down.innerHTML = Math.round(av * 10) / 10;

        // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
    }
});
//el_up.innerHTML = JSON.stringify(array);



// var el_up = document.getElementById("GFG_UP");
var el_down = document.getElementById("LV7");

var markers = [];
$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetV7",
    data: "{}",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {

        markers = eval(resp.d);
        av = 0;
        total = 0
        for (i = 0; i < markers.length; i++) {  //loop through the array
            total += markers[i].y;  //Do the math!
        }
        av = total / markers.length;
        el_down.innerHTML = Math.round(av * 10) / 10;

        // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
    }
});
//el_up.innerHTML = JSON.stringify(array);


// var el_up = document.getElementById("GFG_UP");
var el_down = document.getElementById("LV30");

var markers = [];
$.ajax({
    type: "POST",
    url: "../savedata.asmx/GetV30",
    data: "{}",

    contentType: "application/json; charset=utf-8",
    dataType: "json",

    async: false,
    success: function (resp) {

        markers = eval(resp.d);
        av = 0;
        total = 0
        for (i = 0; i < markers.length; i++) {  //loop through the array
            total += markers[i].y;  //Do the math!
        }
        av = total / markers.length;
        el_down.innerHTML = Math.round(av * 10) / 10;

        // Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
    }
});
                                                    //el_up.innerHTML = JSON.stringify(array);
