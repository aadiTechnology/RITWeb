function ConfirmDelete() {
    var bResult = true
    if (!window.confirm('Are you sure you want to delete this record?')) {
        bResult = false
    }
    return bResult
}

function fnover(varname) {
    var objTXT = document.getElementById(varname)
    objTXT.style.borderWidth = "1"
    objTXT.style.borderColor = "maroon"
    objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)"
}

function fnout(varname) {
    var objTXT = document.getElementById(varname)
    objTXT.style.borderWidth = "1"
    objTXT.style.borderColor = "#a3c07b"
    objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)"
}

function btnsaveonclick(varname) {
    var lbl = document.getElementById(_clientlbl_CheckDependency);
    lbl.innerHTML = "";
    var lbl1 = document.getElementById(_clientlbl_UpdateSucess);
    lbl1.innerHTML = "";
    var lbl1 = document.getElementById(_clientlbl_ErrorMessage);
    lbl1.innerHTML = "";
}

function MobileNumberValidation(oSrc, args) {
    var MobileNumber = document.getElementById(oSrc.id)
    var sMobileNumber = document.getElementById(_clienttxtMobile).value
    sMobileNumber = stripLeadingTrailingBlanks(sMobileNumber)
    var sValue = parseInt(sMobileNumber);
    if (sMobileNumber.length < 1 && sMobileNumber.length > 15) {
        if (sMobileNumber != "") {
            document.getElementById(_clientcst_MobileNumber).errorMessage = "Contact number should be of greater than or equal to 1 and less than or equal to 15 digits."
            args.IsValid = false
            return true
        }
        else {
            document.getElementById(_clientcst_MobileNumber).ErrorMessage = ""
            args.IsValid = true
            return false
        }
    }
    else if (sValue == '0') {
        document.getElementById(_clientcst_MobileNumber).errormessage = "Contact number should not be zero.";
        args.IsValid = false;
        return true;
    }
    args.IsValid = true
    return false
}