<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="SiblingLoginUI.aspx.cs" Inherits="SiblingLoginUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <style type="text/css">
        
        .k-grid .k-button.k-grid-Login{
            margin-left:auto;
            margin-right:auto;
        }    
        
    </style>
    <div class="panel-body col-sm-10">
    <div id="divProgress" style="display:none;">
        <img src="../Images/Loading9.gif" />
        <span>Please wait..</span>
    </div>
        <div class="col-sm-1">
        </div>
        <div class="col-sm-10">
            <div class="widget-body col-sm-12 form-group">
                <div id="divSiblings" data-role="grid">
                </div>
            </div>
            <asp:HiddenField ID="hidStudentId" runat="server" Value="0" />
            <asp:HiddenField ID="hidClass" runat="server" Value="Class" />
            <asp:HiddenField ID="hidStudentName" runat="server" Value="Student Name" />
            <asp:HiddenField ID="hidRegNo" runat="server" Value="Registration No." />
            <asp:HiddenField ID="hidLogin" runat="server" Value="Login" />
            <asp:HiddenField ID="hidUserId" runat="server" Value="0" />
            <asp:HiddenField ID="hidIsSiblingLogin" runat="server" Value="N" />
        </div>
        <div class="col-sm-1">
        </div>
        <script>

            var _schoolId = "<%=miSchoolId %>";
            var _academicYearId = "<%=miAcademicYearId %>";
            var _studentId = "<%=miStudentId %>";
            var _className = "<%=this.hidClass.ClientID %>"
            var _hidStudentName = "<%=this.hidStudentName.ClientID %>"
            var _hidRegNo = "<%=this.hidRegNo.ClientID %>"
            var _hidLogin = "<%=this.hidLogin.ClientID %>"
            var _clienthidUserId = "<%=this.hidUserId.ClientID %>"
            var _clienthidIsSiblingLogin = "<%=this.hidIsSiblingLogin.ClientID %>"

            $(function () {
                $("#divSiblings").kendoGrid({
                    columns: [
                        { field: "RegNo", title: $get(_hidRegNo).value, width: "20%" },
                        { field: "StudentName", title: $get(_hidStudentName).value, width: "40%" },
                        { field: "ClassName", title: $get(_className).value, width: "20%" },
                        {
                            command: [
                            {
                                text: $get(_hidLogin).value,
                                name: "Login",
                                click: showDetails                                
                            },
                            ], title: $get(_hidLogin).value,
                            width: "20%"
                        }
                        ],
                    pageable: false,
                    filterable: false,
                    sortable: false,
                    editable: false,
                    selectable: "single row",
                    dataBound: function (e) {
                        setToolTip();
                    },
                    dataSource: {
                        pageSize: 5,
                        schema: {
                            data: "d.Data",
                            total: "d.Total",
                            model: {
                                fields: {
                                    Date: { type: "date" }
                                }
                            }
                        },
                        batch: true,
                        transport: {
                            read: {
                                url: "SiblingLoginUI.aspx/GetAllSiblingDetails",
                                contentType: "application/json; charset=utf-8",
                                type: "POST",
                                complete: function () {
//                                    var grid = $("#divSiblings").data("kendoGrid");
//                                    var count = grid.dataSource.total();
//                                    if(count == 1)
//                                        window.open("../Common/ControlPanel.aspx", "_self")
                                }
                            },
                            parameterMap: function (data, operation) {
                                if (data.models) {
                                    return JSON.stringify({ products: data.models });
                                } else if (operation == "read") {
                                    data = $.extend({ sort: null, filter: null }, data);
                                    data = $.extend({ aiSchoolId: _schoolId, aiAcademicYearId: _academicYearId, aistudentId: _studentId, aiUserId: $('#' + _clienthidUserId).val(), asIsFromSiblingScreen: $('#' + _clienthidIsSiblingLogin).val() }, data);
                                    return JSON.stringify(data);
                                }
                            }
                        }
                    }
                });
            });

            $(document).ready(function () {
            });

            // this emthod is used to disable buttons.
            function DisableButtons(val) {
                if (val == true) {
                    $(".k-grid-Login").attr("disabled", "disabled");
                }
                else {
                    $(".k-grid-Login").removeAttr("disabled", "disabled");
                }
            }

            // This method is used to navigate to dashboard.
            function NavigatetoControlPanel() {
                window.open("ScreensUI.aspx", "_self");
            }

            // This function is used to set tooltip to kendoButtons.
            function setToolTip() {
                if (!($('.k-grid-Details').data('kendoTooltip'))) {
                    //$('.k-grid-Details').kendoTooltip({ content: "Details" });
                }
            }

            function showDetails(e) {
                e.preventDefault();
                $('#divProgress').show();
                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                LoginToUser(dataItem.UserName)
            }

            function LoginToUser(userName) {                
                DisableButtons(true);
                $.ajax({
                    type: "POST",
                    data: '{"aiSchoolId":"' + _schoolId + '","asUserName": "' + userName + '","aiAcademicYearId":"' + _academicYearId + '","aistudentId":"' + _studentId + '","aiUserId":"' + $('#' + _clienthidUserId).val() + '","asIsFromSiblingScreen":"' + $('#' + _clienthidIsSiblingLogin).val() + '"}',
                    url: "SiblingLoginUI.aspx/LoginToSelectedStudent",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        DisableButtons(false);
                        var data = msg.d.split(',')
                        if (data[0] == 0) {
                            window.open(data[1], "_self");                           
                        }
                        else
                            alert('Selected sibling Is locked.')
                    },
                    error: function (msg) {
                        DisableButtons(false);
                    }
                });
            }

        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
