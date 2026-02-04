<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrePrimaryProgressReportConfig.aspx.cs"
    MasterPageFile="../MasterPages/PopupMasterSml.master" Inherits="PrePrimaryProgressReportConfig" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <div class="MainBodyDiv">
     <asp:UpdatePanel ID="upnlPrePrimaryConfig" runat="server" UpdateMode="Conditional"> 
     <ContentTemplate>
        <table width="100%">
            <tr>
                <td align="left" colspan="2" rowspan="1">
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td style="height: 20px">
                                <asp:Label ID="lblHeader" runat="server" CssClass="MainTitleHead" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2" style="color: #ff3333" valign="top">
                    <asp:Label ID="lblmandatory" runat="server" CssClass="ClsMdtStar" Text="* Mandatory Fields"
                        ForeColor="Red" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:ValidationSummary ID="valSumErrorMsg" HeaderText="Please fix following error(s)"
                        runat="server" />
                    <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg"  EnableViewState="false"></asp:Label>
                </td>
            </tr>
			<tr><td align="center"><asp:Label ID="lblUpdate" runat="server" CssClass="LblNormalImg" Font-Bold="true"
                                                            Font-Size="Small" ForeColor="Blue" Visible="true"></asp:Label></td></tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnlFields" runat="server" Width="100%">
                        <table>
                            <tr>
                                <td>
                                    <table cellpadding="1" cellspacing="2" border="0">
                                        <tr>
                                            <td valign="top" class="ClsBorderlight">
                                                <asp:Label ID="lblHolidayStartDate" runat="server" CssClass="ClsLabel" Text="Heading text :"
                                                    EnableViewState="False"></asp:Label>
                                            </td>
                                            <td valign="top" align="left">
                                                <asp:TextBox ID="txtNameofHoliday" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                    TabIndex="1" Width="367px"></asp:TextBox>&nbsp;
                                                <span class="ClsMdtStar">*</span>&nbsp;
                                                <asp:RequiredFieldValidator ID="reqdHeaderText" runat="server" ControlToValidate="txtNameofHoliday"
                                                    ErrorMessage="Header Text should not be blank." SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr id="trApplicableExam" runat="server">
                                            <td valign="top" class="ClsBorderlight">
                                                <span class="ClsLabel">Applicable for exam :</span>
                                            </td>
                                            <td align="left">
                                                <asp:CheckBoxList ID="chklstExam" runat="server" RepeatColumns="2" class="ClsLabel">
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                        <tr id="trCheckBox" runat="server">
                                            <td valign="top" class="ClsBorderlight">
                                                <span class="ClsLabel"></span>
                                            </td>
                                            <td align="left">
                                                <asp:CheckBox ID="chkIsDescription" runat="server" Text="Is Comment applicable?" class="ClsLabel" TabIndex="2"/>
                                            </td>
                                        </tr>
                                        <tr id="trTest" runat="server">
                                            <td valign="top" class="ClsBorderlight">
                                                <span class="ClsLabel">Applicable for exam :</span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:UpdatePanel runat="server" ID="updateCmb">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="cmbTests" runat="server" CssClass="MidCombo" Enabled="false"
                                                            TabIndex="3">
                                                        </asp:DropDownList>
                                                        &nbsp;
                                                        <asp:Label ID="lblmdtCmb" runat="server" CssClass="ClsMdtStar" ForeColor="Red" Text="*"
                                                            EnableViewState="false"></asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <asp:CustomValidator ID="cstValcmbTests" runat="server" ClientValidationFunction="CstValidation"
                                                    CssClass="ClsLabel" Display="None" ErrorMessage="Applicable for exam should be selected."
                                                    ControlToValidate="cmbTests"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="center" colspan="2">
                                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" TabIndex="4"
                                                    OnClick="btnSave_Click" disable-page="true" OnClientClick="Clearlable()" />
                                                <asp:Button ID="btnCancel" runat="server" Text="Close" CssClass="ClsBtn" TabIndex="5" 
                                                    CausesValidation="False" UseSubmitBehavior="false"  />
                                                   
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:BulletedList ID="BulletedList1" runat="server">
                    </asp:BulletedList>
                </td>
            </tr>
        </table>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        _clientErrLabel = "<%=this.lblErrorMsg.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        _clientcmbTests = "<%=this.cmbTests.ClientID %>"
        _clientchkIsDescription = "<%=this.chkIsDescription.ClientID %>"
        _clientHidUrl = "<%=this.hidUrl.ClientID %>"
         function CstValidation(oSrc, args) {
            if (document.getElementById(_clientchkIsDescription).checked) {
                if (document.getElementById(_clientcmbTests).selectedIndex == 0) {
                    args.IsValid = false
                    return true
                } 
            }
            args.IsValid = true
            return false
        }
        function Enable(sender, sArgs, lblMdtCmb) {
            document.getElementById(sArgs).selectedIndex = 0
            if (sender.checked) {
                document.getElementById(sArgs).disabled = false
                document.getElementById(lblMdtCmb).style.visibility = "visible"
            }
            else {
                document.getElementById(sArgs).disabled = true
                document.getElementById(lblMdtCmb).style.visibility = "hidden"
            } 
        }
        function closewindow() {
            document.getElementById(_clientbtnSave).disabled = true
            document.getElementById(_clientbtnCancel).disabled = true
            window.close()
           }
        function ClearErrorLabel() {
            var isPageValid = true
            if (typeof (Page_ClientValidate) == 'function') {
                isPageValid = Page_ClientValidate()
            }
            
            document.getElementById(_clientErrLabel).innerText = ""
        }
        function ResetFields_btnReset() {
            var bResult = window.confirm("Are you sure to Reset all Field?")
            if (bResult) {
                document.aspnetForm.reset()
            }
            return false
        }
        function fnover(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "maroon"
            objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)"
        }
        function fnout(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "#a3c07b"
            objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)"
        }
        function CloseWindow() {
           window.opener.location.href = document.getElementById(_clientHidUrl).value;
           if (window.opener.progressWindow) {
                window.opener.progressWindow.close()
            }
            window.close();
            }

       function Clearlable() {
           	if ($get("<%=this.lblUpdate.ClientID %>") != null)
           		$get("<%=this.lblUpdate.ClientID %>").innerHTML = "";
           	if ($get("<%=this.lblErrorMsg.ClientID %>") != null)
           	    $get("<%=this.lblErrorMsg.ClientID %>").innerHTML = "";
        }
        window.onunload = refreshParent;
        function refreshParent() {
            window.opener.location.href = document.getElementById(_clientHidUrl).value;
       }

		  
			  
    </script>

    <asp:HiddenField ID="hidHeaderId" runat="server" Value="0" />
    <asp:HiddenField ID="hidStandardId" runat="server" Value="0" />
    <asp:HiddenField ID="HidParentHeadingId" runat="server" Value="0" />
    <asp:HiddenField ID="hidUrl" runat="server" Value="0" />
    <asp:HiddenField ID="hidActionFlag" runat="server" />
    <asp:HiddenField ID="hidIsConfig" runat="server" Value="0"></asp:HiddenField>
</asp:Content>
