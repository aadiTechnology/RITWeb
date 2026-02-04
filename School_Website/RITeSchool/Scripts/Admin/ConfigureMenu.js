function CheckBoxListRoles(source, args) {
    var j = 0
    var checks = document.forms[0].elements
    var boxLength = checks.length
    for (i = 0; i < boxLength; i++) {
        if ((checks[i].type == 'checkbox' && checks[i].id.match("chkListRoles_") != null) || (checks[i].type == 'checkbox' && checks[i].id.match("chkAddListRoles_") != null)) {
            if (checks[i].checked == true) {
                j++
            }
        }
    }
    if (j > 0) {
        args.IsValid = true
        return false
    }
    else {
        args.IsValid = false
        return true
    }
}

function CheckAllUncheckAlls() {
    var checkAll;
    if (document.getElementById(_clientchkAll) != null)
        checkAll = document.getElementById(_clientchkAll).checked

    var iRowCount = 0
    var chk = document.getElementById(_clientchkListRoles + "_" + iRowCount)
    while (chk != null) {
        chk.checked = checkAll
        iRowCount = iRowCount + 1;
        chk = document.getElementById(_clientchkListRoles + "_" + iRowCount);
    }
}

function CheckAllUncheckAllsForAdd() {
    var checkAll;
    if (document.getElementById(_clientchkAddAll) != null)
        checkAll = document.getElementById(_clientchkAddAll).checked

    var iRowCount = 0
    var chk = document.getElementById(_clientchkAddListRoles + "_" + iRowCount)
    while (chk != null) {
        chk.checked = checkAll
        iRowCount = iRowCount + 1;
        chk = document.getElementById(_clientchkAddListRoles + "_" + iRowCount);
    }
}

function ShowPreview() {
    var MenuId = document.getElementById(_clienthidMenuId).value
    window.open(MenuId, '_new', 'scrollbars=yes,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=20,left=100,width=800,height=950')
}