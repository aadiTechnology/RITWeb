<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CashClearanceListUI.ascx.cs"
    Inherits="CashClearanceListUI" %>
<%@ Register Src="~/UserControls/ClearanceListFiltersUI.ascx" TagName="ucClearanceListFiltersUI"
    TagPrefix="uc1" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<table width="100%">
    <tr>
        <td align="center" valign="top" colspan="3">
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
        <td align="left" colspan="3">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="lblNormal" ValidationGroup="Show" />
                    <asp:CustomValidator ID="cstForm" Display="None" runat="server" CssClass="ClsMdtStar"
                        Visible="true" ClientValidationFunction="ValidateControls" ValidationGroup="Show"></asp:CustomValidator>
                    <asp:Label ID="lblError" runat="server" Visible="false" CssClass="ClsMdtStar"></asp:Label>
                    <asp:ValidationSummary ID="valSave" runat="server" CssClass="lblNormal" ValidationGroup="Save" />
                    <asp:CustomValidator ID="cstClearanceDate" Display="None" runat="server" CssClass="ClsMdtStar"
                        Visible="true" ClientValidationFunction="ValidateGridControls" ValidationGroup="Save"></asp:CustomValidator>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                <ContentTemplate>
                    <table width="100%">
                        <tr>
                            <td>
                                <uc1:ucClearanceListFiltersUI ID="ClearanceListFilters" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="top">
                                <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="ClsBtn" TabIndex="7"
                                    Width="100px" ValidationGroup="Show" OnClick="btnShow_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top">
                                <table id="Table1" runat="server" width="100%">
                                    <tr runat="server" id="trTotalRec" align="center" visible="false">
                                        <td>
                                            <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                            <span class="LblNormal">To</span>
                                            <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                            <span class="LblNormal">Out Of</span>
                                            <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                            <span class="LblNormal">Records</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" valign="top">
                                            <asp:GridView ID="grdvwClearedCash" runat="server" Width="100%" AutoGenerateColumns="False"
                                                AllowSorting="false" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                                                BackColor="White" CssClass="GridBorder" AllowPaging="True" EmptyDataRowStyle-HorizontalAlign="Center"
                                                EmptyDataText="No Record Found" TabIndex="8" OnRowDataBound="grdvwClearedCash_RowDataBound"
                                                DataKeyNames="Receipt_Number" 
                                                OnPageIndexChanging="grdvwClearedCash_PageIndexChanging">
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
                                <asp:HiddenField ID="hidPageNo" runat="server" />
                                <asp:HiddenField ID="hidRowCnt" runat="server" />
                                <asp:HiddenField ID="hidReceiptNo" runat="server" />
                                <asp:HiddenField ID="hidCurrentDate" runat="server" />
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
        <td align="right" width="46%">
            <asp:Button ID="btnSave" Text="Save" CssClass="ClsBtn" runat="server" ValidationGroup="Save"
                TabIndex="9" OnClick="btnSave_Click" />
        </td>
        <td>
            <asp:Button ID="btnExport" Text="Export" CssClass="ClsBtn" runat="server" OnClick="btnExport_Click"
                TabIndex="13" />
        </td>
    </tr>
</table>

<script language="javascript" type="text/javascript">
    _clienthidRowCnt = "<%=this.hidRowCnt.ClientID %>"
    _clientGrdId = "<%=this.grdvwClearedCash.ClientID %>"
    _clientlblSuccessMsg = "<%=this.lblSuccessMsg.ClientID %>"
    _clientlblErrorId = "<%=this.lblError.ClientID %>"
    _clientbtnSave = "<%=this.btnSave.ClientID %>"
    _clientbtnShow = "<%=this.btnShow.ClientID %>"
    _clienthidPageNo = "<%=this.hidPageNo.ClientID %>"
    _clientvalSumErrorMsgId = "<%=this.valSumErrorMsg.ClientID %>"
    _clienthidReceiptNo = "<%=this.hidReceiptNo.ClientID %>"
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

    function ValidateGridControls(oSrc, args) {

        if (document.getElementById(_clientlblSuccessMsg) != undefined)
        { document.getElementById(_clientlblSuccessMsg).innerHTML = "" }
        if (document.getElementById(_clientlblErrorId) != undefined) {
            document.getElementById(_clientlblErrorId).innerHTML = ""
        }
        oSrc.errormessage = ""
        var iRowCount = document.getElementById(_clienthidRowCnt).value
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

    function ClearValSum() {

        if (document.getElementById(_clientvalSumErrorMsgId) != null) document.getElementById(_clientvalSumErrorMsgId).style.display = "none"
        if (document.getElementById(_clientvalSumErrorMsgId) != undefined) {
            document.getElementById(_clientvalSumErrorMsgId).innerHTML = ""
        }
        return true
    }

</script>

