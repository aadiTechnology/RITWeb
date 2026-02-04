<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="LeavingCertificateConfigUI.aspx.cs" Inherits="LeavingCertificateConfigUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" Runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%; vertical-align: top">
	<tr>
		<td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
			<span class="ClsMdtStar">* <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label> </span>
		</td>
	</tr>
	<tr>
		<td align="left">
			<asp:ValidationSummary ID="valSummary"
								   runat="server"
								   CssClass="NewClsLabel"
								   ShowSummary="true" />
		</td>
	</tr>
    <tr>
		<td align="center">
			<asp:Label ID="lblErrorMessage"
					   runat="server"
					   EnableViewState="false"
					   ForeColor="Red"
					   Width="100%"
					   Visible="false"
					   CssClass="NewClsLabel"></asp:Label>
		</td>
    </tr>
	<tr>
		<td align="center">
			<asp:Label ID="lblUpdateMessage"
					   runat="server"
					   EnableViewState="false"
					   ForeColor="Blue"
					   Width="100%"
					   Visible="false"
					   CssClass="NewClsLabel"
					   Font-Bold="true"></asp:Label>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:ListView ID="lstvwLCDetails"
						  runat="server"
						  DataKeyNames="SchoolId,OriginalId,Id" 
						  OnItemDataBound="lstvwLCDetails_ItemDataBound">
				<LayoutTemplate>
					<table id="grdLCDetails" runat="server" border="0" cellpadding="0" cellspacing="1" class="GridBorder" width="80%">
						<tr id="trHeader" runat="server" class="ClsGridHeader">
							<td align="center" style="padding: 0 4px;width:50px">
								<input ID="chkAll"
									   type="checkbox"
									   runat="server"
									   onclick="CheckUncheckAll(_clientChkAllId, _clientGridId, '_chkSelect')" />
							</td>
							<td style="padding: 0 4px;width:200px;">
								<asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, ReportDetails%>"></asp:Label>

							</td>
							<td style="padding: 0 4px;">
								<asp:Label ID="Label1"  runat="server" Text="<%$ Resources:LocalizedResources, DisplayedOnReportAs%>"></asp:Label>
							</td>
                            <td style="padding: 0 4px;width:100px;" align="center">
								<asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, SortOrder%>"></asp:Label>
							</td>
                            <td style="padding: 0 4px;" align="left">
                                Default Value
                            </td>
						</tr>
						<tr id="itemPlaceHolder" runat="server"></tr>
					</table>
				</LayoutTemplate>
				<ItemTemplate>
					<tr id="trGridRow" runat="server" class="ClsGridRow">
						<td align="center" style="padding:4px;">
							<asp:CheckBox ID="chkSelect"
										  runat="server"
										  onclick="ChkOnChange(this)"/>
						</td>
						<td style="padding:4px;">
							<span id="txtOriginalName" runat="server">
								<%# Eval("OriginalName") %>
							</span>
						</td>
						<td valign="top" style="padding:4px;">
							<asp:TextBox ID="txtLCDetailsName"
										 runat="server"
										 MaxLength="90"
										 Width="90%"
										 CssClass="LrgTxtBox"
										 Text='<%# Eval("Name") %>'></asp:TextBox>
							<span id="mdtStar" runat="server" class="ClsMdtStar"> * </span>
						</td>
                        <td valign="top" style="padding:4px" align="center">
                        <asp:DropDownList ID="ddlSortOrder" runat="server" CssClass="SmlCombo" Width="70px"></asp:DropDownList>
                        <span id="mdtStarSortOrder" runat="server" class="ClsMdtStar"> * </span>                        
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDefaultValue" runat="server" style="padding:4px;margin-left:4px;" CssClass="LrgTxtBox" Width="98%" MaxLength="100" Text='<%# Eval("DefaultValue") %>'></asp:TextBox>
                        </td>
					</tr>
				</ItemTemplate>
			</asp:ListView>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:Button ID="btnSave"
						runat="server"
						CssClass="ClsBtn"
						Text= "<%$ Resources:LocalizedResources, Save %>"
						disable-page="false"
						OnClick="btnSave_Click" />
			<asp:CustomValidator ID="cstCustomValidator"
								 runat="server"
								 EnableClientScript="true"
								 ClientValidationFunction="cstValidate"
								 Display="None"
								 ErrorMessage="<%$ Resources:LocalizedResources, AtLeastOneReportDetailShouldBeSelected %>"
								 SetFocusOnError="true"></asp:CustomValidator>
			<asp:CustomValidator ID="cstCustomNameValidator"
								 runat="server"
								 EnableClientScript="true"
								 ClientValidationFunction="cstValidateNames"
								 Display="None"
								 ErrorMessage= "<%$ Resources:LocalizedResources, FieldToBeDisplayedOnReportShouldNotBeEmpty %>"
								 SetFocusOnError="true"></asp:CustomValidator>
             <asp:CustomValidator ID="cstvalSort"
								 runat="server"
								 EnableClientScript="true"
								 ClientValidationFunction="cstValidateOrder"
								 Display="None"
								 ErrorMessage= "<%$ Resources:LocalizedResources, PleaseSelectAValidSortOrder %>"
								 SetFocusOnError="true"></asp:CustomValidator>
            <asp:CustomValidator ID="cstvalSortOrder"
								 runat="server"
								 EnableClientScript="true"
								 ClientValidationFunction="cstValidateSortorder"
								 Display="None"
								 ErrorMessage= "<%$ Resources:LocalizedResources, PleaseSelectAValidSortOrder %>"
								 SetFocusOnError="true"></asp:CustomValidator>
		</td>
	</tr>
</table>
<asp:HiddenField ID="hidSortOrderRepeatedForTheRowNo" runat="server"  />
<asp:HiddenField ID="hidCultureInfo" runat="server"  />
<asp:HiddenField ID="hidSortOrderIsMissingForTheRowNo" runat="server"  />
<script type="text/javascript">

    _clientlblUpdateMessage="<%=this.lblUpdateMessage.ClientID %>"
var _clientGridId = '<%=this.lstvwLCDetails.ClientID%>';
var _clientChkAllId = _clientGridId + '_chkAll';
var _iRowCount = '<%=this.lstvwLCDetails.Items.Count%>';
var _clientcstvalSortOrder = '<%=this.cstvalSortOrder.ClientID %>';
var _clientcstvalSort = '<%=this.cstvalSort.ClientID %>';

// This function is used Check/Uncheck all Checkboxes.
function CheckUncheckAll(HeaderCheckboxe, listview, itemName) {
	var checkAll = document.getElementById(HeaderCheckboxe).checked
	var chk
	var iRowCount = 0
	if (iRowCount < 10)
		chk = document.getElementById(listview + "_ctrl" + iRowCount + itemName)
	else
		chk = document.getElementById(listview + "_ctrl" + iRowCount + itemName)
	while (chk != null) {
	    var txt = document.getElementById(listview + "_ctrl" + iRowCount + '_txtLCDetailsName');
	    var cmbSortOrder = document.getElementById(_clientGridId + "_ctrl" + iRowCount + '_ddlSortOrder');
		if (chk.checked == false) {
			chk.checked = checkAll
			txt.disabled = !chk.checked;
			cmbSortOrder.disabled = !chk.checked;
		}
		else {
			chk.checked = checkAll;
			txt.disabled = !chk.checked;
			cmbSortOrder.disabled = !chk.checked;
		}
		var mdtstar = document.getElementById(_clientGridId + "_ctrl" + iRowCount + '_mdtStar');
		mdtstar.style.visibility = txt.disabled ? "hidden" : "visible";

		var mdtStarSortOrder = document.getElementById(_clientGridId + "_ctrl" + iRowCount + '_mdtStarSortOrder');
		mdtstar.style.visibility = txt.disabled ? "hidden" : "visible";
		mdtStarSortOrder.style.visibility = txt.disabled ? "hidden" : "visible";		
		
		iRowCount = iRowCount + 1
		if (iRowCount < 10)
			chk = document.getElementById(listview + "_ctrl" + iRowCount + itemName)
		else
			chk = document.getElementById(listview + "_ctrl" + iRowCount + itemName)
	}
}

// This function is used to enable/disable textbox when it's checkbox is clicked.
function ChkOnChange(src) {
	var iRowCount = src.id.match(/_ctrl(\d+)_chkSelect/)[1];
	var txt = document.getElementById(_clientGridId + "_ctrl" + iRowCount + '_txtLCDetailsName');
	txt.disabled = !txt.disabled;

	var cmbSortOrder = document.getElementById(_clientGridId + "_ctrl" + iRowCount + '_ddlSortOrder');
	cmbSortOrder.disabled = !cmbSortOrder.disabled;

	var mdtstar = document.getElementById(_clientGridId + "_ctrl" + iRowCount + '_mdtStar');
	var mdtStarSortOrder = document.getElementById(_clientGridId + "_ctrl" + iRowCount + '_mdtStarSortOrder');
	mdtstar.style.visibility = txt.disabled ? "hidden" : "visible";
	mdtStarSortOrder.style.visibility = txt.disabled ? "hidden" : "visible";
}

function cstValidateSortorder(src, args) {
    var sList = "";
    var val1 = 0, val2 = 0;
    var cmbSortOrder1 = null;
    var selectedcheck = null;
    var iRowCount = 0, iRowCount1 = 0;
    selectedcheck = document.getElementById(_clientGridId + "_ctrl" + iRowCount + '_chkSelect');
    cmbSortOrder1 = document.getElementById(_clientGridId + "_ctrl" + iRowCount + "_ddlSortOrder");

    while (selectedcheck != null) {
        if (selectedcheck.checked) {
            val1 = cmbSortOrder1.value;
            iRowCount1 = iRowCount + 1;
            var cmbSortOrder2 = document.getElementById(_clientGridId + "_ctrl" + iRowCount1 + "_ddlSortOrder")
            var chkselect = document.getElementById(_clientGridId + "_ctrl" + iRowCount1 + '_chkSelect');

            while (chkselect != null) {
                if (chkselect.checked) {
                    cmbSortOrder2 = document.getElementById(_clientGridId + "_ctrl" + iRowCount1 + "_ddlSortOrder")
                    val2 = cmbSortOrder2.value;
                    if (val1 == val2 && iRowCount1 != iRowCount && (val1 != "-- Select --" || val2 != "-- Select --")) {
                        if (!sList.match((iRowCount1 + 1))) {
                            sList = sList + "," + (iRowCount + 1) + '->' + (iRowCount1 + 1);
                        }
                    }
                }
                iRowCount1 = iRowCount1 + 1;
                chkselect = document.getElementById(_clientGridId + "_ctrl" + iRowCount1 + '_chkSelect');
                cmbSortOrder2 = document.getElementById(_clientGridId + "_ctrl" + iRowCount1 + "_ddlSortOrder")
            }
        }
        iRowCount = iRowCount + 1;
        selectedcheck = document.getElementById(_clientGridId + "_ctrl" + iRowCount + '_chkSelect');
        cmbSortOrder1 = document.getElementById(_clientGridId + "_ctrl" + iRowCount + "_ddlSortOrder");

    }
    if (sList != "") {
        sList = sList.substring(1);
        (document.getElementById(_clientcstvalSortOrder)).errormessage = document.getElementById("<%=this.hidSortOrderRepeatedForTheRowNo.ClientID %>").value + sList + ".";
        args.IsValid = false;
        return true;
    }

    return false;
   }     


// This function is used to validate if atleast one Report Details is selected
function cstValidate(src, args) {
	if(CheckAtleastOneCheckBox(_clientGridId, 'chkSelect', _iRowCount)) {
		args.IsValid = true;
		return false;
	}

	args.IsValid = false;
	return true;
}

// THis fuction is used to validate if enabled textboxes are not empty
function cstValidateNames(src, args) {
    if (document.getElementById(_clientlblUpdateMessage)) {
        document.getElementById(_clientlblUpdateMessage).innerText = "";
        document.getElementById(_clientlblUpdateMessage).innerHTML = "";
    }
	var grid = $get(_clientGridId + '_grdLCDetails');
	var txtBoxes = grid.getElementsByTagName('INPUT');


	for (var i = 0; i <= (_iRowCount * 2); i++) {
	    var txtBox = txtBoxes[i];
	    if (txtBox && txtBox.type == 'text' && !txtBox.disabled && txtBox.value.trim() == '' && txtBox.id.match('txtDefaultValue') == null) {
			args.IsValid = false;
			return true;
		}
	}

	args.IsValid = true;
	return false;
}

function cstValidateOrder(src, args) {
    var list="";
    if (document.getElementById(_clientlblUpdateMessage)) {
        document.getElementById(_clientlblUpdateMessage).innerText = "";
        document.getElementById(_clientlblUpdateMessage).innerHTML = "";
    }
    
    var iRowCount = 0;
        selectedcheck = document.getElementById(_clientGridId + "_ctrl" + iRowCount + '_chkSelect');
        cmbSortOrder1 = document.getElementById(_clientGridId + "_ctrl" + iRowCount + "_ddlSortOrder");
        while (selectedcheck != null ) {
            val1 = cmbSortOrder1.value;
            if (val1 == "-- Select --" && selectedcheck.checked) {
                list = list+","+ (iRowCount + 1);
            }

            iRowCount = iRowCount + 1;
            selectedcheck = document.getElementById(_clientGridId + "_ctrl" + iRowCount + '_chkSelect');
            cmbSortOrder1 = document.getElementById(_clientGridId + "_ctrl" + iRowCount + "_ddlSortOrder");
        }
        
        if (list != "") {
            list = list.substring(1);
            (document.getElementById(_clientcstvalSort)).errormessage = document.getElementById("<%=this.hidSortOrderIsMissingForTheRowNo.ClientID %>").value + list + ".";
            args.IsValid = false;
            return true;
        }
            
    return false;
}

</script>					  
</asp:Content>