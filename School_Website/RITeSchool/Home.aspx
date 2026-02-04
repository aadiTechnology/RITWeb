<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

<html>
<head runat="server">
    <link href="Styles/Styles.css" type="text/css" rel="stylesheet" />
    <link href="Styles/Styles1.css" type="text/css" rel="stylesheet" />
    <link href="Styles/Styles2.css" type="text/css" rel="stylesheet" />
    <link href="Styles/StylesNew.css" type="text/css" rel="stylesheet" />    
    <link rel="SHORTCUT ICON" href="../../images/favicon.ico" type="image/x-icon" />
    <script language="JavaScript" type="text/JavaScript">
    
    _clientLoginButton = "<%=this.FindControl("Login1$LoginButton").ClientID %>";
    
    function OpenFeedback()
        {
            window.open('../ParentsFeedback.aspx','_blank','scrollbars=yes,statusbar=no,resizable=no,top=5,left=30,width=850,height=680');
            return false;
        
        }
        function OpenFeatures()
        {
            window.open('../User_Guide.htm','_blank','scrollbars=yes,statusbar=no,resizable=no,top=5,left=30,width=850,height=680');
            return false;
        
        }
    
function disableControls()
{
    document.getElementById(_clientLoginButton).disabled=true;
}
function fnover(varname)
{
    var objTXT = document.getElementById(varname)
    objTXT.style.borderWidth = "1";
    objTXT.style.borderColor = "maroon";
    objTXT.style.backgroundImage = "url(images/BtnBGRollNew.jpg)";    
}

function fnout(varname)
{
    var objTXT = document.getElementById(varname)
    objTXT.style.borderWidth = "1";
    objTXT.style.borderColor = "#a3c07b";
    objTXT.style.backgroundImage = "url(images/BtnBG.jpg)";    
}
    
function RITeSchoolPPS()
{
    window.open('DOWNLOADS/School_On_Web.pdf');
    return;
}

function RITeSchoolInfo()
{
    window.open('SchoolInfoPopup.html','_new', 'height=690,width=750, status=yes, scrollbars=yes, menubar=no,location=no,top=0,left=0;');
    return;
}

function Open()
{
    window.open('DemoUserNames.aspx','_blank','top=190,left=250,scrollbars=no,toolbars=no,width=440,height=300');
    return;
}

function SelectText()
{
    document.getElementById("Login1_Password").select();
}

    </script>

    <style type="text/css">
        body
        {
            font-family: Trebuchet MS, Lucida Sans Unicode, Arial, sans-serif;
            background-color: #E2EBED;
            margin: 0px;
            padding: 0px;
            height: 100%;
            width: 100%;
            line-height: 130%;
            font-size: 0.9em;
            text-align: center;
        }
        #pageContent
        {
            width: 760px;
            margin: 0 auto; /*text-align:left;
		border-left:1px solid #000;
		border-right:1px solid #000;
		background-color:#FFF;*/
            height: 100%;
        }
        #pageContent p
        {
            padding-left: 10px;
            padding-right: 10px;
        }
        #dhtmlgoodies_progressPane
        {
            width: 100%;
            height: 100%;
            background-color: #FFF;
            z-index: 5000;
            position: absolute;
            left: 0px;
            top: 0px;
        }
        #dhtmlgoodies_progressBar_bg
        {
            position: absolute;
            left: 50%;
            top: 50%;
            width: 300px;
            height: 20px;
            border: 1px solid #000;
            background-color: #EEE;
            margin-left: -150px;
            margin-top: -20px;
            text-align: center;
        }
        #dhtmlgoodies_progressBar_outer
        {
            position: absolute;
            left: 0px;
            top: 0px;
            width: 0px;
            height: 100%;
            overflow: hidden;
        }
        #dhtmlgoodies_progressBar
        {
            position: absolute;
            left: 0px;
            top: 0px;
            width: 300px;
            background-color: #184EB8;
            filter: Alpha(Opacity=0, FinishOpacity=100,Style=1,StartX=0, StartY=0, FinishX=200,FinishY=20);
            height: 100%;
            z-index: 5000;
        }
        #dhtmlgoodies_progressBar_txt
        {
            color: #000;
            z-index: 10000;
            width: 100%;
            height: 100%;
            left: 0px;
            top: 0px;
            position: absolute;
            font-family: arial;
            font-size: 0.8em;
            line-height: 20px; /* Same as height of progress bar */
        }
    </style>
</head>
<body style="background-image: url(images/BGline.gif); background-repeat: repeat;">
    <form id="form1" runat="server" defaultfocus="TextBox1" onsubmit="javascript:disableControls();">
    <table align="center" border="0" cellpadding="0" cellspacing="0" style="width: 90%">
        <tr>
            <td style="text-align: center; vertical-align: middle; height: 150px;">
                <a class="ClsTextLink" href="javascript:RITeSchoolInfo();" style="padding-right: 5px;">
                    <asp:Image ID="Image3" runat="server" ImageUrl="images/ImpAnnouncement.jpg" />
                </a>
            </td>
        </tr>
        <tr>
            <td>
                <!-- Data Insert Here -->
                <table align="center" border="0" cellpadding="0" cellspacing="2" style="border: solid 1px #dddddd;">
                    <tr>
                        <td style="background-image: url(images/BlueDotHrLines.gif);" colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="bottom" colspan="3" style="background-color: #efefef;">
                            <table cellpadding="2" cellspacing="0" style="width: 100%">
                                <tr runat="server" id="trSchoolName">
                                    <td style="background-image: url(images/BlueDotHrLines.gif); width: 25%;">
                                    </td>
                                    <td style="width: 15%; padding-left: 10px;">
                                        <asp:Label EnableViewState="false" ID="SchoolNameLabel" runat="server" CssClass="LblUsrNameHead">Select School Name<img style="padding-left:5px;" src="images/ArrowBlack.GIF" /></asp:Label>
                                    </td>
                                    <td style="width: 55%; padding-right: 5px;">
                                        <asp:DropDownList ID="cmbSchools" runat="server" CssClass="ExLrgCombo" Width="500px">
                                            <asp:ListItem Value="0">Select </asp:ListItem>
                                        </asp:DropDownList>
                                        <span style="color: red">*</span>
                                        <asp:CompareValidator ID="cmp_standard" runat="server" ControlToValidate="cmbSchools"
                                            ValidationGroup="Login1" SetFocusOnError="true" Display="None" ErrorMessage="Please select school Name."
                                            Operator="NotEqual" ValueToCompare="0" CssClass="ClsLabel"></asp:CompareValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="cmbSchools"
                                            ValidationGroup="Go" SetFocusOnError="true" Display="None" ErrorMessage="*" Operator="NotEqual"
                                            ValueToCompare="0" CssClass="ClsLabel">*</asp:CompareValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="Right" valign="top" width="73%" class="bordergray HomePGTopImg">
                            <div style="float: left">
                                <a href="http://www.riteschool.com" target="_blank">
                                    <img alt="www.RITeSchool.com" src="images/spacer.gif" border="0" style="height: 200px;
                                        width: 260px" /></a>
                            </div>
                        </td>
                        <td align="center" class="bordergray" colspan="2" valign="top">
                            <div runat="Server" id="Div1">
                                <asp:Login ID="Login1" runat="server" OnAuthenticate="Login1_Authenticate" RememberMeSet="True">
                                    <TitleTextStyle CssClass="LblUsrNameHead" />
                                    <LayoutTemplate>
                                        <table border="0" width="100%" cellpadding="4" cellspacing="0" style="border-collapse: collapse">
                                            <tr>
                                                <td>
                                                    <table border="0" cellpadding="0" width="100%">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label EnableViewState="False" ID="UserNameLabel" runat="server" AssociatedControlID="UserName"
                                                                    CssClass="TxtBSml">User Name:</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:TextBox ID="UserName" runat="server" CssClass="TxtBoxLogin"></asp:TextBox>
                                                                <span style="color: red">*</span>
                                                                <asp:RequiredFieldValidator Display="None" ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                                    ErrorMessage="User Name is required." SetFocusOnError="true" ToolTip="User Name should not be blank."
                                                                    ValidationGroup="Login1"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label EnableViewState="False" ID="PasswordLabel" runat="server" AssociatedControlID="Password"
                                                                    CssClass="TxtBSml">Password:</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="TxtBoxLogin"></asp:TextBox>
                                                                <span style="color: red">*</span>
                                                                <asp:RequiredFieldValidator Display="None" ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                                    ErrorMessage="Password should not be blank." SetFocusOnError="true" ToolTip="Password is required."
                                                                    ValidationGroup="Login1"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me" CssClass="LblNormalImg"
                                                                    Checked="True" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="color: red" class="LblErrorMsg">
                                                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Button CssClass="ClsBtn" ID="LoginButton" runat="server" CommandName="Login"
                                                                    Text="Log In" ValidationGroup="Login1" BorderStyle="Solid" BorderWidth="1px"
                                                                    UseSubmitBehavior="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="padding-left: 5px; height: 10px">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="padding-left: 5px">
                                                                <asp:Label EnableViewState="false" ID="Label1" runat="server" CssClass="LblUsrNameHead"
                                                                    Text='Click Here<img style="padding-left:1px" src="images/downBlue.gif" />'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" class="BtnClkHere" style="padding-left: 5px">
                                                                <a class="ClickHereTxt" onclick="OpenFeatures();">For RITeSchool Features</a>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                </asp:Login>
                            </div>
                        </td>
                    </tr>
                    <tr class="SubTitleHead">
                        <td colspan="3" style="height: 12px;">
                            <table width="100%">
                                <tr>
                                    <td colspan="3" style="height: 12px; width: 75%;" align="right">
                                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowMessageBox="true"
                                            ShowSummary="false" ValidationGroup="Login1" />
                                        <marquee style="cursor: progress; border-top-width: thin; vertical-align: bottom;
                                            border-top-color: blue;" behavior="scroll" direction="left" scrollamount="2"
                                            scrolldelay="2" onmouseover="this.stop()" onmouseout="this.start()">
                <a href="#" class="navUGFeedback" onclick="OpenFeedback();">Click here to view Feedback about RITeSchool
            </a></marquee>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="3" style="height: 9px">
                            <table border="0" cellpadding="0" cellspacing="2" style="left: 0px; bottom: 2px"
                                width="100%">
                                <tr style="height: 13px">
                                    <td style="width: 50%; height: 13px;" align="left" colspan="2">
                                        <a href="http://www.regulusit.net" target="_blank" border="0">
                                            <asp:Image ID="Image2" runat="server" ImageUrl="~/images/RIT_LogoAnimated.gif" />
                                        </a>
                                    </td>
                                    <td style="width: 50%; height: 13px;" align="right" valign="middle">
                                        <table cellspacing="1" cellpadding="2" width="70%" border="0">
                                            <tr class="ClsTextNormal">
                                                <td class="ClsTextNormal ClsBorderlight" align="right" style="height: 21px">
                                                    <a class="ClsTextLink" href="javascript:Open();" style="padding-right: 5px;"><strong>
                                                        Click Here </strong></a>To view the Demo Login Details
                                                </td>
                                            </tr>
                                            <tr class="ClsTextNormal">
                                                <td height="10px">
                                                </td>
                                            </tr>
                                            <tr class="ClsTextNormal">
                                                <td class="ClsTextNormal ClsBorderlight" align="right" style="height: 21px">
                                                    To download the RITeSchool brochure
                                                    <img src="images/ArrowRedDblNw.gif" style="padding-left: 5px; padding-right: 5px;" />
                                                    <a class="ClsTextLink" href="javascript:RITeSchoolPPS();" style="padding-right: 5px;">
                                                        <strong>Click Here</strong> </a>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr class="SubTitleHead">
                        <td colspan="2" style="height: 9px; padding-right: 5px;" align="left">
                            <div id="footernote" style="font-size: 8pt; color: #808080; font-weight: normal">
                            </div>
                        </td>
                        <td align="right" colspan="1" style="height: 9px; padding-right: 5px;">
                            <div style="font-size: 8pt; color: #808080; font-weight: normal">
                                Best viewed with 1024x768</div>
                        </td>
                    </tr>
                </table>
                <!-- Data Insert End Here -->
            </td>
        </tr>
    </table>
    <span style="padding-left: 25px"></span>
    </form>
</body>

<script type="text/javascript" language="javascript" src="../Scripts/TextColorChange.js">
    var date = new Date()
    var year = date.getYear()
    document.getElementById('footernote').innerHTML = "All rights reserved © 2007 - " + year + ", Regulus Information Technologies"

</script>

</html>
