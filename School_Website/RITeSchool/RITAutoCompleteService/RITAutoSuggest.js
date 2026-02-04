jQuery.support.cors = true;
// This function is used to bind auto complete event to textbox.

var _newSearchText;
var _ajaxGetSearchResultTimer;
var _xhRequest;

function BindAutoCompleteEvent(schoolId, academicYearId, txtobj, cmbstandard, cmbDivision, cmbstdDiv, showLeftStudent) {
    BindAutoCompleteEventForStudent(schoolId, academicYearId, txtobj, cmbstandard, cmbDivision, cmbstdDiv, showLeftStudent, true, false);
}

//function BindAutoCompleteEvent(schoolId, academicYearId, txtobj, standardId, DivisionId, stdDivId, showLeftStudent) {
function BindAutoCompleteEventForStudent(schoolId, academicYearId, txtobj, cmbstandard, cmbDivision, cmbstdDiv, showLeftStudent, includeRegNo, showOnlyLeftStudents) {
    $(txtobj).autocomplete({
        source:
				function (request, response) {
				    var sSearchText = $(txtobj)[0].value;
				    var standardId = 0, DivisionId = 0, stdDivId = 0;

				    if (document.getElementById(cmbstandard) != null)
				        standardId = document.getElementById(cmbstandard).value;

				    if (document.getElementById(cmbDivision) != null && document.getElementById(cmbDivision).value != "-- All --")
				        DivisionId = document.getElementById(cmbDivision).value;

				    if (document.getElementById(cmbstdDiv) != null)
				        stdDivId = document.getElementById(cmbstdDiv).value; ;

				    _newSearchText = sSearchText;

				    //clear already running time out
				    clearTimeout(_ajaxGetSearchResultTimer);

				    //wait for user to finish typing (1 sec in this case) and then make AJAX call
				    _ajaxGetSearchResultTimer = setTimeout(function () {
				        //check current value with that of value present in textbox 1 sec back and if it is same then make AJAX call
				        if ($(txtobj)[0].value == _newSearchText) {
				            GetDataForAutoComplete(request, response, schoolId, academicYearId, sSearchText, standardId, DivisionId, stdDivId, showLeftStudent, includeRegNo, showOnlyLeftStudents);
				        }
				    }, 1000);
				},
        select: function (event, ui) { SearchSelectedValue(ui.item.value) }
    })

    $.ui.autocomplete.prototype._renderItem = function (ul, item) {
        item.label = item.label.replace(new RegExp("(?![^&;]+;)(?!<[^<>]*)(" + $.ui.autocomplete.escapeRegex(this.term) + ")(?![^<>]*>)(?![^&;]+;)", "gi"), "<strong>$1</strong>");
        return $("<li></li>").data("item.autocomplete", item)
                                    .append("<a>" + item.label + "</a>")
                                    .appendTo(ul);
    };

    $(".ui-autocomplete").css("overflow-y", "auto");
    $(".ui-autocomplete").css("height", "200px");
}

function GetDataForAutoComplete(request, response, schoolId, academicYearId, searchText, standardId, divisionId, stdDivId, showLeftStudent, includeRegNo, showOnlyLeftStudents) {
    var serviceUrl = '/RITeSchool/RITAutoCompleteService/SchoolAutoSearchService.svc/StudentAutoSearch';
    var datatype = "json";

    if (_xhRequest != null && _xhRequest != undefined)
        _xhRequest.abort();

    _xhRequest = $.ajax({
        type: "POST",
        url: serviceUrl,
        //data: '{"asSearchText": "' + searchText + '","aiSchoolId":"' + schoolId + '","aiAcademicYearId":"' + academicYearId + '","aiStandardId":"' + standardId + '","aiDivisionId":"' + divisionId + '","aiStdDivId":"' + stdDivId + '","asShowLeftStudents":"' + showLeftStudent + '"}',        
        data: '{"asSearchText": "' + searchText + '","aiSchoolId":"' + schoolId + '","aiAcademicYearId":"' + academicYearId + '","aiStandardId":"' + standardId + '","aiDivisionId":"' + divisionId + '","aiStdDivId":"' + stdDivId + '","asShowLeftStudents":"' + showLeftStudent + '","abIncludeRegNo":"' + includeRegNo + '","abShowOnlyLeftStudents":"'+showOnlyLeftStudents+'"}',
        contentType: "application/json; charset=utf-8",                
        dataType: datatype,
        crossDomain: true,
        async: true,
        success: function (data) {            
            response($.map(data.StudentAutoSearchResult, function (item) {
                return {
                    label: item,
                    value: item
                }
            })
				  );         
        },
        error: function (msg) {
           // alert(msg.statusText);
        }
    });
}


function BindAutoCompleteEventForStaff(schoolId, academicYearId, txtobj, cmbUserRole, showDeleted) {
    $(txtobj).autocomplete({
        source:
				function (request, response) {				    
				    var sSearchText = $(txtobj)[0].value;
				    var userRoleId = 0

				    if (document.getElementById(cmbUserRole) != null)
				        userRoleId = document.getElementById(cmbUserRole).value;

				    _newSearchText = sSearchText;

				    //clear already running time out
				    clearTimeout(_ajaxGetSearchResultTimer);

				    //wait for user to finish typing (1 sec in this case) and then make AJAX call
				    _ajaxGetSearchResultTimer = setTimeout(function () {
				        //check current value with that of value present in textbox 1 sec back and if it is same then make AJAX call
				        if ($(txtobj)[0].value == _newSearchText) {
				            GetStaffDataForAutoComplete(request, response, schoolId, academicYearId, sSearchText, userRoleId, showDeleted);
				        }
				    }, 1000);
				},
		 select: function (event, ui) { SearchSelectedValue(ui.item.value) }
    })

    $.ui.autocomplete.prototype._renderItem = function (ul, item) {
        item.label = item.label.replace(new RegExp("(?![^&;]+;)(?!<[^<>]*)(" + $.ui.autocomplete.escapeRegex(this.term) + ")(?![^<>]*>)(?![^&;]+;)", "gi"), "<strong>$1</strong>");
        return $("<li></li>").data("item.autocomplete", item)
                                    .append("<a>" + item.label + "</a>")
                                    .appendTo(ul);
    };

    $(".ui-autocomplete").css("overflow-y", "auto");
    $(".ui-autocomplete").css("height", "200px");
}

function BindAutoCompleteEventForStaffWithStatus(schoolId, academicYearId, txtobj, cmbUserRole, showDeleted,status) {
    $(txtobj).autocomplete({
        source:
				function (request, response) {
				    var sSearchText = $(txtobj)[0].value;
				    var userRoleId = 0,
                        statusId = 0

				    if (document.getElementById(cmbUserRole) != null)
				        userRoleId = document.getElementById(cmbUserRole).value;

				    if (document.getElementById(status) != null)
				        statusId = document.getElementById(status).value

				    _newSearchText = sSearchText;

				    //clear already running time out
				    clearTimeout(_ajaxGetSearchResultTimer);

				    //wait for user to finish typing (1 sec in this case) and then make AJAX call
				    _ajaxGetSearchResultTimer = setTimeout(function () {
				        //check current value with that of value present in textbox 1 sec back and if it is same then make AJAX call
				        if ($(txtobj)[0].value == _newSearchText) {
				            GetStaffDataWithStatusForAutoComplete(request, response, schoolId, academicYearId, sSearchText, userRoleId, showDeleted, statusId);
				        }
				    }, 1000);
				},
        select: function (event, ui) { SearchSelectedValue(ui.item.value) }
    })

    $.ui.autocomplete.prototype._renderItem = function (ul, item) {
        item.label = item.label.replace(new RegExp("(?![^&;]+;)(?!<[^<>]*)(" + $.ui.autocomplete.escapeRegex(this.term) + ")(?![^<>]*>)(?![^&;]+;)", "gi"), "<strong>$1</strong>");
        return $("<li></li>").data("item.autocomplete", item)
                                    .append("<a>" + item.label + "</a>")
                                    .appendTo(ul);
    };

    $(".ui-autocomplete").css("overflow-y", "auto");
    $(".ui-autocomplete").css("height", "200px");
}

function GetStaffDataForAutoComplete(request, response, schoolId, academicYearId, searchText, userRoleId, showDeleted) {
    var serviceUrl = '/RITeSchool/RITAutoCompleteService/SchoolAutoSearchService.svc/StaffAutoSearch';
    var datatype = "json";

    if (_xhRequest != null && _xhRequest != undefined)
        _xhRequest.abort();

    _xhRequest = $.ajax({
        type: "POST",
        url: serviceUrl,
        data: '{"asSearchText": "' + searchText + '","aiSchoolId":"' + schoolId + '","aiAcademicYearId":"' + academicYearId + '","aiUserRoleId":"' + userRoleId + '","asShowDeleted":"' + showDeleted + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: datatype,
        crossDomain: true,
        async: true,
        success: function (data) {
            response($.map(data.StaffAutoSearchResult, function (item) {
                return {
                    label: item,
                    value: item
                }
            })
				  );
        },
        error: function (msg) {
            //alert(msg.statusText);
        }
    });
}

function GetStaffDataWithStatusForAutoComplete(request, response, schoolId, academicYearId, searchText, userRoleId, showDeleted, status) {
    var serviceUrl = '/RITeSchool/RITAutoCompleteService/SchoolAutoSearchService.svc/StaffAutoSearchWithStatus';
    var datatype = "json";

    if (_xhRequest != null && _xhRequest != undefined)
        _xhRequest.abort();

    _xhRequest = $.ajax({
        type: "POST",
        url: serviceUrl,
        data: '{"asSearchText": "' + searchText + '","aiSchoolId":"' + schoolId + '","aiAcademicYearId":"' + academicYearId + '","aiUserRoleId":"' + userRoleId + '","asShowDeleted":"' + showDeleted + '","aiStatusId":"'+status+'"}',
        contentType: "application/json; charset=utf-8",
        dataType: datatype,
        crossDomain: true,
        async: true,
        success: function (data) {
            response($.map(data.StaffAutoSearchWithStatusResult, function (item) {
                return {
                    label: item,
                    value: item
                }
            })
				  );
        },
        error: function (msg) {
            //alert(msg.statusText);
        }
    });
}

function BindAutoCompleteEventForAllUser(schoolId, academicYearId, txtobj, cmbUserRole, showDeleted) {
    $(txtobj).autocomplete({
        source:
				function (request, response) {				    
				    var sSearchText = $(txtobj)[0].value;
				    var userRoleId = 0

				    if (document.getElementById(cmbUserRole) != null)
				        userRoleId = document.getElementById(cmbUserRole).value;

				    _newSearchText = sSearchText;

				    //clear already running time out
				    clearTimeout(_ajaxGetSearchResultTimer);

				    //wait for user to finish typing (1 sec in this case) and then make AJAX call
				    _ajaxGetSearchResultTimer = setTimeout(function () {
				        //check current value with that of value present in textbox 1 sec back and if it is same then make AJAX call
				        if ($(txtobj)[0].value == _newSearchText) {
				            GetUserDataForAutoComplete(request, response, schoolId, academicYearId, sSearchText, userRoleId, showDeleted);
				        }
				    }, 1000);
				},
		select: function (event, ui) { SearchSelectedValue(ui.item.value) }
    })

    $.ui.autocomplete.prototype._renderItem = function (ul, item) {
        item.label = item.label.replace(new RegExp("(?![^&;]+;)(?!<[^<>]*)(" + $.ui.autocomplete.escapeRegex(this.term) + ")(?![^<>]*>)(?![^&;]+;)", "gi"), "<strong>$1</strong>");
        return $("<li></li>").data("item.autocomplete", item)
                                    .append("<a>" + item.label + "</a>")
                                    .appendTo(ul);
    };

    $(".ui-autocomplete").css("overflow-y", "auto");
    $(".ui-autocomplete").css("height", "200px");
}

function GetUserDataForAutoComplete(request, response, schoolId, academicYearId, searchText, userRoleId, showDeleted) {
    var serviceUrl = '/RITeSchool/RITAutoCompleteService/SchoolAutoSearchService.svc/UserAutoSearch';
    var datatype = "json";

    if (_xhRequest != null && _xhRequest != undefined)
        _xhRequest.abort();

    _xhRequest = $.ajax({
        type: "POST",
        url: serviceUrl,
        data: '{"asSearchText": "' + searchText + '","aiSchoolId":"' + schoolId + '","aiAcademicYearId":"' + academicYearId + '","aiUserRoleId":"' + userRoleId + '","asShowDeleted":"' + showDeleted + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: datatype,
        crossDomain: true,
        async: true,
        success: function (data) {
            response($.map(data.UserAutoSearchResult, function (item) {
                return {
                    label: item,
                    value: item
                }
            })
				  );
        },
        error: function (msg) {
            //alert(msg.statusText);
        }
    });
}

function BindAutoCompleteEventForUser(schoolId, academicYearId, txtobj, cmbUserRole, showDeleted, cmbstandard, cmbDivision, cmbstdDiv) {
    var userRoleId = 0    
    if (document.getElementById(cmbUserRole) != null)
        userRoleId = document.getElementById(cmbUserRole).value;

    if (userRoleId == 0)
        BindAutoCompleteEventForAllUser(schoolId, academicYearId, txtobj, cmbUserRole, showDeleted);
    else if (userRoleId == 3 || userRoleId == 9)
        BindAutoCompleteEventForStudent(schoolId, academicYearId, txtobj, cmbstandard, cmbDivision, cmbstdDiv, showDeleted, false, false)   
    else
        BindAutoCompleteEventForStaff(schoolId, academicYearId, txtobj, cmbUserRole, showDeleted)
}

function SearchResult(txt, val, bt) {   
    txt.value = val;    
    bt.click();
}

function BindAutoCompleteEventforOnlyLeftStudent(schoolId, academicYearId, txtobj, cmbstandard, cmbDivision, cmbstdDiv) {
    BindAutoCompleteEventForStudent(schoolId, academicYearId, txtobj, cmbstandard, cmbDivision, cmbstdDiv, true, true, true);
}

function BindAutoCompleteEventForMessageCenter(schoolId, academicYearId, txtobj, cmbUserRole, userId,hidObj) {
    $(txtobj).autocomplete({
        source:
				function (request, response) {
				    var sSearchText = $(txtobj)[0].value;
				    var userRoleId = 0

				    if (document.getElementById(cmbUserRole) != null)
				        userRoleId = document.getElementById(cmbUserRole).value;

				    var showOnlyCoordinator = false;
				    if (document.getElementById(hidObj) != null)
				        showOnlyCoordinator = document.getElementById(hidObj).value;

				    _newSearchText = sSearchText;

				    //clear already running time out
				    clearTimeout(_ajaxGetSearchResultTimer);

				    //wait for user to finish typing (1 sec in this case) and then make AJAX call
				    _ajaxGetSearchResultTimer = setTimeout(function () {
				        //check current value with that of value present in textbox 1 sec back and if it is same then make AJAX call
				        if ($(txtobj)[0].value == _newSearchText) {
				            GetUserDataForMessageCenter(request, response, schoolId, academicYearId, sSearchText, userRoleId, userId, showOnlyCoordinator);
				        }
				    }, 1000);
				},
				select: function (event, ui) { SearchSelectedValue(ui.item.value); event.preventDefault() }
    })

    $.ui.autocomplete.prototype._renderItem = function (ul, item) {
        item.label = item.label.replace(new RegExp("(?![^&;]+;)(?!<[^<>]*)(" + $.ui.autocomplete.escapeRegex(this.term) + ")(?![^<>]*>)(?![^&;]+;)", "gi"), "<strong>$1</strong>");
        return $("<li></li>").data("item.autocomplete", item)
                                    .append("<a>" + item.label + "</a>")
                                    .appendTo(ul);
    };

    $(".ui-autocomplete").css("overflow-y", "auto");
    $(".ui-autocomplete").css("height", "200px");
}

function GetUserDataForMessageCenter(request, response, schoolId, academicYearId, searchText, userRoleId, userId, showOnlyCoordinator) {
    var serviceUrl = '/RITeSchool/RITAutoCompleteService/SchoolAutoSearchService.svc/GetDataForMessageCenter';
    var datatype = "json";

    if (_xhRequest != null && _xhRequest != undefined)
        _xhRequest.abort();

    _xhRequest = $.ajax({
        type: "POST",
        url: serviceUrl,
        data: '{"asSearchText": "' + searchText + '","aiSchoolId":"' + schoolId + '","aiAcademicYearId":"' + academicYearId + '","aiUserRoleId":"' + userRoleId + '","aiUserId":"' + userId + '","abShowOnlyCoordinator":"'+showOnlyCoordinator+'"}',
        contentType: "application/json; charset=utf-8",
        dataType: datatype,
        crossDomain: true,
        async: true,
        success: function (data) {
            response($.map(data.GetDataForMessageCenterResult, function (item) {
                return {
                    label: item,
                    value: item
                }
            })
				  );
        },
        error: function (msg) {
            //alert(msg.statusText);
        }
    });
}