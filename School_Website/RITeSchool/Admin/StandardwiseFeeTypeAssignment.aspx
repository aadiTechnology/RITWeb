<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardwiseFeeTypeAssignment.aspx.cs"
    Inherits="Standardwise_Fee_Type_Assignment" MasterPageFile="../MasterPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
	<div class="MainBodyDiv">
        <table style="width: 97%" align="center">
            <tr align="center">
                <td>
                    <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg"  EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">                  
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div id="divGridView" class="GridBorder" runat="server" style="width: 800px;overflow-y:auto; overflow-x=auto">
                        <asp:GridView ID="grdStandards" Width="100%" runat="server" AutoGenerateColumns="False"
                            AllowPaging="false" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                            GridLines="None" DataKeyNames='standard_id,original_standard_id,standard_name'
                            OnRowDataBound="grdStandards_RowDataBound" OnPageIndexChanging="grdStandards_PageIndexChanging" OnRowCreated="grdStandards_RowCreated">
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                            </PagerStyle>
                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                            <Columns>
                                <asp:TemplateField HeaderImageUrl="~/RITeSchool/images/GridHead_Std_FeeType.gif" HeaderText="<%$ Resources:LocalizedResources, Standards_FeeTypes%>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStandard" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Wrap="False" Width="5%" HorizontalAlign="Left" CssClass="paddingLSML" />
                                    <HeaderStyle HorizontalAlign="center" VerticalAlign="Middle" Wrap="False" Width="5%" />
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle CssClass="ClsGridRow" />
                            <HeaderStyle CssClass="ClsGridHeader" />
                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                            <EmptyDataRowStyle CssClass="LblNoRecord" />
                        </asp:GridView>
                    </div>                    
                </td>                
            </tr>
            <tr><td></td></tr>
            <tr>
                <td align="center">
                    <table align="center">
                        <tr>
                            <td align="left" class="ClsBorderlight " style="width: 80px; background-color: #ffffc4;">
                                <asp:Label ID="Label" runat="server" BorderWidth="0px" Font-Bold="True" Text= "<%$ Resources:LocalizedResources, Note%>"
                                    CssClass="LblNrmlB"></asp:Label>
                                     <span class="colonPadding">:</span>
                            </td>
                            <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                <asp:Label ID="lblNote" runat="server" Width="750px" BorderWidth="0px" CssClass="LblSmlV" Text= "<%$ Resources:LocalizedResources, NoteForStandardwiseFeeType%>"  ></asp:Label>
                            </td>
                        </tr>
                    </table>
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
                    <asp:Button ID="btnSave" runat="server" Text= "<%$ Resources:LocalizedResources, Save%>" CssClass="ClsBtn" OnClick="BtnSave_Click" disable-page="true" />
                    <asp:Button ID="btnCancel" runat="server" Text= "<%$ Resources:LocalizedResources, Cancel%>" CssClass="ClsBtn"  UseSubmitBehavior="false" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidColumnCount" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="HidRefURl" runat="server"></asp:HiddenField>
    </div>

    <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.grdStandards.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        function DisableButtons() {
            if (document.getElementById(_clientbtnSave)) {
                document.getElementById(_clientbtnSave).disabled = true
                document.getElementById(_clientbtnCancel).disabled = true
            } 
        }

        function CheckAllInColumn(oDocument, sGridName, colNumber, Checked) {
            var start
            start = 2
            var bReturn = true
            var sArr = new Array()
            var k = 0
            var sId
            var n = (oDocument.getElementById(sGridName).rows.length)
            var nCols = (oDocument.getElementById(sGridName).rows[0].cells.length - 1)
            var nRows = n + 1
            var sRow = ""
            var sCol = ""
            for (var i = start ; i < nRows; i++) {
                if (i < 10) {
                    sRow = "_ctl0"
                }
                else {
                    sRow = "_ctl"
                }
                if (colNumber < 10)
                    sCol = "ctl"
                else
                    sCol = "ctl"
                sId = sGridName + sRow + i + "_" + sCol + colNumber;
                oDocument.getElementById(sId).value = Checked;
            } 
        }
        function CheckAll(obj, colNumber) {
            
            if (obj.value != "")
                CheckAllInColumn(document, _clientGridId, colNumber, obj.value);            
        }
    </script>
</asp:Content>
