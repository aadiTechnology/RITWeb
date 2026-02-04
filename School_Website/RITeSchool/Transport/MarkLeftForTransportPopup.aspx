<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="MarkLeftForTransportPopup.aspx.cs" Inherits="MarkLeftForTransportPopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
        <tr align="center">
            <td align="center">
                <table width="100%" align="center">
                    <tr>
                        <td class="ClsGrayMainTitle" style="height: 20px;" align="left">
                            <asp:Label ID="lblAddAcademicYear" runat="server" CssClass="MainTitleHead" Font-Bold="True"
                                Text="Mark As a Left for Transport" EnableViewState="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left">
                 <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="lblNormal" />
            </td>
        </tr>
        <tr>
            <td style="height:20px;"></td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td class="ClsBorderlight" style="white-space: nowrap; width:150px;">
                            <span id="Span3" class="LblNormal">User Name :</span>
                        </td>
                        <td align="left" style="text-align:left;" class="ClsHilightBGB">
                            <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="ClsBorderlight" style="white-space: nowrap; width:150px;">
                            <span id="Span2" class="LblNormal">Left Date :</span>
                        </td>
                        <td align="left" style="text-align:left;">
                            <asp:TextBox ID="txtTransportLeftDate" CssClass="SmlTxtBox" runat="server"></asp:TextBox>
                            <rjs:PopCalendar ID="CalDobPopup" runat="server" Control="txtTransportLeftDate" Format="dd MMM yyyy"
                                ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth."
                                To-Today="true" />
                            <span class="ClsMdtStar">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="ClsBorderlight" style="white-space: nowrap">
                            <span id="Span1" class="LblNormal">Left Reason :</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLeftReason" runat="server" CssClass="ExLrgTxtBox" MaxLength="500"
                                Height="70px" TextMode="MultiLine" Width="280px"></asp:TextBox>
                            <span class="ClsMdtStar">*</span>
                            <asp:CustomValidator ID="cstLeftReason" runat="server" ErrorMessage="" ClientValidationFunction="ValidateLeftReason"
                                Display="None" ValidationGroup="Transport"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr style="height: 60px;">
                        <td align="center" colspan="2">
                            <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" Style="margin-left: 5px;
                                cursor: pointer;" Text="Save" OnClick="btnSave_Click" ValidationGroup="Transport" />
                            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" CausesValidation="false" />
                        </td>
                    </tr>
                    <asp:HiddenField ID="hidUserId" runat="server" Value="0" />   
                    <asp:HiddenField ID="hidStandardId" runat="server" Value="0" />   
                    <asp:HiddenField ID="hidDivisionId" runat="server" Value="0" />   
                    <asp:HiddenField ID="hidSearchText" runat="server" Value="0" />   
                    <asp:HiddenField ID="hidUserRoleId" runat="server" Value="0" />   
                    <asp:HiddenField ID="hidIncludeNotAssociated" runat="server" Value="0" /> 
                    <asp:HiddenField ID="hidRouteId" runat="server" Value="0" /> 
                    <asp:HiddenField ID="hidStopId" runat="server" Value="0" /> 
                    <asp:HiddenField ID="hidShiftId" runat="server" Value="0" /> 
                </table>
            </td>
        </tr>
    </table>

<script language="javascript" type="text/javascript">

    function ValidateLeftReason(oSrc, args) {
            _clienttxtLeftReason = "<%=this.txtLeftReason.ClientID %>"
            _clienttxtTransportLeftDate = "<%=this.txtTransportLeftDate.ClientID %>"
            var LeftReason = document.getElementById(_clienttxtLeftReason).value;
            var LeftDate = document.getElementById(_clienttxtTransportLeftDate).value;
            if (LeftReason == "" && LeftDate == "") {
                alert('Transport Left Reason and Left Date should not be blank.')
                args.IsValid = false
                return true
            }
            else if (LeftReason == "") {
                alert('Transport Left Reason should not be blank.')
                args.IsValid = false
                return true
            }
            else if (LeftDate == "") {
                alert('Transport Left Date should not be blank.')
                args.IsValid = false
                return true   
            }
            else if (LeftReason.length > 100) {
                alert('Left Reason lenght should be less than 100 characters.')
                args.IsValid = false
                return true
            }            
            args.IsValid = true
            return false
        }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
