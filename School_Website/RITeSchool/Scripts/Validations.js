/* Validations.js - This file contains all the validations reqiured for the client side.
*  Follwing are the public methods which are avaiable for any of the .aspx page.
* CheckIfAtleastOneCheckboxInGridIsSelected - the function checks atleast one checkbox from the grid is selected.
* CheckAllOrUncheckAllGridItems - the function checks/unchecks all the items from the grid.
*/
var blanks = " \t\n\r";
function getStartIndex(abPaging) {
    var iStart;


    if (abPaging == true) {
        iStart = 3;
    }
    else {
        iStart = 2;
    }

    return iStart;
}

function DuplicateTextValidation(oDocument, sGridName, sTxtName, sChkName, abPaging, abIgnoreEmpty) {

    var bResult = false;
    var iLoopCounter;
    var iJCounter;
    var grdStandard = document.getElementById(sGridName);
    var iRowcount = grdStandard.rows.length;
    iLoopCounter = getStartIndex(abPaging);
    iRowcount = iRowcount + 1;

    for (; iLoopCounter < iRowcount - 1; iLoopCounter++) {

        var sStandard;
        var ChkStandard;
        var sRow, sChkRow;

        if (iLoopCounter < 10) {
            sRow = "_ctl0";
        }
        else {
            sRow = "_ctl";
        }
        sTxtId = sGridName + sRow + iLoopCounter + "_" + sTxtName;
        sChkId = sGridName + sRow + iLoopCounter + "_" + sChkName;
        sStandard = oDocument.getElementById(sTxtId).value.trim();
        ChkStandard = oDocument.getElementById(sChkId);

        var bIgnoreEmpty = abIgnoreEmpty || false;

        if (ChkStandard.checked && (!bIgnoreEmpty && sStandard != '')) {
            for (iJCounter = iLoopCounter + 1; iJCounter < iRowcount; iJCounter++) {
                var sTxtStandardId;
                var ChkCmpId;
                if (iJCounter < 10) {
                    sChkRow = "_ctl0";
                }
                else {
                    sChkRow = "_ctl";
                }
                sTxtStandardId = sGridName + sChkRow + iJCounter + "_" + sTxtName;
                ChkCmpId = sGridName + sChkRow + iJCounter + "_" + sChkName; ;
                var ChkCmpStd, sCmpStandard;
                sCmpStandard = oDocument.getElementById(sTxtStandardId).value.trim();
                ChkCmpStd = oDocument.getElementById(ChkCmpId);
                if (ChkCmpStd.checked) {
                    if (sStandard.toUpperCase() == sCmpStandard.toUpperCase()) {
                        return false;
                    }
                }

            }
        }

    }
    return true;
}

function CheckAtleastOneCheckBox(_clientListViewId, oChkBoxId, iRowCount) {
    var bFlag = false;
    if (($($(_clientListViewId) + 'input:checkbox[id*=' + oChkBoxId + ']:checked').length) > 0)
        bFlag = true;
    return bFlag;
}

function CheckOrUnCheckAllCheckBox(_clientListViewId, oChkBoxId, iRowCount) {
    var i;
    var oListView = document.getElementById(_clientListViewId);
    var oHdrChk = document.getElementById(_clientListViewId + "_" + oChkBoxId);
    for (i = 0; i < iRowCount - 1; i++) {
        var chk = _clientListViewId + "_ctrl" + i + "_" + oChkBoxId;
        document.getElementById(chk).checked = oHdrChk.checked;
    }
}

function CheckIfAtleastOneCheckboxInGridIsSelected(oDocument, sGridName, sCheckBoxNameInGrid, sAction, bCheckForMoreThanOneSelection, ipagecount, IsalertDisplay) {
    //    var n = oDocument.getElementById(sGridName).rows.length + 1;
    //    var i, j = 0;
    //    var start;


    //    start = getStartIndex(ipagecount);

    //    for (i = start; i <= n; i++) {

    //        var bChecked = false;
    //        if (i < 10) {
    //            if (ipagecount == 1) {
    //                var k = i + 1;
    //                if (k < 10) {
    //                    if (oDocument.getElementById(sGridName + "_ctl0" + k + "_" + sCheckBoxNameInGrid) != null)
    //                        bChecked = oDocument.getElementById(sGridName + "_ctl0" + k + "_" + sCheckBoxNameInGrid).checked;
    //                }
    //                else {
    //                    if (oDocument.getElementById(sGridName + "_ctl" + k + "_" + sCheckBoxNameInGrid) != null)
    //                        bChecked = oDocument.getElementById(sGridName + "_ctl" + k + "_" + sCheckBoxNameInGrid).checked;
    //                }
    //            }
    //            else {
    //                if (oDocument.getElementById(sGridName + "_ctl0" + i + "_" + sCheckBoxNameInGrid) != null)
    //                    bChecked = oDocument.getElementById(sGridName + "_ctl0" + i + "_" + sCheckBoxNameInGrid).checked;
    //            }

    //        }
    //        else {
    //            var k = i + 1;
    //            if (ipagecount == 1) {
    //                if (oDocument.getElementById(sGridName + "_ctl" + k + "_" + sCheckBoxNameInGrid) != null)
    //                    bChecked = oDocument.getElementById(sGridName + "_ctl" + k + "_" + sCheckBoxNameInGrid).checked;
    //            }
    //            else {
    //                if (oDocument.getElementById(sGridName + "_ctl" + i + "_" + sCheckBoxNameInGrid) != null)
    //                    bChecked = oDocument.getElementById(sGridName + "_ctl" + i + "_" + sCheckBoxNameInGrid).checked;
    //            }
    //        }

    //        if (bChecked == true) {
    //            j++;
    //        }
    //    }

    if (($($(sGridName) + 'input:checkbox[id*=' + sCheckBoxNameInGrid + ']:checked').length) < 1) {
        if (IsalertDisplay == 'true') {
            if (sAction == null)
                alert("No checkbox selected for this action.");
            else
                alert(sAction);
        }
        else {
            if (sAction == null)
                return true;
            else
                return false;
        }
        return false;
    }

    if (bCheckForMoreThanOneSelection != "false") {

        if (($($(sGridName) + 'input:checkbox[id*=' + sCheckBoxNameInGrid + ']:checked').length) > 1) {
            if (sAction == null)
                alert("Please select only one element for action.");
            else
                alert("Please select only one element for " + sAction);
            return false;
        }
    }
    return true;
}

function CheckIfAtleastOneRadioButtonInGridIsSelected(oDocument, sGridName, sradioButtonNameInGrid, sAction, bCheckForMoreThanOneSelection, ipagecount, IsalertDisplay) {

    if (($($(sGridName) + 'input:radio[id*=' + sradioButtonNameInGrid + ']:checked').length) < 1) {
        if (IsalertDisplay == 'true') {
            if (sAction == null)
                alert("No checkbox selected for this action.");
            else
                alert(sAction);
        }
        else {
            if (sAction == null)
                return true;
            else
                return false;
        }
        return false;
    }

    if (bCheckForMoreThanOneSelection != "false") {

        if (($($(sGridName) + 'input:radio[id*=' + sradioButtonNameInGrid + ']:checked').length) > 1) {
            if (sAction == null)
                alert("Please select only one element for action.");
            else
                alert("Please select only one element for " + sAction);
            return false;
        }
    }
    return true;
}

//this is to select or unselect the datagrid check boxes 
function CheckAllOrUncheckAllGridItems(oDocument, grdid, obj, objlist, iPageCnt) {


//    //this function decides whether to check or uncheck all
//    $($(grdid) + 'input[id*='+objlist+']:checkbox').attr('checked', obj.checked);
        if (obj.checked)
            DGSelectAll(oDocument, grdid, objlist, iPageCnt)
        else
            DGUnselectAll(oDocument, grdid, objlist, iPageCnt)
}
//---------- 

function DGSelectAll(oDocument, grdid, objid, iPageCnt) {
    //.this function is to check all the items
    var chkbox;
    var i = getStartIndex(iPageCnt);

    if (i < 10)
        chkbox = oDocument.getElementById(grdid + "_ctl0" + i + "_" + objid)
    else
        chkbox = oDocument.getElementById(grdid + "_ctl" + i + "_" + objid)

    while (chkbox != null) {
        chkbox.checked = true;
        i = i + 1;
        if (i < 10)
            chkbox = oDocument.getElementById(grdid + "_ctl0" + i + "_" + objid)
        else
            chkbox = oDocument.getElementById(grdid + "_ctl" + i + "_" + objid)
    }

} //-------------- 

function DGUnselectAll(oDocument, grdid, objid, iPageCnt) {
    //.this function is to check all the items
    var chkbox;
    var i = getStartIndex(iPageCnt);

    if (i < 10)
        chkbox = oDocument.getElementById(grdid + "_ctl0" + i + "_" + objid)
    else
        chkbox = oDocument.getElementById(grdid + "_ctl" + i + "_" + objid)

    while (chkbox != null) {
        chkbox.checked = false;
        i = i + 1;
        if (i < 10)
            chkbox = oDocument.getElementById(grdid + "_ctl0" + i + "_" + objid)
        else
            chkbox = oDocument.getElementById(grdid + "_ctl" + i + "_" + objid)
    }
}






function isEmpty(s) {
    s = stripLeadingTrailingBlanks(s);
    return ((s == null) || (s.length == 0));
}

// Removes leading blank chars (as defined by blanks) from s

function stripLeadingBlanks(s) {
    var i = 0;
    while ((i < s.length) && (blanks.indexOf(s.charAt(i)) != -1))
        i++;
    return s.substring(i, s.length);
}


// Removes trailing blank chars (as defined by blanks) from s

function stripTrailingBlanks(s) {
    var i = s.length - 1;
    while ((i >= 0) && (blanks.indexOf(s.charAt(i)) != -1))
        i--;
    return s.substring(0, i + 1);
}


// Removes leading+trailing blank chars (as defined by blanks) from s

function stripLeadingTrailingBlanks(s) {
    s = stripLeadingBlanks(s);
    s = stripTrailingBlanks(s);
    return s;
}

// Returns true if string is a valid email address: @ and . required,
// at least one char before @, at least one char before and after .

function isEmail(emailStr) {
    /* The following pattern is used to check if the entered e-mail address
    fits the user@domain format.  It also is used to separate the username
    from the domain. */
    var emailPat = /^(.+)@(.+)$/
    /* The following string represents the pattern for matching all special
    characters.  We don't want to allow special characters in the address. 
    These characters include ( ) < > @ , ; : \ " . [ ]    */
    var specialChars = "\\(\\)<>@,;:\\\\\\\"\\.\\[\\]"
    /* The following string represents the range of characters allowed in a 
    username or domainname.  It really states which chars aren't allowed. */
    var validChars = "\[^\\s" + specialChars + "\]"
    /* The following pattern applies if the "user" is a quoted string (in
    which case, there are no rules about which characters are allowed
    and which aren't; anything goes).  E.g. "jiminy cricket"@disney.com
    is a legal e-mail address. */
    var quotedUser = "(\"[^\"]*\")"
    /* The following pattern applies for domains that are IP addresses,
    rather than symbolic names.  E.g. joe@[123.124.233.4] is a legal
    e-mail address. NOTE: The square brackets are required. */
    var ipDomainPat = /^\[(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})\]$/
    /* The following string represents an atom (basically a series of
    non-special characters.) */
    var atom = validChars + '+'
    /* The following string represents one word in the typical username.
    For example, in john.doe@somewhere.com, john and doe are words.
    Basically, a word is either an atom or quoted string. */
    var word = "(" + atom + "|" + quotedUser + ")"
    // The following pattern describes the structure of the user
    var userPat = new RegExp("^" + word + "(\\." + word + ")*$")
    /* The following pattern describes the structure of a normal symbolic
    domain, as opposed to ipDomainPat, shown above. */
    var domainPat = new RegExp("^" + atom + "(\\." + atom + ")*$")
    /* Finally, let's start trying to figure out if the supplied address is
    valid. */

    /* Begin with the coarse pattern to simply break up user@domain into
    different pieces that are easy to analyze. */
    var matchArray = emailStr.match(emailPat)
    if (matchArray == null) {
        /* Too many/few @'s or something; basically, this address doesn't
        even fit the general mould of a valid e-mail address. */
        //alert("Email address seems incorrect (check @ and .'s)")
        return false
    }
    var user = matchArray[1]
    var domain = matchArray[2]

    // See if "user" is valid 
    if (user.match(userPat) == null) {
        // user is not valid
        //alert("The username doesn't seem to be valid.")
        return false
    }

    /* if the e-mail address is at an IP address (as opposed to a symbolic
    host name) make sure the IP address is valid. */
    var IPArray = domain.match(ipDomainPat)
    if (IPArray != null) {
        // this is an IP address
        for (var i = 1; i <= 4; i++) {
            if (IPArray[i] > 255) {
                //      alert("Destination IP address is invalid!")
                return false
            }
        }
        return true
    }

    // Domain is symbolic name
    var domainArray = domain.match(domainPat)
    if (domainArray == null) {
        //alert("The domain name doesn't seem to be valid.")
        return false
    }

    /* domain name seems valid, but now make sure that it ends in a
    three-letter word (like com, edu, gov) or a two-letter word,
    representing country (uk, nl), and that there's a hostname preceding 
    the domain or country. */

    /* Now we need to break up the domain to get a count of how many atoms
    it consists of. */
    var atomPat = new RegExp(atom, "g")
    var domArr = domain.match(atomPat)
    var len = domArr.length
    if (domArr[domArr.length - 1].length < 2 ||
    domArr[domArr.length - 1].length > 3) {
        // the address must end in a two letter or three letter word.
        //alert("The address must end in a three-letter domain, or two letter country.")
        return false
    }

    // Make sure there's a host name preceding the domain.
    if (len < 2) {
        var errStr = "This address is missing a hostname!"
        //alert(errStr)
        return false
    }

    // If we've gotten this far, everything's valid!
    return true;
}

function SelectAllCheckBoxes(obj, sGridName) {
    $($(sGridName) + 'input:checkbox').attr('checked', obj.checked);
}

function ChkIfAtleastOneCheckedInEachColumn(oDocument, sGridName, iRowNumber, checkUnckeck) {
    var start;
    // if (ipagecount>1)
    start = 3;
    //  else
    // start=2;

    var bReturn = true;
    var sArr = new Array();
    var k = 0;
    var sId;
    var n = (oDocument.getElementById(sGridName).rows.length);
    var nCols = (oDocument.getElementById(sGridName).rows[0].cells.length - 1);
    var nRows = n + start - 1; //(no of rows + row no for 1st row - header row)
    var sRow = "";
    var sCol = "";

    for (var j = 0; j < nCols; j++) {

        if (j < 10)
            sCol = "ctl0";
        else
            sCol = "ctl";
        // for(var i=start; i<nRows; i++)    
        {
            if (iRowNumber < 10) {
                sRow = "_ctl0";
            }
            else {
                sRow = "_ctl";
            }

            sId = sGridName + sRow + iRowNumber + "_" + sCol + j;
            if (oDocument.getElementById(sId) != null) {
                oDocument.getElementById(sId).checked = checkUnckeck;
            }

        }

    }
}

function ChkIfAtleastOneCheckedInEachRow(oDocument, sGridName, iPageCnt) {
    var start;
    start = getStartIndex(iPageCnt);

    var bReturn = true;
    var sArr = new Array();
    var k = 0;
    var sId;
    var n = (oDocument.getElementById(sGridName).rows.length);
    var nCols = (oDocument.getElementById(sGridName).rows[0].cells.length - 1);
    var nRows = n + start - 1; //(no of rows + row no for 1st row - header row)
    var sRow = "";
    var sCol = "";

    for (var i = start; i < nRows; i++) {
        if (i < 10) {
            sRow = "_ctl0";
        }
        else {
            sRow = "_ctl";
        }
        for (var j = 0; j < nCols; j++) {

            if (j < 10)
                sCol = "ctl0";
            else
                sCol = "ctl";

            sId = sGridName + sRow + i + "_" + sCol + j;

            if (oDocument.getElementById(sId) != null) {
                if (oDocument.getElementById(sId).checked) {
                    sArr[k] = i;
                    k++;
                    break;
                }
            }

        }

    }


    if (sArr.length < (nRows - start)) {
        bReturn = false;
    }
    else {
        bReturn = true;
    }
    return bReturn;
}

function ChkIfAtleastOneCheckedInEachColumn(oDocument, sGridName, iPageCnt) {

    var start;

    start = getStartIndex(iPageCnt);

    var bReturn = true;
    var sArr = new Array();
    var k = 0;
    var sId;
    var n = (oDocument.getElementById(sGridName).rows.length);
    var nCols = (oDocument.getElementById(sGridName).rows[0].cells.length - 1);
    var nRows = n + start - 1; //(no of rows + row no for 1st row - header row)
    var sRow = "";
    var sCol = "";

    for (var j = 0; j < nCols; j++) {

        if (j < 10)
            sCol = "ctl0";
        else
            sCol = "ctl";
        for (var i = start; i < nRows; i++) {
            if (i < 10) {
                sRow = "_ctl0";
            }
            else {
                sRow = "_ctl";
            }

            sId = sGridName + sRow + i + "_" + sCol + j;
            if (oDocument.getElementById(sId) != null) {
                if (oDocument.getElementById(sId).checked) {
                    sArr[k] = i;
                    k++;
                    break
                }
            }
        }
    }
    var chkRowColCnt = 0;
    for (var j = 0; j < nCols; j++) {
        if (oDocument.getElementById(sGridName).rows[0].cells[j].childNodes[0].type == "checkbox") {
            chkRowColCnt++;
        }
    }

    if (sArr.length < (chkRowColCnt)) {
        bReturn = false;
    }
    else {
        bReturn = true;
    }
    return bReturn;
}

function CheckAllInColumn(oDocument, sGridName, colNumber, Checked, iPageCnt) {


    var start;
    start = getStartIndex(iPageCnt);
    var bReturn = true;
    var sArr = new Array();
    var k = 0;
    var sId;
    var n = (oDocument.getElementById(sGridName).rows.length);
    var nCols = (oDocument.getElementById(sGridName).rows[0].cells.length - 1);
    var nRows = n + start - 1; //(no of rows + row no for 1st row - header row)
    var sRow = "";
    var sCol = "";
    for (var i = start; i < nRows; i++) {

        if (i < 10) {
            sRow = "_ctl0";
        }
        else {
            sRow = "_ctl";
        }

        if (colNumber < 10)
            sCol = "ctl0";
        else
            sCol = "ctl";

        sId = sGridName + sRow + i + "_" + sCol + colNumber;

        if (oDocument.getElementById(sId) != null) {
            oDocument.getElementById(sId).checked = Checked;
        }
    }


}
//2 diamensional grid
function CheckAllInRow(oDocument, sGridName, RowNumber, Checked, iPageCnt) {

    var bReturn = true;
    var sArr = new Array();
    var k = 0;
    var sId;
    var n = (oDocument.getElementById(sGridName).rows.length);
    var nCols = (oDocument.getElementById(sGridName).rows[0].cells.length - 1);
    var sRow = "";
    var sCol = "";
    var start;
    start = getStartIndex(iPageCnt);

    RowNumber = parseInt(RowNumber) + parseInt(start);

    for (var j = 0; j < nCols; j++) {

        if (RowNumber < 10) {
            sRow = "_ctl0";
        }
        else {
            sRow = "_ctl";
        }

        if (j < 10)
            sCol = "ctl0";
        else
            sCol = "ctl";

        sId = sGridName + sRow + RowNumber + "_" + sCol + j;

        if (oDocument.getElementById(sId)) {
            oDocument.getElementById(sId).checked = Checked;
        }

    }
}

function GetReturnValue(args, bFlag) {
    args.IsValid = bFlag;
    return !bFlag;
}

function GetFormattedDate(sDate) {
    if (document.all)
        sDate = new Date(sDate.replace('-', ' '));
    else
        sDate = new Date(convertdate(sDate));
    return sDate
}

function SetErrorMessage(oSrc, ControlId, sErrMsg) {
    oSrc.errormessage = sErrMsg;
    $get(ControlId).errormessage = sErrMsg;
}

//check if atleast 1 checkbox is checked in the grid
//function ChkIfAtleastOneCheckedInTwoDGrid(oDocument, sGridName) {
//    var start;
//    // if (ipagecount>1)
//    start = 3;
//    //  else
//    // start=2;

//    var bReturn = false;
//    var sArr = new Array();
//    var k = 0;
//    var sId;
//    var n = (oDocument.getElementById(sGridName).rows.length);
//    var nCols = (oDocument.getElementById(sGridName).rows[0].cells.length - 1);
//    var nRows = n + start - 1; //(no of rows + row no for 1st row - header row)
//    var sRow = "";
//    var sCol = "";

//    for (var j = 0; j < nCols; j++) {

//        if (j < 10)
//            sCol = "ctl0";
//        else
//            sCol = "ctl";
//        for (var i = start; i < nRows; i++) {
//            if (i < 10) {
//                sRow = "_ctl0";
//            }
//            else {
//                sRow = "_ctl";
//            }

//            sId = sGridName + sRow + i + "_" + sCol + j;
//            if (oDocument.getElementById(sId) != null) {
//                if (oDocument.getElementById(sId).checked) {

//                    bReturn = true;
//                    break
//                }
//            }
//        }
//    }
//    return bReturn;
//}