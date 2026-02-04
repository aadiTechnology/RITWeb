var rit = {};

rit.base = {};

rit.base.ajax = function (type, url, data, successCallback, errorCallback, isAsync, isCrossDomain) {

    if (isAsync == undefined || isAsync == null || isAsync.toString() == '')
        isAsync = true;

    if (isCrossDomain == undefined || isCrossDomain == null || isCrossDomain.toString() == '')
        isCrossDomain = false;

    $.ajax({
        //The type of request to make ("POST" or "GET"), default is "GET".
        type: type,
        // url - A string containing the URL to which the request is sent.
        url: url,
        /* This option is used to set whether the call is to be processed asynchronously or synchronously. */
        async: isAsync,
        /* This allows, server-side redirection to another domain.
        // If you wish to force a crossDomain request (such as JSONP) on the same domain, set the value of crossDomain to true. */
        crossDomain: isCrossDomain,
        /* When sending data to the server, use this content-type. Default is "application/x-www-form-urlencoded".
        // Data will always be transmitted to the server using UTF-8 charset; you must decode this appropriately on the server side.*/
        contentType: "application/json; charset=utf-8",
        /* Data to be sent to the server. It is converted to a query string, if not already a string. 
        It's appended to the url for GET-requests. 
        Object must be Key/Value pairs. If value is an Array, jQuery serializes multiple values with same key based on the value of the traditional setting (described below).*/
        dataType: "json",
        data: JSON.stringify(data),
        /* The type of data that you're expecting back from the server.
        // "json": Evaluates the response as JSON and returns a JavaScript object. */

        success: function (result) {
            if (typeof (successCallback) == 'function')
                successCallback(result);
        },
      error: function (msg) {
            if (msg.status != 0) {
                if (typeof (errorCallback) == 'function') {
                    errorCallback(msg)
                }
                var methodName = url;
                var dataString = JSON.stringify(data);
                //SaveErrorLog(msg, 'URL - ' + methodName + ', Data - ' + dataString + ', Status - ' + msg.status + ', Status Text - ' + msg.statusText, _userId);
            }
        }
    });
}

/*
This function is used to create a cookie for maintaining login details.
*/
function createCookie(name, value, days) {
	try {
		var expires;
		if (days) {
			var date = new Date();
			date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
			expires = "; expires=" + date.toGMTString();
		}
		else expires = "";

		if (typeof (Storage) !== "undefined") {
			localStorage.setItem(name, value + expires + "; path=/");
		}
		else {
			document.cookie = name + "=" + value + expires + "; path=/";
		}
	}
	catch (e) {
	    SaveErrorLog(e, 'MasterPage.master - base.js :- createCookie', _userId);
	}
}

/*
This function is used to read existing cookie.
*/
function readCookie(name) {
    try {
        if (typeof (Storage) !== "undefined") {
            if (localStorage.getItem(name) != null)
                return localStorage.getItem(name).split(';')[0];
        }
        else {
            var nameEQ = name + "=";
            var ca = document.cookie.split(';');

            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') c = c.substring(1, c.length);
                if (c.indexOf(nameEQ) == 0) return (c.substring(nameEQ.length, c.length));
            }
        }
        return null;
    } catch (e) {
        SaveErrorLog(e, 'MasterPage.master - base.js :- readCookie', _userId);
    }
}


/*This method is used to save error log.*/
function SaveErrorLog(Message, methodName, userId) {
    try {
        var browserInfo = getBrowserDetails();
        var message = Message.message;

        if (Message.fileName != undefined)
            message += " file name - " + Message.fileName;

        if (Message.lineNumber != undefined)
            message += " line no - " + Message.lineNumber;

        if (Message.stack != undefined)
            message += " stack - " + Message.stack;

        var data = {
            "asSchoolId": _schoolId,
            "aiAcademicYearId": _academicYearId,
            "asMessage": message,
            "asMethodName": methodName,
            "asBrowserInfo": browserInfo.length > 1 ? browserInfo[0] + " - " + browserInfo[1] : browserInfo[0],
            "aiUserId": userId
        };

        rit.base.ajax("POST", serviceUrl + "LogErrorAtClientSide", data, "");

    } catch (e) {
        //SaveErrorLog(e, 'MasterPage.master - base.js :- createCookie', readCookie("UserId"));
    }
}

//This function is used to detect browser info with browser name and version number
function getBrowserDetails() {
    try {
        var browserName = navigator.appName;
        var browserUserAgent = navigator.userAgent;
        var temp;
        var browserDetails = browserUserAgent.match(/(opera|chrome|safari|firefox|msie)\/?\s*(\.?\d+(\.\d+)*)/i);
        if (browserDetails && (temp = browserUserAgent.match(/version\/([\.\d]+)/i)) != null)
            browserDetails[2] = temp[1];
        browserDetails = browserDetails ? [browserDetails[1], browserDetails[2]] : [browserName, navigator.appVersion, '-?'];
        return browserDetails;
    }
    catch (e) {
        //SaveErrorLog(e, 'MasterPage.master - base.js :- createCookie', readCookie("UserId"));
    }
}