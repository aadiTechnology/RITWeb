<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ResultDetailsUI.aspx.cs" Inherits="ResultDetailsUI" %>

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
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTerm" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center" valign="top">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                            <div style="float: right; vertical-align: top;">
                                                <span style="width: 150px" class="ClsMdtStar">* Mandatory Fields </span>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td align="center" valign="top">
                                            <table>
                                                <tr>
                                                    <td align="center" colspan="3">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" Font-Bold="true"
                                                                    ForeColor="Blue"></asp:Label>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlTerm" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="txtNormal" colspan="3">
                                                        <asp:RequiredFieldValidator ID="ReqStd" runat="server" Display="None" ControlToValidate="ddlStandard"
                                                            InitialValue="0" ErrorMessage="Please select Standard."></asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="ReqDiv" runat="server" Display="None" ControlToValidate="ddlDivision"
                                                            InitialValue="0" ErrorMessage="Please select Division."></asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="ReqTerm" runat="server" Display="None" ControlToValidate="ddlTerm"
                                                            InitialValue="0" ErrorMessage="Please select Term."></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" class="ClsBorderlight" style="width: 100px;">
                                                        <asp:Label ID="lblStandard" runat="server" Text="Standard:" CssClass="ClsLabel"></asp:Label>
                                                    </td>
                                                    <td id="Td1" align="left" runat="server">
                                                        <asp:DropDownList ID="ddlStandard" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddlStandard_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <span class="ClsMdtStar">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" class="ClsBorderlight">
                                                        <asp:Label ID="lblDivision" runat="server" Text="Division:" CssClass="ClsLabel"></asp:Label>
                                                    </td>
                                                    <td id="Td2" align="left" runat="server">
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
                                                <tr>
                                                    <td align="center" class="ClsBorderlight">
                                                        <asp:Label ID="lblTerm" runat="server" Text="Term:" CssClass="ClsLabel"></asp:Label>
                                                    </td>
                                                    <td id="Td3" align="left" runat="server">
                                                        <%-- <asp:UpdatePanel ID="up4" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>--%>
                                                        <asp:DropDownList ID="ddlTerm" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddlTerm_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <%-- </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="ddlTerm" EventName="SelectedIndexChanged" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>--%>
                                                    </td>
                                                    <td>
                                                        <span class="ClsMdtStar">*</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table align="center" width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="up5" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table align="center" width="80%">
                                                        <tr>
                                                            <td valign="top" align="center">
                                                                <asp:ListView ID="lstvwResultDetails" runat="server" DataKeyNames="StudentId" OnItemDataBound="lstvwResultDetails_ItemDataBound"
                                                                     OnDataBound="lstvwResultDetails_DataBound">
                                                                    <LayoutTemplate>
                                                                        <table id="Table1" width="100%" runat="server" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                <th align="left" class="paddingLR" width="70px">
                                                                                    Roll No.
                                                                                </th>
                                                                                <th align="left" class="paddingLR">
                                                                                    Student Name
                                                                                </th>
                                                                                <th align="center" class="paddingLR" width="180px" id="thConduct" runat="server">
                                                                                    Conduct
                                                                                </th>
                                                                                <th align="center" class="paddingLR" width="180px">
                                                                                    Punctuality
                                                                                </th>
                                                                                <th align="center" class="paddingLR" width="180px">
                                                                                    Result
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
                                                                            <td align="center" id="tdConduct" runat="server">
                                                                                <asp:DropDownList ID="ddlConduct" runat="server" CssClass="MidCombo" ViewStateMode="Enabled">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:DropDownList ID="ddlPunctuality" runat="server" CssClass="MidCombo" ViewStateMode="Enabled">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:DropDownList ID="ddlResult" runat="server" CssClass="MidCombo" ViewStateMode="Enabled">
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
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" valign="top">
                                                                <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="ClsBtn" OnClick="btnSave_Click" />
                                                                <asp:HiddenField ID="hidHasEditAccess" runat="server" Value="N" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlTerm" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
