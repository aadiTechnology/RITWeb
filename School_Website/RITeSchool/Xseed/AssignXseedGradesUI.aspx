<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="AssignXseedGradesUI.aspx.cs" Inherits="AssignXseedGradesUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%">
            <tr>
                <td align="center">
                    <table id="tblAssignXseedGrades" runat="server" style="width: 100%;">
                        <tr>
                            <td align="center">
                                <table id="LegendTable" runat="server" align="center" cellpadding="0" cellspacing="1">
                                    <tr>
                                        <td align="left" colspan="1" rowspan="3" style="padding-right: 5px">
                                            <asp:Label ID="lblLegend" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                EnableViewState="false" Font-Bold="True" Text="<%$ Resources:LocalizedResources, Legend %>"></asp:Label>
                                        </td>                                       
                                    </tr>
                                    <tr>                                       
                                        <td align="left" colspan="1">
                                            &nbsp;<asp:Image ID="Image1" runat="server" EnableViewState="False" ImageUrl="~/RITeSchool/images/icoGrid_MarkEntryNotStart.gif" />
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:Label ID="Label4" runat="server" CssClass="ClsTextNormal" EnableViewState="false"
                                                Font-Bold="True" Text="<%$ Resources:LocalizedResources, MarksEntryNotStarted %>"></asp:Label>
                                        </td>
                                        <td align="left" colspan="1">
                                            &nbsp;<asp:Image ID="Image2" runat="server" EnableViewState="False" ImageUrl="~/RITeSchool/images/icoGrid_MarkEntryPartDone.gif" />
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:Label ID="Label8" runat="server" CssClass="ClsTextNormal" EnableViewState="false"
                                                Font-Bold="True" Text="<%$ Resources:LocalizedResources, MarksEntryPartiallyDone %>"></asp:Label>
                                        </td>
                                        <td align="left" colspan="1">
                                            &nbsp;
                                            <asp:Image ID="Image3" CssClass="img-align-unset" runat="server" EnableViewState="False" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:Label ID="Label9" runat="server" CssClass="ClsTextNormal" EnableViewState="False"
                                                Font-Bold="True" Text="<%$ Resources:LocalizedResources, MarksEntryCompleted %>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td rowspan="1">
                                            <asp:Image ID="Image4" runat="server" ImageUrl="~/RITeSchool/images/icoGrid_SubmitExamMarks.gif" />
                                        </td>
                                        <td align="left" colspan="3" style="padding-left: 5px">
                                            <asp:Label ID="Label5" runat="server" CssClass="ClsTextNormal" EnableViewState="false"
                                                Font-Bold="True" Text="<%$ Resources:LocalizedResources, MsgSubmitExamMarks %>"></asp:Label>
                                        </td>
                                        <td align="left" colspan="1">
                                        </td>
                                        <td align="left" colspan="1">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Panel ID="pnlFields" runat="server" Width="100%">
                                    <tr>
                                        <td align="center" colspan="2" valign="bottom">
                                            <table id="Table1" runat="server">
                                                <tr>
                                                    <td align="left" class="ClsBorderlight">
                                                        <span class="ClsLblLgnd" style="font-weight: bold">
                                                        <asp:Label ID="lblGradeText" runat="server" Text="<%$ Resources:LocalizedResources, Assessment %>"></asp:Label>
                                                        <span class="ClsLblLgnd colonPadding"></span></span>&nbsp;
                                                    </td>
                                                    <td align="left" style="padding-right: 15px;">
                                                        <asp:DropDownList ID="cmbAssessment" runat="server" CssClass="LrgCombo" AutoPostBack="True"
                                                            OnSelectedIndexChanged="cmbAssessment_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="ClsBorderlight" runat="server" id="tdTeacher">
                                                        <asp:Label ID="lblTeacher" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                            Font-Bold="True" Text="<%$ Resources:LocalizedResources, SelectSubjectTeacher %>" EnableViewState="false"></asp:Label>
                                                            <span class="ClsLblLgnd colonPadding"></span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="cmbTeachers" AutoPostBack="true" runat="server" CssClass="LrgCombo"
                                                            OnSelectedIndexChanged="cmbTeachers_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr id="trXseedSubjects" runat="server">
                            <td>
                                <asp:UpdatePanel runat="server" ID="uPnl">
                                    <ContentTemplate>
                                        <asp:ListView ID="lstvwXseedSubjects" runat="server" OnItemDataBound="lstvwXseedSubjects_ItemDataBound"
                                            DataKeyNames="StandardDivisionID,SubjectId,EditStatus,SubmitStatus,IsXseedSubject,IsSubmitted"
                                            OnItemCommand="lstvwXseedSubjects_ItemCommand" OnItemEditing="lstvwXseedSubjects_ItemEditing">
                                            <LayoutTemplate>
                                                <table align="center" width="70%" height="100%" runat="server" id="tblStudRemark"
                                                    style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="left" width="14%" style="padding-left: 9px;">
                                                            <asp:Label ID="lblGradeText" runat="server" Text="<%$ Resources:LocalizedResources, Class %>"></asp:Label>
                                                        </th>
                                                        <th align="left" width="40%" style="padding-left: 9px;">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, Subject %>"></asp:Label>
                                                        </th>
                                                        <th align="center">
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, Edit %>"></asp:Label>
                                                        </th>
                                                        <th align="center">
                                                           <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, Submit %>"></asp:Label> 
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblRollNum" runat="server" Text='<%# Eval("Class") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblStudentName" runat="server" Text='<%# Eval("SubjectName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="Edit" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnSubmit" runat="server" CommandName="SUBMIT" CommandArgument="0" />
                                                        <asp:Label ID="lblStatus" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblRollNum" runat="server" Text='<%# Eval("Class") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblStudentName" runat="server" Text='<%# Eval("SubjectName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="Edit" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnSubmit" runat="server" CommandName="SUBMIT" CommandArgument="0" />
                                                        <asp:Label ID="lblStatus" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>                                                        
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divErr" runat="server">
                    </div>
                </td>
            </tr>
        </table>
        <asp:HiddenField runat="server" ID="hidGradesnotenteredfor" />
        <asp:HiddenField ID="hidCultureInfo" runat="server" />
        <asp:HiddenField runat="server" ID="hidAreYouSureYouWantToContinue" />
        <asp:HiddenField runat="server" ID="hidRollNos" />
        <asp:HiddenField runat="server" ID="hidValGradeSumbit" />
    </div>

    <script language="javascript" type="text/javascript">

    	function ConfirmSubmitAction(IncompleteRollNoString) {
    		var bResult = true;
    		var sIncompleteAlert = document.getElementById("<%=this.hidRollNos.ClientID %>").value + " " + document.getElementById("<%=this.hidGradesnotenteredfor.ClientID %>").value +  " : \n" + IncompleteRollNoString + " \n" + document.getElementById("<%=this.hidAreYouSureYouWantToContinue.ClientID %>").value;

    		if (IncompleteRollNoString != '') {
    			if (confirm(sIncompleteAlert)) {
    			    if (!window.confirm(document.getElementById("<%=this.hidValGradeSumbit.ClientID %>").value))
    					bResult = false;
    			}
    			else
    				bResult = false;			
    		}
    		else {
    		    if (!window.confirm(document.getElementById("<%=this.hidValGradeSumbit.ClientID %>").value))
    				bResult = false;
			}
			return bResult;
        }     
    
    </script>

</asp:Content>
