<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="SchoolWebApp.ClearanceListFiltersUI" Codebehind="ClearanceListFiltersUI.ascx.cs" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%--<link href="../Styles/Styles.css" type="text/css" rel="stylesheet" />
<link href="../Styles/Styles1.css" type="text/css" rel="stylesheet" />
<link href="../Styles/Styles2.css" type="text/css" rel="stylesheet" />
<link href="../Styles/Styles3.css" type="text/css" rel="stylesheet" />
<link href="../Styles/round-button.css" type="text/css" rel="stylesheet" />
<link href="../Styles/Toppers.css" type="text/css" rel="stylesheet" />
<link href="../Styles/ppsStyles.css" type="text/css" rel="stylesheet" />
<link rel="SHORTCUT ICON" href="../images/eSchoolApp.ico" type="image/x-icon" />--%>
<style type="text/css">
    .style1
    {
        background-color: #fff;
        border: 1px solid #ddd;
        font-size: 9pt;
        margin: 0;
        padding: 0;
        width: 324px;
    }
    .style2
    {
        width: 213px;
    }
    .style3
    {
        background-color: #fff;
        border: 1px solid #ddd;
        font-size: 9pt;
        margin: 0;
        padding: 0;
        width: 325px;
    }
    .style6
    {
        width: 214px;
    }
    .style8
    {
        background-color: #fff;
        border: 1px solid #ddd;
        font-size: 9pt;
        margin: 0;
        padding: 0;
        width: 276px;
    }
    .style9
    {
        background-color: #fff;
        border: 1px solid #ddd;
        font-size: 9pt;
        margin: 0;
        padding: 0;
        width: 279px;
    }
</style>
<table width="98%" align="center">
    <tr>
        <td align="center">
            <table width="100%" align="center">
                <tr>
                    <td colspan="2" width="100%">
                        <table align="center" cellpadding="1" cellspacing="2" width="100%">
                            <tr>
                                <td class="ClsBorderlight" valign="top">
                                    <asp:RadioButton ID="optRegNo" runat="server" AutoPostBack="true" GroupName="Filter"
                                        OnCheckedChanged="optRegNo_CheckedChanged" />
                                </td>
                                <td class="ClsBorderlight" valign="top">
                                    <span class="ClsLabel">Student Name / Reg. No. :</span>
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtRegNo" runat="server" CssClass="MidTxtBox" MaxLength="50" TabIndex="1"></asp:TextBox>
                                    <%--<asp:Label ID="lblRegNoMandMark" runat="server" CssClass="ClsMdtStar" Height="14px"
                                                            Text="*" Width="14px"></asp:Label>--%>
                                </td>
                            </tr>
                            <tr id="Tr2">
                                <td align="center" class="HilightBGGray" colspan="5">
                                    <img src="../images/ArrowBlueDblRev.gif" /><span class="ClsHilightTextB">OR </span>
                                    <img src="../images/ArrowBlueDblNw.gif" />
                                </td>
                            </tr>
                            <tr id="Tr1">
                                <td  colspan="5">
                                    
                                </td>
                            </tr>
                            <tr id="Tr4">
                                <td  colspan="5">
                                    
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1" valign="top" class="ClsBorderlight">
                                    <asp:RadioButton ID="optPaymentDate" runat="server" AutoPostBack="true" GroupName="Filter"
                                        OnCheckedChanged="optPaymentDate_CheckedChanged" />
                                </td>
                                <td valign="top" colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td class="style3">
                                                <asp:Label  ID="lblPaymenDate" runat="server" class="ClsLabel"> Payment Start Date :</asp:Label>
                                            </td>
                                            <td align="left" valign="top" class="style2">
                                                <asp:TextBox ID="txtPaymentStartDate" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                    TabIndex="2"></asp:TextBox>
                                                <rjs:PopCalendar ID="cFromDate" runat="server" Control="txtPaymentStartDate" Format="dd MMM yyyy"
                                                    ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid from date."
                                                    ControlFocusOnError="True" />
                                                <asp:Label ID="lblFromDateMandMark" runat="server" CssClass="ClsMdtStar" Height="14px"
                                                    Text="*" Width="14px" Visible="False"></asp:Label>
                                            </td>
                                            <td class="style8">
                                                <span class="ClsLabel">End Date :</span>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtPaymentEndDate" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                    TabIndex="3"></asp:TextBox>
                                                <rjs:PopCalendar ID="cToDate" runat="server" Control="txtPaymentEndDate" Format="dd MMM yyyy"
                                                    ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid to date." />
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
                            <tr id="Tr5">
                                <td  colspan="5">
                                    
                                </td>
                            </tr>
                            <tr id="Tr6">
                                <td  colspan="5">
                                    
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
                                            <td class="style1">
                                                <span class="ClsLabel">Clearance Start Date:</span>
                                            </td>
                                            <td align="left" valign="top" class="style6">
                                                <asp:TextBox ID="txtClearanceStartDate" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                    TabIndex="4"></asp:TextBox>
                                                <rjs:PopCalendar ID="calClearanceStartDate" runat="server" Control="txtClearanceStartDate"
                                                    Format="dd MMM yyyy" ShowWeekend="True" Enabled="true" ShowErrorMessage="false"
                                                    InvalidDateMessage="Please select valid from date." ControlFocusOnError="True" />
                                                <asp:Label ID="lblClearanceStartDate" runat="server" CssClass="ClsMdtStar" Height="14px"
                                                    Text="*" Width="14px" Visible="False"></asp:Label>
                                            </td>
                                            <td class="style9">
                                                <span class="ClsLabel">Clearance End Date :</span>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtClearanceEndDate" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                    TabIndex="5"></asp:TextBox>
                                                <rjs:PopCalendar ID="calClearanceEndDate" runat="server" Control="txtClearanceEndDate"
                                                    Format="dd MMM yyyy" ShowWeekend="True" Enabled="true" ShowErrorMessage="false"
                                                    InvalidDateMessage="Please select valid to date." />
                                                <asp:Label ID="lblClearanceEndDate" runat="server" CssClass="ClsMdtStar" Height="14px"
                                                    Text="*" Width="14px" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr7">
                                <td  colspan="5">
                                    
                                </td>
                            </tr>
                            <tr id="Tr8">
                                <td  colspan="5">
                                    
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" class="ClsBorderlight">
                                    <asp:CheckBox ID="chkIncludeAll" runat="server" AutoPostBack="false" TabIndex="6" />
                                </td>
                                <td colspan="2" valign="top" class="ClsBorderlight">
                                    <span class="ClsLabel">Include payments which are cleared.</span>
                                </td>
                            </tr>
                        </table>
                        <asp:CustomValidator ID="cstForm" Display="None" runat="server" CssClass="ClsMdtStar"
                            Visible="true" ClientValidationFunction="ValidateControls" 
                            ValidationGroup="Show" ></asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

<script language="javascript" type="text/javascript">

    _clientoptClearanceDate = "<%=this.optClearanceDate.ClientID %>"
    _clientoptPaymentDate = "<%=this.optPaymentDate.ClientID %>"
    _clientClearanceStartDate = "<%=this.txtClearanceStartDate.ClientID %>"
    _clientClearanceEndDate = "<%=this.txtClearanceEndDate.ClientID %>"
    _clientPaymentStartDate = "<%=this.txtPaymentStartDate.ClientID %>"
    _clientPaymentEndDate = "<%=this.txtPaymentEndDate.ClientID %>"
    _clientcstForm = "<%=this.cstForm.ClientID %>"
    _clientlblPaymenDate = "<%=this.lblPaymenDate.ClientID %>"

    function ValidateControls(oSrc, args) {        

        document.getElementById(_clientcstForm).errormessage = ""

        if (document.getElementById(_clientoptClearanceDate).checked == true) {
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
        else if (document.getElementById(_clientoptPaymentDate).checked == true) {
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
                var sLabel = (document.getElementById(_clientlblPaymenDate).innerHTML);
                
                document.getElementById(_clientcstForm).errormessage = sLabel.substring(0,sLabel.lastIndexOf("Start"))+" end date should be greater than "+ sLabel.substring(0,sLabel.lastIndexOf("Start")) +" start date.";
                args.IsValid = false
                return true
            }
        }
        args.IsValid = true
        return false
    }
</script>

