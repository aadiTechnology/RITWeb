<%@ Control Language="C#" AutoEventWireup="true" Inherits="SchoolWebApp.NoticeDivUC" Codebehind="NoticeDivUC.ascx.cs" %>
<div id="divSchoolNoticesLink" runat="server" style="display: none; visibility: hidden;
    z-index: 500; position: absolute; margin: 0px; padding: 0px; width: 320px; height: 250px;
    border-width: 0px; left: 5px; top: 0px; line-height: normal; border: 2px solid darkgreen;
    background-color: #FFFFBF">
    <div id="divInner" style="background-color: #FFFFBF; padding-top: 3px; height: 30px;
        background-image: url(RITeSchool/images/GridHeaderBG.gif); background-repeat: repeat-x;
        color: Black; text-align: right;" runat="server">
        <div id="divHeader" runat="server" style="font-size: 12px; width: 150px; letter-spacing: 1px;
            padding-left: 8px; font-weight: bold; color: darkgreen; float: left; height: 10px"
            align="left">
            School Notices
        </div>
        <span style="cursor: pointer" onclick="javascript:HidePopupSchoolNoticesLink();">
            <img alt="Hide Popup" style="vertical-align: top" runat="server" src="../images/close_vista.gif"
                border="0" />
        </span>
    </div>
    <div runat="server" align="center" style="padding: 2px; text-align: center; vertical-align: top;
        cursor: pointer; color: #333; overflow: auto; height: 200px; width: 300px; margin-left: 5px;
        background-color: #FFFFBF" id="divUserControl">
        <table width="100%" align="center" style="font-size: 11pt; color: #333; font-family: Arial;">
            <tr>
                <td style="height: 10px;">
                    <hr style="border-style: solid; border-color: Gray; border-bottom-width: thin;" />
                </td>
            </tr>
            <tr align="center">
                <td align="center">
                    <asp:ListView ID="lstvwNotices" runat="server" DataKeyNames="FileName, NoticeContent"
                        OnItemDataBound="lstvwNotices_ItemDataBound" OnDataBound="lstvwNotices_DataBound">
                        <layouttemplate>
                            <table>
                                <tr align="center" runat="server" id="itemPlaceholder">
                                </tr>
                            </table>
                        </layouttemplate>
                        <itemtemplate>
                            <tr align="center">
                                <td align="center">
                                    <asp:HyperLink ID="hlnkNoticeH" Font-Bold="true" Font-Size="Medium" runat="server" style="text-decoration:underline;"
                                        Text='<%# Eval("NoticeName") %>'>
                                    </asp:HyperLink>
                                </td>
                            </tr>
                        </itemtemplate>
                    </asp:ListView>
                </td>
            </tr>
            <tr>
                <td style="height: 10px;">
                    <hr style="border-style: solid; border-color: Gray; border-bottom-width: thin;" />
                </td>
            </tr>
            <tr align="center">
                <td align="center" colspan="2" valign="bottom">
                    <asp:Button ID="btnCancelNotice" runat="server" CssClass="ClsBtnMid ClsBtnClose"
                        Text="Close" CausesValidation="false" Width="75px" />
                </td>
            </tr>
        </table>
    </div>
</div>
<div id="DivTextNotice" runat="server" style="visibility: hidden; display: none;
    border: 2px solid darkgreen; position: absolute; margin: 0px; padding: 0px; width: 550px;
    height: 365px; left: 0px; top: 0px; line-height: normal; width: auto; 
    margin: -100px 10px 10px -100px; background-color: white; filter: progid:DXImageTransform.Microsoft.dropshadow(OffX=5, OffY=5, Color=#7D7E7E);">
    
    <div id="InnerDivHeader" runat="server" style="background-color: Transparent; background-image: url(RITeSchool/images/GridHeaderBG.gif);
        background-repeat: repeat-x; padding: 4px; color: #Black; cursor: pointer;">
        <div  id="divNoticeName" runat="server" style="padding: 1px; font-size: 12px; font-weight: bold; color: #Black; text-align: left;">
           </div>   
        <span style="cursor: hand; margin-top:-17px; margin-left:540px;">
            <img id="Img1" alt="Hide Popup" style="vertical-align: top; margin-top:-15px; text-align:right;" runat="server" src="../images/close_vista.gif"
                onclick="javascript:HideLateFeePopup();" border="0" />
        </span>  
           
    </div>
    <div style="padding: 10px; text-align: left; width: 550px; height: 275px; overflow: auto;"
        class="ClsLabel" id="divText" runat="server">
    </div>
</div>
<asp:HiddenField ID="hidFirstLogIn" runat="server" />
<asp:HiddenField ID="hidSchoolNoticesLinkPopUp" runat="server" Value="N" />
<asp:HiddenField ID="hidSchoolId" runat="server" />
<asp:HiddenField ID="hidWidth" runat="server" />
<asp:HiddenField ID="hidHeight" runat="server" />
<asp:HiddenField ID="hidInnerWidth" runat="server" />
<asp:HiddenField ID="hidInnerHeight" runat="server" />

<script language="javascript" type="text/javascript">

    _clienthidWidth = "<%=this.hidWidth.ClientID %>"
    _clienthidHeight = "<%=this.hidHeight.ClientID %>"
    _clienhidInnerHeight = "<%=this.hidInnerHeight.ClientID %>"
    _clientInnerWidth = "<%=this.hidInnerWidth.ClientID %>"
    _clientbtnCancelNotice = "<%=this.btnCancelNotice.ClientID %>"
    _clientDivTextNotice = "<%=this.DivTextNotice.ClientID %>"
    _clientdivText = "<%=this.divText.ClientID %>"
    _clientdivNoticeName = "<%=this.divNoticeName.ClientID %>"
    function fnover(varname) {
        var objTXT = document.getElementById(varname)
        objTXT.style.borderWidth = "1"
        objTXT.style.borderColor = "maroon"
        objTXT.style.backgroundImage = "url(RITeSchool/images/BtnBGRollNew.jpg)"
    }
    function fnout(varname, doc) {
        var objTXT = document.getElementById(varname)
        objTXT.style.borderWidth = "1";
        objTXT.style.borderColor = "#a3c07b";
        objTXT.style.backgroundImage = "url(RITeSchool/images/BtnBG.jpg)";
        //objTXT.style.color = "Black";
    }
    function ShowNoticePopup(content, NoticeName) {
        var x, y, tt_ovr_
        var cssstyle = document.getElementById("<%=this.DivTextNotice.ClientID %>").style
        document.getElementById(_clientdivText).innerText = content;
        document.getElementById(_clientdivText).innerHTML = content;
        document.getElementById(_clientdivNoticeName).innerText = NoticeName;
        document.getElementById(_clientdivNoticeName).innerHTML = NoticeName;

        
        if (document.getElementById("<%=this.hidSchoolId.ClientID %>").value == 1) {
            var left = parseInt((screen.width / 3))
            var top = parseInt((screen.height / 4))
        }
           else {
           	var left = parseInt((screen.width / 3))
           	var top = parseInt((screen.height / 3))
        }
       
        cssstyle.left = left + "px"
        cssstyle.top = top + "px"
        cssstyle.zIndex = Math.max((window.dd && dd.z) ? (dd.z + 2) : 0, 1010)
        cssstyle.visibility = "visible"


        cssstyle.display = "block"
    }
    function HideLateFeePopup() {
        document.getElementById("<%=this.DivTextNotice.ClientID %>").style.visibility = "hidden"
        document.getElementById("<%=this.DivTextNotice.ClientID %>").style.display = "none"
        var cssstyleMain = document.getElementById("<%=this.DivTextNotice.ClientID %>").style
        cssstyleMain.visibility = "hidden"
        cssstyleMain.display = "none"
        return false
       }

       

       // To initially run the function:
      
    function ShowSchoolNoticesLinkPopup() {
    	_clienthidSchoolNoticesLinkPopUp = "<%=this.hidSchoolNoticesLinkPopUp.ClientID %>";

        if (document.getElementById(_clienthidSchoolNoticesLinkPopUp).value == "N") {
            var x, y, tt_ovr_
            var cssstyle = document.getElementById("<%=this.divSchoolNoticesLink.ClientID %>").style
            var InnerDiv = document.getElementById("<%=this.divUserControl.ClientID %>").style
            var DivImage = document.getElementById("<%=this.divInner.ClientID %>").style
            var width = 10
            var height = 200
            
            if (document.getElementById("<%=this.hidSchoolId.ClientID %>").value == 1) {
            	var left = parseInt((screen.innerWidth / 5))
            	var top = parseInt((window.innerHeight / 5))
            }
            else {
            	var left = parseInt((window.innerWidth / 2.5))
            	var top = parseInt((window.innerHeight / 2.5))
            }
            cssstyle.left = left + "px"
            cssstyle.right = left + "px"
            cssstyle.top = top + "px"
            cssstyle.visibility = "visible"
            cssstyle.display = "block"
            if (document.getElementById(_clienthidHeight).value != "") {            	
                cssstyle.height = document.getElementById(_clienthidHeight).value;
                InnerDiv.height = document.getElementById(_clienhidInnerHeight).value; ;
            }
            if (document.getElementById(_clienthidWidth).value != "") {
                cssstyle.width = document.getElementById(_clienthidWidth).value;
                InnerDiv.width = document.getElementById(_clientInnerWidth).value;
                DivImage.width = document.getElementById(_clienthidWidth).value;

            }

            document.getElementById(_clientbtnCancelNotice).setAttribute("CssClass", "ClsBtnMid");
        }
        else
            HidePopupSchoolNoticesLink()
    }

    function HidePopupSchoolNoticesLink() {
        document.getElementById(_clienthidSchoolNoticesLinkPopUp).value = "N";
        if (document.getElementById("<%=this.divSchoolNoticesLink.ClientID %>") != null) {
            document.getElementById("<%=this.divSchoolNoticesLink.ClientID %>").style.visibility = "hidden"
            document.getElementById("<%=this.divSchoolNoticesLink.ClientID %>").style.display = "none"
        }
        return false
    }

    ShowSchoolNoticesLinkPopup();
    $(window).resize(function () {
    	ShowSchoolNoticesLinkPopup();
    });
</script>
