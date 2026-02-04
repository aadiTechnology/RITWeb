<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ConfigurePeerDetailsUI.aspx.cs" Inherits="ConfigurePeerDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td>
                    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel LblErrorMsg" />
                            <asp:RequiredFieldValidator ID="ReqStd" runat="server" Display="None" ControlToValidate="ddlStandard"
                                InitialValue="0" ErrorMessage="Please select Standard."></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="ReqDiv" runat="server" Display="None" ControlToValidate="ddlDivision"
                                InitialValue="0" ErrorMessage="Please select Division."></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cstCustomValidator" runat="server" EnableClientScript="true"
                                ClientValidationFunction="cstValidate" Display="None" ErrorMessage="Peer Student should be selected for atleast one student."
                                SetFocusOnError="true"></asp:CustomValidator>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center" valign="top">
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <div style="float: right; vertical-align: top;">
                                    <span style="width: 150px" class="ClsMdtStar">* Mandatory Fields </span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblUpdateMessage" runat="server" ForeColor="Blue" Width="100%" EnableViewState="false"
                                            CssClass="ClsLabel" Font-Bold="true"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td align="left" class="ClsBorderlight" style="width:100px;">
                                            <span class="ClsLabel">
                                                <asp:Label ID="lblStandard" runat="server" Text="<%$ Resources:LocalizedResources, Standard%>"></asp:Label>
                                                <span id="spanId1" class="colonPadding">:</span> </span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlStandard" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlStandard_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <span class="ClsMdtStar">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" class="ClsBorderlight">
                                            <span class="ClsLabel">
                                                <asp:Label ID="lblDivision" runat="server" Text="<%$ Resources:LocalizedResources, Division%>"></asp:Label>
                                                <span id="spanId2" class="colonPadding">:</span> </span>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="up3" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlDivision" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <span class="ClsMdtStar">*</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table align="center" width="80%">
                                    <tr>
                                        <td align="center">
                                            <asp:UpdatePanel ID="up4" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:ListView ID="lstvwConfigurePeerDetails" runat="server" DataKeyNames="Id,YearwiseStudentId"
                                                        OnItemDataBound="lstvwConfigurePeerDetails_ItemDataBound">
                                                        <LayoutTemplate>
                                                            <table align="center" width="100%" runat="server" id="tblTravlerInfo" style="color: #333333"
                                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="center" class="paddingLR" width="70px">
                                                                        Roll No.
                                                                    </th>
                                                                    <th align="left" class="paddingLR">
                                                                        Student Name
                                                                    </th>
                                                                    <th align="center" class="paddingLR" width="180px">
                                                                        Peer Student Name
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="Tr2" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                                <td align="center" class="ClspaddingMidT">
                                                                    <asp:Label ID="lblRollNo" runat="server" CssClass="ClsLabel" style="float:inherit" Text='<%#Eval("RollNo")%>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblStudentName" runat="server" Style="padding-right: 5px;" CssClass="ClsLabel" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:DropDownList ID="ddlPeer" runat="server" CssClass="MidCombo" ViewStateMode="Enabled">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <EmptyDataTemplate>
                                                            <div class="LblNoRecord">
                                                                No Record Found.
                                                            </div>
                                                        </EmptyDataTemplate>
                                                    </asp:ListView>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td align="center">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>"
                                                        class="ClsBtn" OnClick="btnSave_Click" Enabled="False" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hidHasEditAccess" runat="server" Value="N" />
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            function cstValidate(src, args) {
                var isFound = false;
                $('[id$=ddlPeer]').each(function () {
                    if ($(this).val() != '0')
                        isFound = true
                    return false;
                })
                if (isFound) {
                    args.IsValid = true;
                    return false;
                }

                args.IsValid = false;
                return true;
            }
            
        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
