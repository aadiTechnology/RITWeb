<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="TestNamesList.aspx.cs" Inherits="TestNamesList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr>
                <td>
                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ShowMessageBox="True" ShowSummary="False"
                        CssClass="LblErrorMsg" />
                    <asp:CustomValidator ID="cst_ExamNames" runat="server" ClientValidationFunction="CstDuplicateTextValidation"
                        CssClass="LblErrorMsg" Display="None" ErrorMessage="<%$ Resources:LocalizedResources,ExamNameCanNotBeDuplicated%>"></asp:CustomValidator>

                         <asp:CustomValidator ID="cstfinalexam" runat="server" ClientValidationFunction="FinalExamValidator"
                        CssClass="LblErrorMsg" Display="None" ErrorMessage=""></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblError" CssClass="LblErrorMsg" runat="server" EnableViewState="false"> </asp:Label>
                </td>
            </tr>
            <tr align="center">
                <td>
                    <div id="Div1" runat="server" style="width: 50%; height: 269px; overflow: auto" class="ClsGridBG GridBorder">
                        <asp:GridView ID="grdTestNames" runat="server" Width="100%" AutoGenerateColumns="False"
                            Height="43px" PageSize="20" OnRowDataBound="grdGroupDetails_RowDataBound" CellPadding="0"
                            CellSpacing="1" ForeColor="#333333" GridLines="None" BackColor="White" DataKeyNames="SchoolWise_Test_Name,SchoolWise_Test_Id,Original_SchoolWise_Test_Id,School_Id,Term_Id,IsFinalExam">
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                            </PagerStyle>
                            <PagerSettings NextPageText="<%$ Resources:LocalizedResources, Next%>" LastPageText="<%$ Resources:LocalizedResources, Last%>" PreviousPageText="<%$ Resources:LocalizedResources, Previous%>" 
                                FirstPageText="<%$ Resources:LocalizedResources, First%>" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <input id="ChkAllDel" type="checkbox" runat="server" onclick="CheckAllOrUncheckAllGridItemsonlocaldocument(document,_clientGridId,this,'ChkBoxDelete')" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkBoxDelete" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="1%" HorizontalAlign="Left" CssClass="paddingLSML" />
                                    <HeaderStyle Width="1%" HorizontalAlign="Left" CssClass="paddingLSML" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, ExamName%>" SortExpression="SchoolWise_Test_Name">
                                    <EditItemTemplate>
                                        &nbsp;
                                    </EditItemTemplate>
                                    <ItemStyle Width="41%" Wrap="False" />
                                    <HeaderStyle Width="41%" Wrap="False"  CssClass="ClsPaddingL"/>
                                    <ItemTemplate>
                                        &nbsp;<asp:TextBox ID="txttest_nameName" runat="server" MaxLength="50" CssClass="LrgTxtBox"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqTxtPrefixVal" runat="server" ControlToValidate="txttest_nameName"
                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources,ExamNameShouldNotBeBlank%>"></asp:RequiredFieldValidator>&nbsp;
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" CssClass="ClspaddingL" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources,Term%>">
                                    <EditItemTemplate>
                                        &nbsp;
                                    </EditItemTemplate>
                                    <ItemStyle Width="21%" HorizontalAlign="Center" Wrap="False" />
                                    <HeaderStyle  Width="21%" Wrap="False" />
                                    <ItemTemplate>
                                        &nbsp;<asp:DropDownList ID="cmbTerm" runat="server" Width="90%" >
                                            <asp:ListItem Text="<%$ Resources:LocalizedResources,Select%>" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:LocalizedResources,Term1%>" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:LocalizedResources,Term2%>" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" CssClass="ClspaddingL" />
                                </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Is Final Exam" SortExpression="SchoolWise_Test_Name">
                                    <EditItemTemplate>
                                        &nbsp;
                                    </EditItemTemplate>
                                    <ItemStyle Width="13%" HorizontalAlign="center"  />
                                    <HeaderStyle Width="13%" HorizontalAlign="center" Wrap="False" />
                                    <ItemTemplate>
                                         <asp:RadioButton ID="optfinalexam" runat="server" CommandName="UserDetails" />
                                    </ItemTemplate>
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
            <tr align="center">
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr align="center">
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources,Save%>" CssClass="ClsBtn" BorderWidth="1px"
                        OnClick="btnSave_Click" disable-page="true" />
                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources,Cancel%>" CssClass="ClsBtn" BorderWidth="1px"
                        OnClick="btnCancel_Click" CausesValidation="False" UseSubmitBehavior="false" />
                </td>
               
            </tr>
            <tr>
              <td style="width: 798px">
                  <asp:HiddenField ID="hidCultureInfo" runat="server" />
                   <asp:HiddenField ID="hidgrdTestNamesRowCount" runat="server" />
                   <asp:HiddenField ID="hidRowIndex" runat="server" />

               </td> 
            </tr>
        </table>
        &nbsp;
        <br />
        <br />
    </div>
    <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.grdTestNames.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        _clientgrdTestNames = "<%=this.grdTestNames.ClientID%>";
        _clientgrdTestNamesRowCount = "<%=this.hidgrdTestNamesRowCount.ClientID%>";
        _clientRowIndex = "<%=this.hidRowIndex.ClientID%>";

        var bPaging = false
        var Page_IsValid = true;
        function ConfirmAction(iPageCount, sActionName) {
            
            Page_IsValid = true;
            var bResult = true
            if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientGridId, 'ChkBoxDelete', sActionName, 'false', bPaging, 'false')) {
                
                bResult = true
                if (typeof (Page_ClientValidate) == 'function') {
                    bResult = Page_ClientValidate()
                }
                
            }
            else {
                
                bResult = false
                alert(sActionName);
                Page_IsValid = false;
            }
            return bResult
        }
        function DisableButtons(ObjBtn) {
            document.getElementById(_clientbtnSave).disabled = true
            document.getElementById(_clientbtnCancel).disabled = true
            __doPostBack(document.getElementById(_clientbtnCancel).name, '')
        }


        //this is to select or unselect the datagrid check boxes 
        function CheckAllOrUncheckAllGridItemsonlocaldocument(oDocument, grdid, obj, objlist, iPageCnt) {
            if (obj.checked) {
                DGSelectAll(oDocument, grdid, objlist, iPageCnt)
                verifyEnableDisableDataRows()
            }
            else {
                DGUnselectAll(oDocument, grdid, objlist, iPageCnt)
                verifyEnableDisableDataRows()
            }
        }


        function DGSelectAll(oDocument, grdid, objid, iPageCnt) {
            //.this function is to check all the items
            var chkbox;
            var i = getStartIndex(iPageCnt);

            if (i < 10)
                chkbox = oDocument.getElementById(grdid + "_ctl0" + i + "_" + objid)
            else
                chkbox = oDocument.getElementById(grdid + "_ctl" + i + "_" + objid)

            while (chkbox != null) {
                chkbox.checked = true;
                i = i + 1;
                if (i < 10)
                    chkbox = oDocument.getElementById(grdid + "_ctl0" + i + "_" + objid)
                else
                    chkbox = oDocument.getElementById(grdid + "_ctl" + i + "_" + objid)
            }

        } //-------------- 

        function DGUnselectAll(oDocument, grdid, objid, iPageCnt) {
            //.this function is to check all the items
            var chkbox;
            var i = getStartIndex(iPageCnt);

            if (i < 10)
                chkbox = oDocument.getElementById(grdid + "_ctl0" + i + "_" + objid)
            else
                chkbox = oDocument.getElementById(grdid + "_ctl" + i + "_" + objid)

            while (chkbox != null) {
                chkbox.checked = false;
                i = i + 1;
                if (i < 10)
                    chkbox = oDocument.getElementById(grdid + "_ctl0" + i + "_" + objid)
                else
                    chkbox = oDocument.getElementById(grdid + "_ctl" + i + "_" + objid)
            }
        }


        function CstDuplicateTextValidation(oSrc, args) {
            if (DuplicateTextValidation(document, _clientGridId, "txttest_nameName", "ChkBoxDelete", false)) {
                args.IsValid = true
                return false
            }
            else {
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function SelectAll(chk, flag) {

            if (flag == 0)
                $("#<%=grdTestNames.ClientID %>_tblNoticeDetails input[type = checkbox]").attr('checked', chk.checked);
        }

        //This function is used to set default status to radio button.
        function validate(obj, iRowIndex, optRowIndex) {
           
          var rowIndex = iRowIndex
            if (iRowIndex < 10)
                rowIndex = '0' + iRowIndex

            var cmb = document.getElementById(_clientgrdTestNames + "_ctl" + rowIndex + "_cmbTerm")

            var optCurrent = document.getElementById(_clientgrdTestNames + "_ctl" + rowIndex + "_optfinalexam")
            
            var i = 2

            if (i < 10)
                counter = '0' + i;
            else
                counter = i;

            var opt = document.getElementById(_clientgrdTestNames + "_ctl0" + i + "_optfinalexam")

            var flag = false

            while (opt != null) {

                var cmbNew = document.getElementById(_clientgrdTestNames + "_ctl" + counter + "_cmbTerm");
                var optNew = document.getElementById(_clientgrdTestNames + "_ctl" + counter + "_optfinalexam");
               
                if (cmbNew != null && cmbNew.value == cmb.value && optNew.checked == true) {
                    flag = true;
                    break;
                }

                i++;

                if (i < 10)
                    counter = '0' + i;
                else
                    counter = i;

                opt = document.getElementById(_clientgrdTestNames + "_ctl" + counter + "_optfinalexam")

            }
            if (flag == false) 
                optCurrent.checked = true;
        }

       //This function is used to verify given checked radio button status.
        function Verify(obj, iRowIndex) {
            var originalIndex = iRowIndex
            iRowIndex  = iRowIndex + 2
            var rowIndex = iRowIndex
            if (iRowIndex < 10)
                rowIndex = '0' + iRowIndex
            var opt = document.getElementById(_clientgrdTestNames + "_ctl" + rowIndex + "_optfinalexam")

            if (opt.checked) {

                SelectFinalExam(opt, originalIndex);
            }
            else {
                validate(obj, iRowIndex, originalIndex)
            }
            
        }

        //This function is used to select appropriate radio button to appropriate exam
        function SelectFinalExam(obj, iRowIndex) {
            iRowIndex  = iRowIndex + 2            
            var bResult = true
            var counter
            var ListRowCnt = document.getElementById(_clientgrdTestNamesRowCount).value

            var rowIndex = iRowIndex
            if (iRowIndex < 10)
                rowIndex = '0' + iRowIndex

            var cmb = document.getElementById(_clientgrdTestNames + "_ctl" + rowIndex + "_cmbTerm")

            var i = 2

            if (i < 10)
                counter = '0' + i;
            else
                counter = i;

            var opt = document.getElementById(_clientgrdTestNames + "_ctl0" + i + "_optfinalexam")

          
            while (opt != null) {

                var cmbNew = document.getElementById(_clientgrdTestNames + "_ctl" + counter + "_cmbTerm")
                
                if (cmbNew != null && cmbNew.value == cmb.value ) {
                    if (document.getElementById(_clientgrdTestNames + "_ctl" + counter + "_optfinalexam") != null) {
                        document.getElementById(_clientgrdTestNames + "_ctl" + counter + "_optfinalexam").checked = false
                    }
                }

                i++;

                if (i < 10)
                    counter = '0' + i;
                else
                    counter = i;

                opt = document.getElementById(_clientgrdTestNames + "_ctl" + counter + "_optfinalexam")
            }
            obj.checked = true;
            return bResult
        }

        function FinalExamValidator(oSrc, args) {

            var isFirstTermConfigured = false;
            var isSecondTermConfigured = false;

            var i = 2

            if (i < 10)
                counter = '0' + i;
            else
                counter = i;

            var cmb = document.getElementById(_clientgrdTestNames + "_ctl0" + i + "_cmbTerm")
            while (cmb != null) {

                var opt =  document.getElementById(_clientgrdTestNames + "_ctl" + counter + "_optfinalexam")

                if (cmb.value == "1" && opt.checked)
                    isFirstTermConfigured = true;
                else if (cmb.value == "2" && opt.checked)
                    isSecondTermConfigured = true;

                if (isFirstTermConfigured && isSecondTermConfigured)
                    break;

                i++;

                if (i < 10)
                    counter = '0' + i;
                else
                    counter = i;

                cmb = document.getElementById(_clientgrdTestNames + "_ctl" + counter + "_cmbTerm")

            }

            if (isFirstTermConfigured == false || isSecondTermConfigured == false) {
                oSrc.errormessage = 'Please select final exam for each term.'
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }
        verifyEnableDisableDataRows()
        function verifyEnableDisableDataRows() {
            var i = 2

            if (i < 10)
                counter = '0' + i;
            else
                counter = i;

            var opt = document.getElementById(_clientgrdTestNames + "_ctl0" + i + "_ChkBoxDelete")
            while (opt != null) {
                EnableDisableEntirRow(opt, i)
                i++;
                if (i < 10)
                    counter = '0' + i;
                else
                    counter = i;
                opt = document.getElementById(_clientgrdTestNames + "_ctl" + counter + "_ChkBoxDelete")
            }
        }

        function EnableDisableEntirRow(obj, iRowIndex) {
            var rowIndex = iRowIndex
                        if (iRowIndex < 10)
                            rowIndex = '0' + iRowIndex

                        var cmb = document.getElementById(_clientgrdTestNames + "_ctl" + rowIndex + "_cmbTerm")
                        var txt = document.getElementById(_clientgrdTestNames + "_ctl" + rowIndex + "_txttest_nameName")
                        var rdo = document.getElementById(_clientgrdTestNames + "_ctl" + rowIndex + "_optfinalexam")
                        if (obj.checked) {
                            cmb.disabled = false;
                            txt.disabled = false;
                            rdo.disabled = false;
                        }
                        else {
                            cmb.disabled = true;
                            txt.disabled = true;
                            rdo.disabled = true;
                            cmb.value = "0"
                            rdo.checked = false
                        }
        }

    </script>
</asp:Content>
