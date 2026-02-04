<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="PrePrimaryProgressReportConfigList.aspx.cs" Inherits="PrePrimaryProgressReportConfigList" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr id="trValSummary" runat="server">
                <td style="height: 20px">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td valign="top">
                                <asp:ValidationSummary ID="ValSummaryErrMsg" CssClass="LblErrorMsg" runat="server"
                                    ShowMessageBox="False" ShowSummary="True" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2" style="color: #ff3333" valign="top">
                    <asp:Label ID="lblmandatory" runat="server" CssClass="ClsMdtStar" Text="* Mandatory Fields"
                        ForeColor="Red" EnableViewState="false"></asp:Label></td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="always" runat="server" ID="UpdatePanel1">
                        <ContentTemplate>
                            <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" 
                                EnableViewState="false"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table border="0" cellpadding="1" cellspacing="1" runat="server" id="tblStdCmb">
                        <tr id="trCombo" runat="server" align="center">
                            <td align="center" colspan="1" class="ClsOnlyBorderlght">
                                <asp:Label ID="lblStandard" runat="server" CssClass="ClsLabel" Text="Standard : "
                                    EnableViewState="false"></asp:Label>
                                <span class="ClsMdtStar" style="color: #ff0000"></span>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbStandard" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged"
                                    CssClass="SmlCombo" TabIndex="1">
                                </asp:DropDownList>
                                <span class="ClsMdtStar" style="color: #ff0000">*&nbsp;</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <table cellpadding="0" cellspacing="1" runat="server" id="tblHeading" visible="false">
                        <tr>
                            <td class="ClsPaddingR">
                                <span class="ClsLblLgnd" style="Font-Weight:bold">Standard :</span>
                            </td>
                            <td class="ClsHilightBGB">
                                <asp:Label ID="lblStandardName" runat="server"></asp:Label>&nbsp;
                            </td>
                            <td class="ClsPaddingR">
                            </td>
                            <td class="ClsPaddingR">
                                <span class="ClsLblLgnd" style="Font-Weight:bold">Development Area :</span>
                            </td>
                            <td class="ClsHilightBGB">
                                <asp:Label ID="lblHeading" runat="server" EnableViewState="False"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left" style="height: 5px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
                        ID="uPnl">
                        <ContentTemplate>
                            <div id="divGridView" runat="server" style="width: 100%;">
                                <asp:GridView CssClass="GridBorder" ID="grdHeaders" runat="server" Width="100%" AutoGenerateColumns="False"
                                    PageSize="20" DataKeyNames="Heading_Id" AllowSorting="True" 
                                    CellPadding="0" CellSpacing="1"
                                    ForeColor="#333333" GridLines="None" OnRowCreated="grdHeaders_RowCreated" OnSorting="grdHeaders_Sorting"
                                    OnRowDataBound="grdHeaders_RowDataBound" EmptyDataText="No Records Found." EmptyDataRowStyle-HorizontalAlign="Center"
                                    OnRowCommand="grdHeaders_RowCommand" TabIndex="2" >
                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Underline="False" Font-Names="Arial"
                                        Font-Size="Small"></PagerStyle>
                                    <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                        FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                    <Columns>
                                        <asp:BoundField HeaderText="Development Area" SortExpression="Heading_Text" DataField="Heading_Text">
                                            <ItemStyle Width="80%" HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                        </asp:BoundField>
                                        <asp:ButtonField ButtonType="Image" HeaderText="Edit" ImageUrl="~/RITeSchool/images/IconGrid_Edit.GIF">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:ButtonField>
                                        <asp:ButtonField ButtonType="Image" CommandName="DELETE_HEADER" HeaderText="Delete"
                                            Text="Delete" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:ButtonField>
                                        <asp:HyperLinkField DataNavigateUrlFields="Heading_Id" HeaderText="Skills" DataNavigateUrlFormatString="~/RITeSchool/Admin/PrePrimaryProgressReportConfigList.aspx?ParentHeading_Id={0}&amp;Mode=SubHeader"
                                            Text="Skills">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:HyperLinkField>
                                    </Columns>
                                    <RowStyle CssClass="ClsGridRow" />
                                    <HeaderStyle CssClass="ClsGridHeader" />
                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                </asp:GridView>
                                <asp:HiddenField ID="hidSortDirection" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hidSortExpression" runat="server"></asp:HiddenField>
								<asp:HiddenField ID="hidStandard" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hidIsConfig" runat="server" />
                                <asp:HiddenField ID="hidMode" runat="server" Value="Heading" />
                                <asp:HiddenField ID="hidHeaderId" runat="server" Value="0" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
             <tr>
                <td align="center">
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center" style="height: 20px" class="ClspaddingT">
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" BorderWidth="1px"
                        OnClick="btnBack_Click" CausesValidation="False" />
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="ClsBtn" BorderWidth="1px"
                        Enabled="false" OnClick="btnAdd_Click" />
                </td>
            </tr>
            <tr>
                <td align="center" style="height: 20px">
                    &nbsp;
                </td>
            </tr>
            <tr runat="server" id="trCopyStandard" visible="false" >
                <td align="center">
                    <table border="0" cellpadding="1" cellspacing="1" runat="server" id="Table1">
                        <tr align="center">
                            <td align="center" colspan="1" class="ClsOnlyBorderlght" >
                                <asp:Label ID="Label3" runat="server" CssClass="ClsLabel" Text="Copy Configuration To : " EnableViewState="False"></asp:Label>
                                <span class="ClsMdtStar" style="color: #ff0000"></span>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbCopyStandard" runat="server" CssClass="SmlCombo">
                                </asp:DropDownList>
                                <span class="ClsMdtStar" style="color: #ff0000">* </span>
                                <asp:CompareValidator runat="server" ID="CompareValidator1" Display="None" ControlToValidate="cmbCopyStandard"
                                    Operator="NotEqual" ValueToCompare="0" ErrorMessage="Please select standard to copy the configuration."></asp:CompareValidator>
                                <asp:Button ID="btnCopy" runat="server" Text="Copy" CssClass="ClsBtn" 
                                    BorderWidth="1px" OnClick="btnCopy_Click" CausesValidation="True" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidSchoolId" runat="server"></asp:HiddenField>
    </div>

    <script type="text/javascript" language="javascript">
        _clientGridId = "<%=this.grdHeaders.ClientID %>"
        _clientlblErrorMsgId = "<%=this.lblErrorMsg.ClientID %>"
        function ConfirmAction(sHeader) {
            var bResult = true
            if (!window.confirm("Are you sure you want to delete this " + sHeader + "?")) {
                bResult = false
            }
            return bResult
        }
        function ConfirmCopyAction() {
            document.getElementById(_clientlblErrorMsgId).style.visibility = "hidden"
            var bResult = false
            var validationResult = true
            if (typeof (Page_ClientValidate) == 'function') {
                validationResult = Page_ClientValidate("")
            }
            if (validationResult == true) {
                if (window.confirm("This action will overwrite all predefined configuration for selected standard. Are you sure you want to continue?")) {
                    bResult = true
                } 
            }
            return bResult
        }
    </script>
</asp:Content>
