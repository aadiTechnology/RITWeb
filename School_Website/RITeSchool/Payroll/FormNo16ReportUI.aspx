<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormNo16ReportUI.aspx.cs"
    MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master" Inherits="FormNo16ReportUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div>
        <asp:HiddenField ID="hidITAmount" runat="server" Value="0" />
        <asp:HiddenField ID="hisIsForSingle" runat="server" Value="Y" />
        <script type="text/javascript" language="javascript">

            ClosePopup();
            function ClosePopup() {
                var amount = document.getElementById("<%=this.hidITAmount.ClientID %>").value;
                var isForSingle = document.getElementById("<%=this.hisIsForSingle.ClientID %>").value;
                window.opener.UpdateAmount(amount, isForSingle);
                window.close();
                window.opener.focus();
            }

        </script>
    </div>
</asp:Content>
