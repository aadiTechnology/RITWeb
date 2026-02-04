
var prm = Sys.WebForms.PageRequestManager.getInstance();
prm.add_endRequest(EndReqHandler);
prm.add_beginRequest(beginRequestHandler)
var editorInstance
function EndReqHandler(sender, args) {
    var postBackElement = sender._postBackSettings.sourceElement;
    //            if (postBackElement.id == _clientTimer || postBackElement.id == _clientbtnDraft)
    //                    ClosePopup();

    if (document.getElementById(_clientHlnkAddBook) != null)
        document.getElementById(_clientHlnkAddBook).disabled = false

    if (postBackElement.id == _clientoptTeachers || postBackElement.id == _clientoptStudents || postBackElement.id == _clientoptSupervisor || postBackElement.id ==_clientoptParentTeacherAssociation)
        ToUserId();

    if (postBackElement.id == _clientoptCCTeachers || postBackElement.id == _clientoptCCStudents || postBackElement.id == _clientoptCCSupervisor || postBackElement.id == _clientoptCCParentTeacherAssociation)
        CCUserId();
}

function beginRequestHandler(sender, args) {
    var postBackElement = sender._postBackSettings.sourceElement;

    //            if (postBackElement.id == _clientTimer || postBackElement.id == _clientbtnDraft)
    //                   OpenWaitingPopup();

    if (document.getElementById(_clientHlnkAddBook) != null)
        document.getElementById(_clientHlnkAddBook).disabled = true
    else
        document.getElementById(_clientHlnkAddBook).disabled = false
}

function Validate_reqToUserId(source, args) {
    var bResult = true;
    var txtUserId = document.getElementById(_clienttxtToUserId);
    if (txtUserId != null) {
        if (txtUserId.value != "") {
            bResult = true
        }
        else {
            bResult = false
            document.getElementById(_clientcstValidate_reqToUserId).errormessage = "At least one message recipient should be selected.";
        }
    }
    args.IsValid = bResult
    return !bResult

}

var sEntireSchool = 'Entire School'
var sTeacherType = 'Teacher'
var sAdminType = 'Admin'
var sPrincipalType = 'Principal'
var sSuperAdminType = 'Software Coordinator'
var sSuperviserType = 'Supervisor'
var sStudent = 'Student'
var sParentTeacherAssociation='ParentTeacherAssociation'////////////
var sAdmin = document.getElementById(_clientHidAdminUserName).value
var sPricipal = document.getElementById(_clientHidPrincipleName).value
var sSuperAdmin = document.getElementById(_clientHidSuperAdminName).value
var sSelectedUsers = ''
var sIsCc = ''
var sIsStudentLevel = ''
function validateGridData(oSrc, args) {

    var grdViewElement = document.getElementById(_ClientGridId)
    if (null == grdViewElement) {
        args.IsValid = false
        return true
    }
    else {
        return false
    }
}
function SetControlsForAdminDetails(obj, UserType) {
    var UsersList = ''
    var SelectedQry = document.getElementById(_clienthidQry).value
    var sUsers = document.getElementById(_clienttxtToUserId).value
    var HasFullAccess = document.getElementById(_clienthidUserHasFullAccess).value
    var reUsers = sUsers.replace(/\s+/g, 'T')
    var reAdmin = sAdmin.replace(/\s+/g, 'T')
    var rePricipal = sPricipal.replace(/\s+/g, 'T')
    var reSuperAdmin = sSuperAdmin.replace(/\s+/g, 'T')
    var iIndex = sUsers.search('(' + sAdminType + ')')
    var ipIndex = sUsers.search('(' + sPrincipalType + ')')
    var isIndex = sUsers.search('(' + sSuperAdminType + ')')
    var sIds = document.getElementById(_clienthidUserId).value
    if (document.getElementById(_clientChkAdmin) != null)
        document.getElementById(_clientChkAdmin).disabled = false;
    if (document.getElementById(_clientchkPrincipal) != null)
        document.getElementById(_clientchkPrincipal).disabled = false;
    if (document.getElementById(_clientchkSuperAdmin) != null)
        document.getElementById(_clientchkSuperAdmin).disabled = false;
    if (document.getElementById(_clientlnkTeacherGroups) != null) 
        document.getElementById(_clientlnkTeacherGroups).disabled = false;
    
    if (sUsers != '') {
        sUsers = sUsers + ', '
    }
    if (sIds != '') {
        sIds = sIds + '; '
    }
    if (UserType == sAdminType) {
        //                if (iIndex == -1) {
        if ((document.getElementById(_clientChkAdmin) != null) && (document.getElementById(_clientChkAdmin).checked)) {
            document.getElementById(_clienttxtToUserId).value = sUsers + sAdmin
        }
        //                }
        else {
            if (!document.getElementById(_clientChkAdmin).checked) {
                document.getElementById(_clienttxtToUserId).value = document.getElementById(_clienttxtToUserId).value.replace(', ' + sAdmin, '')
                document.getElementById(_clienttxtToUserId).value = document.getElementById(_clienttxtToUserId).value.replace(sAdmin + ', ', '')
                document.getElementById(_clienttxtToUserId).value = document.getElementById(_clienttxtToUserId).value.replace(sAdmin, '')
            }

        }
        document.getElementById(_clientHidUserNames).value = document.getElementById(_clienttxtToUserId).value
    }
    if (UserType == sPrincipalType) {
        if (ipIndex == -1) {
            if ((document.getElementById(_clientchkPrincipal) != null) && (document.getElementById(_clientchkPrincipal).checked)) {
                document.getElementById(_clienttxtToUserId).value = sUsers + sPricipal
                if (document.getElementById(_clientHidTeacherId).value != '') {
                    document.getElementById(_clientHidTeacherId).value = document.getElementById(_clientHidTeacherId).value + ";" + document.getElementById(_clientHidPrincipleUserID).value
                    document.getElementById(_clientHidTeacherName).value = document.getElementById(_clientHidTeacherName).value + sPricipal
                    sUsers += document.getElementById(_clientHidTeacherName).value
                }
                else {
                    document.getElementById(_clientHidTeacherId).value = document.getElementById(_clientHidPrincipleUserID).value
                    document.getElementById(_clientHidTeacherName).value = document.getElementById(_clientHidTeacherName).value + sPricipal
                    sUsers += document.getElementById(_clientHidTeacherName).value
                }
            }
        }
        else {
            if (!document.getElementById(_clientchkPrincipal).checked) {
                document.getElementById(_clienttxtToUserId).value = document.getElementById(_clienttxtToUserId).value.replace(', ' + sPricipal, '')
                document.getElementById(_clienttxtToUserId).value = document.getElementById(_clienttxtToUserId).value.replace(sPricipal + ', ', '')
                document.getElementById(_clienttxtToUserId).value = document.getElementById(_clienttxtToUserId).value.replace(sPricipal, '')
                var PrincipleId = document.getElementById(_clientHidPrincipleUserID).value
                document.getElementById(_clientHidTeacherId).value = document.getElementById(_clientHidTeacherId).value.replace(PrincipleId, '')
                document.getElementById(_clientHidTeacherName).value = document.getElementById(_clientHidTeacherName).value.replace(sPricipal, '')


                document.getElementById(_clientHidPTAId).value = document.getElementById(_clientHidPTAId).value.replace(PrincipleId, '')
                document.getElementById(_clientHidTeacherPTAName).value = document.getElementById(_clientHidPTAName).value.replace(sPricipal, '')
            }
        }
        document.getElementById(_clientHidUserNames).value = document.getElementById(_clienttxtToUserId).value
    }
    if (UserType == sSuperAdminType) {
        if (isIndex == -1) {
            if ((document.getElementById(_clientchkSuperAdmin) != null) && (document.getElementById(_clientchkSuperAdmin).checked)) {
                document.getElementById(_clienttxtToUserId).value = sUsers + sSuperAdmin
            }
        }
        else {
            if (!document.getElementById(_clientchkSuperAdmin).checked) {
                document.getElementById(_clienttxtToUserId).value = document.getElementById(_clienttxtToUserId).value.replace(', ' + sSuperAdmin, '')
                document.getElementById(_clienttxtToUserId).value = document.getElementById(_clienttxtToUserId).value.replace(sSuperAdmin + ', ', '')
                document.getElementById(_clienttxtToUserId).value = document.getElementById(_clienttxtToUserId).value.replace(sSuperAdmin, '')



            }
        }
        document.getElementById(_clientHidUserNames).value = document.getElementById(_clienttxtToUserId).value
    }
    else if (UserType == sParentTeacherAssociation) {
        if (isIndex == -1) {
            if ((document.getElementById(_clientchkSuperAdmin) != null) && (document.getElementById(_clientchkSuperAdmin).checked)) {
                document.getElementById(_clienttxtToUserId).value = sUsers + sSuperAdmin
            }
        }
        else {
            if (!document.getElementById(_clientchkSuperAdmin).checked) {
                document.getElementById(_clienttxtToUserId).value = document.getElementById(_clienttxtToUserId).value.replace(', ' + sSuperAdmin, '')
                document.getElementById(_clienttxtToUserId).value = document.getElementById(_clienttxtToUserId).value.replace(sSuperAdmin + ', ', '')
                document.getElementById(_clienttxtToUserId).value = document.getElementById(_clienttxtToUserId).value.replace(sSuperAdmin, '')



            }
        }
        document.getElementById(_clientHidUserNames).value = document.getElementById(_clienttxtToUserId).value
    }
    else if (UserType == 'EntireSchool') {
        document.getElementById(_clienttxtToUserId).value = sEntireSchool
        document.getElementById(_clientHidUserNames).value = sEntireSchool
        ClearAllUsers()
    }
    else {
        if (document.getElementById(_clienttxtToUserId).value == sEntireSchool) {
            document.getElementById(_clienttxtToUserId).value = ""
            document.getElementById(_clientHidUserNames).value = ""
        }
    }
    if (UserType == sTeacherType || UserType == sStudent || UserType == sSuperviserType || UserType == sParentTeacherAssociation || UserType == 'EntireSchool') {
        var bvar = true
        if (UserType == sTeacherType) {
            UsersList = sTeacherType;
        }
        else if (UserType == sStudent) {
            UsersList = 'Student';
            if (document.getElementById(_clientHidUserType).value == sTeacherType && HasFullAccess == 'False')
                sIsStudentLevel = '&IsStudentLevel=Y'
        }
        else if (UserType == sSuperviserType) {
            UsersList = 'Supervisor';
        }

        else if (UserType == sParentTeacherAssociation) {
            UsersList = 'ParentTeacherAssociation';
        }

        else if (UserType == 'EntireSchool') {
            UsersList = 'Entire School';
            bvar = false
            document.getElementById(_clientChkAdmin).disabled = true;
            document.getElementById(_clientChkAdmin).checked = false;
            document.getElementById(_clientchkPrincipal).disabled = true;
            document.getElementById(_clientchkPrincipal).checked = false;
            document.getElementById(_clientchkSuperAdmin).disabled = true;
            document.getElementById(_clientchkSuperAdmin).checked = false;
            document.getElementById(_clientlnkTeacherGroups).disabled = "disabled";
            document.getElementById(_clientHidTeacherName).value = ''
            document.getElementById(_clienthidUserId).value = ''
            document.getElementById(_clientHidSupervisorId).value = ''
            document.getElementById(_clientHidSupervisorName).value = ''
            document.getElementById(_clienthidUserGroupId).value = ''
            document.getElementById(_clienthidUserGroupName).value = ''
        }

        var hidQry = ("Mode=Message&UsersList=" + UsersList + "&sUserId=" + sSelectedUsers + sIsStudentLevel);
        if (bvar)
            window.open("../Common/SelectUserName.aspx?" + hidQry, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=520').focus();
    }
}
function ClearAllUsers() {
    document.getElementById(_clientHidTeacherId).value = ""
    document.getElementById(_clientHidTeacherName).value = ""
    document.getElementById(_clientHidStudentId).value = ""
    document.getElementById(_clientHidStudentName).value = ""
    document.getElementById(_clientHidStdDivId).value = ""
    document.getElementById(_clientHidStdDivName).value = ""
    document.getElementById(_clientHidSupervisorId).value = ""
    document.getElementById(_clientHidSupervisorName).value = ""
    document.getElementById(_clientHidSuperAdminUserId).value = ""
    document.getElementById(_clientHidSuperAdminName).value = ""
    $get(_clienthidUserGroupId).value = "";
    $get(_clienthidUserGroupName).value = "";
}

var sBuyersList = null
var sSuppliersList = null
var sSupplierIdList
var sUserIdList
function SetToUserId(UserName, UserId, isIndivisualUser) {    
    sBuyersList = UserName
    sUserIdList = UserId
    var Principal = "Principal"
    var iUserType = document.getElementById(_clientHidUserType).value
    var HasFullAccess = document.getElementById(_clienthidUserHasFullAccess).value
    if (document.getElementById(_clientoptTeachers).checked == true && isIndivisualUser != "G") {
        document.getElementById(_clientHidTeacherId).value = sUserIdList
        document.getElementById(_clientHidTeacherName).value = sBuyersList
        var TeacherIds = document.getElementById(_clientHidTeacherId).value
        var PrincipleId = document.getElementById(_clientHidPrincipleUserID).value

        if (TeacherIds.match(PrincipleId) != null) {
            var Arr = TeacherIds.split(';');
            var i = 0;
            for (i = 0; i < Arr.length; i++) {
                if (Arr[i].value == PrincipleId && $get(_clientchkPrincipal) != null) {
                    $get(_clientchkPrincipal).checked = true;
                    break;
                }
            }
        }
        else {
            if ($get(_clientchkPrincipal) != null)
                $get(_clientchkPrincipal).checked = false;
        }
    }
    else if (document.getElementById(_clientoptSupervisor).checked == true && isIndivisualUser != "G") {
        document.getElementById(_clientHidSupervisorId).value = sUserIdList
        document.getElementById(_clientHidSupervisorName).value = sBuyersList
    }

    else if (document.getElementById(_clientoptParentTeacherAssociation) && document.getElementById(_clientoptParentTeacherAssociation).checked == true && isIndivisualUser != "G") {      
        document.getElementById(_clientHidPTAId).value = sUserIdList
        document.getElementById(_clientHidPTAName).value = sBuyersList
    }

    else if ((document.getElementById(_clientoptStudents) != null) && (document.getElementById(_clientoptStudents).checked == true) && isIndivisualUser != "G") {
        var isPTA = document.getElementById(_clienthidIsPTAMember).value
        if ((iUserType == sAdminType || iUserType == sSuperviserType || iUserType == sParentTeacherAssociation || HasFullAccess == "True" || isPTA == 'Y') && isIndivisualUser == 'N') {
            document.getElementById(_clientHidStdDivId).value = sUserIdList
            document.getElementById(_clientHidStdDivName).value = sBuyersList
        }
        else {
            document.getElementById(_clientHidStudentId).value = sUserIdList
            document.getElementById(_clientHidStudentName).value = sBuyersList
        }
    }

    //if Groups
    else if (isIndivisualUser == "G") {
        $get(_clienthidUserGroupName).value = UserName;
        $get(_clienthidUserGroupId).value = UserId;
    }

    document.getElementById(_clienttxtToUserId).value = ForatNames();    
}

function SetCcUserId(UserName, UserId, isIndivisualUser) {

    sBuyersList = UserName
    sUserIdList = UserId
    var Principal = "Principal"
    var iUserType = document.getElementById(_clientHidUserType).value
    var HasFullAccess = document.getElementById(_clienthidUserHasFullAccess).value
    if (document.getElementById(_clientoptCCTeachers).checked == true && isIndivisualUser != "G") {
        document.getElementById(_clientHidTeacherIdCC).value = sUserIdList
        document.getElementById(_clientHidTeacherNameCC).value = sBuyersList
        var TeacherIds = document.getElementById(_clientHidTeacherIdCC).value
        var PrincipleId = document.getElementById(_clientHidPrincipleUserIDCC).value

        if (TeacherIds.match(PrincipleId) != null) {
            var Arr = TeacherIds.split(';');
            var i = 0;
            for (i = 0; i < Arr.length; i++) {
                if ((Arr[i].value == PrincipleId || Arr[i] == PrincipleId) && $get(_clientchkPrincipalCC) != null) {
                    $get(_clientchkPrincipalCC).checked = true;
                    break;
                }
            }
        }
        else {
            if ($get(_clientchkPrincipalCC) != null)
                $get(_clientchkPrincipalCC).checked = false;
        }
    }
    else if (document.getElementById(_clientoptCCSupervisor).checked == true && isIndivisualUser != "G") {
        document.getElementById(_clientHidSupervisorIdCC).value = sUserIdList
        document.getElementById(_clientHidSupervisorNameCC).value = sBuyersList
    }

    else if (document.getElementById(_clientoptCCParentTeacherAssociation) && document.getElementById(_clientoptCCParentTeacherAssociation).checked == true && isIndivisualUser != "G") {
        document.getElementById(_clientHidPTAIdCC).value = sUserIdList
        document.getElementById(_clientHidPTANameCC).value = sBuyersList        
    }

    else if ((document.getElementById(_clientoptCCStudents) != null) && (document.getElementById(_clientoptCCStudents).checked == true) && isIndivisualUser != "G") {
        var isPTA = document.getElementById(_clienthidIsPTAMember).value
        if ((iUserType == sAdminType || iUserType == sSuperviserType || iUserType == sParentTeacherAssociation || HasFullAccess == "True" || isPTA == 'Y') && isIndivisualUser == 'N') {
            document.getElementById(_clientHidStdDivIdCC).value = sUserIdList
            document.getElementById(_clientHidStdDivNameCC).value = sBuyersList
        }
        else {
            document.getElementById(_clientHidStudentIdCC).value = sUserIdList
            document.getElementById(_clientHidStudentNameCC).value = sBuyersList
        }
    }

    //if Groups
    else if (isIndivisualUser == "G") {
        $get(_clienthidUserGroupNameCC).value = UserName;
        $get(_clienthidUserGroupIdCC).value = UserId;
    }
    document.getElementById(_clienttxtCCUserId).value = ForatNamesCc();
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
function GetUserIds(isIndivisualUser) {

    var sUserIds = ''
    var iUserType = document.getElementById(_clientHidUserType).value
    if (document.getElementById(_clientoptTeachers).checked == true) {
        sUserIds = document.getElementById(_clientHidTeacherId).value
    }
    else if ((document.getElementById(_clientoptStudents) != null) && (document.getElementById(_clientoptStudents).checked == true)) {
        var isPTA = document.getElementById(_clienthidIsPTAMember).value
        if ((iUserType == sAdminType || iUserType == sSuperviserType || iUserType == sTeacherType || iUserType == sParentTeacherAssociation || isPTA == 'Y') && isIndivisualUser == 'N' && (document.getElementById(_clientHidStdDivId).value != ""))
            sUserIds = document.getElementById(_clientHidStdDivId).value
        else
            sUserIds = document.getElementById(_clientHidStudentId).value
    }
    else if (document.getElementById(_clientoptSupervisor).checked == true) {
        sUserIds = document.getElementById(_clientHidSupervisorId).value
    }

    else if (document.getElementById(_clientoptParentTeacherAssociation) && document.getElementById(_clientoptParentTeacherAssociation).checked == true) {    
    sUserIds = document.getElementById(_clientHidPTAId).value    
    }
    return sUserIds
}

function getUserIdsCc(isIndivisualUser) {
    var sUserIds = ''
    var iUserType = document.getElementById(_clientHidUserType).value
    if (document.getElementById(_clientoptCCTeachers).checked == true) {
        sUserIds = document.getElementById(_clientHidTeacherIdCC).value
    }
    else if ((document.getElementById(_clientoptCCStudents) != null) && (document.getElementById(_clientoptCCStudents).checked == true)) {
        var isPTA = document.getElementById(_clienthidIsPTAMember).value
        if ((iUserType == sAdminType || iUserType == sSuperviserType || iUserType == sTeacherType || iUserType == sParentTeacherAssociation || isPTA == 'Y') && isIndivisualUser == 'N')
            sUserIds = document.getElementById(_clientHidStdDivIdCC).value
        else
            sUserIds = document.getElementById(_clientHidStudentIdCC).value
    }
    else if (document.getElementById(_clientoptCCSupervisor).checked == true) {
        sUserIds = document.getElementById(_clientHidSupervisorIdCC).value
    }

    else if (document.getElementById(_clientoptCCParentTeacherAssociation) && document.getElementById(_clientoptCCParentTeacherAssociation).checked == true) {
        sUserIds = document.getElementById(_clientHidPTAIdCC).value
    }
    return sUserIds
}


function ForatNames() {
    var sUserNameList = ''
    var TeacherIds = document.getElementById(_clientHidTeacherId).value
    var PrincipleId = document.getElementById(_clientHidPrincipleUserID).value
    if (document.getElementById(_clientChkAdmin) != null && document.getElementById(_clientChkAdmin).checked) {
        sUserNameList = sUserNameList + sAdmin
    }
    if ((document.getElementById(_clientchkPrincipal) != null && document.getElementById(_clientchkPrincipal).checked)) {
        if (TeacherIds.match(PrincipleId) != null && TeacherIds.match(PrincipleId).index >= 0) {
            if (sUserNameList == '') {
                sUserNameList = sUserNameList + sPricipal
            } else
                sUserNameList = sUserNameList + ', ' + sPricipal
            if ($get(_clientchkPrincipal) != null)
                $get(_clientchkPrincipal).checked = true
        }
        else {
            if ($get(_clientchkPrincipal) != null)
                $get(_clientchkPrincipal).checked = false
        }
    }
    if (document.getElementById(_clientchkSuperAdmin) != null && document.getElementById(_clientchkSuperAdmin).checked) {

        if (sUserNameList == '')
            sUserNameList = sSuperAdmin
        else
            sUserNameList = sUserNameList + ', ' + sSuperAdmin
    }
    if (document.getElementById(_clientHidAdminReplyName).value != '') {
        if (sUserNameList == '') {
            if (document.getElementById(_clientHidAdminReplyName).value != sAdmin)
                sUserNameList = document.getElementById(_clientHidAdminReplyName).value
        }
        else {
            if (document.getElementById(_clientHidAdminReplyName).value != sAdmin)
                sUserNameList = sUserNameList + ', ' + document.getElementById(_clientHidAdminReplyName).value
        }
    }
    if (document.getElementById(_clientHidTeacherName).value != '') {
        if (sUserNameList == '') {
            sUserNameList = document.getElementById(_clientHidTeacherName).value
        }
        else {
            if ((document.getElementById(_clientchkPrincipal) != null) && document.getElementById(_clientchkPrincipal).checked) {
                document.getElementById(_clientHidTeacherName).value = document.getElementById(_clientHidTeacherName).value.replace(sPricipal + ', ', '')
                document.getElementById(_clientHidTeacherName).value = document.getElementById(_clientHidTeacherName).value.replace(sPricipal, '')
            }
            if ((document.getElementById(_clientChkAdmin) != null) && document.getElementById(_clientChkAdmin).checked) {
                document.getElementById(_clientHidTeacherName).value = document.getElementById(_clientHidTeacherName).value.replace(sAdmin + ', ', '')
                document.getElementById(_clientHidTeacherName).value = document.getElementById(_clientHidTeacherName).value.replace(sAdmin, '')
            }
            if ((document.getElementById(_clientchkSuperAdmin) != null) && document.getElementById(_clientchkSuperAdmin).checked) {
                document.getElementById(_clientHidTeacherName).value = document.getElementById(_clientHidTeacherName).value.replace(sSuperAdmin + ', ', '')
                document.getElementById(_clientHidTeacherName).value = document.getElementById(_clientHidTeacherName).value.replace(sSuperAdmin, '')
            }
            if (document.getElementById(_clientHidTeacherName).value != '') {
                sUserNameList = sUserNameList + ', ' + document.getElementById(_clientHidTeacherName).value
                sUserNameList = sUserNameList.trim();
                if (sUserNameList.endsWith(','))
                    sUserNameList = sUserNameList.substring(0, sUserNameList.length - 1)
            }
        }
    }
    if (document.getElementById(_clientHidStdDivName).value != '') {
        if (sUserNameList == '') {
            sUserNameList = document.getElementById(_clientHidStdDivName).value
        }
        else {
            sUserNameList = sUserNameList + ', ' + document.getElementById(_clientHidStdDivName).value
        }
    }
    if ($get(_clienthidUserGroupName).value != '') {
        if (sUserNameList == '') {
            sUserNameList = $get(_clienthidUserGroupName).value;
        }
        else {
            sUserNameList = sUserNameList + ', ' + $get(_clienthidUserGroupName).value;
        }
    }
    if (document.getElementById(_clientHidStudentName).value != '') {
        if (sUserNameList == '') {
            sUserNameList = document.getElementById(_clientHidStudentName).value
        }
        else {
            sUserNameList = sUserNameList + ', ' + document.getElementById(_clientHidStudentName).value
        }
    }
    if (document.getElementById(_clientHidSupervisorName).value != '') {        
        if (sUserNameList == '') {
            sUserNameList = document.getElementById(_clientHidSupervisorName).value
        }
        else {
            sUserNameList = sUserNameList + ', ' + document.getElementById(_clientHidSupervisorName).value
        }
    }

    if (document.getElementById(_clientHidPTAName).value != '') {
        
        var teacherName = document.getElementById(_clientHidTeacherName).value
        var supName = document.getElementById(_clientHidSupervisorName).value
        var studName = document.getElementById(_clientHidStudentName).value

        var ptaNames = document.getElementById(_clientHidPTAName).value

        var lst = teacherName.split(',')
        for (var k = 0; k < lst.length; k++) {
            ptaNames = ptaNames.replace(lst[k],'')
        }

        lst = supName.split(',')
        for (var k = 0; k < lst.length; k++) {
            ptaNames = ptaNames.replace(lst[k], '')
        }

        lst = studName.split(',')
        for (var k = 0; k < lst.length; k++) {
            ptaNames = ptaNames.replace(lst[k], '')
        }

        if (ptaNames.startsWith(','))
            ptaNames = ptaNames.substring(1)

        ptaNames = ptaNames.trim()

        if (sUserNameList == '') {
            sUserNameList = ptaNames
        }
        else {
            sUserNameList = sUserNameList + ', ' + ptaNames
        }
    }

    return sUserNameList
}

function ForatNamesCc() {
    var sUserNameList = ''
    var TeacherIds = document.getElementById(_clientHidTeacherIdCC).value
    var PrincipleId = document.getElementById(_clientHidPrincipleUserIDCC).value
    if (document.getElementById(_clientChkAdminCC) != null && document.getElementById(_clientChkAdminCC).checked) {
        sUserNameList = sUserNameList + sAdmin
    }
    if ((document.getElementById(_clientchkPrincipalCC) != null && document.getElementById(_clientchkPrincipalCC).checked)) {
        if (TeacherIds.match(PrincipleId) != null && TeacherIds.match(PrincipleId).index >= 0) {
            if (sUserNameList == '') {
                sUserNameList = sUserNameList + sPricipal
            } else
                sUserNameList = sUserNameList + ', ' + sPricipal
            if ($get(_clientchkPrincipalCC) != null)
                $get(_clientchkPrincipalCC).checked = true
        }
        else {
            if ($get(_clientchkPrincipalCC) != null)
                $get(_clientchkPrincipalCC).checked = false
        }
    }
    if (document.getElementById(_clientchkSuperAdminCC) != null && document.getElementById(_clientchkSuperAdminCC).checked) {

        if (sUserNameList == '')
            sUserNameList = sSuperAdmin
        else
            sUserNameList = sUserNameList + ', ' + sSuperAdmin
    }
    if (document.getElementById(_clientHidAdminReplyNameCC).value != '') {
        if (sUserNameList == '') {
            if (document.getElementById(_clientHidAdminReplyNameCC).value != sAdmin)
                sUserNameList = document.getElementById(_clientHidAdminReplyNameCC).value
        }
        else {
            if (document.getElementById(_clientHidAdminReplyNameCC).value != sAdmin)
                sUserNameList = sUserNameList + ', ' + document.getElementById(_clientHidAdminReplyNameCC).value
        }
    }
    if (document.getElementById(_clientHidTeacherNameCC).value != '') {
        if (sUserNameList == '') {
            sUserNameList = document.getElementById(_clientHidTeacherNameCC).value
        }
        else {            
            if ((document.getElementById(_clientchkPrincipalCC) != null) && document.getElementById(_clientchkPrincipalCC).checked) {
                document.getElementById(_clientHidTeacherNameCC).value = document.getElementById(_clientHidTeacherNameCC).value.replace(sPricipal + ', ', '')
                document.getElementById(_clientHidTeacherNameCC).value = document.getElementById(_clientHidTeacherNameCC).value.replace(sPricipal, '')
            }
            if ((document.getElementById(_clientChkAdminCC) != null) && document.getElementById(_clientChkAdminCC).checked) {
                document.getElementById(_clientHidTeacherNameCC).value = document.getElementById(_clientHidTeacherNameCC).value.replace(sAdmin + ', ', '')
                document.getElementById(_clientHidTeacherNameCC).value = document.getElementById(_clientHidTeacherNameCC).value.replace(sAdmin, '')
            }
            if ((document.getElementById(_clientchkSuperAdminCC) != null) && document.getElementById(_clientchkSuperAdminCC).checked) {
                document.getElementById(_clientHidTeacherNameCC).value = document.getElementById(_clientHidTeacherNameCC).value.replace(sSuperAdmin + ', ', '')
                document.getElementById(_clientHidTeacherNameCC).value = document.getElementById(_clientHidTeacherNameCC).value.replace(sSuperAdmin, '')
            }
            if (document.getElementById(_clientHidTeacherNameCC).value != '') {
                sUserNameList = sUserNameList + ', ' + document.getElementById(_clientHidTeacherNameCC).value
                sUserNameList = sUserNameList.trim();
                if(sUserNameList.endsWith(','))
                    sUserNameList = sUserNameList.substring(0, sUserNameList.length - 1)
            }
        }
    }
    if (document.getElementById(_clientHidStdDivNameCC).value != '') {
        if (sUserNameList == '') {
            sUserNameList = document.getElementById(_clientHidStdDivNameCC).value
        }
        else {
            sUserNameList = sUserNameList + ', ' + document.getElementById(_clientHidStdDivNameCC).value
        }
    }
    if ($get(_clienthidUserGroupNameCC).value != '') {
        if (sUserNameList == '') {
            sUserNameList = $get(_clienthidUserGroupNameCC).value;
        }
        else {
            sUserNameList = sUserNameList + ', ' + $get(_clienthidUserGroupNameCC).value;
        }
    }
    if (document.getElementById(_clientHidStudentNameCC).value != '') {
        if (sUserNameList == '') {
            sUserNameList = document.getElementById(_clientHidStudentNameCC).value
        }
        else {
            sUserNameList = sUserNameList + ', ' + document.getElementById(_clientHidStudentNameCC).value
        }
    }
    if (document.getElementById(_clientHidSupervisorNameCC).value != '') {
        if (sUserNameList == '') {
            sUserNameList = document.getElementById(_clientHidSupervisorNameCC).value
        }
        else {
            sUserNameList = sUserNameList + ', ' + document.getElementById(_clientHidSupervisorNameCC).value
        }
    }

    if (document.getElementById(_clientHidPTANameCC).value != '') {
        if (sUserNameList == '') {
            sUserNameList = document.getElementById(_clientHidPTANameCC).value
        }
        else {
            sUserNameList = sUserNameList + ', ' + document.getElementById(_clientHidPTANameCC).value
        }
    }

    return sUserNameList
}
function GetUserNames(isIndivisualUser) {
    var sUserNameList = ''
    var iUserType = document.getElementById(_clientHidUserType).value
    if (document.getElementById(_clientoptTeachers).checked == true) {
        sUserNameList = document.getElementById(_clientHidTeacherName).value
    }


    if (document.getElementById(_clientoptParentTeacherAssociation) && document.getElementById(_clientoptParentTeacherAssociation).checked == true) {
        sUserNameList = document.getElementById(_clientHidPTAName).value
    }


    else if ((document.getElementById(_clientoptStudents) != null) && (document.getElementById(_clientoptStudents).checked == true)) {
        if (document.getElementById(_clientoptStudents).checked == true) {
            if ((iUserType == sAdminType || iUserType == sSuperviserType || iUserType ==sParentTeacherAssociation) && isIndivisualUser == 'N')
                sUserNameList = document.getElementById(_clientHidStdDivName).value
            else
                sUserNameList = document.getElementById(_clientHidStudentName).value
        }
    }

    return sUserNameList
}

function GetUserNamesCc(isIndivisualUser) {
    var sUserNameList = ''
    var iUserType = document.getElementById(_clientHidUserType).value
    if (document.getElementById(_clientoptCCTeachers).checked == true) {
        sUserNameList = document.getElementById(_clientHidTeacherNameCC).value
    }


    if (document.getElementById(_clientoptCCParentTeacherAssociation) && document.getElementById(_clientoptCCParentTeacherAssociation).checked == true) {
        sUserNameList = document.getElementById(_clientHidPTANameCC).value
    }
    else if ((document.getElementById(_clientoptCCStudents) != null) && (document.getElementById(_clientoptCCStudents).checked == true)) {
        if (document.getElementById(_clientoptCCStudents).checked == true) {
            if ((iUserType == sAdminType || iUserType == sSuperviserType || iUserType == sParentTeacherAssociation) && isIndivisualUser == 'N')
                sUserNameList = document.getElementById(_clientHidStdDivNameCC).value
            else
                sUserNameList = document.getElementById(_clientHidStudentNameCC).value
        }
    }

    return sUserNameList
}

function ToUserId() {
    var UserRole
    //    var SelectedQry = document.getElementById(_clienthidQry).value
    var SelectedUserID = document.getElementById(_clienthidUserId).value
    var AlreadySelectedUserId = document.getElementById(_clientHidReplyUserID).value
    var bVal = false
    SelectedUserID = SelectedUserID.replace(AlreadySelectedUserId, '')
    if (document.getElementById(_clientoptTeachers)) {
        if (document.getElementById(_clientoptTeachers).checked == true) {
            UsersList = "Teacher"
            bVal = true
        }
    }
    if (document.getElementById(_clientoptStudents)) {
        if (document.getElementById(_clientoptStudents).checked == true) {
            bVal = true
            UsersList = "Student"
        }
    }
    if (document.getElementById(_clientoptParentTeacherAssociation)) {
        if (document.getElementById(_clientoptParentTeacherAssociation).checked == true) {
            bVal = true
            UsersList = "ParentTeacherAssociation"
        }
    }


    if (document.getElementById(_clientoptSupervisor)) {
        if (document.getElementById(_clientoptSupervisor).checked == true) {
            bVal = true
            UsersList = "Supervisor"
        }
    }
    var bvar = true
    if (document.getElementById(_clientoptAll)) {
        if (document.getElementById(_clientoptAll).checked == true) {
            bVal = true
            alert('All are added in the To-list.')
            UsersList = "EntireSchool"
            bvar = false
        }
    }
    if (!bVal) {
        alert('Select option to whom you want to send mail.')
    } else {
        var hidQry = ("Mode=Message&UsersList=" + UsersList + "&sUserId=" + SelectedUserID + sIsStudentLevel);
        if (bvar)
            window.open("../Common/SelectUserName.aspx?" + hidQry, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=520').focus();
    }
}

function ValidateReceivers(aSrc, args) {
    if (document.getElementById(_clienttxtToUserId).value == '') {
        args.IsValid = false
        return true
    }
    else {
        args.IsValid = true
        return false
    }
}

function FCKeditor_OnComplete(aeditorInstance) {
    editorInstance = aeditorInstance
}

function ValidateContentText(source, args) {    
    var msg = CKEDITOR.instances.edtr1.getData();
    msg = trimAll(msg.replace(/&nbsp;/g, "").replace(/<p>/g, "").replace(/<\/p>/g, ""))
    if (msg == "") {
        args.IsValid = false
        return true
    }
    args.IsValid = true
    return false
}

function validateFile(source, args) {
    var oFileName = document.getElementById(_clientFileUploadClientId).value
    var bIsValid = true
    if (oFileName != "") {
        if (oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".XLS" ||
                oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".DOC" ||
                 oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".JPG"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 5).toUpperCase() == ".JPEG"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".DOCX"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".PDF"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".XLSX"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".PPT"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".PPTX"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".PPS"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".PPSX"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".PNG") {
            var oFileName = document.getElementById(_clientFileUploadClientId).value
            if ((oFileName.indexOf("#", 0) >= 0) || (oFileName.indexOf("%", 0) >= 0) || (oFileName.indexOf("+", 0) >= 0) || (oFileName.indexOf("'", 0) >= 0)) {
                document.getElementById(_clientCustomValId).errormessage = "Filename contains invalid character(s) in first attachment.";
                bIsValid = false;
            }
            else
                bIsValid = true
        }
        else {
            bIsValid = false
            document.getElementById(_clientCustomValId).errormessage = "Invalid file format."
        }
    }
    args.IsValid = bIsValid
    return !bIsValid

}

function validateFile1(source, args) {
    var oFileName = document.getElementById(_clientFileUploadClientId1).value
    var bIsValid = true
    if (oFileName != "") {
        if (oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".XLS" ||
                oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".DOC" ||
                 oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".JPG"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 5).toUpperCase() == ".JPEG"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".DOCX"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".PDF"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".XLSX"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".PPT"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".PPTX"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".PPS"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".PPSX"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".PNG") {
            var oFileName = document.getElementById(_clientFileUploadClientId1).value
            if ((oFileName.indexOf("#", 0) >= 0) || (oFileName.indexOf("%", 0) >= 0) || (oFileName.indexOf("+", 0) >= 0) || (oFileName.indexOf("'", 0) >= 0)) {
                document.getElementById(_clientCustomValId1).errormessage = "Filename contains invalid character(s) in second attachment.";
                bIsValid = false;
            }
            else
                bIsValid = true
        }
        else {
            bIsValid = false
            document.getElementById(_clientCustomValId1).errormessage = "Invalid file format."
        }
    }
    args.IsValid = bIsValid
    return !bIsValid

}

function validateFile2(source, args) {
    var oFileName = document.getElementById(_clientFileUploadClientId2).value
    var bIsValid = true
    if (oFileName != "") {
        if (oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".XLS" ||
                oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".DOC" ||
                 oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".JPG"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 5).toUpperCase() == ".JPEG"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".DOCX"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".PDF"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".XLSX"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".PPT"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".PPTX"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".PPS"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".PPSX"
                 || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".PNG") {
            var oFileName = document.getElementById(_clientFileUploadClientId2).value
            if ((oFileName.indexOf("#", 0) >= 0) || (oFileName.indexOf("%", 0) >= 0) || (oFileName.indexOf("+", 0) >= 0) || (oFileName.indexOf("'", 0) >= 0)) {
                document.getElementById(_clientCustomValId2).errormessage = "Filename contains invalid character(s) in third attachment.";
                bIsValid = false;
            }
            else
                bIsValid = true
        }
        else {
            bIsValid = false
            document.getElementById(_clientCustomValId2).errormessage = "Invalid file format."
        }
    }
    args.IsValid = bIsValid
    return !bIsValid

}

function ResetLabel() {
    if (document.getElementById(_clientlblErr) != null)
        document.getElementById(_clientlblErr).innerHTML = "";
}

function GetRecieptlist() {
    $get(_clientHidReciepents).value = $get(_clienttxtToUserId).value
}

function GetRecieptlist1() {
    $get(_clientHidReciepents1).value = $get(_clienttxtToUserId).value
}

function GetRecieptlist2() {
    $get(_clientHidReciepents2).value = $get(_clienttxtToUserId).value
}

function TeacherGroup() {

    window.open("../Common/MailingGroupPopup.Aspx?" + '', '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=650').focus();
}

function TeacherGroupCc() {
    var sIsCc = "true"
    var SelectedQry = document.getElementById(_clienthidQryCC).value
    window.open("../Common/MailingGroupPopup.Aspx?" + SelectedQry +"&IsCc=" + sIsCc, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=650').focus();
}

function getGroupIds() {
    return $get(_clienthidUserGroupId).value;
}

function getGroupIdsCc() {
    return $get(_clienthidUserGroupIdCC).value;
}

function CheckDuplicateFile(oSrc, args) {
    var otdAttachment = document.getElementById(_tdAttachment);
    var otdAttachment1 = document.getElementById(_tdAttachment1);
    var otdAttachment2 = document.getElementById(_tdAttachment2);
    var oFileName, oFileName1, oFileName2;

    if (otdAttachment == null) {
        oFileName = document.getElementById(_clientFileUploadClientId).value;
        oFileName = oFileName.substr(oFileName.lastIndexOf('\\') + 1);
    }
    else
        oFileName = document.getElementById(_lnkAttachment).innerHTML;

    if (otdAttachment1 == null) {
        oFileName1 = document.getElementById(_clientFileUploadClientId1).value;
        oFileName1 = oFileName1.substr(oFileName1.lastIndexOf('\\') + 1);
    }
    else
        oFileName1 = document.getElementById(_lnkAttachment1).innerHTML;

    if (otdAttachment2 == null) {
        oFileName2 = document.getElementById(_clientFileUploadClientId2).value;
        oFileName2 = oFileName2.substr(oFileName2.lastIndexOf('\\') + 1);
    }
    else
        oFileName2 = document.getElementById(_lnkAttachment2).innerHTML;

    var bIsValid = true;
    if (oFileName != "") {
        if (oFileName != oFileName1 && oFileName != oFileName2) {
            if (oFileName1 != "") {
                if (oFileName1 != oFileName2)
                    bIsValid = true
                else {
                    bIsValid = false;
                    document.getElementById(_clientcvDuplicateFile).errormessage = "File should not be Duplicate."
                }
            }
        }
        else {
            bIsValid = false;
            document.getElementById(_clientcvDuplicateFile).errormessage = "File should not be Duplicate."
        }
    }

    else if (oFileName1 != "") {
        if (oFileName1 != oFileName2) {
            bIsValid = true
        }
        else {
            bIsValid = false;
            document.getElementById(_clientcvDuplicateFile).errormessage = "File should not be Duplicate."

        }
    }

    else if (oFileName2 != "") {
        bIsValid = true;
    }

    args.IsValid = bIsValid
    return !bIsValid
}

function OpenWaitingPopup() {
    var sValue = _clienttblMainBody.value
    $('#divPopup').show(); ContentWindow = $('#divPopup').kendoWindow({ title: "Action - Save", visible: false, modal: true, resizable: false, width: '580px', actions: [] }).data("kendoWindow"); ContentWindow.open(); ContentWindow.center();
}

function ClosePopup() {
    $('#divPopup').data("kendoWindow").close();
}


function SetControlsForAdminCCDetails(obj, UserType) {
    var sUsers = document.getElementById(_clienttxtCCUserId).value
    var HasFullAccess = document.getElementById(_clienthidUserHasFullAccess).value
    var reUsers = sUsers.replace(/\s+/g, 'T')
    var reAdmin = sAdmin.replace(/\s+/g, 'T')
    var rePricipal = sPricipal.replace(/\s+/g, 'T')
    var reSuperAdmin = sSuperAdmin.replace(/\s+/g, 'T')
    var iIndex = sUsers.search('(' + sAdminType + ')')
    var ipIndex = sUsers.search('(' + sPrincipalType + ')')
    var isIndex = sUsers.search('(' + sSuperAdminType + ')')
    var sIds = document.getElementById(_clienthidUserId).value
    if (document.getElementById(_clientChkAdminCC) != null)
        document.getElementById(_clientChkAdminCC).disabled = false;
    if (document.getElementById(_clientchkPrincipalCC) != null)
        document.getElementById(_clientchkPrincipalCC).disabled = false;
    if (document.getElementById(_clientchkSuperAdminCC) != null)
        document.getElementById(_clientchkSuperAdminCC).disabled = false;
    if (document.getElementById(_clientlnkTeacherGroupsCC) != null)
        document.getElementById(_clientlnkTeacherGroupsCC).disabled = false;
    if (sUsers != '') {
        sUsers = sUsers + ', '
    }
    if (sIds != '') {
        sIds = sIds + '; '
    }
    if (UserType == sAdminType) {
        //                if (iIndex == -1) {
        if ((document.getElementById(_clientChkAdminCC) != null) && (document.getElementById(_clientChkAdminCC).checked)) {
            document.getElementById(_clienttxtCCUserId).value = sUsers + sAdmin
        }
        //                }
        else {
            if (!document.getElementById(_clientChkAdminCC).checked) {
                document.getElementById(_clienttxtCCUserId).value = document.getElementById(_clienttxtCCUserId).value.replace(', ' + sAdmin, '')
                document.getElementById(_clienttxtCCUserId).value = document.getElementById(_clienttxtCCUserId).value.replace(sAdmin + ', ', '')
                document.getElementById(_clienttxtCCUserId).value = document.getElementById(_clienttxtCCUserId).value.replace(sAdmin, '')
            }
        }
        document.getElementById(_clientHidUserNames).value = document.getElementById(_clienttxtCCUserId).value
    }
    if (UserType == sPrincipalType) {
    
        if (ipIndex == -1) {
            if ((document.getElementById(_clientchkPrincipalCC) != null) && (document.getElementById(_clientchkPrincipalCC).checked)) {
                document.getElementById(_clienttxtCCUserId).value = sUsers + sPricipal            
                if (document.getElementById(_clientHidTeacherIdCC).value != '') {                
                    document.getElementById(_clientHidTeacherIdCC).value = document.getElementById(_clientHidTeacherIdCC).value + ";" + document.getElementById(_clientHidPrincipleUserIDCC).value
                    document.getElementById(_clientHidTeacherNameCC).value = document.getElementById(_clientHidTeacherNameCC).value + sPricipal
                    sUsers += document.getElementById(_clientHidTeacherNameCC).value
                }
                else {
                    document.getElementById(_clientHidTeacherIdCC).value = document.getElementById(_clientHidPrincipleUserIDCC).value
                    document.getElementById(_clientHidTeacherNameCC).value = document.getElementById(_clientHidTeacherNameCC).value + sPricipal
                    sUsers += document.getElementById(_clientHidTeacherName).value
                }
            }
        }
        else {
            if (!document.getElementById(_clientchkPrincipalCC).checked) {
                document.getElementById(_clienttxtCCUserId).value = document.getElementById(_clienttxtCCUserId).value.replace(', ' + sPricipal, '')
                document.getElementById(_clienttxtCCUserId).value = document.getElementById(_clienttxtCCUserId).value.replace(sPricipal + ', ', '')
                document.getElementById(_clienttxtCCUserId).value = document.getElementById(_clienttxtCCUserId).value.replace(sPricipal, '')
                var PrincipleId = document.getElementById(_clientHidPrincipleUserIDCC).value
                document.getElementById(_clientHidTeacherIdCC).value = document.getElementById(_clientHidTeacherIdCC).value.replace(PrincipleId, '')
                document.getElementById(_clientHidTeacherNameCC).value = document.getElementById(_clientHidTeacherNameCC).value.replace(sPricipal, '')
            }
        }
        document.getElementById(_clientHidUserNamesCC).value = document.getElementById(_clienttxtCCUserId).value
    }
    if (UserType == sSuperAdminType) {
        if (isIndex == -1) {
            if ((document.getElementById(_clientchkSuperAdminCC) != null) && (document.getElementById(_clientchkSuperAdminCC).checked)) {
                document.getElementById(_clienttxtCCUserId).value = sUsers + sSuperAdmin
            }
        }
        else {
            if (!document.getElementById(_clientchkSuperAdminCC).checked) {
                document.getElementById(_clienttxtCCUserId).value = document.getElementById(_clienttxtCCUserId).value.replace(', ' + sSuperAdmin, '')
                document.getElementById(_clienttxtCCUserId).value = document.getElementById(_clienttxtCCUserId).value.replace(sSuperAdmin + ', ', '')
                document.getElementById(_clienttxtCCUserId).value = document.getElementById(_clienttxtCCUserId).value.replace(sSuperAdmin, '')
            }
        }
        document.getElementById(_clientHidUserNames).value = document.getElementById(_clienttxtCCUserId).value
    }
    else if (UserType == 'EntireSchool') {
        document.getElementById(_clienttxtCCUserId).value = sEntireSchool
        document.getElementById(_clientHidUserNamesCC).value = sEntireSchool
        ClearAllUsers()
    }
    else {
        if (document.getElementById(_clienttxtCCUserId).value == sEntireSchool) {
            document.getElementById(_clienttxtCCUserId).value = ""
            document.getElementById(_clientHidUserNamesCC).value = ""
        }
    }

    if (UserType == sTeacherType || UserType == sStudent || UserType == sSuperviserType  || UserType == sParentTeacherAssociation || UserType == 'EntireSchool') {
        var bvar = true
        if (UserType == sTeacherType) {
            UsersList = sTeacherType;
        }
        else if (UserType == sStudent) {
            UsersList = 'Student';
            if (document.getElementById(_clientHidUserType).value == sTeacherType && HasFullAccess == 'False')
                sIsStudentLevel = '&IsStudentLevel=Y'
        }
        else if (UserType == sSuperviserType) {
            UsersList = 'Supervisor';

        }

        else if (UserType == sParentTeacherAssociation) {
            UsersList = 'ParentTeacherAssociation';

        }

        else if (UserType == 'EntireSchool') {
            UsersList = 'Entire School';
            bvar = false;
            document.getElementById(_clientChkAdminCC).disabled = true;
            document.getElementById(_clientChkAdminCC).checked = false;
            document.getElementById(_clientchkPrincipalCC).disabled = true;
            document.getElementById(_clientchkPrincipalCC).checked = false;
            document.getElementById(_clientchkSuperAdminCC).disabled = true;
            document.getElementById(_clientchkSuperAdminCC).checked = false;
            document.getElementById(_clientlnkTeacherGroupsCC).disabled = "disabled";
            document.getElementById(_clientHidTeacherNameCC).value = ''
            document.getElementById(_clienthidUserIdCC).value = ''
            document.getElementById(_clientHidSupervisorIdCC).value = ''
            document.getElementById(_clientHidSupervisorNameCC).value = ''
            document.getElementById(_clienthidUserGroupIdCC).value = ''
            document.getElementById(_clienthidUserGroupNameCC).value = ''
        }

        sIsCc = 'true'
        var hidQryCC = ("Mode=Message&UsersList=" + UsersList + "&sUserId=" + sSelectedUsers + sIsStudentLevel + "&IsCc=" + sIsCc);
        if (bvar)
            window.open("../Common/SelectUserName.aspx?" + hidQryCC, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=520').focus();
    }
}

function CCUserId() {
    var UserRole
    var SelectedQry = document.getElementById(_clienthidQryCC).value
    var SelectedUserID = document.getElementById(_clienthidUserIdCC).value
    var AlreadySelectedUserId = document.getElementById(_clientHidReplyUserIDCC).value
    var bVal = false
    SelectedUserID = SelectedUserID.replace(AlreadySelectedUserId, '')
    if (document.getElementById(_clientoptCCTeachers)) {
        if (document.getElementById(_clientoptCCTeachers).checked == true) {
            UsersList = "Teacher"
            bVal = true
        }
    }
    if (document.getElementById(_clientoptCCStudents)) {
        if (document.getElementById(_clientoptCCStudents).checked == true) {
            bVal = true
            UsersList = "Student"
        }
    }
    if (document.getElementById(_clientoptCCSupervisor)) {
        if (document.getElementById(_clientoptCCSupervisor).checked == true) {
            bVal = true
            UsersList = "Supervisor"
        }
    }

    if (document.getElementById(_clientoptCCParentTeacherAssociation)) {
        if (document.getElementById(_clientoptCCParentTeacherAssociation).checked == true) {
            bVal = true
            UsersList = "ParentTeacherAssociation"
        }
    }

    var bvar = true
    if (document.getElementById(_clientoptCCAll)) {
        if (document.getElementById(_clientoptCCAll).checked == true) {
            bVal = true
            alert('All are added in the To-list.')
            UsersList = "EntireSchool"
            bvar = false
        }
    }
    if (!bVal) {
        alert('Select option to whom you want to send mail.')
    } else {
        var hidQryCC = ("Mode=Message&UsersList=" + UsersList + "&sUserId=" + SelectedUserID + sIsStudentLevel + "&IsCc=" + sIsCc);
        if (bvar)
            window.open("../Common/SelectUserName.aspx?" + hidQryCC, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=520').focus();
    }
}