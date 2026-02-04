<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PopupBlockerUI.aspx.cs" Inherits="RITeSchool_PopupBlockerUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body  style="background-color: AliceBlue;">
    <form id="form1" runat="server">
    <div>
        <table style="background-color: AliceBlue;" border="0" cellpadding="0" cellspacing="1"
            width="100%">
           
            <tr>
                <td valign="top" class="ClsProductimg" style="width: 906px">
                    <div>
                        <table border="0" width="100%" cellpadding="2" cellspacing="1" align="center">
                            <tr>
                                <td class="TitleBlueB"  >
                                    <h1 style="color: Maroon">
                                        How to disable your browser's popup blockers</h1>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtNormal" colspan="2" valign="top" style="font-size: 15px;
                                    letter-spacing: 1px; font-family: Arial">
                                     
                                    <p>
                                        The following includes steps for disabling pop-up window blockers.</p>
                                    <ul>
                                        <li><a href="#IE7">Internet Explorer 7/Internet Explorer 8</a></li>
                                        <li><a href="#IE6">Internet Explorer6</a></li>
                                        <li><a href="#firefoxPC">Firefox (Windows PC)</a></li>
                                         <li><a href="#googlecrome">Google Crome</a></li>
                                        
                                    </ul>
                                    <p>
                                        The following includes steps for disabling Brower toolbars.</p>
                                    <ul>
                                        <li><a href="#yahoo">Yahoo toolbar popup blocker</a></li>
                                        <li><a href="#google">Google toolbar popup blocker</a></li>
                                    </ul>
                                    <h3>
                                        <a name="IE7" id="IE7"></a>How to disable Internet Explorer 7/ Internet Explorer
                                        8 popup blocker</h3>
                                    <ol>
                                        <li>From the <b>Tools</b> menu, select <b>Internet Options</b>.<br/>
                                            <img align="middle" id="imgPopUpBlocker1" runat="server" alt="" src="~/RITeSchool/images/PopupBlocker1.JPG"
                                                width="343" height="199" /><br/>
                                            &nbsp;</li>
                                        <li>From the <b>Privacy</b> tab, uncheck <b>Turn on Pop-up Blocker </b>and click <b>
                                            "OK".</b><br/>
                                            <img align="middle" id="img1" runat="server" alt="" src="~/RITeSchool/images/PopUpBlocker2.JPG"
                                                style="width: 400px" width="367" height="454" /><br/>
                                            &nbsp;For more information on Internet Explorer popup blocker please go to <a target="_blank"
                                                href="http://www.microsoft.com/windowsxp/using/web/sp2_popupblocker.mspx">http://www.microsoft.com/windowsxp/using/web/sp2_popupblocker.mspx</a></li>
                                    </ol>
                                    <h3>
                                        <a name="IE6" id="IE6"></a>How to disable Internet Explorer 6 popup blocker</h3>
                                    <ol>
                                        <li>From the <b>Tools</b> menu, select <b>Internet Options</b>.<br/>
                                            <img align="middle" id="img2" runat="server" alt="" src="~/RITeSchool/images/PopUpBlocker3.JPG"
                                                width="343" height="199" /><br/>
                                            &nbsp;</li>
                                        <li>From the <b>Privacy</b> tab, uncheck <b>Block pop-ups</b>.<br/>
                                            <img align="middle" id="img3" runat="server" alt="" src="~/RITeSchool/images/PopUpBlocker4.JPG"
                                                width="406" height="459" /><br/>
                                            <br/>
                                            &nbsp;For more information on Internet Explorer popup blocker please go to <a target="_blank"
                                                href="http://www.microsoft.com/windowsxp/using/web/sp2_popupblocker.mspx">http://www.microsoft.com/windowsxp/using/web/sp2_popupblocker.mspx</a></li>
                                    </ol>
                                    <h3>
                                        <a name="firefoxPC" id="firefoxPC"></a>How to disable the Firefox popup blocker
                                        (Windows PC)</h3>
                                    <ol>
                                        <li>From the <b>Tools</b> menu, select <b>Options</b>.<br/>
                                            <img align="middle" id="img4" runat="server" alt="" src="~/RITeSchool/images/PopUpBlocker5.JPG"
                                                width="369" height="246" /><br />
                                            &nbsp;</li>
                                        <li>From the <b>Content</b> tab, uncheck <b>Block Popup Windows </b>and click "OK".<br/>
                                            <img align="middle" id="img5" runat="server" alt="" src="~/RITeSchool/images/PopUpBlocker6.JPG"
                                                width="460" height="453" /><br />
                                        </li>                                       
                                        </ol>

                                        <h3>
                                        <a name="googlecrome" id="googlecrome"></a>How to disable the Google Crome popup blocker</h3>
                                    <ol>
                                        <li>Click on this and select option to always allow to open popups from required link.<br/>
                                            <img align="middle" id="img10" runat="server" alt="" src="~/RITeSchool/images/GoogleCromePopupBlocker.png"
                                                width="369" height="246" /><br />
                                            &nbsp;</li>
                                                                          
                                        </ol>
                                    <h3>
                                        <a name="yahoo"></a>How to disable the Yahoo toolbar popup blocker</h3>
                                    <ol>
                                        <li>Locate the Popup blocker icon and click the down arrow.<br/>
                                            <img align="middle" id="img6" runat="server" alt="" src="~/RITeSchool/images/PopUpBlocker7.JPG"
                                                width="418" height="27" /><br/>
                                            &nbsp;</li>
                                        <li>Uncheck <b>Enable Pop-Up Blocker</b>.<br/>
                                            <img align="middle" id="img7" runat="server" alt="" src="~/RITeSchool/images/PopUpBlocker8.JPG"
                                                width="232" height="216" /><br/>
                                            &nbsp;</li>
                                    </ol>
                                    <p>
                                        For more information on the Yahoo toolbar please go to <a target="_blank" href="http://toolbar.yahoo.com/">
                                            http://toolbar.yahoo.com</a>.</p>
                                    <h3>
                                        How to disable the Google toolbar popup blocker?</h3>
                                    <ol>
                                        <li>Click on the Google logo on the Google toolbar and select <b>Options.</b><br/>
                                            <img align="middle" id="img8" runat="server" alt="" src="~/RITeSchool/images/PopUpBlocker9.JPG"
                                                width="278" height="110" /><br/>
                                            &nbsp;</li>
                                        <li>Uncheck <b>Popup Blocker</b>, and then click <b>OK</b>.<br/>
                                            <img align="middle" id="img9" runat="server" alt="" src="~/RITeSchool/images/PopUpBlocker10.JPG"
                                                width="446" height="576" /><br/>
                                            &nbsp;</li>
                                    </ol>
                                    <p>
                                        For more information on the Google toolbar please go to <a target="_blank" href="http://toolbar.google.com/">
                                            http://toolbar.google.com</a>.</p>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
