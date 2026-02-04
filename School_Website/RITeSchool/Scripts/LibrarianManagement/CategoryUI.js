var prm = Sys.WebForms.PageRequestManager.getInstance()
prm.add_beginRequest(BeginReqHandler)
prm.add_endRequest(EndReqHandler)
function ConfirmDelete() {
    var bResult = true
    if (!window.confirm('Are you sure you want to delete this Categories?')) {
        bResult = false
    }
    return bResult
}
function ClearValSum() {
    if (document.getElementById(_clientValSubCat) != null)
        document.getElementById(_clientValSubCat).style.display = "none"
    if (document.getElementById(_clientValMainCat) != null)
        document.getElementById(_clientValMainCat).style.display = "none"
    return true
}
function ClearText() {
    var lblHeading = document.getElementById(_clientlblHeader)
    if (lblHeading != null)
        lblHeading.style.display = "none"
    var lblError = document.getElementById(_clientlblError)
    if (lblError != null)
        lblError.style.display = "none"
    ClearValSum()
    return true
}
function ResetCategoryName(oDDList) {
    ClearText()
    var lblCategory = document.getElementById(_clientlblCategory)
    if (oDDList.selectedIndex == 0)
        lblCategory.innerHTML = 'Category Name :'
    else
        lblCategory.innerHTML = 'Sub Category Name :'
    document.getElementById(_clientTextboxid).value = ""
    document.getElementById(_clienthidCategoryId).value = ""
    document.getElementById(_clienthidCategoryName).value = ""
    document.getElementById(_clienthidSubCategoryId).value = ""
    document.getElementById(_clienthidIsSubCategory).value = "false"
    return true
}
function IsValidateMainCategory(oSrc, args) {
    if (trimAll(document.getElementById(_clientTextboxid).value) == '') {
        var lblText = document.getElementById(_clientlblCategory).innerHTML
        if (lblText == 'Category Name :') {
            args.IsValid = false
            return true
        }
    }
    args.IsValid = true
    return false
}
function IsValidateCategory(oSrc, args) {
    if (trimAll(document.getElementById(_clientTextboxid).value) == '') {
        var lblText = document.getElementById(_clientlblCategory).innerHTML
        if (lblText == 'Sub Category Name :') {
            args.IsValid = false
            return true
        }
    }
    args.IsValid = true
    return false
}
function BeginReqHandler(sender, args) {
    var postBackElement = sender._postBackSettings.sourceElement
    if (postBackElement.id == _clientbtnSave || postBackElement.id == _clientbtnDelete)
        DisableButtons(true)
}
function EndReqHandler(sender, args) {
    var postBackElement = sender._postBackSettings.sourceElement
    if (postBackElement.id == _clientbtnSave || postBackElement.id == _clientbtnDelete)
        DisableButtons(false)
}
function DisableButtons(action) {
    var isPageValid = true
    if (isPageValid) {
        if (document.getElementById(_clientbtnSave) != null)
            document.getElementById(_clientbtnSave).disabled = action
        if (document.getElementById(_clientbtnNew) != null)
            document.getElementById(_clientbtnNew).disabled = action
        if (document.getElementById(_clientbtnBack) != null)
            document.getElementById(_clientbtnBack).disabled = action
    }
}

function ConfirmDelete() {
    var bResult = true
    if (!window.confirm('Are you sure you want to delete this record?')) {
        bResult = false
    }
    return bResult
}