<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="SubmitProgreesReportResult.aspx.cs" Inherits="SubmitProgreesReportResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv" style="vertical-align: top">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="left" colspan="2" rowspan="1" style="height: 20px">
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td style="height: 20px">
                                <span class="MainTitleHead">PrePrimary Progress Report Status</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr align="center" valign="middle" style="height: 30px">
                <td>
                    <asp:Label ID="lblSuccess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                        Font-Bold="true" Visible="true" EnableViewState="false" CssClass="ClsLabel"></asp:Label>
                </td>
            </tr>
            <tr align="center" valign="top">
                <td valign="top" align="center">
                    <asp:UpdatePanel ID="upnl2" runat="server">
                        <ContentTemplate>
                            <div id="div1" class="GridBorder" style="text-align: center; width: 100%; overflow: auto;">
                                <asp:GridView ID="grdMonths" runat="server" Width="100%" AutoGenerateColumns="False"
                                    PageSize="100" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                                    DataKeyNames="PrePrimaryProgressReportMonthId,IsSubmitted,IsPublished,PreprimaryExamConfigurationId,RollNos"
                                    EmptyDataText="No Records Found." OnRowDataBound="grdMonths_RowDataBound" OnRowCommand="grdMonths_RowCommand">
                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                    </PagerStyle>
                                    <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                        FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                    <Columns>
                                        <asp:BoundField DataField="MonthAbbreviation" HeaderText="Month">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnStatus" runat="server" CausesValidation="false" CommandName="SUBMIT"
                                                    CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Submit" />
                                                <asp:Label ID = "lblStatus" runat="server" CssClass ="lblBlkB" Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="22%" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="22%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unpublish Reason">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txlUnpublishReason" runat="server" CssClass="MidTxtBox" Visible="false"
                                                    MaxLength="50"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30%" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unpublish">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnUnpublish" runat="server" CausesValidation="false" CommandName="UBPUBLISH"
                                                    Visible="false" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Submit" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="ClsGridRow" />
                                    <HeaderStyle CssClass="ClsGridHeader" />
                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr align="center" valign="top">
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr align="center" valign="top">
                <td>
                    <asp:Button ID="btnCancel" runat="server" Text="Close" CssClass="ClsBtn" BorderWidth="1px"
                        CausesValidation="False" UseSubmitBehavior="false" />
                    <asp:HiddenField ID="hidStandDivId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidIsMonthConfig" runat="server" Value="" />
                    <asp:HiddenField ID="hidIsUnpublish" runat="server" Value="False" />
                    <asp:HiddenField ID="hidRollNos" runat="server" Value="" />
                    <asp:HiddenField ID="hidStudentCount" runat="server" Value="0" />
                    <asp:HiddenField ID="hidConfigStudentCount" runat="server" Value="0" />
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">

        _clientgrdMonths = "<%=this.grdMonths.ClientID %>";

        function ConfirmAction(RollNos, ispublish) {
            var bResult = false;
            if (RollNos.length > 0)
                bResult = window.confirm("Progress report entry is not started for Roll No(s): " + RollNos + "\nAre you sure you want to continue?");
            else if (!ispublish)
                bResult = window.confirm("Are you sure you want to submit result?");
            else
                bResult = window.confirm("Are you sure you want to publish result?");    

            if (bResult && ispublish)
                bResult = window.confirm("Once you publish the result it will be visible to parents/students. Are you sure you want to continue?");

            return bResult;
        }

        function ConfirmUnpublishAction(rowIndex) {
            if ((rowIndex + "").length == 1)
                rowIndex = "0" + rowIndex;
            var txt = document.getElementById(_clientgrdMonths + "_ctl" + rowIndex + "_txlUnpublishReason");

            if (txt.value.trim() == "") {
                alert("Unpublish reason should not be blank.");
                return false;
            }
            return confirm("Are you sure you want to unpublish result?");
        }

    </script>
</asp:Content>
