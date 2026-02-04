<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="NewQueryPopup.aspx.cs" Inherits="NewQueryPopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td align="left">
                    <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="height: 20px" class="ClsGrayMainTitle" valign="middle">
                                <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                    <tr>
                                        <td align="left" class="MainTitleHead" style="height: 20px">
                                            <span style="font-weight: bold">New Query</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 10px;">
                <td align="right">
                    <div style="float: right; vertical-align: top;">
                        <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="* Mandatory Fields"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:ValidationSummary ID="valSum" runat="server" />
                    <asp:RequiredFieldValidator ID="reqValTitle" runat="server" ErrorMessage="Query should not be blank."
                        ControlToValidate="txtTitle" Display="None" SetFocusOnError="True" ValidateEmptyText="True"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cstValDescription" runat="server" Display="None" ClientValidationFunction="ValidateDescription"
                        SetFocusOnError="True" ValidateEmptyText="True" CssClass="ClsMdtStar"></asp:CustomValidator>
                    <asp:CustomValidator ID="cstValAttachment" runat="server" Display="None" ClientValidationFunction="ValidateFile"
                        SetFocusOnError="True" ValidateEmptyText="True"></asp:CustomValidator>
                    <asp:CustomValidator ID="cstValCategory" runat="server" Display="None" ClientValidationFunction="ValidateCategory"
                        SetFocusOnError="True" ValidateEmptyText="True"></asp:CustomValidator>                    
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>                        
                        <tr>
                            <td align="left" class="ClsBorderlight " style="background-color: #ffffc4;">
                                <asp:Label ID="Label9" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note :"
                                    CssClass="LblNrmlB"></asp:Label>
                            </td>
                            <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                <asp:Label ID="Label11" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="Please use this facility for only academic related queries."></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 10px">
                            <td colspan="2" align="left">
                                <asp:Label ID="lblMessage" runat="server" Text="" Enabled="false" CssClass="ErrMsg"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="ClsBorderlight" width="150px">
                                <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Date "></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtDate" runat="server" CssClass="MidTxtBox" ReadOnly="True"></asp:TextBox>
                                <span class="ClsMdtStar">* </span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="ClsBorderlight">
                                <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text="Query "></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtTitle" runat="server" CssClass="ExLrgTxtBox" Width="400px" MaxLength="100"></asp:TextBox>
                                <span class="ClsMdtStar">* </span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="ClsBorderlight">
                                <asp:Label ID="Label3" runat="server" CssClass="ClsLabel" Text="Description "></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="ExLrgTxtBox" TextMode="MultiLine"
                                    Width="400px" Height="100px"></asp:TextBox>
                                <span class="ClsMdtStar">* </span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="ClsBorderlight">
                                <asp:Label ID="Label4" runat="server" CssClass="ClsLabel" Text="Attachment "></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td align="left">
                                <asp:FileUpload ID="flAttachment" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="left">
                                <span class="LblSmlGray">(Attachment supports files of types - .BMP, .DOC, .DOCX, .JPG,
                                    .JPEG, .PDF, .XLS, .XLSX upto 1 MB.)</span>
                            </td>
                        </tr>                        
                        <tr id="tr1" runat="server">
                            <td align="left" class="ClsBorderlight">
                                <asp:Label ID="Label8" runat="server" CssClass="ClsLabel" Text="Category "></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td align="left">
                                <asp:CheckBoxList ID="chkCategoryLst" runat="server" RepeatDirection="Horizontal"
                                    CssClass="ClsLabel" RepeatColumns="4">
                                </asp:CheckBoxList>
                                <span class="ClsMdtStar">* </span>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" />
                                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" CausesValidation="false"
                                    OnClientClick="ClosePopup()" />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hidQuestionId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidQuestionDetailsId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidSenderUserId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidAttachedFileName" runat="server" Value="" />
                    <asp:HiddenField ID="hidIsModerator" runat="server" Value="0" />
                    <asp:HiddenField ID="hidIsCommunicationStarted" runat="server" Value="0" />
                    <asp:HiddenField ID="hidUserRoleId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidOwnerUserId" runat="server" Value="0" />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">

        var _clientTxtTitle = "<%=this.txtTitle.ClientID %>"        
        var _clientHidIsModerator = "<%=this.hidIsModerator.ClientID %>"        
        var _clienthidQuestionId = "<%=this.hidQuestionId.ClientID %>"
        var _clienthidIsCommunicationStarted = "<%=this.hidIsCommunicationStarted.ClientID %>"
        var _clienttxtDescription = "<%=this.txtDescription.ClientID %>"
        var _clientchkCategoryLst = "<%=this.chkCategoryLst.ClientID %>"
        
        function ClosePopup() {
            window.close();
        }

        function CloseWindow() {
            window.close();
            window.opener.focus();
            window.opener.ShowMessage($get(_clienthidQuestionId).value);
        }

        function ValidateDescription(oSrc, args) {
            var description = $get(_clienttxtDescription).value
            description = description.trim()

//            if (document.getElementById(_clienthidIsCommunicationStarted).value == 1) {
                if (description == "") {
                    oSrc.errormessage = "Description should not be blank.";
                    args.IsValid = false;
                    return true;
                }
                else if (description.length > 300) {
                    oSrc.errormessage = "Description length should not be greater than 300.";
                    args.IsValid = false;
                    return true;
                }
//            }
//            else {
//                if (description.length > 300) {
//                    oSrc.errormessage = "Description length should not be greater than 300.";
//                    args.IsValid = false;
//                    return true;
//                }
//            }

            args.IsValid = true;
            return false;
        }

        function ValidateFile(oSrc, args) {
            var fl = $get("<%=this.flAttachment.ClientID %>").value;

            if (fl != "") {
                if (!(fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".JPG" ||
                      fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".JPEG" ||
                      fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".BMP" ||
                      fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".DOC" ||
                      fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".DOCX" ||
                      fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".XLS" ||
                      fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".XLSX" ||
                      fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".PDF"
                    )) {
                    oSrc.errormessage = "Please select valid file type.";
                    args.IsValid = false;
                    return true;
                }
            }
            args.IsValid = true;
            return false;
        }

        function ValidateCategory(oSrc, args) {
            var found = false;
            var k = 0;
            var chk = document.getElementById(_clientchkCategoryLst + "_" + k)
            while (chk != null) {
                if (chk.checked) {
                    found = true;
                    break;
                }
                k++;
                chk = document.getElementById(_clientchkCategoryLst + "_" + k)
            }

            if (found) {
                args.IsValid = true;
                return false;
            }

            oSrc.errormessage = "At least one category should be selected.";
            args.IsValid = false;
            return true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
