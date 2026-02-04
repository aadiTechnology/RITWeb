<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentsMonthlyStatusDetailsUI.aspx.cs"
    Inherits="StudentsMonthlyStatusDetailsUI" MasterPageFile="../MasterPages/MasterPage.master"
    ViewStateMode="Enabled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <span class="ClsMdtStar">*</span>
                                <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="valSum" runat="server" HeaderText="Please correct following errors."
                                            ValidationGroup="SHOW" />
                                        <asp:RequiredFieldValidator ID="reqcmbCategory" runat="server" Display="None" ControlToValidate="cmbCategory"
                                            InitialValue="0" ErrorMessage="Category should be selected." ValidationGroup="SHOW"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="reqcmbMonth" runat="server" Display="None" ControlToValidate="cmbMonth"
                                            InitialValue="0" ErrorMessage="Month should be selected." ValidationGroup="SHOW"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="reqcmbStandard" runat="server" Display="None" ControlToValidate="cmbStandard"
                                            InitialValue="0" ErrorMessage="Standard should be selected." ValidationGroup="SHOW"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="reqcmbDivision" runat="server" Display="None" ControlToValidate="cmbDivision"
                                            InitialValue="0" ErrorMessage="Division should be selected." ValidationGroup="SHOW"></asp:RequiredFieldValidator>
                                        <asp:ValidationSummary ID="valSaveSummary" runat="server" HeaderText="Please correct the following errors."
                                            ValidationGroup="SAVE" />
                                        <asp:CustomValidator ID="cvAtLeastOneRemark" runat="server" ClientValidationFunction="ValidateAtLeastOneRemark"
                                            ErrorMessage="At least one remark is required." Display="None" ValidationGroup="SAVE" />
                                        <asp:CustomValidator ID="cvRemarkLength" runat="server" ClientValidationFunction="ValidateRemarkLength"
                                            ErrorMessage="Remark length must not exceed 500 characters." Display="None" ValidationGroup="SAVE" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSHOW" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td id="tdMessage" runat="server" align="center" style="height: 10px">
                                <asp:UpdatePanel ID="upnlSuccessMsg" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblmessage" runat="server" Text="" EnableViewState="false" ForeColor="Blue"
                                            Font-Bold="True" CssClass="LblNormal"></asp:Label><br />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSHOW" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>                       
                        <tr>
                            <td align="center" style="height: 154px">
                                <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td align="center" class="ClsBorderlight" style="width: 100px;">
                                                    <span class="ClsLabel">Category :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbCategory" runat="server" CssClass="LrgCombo" ViewStateMode="Enabled">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="ClsBorderlight" style="width: 100px;">
                                                    <span class="ClsLabel">Month :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbMonth" CssClass="LrgCombo" runat="server" EnableViewState="true">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="ClsBorderlight" style="width: 100px;">
                                                    <span class="ClsLabel">Standard :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbStandard" runat="server" CssClass="LrgCombo" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged" ViewStateMode="Enabled">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="ClsBorderlight" style="width: 100px;">
                                                    <span class="ClsLabel">Division :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="cmbDivision" CssClass="LrgCombo" runat="server" EnableViewState="true">
                                                            </asp:DropDownList>
                                                            <span class="ClsMdtStar">*</span>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSHOW" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td align="center">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnShow" CssClass="ClsBtn" runat="server" Text="Show" OnClick="btnShow_Click"
                                                        ValidationGroup="SHOW" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSHOW" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                    <ContentTemplate>
                                        <table width="80%">
                                            <tr>
                                                <td align="center">
                                                    <asp:ListView ID="lstvwStudentMonthlyStatus" runat="server" 
                                                        DataKeyNames="YearWise_Student_Id" 
                                                        onitemdatabound="lstvwStudentMonthlyStatus_ItemDataBound">
                                                        <LayoutTemplate>
                                                            <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                                    <th width="125px" align="left" class="clsLabelgrd">
                                                                        <asp:Label ID="lblEnrollmentno" runat="server" Text="Registration No."></asp:Label>
                                                                    </th>
                                                                    <th width="60px" align="Center" class="clsLabelgrd">
                                                                        <asp:Label ID="lblRollNo" runat="server" Text="<%$ Resources:LocalizedResources, RollNo%>"></asp:Label>
                                                                    </th>
                                                                    <th align="left" class="clsLabelgrd" width="250px">
                                                                        <asp:Label ID="lblStudentName" runat="server" Text="<%$ Resources:LocalizedResources, StudentName%>"></asp:Label>
                                                                    </th>
                                                                    <th align="left" class="clsLabelgrd">
                                                                        <asp:Label ID="lblRemark" runat="server" Textbox="Multiline" Text="Remark"></asp:Label>
                                                                    </th>
                                                                </tr>
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                <td align="Center">
                                                                    <asp:Label ID="lblEnrollmentno" runat="server" CssClass="ClsLabel" Text='<%#Eval("EnrollmentNumber") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblRollNo" runat="server" CssClass="centerText" Text='<%#Eval("RollNumber") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLabel" Text='<%#Eval("StudentName") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtRemark" runat="server" TextMode="Multiline" Rows="2" Style="width: 95%;
                                                                        box-sizing: border-box;" CssClass="ClsLabel" Text='<%#Eval("Remark") %>'></asp:TextBox>
                                                                     <asp:Label ID="lblLength" runat="server" CssClass="clsLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                <td align="center">
                                                                    <asp:Label ID="lblEnrollmentno" runat="server" CssClass="ClsLabel" Text='<%#Eval("EnrollmentNumber") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblRollNo" runat="server" CssClass="centerText" Text='<%#Eval("RollNumber") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLabel" Text='<%#Eval("StudentName") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtRemark" runat="server" TextMode="Multiline" Rows="2" CssClass="ClsLabel"
                                                                        Style="width: 95%; box-sizing: border-box;" Text='<%#Eval("Remark") %>'></asp:TextBox>
                                                                    <asp:Label ID="lblLength" runat="server" CssClass="clsLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                        <EmptyDataTemplate>
                                                            <tr>
                                                                <td class="LblNoRecord" align="center">
                                                                    No record found.
                                                                </td>
                                                            </tr>
                                                        </EmptyDataTemplate>
                                                    </asp:ListView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td align="center">
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnSave" CssClass="ClsBtn" runat="server" Text="Save" OnClick="btnSave_Click"
                                                        Visible="false" ValidationGroup="SAVE" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSHOW" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">


        var _clientlstvwStudentMonthlyStatus = "<%= lstvwStudentMonthlyStatus.ClientID %>";

        function ValidateAtLeastOneRemark(source, args) {
            var hasValue = false;

            $('[id$=_txtRemark]').each(function () {
                if ($.trim($(this).val()) !== '') {
                    hasValue = true;
                    return false; // break loop
                }
            });

            if (!hasValue) {
                source.errormessage = "Value for atleast one remark should be added.";
                args.IsValid = false;
            } else {
                args.IsValid = true;
            }
        }
        function ValidateRemarkLength(source, args) {
            var isValid = true;
            var invalidRollNos = [];

            $('[id$=_txtRemark]').each(function () {
                var remark = $.trim($(this).val());
                if (remark.length > 500) {
                    var rollNo = $(this).closest('tr').find('[id$=_lblRollNo]').text().trim();
                    invalidRollNos.push(rollNo);
                    isValid = false;
                }
            });

            if (!isValid) {
                source.errormessage = "Remark length should not be more than 500 characters for Roll No(s): " + invalidRollNos.join(", ");
                args.IsValid = false;
            } else {
                args.IsValid = true;
            }
        }

        function SetRemarkLength(obj,clId) {            
            $('#' + clId).html('('+(500 - $(obj).val().length-1)+')');
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
