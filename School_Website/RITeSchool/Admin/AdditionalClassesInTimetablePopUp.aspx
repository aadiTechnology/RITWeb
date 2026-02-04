<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="AdditionalClassesInTimetablePopUp.aspx.cs" Inherits="AdditionalClassesInTimetablePopUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
 <style type="text/css">
		.notice-popup-wrapper
		{
			position: absolute;
			left: 130px !important;
			top: 169px !important; 
			border: solid 2px darkgreen;
			background-color: lightyellow;
			font-family: Tahoma;
			z-index:2000;
		}
		.notice-popup-title-text
		{
			margin: 0;
			text-align: left;
			font-size: 14px;
		}
		
		.notice-popup-title-closebtn
		{
			float: right;
			cursor: pointer;
		}
		
		.notice-popup-content
		{
			padding: 15px;
			text-align: left;
			vertical-align: top;
			overflow: auto;
		}
		.web_dialog_overlay
		{
			position: absolute;
			height: 100%;
			width: 100%;
			background:#000333 transparent;
			opacity: .15;
			filter: alpha(opacity=15);
			-moz-opacity: .15;
			z-index:1001;
			display: none;
		}
		.lblerrorr {
		    color:#000333;
		    font-size:9pt;
		    font-weight: normal;
		   
		}
		
	</style>
          <div id="overlay" class="web_dialog_overlay">
	         </div>
	      <div id="divlectcount" runat="server" class="notice-popup-wrapper" style="z-index: 5000; width: 350px; height: auto; margin: margin: -181px -354px -24px -573px; background-color: white;
		                  visibility: hidden; display: none;">
		           <div class="notice-popup-title">
			             <span class="notice-popup-title-closebtn" onclick="HidePopup();">
				              <img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
			             </span>
			             <h4 class="notice-popup-title-text ">
				              Increase Lecture Limit
			             </h4>
		           </div>
		           <div id="popShow" class="notice-popup-content" >
		                <table width="100%">
                               <tr>
                                   <div style="padding-left: 7px; font-size: 13;">
                                          <asp:Label ID="ppErrMsg" runat="server" Visible="true"   />
                                   </div> 
                                   <div>
                                         <caption style="margin-left:-19%;font-size: 13;">
                                            <asp:Label ID="Label12" runat="server" Visible="true"  Text="Do you want to increase limit for subject(s)?" />
                                         </caption>
                                   </div>
                               </tr>
                               <tr align="center">
                                   <td style="padding-left: 25px;">
                                         <asp:Button ID="btnIncreaseCnt" runat="server" CssClass="ClsBtn" Text="OK"  UseSubmitBehavior="false"
                                              ValidationGroup="Return" OnClick="btnIncreaseCnt_Click"  />
                                         <asp:Button ID="btnCancel" runat="server"  
                                              CssClass="ClsBtn" OnClientClick="javascript:HidePopup();return false;" Text="Cancel" />
                                   </td>
                               </tr>
                        </table>
		              </div>
	          </div>   
   
   
    <div class="MainBodyDiv">
        <div style="width: 100%; overflow: auto">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td style="background-color: white;" id="MainDataTable" align="center" valign="top">
                        <!-- Data Insert Here -->
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 97%;">
                            <tr>
                                <td align="left">
                                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="left" rowspan="1" style="height: 5%">
                                                <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                                                    <tr>
                                                        <td style="height: 20px">
                                                            <asp:Label ID="lblIndentDetails" runat="server" CssClass="MainTitleHead" Font-Bold="True"
                                                                Text="<%$ Resources:LocalizedResources, AssignAdditionalLecturesToTeacher %>" EnableViewState="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="Tr1">
                                            <td align="left" style="">
                                                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" />
                                            </td>
                                        </tr>
                                        <tr style="height: 0">
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td align="left" width="75%">
                                                            <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                                                ID="UpdatePanel1">
                                                                <ContentTemplate>
                                                                    <asp:Label ID="lblError" CssClass="LblErrorMsg" runat="server" EnableViewState="False"
                                                                        Width="100%" Visible="False"></asp:Label>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="cmbLectNumber" EventName="SelectedIndexChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="cmbWeekday" EventName="SelectedIndexChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="cmbWeekday" EventName="SelectedIndexChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="btnIncreaseCnt" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td width="25%" align="right">
                                                            <span class="ClsMdtStar">* <asp:Label ID="lblMandatoryFields" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label></span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <table width="80%" cellpadding="2" cellspacing="1">
                                                    <tr id="trTeacherName" runat="server">
                                                        <td align="left" class="ClsBorderlight" width="30%">
                                                            <span class="ClsLabel" style="font-weight: bold;"><asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, TeacherName %>"></asp:Label> : </span>
                                                        </td>
                                                        <td align="left" class="ClsHilightBG" width="55%">
                                                            <asp:Label ID="lblTeacherName" runat="server" CssClass="LblNrmlB" EnableViewState="False"></asp:Label>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr id="trClassName" runat="server">
                                                        <td align="left" class="ClsBorderlight" width="30%">
                                                            <span class="ClsLabel" style="font-weight: bold;"><asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, ClassName %>"></asp:Label> : </span>
                                                        </td>
                                                        <td align="left" class="ClsHilightBG" width="50%">
                                                            <asp:Label ID="lblClassName" runat="server" CssClass="LblNrmlB" EnableViewState="False"></asp:Label>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 5px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server"
                                                    ID="UpdatePanel4">
                                                    <ContentTemplate>
                                                        <table width="80%" cellpadding="2" cellspacing="1">
                                                            <tr id="Tr4">
                                                                <td align="left" class="ClsBorderlight" width="30%">
                                                                    <span class="ClsLabel"><asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, Weekday %>"></asp:Label> : </span>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:DropDownList ID="cmbWeekday" runat="server" CssClass="ExLrgCombo" TabIndex="1"
                                                                        AutoPostBack="True" OnSelectedIndexChanged="cmbWeekday_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <span class="ClsMdtStar">*</span>
                                                                    <asp:CompareValidator ID="cmpWeekday" runat="server" ControlToValidate="cmbWeekday"
                                                                        Display="None" ErrorMessage="<%$ Resources:LocalizedResources, WeekdayShouldBeSelected %>" Operator="NotEqual"
                                                                        ValueToCompare="0"></asp:CompareValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="Tr3">
                                                                <td align="left" class="ClsBorderlight" >
                                                                    <span class="ClsLabel"><asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, LectureNumber %>"></asp:Label> : </span>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:DropDownList ID="cmbLectNumber" runat="server" CssClass="ExLrgCombo" TabIndex="2"
                                                                        AutoPostBack="True" OnSelectedIndexChanged="cmbLectNumber_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <span class="ClsMdtStar">*</span>
                                                                    <asp:CompareValidator ID="cmpLectNumber" runat="server" ControlToValidate="cmbLectNumber"
                                                                        Display="None" ErrorMessage="<%$ Resources:LocalizedResources, LectureNumberShouldBeSelected %>" Operator="NotEqual"
                                                                        ValueToCompare="0"></asp:CompareValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trCmbClassSubject" runat="server">
                                                                <td align="left" class="ClsBorderlight" >
                                                                    <span class="ClsLabel"><asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, ClassSubjects %>"></asp:Label> : </span>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:DropDownList ID="cmbClassSubject" runat="server" CssClass="ExLrgCombo" TabIndex="3">
                                                                    </asp:DropDownList>
                                                                    <span class="ClsMdtStar">*</span>
                                                                    <asp:CompareValidator ID="cmpClassSubject" runat="server" ControlToValidate="cmbClassSubject"
                                                                        Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ClassSubjectShouldBeSelected %>" Operator="NotEqual"
                                                                        ValueToCompare="0"></asp:CompareValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trCmbSubTeacher" runat="server">
                                                                <td align="left" class="ClsBorderlight" >
                                                                    <span class="ClsLabel"><asp:Label ID="Label6" runat="server" Text="<%$ Resources:LocalizedResources, SubjectTeacher %>"></asp:Label> : </span>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:DropDownList ID="cmbSubjectTeacher" runat="server" CssClass="ExLrgCombo" TabIndex="3" OnChange="SetValue(this);">
                                                                    </asp:DropDownList>
                                                                    <span class="ClsMdtStar">*</span>
                                                                    <asp:CompareValidator ID="cmpSubjectTeacher" runat="server" ControlToValidate="cmbSubjectTeacher"
                                                                        Display="None" ErrorMessage="<%$ Resources:LocalizedResources, SubjectTeacherShouldBeSelected %>" Operator="NotEqual"
                                                                        ValueToCompare="0"></asp:CompareValidator>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="cmbWeekday" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="cmbLectNumber" EventName="SelectedIndexChanged" />
                                                         <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" runat="server"/>
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:HiddenField ID="hidTeacherId" runat="server" />
                                    <asp:HiddenField ID="hidStandardId" runat="server" />
                                    <asp:HiddenField ID="hidDivisionId" runat="server" />
                                    <asp:HiddenField ID="hidTeacherName" runat="server"></asp:HiddenField>
									<asp:HiddenField ID="hidSubjectTeacherName" runat="server" />
                                    <asp:HiddenField ID="hidMaxLectCntMessage" runat="server" />
                                    <asp:HiddenField ID="hidWantToInrsCnt" runat="server" Value="0"/>
                                    <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                        ID="UpdatePanel2">
                                        <ContentTemplate>
                                            <asp:HiddenField ID="hidIsDataValid" runat="server" />
                                            <asp:HiddenField ID="hidEncrypt" runat="server" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnIncreaseCnt" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" id="tdSubmit" runat="server">
                        &nbsp;
                        <asp:Button ID="btnSubmit" runat="server" CausesValidation="true" Text="<%$ Resources:LocalizedResources, Submit %>" CssClass="ClsBtnSml"
                            BorderStyle="Solid" UseSubmitBehavior="false" OnClick="btnSubmit_Click" TabIndex="4" />
                        <asp:Button ID="btnClose" runat="server" Text="<%$ Resources:LocalizedResources, Close %>" CssClass="ClsBtnSml" CausesValidation="false"
                            BorderStyle="Solid" UseSubmitBehavior="false" TabIndex="5" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript" language="javascript">
        _clientbtnSubmit = "<%=this.btnSubmit.ClientID%>"
        _clientbtnClose = "<%=this.btnClose.ClientID%>"
        _clienthidIsDataValid = "<%=this.hidIsDataValid.ClientID%>"
        _clienthidEncrypt = "<%=this.hidEncrypt.ClientID%>"
        _clienthidSubTeacherName = "<%=this.hidSubjectTeacherName.ClientID%>"
        _clientcstlbl = "<%=this.ppErrMsg.ClientID%>"
        _clientbtnIncreaseCnt = "<%=this.btnIncreaseCnt.ClientID%>"
        _clienthidMaxLectCntMessage = "<%=this.hidMaxLectCntMessage.ClientID%>"
        _clientlblError = "<%=this.lblError.ClientID%>"
        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)
        function EndReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement
            if (postBackElement != null && postBackElement.id == _clientbtnSubmit || postBackElement.id == _clientbtnClose || postBackElement.id == _clientbtnIncreaseCnt) {
                var sIsValid = document.getElementById(_clienthidIsDataValid).value
                if (sIsValid.toLocaleLowerCase() == "True".toLocaleLowerCase()) {
                    window.opener.location = window.opener.location.pathname + "?" + document.getElementById(_clienthidEncrypt).value
                    window.close()
                    window.opener.focus()
                }
                return false
            }
        }
        function closewindow() {

            document.getElementById(_clientbtnSubmit).disabled = true
            document.getElementById(_clientbtnClose).disabled = true
            window.close()
        }
        function refreshParent() {
            window.opener.location.href = window.opener.location.href
            if (window.opener.progressWindow)
                window.opener.progressWindow.close()
            window.close()
        }

        function SetValue(src) {
            var sSubTeacherName = src.options[src.selectedIndex].text;
            $get(_clienthidSubTeacherName).value = sSubTeacherName;
        }

        function ShowPopup(e, sMessage, sAllMessage) {
            var x, y, tt_ovr_
            var ms = sMessage.toString();
            $("#overlay").show();
            $("#divlectcount").hide();
            document.getElementById('<%=ppErrMsg.ClientID %>').visible = true
            document.getElementById(_clienthidMaxLectCntMessage).Text = sMessage.Text
            document.getElementById(_clientcstlbl).innerText = ms
            document.getElementById(_clientcstlbl).innerHTML = ms


            var cssstyle = $get("<%=this.divlectcount.ClientID %>").style
            var btnReturn = $get("<%=this.btnIncreaseCnt.ClientID %>")
            var width = 150
            var height = 80
            var left = parseInt((screen.width / 2) - (width / 2))
            var top = parseInt((screen.height / 2) - (height / 2))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            // Override the z-index of the topmost wz_dragdrop.js D&D item
            cssstyle.zIndex = Math.max((window.dd && dd.z) ? (dd.z + 2) : 0, 1010);
            cssstyle.zIndex = 2000;
            cssstyle.visibility = "visible";
            cssstyle.display = "block";

        }


        function HidePopup() {
            var validationResult = true
            $("#overlay").hide();
            $("#divlectcount").show();
            if (typeof (Page_ClientValidate) == 'function')
                validationResult = Page_ClientValidate("")
            if (validationResult == false)
                return false
            $get("<%=this.divlectcount.ClientID %>").style.visibility = "hidden"
            $get("<%=this.divlectcount.ClientID %>").style.display = "none"
            return false
        }
        
    </script>
</asp:Content>
