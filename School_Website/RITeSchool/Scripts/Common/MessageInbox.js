function ConfirmAction(iPageCount, sActionName) {
    var bResult = true
    if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientIdGrid, 'ChkBoxDelete', sActionName, 'false', iPageCount, 'true')) {
        if (sActionName == 'At least one message should be selected for deletion.') {
            if (!window.confirm("Are you sure you want to delete the selected message(s)?"))
            { bResult = false }
        }
    }
    else
    { bResult = false; }
    return bResult
}

function ConfirmUnread(iPageCount, sActionName) {
    var bResult = true
    if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientIdGrid, 'ChkBoxDelete', sActionName, 'false', iPageCount, 'true')) {
        bResult = true;
    }
    else
    { bResult = false; }
    return bResult
}

function ConfirmRead(iPageCount, sActionName) {
    var bResult = false
    if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientIdGrid, 'ChkBoxDelete', sActionName, 'false', iPageCount, 'true')) {
        bResult = true;
    }
    return bResult
}

function ConfirmTotalDelete() {
    if ($('[id$=ChkBoxDelete]:checked').length == 0) {
        alert('At least one message should be selected to delete from everyone.');
        return false;
    }
    else {
        return confirm('This action will permanently delete selected message(s) from Sent message list of current user as well as from inbox of all related recipients. Do you want to continue?');
    }
}

function ConfirmDeArchive(iPageCount, sActionName, IsArchive) {
    var bResult = true
    if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientIdGrid, 'ChkBoxDelete', sActionName, 'false', iPageCount, 'true')) {
        if (sActionName == 'At least one message should be selected for trash.') {
            if (!window.confirm("Are you sure you want to trash the selected message(s)?"))
            { bResult = false }
        }
        else {
            if (!window.confirm("Are you sure you want to Un-Delete the selected message(s)?"))
            { bResult = false }
        }
    }
    else
    { bResult = false; }
    return bResult
}

function OpenSettingPopup(obj) {

    _clientdivTemplates = _clientDivSettings
    var x, y, tt_ovr_
    var cssstyle = $get(_clientDivSettings).style
    var width = 750
    var height = 380
    var left = parseInt((screen.width / 2) - (width / 2))
    var top = parseInt((screen.height / 2) - (height / 2))
    cssstyle.left = left + "px"
    cssstyle.top = top + "px"
    cssstyle.visibility = "visible"
    cssstyle.display = "block"
    $get(_clienttxtEmailId).value = $get(_clienthidEmailAddress).value;
    if ($get(_clienthidCanReceiveMail).value == "1")
        $get(_clientchkReceiveMail).checked = true;
    else
        $get(_clientchkReceiveMail).checked = false;
}

function HidePopup() {
    $get(_clientDivSettings).style.visibility = "hidden"
    $get(_clientDivSettings).style.display = "none"
    return false
}

//This function is used to validate Email address.
function EmailValidation() {

    var bIsSelected = true;
    var sEmail = document.getElementById(_clienttxtEmailId).value;
    sEmail = stripLeadingTrailingBlanks(sEmail);

    if (isEmpty(sEmail)) {
        alert(document.getElementById(_clienthidEmailShouldNotBlank).value);
        bIsSelected = false;
    }
    else {
        // If email is not blank then validate for valid email address.
        if (!isEmail(sEmail)) {
            alert(document.getElementById(_clienthidEmailValidation).value);
            bIsSelected = false;
        }
    }

    return bIsSelected;
}

//        function ShowReadReceiptConfirmation(iMessageId, iSendMessageId, PageIndex, iShowMessage) {
//            var query = iMessageId + "," + iSendMessageId + "," + PageIndex
//            if (iShowMessage == 1) {
//                if (window.confirm('The sender of this message has requested  "Read Receipt". Do you want to send it?'))
//                    $get("<%=this.hidIsReadReceiptAccepted.ClientID %>").value = query + ",1";
//                else
//                    $get("<%=this.hidIsReadReceiptAccepted.ClientID %>").value = query + ",0";
//            }
//            else
//                $get("<%=this.hidIsReadReceiptAccepted.ClientID %>").value = query;
//            __doPostBack($get("<%=this.hidIsReadReceiptAccepted.ClientID %>").value.name, '')
//        }

function DisplayReadReceiptDetails(querystring) {
    window.open('ReadReceiptDetailsPopup.aspx?' + querystring, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=650,height=620')
}

function DeleteDraftMessage() {
    return confirm('Are you sure you want to delete this record?');
}
       