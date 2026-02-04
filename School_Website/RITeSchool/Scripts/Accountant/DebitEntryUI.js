var prm = Sys.WebForms.PageRequestManager.getInstance();
prm.add_beginRequest(BeginReqHandler);
prm.add_endRequest(EndReqHandler);

var isPageValid = true;

function BeginReqHandler(sender, args) {
    var postBackElement = sender._postBackSettings.sourceElement;
    if (postBackElement.id == _clientbtnSave)
        DisableButtons(true);
}

function EndReqHandler(sender, args) {
    var postBackElement = sender._postBackSettings.sourceElement;
    if (postBackElement.id == _clientbtnSave)
        DisableButtons(false);

    AutoSearch();
}

function DisableButtons(action) {
    if (isPageValid) {
        if ($get(_clientbtnSave) != null)
            $get(_clientbtnSave).disabled = action;
        if ($get(_clientbtnCancel) != null)
            $get(_clientbtnCancel).disabled = action;
        if ($get(_sClienbtnShow) != null)
            $get(_sClienbtnShow).disabled = action;
        if ($get(_sClienbtnBackUp) != null)
            $get(_sClienbtnBackUp).disabled = action;
    }
}

function ValidateChequeNo(aSrc, args) {
    var rdoCheque = $get(_sClientrdoFeeType + '_2');
    if (rdoCheque && rdoCheque.checked) {
        var ddlChequeno = $get(_clientddlChequeNo).value;
        if (ddlChequeno == "0") {
            isPageValid = false;
            args.IsValid = false;
            if ($get(_clientlblError) != null)
                $get(_clientlblError).innerHTML = "";
            $get(_clientcstChequeNumber).errormessage = document.getElementById(_clienthidChequeNumberShouldBeSelected).value;
        }
    }
}

function ValidateFeeType(aSrc, args) {
    if ($get(_clienttxtFeeType) != null) {
        var sFeetype = $get(_clienttxtFeeType).value;
        if ($get(_clientddlOtherFeeTypes) != null) {

            if (sFeetype.trim() == "" && $get(_clientddlOtherFeeTypes).selectedIndex == 0) {

                if ($get(_clientlblError) != null)
                    $get(_clientlblError).innerHTML = "";

                $get(_clientcstFeeType).errormessage = document.getElementById(_clienthidFeeTypeShouldNotBeBlank).value;
                isPageValid = false;
                args.IsValid = false;
            }
        }
        else {
            if (sFeetype.trim() == "") {

                if ($get(_clientlblError) != null)
                    $get(_clientlblError).innerHTML = "";

                $get(_clientcstFeeType).errormessage = document.getElementById(_clienthidFeeTypeShouldNotBeBlank).value;
                isPageValid = false;
                args.IsValid = false;
            }
        }
    }
    if ($get(_clientddlFeeType) != null) {
        if ($get(_clientddlFeeType).value == 0) {
            if ($get(_clientlblError) != null) {
                $get(_clientlblError).innerHTML = "";
            }
            $get(_clientcstFeeType).errormessage = document.getElementById(_clienthidFeeTypeShouldBeSelected).value;
            isPageValid = false;
            args.IsValid = false;
        }
    }
}

function ValidatePayableFor(aSrc, args) {
    if ($get(_clienttxtPayableFor) != null) {
        if (($get(_clienttxtPayableFor).value).trim() == "") {
            if ($get(_clientlblError) != null)
                $get(_clientlblError).innerHTML = "";
            $get(_clientcstPayableFor).errormessage = document.getElementById(_clienthidPayableForShouldNotBeBlank).value;
            isPageValid = false;
            args.IsValid = false;
        }
    }
    if ($get(_clientddlPayableFor) != null) {
        if ($get(_clientddlPayableFor).value == '--Select--' || $get(_clientddlPayableFor).value == 0) {
            if ($get(_clientlblError) != null)
                $get(_clientlblError).innerHTML = "";
            $get(_clientcstPayableFor).errormessage = document.getElementById(_clienthidPayableForShouldBeSelected).value;
            isPageValid = false;
            args.IsValid = false;
        }
    }
}

function ValidateDueDate(aSrc, args) {
    if ($get(_clientchkNotApplicable).checked == false) {
        if ($get(_clienttxtDueDate).value == "") {
            if ($get(_clientlblError) != null)
                $get(_clientlblError).innerHTML = "";

            $get(_clientcstDueDate).errormessage = document.getElementById(_clienthidDueDateShouldNotBeBlank).value;
            isPageValid = false;
            args.IsValid = false;
            return true
        }
    }
    isPageValid = false;
    args.IsValid = true;

    return false;
}

function ValidateAmount(aSrc, args) {
    debugger;
    var txtAmt = $get(_clienttxtAmt).value;
    var rdoCheque = $get(_sClientrdoFeeType + '_2');
    var txtRegNumber = $get(_clienttxtRegNo).value;

    if (txtAmt == "") {
        if ($get(_clientlblError) != null)
            $get(_clientlblError).innerHTML = "";
        $get(_clientcstAmount).errormessage = document.getElementById(_clienthidAmountShouldNotBeBlank).value;
        isPageValid = false;
        args.IsValid = false;
    }
    else if (txtAmt != "" && txtAmt <= 0 && rdoCheque != null && rdoCheque.checked == false && txtRegNumber != "") {
        if ($get(_clientlblError) != null)
            $get(_clientlblError).innerHTML = "";
        if (!window.confirm("Are you sure you want to save this record with zero amount?")) {
            $get(_clientcstAmount).errormessage = "";
            isPageValid = true;
            args.IsValid = true;
        }
        else {
            isPageValid = true;
            args.IsValid = true;
        }
    }
    else if (txtAmt != "" && txtAmt <= 0 && txtRegNumber == "") {
        if ($get(_clientlblError) != null)
            $get(_clientlblError).innerHTML = "";
        $get(_clientcstAmount).errormessage = document.getElementById(_clienthidAmountShouldNotBeZero).value;
        isPageValid = false;
        args.IsValid = false;
    }
}

function ResetControls() {
    $get(_clientddlDivision).value = "0";
    $get(_clientddlStandard).value = "0";
}

function SendMessage(str) {
    var bResult = true;
    if (isPageValid) {
        var chkSendSMS = $get(_clientchkSendSMS);
        var SendMsg = $get(_clienthidSendSms);
        if (chkSendSMS.checked) {
            if (!window.confirm(document.getElementById(_clienthidDoYouWantToSendFollowingSMSMessage).value + str))
                SendMsg.value = "N";
            else
                SendMsg.value = "Y";
        }
    }
    return bResult;
}

function ConfirmDelete(str, sMessage) {
    var bResult = true;
    var msg = "";
    if (str == "Y")
        msg = document.getElementById(_clienthidAreYouSureYouWantToDeleteThisBounceChequeTransaction).value;
    else
        msg = document.getElementById(_clienthidAreYouSureYouWantToDeleteThisDebitDetails).value;
    if (!window.confirm(msg))
        bResult = false;
    else
        bResult = ConfirmSendMessage(sMessage);

    return bResult;
}

function clickButton(e, buttonid) {
    var evt = e ? e : window.event;
    var bt = $get(buttonid);
    if (bt) {
        if (evt.keyCode == 13) {
            bt.click();
            return false;
        }
    }
}

function NoAction() {
    return false;
}

function blinkIt() {
    if (!document.all)
        return;

    var blinkElements = document.all.tags('blink');
    for (var i = 0; i < blinkElements.length; i++) {
        var blinkElement = blinkElements[i];
        blinkElement.style.visibility = (blinkElement.style.visibility == 'visible') ? 'hidden' : 'visible';
    }
}

function OpenPopup(sQueryString) {
    window.open('CopyFeeConfigurationPopup.aspx?' + sQueryString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=600');
    return false;
}

function SendSms_Message(str) {
    var bResult = true;
    ConfirmSendMessage(str);
    return bResult;
}

function ConfirmSendMessage(str) {
    var bResult = true;
    var chkSendSMS = $get(_clientchkSendSMS);
    var SendSms = $get(_clienthidSendSms);
    var chkSendMsg = $get(_clientchkSendMsg);
    var SendMsg = $get(_clienthidSendMsg);
    if (chkSendSMS.checked && chkSendMsg.checked) {
        if (!window.confirm(document.getElementById(_clienthidDoYouWantToSendSMSTo).value.replace("%studentOfClass%", str) + '.')) {
            SendSms.value = "N";
            SendMsg.value = "N";
        }
        else {
            SendSms.value = "Y";
            SendMsg.value = "Y";
        }
    }
    else if (chkSendSMS.checked) {
        if (!window.confirm(document.getElementById(_clienthidDoYouWantToSendSMSTo).value.replace("%studentOfClass%", str) + '.'))
            SendSms.value = "N";
        else
            SendSms.value = "Y";
    }
    else if (chkSendMsg.checked) {
        if (!window.confirm(document.getElementById(_clienthidDoYouWantToSendMessageTo).value.replace("%studentOfClass%", str) + '.'))
            SendMsg.value = "N";
        else
            SendMsg.value = "Y";
    }
    return bResult;
}

function OtherFeeTypeOnChange(src) {
    var txtFeeType = $get(_clienttxtFeeType);

    if (!txtFeeType)
        return;

    if (src.selectedIndex == 0)
        txtFeeType.disabled = false;
    else {
        txtFeeType.disabled = true;
        txtFeeType.value = "";
    }
}
