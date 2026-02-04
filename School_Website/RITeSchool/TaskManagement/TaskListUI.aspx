<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="TaskListUI.aspx.cs" Inherits="TaskListUI" Title="Untitled Page" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" style="width: 95%">
        <tr align="left">
            <td align="left">
                <asp:ValidationSummary ID="valSumTaskDetails" CssClass="LblErrorMsg" ShowSummary="true"
                    runat="server" ValidationGroup="TaskList" />
            </td>
        </tr>
        <tr align="center">
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <table style="vertical-align: top; width: 900px;" align="center">
                            <tr>
                                <td style="width: 200px;">
                                    &nbsp;
                                </td>
                                <td align="right" style="padding-right: 10px; width: 250px;">
                                    <asp:RadioButton CssClass="ClsLabel" ID="optAssignedTo" Text="Assigned To" runat="server"
                                        Width="223px" TabIndex="1" GroupName="A" Checked="True" AutoPostBack="True" OnCheckedChanged="optAssignedTo_CheckedChanged" />
                                </td>
                                <td align="left" style="width: 200px;">
                                    <asp:RadioButton ID="optAssignedBy" CssClass="ClsLabel" Text="Assigned By" runat="server"
                                        Width="129px" GroupName="A" AutoPostBack="True" OnCheckedChanged="optAssignedBy_CheckedChanged" />
                                </td>
                                <td align="left" style="padding-right: 10px; width: 250px;">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px;">
                                    &nbsp;
                                </td>
                                <td align="left" style="padding-right: 10px; width: 250px;">
                                    &nbsp;
                                </td>
                                <td style="width: 200px;">
                                    &nbsp;
                                </td>
                                <td align="left" style="padding-right: 10px; width: 250px; text-align: center;">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="ClspaddingL">
                                <td class="ClsBorderlight" style="width: 575px; height: 32px; ">
                                    <span id="Span5" class="paddingLSML" >Role :</span>
                                </td>
                                <td align="left" style="padding-left: 5px; width: 250px; height: 32px;">
                                    <div>
                                        <asp:DropDownList ID="cmbDesignation" runat="server" Width="155px" AutoPostBack="True"
                                            OnSelectedIndexChanged="cmbDesignation_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td class="ClsBorderlight" style="width: 200px; height: 32px;">
                                    <span id="Span3" class="paddingLSML" >Resource :</span>
                                </td>
                                <td align="left" style="padding-left: 5px; width: 200px; height: 30px;">
                                    <div>
                                        <asp:DropDownList ID="cmbUser" runat="server" Width="155px" >
                                            <asp:ListItem Value="0">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="ClsBorderlight"  style="width: 200px; height: 32px; ">
                                    <span id="Span13" class="paddingLSML" >Task Type :</span>
                                </td>
                                <td style="width: 216px; padding-left: 5px; height: 30px;">
                                    <asp:DropDownList ID="cmbTaskType" runat="server" Width="155px" >
                                    </asp:DropDownList>
                                </td>
                                <td class="ClsBorderlight" style="width: 299px; height: 30px; ">
                                    <span id="Span1" class="paddingLSML" >Status :</span>
                                </td>
                                <td style="width: 168px; padding-left: 5px; height: 30px;">
                                    <asp:DropDownList ID="cmbStatus" runat="server" Width="155px" >
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="width: 32px;">
                                <td class="ClsBorderlight" style="width: 200px; height: 32px; ">
                                    <span class="paddingLSML" >Start Date and Time : </span>
                                </td>
                                <td style="padding-left: 3px; width: 250px;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtFromDate" MaxLength="11" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                                <rjs:PopCalendar ID="calFromDate" runat="server" Control="txtFromDate" ShowErrorMessage="false"
                                                    InvalidDateMessage="Please select valid Start Date." Format="dd MMM yyyy" ShowWeekend="True" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFromTime" MaxLength="8" runat="server" Width="62px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="ClsBorderlight" style="width: 200px; height: 32px; ">
                                    <span class="paddingLSML">End Date and Time : </span>
                                </td>
                                <td style="padding-left: 3px; width:250px; height: 32px;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtToDate" MaxLength="11" runat="server" Style="margin-left: 0px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <rjs:PopCalendar ID="calToDate" runat="server" Control="txtToDate" ShowErrorMessage="false"
                                                    InvalidDateMessage="Please select valid Start Date." Format="dd MMM yyyy" ShowWeekend="True" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtToTime" MaxLength="8" runat="server" Style="margin-left: 0px"
                                                    Width="62px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:CustomValidator ID="cstInvalidStartTime" CssClass="LblErrorMsg" runat="server"
                                        SetFocusOnError="True" Display="None" ErrorMessage="Please enter valid start time e.g. 10:00 AM."
                                        ClientValidationFunction="IsValidStartTime" ValidationGroup="TaskList"></asp:CustomValidator>
                                    <span class="LblNormal" style="padding-left:7px;">e.g 10:10 AM </span>
                                </td>
                                <td align="right" colspan="2">
                                    <asp:CustomValidator ID="cstInvalidEndTime" CssClass="LblErrorMsg" runat="server"
                                        SetFocusOnError="True" ErrorMessage="Please enter valid end time e.g. 10:00 AM."
                                        Display="None" ClientValidationFunction="IsValidEndTime" ValidationGroup="TaskList"></asp:CustomValidator>
                                    <span class="LblNormal" style="padding-left:7px">e.g 10:10 AM </span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                </td>
                                <td colspan="2" align="right">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 575px">
                                    &nbsp;
                                </td>
                                <td style="width: 216px">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button CssClass="ClsBtn" ID="btnSearch" ValidationGroup="TaskList" CausesValidation="true"
                    runat="server" Text="Search" OnClick="btnSearch_Click" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <table id="tblListView" style="width: 100%;">
                    <tr align="center">
                        <td align="center">
                            <div id="DivWorkFolwDetailList" runat="server" visible="true" class="GridBorder"
                                style="overflow: scroll; height: 210px; width: 1100px">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:ListView ID="lstvwTaskList" runat="server" Style="text-align: left" OnItemDataBound="lstvwTaskList_ItemDataBound"
                                            DataKeyNames="StartTime,EndTime,TaskId,TaskAssignerUserId,TaskStatusId,AssignedToUserId,TaskTypeId,TaskDetailsId"
                                            OnItemCommand="lstvwTaskList_ItemCommand" OnSorting="lstvwTaskList_Sorting">
                                            <LayoutTemplate>
                                                <table runat="server" id="tblTaskList" style="color: #333333" width="1200px" cellpadding="0"
                                                    cellspacing="1" class="GridBorder" align="center">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="center" width="100px">
                                                            Sr. No.
                                                        </th>
                                                        <th align="left" class="paddingLSML" width="450px">
                                                            <asp:LinkButton ID="lnkbtnTaskName" runat="server" CommandName="Sort" CommandArgument="TaskName"
                                                                CausesValidation="false" ForeColor="Black">Task Name
                                                            </asp:LinkButton>
                                                        </th>
                                                        <th align="left" class="paddingLSML" width="165px">
                                                            <asp:LinkButton ID="lnkbtnTaskType" runat="server" CommandName="Sort" CommandArgument="TaskType"
                                                                CausesValidation="false" ForeColor="Black"> Task Type </asp:LinkButton>
                                                        </th>
                                                        <th align="left" width="450px" class="paddingLSML">
                                                            <asp:LinkButton ID="lnkbtnUerName" runat="server" CommandName="Sort" CommandArgument="UserName"
                                                                CausesValidation="false" ForeColor="Black">
                                                            User Name</asp:LinkButton>
                                                        </th>
                                                        <th align="center" width="450px">
                                                            <asp:LinkButton ID="lnkbtnStartDate" runat="server" CommandName="Sort" CommandArgument="StartDate"
                                                                CausesValidation="false" ForeColor="Black"> Start Date and Time </asp:LinkButton>
                                                        </th>
                                                        <th align="center"  width="450px">
                                                            <asp:LinkButton ID="lnkbtnEndDate" runat="server" CommandName="Sort" CommandArgument="EndDate"
                                                                CausesValidation="false" ForeColor="Black"> 
                                                            End Date and Time</asp:LinkButton>
                                                        </th>
                                                        <th align="left" class="paddingLSML" width="150px">
                                                            <asp:LinkButton ID="lnkbtnStatus" runat="server" CommandName="Sort" CommandArgument="StatusName"
                                                                CausesValidation="false" ForeColor="Black"> Status</asp:LinkButton>
                                                        </th>
                                                        <th align="center" width="50px" class="paddingLSML">
                                                            Edit
                                                        </th>
                                                        <th align="center" width="50px" id="thDelete" runat="server" class="paddingLSML">
                                                            Delete
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server" style="width:90px;" class="ClsGridRow" align="center">
                                                    <td align="center">
                                                        <asp:Label ID="lblNo" runat="server" class="paddingLSML"/>
                                                    </td>
                                                    <td align="left" class="paddingLSML" style="width:450px">
                                                        <asp:Label ID="lblTask" runat="server" Text='<%# Eval("TaskName") %>' />
                                                    </td>
                                                    <td align="left" class="paddingLSML" style="width:160px">
                                                        <asp:Label ID="lblTaskType" runat="server" Text='<%# Eval("TaskType") %>' />
                                                    </td>
                                                    <td align="left" class="paddingLSML" style="width:450px">
                                                        <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>' />
                                                    </td>
                                                    <td align="center" style="width:300px">
                                                        <asp:Label ID="lblStartDateTime" runat="server" Text='<%# Eval("StartDate","{0:dd-MMM-yyyy}") %>' />
                                                    </td>
                                                    <td align="center" style="width:300px">
                                                        <asp:Label ID="lblEndDateTime" runat="server" Text='<%# Eval("EndDate","{0:dd-MMM-yyyy}") %>' />
                                                    </td>
                                                    <td align="left" class="paddingLSML">
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("StatusName") %>' />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                    </td>
                                                    <td id="tdDelete" runat="server" align="center">
                                                        <asp:ImageButton ID="imgBtnDelete" runat="server" CommandName="RemoveCommand" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr2" runat="server" style="width:90px;" class="ClsGridAltRow" align="center">
                                                    <td align="center">
                                                        <asp:Label ID="lblNo" runat="server" class="paddingLSML"/>
                                                    </td>
                                                    <td align="left" class="paddingLSML" style="width:450px">
                                                        <asp:Label ID="lblTask" runat="server" Text='<%# Eval("TaskName") %>' />
                                                    </td>
                                                    <td align="left" class="paddingLSML" style="width:160px">
                                                        <asp:Label ID="lblTaskType" runat="server" Text='<%# Eval("TaskType") %>' />
                                                    </td>
                                                    <td align="left" class="paddingLSML"  style="width:160px">
                                                        <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>' />
                                                    </td>
                                                    <td align="center" style="width:300px">
                                                        <asp:Label ID="lblStartDateTime" runat="server" Text='<%# Eval("StartDate","{0:dd-MMM-yyyy}") %>' />
                                                    </td>
                                                    <td align="center" style="width:300px">
                                                        <asp:Label ID="lblEndDateTime" runat="server" Text='<%# Eval("EndDate","{0:dd-MMM-yyyy}") %>' />
                                                    </td>
                                                    <td align="left" class="paddingLSML">
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("StatusName") %>' />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                    </td>
                                                    <td id="tdDelete" runat="server" align="center">
                                                        <asp:ImageButton ID="imgBtnDelete" runat="server" CommandName="RemoveCommand" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td class="LblNoRecord" align="center">
                                                            No Records Found.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                        <asp:HiddenField ID="hidStatusId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidTaskTypeId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidTaskAssignerUserId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidAssignedToUserId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidTaskDetailId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidTaskId" runat="server" Value="0" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="optAssignedBy" EventName="CheckedChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="optAssignedTo" EventName="CheckedChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnAddNewTask" CssClass="ClsBtn" runat="server" Text="Add New Task"
                    Width="120px" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="upnl2" runat="server">
                    <ContentTemplate>
                        <asp:HiddenField ID="hidSortDirection" runat="server" Value="0" />
                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lstvwTaskList" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
      
        <asp:HiddenField ID="hidMode" runat="server" />
    </table>

    <script language="javascript" type="text/javascript">

        _ClienttxtFromTime = "<%=this.txtFromTime.ClientID%>";
        _ClienttxtToTime = "<%=this.txtToTime.ClientID%>";


        function isTimeValid(result) {

            var timeStr = document.getElementById(result).value;
            if (trimAll(timeStr) == '')
                return false;

            var timePat = /^(\d{1,2}):(\d{2})(:(\d{2}))?(\s)(AM|am|PM|pm)?$/;
            var matchArray = timeStr.match(timePat);

            if (matchArray == null)
                return false;

            hour = matchArray[1];
            minute = matchArray[2];
            second = matchArray[4];
            ampm = matchArray[6];

            if (second == "") { second = null; }
            if (ampm == "") { ampm = null; }

            if (hour < 0 || hour > 12)
                return false;

            if (minute < 0 || minute > 59)
                return false;

            if (second != null && (second < 0 || second > 59))
                return false;

            var str;
            if (hour.length == 1)
                str = '0' + hour;
            else
                str = hour;
            if (minute.length == 1)
                str = str + ':' + '0' + minute;
            else
                str = str + ':' + minute;

            str = str + ' ' + ampm.toUpperCase();

            document.getElementById(result).value = str;
            return true;
        }

        function IsValidStartTime(oSrc, args) {

            if (document.getElementById(_ClienttxtFromTime).value != '') {
                if (!isTimeValid(_ClienttxtFromTime)) {
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }
            args.IsValid = true;
            return false;
        }
        
        function IsValidEndTime(oSrc, args) {

            if (document.getElementById(_ClienttxtToTime).value != '') {
                if (!isTimeValid(_ClienttxtToTime)) {
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }
            args.IsValid = true;
            return false;
        }



        function OpenAddNewTaskPopup(sQueryString) {

            window.open('TaskManagementPopup.aspx?' +
                sQueryString, '_new', 'scrollbars=yes,resizable=yes,top=0,left=0,width=850,height=600');
            return false;
        }

        function ConfirmRemove() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
