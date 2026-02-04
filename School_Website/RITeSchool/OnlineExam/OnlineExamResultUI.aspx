<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="OnlineExamResultUI.aspx.cs" Inherits="OnlineExamResultUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
<script type="text/javascript" src="https://polyfill.io/v3/polyfill.min.js?features=es6"></script>
<script id="MathJax-script" type="text/javascript" async src="https://cdn.jsdelivr.net/npm/mathjax@3/es5/tex-mml-chtml.js">
</script>
    <table width="100%">
        <tr>
            <td align="center">
                <table width="80%">
                    <tr>
                        <td align="center">
                            <asp:ListView ID="lstvwStudent" runat="server" OnItemDataBound="lstvwStudent_ItemDataBound">
                                <LayoutTemplate>
                                    <table id="tblStudent" align="center" runat="server" class="GridBorder" width="100%">
                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                            <th align="left" width="75px" class="ClsPaddingL">
                                                <asp:Label ID="lblRollNo" runat="server" Text="Roll No." EnableViewState="false"></asp:Label>
                                            </th>
                                            <th align="left" class="ClsPaddingL">
                                                <asp:Label ID="lblname" runat="server" Text="Student Name" EnableViewState="false"></asp:Label>
                                            </th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="Tr2" runat="server" class="ClsGridRow">
                                        <td align="left" class="ClspaddingL">
                                            <asp:Label ID="lblSubject" runat="server" Font-Bold="true" Text='<%#Eval("RollNo")%>'></asp:Label>
                                        </td>
                                        <td align="left" class="ClspaddingL">
                                            <asp:Label ID="lblStudentName" runat="server" Font-Bold="true" Text='<%#Eval("StudentName")%>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trAnswerDetails" runat="server">
                                        <td>
                                        </td>
                                        <td id="tdAnswerDetails" runat="server">
                                            <asp:ListView ID="lstvwAnswerDetails" runat="server" OnItemDataBound="lstvwAnswerDetails_ItemDataBound">
                                                <LayoutTemplate>
                                                    <table width="100%" runat="server" id="tblRange" style="color: #333333" cellpadding="0"
                                                        cellspacing="1" class="GridBorder">
                                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                            <th align="center" style="width: 50px">
                                                                Sr. No.
                                                            </th>
                                                            <th align="left" style="padding-left: 5px;">
                                                                Question
                                                            </th>
                                                            <th style="width: 200px; padding-left: 5px;" align="left">
                                                                Given Answer
                                                            </th>
                                                        </tr>
                                                        <tr id="itemPlaceholder" runat="server">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr id="Tr2" runat="server" class="ClsGridRow">
                                                        <td align="center">
                                                            <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                Text="<%#Container.DataItemIndex+1 %> "></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblQuestion" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                Text='<%#Eval("Question") %>'></asp:Label>
                                                            <asp:Image ID="imgQuestionAttachment" runat="server" Width="100%" Height="150px" Visible="false" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblAnswer" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                Text='<%#Eval("Answer") %>'></asp:Label>
                                                            <asp:Image ID="imgAttachment" runat="server" Visible="false" Width="50px" Height="50px" style="padding-left:5px;" />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                        <td style="width: 50px" align="center">
                                                            <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                Text="<%#Container.DataItemIndex+1 %> "></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblQuestion" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                Text='<%#Eval("Question") %>'></asp:Label>
                                                            <asp:Image ID="imgQuestionAttachment" runat="server" Width="100%" Height="150px" Visible="false" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblAnswer" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                Text='<%#Eval("Answer") %>'></asp:Label>
                                                            <asp:Image ID="imgAttachment" runat="server" Visible="false" Width="50px" Height="50px" style="padding-left:5px;" />
                                                        </td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                            </asp:ListView>
                                        </td>
                                    </tr>
                                </ItemTemplate>
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
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
