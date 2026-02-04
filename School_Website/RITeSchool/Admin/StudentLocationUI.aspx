<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentLocationUI.aspx.cs" Inherits="SchoolLocationUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table border="0" cellpadding="0" cellspacing="1" style="width: 70%" align="center">
        <tr>
            <td align="right" style="padding-right: 30px" valign="bottom">
                <span class="ClsMdtStar">*</span>
                <asp:Label ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False"
                    Text="Mandatory Fields"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table width="75%">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"
                                ForeColor="Blue" Style="font-weight: bold"></asp:Label>
                            <asp:Label ID="lblErrorMsg" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"
                                ForeColor="Red" Style="float: left;"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table width="100%">
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                            <table>
                                <tr>
                                    <td style="width: 155px;" class="ClsBorderlight">
                                        <asp:Label ID="lblLocation" runat="server" CssClass="ClsLabel" Text="Living Location"
                                            Height="16px"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtLocation" CssClass="MidTxtBox" runat="server"></asp:TextBox>
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                </tr>
                                <tr style="height: 10px;">
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Button ID="btnSave" CssClass="ClsBtn" runat="server" Text="<%$ Resources:LocalizedResources, Save%>" />
                                        <asp:Button ID="btnCancel" CssClass="ClsBtn" runat="server" Text="<%$ Resources:LocalizedResources, Cancel%>"
                                            CausesValidation="False" />
                                    </td>
                                </tr>
                            </table>
                            </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <div id="divLocation" data-role="grid" style="width: 50%; float: inherit; text-align: center;
                    margin-top: 10px;">
                </div>
                <asp:HiddenField ID="hidSchoolId" runat="server" />
                <asp:HiddenField ID="hidAcademicYearId" runat="server" />
                <asp:HiddenField ID="hidLocationId" runat="server" Value="0" />
                <asp:HiddenField ID="hidUserId" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <table>
                    <tr align="center">
                        <td>
                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" PostBackUrl="~/RITeSchool/Admin/schoolconfigurationcontrolpanel.aspx" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script type="text/javascript">

        var _schoolId = $("#" + "<%=hidSchoolId.ClientID %>").val();
        var _academicYearId = $("#" + "<%=hidAcademicYearId.ClientID %>").val();
        var _LocationId = "<%=this.hidLocationId.ClientID %>"
        var _loginUserId = "<%=this.hidUserId.ClientID %>";
        var _message = "<%=this.lblMessage.ClientID %>"
        var _txtLocation = "<%=this.txtLocation.ClientID %>"
        var _lblMessage = "<%=this.lblMessage.ClientID %>"
        var _btnSave = "<%=this.btnSave.ClientID %>"
        var _lblErrorMsg = "<%=this.lblErrorMsg.ClientID %>"
        var _btnCancel = "<%=this.btnCancel.ClientID %>"
        var _btnBack = "<%=this.btnBack.ClientID %>"

        $(function () {
            $("#divLocation").kendoGrid({
                columns: [
                        { field: "LocationName", title: "Location", attributes: { style: "text-align:left;"} },
                         {
                             command: [
                            { text: "Edit", name: "Edit", click: GetLocations },
                            { text: "Delete", name: "Delete", click: DeleteLocation },
                            ], title: "Action", width: "175px"
                         }
                        ],
                pageable: true,
                filterable: true,
                sortable: true,
                editable: "popup",
                selectable: "single row",
                dataBound: function (e) {
                },
                dataSource: {
                    pageSize: 20,
                    schema: {
                        data: "d.Data",
                        total: "d.Total",
                        model: {
                            fields: {
                                "Id": { editable: false, type: "number" },
                                "Location": { editable: false, type: "string" }
                            }
                        }
                    },
                    batch: true,
                    transport: {
                        read: {
                            url: "StudentLocationUI.aspx/GetAllLivingLocation",
                            contentType: "application/json; charset=utf-8",
                            type: "POST",
                            complete: function () {
                            }
                        },

                        update: {
                            url: "StudentLocationUI.aspx/SaveLocation",
                            type: 'POST',
                            dataType: "json",
                            contentType: 'application/json; charset=utf-8',
                            crossDomain: true
                        },

                        parameterMap: function (data, operation) {
                            if (data.models) {
                                return JSON.stringify({ products: data.models });
                            } else if (operation == "read") {
                                data = $.extend({ sort: null, filter: null }, data);
                                data = $.extend({ aiSchoolId: _schoolId, aiUserId: $("#" + _loginUserId).val() }, data);
                                return JSON.stringify(data);
                            }
                            else if (operation == "Edit") {
                                return {
                                    aiSchoolId: _schoolId,
                                    aiAcademicYearId: _academicYearId,
                                    asValue: data.Value
                                };
                            }
                        }
                    }
                }
            });
        });

        function DeleteLocation(e) {

            e.preventDefault();
            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

            if (window.confirm('Are you sure you want to delete this record?')) {
                DisableButtons(true);
                $.ajax({
                    type: "POST",
                    data: '{"aiLocationId":"' + dataItem.Id + '","aiUpdatedById":"' + $("#" + _loginUserId).val() + '","aiSchoolId":"' + _schoolId + '"}',
                    url: "StudentLocationUI.aspx/DeleteStudentLocation",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d == "") {
                            $("#" + _lblMessage).text("Living Location deleted successfully !!!");
                            ReadGrid();
                        }
                        else
                            $("#" + _lblErrorMsg).text(msg.d);
                        DisableButtons(false);
                    },
                    error: function (xhr, errorType, exception) {
                        var errorMessage = exception || xhr.statusText;
                        alert(errorMessage)
                        DisableButtons(false);
                    }
                });
            }
        }

        function GetLocations(e) {
            e.preventDefault();
            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

            $("#" + _btnSave).val("Update")
            $get(_LocationId).value = dataItem.Id
            $("#" + _txtLocation).val(dataItem.LocationName)
        }


        function ValidateLocation() {
            
            var LocationId = $get(_LocationId).value
            var LocationName = $("#" + _txtLocation).val();

            LocationName = LocationName.trim();

            if (LocationName == "")
                alert('Location Name should not be blank.');
            else {
                DisableButtons(true);
                $.ajax({
                    type: "POST",
                    data: '{"aiSchoolId":"' + _schoolId + '","aiId": "' + LocationId + '","aiUserId":"' + $("#" + _loginUserId).val() + '","asLocation":"' + LocationName + '"}',
                    url: "StudentLocationUI.aspx/SaveLocation",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d == "") {
                            CleareFields();
                            if (LocationId == 0) {
                                $("#" + _lblMessage).text("Living Location saved successfully!!!");
                            }
                            else {
                                $("#" + _lblMessage).text("Living Location updated successfully!!!");
                            }
                            ReadGrid();
                            DisableButtons(false);
                        }
                        else {
                            $("#" + _lblMessage).text("");
                            $("#" + _lblErrorMsg).text(msg.d);
                            DisableButtons(false);
                        }
                    },
                    error: function (msg) {
                        $("#" + _lblMessage).text("");
                        DisableButtons(false);
                    }
                });
            }
        }

        function CleareFields() {
            $("#" + _btnSave).val("Save");
            $("#" + _lblMessage).text("");
            $("#" + _lblErrorMsg).text("");
            $("input[type = text], txtLocation").val("");
            $("#" + _LocationId).val(0);
        }

        function DisableButtons(val) {
            if (val == true) {
                $("#" + _btnSave).attr("disabled", "disable");
                $("#divLocation").attr("disabled", "disable");
                $("#" + _btnCancel).attr("disabled", "disable");
                $("#" + _btnBack).attr("disabled", "disable");
            }
            else {
                $("#" + _btnSave).removeAttr("disabled");
                $("#divLocation").removeAttr("disabled");
                $("#" + _btnCancel).removeAttr("disabled");
                $("#" + _btnBack).removeAttr("disabled");
            }
        }

        function ReadGrid() {
            DisableButtons(true);
            $("#divLocation").data("kendoGrid").dataSource.read();
            DisableButtons(false);
        }
    </script>
</asp:Content>
