if ($get(_clienthidIsCc).value == '0') {
    getUserIds()
    GetCount()
}
else {
    getUserIdsCc()
    GetCcCount()
}

var prm = Sys.WebForms.PageRequestManager.getInstance()
prm.add_endRequest(EndReqHandler)


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
function trimAll(sString) {
    while (sString.substring(0, 1) == ' ') {
        sString = sString.substring(1, sString.length)
    }
    while (sString.substring(sString.length - 1, sString.length) == ' ') {
        sString = sString.substring(0, sString.length - 1)
    }
    while (sString.charCodeAt(sString.length - 1) == 10 || sString.charCodeAt(sString.length - 1) == 13) {
        sString = sString.substring(0, sString.length - 1)
    }
    return sString
}
function EndReqHandler(sender, args) {
//    {
//        getUserIds()
//    }
    AutoSearch();
}

function getUserIds() {
    if (document.getElementById(_clientGridId)) {
        if (document.getElementById(_clientSelectedUserId).value == "")
            document.getElementById(_clientSelectedUserId).value = window.opener.GetUserIds(document.getElementById(_clienthidIsIndivisualStudentId).value);
        if (document.getElementById(_clienthidSelectedUserNames).value == "")
            document.getElementById(_clienthidSelectedUserNames).value = window.opener.GetUserNames(document.getElementById(_clienthidIsIndivisualStudentId).value);
        var arrIds = new Array()
        arrIds = document.getElementById(_clientSelectedUserId).value.split(';')
        var i, j
        var iCnt = document.getElementById(_clientGridId).rows.length
        var iSelectedCnt = arrIds.length
        var id = '0'
        var iCheckedCnt = 0
        var sRow, iRowIndex
        var sChkId = '_ChkBoxSelect'
        var bStatus = false;
        var sEleId = ''
        
        var Ids = document.getElementById(_clienthidIds).value.split(',')
        for (i = 1; i < iCnt; i++) {
            bStatus = false;
            id = Ids[i-1];
            for (j = 0; j < iSelectedCnt; j++) {
                if (trimAll(id) == trimAll(arrIds[j])) {
                    iCheckedCnt = iCheckedCnt + 1
                    iRowIndex = parseInt(i) + 1
                    if (iRowIndex < 10) {
                        sRow = '0' + iRowIndex
                    }
                    else {
                        sRow = iRowIndex
                    }
                    sEleId = _clientGridId + '_ctl' + sRow + sChkId
                    if (document.getElementById(sEleId)) {
                        document.getElementById(_clienthidUserIds).value += id + "||"
                        document.getElementById(sEleId).checked = true
                    }
                    break
                }
                else {
                    iCheckedCnt = iCheckedCnt + 1
                    iRowIndex = parseInt(i) + 1
                    if (iRowIndex < 10) {
                        sRow = '0' + iRowIndex
                    }
                    else {
                        sRow = iRowIndex
                    }
                    sEleId = _clientGridId + '_ctl' + sRow + sChkId
                    if (document.getElementById(sEleId).checked == true)
                        bStatus = true;
                }
                if (bStatus == true && document.getElementById(_clientSelectedUserId).value.indexOf(id) == -1) {
                    if (document.getElementById(_clientSelectedUserId).value != "")
                        document.getElementById(_clientSelectedUserId).value += "; " + id;
                    else
                        document.getElementById(_clientSelectedUserId).value += id;
                    var sUserName = document.getElementById(_clientGridId).rows[i].cells[1].innerHTML.trim();
                    if (document.getElementById(_clienthidSelectedUserNames).value != "")
                    //document.getElementById(_clienthidSelectedUserNames).value += ", " + document.getElementById(_clientGridId).rows[i].cells[1].innerHTML.trim().substring(document.getElementById(_clientGridId).rows[i].cells[1].innerHTML.trim().indexOf('Name">') + 6).replace("</span>", "");
                        document.getElementById(_clienthidSelectedUserNames).value += ", " + sUserName.substring(sUserName.indexOf('Name">') + 6).substring(sUserName.indexOf('Name>')).replace("</span>", "").replace("</SPAN>", "");
                    else
                    //document.getElementById(_clienthidSelectedUserNames).value += document.getElementById(_clientGridId).rows[i].cells[1].innerHTML.trim().substring(document.getElementById(_clientGridId).rows[i].cells[1].innerHTML.trim().indexOf('Name">') + 6).replace("</span>", "");
                        document.getElementById(_clienthidSelectedUserNames).value += sUserName.substring(sUserName.indexOf('Name">') + 6).substring(sUserName.indexOf('Name>')).replace("</span>", "").replace("</SPAN>", "");
                }

            }
        }
        if (document.getElementById(_clienthidUserIds).value.length > 2)
            document.getElementById(_clienthidUserIds).value = document.getElementById(_clienthidUserIds).value.substring(0, document.getElementById(_clienthidUserIds).value.length - 2)
    }
}

function getUserIdsCc() {
    if (document.getElementById(_clientGridId)) {
        if (document.getElementById(_clientSelectedUserIdCc).value == "")
            document.getElementById(_clientSelectedUserIdCc).value = window.opener.getUserIdsCc(document.getElementById(_clienthidIsIndivisualStudentId).value);
        if (document.getElementById(_clienthidSelectedUserNamesCc).value == "")
            document.getElementById(_clienthidSelectedUserNamesCc).value = window.opener.GetUserNamesCc(document.getElementById(_clienthidIsIndivisualStudentId).value);
        var arrIds = new Array()
        arrIds = document.getElementById(_clientSelectedUserIdCc).value.split(';')
        var i, j
        var iCnt = document.getElementById(_clientGridId).rows.length
        var iSelectedCnt = arrIds.length
        var id = '0'
        var iCheckedCnt = 0
        var sRow, iRowIndex
        var sChkId = '_ChkBoxSelect'
        var bStatus = false;
        var sEleId = ''
        var Ids = document.getElementById(_clienthidIds).value.split(',')
        for (i = 1; i < iCnt; i++) {
            bStatus = false;
            id = Ids[i-1];
            for (j = 0; j < iSelectedCnt; j++) {
                if (trimAll(id) == trimAll(arrIds[j])) {
                    iCheckedCnt = iCheckedCnt + 1
                    iRowIndex = parseInt(i) + 1
                    if (iRowIndex < 10) {
                        sRow = '0' + iRowIndex
                    }
                    else {
                        sRow = iRowIndex
                    }
                    sEleId = _clientGridId + '_ctl' + sRow + sChkId
                    if (document.getElementById(sEleId)) {
                        document.getElementById(_clienthidUserIdsCc).value += id + "||"
                        document.getElementById(sEleId).checked = true
                    }
                    break
                }
                else {
                    iCheckedCnt = iCheckedCnt + 1
                    iRowIndex = parseInt(i) + 1
                    if (iRowIndex < 10) {
                        sRow = '0' + iRowIndex
                    }
                    else {
                        sRow = iRowIndex
                    }
                    sEleId = _clientGridId + '_ctl' + sRow + sChkId
                    if (document.getElementById(sEleId).checked == true)
                        bStatus = true;
                }
                if (bStatus == true && document.getElementById(_clientSelectedUserIdCc).value.indexOf(id) == -1) {
                    if (document.getElementById(_clientSelectedUserIdCc).value != "")
                        document.getElementById(_clientSelectedUserIdCc).value += "; " + id;
                    else
                        document.getElementById(_clientSelectedUserIdCc).value += id;
                    var sUserName = document.getElementById(_clientGridId).rows[i].cells[1].innerHTML.trim();
                    if (document.getElementById(_clienthidSelectedUserNamesCc).value != "")
                    //document.getElementById(_clienthidSelectedUserNames).value += ", " + document.getElementById(_clientGridId).rows[i].cells[1].innerHTML.trim().substring(document.getElementById(_clientGridId).rows[i].cells[1].innerHTML.trim().indexOf('Name">') + 6).replace("</span>", "");
                        document.getElementById(_clienthidSelectedUserNamesCc).value += ", " + sUserName.substring(sUserName.indexOf('Name">') + 6).substring(sUserName.indexOf('Name>')).replace("</span>", "").replace("</SPAN>", "");
                    else
                    //document.getElementById(_clienthidSelectedUserNames).value += document.getElementById(_clientGridId).rows[i].cells[1].innerHTML.trim().substring(document.getElementById(_clientGridId).rows[i].cells[1].innerHTML.trim().indexOf('Name">') + 6).replace("</span>", "");
                        document.getElementById(_clienthidSelectedUserNamesCc).value += sUserName.substring(sUserName.indexOf('Name">') + 6).substring(sUserName.indexOf('Name>')).replace("</span>", "").replace("</SPAN>", "");
                }

            }
        }
        if (document.getElementById(_clienthidUserIdsCc).value.length > 2)
            document.getElementById(_clienthidUserIdsCc).value = document.getElementById(_clienthidUserIdsCc).value.substring(0, document.getElementById(_clienthidUserIdsCc).value.length - 2)
    }
}

function RemoveUser(UserName, UserId, iRowIndex) {
    var sRow;
    iRowIndex = parseInt(iRowIndex) + 2
    if (iRowIndex < 10) {
        sRow = '0' + iRowIndex
    }
    else {
        sRow = iRowIndex
    }
    var sEleId = _clientGridId + '_ctl' + sRow + '_ChkBoxSelect'
    if ($get(_clienthidIsCc).value == '0') {
        if (document.getElementById(sEleId).checked == false) {
            document.getElementById(_clientSelectedUserId).value = document.getElementById(_clientSelectedUserId).value.replace('; ' + UserId, '').replace(UserId + ', ', '');
            document.getElementById(_clienthidSelectedUserNames).value = document.getElementById(_clienthidSelectedUserNames).value.replace('; ' + UserName, '').replace(UserName + ', ', '');

        }
    }
    else {
        if (document.getElementById(sEleId).checked == false) {
            document.getElementById(_clientSelectedUserIdCc).value = document.getElementById(_clientSelectedUserIdCc).value.replace('; ' + UserId, '').replace(UserId + ', ', '');
            document.getElementById(_clienthidSelectedUserNamesCc).value = document.getElementById(_clienthidSelectedUserNamesCc).value.replace('; ' + UserName, '').replace(UserName + ', ', '');

        }
    }
}
function TABLE1_onclick() { }
function closewindow() {

    if (document.getElementById(_clientimgBtnOk) != null)
        document.getElementById(_clientimgBtnOk).disabled = true
    document.getElementById(_clientbtnClose).disabled = true
    if (document.getElementById(_clientimgBtnOKUp) != null)
        document.getElementById(_clientimgBtnOKUp).disabled = true
    document.getElementById(_clientbtnCloseUp).disabled = true
    window.close()
}
window.focus()
function ConfirmAction(iPageCount, sActionName) {
    var bResult = true
    if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientGridId, 'ChkBoxSelect', sActionName, 'false', iPageCount, 'false')) {
        document.getElementById(_clientimgBtnOk).disabled = true
        document.getElementById(_clientbtnClose).disabled = true
        document.getElementById(_clientimgBtnOKUp).disabled = true
        document.getElementById(_clientbtnCloseUp).disabled = true
        bResult = true
    }
    else {
        bResult = window.confirm(sActionName)
    }
    return bResult
}
function GetCount() {
    document.getElementById(_clientSelectedUserId).value = window.opener.GetUserIds(document.getElementById(_clienthidIsIndivisualStudentId).value)
    document.getElementById(_clienthidSelectedUserNames).value = window.opener.GetUserNames(document.getElementById(_clienthidIsIndivisualStudentId).value)
    var arrIds = new Array()
    var iSelectedCnt = 0
    if (document.getElementById(_clientSelectedUserId).value.length != 0) {
        arrIds = document.getElementById(_clientSelectedUserId).value.split(';')
        iSelectedCnt = arrIds.length
    }
    var iCnt = document.getElementById(_clientGridId).rows.length
    if (iCnt == (iSelectedCnt + 1))
        $get(_clientGridId + "_ctl01_ChkAllDel").checked = true
}

function GetCcCount() {
    document.getElementById(_clientSelectedUserIdCc).value = window.opener.getUserIdsCc(document.getElementById(_clienthidIsIndivisualStudentId).value)
    document.getElementById(_clienthidSelectedUserNamesCc).value = window.opener.GetUserNamesCc(document.getElementById(_clienthidIsIndivisualStudentId).value)
    var arrIds = new Array()
    var iSelectedCnt = 0
    if (document.getElementById(_clientSelectedUserIdCc).value.length != 0) {
        arrIds = document.getElementById(_clientSelectedUserIdCc).value.split(';')
        iSelectedCnt = arrIds.length
    }
    var iCnt = document.getElementById(_clientGridId).rows.length
    if (iCnt == (iSelectedCnt + 1))
        $get(_clientGridId + "_ctl01_ChkAllDel").checked = true
}

$(document).ready(function () {
    AutoSearch();
    EnableRadioButtons();
});

/*This code is used to enabled parent screens diabled radio buttons after load popup*/
function EnableRadioButtons() {
    window.opener.$("input[type=radio][name$=UserType][value!='optEntireSchool']").attr('disabled', false);
}

function AutoSearch() {
    _clienttxtRegNumber = _clientxtName;
    var SchoolId = _clienmiSchoolId;
    var AcademicYearId = _clienmiAcademicYearId
    BindAutoCompleteEvent(SchoolId, AcademicYearId, _clienttxtRegNumber, null, null, null, 0);
}

function SearchSelectedValue(val) {
    txt = document.getElementById(_clientxtName);
    bt = document.getElementById(_clienbtnSearch);
    SearchResult(txt, val, bt);
}