<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MarksGradeConfiguration.aspx.cs"
    MasterPageFile="../MasterPages/MasterPage.master" Inherits="MarksGradeConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv" runat="Server" id="divMain">
        <table align="center" border="0" cellpadding="2" cellspacing="2" width="99%">
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblErrors" runat="server" CssClass="LblErrorMsg" Visible="true" EnableViewState="false"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="imgBtnSave" EventName="Click" />
							<asp:AsyncPostBackTrigger ControlID="ddlStandards" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table>
                                <tbody>                                   
                                    <tr>
                                        <td align="center">
                                            <div style="width: 90%" id="divLblMsg" runat="server" visible="false" class="ClsHilightBGB">
                                                <asp:Label ID="LblMsg" runat="server" EnableViewState="False"></asp:Label></div>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td align="center">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="optSubjects" runat="server" Text="<%$ Resources:LocalizedResources, Subjects%>" Font-Bold="False"
                                                            GroupName="Marks" CssClass="ClsLabel" OnCheckedChanged="optSubjects_CheckedChanged" AutoPostBack="True" />
                                                           </td>
                                                            <td>
                                                        <asp:RadioButton ID="optCocurricular" runat="server" Text="<%$ Resources:LocalizedResources, CoCurricularSubjects%>"
                                                            CssClass="ClsLabel" GroupName="Marks" OnCheckedChanged="optSubjects_CheckedChanged" AutoPostBack="True" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td class="ClsBorderlight">
                                                            <asp:Label ID="lblSelectedStandard" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources, SelectStandard%>"></asp:Label>
                                                            <span class="colonPadding ClsLabel">:</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlStandards" runat="server" OnSelectedIndexChanged="ddlStandards_SelectedIndexChanged"
                                                                AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                    <td>
                                                            <asp:HiddenField ID="hidConfigDependancy" runat="server" />
                                                    </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            &nbsp;
                                            <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                                ID="uPnl">
                                                <ContentTemplate>
                                                    <div align="center" style="width: 100%" id="Div1" runat="server">
                                                        <asp:GridView CssClass="GridBorder" ID="grdMarkGrades" runat="server" ForeColor="#333333"
                                                            GridLines="None" CellSpacing="1" CellPadding="0" OnRowDataBound="grdMarkGrades_RowDataBound"
                                                            AllowPaging="false" PageSize="20" AutoGenerateColumns="False" Width="100%" DataKeyNames="Starting_Marks_Range,Ending_Marks_Range,Grade_Name,remarks,Marks_Grades_Configuration_Id,Academic_Year_Id ,Original_Config_Id">
                                                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                            </PagerStyle>
                                                            <PagerSettings NextPageText="<%$ Resources:LocalizedResources,Next%>" LastPageText="<%$ Resources:LocalizedResources, Last%>" PreviousPageText="<%$ Resources:LocalizedResources, Previous%>"
                                                                FirstPageText="<%$ Resources:LocalizedResources, First%>" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <input id="ChkAllDel" type="checkbox" runat="server" onclick="AllRowsCheckedOrUnchecked(document,this,'ChkBoxDelete')" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="ChkBoxDelete" runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="1%" HorizontalAlign="Left" CssClass="paddingLSML" />
                                                                    <HeaderStyle Width="1%" HorizontalAlign="Left" CssClass="paddingLSML" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, StartingPercentage%>" SortExpression="Starting_Marks_Range">
                                                                    <EditItemTemplate>
                                                                        &nbsp;
                                                                    </EditItemTemplate>
                                                                    <HeaderStyle Width="20%" Wrap="False" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtStartingMarks" CssClass="TxtAlignRght" runat="server" MaxLength="3"
                                                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                                            ondrop="event.returnValue=false" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="20%" Wrap="False" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources,EndingPercentage%>" SortExpression="Ending_Marks_Range">
                                                                    <EditItemTemplate>
                                                                        &nbsp;
                                                                    </EditItemTemplate>
                                                                    <HeaderStyle Width="20%" Wrap="False" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtEndingMarks" CssClass="TxtAlignRght" runat="server" MaxLength="3"
                                                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                                            ondrop="event.returnValue=false" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="20%" Wrap="False" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, GradeName%>" SortExpression="Grade_Name">
                                                                    <EditItemTemplate>
                                                                        &nbsp;
                                                                    </EditItemTemplate>
                                                                    <HeaderStyle Width="20%" Wrap="False" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox CssClass="TxtAlignCenter" ID="txtGradeName" runat="server" MaxLength="4"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="20%" Wrap="False" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, Remarks%>" SortExpression="Remarks">
                                                                    <EditItemTemplate>
                                                                        &nbsp;
                                                                    </EditItemTemplate>
                                                                    <HeaderStyle Width="40%" Wrap="False" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="200" Width="300px" CssClass="SmlTxtBox"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="40%" Wrap="False" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle CssClass="ClsGridRow" />
                                                            <HeaderStyle CssClass="ClsGridHeader" />
                                                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                                        </asp:GridView>
                                                        <asp:HiddenField ID="hidMarkGradeConfigId" runat="server" />
                                                        <asp:HiddenField ID="hidMode" runat="server" />
                                                        <asp:HiddenField ID="hidPleaseFixFollowingErrors" runat="server" />
                                                        <asp:HiddenField ID="hidCultureInfo" runat="server" />
                                                        <asp:HiddenField ID="hidStartingPercentageEndingPercentageShouldNotBeSame" runat="server" />
                                                        <asp:HiddenField ID="hidDuplicatePercentageRangeAreNotAllowed" runat="server" />
                                                        <asp:HiddenField ID="hidStartingPercentageOverlap" runat="server" />
                                                        <asp:HiddenField ID="hidEndingPercentageOverlap" runat="server" />
                                                        <asp:HiddenField ID="hidSomeOfTheStartingEndingPerRangeAreMissing" runat="server" />
                                                        <asp:HiddenField ID="hidMinimumStartingPerShouldBe" runat="server" />
                                                        <asp:HiddenField ID="hidMaximumEndingPerShouldBe" runat="server" />
                                                        <asp:HiddenField ID="hidRowNumber" runat="server" />
                                                        <asp:HiddenField ID="hidStartingPerForFollowingRowsShouldNotBeBlank" runat="server" />
                                                        <asp:HiddenField ID="hidEndingPerForFollowingRowsShouldNotBeBlank" runat="server" />
                                                        <asp:HiddenField ID="hidGradeNameForFollowingRowsShouldNotBeBlank" runat="server" />
                                                        <asp:HiddenField ID="hidStartingPerForFollowingRowsShouldBeLessThanEndingPer" runat="server" />
                                                        <asp:HiddenField ID="hidRemarksForFollowingRowsShouldNotBeBlank" runat="server" />
                                                        <asp:HiddenField ID="hidDuplicateGradeNameIsNotAllowed" runat="server" />
                                                        <asp:HiddenField ID="hidPercentageGradeForStandard" runat="server" />
                                                        <asp:HiddenField ID="hidCanNotBeModifiedAsExamConfigurationAlreadyDone" runat="server" />
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlStandards" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr align="center">
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="imgBtnSave" Text="<%$ Resources:LocalizedResources, Save%>" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                                OnClick="imgBtnSave_Click" Visible="false" UseSubmitBehavior="false"/>
                            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel%>" CssClass="ClsBtn" BorderWidth="1px"
                                OnClick="btnCancel_Click" UseSubmitBehavior="false" 
                                CausesValidation="False" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <div class="MainBodyDiv" runat="Server" id="divError">
        <table border="0" cellpadding="0" cellspacing="0" width="97%" align="center">
            <tr>
                <td colspan="2" align="left" class="LblNoRecord">
                    <div>
                        <asp:Label ID="lblError" CssClass="ClsConfigText" runat="server" 
                            EnableViewState="false"></asp:Label>
                    </div>
                    <div style="padding-top: 10px">
                        <asp:HyperLink ID="hlnkClickHere" EnableViewState="false" runat="Server" Text="<%$ Resources:LocalizedResources, Standard%>"
                            NavigateUrl="~/RITeSchool/Admin/standardslist.aspx" CssClass="ClsConfigLink"></asp:HyperLink>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnCancel1" runat="server" Text="<%$ Resources:LocalizedResources, Cancel%>" CssClass="ClsBtn" 
                        OnClick="btnCancel_Click" CausesValidation="False" />
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
    _clientGridId = "<%=this.grdMarkGrades.ClientID %>";
    _clientSaveId = "<%=this.imgBtnSave.ClientID %>";
    _clientbtnCancel = "<%=this.btnCancel.ClientID %>";
    _clientddlStandards = "<%=this.ddlStandards.ClientID %>";
    _clientErrors = "<%=this.lblErrors.ClientID %>>"
    _clienthidConfigDependancy = "<%=this.hidConfigDependancy.ClientID %>";
    
    var iStartCnt ;
    iStartCnt = getStartIndex(false);
       
    function AllRowsCheckedOrUnchecked(document, ctrl, chkbox)
    { 
        $('#<%=grdMarkGrades.ClientID %> input:checkbox').attr('checked', ctrl.checked);
        var n = document.getElementById(_clientGridId).rows.length + 1;
        var chk;
        for(i=iStartCnt;i<n;i++)
        {
            if (i<10)
                chk = _clientGridId + "_ctl0" + i + "_"+chkbox;
            else 
                chk = _clientGridId + "_ctl" + i + "_"+chkbox;
            
            EnableOrDisableRelatedControl(ctrl,i); 
        }
        
        document.getElementById(_clientSaveId).disabled = !ctrl.checked; 
    }
    
    function EnableOrDisableRelatedControl(obj, RowNumber)
    {
    
        var start,end,grade,remarks;
        if (RowNumber<10){
            start = _clientGridId + "_ctl0" + RowNumber + "_txtStartingMarks";
            end = _clientGridId + "_ctl0" + RowNumber + "_txtEndingMarks";
            grade = _clientGridId + "_ctl0" + RowNumber + "_txtGradeName";
            remarks = _clientGridId + "_ctl0" + RowNumber + "_txtRemarks";}
        else 
            {
            start = _clientGridId + "_ctl" + RowNumber + "_txtStartingMarks";
            end = _clientGridId + "_ctl" + RowNumber + "_txtEndingMarks";
            grade = _clientGridId + "_ctl" + RowNumber + "_txtGradeName";
            remarks = _clientGridId + "_ctl" + RowNumber + "_txtRemarks";}
         
        if (obj.checked)
        {
            document.getElementById(start).disabled = false;
            document.getElementById(end).disabled = false;
            document.getElementById(grade).disabled = false;
            document.getElementById(remarks).disabled = false;
             document.getElementById(_clientSaveId).disabled = false; 
        }
        else 
        {
            document.getElementById(start).disabled = true;
            document.getElementById(end).disabled = true;
            document.getElementById(grade).disabled = true;
            document.getElementById(remarks).disabled = true;
        }
    }

    function ValidateInput(iPageCount, sActionName)
  { 
  
        var sMessage = "";
        var sStartEndNumberMessage ="";
        
        // Check if atleast one checkbox is checked.
        if (CheckIfAtleastOneCheckboxInGridIsSelected(document,_clientGridId,'ChkBoxDelete',sActionName,'false', iPageCount,'true'))
        {
            // Check if for checked rows start, end marks and grades are entered.
            sStartEndNumberMessage = CheckIfStartingEndingMarksAndGradeNameAreBlank();
            sMessage = sMessage + sStartEndNumberMessage;
            
            // If marks are entered then check if the starting marks are less than end marks.
            if (sMessage == "")
            {
                sStartEndNumberMessage = CheckIfStartingMarksAreGreaterThanEndMarks();
                sMessage = sMessage + sStartEndNumberMessage;
            }
            
            // If valid then check if the range specified is not overlaping.
            if (sMessage == "")
            {
                sStartEndNumberMessage = CheckIfGradeNameIsUnique();
                sMessage = sMessage + sStartEndNumberMessage;
                
                sStartEndNumberMessage = CheckIfSpecifiedRangeIsValid();
                sMessage = sMessage + sStartEndNumberMessage;
            }

            if (sMessage != "") {
                alert(document.getElementById("<%=hidPleaseFixFollowingErrors.ClientID %>").value + "\n" + sMessage);
                return false;
            }
            else 
            {
                return true;
            }
        }
}
        
function DisableButtons()
{
    if(document.getElementById(_clientSaveId)!= null)
    {
        document.getElementById(_clientSaveId).disabled = true ;
        document.getElementById(_clientbtnCancel).disabled = true ;
        document.getElementById(_clientddlStandards).disabled = true ;
    }
}

function CheckIfGradeNameExists(iStartRowIndex, sGradeNameToCheck) {
    var n = document.getElementById(_clientGridId).rows.length + 1;
    var grade, chk;
    var bResult = false;
    for(i=iStartRowIndex+1;i<n;i++)
    {
        RowNumber = i;
        if (RowNumber<10)
        {
            grade = _clientGridId + "_ctl0" + RowNumber + "_txtGradeName";
            chk = _clientGridId + "_ctl0" + RowNumber + "_ChkBoxDelete";
        }
        else 
        {
            grade = _clientGridId + "_ctl" + RowNumber + "_txtGradeName";
            chk = _clientGridId + "_ctl" + RowNumber + "_ChkBoxDelete";
        }
        
        if (document.getElementById(chk).checked)
        {
            if (trim(document.getElementById(grade).value) == trim(sGradeNameToCheck))
            {
                bResult = true; 
                break;
            }
        }
    }
    return bResult;
}

function trim(s) 
{ 
    var l=0; var r=s.length -1; 
    while(l < s.length && s[l] == ' ') 
    {     l++; } 
    while(r > l && s[r] == ' ') 
    {     r-=1;     } 
    return s.substring(l, r+1); 
} 


function CheckIfSpecifiedRangeIsValid()
{
    var message = "";
    message = message + CheckIfMinimumAndMaximumMarksAreValid();
    
    if (message == "")
        message = message + CheckIfStartMarksMatchesWithEndMarks();
    
    if (message == "")
        message = message + CheckIfAnyRangeIsMissing();
            
    if (message == "")
        message = message + CheckIfAnyRangeAreDuplicate();
        
    return message;
}

function CheckIfStartMarksMatchesWithEndMarks()
{
    var n = document.getElementById(_clientGridId).rows.length + 1;
    var start,end, chk;
    var startmarks, endmarks, chkSelected;
    var min=-1, max=-1;
    var sMessage="";
    
    for(i=iStartCnt;i<n;i++)
    {
        RowNumber = i;
        if (RowNumber<10)
        {
            start = _clientGridId + "_ctl0" + RowNumber + "_txtStartingMarks";
            end = _clientGridId + "_ctl0" + RowNumber + "_txtEndingMarks";
            chk = _clientGridId + "_ctl0" + RowNumber + "_ChkBoxDelete";
        }
        else 
        {
            start = _clientGridId + "_ctl" + RowNumber + "_txtStartingMarks";
            end = _clientGridId + "_ctl" + RowNumber + "_txtEndingMarks";
            chk = _clientGridId + "_ctl" + RowNumber + "_ChkBoxDelete";
        }
        
        if (document.getElementById(chk).checked)
        {
            if (document.getElementById(end) != null)
            {
                endmarks = document.getElementById(end).value;
                endmarks = parseInt(endmarks);
                if (CheckIfEndMarksEqualsStartMarks(endmarks))
                {
                    sMessage = "dsfd";
                    break;
                }
            }
        }
    }

    if (sMessage != "")
        sMessage ="\n"+ document.getElementById("<%=hidStartingPercentageEndingPercentageShouldNotBeSame.ClientID %>").value;
    return sMessage;
}

function CheckIfEndMarksEqualsStartMarks(iEndMarks)
{
    var n = document.getElementById(_clientGridId).rows.length + 1;
    var start, chk;
    var startmarks, chkSelected;
    var bResult = false;
    
    for(i=iStartCnt+1;i<n;i++)
    {
        RowNumber = i;
        if (RowNumber<10)
        {
            start = _clientGridId + "_ctl0" + RowNumber + "_txtStartingMarks";
            chk = _clientGridId + "_ctl0" + RowNumber + "_ChkBoxDelete";
        }
        else 
        {
            start = _clientGridId + "_ctl" + RowNumber + "_txtStartingMarks";
            chk = _clientGridId + "_ctl" + RowNumber + "_ChkBoxDelete";
        }
        
        if (document.getElementById(chk).checked)
        {
            startmarks = document.getElementById(start).value;
            if (parseInt(startmarks) == parseInt(iEndMarks))
            {
                bResult = true; 
                break;
            }
        }
    }
    return bResult;
}

function CheckIfAnyRangeAreDuplicate()
{
    var n = document.getElementById(_clientGridId).rows.length + 1;
    var start,end, chk;
    var startmarks,endmarks, chkSelected;
    var sStartMessage= "", sEndMessage= "";
    var min=-1, max=-1;
    var sMessage="";
    
    for(i=iStartCnt;i<n;i++)
    {
        RowNumber = i;
        if (RowNumber<10)
        {
            start = _clientGridId + "_ctl0" + RowNumber + "_txtStartingMarks";
            end = _clientGridId + "_ctl0" + RowNumber + "_txtEndingMarks";
            chk = _clientGridId + "_ctl0" + RowNumber + "_ChkBoxDelete";
        }
        else 
        {
            start = _clientGridId + "_ctl" + RowNumber + "_txtStartingMarks";
            end = _clientGridId + "_ctl" + RowNumber + "_txtEndingMarks";
            chk = _clientGridId + "_ctl" + RowNumber + "_ChkBoxDelete";
        }
        
        if (document.getElementById(chk).checked)
        {
            if (document.getElementById(end) != null)
            {
                startmarks = document.getElementById(start).value;
                startmarks = parseInt(startmarks);
            }
            
            if (document.getElementById(end) != null)
            {
                endmarks = document.getElementById(end).value;
                endmarks = parseInt(endmarks);
            }
            if (CheckIfStartEndRangeMatches(startmarks, endmarks,RowNumber)) {
                sMessage = "\n"+ document.getElementById("<%=hidDuplicatePercentageRangeAreNotAllowed.ClientID%>").value;
                break;
            }
        }
    }
    return sMessage;
}

function CheckIfAnyRangeOverLap()
{
    var n = document.getElementById(_clientGridId).rows.length + 1;
    var start,end, chk;
    var startmarks,endmarks, chkSelected;
    var sStartMessage= "", sEndMessage= "";
    var sMessage="";
    
    for(i=iStartCnt;i<n;i++)
    {
        RowNumber = i;
        if (RowNumber<10)
        {
            start = _clientGridId + "_ctl0" + RowNumber + "_txtStartingMarks";
            end = _clientGridId + "_ctl0" + RowNumber + "_txtEndingMarks";
            chk = _clientGridId + "_ctl0" + RowNumber + "_ChkBoxDelete";
        }
        else 
        {
            start = _clientGridId + "_ctl" + RowNumber + "_txtStartingMarks";
            end = _clientGridId + "_ctl" + RowNumber + "_txtEndingMarks";
            chk = _clientGridId + "_ctl" + RowNumber + "_ChkBoxDelete";
        }
        
        if (document.getElementById(chk).checked)
        {
            if (document.getElementById(end) != null)
                startmarks = document.getElementById(start).value;
            if (document.getElementById(end) != null)
                endmarks = document.getElementById(end).value;
            
            if (CheckIfMarksOverLap(startmarks)) {
                sMessage = "\n" + document.getElementById("<%=hidStartingPercentageOverlap.ClientID%>").value;
                break;
            }
            
            if (CheckIfMarksOverLap(endmarks))
            {
                sMessage = "\n" + document.getElementById("<%=hidEndingPercentageOverlap.ClientID%>").value;
                break;
            }
        }
    }
        
    return sMessage;
}

function CheckIfMarksOverLap(iMarks)
{
    var n = document.getElementById(_clientGridId).rows.length + 1;
    var start, chk, end;
    var startmarks, endmarks, chkSelected;
    var bResult = false;
    iMarks = parseInt(iMarks);
    
    for(j=iStartCnt;j<n;j++)
    {
        RowNum = j;
        if (RowNum<10)
        {
            start = _clientGridId + "_ctl0" + RowNum + "_txtStartingMarks";
            end = _clientGridId + "_ctl0" + RowNum + "_txtEndingMarks";
            chk = _clientGridId + "_ctl0" + RowNum + "_ChkBoxDelete";
        }
        else 
        {
            start = _clientGridId + "_ctl" + RowNum + "_txtStartingMarks";
            end = _clientGridId + "_ctl" + RowNum + "_txtEndingMarks";
            chk = _clientGridId + "_ctl" + RowNum + "_ChkBoxDelete";
        }
        
        if (document.getElementById(chk).checked)
        {
            startmarks = document.getElementById(start).value;
            endmarks = document.getElementById(end).value;
            if (parseInt(startmarks) >= iMarks &&  iMarks <= parseInt(endmarks) )
            {
                bResult = true; 
                break;
            }
        }
    }
    return bResult;
}

function CheckIfAnyRangeIsMissing()
{
    var n = document.getElementById(_clientGridId).rows.length + 1;
    var start,end, chk;
    var startmarks,endmarks, chkSelected;
    var sStartMessage= "", sEndMessage= "";
    var min=-1, max=-1;
    var sMessage="";
    
    for(i=iStartCnt;i<n;i++)
    {
        RowNumber = i;
        if (RowNumber<10)
        {
            start = _clientGridId + "_ctl0" + RowNumber + "_txtStartingMarks";
            end = _clientGridId + "_ctl0" + RowNumber + "_txtEndingMarks";
            chk = _clientGridId + "_ctl0" + RowNumber + "_ChkBoxDelete";
        }
        else 
        {
            start = _clientGridId + "_ctl" + RowNumber + "_txtStartingMarks";
            end = _clientGridId + "_ctl" + RowNumber + "_txtEndingMarks";
            chk = _clientGridId + "_ctl" + RowNumber + "_ChkBoxDelete";
        }
        
        if (document.getElementById(chk).checked)
        {
            if (document.getElementById(end) != null)
            {
                endmarks = document.getElementById(end).value;
                endmarks = parseInt(endmarks);
            }
            if (endmarks != 100) 
            {
                if (CheckIfEndRangeIsMissing(endmarks)) {
                    sMessage = "\n" + document.getElementById("<%=hidSomeOfTheStartingEndingPerRangeAreMissing.ClientID%>").value;
                    break;
                }
            }
        }
    }
        
    return sMessage;
}

function CheckIfEndRangeIsMissing(iEndMarks)
{
    var n = document.getElementById(_clientGridId).rows.length + 1;
    var start, chk, end;
    var startmarks, chkSelected;
    var bResult = true;
    iEndMarks = parseInt(iEndMarks) +1 ;
    
    for(j=iStartCnt;j<n;j++)
    {
        RowNum = j;
        if (RowNum<10)
        {
            start = _clientGridId + "_ctl0" + RowNum + "_txtStartingMarks";
            end = _clientGridId + "_ctl0" + RowNumber + "_txtEndingMarks";
            chk = _clientGridId + "_ctl0" + RowNum + "_ChkBoxDelete";
        }
        else 
        {
            start = _clientGridId + "_ctl" + RowNum + "_txtStartingMarks";
            end = _clientGridId + "_ctl" + RowNumber + "_txtEndingMarks";
            chk = _clientGridId + "_ctl" + RowNum + "_ChkBoxDelete";
        }
        
        if (document.getElementById(chk).checked)
        {
            startmarks = document.getElementById(start).value;
            if (parseInt(startmarks) == iEndMarks)
            {
                bResult = false; 
                break;
            }
        }
    }
    return bResult;
}

function CheckIfStartEndRangeMatches(iStartMarks, iEndMarks, iStartIndex)
{
    var n = document.getElementById(_clientGridId).rows.length + 1;
    var start, end, chk;
    var startmarks, chkSelected;
    var bResult = false;
       
    for(j=iStartIndex+1;j<n;j++)
    {
        RowNum = j;
        if (RowNum<10)
        {
            start = _clientGridId + "_ctl0" + RowNum + "_txtStartingMarks";
            end = _clientGridId + "_ctl0" + RowNum + "_txtEndingMarks";
            chk = _clientGridId + "_ctl0" + RowNum + "_ChkBoxDelete";
        }
        else 
        {
            start = _clientGridId + "_ctl" + RowNum + "_txtStartingMarks";
            end = _clientGridId + "_ctl" + RowNum + "_txtEndingMarks";
            chk = _clientGridId + "_ctl" + RowNum + "_ChkBoxDelete";
        }
        
        if (document.getElementById(chk).checked)
        {
            endmarks = document.getElementById(end).value;
            startmarks = document.getElementById(start).value;
            if (parseInt(startmarks) == iStartMarks && parseInt(iEndMarks) == endmarks)
            {
                bResult = true; 
                break;
            }
        }
    }
    return bResult;
}

function CheckIfMinimumAndMaximumMarksAreValid()
{
    var n = document.getElementById(_clientGridId).rows.length + 1;
    var start,end, chk;
    var startmarks,endmarks, chkSelected;
    var sStartMessage= "", sEndMessage= "";
    var min=-1, max=-1;
    var sMessage="";
    
    for(i=iStartCnt;i<n;i++)
    {
        RowNumber = i;
        if (RowNumber<10)
        {
            start = _clientGridId + "_ctl0" + RowNumber + "_txtStartingMarks";
            end = _clientGridId + "_ctl0" + RowNumber + "_txtEndingMarks";
            chk = _clientGridId + "_ctl0" + RowNumber + "_ChkBoxDelete";
        }
        else 
        {
            start = _clientGridId + "_ctl" + RowNumber + "_txtStartingMarks";
            end = _clientGridId + "_ctl" + RowNumber + "_txtEndingMarks";
            chk = _clientGridId + "_ctl" + RowNumber + "_ChkBoxDelete";
        }
        
        if (document.getElementById(chk).checked)
        {
            if (document.getElementById(start) != null)
            {
                startmarks = document.getElementById(start).value;
                startmarks = parseInt(startmarks);
            }
            
            if (document.getElementById(end) != null)
            {
                endmarks = document.getElementById(end).value;
                endmarks = parseInt(endmarks);
            }
            
            if (startmarks < min || min == -1)
                min = startmarks;
            if (endmarks > max || max == -1)
                max = endmarks;
        }
    }

    if (min != 0)
        sMessage = sMessage + "\n" + document.getElementById("<%=hidMinimumStartingPerShouldBe.ClientID%>").value;

    if (max != 100)
        sMessage = sMessage + "\n" + document.getElementById("<%=hidMaximumEndingPerShouldBe.ClientID%>").value;
    
    return sMessage;
}

function CheckIfStartingEndingMarksAndGradeNameAreBlank() {
        var n = document.getElementById(_clientGridId).rows.length + 1;
        var grade, start,end, chk, remarks;
        var gradename, startmarks,endmarks, chkSelected,remarksText;
        var sStartMessage= "", sEndMessage= "", sGradeMessage="", sRemarkMessage="";
               
        var sMessage="";
        
        for(i=iStartCnt;i<n;i++)
        {
            RowNumber = i;
            if (RowNumber<10)
            {
                start = _clientGridId + "_ctl0" + RowNumber + "_txtStartingMarks";
                end = _clientGridId + "_ctl0" + RowNumber + "_txtEndingMarks";
                grade = _clientGridId + "_ctl0" + RowNumber + "_txtGradeName";
                chk = _clientGridId + "_ctl0" + RowNumber + "_ChkBoxDelete";
                remarks = _clientGridId + "_ctl0" + RowNumber + "_txtRemarks";
            }
            else 
            {
                start = _clientGridId + "_ctl" + RowNumber + "_txtStartingMarks";
                end = _clientGridId + "_ctl" + RowNumber + "_txtEndingMarks";
                grade = _clientGridId + "_ctl" + RowNumber + "_txtGradeName";
                chk = _clientGridId + "_ctl" + RowNumber + "_ChkBoxDelete";
                remarks = _clientGridId + "_ctl" + RowNumber + "_txtRemarks";
            }
            
            if (document.getElementById(chk).checked)
            {
                var j;
                j = RowNumber -1;
                
                if (document.getElementById(start) != null)
                {
                    startmarks = document.getElementById(start).value;
                    if (startmarks == "")
                        sStartMessage = sStartMessage + document.getElementById("<%=hidRowNumber.ClientID%>").value + " "+ j + " ";
                }
                
                if (document.getElementById(end) != null)
                {
                    endmarks = document.getElementById(end).value;
                    if (endmarks == "")
                        sEndMessage = sEndMessage + document.getElementById("<%=hidRowNumber.ClientID%>").value + " " + j + " ";
                }
                
                if (document.getElementById(grade) != null)
                {
                    gradename = document.getElementById(grade).value;
                    if (gradename == "")
                        sGradeMessage = sGradeMessage + document.getElementById("<%=hidRowNumber.ClientID%>").value + " " + j + " ";
                }
                if (document.getElementById(remarks) != null)
                {
                    remarksText = document.getElementById(remarks).value;
                    if (remarksText == "")
                        sRemarkMessage = sRemarkMessage + document.getElementById("<%=hidRowNumber.ClientID%>").value + " " + j + " ";
                }
            }
        }

        if (sStartMessage != "")
            sMessage = sMessage + document.getElementById("<%=hidStartingPerForFollowingRowsShouldNotBeBlank.ClientID%>").value + sStartMessage + "\n";
            
        if (sEndMessage != "")
            sMessage = sMessage + document.getElementById("<%=hidEndingPerForFollowingRowsShouldNotBeBlank.ClientID%>").value + sEndMessage + "\n";

        if (sGradeMessage != "")
            sMessage = sMessage + document.getElementById("<%=hidGradeNameForFollowingRowsShouldNotBeBlank.ClientID%>").value + sGradeMessage + "\n";

        if (sRemarkMessage != "")
            sMessage = sMessage + document.getElementById("<%=hidRemarksForFollowingRowsShouldNotBeBlank.ClientID%>").value + sRemarkMessage + "\n";
        
        return sMessage;
}

function CheckIfStartingMarksAreGreaterThanEndMarks() 
{


        var n = document.getElementById(_clientGridId).rows.length + 1;
        var start,end,chk;
        var startmarks,endmarks, chkSelected;
        var sStartMessage= "";
        var sMessage="";
        
        for(i=iStartCnt;i<n;i++)
        {
            RowNumber = i;
            if (RowNumber<10)
            {
                start = _clientGridId + "_ctl0" + RowNumber + "_txtStartingMarks";
                end = _clientGridId + "_ctl0" + RowNumber + "_txtEndingMarks";
                chk = _clientGridId + "_ctl0" + RowNumber + "_ChkBoxDelete";
            }
            else 
            {
                start = _clientGridId + "_ctl" + RowNumber + "_txtStartingMarks";
                end = _clientGridId + "_ctl" + RowNumber + "_txtEndingMarks";
                chk = _clientGridId + "_ctl" + RowNumber + "_ChkBoxDelete";
            }
            
            if (document.getElementById(chk).checked)
            {
                var j ;
                j = RowNumber -2;
                
                if (document.getElementById(start) != null)
                {
                    startmarks = document.getElementById(start).value;
                    if (startmarks != "")
                        startmarks =parseInt(startmarks);
                }
                
                if (document.getElementById(end) != null)
                {
                    endmarks = document.getElementById(end).value;
                    if (endmarks != "")
                        endmarks = parseInt(endmarks);
                }
                
                if (startmarks > endmarks)
                    sStartMessage = sStartMessage + document.getElementById("<%=hidRowNumber.ClientID%>").value + j;
            }
        }
        
        if (sStartMessage != "")
            sMessage = sMessage + document.getElementById("<%=hidStartingPerForFollowingRowsShouldBeLessThanEndingPer.ClientID%>").value + sStartMessage;
        
        return sMessage;
    }
    function CheckIfGradeNameIsUnique() {
    var n = document.getElementById(_clientGridId).rows.length + 1;
    var grade, chk;
    var gradename, chkSelected;
    var sGradeMessage="";
    var sMessage = "";
    for(j=2;j<n;j++)
    {
        RowNumber = j;
        if (RowNumber<10)
        {
            grade = _clientGridId + "_ctl0" + RowNumber + "_txtGradeName";
            chk = _clientGridId + "_ctl0" + RowNumber + "_ChkBoxDelete";
        }
        else 
        {
            grade = _clientGridId + "_ctl" + RowNumber + "_txtGradeName";
            chk = _clientGridId + "_ctl" + RowNumber + "_ChkBoxDelete";
        }
        
        if (document.getElementById(chk).checked)
        {
            if (document.getElementById(grade) != null)
            {
               if (CheckIfGradeNameExists(RowNumber, document.getElementById(grade).value))
               {
                sGradeMessage = "duplicate";
                break;
               }
            }
        }
    }
if (sGradeMessage != "")
    sMessage = sMessage + "\n" + document.getElementById("<%=hidDuplicateGradeNameIsNotAllowed.ClientID%>").value;
    return sMessage;
}

function getStartIndex(abPaging)
{
    var iStart;
    
    if(abPaging == "true") 
    {
        iStart = 3;
    }
    else 
    {
        iStart = 2;
    }
    return iStart;
}

function ClearErrorLabels() {
    if ($get("<%=this.lblErrors.ClientID %>") != null)
        $get("<%=this.lblErrors.ClientID %>").innerHTML = "";
}
function CheckExamConfiguration() {
    if (document.getElementById(_clienthidConfigDependancy).value == "Y") {
        $get("<%=this.lblErrors.ClientID %>").innerHTML = document.getElementById("<%=hidPercentageGradeForStandard.ClientID%>").value + "-" + $get("<%=this.ddlStandards.ClientID %>").options[$get("<%=this.ddlStandards.ClientID %>").selectedIndex].text + " "+ document.getElementById("<%=hidCanNotBeModifiedAsExamConfigurationAlreadyDone.ClientID%>").value;
        return false;
    }
    return true;
}
function OnGridKeyUp(obj, e) {
    UpDownKeyPress(obj.id, e);
}

function OnGridKeyUpNumber(obj, decimalPlaces, allowNegative, e) {
    extractNumber(obj, decimalPlaces, allowNegative);
    UpDownKeyPress(obj.id, e);
}
  
    </script>                   
</asp:Content>
 