<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"   AutoEventWireup="true" CodeFile="QualificationsUI.aspx.cs" Inherits="QualificationsUI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="100%" align="center">
            
            <tr>
                <td align="right" style="color: #ff3333" valign="top">
                    <span class="ClsMdtStar">*</span>
                    <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="80%" align="center">
                        <tr>
                            <td colspan="2">
                                <table align="center" cellpadding="1" cellspacing="2">
                                    <tr>
                                        <td valign="top" class="ClsBorderlight">
                                            <asp:Label ID="lblBankName" runat="server" class="ClsLabel" Style="height: 16px"
                                                Text="Qualification"></asp:Label>
                                            <span class="colonPadding">:</span>
                                        </td>
                                        <td valign="top" align="left">
                                            <asp:TextBox ID="txtQualification" runat="server" CssClass="LrgTxtBox" MaxLength="45"
                                                TabIndex="1" Width="250px"></asp:TextBox>&nbsp;
                                            <asp:HiddenField ID="hidQualificationId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hidDuplicationErr" runat="server" />
                                            <asp:HiddenField ID="hidmiUserId" runat="server" />
                                            <asp:HiddenField ID="hidAcademicYearId" runat="server" />
                                            <asp:HiddenField ID="hidSchoolId" runat="server" />
                                            
                                            <span class="ClsMdtStar">*</span>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" valign="top" colspan="2">
                                            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save%>"
                                                CssClass="ClsBtn" TabIndex="2" />
                                            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel%>"
                                                CssClass="ClsBtn" TabIndex="3" CausesValidation="False" UseSubmitBehavior="false"
                                                OnClientClick="ResetControles(); return false;" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <div id="divQulificationsDetails" style="width:50%;">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <div style="width: 50%;">
                                    <table id="tblNote" runat="server" align="center" width="100%">
                                        <tr>
                                            <td align="left" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                                                <asp:Label ID="Label9" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note :"
                                                    CssClass="LblNrmlB"></asp:Label>
                                            </td>
                                            <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                <asp:Label ID="Label11" runat="server" BorderWidth="0px" CssClass="LblSmlV" 
                                                    Text="Edit and Delete buttons will be disabled if qualification is associated with any user." 
                                                    ></asp:Label>
                                            </td>
                                        </tr>
                                       
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnBack" Text="<%$ Resources:LocalizedResources, Back%>" CssClass="ClsBtn"
                                    runat="server" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            _clienthidQualificationId = "<%=this.hidQualificationId.ClientID %>"
            _clienttxtQualification = "<%=this.txtQualification.ClientID %>";
            _clientbtnSave = "<%=this.btnSave.ClientID %>";
            _clienthidmiUserId = "<%=this.hidmiUserId.ClientID %>";
            _clienthidAcademicYearId = "<%=this.hidAcademicYearId.ClientID %>";
            _clienthidSchoolId = "<%=this.hidSchoolId.ClientID %>";
          

            $(function () {
                FillQualificationDetails();
            });

            //This function is used to save qualification details
            function SaveQualification() {
                var parameter = $get(_clienttxtQualification).value.trim();
                if (parameter.trim() == "") {
                    alert('Qualification should not be blank.');
                    return false;
                }
                else {
                    var str = $('#' + _clienttxtQualification).val();
                    var qualification = str.replace("\\", "\\\\");
                    var finalqualificationstr = qualification.replace(/"/g, '\\"');
                    var inputs = '{"asName":"' + finalqualificationstr + '","aiId": "' + $('#' + _clienthidQualificationId).val() + '","aiUserId": "' + $('#' + _clienthidmiUserId).val() + '","aiAcademicYearId": "' + $('#' + _clienthidAcademicYearId).val() + '","aiSchoolId": "' + $('#' + _clienthidSchoolId).val() + '"}'
                    
                    $.ajax({
                        type: "POST",
                        data: inputs,
                        url: "QualificationsUI.aspx/Save",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            if (isEmpty(msg.d)) {
                                if ($('#' + _clienthidQualificationId).val() == "0")
                                    alert("Qualification saved successfully!!!");
                                else
                                    alert("Qualification updated successfully!!!");
                                ResetControles();
                                $("#divQulificationsDetails").data("kendoGrid").dataSource.read();
                            }
                            else
                                alert('Qualification should not be duplicate.');


                        },
                        error: function (xhr, errorType, exception) {
                            var errorMessage = exception || xhr.statusText;
                            alert(errorMessage)
                            return false;
}
                    });
                }
            }

            //This function is used to manage on data bound event.
            function OnDataBound(e) {
                setToolTip()
                var grid = this;
                grid.tbody.find('>tr').each(function () {

                    var dataItem = grid.dataItem(this);
                    var editButton = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']").find(".k-grid-Edit");
                    var deleteButton = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']").find(".k-grid-Delete");

                    if (dataItem.IsUsedByTeacher == null || dataItem.IsUsedByTeacher == '') {
                        //editButton.prop("enable", true).removeClass("k-state-disabled");
                        deleteButton.prop("enable", true).removeClass("k-state-disabled");
                    }
                    else {
                        //editButton.prop("disabled", true).addClass("k-state-disabled");
                        deleteButton.prop("disabled", true).addClass("k-state-disabled");
                    }
                })
            }

            //This function is used for Edit qualification details.
            function EditQualification(e) {
                e.preventDefault();
                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                $('#' + _clientbtnSave).val('Update');
                $('#' + _clientbtnSave).val('Update');
                $('#' + _clienttxtQualification).val(dataItem.Qualification)
                $('#' + _clienthidQualificationId).val(dataItem.QualificationId)
            }

            //This function is used to reset controls.
            function ResetControles() {
                $('#' + _clienttxtQualification).val('')
                $('#' + _clienthidQualificationId).val('0')
                $('#' + _clientbtnSave).val('Save');
            }

            //This function is used for delete qualification details.
            function DeleteQualification(e) {

                e.preventDefault();
                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                if (window.confirm('Are you sure you want to delete this record?')) {
                    $('#' + _clienthidQualificationId).val('0')
                    $.ajax({
                        type: "POST",
                        data: '{"aiQualificationId":"' + dataItem.QualificationId + '","aiAcademicYearId": "' + $('#' + _clienthidAcademicYearId).val() + '","aiUserId": "' + $('#' + _clienthidmiUserId).val() + '"}',
                        url: "QualificationsUI.aspx/Delete",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            alert('Qualification deleted successfully!!!');
                            $('#' + _clienttxtQualification).val('')
                            $('#' + _clientbtnSave).val('Save');
                            $("#divQulificationsDetails").data("kendoGrid").dataSource.read();
                           
                        },
                        error: function (xhr, errorType, exception) {
                            var errorMessage = exception || xhr.statusText;
                            alert(errorMessage)
                        }
                    });
                }
            }

            // This function is used to set tooltip to kendoButtons.
            function setToolTip() {
                if (!($('.k-grid-Edit').data('kendoTooltip')))
                    $('.k-grid-Edit').kendoTooltip({ content: "Edit" });

                if (!($('.k-grid-Delete').data('kendoTooltip')))
                    $('.k-grid-Delete').kendoTooltip({ content: "Delete" });

            }

            //This function fills qualification details into kendo grid.
            function FillQualificationDetails() {
                var questionGrid = $("#divQulificationsDetails").kendoGrid({
                    columns: [

                        { field: "Qualification", title: "Qualifications" },

                        {
                            command:
                            [
                                { text: "Edit", name: "Edit", click: EditQualification, Tooltip: 'Edit' },
                                { text: "Delete", name: "Delete", click: DeleteQualification }
                            ], title: "Actions", width: "175px"
                        }
                        ],
                    pageable: { info: true, buttonCount: 5 },
                    filterable: false,
                    sortable: { mode: "single", allowUnsort: false },
                    editable: false,
                    selectable: "single row",
                    dataBound: OnDataBound,
                    dataSource: {
                        
                        serverSorting: false,
                        serverFiltering: false,
                        sort: { field: "Qualification", dir: "asc" },
                        pageSize: 20,
                        schema: {
                            data: "d.Data",
                            total: "d.Total"

                        },
                        batch: true,
                        transport: {
                            read: {
                                url: "QualificationsUI.aspx/GetAll",
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

                                    data = $.extend({}, data);
                                    return JSON.stringify(data);
                                }
                            }
                        }
                    }
                });
            }            
        </script>
    </div>
</asp:Content>
