<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="TransportChargesUI.aspx.cs" Inherits="TransportChargesUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" Runat="Server">
<div class="MainBodyDiv">
        <table width="98%" align="center">
            <tr>
                <td align="center">
                    <table width="98%" align="center">
                        <tr>
                            <td colspan="2" width="100%">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table align="center" cellpadding="1" cellspacing="2" width="100%">
                                            <tr>
                                                <td align="right">
                                                    <span class="ClsMdtStar">*</span>
                                                   <asp:Label ID="lblmandatoryField" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>   
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="lblNormal" />
                                                   
                                                </td>
                                            </tr>
                                           
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <table width="80%" id="tblInput" runat="server">
                                                        <tr>
                                                            <td align="left" class="ClsBorderlight">
                                                                <span class="ClsLabel">User Role :</span>
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="cmbRole" runat="server" CssClass="SmlCombo"  TabIndex="1"
                                                                    AutoPostBack="true" onselectedindexchanged="cmbRole_SelectedIndexChanged">
                                                                </asp:DropDownList>  
                                                                <span class="ClsMdtStar">*</span>         
                                                                <asp:RequiredFieldValidator ID="reqCmbRole" runat="server" Display="None" ControlToValidate="cmbRole"
                                                                CssClass="ClsMdtStar" InitialValue="0" ErrorMessage="User Role should be selected."></asp:RequiredFieldValidator>                                                     
                                                            </td>                                                            
                                                            <td class="ClsBorderlight" valign="middle">
                                                                <asp:Label ID="lblStudNameRegNo" runat="server" class="ClsLabel" Text="Name :"></asp:Label>                                                                 
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="txtName" runat="server" CssClass="LrgTxtBox" MaxLength="50" Enabled="false" TabIndex="2" autocomplete="off"></asp:TextBox>
                                                                <span class="ClsMdtStar">*</span>
                                                            <asp:RequiredFieldValidator ID="reqName" Display="None" runat="server" ErrorMessage="Name should not be blank."
															    ControlToValidate="txtName" SetFocusOnError="true"></asp:RequiredFieldValidator>
															</td>
                                                        </tr>                                                        
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" valign="top" colspan="2">
                                                    <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="ClsBtn" TabIndex="1"
                                                        Width="100px" OnClick="btnShow_Click" />
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trTotalRec" align="center">
                                                <td align="center" colspan="2">
                                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwUser">
                                                        <Fields>
                                                            <asp:TemplatePagerField>
                                                                <PagerTemplate>
                                                                    <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                        CssClass="LblNrmlB" />
                                                                    <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources,To%>" />
                                                                    <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                                        CssClass="LblNrmlB" />
                                                                    <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources,OutOf%>" />
                                                                    <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                                        CssClass="LblNrmlB" />
                                                                    <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources,Records%>"/>
                                                                    <br />
                                                                </PagerTemplate>
                                                            </asp:TemplatePagerField>
                                                        </Fields>
                                                    </asp:DataPager>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" valign="top" colspan="2" width="100%">
                                                    <asp:ListView ID="lstvwUser" runat="server" DataSourcID="objDSUserList" OnDataBound="lstvwUser_DataBound"
                                                        OnItemDataBound="lstvwUser_ItemDataBound" 
                                                        DataKeyNames="UserId,Name,TotalAmount,PendingAmount,IsDeactivated,HasRefund">
                                                        <LayoutTemplate>
                                                            <table width="80%" runat="server" id="tblUserInfo" style="color: #333333" cellpadding="0"
                                                                cellspacing="1" class="GridBorder">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">                                                                    
                                                                    <th align="left" width="20%" style="padding-left: 9px;">
                                                                            <asp:Label ID="lblStudentname" runat="server" Text="Name"></asp:Label>
                                                                    </th>
                                                                     <th id="thPaidDate" runat="server" align="right" style="padding-right: 5px;" width="13%">
                                                                            <asp:Label ID="lblPendingFee" runat="server" Text="Pending Amount"></asp:Label>
                                                                    </th>
                                                                    <th align="right" width="13%" style="padding-right: 5px;">
                                                                      <asp:Label ID="lblTotalPaybales" runat="server" Text="Total Amount"></asp:Label>
                                                                    </th>
                                                                    <td id="thPay" runat="server" align="center" width="4%">
                                                                       <asp:Label ID="lblPay" runat="server" Text="Pay"></asp:Label>
                                                                    </td>
                                                                    <td id="Td1" runat="server" align="center" width="4%">
                                                                       <asp:Label ID="Label1" runat="server" Text="Refund"></asp:Label>
                                                                    </td>                                                                                                                           
                                                                    <td id="thCustomReceipt" runat="server" align="center" width="13%" visible="false">
                                                                        <asp:Label ID="lblCustomreceipt" runat="server" Text="Receipt"></asp:Label>
                                                                    </td>                                                                
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                                <tr class="ClsBorderPager" id="trDataPager">
                                                                    <td colspan="8">
                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwUser" PageSize="20">
                                                                            <Fields>
                                                                                <asp:TemplatePagerField>
                                                                                    <PagerTemplate>
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Label ID="MessageLabel" Text="<%$ Resources:LocalizedResources,SelectPage%>" runat="server" CssClass="LblNrmlB" />
                                                                                                    <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td align="right" class="LblNormal">
                                                                                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </PagerTemplate>
                                                                                </asp:TemplatePagerField>
                                                                            </Fields>
                                                                        </asp:DataPager>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="Tr2" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>                                                                
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblStudentname" runat="server" Text='<%#Eval("Name") %>'></asp:Label>
                                                                </td>
                                                                 <td id="tdPaidDate" runat="server" align="right" style="padding-right: 5px;">
                                                                    <asp:Label ID="lblPendingAmount" runat="server" Text='<%#Eval("PendingAmount") %>'></asp:Label>
                                                                </td>
                                                                <td align="right" style="padding-right: 5px;">
                                                                    <asp:Label ID="lblTotalAmount" runat="server" Text='<%#Eval("TotalAmount") %>'></asp:Label>
                                                                </td>                                             
                                                                <td id="tdPay" runat="server" align="center">
                                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="Pay" TabIndex="2"
                                                                        ImageUrl="../images/IconGrid_Edit.GIF" />
                                                                </td>
                                                                <td id="tdRefund" runat="server" align="center">
                                                                    <asp:ImageButton ID="imgBtnRefund" runat="server" CausesValidation="false" TabIndex="2" CommandName="Refund" Visible ='<%# Convert.ToBoolean(Eval("HasRefund")) %>'  
                                                                        ImageUrl="../images/IconGrid_Edit.GIF" />
                                                                </td>
                                                                <td id="tdReceipt" runat="server" align="center" visible="false">
                                                                    <asp:HyperLink ID="hlnkReceipt" runat="server" Text="Receipt" Visible="true" Enabled="false" TabIndex="2" NavigateUrl="#"> </asp:HyperLink>
                                                                </td>                                                                
                                                            </tr>
                                                        </ItemTemplate>                                                        
                                                        <EmptyDataTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td class="LblNoRecord" align="center">
                                                                       <asp:Label ID="lblNoRecordFound" runat="server" Text="<%$ Resources:LocalizedResources,NoRecordFound%>"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </EmptyDataTemplate>
                                                    </asp:ListView>
                                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                    <asp:HiddenField ID="hidUserId" runat="server" />
                                                    <asp:HiddenField ID="hidRoleId" runat="server" />
                                                    <asp:HiddenField ID="hidPageIndex" runat="server" />                                                    
                                                    <asp:HiddenField ID="hidShow" runat="server" />                                                    
                                                </td>
                                            </tr>
                                         
                                        </table>
                                        <asp:ObjectDataSource TypeName="BusinessLogic.TransportChargesBL" EnablePaging="true" 
                                            ID="objDSUserList" runat="server" SelectMethod="GetUserDetails"
                                            SortParameterName="sortExpression" SelectCountMethod="CountUsers" EnableCaching="false">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                    Type="int32" />
                                                <asp:ControlParameter ControlID="txtName" PropertyName="Text" Name="asName" />                                                
                                                <asp:ControlParameter ControlID="cmbRole" PropertyName="Text" Name="asRole" />                                                   
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">

    function OpenPopup(sQueryString) {
        window.open(sQueryString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=850,height=650').focus();
        return false;
    }

    </script>

   <script language="javascript" type="text/javascript">
       _clienttxtName = '#<%=txtName.ClientID%>';
       _clientcmbRole = '<%=cmbRole.ClientID%>';
       var SchoolId = "<%=miSchoolId %>";
       var AcademicYearId = "<%=miAcademicYearId %>"

       $(document).ready(function () {
           BindAutoCompleteEventForUser(SchoolId, AcademicYearId, _clienttxtName, _clientcmbRole, 1, null, null, null);
       });

       var prm = Sys.WebForms.PageRequestManager.getInstance();
       prm.add_endRequest(EndReqHandler);

       function EndReqHandler(sender, args) {
           BindAutoCompleteEventForUser(SchoolId, AcademicYearId, _clienttxtName, _clientcmbRole, 1, null, null, null);
       }

       function clickButton(e, buttonid) {
           var evt = e ? e : window.event;
           var bt = $get(buttonid);
           if (bt) {
               if (evt.keyCode == 13) {
                   $('ul').hide();
               }
           }
       }

       function SearchSelectedValue(val) {
           txt = document.getElementById("<%=this.txtName.ClientID %>");
           bt = document.getElementById("<%=this.btnShow.ClientID %>");
           SearchResult(txt, val, bt);
       }

   </script>
</asp:Content>


