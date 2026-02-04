<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="HomeworkUI.aspx.cs" Inherits="HomeworkUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div>
        <table width="97%">
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%;">
                                <tr>
                                    <td align="right" class="ClsTextNormal" style="padding-right: 10px; height: 19px;">
                                        <span class="ClsMdtStar">*</span>
                                        <asp:Label ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False"
                                            Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trPrecondition" runat="server">
                                    <td id="tdPrecondition" runat="server">
                                        <div runat="server" id="divError">
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trControls" runat="server">
                                    <td>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td align="center" valign="bottom">
                                                    <table id="tblFilter" runat="server" width="400px">
                                                        <tr>
                                                            <td align="left" id="tdTeacher" runat="server" class="ClsBorderlight" colspan="1">
                                                                <asp:Label ID="lblTeacher" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                                    Font-Bold="True" Text="Select Teacher :"></asp:Label>&nbsp;
                                                            </td>
                                                            <td align="left" colspan="1">
                                                                <asp:DropDownList ID="cmbTeachers" AutoPostBack="true" runat="server" CssClass="ExLrgCombo"
                                                                    OnSelectedIndexChanged="cmbTeachers_SelectedIndexChanged" Width="260px">
                                                                </asp:DropDownList>
                                                                <span class="ClsMdtStar">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" id="td1" runat="server" class="ClsBorderlight" colspan="1">
                                                                <asp:Label ID="Label2" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                                                    Text="Select Class :"></asp:Label>&nbsp;
                                                            </td>
                                                            <td align="left" colspan="1">
                                                                <asp:DropDownList ID="cmbClass" AutoPostBack="true" runat="server" CssClass="ExLrgCombo"
                                                                    Width="260px" OnSelectedIndexChanged="cmbClass_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <span class="ClsMdtStar">*</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table width="550px">
                                                        <tr>
                                                            <td align="left">
                                                                <span class="ClsLblLgnd" >My Subjects :</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:ListView ID="lstViewSubjectTeacher" runat="server" OnItemDataBound="lstViewSubjectTeacher_ItemDataBound">
                                                                    <LayoutTemplate>
                                                                        <table id="Table1" align="center" width="550px" runat="server" class="GridBorder">                                                                            
                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                <th align="left" class="paddingL">
                                                                                    <asp:Label ID="lblClass" runat="server" Text="Class"></asp:Label>
                                                                                </th>
                                                                                <th align="left" class="paddingL">
                                                                                    <asp:Label ID="lblSubject" runat="server" Text="Subject"></asp:Label>
                                                                                </th>
                                                                                <th align="center" style="padding-left: 10px;">
                                                                                    <asp:Label ID="lblAdd" runat="server" Text="Assign"></asp:Label>
                                                                                </th>
                                                                            </tr>
                                                                            <tr runat="server" id="itemPlaceholder">
                                                                            </tr>
                                                                        </table>
                                                                    </LayoutTemplate>
                                                                    <ItemTemplate>
                                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblStandard" runat="server" Text='<%# Eval("Class") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Subject_Name") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center" class="paddingL">
                                                                                <asp:ImageButton ID="imgBtnAdd" runat="server" CausesValidation="false" CommandName="Add"
                                                                                    ToolTip="Add homework" ImageUrl="../images/Homework.png" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <tr id="Tr2" runat="server" class="ClsGridAltRow">
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblStandard" runat="server" Text='<%# Eval("Class") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Subject_Name") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center" class="paddingL">
                                                                                <asp:ImageButton ID="imgBtnAdd" runat="server" CausesValidation="false" CommandName="Add"
                                                                                    ToolTip="Add homework" ImageUrl="../images/Homework.png" />
                                                                            </td>
                                                                        </tr>
                                                                    </AlternatingItemTemplate>
                                                                    <EmptyDataTemplate>
                                                                        <tr>
                                                                            <td width="550px" align="center" class="LblNoRecord">
                                                                                No record found.
                                                                            </td>
                                                                        </tr>
                                                                    </EmptyDataTemplate>
                                                                </asp:ListView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblMyClass" width="550px" runat="server" visible="false">
                                                        <tr>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr id="tr3" runat="server">
                                                            <td id="td3" runat="server">
                                                                <div runat="server" id="div2" style="float: left; width: 550px">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <span class="ClsLblLgnd" >My Class Subjects :</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:ListView ID="lstViewClassSubject" runat="server" OnItemDataBound="lstViewClassSubject_ItemDataBound">
                                                                    <LayoutTemplate>
                                                                        <table id="Table1" align="center" width="550px" runat="server" class="GridBorder">                                                                           
                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                <th align="left" class="paddingL">
                                                                                    <asp:Label ID="lblClass" runat="server" Text="Class"></asp:Label>
                                                                                </th>
                                                                                <th align="left" class="paddingL">
                                                                                    <asp:Label ID="lblSubject" runat="server" Text="Subject"></asp:Label>
                                                                                </th>
                                                                                <th align="center" style="padding-left: 10px;">
                                                                                    <asp:Label ID="lblAdd" runat="server" Text="Assign"></asp:Label>
                                                                                </th>
                                                                            </tr>
                                                                            <tr runat="server" id="itemPlaceholder">
                                                                            </tr>
                                                                        </table>
                                                                    </LayoutTemplate>
                                                                    <ItemTemplate>
                                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblStandard" runat="server" Text='<%# Eval("Class") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Subject_Name") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center" class="paddingL">
                                                                                <asp:ImageButton ID="imgBtnAdd" runat="server" CausesValidation="false" CommandName="Add"
                                                                                    ToolTip="Add homework" ImageUrl="../images/Homework.png" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <tr id="Tr2" runat="server" class="ClsGridAltRow">
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblStandard" runat="server" Text='<%# Eval("Class") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Subject_Name") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center" class="paddingL">
                                                                                <asp:ImageButton ID="imgBtnAdd" runat="server" CausesValidation="false" CommandName="Add"
                                                                                    ToolTip="Add homework" ImageUrl="../images/Homework.png" />
                                                                            </td>
                                                                        </tr>
                                                                    </AlternatingItemTemplate>
                                                                    <EmptyDataTemplate>
                                                                        <tr>
                                                                            <td width="550px" align="center" class="LblNoRecord">
                                                                                No record found.
                                                                            </td>
                                                                        </tr>
                                                                    </EmptyDataTemplate>
                                                                </asp:ListView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                  <tr align="center" style="text-align:center; margin:0px auto;">
                                    <td align="center" style="text-align:center;">
                                        <asp:Button ID="btnAdd" runat="server" Text="Add Daily Log" 
                                            CssClass="ClsBtn" Visible = "false" onclick="btnAdd_Click"  />
                                    </td>
                                </tr>
                            </table>
                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                            <asp:HiddenField ID="hidStandardDivisionId" runat="server" Value="0"></asp:HiddenField>
                            <asp:HiddenField ID="hidPublish" runat="server" Value="0"></asp:HiddenField>
                            <asp:HiddenField ID="hidQery" runat="server" />
                            <asp:HiddenField ID="hidAlert" runat="server" />
                            <asp:HiddenField ID="hidUserHasFullAccess" runat="server" Value="False" />
                            <asp:HiddenField ID="hidIsMonthConfig" runat="server" Value="False" />
                            <asp:HiddenField ID="hidUserID" runat="server" Value="False" />
                            <asp:HiddenField ID="hidConfirmSms" runat="server" />
                            <asp:HiddenField ID="hidExamDependencyMsg" runat="server" />
                            <asp:HiddenField ID="hidDependentExamNames" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
