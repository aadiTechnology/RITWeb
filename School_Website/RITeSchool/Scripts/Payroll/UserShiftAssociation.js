
$(document).ready(function () {
    AutoSearch();
});


var prm = Sys.WebForms.PageRequestManager.getInstance();
prm.add_endRequest(EndRequestHandler);

// This function is used to enabled controls once a postback is complete.
function EndRequestHandler() {
    AutoSearch();
}

