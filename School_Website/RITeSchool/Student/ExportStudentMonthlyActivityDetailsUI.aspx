<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExportStudentMonthlyActivityDetailsUI.aspx.cs"  MasterPageFile="../MasterPages/MasterPage.master"
    Inherits="ExportStudentMonthlyActivityDetailsUI"  ViewStateMode="Enabled"%>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                     <table width="100%">
                         <tr>
                            <td align="right">
                                <span class="ClsMdtStar">*</span>
                                <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                            </td>
                         </tr>
                         <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="valSum" runat="server" HeaderText="Please correct following errors."
                                            ValidationGroup="Export" />
                                        <asp:RequiredFieldValidator ID="reqcmbMonth" runat="server" Display="None" ControlToValidate="cmbMonth"
                                            InitialValue="0" ErrorMessage="Month should be selected." ValidationGroup="Export"></asp:RequiredFieldValidator>
                                     </ContentTemplate>
                                    <Triggers>
                                       <asp:PostBackTrigger ControlID ="btnExport" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                         <tr>
                            <td align="center" style="height: 100px">
                               <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                   <table>
                                    <tr>
                                       <td align="center" class="ClsBorderlight" style="width: 100px;">
                                                    <span class="ClsLabel">Month :</span>
                                        </td>
                                         <td align="left">
                                             <asp:DropDownList ID="cmbMonth" CssClass="LrgCombo" runat="server" EnableViewState="true">
                                              </asp:DropDownList>
                                               <span class="ClsMdtStar">*</span>
                                          </td>
                                      </tr>
                                    <tr>
                                          <td align="center" class="ClsBorderlight" style="width: 100px;">
                                            <span class="ClsLabel">Category :</span>
                                          </td>
                                          <td align="left">
                                            <asp:DropDownList ID="cmbCategory" runat="server" CssClass="LrgCombo" ViewStateMode="Enabled">
                                             </asp:DropDownList>
                                          </td>
                                      </tr>
                                   </table>
                                </ContentTemplate>
                                <Triggers>                          
                                  <asp:PostBackTrigger ControlID ="btnExport" />
                               </Triggers>
                             </asp:UpdatePanel> 
                           </td>
                        </tr>
                         <tr>
                            <td align="center">
                              <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                  <table>
                                     <tr>
                                        <td align="center">
                                           <asp:Button ID="btnExport" CssClass="ClsBtn" runat="server" Text="Export" OnClick="btnExport_Click"
                                                 ValidationGroup="Export" />
                                       </td>
                                    </tr>
                                 </table>
                              </ContentTemplate>
                              <Triggers>                          
                                  <asp:PostBackTrigger ControlID ="btnExport" />
                              </Triggers>
                            </asp:UpdatePanel>
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


