function ConfirmDelete() {
    var bResult = true
    if (!window.confirm('Are you sure you want to delete this record?')) {
        bResult = false
    }
    return bResult
}

function IsMaintenanceBillDateValid(oSrc, args) {
    if (document.getElementById(_ClientlblUpdateSuccess)) {
        document.getElementById(_ClientlblUpdateSuccess).innerHTML = "";
        document.getElementById(_ClientlblUpdateSuccess).innerText = "";
    }
    var sMaintenanceDate = (document.getElementById(_ClienttxtMaintenanceDate).value)
    var sBillDate = document.getElementById(_ClienttxtBillDate).value
    if (sMaintenanceDate == "") {
        document.getElementById(_ClientcstMaintenanceBillDateValidation).errormessage = "Maintenance Date should not be blank.";
        args.IsValid = false;
        return false;
    }

    if (sMaintenanceDate != "") {
        var MaintenanceDt = new Date(convertdate((document.getElementById(_ClienttxtMaintenanceDate).value)))
        var BillDt = new Date(convertdate(document.getElementById(_ClienttxtBillDate).value))
        var TodaysDt = new Date()

        if (MaintenanceDt >= TodaysDt) {
            oSrc.errormessage = "Maintenance Date should be less than or equal to today's date.";
            document.getElementById(_ClientcstMaintenanceBillDateValidation).errormessage = "Maintenance Date should be less than or equal to today's date.";
            args.IsValid = false;
            return false;
        }
    }
}

function IsBillDateValid(oSrc, args) {
    if (document.getElementById(_ClientlblUpdateSuccess)) {
        document.getElementById(_ClientlblUpdateSuccess).innerHTML = "";
        document.getElementById(_ClientlblUpdateSuccess).innerText = "";
    }
    var sMaintenanceDate = (document.getElementById(_ClienttxtMaintenanceDate).value)
    var sBillDate = document.getElementById(_ClienttxtBillDate).value

    if (sBillDate == "") {
        document.getElementById(_ClientcstBillDateValidation).errormessage = "Bill Date should not be blank.";
        args.IsValid = false;
        return false;
    }

    var TodaysDt = new Date()
    var MaintenanceDt = new Date(convertdate((document.getElementById(_ClienttxtMaintenanceDate).value)))
    var BillDt = new Date(convertdate(document.getElementById(_ClienttxtBillDate).value))

    if (sBillDate != "" && BillDt > TodaysDt) {
        oSrc.errormessage = "Bill Date should be less than or equal to today's date.";
        document.getElementById(_ClientcstBillDateValidation).errormessage = "Bill Date should be less than or equal to today's date.";
        args.IsValid = false;
        return false;
    }
    
    if (sBillDate != "" && BillDt > MaintenanceDt) {
        oSrc.errormessage = "Bill date should be less than or equal to Maintenance Date.";
        document.getElementById(_ClientcstBillDateValidation).errormessage = "Bill date should be less than or equal to Maintenance Date.";
        args.IsValid = false;
        return false;
    }
}

function IsVehicleNoSelected(oSrc, args) {
    if (document.getElementById(_ClientlblUpdateSuccess)) {
        document.getElementById(_ClientlblUpdateSuccess).innerHTML = "";
        document.getElementById(_ClientlblUpdateSuccess).innerText = "";
    }
    var sVehicleNo = (document.getElementById(_ClienttxtVehicleNo).value)

    if (sVehicleNo == "" || sVehicleNo == "0") {
        document.getElementById(_ClientcstVehicleNoValidation).errormessage = "Vehicle Number should be selected.";
        args.IsValid = false;
        return false;
    }
}



