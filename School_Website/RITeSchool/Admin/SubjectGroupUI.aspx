<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="SubjectGroupUI.aspx.cs" Inherits="SubjectGroupUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div style="padding-left: 10px;">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr >
                <td align="left">
                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td style="width: 50%">
                            </td>
                            <td align="right" style="font-size: 9pt; width: 50%">
                                <span class="ClsMdtStar">*</span>
                                <asp:Label ID="Label36" runat="server" CssClass="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields %>" EnableViewState="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary CssClass="NewClsLabel" ValidationGroup="validateAdd" ID="validSummaryAdd"
                        runat="server" ShowMessageBox="False" ShowSummary="True" />
                    <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                        ID="UpdatePanel1">
                        <ContentTemplate>
                            <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="LblErrorMsg"> </asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr align="center">
                <td style="padding-top: 5px" align="center">
                    <table cellpadding="0" cellspacing="0" style="width: 98%">
                        <tr>
                            <td style="width: 42%" valign="top">
                                <table width="99%" cellpadding="2" cellspacing="2">
                                    <tr>
                                        <td align="left" class="ClsBorderlight" width="150px">
                                            <span class="ClsLabel">
                                                <asp:Label ID="lblClass" runat="server" Text="<%$ Resources:LocalizedResources, Class %>"></asp:Label>
                                                <span id="Span3" class="colonPadding">:</span>
                                                 </span>
                                        </td>
                                        <td align="left" class="ClsHilightBG" width="250px">
                                            <asp:Label ID="lblClassName" runat="server" CssClass="LblNrmlB" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 55%" class="ClsBorderlight">
                                            <span class="ClsLabel">
                                                <asp:Label ID="lblSelectParentSubject" runat="server" Text="<%$ Resources:LocalizedResources, SelectParentSubjectForGroup %>"></asp:Label>
                                                <span id="Span1" class="colonPadding">:</span></span>
                                        </td>
                                        <td style="width: 45%">
                                            <asp:UpdatePanel ID="upnlParentSubject" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="cmbParentSubjects" runat="server" CssClass="MidCombo">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">* </span>
                                                    <asp:CompareValidator ID="cmpValidParentSubject" ValidationGroup="validateAdd" runat="server"
                                                        Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ParenSubjectForGroupShouldBeSelected %>"
                                                        CssClass="ClsMdtStar" ControlToValidate="cmbParentSubjects" Operator="NotEqual"
                                                        ValueToCompare="0"></asp:CompareValidator>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ClsBorderlight">
                                            <span class="ClsLabel">
                                                <asp:Label ID="LblSelectChildSubject" runat="server" Text="<%$ Resources:LocalizedResources, SelectChildSubject %>"></asp:Label>
                                                <span id="Span2" class="colonPadding">:</span></span>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel2">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="cmbSubjects" runat="server" CssClass="MidCombo">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Button ID="BtnAdd" ValidationGroup="validateAdd" Text="<%$ Resources:LocalizedResources, AddSubjectToGroup %>"
                                                runat="server" CssClass="ClsBtnLrg" BorderWidth="1px" OnClick="BtnAdd_Click"
                                                UseSubmitBehavior="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 50%">
                                <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="uPnl">
                                    <ContentTemplate>
                                        <div id="Div1" runat="server" class="GridBorder ClsGridBG" style="width: 100%; height: 205pt;
                                            overflow: auto;">
                                            <asp:GridView ID="grdSubjects" DataKeyNames="Subject_Id" runat="server" Width="100%"
                                                AutoGenerateColumns="False" PageSize="30" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                                                GridLines="None" OnRowCommand="grdSubjects_rowCommand" OnRowDataBound="grdSubjects_RowDataBound">
                                                <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                </PagerStyle>
                                                <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                    FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                <Columns>
                                                    <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, ChildSubjectName %>" SortExpression="Subject_Name" DataField="Subject_Name">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:ButtonField ButtonType="Image" HeaderText="<%$ Resources:LocalizedResources, Delete %>" CommandName="DeleteRow" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                        Text="<%$ Resources:LocalizedResources, Delete %>">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:ButtonField>
                                                </Columns>
                                                <RowStyle CssClass="ClsGridRow" />
                                                <HeaderStyle CssClass="ClsGridHeader" />
                                                <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr align="center">
                <td style="margin-left: 40px">
                    <asp:Button ID="btnSave" Text="<%$ Resources:LocalizedResources, Save %>" CausesValidation="False"
                        runat="server" CssClass="ClsBtnSml" BorderWidth="1px" OnClick="btnSave_Click"
                        disable-page="true" />
                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>"
                        CausesValidation="False" CssClass="ClsBtn" BorderWidth="1px" UseSubmitBehavior="false"
                        PostBackUrl="~/RITeSchool/Admin/SubjectGroupsListUI.aspx" />
                    <asp:HiddenField ID="hidGroupId" runat="server"></asp:HiddenField>
                    <asp:CompareValidator Display="none" ID="CompareValidator1" ValidationGroup="validateAdd"
                        runat="server" ControlToValidate="cmbSubjects" Operator="NotEqual" ValueToCompare="0"
                        ErrorMessage="<%$ Resources:LocalizedResources, PleaseSelectAChildSubjectToAddInTheGroup %>"></asp:CompareValidator>
                    <asp:HiddenField ID="hidSubjectId" runat="server" />
                    <asp:HiddenField ID="hidIsConfig" runat="server" />
                    <asp:HiddenField ID="hidStandardDivisionId" runat="server" />
                    <asp:HiddenField ID="hidClassName" runat="server" />
                    <asp:HiddenField ID="hidSubjectIDs" runat="server" />
                    <asp:HiddenField ID="hidAreYouSureYouWantToDeleteThisSubject" runat="server" />
                    <asp:HiddenField ID="hidCultureInfo" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.grdSubjects.ClientID %>";
        _clientlblErrorId = "<%=this.lblError.ClientID %>";
        _clientBtnAdd = "<%=this.BtnAdd.ClientID %>";
        _clientbtnSave = "<%=this.btnSave.ClientID %>";
        _clientbtnBack = "<%=this.btnBack.ClientID %>";


        function ResetLabel() {
            document.getElementById(_clientlblErrorId).style.display = "none";
            document.getElementById(_clientlblErrorId).innerHTML = "";

        }
        function ValidateSubjectCount(oSrc, args) {

            if (document.getElementById(_clientGridId).rows.length < 2) {
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;

        }
        function ConfirmDelete() {
            var bResult = true;
            {
                if (!window.confirm(document.getElementById("<%=this.hidAreYouSureYouWantToDeleteThisSubject.ClientID %>").value))
                { bResult = false; }
            }
            return bResult;
        } 
    </script>
</asp:Content>
