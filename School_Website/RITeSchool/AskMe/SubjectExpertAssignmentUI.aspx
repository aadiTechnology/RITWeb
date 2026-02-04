<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
AutoEventWireup="true" CodeFile="SubjectExpertAssignmentUI.aspx.cs" Inherits="SubjectExpertAssignmentUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" Runat="Server">
    <div class="MainBodyDiv">
<style>
    .checkbox
    {
    }
</style>
        <table width="100%">
            <tr>
                <td align="center">
                    <table width="80%">
                        <tr style="height:20px;">
                            <td>
                            </td>
                        </tr>
                        <tr align="center">
                            <td id="tdMessage" runat="server" align="center">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label><br />
                                    </ContentTemplate>
                                    <Triggers>                                        
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <table>
                                    <tr>
                                        <td align="center" width="155px;" class="ClsBorderlight">
                                            <asp:Label ID="lblselectuserrole" runat="server" Text="Subject"
                                                CssClass="ClsLabel"></asp:Label>
                                            <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="cmbSubjects" runat="server" CssClass="MidCombo" 
                                                OnChange="LoadData(); return false;">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td align="right" colspan="3">
                                <asp:HiddenField ID="hidSchoolId" runat="server" />
                                <asp:HiddenField ID="hidTeacherId" runat="server" />
                                <asp:HiddenField ID="hidAcademicYearId" runat="server" />                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <div id="divAssignSubjectExpert" data-role="grid" style="width: 50%; float: inherit; text-align: center;
                                   margin-top: 10px;"></div>
                            </td>
                        </tr>  
                        <tr>
                                <td colspan="3" align="center">
                                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Back %>"
                                     CssClass="ClsBtn" BorderWidth="1px" CausesValidation="False" UseSubmitBehavior="false" OnClientClick="PostbackPage(); return false;" />
                                    <asp:Button ID="btnSave" runat="server" 
                                        OnClientClick="SaveTeachers(); return false;" Text="Save" CssClass="ClsBtn" />
                                    <asp:HiddenField ID="hidPostbackURL" runat="server" Value="" />
                                </td>
                         </tr>                                         
                    </table>
                </td>
            </tr>
        </table>
        

        <script type="text/javascript">
            var _SubjectId = "<%=this.cmbSubjects.ClientID %>";
            var _SchoolId = "<%=hidSchoolId.ClientID %>"
            var _TeacherId = "<%=hidTeacherId.ClientID %>"
            var _clientbtnCancelId = "<%=this.btnCancel.ClientID %>"
            var _btnSave = "<%=btnSave.ClientID %>"
            var _AcademicYearId = "<%=hidAcademicYearId.ClientID %>"

//            function DisableButtons() {
//                __doPostBack(document.getElementById(_clientbtnCancelId).name, '')
//            }

            $(function () {
                FillSubjectTeacherDetails();
            });

            function FillSubjectTeacherDetails() {
                var questionGrid = $("#divAssignSubjectExpert").kendoGrid({
                    columns: [
                          { field: "UserName", title: "Teacher Name", attributes: { style: "text-align:left;"} },
                          {
                            field: "IsAssignExpert",
                            title: "Is Subject Expert?",
                            type: "boolean",
                            template: '<input id="checkAll" type="checkbox" #= IsAssignExpert ? checked="checked" : "" # ></input>',
                            width:"130px"
                        }
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
                                "UserId": { editable: false, type: "number" },
                                "UserName": { editable: false, type: "string" },
                                "IsExpert": { editable: false, type: "bit" }
                            }
                        }
                        },
                        batch: true,
                        transport: {
                            read: {
                                url: "SubjectExpertAssignmentUI.aspx/GetSubjectTeachers",
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
                                    data = $.extend({ aiSchoolId: $get(_SchoolId).value,aiAcademicYearId: $get(_AcademicYearId).value, aiSubjectId: parseInt(document.getElementById(_SubjectId).value) }, data);                                    
                                    return JSON.stringify(data);
                                }
                            }
                        }
                    }
                });
            }

            function UpdateCheckbox() {
                $('#divAssignSubjectExpert input[type=checkbox][id!=checkAll]').click(function () {
                    var numChkBoxes = $('#divAssignSubjectExpert input[type=checkbox][id!=checkAll]').length;
                    var numChkBoxesChecked = $('#divAssignSubjectExpert input[type=checkbox][checked][id!=checkAll]').length;
                    if (numChkBoxes == numChkBoxesChecked && numChkBoxes > 0) {
                        $('#checkAll').attr('checked', true);
                    }
                    else {
                        $('#checkAll').attr('checked', false);
                    }
                });
            }

            function SaveTeachers() {
                var grid = $("#divAssignSubjectExpert").data("kendoGrid");
                var TeacherIds;
                var Count = 0;
                grid.tbody.find("input:checked").closest("tr").each(function (index) {
                    grid.select($(this));
                    var dataItem = grid.dataItem($(this));
                    if (TeacherIds == null || TeacherIds == "")
                        TeacherIds = dataItem.UserId;
                    else
                        TeacherIds = TeacherIds + ',' + dataItem.UserId;
                    Count = Count + 1;
                });
                $get(_TeacherId).value = TeacherIds;
                if (Count <= 0) {
                    alert('Please select at least one teacher.');
                }
                else {
                    $.ajax({
                        type: "POST",
                        data: '{"aiSchoolId":"' + $get(_SchoolId).value + '","aiAcademicYearId":"' + $get(_AcademicYearId).value + '","asTeacherId":"' + $get(_TeacherId).value + '","aiSubjectId":"' + parseInt(document.getElementById(_SubjectId).value) +'"}',
                        url: "SubjectExpertAssignmentUI.aspx/SaveExpert",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                                if (msg.d == "") {
                                   alert('Subject Expert saved successfully!!!');                            
                                   return false;
                                }
                                else
                                    alert(msg.d);                            
                        },                        
                    });
                }
            }

            //This method is used to load grid.
            function LoadData() {
                $("#divAssignSubjectExpert").data("kendoGrid").dataSource.read();
                var grid = $("#divAssignSubjectExpert").data("kendoGrid");
                grid.dataSource.page(1);
            }          
               
            function PostbackPage()
            {
                querystring = "<%=this.hidPostbackURL.ClientID %>"
                window.open($('#'+querystring).val(),'_self');
            }

        </script>  
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" Runat="Server">
</asp:Content>

