<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ClassTeacherXseedGradesUI.aspx.cs" Inherits="ClassTeacherXseedGradesUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%">
            <tr id="trPrecondition" runat="server">
                <td>
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table id="tblAssignGrades" runat="server" style="width: 100%;">
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
                                                        <span id="Span1" class="colonPadding"> :</span>
                                                        </span>&nbsp;
                                                    </td>
                                                    <td align="left" style="padding-right: 15px;">
                                                        <asp:DropDownList ID="cmbAssessment" runat="server" CssClass="LrgCombo" AutoPostBack="True"
                                                            OnSelectedIndexChanged="cmbAssessment_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="ClsBorderlight" runat="server" id="tdTeacher">
                                                        <asp:Label ID="lblTeacher" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                            Font-Bold="True" Text="<%$ Resources:LocalizedResources, SelectClassTeacher %>" EnableViewState="false"></asp:Label>
                                                            <span class="colonPadding"> :</span>
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
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="uPnl">
                                    <ContentTemplate>
                                        <asp:ListView ID="lstvwXseedStatus" runat="server" DataKeyNames="StandardDivisionID,SubjectId,EditStatus,IsXseedSubject,IsSubmitted"
                                            OnItemDataBound="lstvwXseedStatus_ItemDataBound" 
                                            OnItemCommand="lstvwXseedStatus_ItemCommand" 
                                            onitemediting="lstvwXseedStatus_ItemEditing">                                            
                                            <LayoutTemplate>
                                                <table align="center" width="65%" height="100%" runat="server" id="tblStudRemark"
                                                    style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="left" style="padding-left: 9px;">
                                                            <asp:Label ID="lblGradeText" runat="server" Text="<%$ Resources:LocalizedResources, Subject %>"></asp:Label>
                                                        </th>
                                                        <th align="center" width="100px">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, Edit %>"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="trSubjectGrade" runat="server" class="ClsGridRow">
                                                    <td align="left" class="paddingL" id="tdSubjectGrade" runat="server">
                                                        <asp:Label ID="lblStudentName" runat="server" Text='<%# Eval("SubjectName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="Edit"
                                                            ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="trSubjectGrade" runat="server" class="ClsGridAltRow">
                                                    <td align="left" class="paddingL" id="tdSubjectGrade" runat="server">
                                                        <asp:Label ID="lblStudentName" runat="server" Text='<%# Eval("SubjectName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="Edit"
                                                            ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        <asp:Label ID="lblNoRecordFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordsFound %>"></asp:Label>No record found.
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <asp:UpdatePanel ID="UpdtPnl1" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnPublish" runat="server" Text="<%$ Resources:LocalizedResources, Publish %>" Visible="true" Enabled="false"
                                CssClass="ClsBtn" UseSubmitBehavior="false" OnClick="btnPublish_Click" />
                            <asp:Button ID="btnUnPublish" runat="server" Width="90px" Text="<%$ Resources:LocalizedResources, Unpublish %>" Visible="true" CssClass="ClsBtn"
                                UseSubmitBehavior="false" Enabled="False" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="hidStandardDivisionId" runat="server" />
                    <asp:HiddenField ID="hidAlert" runat="server" />
                    <asp:HiddenField ID="hidMsgClassTecherXseed" runat="server" />
                    <asp:HiddenField ID="hidCultureInfo" runat="server" />
                </td>
            </tr>
        </table>
    </div>
     <script language="javascript" type="text/javascript">
         _clientbtnPublish = "<%=this.btnPublish.ClientID %>";   
         function ConfirmAction() {
             var bAction = true;

             var bResult = false;
             if (bAction) {
                 if (window.confirm(document.getElementById("<%=this.hidMsgClassTecherXseed.ClientID %>").value)) {
                     bResult = true;
                     document.getElementById(_clientbtnPublish).disabled = true;
                 }
                 else {
                     bResult = false;
                 }
             }
             return bResult;
         }    
 </script>
</asp:Content>
