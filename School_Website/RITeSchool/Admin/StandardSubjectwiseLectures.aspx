<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardSubjectwiseLectures.aspx.cs"
    Inherits="StdSubWiseLectures" MasterPageFile="../MasterPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">

    <div class="MainBodyDiv">
        <table style="width: 97%"  align="center">
            <tr align="center">
                <td>
                    <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg"  EnableViewState="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Panel ID="pnlLegend" runat="server">
                        <table cellpadding="1" cellspacing="2">
                            <tr>
                                <td align="left" colspan="1">
                                    <asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                        Font-Bold="True" Text="<%$ Resources:LocalizedResources, Legend %>" EnableViewState="false"></asp:Label></td>
                                <td align="left" colspan="1">
                                    <asp:Label ID="TextBox1" runat="server" BackColor="#eaeaea" Height="20px" BorderColor="Black"
                                        BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px" EnableViewState ="false"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                </td>
                                <td align="left" colspan="1">
                                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources, SubjectNotAssignToStandard %>"
                                        CssClass="ClsTextNormal" EnableViewState="false"></asp:Label></td>
                                <td align="right" style="width: 5px">
                                </td>
                                <td align="left" colspan="1">
                                    <asp:Label ID="Label1" runat="server" BackColor="#aae2cd" Height="20px" BorderColor="Black"
                                        BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px" EnableViewState ="false"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                </td>
                                <td align="left" colspan="1">
                                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources, MaximumLecturesAllowed %>"
                                        CssClass="ClsTextNormal" EnableViewState="false"></asp:Label></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="center" visible="true" runat="server" id="tdGrid">
                   <div id="GridViewScrollContainer" class="GridBorder"  style="width: 635pt; overflow: scroll;">
                        <asp:GridView ID="grdStandards" UseAccessibleHeader="true" Width="100%" runat="server"
                            AutoGenerateColumns="False"  AllowPaging="False" CellPadding="0"
                            CellSpacing="1" ForeColor="#333333" GridLines="None" OnRowDataBound="grdStandards_RowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                   <ItemTemplate>
                                      <input type="text" id="txtLength" runat="server" style="width: 36px;height: 19px;" maxlength="2" onchange="SetToAllCol(this)" onkeyup="extractNumber(this, 1 ,false);" 
                                            onkeypress="return blockNonNumbers(this, event, false, false);" onpaste="event.returnValue=false;" ondrop="event.returnValue=false;" onblur="extractNumber(this,2,false);"/>
                                   </ItemTemplate>
                                   <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="false" />
                                   <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="false"  />
                                    
                                </asp:TemplateField>
                                <asp:BoundField HeaderImageUrl="~/RITeSchool/images/GridHeader_StdSub_Title.gif" 
                                    SortExpression="Standard_Id" DataField="Standard_Name">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" Width="10%" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" Width="10%" />
                                </asp:BoundField>
                               
                                <asp:BoundField DataField="Standard_Id">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" Width="10%" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" Width="10%" />
                                </asp:BoundField>
                            </Columns>
                            <RowStyle CssClass="ClsGridRow" />
                            <HeaderStyle CssClass="ClsGridHeader" />
                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                            <EmptyDataRowStyle CssClass="LblNoRecord" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>" CssClass="ClsBtn" OnClick="BtnSave_Click" UseSubmitBehavior="false" />
                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>" CssClass="ClsBtn"  
                        UseSubmitBehavior="false" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidColumnCount" runat="server"></asp:HiddenField>        
        <asp:HiddenField ID="hidCultureInfo" runat="server" />
        <asp:HiddenField ID="hidValNoOfLectures" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidPleaseFixFollowingError" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidValLectureZero" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidForStandard" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidSubject" runat="server"></asp:HiddenField>
       

    </div>
    
    <script type="text/javascript" language="javascript">
        _clientStandardGridId = "<%=this.grdStandards.ClientID %>"
        _clienthidColumnCount = "<%=this.hidColumnCount.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"


        function SetToAllCol(e) {
            var sIdClntTxtBox = $(e).attr("id");
            var n = sIdClntTxtBox.lastIndexOf('_');
            var sRow = sIdClntTxtBox.substring(0, n + 1);
            var subjectCnt=document.getElementById(_clienthidColumnCount).value;
            var iTrIndex = $(e).closest('tr').index();
            var iRowIndex = iTrIndex + 2
              for (var iColIndex = 0; iColIndex < subjectCnt; iColIndex++) {
                var sTextBoxid = sRow + "txt_0" + iRowIndex + "_" + iColIndex
                if (document.getElementById(sTextBoxid))
                    document.getElementById(sTextBoxid).value = e.value.toString()
               }
        
        }
        function SetToAllRows(e, CollumnNo) {
           
            var iSelectedValue =e.value
            var k
            var iRowCount = document.getElementById(_clientStandardGridId).rows.length + 1
            for (var i = 2; i <= iRowCount; i++) {
                k = i + 1
                if (i < 10) {
                    srow = "0" + i
                } else {
                    srow = i
                }
                if (k < 10) {
                    srow1 = "0" + k
                } else {
                    srow1 = "0" + k
                }

                var sIdtxtBox = _clientStandardGridId + "_ctl" + srow + "_txt_" + srow1 + "_" + CollumnNo
                if (document.getElementById(sIdtxtBox)) {
                    document.getElementById(sIdtxtBox).value = iSelectedValue.toString()
                }
              
            }
        }


        function DisableButtons() {
            if (document.getElementById(_clientbtnSave)) {
                document.getElementById(_clientbtnSave).disabled = true
                document.getElementById(_clientbtnCancel).disabled = true
            } 
        }
        function validatetextbox(aiCount) {
            var iRowCount = document.getElementById(_clientStandardGridId).rows.length + 1
            var srow = ""
            var srow1 = ""
            var k
            var iStandardCnt1 = 0
            var iStandardCnt2 = 0
            var breturn = false
            var breturn1 = false
            var ichkcount = 0
            var completemessage = ""
            var completemessage1 = ""
            serrmessage = document.getElementById("<%=this.hidValNoOfLectures.ClientID %>").value
            for (var i = 2; i <= iRowCount; i++) {
                k = i + 1
                if (i < 10) {
                    srow = "0" + i
                }
                else {
                    srow = i
                }
                if (k < 10) {
                    srow1 = "0" + k
                }
                else {
                    srow1 = "0" + k
                }
                ichkcount = 0
                iStandardCnt1 = 0
                iStandardCnt2 = 0
                for (var j = 0; j < aiCount - 1; j++) {
                    var sidtxtLect = _clientStandardGridId + "_ctl" + srow + "_txt_" + srow1 + "_" + j
                    var shidStandardName = _clientStandardGridId + "_ctl" + srow + "_hid_" + srow1 + "_" + j
                    if (document.getElementById(sidtxtLect) != null) {
                        var sStandardName = document.getElementById(shidStandardName).value
                        if ((document.getElementById(sidtxtLect).value) == "") {
                            if (iStandardCnt1 == 0) {
                                var iRow = (sStandardName).substring(0, sStandardName.indexOf("("))
                                sblanktotalerrmessage = "\n\r" + document.getElementById("<%=this.hidForStandard.ClientID %>").value + ": " + iRow + "\n\r" + document.getElementById("<%=this.hidSubject.ClientID %>").value + ":"
                                iStandardCnt1++
                                completemessage1 = completemessage1 + sblanktotalerrmessage
                            }
                            var iRow = (sStandardName).substring(sStandardName.indexOf("("), sStandardName.length)
                            completemessage1 = completemessage1 + " \n  - " + iRow
                            breturn1 = true
                        } 
                    } 
                } 
            }
            if (breturn1) {
                alert(document.getElementById("<%=this.hidPleaseFixFollowingError.ClientID %>").value +" \n\r" + serrmessage + completemessage1)
                breturn = false
                return false
            }
            if (breturn) {
            var sMsg=document.getElementById("<%=this.hidPleaseFixFollowingError.ClientID %>").value + " \n\r" + document.getElementById("<%=this.hidValLectureZero.ClientID %>").value + completemessage;
            alert(sMsg)
                return false
            }
            else {
                document.getElementById(_clientbtnSave).disabled = true
                document.getElementById(_clientbtnCancel).disabled = true
                return true
            } 
        }    
    </script>
</asp:Content>
