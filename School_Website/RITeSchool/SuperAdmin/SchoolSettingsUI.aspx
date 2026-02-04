<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    AutoEventWireup="true" CodeFile="SchoolSettingsUI.aspx.cs" Inherits="SchoolSettingsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table border="0" cellpadding="0" cellspacing="1" style="width: 95%" align="center">
        <tr>
            <td align="center">
                <div id="divPopup" style="display: none">
                    <table>
                        <tr>
                            <td align="left" class="ClsBorderlight" width="150px">
                                <span class="ClsLabel">Name : </span>
                            </td>
                            <td align="left" class="ClsHilightBGB">
                                <span id="spnLabel"></span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="ClsBorderlight">
                                <span class="ClsLabel">Description : </span>
                            </td>
                            <td align="left" class="ClsHilightBGB">
                                <span id="spnDescription"></span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="ClsBorderlight">
                                <span class="ClsLabel">Possible Values : </span>
                            </td>
                            <td align="left" class="ClsHilightBGB">
                                <span id="spnPossibleValues"></span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="ClsBorderlight">
                                <span class="ClsLabel">Value : </span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtValue" runat="server" CssClass="LrgTxtBox" Style="width: 300px;
                                    height: 100px;" TextMode="MultiLine"></asp:TextBox>
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
        <tr>
            <td align="center">
                <table width="80%" class="ClsBorderBlue">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblHeader" runat="server" Text="" Style="font-weight: bold; font-size: 20px;"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="Height10">
            <td>
            </td>
        </tr>
        <tr>
            <td align="center">
                <div id="divSettings" data-role="grid" style="width: 80%; float: inherit;">
                </div>
                <asp:HiddenField ID="hidSchoolId" runat="server" Value="0" />
                <asp:HiddenField ID="hidAcademicYearId" runat="server" Value="0" />
                <asp:HiddenField ID="hidSettingId" runat="server" Value="0" />
                <asp:HiddenField ID="hiModulesId" runat="server" Value="0" />
                
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upnl1" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="ClsLabel" Style="color: Blue;
                                        font-weight: bold; float: inherit" EnableViewState="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" PostBackUrl="~/RITeSchool/SuperAdmin/ScreensUI.aspx" />
                                    <asp:Button ID="btnRefreshCache" runat="server" Text="Refresh Cache" CssClass="ClsBtn"
                                        Width="150px" OnClick="btnRefreshCache_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="0" cellspacing="1" style="width: 95%" align="center">
         <tr>
             <td align="center">
                <table width="80%" class="ClsBorderBlue">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblModule" runat="server" Text="" Style="font-weight: bold; font-size: 20px;"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
          </tr>
            <tr class="Height10">
            <td></td>
             </tr>
       
        <tr>    
        <td align="center">
                <div id="divManageModule" data-role="grid" style="width: 50%; float: inherit;">
                </div>
            </td>
        </tr>
        <tr>
                               <td align="center">
                                    <asp:Label ID="LblSavemessage" runat="server" Text="" CssClass="ClsLabel" Style="color: Blue;
                                        font-weight: bold; float: inherit" EnableViewState="false"></asp:Label>
                                </td>
                            </tr>
        <tr>
            <td align="center">
            <asp:UpdatePanel ID="updatepanelsave" runat="server">
                <ContentTemplate>
                <asp:Button ID="btnSaveModule" runat="server"  Text="Save" CssClass="ClsBtn" OnClientClick="UpdateModuleDetails();" />
                </ContentTemplate>
            </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    
    <script type="text/javascript">

        var _schoolId = $("#" + "<%=hidSchoolId.ClientID %>").val();
        var _academicYearId = $("#" + "<%=hidAcademicYearId.ClientID %>").val();
        var _settingId = "<%=this.hidSettingId.ClientID %>"
        var _message = "<%=this.lblMessage.ClientID %>"
        var _message = "<%=this.LblSavemessage.ClientID %>"
        var _ModulesId="<%=this.hiModulesId.ClientID %>"

        var _txtValue = "<%=this.txtValue.ClientID %>"

        $(function () {
            $("#divSettings").kendoGrid({
                columns: [
                        { field: "Name", title: "Name", width: "30%" },
                        { field: "Value", title: "Value", width: "30%" },
                        { field: "PossibleValues", title: "Possible Value", width: "20%", filterable: false },
                        { field: "Description", title: "Description", width: "20%", filterable: false },
                         {
                             command: [
                            {
                                text: "Edit",
                                name: "Edit",
                                click: showDetails
                            },
                            ], title: "Action",
                             width: "100px"
                         }


                // { command: ["edit"], title: "&nbsp;", width: "100px" }

                //                        {
                //                        command: [
                //                                                             {
                //                                                                 text: { edit: "", cancel: "", update: "" },
                //                                                                 name: "edit"
                //                                                             }
                //                                                            , {
                //                                                                text: "",
                //                                                                name: "destroy"
                //                                                            }
                //                                                ]
                //                                                , title: "Action",
                //                        width: "200px"
                //                    }
                        ],
                pageable: true,
                filterable: true,
                sortable: true,
                editable: "popup",
                selectable: "single row",
                dataBound: function (e) {
                    //setToolTip();
                },
                dataSource: {
                    pageSize: 20,
                    schema: {
                        data: "d.Data",
                        total: "d.Total",
                        model: {
                            fields: {
                                "Id": { editable: false, type: "number" },
                                "Name": { editable: false, type: "string" },
                                //                                "Value": { editable: true, type: "string", validation: { required: true} }
                                "Value": { editable: true, type: "string" }
                            }
                        }
                    },
                    batch: true,
                    transport: {
                        read: {
                            url: "SchoolSettingsUI.aspx/GetAllSettings",
                            contentType: "application/json; charset=utf-8",
                            type: "POST",
                            complete: function () {
                            }
                        },
                        //                        update: {
                        //                                url: "SchoolSettingsUI.aspx/UpdateSetting",
                        //                                contentType: 'application/json; charset=utf-8',
                        //                                type: 'POST'
                        //                         },

                        update: {
                            url: "SchoolSettingsUI.aspx/UpdateSetting",
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
                                data = $.extend({ aiSchoolId: _schoolId, aiAcademicYearId: _academicYearId }, data);
                                return JSON.stringify(data);
                            }
                            else if (operation == "update") {
                                //                                data = $.extend({ aiSchoolId: _schoolId, aiAcademicYearId: _academicYearId,aiId: data.Id, asValue: data.Value,asName: data.Name }, data);                                
                                //                                return JSON.stringify(data);

                                return {
                                    aiSchoolId: _schoolId,
                                    aiAcademicYearId: _academicYearId,
                                    aiId: data.Id,
                                    asValue: data.Value,
                                    asName: data.Name
                                };
                            }
                        }
                    }
                }
            });
        });

        $(function () {
            $("#divManageModule").kendoGrid({
                columns: [
                      { title: "<input id='checkAll', type='checkbox' />",
                        field:"Id",
                        template: '<input id="chkSelect" type="checkbox" #=IsActive? checked="checked" : "" # ></input>',
                       width: "50px" },
                      { field: "Name", title: "Name", width: "95%" },
                ],
                pageable: { info: true, buttonCount: 10 },
                    filterable: false,
                    sortable: false,
                    editable: false,
                    selectable: "single row",                   
                    dataSource: {
                        serverPaging: true,
                        serverSorting: false,
                        serverFiltering: false,
                        pageSize: 20,
                        schema: {
                            data: "d.Data",
                            total: "d.Total",
                        model: {
                            fields: {
                                "Name": { editable: false, type: "string" },
                                "Id": { editable: false, type: "number" },
                                "IsActive": { editable: false, type: "bit" }
                            }
                        }
                    },
                    batch: true,
                    transport: {
                        read: {
                            url: "SchoolSettingsUI.aspx/GetAllModule",
                            contentType: "application/json; charset=utf-8",
                            type: "POST",
                            complete: function () {
                            }
                        }
                        ,
                        parameterMap: function (data, operation) {
                            if (data.models) {
                                return JSON.stringify({ products: data.models });
                            } else if (operation == "read") {
                                data = $.extend({ sort: null, filter: null }, data);
                                data = $.extend({ asModuleId: _ModulesId, }, data);
                                return JSON.stringify(data);
                            }
                        }
                    }
                }
            });
        });
        $(document).ready(function () {
                    $('#checkAll').click(function () {
                        if ($(this).attr('checked')) {
                            $('.check-box').attr('checked', 'checked');
                        } else {
                            $('.check-box').removeAttr('checked');
                        }
                    });

                    $('#divManageModule input[type=checkbox][id!=checkAll]').click(function () {
                        var numChkBoxes = $('#divManageModule input[type=checkbox][id!=checkAll]').length;
                        var numChkBoxesChecked = $('#divManageModule input[type=checkbox][checked][id!=checkAll]').length;
                        if (numChkBoxes == numChkBoxesChecked && numChkBoxes > 0) {
                            $('#checkAll').attr('checked', true);
                        }
                        else {
                            $('#checkAll').attr('checked', false);
                        }
                    });
                });

        function UpdateCheckbox() {
            $('#divManageModule input[type=checkbox][id!=checkAll]').click(function () {
                var numChkBoxes = $('#divManageModule input[type=checkbox][id!=checkAll]').length;
                var numChkBoxesChecked = $('#divManageModule input[type=checkbox][checked][id!=checkAll]').length;
                if (numChkBoxes == numChkBoxesChecked && numChkBoxes > 0) {
                    $('#checkAll').attr('checked', true);
                }
                else {
                    $('#checkAll').attr('checked', false);
                }
            });
        }

        
        function UpdateModuleDetails()
        {
            var moduleIds='';
            var grid = $("#divManageModule").data("kendoGrid");
            var Count=0;
            grid.tbody.find("input:checked").closest("tr").each(function (index) {
                grid.select($(this));
                var dataItem = grid.dataItem($(this));

                if (moduleIds == null || moduleIds == "")
                    moduleIds = dataItem.Id;
                else
                    moduleIds = moduleIds + ',' + dataItem.Id;

                Count = Count + 1;
            });
            
                if (Count <= 0) {
                    alert('Please select at least one Module.');
                }
                else {
            $.ajax({
                type: "POST",
                data: '{"asModuleId":"' + moduleIds+ '"}',
                url: "SchoolSettingsUI.aspx/SaveModuleDetails",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    alert("Module assignment saved successfully !!!");
                    return false;
                },
                error: function (xhr, errorType, exception) {
                    var errorMessage = exception || xhr.statusText;
                    alert(errorMessage)
                }
            });
        }
        }

        function showDetails(e) {
            e.preventDefault();
            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

            $("#spnLabel").html(dataItem.Name)
            $("#" + _txtValue).val(dataItem.Value)
            $get(_settingId).value = dataItem.Id
            $("#spnPossibleValues").html(dataItem.PossibleValues)
            $("#spnDescription").html(dataItem.Description)

            $get(_message).innerHTML = "";

            $('#divPopup').show(); ContentWindow = $('#divPopup').kendoWindow({ title: "Edit", visible: false, modal: true, resizable: false, width: '500px' }).data("kendoWindow"); ContentWindow.open(); ContentWindow.center();
        }

        function ClosePopup() {
            $("#divPopup").data("kendoWindow").close();
        }

        function SaveSetting() {
            $get(_message).innerHTML = "";

            var settingId = $get(_settingId).value
            var name = $("#spnLabel").html();
            var value = $("#" + _txtValue).val();
            var possibleValues = $("#spnPossibleValues").html();
            var description = $("#spnDescription").html();

            value = value.trim()
            possibleValues = possibleValues.trim()

            if (value.length > 500) {
                alert("Length of Value should not be greater than 500 characters.")
            }
            else {
                var found = false;
                var lst = new Array();
                if (possibleValues != "")
                    lst = possibleValues.split(',')
                else
                    found = true;

                if (lst.length > 1) {
                    for (var index = 0; index < lst.length; index++) {
                        if (lst[index].trim().toLowerCase() == value.toLowerCase()) {
                            found = true;
                            break;
                        }
                    }

                    if (!found) {
                        alert('Given value should be from possible values list.')

                    }
                }
                else if (lst.length == 1 && possibleValues.split('-').length > 1) {

                    var lst = new Array();
                    if (possibleValues != "")
                        lst = possibleValues.split('-')
                    else
                        found = true;

                    if (lst.length > 1) {
                        if((parseInt(lst[0]) <= parseInt(value) && parseInt(lst[1]) >= parseInt(value)) || (parseInt(lst[0]) >=  parseInt(value) && parseInt(lst[1]) <= parseInt(value)))
                            found = true;
                        else
                            alert('Given value should be from given range.')
                    }
                }
                else
                    alert('Possible values are not in proper format.')

                if (found) {
                    var result = true;
                    if (value.trim() == "") {
                        result = confirm('Are you sure you want to save blank value?');
                    }

                    if (result == true) {
                        $.ajax({
                            type: "POST",
                            data: '{"aiSchoolId":"' + _schoolId + '","aiId": "' + settingId + '","aiAcademicYearId":"' + _academicYearId + '","asName":"' + name + '","asValue":"' + value + '"}',
                            url: "SchoolSettingsUI.aspx/SaveSetting",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                //DisableButtons(false);

                                alert('Record saved successfully.\n\nPlease refresh cache before leaving this page.')
                                $("#divPopup").data("kendoWindow").close();
                                $("#divSettings").data("kendoGrid").dataSource.read();

                            },
                            error: function (msg) {
                                // DisableButtons(false);
                            }
                        });
                    }
                }
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
