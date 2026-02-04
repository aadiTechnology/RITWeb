<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="HealthDetailsStudentListUI.aspx.cs" Inherits="HealthDetailsStudentListUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table border="0" width="100%" cellpadding="0">
        <tr style="margin: 0px auto;">
            <td>
                <table id="tblLearningOutcome" runat="server" style="width: 100%;">
                    <tr>
                        <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="padding-right: 30px" valign="bottom">
                            <span class="ClsMdtStar">*</span>
                            <asp:Label ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False"
                                Text="Mandatory Fields"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                                ShowSummary="true" />
                            <asp:RequiredFieldValidator ID="reqSelectStanderd" runat="server" ErrorMessage="Standard should be selected."
                                ControlToValidate="cmbStandard" InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="reqSelectDivision" runat="server" ErrorMessage="Division should be selected."
                                ControlToValidate="cmbDivision" InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table align="center">
                                <tr>
                                    <td>
                                        <table style="margin: 0px auto;" width="100%">
                                            <tr>
                                                <td id="tdClassTeacherLable" runat="server" align="right" class="ClsBorderlight"
                                                    style="width: 150px;">
                                                    <asp:Label ID="lblTeacher" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                        Font-Bold="True" Text="<%$ Resources:LocalizedResources, Standard %>"></asp:Label>
                                                    <span class="ClsLblLgnd colonPadding">:</span>
                                                </td>
                                                <td id="tdcmbTeachers" runat="server" align="left">
                                                    <asp:DropDownList ID="cmbStandard" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">* </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="tdTermLable" runat="server" align="left" class="ClsBorderlight">
                                                    <asp:Label ID="lblTerm" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                                        Text="<%$ Resources:LocalizedResources, Division %>"></asp:Label>
                                                    <span class="ClsLblLgnd colonPadding">:</span>
                                                </td>
                                                <td id="tdCmbTerm" runat="server" align="left">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="cmbDivision" runat="server" CssClass="MidCombo" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                            <span class="ClsMdtStar">* </span>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 10px;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center" style="text-align: center;">
                                                    <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="ClsBtn" OnClick="btnShow_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr align="center" style="text-align: center; margin: 0px auto;" id="trLegend" runat="server">
                        <td align="center" style="text-align: center; margin: 0px auto;">
                            <table align="center" style="text-align: center;" width="40%">
                                <tr>
                                    <td align="left" colspan="1" style="height: 24px">
                                        <span class="ClsLblLgnd">
                                            <asp:Label ID="lbl" runat="server" Text="<%$ Resources:LocalizedResources, Legend %>"></asp:Label>
                                        </span>&nbsp;
                                        <asp:Label ID="Label1" runat="server" Height="20px" BorderStyle="Solid" BorderWidth="1px"
                                            BackColor="Red" ReadOnly="True" Width="20px"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>&nbsp;
                                        <span class="ClsTextNormal" style="font-weight: bold">
                                            <asp:Label ID="Label3" runat="server" Text="Left Student(s)"></asp:Label></span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr align="center" style="text-align: center; margin: 0px auto;">
                        <td style="width: 100%; height: auto; text-align: center; margin: 0px auto;" align="center">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:ListView ID="lstvwStudentDetails" runat="server" DataKeyNames="Status,StudentId,IsSubmited,IsLeft"
                                        OnItemDataBound="lstvwStudentDetails_ItemDataBound">
                                        <LayoutTemplate>
                                            <table width="40%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder"
                                                align="center">
                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                    <th align="center" width="80px" class="clsLabelgrd">
                                                        <span><b>Roll No.</b></span>
                                                    </th>
                                                    <th align="left" class="clsLabelgrd">
                                                        <span><b>Student Name</b></span>
                                                    </th>
                                                    <th width="80px" align="center" class="clsLabelgrd">
                                                        <asp:Label ID="lblEdit" runat="server" Text="Action"> </asp:Label>
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                <td align="center" style="text-align: center; font-size: 9pt; font-family: Arial;">
                                                    <asp:Label ID="lblRollNo" runat="server" Text='<%#Eval("RollNo") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("StudentName") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:HyperLink ID="hyplnkEdit" runat="server" NavigateUrl="#" Text="Add"></asp:HyperLink>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                <td align="center" style="text-align: center; font-size: 9pt; font-family: Arial;">
                                                    <asp:Label ID="lblRollNo" runat="server" Text='<%#Eval("RollNo") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("StudentName") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:HyperLink ID="hyplnkEdit" runat="server" NavigateUrl="#" Text="Add"></asp:HyperLink>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <table width="50%" align="center" style="text-align: center; margin: 0px auto;">
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
                                    <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        _clienthidQueryString = "<%=this.hidQueryString.ClientID %>";

        function OpenNextPage() {
            var sEncryptedString = document.getElementById(_clienthidQueryString).value;
            window.open('StudentHealthDetailsUI.aspx?' + sEncryptedString, '_self');
            return false;
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
