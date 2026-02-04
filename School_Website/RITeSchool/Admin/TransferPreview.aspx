<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransferPreview.aspx.cs"
    Inherits="TransferPreview" MasterPageFile="../MasterPages/PopupMasterSml.master" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" id="divSrcClassteacher" runat="server">
            <tr>
                <td colspan="3" align="center" class="TblHGray" style="height: 20px">
                 <asp:Label ID="Label1" runat="server" CssClass="Lbl10ptB" Text="<%$ Resources:LocalizedResources, PreviewOfTheChanges %>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    &nbsp;</td>
                <td align="center">
                </td>
                <td align="center">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center" class="TblHGray" style="height: 20px">
                   <%-- <asp:Label ID="Label6" runat="server" CssClass="Lbl10ptB" ></asp:Label>--%>
                    <asp:Label ID="lblSrcTeacher" runat="server" CssClass="Lbl10ptB" EnableViewState="false" Text="<%$ Resources:LocalizedResources, ClassTeacherAssociationOf %>"></asp:Label></td>
                <td align="center">
                </td>
                <td align="center" class="TblHGray" style="height: 20px">
                    <%--<asp:Label ID="Label7" runat="server" CssClass="Lbl10ptB" ></asp:Label>--%>
                    <asp:Label ID="lblTargetTeacher" runat="server" CssClass="Lbl10ptB" EnableViewState="false" Text="<%$ Resources:LocalizedResources, ClassTeacherAssociationOf %>"></asp:Label></td>
            </tr>
            <tr>
                <td colspan="1" align="center" class="ClsBorderlight">
                    <asp:Label ID="lblClassteacherMsg" runat="server" CssClass="ClsLabel" EnableViewState="false"></asp:Label>
                </td>
                <td align="center" colspan="1">
                </td>
                <td colspan="1" align="center" class="ClsBorderlight">
                    <asp:Label ID="lblTargetClassteacherMsg" runat="server" CssClass="ClsLabel" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblSrc" runat="server" CssClass="Lbl10ptB" EnableViewState="false"></asp:Label>
                </td>
                <td align="center">
                </td>
                <td align="center">
                    <asp:Label ID="lblTarget" runat="server" CssClass="Lbl10ptB" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" class="TblHGray" style="height: 20px">
                    <asp:Label ID="lblSubjectHeader" runat="server" CssClass="Lbl10ptB" Text="<%$ Resources:LocalizedResources, ClassSubjectsAssociationOf %>"></asp:Label></td>
                <td align="center">
                </td>
                <td align="center" class="TblHGray" style="height: 20px">
                    <asp:Label ID="lblTargetSubjectHeader" runat="server" CssClass="Lbl10ptB" Text="<%$ Resources:LocalizedResources, ClassSubjectsAssociationOf %>"></asp:Label></td>
            </tr>
            <tr>
                <td align="center" class="ClsBorderlight" style="height: 20px">
                    <asp:Label ID="lblTransferSubjectMsg" runat="server" CssClass="ClsLabel" EnableViewState="false"></asp:Label></td>
                <td align="center">
                </td>
                <td align="center" class="ClsBorderlight" style="height: 20px">
                    <asp:Label ID="lblDeletedSubjectMsg" runat="server" CssClass="ClsLabel" EnableViewState="false"></asp:Label></td>
            </tr>
            <tr>
                <td align="center" style="width: 50%" class="ClsBorderlight" valign="top">
                    <div id="divSrcGrd" runat="server">
                        <asp:GridView ID="grdSrcTeacher" runat="server" AutoGenerateColumns="False" DataKeyNames="Teacher_Id,RowState,Subject_Id,Teacher_Subject_Id, CanTransfer"
                            OnRowDataBound="grdTeacher_Rowdatabound" Width="40%" CellSpacing="1" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="classSubjectName" HeaderText="<%$ Resources:LocalizedResources, Subjects %>" />
                            </Columns>
                            <RowStyle CssClass="TTransGridAltRow" />
                            <HeaderStyle CssClass="UsrGridHead" Font-Size="9pt" />
                            <AlternatingRowStyle CssClass="ClsMarksGridAltRowN" />
                            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                        </asp:GridView>
                    </div>
                </td>
                <td align="center" style="width: 3%">
                </td>
                <td align="center" style="width: 50%" class="ClsBorderlight" valign="top">
                    <div id="divTargetGrd" runat="server">
                        <asp:GridView ID="grdTargetTeacher" runat="server" AutoGenerateColumns="False" DataKeyNames="Teacher_Id,RowState,Subject_Id ,Teacher_Subject_Id, CanTransfer"
                            OnRowDataBound="grdTargetTeacher_Rowdatabound" Width="40%" CellSpacing="1" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="classSubjectName" HeaderText="<%$ Resources:LocalizedResources, Subjects %>" />
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
                <td colspan="3">
                    &nbsp;</td>
            </tr>
            <tr align="center">
                <td colspan="3" class="TblHGray">
                    <asp:Label ID="lblTTHeader" runat="server" CssClass="Lbl10ptB" Text="<%$ Resources:LocalizedResources, ModifiedTimetable %>" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="ClsBorderlight" colspan="3">
                    <table id="LegendTable">
                        <tr>
                            <td align="left" colspan="1">
                                <asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                    Text="<%$ Resources:LocalizedResources, Legend %>" EnableViewState="false"></asp:Label></td>
                            <td align="left" colspan="1" style="width: 15px; border: 1px solid #000" class="ClsHilightBG">
                                <img src="../images/spacer.gif" width="15px" height="15px" /></td>
                            <td align="left" colspan="1">
                                <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources, LectureTransferredFromSource %>"
                                    CssClass="ClsTextNormal" EnableViewState="false"></asp:Label></td>
                            <td align="right" style="width: 5px">
                            </td>
                            <td align="right" style="width: 20px; border: 1px solid #000; padding-right:5px;" class="TTNotClassTchr">
                                Off
                                <%--<img src="../images/spacer.gif" width="20px" height="20px"/>--%>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources, LectureTransferredToTarget %>"
                                    CssClass="ClsTextNormal" EnableViewState="false"></asp:Label>
                            </td>
                            <td align="left" style="width: 5px">
                            </td>
                            <td align="left" style="width: 20px; border: 1px solid #000" class="SubDeleted">
                                Lecture
                                <%--<img src="../images/spacer.gif" width="20px" height="20px"/>--%>
                            </td>
                            <td align="left" valign="middle">
                                <asp:Label ID="Label4" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources, LectureRemoved %>" CssClass="ClsTextNormal"
                                    EnableViewState="false"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblTTStatus" runat="server" CssClass="ClsHilightTextB" EnableViewState="false"
                        Text=""></asp:Label></td>
            </tr>
            <tr>
                <td colspan="3" class="ClsBorderlight">
                    <asp:Panel ID="pnlContainer" runat="server" Visible="true" Style="width: 955px;">
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <asp:Button ID="btnCancel" runat="server" OnClientClick="CloseWindow()" Text="<%$ Resources:LocalizedResources, Close %>"
                        CssClass="ClsBtn" TabIndex="6" CausesValidation="False" UseSubmitBehavior="false" />
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
        function CloseWindow() {
            window.parent.focus()
            window.close()
        }
        function fnover(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "maroon"
            objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)"
        }
        function fnout(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "#a3c07b"
            objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)"
        }
    </script>
</asp:Content>
