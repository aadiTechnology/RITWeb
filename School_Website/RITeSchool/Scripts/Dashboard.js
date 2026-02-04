var _slider;
var _photoGallerySlider;
//This is the milliseconds
var _progressbarStartDelayTime = 500;
var _progressbarStopDelayTime = 1000;
var iSchoolIDforJPS = 111;

$(document).ready(function () {
    try {
        validateAndRemoveLocalStorage();
        var isMVPSSchool = $('#' + _clienthidIsMVPSSchool).val();

        if ((_loggedUserDesignationId == _constPrincipalDesignation && isMVPSSchool == "N")
            || _userRoleId == _constAdminRole
            || (_userRoleId == _constAdminStaff && _supervisorDesignationName == _constSuperviserDesignationName)
            || (_userRoleId == _constAdminStaff && _supervisorDesignationName == _constDirectorDesignationName)) {

            // if principal, admin login and Accounts Cum Admin Officer.
            /* If this is Administrator's first visit or logged in user is Principal then load dashboard details. 
            In case of Administrator, for all visits other than first, dashboard details will be loaded on click of the dashboard toggle button.*/
            if ((_firstVisitForAdmin || readCookie("showNewDashboard") == undefined || readCookie("showNewDashboard") == "true") || _loggedUserDesignationId == _constPrincipalDesignation || _supervisorDesignationName == _constSuperviserDesignationName || _supervisorDesignationName == _constDirectorDesignationName) {

                loadPrincipalAdminDashboard();
            }
        }

        else {

            if (_userRoleId == _constAdminStaff && _supervisorDesignationName == _constSeniorAdministrativeOfficerDesignationName)
                loadAttendanceWidget(true, false);

            var IsFirstLogin = $('#' + _clienthidIsFirstTimeLogin).val();
            var IsFirstTimeLogin;
            if (IsFirstLogin == "Y")
                IsFirstTimeLogin = true;
            else
                IsFirstTimeLogin = false;

            if (!($('#' + _clienthidHideVidgets).val() == '1' && _userRoleId == 3)) {
                loadBirthdayWidget(true, false);
                loadPhotoGalleryWidget(true, IsFirstTimeLogin);
                loadUsersFeedbackWidget(true, false);
                loadUnreadMessageWidget();
            }
            else {
                $('#birthdayRow').hide();
                $('#photoAlbumRow').hide();
                $('#feedbackRow').hide();
                $('#eventRow').hide();

                $('[ID$=lnkMyProfile]').hide();
                $('[ID$=lnkFeedback]').hide();
            }

            if (_userRoleId != "7") {
                loadUpcomingEventsWidget(true, IsFirstTimeLogin);
            }
        }

        setTimeout(function () {
            $("td[id$='tdNoticeBoardMessage']").css('width', '100%');
            $("div[id$='divSchoolNoticeBoard']").show();
        }, 1000);
        // this code is used to set tool tip style.
        $('[data-rel=tooltip]').tooltip({ container: 'body' });
    } catch (e) {
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- document.ready', _userId);
    }
});



// On window resize reload charts.
$(window).resize(function () {
    try {
        if (_loggedUserDesignationId == _constPrincipalDesignation || _userRoleId == _constAdminRole || _supervisorDesignationName == _constSuperviserDesignationName || _supervisorDesignationName == _constDirectorDesignationName || _supervisorDesignationName == _constSeniorAdministrativeOfficerDesignationName){  // if principal/ admin login
            refreshWidgets();
           
        }
    } catch (e) {
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- window.resize', _userId);
    }
});


// Load all widgets.
function loadPrincipalAdminDashboard() {
    try {
    var IsFirstLogin = $('#' + _clienthidIsFirstTimeLogin).val();
        var IsFirstTimeLogin;
        if (IsFirstLogin == "Y")
            IsFirstTimeLogin = true;
        else
            IsFirstTimeLogin = false;

        loadFeeWidget(true, false);
        loadAttendanceWidget(true,false);
        loadExamWiseStudentPerformanceWidget(true, false);
        loadAccountWidget(true, false);
        loadPayrollWidget(true, false);
        loadBirthdayWidget(true, false);
        loadPhotoGalleryWidget(true, IsFirstTimeLogin);
        getStatisticsCurrentTabDetailsCount(true, false);
        loadUsersFeedbackWidget(true, false, "AdminOrPrincipal");
        loadUpcomingEventsWidget(true, IsFirstTimeLogin);
        loadUnreadMessageWidget();

        //After load dashboard set this global variable to false.
        _loadDashboardDetails = false;
    } catch (e) {
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- loadDashboard', _userId);
    }
}


/******************************************* Start - Fee *******************************************/
var _previousSelectedFeeWidgetAcademicYear;

/// Get data for attendance widget and show attendance widget
function loadFeeWidget(loadDefaultData, isRefresh) {
    try {
        showHideKendoProgressbar("divFeeWidget", true, _progressbarStartDelayTime);
        $("#divFeeStatus").removeClass("open");

        var inputParameters =
            {
                aiSchoolId: _schoolId,
                aiAcademicYearId: (loadDefaultData == true && isRefresh == true) ? _academicYearId : $("select[id$='cmbFeeAcademicYear']").val()
            };

        // Local storage key for fee status
        var localStorageKey = "FeeSummary";

        var callback = function (data) {
            if (data.GetFeeSummaryResult != null) {
                $("#divTotalDues").text("Rs. " + data.GetFeeSummaryResult.DuesTillDate);

                // To display the Dues Till Date, total paid fees to JPS School and Concession, Today's Collection For all other schools
                if (_schoolId == iSchoolIDforJPS) {
                    $('#stCollection').html("Total paid fees");
                    $('#stConcession').html("Pending Fees");
                    $("#divTodaysCollection").text("Rs. " + data.GetFeeSummaryResult.TotalPaidFees);
                    $("#divConcession").text("Rs. " + data.GetFeeSummaryResult.DuesTillDate);
                }
                else {
                    $('#stCollection').html("Todays Collection");
                    $('#stConcession').html("Total concession till date");
                    $("#divTodaysCollection").text("Rs. " + data.GetFeeSummaryResult.TodaysCollection);
                    $("#divConcession").text("Rs. " + data.GetFeeSummaryResult.Concession);
                }

                $("#divExpectedAmount").text("Rs. " + data.GetFeeSummaryResult.AmountExpectedToReceive);
                $("#divFeeWidgetContent").show();
                $("#divFeeWidgetMessage").hide();
            }
            else {
                $("#divFeeWidgetContent").hide();
                $("#divFeeWidgetMessage").show();
                removeLocalStorage(localStorageKey);
            }

            $('#lblFeeStatusFilter').text('(Academic Year' + ': ' + $("select[id$='cmbFeeAcademicYear'] option:selected").text() + ')');
            showHideKendoProgressbar("divFeeWidget", false, _progressbarStopDelayTime);

            // This condition is used for to save widget data in local storage when first time load 
            // And refresh local storage case and avoid filter save case.
            if (loadDefaultData == true) {
                var numberOfDaysAhead = 1; //save data in local storage until EOD
                saveToLocalStorage(localStorageKey, data, getExpirationHrs(numberOfDaysAhead)); 
			}
        }

        var errorback = function (msg) {
            showHideKendoProgressbar("divFeeWidget", false, _progressbarStopDelayTime);
            removeLocalStorage(localStorageKey);
        }

        // This condition is used to remove widget data from local storage.
        if (loadDefaultData == true && isRefresh == true) {
            $("select[id$='cmbFeeAcademicYear']").val(_academicYearId);
        }

        // Create data object to pass parameters to get widget details.
        var dataObject = {
            isRefresh: isRefresh,
			loadDefaultData: loadDefaultData,
            keyName: localStorageKey,
            serviceUrl: serviceUrl + "GetFeeSummary",
            data: inputParameters
        };

        // If first time load and local storage is empty or expire then save in local storage
        // This function is used to get fee summary details 
        getWidgetData(dataObject, callback, errorback);
    } catch (e) {
        showHideKendoProgressbar("divFeeWidget", false, _progressbarStopDelayTime);
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- loadFeeWidget', _userId);
    }
}

// Preserve previously selected academic year.
function setFeeWidgetSelectedYear() {
    _previousSelectedFeeWidgetAcademicYear = $("select[id$='cmbFeeAcademicYear']").val();
}

// Reset fee status filters.
function clearFeeStatusFilters() {
    hideFeeStatusWidgetFilter();
    $("select[id$='cmbFeeAcademicYear']").val(_academicYearId);
    loadFeeWidget(false, false);
}

// Hide fee status widget filter.
function hideFeeStatusWidgetFilter(e) {
    $("select[id$='cmbFeeAcademicYear']").val(_previousSelectedFeeWidgetAcademicYear);
    $("#divFeeStatus").removeClass("open");
}

/******************************************* End - Fee *************************************************/

/******************************************* Start - Attendance****************************************/
/// Get data for attendance widget and show attendance widget
var _firstLoadAttendanceDate;
var _previousSelectedAttendanceDate;

function loadAttendanceWidget(loadDefaultData, isRefresh) {
    try {
        $("#divAttendanceSummary").removeClass("open");
        showHideKendoProgressbar("divAttendance", true, _progressbarStartDelayTime);

        var date;
        if (loadDefaultData == true && isRefresh == false) {
            if (_ddlAcademicYear == _academicYearId || _ddlAcademicYear == "0")
                date = new Date();
            else
                date = new Date(_dtAcademicYearEndDate);

            $("#datepicker").kendoDatePicker({ value: date, format: "dd/MM/yyyy", max: date });
            _firstLoadAttendanceDate = date;
        }
		
		// Local storage key for Attendance Summary
        var localStorageKey = "AdminAttendanceSummary";

        var inputParameters =
              {
                  aiSchoolId: _schoolId,
                  aiAcademicYearId: _academicYearId,
                  asDate: (loadDefaultData == true && isRefresh == true) ? kendo.toString(_firstLoadAttendanceDate, 'yyyy-MM-dd') : kendo.toString($("#datepicker").data("kendoDatePicker").value(), 'yyyy-MM-dd'),
                  aiUserId: _userId
              };
              



              var callback = function (data) {
                if (data.GetAttendanceSummaryResult != null) {
                      createGauge(data.GetAttendanceSummaryResult);
                      $("#divAttendanceSummaryWidgetContent").show();
                      $("#divAttendanceSummaryWidgetMessage").hide();

                      $("#spanTotalStudentCount").html("Present Student's Count : "+data.GetAttendanceSummaryResult.AttendanceMarkedStudentCount + "/" + data.GetAttendanceSummaryResult.TotalStudent);
                      $("#spanStuentClassCount").html("Present Classe's Count : " + data.GetAttendanceSummaryResult.AttendanceMarkedClassCount + "/" + data.GetAttendanceSummaryResult.TotalClasses);
                      $('#' + _clientHidGetAttendanceSummaryResultStudents).val(data.GetAttendanceSummaryResult.Students)
                      $('#' + _clientHidGetAttendanceSummaryResultClasses).val(data.GetAttendanceSummaryResult.Classes)
                         }
                  else {
                      $("#divAttendanceSummaryWidgetContent").hide();
                      $("#divAttendanceSummaryWidgetMessage").show();
                      removeLocalStorage(localStorageKey);
                  }

                  $('#lblAttendanceSummaryFilter').text('(Date' + ': ' + kendo.toString($("#datepicker").data("kendoDatePicker").value(), 'dd/MM/yyyy') + ')');
                  showHideKendoProgressbar("divAttendance", false, 1500);

                  // This condition is used for to save widget data in local storage when first time load 
                  // And refresh local storage case and avoid filter save case.
                  if (loadDefaultData == true) {
                      var numberOfDaysAhead = 1; //save data in local storage until EOD
                      saveToLocalStorage(localStorageKey, data, getExpirationHrs(numberOfDaysAhead));
                  }
              }

        var errorback = function (msg) {
            showHideKendoProgressbar("divAttendance", false, 1500);
            removeLocalStorage(localStorageKey);
        };


        // This condition is used to remove widget data from local storage.
        if (loadDefaultData == true && isRefresh == true) {
            $("#datepicker").data("kendoDatePicker").value(_firstLoadAttendanceDate);
        }

        // Create data object to pass parameters to get Attendance Summary.
        var dataObject = {
            isRefresh: isRefresh,
            loadDefaultData: loadDefaultData,
            keyName: localStorageKey,
            serviceUrl: serviceUrl + "GetAttendanceSummary",
            data: inputParameters
        };

        // If first time load and local storage is empty or expire then save in local storage
        // This function is used to get fee Attendance Summary details  
        getWidgetData(dataObject, callback, errorback);

        //This code is used to refresh attendance widget after some time. 
        if (loadDefaultData == true && isRefresh == false) {
            setTimeout(function () {
                if ($("#classAttendanceGauge").data("kendoRadialGauge") != undefined)
                    $("#classAttendanceGauge").data("kendoRadialGauge").redraw();

                if ($("#studentAttendanceGauge").data("kendoRadialGauge") != undefined)
                    $("#studentAttendanceGauge").data("kendoRadialGauge").redraw();
            }, 1000)
          
        }

        

    } catch (e) {
        showHideKendoProgressbar("divAttendance", false, 1500);
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- loadAttendanceWidget', _userId);
    }
}

/// Create gauge widget and show studentwise and classwise attendance.
function createGauge(dataObject) {
    try {
        var classNames;
        var totalClasses;

        // Create guage for attendance marked classes.
        $("#classAttendanceGauge").kendoRadialGauge({
            pointer: {
                value: dataObject.AttendanceMarkedClassCount
            },
            scale: {
                minorUnit: 1,
                startAngle: -30,
                endAngle: 210,
                max: dataObject.TotalClasses,
                height: 315
            },
            height: 315
        });

        // Create guage for attendance marked for student.
        $("#studentAttendanceGauge").kendoRadialGauge({
            pointer: {
                value: dataObject.AttendanceMarkedStudentCount
            },
            scale: {
                minorUnit: 50,
                startAngle: -30,
                endAngle: 210,
                max: dataObject.TotalStudent,
                height: 315
            },
            height: 315
        });

        createMissingAttendanceData(dataObject)
        //To set specific height to gauges so that they match with right side widget's height
        $(".k-gauge svg").attr("height", "202px");
        $("#divPendingAttendance").attr("style", "height: 252px;");
        $("#classAttendanceGauge").data("kendoRadialGauge").redraw();
        $("#studentAttendanceGauge").data("kendoRadialGauge").redraw();

        // create pie chart to display classwise missing attendance details.
        var size = 80; // this number is used to show percentage.
        var oldie = /msie\s*(8|7|6)/.test(navigator.userAgent.toLowerCase());
        $('.easy-pie-chart.percentage').each(function () {
            $(this).easyPieChart({
                barColor: $(this).data('color'),
                trackColor: '#EEEEEE',
                scaleColor: false,
                lineCap: 'butt',
                lineWidth: 8,
                animate: oldie ? false : 1000,
                size: size
            }).css('color', $(this).data('color'));
        });

        //show tooltip for container for new text added to title attribute
        $("div[id^='divSetPercentageContainer_']").tooltip();
    } catch (e) {
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- createGauge', _userId);
    }
}

//Create missing attendance data
function createMissingAttendanceData(dataObject) {
    //Set missing attendance data
    for (var i = 0; i < dataObject.MissingAttendance.length; i++) {
        classNames = dataObject.MissingAttendance[i].ClassNames;
        missingPercentage = dataObject.MissingAttendance[i].MissingPercentage;
        totalClasses = classNames.split(",").length;

        if (missingPercentage == 0 && totalClasses == 1)
            totalClasses = 0;

        //Set total count label and tooltip for the Title of chart
        $("#divSetClassCount_" + (i + 1)).text("Total = " + totalClasses);
        $("#divSetClassCount_" + (i + 1)).attr("data-original-title", classNames);

        //Set % completion for the chart
        $("#divSetPercentageContainer_" + (i + 1)).attr("data-percent", missingPercentage);
        $("#divSetPercentageContainer_" + (i + 1)).attr("data-original-title", classNames);
        //Set percentage number that appears in center of the chart
        $("#divSetPercentage_" + (i + 1)).text(missingPercentage + " %");
    }
}

// On cancel opration set previously selected date.
function setPreviousSelectedDate() {
    _previousSelectedAttendanceDate = kendo.toString($("#datepicker").data("kendoDatePicker").value(), 'dd/MM/yyyy');
}

// On reset opration set default values to control.
function clearAttendanceSummaryFilters() {
    hideAttendanceSummaryFilter();
    $("#datepicker").data("kendoDatePicker").value(_firstLoadAttendanceDate);
    loadAttendanceWidget(false, false);
}
// Hide filter attendance fliter.
function hideAttendanceSummaryFilter() {
    $("#datepicker").data("kendoDatePicker").value(_previousSelectedAttendanceDate);
    $("#divAttendanceSummary").removeClass("open");
}

// reload attendance pie chart on window resize.
function updateAttendancePieChart() {
    $('.easy-pie-chart.percentage').each(function () {
        if ($(this).data('easyPieChart') != undefined) {
            $(this).data('easyPieChart').update(0);
            $(this).data('easyPieChart').update($(this).data('easyPieChart').options.percent);
        }
    });
}

/******************************************* End - Attendance****************************************/

/******************************************** Start -Exam wise Student Performance************************************/
// Get data for student performance widget and show student performance chart
var _firstLoadStandardId;
var _firstLoadExamId;

/*Declare variable which is set previous value*/
var _previousSelectedExam;
var _previousSelectedStandard;

/*This function is used to get performance of student based on the Exam and Standard*/
function loadExamWiseStudentPerformanceWidget(loadDefaultData, isRefresh) {
    try {
        $("#divExamwiseStudentPerformance").removeClass("open");

        // This condition is used to remove widget data from local storage.
        if (loadDefaultData == true && isRefresh == true) {
            $('[id*=cmbStandardName]').val(_firstLoadStandardId);
            $('[id*=cmbStandardWiseExam]').val(_firstLoadExamId);
        }

        var standardName = $('[id*=cmbStandardName]').val() == "0" ? "" : $('[id*=cmbStandardName] option:selected').text();
        var examName = $('[id*=cmbStandardWiseExam]').val() == "0" ? "" : $('[id*=cmbStandardWiseExam] option:selected').text();
        /* This code is used to when standard is not configured*/
        if ($('[id*=cmbStandardName]').val() == "0") {
            showExamNotPublished(standardName, examName);
            return false;
        }

        showHideKendoProgressbar("divExamwiseStudentPerformanceGraph", true, _progressbarStartDelayTime);

        /*When it load first time then set by default selelected value*/
        if (loadDefaultData == true && isRefresh == false) {
            _firstLoadStandardId = $('[id*=cmbStandardName] option:first-child').val();
            _firstLoadExamId = $('[id*=cmbStandardWiseExam] option:first-child').val();
        }

		// Local storage key for exam wise student performance
        var localStorageKey = "ExamWiseStudentPerformance";

        /* Parameter passed to the service call to get result*/
     var inputParameters =
            {
                aiSchoolId: _schoolId,
                aiAcademicYearId: _academicYearId,
                aiStandardId: (loadDefaultData == true && isRefresh == true) ? _firstLoadStandardId : $('[id*=cmbStandardName]').val(),
                aiTestId: (loadDefaultData == true && isRefresh == true) ? _firstLoadExamId : $('[id*=cmbStandardWiseExam]').val()
           }

        /* This block of code is used to ajax call to service and Get result if result return then show chart otherwise show no record found message*/
           var callback = function (data) {
               if (data.GetStandardsPerformanceDataResult != null) {
                   if (data.GetStandardsPerformanceDataResult.GradeDetails.length > 0
                            && data.GetStandardsPerformanceDataResult.Standards.length > 0
                            && data.GetStandardsPerformanceDataResult.MaxStudentCount > 0) {
                       createExamwiseStudentPerformanceChart(data.GetStandardsPerformanceDataResult);
                       $("#divExamwiseStudentPerformanceGraph").css("overflow", "hidden");
                       $("#divExamwiseStudentPerformanceChart").show();
                       $("#divFilterDetails").removeClass('hide');
                       $("#divExamwiseStudentPerformanceMessage").hide();
                       
                   }
                   else {
                       showExamNotPublished(standardName, examName);
                   }
               }
               else {
                   $("#divExamwiseStudentPerformanceGraph").css("height", "");
                   $("#divExamwiseStudentPerformanceChart").hide();
                   $("#divFilterDetails").addClass('hide');
                   $("#divExamwiseStudentPerformanceMessage").text(_errorOcuredMessage);
                   $("#divExamwiseStudentPerformanceMessage").show();
                   removeLocalStorage(localStorageKey);
               }
               $('#lblExamWiseStudentPerformaceFilter').text('(Standard' + ' : ' + standardName + ', Exam' + ' : ' + examName + ')');
               showHideKendoProgressbar("divExamwiseStudentPerformanceGraph", false, _progressbarStopDelayTime);

               // This condition is used for to save widget data in local storage when first time load 
               // And refresh local storage case and avoid filter save case.
               if (loadDefaultData == true) {
                   var numberOfDaysAhead = 7; //save data in local storage until end of 7th day (including today)
                   saveToLocalStorage(localStorageKey, data, getExpirationHrs(numberOfDaysAhead));
               }
           }

        var errorback = function (msg) {
            showHideKendoProgressbar("divExamwiseStudentPerformanceGraph", false, _progressbarStopDelayTime);
            removeLocalStorage(localStorageKey);
        }

        // Create data object to pass parameters to get exam wise student performance details.
        var dataObject = {
            isRefresh: isRefresh,
            loadDefaultData: loadDefaultData,
            keyName: localStorageKey,
            serviceUrl: serviceUrl + "GetStandardsPerformanceData",
            data: inputParameters

        };

        // If first time load and local storage is empty or expire then save in local storage
        // This function is used to get exam wise student performance details. 
        getWidgetData(dataObject, callback, errorback);

        if (loadDefaultData == true && isRefresh == false) {
                setTimeout(function () {
                    if ($("#divExamwiseStudentPerformanceChart").data("kendoChart") != undefined)
                        $("#divExamwiseStudentPerformanceChart").data("kendoChart").refresh();
                }, 1000)
        }

    } catch (e) {
        showHideKendoProgressbar("divExamwiseStudentPerformanceGraph", false, _progressbarStopDelayTime);
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- loadExamWiseStudentPerformanceWidget', _userId);
    }
}

/* This function is used to show no data message*/
function showExamNotPublished(standardName, examName) {
    $("#divExamwiseStudentPerformanceGraph").css("height","");
    $("#divExamwiseStudentPerformanceGraph").css("overflow", "");
    $("#divExamwiseStudentPerformanceChart").hide();
	$("#divFilterDetails").addClass('hide');

    /*If standard is configured then show result with standard name and No record found message*/
    if (standardName != "") {
        $('#lblExamWiseStudentPerformaceFilter').text('(Standard' + ' : ' + standardName + ' , Exam' + ' : ' + examName + ')');
        $('#divExamwiseStudentPerformanceMessage').text('No Record Found (Exam Is Not Published)');
    }
    else {
        $('#lblExamWiseStudentPerformaceFilter').css("display", "none");
        $('#divExamwiseStudentPerformanceMessage').text('Standard / Exam Not Configured');
    }
    
    $('#divExamwiseStudentPerformanceMessage').show();
}

/*This function is used to create donut chart of standard wise exam performance*/
function createExamwiseStudentPerformanceChart(data) {
    var examwiseStudentPerformanceData = getExamwiseStudentPerformanceChartSeries(data.GradeDetails);
    $("#divExamwiseStudentPerformanceChart").kendoChart({
        legend: {
            position: "top"
        },
        seriesDefaults: {
            labels: {
                template: "#= value#",
                position: "center",
                visible: true,
                background: "transparent",
                color: '#FFFFFF'
            }
        },
        series: [{
            type: "donut",
            data: examwiseStudentPerformanceData
        }]
    });

    var divStyle = $("#divExamwiseStudentPerformanceGraph").attr("style");
    
    //remove width & height related style
    divStyle = divStyle.replace("height: 329px !important; width: 100% !important;", "");

    //add width & height as 100%
    divStyle = divStyle + "height: 329px !important; width: 100% !important;"

    $("#divExamwiseStudentPerformanceGraph").attr("style", divStyle);
}



/* This function is used to get Examwise student performance series in array format which we used to show in donut*/
function getExamwiseStudentPerformanceChartSeries(data) {
    var seriesArray = [];
    if (data.length > 0) {
        for (var dataCnt = 0; dataCnt < data.length; dataCnt++) {
            var seriesData = {
                category: data[dataCnt].Grade,
                value: data[dataCnt].StudentCount[0]
            };
            seriesArray.push(seriesData);
        }
    }
    return seriesArray;
}

/*This function is used to set previous value to the variable which is useful on clear and hide function*/
function setPreviousSelectedStandardWiseExam() {
    _previousSelectedStandard = $('[id*=cmbStandardName]').val();
    _previousSelectedExam = $('[id*=cmbStandardWiseExam]').val();
}

/*This function is used clear selected value and Set bydefault first value to the dropdown and return result of based on this value*/
function clearExamwiseStudentPerformnaceWidgetFilter() {
    hideExamwiseStudentPerformanceWidgetFilter();
    $('[id*=cmbStandardName]').val(_firstLoadStandardId);
    $('[id*=cmbStandardWiseExam]').val(_firstLoadExamId);
    loadExamWiseStudentPerformanceWidget(false, false);
}

/*This function is used to hide filter*/
function hideExamwiseStudentPerformanceWidgetFilter(e) {
    $('[id*=cmbStandardName]').val(_previousSelectedStandard);
    $('[id*=cmbStandardWiseExam]').val(_previousSelectedExam);
    $("#divExamwiseStudentPerformance").removeClass("open");
}


/*This function is used to get exam for selected standard*/
function getExamsForSelectedStandard() {
    try {
        showHideKendoProgressbar("ulStandardDivisionId", true, 0);
        /*remove all option from exam dropdown*/
        $('[id*=cmbStandardWiseExam] option').remove(0);

        var data = {
            aiSchoolId: _schoolId,
            aiAcademicYearId: _academicYearId,
            aiStandardId: $('[id*=cmbStandardName]').val()
        }

        /*This block of code is used to call to the service using above parameter and get list of 
        exam available for selected class/standard. Return exam fill into the dropdown*/
        var datatype = "json";

        var callback = function (data) {
            if (data.GetExamsForSelectedStandardResult != null) {
                for (var examCount = 0; examCount < data.GetExamsForSelectedStandardResult.length; examCount++) {
                    $('[id*=cmbStandardWiseExam]').append('<option value="' + data.GetExamsForSelectedStandardResult[examCount].ExamId + '">' + data.GetExamsForSelectedStandardResult[examCount].ExamName + '</option>');
                }
            }
            else {
                //alert("Selected standered's exam are unavailable.");
                //$('[id*=cmbStandardWiseExam]').append('<option value="0"> -- Select -- </option>');
            }
            showHideKendoProgressbar("ulStandardDivisionId", false, 500);
        };

        var errorback = function (msg) {
            showHideKendoProgressbar("ulStandardDivisionId", false, 500);
        }

        rit.base.ajax("Post",
                serviceUrl + "GetExamsForSelectedStandard",
                data,
                callback,
                errorback
		        );
    } catch (e) {
        showHideKendoProgressbar("ulStandardDivisionId", false,500);
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- loadExamsForSelectedStandard', _userId);
    }
}

/******************************************** End -Student Performance************************************/


/******************************************** Start - Accounts************************************/
var _previousSelectedAccountWidgetFinancialYear;
var _selectedAccountWidgetFinancialYear;

// Get data for student performance widget and show student performance chart
function loadAccountWidget(loadDefaultData, isRefresh) {
    try {
        showHideKendoProgressbar("divAccountFlowChartContent", true, _progressbarStartDelayTime);
        $("#divAccounts").removeClass("open");

        if (loadDefaultData == true && isRefresh == false) {
            $("[id*=cmbAccountsFinancialYear]").val($('[id*=ddlFinancialYears] option:selected').val());
            _selectedAccountWidgetFinancialYear = $('[id*=cmbAccountsFinancialYear] option:selected').val()
        }
      	   
        if (isAccountModuleEnabled.toLowerCase() == 'true') {
            var inputParameters = {
                aiSchoolId: _schoolId,
                aiFinancialYearId: (loadDefaultData == true && isRefresh == true) ? _selectedAccountWidgetFinancialYear : $('[id*=cmbAccountsFinancialYear] option:selected').val()
            };

            // Local storage key for account summary
            var localStorageKey = "AccountSummary";
            var callback = function (data) {
                if (data.GetAccountInflowOutflowSummaryResult != null) {
                    createChartForAccount(data.GetAccountInflowOutflowSummaryResult);
                    $("#divAccountFlowChartContent").show();
                    $("#divAccountFlowChartMessage").hide();
                }
                else {
                    $("#divAccountFlowChartContent").hide();
                    $("#divAccountFlowChartMessage").show();
                    removeLocalStorage(localStorageKey);
                }
                showHideKendoProgressbar("divAccountFlowChartContent", false, _progressbarStopDelayTime);

                $('#spanAccountWidgetFilter').text('(Financial Year' + ': ' + $("select[id$=cmbAccountsFinancialYear] option:selected").text() + ')');

                // This condition is used for to save widget data in local storage when first time load 
                // And refresh local storage case and avoid filter save case.
                if (loadDefaultData == true) {
                    var toNextDate = 7; //save data in local storage to upcoming 7th
                    saveToLocalStorage(localStorageKey, data, getHrsToUpcomingDate(toNextDate));
                }
			}				

            var errorback = function (msg) {
                showHideKendoProgressbar("divAccountFlowChartContent", false, _progressbarStopDelayTime);
                removeLocalStorage(localStorageKey);
            }

             // This condition is used to remove widget data from local storage.
            if (loadDefaultData == true && isRefresh == true) {
                $("[id*=cmbAccountsFinancialYear]").val(_selectedAccountWidgetFinancialYear);
            }


            // Create data object to pass parameters to get account summary details.
            var dataObject = {
                isRefresh: isRefresh,
                loadDefaultData: loadDefaultData,
                keyName: localStorageKey,
                serviceUrl: serviceUrl + "GetAccountInflowOutflowSummary",
                data: inputParameters
            };

            // If first time load and local storage is empty or expire then save in local storage
            // This function is used to get account details.
            getWidgetData(dataObject, callback, errorback);
        }
        else {
            var data = {
                MonthwiseInflowAmount: ["472447.32", "278432", "424747.33", "7474233.22", "4264464", "324424", "472447.32", "278432", "424747.33", "7474233.22", "4264464", "324424"],
                MaxSalaryAmount: 5000000,
                MonthwiseOutflowAmount: ["3545553.32", "443555", "643433", "756233", "3435663", "423444", "545553.32", "443555", "643433", "756233", "3435663", "423444"]
            }

            createChartForAccount(data);
            $("#divAccountFlowChartcContent").show();
            showHideKendoProgressbar("divAccountFlowChartContent", false, _progressbarStopDelayTime);

            //If account module is disabled then add overlay div for sample data. And hide settings and refresh icons.
            $("#divAccounts").hide();
            addOverlayDivForSampleData("#divAccountFlowChartContent", '70%');
	}
		
        setTimeout(function () {
            /* This code is used to refresh chart first time load and control is available*/
            if (loadDefaultData && isRefresh == false && $("#accountFlowChart").data("kendoChart") != undefined)
                $("#accountFlowChart").data("kendoChart").refresh();
        }, 1000);
    }
    catch (e) {
        showHideKendoProgressbar("divAccountFlowChartContent", false, _progressbarStopDelayTime);
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- loadAccountWidget', _userId);
    }
}

// Create chart for account iflow and outflow details
function createChartForAccount(data) {
    $("#accountFlowChart").kendoChart({
        legend: {
            visible: true
        },
        seriesDefaults: {
            type: "column",
            stack: false
        },
        chartArea: {
            height: 300
        },
        series: [{
            name: "Inflow",
            data: data.MonthwiseInflowAmount,
            color: "#49C1F7"
        },
        {
            name: "Outflow",
            data: data.MonthwiseOutflowAmount,
            color: "#6BBD6E"
        }],
        valueAxis: {
            max: data.MaxSalaryAmount == 0 ? 10000 : data.MaxSalaryAmount,
            line: {
                visible: false
            },
            minorGridLines: {
                visible: false
            }
        },
        categoryAxis: {
            categories: ["Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "Jan", "Feb", "Mar"],
            majorGridLines: {
                visible: false
            }
        },
        tooltip: {
            visible: true,
            template: "#= series.name #" + ' : Rs.' + '#=showCommaSeparatedAmount(value)#'
        }
    });
}

function setAccountWidgetSelectedYear() {
    _previousSelectedAccountWidgetFinancialYear = $('[id*=cmbAccountsFinancialYear]').val()
}


function clearAccountsWidgetFilters() {
    hideAccountsWidgetFilter();
    $("[id*=cmbAccountsFinancialYear]").val(_selectedAccountWidgetFinancialYear);
    loadAccountWidget(false, false);
}

function hideAccountsWidgetFilter(e) {
    $("[id*=cmbAccountsFinancialYear]").val(_previousSelectedAccountWidgetFinancialYear)
    $("#divAccounts").removeClass("open");
}

function showCommaSeparatedAmount(amount) {
        return amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}
/******************************************** End - Accounts ***************************************/


/******************************************** Start - Payroll***************************************/
var _firstLoadPayrollMonth;
var _previousSelectedPayrollWidgetAcademicYear;
var _previousSelectedPayrollWidgetFinancialYear;
var _selectedPayrollWidgetFinancialYear;
var _firstLoadSelectedPayrollYear;


function loadPayrollWidget(loadDefaultData, isRefresh) {
    //Set dropdown value to current month
    var currentMonth;
    var prevMonth;

    if (loadDefaultData == true && isRefresh == false) {
        $("[id*=cmbPayrollFinancialYear]").val($('[id*=ddlFinancialYears] option:selected').val());
        //Set widget's setting's current month
        currentMonth = new Date().getMonth() + 1;
        $("#cmbPayrollMonth").val(currentMonth);
        _firstLoadPayrollMonth = currentMonth;
        _selectedPayrollWidgetFinancialYear = $('[id*=cmbPayrollFinancialYear] option:selected').val();
        _firstLoadSelectedPayrollYear = $("select[id$='cmbPayrollYear'] option:selected").val()
    }
    else {
        currentMonth = $("#cmbPayrollMonth").val();
    }

    if (loadDefaultData == true && isRefresh == true) {
        currentMonth = _firstLoadPayrollMonth;
        $('select[id$="cmbPayrollYear"] option:last-child').attr("selected", "selected");
    }

    try {
        if (isPayrollModuleEnabled.toLowerCase() == 'true') {
            var inputParameters = {
                aiSchoolId: _schoolId,
                aiYear: (loadDefaultData == true && isRefresh == true) ? _firstLoadSelectedPayrollYear : $("select[id$='cmbPayrollYear'] option:selected").val(),
                aiFinancialYearId: (loadDefaultData == true && isRefresh == true) ? _selectedPayrollWidgetFinancialYear : $('[id*=cmbPayrollFinancialYear]').val(),
                aiMonth: (loadDefaultData == true && isRefresh == true) ? _firstLoadPayrollMonth : $("#cmbPayrollMonth option:selected").val()
            };

			// Local storage key for payroll summary
            var localStorageKey = "PayrollSummary";
                        
            $("#divPayrollWidgetToolbar").removeClass("open");
            //$("#spanPayrollHeader").text("Payroll");
            var callback = function (data) {
                if (data.GetPayrollSummaryResult != null) {
					$("#divPayrollChartMessage").addClass("col-lg-8 col-md-8 col-sm-8 col-xs-8");
                    if (data.GetPayrollSummaryResult.MaxPaidSalaryAmount > 0) {
                        $("#payrollChart").attr("style", "position: relative; visibility: hidden !important;");
                        createPayrollChart(data.GetPayrollSummaryResult);
                        var currentMonth = $("#cmbPayrollMonth").val();
                        var prevMonthofSelect = $("#cmbPayrollMonth").val() - 1;
                        var salPaidForYear = $("select[id$='cmbPayrollYear'] option:selected").text();

                        if (prevMonth > currentMonth) {
                            salPaidForYear = salPaidForYear - 1;
                        }

                        $('#lblSalaryPaidforMonthYear').text(salPaidForYear.toString().substr(0, 4));
                       
                        $("#divPreviousMonthPaidSalary").text('Rs. ' + data.GetPayrollSummaryResult.PreviousMonthPaidSalary);
                        $("#divIncomeTaxAmount").text('Rs. ' + data.GetPayrollSummaryResult.IncomeTaxAmount);

                        $("#divPayrollLeft").show();
                        $("#divPayrollRight").show();
                        $('#divPayrollChartMessage').hide();
						$('#divPayrollChartMessage').css("padding-left", "");
                    }
                    else {
                        $('#divPayrollChartMessage').removeClass("hide");
                        $('#divPayrollChartMessage').text('No Record Found');
                        $('#divPayrollChartMessage').show();
                        $("#divPayrollChart").hide();
                        $("#divPreviousMonthPaidSalary").text('Rs. ' + '0');
                        $("#divIncomeTaxAmount").text('Rs. ' + data.GetPayrollSummaryResult.IncomeTaxAmount);
                        $("#divPayrollLeft").hide();
                        $("#divPayrollRight").show();
                        $('#divPayrollChartMessage').css("padding-left", "");
                    }

                    $('#spanPayrollWidgetFilter').text('(Month' + ': ' + $("#cmbPayrollMonth option:selected").text() + ')');
                    $("#lblIncomeTaxYear").text($("select[id$=cmbPayrollFinancialYear] option:selected").text());

                    setTimeout(function () {
                        if ($("#payrollChart").data("kendoChart") != undefined)
                            $("#payrollChart").data("kendoChart").refresh();
                        $("#payrollChart").attr("style", "position: relative; visibility: inherit !important;");
                    }, 1000);
                }
                else {
                    //$('#divPayrollChartMessage').text(_errorOcuredMessage);
                    $('#divPayrollChartMessage').css("padding-left", "400px !important");
                    $("#divPayrollChartMessage").addClass("col-lg-12 col-md-12 col-sm-12 col-xs-12");
                    $('#divPayrollChartMessage').removeClass("hide");

                    $('#divPayrollChartMessage').show();
                    $("#divPayrollLeft").hide();
                    $("#divPayrollRight").hide();
                    removeLocalStorage(localStorageKey);
                }

                // This condtion is used for to save widget data in local storage when first time load 
                // And refresh local storage case and avoid filter save case.
                if (loadDefaultData == true) {
                    var toNextDate = 7; //save data in local storage to upcoming 7th

                    saveToLocalStorage(localStorageKey, data, getHrsToUpcomingDate(toNextDate));
                }
            }

            var errorback = function (data) {
                showHideKendoProgressbar("divPayrollWidget", false, _progressbarStopDelayTime);
                removeLocalStorage(localStorageKey);
            };
			
            // This condition is used to remove widget data from local storage.
            if (loadDefaultData == true && isRefresh == true) {
                $("[id*=cmbPayrollFinancialYear]").val(_selectedPayrollWidgetFinancialYear);
                $("#cmbPayrollMonth").val(_firstLoadPayrollMonth);
			}

            // Create data object to pass parameters to get payroll summary details.
            var dataObject = {
                isRefresh: isRefresh,
                loadDefaultData: loadDefaultData,
                keyName: localStorageKey,
                serviceUrl: serviceUrl + "GetPayrollSummary",
                data: inputParameters
            };

            // If first time load and local storage is empty or expire then save in local storage
            // This function is used to get payroll details 
            getWidgetData(dataObject, callback, errorback);
        }
        else {
            var data = {
                PreviousMonthPaidSalary: 170400,
                IncomeTaxAmount: 173000,
                MonthWiseSalaryAmount: [1702000, 1708000, 170400]
            };

            createPayrollChart(data);
            $("#lblIncomeTaxYear").text($("select[id$=cmbPayrollFinancialYear] option:selected").text());
            $("#divPreviousMonthPaidSalary").text('Rs. ' + data.PreviousMonthPaidSalary);
            $("#divIncomeTaxAmount").text('Rs. ' + data.IncomeTaxAmount);
            $("#divPayrollChart").show();
            $('#divPayrollChartMessage').hide();
            $("#divPayrollWidgetToolbar").removeClass("open");
            //$("#payrollNoticeContent").text("(Sample data is displayed here as module not enabled)");

            //If payroll module is disabled then add overlay div for sample data. And hide settings and refresh icons.
            $("#divPayrollWidgetToolbar").hide();
            addOverlayDivForSampleData("#divPayrollContent", '76%');
        }


        //Set salary widget's footer text to appropriate
        var prevMonth = currentMonth - 1;

        if (prevMonth == 0)
            prevMonth = 12

        $("#lblSalaryWidgetText").text("Salary Paid for Month " + getMonthName(prevMonth));

        if (loadDefaultData == true && isRefresh == false) {
            setTimeout(function () {
                /* this code is used to refresh chart first time load and control is available*/
                if (loadDefaultData && isRefresh == false && $("#payrollChart").data("kendoChart") != undefined)
                    $("#payrollChart").data("kendoChart").refresh();
            }, 1000);
        }
    }
    catch (e) {
        showHideKendoProgressbar("divPayrollWidget", false, _progressbarStopDelayTime);
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- loadPayrollWidget', _userId);
    }
}
function createPayrollChart(data) {
    ///Begin - create dynamic labels for month axis
    var currentMonth = $("#cmbPayrollMonth").val();
    var prevMonth = $("#cmbPayrollMonth").val() - 1;
    var counter = 2;
    var categoriesArray = new Array();
    var currentAcademicYear = $("select[id$='cmbPayrollYear'] option:selected").text();
    var isYearChanged = false;
    while (counter >= 0) {
        if (prevMonth == 0)
            prevMonth = 12

        if (prevMonth > currentMonth && isYearChanged == false) {
            isYearChanged = true;
            currentAcademicYear = currentAcademicYear - 1;
        }
       
       categoriesArray[counter] = getMonthName(prevMonth) + " " + currentAcademicYear.toString().substr(2, 2);

        prevMonth = prevMonth - 1
        counter = counter - 1;
    }

    //End - create dynamic labels for month axis
    $("#payrollChart").kendoChart({
        chartArea: {
            height: 262,
            opacity: 0.1
        },
        title: {
            text: "Month wise salary"
        },
        legend: {
            visible: false
        },
        seriesDefaults: {
            type: "bar"
        },
        series: [{
            name: "Amount in Rs.",
            data:data.MonthWiseSalaryAmount,
            color: "#3F51B5"
        }],
        categoryAxis: {
            categories: categoriesArray,
            majorGridLines: {
                visible: false
            }
        },
        tooltip: {
            visible: true,
            template: "#= series.name #" + ' : Rs.' + '#=showCommaSeparatedAmount(value)#'
        }
    });
}

// Preserve selected month  and selected year value to reset on cancel opration.
function setPayrollWidgetSelectedMonthAndYear() {
     g_previousSelectedPayrollWidgetMonth = $("#cmbPayrollMonth").val();
     _previousSelectedPayrollWidgetAcademicYear = $("select[id$='cmbPayrollYear']").val();
     _previousSelectedPayrollWidgetFinancialYear = $('[id*=cmbPayrollFinancialYear]').val()
}

// Reset payroll widget filter.
function clearPayrollWidgetFilter() {
    hidPayrollWidgetFilter();
    $("[id*=cmbPayrollFinancialYear]").val(_selectedPayrollWidgetFinancialYear);
    $("#cmbPayrollMonth").val(_firstLoadPayrollMonth);
    loadPayrollWidget(false, false);
}

// Hide payroll widget filter.
function hidPayrollWidgetFilter(e) {
    $('select[id$="cmbPayrollYear"] option:last-child').attr("selected", "selected");
    $('[id*=cmbPayrollFinancialYear]').val(_previousSelectedPayrollWidgetFinancialYear)
    $("#cmbPayrollMonth").val(g_previousSelectedPayrollWidgetMonth);
    $("#divPayrollWidgetToolbar").removeClass("open");
}
/******************************************** End - Payroll ***************************************/

/******************************************** Start - Birthday ***************************************/
/// Get data for Birthday widget and show birthday count on widget
function loadBirthdayWidget(loadDefaultData, isRefresh) {
    try {
        showHideKendoProgressbar("divBirthday", true, _progressbarStartDelayTime);
        $("#divBdayWidgerToolbar").removeClass("open");

        var selectedUserRole = $("#liUser label.active input").length > 0 ? $("#liUser label.active input").val() : '0';
        var selectedView = $("#liBdayView label.active input").length > 0 ? $("#liBdayView label.active input").val() : 'T';

        var localStorageKey = "BirthdayList";

        var defaultUserRoleId = '0';
		// Ulternate key for student and other user 
        if (_userRoleId == 3) {
            selectedUserRole = _userRoleId;
            localStorageKey = localStorageKey + '_' + _userRoleId;
            defaultUserRoleId = '3';
        }

		// Local storage key for birthday list
       var inputParameters = {
            aiSchoolId: _schoolId,
            aiAcademicYearId: _academicYearId,
            aiUserRoleId: (loadDefaultData == true && isRefresh == true) ? defaultUserRoleId : selectedUserRole,
            asView: (loadDefaultData == true && isRefresh == true) ? 'T' : selectedView
         };

        var callback = function (data) {
            if (data.GetUpcomingStaffBdayListResult != null) {
                $("#divBirthday").show();
                $("#divBirthdayWidgetMessage").hide();
                createBirthdayTemplate(data.GetUpcomingStaffBdayListResult);
                $("#spanBirthdayCount").text(data.GetUpcomingStaffBdayListResult.length);
            }
            else {
                $("#spanBirthdayCount").text(0);
                $("#divBirthday").hide();
                $("#divBirthdayWidgetMessage").text(_errorOcuredMessage);
                $("#divBirthdayWidgetMessage").show();
                removeLocalStorage(localStorageKey);
            }

            $("#spanBirthdayCount").removeClass("hide");
            showHideKendoProgressbar("divBirthday", false, _progressbarStopDelayTime);
           
		    // This condition is used for to save widget data in local storage when first time load 
            // And refresh local storage case and avoid filter save case.
            if (loadDefaultData == true) {
				var numberOfDaysAhead = 1; //save data in local storage until EOD
                saveToLocalStorage(localStorageKey, data, getExpirationHrs(numberOfDaysAhead)); 
			}
        }

        var errorback = function (msg) {
            showHideKendoProgressbar("divBirthday", false, _progressbarStopDelayTime);
            removeLocalStorage(localStorageKey);
        }

 
        // This condition is used to remove widget data from local storage.
        if (loadDefaultData == true && isRefresh == true) {

            $("#divBdayWidgerToolbar").removeClass("open");
            $("#liUser label").removeClass('active');
            $("#liBdayView label").removeClass('active');
            $("#liBdayView label:first").addClass('active');
        }

       // Create data object to pass parameters to get summary details.
        var dataObject = {
            isRefresh: isRefresh,
            loadDefaultData: loadDefaultData,
            keyName: localStorageKey,
            serviceUrl: serviceUrl + "GetUpcomingStaffBdayList",
            data: inputParameters
        };

        // If first time load and local storage is empty or expire then save in local storage
        // This function is used to get birthday list 
        getWidgetData(dataObject, callback, errorback)
    } catch (e) {
        showHideKendoProgressbar("divBirthday", false, _progressbarStopDelayTime);
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- loadBirthdayWidget', _userId);
    }
}

// Create brithday widget html.
function createBirthdayTemplate(data, loadDefaultData, isRefresh) {
    try {
        $(".bxslider").hide();
        $(".bxslider").html('');
        var widgetTemplate = " <li>" +
                                "<div class=\"col-md-5 padding-top-20\">" +
                                    "<img src=\"data:image/jpg;base64,@%@PhotoPath@%@\" class=\"img-responsive\" height=\"80px\" width=\"80px\">" +
                                "</div>" +
                                "<div class=\"col-md-7 padding-top-20 bday-student-details\">" +
                                    "<div>" +
                                       "@%@UserName@%@</div>" +
                                    "<div>" +
                                    "@%@Date@%@</div>" +
                                "</div>" +
                            "</li>";
        var bdayHtml = "";
        if (data.length > 0) {
            for (var dataCnt = 0; dataCnt < data.length; dataCnt++) {
                var html = widgetTemplate;
                html = html.replace("@%@UserName@%@", data[dataCnt].UserName);
                html = html.replace("@%@Date@%@", data[dataCnt].Date);
                if (data[dataCnt].PhotoPath != '') {
                    html = html.replace("@%@PhotoPath@%@", data[dataCnt].PhotoPath);
                }
                else {
                    html = html.replace("data:image/jpg;base64,@%@PhotoPath@%@", "../images/empty-profile.jpg");
                }
                bdayHtml += html;
            }

            $(".bxslider").append(bdayHtml);
            var showPager = data.length > 0;

            setTimeout(function () {
                //    // Slider for rotating bday list
                reloadBirthdaySlider(showPager, false);
            }, 2000);
            $("#divBirthday").show();
            $("#divBirthdayWidgetMessage").hide();
        }
        else {
            //bdayHtml = "";        
            //bdayHtml = "<div id='divBirthdayRecordNotFound' style'padding-top:75px!important;padding-bottom:75px!important; text-align:center!important; font-size:15px!important;' >No Record Found</div>";
            $("#divBirthday").hide();
            $("#divBirthdayWidgetMessage").show();
            $("#divBirthdayWidgetMessage").text("No Record Found");

        }
    } catch (e) {
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- createBirthdayTemplate', _userId);
    }
}


// Reload Birthday slider after applying filter.
function reloadBirthdaySlider(showPager, isResizeWindow) {
    var birtdaySliderOptions = {
        auto: true,
        autoControls: false,
        pager: false,
        autoHover: true,
        autoControls: false,
        responsive: true,
        pause: 1500,
        speed: 500,
        onSlideAfter: function () {
            _slider.stopAuto();
            _slider.startAuto();
        },
        startSlide: 1
    };

    if (showPager) {
        $("#divBirthday").add('div-birthday-box');
        $("#divBirthday").parent().css({ "height": "187px" });
        if (typeof (_slider) != "undefined") {

            if (isResizeWindow == true) {
                showHideKendoProgressbar("divBirthday", true, _progressbarStartDelayTime);
            }

            var prevSlideIndexBday = _slider.getCurrentSlide();
            _slider.destroySlider({});
            birtdaySliderOptions.startSlide = prevSlideIndexBday;
            _slider.reloadSlider(birtdaySliderOptions);
            $(".bxslider").show();

            if (isResizeWindow == true) {
                showHideKendoProgressbar("divBirthday", false, _progressbarStopDelayTime);
            }
        }
        else {
            // Slider for rotating bday list
            _slider = $('.bxslider').bxSlider(birtdaySliderOptions);
        }
    }
    else {
        if (typeof (_slider) != "undefined") {
            _slider.destroySlider({});
        }
        // Set height to container for no record found message. 
        $("#divBirthday").parent().attr('style', 'height:45px; text-align:center');
        $("#divBirthday").removeClass('div-birthday-box');
        $("#birthdays").css("margin", "0");
        $("#divBirthdayRecordNotFound").css("margin", "7px");
        $("#divBirthday").parent().attr('style', 'text-align: center; padding: 4px 7px 4px;')
        $("#divBirthday").attr('style', 'min-height: 0 !important;');
    }

    $(".bxslider").show();
}


var _previousSelectedBirthdayWidgetUserRole;
var _previousSelectedBirthdayWidgetUserView;

// Preserve previous selected value.
function setBirthdayWidgetSelectedUserRoleAndView() {
    _previousSelectedBirthdayWidgetUserRole = $("#liUser label.active input").length > 0 ? $("#liUser label.active input").val() : '0';
    _previousSelectedBirthdayWidgetUserView = $("#liBdayView label.active input").length > 0 ? $("#liBdayView label.active input").val() : 'T';
}

// Reset birthday widget filter.
function clearBdayListFilters() {
    $("#divBdayWidgerToolbar").removeClass("open");
    $("#liUser label").removeClass('active');
    $("#liBdayView label").removeClass('active');
    $("#liBdayView label:first").addClass('active');
    loadBirthdayWidget(true, false);
}

// Hide birthday widget filter.
function hideBdayWidgetFilter(e) {
    $("#liUser label").removeClass("active")
    $("#liUser label input[value='" + _previousSelectedBirthdayWidgetUserRole + "']").parent().addClass("active")

    $("#liBdayView label").removeClass("active")
    $("#liBdayView label input[value='" + _previousSelectedBirthdayWidgetUserView + "']").parent().addClass("active")

    $("#divBdayWidgerToolbar").removeClass("open");
}

/******************************************** End - Birthday ***************************************/

/**************************************** Start - Common functions ***********************************************/

// Show filter pop up on click of settings icon of widget.
function showFilter(e, callback) {
    if ($(e).parent().hasClass("open"))
        $(e).parent().removeClass("open");
    else
        $(e).parent().addClass("open");
    callback;
}

function setDefaultDropdownValues() {
//    $("#cmbPhotoGalleryMonth").val((new Date().getMonth() + 1).toString());
//    $("select[id$='cmbPhotoGalleryYear']").val(new Date().getFullYear().toString());

    if ($get(_clienthidShowAllGalleries).value == "Y")
        $("#cmbPhotoGalleryMonth").val(100);
    else
        $("#cmbPhotoGalleryMonth").val((new Date().getMonth() + 1).toString());

    $("select[id$='cmbPhotoGalleryYear']").val(new Date().getFullYear().toString());
}

/**************************************** End - Common functions ***********************************************/


/**************************************** Start - Photo Gallery ****************************************/

var _previousSelectedPhotoGalleryWidgetYear;
var _previousSelectedPhotoGalleryWidgetMonth;

function loadPhotoGalleryWidget(loadDefaultData, isRefresh) {
    try {
    
        showHideKendoProgressbar("divPhotoGalleryWidgetContainer", true, _progressbarStartDelayTime);
        $("#divPhotoGalleryWidget").removeClass("open");

        // Local storage key for photo gallery list
        var localStorageKey = "PhotoGalleryList";

        // This condition is used to remove widget data from local storage.
        if ((loadDefaultData == true && isRefresh == true) || (loadDefaultData == true && isRefresh == false)) {
            setDefaultDropdownValues();
        }

        var mntId = "100";
        if ($get(_clienthidShowAllGalleries).value == "N")
            mntId = (new Date().getMonth() + 1).toString()

        

        var inputParameters = {
            aiSchoolId: _schoolId,
            aiMonth: (loadDefaultData == true && isRefresh == true) ? mntId.toString() : $("#cmbPhotoGalleryMonth option:selected").val(),
            aiYear: (loadDefaultData == true && isRefresh == true) ? (new Date().getFullYear().toString()) : $("select[id$='cmbPhotoGalleryYear'] option:selected").val(),
            abSetPreviousMonth: loadDefaultData,
            aiUserId: _userId
        };

        var callback = function (data) {
            if (data.GetAlbumsListResult != null) {
                $("#divPhotoGallery").show();
                $("#divPhotoGalleryWidgetMessage").hide();
                if (data.GetAlbumsListResult.length == 1 && (data.GetAlbumsListResult[0].ImageList == null || data.GetAlbumsListResult[0].ImageList.length == 0)) {
                    $("#spanAlbumCount").text("0");
                    $("#divPhotoGallery").hide();
                    $("#divPhotoGalleryWidgetMessage").text("No Record Found");
                    $("#divPhotoGalleryWidgetMessage").show();
                }
                else {
                    createPhotoGallery(data.GetAlbumsListResult);

                    $("#cmbPhotoGalleryMonth").val(data.GetAlbumsListResult[0].Month);
                    $("select[id$='cmbPhotoGalleryYear']").val(data.GetAlbumsListResult[0].Year);
                }
            } else {
                $("#divPhotoGallery").hide();
                $("#divPhotoGalleryWidgetMessage").text(_errorOcuredMessage);
                $("#divPhotoGalleryWidgetMessage").show();
                removeLocalStorage(localStorageKey);
            }
            showHideKendoProgressbar("divPhotoGalleryWidgetContainer", false, _progressbarStopDelayTime);
           
            // This condition is used for to save widget data in local storage when first time load 
            // And refresh local storage case and avoid filter save case.
            if (loadDefaultData == true) {
                var numberOfDaysAhead = 7; //save data in local storage until end of 7th day (including today)
                saveToLocalStorage(localStorageKey, data, getExpirationHrs(numberOfDaysAhead));
            }
        }

        var errorback = function (msg) {
            showHideKendoProgressbar("divPhotoGalleryWidgetContainer", false, _progressbarStopDelayTime);
            removeLocalStorage(localStorageKey);
        }
       // Create data object to pass parameters to get summary details.
         var dataObject = {
            isRefresh: isRefresh,
            loadDefaultData: loadDefaultData,
            keyName: localStorageKey,
            serviceUrl: serviceUrl + "GetAlbumsList",
            data: inputParameters
        };

       

        // If first time load and local storage is empty or expire then save in local storage
        // This function is used to get photo gallery details 
        getWidgetData(dataObject, callback, errorback)
    } catch (e) {
        showHideKendoProgressbar("divPhotoGalleryWidgetContainer", false, _progressbarStopDelayTime);
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- loadPhotoGalleryWidget', _userId);
    }
}

// Create html for photo gallery.
function createPhotoGallery(data) {
    try {
        var galleryHtml = "";
        var dataLength = data.length;
        $("#photoGallery").html('');
        if (dataLength > 0) {
            for (var albumCnt = 0; albumCnt < dataLength; albumCnt++) {
                galleryHtml += "<li><a class=\"group" + albumCnt + "\" href=\"../" + data[albumCnt].ImageList[0].ImagePath + "\" title=\"" + data[albumCnt].ImageList[0].Description + "\">" +
                        "<img  src=\"../" + data[albumCnt].ImageList[0].ImagePath + "\" class=\"img-responsive margin-auto\" title=\"Album - " + data[albumCnt].ImageList[0].Description + " (click image to view photos)\"/>" +
                    "</a>" +
                    "@%@ImageHTML@%@" +
                  "</li>";
                var imageHtml = "";

                for (var imageCnt = 1; imageCnt < data[albumCnt].ImageList.length; imageCnt++) {
                    imageHtml += "<a class=\"group" + albumCnt + " hidden\" href=\"../" + data[albumCnt].ImageList[imageCnt].ImagePath + "\" title=\"" + data[albumCnt].ImageList[imageCnt].Description + "\">Album 1</a>";
                }

                galleryHtml = galleryHtml.replace("@%@ImageHTML@%@", imageHtml);
            }

            $("#photoGallery").append(galleryHtml);
            $("#spanAlbumCount").text(dataLength);

            for (var albumCnt = 0; albumCnt < dataLength; albumCnt++) {
                $(".group" + albumCnt + "").colorbox({ rel: "group" + albumCnt + "", transition: "none", width: "75%", height: "75%", slideshow: "true", closeButton: "true", current: '{current} of {total}' });
            }
            var showPager = dataLength > 0;
            $("#photoGallery").hide();
            setTimeout(function () {
                // Slider for rotating bday list.
                reloadphotoGallerySlider(showPager);
            }, 1000);
        }
        else {
            $("#spanAlbumCount").text("0");
            $("#divPhotoGallery").hide();
            $("#divPhotoGalleryWidgetMessage").text("No Record Found");
            $("#divPhotoGalleryWidgetMessage").show();
        }
    }
    catch (e) {
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- createPhotoGallery', _userId);
    }
}

// Reload photo gallery
function reloadphotoGallerySlider(showPager) {
    // show image container
    $("#photoGallery").show();

    if (showPager) {
        var photoGallerySliderOptions = {
            auto: true,
            mode: 'fade',
            captions: true,
            easing: 'ease-in-out',
            autoControls: false,
            pager: showPager,
            autoHover: true,
            responsive: true,
            pause: 1500,
            speed: 1000,
            onSlideAfter: function () {
                _photoGallerySlider.stopAuto();
                _photoGallerySlider.startAuto();
            },
            startSlide: 1
        };
        if (typeof (_photoGallerySlider) != "undefined") {
            var prevSlideIndexGallery = _photoGallerySlider.getCurrentSlide();
            _photoGallerySlider.destroySlider({});
            photoGallerySliderOptions.startSlide = prevSlideIndexGallery;
            _photoGallerySlider.reloadSlider(photoGallerySliderOptions);
        }
        else {
            _photoGallerySlider = $('#photoGallery').bxSlider(photoGallerySliderOptions);
        }
    }
    else {
        $("#divPhotoGalleryWidgetContainer").css("min-height", "0");
        $("#photoGallery").css("margin", "7px");
        if (typeof (_photoGallerySlider) != "undefined") {
            _photoGallerySlider.destroySlider({});
        }
    }
}


// Preserve previous selected year and month
function setPhotoGallerySelectedYearAndMonth() {
    _previousSelectedPhotoGalleryWidgetYear = $("select[id$='cmbPhotoGalleryYear']").val();
    _previousSelectedPhotoGalleryWidgetMonth = $("#cmbPhotoGalleryMonth").val()
}

// Reset photo gallery filters.
function clearPhotoGalleryFilter() {
    hidePhotoGalleryFilter();
    setDefaultDropdownValues();
    loadPhotoGalleryWidget(false,false);
}

// hide photo gallery options.
function hidePhotoGalleryFilter() {
    $("select[id$='cmbPhotoGalleryYear']").val(_previousSelectedPhotoGalleryWidgetYear);
    $("#cmbPhotoGalleryMonth").val(_previousSelectedPhotoGalleryWidgetMonth);
    $("#divPhotoGalleryWidget").removeClass("open");
}

/**************************************** End - Photo Gallery ****************************************/

/******************************************** Start - Statistics Widget (student, staff, library) ***************************************/
function getStatisticsCurrentTabDetailsCount(loadDefaultData, isRefresh) {
    try {
        var selectedStatView = $("#liStatFilter  label.active input").length > 0 ? $("#liStatFilter label.active input").val() : '1';
         selectedStatView = (loadDefaultData == true && isRefresh == true) ? '1' : selectedStatView;

        showHideStatisticTab(selectedStatView);

        if (selectedStatView == "1")
            loadStudentDetailsCount(loadDefaultData, isRefresh)
        else if (selectedStatView == "2")
            loadStaffDetailsCount();
        else
            loadLibraryDetailsCount();
    } catch (e) {
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- getStatisticsCurrentTabDetailsCount', _userId);
    }
}

// Show hide statistic tab according to selected role.
function showHideStatisticTab(selectedStatView) {
    $("#divStatisticTabs div.tab-pane").removeClass('active')
    $(".btn-group label").removeClass('active');
    if (selectedStatView == "1") {
        $("#lblStatisticFilter").text('(Student)');
        $($("#liStatFilter label")[0]).addClass('active');
        $("div [id='student']").addClass('active');
        $("#divAcademicYearLabel").parent().show();
        $("#hlnkRefreshStats").css("display", "");
    }
    else if (selectedStatView == "2") {
        $("#lblStatisticFilter").text('(Staff)');
        $($("#liStatFilter label")[1]).addClass('active');
        $("div [id='staff']").addClass('active');
        $("#divAcademicYearLabel").parent().show();
        $("#hlnkRefreshStats").css("display", "none");
    }
    else {
        $("#lblStatisticFilter").text('(Library)');
         $($("#liStatFilter label")[2]).addClass('active');
        $("div [id='library']").addClass('active');
        $("#divAcademicYearLabel").parent().hide();
        $("#hlnkRefreshStats").css("display", "none");
    }
}

// Show academic year filter.
function showHideAcademicYearFilter(e) {
    var selectedStatView = $(e).val();
    if (selectedStatView == "1" || selectedStatView == "2")
        $("#divAcademicYearLabel").parent().show();
    else
        $("#divAcademicYearLabel").parent().hide();

}
/******************************************** Start - Student count ***************************************/
// Get data for Statistic widget and show student count on widget
function loadStudentDetailsCount(loadDefaultData, isRefresh) {
    try {
        showHideKendoProgressbar("statistics-widget", true, _progressbarStartDelayTime);
        $("#divStatWidgetToolbar").removeClass("open");

            var selectedAcdemicYear = $("select[id$='cmbAcademicYear'] option:selected").val();
            var inputParameters = {
                aiSchoolId: _schoolId,
                aiAcademicYearId: (loadDefaultData == true && isRefresh == true) ? _academicYearId : selectedAcdemicYear
            };
            
            // Local storage key for student statistic
            var localStorageKey = "StudentStatistic";

            var callback = function (data) {
                if (data.GetStudentCountDetailsResult != null) {
                    $("#spanGirlsCount").text(data.GetStudentCountDetailsResult.GirlsCount);
                    $("#spanBoysCount").text(data.GetStudentCountDetailsResult.BoysCount);
                    $("#spanTotalCount").text(data.GetStudentCountDetailsResult.TotalCount);
                    $("#spanLeftCount").text(data.GetStudentCountDetailsResult.LeftCount);
                    $("#spanNewJoinCount").text(data.GetStudentCountDetailsResult.NewJoinCount);
                    $("#spanRteCount").text(data.GetStudentCountDetailsResult.RteCount);
                    animateNumbers("#student");
                    $("#divStudentView").show();
                    $("#divStudentMessage").hide();
                }
                else {
                    $("#divStudentView").hide();
                    $("#divStudentMessage").show();
                    removeLocalStorage(localStorageKey);
                }
                showHideKendoProgressbar("statistics-widget", false, _progressbarStopDelayTime);

                // This condition is used for to save widget data in local storage when first time load 
                // And refresh local storage case and avoid filter save case.
                if (loadDefaultData == true) {
                    var numberOfDaysAhead = 7; //save data in local storage until end of 7th day (including today)

                    saveToLocalStorage(localStorageKey, data, getExpirationHrs(numberOfDaysAhead));
                }
            }

        var errorback = function (msg) {
            showHideKendoProgressbar("statistics-widget", false, _progressbarStopDelayTime);
            removeLocalStorage(localStorageKey);
        }
 
            // This condition is used to remove widget data from local storage.
            if (loadDefaultData == true && isRefresh == true) {

                $("select[id$='cmbAcademicYear']").val(_academicYearId);
            }
            // Create data object to pass parameters to get summary details.
            var dataObject = {
                isRefresh: isRefresh,
                loadDefaultData: loadDefaultData,
                keyName: localStorageKey,
                serviceUrl: serviceUrl + "GetStudentCountDetails",
                data: inputParameters

            };

            // If first time load and local storage is empty or expire then save in local storage
            // This function is used to get student count details 
            getWidgetData(dataObject, callback, errorback);

            $("#divStatWidgetToolbar i[class = 'icon- fa fa-cog']").show();
    } catch (e) {
        showHideKendoProgressbar("statistics-widget", false,_progressbarStopDelayTime);
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- loadStudentDetailsCount', _userId);
    }
}
/******************************************** End - Student count ***************************************/


/******************************************** Start - Staff count ***************************************/
/// Get data for Statistic widget and show staff count on widget
function loadStaffDetailsCount() {
    try {
        showHideKendoProgressbar("statistics-widget", true, _progressbarStartDelayTime);
        $("#divStatWidgetToolbar").removeClass("open");

        var selectedAcdemicYear = $("select[id$='cmbAcademicYear'] option:selected").val();

        var data = {
            aiSchoolId: _schoolId,
            aiAcademicYearId: selectedAcdemicYear
        };

        var callback = function (data) {
            if (data.GetStaffCountDetailsResult != null) {
                $("#spanTeacherCount").text(data.GetStaffCountDetailsResult.TeacherCount);
                $("#spanAdminCount").text(data.GetStaffCountDetailsResult.AdminCount);
                $("#spanOtherCount").text(data.GetStaffCountDetailsResult.OtherCount);
                $("#spanTransportCount").text(data.GetStaffCountDetailsResult.TransportCount);
                $("#spanResignedCount").text(data.GetStaffCountDetailsResult.ResignedCount);
                animateNumbers("#staff");
                $("#staff").addClass('animate');
                $("#divStaffView").show();
                $("#divStaffMessage").hide();
            }
            else {
                $("#staff").removeClass('animate');
                $("#divStaffView").hide();
                $("#divStaffMessage").show();
            }
            showHideKendoProgressbar("statistics-widget", false, _progressbarStopDelayTime);
        }

        var errorback = function (msg) {
            showHideKendoProgressbar("statistics-widget", false, _progressbarStopDelayTime);
        }

        rit.base.ajax("Post",
          serviceUrl + "GetStaffCountDetails",
           data,
           callback,
           errorback
         );

        $("#divStatWidgetToolbar i[class = 'icon- fa fa-cog']").show();
    } catch (e) {
        showHideKendoProgressbar("statistics-widget", false,  _progressbarStopDelayTime);
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- loadStaffDetailsCount', _userId);
    }
}

/******************************************** End - Staff count ***************************************/

/******************************************** Start - Library count ***************************************/
function loadLibraryDetailsCount() {
    try {
        showHideKendoProgressbar("statistics-widget", true, _progressbarStartDelayTime);
        $("#divStatWidgetToolbar").removeClass("open");
        // show library details when library module available.
        if (isLibraryModuleEnabled == 'True' && externalLibrarySite == '') {
            // Show library details
            $("#divLibraryContent").show();
            $("#divNoLibraryModule").hide();
            $("#divStatWidgetToolbar").removeClass("open");

            var data = {
                aiSchoolId: _schoolId
            }

            var callback = function (data) {
                if (data.GetLibraryCountDetailsResult != null) {
                    $("#spanTotal").text(data.GetLibraryCountDetailsResult.TotalCount);
                    $("#spanReceived").text(data.GetLibraryCountDetailsResult.ReceivedCount);
                    $("#spanPurchased").text(data.GetLibraryCountDetailsResult.PurchasedCount);
                    $("#sapnLost").text(data.GetLibraryCountDetailsResult.LostCount);
                    animateNumbers("#library");
                    $("#divLibararyView").show();
                    $("#divLibararyWidgetMessage").hide();
                } else {
                    $("#divLibararyView").hide();
                    $("#divLibararyWidgetMessage").show();
                    $("#divLibararyWidgetMessage").addClass("padding-top-120");
                }

                showHideKendoProgressbar("statistics-widget", false, _progressbarStopDelayTime);
            }

            var errorback = function (msg) {
                showHideKendoProgressbar("statistics-widget", false, _progressbarStopDelayTime);
            }

            rit.base.ajax("Post",
                      serviceUrl + "GetLibraryCountDetails",
                       data,
                       callback,
                       errorback
                     );
        }
        else {
            // hide library details and show module not available note/ external library link
            $("#divLibraryContent").hide();
            // Condition is used to check libarary module is enabled and url is empty.
            if (isLibraryModuleEnabled == 'True' && externalLibrarySite != '') {
                $("#divNoLibraryModule").html("<div style='padding-top:120px;'>Click <a target='_blank' href='" + externalLibrarySite + "'>here</a> to go to external library</div>");
            }
            else {
                $("#divNoLibraryModule").text("Library module is not available");
                $("#divNoLibraryModule").addClass("padding-top-120");
            }
            $("#divNoLibraryModule").show();
            showHideKendoProgressbar("statistics-widget", false, _progressbarStopDelayTime);
        }
    } catch (e) {
        showHideKendoProgressbar("statistics-widget", false, _progressbarStopDelayTime);
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- loadLibraryDetailsCount', _userId);
    }
}
/******************************************** End - Library count ***************************************/

// Set previous selected academic year. 
var _previousSelectedStatisticsWidgetAcademicYear;
function setStatisticsWidgetSelectedYear() {
    _previousSelectedStatisticsWidgetAcademicYear = $("select[id$='cmbAcademicYear']").val();
}

// Reset stat widget filter.
function clearStatisticsWidgetFilter() {
    hideStatisticsWidgetFilter();
    $("select[id$='cmbAcademicYear']").val(_academicYearId);
    getStatisticsCurrentTabDetailsCount();
}

// Hide hide stat widget filter.
function hideStatisticsWidgetFilter(e) {
    $("select[id$='cmbAcademicYear']").val(_previousSelectedStatisticsWidgetAcademicYear)
    $("#liStatFilter label").removeClass('active')
    $("#liStatFilter label:first").addClass('active');
    $("#divAcademicYearLabel").parent().show();
    $("#divStatWidgetToolbar").removeClass("open");
}

/******************************************** End - Statistics Widget (student, staff, library) ***************************************/

/******************************************** Start - Feedback ***************************************/
/// Get data for Feedback widget and show user feedback on widget
function loadUsersFeedbackWidget(loadDefaultData, isRefresh, userWiseKey) {

    try {
        showHideKendoProgressbar("feedback-widget", true, _progressbarStartDelayTime);
        var inputParameters = {
            aiSchoolId: _schoolId,
            aiUserRoleId: _userRoleId,
            asDesignationId: _loggedUserDesignationId,
            abIsAccountsCumAdminOfficer: _supervisorDesignationName == _constSuperviserDesignationName || _supervisorDesignationName == _constDirectorDesignationName ? true : false
        }

        // Local storage key for feed back List
        var localStorageKey = "FeedbackList";
        if (userWiseKey != undefined && userWiseKey != '')
            localStorageKey = localStorageKey + '_' + userWiseKey;
		
        var callback = function (data) {
            if (data.GetUserFeedbackResult != null) {
                $("#divFeedbackWidgetContent").show();
                $("#divFeedbackWidgetMessage").hide();
                createUserFeedbackWidget(data.GetUserFeedbackResult);
            } else {
                $("#divFeedbackWidgetContent").hide();
                $("#divFeedbackWidgetMessage").text(_errorOcuredMessage);
                $("#divFeedbackWidgetMessage").show();
                removeLocalStorage(localStorageKey);
            }
            showHideKendoProgressbar("feedback-widget", false, _progressbarStopDelayTime);
           
		    // This condition is used for to save widget data in local storage when first time load 
            // And refresh local storage case and avoid filter save case.
            if (loadDefaultData == true) {
				var numberOfDaysAhead = 1;
                saveToLocalStorage(localStorageKey, data, getExpirationHrs(numberOfDaysAhead)); //save data in local storage for 24 hr
             }
        }

        var errorback = function (msg) {
            showHideKendoProgressbar("feedback-widget", false, _progressbarStopDelayTime);
            removeLocalStorage(localStorageKey);
        }

       // Create data object to pass parameters to get feed back details.
        var dataObject = {
            isRefresh: isRefresh,
            loadDefaultData: loadDefaultData,
            keyName: localStorageKey,
            serviceUrl: serviceUrl + "GetUserFeedback",
            data: inputParameters
        };

        // If first time load and local storage is empty or expire then save in local storage
        // This function is used to get feedback details 
        getWidgetData(dataObject, callback, errorback);
    } catch (e) {
        showHideKendoProgressbar("feedback-widget", false, _progressbarStopDelayTime);
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- loadUsersFeedbackWidget', _userId);
    }
}


/// Create and append dynamic html for feedbck widget. 
function createUserFeedbackWidget(data) {
    try {
        var feedbackHtml = "";
        if (data.length > 0) {
            //$("#divShowAllFeedback").show();
            var feedbackTemplate = " <div class=\"itemdiv padding-top-bottom-5 commentdiv\">" +
                             "<div class=\"body\">" +
                                "<div class=\"name\">" +
                                    "<span class=\"username\" >@%@UserName@%@</span>" +
                                "</div>" +
                                "<div class=\"time\">" +
                                    "<i class=\"icon- fa fa-clock-o\"></i><span class=\"green\">&nbsp;@%@Date@%@</span>" +
                                "</div>" +
                                "<div class=\"text\" data-rel=\"tooltip\" data-placement=\"left\" data-original-title=\"@%@Tooltip@%@\" title =\"\" >" +
                                    "<i class=\"icon- fa fa-quote-left\"></i> @%@Feedback@%@" +
                                "</div>" +
                            "</div>" +
                        "</div>";

            for (var dataCnt = 0; dataCnt < data.length; dataCnt++) {
                var html = feedbackTemplate;
                var text = data[dataCnt].Text;
                html = html.replace("@%@UserName@%@", data[dataCnt].UserName);
                html = html.replace("@%@Date@%@", data[dataCnt].Date);
                if (text.length > 200) {
                    html = html.replace('@%@Tooltip@%@', text);
                    var subText = text.substring(0, 200);
                    html = html.replace("@%@Feedback@%@", subText + '...');
                }
                else {
                    html = html.replace('@%@Tooltip@%@', '');
                    html = html.replace("@%@Feedback@%@", text);
                }

                feedbackHtml += html;
            }
			$(".comments").html(feedbackHtml);

            // add slim scroll to feedback
            $('.comments').slimScroll({
                height: '277px'
            });

            $('[data-rel=tooltip]').tooltip({ container: "#feedback-widget" });
        }
        else {
            $("#divFeedbackWidgetContent").hide();
            $("#divFeedbackWidgetMessage").text("No Record Found");
            $("#divFeedbackWidgetMessage").show();

        }
    } catch (e) {
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- createUserFeedbackWidget', _userId);
    }
}

/******************************************** End - Feedback***************************************/

/******************************************** common function to all widget**************************************/
/*This function is used to show or hide progress bar based on the control pass and and value of isshow parameter 
because here we are passing isshow true when need to show progress bar*/
function showHideKendoProgressbar(controlId, isShow, delay) {
    try {
        /* hide progressbar after passed delay*/
        setTimeout(function () {
            kendo.ui.progress($("#" + controlId + ""), isShow);
        }, delay);

    } catch (e) {
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- showHideKendoProgressbar (controlId - ' + controlId + ')', _userId);
    }
}

// Reload chart and gauge
function refreshWidgets() {
    if ($("#classAttendanceGauge").data("kendoRadialGauge") != undefined)
        $("#classAttendanceGauge").data("kendoRadialGauge").redraw();

    if ($("#studentAttendanceGauge").data("kendoRadialGauge") != undefined)
        $("#studentAttendanceGauge").data("kendoRadialGauge").redraw();

    if ($("#studentPerformanceChart").data("kendoChart") != undefined)
        $("#studentPerformanceChart").data("kendoChart").refresh();

    if ($("#accountFlowChart").data("kendoChart") != undefined)
        $("#accountFlowChart").data("kendoChart").refresh();

    if ($("#divExamwiseStudentPerformanceChart").data("kendoChart") != undefined)
        $("#divExamwiseStudentPerformanceChart").data("kendoChart").refresh();

    if ($("#payrollChart").data("kendoChart") != undefined)
        $("#payrollChart").data("kendoChart").refresh();

    updateAttendancePieChart();
}

// Show animation effect to numbers having count class
function animateNumbers(parentContainer) {
    $(parentContainer + ' .count').each(function () {
        $(this).prop('Counter', 0).animate({
            Counter: $(this).text()
        }, {
            duration: 1000,
            easing: 'swing',
            step: function (now) {
                $(this).text(Math.ceil(now));
            }
        });
    });
}

/**************************************** Start - Bind Animation Events ****************************************/

setAnimationForControls();

function setAnimationForControls() {
    try {
        $(".animate").each(function () {
            $(this).bind('inview', function (event, visible) {
                var $this = $(this),

            $animation = ($this.data("animation") !== undefined) ? $this.data("animation") : "slideUp";
                $delay = ($this.data("delay") !== undefined) ? $this.data("delay") : 300;

                if (visible == true) {
                    setTimeout(function () { $this.addClass($animation); }, $delay);
                } else {
                    setTimeout(function () { $this.removeClass($animation); }, $delay);
                }
            });
        });
    } catch (e) {
        SaveErrorLog(e, 'ControlPanel.aspx - Dashboard.js :- setAnimationForControls', _userId);
    }
}
/**************************************** End - Bind Animation Events ****************************************/

/**************************************** Start - Local Storage Related **********************************/

//This is common function for get details of widgets. 
function getWidgetData(dataObject, callback, errorback) {
    
    var localStorageData = null;

	//This code is used to remove data from localstorage when user tries to refresh widget data
    if (dataObject.loadDefaultData == true && dataObject.isRefresh == true)
        removeLocalStorage(dataObject.keyName);
    else if (dataObject.isRefresh == false) // This code is used to remove data from localstorage when user tries to refresh widget data
        localStorageData = loadDataFromLocalStorage(dataObject.keyName);

    if (localStorageData == null) {
        rit.base.ajax("Post",
                dataObject.serviceUrl,
                dataObject.data,
                callback,
                errorback
           );
    }
    else {
        callback(localStorageData);
    }
}

// This function is use to store data in local storage with key.
function saveToLocalStorage(key, jsonData, expirationHrs) {
    if (loadDataFromLocalStorage(key) == null) {
        if (typeof (Storage) == "undefined") { return false; }

        removeLocalStorage(key);

        var record;
        if (expirationHrs != undefined) {
            var expirationMS = expirationHrs * 60 * 60 * 1000;
            record = { value: JSON.stringify(jsonData), timestamp: new Date().getTime() + expirationMS, created: new Date().getTime() }
        }
        else {
            record = { value: JSON.stringify(jsonData) };
        }

        localStorage.setItem(key, JSON.stringify(record));
        return jsonData;
    }
}

// This function is used for clear local storage.
function removeLocalStorage(key) {
    localStorage.removeItem(key);
}

// This function is used to get expire hrs.
function getExpirationHrs(numberOfDaysAhead) {
    var hrsPassedForTheDay = new Date().getHours() - 1;
    return (24 * numberOfDaysAhead) - hrsPassedForTheDay;
}
// This is a Common function used to apply overlay div.
function addOverlayDivForSampleData(containerDivSelector, top)
{
    $(containerDivSelector).append("<div class='overlay-container'>"
                + "<div class='col-xs-12 overlay-wrapper'>"
                + "<span class='overlay-msg' style='top:" + top + "'>Sample Data</span></div></div>");
}
/**************************************** End - Local Storage related **********************************/


/* this function is used to get upcoming events */
function loadUpcomingEventsWidget(loadDefaultData, isRefresh) {
    try {
        var inputParameters =
         {
             aiSchoolId: _schoolId,
             aiAcademicYearId: _academicYearId,
             aiUserId: _userId,
             aiUserRoleId: _userRoleId,
             isScreenFullAccess: _isFullScreenAccess
         };

         // Local storage key for upcoming events list
        var localStorageKey = "UpcomingEventsList"+ "_" + _userId;

        var callback = function (data) {
            if (data.GetUpcomingEventsResult != null) {
                createUpcomingEventList(data.GetUpcomingEventsResult);
                $("#liSeeAllEvents").show();
            }
            else {
                $("#divEventContainer").addClass("hide");
                $("#divInnerUlEvents").removeClass("hide");
                $("#liSeeAllEvents").hide();
                removeLocalStorage(localStorageKey);
            }

            // This condition is used for to save widget data in local storage when first time load 
            // And refresh local storage case and avoid filter save case.
            if (loadDefaultData == true) {
                var numberOfDaysAhead = 3;
                saveToLocalStorage(localStorageKey, data, getExpirationHrs(numberOfDaysAhead)); //save data in local storage for 3 day
            }
        }

        var errorback = function (msg) {
            alert(msg.statusText);
            removeLocalStorage(localStorageKey);
        }

        // Create data object to pass parameters to get upcoming events list.
        var dataObject = {
            isRefresh: isRefresh,
			loadDefaultData: loadDefaultData,
            keyName: localStorageKey,
            serviceUrl: serviceUrl + "GetUpcomingEvents",
            data: inputParameters
        };

        // If first time load and local storage is empty or expire then save in local storage
        // This function is used to get upcoming events list 
        getWidgetData(dataObject, callback, errorback);
    }

    catch (e) {
        SaveErrorLog(e, 'MasterPage.master - Dashboard.js :- loadUpcomingEventsWidget', _userId);
    }
}

/*This function is used to create upocoming event list based on return data*/
function createUpcomingEventList(data) {
    try {
        
        var eventDate = "";
        $("#innerUlEvents").html('');
        kendo.ui.progress($("#innerUlEvents"), true);
        var liTemplate = "<li class='padding-5 padding-left-17 @%@class@%@'>" +
                                     "<span>" +
                                           "<span>@%@EventTitle@%@&nbsp (@%@StartDate@%@)</span>" +
                                           "<div class=\"padding-bottom-5\">Standard(s) : @%@StandardName@%@</div>" +
                                         "</span>" +
                                          "</li>";
        if (data.length > 0) {

            var EventHtml = "";
            var todaysDate = new Date();
            var date = todaysDate.getDate();
            var month = todaysDate.getMonth() + 1; //January is 0!
            var year = todaysDate.getFullYear();
            if (date < 10) {
                date = '0' + date;
            }
            if (month < 10) {
                month = '0' + month;
            } 

            todaysDate = new Date(year + '-' + month + '-' + date);
            
            for (var iCnt = 0; iCnt < data.length; iCnt++) {
                var endDate = data[iCnt].EndDateUniversal;

                var isEventInFuture = false;
                if (new Date(endDate).getTime() - todaysDate.getTime() >= 0)
                    isEventInFuture = true;

                if (isEventInFuture) {//local storage can contain old events, do not consider those
                    var template = liTemplate;
                    if (data[iCnt].EventType == "Holiday")
                        template = template.replace('@%@class@%@', "holidays");
                    if (data[iCnt].EventType == "Exam")
                        template = template.replace('@%@class@%@', "exam");
                    if (data[iCnt].EventType == "Event")
                        template = template.replace('@%@class@%@', "events");

                    if (data[iCnt].StartDate == data[iCnt].EndDate)
                        eventDate = data[iCnt].StartDate.substring(0, 6);
                    else
                        eventDate = data[iCnt].StartDate.substring(0, 6) + " to " + data[iCnt].EndDate.substring(0, 6);

                    EventHtml += template.replace("@%@EventTitle@%@", data[iCnt].EventTitle)
                                     .replace("@%@StandardName@%@", data[iCnt].StandardName)
                                     .replace("@%@StartDate@%@", eventDate);
                }
            }
			
			if (data.length < 4) {
				$("#innerUlEvents").css("height", 'auto');
				$("#innerUlEvents").css("min-height", 'auto');
			   }

            $("#innerUlEvents").append(EventHtml);

            // add slim scroll to feedback
            $('#innerUlEvents').slimScroll({
                height: '318px',
                width: '100%'
            });

            setTimeout(function () {
                kendo.ui.progress($("#innerUlEvents"), false);
            }, 1000);

            $("#divEventContainer").removeClass('hide');
            $("#divInnerUlEvents").addClass('hide');
        }
       else {
            // EventHtml = "<li class=\"error-message\"><a> No Record Found</a></li>";
            $("#divEventContainer").addClass('hide');
            $("#divInnerUlEvents").removeClass('hide');
            $("#divInnerUlEvents").text("No Record Found");
        }
    }
    catch (e) {
        SaveErrorLog(e, 'MasterPage.master :- createUpcomingEventList', _userId);
    }
}

/* This function is used to get unread message*/
function loadUnreadMessageWidget() {
    try {
         // Local storage key for user profile picture
        var localStorageKeyOfUserPic = "UserProfilePic" + "_" + _userId;
        var profilePicData = null;
        profilePicData = loadDataFromLocalStorage(localStorageKeyOfUserPic);
        var data =
              {
                  aiSchoolId: _schoolId,
                  aiAcademicYearId: _academicYearId,
                  aiReceiverId: _userId,
                  aiReceiverRoleId: _userRoleId,
                  asProfilePicUpdDt: (profilePicData != null && Object.keys(profilePicData).length !== 0 ? profilePicData[0].UpdateDate : "")
              };

              //On success bind the list of messages.
              var callback = function (data) {
                  if (data.GetUnreadMessageListResult != null) {
                      createMessageList(data.GetUnreadMessageListResult.UnreadMessages, data.GetUnreadMessageListResult.SenderPhoto);
                      $("#unreadmsg-Count").text(data.GetUnreadMessageListResult.UnreadMessageCount);
                      showHideUnreadMessageList(true);
                  }
                  else {
                      showHideUnreadMessageList(false);
                  }

                  // This condition is used for to save profile picture in local storage when first time load 
                  // And refresh local storage case and avoid filter save case.
                  if (data.GetUnreadMessageListResult != null && data.GetUnreadMessageListResult.UserProfilePicData != null && data.GetUnreadMessageListResult.UserProfilePicData.length != 0) {
                      removeLocalStorage(localStorageKeyOfUserPic);
                      saveToLocalStorage(localStorageKeyOfUserPic, data.GetUnreadMessageListResult.UserProfilePicData);
                      profilePicData = loadDataFromLocalStorage(localStorageKeyOfUserPic);
                  }

                  // Set user profile picture and user details picture
                  if (profilePicData != null && profilePicData.length != 0 && profilePicData[0].ProfilePicture != null && profilePicData[0].ProfilePicture != "") {
                      var userImage = "data:image/png;base64," + profilePicData[0].ProfilePicture;
                      SetUserProfilePic(_userRoleId, userImage);
                  }
              }

        var errorback = function (msg) {
            showHideUnreadMessageList(false);
        }

        rit.base.ajax("Post",
                serviceUrl + "GetUnreadMessageList",
                 data,
                callback,
                errorback
                );
    }
    catch (e) {
        SaveErrorLog(e, 'MasterPage.master :- loadUnreadMessageWidget', _userId);
    }
}


/* This function is used to set images from local storage */
function SetUserProfilePic(roleId, userImage) {
    // Set user profile picture
    $("[id*='imgProfilePic']").attr("src", userImage);

    if (roleId == '2')
        $("[id*='imgTeacher']").attr("src", userImage); // Set pic of teacher details page
    if (roleId == '3')
        $("[id*='imgPhoto']").attr("src", userImage); // Set pic of student details page
    if (roleId == '6')
        $("[id*='imgSuperVisor']").attr("src", userImage); // Set pic of admin staff details page
    if (roleId == '7')
        $("[id*='imgSuperVisor']").attr("src", userImage); // Set pic of other staff details page
}

/* This function is used to show hide list of messages based on the value*/
function showHideUnreadMessageList(isShow) {
    if (isShow) {
        $("#unreadmsg-Count").show();
        $("#liSeeallmessages").show();
    }
    else {
        $("#unreadmsg-Count").hide();
        $("#liSeeallmessages").hide();
        $("#divMessageCotainer").addClass("hide");
        $("#divInnerUlMessage").removeClass("hide");
        $("#liSeeallmessages").hide();

    }
}

/*This function is used to create list based on return data*/
function createMessageList(data, dataSenderPhoto) {
    try {
        var MessageViewUrl = pageUrl + "/RITeSchool/Common/MessageViewUI.aspx?";
        $("#innerUlMessage").html('');
        kendo.ui.progress($("#innerUlMessage"), true);

        var liTemplate = "<a href=\"@%@MessageViewUrl@%@\" class=\"unread-msg-li\">" +
                            "<img src=\"data:image/jpg;base64,@%@PhotoPath@%@\" class=\"msg-user-photo\">" +
                            "<span class=\"unread-msg-body\">" +
                                "<span class=\"unread-msg-title\">" +
                                    "<span class=\"blue\">@%@UserName@%@</span>" +
                                    "<div class='unread-msg-subject'>@%@Subject@%@</div>" +
                                "</span>" +
                                "<span class=\"unread-msg-time\">" +
                                    "<i class=\"icon- fa fa-clock-o\"></i>" +
                                    "<span>&nbsp; @%@Date@%@</span>" +
                                "</span>" +
                            "</span>" +
                        "</a>";

        if (data.length > 0) {
            var messageHtml = "";
            for (var iCnt = 0; iCnt < data.length; iCnt++) {
                var template = liTemplate;

                for (var iCntOfUser = 0; iCntOfUser < dataSenderPhoto.length; iCntOfUser++) 
                {
                    if (dataSenderPhoto[iCntOfUser].Id == data[iCnt].SenderUserId) {
                        if (dataSenderPhoto[iCntOfUser].Photo == '')
                            template = template.replace("data:image/jpg;base64,@%@PhotoPath@%@", _defaultProfilePicPath);
                        else
                            template = template.replace("@%@PhotoPath@%@", dataSenderPhoto[iCntOfUser].Photo);
                    }
                }

                messageHtml += template.replace("@%@MessageViewUrl@%@", MessageViewUrl + data[iCnt].ReturnUrl)
                                   .replace("@%@Subject@%@", data[iCnt].Subject)
                                   .replace("@%@UserName@%@", data[iCnt].UserName)
                                   .replace("@%@Date@%@", data[iCnt].Date);
            }
        
            if (data.length < 3) {
                $("#innerUlMessage").css("height", 'auto');
                $("#innerUlMessage").css("min-height", 'auto');
            }

            $("#innerUlMessage").append(messageHtml);

            // add slim scroll to feedback
            $('#innerUlMessage').slimScroll({
                height: '318px'
            });

            setTimeout(function () {
                kendo.ui.progress($("#innerUlMessage"), false);
            }, 1000);

            $("#divMessageCotainer").show();
            $("#divInnerUlMessage").addClass('hide');
        }
        else {
            // messageHtml = "<li class=\"error-message no-record-message\"><a> No Record Found</a></li>";
            $("#divMessageCotainer").hide();
            $("#divInnerUlMessage").removeClass('hide');
            $("#divInnerUlMessage").text("No Record Found");
        }
    }
    catch (e) {
        SaveErrorLog(e, 'MasterPage.master :- createMessageList', _userId);
    }
}

// This function is used to get hours of the upcoming date (passed date).
function getHrsToUpcomingDate(p_dayNo, p_startDate){
    var startDate;

    if(p_startDate)
        startDate = new Date(p_startDate);
    else
        startDate = new Date();
  
    var startDayNo = startDate.getDate();
    var startMonthNo = startDate.getMonth() + 1;
    var startMonthYear = startDate.getFullYear();

    var tempMonthNo;
    var tempYearNo;

    if(startDayNo < p_dayNo){ //if user accesses screen on day 1 to day 6 then upcoming date will be from current month
        tempMonthNo = startMonthNo;
        tempYearNo = startMonthYear;
    }
    else{ //if user accesses screen on day 7 to last day of month then upcoming date will be from next month
        if (startMonthNo == 12) { //if current month is Dec then for getting next month increase year by 1 and set monthNo to 1
            tempMonthNo = 1;
            tempYearNo = startMonthYear + 1;
        }
        else{
            tempMonthNo = startMonthNo + 1;
            tempYearNo = startMonthYear;
        }
    }

    if (p_dayNo < 10) {
        p_dayNo = '0' + p_dayNo;
    }
    if (tempMonthNo < 10) {
        tempMonthNo = '0' + tempMonthNo;
    }

    var upComingDate = new Date(tempYearNo + "-" + tempMonthNo + "-" + p_dayNo); //construct upcoming date

    var miliSecsToUpcomingDate = upComingDate.getTime() - startDate.getTime(); //calculate differance between upcoming date & current date

    var hrsToUpcomingDate = miliSecsToUpcomingDate / (1000 * 60 * 60); //convert mili seconds to hrs
 
    return parseInt(hrsToUpcomingDate);
}

function SetTooltip(id, msg) {
    $("#" + id).attr("data-content", msg);
}

// This function is used to set tooltip attribute
function refreshToolTip(id, key, userWiseKey) {
    if(userWiseKey == 'AdminOrPrincipal')
        key = key + '_' + userWiseKey;
    else if(userWiseKey == _userId)
        key = key + '_' + userWiseKey;

    $("#" + id).attr("data-content", getRefreshedToolTip(key));
}


// This function is used for get refresh tooltip
    function getRefreshedToolTip(key) {
        if (typeof (Storage) == "undefined") {
            return "Click here to reload data.";
        }

        if (typeof (Storage) == "undefined") {
            return "Click here to reload data.";
        }

        // Get data from local storage and check otherwise return false.
        var record = JSON.parse(localStorage.getItem(key));

        if (!record) {
            return "Click here to reload data.";
        }

        // If timestamp expire then return false and clear local storage
        var miliSecondsOld = new Date().getTime() - record.created;

        var daysOld = parseInt(miliSecondsOld / (1000 * 60 * 60 * 24))

        if (daysOld > 0) {
            return "You are viewing " + daysOld + " day(s) old data, click here to see latest data.";
        }
        else {
            var hrsOld = parseInt(miliSecondsOld / (1000 * 60 * 60));

            if (hrsOld > 0) {
                return "You are viewing " + hrsOld + " hr(s) old data, click here to see latest data.";
            }
            else {
                var minsOld = parseInt(miliSecondsOld / (1000 * 60))
                if (minsOld > 0)
                    return "You are viewing " + minsOld + " minute(s) old data, click here to see latest data.";
                else {
                    var secsOld = parseInt(miliSecondsOld / 1000)
                    if (secsOld > 0)
                        return "You are viewing " + secsOld + " second(s) old data, click here to see latest data.";
                    else
                        return "Click here to reload data.";
                }
            }
        }
    }

    //This event is used to remove data from localstorage because after change academic year and financial selected financial and academic year value.
    $("select[id$='cmbAcademicYearID']").change(function () {
        var selectedAcademicYear = $("select[id$='cmbFeeAcademicYear'] option:selected").val();
        createCookie("AcademicYearId", selectedAcademicYear, 1);
        removeRoleWiseAcademicYearLocalStorage();
    });

    $("select[id$='ddlFinancialYears']").change(function () {
        var selectedFinancialYear = $("select[id$='ddlFinancialYears'] option:selected").val();
        createCookie("FinancialYears", selectedFinancialYear, 1);
        removeFinancialLocalStorage();
    });

    //This function is used to validate academic year id and financial year id. 
    function validateAndRemoveLocalStorage() {
        //This condition is used to check existing cookie available or not. If not then create cookie of academic year. 
        if (readCookie("AcademicYearId") != null && readCookie("AcademicYearId") != undefined) {
            //Check cookie value is same as default selected academic year id if not then remove localstorage and set cookies as default academic year id value.
            if (readCookie("AcademicYearId") != _academicYearId) {
                removeRoleWiseAcademicYearLocalStorage();
                createCookie("AcademicYearId", _academicYearId, 1);
            }
            else {
                createCookie("AcademicYearId", _academicYearId, 1);
            }
        }
        else {
            createCookie("AcademicYearId", _academicYearId, 1);
        }


        var selectedFinancialYear = $("select[id$='ddlFinancialYears'] option:selected").val();
        //This condition is used to check existing localstorage is available or not if not then create cookies. 
        if (readCookie("FinancialYears") != null && readCookie("FinancialYears") != undefined) {
            //Check cookies value is same as default selected financial year id if not then remove localstorage and set cookies as default financial year id value.
            if (readCookie("FinancialYears") != selectedFinancialYear) {
                removeFinancialLocalStorage();
                createCookie("FinancialYears", selectedFinancialYear, 1);
            }
            else {
                createCookie("FinancialYears", selectedFinancialYear, 1);
            }
        }
        else {
            createCookie("FinancialYears", selectedFinancialYear, 1);
        }
    }

    //This function is used to remove academic year localstorage based on the role
    function removeRoleWiseAcademicYearLocalStorage() {
        var localStorageKeyName = "";

        //If logged in user will be admin or principal then remove all widget localstorage data except user specific.
        if (_loggedUserDesignationId == _constPrincipalDesignation || _userRoleId == _constAdminRole || _supervisorDesignationName == _constSuperviserDesignationName || _supervisorDesignationName == _constDirectorDesignationName) {
            localStorageKeyName = ["FeeSummary", "AdminAttendanceSummary", "UpcomingEventsList" + "_" + _userId, "ExamWiseStudentPerformance", "AccountSummary", "PayrollSummary", "BirthdayList", "StudentStatistic"];
        }
        else {
            // Otherwise only need to remove localstorage dat of birthday and upcomingevents.
            localStorageKeyName = ["UpcomingEventsList" + "_" + _userId, "BirthdayList"];
        }

        localStorageKeyName.forEach(function (key) {
            removeLocalStorage(key);
        });
    }

    //This function is used to remove financial year localstorage.
    function removeFinancialLocalStorage() {
        if (_loggedUserDesignationId == _constPrincipalDesignation || _userRoleId == _constAdminRole || _supervisorDesignationName == _constSuperviserDesignationName || _supervisorDesignationName == _constDirectorDesignationName) {
            var localStorageKeyName = ["PayrollSummary", "AccountSummary"];
            localStorageKeyName.forEach(function (key) {
                removeLocalStorage(key);
            });
        }
    }


    
   