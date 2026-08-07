<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="AssignGradesUI.aspx.cs" Inherits="AssignGradesUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr align="center">
                <td id="tdMessage" runat="server" align="center">  
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>                  
                            <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label> 
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbExams" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="btnPublish" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>                       
                </td>
            </tr>
            <tr>
                <td style="height:10px;"></td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td align="left" class="ClsBorderlight" style="width: 100px">
                                <span class="ClsLabel">Exam :</span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbExams" runat="server" CssClass="LrgCombo" AutoPostBack="True"
                                    OnSelectedIndexChanged="cmbExams_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td align="left" class="ClsBorderlight" style="width: 100px">
                                <span class="ClsLabel">Teacher :</span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbTeachers" runat="server" CssClass="LrgCombo" AutoPostBack="True"
                                    OnSelectedIndexChanged="cmbTeachers_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td align="left" class="ClsBorderlight" style="width: 100px">
                                <span class="ClsLabel">Class :</span>
                            </td>
                            <td align="left">
                                <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>                                
                                    <asp:DropDownList ID="cmbClass" runat="server" CssClass="LrgCombo" 
                                        AutoPostBack="True" onselectedindexchanged="cmbClass_SelectedIndexChanged"></asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                                </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="Height10">
                <td>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="50%">
                                <tr>
                                    <td>
                                        <asp:ListView ID="lstvwSubjects" runat="server" DataKeyNames="Standard_Division_Id,Subject_Id, IsSubmitted, IsSubjectTeacher,IsCoCurricularSubject,StandardDivision"
                                            OnItemCommand="lstvwSubjects_ItemCommand" 
                                            OnSelectedIndexChanging="lstvwSubjects_SelectedIndexChanging" 
                                            onitemdatabound="lstvwSubjects_ItemDataBound">
                                            <LayoutTemplate>
                                                <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                        <th align="left" style="padding-left: 5px">
                                                            Class
                                                        </th>
                                                        <th align="left" width="40%" style="padding-left: 5px">
                                                            Subject
                                                        </th>
                                                        <th width="150px" align="center">
                                                            Select
                                                        </th>
                                                        <th id="thSummary" runat="server" visible="false" width="150px">
                                                            Add Summary
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                    <td align="left">
                                                        <asp:Label ID="lblClass" runat="server" CssClass="ClsLabel" Text='<%#Eval("StandardDivision") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblParameter" runat="server" CssClass="ClsLabelL" Text='<%#Eval("Subject_Name") %>'></asp:Label>
                                                    </td>
                                                    <td align="center" style="padding-top:5px;">
                                                        <asp:ImageButton ID="btnSelect" runat="server" CausesValidation="false" CommandName="SELECT"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/selection5.gif"
                                                            ToolTip="Select" />
                                                        <asp:Label ID="lblDash" runat="server" CssClass="ClsLabel" Style="float: inherit" Text="-" Visible="false"></asp:Label>
                                                    </td>
                                                    <td align="center" style="padding-top:5px;" id="tdSummary" runat="server" visible="false">
                                                        <asp:ImageButton ID="btnAddSummary" runat="server" CausesValidation="false" CommandName="AddSummary"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/Add_Grace.png"
                                                            ToolTip="AddSummary" />
                                                        <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Style="float: inherit" Text="-" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                    <td align="left">
                                                        <asp:Label ID="lblClass" runat="server" CssClass="ClsLabel" Text='<%#Eval("StandardDivision") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblParameter" runat="server" CssClass="ClsLabelL" Text='<%#Eval("Subject_Name") %>'></asp:Label>
                                                    </td>
                                                    <td align="center" style="padding-top:5px;">
                                                        <asp:ImageButton ID="btnSelect" runat="server" CausesValidation="false" CommandName="SELECT"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/selection5.gif"
                                                            ToolTip="Select" />
                                                        <b><asp:Label ID="lblDash" runat="server" CssClass="ClsLabel" Style="float: inherit" Text="-" Visible="false"></asp:Label></b>
                                                    </td>
                                                    <td align="center" style="padding-top:5px;" id="tdSummary" runat="server" visible="false">
                                                        <asp:ImageButton ID="btnAddSummary" runat="server" CausesValidation="false" CommandName="AddSummary"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/Add_Grace.png"
                                                            ToolTip="AddSummary" />
                                                        <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Style="float: inherit" Text="-" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        <asp:Label ID="lblNoRecFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                                <tr align="center" style="text-align:center; margin:0px auto;">
                                    <td align="center" style="text-align:center;">
                                        <asp:Button ID="btnPublish" runat="server" CausesValidation="false" CssClass="ClsBtnMid"                                                                                    
                                        Text="Publish" onclick="btnPublish_Click" Visible = "false" />
                                        <asp:HiddenField ID="hidIsClassTeacher" runat="server" Value="N" />
                                        <asp:HiddenField ID="hidStdDivId" runat="server" Value="0" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbExams" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="btnPublish" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>            
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
