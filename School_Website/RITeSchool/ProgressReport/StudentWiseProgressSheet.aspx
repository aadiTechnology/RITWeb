<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="StudentWiseProgressSheet.aspx.cs" Inherits="StudentWiseProgressSheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%">
            <tr>
                <td align="center">
                    <asp:UpdatePanel ChildrenAsTriggers="False" UpdateMode="Conditional" runat="server"
                        ID="uPnl">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblErrorMsg" runat="server" Visible="False" Width="800px" CssClass="LblNoRecord"
                                            EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnCancelUp" runat="server" Visible="false" BorderStyle="Solid" BorderWidth="1px"
                                            CausesValidation="false" CssClass="ClsBtnSml" PostBackUrl="~/RITeSchool/ProgressReport/AssignExamWiseStudentMarksUI.aspx" Text="Back" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table id = "tblProgress" runat="server" width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:UpdatePanel ID="UPanelStandardt" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="GridViewScrollContainer" runat="server" Visible="true" Style="width: 842px;
                                                                left: 0px;">
                                                            </asp:Panel>
                                                            <asp:Panel ID="ResultContainer" runat="server" Visible="true" Style="overflow: auto;
                                                                width: 842px; left: 0px;">
                                                            </asp:Panel>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnCancel" runat="server" BorderStyle="Solid" BorderWidth="1px" CausesValidation="false"
                                            CssClass="ClsBtnSml" PostBackUrl="~/RITeSchool/ProgressReport/AssignExamWiseStudentMarksUI.aspx" Text="Back" Visible="True" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
