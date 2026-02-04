<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentListForAssessmentDetailsUI.aspx.cs" Inherits="StudentListForAssessmentDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td align="center" valign="top">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <div style="float: right; vertical-align: top;">
                                    <span style="width: 150px" class="ClsMdtStar">* Mandatory Fields </span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td align="center" class="ClsBorderlight" style="width: 120px;">
                                            <asp:Label ID="lblTest" runat="server" CssClass="ClsLabel" Text="Test Name :"></asp:Label>
                                        </td>
                                        <td id="Td3" align="left" runat="server">
                                            <asp:DropDownList ID="ddlTest" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlTest_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <span class="ClsMdtStar">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="ClsBorderlight" style="width: 120px;">
                                            <asp:Label ID="lblStandard" runat="server" CssClass="ClsLabel" Text="Standard:"></asp:Label>
                                        </td>
                                        <td id="Td1" align="left" runat="server">
                                            <asp:DropDownList ID="ddlStandard" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlStandard_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <span class="ClsMdtStar">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="ClsBorderlight" style="width: 120px;">
                                            <asp:Label ID="lblDivision" runat="server" CssClass="ClsLabel" Text="Division:"></asp:Label>
                                        </td>
                                        <td id="Td2" align="left" runat="server">
                                            <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlDivision" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table align="center" width="50%">
                                            <tr>
                                                <td valign="top" align="center">
                                                    <asp:ListView ID="lstvwStudentListForAssessment" runat="server" DataKeyNames="StudentId,StandardId"
                                                        OnItemDataBound="lstvwStudentListForAssessment_ItemDataBound">
                                                        <LayoutTemplate>
                                                            <table id="Table1" width="100%" runat="server" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="left" class="paddingLR" width="70px">
                                                                        Roll No.
                                                                    </th>
                                                                    <th align="left" class="paddingLR">
                                                                        Student Name
                                                                    </th>
                                                                    <th align="center" width="100px">
                                                                        <asp:Label ID="lblSelectStudent" runat="server" Text="Select" />
                                                                    </th>
                                                                    <th align="center" class="PaddingL-10" width="150px" id="thStatus" runat="server">
                                                                        <asp:Label ID="lblStatus" runat="server" Text="Submit Status"> </asp:Label>
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="trItemtemplate" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                                <td align="center" class="ClspaddingLR">
                                                                    <asp:Label ID="lblRollNo" runat="server" CssClass="ClsLabel" Text='<%#Eval("RollNo") %>'></asp:Label>
                                                                </td>
                                                                <td align="center" class="ClspaddingLR">
                                                                    <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLabel" Text='<%#Eval("StudentName") %>'></asp:Label>
                                                                </td>
                                                                <td id="tdSelectStudent" runat="server" align="center">
                                                                    <asp:ImageButton ID="imgBtnSelect" runat="server" ImageUrl="../images/selection5.gif"
                                                                        AlternateText="Select" CausesValidation="false" ToolTip="Click to save student Assessment details." />
                                                                </td>
                                                                <td align="center" id="tdStatus" runat="server" viewstatemode="Enabled" class="PaddingL-10">
                                                                    <asp:Image ID="ImgSelfSubmit" runat="server" ToolTip="Self Assessment" ImageUrl="../images/IconGrid_AssignTrue.gif" />
                                                                    <asp:Image ID="ImgPeerSubmit" runat="server" ToolTip="Peer Feedback" ImageUrl="../images/IconGrid_AssignTrue.gif" />
                                                                    <asp:Image ID="ImgParentSubmit" runat="server" ToolTip="Parent Feedback" ImageUrl="../images/IconGrid_AssignTrue.gif" />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <EmptyDataTemplate>
                                                            <div class="LblNoRecord">
                                                                No Record Found.
                                                            </div>
                                                        </EmptyDataTemplate>
                                                    </asp:ListView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlTest" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hidHasEditAccess" runat="server" Value="N" />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">

        function OpenWindow(querystring) {
            window.open('../Student/StudentAssessmentDetailsUI.aspx?' + querystring, '_self');
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
