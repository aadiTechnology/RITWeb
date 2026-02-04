<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Schoolwise_Events_Pop_Up.aspx.cs"
    MasterPageFile="../MasterPages/PopupMasterSml.master" Inherits=" AddEventPopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
       <style type="text/css">        
        .LstBoxHeight
        {
           height: 50px !important;                       
        }
        </style>
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 95%;
        vertical-align: top">       
        <tr>
            <td style="background-color: white" id="MainDataTable" align="center" valign="top">
                <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                    <tr>
                        <td style="height: 19px" align="left" colspan="4" valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" width="99%">
                                <tr>
                                    <td class="ClsGrayMainTitle" style="height: 20px; width: 99%;">                                        
                                        <span class="MainTitleHead" style="font-weight: bold"> <asp:Label ID="lblEventsManagements" runat="server"  Text="<%$ Resources:LocalizedResources, EventsManagement %>"></asp:Label></span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="3" style="height: 19px;">
                            <asp:ValidationSummary runat="server" ID="ValErrMsg" ShowMessageBox="false" ShowSummary="true"
                                Width="98%" />
                            <asp:Label ID="lblErrorMsg" runat="server" Text="" CssClass="LblErrorMsg" EnableViewState="False"></asp:Label>                            
                            <asp:CustomValidator Display="None" ID="cstDescription" runat="server" ClientValidationFunction="ValidateDescription"
                                ErrorMessage="<%$ Resources:LocalizedResources, EventDescriptionShouldNotBeBlank %>" SetFocusOnError="True"
                                Style="position: relative">
                            </asp:CustomValidator>                           
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="calStartDate"
                                Display="None" ErrorMessage="<%$ Resources:LocalizedResources, EventStartDateShouldNotBeBlank %>" Style="position: relative"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cst_StartAndEndDate" runat="server" ClientValidationFunction="cstStartAndEndDate"
                                ControlToValidate="calEndDate" SetFocusOnError="True" ErrorMessage="<%$ Resources:LocalizedResources, EventEndDateShouldNotLessThanStartDate %>"
                                Display="None" Style="position: relative"></asp:CustomValidator>
                            <asp:CustomValidator ID="cst_StartDate" runat="server" ClientValidationFunction="cstStartDate"
                                ControlToValidate="calStartDate" Display="None" SetFocusOnError="True" ErrorMessage="<%$ Resources:LocalizedResources, StartDate %>"
                                Style="position: relative"></asp:CustomValidator>
                            <asp:CustomValidator Display="None" ID="cst_EndDate" runat="server" ClientValidationFunction="cstEndDate"
                                ControlToValidate="calEndDate" SetFocusOnError="True" ErrorMessage="<%$ Resources:LocalizedResources, EndDate %>"
                                Style="position: relative">
                            </asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="req_EndDate" runat="server" ControlToValidate="calEndDate"
                                Display="None" ErrorMessage="<%$ Resources:LocalizedResources, EvenEndDateShouldNotBlank %>" Style="position: relative"></asp:RequiredFieldValidator>
                            <asp:CustomValidator Display="None" ID="cstStandard" runat="server" ClientValidationFunction="ValidateStandards"
                                ErrorMessage="<%$ Resources:LocalizedResources, valClassSelected%>"
                                SetFocusOnError="True" Style="position: relative">
                            </asp:CustomValidator>
                        </td>
                        <td align="right" class="ClsTextNormal" style="padding-right: 10px; height: 19px;"
                            valign="top">
                            <span class="ClsMdtStar">* <asp:Label runat="server" ID="Label5" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label></span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="eventpnl" runat="server">
                    <ContentTemplate>
                        <table id="tblUserInfo" border="0" cellpadding="1" cellspacing="2" align="center"
                            style="width: 80%;">
                            <tr>
                                <td align="left" valign="top" class="ClsBorderlight">
                                    <span class="ClsLabel"> <asp:Label runat="server" ID="lblEventsList" Text="<%$ Resources:LocalizedResources, EventsList %>"></asp:Label> : </span>
                                </td>
                                <td class="ClsBorderlight" valign="top">                                    
                                    <span class="ClsLabel"><asp:Label runat="server" ID="Label2" Text="Event Title"></asp:Label> :</span>
                                </td>
                                <td valign="top">
                                </td>
                            </tr>
                            <tr style="vertical-align:top;">
                                <td align="left" class="ClsBorderlight">
                                    <asp:ListBox runat="server" ID="lstEvents" Width="220px" CssClass="LstBoxHeight" AutoPostBack="true"
                                        OnSelectedIndexChanged="lstEvents_SelectedIndexChanged"
                                        DataTextField="Event_Description" DataValueField="Event_Id"></asp:ListBox>
                                </td>
                                <td class="ClsBorderlight" align="left">
                                    <asp:TextBox ID="txtEventDesc" runat="server" TextMode="MultiLine" CssClass="ExLrgTxtBox"
                                        Width="267px" TabIndex="1" MaxLength="100" Height="50px"></asp:TextBox>
                                    &nbsp;
                                </td>
                                <td align="left" valign="top">
                                    <asp:Label ID="Label1" runat="server" CssClass="ClsMdtStar" ForeColor="Red" Text="*"
                                        EnableViewState="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight" valign="top">
                                    <span class="LblRht"> <asp:Label runat="server" ID="lblEventDescription" Text="Description"></asp:Label> :</span>
                                </td>
                                <td align="left" class="ClsBorderlight" valign="top">
                                    <asp:TextBox ID="txtEvevtDescription" CssClass="ExLrgTxtBox" runat="server" AutoPostBack="True"
                                        TabIndex="2" TextMode="MultiLine" MaxLength="600" Width="267px" Height="90px"></asp:TextBox>                                  
                                    
                                </td>
                                <td align="left" valign="top">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight" valign="top">
                                    <span class="LblRht"> <asp:Label runat="server" ID="Label3" Text="<%$ Resources:LocalizedResources, EventStartDate %>"></asp:Label> :</span>
                                </td>
                                <td align="left" class="ClsBorderlight" valign="top">
                                    <asp:TextBox ID="calStartDate" CssClass="SmlCombo" runat="server" AutoPostBack="True"
                                        TabIndex="2"></asp:TextBox>
                                    <rjs:PopCalendar ID="cStartDate" runat="server" Control="calStartDate" Format="dd MMM yyyy" Culture="en"
                                        ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources, EventStartDateShouldNotBeBlank %>" />
                                    <span class="ClsMdtStar">*</span> &nbsp;&nbsp;
                                </td>
                                <td align="left" valign="top">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight" valign="top">
                                    <span class="LblRht"> <asp:Label runat="server" ID="Label4" Text="<%$ Resources:LocalizedResources, EventEndDate %>"></asp:Label> :</span>
                                </td>
                                <td align="left" class="ClsBorderlight" valign="top">
                                    <asp:TextBox ID="calEndDate" CssClass="SmlCombo" runat="server" AutoPostBack="True"
                                        TabIndex="3"></asp:TextBox>
                                    <rjs:PopCalendar ID="csEndDate" runat="server" Control="calEndDate" Format="dd MMM yyyy" Culture="en"
                                        ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources, EvenEndDateShouldNotBlank %>" />
                                    <span class="ClsMdtStar">*</span>&nbsp;&nbsp;&nbsp;
                                </td>
                                <td align="left" valign="top">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight" valign="top">
                                <span class="LblRht"> :</span>
                                    <asp:Label ID="Label6" runat="server" Text="Associated Class(es)" CssClass="LblRht"
                                        EnableViewState="False"></asp:Label><br />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="chkAll" runat="server" Text="<%$ Resources:LocalizedResources, SelectAll %>" />
                                </td>
                                <td class="ClsBorderlight" valign="top" align="left"> 
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>                                   
                                        <asp:ListView ID="lstvwStandardDivisions" runat="server" DataKeyNames="StandardId" 
                                                    OnItemDataBound="lstvwStandardDivisions_ItemDataBound">
                                                    <LayoutTemplate>
                                                        <table align="right" width="100%" runat="server" id="tblStaffInfo" style="color: #333333"
                                                            cellpadding="0" cellspacing="1" class="GridBorder">                                                        
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="trGridRow" runat="server" class="ClsGridAltRow">
                                                            <td align="left" style="padding-left: 5px">
                                                                <asp:CheckBox ID="chkStandard" runat="server" Text='<%# Eval("StandardName") %>'/>                                                            
                                                            </td>
                                                            <td align="left" style="padding-left: 5px">                                                        
                                                                <asp:CheckBoxList ID="chkStandardDivLst" runat="server" RepeatDirection="Horizontal"
                                                                    CssClass="ClsLabel" RepeatColumns="6">
                                                                </asp:CheckBoxList>
                                                            </td>                                
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="trGridRow" runat="server" class="ClsGridAltRow" style="height:10px">
                                                            <td align="left" style="padding-left: 5px">
                                                                <asp:CheckBox ID="chkStandard" runat="server" Text='<%# Eval("StandardName") %>'   />                                                           
                                                            </td>
                                                            <td align="left" style="padding-left: 5px">                                                            
                                                                <asp:CheckBoxList ID="chkStandardDivLst" runat="server" RepeatDirection="Horizontal"
                                                                    CssClass="ClsLabel" RepeatColumns="6">
                                                                </asp:CheckBoxList>
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td class="LblNoRecord" align="center">
                                                                <asp:Label ID="lblNoRecord" runat="server" Text= "<%$ Resources:LocalizedResources, NoRecordsFound%>" 
                                            EnableViewState="False"></asp:Label>       
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="lstEvents" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight" valign="top">
                                </td>
                                <td align="left" class="ClsBorderlight" valign="top">
                                    <asp:CheckBox ID="chkDisplayOnHomepage" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, DisplayOnHomepage %>" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight" width="90px">
                                    <span class="ClsLabel">Select Photo :</span>
                                </td>
                                <td align="left" width="200px">
                                    <asp:FileUpload ID="FilUpImg" runat="server" CssClass="LrgTxtBox" Width="200px" />
                                    <asp:ImageButton ID="btnView" runat="server"  CausesValidation="false" ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible = "false"  /> 
                                    <asp:ImageButton ID="imgbtnDelete" runat="server"  CausesValidation="false" 
                                        ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" 
                                        OnClientClick="return ConfirmDelete()" Visible = "false" 
                                        onclick="imgbtnDelete_Click" EnableViewState = "true"  />                                     
                                </td>                                                             
                            </tr>
                            <tr>
                                <td>
                                
                                </td>
                                <td class="ClsBorderlight">
                                    <span class="LblSmlGray">(Supports only .PDF, .JPG, .PNG, .BMP, .JPEG file type. File size should not exceed
                                                    1MB.)&nbsp; &nbsp;&nbsp;</span>
                                </td>
                            </tr>
                            <asp:HiddenField ID="hidEventImage" runat="server" Value = "" />
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="imgbtnDelete" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 95%;
        vertical-align: top">
        <tr>
            <td align="center" colspan="2">
                <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                    <tr>
                        <td align="left" style="width: 25%; height: 20px">
                        </td>
                        <td align="right" style="width: 25%; height: 20px">
                            <asp:Button CssClass="ClsBtn" ID="BtnNew" runat="server" Text="<%$ Resources:LocalizedResources, New %>" BorderWidth="1px"
                                UseSubmitBehavior="false" OnClick="BtnNew_Click" CausesValidation="false" ValidationGroup="1">
                            </asp:Button>
                        </td>
                        <td align="left" style="width: 5%; height: 20px">
                            <asp:Button CssClass="ClsBtn" ID="btnsave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>" BorderWidth="1px"
                                OnClick="BtnSave_Click" disable-page="true"></asp:Button>
                        </td>
                        <td align="left" style="height: 20px">
                            <asp:UpdatePanel ID="pnl" runat="server">
                                <ContentTemplate>
                                    <asp:Button CssClass="ClsBtn" ID="BtnDelete" CausesValidation="false" runat="server"
                                        Text="<%$ Resources:LocalizedResources, Delete %>" BorderWidth="1px" UseSubmitBehavior="false" OnClick="BtnDelete_Click"
                                        Visible="false"></asp:Button>
                                    <asp:HiddenField ID="hidIsNewRecord" runat="server" Value="true" />
                                    <asp:HiddenField ID="hidEventID" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidYearEndDate" runat="server" />
                                    <asp:HiddenField ID="hidYearStartDate" runat="server" />
                                    <asp:HiddenField ID="hidEndDate" runat="server" />
                                    <asp:HiddenField ID="hidChkLstCnt" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td align="left" style="width: 25%; height: 20px">
                            <asp:Button CssClass="ClsBtn" ID="btncancel" CausesValidation="false" runat="server"
                                Text="<%$ Resources:LocalizedResources, Close %>" BorderWidth="1px" UseSubmitBehavior="false"></asp:Button>
                        </td>
                        <td align="left" style="width: 25%; height: 20px">
                            &nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hidValEventStartDate" runat="server" />
                <asp:HiddenField ID="hidValEventEndDate" runat="server" />
                <asp:HiddenField ID="hidAreYouSureYouWantDeleteEvent" runat="server" />
                <asp:HiddenField ID="hidValEventDisplayOnHomePage" runat="server" />
                <asp:HiddenField ID="hidValEventLength" runat="server" />
                <asp:HiddenField ID="hidEventDescriptionShouldNotBeBlank" runat="server" />
            </td>
            <td align="left" colspan="1" style="width: 3px">
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">

        _clientYearStartDate = "<%=this.hidYearStartDate.ClientID %>";
        _clientYearEndDate = "<%=this.hidYearEndDate.ClientID %>";
        _clientCstStartDate = "<%=this.cst_StartDate.ClientID %>";
        _clientCstEndDate = "<%=this.cst_EndDate.ClientID %>";
        _clientcalStartDateID = "<%=this.calStartDate.ClientID %>";
        _clientcalEndDateID = "<%=this.calEndDate.ClientID %>";
        _clienthidEndDateID = "<%=this.hidEndDate.ClientID %>";
        _clientbtnSave = "<%=this.btnsave.ClientID %>";
        _clientbtnCancel = "<%=this.btncancel.ClientID %>";
        _clientbtnBtnNew = "<%=this.BtnNew.ClientID %>";
        _clientBtnDelete = "<%=this.BtnDelete.ClientID %>";
        _clienttxtEventDesc = "<%=this.txtEventDesc.ClientID %>";
        _clienthidChkLstCnt = "<%=this.hidChkLstCnt.ClientID %>";
        _clientchkAll = "<%=this.chkAll.ClientID %>";
        _clientchkdisplayonhomepage = "<%=this.chkDisplayOnHomepage.ClientID %>";
        _clientCustvalDescription = "<%=this.cstDescription.ClientID %>";        
        _clientlstvwStandardDivisions = "<%=this.lstvwStandardDivisions.ClientID %>"
        var isSaveButton = "Y";

        function CheckOrUncheckAllCheckBox() {
            var listView = document.getElementById('<%= lstvwStandardDivisions.FindControl("tblStaffInfo").ClientID %>');
                        
            for (var i = 0; i < listView.rows.length; i++) {
                var inputs = listView.rows[i].getElementsByTagName('input');
                for (var j = 0; j < inputs.length; j++) {
                    if (inputs[j].type == "checkbox") {
                        if (document.getElementById(_clientchkAll).checked)
                            inputs[j].checked = true;
                        else
                            inputs[j].checked = false;
                    }
                }
            }
        }

        function cstStartAndEndDate(aSrc, args) {
            var dtEndDate, dtStartDate;
        	if (isSaveButton == "Y") {
                if (document.all) {
                    dtEndDate = new Date(document.getElementById(_clientcalEndDateID).value.replace('-', ' '));
                	dtStartDate = new Date(document.getElementById(_clientcalStartDateID).value.replace('-', ' '));
                }
                else {
                    dtEndDate = new Date(convertvaliddate(document.getElementById(_clientcalEndDateID).value));
                	dtStartDate = new Date(convertvaliddate(document.getElementById(_clientcalStartDateID).value));
                }
                document.getElementById(_clienthidEndDateID).value = document.getElementById(_clientcalEndDateID).value;
        		var strStartDate = document.getElementById(_clientcalStartDateID).value;
        		var strEndDate = document.getElementById(_clientcalEndDateID).value;
        		if (!(dtStartDate <= dtEndDate)) {
                    args.IsValid = false;
        			return true;
        		}
                else {
                    args.IsValid = true;
        			return false;
        		}
            }
            else {
                args.IsValid = true;
        		return false;
        	}
  }

        function closewindow(StandardId) {
            document.getElementById(_clientbtnSave).disabled = true;
        	document.getElementById(_clientbtnCancel).disabled = true;
        	document.getElementById(_clientbtnBtnNew).disabled = true;
        	if (document.getElementById(_clientBtnDelete) != null) {
                document.getElementById(_clientBtnDelete).disabled = true;
        	}
            var xmlHttpObj = CreateHTTPReqObj();
        	if (xmlHttpObj) {
                var dtEventDate = document.getElementById(_clientcalStartDateID).value;
        		var url = "../Ajax.ashx?EventDate=EventDate=" + dtEventDate + "&Standard_Id=" + StandardId + "&task=CloseEventWindow";
        		xmlHttpObj.open("GET", url, true);
        		xmlHttpObj.onreadystatechange = function () {
                    if (xmlHttpObj.readyState == 4) {
                        if (xmlHttpObj.status == 200) {
                            var optionText = xmlHttpObj.responseText;
                        	var sQueryString = "../Common/AnnualEventPlanner.aspx?" + optionText;
                        	window.opener.location = sQueryString;
                        	window.opener.focus();
                        	window.close();
                        }
                    }
                };
        		xmlHttpObj.send(null);
        	}
            else {
                alert('Sad!!');
        	}
        }

        function CheckDate() {
            var sDate;
        	if (document.all)
                sDate = new Date(document.getElementById(_clientcalStartDateID).value.replace('-', ' '));
        	else
                sDate = new Date(convertvaliddate(document.getElementById(_clientcalStartDateID).value));
        	document.getElementById(_clientcalEndDateID).value = document.getElementById(_clientcalStartDateID).value;
     }


     function ValidateStandards(aSrc, args) {         
            var j = 0;
            var checks = document.forms[0].elements;
            var boxLength = checks.length;

            for (i = 0; i < boxLength; i++) {
                if (checks[i].type == 'checkbox') {
                    if (checks[i].checked == true) {
                        j++;
                    }
                }
            }

            if (j > 0) {
                args.IsValid = true;
                return false;
            }
            else {
                args.IsValid = false;
                return true;
            }
        }


        function DisableButtons() {
            if (typeof (Page_ClientValidate) == 'function') {
                isPageValid = Page_ClientValidate();
            }
            if (isPageValid) {
                
            	document.getElementById(_clientbtnBtnNew).disabled = true;
            	if (document.getElementById(_clientBtnDelete) != null) {
                    document.getElementById(_clientBtnDelete).disabled = true;
            	}
            }
        }

        function cstStartDate(aSrc, args) {
            var dtEndDate, dtStartDate;
        	if (isSaveButton == "Y") {
                if (document.all) {
                    dtEndDate = new Date(document.getElementById(_clientcalEndDateID).value.replace('-', ' '));
                	dtStartDate = new Date(document.getElementById(_clientcalStartDateID).value.replace('-', ' '));
                }
                else {
                    dtEndDate = new Date(convertvaliddate(document.getElementById(_clientcalEndDateID).value));
                	dtStartDate = new Date(convertvaliddate(document.getElementById(_clientcalStartDateID).value));
                }
                document.getElementById(_clienthidEndDateID).value = document.getElementById(_clientcalEndDateID).value;
        		if (!(CheckIfDateInAcademicYear(dtStartDate))) {
                    var dtYearStartDate = new Date(document.getElementById(_clientYearStartDate).value);
        			var dtYearEndDate = new Date(document.getElementById(_clientYearEndDate).value);
        			var strStartYear = getDateString(dtYearStartDate);
        			var strEndYear = getDateString(dtYearEndDate);
        			document.getElementById(_clientCstStartDate).errormessage = document.getElementById("<%=this.hidValEventStartDate.ClientID %>").value.replace('%StartYear%', strStartYear).replace('%EndYear%', strEndYear); 
        			args.IsValid = false;
        			return true;
        		}
                else {
                    args.IsValid = true;
        			return false;
        		}
            }
            else {
                args.IsValid = true;
        		return false;
        	}
  }

        function cstEndDate(aSrc, args) {
            var dtEndDate, dtStartDate;
        	if (isSaveButton == "Y") {
                if (document.all) {
                    dtEndDate = new Date(document.getElementById(_clientcalEndDateID).value.replace('-', ' '));
                	dtStartDate = new Date(document.getElementById(_clientcalStartDateID).value.replace('-', ' '));
                }
                else {
                    dtEndDate = new Date(convertvaliddate(document.getElementById(_clientcalEndDateID).value));
                	dtStartDate = new Date(convertvaliddate(document.getElementById(_clientcalStartDateID).value));
                }
                document.getElementById(_clienthidEndDateID).value = document.getElementById(_clientcalEndDateID).value;
        		if (!(CheckIfDateInAcademicYear(dtEndDate))) {
                    var dtYearStartDate = new Date(document.getElementById(_clientYearStartDate).value);
        			var dtYearEndDate = new Date(document.getElementById(_clientYearEndDate).value);
        			var strStartYear = getDateString(dtYearStartDate);
        			var strEndYear = getDateString(dtYearEndDate);
        			document.getElementById(_clientCstEndDate).errormessage = document.getElementById("<%=this.hidValEventEndDate.ClientID %>").value.replace('%StartYear%', strStartYear).replace('%EndYear%', strEndYear);
        			args.IsValid = false;
        			return true;
        		}
                else {
                    args.IsValid = true;
        			return false;
        		}
            }
            else {
                args.IsValid = true;
        		return false;
        	}
  }

        function CheckIfDateInAcademicYear(dtObj) {
            var bReturn;
        	var dtYearStartDate = new Date(document.getElementById(_clientYearStartDate).value);
        	var dtYearEndDate = new Date(document.getElementById(_clientYearEndDate).value);
        	if ((dtObj < dtYearStartDate) || (dtObj > dtYearEndDate)) {
                bReturn = false;
        	}
            else {
                bReturn = true;
        	}
            return bReturn;
        }

        function getDateString(obj) {
            var strDate = obj.getDate() + "-";
        	var strMonth = parseInt(obj.getMonth()) + 1;
        	strMonth = getMonthName(strMonth);
        	strDate = strDate + strMonth + "-";
        	strDate = strDate + obj.getFullYear();
        	return strDate;
     }

        function getMonthName(month) {
            switch (month) {
                case 1:
                    return "Jan";
                	break;
            	case 2:
                    return "Feb";
            		break;
            	case 3:
                    return "March";
            		break;
            	case 4:
                    return "April";
            		break;
            	case 5:
                    return "May";
            		break;
            	case 6:
                    return "June";
            		break;
            	case 7:
                    return "july";
            		break;
            	case 8:
                    return "Aug";
            		break;
            	case 9:
                    return "Sep";
            		break;
            	case 10:
                    return "Oct";
            		break;
            	case 11:
                    return "Nov";
            		break;
            	case 12:
                    return "Dec";
            		break;
            }
  }

        function ConfirmDelete() {
            var bResult = true;
            if (!window.confirm(document.getElementById("<%=this.hidAreYouSureYouWantDeleteEvent.ClientID %>").value)) {                
                bResult = false;
        	}
            return bResult;
        } 
              
        function ValidateControls() {
            isSaveButton = "N";
        	if (typeof (Page_ClientValidate) == 'function') {
                validationResult = Page_ClientValidate("");
        	}
            if (validationResult == false) {
                return false;
            }
        }

        function trimAll(sString) {
            while (sString.substring(0, 1) == ' ') {
                sString = sString.substring(1, sString.length);
            }
            while (sString.substring(sString.length - 1, sString.length) == ' ') {
                sString = sString.substring(0, sString.length - 1);
            }
            while (sString.charCodeAt(sString.length - 1) == 10 || sString.charCodeAt(sString.length - 1) == 13) {
                sString = sString.substring(0, sString.length - 1);
            }
            return sString;
        }

        function fnover(varname) {
            var objTXT = document.getElementById(varname);
        	objTXT.style.borderWidth = "1";
        	objTXT.style.borderColor = "maroon";
        	objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)";
     }

        function fnout(varname) {
            var objTXT = document.getElementById(varname);
        	objTXT.style.borderWidth = "1";
        	objTXT.style.borderColor = "#a3c07b";
        	objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)";
     }

        function ValidateDescription(oSrc, args) {
            if (isSaveButton == "Y" && document.getElementById(_clienttxtEventDesc) != null) {
                var description = document.getElementById(_clienttxtEventDesc).value;
            	if (trimAll(description) == "") {
            	    document.getElementById(_clientCustvalDescription).errormessage = document.getElementById("<%=this.hidEventDescriptionShouldNotBeBlank.ClientID %>").value;
                    args.IsValid = false;
            		return true;
            	}
                else if (document.getElementById(_clientchkdisplayonhomepage).checked && description.length > 40) {

                    document.getElementById(_clientCustvalDescription).errormessage = document.getElementById("<%=this.hidValEventDisplayOnHomePage.ClientID %>").value;
                    args.IsValid = false;
                    return true;
                }
                else if (document.getElementById(_clientchkdisplayonhomepage).checked == false && description.length > 100) {
                    document.getElementById(_clientCustvalDescription).errormessage = document.getElementById("<%=this.hidValEventLength.ClientID %>").value;
                    args.IsValid = false;
                    return true;
            	}

            }
            args.IsValid = true;
        	return false;
     }

      //This function is used to open popun on click on link annual planner.
     function OpenWindow(sfilepath) {         
          window.open(sfilepath);
          return false;
      }

      //This function is used take confirmation about delete.
      function ConfirmDelete() {
          return window.confirm('Are you sure you want to delete Event Image?')
      }

      function CheckAll(obj, iRowCount) {
          var chk
          var iRowCnt = 0
          chk = document.getElementById(_clientlstvwStandardDivisions + "_ctrl" + iRowCount + "_chkStandardDivLst_" + iRowCnt);
          while (chk != null) {
              chk.checked = obj.checked;
              iRowCnt = iRowCnt + 1
              chk = document.getElementById(_clientlstvwStandardDivisions + "_ctrl" + iRowCount + "_chkStandardDivLst_" + iRowCnt);
          }
          CheckAllDependancy();
      }

      function CheckAllCheck(iRowCount) {
          var chk, obj
          var isChecked = 1
          var iRowCnt = 0
          obj = document.getElementById(_clientlstvwStandardDivisions + "_ctrl" + iRowCount + "_chkStandard");
          chk = document.getElementById(_clientlstvwStandardDivisions + "_ctrl" + iRowCount + "_chkStandardDivLst_" + iRowCnt);
          while (chk != null) {
              if (!chk.checked) {
                  isChecked = 0;
                  break;
              }
              iRowCnt = iRowCnt + 1

              chk = document.getElementById(_clientlstvwStandardDivisions + "_ctrl" + iRowCount + "_chkStandardDivLst_" + iRowCnt);
          }
          if (isChecked == 0 )
              obj.checked = false;
          else
              obj.checked = true;

          CheckAllDependancy();
      }

      function CheckAllDependancy() {
          var CheckAll = document.getElementById(_clientchkAll).value;
          var v1 = 0;

          var listView = document.getElementById('<%= lstvwStandardDivisions.FindControl("tblStaffInfo").ClientID %>');

          for (var i = 0; i < listView.rows.length; i++) {
              var inputs = listView.rows[i].getElementsByTagName('input');
              for (var j = 0; j < inputs.length; j++) {
                  if (inputs[j].type == "checkbox") {
                      if (!inputs[j].checked) {
                          v1 = 1;
                          break;
                      }
                  }
                  if (v1 == 1)
                      break;
              }
          }
          if (v1 == 1)
              document.getElementById(_clientchkAll).checked = false;
          else
              document.getElementById(_clientchkAll).checked = true;
      }

    </script>       
</asp:Content>
