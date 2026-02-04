<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExamScheduleInstructionsPopUp.aspx.cs" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master" Inherits="StandardwiseExamSchedulePopup" ValidateRequest="false" %>
<%@ OutputCache Location="None" VaryByParam="none" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv" style="vertical-align: top">
        <table width="100%"  cellpadding="2" style="vertical-align: top">
            <tr>
                <td align="left" colspan="2" rowspan="1" style="height: 20px">
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td style="height: 20px">
                                    <span class="MainTitleHead" style="font-weight:bold">Exam Instructions</span> </td>
                        </tr>
                    </table>
                </td>
            </tr>
			<tr>
				<td align="left">
					<asp:ValidationSummary ID="valSumErrorMsg" CssClass="ClsLabel" runat="server" />
				</td>
			</tr>
            <tr>
                <td valign="top">
                    <table style="width: 100%; height: 30%">
                        <tr>
                            <td align="right" valign="middle">
                                    <span class="LblNrmlB">Instructions :</span>
                            </td>
                            <td align="left" >
                                <asp:TextBox ID="txtInstructions" runat="server" CssClass="SmlCombo" Rows="4" TextMode="MultiLine" MaxLength="500"
                                    Width="450px">
                                </asp:TextBox>
								<asp:RegularExpressionValidator ID="cst_Remark" runat="server" Display="None" ControlToValidate="txtInstructions"
                                        ErrorMessage="Instructions should be less than 500 characters." ValidationExpression="^[\s\S]{0,500}$">
                                </asp:RegularExpressionValidator>
                                <asp:HiddenField ID="hidStandardwiseExamScheduleId" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="top" colspan="2">
                                <asp:Button ID="btnSave" runat="server" Text="Submit" CssClass="ClsBtn" OnClick="btnSave_Click" />&nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" />
                            </td>
                        </tr>
                        </table>
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
		function closewindow() {
            window.close();
        }
    </script>
</asp:Content>
