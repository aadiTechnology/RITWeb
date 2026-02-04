<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="ExamTypePopup.aspx.cs" Inherits="ExamTypePopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="100%" align="center">
            <tr>
                <td align="left" rowspan="1">
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td style="height: 20px">
                                <asp:Label ID="lblAddExamType" runat="server" class="MainTitleHead" Text="Add Exam Type"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right" style="color: #ff3333" valign="top">
                    <span class="ClsMdtStar">*</span>
                    <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:ValidationSummary ID="valSumErrorMsg" HeaderText="<%$ Resources:LocalizedResources, PleaseFixFollowingError%>"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <tr>
                        <td align="center" id="tdMessage" runat="server" colspan="2">
                            <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                        </td>
                    </tr>
                    <table width="80%" align="center">
                        <tr>
                            <td>
                                <table align="center" cellpadding="1" cellspacing="2">
                                    <tr>
                                        <td valign="top" class="ClsBorderlight">
                                            <asp:Label ID="lblExamType" runat="server" class="ClsLabel" Style="height: 16px"
                                                Text="Exam Type Name"></asp:Label>
                                            <span class="colonPadding">:</span>
                                        </td>
                                        <td valign="top" align="left">
                                            <asp:TextBox ID="txtExamName" runat="server" CssClass="LrgTxtBox" MaxLength="50"
                                                TabIndex="1" Width="300px"></asp:TextBox>&nbsp; <span class="ClsMdtStar">*</span>&nbsp;
                                            <asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtExamName"
                                                ErrorMessage="Exam name should not be blank." SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateGroupName"
                                                SetFocusOnError="True" Display="None" ErrorMessage="Exam Type Name should not be duplicate."></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="ClsBorderlight">
                                            <asp:Label ID="LblSortOrder" runat="server" class="ClsLabel" Style="height: 16px"
                                                Text="Sort Order"></asp:Label>
                                            <span class="colonPadding">:</span>
                                        </td>
                                        <td valign="top" align="left">
                                            <asp:TextBox ID="TxtSortOrder" runat="server" CssClass="LrgTxtBox" MaxLength="3"
                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                            ondrop="event.returnValue=false;"
                                                TabIndex="2" Width="300px"></asp:TextBox>&nbsp; <span class="ClsMdtStar">*</span>&nbsp;




                                            <asp:RequiredFieldValidator ID="reqsort" runat="server" ControlToValidate="TxtSortOrder"
                                                ErrorMessage="Sort Order should not be blank." SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="ValidateSortOrder"
                                                SetFocusOnError="True" Display="None" ErrorMessage="Sort order should not be duplicate."></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="ClsBorderlight">
                                            <asp:Label ID="lblConsiderExamStatus" runat="server" class="ClsLabel" Style="height: 16px"
                                                Text="Consider Exam Status"></asp:Label>
                                            <span class="colonPadding">:</span>
                                        </td>
                                        <td valign="top" align="left">
                                            <asp:CheckBox ID="CheckBox1" runat="server" TabIndex="2" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" TabIndex="3"
                                                disable-page="true" OnClick="btnSave_Click" />
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" TabIndex="4"
                                                CausesValidation="False" UseSubmitBehavior="false" OnClick="btnCancel_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <tr>
                                    <asp:ListView ID="lstvwExamTypes" runat="server" DataKeyNames="TestTypeId,ConsiderExamStatus"
                                        OnItemCommand="lstvwExamTypes_ItemCommand" OnItemDataBound="lstvwExamTypes_ItemDataBound">
                                        <LayoutTemplate>

                                            <table cellpadding="0" cellspacing="0" width="500px">
                                                <tr>
                                                    <td style="height: 40px" id="trLbl" runat="server" align="left">
                                                        <span class="ClsLblLgnd">Exam Types :</span>
                                                    </td>
                                                </tr>
                                            </table>
                                              <div style="height: 475px; width: 500px; overflow: scroll;">
                                            <table cellpadding="0" cellspacing="1" class="GridBorder" width="100%" style="color: #333333">
                                                <tr align="center" id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="left" class="paddingL" style="width: 175px; font-size: 9pt;">
                                                        Exam Type
                                                    </th>
                                                    <th align="center" id="thChkSelectAll" runat="server" style="width: 150px; font-size: 9pt;">
                                                        <asp:Label ID="lblExamType" runat="server" Text="Consider Exam Status"></asp:Label>
                                                    </th>
                                                    <th align="center" id="th1" runat="server" style="width: 70px; font-size: 9pt;">
                                                        <asp:Label ID="lblsort" runat="server" Text="Sort Order"></asp:Label>
                                                    </th>
                                                    <th align="center" width="50px">
                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:LocalizedResources, Edit%>"></asp:Label>
                                                    </th>
                                                    <th align="center" width="50px">
                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:LocalizedResources, Delete%>"></asp:Label>
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                            </div>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="trGridRow" runat="server" class="ClsGridRow">
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="lblExamType" runat="server" Text='<%# Eval("TestTypeName") %>' />
                                                    <asp:HiddenField ID="hidTestId" runat="server" Value='<%#Eval("TestTypeId") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="ImgBtn1" runat="server" CausesValidation="false"
                                                         ImageUrl="../images/IconGrid_AssignTrue.gif" />
                                                </td>
                                                <td align="center" class="paddingL">
                                                    <asp:Label ID="lblsort" runat="server" Text='<%# Eval("SortOrder") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                        ToolTip="<%$ Resources:LocalizedResources, Edit%>" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                        ToolTip="<%$ Resources:LocalizedResources, Delete%>" ImageUrl="../images/IconGrid_Delete.gif" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </tr>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:HiddenField ID="HidTestTypeId" runat="server" />
                                <asp:Button ID="btnClose" Text="Close" CssClass="ClsBtn" runat="server" CausesValidation="false" 
                                    onclick="btnClose_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <script type="text/javascript" language="javascript">
            var _clientlstvwExamTypes = "<%=this.lstvwExamTypes.ClientID %>";

            function ValidateGroupName(oSrc, args) {
             var isFound = false;
                var rowNumber = 0;

                var selectedGroupId = document.getElementById("<%=this.HidTestTypeId.ClientID %>").value;
                var newName = document.getElementById("<%=this.txtExamName.ClientID %>").value.trim();

                var Name = document.getElementById(_clientlstvwExamTypes + "_ctrl" + rowNumber + "_lblExamType")

                while (Name != null) {

                    var GroupId = document.getElementById(_clientlstvwExamTypes + "_ctrl" + rowNumber + "_hidTestId").value;
                    if (selectedGroupId != GroupId && Name.innerHTML.toLowerCase() == newName.toLowerCase()) {
                        isFound = true;
                        break;
                    }

                    rowNumber++;
                    Name = document.getElementById(_clientlstvwExamTypes + "_ctrl" + rowNumber + "_lblExamType")
                }

                args.IsValid = !isFound;
                return isFound;
            }

            function ValidateSortOrder(oSrc, args) {
                var isFound = false;
                var rowNumber = 0;

                var selectedGroupId = document.getElementById("<%=this.HidTestTypeId.ClientID %>").value;
                var newName = document.getElementById("<%=this.TxtSortOrder.ClientID %>").value.trim();

                var Name = document.getElementById(_clientlstvwExamTypes + "_ctrl" + rowNumber + "_lblsort")

                while (Name != null) {

                    var GroupId = document.getElementById(_clientlstvwExamTypes + "_ctrl" + rowNumber + "_hidTestId").value;
                    if (selectedGroupId != GroupId && Name.innerHTML.toLowerCase() == newName.toLowerCase()) {
                        isFound = true;
                        break;
                    }

                    rowNumber++;
                    Name = document.getElementById(_clientlstvwExamTypes + "_ctrl" + rowNumber + "_lblsort")
                }

                args.IsValid = !isFound;
                return isFound;
            }

            function ConfirmDelete() {
                return confirm('Are you sure you want to delete this record?');
            }
        </script>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
