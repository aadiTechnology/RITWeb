<%@ Page Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    AutoEventWireup="true" CodeFile="StartNextAcademic.aspx.cs" Inherits="Admin_StartNextAcademic" ViewStateEncryptionMode="Never" EnableEventValidation="false" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.grdConfiguration.ClientID %>";         
    </script>

    <asp:UpdatePanel runat="server" ID="UpnlwizNextAcaGen">
        <ContentTemplate>
            <table>
                <tr>
                    <td align="center">
                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="LblErrorMsg"  HeaderText="<%$ Resources:LocalizedResources, PleaseFixFollowingError %>"
                            ShowMessageBox="false" ShowSummary="true" />
                        <asp:Label ID="lblErrorMsg" runat="server" Visible="False" CssClass="LblNoRecord"
                            EnableViewState="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="height:20px">
                        <asp:Label ID="lblMidAcademicYr" runat="server" Visible="False" CssClass="LblErrorMsg"
                            EnableViewState="False"></asp:Label>
                    </td>
                </tr>                
                <tr>
                    <td align="center">
                        <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="ClsBtnMid" 
                            Text="Back" OnClick="wizNextAcaGen_CancelButtonClick" Visible="False" />
                    </td>
                </tr>
            </table>
            
            <asp:Wizard ID="wizNextAcaGen" runat="server" OnLoad="wizNextAcaGen_Load" DisplaySideBar="False"
                ActiveStepIndex="0" DisplayCancelButton="True" OnNextButtonClick="wizNextAcaGen_NextButtonClick"
                Width="97%" OnFinishButtonClick="wizNextAcaGen_FinishButtonClick" OnCancelButtonClick="wizNextAcaGen_CancelButtonClick"
                OnPreviousButtonClick="wizNextAcaGen_PreviousButtonClick" 
                onactivestepchanged="wizNextAcaGen_ActiveStepChanged">
                <WizardSteps>
                    <asp:WizardStep ID="AcaGenStep0" runat="server" Title="Step 0">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel12">
                            <ContentTemplate>
                                <table cellpadding="0" cellspacing="1" align="center">
                                    <tr>
                                        <td class="ClsBorderlight">
                                            <asp:Label ID="lblStepOption" runat="server" Text="<%$ Resources:LocalizedResources, ChooseTypeOfGeneration %>" CssClass="LblNrmlB"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ClsBorderlight">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ClsBorderlight">
                                            &nbsp;<asp:RadioButton ID="rdoMidAcademic" runat="server" Checked="true" GroupName="AcademicType"
                                                Text="<%$ Resources:LocalizedResources, MidAcademicYearGeneration%>" OnCheckedChanged="rdoMidAcademic_CheckedChanged"
                                                AutoPostBack="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ClsBorderlight">
                                            &nbsp;<asp:RadioButton ID="rdoFinalAcademic" runat="server" GroupName="AcademicType"
                                                Text="<%$ Resources:LocalizedResources, FinalAcademicYearGeneration%>" OnCheckedChanged="rdoMidAcademic_CheckedChanged"
                                                AutoPostBack="true" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <br />
                        <asp:UpdatePanel runat="server" ID="UpdatePanel11">
                            <ContentTemplate>
                                <table cellpadding="0" cellspacing="1" align="center" id="tblChkList" runat="server"
                                    visible="false">
                                    <tr>
                                        <td width="100%" class="ClsBorderLight" align="center">
                                            <asp:Label ID="lblApplicableTo" runat="server" CssClass="NewClsLabel" EnableViewState="False"
                                                Text="<%$ Resources:LocalizedResources, BeforeProceedingToFinalYearGenerationFollowingConfigurationsMustBeConfiguredInMidYearPleaseConfirm%>"
                                                Width="470px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100%" class="ClsBorderLight" align="center">
                                            <asp:CheckBoxList ID="chkListConfiguration" runat="server" CellPadding="0" CellSpacing="0"
                                                CssClass="ClsLabel" Width="100%">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <br />
                        <span style="color: #ff0000"></span>&nbsp;&nbsp;<span style="color: #ff0000" runat="server"
                            id="Span1"></span>
                    </asp:WizardStep>
                    <asp:WizardStep ID="AcaGenStep1" runat="server" Title="Step 1">
                        <table cellpadding="0" cellspacing="1" align="center">
                            <tr>
                                <td class="ClsBorderlight" colspan="2">
                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblNote" runat="server" Text="<%$ Resources:LocalizedResources, TheExistingNewAcademicYearIs %>" CssClass="LblNrmlB"></asp:Label>
                                            </td>
                                            <td class="LblNoRecText paddingLSML">
                                                <asp:Label ID="lblNextAcademicYearVal" runat="server" Text="Label" CssClass="ClsLabel"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="ClsBorderlight" colspan="2">
                                    <asp:Label ID="lblAcademicYearNote" runat="server" Text="<%$ Resources:LocalizedResources, DeleteTheExistingOneBeforeCcreatingAnotherNewAcademicYear %>"
                                        CssClass="LblNrmlB"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trChkDeleteYear" visible="true">
                                <td class="ClsBorderlight" colspan="2">
                                    <asp:CheckBox ID="chkDeleteYear" runat="server" Checked="True" Text="<%$ Resources:LocalizedResources, DeleteExistingNewAcademicYear %>"
                                        CssClass="NewClsLabel" />
                                    <span style="color: #ff0000">*</span><span style="color: #ff0000"></span>
                                </td>
                            </tr>
                            <tr><td>&nbsp;</td></tr>
                            <tr runat="server" id="trgrdAlreadyConfigured" visible="false" style="padding-top;30px;">
                                <td>
                                    <div id="Div2" runat="server" align="center">
                                        <asp:GridView ID="grdAlreadyConfigured" CssClass="GridBorder" runat="server" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="1" DataKeyNames="Configure_Id" ForeColor="#333333"
                                            GridLines="None" PageSize="2000" Width="90%">
                                            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                            <Columns>
                                                <asp:BoundField DataField="Configure_Name" HeaderText="<%$ Resources:LocalizedResources, ConfigurationName %>" SortExpression="Configure_Name">
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                </asp:BoundField>
                                            </Columns>
                                            <RowStyle CssClass="ClsGridRow" />
                                            <PagerStyle CssClass="ClsNwGridPaging" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                                                Font-Underline="False" ForeColor="Black" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="ClsGridHeader" />
                                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <span style="color: #ff0000"></span>
                        <asp:HiddenField ID="hidIsMidCreated" runat="server" />
                        <asp:HiddenField ID="hidNextYearStartDt" runat="server" />
                        <asp:HiddenField ID="hidNextYearEndDt" runat="server" />
                        <asp:HiddenField ID="hidNextAcademiYearId" runat="server" />
                        <asp:HiddenField ID="hidFinalYearGenerated" runat="server" />
                        &nbsp;&nbsp;<span style="color: #ff0000" runat="server" id="spnMandatory"></span>
                    </asp:WizardStep>
                    <asp:WizardStep ID="AcaGenStep2" runat="server" Title="Step 2">
                        <table align="center">
                            <tr>
                                <td colspan="2" class="ClsBorderlight">
                                    <asp:Label ID="lblNewYearNote" runat="server" Text="<%$ Resources:LocalizedResources, NewAcademicYearBsBeingCreatedWithDates%>"
                                        CssClass="LblNrmlB"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" class="ClsBorderlight">
                                    <asp:Label ID="lblStartYear" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, Startdate %>"

                                        EnableViewState="False"></asp:Label>
                                </td>
                                <td valign="top" align="left" class="ClsBorderlight">
                                    <asp:TextBox ID="calStartDate" CssClass="SmlCombo" runat="server" AutoPostBack="True"></asp:TextBox>
                                    <rjs:PopCalendar ID="cStartDate" runat="server" Control="calStartDate" Format="dd mmm yyyy" Culture="en"
                                        ShowWeekend="True" InvalidDateMessage="<%$ Resources:LocalizedResources, CalMsgStartDate %>" ShowErrorMessage="False" />
                                    <asp:Label ID="Label2" runat="server" CssClass="ClsMdtStar" ForeColor="Red" Text="*"
                                        EnableViewState="False"></asp:Label>
                                    <asp:RequiredFieldValidator ID="reqForStartDate" runat="server" ControlToValidate="calStartDate"
                                        CssClass="ClsMdtStar" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ValStartDateBlank %>"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" class="ClsBorderlight">
                                    <asp:Label ID="lblEndYear" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, EndDate %>" EnableViewState="False"></asp:Label>
                                </td>
                                <td align="left" valign="top" class="ClsBorderlight">
                                    <asp:TextBox ID="calEndDate" CssClass="SmlCombo" runat="server" AutoPostBack="True"></asp:TextBox>
                                    <rjs:PopCalendar ID="cEndDate" runat="server" Control="calEndDate" Format="dd mmm yyyy" Culture="en"
                                        ShowWeekend="True" InvalidDateMessage="<%$ Resources:LocalizedResources, CalMsgEndDate %>" ShowErrorMessage="False" />
                                    <asp:Label ID="Label1" runat="server" CssClass="ClsMdtStar" ForeColor="Red" Text="*"
                                        EnableViewState="False"></asp:Label>
                                    <asp:RequiredFieldValidator ID="reqForEndDate" runat="server" ControlToValidate="calEndDate"
                                        CssClass="ClsMdtStar" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ValEndDateBlank %>"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cstEndDate" Display="None" runat="server" CssClass="ClsMdtStar"
                                        ControlToValidate="calEndDate" ErrorMessage="<%$ Resources:LocalizedResources, valForEndDateGreater %>"
                                        ClientValidationFunction="checkEndDate"></asp:CustomValidator>
                                </td>
                            </tr>
                        </table>
                    </asp:WizardStep>
                    <asp:WizardStep ID="AcaGenStep3" runat="server" Title="Step 3">
                        <table align="center">
                            <tr>
                                <td align="center">
                                    <table align="center">
                                        <tr>
                                            <td align="center">
                                                <div class="ClsHilightBGB" style="text-align: left">
                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, SelectTheConfigurationsToCopyThemToNewlyCreatedAcademicYear %>"
                                                        CssClass="LblNrmlB"></asp:Label><br />
                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, TheConfigurationYouDoNotChooseNeedToConfigureManually %>"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <div id="Div1" runat="server" align="center">
                                        <asp:GridView ID="grdConfiguration" runat="server" CssClass="GridBorder" AutoGenerateColumns="False" CellPadding="0"
                                            CellSpacing="1" DataKeyNames="Configure_Id,DependantConf" ForeColor="#333333"
                                            GridLines="None" PageSize="2000" Width="90%">
                                            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, Selects %>"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelectConf" runat="server" />
                                                        <input id="hidRefCount" runat="Server" type="hidden" value="0" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Configure_Name" HeaderText="<%$ Resources:LocalizedResources, ConfigurationName %>" SortExpression="Configure_Name">
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                </asp:BoundField>
                                            </Columns>
                                            <RowStyle CssClass="ClsGridRow" />
                                            <PagerStyle CssClass="ClsNwGridPaging" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                                                Font-Underline="False" ForeColor="Black" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="ClsGridHeader" />
                                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <%--<tr>
                                <td align="center" class="ClsBorderlight LblNrmlB ClspaddingL">
                                    <asp:Label ID="lblDebitEntryNote" BackColor="Azure" runat="server" Text="Late fees, Debit entry and Debit log will not transfer in this wizard, You need to explicitly configure these details before adding new student."></asp:Label>
                                </td>
                            </tr>--%>
                        </table>
                    </asp:WizardStep>
                    <asp:WizardStep ID="AcaGenStep4" runat="server" Title="Step 4">
                        <table style="width: 100%">
                            <tr>
                                <td class="ClsBorderlight">
                                    <asp:Label ID="lbl" runat="server" Text="<%$ Resources:LocalizedResources, DoYouWantToGenerateRollNumbersOfStudentForNewlyCreatedAcademicYear %>"

                                        CssClass="LblNrmlB"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" class="ClsBorderlight">
                                    <asp:CheckBox ID="chkGenRollNos" runat="server"  Checked="true" Text="<%$ Resources:LocalizedResources, GenerateRollNumbersForAllStudents %>" />
                                </td>
                            </tr>
                          <%--  <tr>
                                <td valign="top">
                                    &nbsp;
                                </td>
                            </tr>--%>
                            <%--<tr>
                                <td class="ClsBorderlight">
                                    <asp:Label ID="lblRegNo" runat="server" Text="Do You want to generate registration numbers of student's having provisional admission?"
                                        CssClass="LblNrmlB"></asp:Label>
                                </td>
                            </tr>--%>
                            <%--<tr>
                                <td valign="top" class="ClsBorderlight">
                                    <asp:CheckBox ID="chkGenRegNos" runat="server" Text="Generate registration numbers for provisional students." />
                                </td>
                            </tr>--%>
                             <tr>
                                <td valign="top">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="ClsBorderlight">
                                    <asp:Label ID="lblDebitEntries" runat="server" Text="<%$ Resources:LocalizedResources, DoYouWantToGenerateDefaultDebitEntriesInAcademicYearGeneration %>"
                                        CssClass="LblNrmlB"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" class="ClsBorderlight">
                                    <asp:CheckBox ID="chkGenDebitEntries" runat="server" Checked="true"  Text="<%$ Resources:LocalizedResources, GenerateDefaultDebitEntries %>" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td valign="top">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="ClsBorderlight">
                                    <asp:Label ID="lblTransportData" runat="server" Text="<%$ Resources:LocalizedResources, DoYouWantToGenerateDefaultTransportDataInAcademicYearGeneration %>"
                                        CssClass="LblNrmlB"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" class="ClsBorderlight">
                                    <asp:CheckBox ID="chkTransportData" runat="server" Checked="true"  Text="<%$ Resources:LocalizedResources, GenerateDefaultTransportData %>"
 />
                                </td>
                            </tr>
                        </table>
                    </asp:WizardStep>
                    <asp:WizardStep ID="AcaGenStep5" runat="server" StepType="Complete" Title="Step 5">
                        <table style="width: 100%">
                            <tr>
                                <td align="center">
                                    <div class="ClsHilightBGB" style="width: 55%" align="center">
                                        <asp:Label ID="lblComplete" runat="server" Text="<%$ Resources:LocalizedResources, NextAcademicYearHasBeenGeneratedWithSelectedConfigurations %>"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="center">
                                    &nbsp;<asp:Button ID="btnOK" runat="server" CssClass="ClsBtnMid" OnClick="wizNextAcaGen_CancelButtonClick"
                                        Text="OK" />
                                </td>
                            </tr>
                        </table>
                    </asp:WizardStep>
                </WizardSteps>
                <StepStyle ForeColor="#333333" />
                <SideBarStyle BackColor="#507CD1" VerticalAlign="Top" />
                <SideBarButtonStyle BackColor="#507CD1" Font-Names="Verdana" ForeColor="White" />
                <HeaderStyle BackColor="#284E98" BorderColor="#EFF3FB" BorderStyle="Solid" BorderWidth="2px"
                    Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                <FinishNavigationTemplate>
                    <asp:Button ID="FinishPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                        CssClass="ClsBtnMid" Text="<%$ Resources:LocalizedResources, Previous %>" />
                    <asp:Button ID="FinishButton" runat="server" CommandName="MoveComplete" CssClass="ClsBtnMid"
                        Text="<%$ Resources:LocalizedResources, Finish %>" />
                    <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                        CssClass="ClsBtnMid" Text="<%$ Resources:LocalizedResources, Cancel %>" />
                </FinishNavigationTemplate>
                <StartNavigationTemplate>
                    <asp:Button ID="FinishNextButton" runat="server" CommandName="MoveNext" CausesValidation="False"
                        CssClass="ClsBtnMid" Text="<%$ Resources:LocalizedResources, Next %>" />&nbsp;
                    <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                        CssClass="ClsBtnMid" Text="<%$ Resources:LocalizedResources, Cancel %>" />
                </StartNavigationTemplate>
                <StepNavigationTemplate>
                    <asp:Button ID="StepPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                        CssClass="ClsBtnMid" Text="<%$ Resources:LocalizedResources, Previous %>" />
                    <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" CssClass="ClsBtnMid"
                        Text="<%$ Resources:LocalizedResources, Next %>" />
                    <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                        CssClass="ClsBtnMid" Text="<%$ Resources:LocalizedResources, Cancel %>" />
                </StepNavigationTemplate>
            </asp:Wizard>

           
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript" language="javascript">
        _clientcalEndDate = "<%=this.calEndDate.ClientID %>"
        _clientcalStartDate = "<%=this.calStartDate.ClientID %>"
        _clientChkLstchkListConfigurationId = "<%=this.chkListConfiguration.ClientID%>"
        _clientlblErrorMsg = "<%=this.lblErrorMsg.ClientID %>"
        function NextEnable(icount, sArgs) {
            var j = 0
            for (i = 0; i < icount; i++) {
                ChBoxL_Id = _clientChkLstchkListConfigurationId + '_' + i
                chk = document.getElementById(ChBoxL_Id)
                if (chk != null) {
                    if (chk.checked == true) {
                        j++
                    } 
                } 
            }
            if (j == icount) {
                document.getElementById(sArgs).disabled = false
            }
            else {
                document.getElementById(sArgs).disabled = true
            }
              }
        function Enable(sender, sArgs) {
            if (sender.checked) {
                document.getElementById(sArgs).disabled = false
            }
            else {
                document.getElementById(sArgs).disabled = true
            } 
        }
        function EnableCntrl(sender, sArgs) {
            document.getElementById(sender).disabled = sArgs
           }
        function Check(selfChk, sArgs) {
            var chkbox
            if (sArgs.length > 1) {
                sArrChk = sArgs.split('|')
                if (selfChk.checked == true) {
                    for (var i = 0; i < sArrChk.length; i++) {
                        var HidRefCnt = _clientGridId + sArrChk[i]
                        HidRefCnt = HidRefCnt.replace("chkSelectConf", "hidRefCount")
                        var HidRef = document.getElementById(HidRefCnt)
                        HidRef.value = parseInt(HidRef.value) + 1
                        document.getElementById(_clientGridId + sArrChk[i]).checked = true
                        $("#" + _clientGridId + sArrChk[i]).attr("disabled", true);
                        $("#" + _clientGridId).removeAttr("disabled");
                    }
                } else {
                    for (var i = 0; i < sArrChk.length; i++) {
                        chkbox = document.getElementById(_clientGridId + sArrChk[i])
                        var HidRefCnt = _clientGridId + sArrChk[i]
                        HidRefCnt = HidRefCnt.replace("chkSelectConf", "hidRefCount")
                        var HidRef = document.getElementById(HidRefCnt)
                        if (HidRef.value > 0)
                            HidRef.value = parseInt(HidRef.value) - 1
                        if (HidRef.value == 0)
                            chkbox.disabled = false
                    } 
                } 
            }
        }
       // function CheckAllCheck(oDocument, _clientGridId, args, chkSelectConf, abPaging) {  }
        function ConfirmAction(iPageCount, sActionName) {
            var bResult = true
            if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientGridId, 'chkSelectConf', sActionName, 'false', iPageCount, 'true')) {
                bResult = true
            }
            else {
                bResult = false
            }
            return bResult
        }
        function getCalDateStr(sId) {
            var dt = document.getElementById(sId).value
            var sInputDate
            if (window.navigator.appName == "Microsoft Internet Explorer") {
                sInputDate = new Date(dt.replace(/-/g, ' '))
            }
            else {
                sInputDate = new Date(dt.replace(/-/g, '/'))
            }
            return sInputDate
        }
        function checkEndDate(oSrc, args) {
            var startdate = document.getElementById(_clientcalStartDate).value
            var sdate = getCalDateStr(_clientcalStartDate)
            var edate = getCalDateStr(_clientcalEndDate)
            if (sdate >= edate) {
                if (document.getElementById(_clientlblErrorMsg) != null) {
                    document.getElementById(_clientlblErrorMsg).innerHTML = "";
                    document.getElementById(_clientlblErrorMsg).style.display = "none";
                }
                args.IsValid = false            
                return true
            }
            args.IsValid = true
            return false
        }
        function SetDisabled(el, val) {
            try {
                el.disabled = val
            }
            catch (E) { } 
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
