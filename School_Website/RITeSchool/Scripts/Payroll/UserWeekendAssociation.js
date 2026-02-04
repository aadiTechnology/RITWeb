

$(document).ready(function () {
    AutoSearch();
});

function AutoSearch() {
    BindAutoCompleteEventForStaff("<%=miSchoolId %>", "<%=miAcademicYearId %>", _slienttxtUserName, null, null, null, 0);
}

function SearchSelectedValue(val) {
    txt = document.getElementById(_slienttxtUserName);
    bt = document.getElementById(_clientbtnSearch);
    SearchResult(txt, val, bt);
}