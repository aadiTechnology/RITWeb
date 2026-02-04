function ResetErrLabel() {

    var isPageValid = true
    if (document.getElementById(_clientLabelId) != null)
        document.getElementById(_clientLabelId).style.display = "none"
    if (document.getElementById(_clientlblUpdateSucess) != null)
        document.getElementById(_clientlblUpdateSucess).style.display = "none"
    if (typeof (Page_ClientValidate) == 'function') {

        isPageValid = Page_ClientValidate()
    }
}

function CheckFileTypeForAadharNumber(sFileName) {
    var bIsValid;
    if (sFileName != "") {

        if (sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".JPG" || sFileName.substr(sFileName.lastIndexOf('.'), 5).toUpperCase() == ".JPEG" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".PNG" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".BMP" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".PDF") {

            bIsValid = true;
        }
        else {
            bIsValid = false;
        }
    }
    else {
        bIsValid = false;
    }
    return bIsValid;
}

function ValidateAadharScanCopy(aSrc, args) {
    var myImage = new Image();
    myImage.src = document.getElementById(_clientfuAadharNumber).value;

    var iWidth = myImage.width
    var iHeight = myImage.height

    if (CheckFileTypeForAadharNumber(myImage.src))//if file type is valid
    {
        if (document.getElementById(_clientfuAadharNumber).files[0].size <= 3145728) {
        }
        else {
            document.getElementById(_cstValidateAadharScanCopy).errormessage = "File size should not be greater than 3 MB.";
            args.IsValid = false;
            return true;
        }
    }
    else//if file type is not valid
    {
        document.getElementById(_cstValidateAadharScanCopy).errormessage = "File type should be between .pdf, .jpg, .jpeg, .png and .bmp.";
        args.IsValid = false;
        return true;
    }

    args.IsValid = true;
    return false;
}


function CheckFileTypeForBirthCertificate(sFileName) {
    var bIsValid;
    if (sFileName != "") {

        if (sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".JPG" || sFileName.substr(sFileName.lastIndexOf('.'), 5).toUpperCase() == ".JPEG" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".PNG" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".BMP" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".PDF") {

            bIsValid = true;
        }
        else {
            bIsValid = false;
        }
    }
    else {
        bIsValid = false;
    }
    return bIsValid;
}

function ValidateBirthCertificate(aSrc, args) {
    var myImage = new Image();
    myImage.src = document.getElementById(_clientfuBirthCertificate).value;

    var iWidth = myImage.width
    var iHeight = myImage.height

    if (CheckFileTypeForBirthCertificate(myImage.src))//if file type is valid
    {
        if (document.getElementById(_clientfuBirthCertificate).files[0].size <= 3145728) {
        }
        else {
            document.getElementById(_cstValidateBirthCertificate).errormessage = "File size should not be greater than 3 MB.";
            args.IsValid = false;
            return true;
        }
    }
    else//if file type is not valid
    {
        document.getElementById(_cstValidateBirthCertificate).errormessage = "File type should be between .pdf, .jpg, .jpeg, .png and .bmp.";
        args.IsValid = false;
        return true;
    }

    args.IsValid = true;
    return false;
}