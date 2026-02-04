

function PinCodeValidation(oSrc, args) {
    var sPIN = document.getElementById(_clienttxtPincode).value
    sPIN = stripLeadingTrailingBlanks(sPIN)
    if (sPIN.length != 6 && sPIN.length != 0) {
        document.getElementById(_clientcst_PIN).ErrorMessage = "Pincode should be of 6 digits."
        args.IsValid = false
        return true
    }
    args.IsValid = true
    return false
}
function MobileNumberValidation(oSrc, args) {    
    var sMobileNumber
    var iMobileNumber
    var MobileNumber1 = document.getElementById(oSrc.id)
    if (MobileNumber1.id == "ctl00_MainBody_cst_MobileNumber") {
        sMobileNumber = document.getElementById(_clienttxtMobile).value
        iMobileNumber = 1;
    }
    else {
        sMobileNumber = document.getElementById(_clienttxtMobile2).value
        iMobileNumber = 2;
    }
    sMobileNumber = stripLeadingTrailingBlanks(sMobileNumber)
    if (sMobileNumber.length < 10) {
        if (sMobileNumber != "" || iMobileNumber != 2) {
            document.getElementById(_clientcst_MobileNumber).ErrorMessage = "Mobile number should be of 10 digits."
            args.IsValid = false
            return true
        } else {
            document.getElementById(_clientcst_MobileNumber2).ErrorMessage = ""
            args.IsValid = true
            return false
        }
    }
    else if (sMobileNumber.substring(0, 1) == '0') {        
        if (iMobileNumber == 1) {
            if ($('#' + _clienthidShowResidentTypeValidation).val() == 'Y')
                document.getElementById(_clientcst_MobileNumber).errormessage = "Father mobile number should not start with zero.";
            else
                document.getElementById(_clientcst_MobileNumber).errormessage = "Mobile number1 should not start with zero.";
        }
        else {
            if ($('#' + _clienthidShowResidentTypeValidation).val() == 'Y')
                document.getElementById(_clientcst_MobileNumber2).errormessage = "Mother mobile number should not start with zero.";
            else
                document.getElementById(_clientcst_MobileNumber2).errormessage = "Mobile number2 should not start with zero.";
        }
        args.IsValid = false;
        return true;
    }
    args.IsValid = true
    return false
}
function EmailValidation(oSrc, args) {
    var sEmail = document.getElementById(_clienttxtEmailId).value
    var sSchoolId = document.getElementById(_clienthidSchoolId).value
    var sOWSSchoolID = document.getElementById(_clienthidOWSSchoolId).value
    sEmail = stripLeadingTrailingBlanks(sEmail)
    if (sSchoolId != sOWSSchoolID) {
        if (isEmpty(sEmail)) {
            document.getElementById(_clientcstValEmailId).errormessage = "Email Address should not be blank."
            args.IsValid = false
            return true
        }
        else {
            if (!isEmail(sEmail)) {
                document.getElementById(_clientcstValEmailId).errormessage = "Email Address should be in valid format(For Example :\" john.smith@yahoo.com \")."
                args.IsValid = false
                return true
            }
        }
    }
    else {
        if (!isEmpty(sEmail)) {
            if (!isEmail(sEmail)) {
                document.getElementById(_clientcstValEmailId).errormessage = "Email Address should be in valid format(For Example :\" john.smith@yahoo.com \")."
                args.IsValid = false
                return true
            }
        }
    }
    args.IsValid = true
    return false
}
function checkDOB(oSrc, args) {    
    var dtDob = document.getElementById(_clienttxtCalDobPopup).value;
    var dtMaxDob = document.getElementById(_clienthidMaxBdate).value;
    var dtMinDob = document.getElementById(_clienthidMinBdate).value;

    var ValidDateFormat = convertvaliddate(dtDob)
    var StudentDOB = new Date(ValidDateFormat)
    // Date format should be mm/dd/yyyy hh:mm:ss
    var EligibleBirthDate = new Date('1/1/2010 00:00:00')
    var dtMinDOB, dtMaxDOB;
    if (dtMinDob != '')
        dtMinDOB = new Date(convertdate(dtMinDob));

    if (dtMaxDob != '')
        dtMaxDOB = new Date(convertdate(dtMaxDob));

    var dob
    if (document.all)
        dob = new Date(dtDob.replace('-', ' '))
    else
        dob = new Date(convertdate(dtDob))
    var serverdate = document.getElementById(_clienthidServerDt).value
    var SserverDt

    if (document.all)
        SserverDt = new Date(serverdate.replace('-', ' '))
    else
        SserverDt = new Date(convertdate(serverdate))

    if (dob > SserverDt) {
        document.getElementById(_clientcstDOB).errormessage = "Date of birth should not be future date."
        args.IsValid = false;
        return true;
    }

    if (dtMinDob != '' && dtMaxDob != '') {
        if (StudentDOB >= dtMinDOB && StudentDOB <= dtMaxDOB) {
            args.IsValid = true;
        }
        else {
            document.getElementById(_clientcstDOB).errormessage = "Date of birth should be between " + dtMinDob + " to " + dtMaxDob + ".";
            args.IsValid = false;
        }
    }
    else if (dtMinDob == '' && dtMaxDob != '' && dtMaxDOB < StudentDOB) {
        document.getElementById(_clientcstDOB).errormessage = "Date of birth should be less than " + dtMaxDob + ".";
        args.IsValid = false;
    }
    else if (dtMinDob != '' && dtMaxDob == '' && dtMinDOB > StudentDOB) {
        document.getElementById(_clientcstDOB).errormessage = "Date of birth should be greater than " + dtMinDob + ".";
        args.IsValid = false;
    }

    return true;
}
function ValidateControls() {
    if (typeof (Page_ClientValidate) == "function") {
        validationResult = Page_ClientValidate("")
    }

    if (validationResult == false)
        return false
    return true
}

function StandardOnChangeHandler(src) {
    if (!src)
        return;

    var stdId = src.value;

    var hidMaxDOB = document.getElementById(_clienthidMaxBdate);
    var hidMinDOB = document.getElementById(_clienthidMinBdate);

    if (minmaxDOBMap[stdId] != undefined)
        hidMinDOB.value = minmaxDOBMap[stdId].min;

    if (minmaxDOBMap[stdId] != undefined)
        hidMaxDOB.value = minmaxDOBMap[stdId].max;
    
    if ($('#' + _clienthidShowValidationForSchool).val() == "Y") {
        var std = src.options[src.selectedIndex].text
        if (parseInt(std) >= 5) {
            $('#' + _clientxtSchoolName).css("backgroundColor", "#ffffa0")
            $('#' + _clienttxtPreviousSchoolAddress).css("backgroundColor", "#ffffa0")
            $('#' + _clienttxtPreviousSchoolUDISENo).css("backgroundColor", "#ffffa0")
            $('#' + _clienttxtLastStd).css("backgroundColor", "#ffffa0")
            $('#' + _clienthidShowLastSchoolValidation).val("1")
        }
        else {
            $('#' + _clientxtSchoolName).css("backgroundColor", "white")
            $('#' + _clienttxtPreviousSchoolAddress).css("backgroundColor", "white")
            $('#' + _clienttxtPreviousSchoolUDISENo).css("backgroundColor", "white")
            $('#' + _clienttxtLastStd).css("backgroundColor", "white")
            $('#' + _clienthidShowLastSchoolValidation).val("0")
        }
    }
}