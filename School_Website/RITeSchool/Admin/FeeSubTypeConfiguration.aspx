<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FeeSubTypeConfiguration.aspx.cs" Inherits="FeeSubTypeConfiguration" MasterPageFile="../MasterPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
    	<style type="text/css">
	    	.radiobtnlist td {
	    		padding: 0 3px;
	    	}
	    </style>
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr>
                <td valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td valign="top">
                                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ShowMessageBox="True" ShowSummary="False"
                                    CssClass="LblErrorMsg" ForeColor="" />
                                <asp:CustomValidator ID="cstValFeeSubType" runat="server" ClientValidationFunction="CstDuplicateTextValidation"
                                    ErrorMessage= "<%$ Resources:LocalizedResources, valFeeSubType%>" Display="None" CssClass="LblErrorMsg"></asp:CustomValidator>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
             <tr align="center">
                <td>
                    <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg"  EnableViewState="false" style="margin: 10px 0;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                	<asp:GridView ID="grdFeeSubTypeConfiguration" runat="server" Width="400px" CellPadding="0" class="GridBorder"
                	              CellSpacing="1" AutoGenerateColumns="False" AllowPaging="false"
                	              ForeColor="#333333" GridLines="None" OnRowDataBound="grdFeeSubTypeConfiguration_RowDataBound"
                	              DataKeyNames="Fee_SubType_Id,School_Id,Original_Fee_SubType_Id,Fee_SubType">
                		<PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Underline="False" Font-Names="Arial"
                		            Font-Size="Small"></PagerStyle>
                		<PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                		               FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                		<Columns>
                			<asp:TemplateField>
                				<HeaderTemplate>
									<input id="chkSelectAll" runat="server" type="checkbox" onclick="CheckAllCheckBox(this);"  />
                				</HeaderTemplate>
                				<ItemTemplate>
                					<asp:CheckBox ID="chkSelect" runat="server"/>
                				</ItemTemplate>
                				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                				<HeaderStyle Width="40px" HorizontalAlign="Center" VerticalAlign="Middle" />
                			</asp:TemplateField>
                			<asp:TemplateField HeaderText= "<%$ Resources:LocalizedResources, FeeSubTypeName%>">
                				<ItemTemplate>
                					&nbsp;<asp:RequiredFieldValidator ID="RFVtxtFeeSubType" runat="server" Display="None"
                					                                  ErrorMessage= "<%$ Resources:LocalizedResources, valEnterFeeSubType%>" ControlToValidate="txtFeeSubType"></asp:RequiredFieldValidator>
                					<asp:TextBox ID="txtFeeSubType" CssClass="LrgTxtBox" runat="server" MaxLength="50" Width="360px"></asp:TextBox>
                				</ItemTemplate>
								<ItemStyle HorizontalAlign="Left" />
                				<HeaderStyle Width="470px" HorizontalAlign="Left" CssClass="paddingLSML" VerticalAlign="Middle" />
                			</asp:TemplateField>                			
                		</Columns>
                		<RowStyle CssClass="ClsGridRow" />
                		<HeaderStyle CssClass="ClsGridHeader" />
                		<AlternatingRowStyle CssClass="ClsGridAltRow" />
                		<EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                	</asp:GridView>
                </td>
            </tr>
            <tr>
                <td align="center">                
                    <asp:Button ID="btnSave" runat="server" Text= "<%$ Resources:LocalizedResources, Save%>" OnClick="btnSave_Click" CssClass="ClsBtn" UseSubmitBehavior="false" style="margin-top: 10px;" />
                    <asp:Button ID="btnCancel" runat="server" Text= "<%$ Resources:LocalizedResources, Cancel%>" CssClass="ClsBtn" CausesValidation="False" UseSubmitBehavior="false" />
				</td>
            </tr>
        </table>
        <asp:HiddenField ID="hidConfigurationFlag" runat="server" />
        <asp:HiddenField ID = "hidCultureInfo" runat = "server" />

        <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.grdFeeSubTypeConfiguration.ClientID %>";
        _clientbtnSave = "<%=this.btnSave.ClientID %>";
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>";        
            
        function ConfirmAction(iPageCount, sActionName) {
            var bResult = true;
        	
            if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientGridId, 'chkSelect', sActionName, 'false', iPageCount, 'true')) {
                bResult = true;
                if (typeof (Page_ClientValidate) == 'function')
                    bResult = Page_ClientValidate();
                
                if (bResult)
                    DisableButtons();
            }
            else
                bResult = false;
            return bResult;
        }
  
        function DisableButtons() {
            $get(_clientbtnSave).disabled = true;
            $get(_clientbtnCancel).disabled = true;
        }
 
        function CstDuplicateTextValidation(oSrc, args) {
            args.IsValid = DuplicateTextValidation(document, _clientGridId, "txtFeeSubType", "chkSelect", false);
            return !args.IsValid;
        }	

		function CheckAllCheckBox(chk) {
		    $('#<%=grdFeeSubTypeConfiguration.ClientID %> input:checkbox').attr('checked', chk.checked);

		    var grid = $get(_clientGridId);
		    var icount = grid.rows.length;
		    for (var i = 0; i < icount - 1; i++)
		        ChkOnChange(i);
		}

		function ChkOnChange(iRowCount) {

		    iRowCount = iRowCount + 2;
		    var cntrl;
		    var txt;
		    if (iRowCount < 10)
		        cntrl = "_ctl0";
		    else
		        cntrl = "_ctl";

		    var checkbox = $get(_clientGridId + cntrl + iRowCount + '_chkSelect');
		    txt = $get(_clientGridId + cntrl + iRowCount + '_txtFeeSubType');
		    if (checkbox != null && checkbox.checked)
		        txt.disabled = false;
		    else
		        txt.disabled = true;
		}

		// Determines if all the checkboxes (excluding header checkbox) are checked.
		// Returns true if they are, false otherwise.
		function AllChecked(src) {
			var chkTotalCount = $('input[type=checkbox][id$=chkSelect]:not(:disabled)', src).length;
			var chkSelectedCount = $('input[type=checkbox][id$=chkSelect]:checked', src).length;
			return chkTotalCount == chkSelectedCount;
		}
        </script>
    </div>
</asp:Content>
