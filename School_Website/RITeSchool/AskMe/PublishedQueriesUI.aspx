<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PublishedQueriesUI.aspx.cs" Inherits="PublishedQueriesUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <div>
            <table width="100%">
                <tr>
                    <td align="center">
                        <table width="80%">
                            <tr>
                                <td colspan="2" align="center">
                                    <table>
                                        <tr>
                                            <td class="ClsBorderlight" width="100px;">
                                                <span class="ClsLabel">Query :</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="ExLrgTxtBox" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight">
                                                <span class="ClsLabel">Category :</span>
                                            </td>
                                            <td align="left">
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
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <div id="divNewQuery" runat="server" visible="false" class="ClsGreenBG" style="width: 90px; float: right;">
                                        <asp:LinkButton ID="LinkButton1" runat="server" Text="New Query" CssClass="SubTitle"
                                            OnClientClick="ShowNewPopup(); return false;"></asp:LinkButton>
                                    </div>
                                    <div class="ClsGreenBG" style="width: 100px; float: right;">
                                        <asp:LinkButton ID="lnkNewQuery" runat="server" Text="My Queries" CssClass="SubTitle" style="float:left"
                                            OnClientClick="ShowMyQueries(); return false;"></asp:LinkButton>
                                    </div>
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="divQuestions">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <input type="hidden" id="hidQuestionId" value="0" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <script type="text/javascript">

                var _schoolId = "<%=miSchoolId %>";
                var _academicYearId = "<%=miAcademicYearId %>";
                var _loginUserId = "<%=miUserId %>";
                var _status = 0; // closed status
                var _filter = "<%=this.txtSearch.ClientID %>"
                var _clientchkCategoryLst = "<%=this.chkCategoryLst.ClientID %>"

                $(function () {
                    FillQuestionDetails();
                });

                function FillQuestionDetails() {
                    var questionGrid = $("#divQuestions").kendoGrid({
                        columns: [
                        { field: "Title", title: "Query" },
//                        { field: "LastUpdatedDate", title: "Published Date", width: "150px", format: "{0:dd-MMM-yyyy}" },
                        {
                            command: [
                            {
                                text: "View", name: "View", click: ViewCommunication
                            }
                            ], title: "Actions", width: "100px"
                        }
                        ],
                        pageable: { info: true, buttonCount: 5 },
                        filterable: false,
                        sortable: { mode: "single", allowUnsort: false },
                        editable: false,
                        selectable: "single row",
                        dataSource: {
                            serverPaging: true,
                            serverSorting: true,
                            serverFiltering: true,
                            pageSize: 20,
                            schema: {
                                data: "d.Data",
                                total: "d.Total",
                                model: {
//                                    fields: {
//                                        LastUpdatedDate: { type: "date" }
//                                    }
                                }
                            },
                            batch: true,
                            transport: {
                                read: {
                                    url: "PublishedQueriesUI.aspx/GetAllQuestions",
                                    contentType: "application/json; charset=utf-8",
                                    type: "POST",
                                    complete: function () {
                                    }
                                },
                                parameterMap: function (data, operation) {
                                    if (data.models) {
                                        return JSON.stringify({ products: data.models });
                                    } else if (operation == "read") {
                                        data = $.extend({ sort: null, filter: null }, data);
                                        data = $.extend({ aiSchoolId: _schoolId, aiAcademicYearId: _academicYearId, aiLoginUserId: _loginUserId, aiStatusId: _status, asFilter: document.getElementById(_filter).value, asCategories: GetCategoryList() }, data);
                                        return JSON.stringify(data);
                                    }
                                }
                            }
                        }
                    });
                }

                function GetCategoryList() {
                    return "";
                }

                function ShowNewPopup() {
                    var queryString = '{"aiQuestionId":"' + 0 + '","aiQuestionDetailsId":"' + 0 + '","aiIsReply":"' + 0 + '"}'
                    
                    $.ajax({
                        type: "POST",
                        data: queryString,
                        url: "PublishedQueriesUI.aspx/GetNewQueryString",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            window.open('NewQueryPopup.aspx?' + msg.d, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=650,height=650').focus();
                            return false;
                        },
                        error: function (xhr, errorType, exception) {
                            var errorMessage = exception || xhr.statusText;
                            alert(errorMessage)
                        }
                    });
                }

                function OpenPopup(filename, queryString, popupWidth) {
                    $.ajax({
                        type: "POST",
                        data: queryString,
                        url: "PublishedQueriesUI.aspx/GetQueryString",
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

                function ViewCommunication(e) {
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                    var queryString = '{"aiQuestionId":"' + dataItem.Id + '","aiIsPublishedView":"1"}'
                    OpenPopup("ViewQueryDetailsPopup.aspx", queryString, 610);
                }

                function ShowMyQueries() {
                    window.open("QueryDetailsUI.aspx", "_self");
                }

                function FilterQuestionDetails() {
                    $("#divQuestions").data("kendoGrid").dataSource.read();
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
                    if (id == 0)
                        alert('Query saved successfully!!!\n\nPlease open "My Queries" page to see saved query.')
                }
                
            </script>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
