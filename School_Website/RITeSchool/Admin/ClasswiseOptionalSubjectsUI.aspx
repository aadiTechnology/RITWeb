<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ClasswiseOptionalSubjectsUI.aspx.cs" Inherits="ClasswiseOptionalSubjectsUI"
    Title="Classwise Optional Subject Configuration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="800px">
            <tr>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" width="800px">
                        <tr>
                            <td align="right" colspan="2" style="color: #ff3333" valign="top">
                                <span class="ClsMdtStar">
                                    * <asp:Label ID="lblMandatoryField" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ValidationGroup="Save"
                                    CssClass="ClsLabel" ShowSummary="true"  />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 800px;">
                                <asp:Label ID="lblErrorMessage" runat="server" CssClass="ClsMdtStar" EnableViewState="False"
                                    ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trListview">
                <td align="center">
                    <table id="tblOptionalSubTable" border="0" cellpadding="1" cellspacing="1" runat="server"
                        width="600px">
                        <tr align="center">
                            <td align="center" class="ClsTextNormal" colspan="2">
                                <asp:Label ID="lblUpdateSucess" runat="server" EnableViewState="False" Font-Bold="True"
                                    ForeColor="Blue" Height="20px"></asp:Label>
                            </td>
                        </tr>
                        <tr align="center">
                            <td align="center">
                                <table>
                                    <tr align="center">
                                        <td align="center" class="ClsBorderlight" style="padding-left: 10px; padding-right: 5px;"
                                            colspan="1">
                                            <span class="ClsLabel">
                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, Class %>"></asp:Label>
                                                <span id="Span3" class="colonPadding">:</span> </span>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="cmbClass" runat="server" CausesValidation="false" CssClass="SmlCombo"
                                                AutoPostBack="true" Height="22px" Width="125px" OnSelectedIndexChanged="cmbClass_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <span class="ClsMdtStar" style="color: #ff0000">*&nbsp;</span>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td align="center" class="ClsBorderlight" style="padding-left: 10px; padding-right: 5px;"
                                            colspan="1">
                                            <span class="ClsLabel">
                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, CompulsorySubjectsCount %>">></asp:Label>
                                                <span id="Span1" class="colonPadding">:</span> </span>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtNoOfSubjects" CssClass="SmlTxtBox" runat="server" MaxLength="2"
                                                onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                ondrop="event.returnValue=false" />
                                            <span class="ClsMdtStar" style="color: #ff0000">*&nbsp;</span>
                                            <asp:RequiredFieldValidator ID="reqvaltxtNoOfSubjects" runat="server" ControlToValidate="txtNoOfSubjects"
                                                                        Display="None" ErrorMessage="<%$ Resources:LocalizedResources, CompulsorySubjectsCountShouldNotBeBlank %>"
                                                ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td align="center" class="ClsBorderlight" style="padding-left: 10px; padding-right: 5px;"
                                            colspan="1">
                                            <span class="ClsLabel">
                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, OptionalSubjectGroupName %>"></asp:Label>
                                                <span id="Span2" class="colonPadding">:</span></span>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtOptionalSubjectGrouptName" runat="server" CssClass="MidTxtBox"
                                                MaxLength="4" onblur="formatName(this)"></asp:TextBox>
                                       </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <table id="tblLstVw" align="center" width="550px">
                                    <tr id="trLegend" runat="server">
                                        <td align="center" style="height: 1px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span class="ClsLblLgnd" style="font: Bold">
                                                            <asp:Label ID="lblLegend" runat="server" Text="<%$ Resources:LocalizedResources, Legend %>"></asp:Label>
                                                        </span>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="TextBox3" runat="server" BackColor="#A8B39D" Height="20px" BorderColor="Black"
                                                            BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px" EnableViewState="False"><img src="../images/spacer.gif" height="20px" width="20px" /></asp:Label>
                                                    </td>
                                                    <td>
                                                        <span class="ClsTextNormal" style="font: Bold">
                                                            <asp:Label ID="lblOptionalSubjectGroup" runat="server" Text="<%$ Resources:LocalizedResources, OptionalSubjectGroup %>"></asp:Label>
                                                        </span>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="TextBox4" runat="server" BackColor="#ACC16F" Height="20px" BorderColor="Black"
                                                            BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px" EnableViewState="False"><img src="../images/spacer.gif" height="20px" width="20px" /></asp:Label>
                                                    </td>
                                                    <td>
                                                        <span class="ClsTextNormal" style="font: Bold">
                                                            <asp:Label ID="lblSubjectGroup" runat="server" Text="<%$ Resources:LocalizedResources, SubjectGroup %>"></asp:Label></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <div id="divContainer" class="GridBorder" runat="server" style="height: 295px; width: 549px;
                                                overflow: auto">
                                                <asp:ListView ID="lstvwClassWiseOptionalSubject" runat="server" DataKeyNames="SubjectId,OptionalSubjectsId,ParentOptionalSubjectId,SubjectGroupId,IsDefault"
                                                    OnItemDataBound="lstvwClassWiseOptionalSubject_ItemDataBound">
                                                    <LayoutTemplate>
                                                        <table align="center" width="530px" id="tblOptSubList" style="color: #333333" cellpadding="0"
                                                            cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="center" style="width: 50px">
                                                                    <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckUncheckAll(this);" />
                                                                </th>
                                                                <th align="left" style="width: 500px; padding-left: 10px">
                                                                    <asp:Label ID="lblSubject" runat="server" Text="<%$ Resources:LocalizedResources, Subject %>"></asp:Label>
                                                                </th>
                                                                <th align="center" style="width: 150px">
                                                                    <asp:Label ID="lblIsDefault" runat="server" Text="<%$ Resources:LocalizedResources, IsDefault %>"></asp:Label>
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="Tr2" runat="server" class='<%# (int)Eval("SubjectGroupId") != 0 ? "ClsSubjectGroup" :(int)Eval("ParentOptionalSubjectId") != 0 ? "ClsOptionalSubject" : "ClsGridRow"  %>'>
                                                            <td align="center" width="50px">
                                                                <asp:CheckBox ID="ChkSelect" runat="server" onclick="ChkOnChange(this)" />
                                                            </td>
                                                            <td align="left" style="padding-left: 8px; width: 200px;">
                                                                <asp:Label ID="lblSubjectName" runat="server" Text='<%#Eval("SubjectName")%>' CssClass="LblNormal"></asp:Label>
                                                            </td>
                                                            <td align="center" width="150px">
                                                                <asp:CheckBox ID="chkIsDefault" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="Tr3" runat="server" class='<%# (int)Eval("SubjectGroupId") != 0 ? "ClsSubjectGroup" :(int)Eval("ParentOptionalSubjectId") != 0 ? "ClsOptionalSubject" : "ClsGridAltRow" %>'>
                                                            <td align="center" width="50px">
                                                                <asp:CheckBox ID="ChkSelect" runat="server" onclick="ChkOnChange(this)" />
                                                            </td>
                                                            <td align="left" style="padding-left: 8px; width: 200px;">
                                                                <asp:Label ID="lblSubjectName" runat="server" Text='<%#Eval("SubjectName")%>' CssClass="LblNormal"></asp:Label>
                                                            </td>
                                                            <td align="center" width="150px">
                                                                <asp:CheckBox ID="chkIsDefault" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                </asp:ListView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trNoRecordMsg" runat="server" visible="false">
                            <td style="height: 10px;" align="center" colspan="2">
                                <span class="LblNoRecord" style="font: Bold; width: 78%">
                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordsFound %>"></asp:Label></span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnSave" Text="<%$ Resources:LocalizedResources, Save %>" runat="server"
                        CssClass="ClsBtn" ValidationGroup="Save" disable-page="true" OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>"
                        CssClass="ClsBtn" BorderWidth="1px" CausesValidation="False" UseSubmitBehavior="false" OnClick="btnCancel_Click" />
                         <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back %>"
                        CssClass="ClsBtn" BorderWidth="1px" CausesValidation="False" UseSubmitBehavior="false" />
                </td>
            </tr>
            <tr id="tr1">
                <td align="center">
                    <table id="Table1" border="0" cellpadding="1" cellspacing="1" runat="server" width="800px">
                        <tr>
                            <td align="center" colspan="2">
                                <table id="Table2" align="center" width="800px">
                                    <tr>
                                        <td align="center" style="height: 1px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <div id="divOptionalSubjectDetalis" class="GridBorder" runat="server" style="width: 900px;
                                                overflow: auto">
                                                <asp:ListView ID="lstvwOptionalSubjectDetalis" runat="server" DataKeyNames="SubjectId,OptionalSubjectsId,ParentOptionalSubjectId"
                                                    OnItemCommand="lstvwOptionalSubjectDetalis_ItemCommand" OnItemDataBound="lstvwOptionalSubjectDetalis_ItemDataBound"
                                                    OnDataBound="lstvwOptionalSubjectDetalis_DataBound">
                                                    <LayoutTemplate>
                                                        <table align="center" width="900px" id="tblOptSubList" style="color: #333333" cellpadding="0"
                                                            cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="left" style="width: 250px; padding-left: 10px">
                                                                    <asp:Label ID="lblOptionalSubjectGroupName" runat="server" Text="<%$ Resources:LocalizedResources, OptionalSubjectGroupName %>"></asp:Label>
                                                                </th>
                                                                <th align="left" style="width: 270px; padding-left: 10px">
                                                                    <asp:Label ID="lblSubject" runat="server" Text="<%$ Resources:LocalizedResources, Subject %>"></asp:Label>
                                                                </th>
                                                                <th align="left" style="width: 200px; padding-left: 10px">
                                                                    <asp:Label ID="lblCompulsorySubjectsCount" runat="server" Text="<%$ Resources:LocalizedResources, CompulsorySubjectsCount %>"></asp:Label>
                                                                </th>
                                                                <th align="center" style="width: 100px; padding-left: 10px">
                                                                    <asp:Label ID="lblEdit" runat="server" Text="<%$ Resources:LocalizedResources, Edit %>"></asp:Label>
                                                                </th>
                                                                <th align="center" style="width: 100px; padding-left: 10px">
                                                                    <asp:Label ID="lblDelete" runat="server" Text="<%$ Resources:LocalizedResources, Delete %>"></asp:Label>
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                            <td align="left" style="padding-left: 8px;">
                                                                <asp:Label ID="lblGroupName" runat="server" Text='<%#Eval("OptionalSubjectName")%>'
                                                                    CssClass="LblNormal"></asp:Label>
                                                            </td>
                                                            <td align="left" style="padding-left: 8px;">
                                                                <asp:Label ID="lblSubjectName" runat="server" Text='<%#Eval("SubjectName")%>' CssClass="LblNormal"></asp:Label>
                                                            </td>
                                                            <td align="center" style="padding-left: 8px;">
                                                                <asp:Label ID="lblNoOfSubjects" runat="server" Text='<%#Eval("NoOfSubjects")%>' CssClass="LblNormal"></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateSubjectGroup"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveSubjectGroup" CausesValidation="false"
                                                                    runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                            <td align="left" style="padding-left: 8px;">
                                                                <asp:Label ID="lblGroupName" runat="server" Text='<%#Eval("OptionalSubjectName")%>'
                                                                    CssClass="LblNormal"></asp:Label>
                                                            </td>
                                                            <td align="left" style="padding-left: 8px;">
                                                                <asp:Label ID="lblSubjectName" runat="server" Text='<%#Eval("SubjectName")%>' CssClass="LblNormal"></asp:Label>
                                                            </td>
                                                            <td align="center" style="padding-left: 8px;">
                                                                <asp:Label ID="lblNoOfSubjects" runat="server" Text='<%#Eval("NoOfSubjects")%>' CssClass="LblNormal"></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateSubjectGroup"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveSubjectGroup" CausesValidation="false"
                                                                    runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                </asp:ListView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr4" runat="server" visible="false">
                            <td style="height: 10px;" align="center" colspan="2">
                                <span class="LblNoRecord" style="font: Bold; width: 78%">
                                    <asp:Label ID="lblNoRecordsFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordsFound %>"></asp:Label></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:HiddenField ID="hidMode" runat="server" />
                                <asp:HiddenField ID="hidNoOfSubjects" runat="server" />
                                <asp:HiddenField ID="hidOptionalSubjectGroupName" runat="server" />
                                <asp:HiddenField ID="hidParentOptionalSubjectGroupId" runat="server" />
                                <asp:HiddenField ID="hidAreYouSureYouWantToDeleteThisRecords" runat="server" />
                                <asp:HiddenField ID="hidCultureInfo" runat="server" />
                                <asp:HiddenField ID="hidAtLeastOneOptionalSubjectShouldBeSelected" runat="server" />
                                <asp:HiddenField ID="hidDefaultSubjectCanBeSelected" runat="server" />
                                <asp:HiddenField ID="hidAtLeast" runat="server" />
                                <asp:HiddenField ID="hidAtMost" runat="server" />
                                <asp:HiddenField ID="hidOptionalSubjectGroupNameShouldNotBeDuplicated" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        _clientlstvwClassWiseOptionalSubjectId = "<%=this.lstvwClassWiseOptionalSubject.ClientID %>"
        _clientlstvwOptionalSubjectDetalisId = "<%=this.lstvwOptionalSubjectDetalis.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clienttxtNoOfSubjects = "<%=this.txtNoOfSubjects.ClientID %>"
        _clienttxtOptionalSubjectGrouptName = "<%=this.txtOptionalSubjectGrouptName.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        _clientlblErrorMessage = "<%=this.lblErrorMessage.ClientID %>"
        clientlblErrorMessage = "<%=this.lblErrorMessage.ClientID %>"
        _ClientChkAll = _clientlstvwClassWiseOptionalSubjectId + "_ChkSelectAll";

        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>";

        var _ChkSelectAll = '_ChkSelectAll';
        var _ctrl = '_ctrl';
        var _clientlstvwClassWiseOptionalSubject = '<%= this.lstvwClassWiseOptionalSubject.ClientID %>';

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm(document.getElementById("<%=this.hidAreYouSureYouWantToDeleteThisRecords.ClientID %>").value)) {
                bResult = false
            }
            return bResult
        }

        var Page_IsValid = true;
        function ValidateSubjects() {
            $("[id*=lblErrorMessage]")[0].innerHTML = '';
            var chk;
            Page_IsValid = true;
            var iRowCount = 0;
            var IsChecked = false;
            var isPageValid = true;
            isPageValid = Page_ClientValidate("Save");
            if (!isPageValid) {
                document.getElementById(_clientlblUpdateSucess).innerHTML = "";
                return true;
            }
            if (isPageValid) {
                if (document.getElementById(_clientlblUpdateSucess) != undefined) {
                    document.getElementById(_clientlblUpdateSucess).innerHTML = ""
                }
                chk = CheckAtleastOneCheckBox(_clientlstvwClassWiseOptionalSubjectId, 'ChkSelect', $get('tblOptSubList').rows.length)
                if (chk != null) {
                    if (chk == true) {
                        IsChecked = true;
                    }
                }
                if (!IsChecked) {
                    alert(document.getElementById("<%=this.hidAtLeastOneOptionalSubjectShouldBeSelected.ClientID %>").value);
                    Page_IsValid = false;
                    return false
                }

                var NoOfSubjects = document.getElementById(_clienttxtNoOfSubjects).value
                var iDefaultSubjects = 0
                $("input:checkbox[id$=_chkIsDefault]").each(
                    function () {
                        if (!this.disabled && this.checked)
                            iDefaultSubjects++;
                    }
                );

                if (NoOfSubjects != iDefaultSubjects) {
                    if (NoOfSubjects < iDefaultSubjects) {
                        alert(document.getElementById("<%=this.hidAtMost.ClientID %>").value + NoOfSubjects + document.getElementById("<%=this.hidDefaultSubjectCanBeSelected.ClientID %>").value);
                        Page_IsValid = false;
                        return false;
                    }
                    else if (NoOfSubjects > iDefaultSubjects) {
                        
                        alert(document.getElementById("<%=this.hidAtLeast.ClientID %>").value + NoOfSubjects + document.getElementById("<%=this.hidDefaultSubjectCanBeSelected.ClientID %>").value);
                        Page_IsValid = false;
                        return false;
                    }
                }

                var bResult = true;
                $("[id$=_lblSubjectName]").each(
                    function () {
                        if (document.getElementById(_clienttxtOptionalSubjectGrouptName).value!="") 
                            if (document.getElementById(_clienttxtOptionalSubjectGrouptName).value == this.innerHTML)
                                  bResult = false;
                    }
                );

                if (!bResult) {
                    alert(document.getElementById("<%=this.hidOptionalSubjectGroupNameShouldNotBeDuplicated.ClientID %>").value)
                    Page_IsValid = false;
                    return false;
                }

                return true;
            }
        }

        // This function is used to Check Uncheck all checkboxes in the ListView
        function CheckUncheckAll(src) {
            if (src == null)
                src = $get(_clientlstvwClassWiseOptionalSubject + '_ChkSelectAll');

            var iRowCount = 0;
            var chk = $get(_clientlstvwClassWiseOptionalSubject + _ctrl + iRowCount + '_ChkSelect');
            while (chk != null) {
                chk.checked = src.checked;
                ChkOnChange(chk);
                iRowCount++;
                chk = $get(_clientlstvwClassWiseOptionalSubject + _ctrl + iRowCount + '_ChkSelect');
            }
        }

        // This function is uesd to  single Check box in list view
        function ChkOnChange(src) {
            var iRowNo = src.id.match(/_ctrl(\d+)_ChkSelect/)[1];
            var isClassWiseOptionalSubject = $get(_clientlstvwClassWiseOptionalSubject + _ctrl + iRowNo + '_chkIsDefault');
            isClassWiseOptionalSubject.checked = false;
            isClassWiseOptionalSubject.disabled = !src.checked;
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
