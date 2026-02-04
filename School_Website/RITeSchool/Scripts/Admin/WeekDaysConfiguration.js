var Page_IsValid = true;

function ConfirmAction(iPageCount, sActionName) {
    Page_IsValid = true;
    var bResult = true
    if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientGridId, 'ChkAllCheckedWeekDays', sActionName, 'false', iPageCount, 'true')) {
        bResult = true

    }
    else {
        bResult = false
        Page_IsValid = false;
    }
    return bResult
}

function DisableButtons(ObjBtn) {
    document.getElementById(_clientbtnSave).disabled = true
    document.getElementById(_clientbtnCancel).disabled = true
}

function onlyAlphanumeric(e, t) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode > 47 && charCode < 58) || (charCode == 8))
            return true;
        else
            return false;
    }
    catch (err) {
        alert(err.Description);
    }
}