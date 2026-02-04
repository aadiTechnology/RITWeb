<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="LessonPlanApprovalUI.aspx.cs" Inherits="LessonPlanApprovalUI" ViewStateMode="Disabled"%>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
    <div id="divPopup" style="display:none">
        <span style="font-weight:bold;color:Black;font-size:medium;font-family:Rockwell"><i class="fa fa-spinner fa-spin progress-spinner"></i>&nbsp;&nbsp;We are saving current Lesson Plan details. Please wait..</span>
    </div>
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="98%">
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="100%">
                        <tr>
                            <td width="10%">
                            </td>
                            <td width="80%" align="left">
                             <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:ValidationSummary ID="valSum" runat="server" CssClass="ClsMdtStar" ViewStateMode="Enabled"/>
                                    <asp:CustomValidator ID="CustomValidator2" runat="server" ViewStateMode="Enabled" Display="None" ClientValidationFunction="ValidateDate"></asp:CustomValidator>
                                    <asp:CustomValidator ID="CustomValidator3" runat="server" ViewStateMode="Enabled" Display="None" ClientValidationFunction="ValidateAcademicDates"></asp:CustomValidator>
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ViewStateMode="Enabled" Display="None" ClientValidationFunction="ValidateComment"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cstValComments" runat="server" ViewStateMode="Enabled" Display="None" ClientValidationFunction="ValidateCommentLength"></asp:CustomValidator>
                                    <asp:CustomValidator ID="valComment" runat="server" ViewStateMode="Enabled" Display="None" ClientValidationFunction="ValidateBlankComment"></asp:CustomValidator>
                                    <asp:CustomValidator ID="CustValidateSubjectDate" runat="server" ViewStateMode="Enabled" Display="None" ErrorMessage="Invalid start date." ClientValidationFunction="ValidateSubjectDate"></asp:CustomValidator>
                                    <asp:CustomValidator ID="CustValidateBlankSubjectDate" runat="server" ViewStateMode="Enabled" Display="None" ClientValidationFunction="ValidateBlankSubjectDate"></asp:CustomValidator>
                                    <asp:CustomValidator ID="CustValidateRangeSubjectDate" runat="server" ViewStateMode="Enabled" Display="None" ClientValidationFunction="ValidateRangeSubjectDate"></asp:CustomValidator>
                                </ContentTemplate>
                                <Triggers>                                                               
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSaveUpper" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSaveComment" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSaveCommentUpper" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnApprove" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnApproveUpper" EventName="Click" />                                    
                                </Triggers>
                             </asp:UpdatePanel>
                            </td>
                            <td width="10%" align="right">
                                <span class="ClsMdtStar">* Mandatory Fields</span>
                            </td>
                        </tr>
                         <tr>
                            <td align="right" colspan="3">
                                <table>
                                <tr>
                                    <td class="ClsGreenBG">
                                         <asp:LinkButton ID ="lnkbtnTranslationTool"  runat="server" ViewStateMode="Enabled"  CssClass="SubTitle">Translation Tool</asp:LinkButton>
                                    </td>
                                    <td class="ClsGreenBG">
                                        <asp:LinkButton ID ="lnkbtnTranslationGuide"  runat="server" ViewStateMode="Enabled" CssClass="SubTitle">Translation Guide</asp:LinkButton>           
                                    </td>
                                </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td align="left" class="ClsBorderlight" width="100px">
                                        <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Teacher : "></asp:Label>
                                    </td>
                                    <td align="left" class="ClsHilightBGB" width="250px">
                                        <asp:Label ID="lblTeacherName" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel" Text=""></asp:Label>
                                    </td>
                                    <td align="left" class="ClsBorderlight" width="100px">
                                        <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text="Start Date : "></asp:Label>
                                    </td>
                                    <td align="left" class="ClsHilightBGB" width="100px" id="tdLegendStartDate" runat="server" viewstatemode="Enabled" visible="false">
                                        <asp:Label ID="lblStartDate" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel" Text=""></asp:Label>
                                    </td>
                                    <td align="left" id="tdStartDate" runat="server" viewstatemode="Enabled" >
                                        <asp:TextBox ID="txtStartDate" CssClass="MidTxtBox" runat="server" ViewStateMode="Enabled" ReadOnly="True" />
                                        <rjs:PopCalendar ID="calStartDate" runat="server" ViewStateMode="Enabled" Control="txtStartDate" Format="dd MMM yyyy"
                                            Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Start Date should not be blank."
                                            AutoPostBack="False" />
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                    <td align="left" class="ClsBorderlight" width="100px">
                                        <asp:Label ID="Label3" runat="server" CssClass="ClsLabel" Text="End Date : "></asp:Label>
                                    </td>
                                    <td align="left" class="ClsHilightBGB" width="100px" id="tdLegendEndDate" runat="server" visible="false">
                                        <asp:Label ID="lblEndDate" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel" Text=""></asp:Label>
                                    </td>
                                    <td align="left" id="tdEndDate" runat="server" viewstatemode="Enabled" >
                                        <asp:TextBox ID="txtEndDate" CssClass="MidTxtBox" runat="server" ViewStateMode="Enabled" ReadOnly="True" />
                                        <rjs:PopCalendar ID="calEndDate" runat="server" ViewStateMode="Enabled" Control="txtEndDate" Format="dd MMM yyyy"
                                            Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="End Date should not be blank."
                                            AutoPostBack="False" />
                                        <span class="ClsMdtStar">* </span>
                                    </td>                                    
                                    <td>                                  
                                        <asp:Button ID="btnSaveDate" runat="server" ViewStateMode="Enabled" Text="Update Date" Visible="false" 
                                           CssClass="ClsBtn" disable-page="true"
                                         UseSubmitBehavior="false" onclick="btnSaveDate_Click" CausesValidation = "false"/>                                                  
                                        
                                    </td>                                                                                                              
                                </tr>
                                <tr id="trWordSearch" runat="server" visible="false">
                                    <td colspan="9" align="center">
                                        <table>
                                            <tr>
                                                <td class="ClsBorderlight" width="150px">
                                                    <span class="ClsLabel">Word / Section :</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="LrgTxtBox" onkeyup="FilterData(this)"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trWords" runat="server" visible="false">
                                    <td class="ClsBorderlight">
                                        <span class="ClsLabel">Words : </span>
                                    </td>
                                    <td colspan="8">
                                        <asp:TextBox ID="txtWords" runat="server" CssClass="ExLrgTxtBox" TextMode="MultiLine" Width="100%" Height="100px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trPhrases" runat="server" visible="false">
                                    <td class="ClsBorderlight">
                                        <span class="ClsLabel">Sentences : </span>
                                    </td>
                                    <td colspan="8">
                                        <asp:TextBox ID="txtPhrases" runat="server" CssClass="ExLrgTxtBox" TextMode="MultiLine" Width="100%" Height="100px"></asp:TextBox>
                                    </td>
                                </tr>
                                <asp:HiddenField ID="hidNewStartDate" runat="server" ViewStateMode="Enabled" />
                                <asp:HiddenField ID="hidNewEndDate" runat="server" ViewStateMode="Enabled" /> 
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveUpper" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveComment" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveCommentUpper" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnBack" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnApprove" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnApproveUpper" EventName="Click" />   
                            <asp:AsyncPostBackTrigger ControlID="btnSaveDate" EventName="Click" />                          
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr class="Height10">
                <td>
                </td>
            </tr>            
            <tr>
                <td colspan="8" align="center">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                    <asp:Button ID="btnSaveUpper" runat="server" ViewStateMode="Enabled" Text="Save" CssClass="ClsBtn" disable-page="true"
                                UseSubmitBehavior="false" onclick="btnSaveUpper_Click"/>
                    <asp:Button ID="btnSaveCommentUpper" runat="server" ViewStateMode="Enabled" Text="Save" CssClass="ClsBtn" 
                                UseSubmitBehavior="false" disable-page="true" onclick="btnSaveCommentUpper_Click"/>
                    <asp:Button ID="btnApproveUpper" runat="server" ViewStateMode="Enabled" Text="Approve" 
                        CssClass="ClsBtn" CausesValidation="false" onclick="btnApproveUpper_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveUpper" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveComment" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveCommentUpper" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnBack" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnApprove" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnApproveUpper" EventName="Click" />                            
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr class="Height10">
                <td>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="80%">
                                <tr>
                                    <td align="center" id="tdMessageTop" runat="server" viewstatemode="Enabled">
                                        <asp:Label ID="lblErrMessageTop" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                            Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveUpper" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnApprove" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnApproveUpper" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveComment" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveCommentUpper" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveDate" EventName="Click" />                          
                            <asp:AsyncPostBackTrigger ControlID="timer" EventName="Tick" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>                           
                            <table id="tblLessons" runat="server" viewstatemode="Enabled" width="80%">
                            </table>
                            <table id="tblComments" runat="server" width="80%">
                            </table>
                            <asp:HiddenField ID="hidUserId" runat="server" ViewStateMode="Enabled" Value="0" />
                            <asp:HiddenField ID="hidStartDate" runat="server" ViewStateMode="Enabled" Value="" />
                            <asp:HiddenField ID="hidEndDate" runat="server" ViewStateMode="Enabled" Value="" />
                            <asp:HiddenField ID="hidOldStartDate" runat="server" ViewStateMode="Enabled" Value="" />
                            <asp:HiddenField ID="hidOldEndDate" runat="server" ViewStateMode="Enabled" Value="" />
                            <asp:HiddenField ID="hidIsReportingUser" runat="server" ViewStateMode="Enabled"  Value="0" />
                            <asp:HiddenField ID="hidAcademicYearStartDate" runat="server" ViewStateMode="Enabled"  Value="0" />
                            <asp:HiddenField ID="hidAcademicYearEndDate" runat="server" ViewStateMode="Enabled" Value="0" />         
                            <asp:HiddenField ID="hidStandardDivIds" runat="server" ViewStateMode="Enabled" Value="" />    
                            <asp:HiddenField ID="hidWords" runat="server" ViewStateMode="Enabled" Value="" />
                            <asp:HiddenField ID="hidPhrases" runat="server" ViewStateMode="Enabled" Value="" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveUpper" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveComment" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveCommentUpper" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnBack" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnApprove" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnApproveUpper" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="timer" EventName="Tick" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="80%">
                                <tr>
                                    <td align="center" id="tdMessage" runat="server" viewstatemode="Enabled" >
                                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                            Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveUpper" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnApprove" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnApproveUpper" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveComment" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveCommentUpper" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="timer" EventName="Tick" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBack" runat="server" ViewStateMode="Enabled" Text="Back" CssClass="ClsBtn" CausesValidation="false"
                                OnClick="btnBack_Click" />
                            <asp:Button ID="btnSave" runat="server" ViewStateMode="Enabled" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" disable-page="true"
                                UseSubmitBehavior="false" />
                            <asp:Button ID="btnSaveComment" runat="server" ViewStateMode="Enabled" Text="Save" CssClass="ClsBtn" UseSubmitBehavior="false" disable-page="true"
                                OnClick="btnSaveComment_Click" />
                            <asp:Button ID="btnApprove" runat="server" ViewStateMode="Enabled" Text="Approve" CssClass="ClsBtn" CausesValidation="false" 
                                OnClick="btnApprove_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveUpper" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveComment" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveCommentUpper" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnBack" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnApprove" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnApproveUpper" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>                    
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlTimer" runat="server">
                        <ContentTemplate>
                            <asp:Timer ID="timer" runat="server" ViewStateMode="Enabled" Interval="300000" Enabled="false" 
                                OnTick="timer_Tick">
                            </asp:Timer>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="timer" EventName="Tick" />                            
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            _isReportingUser = "<%=this.hidIsReportingUser.ClientID %>"
            _clientlblMessage = "<%=this.lblMessage.ClientID %>"
            _clientlblErrMessageTop = "<%=this.lblErrMessageTop.ClientID %>"
            _clienttxtStartDate = "<%=this.txtStartDate.ClientID %>"
            _clienttxtEndDate = "<%=this.txtEndDate.ClientID %>"
            _clienthidAcademicYearStartDate = "<%=this.hidAcademicYearStartDate.ClientID %>"
            _clienthidAcademicYearEndDate = "<%=this.hidAcademicYearEndDate.ClientID %>"
            _clientTimer = "<%=this.timer.ClientID %>"
            _clientbtnSave = "<%=this.btnSave.ClientID %>"
            _clientbtnUpperSave = "<%=this.btnSaveUpper.ClientID %>"
            _clienthidStandardDivIds = "<%=this.hidStandardDivIds.ClientID %>"
            _clienttxtWords = "<%=this.txtWords.ClientID %>"
            _clienttxtPhrases = "<%=this.txtPhrases.ClientID %>"

            function ConfirmSubmit() {
                return window.confirm("After this action you will not be able to change any details. Do you want to continue?");
            }

            function ValidateCommentLength(oSrc, args) {
                var sRows = ""
                var comments = document.getElementsByTagName("textarea");

                for (var k = 0; k < comments.length; k++) {
                    var comment = comments[k]

                    if (comment.value.trim() != "" && comment.value.trim().length > 4000) {
                        if (sRows.match((k + 1)) == null)
                            sRows = sRows + ", " + (k + 1)
                    }
                }

                if (sRows != "") {
                    sRows = sRows.substring(1)
                    if ($('#' + _isReportingUser).val() == "0")
                        oSrc.errormessage = "Comment length should not be greater than 4000 characters for row(s) : " + sRows
                    else
                        oSrc.errormessage = "Lesson Plan details length should not be greater than 4000 characters for row(s) : " + sRows
                    args.IsValid = false;
                    return true;
                }

                args.IsValid = true;
                return false;
            }

            function ValidateComment(oSrc, args) {
                var isFound = false
                var comments = document.getElementsByTagName("textarea");
                for (var k = 0; k < comments.length; k++) {
                    var comment = comments[k]
                    if (comment.value.trim() != "") {
                        isFound = true
                        break;
                    }
                }

                if (!isFound) {
                    if ($('#' + _isReportingUser).val() != "0") 
                    {
                        oSrc.errormessage = "Lesson Plan should be set for at least one parameter.";
                        args.IsValid = false;
                        return true;
                    }
                }

                args.IsValid = true;
                return false;
            }

            function ValidateBlankComment(oSrc, args) {
                var isFound = false
                var comments = document.getElementsByTagName("textarea");
                for (var k = 0; k < comments.length; k++) {
                    var comment = comments[k]
                    if (comment.value.trim() != "") {
                        isFound = true
                        break;
                    }
                }

                if (!isFound) {
                    if ($('#' + _isReportingUser).val() == "0") {
                        oSrc.errormessage = "Comment should not be blank.";
                        args.IsValid = false;
                        return true;
                    }
                }

                args.IsValid = true;
                return false;
            }

            function ClearMessage() {
                if (document.getElementById(_clientlblMessage) != null)
                    $get(_clientlblMessage).innerHTML = "";

                if (document.getElementById(_clientlblErrMessageTop) != null)
                    $get(_clientlblErrMessageTop).innerHTML = "";
            }

            function OpenWindow(sfilepath) {
                window.open(sfilepath, '_new', 'scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600');
                return false;
            }


            function ValidateDate(oSrc, args) {
                if (document.getElementById(_clienttxtStartDate) != null) {
                    var startDate = document.getElementById(_clienttxtStartDate).value;
                    
                    if (document.all)
                        startDate = new Date(startDate.replace('-', ' '));
                    else
                        startDate = new Date(convertdate(startDate));

                    var endDate = document.getElementById(_clienttxtEndDate).value;

                    if (document.all)
                        endDate = new Date(endDate.replace('-', ' '));
                    else
                        endDate = new Date(convertdate(endDate));

                    if (startDate > endDate) {
                        oSrc.errormessage = "End Date should not be less than Start Date.";
                        args.IsValid = false;
                        return true;
                    }
                }
                args.IsValid = true;
                return false;
            }

            function ValidateAcademicDates(oSrc, args) {
                if (document.getElementById(_clienttxtStartDate) != null) {
                    var startDate = document.getElementById(_clienttxtStartDate).value;
                    var endDate = document.getElementById(_clienttxtEndDate).value;
                    var ayStartDate = document.getElementById(_clienthidAcademicYearStartDate).value;
                    var ayEndDate = document.getElementById(_clienthidAcademicYearEndDate).value;

                    var academicYearStartDate = ayStartDate
                    var academicYearEndDate = ayEndDate

                    if (document.all) {
                        startDate = new Date(startDate.replace('-', ' '));
                        endDate = new Date(endDate.replace('-', ' '));
                        academicYearStartDate = new Date(academicYearStartDate.replace('-', ' '));
                        academicYearEndDate = new Date(academicYearEndDate.replace('-', ' '));
                    }
                    else {
                        startDate = new Date(convertdate(startDate));
                        endDate = new Date(convertdate(endDate));
                        academicYearStartDate = new Date(convertdate(academicYearStartDate));
                        academicYearEndDate = new Date(convertdate(academicYearEndDate));
                    }

                    if (startDate < academicYearStartDate || endDate < academicYearStartDate || startDate > academicYearEndDate || endDate > academicYearEndDate) {
                        oSrc.errormessage = "Date(s) should not be out of academic year (" + ayStartDate + " to " + ayEndDate + ").";
                        args.IsValid = false;
                        return true;
                    }
                }
                args.IsValid = true;
                return false;
            }

            function OpenPopup() {
                $('#divPopup').show(); ContentWindow = $('#divPopup').kendoWindow({ title: "Action - Save", visible: false, modal: true, resizable: false, width: '500px',actions:[] }).data("kendoWindow"); ContentWindow.open(); ContentWindow.center();
            }

            function ClosePopup() {
                $("#divPopup").data("kendoWindow").close();
            }

            var prm = Sys.WebForms.PageRequestManager.getInstance()
            prm.add_endRequest(EndReqHandler)
            prm.add_beginRequest(beginRequestHandler)
            function EndReqHandler(sender, args) {
                var postBackElement = sender._postBackSettings.sourceElement
                if (postBackElement.id == _clientTimer || postBackElement.id == _clientbtnSave || postBackElement.id == _clientbtnUpperSave)
                    ClosePopup();
            }
            function beginRequestHandler(sender, args) {
                var postBackElement = sender._postBackSettings.sourceElement
                if (postBackElement.id == _clientTimer || postBackElement.id == _clientbtnSave || postBackElement.id == _clientbtnUpperSave)
                    OpenPopup();
            }

            function CopyToOtherClass(object) {
                var sReturnValue = true;
                if (!window.confirm('This action will copy details of this subject section and paste / overwrite it on subject section of other classes of same standard present on this screen. Do you want to continue?')) {
                    sReturnValue = false;
                }                
                if (sReturnValue == true) {
                    var StandardDivIds = document.getElementById(_clienthidStandardDivIds).value
                    var mainIds = object.id.split('_')
                    if (mainIds.length > 0) {
                        var stdDivId = mainIds[1];
                        var newId = "txtComment_" + mainIds[3] + "_" + mainIds[4];
                        var ids = "";
                        var isFound = false;
                        var stddivIds = StandardDivIds.split('$');
                        for (var i = 0; i < stddivIds.length; i++) {
                            ids = stddivIds[i].split(',')
                            if (ids.length > 0) {
                                for (var j = 0; j < ids.length; j++) {
                                    if (ids[j].trim() == mainIds[3].trim()) {
                                        isFound = true;
                                        break
                                    }
                                }
                            }

                            if (isFound == true) {
                                break;
                            }
                        }

                        $('[id*=' + newId + ']').each(function () {
                            var val = $(this).val()
                            var secondId = this.id.split("_")

                            for (var k = 0; k < ids.length; k++) {
                                if (ids[k] != mainIds[3]) {
                                    var finalId = secondId[2] + "_" + ids[k] + "_" + secondId[4] + "_" + secondId[5] + "_" + secondId[6]
                                    $('[id*=' + finalId + ']').val(val)
                                }
                            }
                        })
                    }
                }
            }

            function ValidateSubjectDate(oSrc, args) {
                var isFound = false;

                var wrongDateClasses = ''

                $('[id*=txtSubjectStartDate]').each(function () {

                    var SubjectStartDate = $(this).val()

                    var id = this.id.replace('StartDate_', 'EndDate_')

                    var SubjectEndDate = $('#' + id).val();

                    if (SubjectStartDate != '' && SubjectEndDate != '') {
                    
                    var isValid = IsValidDate(SubjectStartDate, SubjectEndDate)

                    if (!isValid) {
                        var newId = this.id.replace('txtSubjectStartDate_', 'hid_')
                        var className = $('#' + newId).val();
                        wrongDateClasses = wrongDateClasses + ', ' + className                 
                        }
                    }

                })

                if(wrongDateClasses.length > 0)
                    wrongDateClasses = wrongDateClasses.substring(2)

                if (wrongDateClasses != '') {
                    oSrc.errormessage = "Subject End date should not be less than subject start date for class(s) : " + wrongDateClasses;
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }

            function IsValidDate(SubjectStartDate, SubjectEndDate) {
                if (document.all)
                    SubjectStartDate = new Date(SubjectStartDate.replace('-', ' '));
                else
                    SubjectStartDate = new Date(convertdate(SubjectStartDate));

                if (document.all)
                    SubjectEndDate = new Date(SubjectEndDate.replace('-', ' '));
                else
                    SubjectEndDate = new Date(convertdate(SubjectEndDate));

                if (SubjectStartDate > SubjectEndDate)
                    return false;
                else
                    return true;
            }

            function ValidateBlankSubjectDate(oSrc, args) {
                var isFound = false;

                var wrongDateClasses = ''

                $('[id*=txtSubjectStartDate]').each(function () {

                    var SubjectStartDate = $(this).val()

                    var id = this.id.replace('StartDate_', 'EndDate_')

                    var SubjectEndDate = $('#' + id).val();

                    if (SubjectStartDate == '' || SubjectEndDate == '') {
                        var newId = this.id.replace('txtSubjectStartDate_', 'hid_')
                        var className = $('#' + newId).val();
                        wrongDateClasses = wrongDateClasses + ', ' + className
                    }

                })

                if (wrongDateClasses.length > 0)
                    wrongDateClasses = wrongDateClasses.substring(2)

                if (wrongDateClasses != '') {
                    oSrc.errormessage = "Subject Start/End date should not be blank for class(s) : " + wrongDateClasses;
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }

            function ValidateRangeSubjectDate(oSrc, args) {
                var isFound = false;
                
                var wrongDateClasses = ''

                $('[id*=txtSubjectStartDate]').each(function () {

                    var SubjectStartDate = $(this).val()

                    var id = this.id.replace('StartDate_', 'EndDate_')

                    var SubjectEndDate = $('#' + id).val();

                    if (SubjectStartDate != '' && SubjectEndDate != '') {

                        var isValid = IsValidRangeDate(SubjectStartDate, SubjectEndDate)

                        if (!isValid) {
                            var newId = this.id.replace('txtSubjectStartDate_', 'hid_')
                            var className = $('#' + newId).val();
                            wrongDateClasses = wrongDateClasses + ', ' + className
                        }
                    }

                })

                if (wrongDateClasses.length > 0)
                    wrongDateClasses = wrongDateClasses.substring(2)

                if (wrongDateClasses != '') {
                    oSrc.errormessage = "Subject's Start and End date should be between main start and end date for class(s) : " + wrongDateClasses;
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }


            function IsValidRangeDate(SubjectStartDate, SubjectEndDate) {
                var startDate = document.getElementById(_clienttxtStartDate).value;
                var endDate = document.getElementById(_clienttxtEndDate).value;

                if (document.all)
                    startDate = new Date(startDate.replace('-', ' '));
                else
                    startDate = new Date(convertdate(startDate));

                if (document.all)
                    endDate = new Date(endDate.replace('-', ' '));
                else
                    endDate = new Date(convertdate(endDate));
                
                if (document.all)
                    SubjectStartDate = new Date(SubjectStartDate.replace('-', ' '));
                else
                    SubjectStartDate = new Date(convertdate(SubjectStartDate));

                if (document.all)
                    SubjectEndDate = new Date(SubjectEndDate.replace('-', ' '));
                else
                    SubjectEndDate = new Date(convertdate(SubjectEndDate));

                if (startDate <= SubjectStartDate && SubjectStartDate <= endDate && startDate <= SubjectEndDate && SubjectEndDate <= endDate)
                    return true;
                else
                    return false;
            }

            function FilterData(txt) {
                var val = txt.value.toLowerCase();
                var words = eval('[' + $get('<%= hidWords.ClientID %>').value + ']')[0];
                var phrases = eval('[' + $get('<%= hidPhrases.ClientID %>').value + ']')[0];

                // words
                var resultsWords = $.grep(words, function (elem) {
                    return elem.toLowerCase().indexOf(val) > -1;
                });

                var dataWords = resultsWords.join(', ')

                $('#' + _clienttxtWords).val(dataWords)

                //Phrases
                var resultsPhrases = $.grep(phrases, function (elem) {
                    return elem.toLowerCase().indexOf(val) > -1;
                });

                var dataPhrases = resultsPhrases.join(', ')

                $('#' + _clienttxtPhrases).val(dataPhrases)
            }

        </script>


    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
