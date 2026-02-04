<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="StudentLibraryFeesUI.aspx.cs" Inherits="StudentLibraryFees" EnableEventValidation="false" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="98%">
            <tr id="trPrecondition" runat="server" visible="false">
                <td align="left" valign="top" style="height: 20px">
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>
            <tr>
                <td class="ClsMdtStar" align="left">
                    <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" ForeColor="red" EnableViewState="false"
                        Text=""></asp:Label>
                    <asp:ValidationSummary ID="valRegNumber" runat="server" ShowMessageBox="False" ValidationGroup="RegNumber"
                        ShowSummary="True" CssClass="ClsLabel" />
                    <%--<asp:ValidationSummary ID="valStandardRegNumber" runat="server" ShowMessageBox="False"
                        ValidationGroup="StandardRegNumber" ShowSummary="True" CssClass="ClsLabel" />--%>
                    <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="true"
                        ValidationGroup="Payment" ShowSummary="True" CssClass="ClsLabel" />--%>
                   <%-- <asp:CompareValidator ID="CompareValidator1" runat="server" CssClass="ClsLabel" ValueToCompare="0"
                        Operator="NotEqual" ErrorMessage="Fee type should be selected." Display="None"
                        ControlToValidate="cmbFeeType" ValidationGroup="Payment"></asp:CompareValidator>--%>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlInput" runat="server">
                        <table style="width: 100%;" cellpadding="0" cellspacing="1">
                            <tr>
                                <td align="center">
                                    <div style="float: right" class="LblErrorMsg" id="LblErrorMsg" runat="server">
                                        * Mandatory Fields</div>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                        ID="UPnl">
                                        <ContentTemplate>
                                            <table runat="server" id="tblStudentInputFields" cellpadding="0" cellspacing="1">
                                                <tr>
                                                    <td colspan="4">
                                                        <div style="float: left; padding-left: 5px; padding-right: 5px;" class="LblErrorMsg">
                                                            <asp:Label ID="lblStuError" runat="server"></asp:Label></div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="3">
                                                        <table runat="server" id="Table3" cellpadding="0" cellspacing="1">
                                                            <tr>
                                                    <td align="left" class="ClsBorderlight">
                                                        <asp:Label ID="Label13" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                            Text="Registration Number :"></asp:Label></td>
                                                    <td align="left" class="ClsMdtStar">
                                                        <asp:TextBox ID="txtRegNumber" TabIndex="0" runat="server" MaxLength="50" CssClass="MidTxtBox"></asp:TextBox>&nbsp;
                                                        <asp:RequiredFieldValidator ID="reqRegName" Display="None" runat="server" ErrorMessage="Registration Number should be entered."
                                                            ControlToValidate="txtRegNumber" ValidationGroup="RegNumber" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td align="left" class="ClsMdtStar">
                                                        *</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnSearch1" runat="server" Text="Show" CssClass="ClsBtnMid" ValidationGroup="RegNumber" /></td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                     <%--   <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnSearch1" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnPay" EventName="click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnPayPrint" EventName="click" />
                                            <asp:AsyncPostBackTrigger ControlID="cmbFeeType" EventName="SelectedIndexChanged" />
                                        </Triggers>--%>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <table runat="server" id="trTeacher" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="ClsBorderlight">
                                                <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                    Text="Select Student :"></asp:Label></td>
                                            <td class="LblErrorMsg">
                                                <asp:DropDownList ID="cmbRollNo" runat="server" CssClass="ExLrgCombo"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>*
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                   <asp:UpdatePanel ID="UPanelStudent" runat="server">
                                        <ContentTemplate>
                                            <table style="width: 100%;" cellpadding="0" cellspacing="1">
                                                <asp:Panel ID="pnlFields" runat="server">
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="Label4" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                                Text="Student Name :"></asp:Label>
                                                        </td>
                                                        <td align="left" class="HilightBGGray" style="width: 50%">
                                                            <asp:Label ID="lblStudentName" runat="server" CssClass="LblNrmlB" Text="" EnableViewState="true"></asp:Label>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="Label5" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                                Text="Roll Number :"></asp:Label>
                                                        </td>
                                                        <td align="left" class="HilightBGGray">
                                                            <asp:Label ID="lblRollNumber" runat="server" CssClass="LblNrmlB" Text="" EnableViewState="true"></asp:Label>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="Label6" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                                Text="Standard-Division :"></asp:Label>
                                                        </td>
                                                        <td align="left" class="HilightBGGray">
                                                            <asp:Label ID="lblStandardDivision" runat="server" CssClass="LblNrmlB" Text="" EnableViewState="true"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </asp:Panel>
                                                <asp:HiddenField ID="hidYearwiseStudentId" runat="server"></asp:HiddenField>
                                                <asp:HiddenField ID="hidLateFeeAmount" runat="server"></asp:HiddenField>
                                                <asp:HiddenField ID="hidStandardId" runat="server"></asp:HiddenField>
                                                <asp:HiddenField ID="hidStdDivId" runat="server"></asp:HiddenField>
                                                <asp:HiddenField ID="hidDivisionId" runat="server"></asp:HiddenField>
                                                <asp:HiddenField ID="hidMonthlyFees" runat="server"></asp:HiddenField>
                                                <asp:HiddenField ID="hidURL" runat="server"></asp:HiddenField>
                                                <%-- <asp:HiddenField ID="hidPaidIntervals" Value="0" runat="server"></asp:HiddenField>--%>
                                            </table>
                                       </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="1">
                                        <tr>
                                            <td class="ClsBorderlight" style="padding-right: 9px">
                                                <asp:Label ID="lblPayDate" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                    Text="Payment Date  :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="calStartDate" CssClass="SmlCombo" runat="server" AutoPostBack="True"></asp:TextBox>
                                                <rjs:PopCalendar ID="cStartDate" runat="server" Control="calStartDate" Format="dd MMM yyyy"
                                                    ShowWeekend="True" ClientScriptOnDateChanged="ChangeLateFess()" ShowErrorMessage="false"
                                                    InvalidDateMessage="Please select valid payment date." />
                                                <asp:RequiredFieldValidator ID="ReqPaymentDate" Display="None" runat="server" ErrorMessage="Payment date should not be blank."
                                                    ControlToValidate="calStartDate" ValidationGroup="RegNumber" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </td>
                                            <td  class="paddingLSML">
                                                <asp:Label ID="lblMdt" runat="server" CssClass="LblErrorMsg" EnableViewState="False"
                                                    Text="*" Width="7px"></asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="height: 229px">
                                    <%--<asp:UpdatePanel ID="UPnlPaymentDetails" UpdateMode="Conditional" runat="server">
                                        <ContentTemplate>--%>
                                            <table runat="server" id="tblPaymentDetails" width="100%" cellpadding="0" cellspacing="1">
                                                <%--<tr>
                                               
                                            </tr>--%>
                                                <tr>
                                                    <td class="ClsBorderlight" style="width: 28%">
                                                        &nbsp;</td>
                                                    <td style="width: 43%;">
                                                        &nbsp;
                                                        </td>
                                                    <td colspan="2" rowspan="5" valign="top" style="width: 39%; padding-top: 20px;">
                                                        <table runat="server" id="Table1" width="100%" cellpadding="0" cellspacing="1">
                                                            <tr>
                                                                <td class="ClsBorderlight paddingLSML">
                                                                    &nbsp;<asp:Label ID="lblLateFeeRate" runat="server" CssClass="LblSmlGray" Text="Late Fee Amount Per Day :"
                                                                        EnableViewState="False" Width="143px"></asp:Label></td>
                                                                <td class="ClsBorderlight">
                                                                    <asp:Label ID="txtLateFeeRate" runat="server" CssClass="LblSmlRslt" EnableViewState="true"></asp:Label>&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="ClsBorderlight paddingLSML">
                                                                    &nbsp;<asp:Label ID="lblFeesRate" runat="server" CssClass="LblSmlGray" Text="Rate :"
                                                                        EnableViewState="true"></asp:Label></td>
                                                                <td class="ClsBorderlight">
                                                                    <asp:TextBox ID="txtFeeRate" runat="server" TabIndex="-1" BorderWidth="0px" ReadOnly="true"
                                                                        CssClass="LblSmlRslt" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="ClsBorderlight paddingLSML">
                                                                    &nbsp;<asp:Label ID="lblDueDateText" runat="server" CssClass="LblSmlGray" Text="Due Date :"
                                                                        EnableViewState="false"></asp:Label>
                                                                    <asp:HiddenField ID="hidDueDate" runat="server"></asp:HiddenField>
                                                                </td>
                                                                <td class="ClsBorderlight">
                                                                    &nbsp;<asp:Label ID="lblDueDate" runat="server" CssClass="LblSmlRslt" EnableViewState="true"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="ClsBorderlight">
                                                        <asp:Label ID="lblInterval" runat="server" CssClass="ClsLabel" Text="Fees Payment for the period of :"></asp:Label></td>
                                                    <td><asp:TextBox ID="txtFeesPaymentPeriod" runat="server" CssClass="SmlTxtBox"></asp:TextBox>
                                                        <asp:Label ID="lblInterval1" runat="server" CssClass="LblNormal" Text="" EnableViewState="true"></asp:Label>
                                                        
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="ClsBorderlight">
                                                        <asp:Label ID="Label11" runat="server" CssClass="ClsLabel" Text="Late Fee Amount :"
                                                            EnableViewState="false"></asp:Label></td>
                                                    <td colspan="1" >
                                                        <asp:TextBox ID="txtLateFeeAmount" ReadOnly="true" runat="server" CssClass="SmlTxtBox"
                                                            MaxLength="5" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                            ondrop="event.returnValue=false" />
                                                            <asp:Label ID="lblDistribution" runat="server" CssClass="LblNormal" 
                                                            EnableViewState="False"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="ClsBorderlight">
                                                        <asp:Label ID="lblDueAmount" runat="server" CssClass="ClsLabel" Text="Due Amount :"
                                                            EnableViewState="false"></asp:Label></td>
                                                    <td>
                                                        <asp:TextBox ID="txtDueAmount" TabIndex="-1" ReadOnly="true" runat="server" CssClass="SmlTxtBox"
                                                            MaxLength="50" BorderColor="DarkGray" BorderStyle="Solid" BorderWidth="1px" onblur="extractNumber(this,0,false);"
                                                            onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false); "
                                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="ClsBorderlight">
                                                        <asp:Label ID="Label3" runat="server" CssClass="ClsLabel" Text="Concession Amount : <i><b>( Less )</b></i>"
                                                            EnableViewState="False"></asp:Label></td>
                                                    <td>
                                                        <asp:TextBox ID="txtConcession" runat="server" CssClass="SmlTxtBox" MaxLength="50"
                                                            BorderColor="DarkGray" BorderStyle="Solid" BorderWidth="1px" onblur="extractNumber(this,0,false);"
                                                            onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false); "
                                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                                        <asp:CompareValidator ID="CmpConcession" Display="None" ValidationGroup="Payment"
                                                            ControlToCompare="txtDueAmount" Operator="LessThanEqual" ControlToValidate="txtConcession"
                                                            Type="Integer" runat="server" ErrorMessage="Concession amount should not be greater than due amount."></asp:CompareValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="ClsBorderlight">
                                                        <asp:Label ID="Label10" runat="server" CssClass="ClsLabel" Text="Total Amount :"
                                                            EnableViewState="false"></asp:Label></td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtTotalAmount" TabIndex="-1" CssClass="ClsBorderP ClsPrintHead "
                                                            MaxLength="50" runat="server" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                            onkeypress="return blockNonNumbers (this, event, false, false);" ReadOnly="true"
                                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false" Width="96px" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table runat="server" id="tblCheckList" width="100%" cellpadding="0" cellspacing="1">
                                                <tr runat="server" id="trPaid" visible="false">
                                                    <td class="ClsBorderlight" style="width: 20%">
                                                        &nbsp;</td>
                                                    <td class="ClsBorderlight">
                                                        &nbsp;</td>
                                                </tr>
                                            </table>
                                            <table runat="server" id="Table2" width="100%" cellpadding="0" cellspacing="1">
                                                <tr runat="server" id="tr1">
                                                    <td align="right" style="height: 25px">
                                                        <%--<input type="button" class="BtnHLight" id="HTMLbtnSPay"  onclick="this.disabled=true; document.forms[0]._clientbtnPay.click();this.disabled=false;"
                                                        runat="server" value="Pay" />--%>
                                                        <asp:Button ID="btnPay" CausesValidation="true" ValidationGroup="Payment" runat="server"
                                                            Text="Pay" CssClass="BtnHLight" /></td>
                                                    <td align="left" style="height: 25px">
                                                        <%-- <input type="button" class="ClsBtnMid"
                                                            id="HTMLbtnSPayPrint" onclick="this.disabled=true; document.forms[0]._clientbtnPayPrint.click();this.disabled=false;"
                                                            runat="server" value="Pay & Print" visible="false" />--%>
                                                        <asp:Button ID="btnPayPrint" ValidationGroup="Payment" runat="server" Text="Pay & Print"
                                                            CausesValidation="true" CssClass="BtnHLight" />
                                                    </td>
                                                </tr>
                                            </table>
                                      <%--  </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnSearch1" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="ClsBorderlight" colspan="6">
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                        <ContentTemplate>
                                            <div id="divFeesPaid" runat="server" visible="true" style="background-color: #eaeaea">
                                                <div id="divPaidHeader" class="" runat="server">
                                                    <asp:Label ID="lblPaidHeader" runat="server" Font-Size="12pt" Font-Bold="true" CssClass="ClsLabel"
                                                        Text="Fees Paid Till Date" EnableViewState="false"></asp:Label>
                                                </div>
                                                <br />
                                                <br />
                                                <div style="background-color: #ffffff">
                                                    <asp:GridView CssClass="GridBorder" ID="grdFeesPaid" runat="server" AutoGenerateColumns="False"
                                                        Height="100%" PageSize="1100" AllowPaging="False" CellPadding="0" CellSpacing="1"
                                                        ForeColor="#333333" GridLines="None"
                                                        Width="100%" >
                                                        <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                        </PagerStyle>
                                                        <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                            FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Receipt No." SortExpression="Receipt_Number" DataField="Receipt_Number">
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Fee Type" DataField="Fee_Type">
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Paid Date" SortExpression="payment_date" DataField="payment_date">
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Duration" SortExpression="Intervalname" DataField="Intervalname">
                                                                <ItemStyle HorizontalAlign="Center" Wrap="True" VerticalAlign="Middle" Width="150px" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Due Amt. (Rs)" SortExpression="Due_Amount" DataField="Due_Amount">
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Concession (Rs)" SortExpression="Concession_Amount"
                                                                DataField="Concession_Amount">
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Late Fee (Rs)" SortExpression="Late_Fee_Amount" DataField="Late_Fee_Amount">
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Total  Amt. Paid (Rs)" SortExpression="Total_Fee_Amount"
                                                                DataField="Total_Fee_Amount">
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Receipt Print">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="lnkMini" runat="server" Text="Mini" Visible="true" />
                                                                    <asp:Label ID="lblSeparator" runat="server" Text=" / " Width="10px" EnableViewState="false" />
                                                                    <asp:HyperLink ID="lnkDetails" Text="Details" runat="server" Visible="true"/>
                                                                </ItemTemplate>
                                                                <ItemStyle Wrap="False" Width="110px" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <RowStyle CssClass="ClsMarksGridAltRowN" />
                                                        <HeaderStyle CssClass="ClsMarksGridHeader" />
                                                        <AlternatingRowStyle CssClass="ClsMarksGridAltRowN" />
                                                        <EmptyDataRowStyle CssClass="LblNoRecord" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                       <%-- <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnPay" EventName="click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnPayPrint" EventName="click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnSearch1" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="cmbRollNo" EventName="SelectedIndexChanged" />
                                        </Triggers>--%>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button UseSubmitBehavior="false" ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn"
                        CausesValidation="False" OnClick="btnBack_Click" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidTodaysDate" Value="0" runat="server"></asp:HiddenField>
        <%--<asp:CompareValidator ID="CompareValidator2" runat="server" CssClass="ClsLabel" Visible="True"
                                            ValueToCompare="0" Operator="NotEqual" ErrorMessage="Fee type should be selected."
                                            Display="None" ControlToValidate="cmbFeeType" ValidationGroup="Payment"></asp:CompareValidator>
    --%>
    </div>

    <script language="javascript" type="text/javascript">
    
    /*
    _clienttxtLateFeeAmountId = "<%=this.txtLateFeeAmount.ClientID %>";
      _clienttxtLateFeeRateId = "<%=this.txtLateFeeRate.ClientID %>";    
    _clienttxtDueAmountId = "<%=this.txtDueAmount.ClientID %>";
    _clienttxtTotalAmountId = "<%=this.txtTotalAmount.ClientID %>";
    _clienttxtConcessionId = "<%=this.txtConcession.ClientID %>";
    
    _clientlblStuError = "<%=this.lblStuError.ClientID %>";
    _clientbtnPayPrint = "<%=this.btnPayPrint.ClientID %>";
    _clientbtnPay = "<%=this.btnPay.ClientID %>";
    _clientlblDueDate = "<%=this.hidDueDate.ClientID %>";
    _clientcalStartDate = "<%=this.calStartDate.ClientID %>";
    _clientlblLateFeeRate = "<%=this.lblLateFeeRate.ClientID %>";
     _clientLabel1Id = "<%=this.Label1.ClientID %>";
     _clientDate = "<%=this.hidTodaysDate.ClientID %>";
    _clientConcession = "<%=this.CmpConcession.ClientID %>";
    _clientPnlFields = "<%=this.pnlFields.ClientID %>";
    _clientbtnSearch1 = "<%=this.btnSearch1.ClientID %>";
    _clienthidURL = "<%=this.hidURL.ClientID %>";
    
    
    
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(EndReqHandler);
     
    function EndReqHandler(sender, args)
    {
       var postBackElement =  sender._postBackSettings.sourceElement;
       if (postBackElement.id == _clientbtnPayPrint)
        {
            if (document.getElementById(_clienthidURL).value != '')
            { 
            window.open( document.getElementById(_clienthidURL).value, '_new','scrollbars=yes,resizable=no,top=0,left=0,width=800,height=400');
            document.getElementById(_clienthidURL).value = '';
            }
        }
        
    }
    
    function SetdefaultButton()
    {
        if(document.getElementById(_clientPnlFields))
        {
            document.getElementById(_clientPnlFields).DefaultButton =  _clientbtnSearch1;
        }
    }
    function displayTotalAmount()
    {
        if(document.getElementById(_clienttxtLateFeeAmountId).value == "")
        {
            document.getElementById(_clienttxtLateFeeAmountId).value = "0";
        }
         if(document.getElementById(_clienttxtConcessionId).value == "")
        {
            document.getElementById(_clienttxtConcessionId).value = "0";
        }
        var iTot =  parseInt(RemoveLeadingZeroes(document.getElementById(_clienttxtDueAmountId).value) )+
                    parseInt(RemoveLeadingZeroes(document.getElementById(_clienttxtLateFeeAmountId).value))-parseInt(RemoveLeadingZeroes(document.getElementById(_clienttxtConcessionId).value));
        if(isNaN(iTot))
        {
            iTot = 0;
      
        }
        document.getElementById(_clienttxtTotalAmountId).value = iTot;
                    
    }

    function ResetPaymentFields()
    {
     if(document.getElementById(_clienttxtDueAmountId))
     {

        document.getElementById(_clienttxtDueAmountId).value = "";
        document.getElementById(_clienttxtLateFeeAmountId).value = "";
        document.getElementById(_clienttxtTotalAmountId).value = "";
        document.getElementById(_clientcmbFeeTypeId).value = "0";
       // document.getElementById("ctl00_MainBody_lblDueDateText").InnerText = "";
      //  document.getElementById("ctl00_MainBody_lblDueDate").InnerText = "";
        ClearIntervalCombo();
       }
    }
    function ClearIntervalCombo()
    {
       if(document.getElementById(_clientcmbInterValId))
       {
        document.getElementById(_clientcmbInterValId).options.length = 1;   
       }
    }
    function ClearErrors()
    {
         document.getElementById(_clientLabel1Id).innerText = "";
         document.getElementById(_clientLabel1Id).innerHTML = "";
    }
    function ValidatePaymentDetails()
    {
        
         document.getElementById(_clientlblStuError).innerText = "";
         document.getElementById(_clientlblStuError).innerHTML = "";
         var bReturn = true;
          sErrorMessage = "Please fix following error(s):";
          var sErrMsgFeeType ="";
          var sErrMsgPymntdt ="";
          var sErrMsgConcession ="";
         if(document.getElementById(_clientcmbFeeTypeId).value == "0")
         {

           
            sErrMsgFeeType = "<BR>\r - Fee type should be selected.";
            document.getElementById(_clientCompareValidator1).isValid == false;
            document.getElementById(_clientlblStuError).style.display = '';
            document.getElementById(_clientcmbFeeTypeId).focus();
            bReturn =  false;
         }
         var iConcessionAmt = document.getElementById(_clienttxtConcessionId).value;
         if( iConcessionAmt!= "")
         {
            if(parseInt(iConcessionAmt) > parseInt(RemoveLeadingZeroes(document.getElementById(_clienttxtDueAmountId).value)))
            {
                  document.getElementById(_clientConcession).isValid == false;
                  sErrMsgConcession = "<BR>\r -Concession amount should not be greater than due amount.";
                  bReturn =  false;
                    document.getElementById(_clienttxtConcessionId).value ="0";
                    document.getElementById(_clienttxtConcessionId).focus();
                  
            }
         }
         if(document.getElementById(_clientcalStartDate).value =="")
         {
            sErrMsgPymntdt ="<BR>\r - Payment date should not be blank. "
            bReturn =  false;
         }
         if(sErrMsgFeeType !="")
         {
            sErrorMessage = sErrorMessage+ sErrMsgFeeType;
         }
         if(sErrMsgPymntdt!="")
         {
            sErrorMessage = sErrorMessage + sErrMsgPymntdt;
         }
         if(sErrMsgConcession != "")
         {
           sErrorMessage = sErrorMessage + sErrMsgConcession;
         }
        if(!bReturn)
        {
            document.getElementById(_clientLabel1Id).innerText = sErrorMessage;
            document.getElementById(_clientLabel1Id).innerHTML = sErrorMessage;
        }
        else
        {
            document.getElementById(_clientLabel1Id).innerText = "";
            document.getElementById(_clientLabel1Id).innerHTML = "";
        }
        if(bReturn)
        {
            bReturn = ConfirmDate();
        }
        return bReturn;
    
    }
    function ChangeLateFess()
    {
        document.getElementById(_clientlblStuError).innerText = "";
         document.getElementById(_clientlblStuError).innerHTML = "";
         var oDtInput, oDtDue;
         var sDate = getCalDateStr(_clientcalStartDate);
         if(document.getElementById(_clientcmbFeeTypeId))
         {
             if(document.getElementById(_clientcmbFeeTypeId).value != "0")
             {
                 if(document.getElementById(_clientlblDueDate)!= null)
                 {
                     var sDueDate = document.getElementById(_clientlblDueDate).value; 
                    
                     if(sDueDate != '' && sDueDate != 'N/A')     
                     {
                        var oDtDue =new Date(document.getElementById(_clientlblDueDate).value);
                        //late fee rate
                        var iAmt = document.getElementById(_clienttxtLateFeeRateId).innerHTML;
                        var diff = sDate.getTime() - oDtDue.getTime();
                        var oneMinute = 60 * 1000;
                        var oneHour = oneMinute * 60;
                        var oneDay = oneHour * 24;
                        diff = Math.floor(diff/oneDay); 
                        var iLatefeeAmt = diff * iAmt;
                        if(iLatefeeAmt < 0 )
                        {
                            iLatefeeAmt = 0;
                        }
                        document.getElementById(_clienttxtLateFeeAmountId).value = iLatefeeAmt; 
                        var iTot =  parseInt(RemoveLeadingZeroes(document.getElementById(_clienttxtDueAmountId).value)) +iLatefeeAmt -parseInt(RemoveLeadingZeroes(document.getElementById(_clienttxtConcessionId).value));
                         document.getElementById(_clienttxtTotalAmountId).value = iTot;
                        
                     }
                     else
                     {
                        document.getElementById(_clienttxtLateFeeAmountId).value = "0";
                        document.getElementById(_clienttxtTotalAmountId).value = document.getElementById(_clienttxtDueAmountId).value;
                     }
                }
           }
           else
           {
            document.getElementById(_clienttxtLateFeeAmountId).value = "0";
             document.getElementById(_clienttxtTotalAmountId).value ="0"
             document.getElementById(_clienttxtTotalAmountId).value ="0"
             document.getElementById(_clientConcession).value = "0";
           }
       }
    }
    function getCalDateStr( sId)
    {
     var dt = document.getElementById(sId).value;
      var sInputDate;
     if(window.navigator.appName == "Microsoft Internet Explorer")
     {
       sInputDate =  new Date(dt.replace(/-/g, ' ')); 
      }
      else
      {
       sInputDate =  new Date(dt.replace(/-/g, '/')); 
      }
      return sInputDate;
    }
    function ConfirmDate()
    {
      var bResult;
      var sToday = Date(document.getElementById(_clientDate).value);
     
      sToday = getFormattedDate(sToday); 
      var sInputDate = getCalDateStr(_clientcalStartDate);
      sInputDate = getFormattedDate(sInputDate);
      
      
      if(sToday != sInputDate)
      {
         if (!window.confirm("Payment date is modified. Are you sure you want to continue?") )
         { 
                bResult= false;
         }
        else
        {
            bResult= true;
        }
       
      }
     
    
      return bResult;

      
    }
*/

    </script>

</asp:Content>
