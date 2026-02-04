<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="LeftStudentsDetailsUI.aspx.cs" Inherits="LeftStudentsDetailsUI" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <div>
            <style>
                .check-box
                {
                    font-weight: bold;
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
                                            <td align="center" width="155px;" class="ClsBorderlight">
                                                <asp:Label ID="lblselectuserrole" runat="server" Text="<%$ Resources:LocalizedResources, AcademicYear%>"
                                                    CssClass="ClsLabel"></asp:Label>
                                                <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="cmbAcademicYear" runat="server" CssClass="MidCombo" OnChange="LoadData(); return false;">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" width="155px;" class="ClsBorderlight">
                                                <asp:Label ID="lblStandard" runat="server" Text="Standard Name" CssClass="ClsLabel"></asp:Label>
                                                <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="cmbStandardId" runat="server" CssClass="MidCombo" OnChange="LoadData(); return false;">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight">
                                                <asp:Label ID="lblname" runat="server" Text="<%$ Resources:LocalizedResources, NameRegNo%>"
                                                    CssClass="ClsLabel"></asp:Label>
                                                <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="ExLrgTxtBox" MaxLength="100"
                                                    autocomplete="off"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:Button ID="btnSearch" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Search%>"
                                                    OnClientClick="LoadData(); return false;" ToolTip="Search" />
                                                <asp:HiddenField ID="hidSchoolId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hidAcademicYearId" runat="server" />
                                                <asp:HiddenField ID="hidStudentId" runat="server" />
                                                <asp:HiddenField ID="hidCanEdit" runat="server" />
                                                <asp:HiddenField ID="hidIsSuperAdmin" runat="server" />
                                                <asp:HiddenField ID="hidCurrentAcademicYearId" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td align="right" colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div id="divLeftStudentDetails">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">
                                    <asp:Button ID="btnSendSMSToAll" runat="server" Text="Send SMS" CssClass="ClsBtn"
                                        OnClientClick="OpenPopup(); return false;" ToolTip="Send SMS" />
                                    <asp:Button ID="btnReadmission" runat="server" Text="Readmit" CssClass="ClsBtn" OnClientClick="AlertReadmission(); return false;"
                                        ToolTip="Readmission" />
                                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back %>"
                                        CssClass="ClsBtn" />
                                    <input type="hidden" id="hidQuestionId" value="0" />
                                    <input type="hidden" id="hisIsModerator" value="0" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <script type="text/javascript">
                var _AcademicYearId = "<%=this.cmbAcademicYear.ClientID %>";
                var _StandardId = "<%=this.cmbStandardId.ClientID %>";
                var _school_Id = "<%=this.hidSchoolId.ClientID %>";
                var _academicYearId = "<%=miAcademicYearId %>";
                var _namefilter = "<%=this.txtSearch.ClientID %>"
                var _txtSMS = "<%=this.txtSMS.ClientID %>"
                var _lblcount = "<%=this.lblCount.ClientID %>"
                var _hidAcademicYearId = "<%=this.hidAcademicYearId.ClientID %>"
                var _hidStudentId = "<%=this.hidStudentId.ClientID %>"
                var _hidCanEdit = "<%=this.hidCanEdit.ClientID %>"
                var _hidCurrentAcademicYearId = "<%=this.hidCurrentAcademicYearId.ClientID %>"
                var _hidSuperAdmin = "<%=this.hidIsSuperAdmin.ClientID %>"

                $(function () {
                    FillLeftStudentDetails();
                });


                //This fucntion fills left students details into kendo grid.
                function FillLeftStudentDetails() {

                    var questionGrid = $("#divLeftStudentDetails").kendoGrid({
                        columns: [
                        { title: "<input id='checkAll', type='checkbox' />", template: "<input type=\"checkbox\" name='chkSelect' class='check-box'/>", width: "50px" },
                        { field: "YearValue", title: "Academic Year", width: "130px", align: "center" },
                        { field: "ClassName", title: "Class", sortable: false, width: "150px" },
                        { field: "RegNo", title: " Reg No.", width: "100px", sortable: false },
                        { field: "Name", title: "Name", sortable: false },
                        { field: "SchoolLeftDate", title: "Left Date", width: "100px", sortable: false, format: "{0:dd-MMM-yyyy}" },

                        {
                            command:
                            [
                                { text: "View", name: "View", click: ShowPopup }
                            ], title: "Actions", width: "100px"
                        },

                        {
                            command:
                            [
                                { text: "Send SMS", name: "SendSMS", click: ShowDiv }
                            ], title: "Send SMS", width: "85px"
                        },

                           {
                               command:
                               [
                                   { text: "Readmit", name: "Readmission", click: ShowReadmission }
                               ], title: "Readmit", width: "100px"
                           }

                        ],
                        pageable: { info: true, buttonCount: 5, change: function (e) { ClearControl(); UpdateCheckbox() } },
                        filterable: false,
                        //sortable: { mode: "single", allowUnsort: false },
                        sortable: false,
                        editable: false,
                        selectable: "single row",
                        dataBound: OnDataBound,
                        dataSource: {
                            serverPaging: true,
                            serverSorting: false,
                            serverFiltering: false,
                            pageSize: 20,
                            schema: {
                                data: "d.Data",
                                total: "d.Total"
                            },
                            batch: true,
                            transport: {
                                read: {
                                    url: "LeftStudentsDetailsUI.aspx/Get",
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

                                        data = $.extend({ aiSchoolId: $get(_school_Id).value, aiAcademicYearId: $get(_AcademicYearId).value, aiStandardId: $get(_StandardId).value, asNameFilter: document.getElementById(_namefilter).value }, data);
                                        return JSON.stringify(data);
                                    }
                                }
                            }
                        }
                    });
                }


                //This function is used for display popup window to view left student details.
                function ShowPopup(e) {
                    //e.preventDefault();
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                    $.ajax({
                        type: "POST",
                        data: '{"aiSchoolId":"' + $get(_school_Id).value + '","aiAcademicYearId":"' + dataItem.AcademicYearId + '","aiStudentId":"' + dataItem.StudentId + '","asStudentName":"' + dataItem.Name + '","asClassName":"' + dataItem.ClassName + '","asRegNo":"' + dataItem.RegNo + '","aiStandardId":"' + dataItem.StandardId + '","aiDivisionId":"' + dataItem.DivisionId + '","asLeftDate":"' + dataItem.SchoolLeftDate + '"}',
                        url: "LeftStudentsDetailsUI.aspx/GetQueryString",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            window.open('../Teacher/StudentUI.aspx?' + msg.d, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=900,height=800').focus();
                            return false;
                        },
                        error: function (xhr, errorType, exception) {
                            var errorMessage = exception || xhr.statusText;
                            alert(errorMessage)
                        }
                    });
                }

                function SendSMSToStudent() {
                    if (document.getElementById(_txtSMS).value == "") {
                        alert("SMS Text should not be blank.");
                        return false;
                    }
                    else if (document.getElementById(_txtSMS).value.length > 460) {
                        alert("SMS Text length should not be more than 460 characters.");
                        return false;
                    }
                    else {
                        $.ajax({
                            type: "POST",
                            data: '{"aiSchoolId":"' + $get(_school_Id).value + '","aiAcademicYearId":"' + $get(_hidAcademicYearId).value + '","aiStudentId":"' + $get(_hidStudentId).value + '","SMSText":"' + document.getElementById(_txtSMS).value + '","aiCurrentAcademicYearId":"' + $get(_hidCurrentAcademicYearId).value + '"}',
                            url: "LeftStudentsDetailsUI.aspx/SendSMS",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                onClose1();
                                alert("SMS sent successfully.");
                                ClearControl();
                                return false;
                            },
                            error: function (xhr, errorType, exception) {
                                var errorMessage = exception || xhr.statusText;
                                alert(errorMessage)
                            }
                        });
                    }
                }

                //This function is used for display popup window to send SMS to left student.
                function ShowDiv(e) {
                    //e.preventDefault();
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                    $get(_hidAcademicYearId).value = dataItem.AcademicYearId;
                    $get(_hidStudentId).value = dataItem.StudentId;
                    $('#divPopup').show(); ContentWindow = $('#divPopup').kendoWindow({ title: "SMS", visible: false, modal: true, resizable: false, width: '450px', close: onClose1 }).data("kendoWindow"); ContentWindow.open(); ContentWindow.center();

                }


                function Count() {

                    var v1 = document.getElementById(_txtSMS);
                    var v2 = document.getElementById(_lblcount);
                    v2.innerHTML = v1.value.length;
                }




                //This method is used to load grid.
                function LoadData() {
                    $("#divLeftStudentDetails").data("kendoGrid").dataSource.read();

                    var grid = $("#divLeftStudentDetails").data("kendoGrid");
                    grid.dataSource.page(1);

                }

                initialize();
                /*This function is used to show kendo dropdown*/
                function initialize() {
                    $("#" + "<%=cmbAcademicYear.ClientID %>").kendoDropDownList();
                    $("#" + "<%=cmbStandardId.ClientID %>").kendoDropDownList();
                }

                function OnDataBound(e) {
                    if ($get(_hidCanEdit).value == "1") {
                        $("#divLeftStudentDetails").data("kendoGrid").showColumn(6);
                    }
                    else {
                        $("#divLeftStudentDetails").data("kendoGrid").hideColumn(6);
                    }


                    if ($get(_hidSuperAdmin).value == "Y") {
                        $("#divLeftStudentDetails").data("kendoGrid").showColumn(8);
                    }
                    else {
                        $("#divLeftStudentDetails").data("kendoGrid").hideColumn(8);
                    }

                }

                $(document).ready(function () {
                    $('#checkAll').click(function () {
                        if ($(this).attr('checked')) {
                            $('.check-box').attr('checked', 'checked');
                        } else {
                            $('.check-box').removeAttr('checked');
                        }
                    });

                    $('#divLeftStudentDetails input[type=checkbox][id!=checkAll]').click(function () {
                        var numChkBoxes = $('#divLeftStudentDetails input[type=checkbox][id!=checkAll]').length;
                        var numChkBoxesChecked = $('#divLeftStudentDetails input[type=checkbox][checked][id!=checkAll]').length;
                        if (numChkBoxes == numChkBoxesChecked && numChkBoxes > 0) {
                            $('#checkAll').attr('checked', true);
                        }
                        else {
                            $('#checkAll').attr('checked', false);
                        }
                    });
                });

                function UpdateCheckbox() {
                    $('#divLeftStudentDetails input[type=checkbox][id!=checkAll]').click(function () {
                        var numChkBoxes = $('#divLeftStudentDetails input[type=checkbox][id!=checkAll]').length;
                        var numChkBoxesChecked = $('#divLeftStudentDetails input[type=checkbox][checked][id!=checkAll]').length;
                        if (numChkBoxes == numChkBoxesChecked && numChkBoxes > 0) {
                            $('#checkAll').attr('checked', true);
                        }
                        else {
                            $('#checkAll').attr('checked', false);
                        }
                    });
                }

                function OpenPopup() {
                    var grid = $("#divLeftStudentDetails").data("kendoGrid");
                    var StudentIds;
                    var Count = 0;
                    grid.tbody.find("input:checked").closest("tr").each(function (index) {
                        grid.select($(this));
                        var dataItem = grid.dataItem($(this));
                        $get(_hidAcademicYearId).value = dataItem.AcademicYearId;
                        if (StudentIds == null || StudentIds == "")
                            StudentIds = dataItem.StudentId;
                        else
                            StudentIds = StudentIds + ',' + dataItem.StudentId;
                        Count = Count + 1;
                    });
                    $get(_hidStudentId).value = StudentIds;
                    if (Count > 0) {
                        $('#divPopup').show(); ContentWindow = $('#divPopup').kendoWindow({ title: "SMS", visible: false, modal: true, resizable: false, width: '450px', close: onClose1 }).data("kendoWindow"); ContentWindow.open(); ContentWindow.center();
                    }
                    else
                        alert('Please select at least one student.');
                }

                function ClearControl() {
                    $('#checkAll').removeAttr('checked');
                    $('.check-box').removeAttr('checked');
                }

                function onClose1() {
                    ContentWindow = $('#divPopup').kendoWindow({ title: "SMS", visible: false, modal: true, resizable: false, width: '450px' }).data("kendoWindow"); ContentWindow.close(); ContentWindow.center();
                    document.getElementById(_txtSMS).value = "";
                    document.getElementById(_lblcount).innerHTML = "0";
                }


                //This function is about multiple left students Readmission.
                function AlertReadmission() {
                    var grid = $("#divLeftStudentDetails").data("kendoGrid");
                    var StudentIds;
                    var Count = 0;
                    grid.tbody.find("input:checked").closest("tr").each(function (index) {
                        grid.select($(this));
                        var dataItem = grid.dataItem($(this));
                        $get(_hidAcademicYearId).value = dataItem.AcademicYearId;
                        if (StudentIds == null || StudentIds == "")
                            StudentIds = dataItem.StudentId;
                        else
                            StudentIds = StudentIds + ',' + dataItem.StudentId;
                        Count = Count + 1;
                    });
                    $get(_hidStudentId).value = StudentIds;
                    if (Count > 0) {
                        $.ajax({
                            type: "POST",
                            data: '{"aiSchoolId":"' + $get(_school_Id).value + '","aiAcademicYearId":"' + $get(_hidAcademicYearId).value + '","aiStudentId":"' + $get(_hidStudentId).value + '"}',
                            url: "LeftStudentsDetailsUI.aspx/ReadmissionLeftStudent",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                window.alert("Readmission done successfully.");
                                LoadData();
                                ClearControl();                                
                                return false;
                            },
                            error: function (xhr, errorType, exception) {
                                var errorMessage = exception || xhr.statusText;
                                alert(errorMessage)
                            }
                        });
                    }
                    else
                        alert('Please select at least one student.');
                }

                //This function is used to display Readmission for left student.
                function ShowReadmission(e) {

                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

                    var qry = '{"aiSchoolId":"' + $get(_school_Id).value + '","aiAcademicYearId":"' + dataItem.AcademicYearId + '","aiStudentId":"' + dataItem.StudentId + '"}'
                    $.ajax({
                        type: "POST",
                        data: qry,
                        url: "LeftStudentsDetailsUI.aspx/ReadmissionLeftStudent",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            alert("Readmission done successfully.");
                            LoadData();
                            ClearControl();
                            //ReadGrid();
                            return false;
                        },
                        error: function (xhr, errorType, exception) {
                            var errorMessage = exception || xhr.statusText;
                            alert(errorMessage)
                        }
                    });
                    //                }
                }
                //This function is used for refresh the Kendogrid after Readmission process has done.
                function ReadGrid() {
                    $("#divLeftStudentDetails").data("kendoGrid").dataSource.read();
                }

                
            </script>
            <div id="divPopup" style="display: none; background-image: url(../images/BGline.gif);
                background-repeat: repeat;">
                <table align="center">
                    <tr>
                        <td>
                            <asp:TextBox ID="txtSMS" runat="server" CssClass="ExLrgTxtBox" Width="370px" Height="110px"
                                MaxLength="320" TextMode="MultiLine" OnKeyPress="Count()" OnKeyUp="Count();"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblCount" runat="server" Text="" CssClass="ClsLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnSendSMS" Text="Send SMS" CssClass="ClsBtn" runat="server" CausesValidation="false"
                                OnClientClick="SendSMSToStudent();" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
