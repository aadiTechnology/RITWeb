var toolbarDisabledState = "disable"
function toggleFCKeditor(editorInstance) {
    if ((!document.all && editorInstance.EditorDocument.designMode.toLowerCase() != "off") || (document.all && editorInstance.EditorDocument.body.disabled == false)) {
        if (document.all) {
            editorInstance.EditorDocument.body.contentEditable = 'false'
        }
        else {
            editorInstance.EditorDocument.designMode = "off"
        }
        switch (toolbarDisabledState) {
            case "collapse": editorInstance.EditorWindow.parent.FCK.ToolbarSet._ChangeVisibility(true)
            case "disable": editorInstance.EditorWindow.parent.FCK.ToolbarSet.Disable()
                buttonRefreshStateClone = editorInstance.EditorWindow.parent.FCKToolbarButton.prototype.RefreshState
                specialComboRefreshStateClone = editorInstance.EditorWindow.parent.FCKToolbarSpecialCombo.prototype.RefreshState
                editorInstance.EditorWindow.parent.FCKToolbarButton.prototype.RefreshState = function () { return false; }
                editorInstance.EditorWindow.parent.FCKToolbarSpecialCombo.prototype.RefreshState = function () { return false; }
                break
            case "hide": if (editorInstance.EditorWindow.parent.document.getElementById("xExpanded").style.display != "none") {
                    editorInstance.EditorWindow.parent.document.getElementById("xExpanded").isHidden = true
                    editorInstance.EditorWindow.parent.document.getElementById("xExpanded").style.display = "none"
                }
                else {
                    editorInstance.EditorWindow.parent.document.getElementById("xCollapsed").style.display = "none"
                }
                break
        }
    }
    else {
        if (document.all) {
            editorInstance.EditorDocument.body.contentEditable = 'false'
        }
        else {
            editorInstance.EditorDocument.designMode = "on"
        }
        switch (toolbarDisabledState) {
            case "collapse": editorInstance.EditorWindow.parent.FCK.ToolbarSet._ChangeVisibility(false)
            case "disable": editorInstance.EditorWindow.parent.FCK.ToolbarSet.Enable()
                editorInstance.EditorWindow.parent.FCKToolbarButton.prototype.RefreshState = buttonRefreshStateClone
                editorInstance.EditorWindow.parent.FCKToolbarSpecialCombo.prototype.RefreshState = specialComboRefreshStateClone
                break
            case "hide": if (editorInstance.EditorWindow.parent.document.getElementById("xExpanded").isHidden == true) {
                    editorInstance.EditorWindow.parent.document.getElementById("xExpanded").isHidden = false
                    editorInstance.EditorWindow.parent.document.getElementById("xExpanded").style.display = ""
                }
                else {
                    editorInstance.EditorWindow.parent.document.getElementById("xCollapsed").style.display = ""
                }
                break
        }
        editorInstance.EditorWindow.focus()
        editorInstance.EditorWindow.parent.FCK.ToolbarSet.RefreshModeState()
    }
}
function FCKeditor_OnComplete(editorInstance) {
    toggleFCKeditor(editorInstance)
}

ShowReadReceiptConfirmation();
function ShowReadReceiptConfirmation() {
    var iShowMessage = $get(_clienthidShowRequestMessage).value;
    if (iShowMessage == "1") {
        OpenReadReceiptPopup()
        return false;
    }

}

function OpenReadReceiptPopup() {
    _clientdivTemplates = _clientDivSettings
    var x, y, tt_ovr_
    var cssstyle = $get(_clientDivSettings).style
    var cssInboxTable = $get(_clienttblInbox).style
    var pageWidth = window.screen.width
    var pageHeight = 400
    var left = parseInt((pageWidth / 2.7))
    var top = parseInt((pageHeight / 1.2))
    cssstyle.left = left + "px"
    cssstyle.top = top + "px"
    cssstyle.visibility = "visible"
    cssInboxTable.visibility = "hidden"
    cssstyle.display = "block"
}

function HidePopup() {
    $get(_clientDivSettings).style.visibility = "hidden"
    $get(_clientDivSettings).style.display = "none"
    return false;
}