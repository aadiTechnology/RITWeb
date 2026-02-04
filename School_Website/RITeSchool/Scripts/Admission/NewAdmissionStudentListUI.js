

var _totalWinHeight;
var _adjWinHeight;
var _rightFooterPos;
var _bottomFooterPos;

window.onresize = setTotal;
window.onscroll = setTotal;
window.onload = setTotal;

function setTotal() {
    _totalWinHeight = document.body.scrollHeight;
    _adjWinHeight = _totalWinHeight; //-608;

    if (document.getElementById(_cltdivAttendanceAlert) != null) {
        _rightFooterPos = document.body.clientHeight - parseInt(document.getElementById(_cltdivAttendanceAlert).style.height);
        document.getElementById(_cltdivAttendanceAlert).style.top = _rightFooterPos;
    }
    window_onscroll();
}

function window_onscroll() {
    if (document.body.scrollTop <= _adjWinHeight) {
        if (document.getElementById(_cltdivAttendanceAlert) != null) {
            document.getElementById(_cltdivAttendanceAlert).style.top = document.body.scrollTop + _rightFooterPos;
        }
    }

}

function ClearContriols() {
    document.getElementById(_clientddlAdmissionType).value = "0";
    document.getElementById(_clientddlStandard).value = "0";
    document.getElementById(_clienttxtStudentName).value = "";
    $get(_clientcmbStatus).value = "0";
    return false;
}

function Confirmed() {
    if (!window.confirm('Are you sure you want to confirm this student?')) {
        bResult = false;
        return false;
    }
    else
        return true;
}

function VerifyAtleastOneCheckBox() {
    var bFlag = CheckAtleastOneCheckBox(_clientlstvwGroup, 'chkIsConfirm', 2)
    if (bFlag) {

        OpenDisionSelectionPopup();
    }
    else {
        alert("Please fix following error(s): \n\r\n\r" + "At least one student should be selected for confirm.")
        return false
    }
}


function OpenStatusPopup(rowIndex) {
    var queryString = document.getElementById(_clientlstvwGroup + "_ctrl" + rowIndex + "_hidQueryString").value
    window.open('AdmissionStatusPopup.aspx?' + queryString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=850,height=700')

}


function UpdateStaus(studentAdmissionId) {
   
    $get(_clienthidStudentAdmissionId).value = studentAdmissionId;
    __doPostBack(document.getElementById(_clienthidStudentAdmissionId).name, '')
}


function openReport(url) {
    window.open(url, '_blank', 'scrollbars=yes,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=20,left=100,width=800,height=600');
}


//This function is used to open division selection popup.
function OpenDisionSelectionPopup() {
    var x, y, tt_ovr_

    var Text = ddlReport.options[ddlReport.selectedIndex].text;
    standardname.innerHTML = Text;
    var pageWidth = window.screen.width
    var pageHeight = 400
    var left = parseInt((pageWidth / 4.5))
    var top = parseInt((pageHeight / 1.5))
    cssstyle.left = left + "px"
    cssstyle.top = top + "px"
    cssstyle.visibility = "visible"
    cssstyle.display = "block"

    return true;
}

//this function is used hid popup.
function HidePopup() {

    $get(divconfirm).style.visibility = "hidden"
    $get(divconfirm).style.display = "none"
    return false;
}

function ConfirmDelete() {
    var bResult = true
    if (!window.confirm('Are you sure you want to delete this record?')) {
        bResult = false
    }
    return bResult
}
