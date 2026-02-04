<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentHealthDetailsUI.aspx.cs" Inherits="StudentHealthDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table border="0" width="100%" cellpadding="0">
        <tr style="margin: 0px auto;">
            <td>
                <table id="tblStudentHealthDetails" runat="server" style="width: 100%;">
                    <tr>
                        <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px;">
                        </td>
                    </tr>
                    <tr>
                        <td id="tdMessage" runat="server" align="center">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                                        ShowSummary="true" />
                                    <asp:CustomValidator ID="cstHealthAnswer" runat="server" ErrorMessage="" Display="None"
                                        ClientValidationFunction="ValidateStudentHealthDetails"></asp:CustomValidator>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr align="center" style="text-align: center; margin: 0px auto;">
                        <td align="center" style="text-align: center; margin: 0px auto;">
                            <table width="50%" align="center" style="text-align: center;">
                                <tr>
                                    <td class="clsBorderLight" style="height: 30px;">
                                        <span class="clsLabel">Enrolment No. :</span>
                                    </td>
                                    <td class="ClsHilightBGB" style="width: 20%">
                                        <asp:Label ID="lblEnrolmentNo" runat="server" CssClass="clsLabel" ViewStateMode="Enabled"></asp:Label>
                                    </td>
                                    <td class="clsBorderLight" style="height: 30px;">
                                        <span class="clsLabel">Name :</span>
                                    </td>
                                    <td class="ClsHilightBGB" style="width: 50%">
                                        <asp:Label ID="lblName" runat="server" CssClass="clsLabel" ViewStateMode="Enabled"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="clsBorderLight" style="height: 30px;">
                                        <span class="clsLabel">Roll No. :</span>
                                    </td>
                                    <td class="ClsHilightBGB">
                                        <asp:Label ID="lblRollNo" runat="server" CssClass="clsLabel" ViewStateMode="Enabled"></asp:Label>
                                    </td>
                                    <td class="clsBorderLight" style="height: 30px;">
                                        <span class="clsLabel">Class :</span>
                                    </td>
                                    <td class="ClsHilightBGB">
                                        <asp:Label ID="lblClass" runat="server" CssClass="clsLabel" ViewStateMode="Enabled"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px;">
                        </td>
                    </tr>
                    <tr style="text-align: center; margin: 0px auto;">
                        <td style="text-align: center; margin: 0px auto;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:ListView ID="lstvwStudentHealthDetails" runat="server" DataKeyNames="StudentId,EnrolmentNo,ComponentId,ParameterId"
                                        OnDataBound="lstvwStudentHealthDetails_DataBound" 
                                        onitemdatabound="lstvwStudentHealthDetails_ItemDataBound">
                                        <LayoutTemplate>
                                            <table width="70%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder"
                                                align="center">
                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                    <th align="left" width="180px" class="clsLabelgrd">
                                                        <span><b>Components</b></span>
                                                    </th>
                                                    <th align="left" class="clsLabelgrd">
                                                        <span><b>Parameters</b></span>
                                                    </th>
                                                    <th align="center" width="350px" class="clsLabelgrd">
                                                        <span><b>Remark</b></span>
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="trGridRow" runat="server" class="ClsGridRow">
                                                <td align="left">
                                                    <asp:Label ID="lblComponent" runat="server" CssClass="ClsLabel" Text='<%#Eval("Component") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblParameters" runat="server" CssClass="ClsLabel" Style="float: inherit; padding-left: 0px;"
                                                        Text='<%#Eval("Parameter") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtAnswer" runat="server" Text='<%#Eval("Answer") %>' CssClass="MidCombo"
                                                        Width="350px" Height="50px" MaxLength="200" TextMode="MultiLine">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        No record found.
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px;">
                        </td>
                    </tr>
                    <tr align="center" style="text-align: center; margin: 0px auto;">
                        <td align="center" style="text-align: center; margin: 0px auto;">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false"
                                        ViewStateMode="Enabled" />
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click"
                                        ViewStateMode="Enabled" />
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="ClsBtn" CausesValidation="false"
                                        ViewStateMode="Enabled" OnClick="btnSubmit_Click" />
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="ClsBtn" CausesValidation="false"
                                        ViewStateMode="Enabled" OnClick="btnClear_Click" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="hidStudentId" runat="server" Value="0" />
                <asp:HiddenField ID="hidStandardId" runat="server" Value="0" />
                <asp:HiddenField ID="hidDivisionId" runat="server" Value="0" />
                <asp:HiddenField ID="hidQueryString" runat="server" Value="0" />                
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        _clienthidQueryString = "<%=this.hidQueryString.ClientID %>";

        function OpenStudentListScreen() {
            var sEncryptedString = document.getElementById(_clienthidQueryString).value;
            window.open('HealthDetailsStudentListUI.aspx?' + sEncryptedString, '_self');
            return false;
        }

        function ValidateStudentHealthDetails(oSrc, args) {
            var found = false
            $('[id$=txtAnswer]').each(function () {
                if ($(this).val().trim() != "") {
                    found = true;
                }
            });

            if (!found) {
                oSrc.errormessage = "Please enter remark for atleast one parameter.";
                args.IsValid = false;
                return true;
            }
        }

        function OnGridKeyUp(obj, e) {
            UpDownKeyPress(obj.id, e);
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
