<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SmsTemplateUI.aspx.cs" Inherits="SmsTemplateUI"
    MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%">
        <tr>
            <td valign="bottom">
                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true" />
            </td>
            <td>
                <span class="ClsMdtStar">* Mandatory Fields</span>
            </td>
        </tr>
        <tr>
            <td align="center" style="width: 85%">
                <table id="tblSms" runat="server" border="0" cellpadding="1" cellspacing="2" style="width: 85%;
                    margin-left: 19px;">
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height:20px">
                        <td align="left" class="ClsBorderLight"  style="height:20px; width: 128px;" >
                            <%--<asp:Label ID="Label10" runat="server" CssClass="ClsLabel"   Text="Sms Name :" 
                                EnableViewState="False"></asp:Label>--%>
                                <span class="ClsLabel">Sms Name :</span>
                        </td>
                        <td align="left" class="ClsMdtStar" style="height:20px; width:100%" colspan="2" >
                            <asp:DropDownList ID="cmbSmsName" runat="server"  OnSelectedIndexChanged="cmbSmsName_SelectedIndexChanged"
                                AutoPostBack="True" Height="20px" Width="160px" >
                            </asp:DropDownList><span class="ClsMdtStar">*</span>
                            
                        </td>
                        <td align="right" style="height:20px">
                            <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Abbreviation" ForeColor="Brown"
                                Font-Bold="True" Height="16px" Width="92px" EnableViewState="False"></asp:Label>
                               
                        </td>
                        <td align="center" style="height:20px">
                            <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text="Name" ForeColor="Brown"
                                Font-Bold="True" Height="16px" Width="92px" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderLight" style="width: 128px">
                            <%--<asp:Label ID="lblTemplate" runat="server" CssClass="ClsLabel" Text="Template Text :"
                                Height="16px" Width="92px" EnableViewState="False"></asp:Label>--%>
                                <span class="ClsLabel" style="height:16px;width:92px">Sms Name :</span>
                        </td>
                        <td align="left" style="width: 107%; height: 51px;" colspan="2">
                            <asp:TextBox ID="txtTemplate" runat="server" CssClass="ExLrgTxtBox" Style="height: 100px;
                                width: 100%" TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td align="center">
                        
                            <asp:ListBox ID="lstAbbreviation" runat="server" AutoPostBack="true" Height="100%"
                                OnSelectedIndexChanged="lstAbbreviation_SelectedIndexChanged"></asp:ListBox>
                        </td>
                        <td align="center">
                        
                            <asp:ListBox ID="lstAbbreviationName" runat="server" AutoPostBack="true" Height="100%"
                                OnSelectedIndexChanged="lstAbbreviationName_SelectedIndexChanged"></asp:ListBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 128px">
                            <asp:CustomValidator ID="cstTemplate" runat="server" ClientValidationFunction="CheckLength"
                                 ErrorMessage="Template should not be blank."
                                Display="None" EnableClientScript="true" CssClass="ClsMdtStar"></asp:CustomValidator>
                        </td>
                        <td align="left" style="width: 107%">
                            <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                                CausesValidation="True" OnClick="btnSave_Click" UseSubmitBehavior="true" 
                                Width="53px" />
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="ClsBtn" BorderWidth="1px"
                                CausesValidation="False" UseSubmitBehavior="false" 
                                onclick="btnDelete_Click" Width="54px" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                                CausesValidation="False" UseSubmitBehavior="false" 
                                onclick="btnCancel_Click" Width="62px" />&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 128px">
                            &nbsp;
                        </td>
                        <td align="left" style="width: 107%">
                            <asp:HiddenField ID="hidMode" runat="server" />
                            <asp:HiddenField ID="hidTemplateId" runat="server" />
                            <asp:HiddenField ID="hidTemplateText" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">
        _clientcstTemplate = "<%=this.cstTemplate.ClientID%>"
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _sTemplateText = "<%=this.txtTemplate.ClientID %>"
        function CheckLength(oSrc, args) {
            var sTemplateText = document.getElementById(_sTemplateText).value
            var sTemplateText = sTemplateText.trim()
            document.getElementById(_clientcstTemplate).errormessage = ""
            if (sTemplateText.length == 0) {
                document.getElementById(_clientcstTemplate).errormessage = "Template should not be blank."
                args.IsValid = false
                return true
            }
            if (sTemplateText.length > 180) {
                document.getElementById(_clientcstTemplate).errormessage = "Template should be of 180 characters."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        function ResetUpdateLbl() {
            if (document.getElementById(_clientlblUpdateSucess) != null) {
                document.getElementById(_clientlblUpdateSucess).style.display = "none"
            } 
        }
    </script>
</asp:Content>
