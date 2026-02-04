function EnableParentStaffSearch() {
    var chkParentStaff = document.getElementById(_clientchkForParentStaff)
    if (chkParentStaff.checked) {
        document.getElementById(_clientcmbStandard).value = "0";
        document.getElementById(_clientcmbStandard).disabled = true;
    }
    else
        document.getElementById(_clientcmbStandard).disabled = false;
}

function ConfirmWriteOffBook() {
    var bResult = true
    if (!window.confirm("Are you sure you want to write off this book?")) {
        bResult = false
    }
    return bResult
}

//This function is used to open issue date popup.
function ShowPopup(e, IssueDate) {
    alert("Check");
    var x, y, tt_ovr_
    var cssstyle = $get(_clientupdtpnlPopUp).style
    $get(_clienttxtReturnDate).value = IssueDate
    $get(_clienthidActIssueDate).value = $get(_clienttxtReturnDate).value

    var width = 100
    var height = -120
    var left = parseInt((screen.width / 2) - (width / 2))
    var top = parseInt((screen.height / 2) - (height / 2))
    cssstyle.left = left + "px"
    cssstyle.top = top + "px"
    // Override the z-index of the topmost wz_dragdrop.js D&D item
    cssstyle.zIndex = Math.max((window.dd && dd.z) ? (dd.z + 2) : 0, 1010);
    cssstyle.visibility = "visible";
    cssstyle.display = "block";

}

//This function is used to get confirmation from user.
function ConfirmReturn() {
    alert("Check");
    var bResult = true
    var validationResult = true
    var iLateFee = 0
    if (typeof (Page_ClientValidate) == 'function') {
        validationResult = Page_ClientValidate("")
    }
    if (validationResult == false) {
        return false
    }

    var sMsg = "Are you sure you want to issue this book?"
    if (!window.confirm(sMsg)) {
        bResult = false
    }
    else {
        HidePopup()
    }

    return bResult
}


//This function is used to close popup.
function HidePopup() {
    $get(_clientupdtpnlPopUp).style.visibility = "hidden"
    $get(_clientupdtpnlPopUp).style.display = "none"
    var validationResult = true
    if (typeof (Page_ClientValidate) == 'function') {
        validationResult = Page_ClientValidate("")
    }
    if (validationResult == false) {
        return false
    }
    var dtActIssueDate = document.getElementById(_clienttxtReturnDate).value
    $get(_clienthidActIssueDate).value = dtActIssueDate
    var cssstyleMain = $get(_clientdivMain).style
    cssstyleMain.visibility = "hidden"
    cssstyleMain.display = "none"
    return false
}

function ValidateSearchUserRole(source, args) {
    var iSubjectIndex = document.getElementById(_clientcmbUser).selectedIndex
    var bIsValid = true
    if (iSubjectIndex == 0) {
        document.getElementById(_clientcstUserRole).errormessage = "Please select the User role."
        bIsValid = false
    }
    args.IsValid = bIsValid
    return bIsValid
}

function SelectBook(src) {
    var iSourceTableRowNo = src.id.match(/_ctrl(\d+)_lstvwBookDetails/)[1];
    var tblBookDetails = $get(_clientListId + '_ctrl' + iSourceTableRowNo + '_lstvwBookDetails_tblContacts');
    if (tblBookDetails.rows) {
        for (var i = 1; i < tblBookDetails.rows.length; i++) {
            var optSelectToIssue = $get(_clientListId + '_ctrl' + iSourceTableRowNo + '_lstvwBookDetails_ctrl' + (i - 1) + '_optSelectToIssue');
            optSelectToIssue.checked = false;
        }
    }
    src.checked = true;
}

function SelectUser(obj, iRowIndex) {
    var bResult = true
    var ListRowCnt = document.getElementById(_clienthidUserRowCnt).value
    var SubLstRowCnt = document.getElementById(_clienthidUsersBookDetails).value
    document.getElementById(_clienthidRowIndex).value = '';
    for (var i = 0; i < ListRowCnt; i++) {
        if (document.getElementById(_clientlstvwUsers + "_ctrl" + i + "_optSelectUser") != null) {

            var rid = document.getElementById(_clientlstvwUsers + "_ctrl" + i + "_optSelectUser")
            rid.checked = false

            var chkParent = document.getElementById(_clientlstvwUsers + "_ctrl" + i + "_chkForParent")
            if (chkParent) {
                chkParent.checked = false;
                chkParent.disabled = true;
            }
        }
    }
    obj.checked = true;
    var chkSelectParent = document.getElementById(_clientlstvwUsers + "_ctrl" + iRowIndex + "_chkForParent")
    if (chkSelectParent)
        document.getElementById(_clientlstvwUsers + "_ctrl" + iRowIndex + "_chkForParent").disabled = false
    document.getElementById(_clienthidRowIndex).value = iRowIndex;
    return bResult
}
// This is the event handler for Page DropDownList
// It will warn the user that on page change, he will lose any selections he has made
// (This only happens when he has actually made a selection)
// @param oSrc: The DropDownList element
function WarnOnPageChange(oSrc) {
    var BookList = document.getElementById(_clientListId + '_tblShiftInfo');
    var BookSelected = false;
    for (var i = 1, len = BookList.rows.length; i < len; i++) {
        if (BookList.rows[i].id.indexOf('trBookDetails') > -1) {
            BookSelected = true;
            break;
        }
    }
    // Prompt the user if there is atleast one book detail showing
    if (BookSelected && !confirm("If you navigate to another page, you will loose any book selections made on this page.\nClick OK to continue or Cancel to stay on this page.")) {
        // We need to reset the DropDowList selectedIndex since the user wants to stay on the same page.
        var currPageNo = document.getElementById(_clienthidBookPageNo).value;
        if (currPageNo && currPageNo != '')
            oSrc.selectedIndex = currPageNo - 1;
        // The else condition is required since currPageNo will only have a value if the user changes the page atleast once
        // Its value is set in the code-behind in the SelectedIndexChanged event handler
        else
            oSrc.selectedIndex = 0;
        return false;
    }
    return true;
}

function SearchSelectedValue(val) {
    txt = document.getElementById(_clienttxtUserName);
    bt = document.getElementById(_clientbtnUserSearch);
    SearchResult(txt, val, bt);
}