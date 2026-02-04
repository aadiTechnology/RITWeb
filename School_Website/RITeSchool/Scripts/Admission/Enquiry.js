
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
    if (sMobileNumber != "" && sMobileNumber.length < 10) {
        if (iMobileNumber != 2) {
            //document.getElementById(_clientcst_MobileNumber).ErrorMessage = "Mobile number should be of 10 digits."
            oSrc.errormessage = "Mobile number should be of 10 digits."
            args.IsValid = false
            return true
        }
        else if (iMobileNumber == 2) {
            oSrc.errormessage = "Mother Mobile number should be of 10 digits."
            args.IsValid = false
            return true
        }
        else {
            document.getElementById(_clientcst_MobileNumber2).ErrorMessage = ""
            args.IsValid = true
            return false
        }
    }
    else if (sMobileNumber.substring(0, 1) == '0') {
        if (iMobileNumber == 1)
            document.getElementById(_clientcst_MobileNumber).errormessage = "Mobile number1 should not start with zero.";
        else
            document.getElementById(_clientcst_MobileNumber2).errormessage = "Mobile number2 should not start with zero.";
        args.IsValid = false;
        return true;
    }
    args.IsValid = true
    return false
}

function EmailValidation(oSrc, args) {    
    var sEmail = document.getElementById(_clienttxtEmailId).value
    var schoolId = document.getElementById(_clienthidSchoolId).value
    var sSNSSchoolId = document.getElementById(_clienthidSNSSchoolId).value
    var sSPSSchoolId = document.getElementById(_clienthidSPSSchoolId).value
    var sSVPSchoolId = document.getElementById(_clienthidSVPSchoolId).value    
    var sSVNPSchoolId = document.getElementById(_clienthidSVNPSchoolId).value
    sEmail = stripLeadingTrailingBlanks(sEmail)
    if (schoolId == sSNSSchoolId || schoolId == sSPSSchoolId || schoolId == sSVPSchoolId || schoolId == sSVNPSchoolId) {
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
    var dob

    if (document.all)
        dob = new Date(dtDob.replace('-', ' '))
    else
        dob = new Date(convertdate(dtDob))
    var serverdate = document.getElementById(_clienthidDOB).value
   
   // var today = new Date();

    var todaydate;
    if (document.all)
        todaydate = new Date(serverdate.replace('-', ' '))
    else
        todaydate = new Date(convertdate(serverdate));

    if (dob >= todaydate) {
        //document.getElementById(_clientcstDOB).errormessage = "Date of birth should not be future date."
        oSrc.errormessage = "Date of birth should not be future date."
        args.IsValid = false;
        return true;
    }
}

function ValidateControls() {
    if (typeof (Page_ClientValidate) == "function") {
        validationResult = Page_ClientValidate("")
    }

    if (validationResult == false)
        return false
    return true
}

function ValiadteMotherName(oSrc, args) {
    if ($('#' + _clienthidIsMotherNameMandatory).val() == 1) {
        if ($('#' + _clienttxtMName).val().trim() == "") {
            args.IsValid = false
            return true;
        }
    }

    args.IsValid = true
    return false
}