<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PublishOnlineExamUI.aspx.cs" Inherits="PublishOnlineExamUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%">
        <tr>
            <td align="center">
                <table width="700px">
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="updatepanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" Font-Bold="true"
                                        ForeColor="Blue"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="cmbExam" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="btnPublish" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnUnPublish" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table id="tblControls" runat="server">
                                <tr>
                                    <td class="ClsBorderlight paddingL" style="width: 100px;" runat="server" id="tdSubject">
                                        <asp:Label ID="lblClass" runat="server" CssClass="ClsLabel" Text="Class" EnableViewState="false"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbClass" runat="server" Width="200px" OnSelectedIndexChanged="cmbClass_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="ClsBorderlight paddingL" style="width: 100px;" runat="server" id="td1">
                                        <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Exam" EnableViewState="false"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbExam" runat="server" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="cmbExam_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="updatepanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="80%">
                                        <tr id="trlstvw" runat="server" align="left">
                                            <td>
                                                <asp:ListView ID="lstvwStudent" runat="server" DataKeyNames="AnswerTypeId" OnItemDataBound="lstvwStudent_ItemDataBound">
                                                    <LayoutTemplate>
                                                        <table id="tblStudent" align="center" runat="server" class="GridBorder" width="100%">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="left" style="padding-left: 5px;">
                                                                    Subject
                                                                </th>
                                                                <th align="center" style="width: 100px;">
                                                                    Present
                                                                </th>
                                                                <th align="center" style="width: 100px;">
                                                                    Absent
                                                                </th>
                                                                <th align="center" style="width: 100px;">
                                                                    Is Published?
                                                                </th>
                                                                <th align="center" style="width: 100px;">
                                                                    View
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                            <td align="left">
                                                                <asp:Label ID="lblSubject" CssClass="clsLabel" runat="server" Text='<%#Eval("Subject")%>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("Present")%>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="Label3" runat="server" Text='<%#Eval("Absent")%>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Image ID="imgSubmitted" runat="server" ImageUrl="../images/IconGrid_AssignTrue.gif"
                                                                    Visible='<%# Eval("IsPublished") %>' />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="lnkDetails" runat="server" CausesValidation="false" CommandArgument="<%# Container.DataItemIndex %>"
                                                                    CommandName="SelectCommand" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="Tr2" runat="server" class="ClsGridAltRow">
                                                            <td align="left">
                                                                <asp:Label ID="lblSubject" CssClass="clsLabel" runat="server" Text='<%#Eval("Subject")%>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("Present")%>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="Label3" runat="server" Text='<%#Eval("Absent")%>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Image ID="imgSubmitted" runat="server" ImageUrl="../images/IconGrid_AssignTrue.gif"
                                                                    Visible='<%# Eval("IsPublished") %>' />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="lnkDetails" runat="server" CausesValidation="false" CommandArgument="<%# Container.DataItemIndex %>"
                                                                    CommandName="SelectCommand" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <tr>
                                                            <td width="200px" align="center" class="LblNoRecord">
                                                                No record found.
                                                            </td>
                                                        </tr>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="cmbExam" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="btnPublish" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnUnPublish" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="updatepanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnPublish" runat="server" Text="Publish" CssClass="ClsBtn" OnClick="btnPublish_Click" />
                                    <asp:Button ID="btnUnPublish" runat="server" Text="Un-Publish" CssClass="ClsBtn"
                                        OnClick="btnUnPublish_Click" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="cmbExam" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="btnPublish" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnUnPublish" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <asp:HiddenField ID="hidStdDivId" runat="server" />
                    <asp:HiddenField ID="hidUserHasFullAccess" runat="server" Value="N" />
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
