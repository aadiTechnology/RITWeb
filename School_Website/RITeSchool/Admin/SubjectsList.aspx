<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SubjectsList.aspx.cs" Inherits="SubjectsList" MasterPageFile="../MasterPages/MasterPage.master"%>

<asp:content id="Content1" contentplaceholderid="MainBody" runat="Server">
 
    <div class="MainBodyDiv">
     <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%" >
        <tr>
            <td >  
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" CssClass="LblErrorMsg" />
                <asp:CustomValidator ID="cst_Subject" runat="server" ClientValidationFunction="CstDuplicateTextValidation"
                    CssClass="LblErrorMsg" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, SubjectNamesShouldNotBeDuplicate %>" ></asp:CustomValidator>
				<asp:CustomValidator ID="cstShortNameValidator"
									 runat="server"
									 Display="None"
									 ClientValidationFunction="ValidateShortName" />
                <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg"  EnableViewState="false"></asp:Label>
			</td>
        </tr>
        <tr align="center">
        <td> 
        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
      <div id="div1" class="GridBorder" style=" width:50%;height:189pt;overflow:auto;">  
        <asp:GridView ID="grdSubjects" UseAccessibleHeader="true" runat="server" Width="100%" AutoGenerateColumns="False" 
             OnRowDataBound="grdGroupDetails_RowDataBound" PageSize="100"  CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None" DataKeyNames="Subject_Name,Short_Name,Subject_Id,Original_Subject_Id,School_Id,Is_CoCurricularActivity,IsAttitudeSubject" OnRowCreated="grdSubjects_RowCreated">
            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False"></PagerStyle>
            <PagerSettings NextPageText="<%$ Resources:LocalizedResources, NextPageText %>"  LastPageText="<%$ Resources:LocalizedResources, LastPageText %>" 
             PreviousPageText="<%$ Resources:LocalizedResources, PreviousPageText %>"  FirstPageText="<%$ Resources:LocalizedResources, FirstPageText %>"  Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <input id="ChkAllDel" type="checkbox" runat="server" onclick="SelectAll(this);" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkBoxDelete" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="1%" HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center"/>
                    
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, SubjectName %>" SortExpression="Subject_Name">
					<EditItemTemplate>
						&nbsp;
					</EditItemTemplate>
					<ItemStyle Width="15%" Wrap="False" />
					<HeaderStyle Width="15%" Wrap="False" />
					<ItemTemplate>
						&nbsp;<asp:TextBox ID="txtSubjectName" runat="server" MaxLength="50" CssClass="MidTxtBox" Text='<%# Eval("Subject_Name") %>' ></asp:TextBox>
						<asp:RequiredFieldValidator ID="reqTxtPrefixVal" runat="server" ControlToValidate="txtSubjectName"
							Display="None" ErrorMessage="<%$ Resources:LocalizedResources, SubjectNameShouldNotBeBlank %>"></asp:RequiredFieldValidator>&nbsp;
					</ItemTemplate>
					<ItemStyle HorizontalAlign="Left" CssClass="paddingLSML"/>
				</asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, ShortName  %>" SortExpression="Short_Name">
					<EditItemTemplate>
						&nbsp;
					</EditItemTemplate>
					<ItemStyle Width="15%" Wrap="False" />
					<HeaderStyle Width="15%" Wrap="False" />
					<ItemTemplate>
						<asp:TextBox ID="txtShortName" runat="server" MaxLength="6" CssClass="MidTxtBox" Text='<%# Eval("Short_Name") %>' ></asp:TextBox>
					</ItemTemplate>
					<ItemStyle HorizontalAlign="Left" CssClass="paddingLSML"/>
				</asp:TemplateField>
				<asp:TemplateField>
                    <HeaderTemplate>
                    <asp:Label ID="lblIsCoCurricularActivity" runat="server" Text="<%$ Resources:LocalizedResources, IsCoCurricularActivity  %>"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkBoxIsCoCurricularActivity" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                    <HeaderStyle Width="10%" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center"/>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                    <asp:Label ID="lblIsAttitudeSubject" runat="server" Text="Is Attitude Subject?"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkIsAttitudeSubject" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                    <HeaderStyle Width="10%" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center"/>
                </asp:TemplateField>
            </Columns>
            <RowStyle CssClass="ClsGridRow" />
            <HeaderStyle CssClass="ClsGridHeader" />
            <AlternatingRowStyle CssClass="ClsGridAltRow" />
            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
        </asp:GridView>
          </div>
       </ContentTemplate>
       </asp:UpdatePanel>
     </td>
     </tr>
         <tr align="center">
             <td>
                 &nbsp;</td>
         </tr>
         <tr align="center">
             <td>
        <asp:Button ID="imgBtnSave" Text="<%$ Resources:LocalizedResources, Save %>" runat="server"  CssClass="ClsBtn"  BorderWidth="1px" OnClick="imgBtnSave_Click" disable-page="false" />
		<asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>" CssClass="ClsBtn" BorderWidth="1px"  CausesValidation="False" UseSubmitBehavior="false"/></td>
         </tr>
     </table>   
        <asp:HiddenField id="hidSortDirection" runat="server"></asp:HiddenField>
         <asp:HiddenField ID="hidCultureInfo" runat="server" />
        <asp:HiddenField id="hidSchoolId" runat="server"></asp:HiddenField>
        <asp:HiddenField id="hidConfigType" runat="server"></asp:HiddenField>
        <asp:HiddenField id="hidSortExpression" runat="server"></asp:HiddenField>
        <asp:HiddenField id="hidShortNameDuplicated" runat="server"></asp:HiddenField>
        <asp:HiddenField id="hidsSelectAtLeastOneSubject" runat="server"></asp:HiddenField>
      <br />
      <br />                                                     
   </div>  
   
    <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.grdSubjects.ClientID %>"
        _clientimgBtnSave = "<%=this.imgBtnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"


        function EnableCheck(obj, IsCoCurricular, isAttitudeSubject) {            
            if (obj.checked) {
                IsCoCurricular.disabled = false;

                if (IsCoCurricular.checked)
                    isAttitudeSubject.disabled = false;
                else
                    isAttitudeSubject.disabled = true;
                        
            }
            else {
                isAttitudeSubject.disabled = true;
                IsCoCurricular.disabled = true;
                IsCoCurricular.checked = false;
                isAttitudeSubject.checked = false
            }
        }

        function EnableAttitudeField(obj, isAttitudeSubject) {
            if (obj.checked) {
                isAttitudeSubject.disabled = false;
            }
            else {
                isAttitudeSubject.disabled = true;
                isAttitudeSubject.checked = false;
            }
        }

        PageLoad();

        //This function is used to set condition on isCoCurricular checkbox about enable or disable.
        function PageLoad() {
            //var DefaultString = "ctl00_MainBody_grdSubjects_";
            var iTotalRows = document.getElementById('ctl00_MainBody_grdSubjects').rows.length;
            var i = 2;
            for (i = 2; i <= iTotalRows; i++) {
                 if(i < 10)
                        var str = "_ctl0" + i;
                 else
                        var str = "_ctl" + i;
                var chk = document.getElementById(_clientGridId + str + "_ChkBoxDelete");
                var chk2 = document.getElementById(_clientGridId + str + "_ChkBoxIsCoCurricularActivity");
                var chk3 = document.getElementById(_clientGridId + str + "_chkIsAttitudeSubject");
                EnableCheck(chk, chk2, chk3);
            }
        }
        function DisableButtons() {
            document.getElementById(_clientimgBtnSave).disabled = true
            document.getElementById(_clientbtnCancel).disabled = true
            __doPostBack(document.getElementById(_clientbtnCancel).name, '')
        }
        function ConfirmAction(iPageCount, sActionName) {
            var bResult = true
            if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientGridId, 'ChkBoxDelete', sActionName, 'false', iPageCount, 'true')) {
                bResult = true
                if (typeof (Page_ClientValidate) == 'function') {
                    bResult = Page_ClientValidate()
                }
                 
            }
            else
            { bResult = false; }
            return bResult
        }
        function CstDuplicateTextValidation(oSrc, args) {
            if (DuplicateTextValidation(document, _clientGridId, "txtSubjectName", "ChkBoxDelete", false)) {
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

        function DisableChkBoxIsCoCurricularActivity(chkBoxIsDeleted) {
            
            var sRowno = chkBoxIsDeleted.id.substring(30, 32);
            var chkBoxIsCoCurricularactivity = document.getElementById('ctl00_MainBody_grdSubjects_ctl' + sRowno + '_ChkBoxIsCoCurricularactivity')
            if (chkBoxIsDeleted.checked) {
                chkBoxIsCoCurricularactivity.disabled = false;
            }
            else {
                chkBoxIsCoCurricularactivity.disabled = true;
                chkBoxIsCoCurricularactivity.checked = false;
            }

              }

              function ValidateShortName(src, args) {
				var bResult = DuplicateTextValidation(document, _clientGridId, "txtShortName", "ChkBoxDelete", false, false);
              	args.IsValid = bResult;
              	if (!args.IsValid)
              	    src.errormessage = document.getElementById("<%=this.hidShortNameDuplicated.ClientID %>").value;
				return !args.IsValid;
}

function SelectAll(src) {
    $('#<%=grdSubjects.ClientID %> input:checkbox').attr('checked', src.checked);
    PageLoad();
}

    </script>
  </asp:content>
