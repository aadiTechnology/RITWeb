<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="SelectCommunicationPopup.aspx.cs" Inherits="SelectCommunicationPopup" %>

<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td align="left">
                    <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="height: 20px" class="ClsGrayMainTitle" valign="middle">
                                <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                    <tr>
                                        <td align="left" class="MainTitleHead" style="height: 20px">
                                            <span style="font-weight: bold">Communication Details</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 30px;">
                <td align="right">
                    <div style="float: right; vertical-align: top;">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="90%">
                        <tr>
                            <td align="left" width="100px" class="clsBorderLight">
                                <asp:Label ID="Label1" class="clsLabel" runat="server">Query : </asp:Label>
                            </td>
                            <td class="ClsHilightBGB">
                                <asp:Label ID="lblMainQuestion" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                <div style="width: 90%" id="divCommunicationDetails">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center" style="padding-top: 20px;">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClientClick="SaveSelections(); return false;" />
                    <asp:Button ID="btnPublish" runat="server" Text="Publish" CssClass="ClsBtn" UseSubmitBehavior="True" OnClientClick="PublishQuery(); return false;" />
                 <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" CausesValidation="false"
                        OnClientClick="window.close()" />
                </td>
            </tr>
           
        </table>
        <asp:HiddenField runat="server" ID="hidSchoolId" />
        <asp:HiddenField runat="server" ID="hidQuestionId" />
        <asp:HiddenField runat="server" ID="hidSelectedQuestionIds" />
        <asp:HiddenField runat="server" ID="hidUserId" />
        <asp:HiddenField runat="server" ID="hidAcademicYearId" />
        <asp:HiddenField runat="server" ID="hidEnablePublishButton" Value="0" />
        <asp:HiddenField runat="server" ID="hidIsQueryPublished" Value="0" />        

        <script>
            _hidSchoolId = "<%=this.hidSchoolId.ClientID %>";
            _hidQuestionId = "<%=this.hidQuestionId.ClientID %>";
            _hidAcademicYearId = "<%=this.hidAcademicYearId.ClientID %>";
            _hidSelectedQuestionIds = "<%=this.hidSelectedQuestionIds.ClientID %>";
            _hidUserId = "<%=this.hidUserId.ClientID %>";
            _btnPublish = "<%=this.btnPublish.ClientID%>";
            _hidEnablePublishButton ="<%=this.hidEnablePublishButton.ClientID %>"
            _hidIsQueryPublished ="<%=this.hidIsQueryPublished.ClientID %>"
            _btnSave = "<%=this.btnSave.ClientID %>"
            
            //this function used to call fill communication details.
            $(function () {
                FillCommuncationDetails();
            });

            //This fucntion fills  details into kendo grid.
            function FillCommuncationDetails() {
                var questionGrid = $("#divCommunicationDetails").kendoGrid({
                    columns: [
                       
                          { field: "CommunicationDate", title: "Date", attributes: { style: "text-align:left; "}, format: "{0:dd-MMM-yyyy}",width: "75px" },
                        { field: "SenderUserName", title: "Name", attributes: { style: "text-align:left;"},width : "175px" },
                        { field: "Communication", title: "Communication", attributes: { style: "text-align:left;"},width:"300px" },
                         {
                              field: "IsDisplayCommunication",
                              title: "<input id='checkAll', type='checkbox' />",
                              type: "boolean",
                              template: '<input id="checkAll" type=\"checkbox\"  name="chkSelect" class="check-box" #= IsDisplayCommunication ? checked="checked" : "" # ></input>', width: "50px"
                          }
                        ],
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
                                    "Id": { editable: false, type: "number" },
                                    "CommunicationDate": { editable: false, type: "date" },
                                    "SenderUserName": { editable: false, type: "string" },
                                    "Communication": { editable: false, type: "string" },
                                    "MainQuestion": { editable: false, type: "string" },
                                    "IsDisplayCommunication": { editable: false, type: "bit" },
                                    "IsPublished": { editable: false, type: "bit" },
                                }
                            }
                        },
                        
                        batch: true,
                        transport: {
                            read: {
                                url: "SelectCommunicationPopup.aspx/Get",
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
                                    data = $.extend({ aiSchoolId: parseInt(document.getElementById(_hidSchoolId).value), aiAcademicYearId: parseInt(document.getElementById(_hidAcademicYearId).value), aiQuestionId: parseInt(document.getElementById(_hidQuestionId).value)}, data);
                                    return JSON.stringify(data);
                                }
                            }
                        }
                    }

                });
            }

            $(document).ready(function () {                
                $('#checkAll').click(function () {
                    if ($(this).attr('checked')) {
                        $('.check-box').attr('checked', 'checked');
                    } else {
                        $('.check-box').removeAttr('checked');
                    }
                    
                });
                
                $('#divCommunicationDetails input[type=checkbox][id!=checkAll]').click(function () {
                    var numChkBoxes = $('#divCommunicationDetails input[type=checkbox][id!=checkAll]').length;
                    var numChkBoxesChecked = $('#divCommunicationDetails input[type=checkbox][checked][id!=checkAll]').length;
                    if (numChkBoxes == numChkBoxesChecked && numChkBoxes > 0) {
                        $('#checkAll').attr('checked', true);
                    }
                    else {
                        $('#checkAll').attr('checked', false);
                    }
                });

                SetButtonState()
            });

            function SetButtonState()
            {
              if(parseInt($('#'+_hidIsQueryPublished).val()) == 1)
                {
                    $('#'+_btnSave).prop("disabled", true).addClass("k-state-disabled");
                    $('#'+_btnPublish).val("Unpublish")
                }
                else
                {
                    $('#'+_btnSave).prop("disabled", false).removeClass("k-state-disabled");
                    $('#'+_btnPublish).val("Publish")
                    }

                if($('#'+_hidEnablePublishButton).val() == "0")
                    $('#'+_btnPublish).prop("disabled", true).addClass("k-state-disabled");
                else
                    $('#'+_btnPublish).prop("disabled", false).removeClass("k-state-disabled");
            }

            //This function is used to set disablity of grid.
            function SetDisablilty(e) {
                $.map($("#divCommunicationDetails").find("input:checkbox"),
                function (item) {
                    if(parseInt($('#'+_hidIsQueryPublished).val()) == 1)
                        $(item).attr('disabled', 'disabled');
                }
                );
            }

            //This function is used to save selected communication details.
            function SaveSelections() {
                
                var grid = $("#divCommunicationDetails").data("kendoGrid");
                var QuestionIds = '';
                var Count = 0;

                grid.tbody.find("input:checked").closest("tr").each(function (index) {
                    grid.select($(this));
                    var dataItem = grid.dataItem($(this));
                    QuestionIds = QuestionIds + ', ' + dataItem.Id;

                    Count = Count + 1;
                });

                $get(_hidSelectedQuestionIds).value = QuestionIds;
                
                if(Count <= 0) {
                    alert('Please select at least one communication.');
                } else {
                    $.ajax({
                        type: "POST",
                        data: '{"aiSchoolId":"'+ parseInt(document.getElementById(_hidSchoolId).value) +'","asSelectedQuestionIds": "'+ $get(_hidSelectedQuestionIds).value +'","aiMasterQuestionId": "'+ parseInt(document.getElementById(_hidQuestionId).value) +'","aiUserId":"'+ parseInt(document.getElementById(_hidUserId).value) +'"}',
                        url: "SelectCommunicationPopup.aspx/SaveSelection",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            alert("Communication details saved successfully !!!");
                            $('#'+_btnPublish).prop("disabled", false).removeClass("k-state-disabled");
                            $("#divCommunicationDetails").data("kendoGrid").dataSource.read();
                            return false;
                        },                        
                    });
                }
            }

            function PublishQuery()
            {
            var isPublishAction = 1
            if(parseInt($('#'+_hidIsQueryPublished).val()) == 1)
                isPublishAction = 0
            
                 $.ajax({
                        type: "POST",
                        data: '{"aiSchoolId":"'+ $('#'+_hidSchoolId).val() +'","aiAcademicYearId": "'+ $('#'+_hidAcademicYearId).val() +'","aiQuestionId": "'+ $('#'+_hidQuestionId).val() +'","aiUserId":"'+ $('#'+_hidUserId).val() +'","aiIsPublish":"'+isPublishAction+'"}',
                        url: "SelectCommunicationPopup.aspx/PublishQuery",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            if(isPublishAction == 1)
                            {
                                alert("Communication details published successfully !!!");
                                $('#'+_btnPublish).val("Unpublish")
                                $('#'+_hidIsQueryPublished).val("1")
                                $('#'+_btnSave).prop("disabled", true).addClass("k-state-disabled");
                            }
                            else
                            {
                                alert("Communication details unpublished successfully !!!");
                                $('#'+_btnPublish).val("Publish")
                                $('#'+_hidIsQueryPublished).val("0")
                                $('#'+_btnSave).prop("disabled", false).removeClass("k-state-disabled");
                            }
                            $("#divCommunicationDetails").data("kendoGrid").dataSource.read();
                            return false;
                        },
                    });
            }

        </script>
    </div>
</asp:Content>
