$(document).ready(function () {

    var query = $('#ctl00_MainBody_DtPgCount1');
    var isVisible = query.is(':visible');

    if (isVisible != true) {
        $('#tdnonpunched').addClass("padding_Top_21");
    } 

    var query1 = $('#ctl00_MainBody_DtPgCount');
    var isVisible1 = query1.is(':visible');

    if (isVisible1 != true) {
        $('#tdpunched').addClass("padding_Top_21");
    } 

});
