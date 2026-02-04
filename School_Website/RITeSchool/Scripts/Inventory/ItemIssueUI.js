function ValidateIssueItem(othis, rowIndex) {    
    var isValidate = 0;
    var lstvwReqItems = _clientlstvwReqItems;
    var lblItemName = 'lblItemName';
    var lblStockQuantity = 'lblStockQuantity';
    var lblReqQuantity = 'lblReqQuantity';
    var cmbUnits = 'cmbUnits'
    var txtIssueQuantity = 'txtIssueQuantity';
    var txtComment = 'txtComment';
    var btnIssue = 'btnIssue'
    var hidIssueItemId = "hidIssueItemId";

    isValidate = validateIssueQuantity(lstvwReqItems, lblItemName, lblStockQuantity, lblReqQuantity, txtIssueQuantity, cmbUnits, txtComment, btnIssue, othis, rowIndex)

    var StockQuantity = lstvwReqItems + '_ctrl' + rowIndex + '_' + lblStockQuantity;
    var ReqQuantity = lstvwReqItems + '_ctrl' + rowIndex + '_' + lblReqQuantity;
    var IssueQuantity = lstvwReqItems + '_ctrl' + rowIndex + '_' + txtIssueQuantity;
    var ItemUnit = lstvwReqItems + '_ctrl' + rowIndex + '_' + cmbUnits;
    var Comment = lstvwReqItems + '_ctrl' + rowIndex + '_' + txtComment;
    var IssueItemId = lstvwReqItems + '_ctrl' + rowIndex + '_' + hidIssueItemId;

    var sStockQuantity = document.getElementById(StockQuantity);
    var sReqQuantity = document.getElementById(ReqQuantity);
    var sIssueQuantity = document.getElementById(IssueQuantity);
    var sComment = document.getElementById(Comment).value;
    var IssueItemId = document.getElementById(IssueItemId).value;

    var dStockQuantity = parseFloat(sStockQuantity.innerHTML);
    var dReqQuantity = parseFloat(sReqQuantity.innerHTML);
    var dIssueQuantity = parseFloat(sIssueQuantity.value);
    var sItemUnit = document.getElementById(ItemUnit);
    var Comment = document.getElementById(sComment);    

    document.getElementById(_clienthidIssueQuantity).value = dIssueQuantity;
    document.getElementById(_clienthidIssueUnits).value = parseInt(sItemUnit.value);
    document.getElementById(_clienthidStockBalance).value = dStockQuantity;
    document.getElementById(_clienthidComment).value = Comment;
    document.getElementById(_clienthidItemId).value = IssueItemId;

    if (parseInt(isValidate) > 0)
        return true;
    else {
        if (dReqQuantity > dIssueQuantity) {
            OpenConfirmationPopup();
            return true;
        }
        else if (dReqQuantity == dIssueQuantity) {
            return false;
        }
        else {
            return true;
        }
        return false;
    }
}


function validateIssueQuantity(lstvwReqItems, lblItemName, lblStockQuantity, lblReqQuantity, txtIssueQuantity, cmbUnits, txtComment, btnIssue, othis, iIndex) {
    var iCount = 0;

    var lstvwReqItemsIssue = document.getElementById(lstvwReqItems + '_Table1');
    if (lstvwReqItemsIssue != null) {
        var oButton = "" + othis.id;

        var pieceCount = document.getElementById(lstvwReqItems + '_ctrl' + iIndex + '_' + 'hidPieceCount').value;

        var ItemName = lstvwReqItems + '_ctrl' + iIndex + '_' + lblItemName;
        var StockQuantity = lstvwReqItems + '_ctrl' + iIndex + '_' + lblStockQuantity;
        var ReqQuantity = lstvwReqItems + '_ctrl' + iIndex + '_' + lblReqQuantity;
        var IssueQuantity = lstvwReqItems + '_ctrl' + iIndex + '_' + txtIssueQuantity;
        var ItemUnit = lstvwReqItems + '_ctrl' + iIndex + '_' + cmbUnits;
        var Comment = lstvwReqItems + '_ctrl' + iIndex + '_' + txtComment;
        var sItemName = document.getElementById(ItemName).innerHTML;
        var sStockQuantity = document.getElementById(StockQuantity);
        var sReqQuantity = document.getElementById(ReqQuantity);
        var sIssueQuantity = document.getElementById(IssueQuantity);
        var sItemUnit = document.getElementById(ItemUnit);
        var sComment = document.getElementById(Comment).value;
        var sMessageComment = '';
        var sMessage = '';

        if (sIssueQuantity != null && sIssueQuantity.value != '' && trimAll(sIssueQuantity.value) != '.') {
            var dStockQuantity = parseFloat(sStockQuantity.innerHTML);
            var dReqQuantity = parseFloat(sReqQuantity.innerHTML);
            var dIssueQuantity = parseFloat(sIssueQuantity.value);

            if (parseInt(sItemUnit.value) == 0)
                dIssueQuantity = dIssueQuantity * parseInt(pieceCount)

            if (dIssueQuantity == 0) {
                iCount = iCount + 1;
                sMessage = 'Issue quantity should not be zero for item \'' + sItemName + '\'.';
            }
            else if (dStockQuantity < dIssueQuantity) {
                iCount = iCount + 1;
                sMessage = 'Issue quantity should be less than stock quantity and required quantity for item \'' + sItemName + '\'.';
            }
            else if (dReqQuantity < dIssueQuantity) {
                iCount = iCount + 1;
                sMessage = 'Issue quantity should not be greater than required quantity for item \'' + sItemName + '\'.';
            }
        }
        else if (trimAll(sIssueQuantity.value) == '.') {
            iCount = iCount + 1;
            sMessage = 'Please enter valid Issue quantity for item \'' + sItemName + '\'.';
        }
        else {
            iCount = iCount + 1;
            sMessage = 'Issue quantity should not be blank for item \'' + sItemName + '\'.';
        }
        var chk = document.getElementById(_clientchkIsGeneral);
        if (chk != null && chk.checked == true) {
            if (sComment == '') {
                iCount = iCount + 1;
                sMessage = sMessage + '\n' + 'Comment should not be blank for item \'' + sItemName + '\'.';

            }
        }
        if (sMessage != '')
            alert(sMessage);

        document.getElementById(_clientHidItemName).value = sItemName;
        document.getElementById(_clientHidIssueQty).value = dIssueQuantity;
        document.getElementById(_clientHidItemUnit).value = sItemUnit.options[sItemUnit.selectedIndex].text
        
    }
    return iCount;

}


function CheckUncheckAll(obj) {
    var rowIndex = 0
    var spn = document.getElementById(_clientlstvwReqItems + "_ctrl" + rowIndex + "_lblItemCode")
    while (spn != null) {

        var index = 0
        var chk = document.getElementById(_clientlstvwReqItems + "_ctrl" + rowIndex + "_lstItemDetails_ctrl" + index + "_chkItemSelect")
        var index = 0

        while (chk != null) {
            chk.checked = obj.checked
            index++;
            chk = document.getElementById(_clientlstvwReqItems + "_ctrl" + rowIndex + "_lstItemDetails_ctrl" + index + "_chkItemSelect")
        }

        index = 0
        rowIndex++;
        spn = document.getElementById(_clientlstvwReqItems + "_ctrl" + rowIndex + "_lblItemCode")
    }
}

function OpenConfirmationPopup() {
    $('#' + _clientdivConfirmation).fadeIn(700);
    var x, y, tt_ovr_
    var cssstyle = $get(_clientdivConfirmation).style
    var width = 600
    var height = 120
    var left = parseInt((screen.width / 2) - (width / 2.3)) - 100
    var top = parseInt((screen.height / 2) - (height / 2)) - 70
    cssstyle.left = left + "px"
    cssstyle.top = top + "px"
}

function ValidateItem(itemIndex) {
    var requiredQuantity = document.getElementById(_clientlstvwReqItems + "_ctrl" + itemIndex + "_txtIssueQuantity").value
    var issueQuantity = document.getElementById(_clientlstvwReqItems + "_ctrl" + itemIndex + "_lblReqQuantity").innerHTML
    var stockQuantity = document.getElementById(_clientlstvwReqItems + "_ctrl" + itemIndex + "_lblStockQuantity").innerHTML

    var index = 0
    var chk = document.getElementById(_clientlstvwReqItems + "_ctrl" + itemIndex + "_lstItemDetails_ctrl" + index + "_chkItemSelect")

    var checkedCount = 0

    while (chk != null) {
        if (chk.checked)
            checkedCount++;
        index++;
        chk = document.getElementById(_clientlstvwReqItems + "_ctrl" + itemIndex + "_lstItemDetails_ctrl" + index + "_chkItemSelect")
    }

    if (parseFloat(requiredQuantity) > parseFloat(issueQuantity)) { 
        alert("Selected item's Issue Quantity should not be greater than Required Quantity.")
        return false;
    }
    else if (parseFloat(stockQuantity) < checkedCount) {
        alert("Selected item's count should not be greater than Stock Quantity.")
        return false;
    }
    else {
        var uom = document.getElementById(_clientlstvwReqItems + '_ctrl' + itemIndex + '_' + 'cmbUnits').value
        var pieceCount = document.getElementById(_clientlstvwReqItems + '_ctrl' + itemIndex + '_' + 'hidPieceCount').value;

        if (parseInt(uom) == 0)
            requiredQuantity = requiredQuantity * parseInt(pieceCount)

        if (parseFloat(requiredQuantity) != checkedCount) {
            {
                //                        if (confirm("Selected item's count is not same as Issue Quantity. Do you want to continue with selected item's quantity?")) {
                //                            document.getElementById(_clientlstvwReqItems + "_ctrl" + itemIndex + "_txtIssueQuantity").value = checkedCount;
                //                            $('#' + '<%=this.hidSelectedItemQuantity.ClientID %>').val(checkedCount)
                //                            return true;
                //                        }
                //                        else
                //                            return false;
                alert("Selected item's count should be same as Issue Quantity.")
                return false;
            }
        }
        else
            return true;
    }
}