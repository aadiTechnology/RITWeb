<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ActivityAssignmentUI.aspx.cs" ViewStateMode="Disabled"
    Inherits="ActivityAssignmentUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <style type="text/css">
        .ob-right
        {
            text-align: center;
        }
        .check-box
        {
            font-weight: bold;
        }
    </style>
    <div class="MainBodyDiv">
        <table width="100%">
            <tr>
                <td valign="top" style="height: 10px">
                    <table width="100%">
                        <tr>
                            <td width="50%">
                                <div style="float: right;">
                                    <span class="ClsMdtStar">* Mandatory Fields </span>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="80%">
                        <tr style="height: 20px;">
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <table align="center">
                                    <tr>
                                        <td align="center">
                                            <table>
                                                <tr>
                                                    <td align="center" width="155px" class="ClsBorderlight">
                                                        <asp:Label ID="lblselectuserrole" runat="server" Text="Activity" CssClass="ClsLabel"></asp:Label>
                                                        <span class="ClsLabel colonPadding">:</span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="cmbActivity" ViewStateMode="Enabled" runat="server" CssClass="LrgCombo"
                                                            ValidationGroup="Search">
                                                        </asp:DropDownList>
                                                        <span class="ClsMdtStar">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" class="ClsBorderlight">
                                                        <asp:Label ID="lblUserRole" runat="server" Text="User Role" CssClass="ClsLabel"></asp:Label>
                                                        <span class="ClsLabel colonPadding">:</span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="cmbUserRole" runat="server" ViewStateMode="Enabled" CssClass="LrgCombo"
                                                            ValidationGroup="Search">
                                                        </asp:DropDownList>
                                                        <span class="ClsMdtStar">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="ClsBorderlight">
                                                        <span class="ClsLabel">Name : </span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtName" runat="server" CssClass="ExLrgTxtBox" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn" ValidationGroup="Search"
                                                OnClientClick="FillActivityDetails(); return false;" CausesValidation="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div id="divActivityAssignment" data-role="grid" style="width: 50%; float: inherit;
                                                        text-align: center; margin-top: 10px;">
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Back %>"
                                                CssClass="ClsBtn" BorderWidth="1px" CausesValidation="False" PostBackUrl="~/RITeSchool/Payroll/SalaryDetailsUI.aspx" />
                                            <asp:Button ID="btnSave" runat="server" OnClientClick="UpdateTeacherActivityDetails(); return false;"
                                                Text="Save" CssClass="ClsBtn" CausesValidation="false" />
                                            <asp:HiddenField ID="hidSchoolId" runat="server" Value="" />
                                            <asp:HiddenField ID="hidUpdatedById" runat="server" Value="" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        var _SchoolId = "<%=hidSchoolId.ClientID %>";
        var _UpdatedById = "<%=hidUpdatedById.ClientID %>"
        var _UserRoleId = "<%=this.cmbUserRole.ClientID %>"
        var _UserName = "<%=this.txtName.ClientID %>"
        var _ActivityId = "<%=this.cmbActivity.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>";
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>";
        _clientcmbUserRole = "<%=this.cmbUserRole.ClientID %>";
        _clientcmbActivity = "<%=this.cmbActivity.ClientID %>";

        //        function EndReqHandler(sender, args) {
        //            DisableControls(false);         
        //        }
        //        function beginRequestHandler(sender, args) {
        //            DisableControls(true);
        //        }

        //        function DisableControls(action) {
        //            if (document.getElementById(_clientbtnSave) != null)
        //                document.getElementById(_clientbtnSave).disabled = action
        //            if (document.getElementById(_clientbtnCancel) != null)
        //                document.getElementById(_clientbtnCancel).disabled = action
        //            if (document.getElementById(_clientcmbUserRole) != null)
        //                document.getElementById(_clientcmbUserRole).disabled = action
        //            if (document.getElementById(_clientcmbActivity) != null)
        //                document.getElementById(_clientcmbActivity).disabled = action
        //        }

        function FillActivityDetails() {
            var sValue = CheckComboboxValue();
            if (sValue == false) {
                var questionGrid = $("#divActivityAssignment").kendoGrid({
                    columns: [
                          { title: "<input id='checkAll', type='checkbox' onclick='SetCheckboxState(this, 0)' />",
                              field: "UserId",
                              template: "<input type=\"checkbox\" name='chkSelect' class='check-box' #= IsSaved? checked='checked' : '' #/>",
                              width: "50px",
                              attributes: { class: "ob-right" }
                          },
                          { field: "UserName", title: "Name", attributes: { style: "text-align:left;"} }
                        ],
                    pageable: {
                        change: function (e) {                            
                            SetCheckboxState(checkAll, 1);
                        }
                    },
                    filterable: false,
                    sortable: false,
                    editable: false,
                    selectable: "single row",
                    dataSource: {
                        serverPaging: false,
                        serverSorting: false,
                        serverFiltering: false,
                        pageSize: 20,
                        schema: {
                            data: "d.Data",
                            total: "d.Total",
                            model: {
                                fields: {
                                    "UserId": { editable: false, type: "number" },
                                    "UserName": { editable: false, type: "string" },
                                    "IsSaved": { editable: false, type: "bit" }
                                }
                            }
                        },
                        batch: true,
                        transport: {
                            read: {
                                url: "ActivityAssignmentUI.aspx/GetTeachersForActivityAssignment",
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
                                    data = $.extend({ aiUserRoleId: parseInt(document.getElementById(_UserRoleId).value), aiSchoolId: $get(_SchoolId).value, asUserName: document.getElementById(_UserName).value, aiActivityId: parseInt(document.getElementById(_ActivityId).value) }, data);
                                    return JSON.stringify(data);
                                }
                            }
                        }
                    }
                });
            }
        }
        function UpdateTeacherActivityDetails() {
            var asUserIds = '';
            var asUncheckedUserIds = '';
            var grid = $("#divActivityAssignment").data("kendoGrid");
            var Count = 0;

            grid.tbody.find("input:checked").closest("tr").each(function (index) {
                grid.select($(this));
                var dataItem = grid.dataItem($(this));

                if (asUserIds == null || asUserIds == "")
                    asUserIds = dataItem.UserId;
                else
                    asUserIds = asUserIds + ',' + dataItem.UserId;

                Count = Count + 1;
            });

            grid.tbody.find("input:not(:checked)").closest("tr").each(function (index) {
                grid.select($(this));
                var dataItem = grid.dataItem($(this));

                if (asUncheckedUserIds == null || asUncheckedUserIds == "")
                    asUncheckedUserIds = dataItem.UserId;
                else
                    asUncheckedUserIds = asUncheckedUserIds + ',' + dataItem.UserId;
            });

            $.ajax({
                type: "POST",
                data: '{"aiActivityId":"' + parseInt(document.getElementById(_ActivityId).value) + '","asCheckUserIds":"' + asUserIds + '","asUnCheckUserIds":"' + asUncheckedUserIds + '","aiSchoolId":"' + $get(_SchoolId).value + '","aiUpdatedById":"' + $get(_UpdatedById).value + '"}',
                url: "ActivityAssignmentUI.aspx/SaveUsersActivity   ",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    alert("Your Record Saved successfully.");
                    return false;
                },
                error: function (xhr, errorType, exception) {
                    var errorMessage = exception || xhr.statusText;
                    alert(errorMessage)
                }
            });
        }
        function SetCheckboxState(obj, sval) {
            if (sval == 0) {                    
                if (obj.checked) {                    
                    $('.check-box').attr('checked', 'checked');
                } else {                    
                    $('.check-box').removeAttr('checked');
                }
            }
            else {
                if (obj.checked) {                    
                    $(obj).removeAttr('checked');
                }
            }
        }
        function CheckComboboxValue() {
            var ActivityId = parseInt(document.getElementById(_ActivityId).value);
            var UserRoleId = parseInt(document.getElementById(_UserRoleId).value);
            if (ActivityId == 0 && UserRoleId == 0) {
                alert("Activity & User Role should be selected.")
                return true;
            }
            else if (ActivityId == 0 && UserRoleId != 0) {
                alert("Activity should be selected");
                return true;
            }
            else if (ActivityId != 0 && UserRoleId == 0) {
                alert("User Role should be selected.");
                return true;
            }
            else {
                return false;
            }
        }    
    </script>
    <%--<script language="javascript" type="text/javascript">
        $(document).ready(function () {
            AutoSearch();
        });

        function AutoSearch() {
            _slienttxtUserName = '#<%=txtName.ClientID%>';
            var SchoolId = "<%=miSchoolId %>";
            var AcademicYearId = "<%=miAcademicYearId %>"
            BindAutoCompleteEventForStaff(SchoolId, AcademicYearId, _slienttxtUserName, null, 0);
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtName.ClientID %>");
            bt = document.getElementById("<%=this.btnSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
