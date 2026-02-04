function getDateString(oDtobj) {
    var obj = new Date(oDtobj);
    var strDate = obj.getDate() + "-";
    var strMonth = parseInt(obj.getMonth()) + 1;
    strMonth = getMonthName(strMonth);
    strDate = strDate + strMonth + "-";
    strDate = strDate + obj.getFullYear();
    return strDate;
}
// This function creates a date string with time as 00.
function getFormattedDate(oDtobj) {
    var obj = new Date(oDtobj);
    var strDate = obj.getDate() + "-";
    var strMonth = parseInt(obj.getMonth()) + 1;
    strDate = strDate + strMonth + "-";
    strDate = strDate + obj.getFullYear();
    return strDate;
}
function trimAll(sString) {
    while (sString.substring(0, 1) == ' ') {
        sString = sString.substring(1, sString.length);
    }
    while (sString.substring(sString.length - 1, sString.length) == ' ') {
        sString = sString.substring(0, sString.length - 1);
    }
    while (sString.charCodeAt(sString.length - 1) == 10 || sString.charCodeAt(sString.length - 1) == 13) {
        sString = sString.substring(0, sString.length - 1);
    }
    return sString;
}
function RemoveLeadingZeroes(sString) {
    sString = trimAll(sString);
    if (sString.length > 1) {
        while (sString.substring(0, 1) == '0') {
            sString = sString.substring(1, sString.length);
        }
        if (sString.length == 0) {
            sString = '0';
        }
    }
    return sString;

}
function getMonthName(month) {
    switch (month) {
        case 1:
            return "Jan";
            break;

        case 2:
            return "Feb";
            break;

        case 3:
            return "Mar";
            break;

        case 4:
            return "Apr";
            break;

        case 5:
            return "May";
            break;

        case 6:
            return "Jun";
            break;

        case 7:
            return "Jul";
            break;

        case 8:
            return "Aug";
            break;

        case 9:
            return "Sep";
            break;

        case 10:
            return "Oct";
            break;

        case 11:
            return "Nov";
            break;

        case 12:
            return "Dec";
            break;
    }
}

function Reorder(eSelect, SCheckBoxname, grdid, iCurrentField, numSelects, lblSuccessId) {
    var eForm = eSelect.form;
    var iNewOrder = eSelect.selectedIndex + 1;
    var iPrevOrder;
    var positions = new Array(numSelects);
    var ix;
    var iRowNum

    if (document.getElementById(lblSuccessId) != null)
        document.getElementById(lblSuccessId).innerHTML = '';
    for (ix = 0; ix < numSelects; ix++) {
        positions[ix] = 0;
    }
    for (ix = 0; ix < numSelects; ix++) {
        if ((ix + 2) < 10) {
            iRowNum = "_ctl0";
        }
        else {
            iRowNum = "_ctl";
        }
        positions[document.getElementById(grdid + iRowNum + (ix + 2) + "_" + SCheckBoxname).selectedIndex] = 1;
    }

    for (ix = 0; ix < numSelects; ix++) {
        if (positions[ix] == 0) {
            iPrevOrder = ix + 1;
            break;
        }
    }
    if (iNewOrder != iPrevOrder) {
        var iInc = iNewOrder > iPrevOrder ? -1 : 1
        var iMin = Math.min(iNewOrder, iPrevOrder);
        var iMax = Math.max(iNewOrder, iPrevOrder);

        for (var iField = 0; iField < numSelects; iField++) {
            if (iField != iCurrentField) {
                if ((iField + 2) < 10) {
                    iRowNum = "_ctl0";
                }
                else {
                    iRowNum = "_ctl";
                }
                if (document.getElementById(grdid + iRowNum + (iField + 2) + "_" + SCheckBoxname).selectedIndex + 1 >= iMin &&
					document.getElementById(grdid + iRowNum + (iField + 2) + "_" + SCheckBoxname).selectedIndex + 1 <= iMax) {
                    document.getElementById(grdid + iRowNum + (iField + 2) + "_" + SCheckBoxname).selectedIndex += iInc;
                }
            }
        }
    }
}


function ValidateRollNumbersInListView(SCheckBoxname, SLabelname, SRegLabelname, grdid, numSelects) {
	numSelects = parseInt(numSelects);

    var iPrevOrder;
    var positions = new Array(numSelects + 2);
    var ix;
    var sMsg = "";
	

    for (ix = 2; ix < numSelects + 2; ix++) {
        positions[ix] = 0;
    }

    var iRowNum = "_ctl";
    var iRowNum1 = "_ctl0";
    
	for (ix = 2; ix < numSelects + 2; ix++) {
        if (document.getElementById(grdid + (ix > 9 ? iRowNum : iRowNum1) + (ix) + "_" + SCheckBoxname) != null)
            positions[ix] = document.getElementById(grdid + (ix > 9 ? iRowNum : iRowNum1) + (ix) + "_" + SCheckBoxname).value;
        else// if (document.getElementById(grdid + (ix > 9 ? iRowNum : iRowNum1) + (ix) + "_" + SLabelname) != null)
            positions[ix] = -999;
    }

    for (ix = 2; ix < numSelects + 2; ix++) {
        if (positions[ix] != -999) {
            if (document.getElementById(grdid + (ix > 9 ? iRowNum : iRowNum1) + (ix) + "_" + SCheckBoxname) != null)
                document.getElementById(grdid + (ix > 9 ? iRowNum : iRowNum1) + (ix) + "_" + SCheckBoxname).className = "ExSmlTxtBoxP";
        }
        if (positions[ix] == "" || positions[ix] == "undefined" || parseFloat(positions[ix]) == 0) {
        	var rollNo =  document.getElementById(grdid + (ix > 9 ? iRowNum : iRowNum1) + (ix) + "_" + SCheckBoxname);
        	if (rollNo)	{
				var row = $(rollNo).closest('tr').get(0);
        		if (row) {
					var regNo = row.cells[0].innerHTML;
        			sMsg = sMsg + regNo + ", ";
        		}
        		rollNo.className = "TxtBoxMaxLect";
			}
        }        
    }
    
    var sMessage="";
    if (sMsg.length > 0) {
        sMsg = sMsg.substring(0, sMsg.length - 2);
        sMessage = " Please enter roll number for student(s) having Reg. No. " + sMsg + "\n\r";
    }
    else {
        for (ix = 2; ix < numSelects + 2; ix++) {
            if (positions[ix] != -999) {
                if (document.getElementById(grdid + (ix > 9 ? iRowNum : iRowNum1) + (ix) + "_" + SCheckBoxname) != null)
                    document.getElementById(grdid + (ix > 9 ? iRowNum : iRowNum1) + (ix) + "_" + SCheckBoxname).className = "ExSmlTxtBoxP";
            
				for (iy = 2; iy < numSelects + 2; iy++) {
					if (ix != iy && positions[iy] != -999 && parseFloat(positions[ix]) == parseFloat(positions[iy])) {
						if (sMsg.match(parseFloat(positions[iy]) + ", ") == null) {
                    		sMsg = sMsg + parseFloat(positions[iy]) + ", ";
						}
						if (document.getElementById(grdid + (ix > 9 ? iRowNum : iRowNum1) + (ix) + "_" + SCheckBoxname) != null && document.getElementById(grdid + (iy > 9 ? iRowNum : iRowNum1) + (iy) + "_" + SCheckBoxname) != null) {
							document.getElementById(grdid + (ix > 9 ? iRowNum : iRowNum1) + (ix) + "_" + SCheckBoxname).className = "TxtBoxMaxLect";
							document.getElementById(grdid + (iy > 9 ? iRowNum : iRowNum1) + (iy) + "_" + SCheckBoxname).className = "TxtBoxMaxLect";
						}
					}
				}
			}
        }
       
        if (sMsg.length > 2) {
            sMsg = sMsg.substring(0, sMsg.length - 2);
            sMessage = " Roll number(s) " + sMsg + " are duplicate for students.";
        }
    }
    if (sMessage.length > 0 && sMsg.length > 0) {
        alert(sMessage);
        return false;
    }
    return true;
}

function Round(number, decimalPlaces) {
    return Math.round(number * Math.pow(10, decimalPlaces)) / Math.pow(10, decimalPlaces);
}