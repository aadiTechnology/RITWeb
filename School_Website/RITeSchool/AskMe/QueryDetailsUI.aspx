<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="QueryDetailsUI.aspx.cs" Inherits="QueryDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <div>
        <style type="text/css">
            .clsUnsubmitRow
            {
                font-weight:bold;
            }
            
            a.k-button.k-button-icontext{
                min-width : 0;
            }
        </style>
            <table width="100%">
                <tr>
                    <td align="center">
                        <table width="80%">
                            <tr>
                                <td colspan="3" align="center">
                                    <table>
                                        <tr>
                                            <td align="center" width="100px;" class="ClsBorderlight">
                                                <span class="ClsLabel">Status :</span>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="cmbStatus" runat="server" onchange="FilterQuestionDetails()"
                                                    CssClass="MidCombo">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight">
                                                <span class="ClsLabel">Query :</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="ExLrgTxtBox" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight">
                                                <span class="ClsLabel">Category :</span>
                                            </td>
                                            <td>
                                                <asp:Panel ID="pnlCategories" runat="server" ScrollBars="Vertical" Width="500px"
                                                    Height="100px" class="ClsBorderlight">
                                                    <asp:CheckBoxList ID="chkCategoryLst" runat="server" RepeatDirection="Horizontal"
                                                        CssClass="ClsLabel" RepeatColumns="5">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn" OnClientClick="FilterQuestionDetails(); return false;" />
                                                <asp:Button ID="btnNewQery" runat="server" Text="New Query" CssClass="ClsBtn" OnClientClick="ShowNewPopup(); return false;" Visible="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td align="right" colspan="3">                                    
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div id="divQuestions">
                                    </div>
                                    <div id="divUserSelection" style="display:none;">
                                        <table width="100%">
                                            <tr id="trUserRole" runat="server">
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:Label ID="Label5" runat="server" CssClass="ClsLabel" Text="User Role "></asp:Label>
                                                    <span class="ClsLabel colonPadding">:</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbUserRole" runat="server" CssClass="MidCombo" AutoPostBack="True"
                                                        >
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">* </span>
                                                </td>
                                            </tr>
                                            <tr id="trUser" runat="server">
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:Label ID="Label6" runat="server" CssClass="ClsLabel" Text="User "></asp:Label>
                                                    <span class="ClsLabel colonPadding">:</span>
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="cmbUser" runat="server" CssClass="LrgCombo">
                                                            </asp:DropDownList>
                                                            <span class="ClsMdtStar">* </span>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="cmbUserRole" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">
                                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back %>"
                                        CssClass="ClsBtn" OnClientClick="OpenAskMePage(); return false;" />
                                    <input type="hidden" id="hidQuestionId" value="0" />
                                    <input type="hidden" id="hisIsModerator" value="0" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="Updatepanel1" runat = "server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div id="divTemplates" runat="server" style="visibility: hidden; display: none; position: fixed;
                                    margin: 0px; padding: 0px; width: 390px; height: auto; border-width: 1px; left: 5px;
                                    top: 0px; line-height: normal; border: solid 2px darkgreen; margin: -110px 0px 0px 00px;
                                    background-color: white;">
                                    <div style="background-color:#eedfcc; height:22px;">
                                        <div style="font-size: 12px; width: 350px; letter-spacing: 1px; padding-left: 8px;
                                            font-weight: bolder; color: darkgreen; float: left; height: 10px" align="left">
                                            Read Receipt :-
                                        </div>
                                        <span style="cursor: hand; float: right;" onclick="javascript:HidePopup();">
                                            <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif" border="0" />
                                        </span>
                                    </div>
                                    <div style="padding: 2px; background-color: white; text-align: left; vertical-align: top;
                                        color: #333; overflow: auto; height: auto; width: 380px; margin-left: 1px" id="Div5">
                                        <asp:Label ID="lblAllRemarks" runat="server" CssClass="ClsLabel" Style="float: left; font-family:Verdana; font-size:13px; padding-top:15px; padding-bottom:15px;"></asp:Label>
                                    </div>
                                    <div style="padding-bottom:10px;" align="center">
                                        <asp:Button ID="btnClose" runat="server" OnClientClick="HidePopup()" CssClass="ClsBtn" Text="Close" />
                                  </div>
                                 </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID = "btnClose" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>                   
            <script type="text/javascript">

                var _schoolId = "<%=miSchoolId %>";
                var _academicYearId = "<%=miAcademicYearId %>";
                var _loginUserId = "<%=miUserId %>";
                var _status = "<%=this.cmbStatus.ClientID %>"
                var _filter = "<%=this.txtSearch.ClientID %>"
                var _clientchkCategoryLst = "<%=this.chkCategoryLst.ClientID %>"

                $(function () {
                    FillQuestionDetails();
                    $(".TransferToPrevious").width(300);
                });                

                function FillQuestionDetails() {
                    var questionGrid = $("#divQuestions").kendoGrid({
                        columns: [
                        { field: "Title", title: "Query",template: '#=SetFontState(IsQueryInUnsubmitState, Title)#' },
                        { field: "Status", title: "Status", width: "100px"},
                        { field: "LastUpdatedDate", title: "Last Updated Date", width: "150px", format: "{0:dd-MMM-yyyy}"},
                        {
                            command: [
                            { text: "View", name: "View", click: ViewCommunication },
                            { text: "Owner(s)", name: "SetOwner", click: SetOwner },
                            { text: "Answer", name: "Reply", click: SendReply },
                            ], title: "Basic Actions", width: "200px"
                        },
                        {
                            command: [                          
                            { text: "<<", name: "TransferToPrevious", click: TransferToPrevious },
                            { text: ">>", name: "TransferToNext", click: TransferToNext },
                            ], title: "Submit Actions", width: "120px"
                        },
                        {
                            command: [
                            { text: "Read Rcpt", name: "ViewStatus", click: ViewStatus },
                            { text: "Publish", name: "Publish", click: ShowSelectionPopup },
                            { text: "Invalid", name: "MarkAsInvalid", click: MarkAsInvalid },
                            ], title: "Other Actions", width: "230px"
                        }
                        ],
                        pageable: { info: true, buttonCount: 5 },
                        filterable: false,
                        sortable: { mode: "single", allowUnsort: false },
                        editable: false,
                        selectable: "single row",
                        detailInit: detailInit,
                        dataBound: SetButtonState,
                        dataSource: {
                            serverPaging: true,
                            serverSorting: true,
                            serverFiltering: true,
                            pageSize: 20,
                            schema: {
                                data: "d.Data",
                                total: "d.Total",
                                model: {
                                    fields: {
                                        LastUpdatedDate: { type: "date" }
                                    }
                                }
                            },
                            batch: true,
                            transport: {
                                read: {
                                    url: "QueryDetailsUI.aspx/GetAllQuestions",
                                    contentType: "application/json; charset=utf-8",
                                    type: "POST",
                                    complete: function () {
                                        showHidePager();
                                    }
                                },
                                parameterMap: function (data, operation) {
                                    if (data.models) {
                                        return JSON.stringify({ products: data.models });
                                    } else if (operation == "read") {
                                        data = $.extend({ sort: null, filter: null }, data);
                                        data = $.extend({ aiSchoolId: _schoolId, aiAcademicYearId: _academicYearId, aiLoginUserId: _loginUserId, aiStatusId: document.getElementById(_status).value, asFilter: document.getElementById(_filter).value, asCategories: GetCategoryList() }, data);
                                        return JSON.stringify(data);
                                    }
                                }
                            }
                        }
                    });
                }


                //This function is used for display popup window to view left student details.
                function ShowSelectionPopup(e) {
                    //e.preventDefault();
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                    $.ajax({
                        type: "POST",
                        data: '{"aiQuestionId":"' + dataItem.Id + '"}',
                        url: "QueryDetailsUI.aspx/SetQueryString",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            window.open('../AskMe/SelectCommunicationPopup.aspx?' + msg.d, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=600').focus();
                            return false;
                        },
                        error: function (xhr, errorType, exception) {
                            var errorMessage = exception || xhr.statusText;
                            alert(errorMessage)
                        }
                    });
                }

                function detailInit(e) {
                    $("#hidQuestionId").val(e.data.Id);
                    $("<div/>").appendTo(e.detailCell).kendoGrid({
                        columns: [
                        { field: "SenderName", title: "Sender Name", sortable: false, template: '#=SetCommunicationFontState(IsSubmitted, SenderName)#' },
                        { field: "Date", title: "Date", width: "150px", format: "{0:dd-MMM-yyyy hh:mm tt}" },
                        {
                            command: [
                                                        { text: "Edit", name: "Edit", click: ShowPopup },
                                                        { text: "Delete", name: "Delete", click: DeleteRecord },
                            ], title: "Actions", width:"150px"
                        }
                        ],
                        pageable: { info: true, buttonCount: 5 },
                        filterable: false,
                        sortable: { mode: "single", allowUnsort: false },
                        editable: false,
                        selectable: "single row",
                        dataBound: onDataBound,
                        dataSource: {
                            serverPaging: true,
                            serverSorting: true,
                            serverFiltering: true,
                            pageSize: 10,
                            schema: {
                                data: "d.Data",
                                total: "d.Total",
                                model: { fields: { Date: { type: "date"} }
                                }
                            },
                            batch: true,
                            transport: {
                                read: {
                                    url: "QueryDetailsUI.aspx/GetAllQuestionCommunications",
                                    contentType: "application/json; charset=utf-8",
                                    type: "POST",
                                    complete: function () {
                                        showHidePager();
                                    }
                                },
                                parameterMap: function (data, operation) {
                                    if (data.models) {
                                        return JSON.stringify({ products: data.models });
                                    } else if (operation == "read") {
                                        data = $.extend({ sort: null, filter: null }, data);
                                        data = $.extend({ aiSchoolId: _schoolId, aiAcademicYearId: _academicYearId, aiQuestionId: e.data.Id, aiLoginUserId: _loginUserId }, data);
                                        return JSON.stringify(data);
                                    }
                                }
                            }
                        }
                    });
                }

                function SetButtonState(e) {                

                    var grid = this;
                    grid.tbody.find('>tr').each(function () {
                        var dataItem = grid.dataItem(this);
                        var replyButton = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']").find(".k-grid-Reply");
                        var publishButton = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']").find(".k-grid-Publish");
                        var SetOwner = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']").find(".k-grid-SetOwner");
                        var viewButton = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']").find(".k-grid-View");
                        var ViewStatus = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']").find(".k-grid-view");

                        var transferToNext = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']").find(".k-grid-TransferToNext");
                        var transferToPrevious = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']").find(".k-grid-TransferToPrevious");

                        var invalidButton = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']").find(".k-grid-MarkAsInvalid");

                        publishButton.hide();                        
                        if ($get("<%=this.hisIsModerator.ClientID %>").value == 1) {
                            if (dataItem.ShowPublishButton == false) // closed status
                            {
                                //publishButton.hide();
                                publishButton.show();
                                publishButton.prop("disabled", true).addClass("k-state-disabled");
                            }
                            else {
                                if (dataItem.IsPublished == true) {
                                    replyButton.prop("disabled", true).addClass("k-state-disabled");
                                    transferToNext.prop("disabled", true).addClass("k-state-disabled");
                                    transferToPrevious.prop("disabled", true).addClass("k-state-disabled");
                                    publishButton.kendoTooltip({ content: "Un-Publish Question" });
                                    publishButton.text("Unpublish")
                                }
                                else {
                                    replyButton.prop("enable", true).removeClass("k-state-disabled");
                                    transferToNext.prop("enable", true).removeClass("k-state-disabled");
                                    transferToPrevious.prop("enable", true).removeClass("k-state-disabled");
                                    publishButton.kendoTooltip({ content: "Publish Query" });
                                    publishButton.text("Publish")
                                }

                                publishButton.show();
                                publishButton.prop("enable", true).removeClass("k-state-disabled");
                            }
                        }

                        if (dataItem.AllowReply == false) {
                            replyButton.prop("disabled", true).addClass("k-state-disabled");
                        }

                        if (dataItem.AllowForward)
                            transferToNext.prop("enable", true).removeClass("k-state-disabled");
                        else
                            transferToNext.prop("disabled", true).addClass("k-state-disabled");

                        if (dataItem.AllowBackward)
                            transferToPrevious.prop("enable", true).removeClass("k-state-disabled");
                        else
                            transferToPrevious.prop("disabled", true).addClass("k-state-disabled");

                        transferToPrevious.width(30)
                        transferToNext.width(30)

                        SetOwner.width(60)
                        replyButton.width(60)
                        viewButton.width(50)
                        invalidButton.width(50)
                        publishButton.width(75)

                        transferToNext.kendoTooltip({ content: "Submit to Next" });
                        transferToPrevious.kendoTooltip({ content: "Submit to Previous" });
                        SetOwner.kendoTooltip({ content: "Owner Assignment" });
                        replyButton.kendoTooltip({ content: "Send Reply" });
                        viewButton.kendoTooltip({ content: "View Total Communication" });
                        invalidButton.kendoTooltip({ content: "Mark As Invalid" });

                        if (dataItem.ShowOwnerButton) {
                            SetOwner.show();
                        }
                        else {
                            SetOwner.hide();
                        }
                        
                        if (dataItem.IsInvalidQuestion) {
                            transferToPrevious.prop("disabled", true).addClass("k-state-disabled");
                            transferToNext.prop("disabled", true).addClass("k-state-disabled");
                            replyButton.prop("disabled", true).addClass("k-state-disabled");
                            invalidButton.text("Valid")
                            invalidButton.kendoTooltip({ content: "Mark As Valid" });
                        }
                        else
                            invalidButton.text("Invalid")

                        invalidButton.hide();
                        if ($get("<%=this.hisIsModerator.ClientID %>").value == 1) {
                        invalidButton.show()
                            if(dataItem.ShowInvalidButton)
                             {  
                                invalidButton.prop("enable", true).removeClass("k-state-disabled");
                             }
                             else{
                                invalidButton.prop("disabled", true).addClass("k-state-disabled");
                             }   
                        }
                    })
                }

                $(document).ready(function () {
                });

                function showHidePager() {
                }

                function ShowPopup(e) {
                    e.preventDefault();
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                    var queryString = '{"aiQuestionId":"' + $("#hidQuestionId").val() + '","aiQuestionDetailsId":"' + dataItem.Id + '","aiIsReply":"' + 0 + '"}'
                    OpenPopup("NewQueryPopup.aspx", queryString, 650);
                }

                function OpenPopup(filename, queryString, popupWidth) {
                    $.ajax({
                        type: "POST",
                        data: queryString,
                        url: "QueryDetailsUI.aspx/GetQueryString",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            window.open(filename + '?' + msg.d, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=650,height=' + popupWidth).focus();
                            return false;
                        },
                        error: function (xhr, errorType, exception) {
                            var errorMessage = exception || xhr.statusText;
                            alert(errorMessage)
                        }
                    });
                }

                function MarkAsInvalid(e) {
                    e.preventDefault();
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                    var isInvalidAction = dataItem.IsInvalidQuestion ? false : true;
                    var name = (isInvalidAction ? "valid" : "invalid")
                    
                    if(window.confirm('Are you sure you want to mark this query as ' + name + '?'))
                    {
                        var queryString = '{"aiQuestionId":"' + dataItem.Id + '","aiSchoolId":"' + _schoolId + '","aiAcademicYearId":"' + _academicYearId + '","aiUserId":"' + _userId + '","abIsInvalidAction":"' + isInvalidAction + '"}'
                        
                        $.ajax({
                            type: "POST",
                            data: queryString,
                            url: "QueryDetailsUI.aspx/MarkAsInvalid",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                alert('This query is marked as invalid.')
                                return false;
                            },
                            error: function (xhr, errorType, exception) {
                                var errorMessage = exception || xhr.statusText;
                                alert(errorMessage)
                            }
                        });
                    }
                    FilterQuestionDetails();
                }

                function SendReply(e) {
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                    var queryString = '{"aiQuestionId":"' + dataItem.Id + '","aiQuestionDetailsId":"' + 0 + '","aiIsReply":"' + 1 + '"}'
                    OpenPopup("NewQueryPopup.aspx", queryString, 650);
                }

                function TransferToPrevious(e) {
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

                    if (window.confirm('Are you sure you want to submit reply?')) {
                        $.ajax({
                            type: "POST",
                            data: '{"aiSchoolId":"' + _schoolId + '","aiQuestionId":"' + dataItem.Id + '","aiUpdatedById":"' + _loginUserId + '","abIsForward":"false","aiAcademicYearId":"' + _academicYearId + '"}',
                            url: "QueryDetailsUI.aspx/AssignCommunication",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                alert("Query communication successfully transferred to student!!!");
                                $("#divQuestions").data("kendoGrid").dataSource.read();
                            },
                            error: function (xhr, errorType, exception) {
                                var errorMessage = exception || xhr.statusText;
                                alert(errorMessage)
                            }
                        });
                    }
                }

                function TransferToNext(e) {
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                    var querystring = '{"aiSchoolId":"' + _schoolId + '","aiQuestionId":"' + dataItem.Id + '","aiUpdatedById":"' + _loginUserId + '","abIsForward":"true","aiAcademicYearId":"' + _academicYearId + '"}'
                    
                    if (window.confirm('Are you sure you want to submit this query?')) {
                        $.ajax({
                            type: "POST",
                            data: querystring,
                            url: "QueryDetailsUI.aspx/AssignCommunication",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                if(msg.d == "")
                                {
                                    alert("Query communication submitted successfully!!!");
                                    $("#divQuestions").data("kendoGrid").dataSource.read();
                                }
                                else
                                    alert(msg.d)
                            },
                            error: function (xhr, errorType, exception) {
                                var errorMessage = exception || xhr.statusText;
                                alert(errorMessage)
                            }
                        });
                    }
                }

                function ShowNewPopup() {
                    var queryString = '{"aiQuestionId":"' + 0 + '","aiQuestionDetailsId":"' + 0 + '","aiIsReply":"' + 0 + '"}'
                    OpenPopup("NewQueryPopup.aspx", queryString, 650);
                }

                function ViewCommunication(e) {
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                    var queryString = '{"aiQuestionId":"' + dataItem.Id + '","aiQuestionDetailsId":"' + 0 + '","aiIsReply":"' + 0 + '"}'
                    OpenPopup("ViewQueryDetailsPopup.aspx", queryString, 610);
                }

                function DeleteRecord(e) {
                    e.preventDefault();
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

                    if (window.confirm('Are you sure you want to delete this record?')) {
                        $.ajax({
                            type: "POST",
                            data: '{"aiQuestionDetailsId":"' + dataItem.Id + '","aiUpdatedById":"' + _loginUserId + '"}',
                            url: "QueryDetailsUI.aspx/DeleteQuestionDetails",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                $("#divQuestions").data("kendoGrid").dataSource.read();
                            },
                            error: function (xhr, errorType, exception) {
                                var errorMessage = exception || xhr.statusText;
                                alert(errorMessage)
                            }
                        });
                    }
                }

                function RefreshQuestionDetails() {
                    $("#divQuestions").data("kendoGrid").dataSource.read();
                }

                function onDataBound(e) {
                    
                    var grid = this;
                    var isModerator = $get("<%=this.hisIsModerator.ClientID %>").value
                    var innergrid = grid.tbody.find('>tr');
                    grid.tbody.find('>tr').each(function () {
                        var dataItem = grid.dataItem(this);
                        var editButton = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']").find(".k-grid-Edit");
                        var deleteButton = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']").find(".k-grid-Delete");
                        
                        if (dataItem.IsSubmitted == false && isModerator == 1) {
                            grid.tbody.find("tr[data-uid='" + dataItem.uid + "']").addClass("clsUnsubmitRow");
                        }


                        if (dataItem.IsSubmitted) {
                            editButton.prop("disabled", true).addClass("k-state-disabled");
                            deleteButton.prop("disabled", true).addClass("k-state-disabled");
                        }
                        else if (dataItem.IsPublished == false && isModerator == 1 && dataItem.IsEditable) {
                            editButton.prop("enable", true).removeClass("k-state-disabled");
                            deleteButton.prop("enable", true).removeClass("k-state-disabled");
                        }
                        else if (dataItem.IsPublished || dataItem.SenderUserId != _loginUserId || dataItem.IsEditable == false) {
                            editButton.prop("disabled", true).addClass("k-state-disabled");
                            deleteButton.prop("disabled", true).addClass("k-state-disabled");
                        }

                        if (dataItem.IsInvalidQuery) {
                            editButton.prop("disabled", true).addClass("k-state-disabled");
                            deleteButton.prop("disabled", true).addClass("k-state-disabled");
                        }

                        if(dataItem.IsSubmitted)
                            deleteButton.prop("disabled", true).addClass("k-state-disabled");

                        editButton.width(60)
                        deleteButton.width(60)

                        editButton.kendoTooltip({ content: "Edit" });
                        deleteButton.kendoTooltip({ content: "Delete" });
                    })
                }

                function FilterQuestionDetails() {
                    $("#divQuestions").data("kendoGrid").dataSource.read();
                }

                function OpenAskMePage() {
                    window.open("PublishedQueriesUI.aspx", "_self");
                }
                

                function PublishCommunication(e) {

                    window.open('SelectCommunicationPopup.aspx', '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=1100, height=800').focus();
                }

                function SubmitRecord(e) {
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

                    var status = 'submit'
                    var successMessage = 'submitted'
                    if (dataItem.IsPublished == true) {
                        status = 'unsubmit'
                        successMessage = 'unsubmitted'
                    }

                    if (window.confirm('Are you sure you want to ' + status + ' this query communication?')) {
                        $.ajax({
                            type: "POST",
                            data: '{"aiSchoolId":"' + _schoolId + '","aiQuestionDetailsId":"' + dataItem.Id + '","aiUpdatedById":"' + _loginUserId + '","abIsSubmitted":"' + dataItem.IsSubmitted + '"}',
                            url: "QueryDetailsUI.aspx/SubmitCommunication",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                alert("Query communication " + successMessage + " successfully !!!");
                                $("#divQuestions").data("kendoGrid").dataSource.read();
                            },
                            error: function (xhr, errorType, exception) {
                                var errorMessage = exception || xhr.statusText;
                                alert(errorMessage)
                            }
                        });
                    }
                }

                function GetCategoryList() {
                    var k = 0
                    var categoryIds = ''
                    var chk = document.getElementById(_clientchkCategoryLst + '_' + k)
                    while (chk != null) {
                        if (chk.checked) {
                            categoryIds = categoryIds + "," + (k + 1)
                        }
                        k++;
                        chk = document.getElementById(_clientchkCategoryLst + '_' + k)
                    }

                    if (categoryIds.length > 0)
                        categoryIds = categoryIds.substring(1)

                    return categoryIds;
                }

                function ShowMessage(id) {
                    FilterQuestionDetails();
                }

                function SetOwner(e) {
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                    $.ajax({
                        type: "POST",
                        data: '{"aiQuestionId":"' + dataItem.Id + '"}',
                        url: "QueryDetailsUI.aspx/GetOwnerAssignmentQueryString",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            window.open('OwnerAssignmentPopup.aspx?' + msg.d, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=750,height=550').focus();
                            return false;
                        },
                        error: function (xhr, errorType, exception) {
                            var errorMessage = exception || xhr.statusText;
                            alert(errorMessage)
                        }
                    });
                }

                function SetFontState(state, field) {
                    var isModerator = $get("<%=this.hisIsModerator.ClientID %>").value
                    if (isModerator == 1 && state) {
                        return "<B>" + field + "</B>";
                    }
                    else
                        return field;
                }

                function SetCommunicationFontState(state, field) {
                    var isModerator = $get("<%=this.hisIsModerator.ClientID %>").value             
                    if (isModerator == 1 && state == false) {
                        return "<B>" + field + "</B>";
                    }
                    else
                        return field;
                }

                function ViewStatus(e) {           
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));                                                    
                         $.ajax({
                            type: "POST",
                            data: '{"aiSchoolId":"' + _schoolId + '","aiQuestionId":"' + dataItem.Id + '","aiAcademicYearId":"' + _academicYearId + '","aiLoginUserId":"' +_loginUserId +'"}',
                            url: "QueryDetailsUI.aspx/GetReadReceiptDetails",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {                           
                             _clientdivTemplates = "<%=this.divTemplates.ClientID %>"
                                var x, y
                                var cssstyle = $get("<%=this.divTemplates.ClientID %>").style
                                var width = 350
                                var height = 150
                                var left = parseInt((screen.width / 2) - (width / 2))
                                var top = parseInt((screen.height / 2) - (height / 2))
                                cssstyle.left = left + "px"
                                cssstyle.top = top + "px"
                                cssstyle.visibility = "visible"
                                cssstyle.display = "block"

                                $get("<%=this.lblAllRemarks.ClientID %>").innerHTML = msg.d                                                 
                            return false;
                             },               
                        });
                }

                function HidePopup()
                {
                    $get("<%=this.divTemplates.ClientID %>").style.visibility = "hidden"
                    $get("<%=this.divTemplates.ClientID %>").style.display = "none"
                    return false
                }

            </script>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
