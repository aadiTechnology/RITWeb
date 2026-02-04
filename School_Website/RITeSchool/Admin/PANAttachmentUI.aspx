<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PANAttachmentUI.aspx.cs" Inherits="PANAttachmentUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <div>
            <table width="100%">
                <tr>
                    <td align="center">
                        <table width="80%">
                            <tr>
                                <td colspan="3" align="center">
                                    <table>
                                        <tr>
                                            <td align="center" width="155px;" class="ClsBorderlight">
                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, Category%>"
                                                    CssClass="ClsLabel"></asp:Label>
                                                <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="cmbCategory" runat="server" CssClass="LrgCombo"
                                                    AutoPostBack="true" onselectedindexchanged="cmbCategory_SelectedIndexChanged">
                                                <asp:ListItem Value="1" Text = "PAN No."></asp:ListItem>
                                                <asp:ListItem Value="2" Text = "Aadhar Card No."></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" width="155px;" class="ClsBorderlight">
                                                <asp:Label ID="lblselectuserrole" runat="server" Text="<%$ Resources:LocalizedResources, UserRole%>"
                                                    CssClass="ClsLabel"></asp:Label>
                                                <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td align="left">
                                             <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                        <asp:DropDownList ID="cmbUserRole" runat="server" CssClass="LrgCombo" onchange="ShowClass()">
                                                        </asp:DropDownList>
                                                        <span class="ClsMdtStar">*</span>
                                                 </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbCategory" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr id="trClass" style="display:none;">
                                            <td align="center" width="155px;" class="ClsBorderlight">
                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, Class%>"
                                                    CssClass="ClsLabel"></asp:Label>
                                                <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="cmbClass" runat="server" CssClass="LrgCombo">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="trLeft" style="display:none;">
                                            <td class="ClsBorderlight">
                                                <asp:Label ID="Label3" runat="server" Text="Include Left Students?"
                                                    CssClass="ClsLabel"></asp:Label>
                                                <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIncludeLeft" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight">
                                                <asp:Label ID="lblshowalldetails" runat="server" Text="<%$ Resources:LocalizedResources, ShowAllDetails%>"
                                                    CssClass="ClsLabel"></asp:Label>
                                                <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkshowalldetails" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight">
                                                <asp:Label ID="lblname" runat="server" Text="<%$ Resources:LocalizedResources, Name%>"
                                                    CssClass="ClsLabel"></asp:Label>
                                                <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="ExLrgTxtBox" MaxLength="100" autocomplete="off"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:Button ID="btnSearch" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Search%>"
                                                    OnClientClick="FilterPANDetails(); return false;" ToolTip="Search" />
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
                            <tr id="trLegend" style="display:none">
                              <td colspan="3" >
                              <span class="ClsLblLgnd" style="padding-left:5px;">Legend : </span>&nbsp;
                              <span style="background-color: #F08080; height: 20px; border-color: Black; border-style: Solid;
                              border-width: 1px; width: 20px">
                              <img src="../images/spacer.gif" width="20px" height="20px" /></span>
                                 <span style="color:#066;font-weight:700;font-size:9pt;padding-left:2px;">PAN / Aadhar Card file is not uploaded</span>

                                 <span style="background-color: #AEB6BF; height: 20px; border-color: Black; border-style: Solid;
                              border-width: 1px; width: 20px">
                              <img src="../images/spacer.gif" width="20px" height="20px" /></span>
                                 <span style="color:#066;font-weight:700;font-size:9pt;padding-left:2px;">Left Students</span>

                              </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div id="divPanAttachmentDetails">
                                    </div>
                                </td>
                            </tr>
                            <tr id="trNotes" style="display:none">
                                <td>
                                    <table id="tblNote" runat="server" align="center" width="100%">
                                        <tr>
                                            <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                <asp:Label ID="Label14" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note1 :"
                                                    CssClass="LblNrmlB" Height="16px"></asp:Label>
                                            </td>
                                            <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                <asp:Label ID="Label15" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="View button will be disabled when PAN /Aadhar card attachment does not exist for user."></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">
                                    <input type="hidden" id="hidQuestionId" value="0" />
                                    <input type="hidden" id="hisIsModerator" value="0" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <script type="text/javascript">
                var _loginUserRole = "<%=this.cmbUserRole.ClientID %>";
                var _schoolId = "<%=miSchoolId %>";
                var _academicYearId = "<%=miAcademicYearId %>";
                var _namefilter = "<%=this.txtSearch.ClientID %>"
                var _showalldetails = "<%=this.chkshowalldetails.ClientID %>"
                var _chkIncludeLeft = "<%=this.chkIncludeLeft.ClientID %>"
                _clientcmbCategory = "<%=this.cmbCategory.ClientID %>"
                _clientcmbClass = "<%=this.cmbClass.ClientID %>"

                $(function () {
                    document.getElementById(_showalldetails).checked = true;
                    FillPANAttachmentDetails();

                    var grid = $("#divPanAttachmentDetails").data("kendoGrid");
                    grid.hideColumn("ClassName");
                    grid.hideColumn("RollNo");
                });

                //This fucntion fills PAN attachment details into kendo grid.
                function FillPANAttachmentDetails() {
                    var questionGrid = $("#divPanAttachmentDetails").kendoGrid({
                        columns: [

                        { field: "Name", title: "Name" },
                        { field: "ClassName", title: "Class" },
                        { field: "RollNo", title: "Roll No." },
                         { field: "PanNo", title: "PAN No.", width: "175px", sortable: false },

                        {
                            command:
                            [
                                { text: "Upload", name: "Upload", click: ShowPopup },
                                { text: "View", name: "View", click: viewpancopy }
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
                            serverPaging: true,
                            serverSorting: true,
                            serverFiltering: true,
                            sort: { field: "Name", dir: "asc" },
                            pageSize: 20,
                            schema: {
                                data: "d.Data",
                                total: "d.Total"
                            },
                            batch: true,
                            transport: {
                                read: {
                                    url: "PANAttachmentUI.aspx/GetAllPanAttachmentDetails",
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
                                        var chk = document.getElementById(_showalldetails);
                                        var left = document.getElementById(_chkIncludeLeft).checked;
                                        var showall = 0;

                                        if (chk.checked)
                                            showall = 1;
                                        data = $.extend({ aiUserRoleId: document.getElementById(_loginUserRole).value, aiSchoolId: _schoolId, aiAcademicYearId: _academicYearId, asNameFilter: document.getElementById(_namefilter).value, abShowAllDetails: showall, aiCategoryId: $('#' + _clientcmbCategory).val(), aiStdDivId: parseInt($('#' + _clientcmbClass).val()), asIncludeLeftStudents: left }, data);
                                        return JSON.stringify(data);
                                    }
                                }
                            }
                        }
                    });
                }

                //This function is used for display popup window to upload pan copy.
                function ShowPopup(e) {
                    e.preventDefault();
                    
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                    $.ajax({
                        type: "POST",
                        data: '{"aiUserId":"' + dataItem.UserId + '","aiCategoryId":"' + $('#' + _clientcmbCategory).val() + '"}',
                        url: "PANAttachmentUI.aspx/GetQueryString",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            window.open('../Admin/PANAttachmentPopup.aspx?' + msg.d, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=650,height=450').focus();
                            return false;
                        },
                        error: function (xhr, errorType, exception) {
                            var errorMessage = exception || xhr.statusText;
                            alert(errorMessage)
                        }
                    });
                }

                // This function is used to set tooltip to kendoButtons.
                function setToolTip() {
                    if (!($('.k-grid-Upload').data('kendoTooltip'))) 
                        $('.k-grid-Upload').kendoTooltip({ content: "Upload" });
                    
                    if (!($('.k-grid-View').data('kendoTooltip'))) 
                        $('.k-grid-View').kendoTooltip({ content: "View" });
                    
                }

                // This fuction is used to display attached PAN copy.
                function viewpancopy(e) {
                    e.preventDefault();
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                    window.open(dataItem.PanAttachment)
                }

                // This function is used to accept filter parameter.
                function FilterPANDetails() {

                    if (parseInt($('#' + _loginUserRole).val()) == 0)
                        alert('User Role should be selected.')
                    else {
                        var grid = $("#divPanAttachmentDetails").data("kendoGrid")
                        if (grid.dataSource.page() != 1) 
                            grid.dataSource.page(1);

                        grid.dataSource.read({ parameter: "value" });


                        $('#trLegend').show()
                        $('#trNotes').show()

                        if (parseInt($('#' + _clientcmbCategory).val()) == 1) {
                            $("#divPanAttachmentDetails thead [data-field=PanNo]").html("PAN No.")
                        }
                        else {
                            $("#divPanAttachmentDetails thead [data-field=PanNo]").html("Aadhar Card No.")
                        }

                        var grid = $("#divPanAttachmentDetails").data("kendoGrid");
                        if (parseInt($('#' + _loginUserRole).val()) != 3) {
                            grid.hideColumn("ClassName");
                            grid.hideColumn("RollNo");
                        }
                        else {
                            grid.showColumn("ClassName");
                            grid.showColumn("RollNo");
                        }
                    }

                    AutoSearch();
                }

                function OnDataBound(e) {
                    setToolTip()
                    var grid = this;
                    grid.tbody.find('>tr').each(function () {
                        var dataItem = grid.dataItem(this);

                        var currenRow = grid.table.find("tr[data-uid='" + dataItem.uid + "']");
                        var uploadButton = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']").find(".k-grid-Upload");
                        var viewButton = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']").find(".k-grid-View");

                        if (dataItem.PanAttachment == null || dataItem.PanAttachment == '') {
                            viewButton.prop("disabled", true).addClass("k-state-disabled");
                            $(currenRow).css("background-color", "#f4b2b2");                            
                        } else {
                            viewButton.prop("enable", true).removeClass("k-state-disabled");
                            $(currenRow).css("background-color", "");
                        }

                        if (dataItem.IsLeftStudent)
                            $(currenRow).css("background-color", "#AEB6BF");
                        

                    })
                }

                function ShowClass() {
                    if (parseInt($('#' + _loginUserRole).val()) == 3) {
                        $('#trClass').show(1000);
                        $('#trLeft').show(1000);
                    }
                    else {
                        $('#trClass').hide(1000)
                        $('#trLeft').hide(1000);

                        document.getElementById(_chkIncludeLeft).checked = false;
                        document.getElementById(_clientcmbClass).value = "0";
                    }
                }

            </script>

            <script>

                $(document).ready(function () {
                    AutoSearch();
                });

                function AutoSearch() {                    
                    _clienttxtSearch = '#<%=txtSearch.ClientID%>';                     
                                    
                    var SchoolId = "<%=miSchoolId %>";
                    var AcademicYearId = "<%=miAcademicYearId %>"
                    var _loginUserRole = "<%=this.cmbUserRole.ClientID %>";
                    BindAutoCompleteEventForStaff(SchoolId, AcademicYearId, _clienttxtSearch, _loginUserRole, 0)
                }

                function SearchSelectedValue(val) {
                    txt = document.getElementById("<%=this.txtSearch.ClientID %>");
                    bt = document.getElementById("<%=this.btnSearch.ClientID %>");
                    SearchResult(txt, val, bt);
                }

            </script>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
