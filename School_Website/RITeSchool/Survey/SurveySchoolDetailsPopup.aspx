<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="SurveySchoolDetailsPopup.aspx.cs" Inherits="SurveySchoolDetailsPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr>
                <td align="center">
                    <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="left" style="height: 20px" class="ClsGrayMainTitle" valign="middle">
                                <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                    <tr>
                                        <td align="left" class="MainTitleHead" style="height: 20px">
                                            <span style="font-weight: bold">School Registration</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <div id="divPopup" style="display: none;">
                                    <table>
                                        <tr>
                                            <td align="left" class="ClsBorderlight">
                                                <span class="ClsLabel">School Name : </span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtValue" runat="server" CssClass="LrgTxtBox" Style="width: 275px;"
                                                    TextMode="MultiLine"></asp:TextBox>
                                                <span class="ClsMdtStar">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClientClick="SaveSetting()" />
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" OnClientClick="ClosePopup(); return false;" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr class="Height10">
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <span class="ClsMdtStar">*</span>
                                <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td align="center">
                                            <div id="divSettings" data-role="grid" style="width: 80%; float: inherit;">
                                            </div>
                                            <asp:HiddenField ID="hidSchoolId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hidAcademicYearId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hidmiUserId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hidsurveySchoolId" runat="server" Value="0" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="upnl1" runat="server">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="ClsBtn" OnClientClick="addDetails();" />
                                                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" OnClientClick="CloseWindow(); return false;" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">

        var _schoolId = $("#" + "<%=hidSchoolId.ClientID %>").val();
        var _academicYearId = $("#" + "<%=hidAcademicYearId.ClientID %>").val();
        var _surveySchoolId = "<%=this.hidsurveySchoolId.ClientID %>"
        var _userId = $("#" + "<%=hidmiUserId.ClientID %>").val();
        var _txtValue = "<%=this.txtValue.ClientID %>"

        $(function () {
            $("#divSettings").kendoGrid({
                columns: [
                        { field: "Name", title: "Name", width: "80%" },

                         {
                             command: [
                            {
                                text: "Edit",
                                name: "Edit",
                                click: showDetails
                            },
                             {
                                 text: "Delete",
                                 name: "Delete",
                                 click: DeleteSurveySchool
                             }
                            ], title: "Action", width: "40%"

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
                    pageSize: 10,
                    schema: {
                        data: "d.Data",
                        total: "d.Total",
                        model: {
                            fields: {
                                "Id": { editable: false, type: "number" },
                                "Name": { editable: false, type: "string" }
                            }
                        }
                    },
                    batch: true,
                    transport: {
                        read: {
                            url: "SurveySchoolDetailsPopup.aspx/GetAll",
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
                                data = $.extend({ aiSchoolId: _schoolId, aiAcademicYearId: _academicYearId }, data);
                                return JSON.stringify(data);
                            } else if (operation == "delete") {
                                data = $.extend({ sort: null, filter: null }, data);
                                data = $.extend({ aiSchoolId: _schoolId, aiAcademicYearId: _academicYearId }, data);
                            }
                        }

                    }
                }
            });
        });

        ///This method is used to initialize div controls.
        function showDetails(e) {
            e.preventDefault();
            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

            $("#" + _txtValue).val(dataItem.Name)
            $get(_surveySchoolId).value = dataItem.Id

            $('#divPopup').show(); ContentWindow = $('#divPopup').kendoWindow({ title: "Edit", visible: false, modal: true, resizable: false, width: '400px' }).data("kendoWindow"); ContentWindow.open(); ContentWindow.center();
        }

        ///This method is used to initialize add controls.
        function addDetails() {
            $get(_surveySchoolId).value = 0;
            $("#" + _txtValue).val('')

            $('#divPopup').show(); ContentWindow = $('#divPopup').kendoWindow({ title: "Edit", visible: false, modal: true, resizable: false, width: '400px' }).data("kendoWindow"); ContentWindow.open(); ContentWindow.center();
        }

        ///This method is used to delete school record.
        function DeleteSurveySchool(e) {
            e.preventDefault();
            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
            if (window.confirm('Are you sure you want to delete this record?')) {
                $('#' + _surveySchoolId).val('0')
                $.ajax({
                    type: "POST",

                    data: '{"aiSchoolId":"' + _schoolId + '","aiAcademicYearId": "' + _academicYearId + '","aiServaySchoolId": "' + dataItem.Id + '","aiUserId": "' + _userId + '"}',
                    url: "SurveySchoolDetailsPopup.aspx/Delete",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        alert('Record deleted successfully!!!');
                        $("#divSettings").data("kendoGrid").dataSource.read();

                    },
                    error: function (xhr, errorType, exception) {
                        var errorMessage = exception || xhr.statusText;
                        alert(errorMessage)
                    }
                });
            }
        }

        ///This method is used to close div.
        function ClosePopup() {
            $("#divPopup").data("kendoWindow").close();
        }

        ///This method is used to populate and pass parameters to save method.
        function SaveSetting() {


            var surveySchoolId = $get(_surveySchoolId).value
            var value = $("#" + _txtValue).val();
            var result = true;

            if (value.trim() == "") {
                alert('School Name should not be blank.');
                result = false;
            }

            if (result == true) {
                $.ajax({
                    type: "POST",
                    data: '{"aiSchoolId":"' + _schoolId + '","aiAcademicYearId": "' + _academicYearId + '","SurveySchoolId":"' + surveySchoolId + '","SurveySchoolName":"' + value + '","aiUserId":"' + _userId + '"}',
                    url: "SurveySchoolDetailsPopup.aspx/Save",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d == '') {
                            if (surveySchoolId == 0)
                                alert('Record Saved Successfully!!!');
                            else
                                alert('Record Updated Successfully!!!');

                        }
                        else
                            alert('School should not be duplicate.')

                        $("#divPopup").data("kendoWindow").close();
                        $("#divSettings").data("kendoGrid").dataSource.read();

                    },
                    error: function (msg) {
                    }
                });
            }
        }

        ///This method is used to close registration schools pop up.
        function CloseWindow() {
            window.close();
            window.opener.focus();
            window.opener.RefreshSchoolCombo();
        }

    </script>
</asp:Content>
