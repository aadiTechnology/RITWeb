$.ajaxSetup({ cache: false });
$.ajaxSetup({ async: false });

window.onload = onPageLoad;

function onPageLoad() {
    SetPageDefaults();
    collapseSidebarAsPerScreenWidth();
}

$(window).resize(
    function () {
        collapseSidebarAsPerScreenWidth();
    }
);
/* This function is used to read query string.*/
    function GetQueryStringParameters() {
        var vars = [], hash;
        var hashes = window.location.href.slice(href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    }

    /*This function is used to collapse screen menu. */
function collapseSidebarAsPerScreenWidth() {
    if ($(window).width() < 1200)
        $("#sidebar").addClass("sidebar menu-min");
    else
        $("#sidebar").removeClass("menu-min");
}

/* This function is used to set active menu.*/
function SetActiveMenu(navUserManagement) {
    $(".nav.nav-list li").removeClass("active");
    $("#" + navUserManagement).addClass("active");
}

/* This function is used to apply padding to lable on pop up.*/
function applyPaddingToReadonlyFields() {
    $(".k-edit-field").filter(function () { return this.childElementCount == 0; }).css('padding-top', '6px');
}

/*This function is used to make all fields read only.*/
function applyReadOnlyEffectToEditPopup() {
    $(".k-edit-form-container textarea").replaceWith(function () {
        return '<lable class=' + this.className + '>' + this.value + '</lable>';
    });
    $(".k-edit-form-container input[type=text]").replaceWith(function () {
        return '<lable class=' + this.className + '>' + this.value + '</lable>';
    });

    $(".k-edit-form-container .k-widget.k-datepicker.k-header").replaceWith(function () {
        return this.childNodes[0].childNodes[0];
    });

    $(".k-edit-form-container .k-widget.k-dropdown.k-header").replaceWith(function () {
        return '<lable class="k-input">' + $(this.childNodes[0].childNodes[0]).text() + '</lable>';
    });

    $(".k-widget .k-timepicker").replaceWith(function () {
        return '<lable class="k-input">' + $(this.childNodes[0].childNodes[0]).text() + '</lable>';
    });

    $(".k-edit-form-container .k-edit-field").filter(function () { return this.childElementCount >= 1; }).css('padding-top', '5px');
    $(".k-button.k-button-icontext.k-grid-update").hide();
}

 /*This function is used to show alert message*/
function showAlert(alertText) {
    $.gritter.add({
        title: alertText,
        image: '../assets/avatars/avatar3.png',
        sticky: false,
        class_name: 'gritter-warning gritter-light gritter-center'
    });
}

/*This function is used to show alrt pop up for error.*/
function showError(errorMsg) {
    $.gritter.add({
        title: errorMsg,
        image: '../assets/avatars/avatar3.png',
        sticky: false,
        class_name: 'gritter-error gritter-light gritter-center'
    });
}

/*This function is used to show kendo alert.*/
window.kendoAlert = function (title, message) {
    var win = $('<div>');
    var options = {
        modal: true,
        pinned: true,
        resizeable: false,
        title: title || 'Alert',
        minWidth: '300px'
    };
    win.kendoWindow(options)
     .getKendoWindow()
     .content(message)
     .center()
     .open();
};

/*This function is used to show required field mark(*) for required fields*/
function showRequiredFields(textBoxControls, dropDownControls, textareaControls) {
$(".k-edit-form-container input[type=text]").each(function () {
       if (textBoxControls.split(",").indexOf($(this).closest(".k-edit-field").attr("data-container-for")) > -1) {
           $(this).closest(".k-edit-field").append('<span class="required">*</span>');
       }
   });

   $(".k-edit-form-container .k-widget.k-dropdown.k-header").each(function () {
       if (dropDownControls.split(",").indexOf($(this).closest(".k-edit-field").attr("data-container-for")) > -1) {
           $(this).closest(".k-edit-field").append('<span class="required">*</span>');
       }
   });


   $(".k-edit-form-container textarea").each(function () {
       if (textareaControls.split(",").indexOf($(this).closest(".k-edit-field").attr("data-container-for")) > -1) {
           $(this).closest(".k-edit-field").append('<span class="required">*</span>');
       }
   });
}

/*This function is used to get json date stamp.*/
function GetDateFromJsonTimeStamp(timestamp) {
    var date = new Date(timestamp);
    var month = new Array();
    month[0] = "Jan";
    month[1] = "Feb";
    month[2] = "Mar";
    month[3] = "Apr";
    month[4] = "May";
    month[5] = "Jun";
    month[6] = "Jul";
    month[7] = "Aug";
    month[8] = "Sep";
    month[9] = "Oct";
    month[10] = "Nov";
    month[11] = "Dec";
    var time_t = "";
    (date.getHours() < 12) ? time_t = "AM" : time_t = "PM";
    month = month[date.getMonth()]
    var formattedDate = date.getDate() + "-" + month + "-" + date.getFullYear();
    hours = date.getHours();
    (hours == 0) ? hours = 12 : hours = hours;
    (hours > 12) ? hours = hours - 12 : hours = hours;
    hours = (hours < 10) ? "0" + hours : hours;
    var minutes = (date.getMinutes() < 10) ? "0" + date.getMinutes() : date.getMinutes();
    var seconds = (date.getSeconds() < 10) ? "0" + date.getSeconds() : date.getSeconds();
    var formattedTime = hours + ":" + minutes + " " + time_t;
    formattedDate = formattedDate + " " + formattedTime;

    var newMonth = (date.getMonth() < 10) ? "0" + date.getMonth() : date.getMonth();
    var newDate = (date.getDate() < 10) ? "0" + date.getDate() : date.getDate()
    newDate = new Date(date.getFullYear(), newMonth, newDate, hours, minutes);
    return newDate;
}

/*This function is used to get date format from json time stamp. */
function GetStringDateFromJsonTimeStamp(timestamp) {
    var date = new Date(timestamp);
    return GetStringDateFromDate(date);
}
/*This function is used to get date format.*/
function GetStringDateFromDate(date) {
    var month = new Array();
    month[0] = "Jan";
    month[1] = "Feb";
    month[2] = "Mar";
    month[3] = "Apr";
    month[4] = "May";
    month[5] = "Jun";
    month[6] = "Jul";
    month[7] = "Aug";
    month[8] = "Sep";
    month[9] = "Oct";
    month[10] = "Nov";
    month[11] = "Dec";
    var time_t = "";
    (date.getHours() < 12) ? time_t = "AM" : time_t = "PM";
    month = month[date.getMonth()]
    var formattedDate = date.getDate() + "-" + month + "-" + date.getFullYear();
    hours = date.getHours();
    (hours == 0) ? hours = 12 : hours = hours;
    (hours > 12) ? hours = hours - 12 : hours = hours;
    hours = (hours < 10) ? "0" + hours : hours;
    var minutes = (date.getMinutes() < 10) ? "0" + date.getMinutes() : date.getMinutes();
    var seconds = (date.getSeconds() < 10) ? "0" + date.getSeconds() : date.getSeconds();
    var formattedTime = hours + ":" + minutes + " " + time_t;
    formattedDate = formattedDate + " " + formattedTime;
    return formattedDate;
}

/*This function is used to display date in specified format.*/
function displayDate(date) {
    return kendo.toString(kendo.parseDate(date, "yyyy/MM/dd"), _clientDateFormat);
}

/*This function is used to display time in specified format.*/
function displayTime(date) {
    return kendo.toString(kendo.parseDate(date, "yyyy/MM/dd"), "hh:mm tt");
}
/*this function is used setset pop up tool tip and header text*/
function setEditPopupTooltip(data) {
    data.container.kendoWindow("title", data.model.isNew() ? "Add" : "Edit"); // Set appropriate title in according to mode.
    $('.k-grid-update').kendoTooltip({ content: data.model.isNew() ? "Add" : "Update" });
    $('.k-grid-cancel').kendoTooltip({ content: "Cancel" });
}
/*This function is used set button width */
function setButtonWidth() {    
    $(".k-toolbar.k-grid-toolbar").find('a').addClass("btnWidth");
    $(".k-edit-buttons").find('a').addClass("btnWidth");
}

/* This function is used to show or hide control in edit mode*/
function showHideControlsForEdit(fieldName, showField) {
    if (showField) {
        $($(".k-edit-label label").filter(function () { return $(this).attr("for") == fieldName; })[0].parentElement).show();
        $(".k-edit-field").filter(function () { return $(this).attr("data-container-for") == fieldName; }).show();
    }
    else {
        $($(".k-edit-label label").filter(function () { return $(this).attr("for") == fieldName; })[0].parentElement).hide();
        $(".k-edit-field").filter(function () { return $(this).attr("data-container-for") == fieldName; }).hide();
    }
}

/*This function is used to set pop up size*/
function SetPopUpSize() {
    setTimeout(function () {
        if ($(".k-edit-form-container").size() > 0)
            $(".k-edit-form-container").parent().data("kendoWindow").center();
        $(".k-edit-form-container").css("width", "850px");
        $(".k-edit-label").css("width", "15%");
        $(".k-edit-field").css("width", "77%");
        $(".k-edit-field").filter(function () { return $(this).attr("data-container-for") == "rateDetailsGrid" }).css('width', '96%');
    }, 100);
    setTimeout(function () {
        if ($(".k-edit-form-container").size() > 0)
            $(".k-edit-form-container").parent().data("kendoWindow").center();
    }
            , 200);
}