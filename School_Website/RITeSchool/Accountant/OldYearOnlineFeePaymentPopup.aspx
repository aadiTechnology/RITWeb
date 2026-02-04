<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="OldYearOnlineFeePaymentPopup.aspx.cs" Inherits="OldYearOnlineFeePaymentPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <asp:UpdatePanel ID="upnl1" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td align="left" colspan="2" rowspan="1">
                            <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                                <tr>
                                    <td style="height: 20px">
                                        <asp:Label ID="lblHeader" runat="server" CssClass="MainTitleHead" Font-Bold="True"
                                            Text="Pay Last Year Fees Online"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                  <tr id="trAcademicYear" runat="server" >
				    <td align="left">
					    <table width="100%">
					        <tr>
					            <td align="left" width="100">
									<asp:Label ID="lblacademicYr" BorderWidth="1px" BorderColor="Silver" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, AcademicYear%>"></asp:Label>
							    </td>
								<td align="left">
							    	<asp:DropDownList ID="cmbAcademicYrId" runat="server" ViewStateMode="Enabled" AutoPostBack="true" CssClass="midCombo"
							            OnSelectedIndexChanged="cmbAcademicYrId_SelectedIndexChanged">
									</asp:DropDownList>
								</td>
					          </tr>
                           </table>
                         </td>
                      </tr>                 
                    <tr>
                        <td align="left">
                            <asp:ValidationSummary ID="ValSUm" runat="server" />
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="At least one fee type should be selected."
                                Display="None" ClientValidationFunction="ValidateFees"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:ListView ID="lstvwFeeDetails" runat="server" OnItemDataBound="lstvwFeeDetails_ItemDataBound" DataKeyNames="Schoolwise_Student_Fee_Id">
                                <LayoutTemplate>
                                    <table width="100%" runat="server" id="tblStudentInfo" style="color: #333333" cellpadding="0"
                                        cellspacing="1" class="GridBorder">
                                        <tr id="trHeader" runat="server" class="ClsMarksGridHeader">
                                            <th id="thchk" runat="server" align="center" width="4%">
                                                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this);" />
                                            </th>
                                            <th id="thFeeType" runat="server" align="left" style="padding-left: 5px">
                                                <asp:Label ID="lblStudent" runat="server" Text="<%$ Resources:LocalizedResources, FeeType %>"
                                                    EnableViewState="False"></asp:Label>
                                            </th>
                                            <th id="thPaybleFor" runat="server" align="left" width="150px" style="padding-left: 5px">
                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, PaybleFor %>"
                                                    EnableViewState="False"></asp:Label>
                                            </th>
                                            <th id="thAmount" runat="server" align="right" width="75px" style="padding-right: 5px">
                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, Amount %>"
                                                    EnableViewState="False"></asp:Label>
                                            </th>
                                            <th id="thDueDate" runat="server" align="center" width="100px">
                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:LocalizedResources, DueDate %>"
                                                    EnableViewState="False"></asp:Label>
                                            </th>
                                            <th id="th1" runat="server" align="center" width="100px">
                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, LateFee %>"
                                                    EnableViewState="False"></asp:Label>
                                            </th>
                                            <th style="width:100px">
                                                <asp:Label ID="Label5" runat="server" Text="Receipt"></asp:Label>
                                            </th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="trlstvwRow" runat="server" class="ClsMarksGridAltRowN">
                                        <td id="tdchk" runat="server" align="center">
                                            <asp:CheckBox ID="chkSelect" runat="server" Visible="false" />
                                        </td>
                                        <td id="tdFeeType" runat="server" align="left" style="padding-left: 5px">
                                            <asp:Label ID="lblFeeType" runat="server" Text='<%# Eval("Fee_Type") %>' />
                                        </td>
                                        <td id="tdPaybleFor" runat="server" align="left" style="padding-left: 5px">
                                            <asp:Label ID="lblPaybleFor" runat="server" Text='<%# Eval("Payable_For") %>' />
                                        </td>
                                        <td id="tdAmount" runat="server" align="right" style="padding-right: 5px">
                                            <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount") %>' />
                                        </td>
                                        <td id="tdDueDate" runat="server" align="center">
                                            <asp:Label ID="lblPaidDate" runat="server" Text='<%#Eval("Paid_Date","{0:dd-MMM-yyyy}")%>' />
                                        </td>
                                        <td id="td1" runat="server" align="center">
                                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("LateFee") %>' />
                                        </td>
                                        <td align="center">                                            
                                            <asp:HyperLink ID="lnkMini" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,Receipt%>" Visible="false" NavigateUrl="FeesMiniReceipt.aspx?"/>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td class="LblNoRecord" align="center">
                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordsFound %>"
                                                    EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnPay" runat="server" Text="Pay" CssClass="ClsBtn" OnClick="btnPay_Click" />
                            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" CausesValidation="false" OnClientClick="window.close()" />
                            <asp:HiddenField ID="hidStudentId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidQueryString" runat="server" Value="" />

                            <asp:HiddenField ID="hidOldStudentId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidOldAcademicYearId" runat="server" Value="0" />
                        </td>
                    </tr>                    
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <script language="javascript" type="text/javascript">

            _clientbtnPay = "<%=this.btnPay.ClientID %>"

            function ValidateFees(oSrc, args) {

                if ($('[type=checkbox][id$=chkSelect]:checked').length == 0) {
                    args.IsValid = false;
                    return true;
                }

                args.IsValid = true;
                return false;
            }

            function CheckAll(obj) {
                var chkAll = $('[id$=chkSelectAll]').attr('checked')

                var checkAll = $("[id$=chkSelectAll]").attr('checked');
                if (checkAll)
                    $("[id$=chkSelect]").attr('checked', checkAll);
                else
                    $("[id$=chkSelect]").removeAttr('checked');
            }

            $(document).ready(function () {
                $("[id$=chkSelect]").click(function () {
                    if ($("[id$=chkSelect]").length == $("[id$=chkSelect]:checked").length)
                        $("[id$=chkSelectAll]").attr('checked', "checked");
                    else $("[id$=chkSelectAll]").removeAttr("checked");
                });

            });

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(EndReqHandler);

            function EndReqHandler(sender, args) {
            
                var postBackElement = sender._postBackSettings.sourceElement;
                var sEncrypt = $get("<%=this.hidQueryString.ClientID %>").value;

                if (postBackElement != null && postBackElement.id == _clientbtnPay) {
                    window.open("PayFeeOnline.aspx?" + sEncrypt, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=850,height=500').focus();
                    return false;
                }

            }

        </script>
    </div>
</asp:Content>
