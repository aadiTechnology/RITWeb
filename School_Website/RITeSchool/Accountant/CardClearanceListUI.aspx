<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="CardClearanceListUI.aspx.cs" Inherits="CardClearanceListUI" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="MainBodydiv" runat="server">
        <table width="98%" align="center">
            <tr>
                <td align="center" valign="top">
                    <asp:UpdatePanel ID="upnlSuccessMsg" runat="server" UpdateMode="Conditional">
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
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="lblNormal" ValidationGroup="Show" />
                            <asp:CustomValidator ID="cstForm" Display="None" runat="server" CssClass="ClsMdtStar"
                                Visible="true" ClientValidationFunction="ValidateControls" ValidationGroup="Show"></asp:CustomValidator>
                            <asp:Label ID="lblError" runat="server" Visible="false" CssClass="ClsMdtStar"></asp:Label>
                            <asp:ValidationSummary ID="valSave" runat="server" CssClass="lblNormal" ValidationGroup="Save" />
                            <asp:CustomValidator ID="cstCardClearanceDate" Display="None" runat="server" CssClass="ClsMdtStar"
                                Visible="true" ClientValidationFunction="ValidateGridControls" ValidationGroup="Save"></asp:CustomValidator>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="optPaymentDate" EventName="CheckedChanged" />
                            <asp:AsyncPostBackTrigger ControlID="optClearanceDate" EventName="CheckedChanged" />
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
                                                <td class="ClsBorderlight" valign="top">
                                                    <asp:RadioButton ID="optRegNo" runat="server" AutoPostBack="true" GroupName="Filter"
                                                        OnCheckedChanged="optRegNo_CheckedChanged" TabIndex="1" />
                                                </td>
                                                <td class="ClsBorderlight" valign="top">
                                                    <span class="ClsLabel">Student Name / Reg. No. :</span>
                                                </td>
                                                <td align="left" valign="top">
                                                    <asp:TextBox ID="txtRegNo" runat="server" CssClass="MidTxtBox" MaxLength="50" 
                                                        TabIndex="2"></asp:TextBox>
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
                                                    <asp:RadioButton ID="optPaymentDate" runat="server" AutoPostBack="true" GroupName="Filter"
                                                        OnCheckedChanged="optPaymentDate_CheckedChanged" TabIndex="3" />
                                                </td>
                                                <td valign="top" colspan="2">
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="ClsBorderlight">
                                                                <span class="ClsLabel">Payment Start Date :</span>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="txtPaymentStartDate" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                                    TabIndex="4"></asp:TextBox>
                                                                <rjs:PopCalendar ID="cFromDate" runat="server" Control="txtPaymentStartDate" Format="dd MMM yyyy"
                                                                    ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid from date."
                                                                    ControlFocusOnError="True" />
                                                            </td>
                                                            <td class="ClsBorderlight">
                                                                <span class="ClsLabel">End Date :</span>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="txtPaymentEndDate" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                                    TabIndex="5"></asp:TextBox>
                                                                <rjs:PopCalendar ID="cToDate" runat="server" Control="txtPaymentEndDate" Format="dd MMM yyyy"
                                                                    ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid to date." />
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
                                                        OnCheckedChanged="optClearanceDate_CheckedChanged" TabIndex="6" />
                                                </td>
                                                <td valign="top" colspan="2">
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="ClsBorderlight">
                                                                <span class="ClsLabel">Clearance Start Date:</span>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="txtClearanceStartDate" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                                    TabIndex="7"></asp:TextBox>
                                                                <rjs:PopCalendar ID="calClearanceStartDate" runat="server" Control="txtClearanceStartDate"
                                                                    Format="dd MMM yyyy" ShowWeekend="True" Enabled="true" ShowErrorMessage="false"
                                                                    InvalidDateMessage="Please select valid from date." ControlFocusOnError="True" />
                                                            </td>
                                                            <td class="ClsBorderlight">
                                                                <span class="ClsLabel">Clearance End Date :</span>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="txtClearanceEndDate" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                                    TabIndex="8"></asp:TextBox>
                                                                <rjs:PopCalendar ID="calClearanceEndDate" runat="server" Control="txtClearanceEndDate"
                                                                    Format="dd MMM yyyy" ShowWeekend="True" Enabled="true" ShowErrorMessage="false"
                                                                    InvalidDateMessage="Please select valid to date." />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" class="ClsBorderlight">
                                                    <asp:CheckBox ID="chkIncludeAll" runat="server" AutoPostBack="false" 
                                                        TabIndex="9" />
                                                </td>
                                                <td colspan="2" valign="top" class="ClsBorderlight">
                                                    <span class="ClsLabel">Include Card payments which are cleared.</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" class="ClsBorderlight">
                                                    <asp:Label ID="lblCardType" runat="server" Text="Swipe card type :" TabIndex="6" />
                                                </td>
                                                <td colspan="2" valign="top" class="ClsBorderlight">
                                                   <asp:DropDownList ID="cmbCardType" runat="server" AutoPostBack="false" 
                                                        TabIndex="10"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td align="center" valign="top" colspan="3">
                                                    <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="ClsBtn" TabIndex="11"
                                                        Width="100px" ValidationGroup="Show" OnClick="btnShow_Click" />
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
                                                                <asp:GridView ID="grdvwCardPayments" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                    AllowSorting="false" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                                                                    BackColor="White" CssClass="GridBorder" AllowPaging="True" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                    EmptyDataText="No Record Found" DataKeyNames="StudentCardPaymentDetailsId" 
                                                                    onrowdatabound="grdvwCardPayments_RowDataBound" TabIndex="12">
                                                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                                    </PagerStyle>
                                                                    <Columns>
                                                                        <asp:BoundField HeaderText="Reg.No" DataField="RegNo">
                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" />
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" CssClass="ClspaddingR" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Name" DataField="StudentName">
                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" />
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" CssClass="ClspaddingR" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Class" DataField="ClassName">
                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" />
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" CssClass="ClspaddingR" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Amount" DataField="Amount">
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="ClspaddingR" />
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Paid For" DataField="Payable_For">
                                                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Middle" CssClass="paddingLSML" />
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" CssClass="ClspaddingR" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Paid Date">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtPaidDate" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                                                    TabIndex="8" Text='<%#Eval("Paid_Date","{0:dd-MMM-yyyy}")%>'></asp:TextBox>
                                                                                <rjs:PopCalendar ID="cPaidDate" runat="server" Control="txtPaidDate" Format="dd MMM yyyy"
                                                                                    ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid to date." />
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="17%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            <HeaderStyle Width="17%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Clearance Date">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtclearance" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                                                    TabIndex="8" Text='<%#Eval("ClearanceDate","{0:dd-MMM-yyyy}")%>'></asp:TextBox>
                                                                                <rjs:PopCalendar ID="cClrDate" runat="server" Control="txtclearance" Format="dd MMM yyyy"
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
                                                                                        runat="server" OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged">
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
                                                    <asp:HiddenField ID="hidPageNo" runat="server" />
                                                    <asp:HiddenField ID="hidRowCnt" runat="server" />
                                                    <asp:HiddenField ID="hidCurrentDate" runat="server" />
                                                </td>
                                            </tr>
                                              <tr>
                                                <td>
                                                    <asp:ObjectDataSource TypeName="BusinessLogic.CashClearanceListBL" EnablePaging="true" ID="objDSCardPayment"
                                                        runat="server" SelectMethod="GetCardPaymentList" SortParameterName="sortExpression"
                                                        SelectCountMethod="CountCardPayments" EnableCaching="false"  OnSelected="GrdDSobj_Selected">
                                                        <SelectParameters>
                                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                            <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                Type="string" />
                                                            <asp:ControlParameter ControlID="txtRegNo" PropertyName="Text" Name="asRegNo" DefaultValue="" />
                                                            <asp:ControlParameter ControlID="txtPaymentStartDate" PropertyName="Text" Name="asPaymentStartDate" DefaultValue=""  />
                                                            <asp:ControlParameter ControlID="txtPaymentEndDate" PropertyName="Text" Name="asPaymentEndDate" DefaultValue="" />
                                                            <asp:ControlParameter ControlID="txtClearanceStartDate" PropertyName="Text" Name="asClearanceStartDate" DefaultValue="" />
                                                            <asp:ControlParameter ControlID="txtClearanceEndDate" PropertyName="Text" Name="asClearanceEndDate" DefaultValue="" />
                                                            <asp:ControlParameter ControlID="chkIncludeAll" DbType="Boolean" PropertyName="Checked" Name="abIncludeAll" />
                                                            <asp:ControlParameter ControlID="cmbCardType" PropertyName="SelectedValue" Name="aiCardType" />
                                                            
                                                            
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="46%">
                                <asp:Button ID="btnSave" Text="Save" CssClass="ClsBtn" runat="server" ValidationGroup="Save"
                                    TabIndex="13" OnClick="btnSave_Click" />
                            </td>
                            <td>
                                     <asp:Button ID="btnExport" Text="Export" CssClass="ClsBtn" runat="server" OnClick="btnExport_Click"
                                    TabIndex="15" Visible="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
        _clienthidRowCnt = "<%=this.hidRowCnt.ClientID %>"
        _clientGrdId = "<%=this.grdvwCardPayments.ClientID %>"
        _clientlblSuccessMsg = "<%=this.lblSuccessMsg.ClientID %>"
        _clientlblErrorId = "<%=this.lblError.ClientID %>"
        _clientoptClearanceDate = "<%=this.optClearanceDate.ClientID %>"
        _clientoptPaymentDate = "<%=this.optPaymentDate.ClientID %>"
        _clientClearanceStartDate = "<%=this.txtClearanceStartDate.ClientID %>"
        _clientClearanceEndDate = "<%=this.txtClearanceEndDate.ClientID %>"
        _clientPaymentStartDate = "<%=this.txtPaymentStartDate.ClientID %>"
        _clientPaymentEndDate = "<%=this.txtPaymentEndDate.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnShow = "<%=this.btnShow.ClientID %>"
        _clienthidPageNo = "<%=this.hidPageNo.ClientID %>"
        _clientvalSumErrorMsgId = "<%=this.valSumErrorMsg.ClientID %>"
        _clientcstForm = "<%=this.cstForm.ClientID %>"
        _clienthidCurrentDate = "<%=this.hidCurrentDate.ClientID%>"
        _clientbtnExport = "<%=this.btnExport.ClientID %>"
       

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

        function ValidateControls(oSrc, args) {
            document.getElementById(_clientcstForm).errormessage = ""
            if (document.getElementById(_clientlblSuccessMsg) != undefined) {
                document.getElementById(_clientlblSuccessMsg).innerHTML = ""
            }
            if (document.getElementById(_clientlblErrorId) != undefined) {
                document.getElementById(_clientlblErrorId).innerHTML = ""
            }
            if (document.getElementById(_clientoptClearanceDate).checked) {
                var fromDate
                var toDate
                if (document.all) {
                    fromDate = new Date((document.getElementById(_clientClearanceStartDate).value).replace('-', ' '))
                    toDate = new Date((document.getElementById(_clientClearanceEndDate).value).replace('-', ' '))
                }
                else {
                    fromDate = new Date(convertdate(document.getElementById(_clientClearanceStartDate).value))
                    toDate = new Date(convertdate(document.getElementById(_clientClearanceEndDate).value))
                }
                if (fromDate > toDate) {
                    document.getElementById(_clientcstForm).errormessage = "Clearance end date should be greater than clearance start date"
                    args.IsValid = false
                    return true
                }
            }
            else if (document.getElementById(_clientoptPaymentDate).checked) {
                var fromDate
                var toDate
                if (document.all) {
                    fromDate = new Date((document.getElementById(_clientPaymentStartDate).value).replace('-', ' '))
                    toDate = new Date((document.getElementById(_clientPaymentEndDate).value).replace('-', ' '))
                }
                else {
                    fromDate = new Date(convertdate(document.getElementById(_clientPaymentStartDate).value))
                    toDate = new Date(convertdate(document.getElementById(_clientPaymentEndDate).value))
                }
                if (fromDate > toDate) {
                    document.getElementById(_clientcstForm).errormessage = "Payment end date should be greater than Payment start date."
                    args.IsValid = false
                    return true
                }
            }
            args.IsValid = true
            return false
        }


        function MessageAboutDate(oCmb) {
            var bIsValid
            if (window.confirm('If you change the page then selected date and entered Register number from current page will get lost. Do you want to continue?'))
                bIsValid = true
            else {
                document.getElementById(oCmb).value = document.getElementById(_clienthidPageNo).value
                bIsValid = false
            }
            return bIsValid
        }

        function ClearValSum() {

            if (document.getElementById(_clientvalSumErrorMsgId) != null) document.getElementById(_clientvalSumErrorMsgId).style.display = "none"
            if (document.getElementById(_clientvalSumErrorMsgId) != undefined) {
                document.getElementById(_clientvalSumErrorMsgId).innerHTML = ""
            }
            return true
        }

//        var prm = Sys.WebForms.PageRequestManager.getInstance()
//        prm.add_endRequest(EndReqHandler)
//        function EndReqHandler(sender, args) {
//            var postBackElement = sender._postBackSettings.sourceElement
//            if (postBackElement != null && postBackElement.id == _clientbtnShow) {
//                if (postBackElement.value == "Show") {
//                    if (document.getElementById(_clientGrdId) != undefined && document.getElementById(_clientGrdId) != null) {
//                        var iCount = document.getElementById(_clientGrdId).rows.length - 1
//                        if (iCount > 0) {
//                            document.getElementById(_clientbtnSave).style.visibility = "inherit"
//                        }
//                    }
//                }
//                if (postBackElement.value == "Change Input") {
//                    if (document.getElementById(_clientlblSuccessMsg) != undefined) {
//                        document.getElementById(_clientlblSuccessMsg).innerHTML = ""
//                    }
//                    document.getElementById(_clientbtnSave).style.visibility = "Hidden"
//                }
//            }
//        }
        function ValidateGridControls(oSrc, args) {

            if (document.getElementById(_clientlblSuccessMsg) != undefined)

            { document.getElementById(_clientlblSuccessMsg).innerHTML = "" }

            if (document.getElementById(_clientlblErrorId) != undefined) {
                document.getElementById(_clientlblErrorId).innerHTML = ""
            }
            oSrc.errormessage = ""
            var iRowCount = document.getElementById(_clienthidRowCnt).value
            //var iRowCount = document.getElementById(_clientGrdId).rows.length - 1
            var iRowNoP = ""
            var iRowNos = ""
            var dtToday
            document.getElementById(_clienthidCurrentDate).value = new Date().format("dd-MMM-yyyy")
            var TodayDate = document.getElementById(_clienthidCurrentDate).value

            for (i = 1; i <= iRowCount; i++) {
                if (i < 9) {
                    sRow = "_ctl0" + (i + 1) + "_txtclearance"
                    sRowPayment = "_ctl0" + (i + 1) + "_txtPaidDate"
                    var PaymentDate = document.getElementById(_clientGrdId + sRowPayment)
                    var txtClearanceDate = document.getElementById(_clientGrdId + sRow)

                    if ((PaymentDate).value != "" && (txtClearanceDate).value != "") {

                        var DateOfPayment = new Date(convertvaliddate(PaymentDate.value))
                        var DateOfClearance = new Date(convertvaliddate(txtClearanceDate.value))
                        if (document.all)
                            dtToday = new Date(TodayDate.replace('-', ' '))
                        else
                            dtToday = new Date(convertdate(TodayDate))

                        if (DateOfPayment > DateOfClearance)
                            iRowNos += i.toString() + ", "
                        else if (dtToday < DateOfClearance)
                            iRowNoP += i.toString() + ", "
                    }
                }
                else {
                    sRow = "_ctl" + (i + 1) + "_txtclearance"
                    sRowPayment = "_ctl" + (i + 1) + "_txtPaidDate"
                    var PaymentDate = document.getElementById(_clientGrdId + sRowPayment)
                    var txtClearanceDate = document.getElementById(_clientGrdId + sRow)

                    if ((PaymentDate).value != "" && (txtClearanceDate).value != "") {
                        var DateOfPayment = new Date(convertvaliddate(PaymentDate.value))
                        var DateOfClearance = new Date(convertvaliddate(txtClearanceDate.value))
                        if (document.all)
                            dtToday = new Date(TodayDate.replace('-', ' '))
                        else
                            dtToday = new Date(convertdate(TodayDate))

                        if (DateOfPayment > DateOfClearance)
                            iRowNos += i.toString() + ", "
                        else if (dtToday < DateOfClearance)
                            iRowNoP += i.toString() + ", "
                    }
                }
            }
            if (iRowNos != "") {
                iRowNos = iRowNos.substring(0, iRowNos.lastIndexOf(","))
                oSrc.errormessage = "Clearance date should be greater than paid date for row(s) : " + iRowNos + "<br/>"
                args.IsValid = false
                return true
            }
            if (iRowNoP != "") {
                iRowNoP = iRowNoP.substring(0, iRowNoP.lastIndexOf(","))
                oSrc.errormessage += "Clearance date should not be future date for row(s) : " + iRowNoP + "<br/>"
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false

        }
    </script>

</asp:Content>
