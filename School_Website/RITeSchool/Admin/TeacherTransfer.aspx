<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="TeacherTransfer.aspx.cs" EnableEventValidation="false" Inherits="TeacherTransferUI" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <style>
        .ClsGridAltRow > td, .ClsGridRow > td {
            border-top:0 solid #bedcde;
        }
    </style>
    <div class="MainBodyDiv">
        <table id="tblValidation" runat="server" width="95%">
            <tr align="left">
                <td align="left">
                    <asp:ValidationSummary ID="valSumErrorMsg" ValidationGroup="show" runat="server"
                        CssClass="ClsLabel" />
                    <asp:CompareValidator ID="CompareValidator3" runat="server" ValidationGroup="show"
                        ControlToValidate="cmbSrcTeacher" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, valSourceTeacher %>"
                        Operator="NotEqual" ValueToCompare="-1" CssClass="ClsLabel"></asp:CompareValidator>
                    <asp:CompareValidator ID="CompareValidator4" ValidationGroup="show" runat="server"
                        ControlToValidate="cmbTargetTeacher" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ValTargetTeacher %>"
                        Operator="NotEqual" ValueToCompare="0" CssClass="ClsLabel"></asp:CompareValidator>
                    <asp:CompareValidator ID="CompareValidator5" runat="server" ValidationGroup="show"
                        ControlToValidate="cmbTargetTeacher" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ValSourceAndTarget %>"
                        Operator="NotEqual" ControlToCompare="cmbSrcTeacher" CssClass="ClsLabel"></asp:CompareValidator>
                </td>
            </tr>
        </table>
        <table width="95%">
            <tr id="trdivErr" runat="server" visible="false">
                <td style="width: 100%">
                    <div id="divErr" runat="server"></div>
                </td>
            </tr>
            <tr id="trButtons" runat="server" visible="false">
				<td align="center" style="width: 100%;">
					<asp:Button ID="bnt_Back"
								runat="server"
								CssClass="ClsBtnMid"
								Height="24px"
								Text="<%$ Resources:LocalizedResources, Back %>"
								CausesValidation="false"
								OnClick="bnt_Back_Click" />
								
				</td>
            </tr>
            <tr id="trTeachers" runat="server" align="left">
                <td align="left">
                    <asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server"
                        ID="UpdatePanel3">
                        <ContentTemplate>
                            <table runat="server" id="tblInputFields" cellpadding="0" cellspacing="2" width="90%">
                                <tr id="Tr1" runat="Server">
                                    <td align="center" colspan="1" rowspan="6" style="width: 0px">
                                    </td>
                                    <td align="center" colspan="5">
                                        <asp:UpdatePanel UpdateMode="Always" runat="server" ID="UpdatePanel9">
                                            <ContentTemplate>
                                                <asp:Label ID="lblStatus" runat="server" CssClass="ClsHilightTextB" EnableViewState="false"
                                                    Text=""></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr id="Tr2" runat="Server">
                                    <td class="ClsBorderlight" style="width: 35%">
                                        <asp:Label ID="Label3" CssClass="clsLabel" Text="<%$ Resources:LocalizedResources, SourceTeacher %>" runat="server"></asp:Label>
                                        <span class=" colonPadding"> :</span>
                                    </td>
                                    <td class="HilightBGGray Clspadding" align="center">
                                        <asp:Label ID="Label6" runat="server" CssClass="ClsHilightText" Font-Bold="false"><img src="../images/ArrowBlueDblNw.gif" /><span class="ClsHilightTextB"></span><img src="../images/ArrowBlueDblNw.gif" /> </asp:Label>
                                    </td>
                                    <td class="ClsBorderlight" style="width: 35%">
                                        <asp:Label ID="Label4" CssClass="clsLabel" Text="<%$ Resources:LocalizedResources, TargetTeacher %>" runat="server"></asp:Label>
                                        <span class=" colonPadding"> :</span>
                                    </td>
                                    <td>
                                        <span class="ClsMdtStar">* 
                                        <asp:Label ID="lblManatoryText" Text="<%$ Resources:LocalizedResources, MandatoryFields %>" runat="server"></asp:Label>
                                        </span>
                                    </td>
                                </tr>
                                <tr id="Tr7" runat="Server">
                                    <td class="ClsBorderlight">
                                        <asp:DropDownList ID="cmbSrcTeacher" runat="server" CssClass="LrgCombo" Width="212px"
                                            TabIndex="1">
                                        </asp:DropDownList>
                                        <span style="font-size: 9pt; color: #ff0000">*</span>
                                    </td>
                                    <td class="ClsBorderlight" align="center" style="width: 100px">
                                        &nbsp;
                                    </td>
                                    <td class="ClsBorderlight">
                                        <asp:DropDownList ID="cmbTargetTeacher" runat="server" CssClass="LrgCombo" Width="212px"
                                            TabIndex="2">
                                        </asp:DropDownList>
                                        <span style="font-size: 9pt; color: #ff0000">*</span>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnShow" runat="server" Text="<%$ Resources:LocalizedResources, Show %>" CssClass="ClsBtnMid" Height="24px"
                                            CausesValidation="true" ValidationGroup="show" TabIndex="3" OnClick="btnShow_Click" />
                                    </td>
                                </tr>
                                <tr runat="Server">
                                    <td align="center">
                                        <asp:Image ID="imgArrow" runat="server" ImageUrl="~/RITeSchool/images/ArrowRedDwn.gif" />
                                    </td>
                                    <td align="center" style="width: 100px">
                                    </td>
                                    <td align="center">
                                        <asp:Image ID="imgArrow2" runat="server" ImageUrl="~/RITeSchool/images/ArrowRedDwn.gif" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr id="Tr4" runat="Server">
                                    <td align="center" colspan="4">
                                    </td>
                                </tr>
                                <tr runat="Server" id="Tr6">
                                    <td align="center" colspan="4">
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="trTransfer" runat="server" align="left">
                <td align="left">
                    <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                        ID="UpdatePanel1">
                        <ContentTemplate>
                            <table runat="server" id="Table1" cellpadding="0" cellspacing="2" width="90%">
                                <tr runat="server" id="trClassHeader">
                                    <td align="center" class="TblHGray" style="height: 20px; width: 49%">
                                        <asp:Label ID="lblClassHeader" runat="server" CssClass="Lbl10ptB" Text="<%$ Resources:LocalizedResources, SourceTeacherClass %>"></asp:Label>
                                    </td>
                                    <td align="center" style="height: 20px; width: 2%">
                                    </td>
                                    <td align="center" class="TblHGray" style="height: 20px; width: 49%">
                                        <asp:Label ID="Label1" runat="server" CssClass="Lbl10ptB" Text="<%$ Resources:LocalizedResources, TargetTeacherClass %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 49%">
                                        <table align="center" id="divSrcClassteacher" runat="server" width="100%">
                                            <tr>
                                                <td colspan="2" class="ClsBorderlight " align="center">
                                                    <asp:UpdatePanel ChildrenAsTriggers="false" RenderMode="Block" EnableViewState="false"
                                                        UpdateMode="Conditional" runat="server" ID="UpdatePanel5">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblMsg" runat="server" CssClass="ClsLabel" Text="" EnableViewState="true"></asp:Label>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkTransfer" EventName="CheckedChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="chkRemove" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <asp:Label ID="lblSrc" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, Class %>" EnableViewState="false"></asp:Label>
                                                    <span class=" colonPadding"> :</span>
                                                </td>
                                                <td class="ClsBorderlight">
                                                    <asp:CheckBox ID="chkTransfer" runat="server" Text="<%$ Resources:LocalizedResources, Transfer %>" AutoPostBack="true"
                                                        CssClass="ClsLabel" OnCheckedChanged="chkTransfer_CheckChanged" TabIndex="4" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 2%">
                                    </td>
                                    <td style="width: 49%">
                                        <table align="center" id="divTargetClassteacher" runat="server" width="100%">
                                            <tr>
                                                <td colspan="2" class="ClsBorderlight" align="center">
                                                    <asp:UpdatePanel ChildrenAsTriggers="false" RenderMode="Block" EnableViewState="false"
                                                        UpdateMode="Conditional" runat="server" ID="UpdatePanel8">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblTargetMsg" runat="server" CssClass="ClsLabel" Text=""></asp:Label>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkTransfer" EventName="CheckedChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="chkRemove" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <asp:Label ID="lblTarget" runat="server" CssClass="ClsLabel" Text="" EnableViewState="false"></asp:Label>
                                                </td>
                                                <td class="ClsBorderlight">
                                                    <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                                        ID="UpdatePanel4">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkRemove" runat="server" Text="<%$ Resources:LocalizedResources, Remove %>" AutoPostBack="true" CssClass="ClsLabel"
                                                                OnCheckedChanged="chkRemove_CheckedChanged" TabIndex="5" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkTransfer" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 20px; width: 49%">
                                    </td>
                                    <td style="height: 20px; width: 2%">
                                    </td>
                                    <td style="height: 20px; width: 49%">
                                    </td>
                                </tr>
                                <tr runat="server" id="trDisabled">
                                    <td class="ClsBorderlight" style="width: 49%">
                                        <asp:Label ID="lblDisabledMsg" runat="server" CssClass="ClsLabel" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" id="trSubjectHeader">
                                    <td align="center" class="TblHGray" style="height: 20px; width: 49%">
                                        <asp:Label ID="lblSubjectHeader" runat="server" CssClass="Lbl10ptB" Text="<%$ Resources:LocalizedResources, ClassSubjectsSourceTeacher %>"></asp:Label>
                                    </td>
                                    <td align="center" style="height: 20px; width: 2%">
                                    </td>
                                    <td align="center" class="TblHGray" style="height: 20px; width: 49%">
                                        <asp:Label ID="Label5" runat="server" CssClass="Lbl10ptB" Text="<%$ Resources:LocalizedResources, ClassSubjectsTargetTeacher %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" id="trMsgs">
                                    <td class="ClsBorderlight" style="width: 49%">
                                        <asp:Label ID="lblSubjectMsg" runat="server" CssClass="ClsLabel" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 2%">
                                    </td>
                                    <td class="ClsBorderlight" style="width: 49%">
                                        <asp:Label ID="lblTargetSubjectMsg" runat="server" CssClass="ClsLabel" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" id="trSubjectGrd" width="100%">
                                    <td align="center" valign="top" class="ClsMarksGridHeader" style="padding: 0; width: 49%">
                                        <div id="divSrcGrd" runat="server" class="ClsBorderlight">
                                            <asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server"
                                                ID="UpdatePanel10">
                                                <ContentTemplate>
                                                    <asp:GridView ID="grdSrcTeacher" runat="server" AutoGenerateColumns="False" DataKeyNames="Teacher_Id,RowState,Subject_Id,Teacher_Subject_Id, CanTransfer"
                                                        OnRowDataBound="grdTeacher_Rowdatabound" GridLines="none" CellSpacing="1">
                                                        <Columns>
                                                            <asp:BoundField DataField="classSubjectName" HeaderText="<%$ Resources:LocalizedResources, Subject %>" 
                                                                ItemStyle-CssClass="ClspaddingL" >
                                                                <ItemStyle CssClass="ClspaddingL" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, Transfer %>">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkTeacherSubject" runat="server" TabIndex="12" AutoPostBack="true" OnCheckedChanged="chkTeacherSubject_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="1%" HorizontalAlign="Center" CssClass="ClspaddingL" />
                                                                <HeaderStyle Width="1%" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <RowStyle CssClass="TTransGridAltRow" />
                                                        <HeaderStyle CssClass="UsrGridHead" Font-Size="9pt" />
                                                        <AlternatingRowStyle CssClass="ClsMarksGridAltRowN" />
                                                        <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                    <td align="center" valign="top" style="width: 2%">
                                    </td>
                                    <td align="center" valign="top" class="ClsGridBG" style="padding: 0; width: 49%">
                                        <div id="divTargetGrd" runat="server">
                                            <asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server"
                                                ID="UpdatePanel11">
                                                <ContentTemplate>
                                                    <asp:GridView ID="grdTargetTeacher" runat="server" AutoGenerateColumns="False" DataKeyNames="Teacher_Id,RowState,Subject_Id ,Teacher_Subject_Id, CanTransfer"
                                                        OnRowDataBound="grdTargetTeacher_Rowdatabound" GridLines="none" 
                                                        CellSpacing="1">
                                                        <Columns>
                                                            <asp:BoundField DataField="classSubjectName" HeaderText="<%$ Resources:LocalizedResources, Subject %>"
                                                                ItemStyle-CssClass="ClspaddingL" >
                                                                <ItemStyle CssClass="ClspaddingL" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, Remove %>">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkTeacherSubject" runat="server" TabIndex="13" AutoPostBack="true" OnCheckedChanged="chkTargetTeacherSubject_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="1%" HorizontalAlign="Center" CssClass="ClspaddingL" />
                                                                <HeaderStyle Width="1%" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <RowStyle CssClass="ClsGridRow" />
                                                        <HeaderStyle CssClass="ClsGridHeader" />
                                                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                        <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 20px; width: 49%">
                                    </td>
                                    <td style="height: 20px; width: 2%">
                                    </td>
                                    <td style="height: 20px; width: 49%">
                                    </td>
                                </tr>
                                <tr style="width: 100%">
                                    <td style="width: 49%">
                                        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                            ID="UpdatePanel6">
                                            <ContentTemplate>
                                                <table width="100%" runat="server" id="tblSrcAssembly">
                                                    <tr>
                                                        <td width="50%" ID="tdSrcAssembly" runat="server" class="ClsBorderlight" colspan="2">
                                                            <asp:CheckBox ID="chkSrcAssembly" runat="server" Text="<%$ Resources:LocalizedResources, IsAssemblyApplicable %>" AutoPostBack="true"
                                                                CssClass="ClsLabel" OnCheckedChanged="chkSrcAssembly_CheckedChanged" 
                                                                TabIndex="6" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="50%" ID="tdSrcMPT" runat="server" class="ClsBorderlight" colspan="2">
                                                            <asp:CheckBox ID="chkSrcMPT" runat="server" Text="<%$ Resources:LocalizedResources, IsMPTApplicable %>" AutoPostBack="true"
                                                                CssClass="ClsLabel" OnCheckedChanged="chkSrcMPT_CheckedChanged" 
                                                                TabIndex="7" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="50%" ID="tdSrcStayBack" runat="server"  class="ClsBorderlight" colspan="2">
                                                            <asp:CheckBox ID="chkSrcStayBack" runat="server" Text="<%$ Resources:LocalizedResources, IsStaybackApplicable %>" AutoPostBack="true"
                                                                CssClass="ClsLabel" OnCheckedChanged="chkSrcStayBack_CheckedChanged" 
                                                                TabIndex="8" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td style="height: 20px; width: 2%">
                                    </td>
                                    <td style="width: 49%">
                                        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                            ID="UpdatePanel7">
                                            <ContentTemplate>
                                                <table width="100%" runat="server" id="tblTrgtAssembly">
                                                    <tr>
                                                        <td width="50%" ID="tdTrgtAssembly" runat="server" class="ClsBorderlight" colspan="2">
                                                            <asp:CheckBox ID="chkTrgtAssembly" runat="server" Text="<%$ Resources:LocalizedResources, IsAssemblyApplicable %>"
                                                                AutoPostBack="true" CssClass="ClsLabel" 
                                                                OnCheckedChanged="chkTrgtAssembly_CheckedChanged" TabIndex="9" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="50%" ID="tdTrgtMPT" runat="server" class="ClsBorderlight" colspan="2">
                                                            <asp:CheckBox ID="chkTrgtMPT" runat="server" Text="<%$ Resources:LocalizedResources, IsMPTApplicable %>" AutoPostBack="true"
                                                                CssClass="ClsLabel" OnCheckedChanged="chkTrgtMPT_CheckedChanged" 
                                                                TabIndex="10" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="50%" ID="tdTrgtStayBack" runat="server" class="ClsBorderlight" colspan="2">
                                                            <asp:CheckBox ID="chkTrgtStayBack" runat="server" Text="<%$ Resources:LocalizedResources, IsStaybackApplicable %>"
                                                                AutoPostBack="true" CssClass="ClsLabel" 
                                                                OnCheckedChanged="chkTrgtStayBack_CheckedChanged" TabIndex="11" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td class="ClsBorderlight" ID="tdBrderLine" runat="server" colspan="3" style="width: 100%">
                                    </td>
                                </tr>
                                <tr align="left" runat="server" id="trAddNote" style="width: 100%">
                                    <td colspan="3" style="width: 100%">
                                        <table>
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label11" runat="server" BorderWidth="0px" Font-Bold="True" Text="<%$ Resources:LocalizedResources, Note1 %>"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                        <span> :</span>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="2" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="Label21" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="<%$ Resources:LocalizedResources, NoteTeacherTransfer %>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label12" runat="server" BorderWidth="0px" Font-Bold="True" Text="<%$ Resources:LocalizedResources, Note2 %>"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                        <span> :</span>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="2" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="Label13" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="<%$ Resources:LocalizedResources, NoteTeacherTransfer1 %>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label9" runat="server" BorderWidth="0px" Font-Bold="True" Text="<%$ Resources:LocalizedResources, Note3 %>"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                        <span> :</span>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="2" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="Label10" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="<%$ Resources:LocalizedResources, NoteTeacherTransfer2 %>"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr align="center" runat="server" id="trTT">
                                    <td colspan="3" class="TblHGray" style="width: 100%">
                                        <asp:Label ID="lblTTHeader" runat="server" CssClass="Lbl10ptB" Text="<%$ Resources:LocalizedResources, Timetable %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trLegendTable" runat="server">
                                    <td class="ClsBorderlight" colspan="3" style="width: 100%">
                                        <table>
                                            <tr>
                                                <td align="left" colspan="1">
                                                    <asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                                        Text="<%$ Resources:LocalizedResources, Legend %>" EnableViewState="false"></asp:Label>
                                                </td>
                                                <td align="left" colspan="1" style="width: 15px; border: 1px solid #000" class="SubUpdate">
                                                    <img src="../images/spacer.gif" width="15px" height="13px" />
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources, LectureTransferred %>"
                                                        CssClass="ClsTextNormal" EnableViewState="false"></asp:Label>
                                                </td>
                                                <td align="right" style="width: 5px">
                                                </td>
                                                <td align="right" style="width: 15px; border: 1px solid #000" class="SubUpdateDel">
                                                    <img src="../images/spacer.gif" width="15px" height="13px" />
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="Label7" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources, LectureRemovedTransferred %>"
                                                        CssClass="ClsTextNormal" EnableViewState="false"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 5px">
                                                </td>
                                                <td align="left" style="width: 15px; border: 1px solid #000" class="SubDeleted">
                                                    <img src="../images/spacer.gif" width="15px" height="13px" />
                                                </td>
                                                <td align="left" valign="middle">
                                                    <asp:Label ID="Label8" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources, LectureRemoved %>" CssClass="ClsTextNormal"
                                                        EnableViewState="false"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td colspan="3" valign="top" style="width: 100%">
                                        <asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Always" runat="server" ID="UpdatePanel2">
                                            <ContentTemplate>
                                                <div runat="server" id="divTTmsg" visible="false">
                                                    <asp:Label ID="lblTTStatus" runat="server" CssClass="ClsHilightTextB" EnableViewState="False"></asp:Label>
                                                </div>
                                                <div id="divTT" runat="server" style="vertical-align: top">
                                                    <asp:Panel ID="pnlContainer" runat="server" Visible="true" Style="width: 800px;">
                                                    </asp:Panel>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>                                
                                <tr>
                                    <td colspan="3" style="height: 52px; width: 100%">
                                        <div runat="server" id="divBtn">
                                            <table width="100%">
                                                <tr>
                                                    <td align="center" colspan="3">
                                                        <asp:Button UseSubmitBehavior="false" ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back %>" CssClass="ClsBtn"
                                                            Height="24px" CausesValidation="False" OnClick="btnBack_Click" 
                                                            TabIndex="15" />
                                                        <asp:Button ID="btnPreVw" OnClientClick="openPreview()" runat="server" Text="<%$ Resources:LocalizedResources, Preview %>"
                                                            CssClass="ClsBtn" Height="24px" CausesValidation="False" 
                                                            OnClick="btnPrevw_Click" TabIndex="16" />
                                                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>" CssClass="ClsBtn" Height="24px"
                                                            CausesValidation="true" TabIndex="17" OnClick="btnSave_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                    &nbsp;
                </td>
            </tr>
        </table>
        <table width="95%">
            <tr>
                <td>
                    <asp:HiddenField ID="hidIsTTConfig" runat="server" />
                    <asp:HiddenField ID="hidIsDisable" runat="server" />
                    <asp:HiddenField ID="hidbtnShow" runat="server" />
                    <asp:HiddenField ID="hidCultureInfo" runat="server" />
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">
        var iStRow = 2
        var sId = "ctl00_MainBody_tbl_TTS"
        var sStyleDeleted = ''
        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)
        prm.add_beginRequest(beginRequestHandler)
        var iScroll
        var sId = "ctl00_MainBody_divLectures"
        function EndReqHandler(sender, args) {
            if (document.getElementById(sId)) {
                document.getElementById(sId).scrollLeft = iScroll
            } 
        }
        function beginRequestHandler(sender, args) {
            if (document.getElementById(sId)) {
                iScroll = document.getElementById(sId).scrollLeft
            } 
        }
        function openPreview() {
            window.open('TransferPreview.aspx', '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=1700,height=620')
        }
    </script>
</asp:Content>
