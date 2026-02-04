<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="OnlineBankDetails.aspx.cs" Inherits="OnlineBankDetails" %>
    <%@ OutputCache Duration="200" VaryByParam="none" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div style="width: 97%; height: 100%" align="center">
        <table class="paddingLR" cellspacing="2" cellpadding="0" border="0" style="width: 100%;
            height: 100%">
            <tr>
                <td class="ClsGrayMainTitle" style="height: 20px; width: 100%;" align="left">
                    <asp:Label ID="lblHeader" Text="Online Bank / Card Details" runat="server" CssClass="MainTitleHead"
                        Font-Bold="True"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">
                    <table width="100%">
                        <tr style="width:100%">
                            <td  align="left" >
                            <div style="background-color:#D8CCBC;font-family:verdana;color:#000762;font-size:12px;font-weight:700;border-style:solid;border-width:1px;padding:5px; width:100%"">
                            <asp:Label ID="lblNote1" runat="server" Text="
                                For Online Fee Payment, you need to confirm payment amount and proceed further to
                                make payment through Internet Banking. Please make sure you know your Net banking
                                USER ID and PASSWORD. The Internet banking is available for the selected banks / cards only.
                                Here is the list of the same." CssClass="LblUsrNameHead"></asp:Label>
                            <asp:Label ID="lblNote2" runat="server" Text="
                                For Online Fee Payment, you need to confirm payment amount and proceed further to
                                make payment through Internet Banking. Please make sure you know your Net banking
                                USER ID and PASSWORD. Banks may differ as bank selection will happen at payment gateway." CssClass="LblUsrNameHead"></asp:Label>
                                </div>
                            </td>
                        </tr>
                         <tr id="trServiceTaxNote" runat="server">
                                    <td  >
                                    <div style="background-color:#D8CCBC;font-family:verdana;color:#000762;font-size:12px;font-weight:700;border-style:solid;border-width:1px;padding:5px; width:100%">
                                    <asp:Label ID="Label5" runat="server" Text=" Note :-" CssClass="LblUsrNameHead"></asp:Label>
                                    <asp:Label ID="Label6" runat="server" Text="The Service Tax is applicable only on Processing Charges."  ForeColor= "Red" CssClass="LblUsrNameHead"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                        <tr>
                            <td align="left">
                               <span class="ClsLblLgnd" style="font-weight: bold">Bank(s) : </span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="LblUsrNameHead">
                                <table cellpadding="3" cellspacing="1" width="100%">
                                    <tr id="trLstReqItems" runat="server" visible="true">
                                        <td valign="top" width="100%">
                                            <asp:ListView ID="lstvwBankDetails" runat="server" DataKeyNames="">
                                                <LayoutTemplate>
                                                    <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                        cellspacing="1" class="GridBorder">
                                                        <tr>
                                                            <td>
                                                                <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                    cellspacing="1">
                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                        <th align="center" class="ClspaddingL">
                                                                            No.
                                                                        </th>
                                                                        <th align="left" class="ClspaddingL">
                                                                            Bank Name
                                                                        </th>
                                                                        <th align="left" class="ClspaddingL">
                                                                            Processing Charges
                                                                        </th>
                                                                          <th align="left" class="ClspaddingL">
                                                                            Service Tax
                                                                        </th>
                                                                        <th align="center" class="ClspaddingL">
                                                                            No.
                                                                        </th>
                                                                        <th align="left" class="ClspaddingL">
                                                                            Bank Name
                                                                        </th>
                                                                        <th align="left" class="ClspaddingL">
                                                                            Processing Charges
                                                                        </th>
                                                                        <th align="left" class="ClspaddingL">
                                                                            Service Tax
                                                                        </th>
                                                                    </tr>
                                                                    <tr id="itemPlaceholder" runat="server">
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr id="Tr2" runat="server" class="ClsGridRow">
                                                        <td align="center" class="ClspaddingMidT">
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("OrginalRowNo") %>' CssClass="ClspaddingMidT" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblFeeType" runat="server" Text='<%# Eval("RegisterdBankName") %>'
                                                                CssClass="ClspaddingL" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblPaybleFor" runat="server" Text='<%# Eval("ProcessingCharge") %>'
                                                                CssClass="ClspaddingL" />
                                                        </td>
                                                         <td class="paddingLR" align="left">
                                                            <asp:Label ID="Label6" runat="server" Text='<%# Eval("ServiceTaxInPercentInWord") %>'
                                                                CssClass="ClspaddingL" />
                                                        </td>
                                                        <td align="center">
                                                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("OrginalRowNoSecond") %>' CssClass="ClspaddingMidT" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("RegisterdBankNameSecond") %>'
                                                                CssClass="ClspaddingL" />
                                                        </td>
                                                        <td class="paddingLR" align="left">
                                                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("ProcessingChargeSecond") %>'
                                                                CssClass="ClspaddingL" />
                                                        </td>
                                                          <td class="paddingLR" align="left">
                                                            <asp:Label ID="Label5" runat="server" Text='<%# Eval("ServiceTaxInPercentInWordSecond") %>'
                                                                CssClass="ClspaddingL" />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                        <td align="center" class="ClspaddingMidT">
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("OrginalRowNo") %>' CssClass="ClspaddingMidT" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblFeeType" runat="server" Text='<%# Eval("RegisterdBankName") %>'
                                                                CssClass="ClspaddingL" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblPaybleFor" runat="server" Text='<%# Eval("ProcessingCharge") %>'
                                                                CssClass="ClspaddingL" />
                                                        </td>
                                                         <td class="paddingLR" align="left">
                                                            <asp:Label ID="Label8" runat="server" Text ='<%# Eval("ServiceTaxInPercentInWord") %>'
                                                                CssClass="ClspaddingL" />
                                                        </td>
                                                        <td align="center">
                                                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("OrginalRowNoSecond") %>' CssClass="ClspaddingMidT" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("RegisterdBankNameSecond") %>'
                                                                CssClass="ClspaddingL" />
                                                        </td>
                                                        <td class="paddingLR" align="left">
                                                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("ProcessingChargeSecond") %>'
                                                                CssClass="ClspaddingL" />
                                                        </td>
                                                        <td class="paddingLR" align="left">
                                                            <asp:Label ID="Label7" runat="server" Text ='<%# Eval("ServiceTaxInPercentInWordSecond") %>'
                                                                CssClass="ClspaddingL" />
                                                        </td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                            </asp:ListView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr id="trCardGateway" runat="server">
                            <td align="left">
                               <span class="ClsLblLgnd" style="font-weight: bold">Card(s) : </span>
                            </td>
                        </tr>
                        <tr id="trCardDetails" runat="server">
                            <td align="left" class="LblUsrNameHead">
                                <table cellpadding="3" cellspacing="1" width="100%">
                                    <tr id="tr1" runat="server" visible="true">
                                        <td valign="top" width="100%">
                                            <asp:ListView ID="lstvwCardDetails" runat="server" DataKeyNames="">
                                                <LayoutTemplate>
                                                    <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                        cellspacing="1" class="GridBorder">
                                                        <tr>
                                                            <td>
                                                                <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                    cellspacing="1">
                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                       <th align="center" class="ClspaddingL">
                                                                            No.
                                                                        </th>
                                                                        <th align="left" class="ClspaddingL">
                                                                            Bank Name
                                                                        </th>
                                                                        <th align="left" class="ClspaddingL">
                                                                            Processing Charges
                                                                        </th>
                                                                         <th align="left" class="ClspaddingL">
                                                                            Service Tax
                                                                        </th> 
                                                                        <th align="center" class="ClspaddingL">
                                                                            No.
                                                                        </th>
                                                                        <th align="left" class="ClspaddingL">
                                                                            Bank Name
                                                                        </th>
                                                                        <th align="left" class="ClspaddingL">
                                                                            Processing Charges
                                                                        </th>  
                                                                        <th align="left" class="ClspaddingL">
                                                                            Service Tax
                                                                        </th>                                           
                                                                    </tr>
                                                                    <tr id="itemPlaceholder" runat="server">
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr id="Tr2" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                        <td align="center" class="ClspaddingMidT">
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("OrginalRowNo") %>' CssClass="ClspaddingMidT" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblFeeType" runat="server" Text='<%# Eval("RegisterdBankName") %>'
                                                                CssClass="ClspaddingL" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblPaybleFor" runat="server" Text ='<%# Eval("ProcessingCharge") %>'
                                                                CssClass="ClspaddingL" />
                                                        </td>
                                                         <td class="paddingLR" align="left">
                                                            <asp:Label ID="Label10" runat="server" Text='<%# Eval("ServiceTaxInPercentInWord") %>'
                                                                CssClass="ClspaddingL" />
                                                        </td>
                                                        <td align="center">
                                                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("OrginalRowNoSecond") %>' CssClass="ClspaddingMidT" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("RegisterdBankNameSecond") %>'
                                                                CssClass="ClspaddingL" />
                                                        </td>
                                                        <td class="paddingLR" align="left">
                                                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("ProcessingChargeSecond") %>'
                                                                CssClass="ClspaddingL" />
                                                        </td>   
                                                          <td class="paddingLR" align="left">
                                                            <asp:Label ID="Label9" runat="server" Text ='<%# Eval("ServiceTaxInPercentInWordSecond") %>'
                                                                CssClass="ClspaddingL" />
                                                        </td>                                     
                                                    </tr>
                                                </ItemTemplate>                                               
                                            </asp:ListView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnClose" runat="server" BorderStyle="Solid" BorderWidth="1px" CssClass="ClsBtnSml"
                                    Text="Close" Visible="True" Width="80px" CausesValidation="false" OnClick="btnClose_Click" />
                                 <asp:Button ID="btnBack" Text="Back" runat="server" CssClass="ClsBtnMid" CausesValidation="False"
								            TabIndex="2" UseSubmitBehavior="false" OnClick="btnBack_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">
        function refreshParent() {
            window.close();
        }
    </script>

</asp:Content>
