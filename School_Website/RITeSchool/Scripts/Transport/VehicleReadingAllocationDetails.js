var sReadingFrom = document.getElementById(_ClienttxtReadingFrom).value
var sReadingTo = document.getElementById(_ClienttxtReadingTo).value

function IsValidReadings(oSrc, args) {
    var sReadingFrom = document.getElementById(_ClienttxtReadingFrom).value
    var sReadingTo = document.getElementById(_ClienttxtReadingTo).value

    var iReadingFrom = parseFloat(sReadingFrom)
    var iReadingTo = parseFloat(sReadingTo)

    if (iReadingFrom > iReadingTo) {
        oSrc.errormessage = "Reading To should be greater than Readin From.";
        args.IsValid = false;
    }
    return !args.IsValid;
}

function ResetUpdateLbl() {
    if (document.getElementById(_clientlblUpdateSucess) != null) {
        document.getElementById(_clientlblUpdateSucess).style.display = "none"
    }
    if (document.getElementById(_clientlblErrorMsg) != null) {
        document.getElementById(_clientlblErrorMsg).style.display = "none"
        document.getElementById(_clientlblErrorMsg).innerHTML = ""
    }
}

function ConfirmDelete() {
    var bResult = true
    if (!window.confirm('Are you sure you want to delete this record?')) {
        bResult = false
    }
    return bResult
}