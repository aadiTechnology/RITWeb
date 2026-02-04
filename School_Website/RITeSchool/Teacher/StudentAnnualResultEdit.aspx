<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentAnnualResultEdit.aspx.cs"
    Inherits="StudentAnnualResultEdit" MasterPageFile="../MasterPages/MasterPage.master" %>

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
            <asp:HiddenField ID="HidBackUrl" runat="server" /><asp:HiddenField ID="hidSubjectLists" runat="server" />
            <div style="padding-bottom: 7px; padding-top: 5px">
                <asp:Button ID="btnResult" runat="server" BorderStyle="Solid" BorderWidth="1px" CausesValidation="false"
                    CssClass="ClsBtnLrg" Text="Save & Generate Result" Visible="True" OnClick="btnResult_Click"
                    UseSubmitBehavior="false" />
                <asp:Button ID="btnBack" runat="server" BorderStyle="Solid" BorderWidth="1px" CssClass="ClsBtnSml"
                    Text="Back" Visible="True" OnClick="btnBack_Click" UseSubmitBehavior="false" />
            </div>
            <asp:Panel ID="ResultContainer" runat="server" Visible="true" Style="overflow: auto;
                width: 842px; left: 0px;">
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">
    _clientIdhidEdited = "<%=this.hidEdited.ClientID%>";
    _clientbtnResult = "<%=this.btnResult.ClientID%>";  
    _clienthidSubjectLists = "<%=this.hidSubjectLists.ClientID%>";  
    
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
		
		function Validate(textbox, container, MarksScored ,TotalMarks, SubjMaxVal, stdMaxVal)
		{
		
		    var sMarks = textbox.value;
		    var iMarks = parseInt(sMarks);		    	
		    if(sMarks.length<=0)
		        textbox.value ="0";			        
		    if(iMarks>SubjMaxVal || (iMarks + MarksScored) > TotalMarks ||getRowTotalMarks(container)>stdMaxVal)
		    {
		        textbox.value =document.getElementById("marks").value;				        
		        textbox.focus();
		    }
		    document.getElementById(_clientIdhidEdited).value="1";
		}
		
		
		function getRowTotalMarks(container)
        {
            var totmarks = 0;            
            var arr = document.getElementById(_clienthidSubjectLists).value.split("||");
            var iSubject ;
            for(iSubject = 0; iSubject< arr.length; iSubject++)
            {        
            
                var marks, txtName;
                txtName = container +"_" + arr[iSubject];                     
                
                if (document.getElementById(txtName) != null)
                {
                    marks = document.getElementById(txtName).value;                
                    if (marks != "")
                        totmarks = totmarks + parseInt(RemoveLeadingZeroes(marks));
                }
            }       
            
          return totmarks;
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
    </script>

</asp:Content>
