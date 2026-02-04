<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true"
	CodeFile="AnnualEventPlanner.aspx.cs" Inherits="EventPlanner" ViewStateMode="Enabled" %>
<%@ Register Assembly="EventCalendar" Namespace="ExtendedControls" TagPrefix="Calender" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
	<table align="center" border="0" cellpadding="0" cellspacing="0" style="width: 98%;">
       <tr>
         <td align="right" colspan ="2">
            <span class="ClsMdtStar">*
              <asp:Label ID="Label11" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label></span>
         </td>
        </tr>
		<tr id="trLegend" runat="server" valign="middle">
			<td align="center">
				<table cellpadding="0" cellspacing="0" style="width: 100%;">
					<tr>
						<td align="left" style="width: 50%">
							<table border="0" cellpadding="0" cellspacing="0" id="tblLegend" runat="server">
								<tr>
									<td class="ClsLblLgnd" align="left" style="float:none">
                                      <span class="ClsLblLgnd" style="float:none"><asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, Legend %>"></asp:Label> :</span>
										&nbsp;</td>
									<td align="left" valign="top" class="Holidays">
                                     <span  class="lblWorkingDaysR"><asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, Holidays %>"></asp:Label></span>
									</td>
									<td style="width: 20px">
									</td>
									<td align="left" valign="top" class="PresentDay">                            
										&nbsp;</td>
									<td align="left" valign="top" class="exam">
                                     <span  class="lblPresentDaysR"><asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, Exams %>"></asp:Label></span>
									</td>
									<td style="width: 20px">
									</td>
									<td align="left" valign="top" class="AbsentDay">                            
										&nbsp;</td>
									<td align="left" valign="top" class="Events">
                                      <span  class="lblAbsentDaysR"><asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, Events %>"></asp:Label></span>
									</td>
								</tr>
							</table>
						</td>
						<td align="right">
                            <div class="ClsGreenBG" style="width:130px; height: 18px; vertical-align: bottom; padding: 4px 4px 0 0; margin: 3px;">
                                <a href="#" runat="server" target="_blank" class="SubTitle" onclick="window.open('EventOverview.aspx', '_blank', 'location=0,menubar=0,status=0,titlebar=0,toolbar=0,scrollbars=1,resizable=0,top=0,left=0,width=900,height=650'); return false;"><asp:Label ID="Label10" runat="server" Text="<%$ Resources:LocalizedResources, EventsOverview %>"></asp:Label></a>
                            </div>
						</td>
					</tr>
					<tr>
						 <td align="center" colspan="2">
							<table >
								<tbody>
									<tr>
										<td class="ClsBorderlight" id="tdStd" runat="server" ViewStateMode="Enabled" align="left">											
												<span class="ClsLabel "><asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, SelectStandard %>"></asp:Label> : </span>
										&nbsp;</td>
										<td runat="server" ViewStateMode="Enabled" id="tdCmbStd">
											<asp:DropDownList ViewStateMode="Enabled" AutoPostBack="true" CssClass="ClsSmlTxtBox  " ID="ddlStandard" OnSelectedIndexChanged="ddlStandard_SelectedIndexChanged"
												runat="server">
											</asp:DropDownList>
										</td>
                                        <td class="ClsBorderlight" id="tdDiv" runat="server" ViewStateMode="Enabled" align="left">											
												<span class="ClsLabel "><asp:Label ID="Label12" runat="server" Text="<%$ Resources:LocalizedResources, SelectDivision %>"></asp:Label> : </span>
										&nbsp;</td>
										<td runat="server" ViewStateMode="Enabled" id="tdCmbDiv">
											<asp:DropDownList ViewStateMode="Enabled" AutoPostBack="true" CssClass="ClsSmlTxtBox" Width="70px" ID="ddlDivision" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged"
												runat="server">
											</asp:DropDownList>
										</td>
										<td runat="server" ViewStateMode="Enabled">
											<span class="ClsLabel "><asp:Label ID="Label6" runat="server" Text="<%$ Resources:LocalizedResources, SelectMonth %>"></asp:Label> : </span>
										&nbsp;</td>
										<td >
											<asp:DropDownList ViewStateMode="Enabled" AutoPostBack="true" CssClass="ClsSmlTxtBox " ID="cmbMonth"
												runat="server" OnSelectedIndexChanged ="cmbMonth_SelectedIndexChanged">
											</asp:DropDownList>
										</td>
										<td class="ClsBorderlight">
											 <span class="ClsLabel "><asp:Label ID="Label7" runat="server" Text="<%$ Resources:LocalizedResources, SelectYear %>"></asp:Label> : </span>
										&nbsp;</td>
										<td>
											<asp:DropDownList ViewStateMode="Enabled" AutoPostBack="true" CssClass="ClsSmlTxtBox " ID="cmbYears" 
												runat="server" OnSelectedIndexChanged="cmbYears_SelectedIndexChanged">
											</asp:DropDownList>
										</td>
                                        <td align="right" colspan="2">
                                        <asp:LinkButton ID ="lnkbtnAnnualPlanner"  ViewStateMode="Enabled" runat="server"  CssClass="SMSLblSMlBlue" Style="vertical-align: bottom;
                                            padding-left: 10px; font-size: 9pt; font-weight: bold; font-family: Verdana;">Add Annual Planner</asp:LinkButton>
                                            <asp:LinkButton ID ="lnkbtnAnnualPlannerread"  ViewStateMode="Enabled" runat="server"  CssClass="SMSLblSMlBlue" Style="vertical-align: bottom;
                                            padding-left: 10px; font-size: 9pt; font-weight: bold; font-family: Verdana;">Annual Planner</asp:LinkButton>
                                        </td>
									</tr>
								</tbody>
							</table>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr id="trPlanner" runat="server">
			<td>
				<table id="tblPlanner" runat="server" width="100%">
					<tr>
						<td align="center" style="width: 100%" valign="bottom">
							<asp:Label ID="lblErrMsg" EnableViewState="false" runat="server" CssClass="LblErrorMsg" Visible="false" Width="100%"></asp:Label>
						
                              <asp:Label ID="lblSuccess" runat="server" ForeColor="Blue" Width="100%" 
                                                EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                             <asp:Label ID="lblError" runat="server" ForeColor="Red" Width="100%" 
                                                EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                            </td>
					</tr>
					<tr>
						<td align="center" valign="top" style="padding-top: 10px" id="tdEventCalendar" runat="server">
							<Calender:EventCalendar ID="EventCalendar" ViewStateMode="Enabled" BackColor="#fdf8ef" runat="server" BorderColor="Silver"
								CellPadding="0" DayNameFormat="Full" EventDescriptionColumnName="" Font-Bold="true"
								EventEndDateColumnName="" EventHeaderColumnName="" EventStartDateColumnName="" OnDayRender = "EventCalendar_DayRender"
								Font-Names="Arial" Font-Size="8pt" ForeColor="Black" Height="50px" NextPrevFormat="FullMonth"
								ShowDescriptionAsToolTip="True" ShowGridLines="True" Width="100%" OnVisibleMonthChanged="EventCalendar_VisibleMonthChanged"
								SelectionMode="Day" OnSelectionChanged="EventCalendar_SelectionChanged" 
								ToolTip="<%$ Resources:LocalizedResources, AnnualPlanner %>" TabIndex="1">
								<SelectedDayStyle BackColor="#E7E7E7" Font-Bold="True" ForeColor="Black" BorderColor="LightSteelBlue"
									BorderStyle="Solid" BorderWidth="1px" />
								<SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
								<WeekendDayStyle BackColor="Transparent" Font-Bold="True" />
								<NextPrevStyle Font-Size="10pt" ForeColor="Navy" Font-Bold="true" />
								<DayHeaderStyle Height="25px" CssClass="PlnrDayHeader" />
								<TitleStyle Font-Bold="True" Font-Size="10pt" ForeColor="Black" Height="25px" BorderStyle="None"
									CssClass="PlnrMnthHead" />
								<DayStyle Height="46px" />
							</Calender:EventCalendar>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr id="trNotes" runat="server">
			<td>
				<table style="width: 100%;" runat="server" id="trNote">
					<tr>
						<td align="left" colspan="1" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;
							height: 17px;">							
								<span class="LblNrmlB" style="font-weight:bold"><asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources, Note %>"></asp:Label> :</span></td>
						<td align="left" colspan="1" class="ClsBorderlight" style="padding-left: 5px; width: 70%;
							height: 17px;">							
								<span class="LblSmlV"><asp:Label ID="Label9" runat="server" Text="<%$ Resources:LocalizedResources, MsgAnnualEventPlanner %>"></asp:Label></span>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td>
				<div id="divErr" runat="server"></div>
			</td>
		</tr>
         <tr>
            <td>
                <div id="divAnnualPlanner" runat="server" style="visibility: hidden; display: none; position: absolute;
                    margin: 0px; padding: 0px; width: 415px; height: 150px; border-width: 0px; left: 5px;
                    top: 0px; line-height: normal; border: solid 2px darkgreen; margin: 100px 0px 0px 50px;
                    background-color: white; z-index: 100">
                      <div style="background-color: Transparent; background-image: url(../images/GridHeaderBG.gif);
                        background-repeat: repeat-x; padding: 4px; color: #Black; text-align: right;">
                         <div style="padding: 1px;  font-size: 12px; font-weight: bold;
                            color: #Black; float: left">
                            <asp:Label ID="Label33" runat="server" Text="Add Annual Planner !!!"></asp:Label>
                        </div>
                        <span style="cursor: hand;" onclick="javascript:HidePopup();">
                            <img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
                        </span>
                     </div>
                     <div style="padding: 2px; background-color: ThreeDFace WindowFrame; text-align: left;
                        width: 100%; vertical-align: top; color: #333; overflow: auto; height: 420px;"
                        id="PopupInfo">
                       <table  style="width: 100%" >
                                    <tr id = "trfileuploadcontrol" runat = "server"  >
                                        <td class="ClsBorderlight paddingL" align="center">
                                             <span class="ClsLabel">File Path :</span>
                                        </td>
                                        <td align="left" style="width: 320px" >
                                             <asp:FileUpload ID="fileUploadItems" runat="server" ToolTip="Only PDF,PNG and JPG files are allowed" /><span class="ClsMdtStar">*</span>
                                             <asp:ImageButton ID="btnDelete" runat="server"  CausesValidation="false" ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" OnClientClick="return ConfirmDelete()" onclick="btnDelete_Click" /> 
                                             <asp:ImageButton ID="btnView" runat="server"  CausesValidation="false" ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif"   /> 
                                        </td>
                                     </tr>
                                      <tr id = "trfileuploadnote" runat = "server" >
                                        
                                         <td align="left" class="paddingL" colspan="2">
                                                <span class="LblSmlGray">(Supports only .PDF, .PNG and .JPG file type. File size should not exceed
                                                2 MB.)</span>
                                         </td>
                                        </tr>
                                     <tr style="width:100%">
                                        <td align="center" colspan=2 style="height: 33px">
                                                  <asp:Button ID="btnSave" runat="server" 
                                                  Text="<%$ Resources:LocalizedResources, Save%>" CssClass="ClsBtn"  
                                                  onclick="btnSave_Click"  OnClientClick ="if(!IsFileValid())return false;" />
                                        </td>
                                     </tr>
                          </table>
                       </div>
                    </div>
                </td>
            </tr>
		<tr>
			<td align="left" valign="top">
				<asp:HiddenField ID="hidEventDate" runat="server" />
				<asp:HiddenField ID="hidStandardId" runat="server" />
                <asp:HiddenField ID="hidDivisionId" runat="server" />
				<asp:HiddenField ID="hidCurrentDate" runat="server" Value=""/>
				<asp:HiddenField ID="hidUserHasFullAccess" runat="server" Value="False" />
			</td>
		</tr>
	</table>
     <script language="javascript" type="text/javascript">
         _ClientfileUploadItems = "<%=this.fileUploadItems.ClientID %>";
         _ClientlblSuccess = "<%=this.lblSuccess.ClientID %>";
         _ClientlblError = "<%=this.lblError.ClientID %>";

         //This function is used to open popun on click on link annual planner.
         function OpenWindow(sfilepath) {
             window.open(sfilepath, '_new', 'scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600');
             return false;
         }

         //This function is used to open popun on click on link upload annual planner.
         function ShowAnnualPlannerPopup() {
             document.getElementById(_ClientfileUploadItems).value = '';
             document.getElementById(_ClientlblSuccess).innerHTML = '';
             document.getElementById(_ClientlblError).innerHTML = '';
             var x, y, tt_ovr_
             var cssstyle = $get("<%=this.divAnnualPlanner.ClientID %>").style
             var width = 600
             var height = 380
             var left = parseInt((screen.width / 2) - (width / 2))
             var top = parseInt((screen.height / 2) - (height / 2))
             cssstyle.left = left + "px"
             cssstyle.top = top + "px"
             cssstyle.visibility = "visible"
             cssstyle.display = "block"
         }

         //This function is used to close popup.
         function HidePopup() {
             $get("<%=this.divAnnualPlanner.ClientID %>").style.visibility = "hidden"
             $get("<%=this.divAnnualPlanner.ClientID %>").style.display = "none"
             return false
         }

         //This function is used take confirmation about delete.
         function ConfirmDelete() {
             return window.confirm('Are you sure you want to delete current Annual Planner file?')
         }

         //This function is used to validate is file uploaded by user or not
         function IsFileValid() {
             if (document.getElementById(_ClientlblSuccess)) {
                 document.getElementById(_ClientlblSuccess).innerHTML = "";
                 document.getElementById(_ClientlblSuccess).innerText = "";
             }

             if (document.getElementById(_ClientlblError)) {
                 document.getElementById(_ClientlblError).innerHTML = "";
                 document.getElementById(_ClientlblError).innerText = "";
             }

             var lblUFileNameval = "";
             var myImage = document.getElementById(_ClientfileUploadItems).value;
             var file = document.getElementById(_ClientfileUploadItems);
             if (myImage == "") {
                 alert('File to be uploaded should be selected.');
                 return false;
             }
             else {
                 if (myImage.substr(myImage.lastIndexOf('.'), 4).toUpperCase() == ".PDF") {
                     if (file.size > 2097152) {
                         alert('File size is too large.');
                         return false
                     }
                     else
                         return true;
                 }

                else if (myImage.substr(myImage.lastIndexOf('.'), 4).toUpperCase() == ".PNG") {
                    if (file.size > 2097152) {
                         alert('File size is too large.');
                         return false
                     }
                     else
                         return true;
                 }

                else if (myImage.substr(myImage.lastIndexOf('.'), 4).toUpperCase() == ".JPG") {
                    if (file.size > 2097152) {
                         alert('File size is too large.');
                         return false
                     }
                     else
                         return true;
                 }

                 else {
                     alert('Invalid file type.');
                     return false;
                 }
             }
         }
        
    </script>
</asp:Content>
