// version: beta
// created: 2005-08-30
// updated: 2005-08-31
// mredkj.com
function extractNumber(obj, decimalPlaces, allowNegative) {
    var temp = obj.value;

    // avoid changing things if already formatted correctly
    var reg0Str = '[0-9]*';
    if (decimalPlaces > 0) {
        reg0Str += '\\.?[0-9]{0,' + decimalPlaces + '}';
    } else if (decimalPlaces < 0) {
        reg0Str += '\\.?[0-9]*';
    }
    reg0Str = allowNegative ? '^-?' + reg0Str : '^' + reg0Str;
    reg0Str = reg0Str + '$';
    var reg0 = new RegExp(reg0Str);
    if (reg0.test(temp)) return true;

    // first replace all non numbers
    var reg1Str = '[^0-9' + (decimalPlaces != 0 ? '.' : '') + (allowNegative ? '-' : '') + ']';
    var reg1 = new RegExp(reg1Str, 'g');
    temp = temp.replace(reg1, '');

    if (allowNegative) {
        // replace extra negative
        var hasNegative = temp.length > 0 && temp.charAt(0) == '-';
        var reg2 = /-/g;
        temp = temp.replace(reg2, '');
        if (hasNegative) temp = '-' + temp;
    }

    if (decimalPlaces != 0) {
        var reg3 = /\./g;
        var reg3Array = reg3.exec(temp);
        if (reg3Array != null) {
            // keep only first occurrence of .
            //  and the number of places specified by decimalPlaces or the entire string if decimalPlaces < 0
            var reg3Right = temp.substring(reg3Array.index + reg3Array[0].length);
            reg3Right = reg3Right.replace(reg3, '');
            reg3Right = decimalPlaces > 0 ? reg3Right.substring(0, decimalPlaces) : reg3Right;
            temp = temp.substring(0, reg3Array.index) + '.' + reg3Right;
        }
    }

    obj.value = temp;
}

//JQuery
function CheckOrUncheckHeader(checkBox, columnCheckBoxes, columnCheckedCheckBoxes, headerCheckBox) {
    var chkCount = columnCheckBoxes.length
    if (columnCheckedCheckBoxes.length != chkCount)
        headerCheckBox.removeAttr('checked');
    else if (columnCheckedCheckBoxes.length == chkCount)
        headerCheckBox.attr('checked', checkBox.checked);
}

function blockNonNumbers(obj, e, allowDecimal, allowNegative) {
    var key;
    var isCtrl = false;
    var keychar;
    var reg;

    if (window.event) {
        key = e.keyCode || e.which;
        isCtrl = window.event.ctrlKey
    }
    else if (e.which) {
        key = e.which;
        isCtrl = e.ctrlKey;
    }

    if (isNaN(key)) return true;

    keychar = String.fromCharCode(key);

    // check for backspace or delete, or if Ctrl was pressed
    if (key == 8 || isCtrl) {
        return true;
    }

    reg = /\d/;
    var isFirstN = allowNegative ? keychar == '-' && obj.value.indexOf('-') == -1 : false;
    var isFirstD = allowDecimal ? keychar == '.' && obj.value.indexOf('.') == -1 : false;

    return isFirstN || isFirstD || reg.test(keychar);
}

function formatName(obj) {
    obj.value = obj.value.trim();
    var words = obj.value.split(' ');
    var Text = '';
    var iWordCount;
    var iSpaceCount = 0;
    for (iWordCount = 0; words.length > iWordCount; iWordCount++)
        if (words[iWordCount] != '') {
            Text += ' ' + words[iWordCount].substring(0, 1).toUpperCase() + words[iWordCount].substring(1);
        }
        obj.value = Text.trim();
}

function extractPhNumbers(obj) {
    var temp = obj.value;
    // avoid changing things if already formatted correctly
    var reg0Str = '[0-9,]\s*';
    //reg0Str += '\\?[0-9]\s*';

    var reg0 = new RegExp(reg0Str);
    if (reg0.test(temp)) return true;

    // first replace all non numbers
    var reg1Str = '[^0-9]';
    var reg1 = new RegExp(reg1Str, 'g');
    temp = temp.replace(reg1, '');

    obj.value = temp;
}
function blockNonPhNumbers(obj, e) {
    var key;
    var isCtrl = false;
    var keychar;
    var reg;
    if (window.event) {
        key = e.keyCode;
        isCtrl = window.event.ctrlKey
    }
    else if (e.which) {
        key = e.which;
        isCtrl = e.ctrlKey;
    }

    if (isNaN(key)) return true;

    keychar = String.fromCharCode(key);

    // check for backspace or delete, or if Ctrl was pressed
    if (key == 8 || isCtrl) {
        return true;
    }

    reg = /\d/;
    reg1 = /\s/;
    reg2 = /,/;
    return reg.test(keychar) || reg1.test(keychar) || reg2.test(keychar);
}

function trimLeedingZero(sString) {
    while (sString.substring(0, 1) == '0') {
        sString = sString.substring(1, sString.length);
    }
    return sString;
}


var months = new Array(12);
months[0] = "jan";
months[1] = "feb";
months[2] = "mar";
months[3] = "apr";
months[4] = "may";
months[5] = "jun";
months[6] = "jul";
months[7] = "aug";
months[8] = "sep";
months[9] = "oct";
months[10] = "nov";
months[11] = "dec";

function convertvaliddate(date) {

    var bits;
    if (date.match("-") != null) {
        bits = date.split("-");
    }
    if (bits != undefined) {
        bits[1] = bits[1].toLowerCase();
        var month = 0, day = bits[0], year = bits[2];
        for (var i = 0; i < 12; i++) {
            if (bits[1] == months[i]) {
                month = i + 1;
            }
        }
        var newdate = month + "/" + day + "/" + year;
        return newdate;
    }
    else
        return "";
}

function convertvaliddate2(date) {
    var bits;
    if (date.match("-") != null) {
        bits = date.split("-");
    }
    if (bits != undefined) {
        bits[1] = bits[1].toLowerCase();
        var month = bits[1], day = bits[0], year = bits[2];
        for (var i = 0; i < 12; i++) {
            if (bits[1] == months[i]) {
                month = i + 1;
            }
        }
        if (isNaN(day))
            return "";
        if (isNaN(year))
            return "";
        if (isNaN(month))
            return "";        
        if (month < 0)
            return "";

        var newdate = month + "/" + day + "/" + year;
        return newdate;
    }
    else
        return "";
}


function convertdate(date) {
//    var bits = date.split("-");
//    bits[1] = bits[1].toLowerCase();
//    var month = 0, day = bits[0], year = bits[2];
//    for (var i = 0; i < 12; i++) {
//        if (bits[1] == months[i]) {
//            month = i + 1;
//        }
//    }
//    var newdate = month + "/" + day + "/" + year;
//    return newdate;
	return convertvaliddate2(date);
}

//function CheckSelection(listview, ItemName) {
//    var chk
//    var isSelected = false;
//    var iRowCount = 0;
//    if (iRowCount < 10)
//        chk = document.getElementById(listview + "_ctrl" + iRowCount + ItemName)
//    else
//        chk = document.getElementById(listview + "_ctrl" + iRowCount + ItemName)

//    while (chk != null) {
//        if (chk.checked)
//            isSelected = true;
//        iRowCount = iRowCount + 1;
//        if (iRowCount < 10)
//            chk = document.getElementById(listview + "_ctrl" + iRowCount + ItemName)
//        else
//            chk = document.getElementById(listview + "_ctrl" + iRowCount + ItemName)
//    }
//    return isSelected;
//}

function CheckSelection(listview, ItemName) {
    if (($($(listview) + 'input:checkbox[id*=' + ItemName + ']:checked').length <= 0)) {
        return false;
    }
    else {
        return true;
    }
}


function DuplicateText(document, sListview, sChkSelect, sTextBox) {
    var chk
    var sDuplicate = false;
    var iRowCount = 0;
    chk = document.getElementById(sListview + "_ctrl" + iRowCount + sChkSelect)
    while (chk != null) {
        if (chk.checked == true) {
            var txtsTextBox = document.getElementById(sListview + "_ctrl" + iRowCount + sTextBox)
            var i_RowCount = iRowCount + 1

            var chk_next = document.getElementById(sListview + "_ctrl" + i_RowCount + sChkSelect)
            while (chk_next != null) {

                if (chk_next.checked == true) {
                    txt_next_sTextBox = document.getElementById(sListview + "_ctrl" + i_RowCount + sTextBox)
                    var upper_txtsTextBox = txtsTextBox.value.trim();
                    var upper_txt_next_sTextBox = txt_next_sTextBox.value.trim();
                    if (upper_txtsTextBox.toUpperCase() == upper_txt_next_sTextBox.toUpperCase()) {
                        sDuplicate = true;
                        break;
                    }
                }
                i_RowCount = i_RowCount + 1;

                chk_next = document.getElementById(sListview + "_ctrl" + i_RowCount + sChkSelect)
            }

        }
        iRowCount = iRowCount + 1;
        chk = document.getElementById(sListview + "_ctrl" + iRowCount + sChkSelect)
    }
    if (sDuplicate == true)
        return sDuplicate;
}

function blockNonAlphabates(obj, e) {        
            var key;
            var isCtrl = false;
            var keychar;
            var reg;

            if (window.event) {
                key = e.keyCode;
                isCtrl = window.event.ctrlKey
            }
            else if (e.which) {
                key = e.which;
                isCtrl = e.ctrlKey;
            }

            if (isNaN(key)) return true;

            keychar = String.fromCharCode(key);

            // check for backspace or delete, or if Ctrl was pressed
            if (key == 8 || isCtrl) {
                return true;
            }

            reg = /[0-9a-zA-Z.\- ]/;

            return reg.test(keychar);
        }

        function UpDownKeyPress(obj, e) {
        
            if (e.keyCode == 40 || e.keyCode == 38) {

                var spr = '_ctl'
                if (obj.match('_ctrl') != null)
                    spr = '_ctrl'

                var arr = obj.split(spr)
                var count = arr[1].substring(0, 2)
                if (count.match("_") != null)
                    count = count.substring(0, count.length - 1)

                var end = arr[1].substring(2)
                if (end.match("_") == null)
                    end = "_" + end;

                var no = 0

                if (e.keyCode == 40)
                    no = parseInt(count) + 1;
                else
                    no = parseInt(count) - 1;

                var num = "";
                if (spr == "_ctl") {
                    if (no < 10)
                        num = "_ctl0" + no
                    else
                        num = "_ctl" + no;
                }
                else
                    num = "_ctrl"+ no;

                var val = arr[0] + num + end
                var txt = document.getElementById(val)
                if (txt != null)
                    if (txt.disabled) {

                        var rowIndex = 0

                        if (e.keyCode == 40)
                            rowIndex = no + 1
                        else
                            rowIndex = no - 1

                        var txtNext;

                        if (spr == "_ctl") {
                            if (rowIndex < 10)
                                txtNext = document.getElementById(arr[0] + "_ctl0" + rowIndex + end)
                            else
                                txtNext = document.getElementById(arr[0] + "_ctl" + rowIndex + end)
                        }
                        else
                            txtNext = document.getElementById(arr[0] + "_ctrl" + rowIndex + end)

                        while (txtNext != null) {

                            if (!txtNext.disabled) {
                                txtNext.focus();
                                break
                            }

                            if (e.keyCode == 40)
                                rowIndex = rowIndex + 1
                            else
                                rowIndex = rowIndex - 1

                            if (spr == "_ctl") {
                                if (rowIndex < 10)
                                    txtNext = document.getElementById(arr[0] + "_ctl0" + rowIndex + end)
                                else
                                    txtNext = document.getElementById(arr[0] + "_ctl" + rowIndex + end)
                            }
                            else
                                txtNext = document.getElementById(arr[0] + "_ctrl" + rowIndex + end)
                        }
                    }
                    else
                        document.getElementById(val).focus()
            }
            }

            function AllowOnlyNameFormat(event) {
                var regex = new RegExp("^[a-zA-Z_ -]+$");
                var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                if (!regex.test(key)) {
                    event.preventDefault();
                    return false;
                }
            }