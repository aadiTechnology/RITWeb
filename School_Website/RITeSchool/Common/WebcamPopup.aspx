<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master" AutoEventWireup="true" CodeFile="WebcamPopup.aspx.cs" Inherits="WebcamPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" Runat="Server">
	<div>
<div style="padding-left:10px">
 <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px; width:98%">
 
        <tr>
            <td align="left">
                <asp:Label ID="lblHeader" runat="server" CssClass="MainTitleHead" Font-Bold="True">Capture Image Using Webcam</asp:Label>
            </td>
        </tr>
  </table>
</div>
<div style="height:80px;">

</div>
<asp:Label ID="lblSessionEmptyCheck" runat="server" CssClass="ClsHilightErrorB" ></asp:Label>
		<div style="margin-top:25px">
		 <object width="405" height="190">
			<param name="movie" value="WebcamResources/save_picture.swf">
			<embed src="WebcamResources/save_picture.swf" width="405" height="190">
		    </embed>
	     </object>
		</div>
		<div>
		     <div>
				 <asp:Button ID="btnSubmit" CssClass="ClsBtn" runat="server" Text="Submit" 
					  onclick="btnSubmit_Click" />
					 
			&nbsp;&nbsp;
				 <asp:Button ID="btnClose" CssClass="ClsBtn" runat="server" Text="Close" 
					 onclick="btnClose_Click" />
					 
			</div>
		</div>
		<asp:HiddenField ID="HidRowCount" runat="server" /> 
		<asp:HiddenField ID="HidPerentPage" runat="server" /> 
  </div>
   <script language="javascript" type="text/javascript">
//   	function CloseWindow() {
//   		window.close();
//   		window.opener.focus();
//   	}
    </script>
</asp:Content>

