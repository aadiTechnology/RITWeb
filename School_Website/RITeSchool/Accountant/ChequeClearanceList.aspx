<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChequeClearanceList.aspx.cs"
    MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" Inherits="ChequeClearanceList" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="98%" align="center">
            <tr>
                <td align="center" valign="top">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
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
                <td align="right" style="color: #ff3333" valign="top">
                    <span class="ClsMdtStar">* Mandatory Fields </span>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="lblNormal" ValidationGroup="Show" />
                            <asp:CustomValidator ID="cstForm" Display="None" runat="server" CssClass="ClsMdtStar"
                                Visible="true" ErrorMessage="Cheque number should not be blank." ClientValidationFunction="ValidateControls"
                                ValidationGroup="Show"></asp:CustomValidator>
                            <asp:Label ID="lblError" runat="server" Visible="false" CssClass="ClsMdtStar"></asp:Label>
                            <asp:ValidationSummary ID="valSave" runat="server" CssClass="lblNormal" ValidationGroup="Save" />
                            <asp:CustomValidator ID="cstClearanceDate" Display="None" runat="server" CssClass="ClsMdtStar"
                                Visible="true" ErrorMessage="Cheque number should not be blank." ClientValidationFunction="ValidateGridControls"
                                ValidationGroup="Save"></asp:CustomValidator>
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
                            <td colspan="2" width="100%">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <table align="center" cellpadding="1" cellspacing="2" width="100%">
                                            <tr>
                                                <td class="ClsBorderlight" valign="top" width="5%">
                                                    <asp:RadioButton ID="optChequeNumber" runat="server" GroupName="Filter" AutoPostBack="true"
                                                        Checked="true" OnCheckedChanged="optChequeNumber_CheckedChanged" TabIndex="1" />
                                                </td>
                                                <td valign="top" class="ClsBorderlight" width="30%">
                                                    <span class="ClsLabel">Cheque Number :</span>
                                                </td>
                                                <td valign="top" align="left" width="70%">
                                                    <asp:TextBox ID="txtChequeNumber" runat="server" CssClass="MidTxtBox" MaxLength="6"
                                                        onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                        onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                                        ondrop="event.returnValue=false;" TabIndex="2"></asp:TextBox>
                                                    <asp:Label ID="lblChequeNumberMandMark" runat="server" CssClass="ClsMdtStar" Text="*"
                                                        Height="14px" Width="14px"></asp:Label>
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
                                                    <asp:RadioButton ID="optRegNo" runat="server" AutoPostBack="true" GroupName="Filter"
                                                        OnCheckedChanged="optRegNo_CheckedChanged" />
                                                </td>
                                                <td class="ClsBorderlight" valign="top">
                                                    <span class="ClsLabel">Student Name / Reg. No. :</span>
                                                </td>
                                                <td align="left" valign="top">
                                                    <asp:TextBox ID="txtRegNo" runat="server" CssClass="MidTxtBox" MaxLength="50" TabIndex="3"></asp:TextBox>
                                                    <asp:Label ID="lblRegNoMandMark" runat="server" CssClass="ClsMdtStar" Height="14px"
                                                        Text="*" Width="14px" Visible="False"></asp:Label>
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
                                                    <asp:RadioButton ID="optDate" runat="server" AutoPostBack="true" GroupName="Filter"
                                                        OnCheckedChanged="optDate_CheckedChanged" />
                                                </td>
                                                <td valign="top" colspan="2">
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="ClsBorderlight">
                                                                <span class="ClsLabel">Payment Start Date :</span>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                                    TabIndex="4"></asp:TextBox>
                                                                <rjs:PopCalendar ID="cFromDate" runat="server" Control="txtFromDate" Format="dd MMM yyyy" Culture="en"
                                                                    ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid from date."
                                                                    ControlFocusOnError="True" />
                                                                <asp:Label ID="lblFromDateMandMark" runat="server" CssClass="ClsMdtStar" Height="14px"
                                                                    Text="*" Width="14px" Visible="False"></asp:Label>
                                                            </td>
                                                            <td class="ClsBorderlight">
                                                                <span class="ClsLabel">End Date :</span>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="txtToDate" runat="server" CssClass="SmlTxtBox" MaxLength="11" TabIndex="5"></asp:TextBox>
                                                                <rjs:PopCalendar ID="cToDate" runat="server" Control="txtToDate" Format="dd MMM yyyy" Culture="en"
                                                                    ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid to date."
                                                                    ClientScriptOnDateChanged="SetFocusonTOdate()" />
                                                                <asp:Label ID="lblToDateMandMark" runat="server" CssClass="ClsMdtStar" Height="14px"
                                                                    Text="*" Width="14px" Visible="False"></asp:Label>
                                                            </td>
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
                                                <td colspan="1" valign="top" class="ClsBorderlight">
                                                    <asp:RadioButton ID="optClearanceDate" runat="server" AutoPostBack="true" GroupName="Filter"
                                                        OnCheckedChanged="optClearanceDate_CheckedChanged" />
                                                </td>
                                                <td valign="top" colspan="2">
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="ClsBorderlight">
                                                                <span class="ClsLabel">Clearance Start Date :</span>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="txtClearanceStartDate" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                                    TabIndex="4"></asp:TextBox>
                                                                <rjs:PopCalendar ID="calClearanceStartDate" runat="server" Control="txtClearanceStartDate" Culture="en"
                                                                    Format="dd MMM yyyy" ShowWeekend="True" Enabled="true" ShowErrorMessage="false"
                                                                    InvalidDateMessage="Please select valid from date." ControlFocusOnError="True" />
                                                                <asp:Label ID="lblClearanceStartDate" runat="server" CssClass="ClsMdtStar" Height="14px"
                                                                    Text="*" Width="14px" Visible="False"></asp:Label>
                                                            </td>
                                                            <td class="ClsBorderlight">
                                                                <span class="ClsLabel">Clearance End Date :</span>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="txtClearanceEndDate" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                                    TabIndex="5"></asp:TextBox>
                                                                <rjs:PopCalendar ID="calClearanceEndDate" runat="server" Control="txtClearanceEndDate" Culture="en"
                                                                    Format="dd MMM yyyy" ShowWeekend="True" Enabled="true" ShowErrorMessage="false"
                                                                    InvalidDateMessage="Please select valid to date." ClientScriptOnDateChanged="SetFocusonEndDate()" />
                                                                <asp:Label ID="lblClearanceEndDate" runat="server" CssClass="ClsMdtStar" Height="14px"
                                                                    Text="*" Width="14px" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" class="ClsBorderlight">
                                                    <asp:CheckBox ID="chkIncludeAll" runat="server" AutoPostBack="false" TabIndex="6" />
                                                </td>
                                                <td colspan="2" valign="top" class="ClsBorderlight">
                                                    <span class="ClsLabel">Include cheques which are cleared.</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" class="ClsBorderlight">
                                                    <asp:CheckBox ID="chkCautionMoney" runat="server" AutoPostBack="false" TabIndex="7" />
                                                </td>
                                                <td colspan="2" valign="top" class="ClsBorderlight">
                                                    <span class="ClsLabel">Include caution money details.</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" valign="top" colspan="3">
                                                    <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="ClsBtn" TabIndex="7"
                                                        OnClick="btnShow_Click" UseSubmitBehavior="false" Width="100px" ValidationGroup="Show" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="3">
                                                    <table id="tblLegend" runat="server" align="center">
                                                        <tr>
                                                            <td align="left" colspan="1">
                                                                <asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                                                    Text="Legend" EnableViewState="False"></asp:Label>
                                                            </td>
                                                            <td align="left" colspan="1">
                                                                &nbsp;<asp:Label ID="txtUserStop" runat="server" BackColor="LightBlue" Height="20px"
                                                                    BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"
                                                                    EnableViewState="False"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                                            </td>
                                                            <td align="left" colspan="1">
                                                                <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Caution Money Details"
                                                                    CssClass="ClsTextNormal" EnableViewState="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top" colspan="3">
                                                    <table runat="server" width="100%">
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
                                                                <asp:GridView ID="grdCheques" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                    AllowSorting="false" CellPadding="0" CellSpacing="1" OnRowDataBound="grdCheques_RowDataBound"
                                                                    ForeColor="#333333" GridLines="None" BackColor="White" DataKeyNames="PostDated_Cheque_Id,Enrolment_Number,Cheque_Date,Bank_Id,Student_Id,Payment_Cheque_Id"
                                                                    CssClass="GridBorder" AllowPaging="True" OnPageIndexChanging="grdCheques_PageIndexChanging"
                                                                    EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataText="No Record Found" TabIndex="8">
                                                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                                    </PagerStyle>
                                                                    <Columns>
                                                                        <asp:BoundField HeaderText="Reg.No." DataField="Enrolment_Number">
                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" />
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="true" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Name" DataField="StudentName">
                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Width="190px" />
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="true" />
                                                                        </asp:BoundField>
                                                                         <asp:BoundField HeaderText="Class" DataField="ClassName">
                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Width="80" />
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="true" />
                                                                        </asp:BoundField>
                                                                        
                                                                        <asp:TemplateField HeaderText="Chq. No.">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtChequeNo" runat="server" CssClass="SmlTxtBox" Width="60px" MaxLength="6"
                                                                                    TabIndex="8" Text='<%#Eval("Cheque_Number")%>' onblur="extractNumber(this,0,false);"
                                                                                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                                                    onpaste="event.returnValue=false;" ondrop="event.returnValue=false;"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="60px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                            <HeaderStyle Width="60px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField HeaderText="Bank" DataField="Bank_Name">
                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Width="15%" />
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="true" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Amt." DataField="Cheque_Amount">
                                                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" CssClass="ClspaddingR" />
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Cheque Dt.">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtChequeDate" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                                                    TabIndex="8" Text='<%#Eval("Cheque_Date","{0:dd-MMM-yyyy}")%>'></asp:TextBox>
                                                                                <rjs:PopCalendar ID="cChqDate" runat="server" Control="txtChequeDate" Format="dd MMM yyyy" Culture="en"
                                                                                    ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid to date." />
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="17%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            <HeaderStyle Width="17%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField HeaderText="Payment Dt." DataField="Paid_Date">
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Clearance Dt.">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtclearance" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                                                    TabIndex="8" Text='<%#Eval("Cheque_Passed_Date","{0:dd-MMM-yyyy}")%>'></asp:TextBox>
                                                                                <rjs:PopCalendar ID="cClrDate" runat="server" Control="txtclearance" Format="dd MMM yyyy" Culture="en"
                                                                                    ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid to date." />
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="17%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            <HeaderStyle Width="17%" HorizontalAlign="Center" VerticalAlign="Middle" />
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
                                                                            <%--<asp:Label ID="Label9" runat="server" CssClass="LblNrmlB" EnableViewState="False"
                                                        Width="200px">Total Pending Amount :</asp:Label>--%>
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
                                                    <asp:HiddenField ID="hidChequeId" runat="server" />
                                                    <asp:HiddenField ID="hidPageNo" runat="server" />
                                                    <asp:HiddenField ID="hidServerDate" runat="server" />
                                                    <asp:HiddenField ID="hidRowCnt" runat="server" />
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
                            <td align="right" width="55%">
                                <asp:Button ID="btnSave" Text="Save" CssClass="ClsBtn" runat="server" OnClick="btnSave_Click"
                                    ValidationGroup="Save" TabIndex="9" />
                            </td>
                            <td align="left" width="45%">
                                <asp:Button ID="btnExport" Text="Export" CssClass="ClsBtn" runat="server" OnClick="btnExport_Click"
                                    TabIndex="11" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
        _clientbtnShow = "<%=this.btnShow.ClientID %>"
        _clientbtnExport = "<%=this.btnExport.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientvalSumErrorMsgId = "<%=this.valSumErrorMsg.ClientID %>"
        _clientoptChequeNumberId = "<%=this.optChequeNumber.ClientID %>"
        _clientoptRegNoId = "<%=this.optRegNo.ClientID %>"
        _clientoptDateId = "<%=this.optDate.ClientID %>"
        _clientoptClearanceDate = "<%=this.optClearanceDate.ClientID %>"
        _clienttxtChequeNumberId = "<%=this.txtChequeNumber.ClientID %>"
        _clienttxtRegNoId = "<%=this.txtRegNo.ClientID %>"
        _clienttxtFromDateId = "<%=this.txtFromDate.ClientID %>"
        _clientcFromDateId = "<%=this.cFromDate.ClientID %>"
        _clienttxtToDateId = "<%=this.txtToDate.ClientID %>"
        _clienttxtEndDateId = "<%=this.txtClearanceEndDate.ClientID %>"
        _clienttxtClearanceStartDateId = "<%=this.txtClearanceStartDate.ClientID %>"
        _clienttcToDateId = "<%=this.cToDate.ClientID %>"
        _clientcstFormId = "<%=this.cstForm.ClientID %>"
        _clientlblErrorId = "<%=this.lblError.ClientID %>"
        _clientGrdId = "<%=this.grdCheques.ClientID %>"
        _clienthidPageNo = "<%=this.hidPageNo.ClientID %>"
        _clienthidServerDate = "<%=this.hidServerDate.ClientID %>"
        _clientlblSuccessMsg = "<%=this.lblSuccessMsg.ClientID %>"
        _clienthidRowCnt = "<%=this.hidRowCnt.ClientID %>"
        _clienthidChequeId = "<%=this.hidChequeId.ClientID %>"
        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)
        function EndReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement
            if ((postBackElement != null && postBackElement.id == _clientbtnShow) || (postBackElement != null && postBackElement.id == _clientbtnSave)) {
                if (document.getElementById(_clientbtnExport).style.visibility == "hidden") {
                    if (document.getElementById(_clientGrdId) != undefined && document.getElementById(_clientGrdId) != null) {
                        var iCount = document.getElementById(_clientGrdId).rows.length - 1
                        if (iCount > 0) {
                            document.getElementById(_clientbtnExport).style.visibility = "inherit"
                            document.getElementById(_clientbtnSave).style.visibility = "inherit"
                        }
                        else {
                            if (document.getElementById(_clientlblSuccessMsg) != undefined) {
                                document.getElementById(_clientlblSuccessMsg).innerHTML = ""
                            }
                            document.getElementById(_clientbtnExport).style.visibility = "hidden"
                            document.getElementById(_clientbtnSave).style.visibility = "hidden"
                        }
                    }
                    else {
                        if (document.getElementById(_clientlblSuccessMsg) != undefined) {
                            document.getElementById(_clientlblSuccessMsg).innerHTML = ""
                        }
                        document.getElementById(_clientbtnExport).style.visibility = "hidden"
                        document.getElementById(_clientbtnSave).style.visibility = "hidden"
                    }
                }
                else {
                    if ((postBackElement != null && postBackElement.id == _clientbtnSave)) {
                        document.getElementById(_clientbtnExport).style.visibility = "inherit"
                        document.getElementById(_clientbtnSave).style.visibility = "inherit"
                        if (document.getElementById(_clientGrdId) != undefined && document.getElementById(_clientGrdId) != null) {
                            var iCount = document.getElementById(_clientGrdId).rows.length - 1
                            if (iCount > 0) {
                                document.getElementById(_clientbtnExport).style.visibility = "inherit"
                                document.getElementById(_clientbtnSave).style.visibility = "inherit"
                            }
                            else {
                                if (document.getElementById(_clientlblSuccessMsg) != undefined) {
                                    document.getElementById(_clientlblSuccessMsg).innerHTML = ""
                                }
                                document.getElementById(_clientbtnExport).style.visibility = "hidden"
                                document.getElementById(_clientbtnSave).style.visibility = "hidden"
                            }
                        }
                        else {
                            if (document.getElementById(_clientlblSuccessMsg) != undefined) {
                                document.getElementById(_clientlblSuccessMsg).innerHTML = ""
                            }
                            document.getElementById(_clientbtnExport).style.visibility = "hidden"
                            document.getElementById(_clientbtnSave).style.visibility = "hidden"
                        }
                    }
                    else {
                        if (document.getElementById(_clientlblSuccessMsg) != undefined) {
                            document.getElementById(_clientlblSuccessMsg).innerHTML = ""
                        }
                        document.getElementById(_clientbtnExport).style.visibility = "hidden"
                        document.getElementById(_clientbtnSave).style.visibility = "hidden"
                    }
                }
            }
        }
        function ClearErrorLabel() {
            var isPageValid = true
            if (typeof (Page_ClientValidate) == 'function') {
                isPageValid = Page_ClientValidate()
            }
        }
        function SetFocusonTOdate() {
            var txtToDateId
            txtToDateId = document.getElementById(_clienttxtToDateId)
            txtToDateId.focus()
        }
        function SetFocusonEndDate() {
            var txtToDateId
            txtToDateId = document.getElementById(_clienttxtEndDateId)
            txtToDateId.focus()
        }
        function ClearValSum() {
            if (document.getElementById(_clientvalSumErrorMsgId) != null) document.getElementById(_clientvalSumErrorMsgId).style.display = "none"
            if (document.getElementById(_clientlblErrorId) != undefined) {
                document.getElementById(_clientlblErrorId).innerHTML = ""
            }
            return true
        }
        function ValidateControls(oSrc, args) {
            if (document.getElementById(_clientlblSuccessMsg) != undefined) {
                document.getElementById(_clientlblSuccessMsg).innerHTML = ""
            }
            if (document.getElementById(_clientlblErrorId) != undefined) {
                document.getElementById(_clientlblErrorId).innerHTML = ""
            }
            if (document.getElementById(_clientoptChequeNumberId).checked) {
                if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtChequeNumberId).value) == "") {
                    oSrc.errormessage = "Cheque number should not be blank."
                    args.IsValid = false
                    return true
                }
            }
            else if (document.getElementById(_clientoptClearanceDate).checked) {
                if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtClearanceStartDateId).value) == ""
&& stripLeadingTrailingBlanks(document.getElementById(_clienttxtEndDateId).value) == "") {
                    oSrc.errormessage = "Clearance start date and end date should not be blank."
                    args.IsValid = false
                    return true
                }
                else if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtClearanceStartDateId).value) == "") {
                    oSrc.errormessage = "Clearance start date should not be blank."
                    args.IsValid = false
                    return true
                }
                else {
                    if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtEndDateId).value) == "") {
                        oSrc.errormessage = "Clearance end date should not be blank."
                        args.IsValid = false
                        return true
                    }
                    else {
                        var fromDate
                        var toDate
                        if (document.all) {
                            fromDate = new Date((document.getElementById(_clienttxtClearanceStartDateId).value).replace('-', ' '))
                            toDate = new Date((document.getElementById(_clienttxtEndDateId).value).replace('-', ' '))
                        }
                        else {
                            fromDate = new Date(convertdate(document.getElementById(_clienttxtClearanceStartDateId).value))
                            toDate = new Date(convertdate(document.getElementById(_clienttxtEndDateId).value))
                        }
                        if (fromDate > toDate) {
                            oSrc.errormessage = "Clearance end date should be greater than clearance start date"
                            args.IsValid = false
                            return true
                        }
                    }
                }
                args.IsValid = true
                return false
            }
            else {
                if (document.getElementById(_clientoptRegNoId).checked) {
                    if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtRegNoId).value) == "") {
                        oSrc.errormessage = "Student registration number should not be blank."
                        args.IsValid = false
                        return true
                    }
                }
                else {
                    if (document.getElementById(_clientoptDateId).checked) {
                        if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtFromDateId).value) == ""
&& stripLeadingTrailingBlanks(document.getElementById(_clienttxtToDateId).value) == "") {
                            oSrc.errormessage = "Payment start date and end date should not be blank."
                            args.IsValid = false
                            return true
                        }
                        else if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtFromDateId).value) == "") {
                            oSrc.errormessage = "Payment Start date should not be blank."
                            args.IsValid = false
                            return true
                        }
                        else {
                            if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtToDateId).value) == "") {
                                oSrc.errormessage = "End date should not be blank."
                                args.IsValid = false
                                return true
                            }
                            else {
                                var fromDate
                                var toDate
                                if (document.all) {
                                    fromDate = new Date((document.getElementById(_clienttxtFromDateId).value).replace('-', ' '))
                                    toDate = new Date((document.getElementById(_clienttxtToDateId).value).replace('-', ' '))
                                }
                                else {
                                    fromDate = new Date(convertdate(document.getElementById(_clienttxtFromDateId).value))
                                    toDate = new Date(convertdate(document.getElementById(_clienttxtToDateId).value))
                                }
                                if (fromDate > toDate) {
                                    oSrc.errormessage = "End date should be greater than from date"
                                    args.IsValid = false
                                    return true
                                }
                            }
                        }
                    }
                }
            }
            args.IsValid = true
            return false
        }
        function ValidateGridControls(oSrc, args) {
            if (document.getElementById(_clientlblSuccessMsg) != undefined) {
                document.getElementById(_clientlblSuccessMsg).innerHTML = ""
            }
            if (document.getElementById(_clientlblErrorId) != undefined) {
                document.getElementById(_clientlblErrorId).innerHTML = ""
            }
            oSrc.errormessage = ""
            var iRowCnt = document.getElementById(_clienthidRowCnt).value
            var iRowNoP = ""
            var iRowNos = ""
            var iRowChequeNo = ""
            var iChequeDate = ""
            for (i = 1; i <= iRowCnt; i++) {
                if (i < 9) {
                    sRow = "_ctl0" + (i + 1) + "_txtclearance"
                    sRow1 = "_ctl0" + (i + 1) + "_txtChequeDate"
                    var paymentDate = document.getElementById(_clientGrdId).rows[i].cells[5].innerHTML
                    var txtBox1 = document.getElementById(_clientGrdId + sRow)
                    var txtChequeDate = document.getElementById(_clientGrdId + sRow1)
                    if ((txtChequeDate).value == "") {
                        iChequeDate += i.toString() + ", "
                    }
                    if ((txtBox1).value != "" && (txtChequeDate).value != "") {
                        var dpaymentDate = new Date(convertvaliddate(txtChequeDate.value))
                        var dClearanceDate = new Date(convertvaliddate(txtBox1.value))
                        var dserverDate = new Date($get("<%=this.hidServerDate.ClientID %>").value);
                        if (dserverDate < dClearanceDate)
                            iRowNoP += i.toString() + ", "
                        else if (dpaymentDate > dClearanceDate)
                            iRowNos += i.toString() + ", "
                    }
                    sRow = "_ctl0" + (i + 1) + "_txtChequeNo"
                    txtBox1 = document.getElementById(_clientGrdId + sRow)
                    if ((txtBox1).value == "")
                        iRowChequeNo += i.toString() + ", "
                }
                else {
                    sRow = "_ctl" + (i + 1) + "_txtclearance"
                    sRow1 = "_ctl" + (i + 1) + "_txtChequeDate"
                    var paymentDate = document.getElementById(_clientGrdId).rows[i].cells[5].innerHTML
                    var txtBox1 = document.getElementById(_clientGrdId + sRow)
                    var txtChequeDate = document.getElementById(_clientGrdId + sRow1)
                    if ((txtChequeDate).value == "") {
                        iChequeDate += i.toString() + ", "
                    }
                    if ((txtBox1).value != "" && (txtChequeDate).value != "") {
                        var dpaymentDate = new Date(convertvaliddate(txtChequeDate.value))
                        var dClearanceDate = new Date(convertvaliddate(txtBox1.value))
                        var dserverDate = new Date($get("<%=this.hidServerDate.ClientID %>").value);
                        if (dserverDate < dClearanceDate)
                            iRowNoP += i.toString() + ", "
                        else if (dpaymentDate > dClearanceDate)
                            iRowNos += i.toString() + ", "
                    }
                    sRow = "_ctl" + (i + 1) + "_txtChequeNo"
                    txtBox1 = document.getElementById(_clientGrdId + sRow)
                    if ((txtBox1).value == "")
                        iRowChequeNo += i.toString() + ", "
                }
            }
            if (iRowChequeNo != "") {
                iRowChequeNo = iRowChequeNo.substring(0, iRowChequeNo.lastIndexOf(","))
                oSrc.errormessage = "Cheque number should not be blank for row(s) : " + iRowChequeNo + "<br/>"
                if (iRowNoP != "") {
                    iRowNoP = iRowNoP.substring(0, iRowNoP.lastIndexOf(","))
                    oSrc.errormessage += "Cheque clearance date should not be future date for row(s) : " + iRowNoP + "<br/>"
                }
                if (iRowNos != "") {
                    iRowNos = iRowNos.substring(0, iRowNos.lastIndexOf(","))
                    oSrc.errormessage += "Cheque clearance date should be greater than cheque date for row(s) : " + iRowNos + "<br/>"
                }
                if (iChequeDate != "") {
                    iChequeDate = iChequeDate.substring(0, iChequeDate.lastIndexOf(","))
                    oSrc.errormessage += "Cheque date should not be blank for row(s) : " + iChequeDate
                }
                args.IsValid = false
                return true
            }
            else if (iRowNoP != "") {
                iRowNoP = iRowNoP.substring(0, iRowNoP.lastIndexOf(","))
                oSrc.errormessage = "Cheque clearance date should not be future date for row(s) : " + iRowNoP + "<br/>"
                if (iRowNos != "") {
                    iRowNos = iRowNos.substring(0, iRowNos.lastIndexOf(","))
                    oSrc.errormessage += "Cheque clearance date should be greater than cheque date for row(s) : " + iRowNos + "<br/>"
                }
                if (iChequeDate != "") {
                    iChequeDate = iChequeDate.substring(0, iChequeDate.lastIndexOf(","))
                    oSrc.errormessage += "Cheque date should not be blank for row(s) : " + iChequeDate
                }
                args.IsValid = false
                return true
            }
            else if (iRowNos != "") {
                iRowNos = iRowNos.substring(0, iRowNos.lastIndexOf(","))
                oSrc.errormessage = "Cheque clearance date should be greater than cheque date for rows : " + iRowNos + "<br/>"
                if (iChequeDate != "") {
                    iChequeDate = iChequeDate.substring(0, iChequeDate.lastIndexOf(","))
                    oSrc.errormessage += "Cheque date should not be blank for row(s) : " + iChequeDate
                }
                args.IsValid = false
                return true
            }
            else if (iChequeDate != "") {
                iChequeDate = iChequeDate.substring(0, iChequeDate.lastIndexOf(","))
                oSrc.errormessage = "Cheque date should not be blank for row(s) : " + iChequeDate
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        function fnover(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "maroon"
            objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)"
        }
        function fnout(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "#a3c07b"
            objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)"
        }
        function clickButton(e) {
            var evt = e ? e : window.event
            if (evt.keyCode == 13) {
                return false
            }
        }
        function MessageAboutDate(oCmb) {
            var bIsValid
            if (window.confirm('If you change the page then selected date and entered cheque number from current page will get lost. Do you want to continue?'))
                bIsValid = true
            else {
                document.getElementById(oCmb).value = document.getElementById(_clienthidPageNo).value
                bIsValid = false
            }
            return bIsValid
        }        
    </script>

</asp:Content>
