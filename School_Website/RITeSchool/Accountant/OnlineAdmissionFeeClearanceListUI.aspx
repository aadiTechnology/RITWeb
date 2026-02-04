<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="OnlineAdmissionFeeClearanceListUI.aspx.cs" Inherits="OnlineAdmissionFeeClearanceListUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="98%" align="center">
            <tr>
                <td align="center" valign="top">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblSuccessMsg" runat="server" CssClass="LblNormalImg" Font-Bold="true"
                                Font-Size="Small" ForeColor="Blue" Visible="true"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblError" runat="server" Visible="false" CssClass="ClsMdtStar"></asp:Label>
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="LblNormal" ValidationGroup="Save" />
                            <asp:CustomValidator ID="cstClearanceDate" Display="None" runat="server" CssClass="ClsMdtStar"
                                Visible="true" ErrorMessage="TPSLTransactionID should not be blank." ClientValidationFunction="ValidateGridControls"
                                ValidationGroup="Save"></asp:CustomValidator>
                                   <asp:CustomValidator ID="cstDate" Display="None" runat="server" CssClass="ClsMdtStar"
                                Visible="true" 
                                ValidationGroup="Save"></asp:CustomValidator>
                            <asp:CustomValidator ID="cstDepositBankValidator" runat="server" Display="None" ValidationGroup="Save"
							                     ClientValidationFunction="ValidateDepositBank" />    
                            <asp:CustomValidator ID="cstAcValidateClearanceDate"
							                     runat="server"
							                     Display="None"
							                     ClientValidationFunction="AccountsValidateClearanceDate"
							                     ValidationGroup="Save"
							                     EnableClientScript="true" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="100%" align="center">
                        <tr>
                            <td colspan="3" width="100%">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <table align="center" cellpadding="1" cellspacing="2" width="100%">
                                            <tr>
                                                <td class="ClsBorderlight" valign="top" width="5%">
                                                    <asp:RadioButton ID="optTransactionNumber" runat="server" GroupName="Filter" AutoPostBack="true"
                                                        Checked="true" TabIndex="1" OnCheckedChanged="optTransactionNumber_CheckedChanged" />
                                                </td>
                                                <td valign="top" class="ClsBorderlight" width="30%">
                                                    <span class="ClsLabel">TPSLTransaction ID :</span>
                                                </td>
                                                <td valign="top" align="left" width="70%">
                                                    <asp:TextBox ID="txtTransactionIDNumber" runat="server" CssClass="MidTxtBox" MaxLength="30"
                                                        onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                        onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                                        ondrop="event.returnValue=false;" TabIndex="2"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="Tr1">
                                                <td align="center" class="HilightBGGray" colspan="5">
                                                    <img src="../images/ArrowBlueDblRev.gif" /><span class="ClsHilightTextB">OR </span>
                                                    <img src="../images/ArrowBlueDblNw.gif" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight" valign="top">
                                                    <asp:RadioButton ID="optFormNo" runat="server" AutoPostBack="true" GroupName="Filter"
                                                        OnCheckedChanged="optFormNo_CheckedChanged" TabIndex="3" />
                                                </td>
                                                <td class="ClsBorderlight" valign="top">
                                                    <span class="ClsLabel">Form Number / Student Name :</span>
                                                </td>
                                                <td align="left" valign="top">
                                                    <asp:TextBox ID="txtFormNo" runat="server" CssClass="MidTxtBox" MaxLength="50" TabIndex="4"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="Tr2">
                                                <td align="center" class="HilightBGGray" colspan="5">
                                                    <img src="../images/ArrowBlueDblRev.gif" /><span class="ClsHilightTextB">OR </span>
                                                    <img src="../images/ArrowBlueDblNw.gif" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="1" valign="top" class="ClsBorderlight">
                                                    <asp:RadioButton ID="optStandardName" runat="server" AutoPostBack="true" GroupName="Filter"
                                                        OnCheckedChanged="optStandardName_CheckedChanged" TabIndex="5" />
                                                </td>
                                                <td class="ClsBorderlight" valign="top">
                                                    <span class="ClsLabel">Standard Name :</span>
                                                </td>
                                                <td align="left" valign="top">
                                                    <asp:DropDownList ID="cmbStandardName" runat="server" CssClass="MidCombo" TabIndex="6"
                                                        AppendDataBoundItems="True">
                                                       
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                             <tr id="Tr4">
												<td align="center" class="HilightBGGray" colspan="5">
													<img src="../images/ArrowBlueDblRev.gif" /><span class="ClsHilightTextB">OR </span>
													<img src="../images/ArrowBlueDblNw.gif" />
												</td>
											</tr>
                                            <tr>
												<td colspan="1" valign="top" class="ClsBorderlight" style="width: 12%">
													<asp:RadioButton ID="optPaymentDate" runat="server" ViewStateMode="Enabled" AutoPostBack="true" GroupName="Filter"
													                 OnCheckedChanged="optPaymentDate_CheckedChanged" />
												</td>
												<td valign="top" colspan="2">
													<table width="100%">
														<tr>
															<td class="ClsBorderlight" style="width: 205px">
																<asp:Label class="ClsLabel" runat="server" ViewStateMode="Enabled" ID="lblPaymentDate" Text="Payment Start Date :" />
															</td>
															<td align="left" valign="top" style="width: 194px">
																<asp:TextBox ID="txtPaymentStartDate" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox" MaxLength="11"
																             TabIndex="2"></asp:TextBox>
																<rjs:PopCalendar ID="cFromDate" runat="server" ViewStateMode="Enabled" Control="txtPaymentStartDate" Format="dd MMM yyyy" Culture="en"
																                 ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid from date."
																                 ControlFocusOnError="True" />																
															</td>
															<td class="ClsBorderlight" style="width: 144px">
																<span class="ClsLabel">End Date :</span>
															</td>
															<td align="left" valign="top" style="width: 194px">
																<asp:TextBox ID="txtPaymentEndDate" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox" MaxLength="11"
																             TabIndex="3"></asp:TextBox>
																<rjs:PopCalendar ID="cToDate" runat="server" ViewStateMode="Enabled" Control="txtPaymentEndDate" Format="dd MMM yyyy" Culture="en"
																                 ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid to date." />																
															</td>
                                                           <%-- <td id ="tdPaymentBankName" runat ="server" viewstatemode="Enabled" class="ClsBorderlight" style="width: 144px">
																<span  class="ClsLabel" >Bank Name :</span>
															</td>
															<td id ="tdcmbPaymentBankName" runat ="server" viewstatemode="Enabled" align="left" valign="top">
																<asp:DropDownList ID="cmbPaymentBank" AutoPostBack="true" CssClass="LrgCombo"
																					                  runat="server" ViewStateMode="Enabled" >
																					</asp:DropDownList>																
															</td>--%>
														</tr>
													</table>
												</td>
											</tr>
                                            <tr id="Tr3">
												<td align="center" class="HilightBGGray" colspan="5">
													<img src="../images/ArrowBlueDblRev.gif" /><span class="ClsHilightTextB">OR </span>
													<img src="../images/ArrowBlueDblNw.gif" />
												</td>
											</tr>
                                            <tr>
												<td colspan="1" valign="top" class="ClsBorderlight" style="width: 12%">
													<asp:RadioButton ID="optClearanceDate" runat="server" ViewStateMode="Enabled" AutoPostBack="true" GroupName="Filter"
													                 OnCheckedChanged="optClearanceDate_CheckedChanged" />
												</td>
												<td valign="top" colspan="2">
													<table width="100%">
														<tr>
															<td class="ClsBorderlight" style="width: 206px">
																<span class="ClsLabel">Clearance Start Date:</span>
															</td>
															<td align="left" valign="top" style="width: 194px">
																<asp:TextBox ID="txtClearanceStartDate" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox" MaxLength="11"
																             TabIndex="4"></asp:TextBox>
																<rjs:PopCalendar ID="calClearanceStartDate" runat="server" ViewStateMode="Enabled" Control="txtClearanceStartDate" Culture="en"
																                 Format="dd MMM yyyy" ShowWeekend="True" Enabled="true" ShowErrorMessage="false"
																                 InvalidDateMessage="Please select valid from date." ControlFocusOnError="True" />																
															</td>
															<td class="ClsBorderlight" style="width: 146px">
																<span class="ClsLabel">End Date :</span>
															</td>
															<td align="left" valign="top" style="width: 194px">
																<asp:TextBox ID="txtClearanceEndDate" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox" MaxLength="11"
																             TabIndex="5"></asp:TextBox>
																<rjs:PopCalendar ID="calClearanceEndDate" runat="server" ViewStateMode="Enabled" Control="txtClearanceEndDate" Culture="en"
																                 Format="dd MMM yyyy" ShowWeekend="True" Enabled="true" ShowErrorMessage="false"
																                 InvalidDateMessage="Please select valid to date." />																
															</td>
                                                           <%-- <td class="ClsBorderlight" style="width: 146px">
																<span class="ClsLabel">Bank Name :</span>
															</td>
															<td align="left" valign="top">
																<asp:DropDownList ID="cmbClearanceBank" AutoPostBack="true" CssClass="LrgCombo"
																					                  runat="server" ViewStateMode="Enabled" >
																					</asp:DropDownList>															
															</td>--%>
														</tr>
													</table>
												</td>
											</tr>
                                            <tr>
                                                <td valign="top" class="ClsBorderlight">
                                                    <asp:CheckBox ID="chkIncludeAll" runat="server" AutoPostBack="false" TabIndex="8" />
                                                </td>
                                                <td colspan="2" valign="top" class="ClsBorderlight">
                                                    <span class="ClsLabel">Include transaction records which are cleared.</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" valign="top" colspan="3">
                                                    <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="ClsBtn" TabIndex="7"
                                                        OnClick="btnShow_Click" Width="100px" CausesValidation="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top" colspan="3">
                                                    <table id="Table1" runat="server" width="100%">
                                                        <tr runat="server" id="trTotalRec" align="center" visible="false">
                                                            <td colspan="6">
                                                                <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                                                <span class="LblNormal">To</span>
                                                                <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                                <span class="LblNormal">Out Of</span>
                                                                <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                                                <span class="LblNormal">Records</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" valign="top" colspan="3">
                                                                <asp:GridView ID="grdOnlineAdmissionFeeDetails" runat="server" Width="100%" ForeColor="#333333"
                                                                    AutoGenerateColumns="False" BackColor="White" CssClass="GridBorder" GridLines="None"
                                                                    DataKeyNames="NetBankingPaymentTransactionID,DepositeBankId,Form_Number,StudentAdmissionId,DepositedBankName" CellPadding="0" CellSpacing="1"
                                                                    AllowPaging="True" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataText="No Record Found"
                                                                    TabIndex="8" OnRowDataBound="grdOnlineAdmissionFeeDetails_RowDataBound" OnPageIndexChanging="grdOnlineAdmissionFeeDetails_PageIndexChanging">
                                                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                                    </PagerStyle>
                                                                    <Columns>
                                                                        <asp:BoundField HeaderText="Form No." DataField="Form_Number">
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="paddingLSML"
                                                                                Width="8%" />
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" CssClass="paddingLSML" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Student Name" DataField="StudentName">
                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Width="22%" />
                                                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="false" CssClass="paddingLSML" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Standard" DataField="Standard_Name" >
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"  Width="8%"  CssClass="paddingLSML"/>
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" CssClass="paddingLSML"/>
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="TPSLTransac. ID" >
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtTSPLTransactionID" runat="server" CssClass="SmlTxtBox" Width=" 100px"
                                                                                    TabIndex="9" MaxLength="30" Text='<%#Eval("TPSLTransactionID")%>' onblur="extractNumber(this,0,false);"
                                                                                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                                                    onpaste="event.returnValue=false;" ondrop="event.returnValue=false;">
                                                                                </asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="18%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="paddingLSML"/>
                                                                            <HeaderStyle Width="18%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="paddingLSML" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField HeaderText="Bank" DataField="RegisterdBankName">
                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Width="15%" />
                                                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="true" CssClass="paddingLSML" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Amount" DataField="Amount">
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"   Width="12%" />
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Transaction Dt." DataField="TransactionDateTime">
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" CssClass="paddingLSML" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Clearance Dt.">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtclearance" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                                                    TabIndex="10" Text='<%#Eval("ClearanceDate","{0:dd-MMM-yyyy}")%>'></asp:TextBox>
                                                                                <rjs:PopCalendar ID="cClrDate" runat="server" Control="txtclearance" Format="dd MMM yyyy"
                                                                                    ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid to date." />
                                                                            </ItemTemplate>
                                                                             <ItemStyle Width="17%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            <HeaderStyle Width="17%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Deposited Bank Name">
																		<ItemTemplate>
																			<asp:DropDownList ID="ddlDepositedBankList" runat="server" CssClass="MidCombo" />
																		</ItemTemplate>
																		<ItemStyle Width="150px" HorizontalAlign="Center" VerticalAlign="Middle" />
																		<HeaderStyle Width="150px" HorizontalAlign="Center" VerticalAlign="Middle" />
																	</asp:TemplateField>
                                                                    </Columns>
                                                                    <RowStyle CssClass="ClsGridRow" />
                                                                    <HeaderStyle CssClass="ClsGridHeader" />
                                                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                                                    <PagerTemplate>
                                                                        <table width="100%" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td width="70%" align="left" class="ClsBorderPager" valign="middle">
                                                                                    <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                    <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" CssClass="LblNormal"
                                                                                        OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td width="30%" align="right" class="ClsBorderPager" valign="middle">
                                                                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </PagerTemplate>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td>
                                                                <table align="center" id="tblTotalAmount" runat="server" visible="false">
                                                                    <tr>
                                                                        <td style="background-color: #e4efc4;" align="left">
                                                                           <span class="LblNrmlB" style="width: 200px">Total Amount :</span>
                                                                        </td>
                                                                        <td align="left" style="background-color: #eaeaea">
                                                                            <asp:Label ID="lblTotalAmount" Width=" 75px" runat="server" CssClass="ClsHilightFeeL" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:HiddenField ID="hidPageNo" runat="server" />
                                                    <asp:HiddenField ID="hidRowCnt" runat="server" />
                                                    <asp:HiddenField ID="hidServerDate" runat="server" />
                                                    <asp:HiddenField ID="hidCurrentDate" runat="server" />
                                                    <asp:HiddenField ID="hidFinancialYearJSON" runat="server" />
													<asp:HiddenField ID="hidCanEditOldFinancialYear" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" width="46%" >
                                <asp:Button ID="btnSave" Text="Save" CssClass="ClsBtn" runat="server" ValidationGroup="Save"
                                    TabIndex="11" OnClick="btnSave_Click" />
                                <asp:Button ID="btnExport" Text="Export" CssClass="ClsBtn" runat="server" CausesValidation="false"
                                    TabIndex="12" onclick="btnExport_Click" />
                           </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">

        _clienthidRowCnt = "<%=this.hidRowCnt.ClientID %>"
        _clientGrdId = "<%=this.grdOnlineAdmissionFeeDetails.ClientID %>"
        _clientlblSuccessMsg = "<%=this.lblSuccessMsg.ClientID %>"
        _clientlblErrorId = "<%=this.lblError.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnShow = "<%=this.btnShow.ClientID %>"
        _clienthidPageNo = "<%=this.hidPageNo.ClientID %>"
        _clientvalSumErrorMsgId = "<%=this.valSumErrorMsg.ClientID %>"
        _clienthidCurrentDate = "<%=this.hidCurrentDate.ClientID%>"
        _clientcstDate = "<%=this.cstDate.ClientID%>"
        _clientbtnExport = "<%=this.btnExport.ClientID %>"        

        // Financial year related
        var _FinancialYear = eval('[' + $get('<%= this.hidFinancialYearJSON.ClientID %>').value + ']')[0];
        var _CanEditOldFinancialYear = Boolean($get('<%= this.hidCanEditOldFinancialYear.ClientID %>').value == 'true');

        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)

        function EndReqHandler(sender, args) {
            
            var postBackElement = sender._postBackSettings.sourceElement
            if (postBackElement != null && postBackElement.id == _clientbtnShow) {
                if (postBackElement.value == "Show") {
                    if (document.getElementById(_clientGrdId) != undefined && document.getElementById(_clientGrdId) != null) {
                        var iCount = document.getElementById(_clientGrdId).rows.length - 1
                        if (iCount > 0) {
                            document.getElementById(_clientbtnSave).style.visibility = "inherit"
                            document.getElementById(_clientbtnExport).style.visibility = "inherit"
                        }
                    }
                }
                if (postBackElement.value == "Change Input") {

                    if (document.getElementById(_clientlblSuccessMsg) != undefined) {
                        document.getElementById(_clientlblSuccessMsg).innerHTML = ""
                    }
                    document.getElementById(_clientbtnSave).style.visibility = "Hidden"
                    document.getElementById(_clientbtnExport).style.visibility = "Hidden"
                }
            }
        }
        function ClearValSum() {
            
            if (document.getElementById(_clientvalSumErrorMsgId) != null)
                document.getElementById(_clientvalSumErrorMsgId).style.display = "none"
            if (document.getElementById(_clientlblErrorId) != undefined) {
                document.getElementById(_clientlblErrorId).innerHTML = ""
            }
            return true
        }

        function MessageAboutDate(oCmb) {
            var bIsValid
            if (window.confirm('If you change the page then selected date and entered TPSLTransactionID number from current page will be lost. Do you want to continue?'))
                bIsValid = true
            else {
                document.getElementById(oCmb).value = document.getElementById(_clienthidPageNo).value
                bIsValid = false
            }
            return bIsValid
        }

        function ValidateDepositBank(src, args) {
            args.IsValid = true;
            var iRowNos = [];
            var txtClearanceDate, ddlDepositedBank;
            $('tr', $('#' + _clientGrdId))
				.each(function (index) {
				    if (!(this.className == "ClsGridRow" || this.className == "ClsGridAltRow"))
				        return;

				    txtClearanceDate = $('input[id$="_txtclearance"]', this)[0];
				    ddlDepositedBank = $('select[id$="_ddlDepositedBankList"]', this)[0];

				    if (ddlDepositedBank && txtClearanceDate.value.trim() != '' && ddlDepositedBank.value == '0')
				        iRowNos.push(index);
				});

            if (iRowNos.length > 0) {
                args.IsValid = false;
                src.errormessage = "Deposited Bank Name should be selected for row(s) : " + iRowNos.join(', ');
            }
            return !args.IsValid;
        }

        function ValidateGridControls(oSrc, args) {
            
            document.getElementById(_clienthidCurrentDate).value = new Date().format("dd-MMM-yyyy")
            if (document.getElementById(_clientlblSuccessMsg) != undefined) {
                document.getElementById(_clientlblSuccessMsg).innerHTML = ""
            }
            if (document.getElementById(_clientlblErrorId) != undefined) {
                document.getElementById(_clientlblErrorId).innerHTML = ""
            }
            oSrc.errormessage = ""
            var iRowCount = document.getElementById(_clienthidRowCnt).value
            var TodayDate = document.getElementById(_clienthidCurrentDate).value
            var iRowChequeNo=""
            var iRowNoP = ""
            var iRowNos = ""
            var dtToday

            for (i = 1; i <= iRowCount; i++) 
            {
                if (i < 9) 
                {
                    sRow = "_ctl0" + (i + 1) + "_txtclearance"
                    var TransactionDate = document.getElementById(_clientGrdId).rows[i].cells[6].innerHTML
                    var txtClearanceDate = document.getElementById(_clientGrdId + sRow)

                    if ((TransactionDate).value != "" && (txtClearanceDate).value != "") 
                    {
                        var sDate = TransactionDate.split("");
                        var DateString = ""
                        for (j = 0; j <= 10; j++) 
                        {
                            if (j == 2 || j == 6)
                                sDate[j] = "-"
                            DateString += sDate[j]
                        }
                        var DateOfTransaction = new Date(convertvaliddate(DateString))
                        var DateOfClearance = new Date(convertvaliddate(txtClearanceDate.value))
                        if (document.all)
                            dtToday = new Date(TodayDate.replace('-', ' '))
                        else
                            dtToday = new Date(convertdate(TodayDate))

                        if (DateOfTransaction > DateOfClearance)
                            iRowNos += i.toString() + ", "
                        else if (dtToday < DateOfClearance)
                            iRowNoP += i.toString() + ", "
                   
                    }
                    sRow1 = "_ctl0" + (i + 1) + "_txtTSPLTransactionID"
                    txtTSPLTransactionID = document.getElementById(_clientGrdId + sRow1)
                    if ((txtTSPLTransactionID).value == "")
                        iRowChequeNo += i.toString() + ", "                  
                }
                else {
                    sRow = "_ctl" + (i + 1) + "_txtclearance"
                    sRow1 = "_ctl" + (i + 1) + "_txtTSPLTransactionID"
                    var TransactionDate = document.getElementById(_clientGrdId).rows[i].cells[6].innerHTML
                    var txtClearanceDate = document.getElementById(_clientGrdId + sRow)

                    if ((TransactionDate).value != "" && (txtClearanceDate).value != "") {
                        var sDate = TransactionDate.split("");
                        var DateString = ""
                        for (j = 0; j <= 10; j++) {
                            if (j == 2 || j == 6)
                                sDate[j] = "-"
                            DateString += sDate[j]
                        }
                        var DateOfTransaction = new Date(convertvaliddate(DateString))
                        var DateOfClearance = new Date(convertvaliddate(txtClearanceDate.value))
                        if (document.all)
                            dtToday = new Date(TodayDate.replace('-', ' '))
                        else
                            dtToday = new Date(convertdate(TodayDate))

                        if (DateOfTransaction > DateOfClearance)
                            iRowNos += i.toString() + ", "
                        else if (dtToday < DateOfClearance)
                            iRowNoP += i.toString() + ", "
                            
                    }
                    sRow1 = "_ctl" + (i + 1) + "_txtTSPLTransactionID"
                    txtTSPLTransactionID = document.getElementById(_clientGrdId + sRow1)
                    if ((txtTSPLTransactionID).value == "")
                        iRowChequeNo += i.toString() + ", "
                }
            }

            if (iRowChequeNo != "") {
               
                iRowChequeNo = iRowChequeNo.substring(0, iRowChequeNo.lastIndexOf(","))
                oSrc.errormessage = "TSPLTransactionID should not be blank for row(s) : " + iRowChequeNo + "<br/>"
                if (iRowNos != "") {
                    iRowNos = iRowNos.substring(0, iRowNos.lastIndexOf(","))
                    oSrc.errormessage += "Clearance date should be greater than transaction date for row(s) : " + iRowNos + "<br/>"
                }
                if (iRowNoP != "") {
                    iRowNoP = iRowNoP.substring(0, iRowNoP.lastIndexOf(","))
                    oSrc.errormessage += "Clearance date should not be future date for row(s) : " + iRowNoP + "<br/>"
                }
                args.IsValid = false
                return true
            }
            if (iRowNos != "") {
                iRowNos = iRowNos.substring(0, iRowNos.lastIndexOf(","))
                oSrc.errormessage += "Clearance date should be greater than transaction date for row(s) : " + iRowNos + "<br/>"
                args.IsValid = false
               return true
            }
            if (iRowNoP != "") {
                iRowNoP = iRowNoP.substring(0, iRowNoP.lastIndexOf(","))
                oSrc.errormessage += "Clearance date should not be future date for row(s) : " + iRowNoP + "<br/>"
                args.IsValid = false
                return true
            }
            if (args.IsValid == false)
                return true
            else    
                return false


        }

        function AccountsValidateClearanceDate(src, args) {
            args.IsValid = true;
            if (!_FinancialYear)
                return;

            if (_FinancialYear.IsClosed && !_CanEditOldFinancialYear) {
                args.IsValid = false;
                src.errormessage = 'Financial year is closed and you do not have edit access.';
            }
            else {
                var dtFinancialYearStartDate = new Date(parseInt(_FinancialYear.StartDate.replace("/Date(", "").replace(")/", ""), 10));
                var dtFinancialYearEndDate = new Date(parseInt(_FinancialYear.EndDate.replace("/Date(", "").replace(")/", ""), 10));
                var clearanceDate;
                var iRowNos = [];
                $('tr', $('#' + _clientGrdId))
					.each(function (index) {
					    if (!(this.className == "ClsGridRow" || this.className == "ClsGridAltRow"))
					        return;

					    clearanceDate = $('input[id$="_txtclearance"]', this)[0].value.replace(/[-\.]/g, ' ');

					    if (!clearanceDate || clearanceDate == '')
					        return;

					    clearanceDate = new Date(clearanceDate);

					    if (clearanceDate < dtFinancialYearStartDate || clearanceDate > dtFinancialYearEndDate)
					        iRowNos.push(index);
					});
                if (iRowNos.length > 0) {
                    args.IsValid = false;
                    src.errormessage = 'Clearance date should be within current financial year (i.e. from 1-April-' + dtFinancialYearStartDate.getFullYear() + ' to 31-March-' + dtFinancialYearEndDate.getFullYear() + ') for row(s) : ' + iRowNos.join(', ');
                }
            }
            return !args.ISValid;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
