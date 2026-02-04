<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebcamNewPopup.aspx.cs" Inherits="WebcamNewPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
        table
        {
            border: 1px solid #ccc;
            border-collapse: collapse;
        }
        table th
        {
            color: #333;
            font-weight: bold;
        }
        table th, table td
        {
            padding: 5px;
            width: 300px;
        }
        .style1
        {
            width: 300px;
        }
        
        .ClsBtn
        {
            border: solid 1px #a3c07b;
            width: 75px;
            font-family: Arial;
            font-size: 9pt;
            font-weight: 700;
            background-position: 50% bottom;
            background-image: url(../images/BtnBG.jpg);
            background-repeat: repeat-x;
            text-transform: capitalize;
            cursor: pointer;
            margin: 2px 0 0 1px;
            padding: 2px;
        }
        
        input[type="submit"], input[type="button"], input[type="reset"]
        {
            background: #77a809 linear-gradient(to bottom, #37c8a6 5%, #177d5b 100%) repeat scroll 0 0 !important;
            border: 1px solid #007c3e !important;
            border-radius: 5px;
            color: #fff !important;
            padding: 5px 10px;
        }
    </style>
</head>
<body>
    <table id="tblMain" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <th align="center" style="width: 50%; border: 1px solid #ccc;">
                <u>Camera</u>
            </th>
            <th align="center" style="border: 1px solid #ccc;">
                <u>Captured Picture</u>
            </th>
        </tr>
        <tr>
            <td style="border: 1px solid #ccc;" align="center">
                <div id="webcam">
                </div>
            </td>
            <td style="border: 1px solid #ccc;" align="center">
                <img id="imgCapture" />
            </td>
        </tr>
        <tr>
            <td align="center" style="border: 1px solid #ccc;">
                <input type="button" id="btnCapture" value="CAPTURE" class="button" />
            </td>
            <td align="center" style="border: 1px solid #ccc;">
                <input type="button" id="btnUpload" value="UPLOAD" disabled="disabled" class="button" />
            </td>
        </tr>
    </table>
    <form id="Form1" runat="server">
    <table width="100%" style="border: 1px solid white;">
        <tr>
            <td align="left">
                <asp:Label ID="Label1" runat="server" Text="Steps : Click on Capture Button => Click on Upload Button => Click on Submit Button."
                    ForeColor="Navy" Font-Bold="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" EnableViewState="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnSubmit" runat="server" Text="SUBMIT" OnClick="btnSubmit_Click" />
                <asp:Button ID="btnClose" runat="server" Text="CLOSE" OnClick="btnClose_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="HidRowCount" runat="server" />
                <asp:HiddenField ID="HidPerentPage" runat="server" />
            </td>
        </tr>
    </table>
    </form>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="WebCamHTML5/WebCam.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            Webcam.set({
                width: 320,
                height: 240,
                image_format: 'jpeg',
                jpeg_quality: 90
            });
            Webcam.attach('#webcam');

            Webcam.set({
                width: 112,
                height: 151,
                image_format: 'jpeg',
                jpeg_quality: 90
            });
            Webcam.attach('#imgCapture');


            $("#btnCapture").click(function () {
                Webcam.snap(function (data_uri) {
                    $("#imgCapture")[0].src = data_uri;
                    $("#btnUpload").removeAttr("disabled");
                });
            });
            $("#btnUpload").click(function () {
                $.ajax({
                    type: "POST",
                    url: "WebcamNewPopup.aspx/SaveCapturedImage",
                    data: "{data: '" + $("#imgCapture")[0].src + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (r) { }
                });
            });
        });
    </script>
</body>
</html>
