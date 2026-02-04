
function enabledisablecontrols(btn) { }

function checkIAccept(oSrc, args) {
    args.IsValid = true;

    if (document.getElementById(_clienthidShowParentConsentRestriction) == null || document.getElementById(_clienthidShowParentConsentRestriction).value != "Y")
        args.IsValid = document.getElementById(rdoAccept).checked
    else {
        if (document.getElementById(rdoAccept).checked && document.getElementById(_clientchkParentConsentForm).checked)
            args.IsValid = true;
        else {
            args.IsValid = false;

            if (document.getElementById(rdoAccept).checked)
                oSrc.errormessage = "'Parent Consent Form' should be selected."
            else if (document.getElementById(_clientchkParentConsentForm).checked)
                oSrc.errormessage = "'I accept' should be selected."
            else
                oSrc.errormessage = "'Parent Consent Form' & 'I accept' should be selected."
        }
    }
    return !args.IsValid;
}

function ValidateControls(src) {
    var bIsValid = false;
    if (typeof (Page_ClientValidate) == "function")
        bIsValid = Page_ClientValidate("");

    if (!bIsValid)
        src.style.display = '';

    return bIsValid;
}