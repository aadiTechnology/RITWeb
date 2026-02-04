<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master" AutoEventWireup="true" CodeFile="TestReportPopup.aspx.cs" Inherits="TestReportPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" Runat="Server">

<script type="text/javascript" language=javascript>

    ClosePopup();
    function ClosePopup() {
    alert('test')
       this.close()
    }
         
</script>
</asp:Content>

