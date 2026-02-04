<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="OnlineExamDetailsUI.aspx.cs" Inherits="OnlineExamDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%">
        <tr>
            <td align="center">
                <table width="100%">
                    <tr>
                        <td align="center">
                            <table id="tblControls" runat="server">
                                <tr>
                                    <td class="ClsBorderlight" style="width:100px">
                                        <asp:Label ID="lblExam" runat="server" CssClass="ClsLabel" Text="Exam" EnableViewState="false"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td runat="server" id="tdSubjectCmb">
                                        <asp:DropDownList ID="cmbExam" runat="server" Width="200px" OnSelectedIndexChanged="cmbExam_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px;">
                        </td>
                    </tr>
                    <tr align="center" style="text-align: center; margin 0px auto;">
                        <td align="center">
                            <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table style="text-align: center; margin: 0px auto;" align="center" width="50%">
                                        <tr id="trlstvw" runat="server" align="center">
                                            <td>
                                                <asp:ListView ID="lstvwExam" runat="server" OnItemDataBound="lstvwExam_ItemDataBound"
                                                    DataKeyNames="ExamID, Id, StartTime, EndTime, SubjectId, StandardDivisionId, IsSubmited"
                                                    OnItemCommand="lstvwExam_ItemCommand">
                                                    <LayoutTemplate>
                                                        <table id="tblhomework" align="center" runat="server" width="100%" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader" style="height: 15px;">
                                                                <th align="left" class="ClsPaddingL">
                                                                    <asp:Label ID="lblDisplayName" runat="server" Text="Subject" EnableViewState="false"></asp:Label>
                                                                </th>
                                                                <th align="center" width="120px">
                                                                    <asp:Label ID="lblStartTime" runat="server" Text=" Exam Date " EnableViewState="false"></asp:Label>
                                                                </th>
                                                                <th align="center" width="120px">
                                                                    <asp:Label ID="lblstartTime1" runat="server" Text=" Start  Time" EnableViewState="false"></asp:Label>
                                                                </th>
                                                                <th align="center" width="120px">
                                                                    <asp:Label ID="Label3" runat="server" Text=" End Time " EnableViewState="false"></asp:Label>
                                                                </th>
                                                                <th align="center" width="100px">
                                                                    <asp:Label ID="lblDisplayValue" runat="server" Text="Link" EnableViewState="false"></asp:Label>
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblSubject" runat="server" Text='<%#Eval("Subject_Name")%>'></asp:Label>
                                                            </td>
                                                            <td align="center" width="120px">
                                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("StartDateAndTime", "{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                            </td>
                                                            <td align="center" width="120px">
                                                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("StartDateAndTime", "{0:hh:mm tt}")%>'></asp:Label>
                                                            </td>
                                                            <td align="center" width="120px">
                                                                <asp:Label ID="Label5" runat="server" Text='<%#Eval("EndDateAndTime", "{0:hh:mm tt}")%>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:LinkButton ID="lnkDetails" runat="server" CausesValidation="false" CssClass="SMSLblSMlBlue"
                                                                    Text="Exam" CommandName="SelectCommand" CommandArgument='<%# Eval("Id")%>' />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="Tr2" runat="server" class="ClsGridAltRow">
                                                           <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblSubject" runat="server" Text='<%#Eval("Subject_Name")%>'></asp:Label>
                                                            </td>
                                                            <td align="center" width="120px">
                                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("StartDateAndTime", "{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                            </td>
                                                            <td align="center" width="120px">
                                                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("StartDateAndTime", "{0:hh:mm tt}")%>'></asp:Label>
                                                            </td>
                                                            <td align="center" width="120px">
                                                                <asp:Label ID="Label5" runat="server" Text='<%#Eval("EndDateAndTime", "{0:hh:mm tt}")%>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:LinkButton ID="lnkDetails" runat="server" CausesValidation="false" CssClass="SMSLblSMlBlue"
                                                                    Text="Exam" CommandName="SelectCommand" CommandArgument='<%# Eval("Id")%>' />
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <tr style="border: 1px solid;">
                                                            <td align="center" class="LblNoRecord">
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
                                    <asp:AsyncPostBackTrigger ControlID="cmbExam" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />                                    
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Timer ID="Timer1" runat="server" Enabled="False" ontick="Timer1_Tick">
                            </asp:Timer>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
