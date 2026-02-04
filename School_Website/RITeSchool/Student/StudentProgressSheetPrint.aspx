<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentProgressSheetPrint.aspx.cs" MasterPageFile="~/RITeSchool/MasterPages/BlankMaster.master" Inherits="StudentProgressSheetPrint" %>

<asp:Content ID="headContent" runat="server" ContentPlaceHolderID="headContentPlaceHolder">
    <style type="text/css">
		.Clspadding{padding:0 1px}
		.ClsBGWhite{background-color:#FFF}
		.PBorderBtm{font-family:Arial;font-size:8pt;font-weight:Normal;border-bottom-color:#000;border-bottom-style:dashed;border-bottom-width:1px}
		.PClsMarksCell{font-family:Arial;font-size:8pt;font-weight:Normal;padding-right:1px;background-color:#fff}
		.PClsTotalMarksCell{color:#000;font-family:Arial;font-size:8pt;font-weight:400;background-color:#fff;padding:0 1px}
		.PClsMarksGridHeader{font-weight:700;font-size:8pt;color:#333;text-decoration:none;padding-right:1px;padding-left:1px;height:20px;background-color:#fff}
		.PClsTestHeader{font-size:8pt;font-family:Verdana;color:#000;text-decoration:none;height:20px;background-color:#fff;padding:0 1px}
		.PClsMarksGridHeaderBG{font-weight:700;font-size:8pt;color:#333;text-decoration:none;padding-right:1px;padding-left:1px;height:20px;vertical-align:bottom;background-color:#fff}
		.PTotalType{font-weight:700;font-size:8pt;color:#000;background-color:#fff;padding:0 1px}
		.PTotalHead{background-color:#fff;font-weight:700;font-size:8pt;color:#000;padding:0 1px}
		.PReportOuter{background-color:#939393}
		.PClsHilightTextB{padding-left:1px;color:#000;font-size:11pt;font-weight:700}
		.PActualSchoolName{font-weight:700;font-family:Tahoma,;color:#000;font-size:15pt;text-transform:capitalize;border-bottom:1px solid #ddd;background-color:#fff;padding:1px}
		.PSocietyName{font-weight:700;font-family:Tahoma,;color:#000;font-size:12pt;text-transform:capitalize;border-bottom:1px solid #ddd;background-color:#fff;padding:1px}
		.PAnRClsReportHead{font-weight:700;font-family:Tahoma,;color:#000;text-transform:capitalize;font-size:13pt;border-bottom:1px solid #ddd;background-color:#b9f6f2;padding:1px}
		.PClsReportHead{font-weight:700;font-family:Tahoma,;color:#000;text-transform:capitalize;font-size:13pt;border-bottom:1px solid #ddd;background-color:#fff;padding:1px}
		.Dottedhr{border-bottom:#000 1px dashed;border-top:none;border-right:none;border-left:none;margin:21px 0}
		.ClspaddingL{padding-left:2px;text-align:center}
		.ClspaddingR{padding-right:2px}
		.PClsMarksGridRow,.PClsMarksGridAltRow{margin-left:1px;height:20px;font-weight:700;font-size:8pt;padding-left:1px;background-color:#fff}
	</style>
</asp:Content>
<asp:Content ID="bodyContent" runat="server" ContentPlaceHolderID="bodyContentPlaceHolder">
    <script language="javascript" type="text/javascript">
        function PrintSheet() {
            _clienthidMode = "<%=this.hidMode.ClientID %>";

            if (document.getElementById(_clienthidMode).value != "TeacherView")
                window.print();
            return false;
        }

        function ValidateMaxLength(val, maxLength) {
            if (val.value.length > maxLength) {
                val.value = val.value.substring(0, maxLength);
                return false;
            }
            return true;
        }
    </script>

    <asp:HiddenField ID="hidMode" runat="server" />
    <asp:Label ID="lblErrorMsg" runat="server" Visible="False" CssClass="LblNoRecord" EnableViewState="False" />
    <asp:Panel ID="GridViewContainer" runat="server" EnableViewState="false" Visible="true" Style="padding-left: 20px; margin-top: 50px"></asp:Panel>
    <asp:Panel ID="ResultContainer" runat="server" Visible="true" Style="width: 842px; padding-left: 20px; margin-top: 50px"></asp:Panel>
    
	<script language="javascript" type="text/javascript">
        PrintSheet();
    </script>
    
	<asp:HiddenField ID="hidSubName" runat="server" />
    <asp:HiddenField ID="hidRowSpan" runat="server" />
    <asp:HiddenField ID="hidRowNo" runat="server" Value="-1" />
</asp:Content>
