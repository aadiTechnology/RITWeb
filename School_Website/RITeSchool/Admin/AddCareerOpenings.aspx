<%@ Page Title="" Language="C#"  MasterPageFile="../MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="AddCareerOpenings.aspx.cs" Inherits="AddCareerOpenings" ViewStateMode="Disabled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
 <div class="MainBodyDiv">
  <table width="100%" align="center">
   <tr>
        <td  align="left">
            <asp:ValidationSummary ID="valSumErrorMsgText" runat="server" CssClass="ClsLabel"
                ShowSummary="true" ValidationGroup="TextCareer" />
        </td>
    </tr>
    <tr>
        <td align="center">
            <table>
            
                <tr>
                    <td class="TxtNormal" align="center" colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Width="100%" Visible="false"
                                    CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
      <tr id="trTextControls" runat="server" align="center">
                <td align="center">
                    <table id="tblTextNoticeControls" width="100%" runat="server" visible="true">
                        <tr>
                            <td align="center">
                                <table style="width: 80%">
                                    <tr>
                                        <td class="paddingL" align="center" >
                                             <table runat="server" id="tblJobControls" style="height: 52px; width: 500px">
                                                <tr>
                                                    <td align="right" style="width: 50%" class="ClsBorderlight paddingL"> 
                                                       <span class="ClsLabel">Post :</span>
                                                    </td>
                                                    <td align="left" style="width: 50%" colspan="2">
                                                         <asp:TextBox ID="txtJobTitle" class="ExLrgTxtBox" runat="server" MaxLength="100"></asp:TextBox>
                                                         <span class="ClsMdtStar">*&nbsp;</span>
                                                         <asp:RequiredFieldValidator ID="reqPostName" runat="server" ErrorMessage="Post should not be blank."
                                                    ValidationGroup="TextCareer" ControlToValidate="txtJobTitle" Display="None"> </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td class="ClsBorderlight paddingL" align="left" >
                                                        <span class="ClsLabel">Qualification :</span>
                                                    </td>
                                                    <td  align="left" colspan="2">
                                                        <asp:TextBox ID="txtQualification" class="ExLrgTxtBox" runat="server" MaxLength="100"></asp:TextBox>
                                                        <span class="ClsMdtStar">*&nbsp;</span>
                                                         <asp:RequiredFieldValidator ID="reqQualification" runat="server" ErrorMessage="Qualification should not be blank."
                                                    ValidationGroup="TextCareer" ControlToValidate="txtQualification" Display="None"> </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td class="ClsBorderlight paddingL" align="left" >
                                                        <span class="ClsLabel">Sort Order :</span>
                                                    </td>
                                                    <td class="TxtNormal" align="left" colspan="2" >
                                                        <asp:TextBox runat="server" ID="txtSortorder" CssClass="ExLrgTxtBox" MaxLength="3" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                            onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"> </asp:TextBox>
                                                             <span class="ClsMdtStar">*&nbsp;</span>
                                                        <asp:RequiredFieldValidator ID="reqValSortorder" runat="server" ValidationGroup="TextCareer"
                                                    ErrorMessage="Sort Order should not be blank." ControlToValidate="txtSortorder"
                                                    Display="None"> </asp:RequiredFieldValidator>
                                                  </td>
                                                </tr>
                                                 <tr>
                                                    <td class="ClsBorderlight paddingL" align="left" >
                                                        <span class="ClsLabel">Experience :</span>
                                                    </td>
                                                    <td  align="left" colspan="2">
                                                        <asp:TextBox runat="server" ID="txtExperience" CssClass="ExLrgTxtBox" MaxLength="3"
                                                            onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                            onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"> </asp:TextBox>
                                                         <span class="ClsMdtStar">*&nbsp;</span>
                                                             <asp:RequiredFieldValidator ID="reqValExperience" runat="server" ValidationGroup="TextCareer"
                                                            ErrorMessage="Experience should not be blank." ControlToValidate="txtExperience"
                                                            Display="None"> </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="ClsBorderlight paddingL" align="left" >
                                                        <span class="ClsLabel">Description :</span>
                                                    </td>
                                                    <td  align="left">
                                                        <asp:TextBox ID="txtDescription" class="ExLrgTxtBox" runat="server" TextMode="MultiLine" Columns="21" Rows="4" Width="330px" Height="150px">
                                                        </asp:TextBox> 
                                                        <asp:RequiredFieldValidator ID="reqDesscription" runat="server" ValidationGroup="TextCareer"
                                                        ErrorMessage="Description should not be blank." ControlToValidate="txtDescription"
                                                        Display="None"> </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td   valign="top">
                                                    <span class="ClsMdtStar">*&nbsp;</span>
                                                    </td>
                                                </tr>
                                                 
                                              </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button runat="server" Text="Save" class="ClsBtn" ID="btnSaveText" disable-page="true" OnClick="btnSaveText_Click" ValidationGroup="TextCareer" ViewStateMode="Enabled"/>
                                <asp:Button runat="server" Text="Cancel" class="ClsBtn" ID="btnCancelText" CausesValidation="False" OnClick="btnCancelText_Click"/>
                            </td>
                        </tr>
                    </table>
                </td>
           </tr>
            <tr id="trLink" runat="server">
            <td>
                <table id="tblLstvwCareerOpenings" align="center" width="70%" runat="server">
                    <tr>
                        <td align="center" style="width: 100%">
                            <table align="center" width="100%">
                                <tr id="trPager" runat="server" width="100%">
                                    <td align="center">
                                        <asp:ListView ID="lstvwCareerDetails" DataKeyNames="JobId" runat="server" ViewStateMode="Enabled"
                                        OnItemCommand="lstvwCareerDetails_ItemCommand" >
                                            <LayoutTemplate>
                                                <table id="tblNoticeDetails" runat="server" align="center" cellpadding="0" cellspacing="1"
                                                    class="GridBorder" width="100%">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="left" class="paddingLSML" width="10%;">
                                                            Post
                                                        </th>
                                                        <th align="center" width="7%">
                                                           Qualification
                                                        </th>
                                                         <th align="center" class="paddingL" style="width: 10%;">
                                                           Experience 
                                                        </th>
                                                        <th id ="thFileName" runat = "server" align="left" class="paddingL" style="width: 4%;" > 
                                                          Sort Order
                                                        </th>
                                                        <th style="width: 4%">
                                                            <asp:Label ID="Label1" runat="server" Text="Is On Live?"></asp:Label>
                                                        </th>
                                                        <th align="center" style="width: 2%;">
                                                            Edit
                                                        </th>
                                                        <th align="center" style="width: 2%;">
                                                            Delete
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="trItemtemplate" runat="server" class="ClsGridRow">
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblJobtitle" runat="server" Text='<%# Eval("JobTitle") %>'>
                                                        </asp:Label>
                                                    </td>
                                                    <td align="center" class="paddingL">
                                                        <asp:Label ID="lblQualification" runat="server" Text='<%# Eval("Qualification") %>'>
                                                        </asp:Label>
                                                    </td>
                                                     <td align="center" class="paddingL">
                                                        <asp:Label ID="lblExperience" runat="server" Text='<%# Eval("Experience") %>'>
                                                        </asp:Label>
                                                    </td>
                                                    <td  align="center" class="paddingL" >
                                                       <asp:Label ID="lblSortOrder" runat="server" Text='<%# Eval("SortOrder") %>'>
                                                        </asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkSelect" runat="server" ToolTip="Select Career openings to display under School Career Openings."
                                                            Checked='<%# Eval("IsSelected") %>'></asp:CheckBox>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" CommandName="UpdateCareerDetails" CausesValidation="false"
                                                            ToolTip="Edit" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnDelete" runat="server" CommandName="DeleteCareerDetails" CausesValidation="false"
                                                            ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" OnClientClick="return ConfirmDelete()" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="trItemtemplate" align="center" runat="server" class="ClsGridAltRow">
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblJobtitle" runat="server" Text='<%# Eval("JobTitle") %>'>
                                                        </asp:Label>
                                                    </td>
                                                    <td align="center" class="paddingL">
                                                        <asp:Label ID="lblQualification" runat="server" Text='<%# Eval("Qualification") %>'>
                                                        </asp:Label>
                                                    </td>
                                                     <td align="center" class="paddingL">
                                                        <asp:Label ID="lblExperience" runat="server" Text='<%# Eval("Experience") %>'>
                                                        </asp:Label>
                                                    </td>
                                                     <td align="center" class="paddingL">
                                                       <asp:Label ID="lblSortOrder" runat="server" Text='<%# Eval("SortOrder") %>'>
                                                        </asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkSelect" runat="server" ToolTip="Select Career openings to display under School Career Openings."
                                                            Checked='<%# Eval("IsSelected") %>'></asp:CheckBox>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCareerDetails"
                                                            ImageUrl="../images/IconGrid_Edit.GIF" ToolTip="Edit" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="DeleteCareerDetails"
                                                            ImageUrl="../images/IconGrid_Delete.GIF" ToolTip="Delete" OnClientClick="return ConfirmDelete()" />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr style="width: 800px">
                                                    <td align="center" class="LblNoRecord">
                                                        No record found.
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       <asp:HiddenField ID="hidJobId" runat="server" Value="0"  />
                                       <asp:HiddenField ID="hidRowNo" runat="server" Value="0" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trSave" runat="server">
                    <td align="center">
                        <table>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnSaveSelected" runat="server" Text="Save" CssClass="ClsBtn" CausesValidation="false"
                                        disable-page="true" onclick="btnSaveSelected_Click" />
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
 <script type="text/javascript" language="javascript" >
 </script>
    <script src="../Scripts/Admin/CareerOpenings.js" type="text/javascript"></script>
</asp:Content>