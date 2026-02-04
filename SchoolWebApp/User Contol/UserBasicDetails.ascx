<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="SchoolWebApp.UserBasicDetailsUC" Codebehind="UserBasicDetails.ascx.cs" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="EventCalendar" Namespace="ExtendedControls" TagPrefix="Calender" %>
<table border="0" cellpadding="1" cellspacing="2" width="100%">
    <tr>
        <td class="ClsBorderlight" id="tdUC" runat="server">
            <span class="ClsLabel">Pan No. : </span>
        </td>
        <td align="left">
            <asp:TextBox ID="txtPanNo" runat="server" onkeypress="return PreventSpecialChars(event);" TabIndex="101" CssClass="MidTxtBox" MaxLength="20"></asp:TextBox>            
        </td>
    </tr>
    <tr id="trDate" runat="server">
        <td class="ClsBorderlight">
            <span class="ClsLabel">Joining Date : </span>
        </td>
        <td align="left">
            <asp:TextBox ID="txtJoiningDate" CssClass="SmlCombo" runat="server" Style="vertical-align: bottom"
                ValidationGroup="Save" CausesValidation="true" MaxLength="11" onpaste="event.returnValue=false" TabIndex="102"
                ondrop="event.returnValue=false"></asp:TextBox>
            <rjs:PopCalendar ID="calJoiningDate" runat="server" Control="txtJoiningDate" Format="dd MMM yyyy"
                ControlFocusOnError="True" ShowErrorMessage="false" ShowWeekend="true" InvalidDateMessage="Please select valid date."/>           
            <asp:CustomValidator ID="csttxtDateofJoining" runat="server" ClientValidationFunction="validateJoiningDate" ControlToValidate="txtJoiningDate"
                CssClass="ClsMdtStar" Display="None" EnableClientScript="true" ValidationGroup="Save"
                Visible="true"></asp:CustomValidator>
        </td>
    </tr>
    <tr>
        <td class="ClsBorderlight">
            <span class="ClsLabel">Permanent Date : </span>
        </td>
        <td align="left">
            <asp:TextBox ID="txtPermanentDate" CssClass="SmlCombo" runat="server" Style="vertical-align: bottom" TabIndex="103" ValidationGroup="Save"
                MaxLength="11" onpaste="event.returnValue=false" ondrop="event.returnValue=false" CausesValidation="true"></asp:TextBox>
            <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtPermanentDate" Format="dd MMM yyyy"
                ControlFocusOnError="True" ShowErrorMessage="false" ShowWeekend="true" InvalidDateMessage="Please select valid date." />
            <asp:CustomValidator ID="cstPermanentDate" runat="server" ClientValidationFunction="validateDates" ControlToValidate="txtPermanentDate"
                CssClass="ClsMdtStar" Display="None" EnableClientScript="true" ValidationGroup="Save"
                Visible="true"></asp:CustomValidator>
        </td>
    </tr>
    <tr>
        <td class="ClsBorderlight">
            <span class="ClsLabel">Resignation Date : </span>
        </td>
        <td align="left">
            <asp:TextBox ID="txtResignationDate" CssClass="SmlCombo" runat="server" Style="vertical-align: bottom" TabIndex="104"
                CausesValidation="true" MaxLength="11" onpaste="event.returnValue=false" ondrop="event.returnValue=false"></asp:TextBox>
            <rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtResignationDate" Format="dd MMM yyyy" 
                ControlFocusOnError="True" ShowErrorMessage="false" ShowWeekend="true" InvalidDateMessage="Please select valid date." />
            <asp:CustomValidator ID="cstvalResignDate" runat="server" ClientValidationFunction="validateResignDate" ControlToValidate="txtResignationDate"
                CssClass="ClsMdtStar" Display="None" EnableClientScript="true" ValidationGroup="Save"
                Visible="true"></asp:CustomValidator>
        </td>
    </tr>
    <tr>
        <td colspan="2">
           <%-- <asp:HiddenField ID="hidUserIdUC" runat="server" Value="" />--%>
        </td>
    </tr>
</table>
<script language="javascript" type="text/javascript">

    _clienttxtJoiningDate = "<%=this.txtJoiningDate.ClientID %>";
    _clientcsttxtDateofJoining = "<%=this.csttxtDateofJoining.ClientID %>";
    _clienttxtPermanentDate = "<%=this.txtPermanentDate.ClientID %>";
    _clienttxtResignDate = "<%=this.txtResignationDate.ClientID %>";
    _clientcstvalResignDate = "<%=this.cstvalResignDate.ClientID %>";
    _clienttxtPanNo = "<%=this.txtPanNo.ClientID %>";
    _clientcstPermanentDate = "<%=this.cstPermanentDate.ClientID %>";

    function validateJoiningDate(source, args) {
        var txtDOJ = trimAll(document.getElementById(_clienttxtJoiningDate).value);
        var txtDOP = trimAll(document.getElementById(_clienttxtPermanentDate).value);

        var txtStartDate = $get(_clienttxtJoiningDate);
        var dtStartDate = new Date(txtStartDate.value.replace(/-/g, ' '));

        var txtPermanentDate = $get(_clienttxtPermanentDate);
        var dtPermanentDate = new Date(txtPermanentDate.value.replace(/-/g, ' '));

        var bIsValid = true;
        if (txtDOJ.trim() != "") {
            if (!IsValidDate(dtStartDate)) {
                bIsValid = false;
                document.getElementById(_clientcsttxtDateofJoining).errormessage = "Please select a valid Joining Date.";
            }
            else if (txtDOP.trim() != "" && txtDOJ.trim() != "") {

                if (!IsValidDate(dtPermanentDate)) {
                    bIsValid = false;
                    document.getElementById(_clientcsttxtDateofJoining).errormessage = "Please select a valid Permanent Date.";
                }
                else if (IsValidDate(dtPermanentDate) && (dtStartDate > dtPermanentDate)) {
                    bIsValid = false;
                    document.getElementById(_clientcsttxtDateofJoining).errormessage = "Permanent Date should be greater than or equal to Joining Date.";
                }
            }           
        }
        args.IsValid = bIsValid;
        return !bIsValid;
    }

    function validateDates(source, args) {
        var txtDOJ = trimAll(document.getElementById(_clienttxtJoiningDate).value);        
        var txtDOP = trimAll(document.getElementById(_clienttxtPermanentDate).value);
        var txtDOR = trimAll(document.getElementById(_clienttxtResignDate).value);

        var txtStartDate = $get(_clienttxtJoiningDate);
        var dtStartDate = new Date(txtStartDate.value.replace(/-/g, ' '));
        var txtPermanentDate = $get(_clienttxtPermanentDate);
        var dtPermanentDate = new Date(txtPermanentDate.value.replace(/-/g, ' '));
        var txtResignDate = $get(_clienttxtResignDate);
        var dtResignDate = new Date(txtResignDate.value.replace(/-/g, ' '));

        var bIsValid = true;
        if (txtDOP.trim() != "" && txtDOR.trim() == "") {

            if (IsValidDate(dtPermanentDate)) {               
                if (txtDOJ.trim() == "") {
                    bIsValid = false;
                    document.getElementById(_clientcstPermanentDate).errormessage = "Please select a Joining Date first.";
                }
            }

        }
        args.IsValid = bIsValid;
        return !bIsValid;

    }

    function IsValidDate(date) {
        if (typeof (date) == 'string') 
            date = new Date(date);
        return !(date == 'Invalid Date' || date == 'NaN' || date.getFullYear() < 1900);
        
    }

    function validateResignDate(source, args) {
        var txtDOJ = trimAll(document.getElementById(_clienttxtJoiningDate).value);
        var txtDOR = trimAll(document.getElementById(_clienttxtResignDate).value);
        var txtDOP = trimAll(document.getElementById(_clienttxtPermanentDate).value);

        var txtStartDate = $get(_clienttxtJoiningDate);
        var dtStartDate = new Date(txtStartDate.value.replace(/-/g, ' '));

        var txtResignDate = $get(_clienttxtResignDate);
        var dtResignDate = new Date(txtResignDate.value.replace(/-/g, ' '));

        var txtPermanentDate = $get(_clienttxtPermanentDate);
        var dtPermanentDate = new Date(txtPermanentDate.value.replace(/-/g, ' '));

        var bIsValid = true;
        if (txtDOR.trim() != "") {            
            if (txtDOJ.trim() != "") {
                if (!IsValidDate(dtResignDate)) {
                    bIsValid = false;
                    document.getElementById(_clientcstvalResignDate).errormessage = "Please select a valid Resignation Date."
                }
                else if (dtStartDate >= dtResignDate) {
                    bIsValid = false;
                    document.getElementById(_clientcstvalResignDate).errormessage = "Resignation Date should be greater than Joining Date."
                }
                if (txtDOP.trim() != "" && dtPermanentDate >= dtResignDate) {
                    bIsValid = false;
                    document.getElementById(_clientcstvalResignDate).errormessage = "Resignation Date should be greater than Permanent Date and Joining Date."
                }
            }

            else {
                bIsValid = false;
                document.getElementById(_clientcstvalResignDate).errormessage = "Please select a Joining Date first."
            }
        }
        else if (txtDOJ.trim() != "") {
            if (txtDOP.trim() != "" && !IsValidDate(dtPermanentDate)) {
                bIsValid = false;
                document.getElementById(_clientcstvalResignDate).errormessage = "Please select a valid Permanent Date."
            }
        }
        args.IsValid = bIsValid;
        return !bIsValid;

    }

    function PreventSpecialChars(e) {
        var k;
        document.all ? k = e.keyCode : k = e.which;
        return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || (k >= 48 && k <= 57) || k == 0 || k == 9);
    }
    
</script>
