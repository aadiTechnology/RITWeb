
function ConfirmDelete() {
    var bResult = true
    if (!window.confirm('Are you sure you want to delete this record?')) {
        bResult = false
    }
    return bResult
}

function DisableCheckBox() {
    if (document.getElementById(_clientIsDefault).checked) {
        document.getElementById(_clientIsDefault).getAttribute("disabled")
    }
    else {
        document.getElementById(_clientIsDefault).disabled = false;
    }
}

function ResetUpdateLbl() {
    if (document.getElementById(_clientlblUpdateSucess) != null) {
        document.getElementById(_clientlblUpdateSucess).style.display = "none"
    }
    if (document.getElementById(_clientcst_LblErrMsg) != null) {
        document.getElementById(_clientcst_LblErrMsg).style.display = "none"
        document.getElementById(_clientcst_LblErrMsg).innerHTML = ""
    }
}

function isTimeValid(result) {
    var timeStr = document.getElementById(result).value;
    timeStr = timeStr.toUpperCase();
    if (trimAll(timeStr) == '')
        return false;

    var timePat = "\([0-2][0-9]):([0-5][0-9])$";

    if (timeStr != "") {
        if (Regs = timeStr.match(timePat)) {
            if ((Regs[1] > 23)) {
                return false;
            }
            else {
                return true;
            }
        }
        else {
            return false;
        }
    }
}

function IsValidStartTime(oSrc, args) {
    if (document.getElementById(_ClienttxtShiftStartTime)) {
        if (document.getElementById(_ClienttxtShiftStartTime).value != '') {
            if (!isTimeValid(_ClienttxtShiftStartTime)) {
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }
    }
    args.IsValid = true;
    return false;
}

function IsValidEndTime(oSrc, args) {
    if (document.getElementById(_ClienttxtShiftEndTime)) {
        if (document.getElementById(_ClienttxtShiftEndTime).value != '') {
            if (!isTimeValid(_ClienttxtShiftEndTime)) {
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }
    }
    args.IsValid = true;
    return false;
}

function IsValidHalfDayTime(oSrc, args) {
    if (document.getElementById(_ClienttxtHalfDayTime)) {
        if (document.getElementById(_ClienttxtHalfDayTime).value != '') {
            if (!isTimeValid(_ClienttxtHalfDayTime)) {
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }
    }
    args.IsValid = true;
    return false;
}

function IsValidLateMarkTime(oSrc, args) {
    if (document.getElementById(_ClienttxtLateMarkTime)) {
        if (document.getElementById(_ClienttxtLateMarkTime).value != '') {
            if (!isTimeValid(_ClienttxtLateMarkTime)) {
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }
    }
    args.IsValid = true;
    return false;
}

function IsValidStartEndTime(oSrc, args) {
    var sStrTime = document.getElementById(_ClienttxtShiftStartTime).value;
    var sEndTime = document.getElementById(_ClienttxtShiftEndTime).value;

    var sShiftStartTime = sStrTime.split(":");
    var sShiftEndTime = sEndTime.split(":");

    var date1 = new Date(parseInt("2001", 10), (parseInt("01", 10)) - 1, parseInt("01", 10), parseInt(sShiftStartTime[0], 10), parseInt(sShiftStartTime[1], 10));
    var date2 = new Date(parseInt("2001", 10), (parseInt("01", 10)) - 1, parseInt("01", 10), parseInt(sShiftEndTime[0], 10), parseInt(sShiftEndTime[1], 10));

    var sStartDate = date1.valueOf();
    var sEndDate = date2.valueOf();

    if (sStartDate > sEndDate) {
        oSrc.errormessage = "End time should be greater than start time.";
        args.IsValid = false;
        return true;
    } 
}

function IsValidLateMarkHalfDayTime(oSrc, args) {
    var sStrHalfDayTime = document.getElementById(_ClienttxtHalfDayTime).value;
    var sStrLateMarkTime = document.getElementById(_ClienttxtLateMarkTime).value;

    var sShiftStartTime = sStrHalfDayTime.split(":");
    var sShiftEndTime = sStrLateMarkTime.split(":");

    var date1 = new Date(parseInt("2001", 10), (parseInt("01", 10)) - 1, parseInt("01", 10), parseInt(sShiftStartTime[0], 10), parseInt(sShiftStartTime[1], 10));
    var date2 = new Date(parseInt("2001", 10), (parseInt("01", 10)) - 1, parseInt("01", 10), parseInt(sShiftEndTime[0], 10), parseInt(sShiftEndTime[1], 10));

    var sStartDate = date1.valueOf();
    var sEndDate = date2.valueOf();

    if (sStartDate < sEndDate) {
        oSrc.errormessage = "Late mark time should be less than halt day time.";
        args.IsValid = false;
        return true;
    }
}

function IsValidEndDayTime(oSrc, args) {
    var sHalfDayTime = document.getElementById(_ClienttxtHalfDayTime).value;
    var sShiftEndTime = document.getElementById(_ClienttxtShiftEndTime).value;

    var sStartTime = sHalfDayTime.split(":");
    var sEndTime = sShiftEndTime.split(":");

    var date1 = new Date(parseInt("2001", 10), (parseInt("01", 10)) - 1, parseInt("01", 10), parseInt(sStartTime[0], 10), parseInt(sStartTime[1], 10));
    var date2 = new Date(parseInt("2001", 10), (parseInt("01", 10)) - 1, parseInt("01", 10), parseInt(sEndTime[0], 10), parseInt(sEndTime[1], 10));

    var sStartDate = date1.valueOf();
    var sEndDate = date2.valueOf();

    if (sStartDate > sEndDate) {
        oSrc.errormessage = "End time should be greater than half day time.";
        args.IsValid = false;
        return true;
    }
}

function IsValidStartDayTime(oSrc, args) {
    var sShiftStartTime = document.getElementById(_ClienttxtShiftStartTime).value;
    var sHalfDayTime = document.getElementById(_ClienttxtHalfDayTime).value;

    var sStartTime = sShiftStartTime.split(":");
    var sEndTime = sHalfDayTime.split(":");

    var date1 = new Date(parseInt("2001", 10), (parseInt("01", 10)) - 1, parseInt("01", 10), parseInt(sStartTime[0], 10), parseInt(sStartTime[1], 10));
    var date2 = new Date(parseInt("2001", 10), (parseInt("01", 10)) - 1, parseInt("01", 10), parseInt(sEndTime[0], 10), parseInt(sEndTime[1], 10));

    var sStartDate = date1.valueOf();
    var sEndDate = date2.valueOf();

    if (sStartDate > sEndDate) {
        oSrc.errormessage = "Start time should be less than half day time.";
        args.IsValid = false;
        return true;
    }
}


function IsValidStartTimeAndLateMarkTime(oSrc, args) {
    var sShiftStartTime = document.getElementById(_ClienttxtShiftStartTime).value;
    var sLateMarkTime = document.getElementById(_ClienttxtLateMarkTime).value;

    var sStartTime = sShiftStartTime.split(":");
    var sEndTime = sLateMarkTime.split(":");

    var date1 = new Date(parseInt("2001", 10), (parseInt("01", 10)) - 1, parseInt("01", 10), parseInt(sStartTime[0], 10), parseInt(sStartTime[1], 10));
    var date2 = new Date(parseInt("2001", 10), (parseInt("01", 10)) - 1, parseInt("01", 10), parseInt(sEndTime[0], 10), parseInt(sEndTime[1], 10));

    var sStartDate = date1.valueOf();
    var sEndDate = date2.valueOf();

    if (sStartDate > sEndDate) {
        oSrc.errormessage = "Late mark time should be greater than start time.";
        args.IsValid = false;
        return true;
    }
}