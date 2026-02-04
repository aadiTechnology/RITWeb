<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoginFromBetaPage.aspx.cs"
    Inherits="LoginFromBetaPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        body
        {
            font-family: Arial, sans-serif;
            text-align: center;
            background-color: white;
            margin: 50px;
        }
        .error-box
        {
            display: inline-block;
            background-color: #dc3545;
            color: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        }
        .error-box h2
        {
            margin: 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="error-box">
            <h2>
                ⚠️ An unexpected error occurred. Please try again later.</h2>
        </div>
        <asp:HiddenField ID="hidScreenWidth" runat="server" />
    </div>
    <script type="text/javascript">
        _hidScreenWidth = "<%=this.hidScreenWidth.ClientID%>";
        function SetWidth() {
            if (document.getElementById(_hidScreenWidth) != null) {
                var hidScreenWidth = document.getElementById(_hidScreenWidth)
                hidScreenWidth.value = "" + window.screen.width;
            }
        }
        SetWidth();
    </script>
    </form>
</body>
</html>
