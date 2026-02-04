<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentProgressSheetEdit.aspx.cs"
    Inherits="StudentProgressSheetEdit" MasterPageFile="../MasterPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <asp:Panel ID="pnlErrorMsg" Visible="false" runat="server" Width="100%">
        <table>
            <tr>
                <td align="left">
                    <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="blue"
                        Width="100%" CssClass="ClsConfigText" EnableViewState="false"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:UpdatePanel ID="UPanelStandardt" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="GridViewScrollContainer" runat="server" Visible="true" Style="overflow: auto;
                width: 842px; left: 0px;">
            </asp:Panel>
            <input type="hidden" id="marks" />
            <asp:HiddenField ID="hidEdited" Value="0" runat="server" />
            <asp:HiddenField ID="hidResultGenrted" Value="1" runat="server" />
            <asp:HiddenField ID="HidBackUrl" runat="server" /><asp:HiddenField ID="hidIsGraceApplied" Value="0" runat="server" />
            <div style="padding-bottom: 7px; padding-top: 5px">
                <asp:Button ID="btnResult" runat="server" BorderStyle="Solid" BorderWidth="1px" CausesValidation="false"
                    CssClass="ClsBtnLrg" Text="Save & Generate Result" Visible="True" OnClick="btnResult_Click"
                    UseSubmitBehavior="false" />               
            </div>
            <asp:Panel ID="ResultContainer" runat="server" Visible="true" Style="overflow: auto;
                width: 842px; left: 0px;">
            </asp:Panel>
            <div> <asp:Button ID="btnBack" runat="server" BorderStyle="Solid" BorderWidth="1px" CssClass="ClsBtnSml"
                    Text="Back" Visible="True" OnClick="btnBack_Click" UseSubmitBehavior="false" /></div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">
    _clientIdhidEdited = "<%=this.hidEdited.ClientID%>";
    _clientbtnResult = "<%=this.btnResult.ClientID%>";  
    
        function DisableButtons()
        {
            document.getElementById(_clientbtnResult).disabled=true;
            
        }
          
        function checkNumber(val) 
		{
			var strPass = val.value;
			var strLength = strPass.length;
			var lchar = val.value.charAt((strLength) - 1);
			var cCode = CalcKeyCode(lchar);
        
			/* Check if the keyed in character is a number
				do you want alphabetic UPPERCASE only ?
				or lower case only just check their respective
				codes and replace the 48 and 57 */

			if (cCode < 48 || cCode > 57) 
			{
//				ifs(cCode!=46)
//				{
					var myNumber = val.value.substring(0, (strLength) - 1);
					val.value = myNumber;
//				}
			}
			return false;
		}
		
		function Validate(textbox,MaxVal)
		{
		    var sMarks = textbox.value;
		    var iMarks = parseFloat(sMarks);		    	
		    if(sMarks.length<=0)
		        textbox.value ="0";	
		    if(iMarks>MaxVal)
		    {
		        textbox.value =document.getElementById("marks").value;				        
		        textbox.focus();
		    }
		    document.getElementById(_clientIdhidEdited).value="1";
		}
		
		function CalcKeyCode(aChar)
		{
			var character = aChar.substring(0,1);
			var code = aChar.charCodeAt(0);
			//alert(code);
			return code;
		}
		
		function SetValue(textbox)
		{
		    document.getElementById("marks").value=textbox.value;
		}
		
		function ShowGraceWarning()
        {
            var bResult = false;
            if (window.confirm("This action will overwrite the grace marks applied. Are you sure you want to continue?") )
            {                 
                bResult= true;
            }
            else
            {   bResult =false;
            }
            return bResult;
        }
    </script>

</asp:Content>
