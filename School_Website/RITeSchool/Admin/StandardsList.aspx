<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardsList.aspx.cs" MasterPageFile="../MasterPages/MasterPage.master"
    Inherits="StandardsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server"> 

    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr align="center">
                <td align="left">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td valign="top">
                                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="LblErrorMsg"
                                    ForeColor="" ShowMessageBox="false" ShowSummary="true" />
                                <asp:CustomValidator ID="cstValStrength" runat="server" Display="None" ClientValidationFunction="ValidateStrength"></asp:CustomValidator>
                                <asp:CustomValidator ID="CustomValidator1" runat="server" Display="None" ClientValidationFunction="ValidateThreshold"></asp:CustomValidator>
                                <asp:CustomValidator ID="CustomValidator2" runat="server" Display="None" ClientValidationFunction="CompareThreshold"></asp:CustomValidator>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr align="center">
                <td>
                    <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr align="center">
                <td align="center">
                    <div id="Div1" runat="server" style="width: 50%; overflow: auto;">
                        <asp:GridView CssClass="GridBorder" ID="grdStandards" runat="server" Width="100%"
                            AutoGenerateColumns="False" Height="43px" PageSize="20" AllowPaging="False" OnRowDataBound="grdGroupDetails_RowDataBound"
                            CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None" DataKeyNames="Standard_Id,Original_Standard_Id,School_Id,Is_PrePrimary,Section,Strength,Threshold,NextOriginalStandardId">
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                            </PagerStyle>
                            <PagerSettings NextPageText="<%$ Resources:LocalizedResources, NextPageText %>" LastPageText="<%$ Resources:LocalizedResources, LastPageText  %>" 
                            PreviousPageText="<%$ Resources:LocalizedResources, PreviousPageText  %>" FirstPageText="<%$ Resources:LocalizedResources, FirstPageText  %>" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <input id="ChkAllDel" type="checkbox" runat="server" style="margin-left: 2px" onclick="SelectAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkBoxDelete" runat="server" />
                                        <asp:HiddenField ID="hidIsNewStandard" runat="server" Value="N" />
                                    </ItemTemplate>
                                    <ItemStyle Width="30px" HorizontalAlign="Center" />
                                    <HeaderStyle Width="30px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, StandardListgrdStandardsHeader%>"
                                    DataField="Standard_Name">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                </asp:BoundField>                                
                                <asp:TemplateField HeaderText="Max. Strength">
									<ItemTemplate>										
                                        <asp:TextBox ID="txtStrength" runat="server" CssClass="SmlTxtBox" MaxLength="2" style="text-align:right; padding-right:5px;width:50px;"
                                        onblur="extractNumber(this,0,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                       onkeyup="extractNumber(this,0,false);" onpaste="event.returnValue=false"></asp:TextBox>
									</ItemTemplate>
									<ControlStyle CssClass="SmlTxtBox" />
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
									<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
								</asp:TemplateField>
                                <asp:TemplateField HeaderText="Threshold">
									<ItemTemplate>										
                                        <asp:TextBox ID="txtThreshold" runat="server" MaxLength="2" style="text-align:right; padding-right:5px;width:50px;"
                                        onblur="extractNumber(this,0,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                       onkeyup="extractNumber(this,0,false);" onpaste="event.returnValue=false"></asp:TextBox>
									</ItemTemplate>
									<ControlStyle CssClass="SmlTxtBox" />
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
									<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
								</asp:TemplateField>
                            </Columns>
                            <RowStyle CssClass="ClsGridRow" />
                            <HeaderStyle CssClass="ClsGridHeader" />
                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table align="center" width="830px">
                        <tr>
                            <td align="left" class="ClsBorderlight " style="width: 80px; background-color: #ffffc4;">
                                <asp:Label ID="Label" runat="server" BorderWidth="0px" Font-Bold="True" Text="<%$ Resources:LocalizedResources, StandardListNote %>"
                                    CssClass="LblNrmlB"></asp:Label>
                                <span id="Span1" class="colonPadding">:</span>
                            </td>
                            <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                <asp:Label ID="lblNote" runat="server" Width="750px" BorderWidth="0px" CssClass="LblSmlV" 
                                Text="<%$ Resources:LocalizedResources, StandardListNoteText %>" ></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr align="center">
                <td align="center">
                    <asp:Button ID="imgBtnSave" Text="<%$ Resources:LocalizedResources, Save %>" runat="server"
                        CssClass="ClsBtn" BorderWidth="1px" OnClick="imgBtnSave_Click" UseSubmitBehavior="false" />
                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>"
                        CssClass="ClsBtn" BorderWidth="1px" CausesValidation="False" UseSubmitBehavior="false" />
                </td>
            </tr>
            <asp:HiddenField ID="hidNavigate" runat="server" />
            <asp:HiddenField ID="hidStartDate" runat="server" />
            <asp:HiddenField ID="hidEndDate" runat="server" />
            <asp:HiddenField ID="hidCultureInfo" runat="server" />
            <asp:HiddenField ID="hidWanttoSaveAcademicYear" runat="server" />
        </table>
    </div>
    <script language="javascript" type="text/javascript">
        _clientSaveId = "<%=this.imgBtnSave.ClientID %>"
        _clientbtnCancelId = "<%=this.btnCancel.ClientID %>"
        _clientGridId = "<%=this.grdStandards.ClientID %>"
        _clienthidNavigate = "<%=this.hidNavigate.ClientID %>"
        _clientlblNote = "<%=this.lblNote.ClientID %>"
        function DisableButtons() {
            document.getElementById(_clientSaveId).disabled = true
            document.getElementById(_clientbtnCancelId).disabled = true
            __doPostBack(document.getElementById(_clientbtnCancelId).name, '')
        }
        function ConfirmAction(iPageCount, sActionName) {
            var bResult = true
            if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientGridId, 'ChkBoxDelete', sActionName, 'false', iPageCount, 'true')) {
                bResult = true
                if (typeof (Page_ClientValidate) == 'function') {
                    bResult = Page_ClientValidate()
                }
                if (bResult) {
                    document.getElementById(_clientSaveId).disabled = true
                    document.getElementById(_clientbtnCancelId).disabled = true
                }
                if (IsNewStandard() && window.confirm(document.getElementById(_clientlblNote).innerHTML + '\n' + document.getElementById("<%=hidWanttoSaveAcademicYear.ClientID %>").value))
                    document.getElementById(_clienthidNavigate).value = "Y";
                else
                    document.getElementById(_clienthidNavigate).value = "N";
            }
            else
            { bResult = false; }
            return bResult
        }
        function DisableText(notodisable) {
            var count = document.forms[0].elements.length
            for (i = 0; i < count; i++) {
                var element = document.forms[0].elements[i]
                if (element.id != notodisable && element.type == "Submit")
                { element.disabled = true; }
            }
        }

        function SelectAll(chk) {
            $('#<%=grdStandards.ClientID %> input:checkbox').attr('checked', chk.checked);
        }

        function IsNewStandard() {
            var DefaultString = "ctl00_MainBody_grdStandards_";
            var iTotalRows = document.getElementById('ctl00_MainBody_grdStandards').rows.length;
            var i = 2;
            for (i = 2; i <= iTotalRows; i++) {
                var str = "";
                if (i >= 10)
                    str = "ctl" + i;
                else
                    str = "ctl0" + i;
                if (document.getElementById(DefaultString + str + '_ChkBoxDelete').checked && document.getElementById(DefaultString + str + '_hidIsNewStandard').value == "Y") {
                    return true
                }
            }
            return false;
        }

        function ValidateStrength(oSrc, args) {
            var i = 2;
            var chk = document.getElementById(_clientGridId + "_ctl02" + "_ChkBoxDelete")
            var str = "_ctl0" + i;
            var found = false;
            
            while (chk != null) {

                if (chk.checked)
                {
                    var strength = document.getElementById(_clientGridId + str + "_txtStrength").value
                    var threshold = document.getElementById(_clientGridId + str + "_txtThreshold").value
                    if (strength != "" && parseInt(strength) != 0 && (threshold == "" || parseInt(threshold) == 0)) {
                        found = true;
                        break;
                    }
                }

                i++;
                if (i >= 10)
                    str = "_ctl" + i;
                else
                    str = "_ctl0" + i;
                chk = document.getElementById(_clientGridId + str + "_ChkBoxDelete")
            }

            if (found) {
                oSrc.errormessage = "If Max. Strength is set then Threshold should not be empty.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateThreshold(oSrc, args) {
            var i = 2;
            var chk = document.getElementById(_clientGridId + "_ctl02" + "_ChkBoxDelete")
            var str = "_ctl0" + i;
            var found = false;

            while (chk != null) {

                if (chk.checked) {
                    var strength = document.getElementById(_clientGridId + str + "_txtStrength").value
                    var threshold = document.getElementById(_clientGridId + str + "_txtThreshold").value
                                        
                    if (threshold != "" && parseInt(threshold) != 0 && (strength == "" || parseInt(strength) == 0)) {
                        found = true;
                        break;
                    }
                }

                i++;
                if (i >= 10)
                    str = "_ctl" + i;
                else
                    str = "_ctl0" + i;
                chk = document.getElementById(_clientGridId + str + "_ChkBoxDelete")
            }

            if (found) {
                oSrc.errormessage = "If Threshold is set then Max. Strength should not be empty.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function CompareThreshold(oSrc, args) {
            var i = 2;
            var chk = document.getElementById(_clientGridId + "_ctl02" + "_ChkBoxDelete")
            var str = "_ctl0" + i;
            var found = false;

            while (chk != null) {

                if (chk.checked) {
                    var strength = document.getElementById(_clientGridId + str + "_txtStrength").value
                    var threshold = document.getElementById(_clientGridId + str + "_txtThreshold").value
                    if (strength != "" && parseInt(strength) != 0 && threshold != "" && parseInt(threshold) != 0 && parseInt(strength) <= parseInt(threshold)) {
                        found = true;
                        break;
                    }
                }

                i++;
                if (i >= 10)
                    str = "_ctl" + i;
                else
                    str = "_ctl0" + i;
                chk = document.getElementById(_clientGridId + str + "_ChkBoxDelete")
            }

            if (found) {
                oSrc.errormessage = "Threshold should not be greater than or equal to Max. Strength.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

    </script>
</asp:Content>
