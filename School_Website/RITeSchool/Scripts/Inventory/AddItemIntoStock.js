function ValidateReturnItem(othis, rowIndex) {
    var isValidate = 0;
    var lstvwIssuedReqItems = _clientlstvwIssuedReqItems;
    var lblItemName = 'lblItemName';
    var lblIssuedQuantity = 'lblIssuedQuantity';
    var lblReturnQty = 'lblReturnQty';
    var cmbUnits = 'cmbUnits'
    var txtReturnQuantity = 'txtReturnQuantity';
    var txtComment = 'txtComment';
    var btnIssue = 'btnIssue'

    isValidate = validateReturnQuantity(lstvwIssuedReqItems, lblItemName, lblIssuedQuantity, lblReturnQty, txtReturnQuantity, cmbUnits, txtComment, btnIssue, othis, rowIndex)

    if (parseInt(isValidate) > 0)
        return true;
    else
        return false;
}

function validateReturnQuantity(lstvwIssuedReqItems, lblItemName, lblIssuedQuantity, lblReturnQty, txtReturnQuantity, cmbUnits, txtComment, btnIssue, othis, iIndex) {
    var iCount = 0;

    var lstvwIssuedReqItemsIssue = document.getElementById(lstvwIssuedReqItems + '_Table1');
    if (lstvwIssuedReqItemsIssue != null) {
        var oButton = "" + othis.id;

        var pieceCount = document.getElementById(lstvwIssuedReqItems + '_ctrl' + iIndex + '_' + 'hidPieceCount').value;

        var ItemName = lstvwIssuedReqItems + '_ctrl' + iIndex + '_' + lblItemName;
        var IssueQuantity = lstvwIssuedReqItems + '_ctrl' + iIndex + '_' + lblIssuedQuantity;
        var ReturnQuantity = lstvwIssuedReqItems + '_ctrl' + iIndex + '_' + lblReturnQty;
        var QuantityToReturn = lstvwIssuedReqItems + '_ctrl' + iIndex + '_' + txtReturnQuantity;
        var ItemUnit = lstvwIssuedReqItems + '_ctrl' + iIndex + '_' + cmbUnits;
        var Comment = lstvwIssuedReqItems + '_ctrl' + iIndex + '_' + txtComment;
        var sItemName = document.getElementById(ItemName).innerHTML;
        var sIssuedQuantity = document.getElementById(IssueQuantity);
        var sReturnQuantity = document.getElementById(ReturnQuantity);
        var sQuantityToReturn = document.getElementById(QuantityToReturn);
        var sItemUnit = document.getElementById(ItemUnit);
        var sComment = document.getElementById(Comment).value;
        var sMessageComment = '';
        var sMessage = '';

        if (sQuantityToReturn != null && sQuantityToReturn.value != '' && trimAll(sQuantityToReturn.value) != '.') {
            var dIssuedQuantity = parseFloat(sIssuedQuantity.innerHTML);
            var dReturnQuantity = parseFloat(sReturnQuantity.innerHTML);
            var dQuantityToReturn = parseFloat(sQuantityToReturn.value);

            if (parseInt(sItemUnit.value) == 0)
                dQuantityToReturn = dQuantityToReturn * parseInt(pieceCount)

            if (dQuantityToReturn == 0) {
                iCount = iCount + 1;
                sMessage = 'Return quantity should not be zero for item \'' + sItemName + '\'.';
            }
            else if (dIssuedQuantity < dQuantityToReturn) {
                iCount = iCount + 1;
                sMessage = 'Return quantity should be less than issue quantity and consumed quantity for item \'' + sItemName + '\'.';
            }
            else if (dReturnQuantity < dQuantityToReturn) {
                iCount = iCount + 1;
                sMessage = 'Return quantity should not be greater than consumed quantity for item \'' + sItemName + '\'.';
            }
        }
        else if (trimAll(dQuantityToReturn.value) == '.') {
            iCount = iCount + 1;
            sMessage = 'Please enter valid Return quantity for item \'' + sItemName + '\'.';
        }
        else {
            iCount = iCount + 1;
            sMessage = 'Return quantity should not be blank for item \'' + sItemName + '\'.';
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
        document.getElementById(_clientHidReturnQty).value = dQuantityToReturn;
        document.getElementById(_clientHidItemUnit).value = sItemUnit;
    }
    return iCount;

}

function CheckUncheckAll(obj) {
    var rowIndex = 0
    var spn = document.getElementById(_clientlstvwIssuedReqItems + "_ctrl" + rowIndex + "_lblItemCode")

    while (spn != null) {

        var index = 0
        var chk = document.getElementById(_clientlstvwIssuedReqItems + "_ctrl" + rowIndex + "_lstItemDetails_ctrl" + index + "_chkItemSelect")
        var index = 0

        while (chk != null) {
            chk.checked = obj.checked
            index++;
            chk = document.getElementById(_clientlstvwIssuedReqItems + "_ctrl" + rowIndex + "_lstItemDetails_ctrl" + index + "_chkItemSelect")
        }

        index = 0
        rowIndex++;
        spn = document.getElementById(_clientlstvwIssuedReqItems + "_ctrl" + rowIndex + "_lblItemCode")
    }
}

function ValidateItem(itemIndex) {
    var IssuedQuantity = document.getElementById(_clientlstvwIssuedReqItems + "_ctrl" + itemIndex + "_lblIssuedQuantity").innerHTML
    var requiredQuantity = document.getElementById(_clientlstvwIssuedReqItems + "_ctrl" + itemIndex + "_txtReturnQuantity").value

    var index = 0
    var chk = document.getElementById(_clientlstvwIssuedReqItems + "_ctrl" + itemIndex + "_lstItemDetails_ctrl" + index + "_chkItemSelect")

    var checkedCount = 0

    while (chk != null) {
        if (chk.checked)
            checkedCount++;
        index++;
        chk = document.getElementById(_clientlstvwIssuedReqItems + "_ctrl" + itemIndex + "_lstItemDetails_ctrl" + index + "_chkItemSelect")
    }

    if (parseFloat(IssuedQuantity) < checkedCount) {
        alert("Selected item's count should not be greater than Issued Quantity.")
        return false;
    }
    else {
        var uom = document.getElementById(_clientlstvwIssuedReqItems + '_ctrl' + itemIndex + '_' + 'cmbUnits').value
        var pieceCount = document.getElementById(_clientlstvwIssuedReqItems + '_ctrl' + itemIndex + '_' + 'hidPieceCount').value;

        if (parseInt(uom) == 0)
            requiredQuantity = requiredQuantity * parseInt(pieceCount)

        if (parseFloat(requiredQuantity) != checkedCount) { 
            {
                //                        if (confirm("Selected item's count is not same as Issue Quantity. Do you want to continue with selected item's quantity?")) {
                //                            document.getElementById(_clientlstvwIssuedReqItems + "_ctrl" + itemIndex + "_txtReturnQuantity").value = checkedCount;
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