<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentIdentityCards.aspx.cs"
    Inherits="StudentIdentityCards" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/RITeSchool/Styles/Styles.css" type="text/css" rel="stylesheet" />
    <link href="~/RITeSchool/Styles/Styles1.css" type="text/css" rel="stylesheet" />
    <link href="~/RITeSchool/Styles/Styles2.css" type="text/css" rel="stylesheet" />
    <link href="~/RITeSchool/Styles/Styles3.css" type="text/css" rel="stylesheet" />
    
    <STYLE TYPE="text/css">
     P.breakhere {page-break-before: always}
    </STYLE> 

</head>

<script language="javascript" type="text/javascript">
    function PrintSheet() {
        window.print();
        return false;
    }
</script>
<body style="background-image: none">
    <form id="frm" runat="Server">
        <table id="GridViewContainer" runat="server" enableviewstate="false" visible="true">
        </table>
    </form>
</body>

<script language="javascript" type="text/javascript">
    PrintSheet();
</script>

</html>
