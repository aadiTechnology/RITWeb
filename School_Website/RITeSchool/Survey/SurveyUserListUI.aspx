<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="SurveyUserListUI.aspx.cs" Inherits="SurveyUserListUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="98%" align="center">
            <tr>
                <td align="center">
                    <table width="100%">
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td align="left" width="100px" class="ClsBorderlight">
                                            <span class="ClsLabel">Survey :</span>
                                        </td>
                                        <td align="left" width="275px">
                                            <asp:DropDownList ID="cmbSurvey" runat="server" CssClass="ExLrgCombo" Style="width: 250px">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" width="100px" class="ClsBorderlight">
                                            <span class="ClsLabel">User Role :</span>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="cmbUserRole" runat="server" CssClass="LrgCombo">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" width="100px" class="ClsBorderlight">
                                            <span class="ClsLabel">Name :</span>
                                        </td>
                                        <td align="left" colspan="2">
                                            <asp:TextBox ID="txtSearch" runat="server" Style="width: 99%" CssClass="ExLrgTxtBoxRA"
                                                MaxLength="100"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            <asp:Button ID="btnShow" runat="server" Text="Search" CssClass="ClsBtn" Style="margin-left: 5px" OnClientClick="LoadData(); return false;" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <div id="divUsers" style="width: 70%">
                                </div>
                                <asp:HiddenField ID="hidSchoolId" runat="server" Value="0" />
                                <asp:HiddenField ID="hidAcademicYearId" runat="server" Value="0" />
                                <asp:HiddenField ID="hidSurvey" runat="server" Value="0" />
                                <asp:HiddenField ID="hidUserRole" runat="server" Value="0" />
                                <asp:HiddenField ID="hidFilter" runat="server" Value="" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            _clientcmbSurvey = "<%=this.cmbSurvey.ClientID %>"
            _clientcmbUserRole = "<%=this.cmbUserRole.ClientID %>"
            _clienthidSchoolId = "<%=this.hidSchoolId.ClientID %>"
            _clienthidAcademicYearId = "<%=this.hidAcademicYearId.ClientID %>"
            _clienttxtSearch = "<%=this.txtSearch.ClientID %>"
            _clienthidSurvey = "<%=this.hidSurvey.ClientID %>"
            _clienthidUserRole = "<%=this.hidUserRole.ClientID %>"
            _clienthidFilter = "<%=this.hidFilter.ClientID %>"

            $(document).ready(function () {
                $('#' + _clientcmbSurvey).kendoDropDownList();
                $('#' + _clientcmbUserRole).kendoDropDownList();
                FillUserDetails();
            }
            );

            function FillUserDetails() { 
                var questionGrid = $("#divUsers").kendoGrid({
                    columns: [

                        { field: "ClassName", title: "Class", width: "100px" },
                        { field: "RegNo", title: "Enrollment Number", width: "150px" },
                        { field: "UserName", title: "Name" },
                        {
                            command:
                            [
                                { text: "View", name: "View", click: ViewUserSurveyDetails }
                            ], title: "Action", width: "100px"
                        }
                        ],
                    pageable: { info: true, buttonCount: 5 },
                    filterable: false,
                    sortable: false,
                    editable: false,
                    selectable: "single row",
                    dataBound: OnDataBound,
                    dataSource: {
                        serverPaging: true,
                        pageSize: 20,
                        schema: {
                            data: "d.Data",
                            total: "d.Total"
                        },
                        batch: true,
                        transport: {
                            read: {
                                url: "SurveyUserListUI.aspx/GetAllUsers",
                                contentType: "application/json; charset=utf-8",
                                type: "POST",
                                complete: function () {
                                }
                            },
                            parameterMap: function (data, operation) {
                                if (data.models)
                                    return JSON.stringify({ products: data.models });
                                else if (operation == "read") {
                                    data = $.extend({ sort: null, filter: null }, data);
                                    data = $.extend({ aiSchoolId: $('#' + _clienthidSchoolId).val(), aiAcademicYearId: $('#' + _clienthidAcademicYearId).val(), aiSurveyId: $('#' + _clientcmbSurvey).val(), aiUserRoleId: $('#' + _clientcmbUserRole).val(), asFilter: $('#' + _clienttxtSearch).val() }, data);
                                    return JSON.stringify(data);
                                }
                            }
                        }
                    }
                });
            }

            function OnDataBound(e) {

                var grid = this;
                grid.tbody.find('>tr').each(function () {
                    var dataItem = grid.dataItem(this);
                    var ViewButton = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']").find(".k-grid-View");

                    if (dataItem.IsSubmitted == false)
                        ViewButton.text("Add")
                    else
                        ViewButton.text("View")

                    if (dataItem.AllowSubmission == true) {
                        ViewButton.prop("enable", true).removeClass("k-state-disabled");
                    }
                    else {
                        if (dataItem.IsSubmitted == false)
                            ViewButton.prop("disabled", true).addClass("k-state-disabled");
                        else
                            ViewButton.prop("enable", true).removeClass("k-state-disabled");
                    }
                })
            }

            function ViewUserSurveyDetails(e) {
                e.preventDefault();
                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

                $.ajax({
                    type: "POST",
                    data: '{"aiSurveyId":"' + $('#' + _clienthidSurvey).val() + '","aiUserId": "' + dataItem.UserId + '","asFilter":"' + $('#' + _clienthidFilter).val() + '","aiUserRoleId":"' + $('#' + _clienthidUserRole).val() + '"}',
                    url: "SurveyUserListUI.aspx/ReadQuerystring",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        window.open(msg.d, "_self");
                    },
                    error: function (msg) {
                
                    }
                });
            }

            function LoadData() {
                var surveyId = $('#' + _clientcmbSurvey).val()
                var userRoleId =  $('#' + _clientcmbUserRole).val()
                var filter = $('#' + _clienttxtSearch).val()

                var isSurveySelected = true
                if (surveyId == 0)
                    isSurveySelected = false;

                var isUserRoleSelected = true
                if (userRoleId == 0)
                    isUserRoleSelected = false;

                if (!isSurveySelected && !isUserRoleSelected) {
                    alert('Survey should be selected.\nUser Role should be selected.')
                }
                else if (!isSurveySelected) {
                    alert('Survey should be selected.')
                }
                else if (!isUserRoleSelected) {
                    alert('User Role should be selected.')
                }
                else {
                    $('#' + _clienthidSurvey).val(surveyId)
                    $('#' + _clienthidUserRole).val(userRoleId)
                    $('#' + _clienthidFilter).val(filter)

                    $("#divUsers").data("kendoGrid").dataSource.read();
                    $("#divUsers").data("kendoGrid").dataSource.page(1);
                }
            }

        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
