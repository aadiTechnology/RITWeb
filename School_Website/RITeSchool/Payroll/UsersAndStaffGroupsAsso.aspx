<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" EnableViewStateMac="true" EnableEventValidation="false"
    CodeFile="UsersAndStaffGroupsAsso.aspx.cs" Inherits="UserAndStaffGroupAsso" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnl1" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr align="center" id="trValSummary">
                                    <td align="center">
                                        <asp:ValidationSummary ID="valSum" CssClass="LblErrorMsg" ShowSummary="true" runat="server"
                                            ValidationGroup="Save" />
                                        <asp:CustomValidator ID="cstPermanentDateValidations1" Display="None" runat="server"
                                            ValidationGroup="Save" CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage=""
                                            SetFocusOnError="True" ClientValidationFunction="PermanentDateValidations1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstPermanentDateValidations" Display="None" runat="server"
                                            ValidationGroup="Save" CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage=""
                                            SetFocusOnError="True" ClientValidationFunction="PermanentDateValidations"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator1" Display="None" runat="server"
                                            ValidationGroup="Save" CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage=""
                                            SetFocusOnError="True" ClientValidationFunction="ValidateTransferDate"></asp:CustomValidator>
                                         <asp:CustomValidator ID="CustomValidator2" Display="None" runat="server"
                                            ValidationGroup="Save" CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage=""
                                            SetFocusOnError="True" ClientValidationFunction="TransferDateValidations"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator3" Display="None" runat="server"
                                            ValidationGroup="Save" CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage=""
                                            SetFocusOnError="True" ClientValidationFunction="TransferDateValidations1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstDate" Display="None" runat="server" ValidationGroup="Save"
                                            CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage="" SetFocusOnError="True"
                                            ClientValidationFunction="DateValidations"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstResigDate" Display="None" runat="server" ValidationGroup="Save"
                                            CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage="" SetFocusOnError="True"
                                            ClientValidationFunction="ResignDateValidation"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstResignationDate" Display="None" runat="server" ValidationGroup="Save"
                                            CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage="" SetFocusOnError="True"
                                            ClientValidationFunction="ResignationDateValidations"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr align="center" id="trErrorMessage" runat="server" visible="false">
                                    <td align="left" style="width: 100%">
                                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="ClsMdtStar" EnableViewState="False"
                                            ForeColor="Red"></asp:Label>
                                        <table align="center">
                                            <caption>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lblMessage" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                            Font-Bold="True" ForeColor="Blue" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </caption>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table id="LegendTable" runat="server">
                                            <tr>
                                                <td align="left" colspan="1">
                                                    <span class="ClsLblLgnd"><b>Legend</b> </span>
                                                </td>
                                                <td align="right" colspan="1">
                                                    <asp:Image ID="img1" runat="server" ImageUrl="~/RITeSchool/images/Icon_UserUnlock.gif"
                                                        Border="0" Width="20px" />
                                                </td>
                                                <td align="left" colspan="1">
                                                    <span class="ClsTextNormal"><b>Activate</b> </span>
                                                </td>
                                                <td align="right" colspan="1">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/RITeSchool/images/Icon_UserLock.gif"
                                                        Border="0" Width="20px" />
                                                </td>
                                                <td align="left" colspan="1">
                                                    <span class="ClsTextNormal"><b>Deactivate</b> </span>
                                                </td>
                                                <td align="center" valign="middle" style="border: 1px solid #000000;">
                                                    <asp:Label ID="Label7" runat="server" BackColor="White" CssClass="ClsLblLgnd" Text="Deleted User"
                                                        ForeColor="Red" Font-Bold="False" BorderStyle="None" BorderWidth="1px" ReadOnly="True"
                                                        Width="100px" EnableViewState="False"></asp:Label>
                                                </td>
                                                <td align="center" valign="middle" class="LblNormal" style="border: 1px solid #000000; padding-right : 4px;">
                                                    <b>E/D - Earning/Deduction</b>
                                                </td>
                                                <td align="center" valign="middle" class="LblNormal" style="border: 1px solid #000000; padding-right : 4px;">
                                                    <b>Y/L - Yearwise Leave</b>
                                                </td>
                                                <td align="center" valign="middle" class="LblNormal" style="border: 1px solid #000000; padding-right : 4px;">
                                                    <b>L/E - Leave Encashment</b>
                                                </td>
                                                <td align="center" valign="middle" class="LblNormal" style="border: 1px solid #000000; padding-right : 4px;">
                                                    <b>Act/DAct - Activate/Deactivate</b>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trRole" runat="server">
                                    <td align="center">
                                        <table align="center">
                                        <tr class="Height10">
                                            <td></td>
                                        </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderLight">
                                                    <span id="lblUserRole" class="ClsLabel">User Role:</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbUserRoles" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbUserRoles_SelectedIndexChanged" onchange="Page_BlockSubmit = false;">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="left" class="ClsBorderLight">
                                                    <span id="Span1" class="ClsLabel">User Name: </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtUserName" runat="server" autocomplete="off" Width="200px" MaxLength="100"></asp:TextBox>
                                                </td>


                                               
                                                 </tr>
                                                <tr align="center">
                                                <td align="left" class="ClsBorderLight">
                                                     <asp:Label ID="Label6" runat="server" class="ClsLabel" Text="User Type"></asp:Label>
                                                     <span class="ClsLabel colonPadding">:</span>
                                                </td>
                                                <td align="left">
                                                     <asp:DropDownList ID="ddlUserType" runat="server" AutoPostBack="true" CssClass="MidCombo"  OnSelectedIndexChanged="ddlUserType_SelectedIndexChanged">                                                                                        
                                                     </asp:DropDownList>
                                                </td>
                                                 <td align="left" class="ClsBorderLight">
                                                    <span id="Span2" class="ClsLabel">Sort By Staff Name: </span>
                                                </td>
                                                <td>

                                                    <asp:RadioButton ID="RdbWithSalutation" Text="With Salutation" 
                                                        runat="server" GroupName="Show" AutoPostBack="True" 
                                                        oncheckedchanged="RdbWithSalutation_CheckedChanged"/>
                                                    
                                                    <asp:RadioButton ID="RdbWithoutSalutation" Text="Without Salutation" 
                                                        runat="server" GroupName="Show" AutoPostBack="True" 
                                                        oncheckedchanged="RdbWithoutSalutation_CheckedChanged"/>

                                                </td>

                                           


                                            </tr>
                                            <tr>
                                                <td colspan="4" align="center">
                                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn" BorderWidth="1px"
                                                                    CausesValidation="false" OnClick="btnSearch_Click" />
                                                </td>
                                            </tr>                                            
                                            <tr>
                                                <td colspan="4" align="center">
                                                <table>
                                                    <tr>                                                    
                                                        <td id="tdStaffStatus" valign="middle" >
                                                            <div style="width: 110px; height: 18px; vertical-align: bottom; padding-top: 4px"
                                                                class="ClsGreenBG">
                                                                <asp:LinkButton ID = "hlnkStaffStatus" runat = "server" CssClass="SubTitle"  
                                                                    EnableViewState="False"  >Service Type</asp:LinkButton>
                                                        
                                                            </div>
                                                        </td>
                                                        <td id="tdEdPercentage" valign="middle">
                                                            <div style="width: 250px; height: 18px; vertical-align: bottom; padding-top: 4px"
                                                                class="ClsGreenBG">
                                                                <asp:HyperLink ID="HyperLink1" runat="server" CssClass="SubTitle" NavigateUrl="~/RITeSchool/Payroll/GrossSalaryDetailsUI.aspx"
                                                                    EnableViewState="False">User - Payment Category Association</asp:HyperLink>
                                                            </div>
                                                        </td>
                                                         <td align="left" style="height: 18px" class="ClsGreenBG">
                                                            <asp:LinkButton ID="lnkCarryForword" runat="server" ViewStateMode="Enabled" Text="Users Joining Details"
                                                                CssClass="SubTitle"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height: 10px;">
                                    <td> 
                                    </td>
                                </tr>
                                 <tr id="trUserType" runat="server" visible="false">
                                    <td align="center">
                                        <table align="center">
                                            
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height: 10px;">
                                    <td>
                                    </td>
                                </tr>
                                <tr id="trPagerUserStaffGrAss" runat="server">
                                    <td align="center">
                                        <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwAssociation">
                                            <Fields>
                                                <asp:TemplatePagerField>
                                                    <PagerTemplate>
                                                        <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                        <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                        <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                        <br />
                                                    </PagerTemplate>
                                                </asp:TemplatePagerField>
                                            </Fields>
                                        </asp:DataPager>
                                    </td>
                                </tr>
                                <tr id="trlistview" runat="server" align="center">
                                    <td align="center">
                                        <asp:ListView ID="lstvwAssociation" runat="server" EnableViewState="true" DataKeyNames="UsersStaffGroupsAssociationId,User_Id,UserName,Is_Locked,IsDeleted,StaffGroupId"
                                            OnItemDataBound="lstvwAssociation_ItemDataBound" OnItemCommand="lstvwAssociation_ItemCommand"
                                            OnDataBound="lstvwAssociation_DataBound">
                                            <LayoutTemplate>
                                                <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                    cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="center" class="locked">
                                                        </th>
                                                        <th align="left" class="locked">
                                                            Staff Name
                                                        </th>
                                                        <th align="left" class="locked">
                                                            Staff Group
                                                        </th>
                                                        <th align="right" class="locked">
                                                            Employee No.
                                                        </th>
                                                        <th align="right" class="locked">
                                                            Account No.
                                                        </th>
                                                        <th align="right" class="locked">
                                                            P.F. No.
                                                        </th>
                                                         <th align="right" class="locked">
                                                            UAN
                                                        </th>
                                                        <th align="right" class="locked">
                                                            Pan No.
                                                        </th>
                                                        <th align="center" style="width: 300px">
                                                            Joining Date
                                                        </th>
                                                        <th align="center" style="width: 300px">
                                                            Permanent Date
                                                        </th>
                                                        <th align="center" style="width: 300px">
                                                            Transfer Date
                                                        </th>
                                                        <th align="center" style="width: 300px">
                                                            Resignation Date
                                                        </th>
                                                        <th align="center" class="paddingLR" style="width: 170px">
                                                            E/D
                                                        </th>
                                                        <th align="center" class="paddingLR" style="width: 170px">
                                                            Y/L
                                                        </th>
                                                        <th align="center" class="paddingLR" style="width: 170px">
                                                            L/E
                                                        </th>
                                                        <th align="center" class="paddingLR" style="width: 170px">
                                                            Insurance
                                                        </th>
                                                        <th align="center" class="locked" style="width: 170px">
                                                            Act/DAct
                                                        </th>
                                                    </tr>
                                                    <tr id="trHeaderContol" runat="server" class="ClsGridHeader">
                                                        <th align="center" width="30px">
                                                            <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckAllUncheckAlls()" />
                                                        </th>
                                                        <th align="left" style="padding-left: 10px; width: 50%">
                                                        </th>
                                                        <th align="left">
                                                            <asp:DropDownList ID="cmbAllStaffGroups" runat="server" Width="140px" CssClass="MidCombo"
                                                                onchange="ChangeAllStaffGroups()">
                                                            </asp:DropDownList>
                                                        </th>
                                                        <th align="center">
                                                        </th>
                                                        <th align="center">
                                                        </th>
                                                        <th align="center">
                                                        </th>
                                                        <th align="center">
                                                        </th>
                                                        <th align="center" style="width: 300px">
                                                        </th>
                                                        <th align="center" style="width: 300px">
                                                        </th>
                                                        <th align="center" style="width: 300px">
                                                        </th>
                                                        <th align="center" style="width: 300px">
                                                        </th>
                                                        <th align="center" style="width: 300px">
                                                        </th>
                                                        <th align="center" style="width: 170px">
                                                        </th>
                                                        <th align="center" style="width: 170px">
                                                        </th>
                                                        <th align="center" style="width: 170px">
                                                        </th>
                                                        <th align="center" style="width: 170px">
                                                        </th>
                                                        <th align="center" style="width: 170px">
                                                        </th>
                                                    </tr>
                                                    <tr style="background-color: Black; height: 2px">
                                                        <td colspan="17">
                                                        </td>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                    <tr class="ClsBorderPager" id="trDataPager">
                                                        <td colspan="17">
                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwAssociation"
                                                                PageSize="20">
                                                                <Fields>
                                                                    <asp:TemplatePagerField>
                                                                        <PagerTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                        <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged" onchange="Page_BlockSubmit = false;">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td align="right" class="LblNormal">
                                                                                        <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </PagerTemplate>
                                                                    </asp:TemplatePagerField>
                                                                </Fields>
                                                            </asp:DataPager>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="trItem" runat="server" class="ClsGridRow">
                                                    <td align="center">
                                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                                    </td>
                                                    <td class="paddingL">                                                        
                                                        <asp:LinkButton ID="lblStaffName" runat="server" Width="180px" Text='<%#Eval("UserName") %>'></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbStaffGroups" Width="140px" runat="server" CssClass="MidCombo">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtEmployeeNo" CssClass="MidTxtBox" runat="server" Width="150px"
                                                            MaxLength="50" Text='<%#Eval("EmployeeNo") %>' Style="text-align: right" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtAccountNo" CssClass="MidTxtBox" Width="110px" runat="server"
                                                            MaxLength="15" Text='<%#Eval("AccountNo") %>' onblur="extractNumber(this,0,true);"
                                                            onkeyup="extractNumber(this,0,true);" onkeypress="return blockNonNumbers (this, event, false, true);"
                                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false" Style="text-align: right" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtProvidentFundNo" CssClass="MidTxtBox" Width="180px" runat="server"
                                                            MaxLength="30" Text='<%#Eval("ProvidentFundNo") %>' Style="text-align: right" />
                                                    </td>
                                                      <td align="center">
                                                        <asp:TextBox ID="txtUAN" CssClass="MidTxtBox" Width="100px" runat="server"
                                                            MaxLength="30" Text='<%#Eval("UAN") %>' Style="text-align: right" onblur="extractNumber(this,0,true);"
                                                            onkeyup="extractNumber(this,0,true);" onkeypress="return blockNonNumbers (this, event, false, true);"
                                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false"  />
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtPanNo" CssClass="MidTxtBox" runat="server" Width="110px" MaxLength="20"
                                                            onkeydown="return PreventSpecialChars(event);" Text='<%#Eval("PanNo") %>' Style="text-align: right" />
                                                    </td>
                                                    <td align="center" style="width: 130px">
                                                        <table style="width: 130px">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtDateOfJoining" runat="server" MaxLength="11" Text='<%#Eval("DateOfJoining","{0:dd-MMM-yyyy}") %>'
                                                                        CssClass="SmlTxtBox"></asp:TextBox>
                                                                    <rjs:PopCalendar ID="calDateOfJoining" runat="server" Control="txtDateOfJoining"
                                                                        ValidationGroup="Save" Format="dd MMM yyyy" ShowErrorMessage="false" ShowWeekend="True"
                                                                        InvalidDateMessage="Enter valid date." AutoPostBack="True" IncrementX="0" IncrementY="100"
                                                                        OnSelectionChanged="DateChanged" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="center" style="width: 130px" id="tdDateOfPermanent" runat="server">
                                                        <table style="width: 130px">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtDateOfPermanent" runat="server" MaxLength="11" Text='<%#Eval("DateOfPermanent","{0:dd-MMM-yyyy}") %>'
                                                                        CssClass="SmlTxtBox"></asp:TextBox>
                                                                    <rjs:PopCalendar ID="calDatePermanent" runat="server" Control="txtDateOfPermanent"
                                                                        ValidationGroup="Save" Format="dd MMM yyyy" ShowErrorMessage="false" AutoPostBack="True"
                                                                        ShowWeekend="True" InvalidDateMessage="" OnSelectionChanged="DateChanged" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="center" style="width: 130px" id="td1" runat="server">
                                                        <table style="width: 130px">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtTransferDate" runat="server" MaxLength="11" Text='<%#Eval("TransferDate","{0:dd-MMM-yyyy}") %>'
                                                                        CssClass="SmlTxtBox"></asp:TextBox>
                                                                    <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtTransferDate"
                                                                        ValidationGroup="Save" Format="dd MMM yyyy" AutoPostBack="True" ShowErrorMessage="false"
                                                                        ShowWeekend="True" InvalidDateMessage="" OnSelectionChanged="DateChanged" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="center" style="width: 130px">
                                                        <table style="width: 130px">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtDateOfResign" runat="server" MaxLength="11" Text='<%#Eval("DateOfResign","{0:dd-MMM-yyyy}") %>'
                                                                        CssClass="SmlTxtBox"></asp:TextBox>
                                                                    <rjs:PopCalendar ID="calDateOfResign" runat="server" Control="txtDateOfResign" ValidationGroup="Save"
                                                                        Format="dd MMM yyyy" ShowErrorMessage="false" AutoPostBack="True" ShowWeekend="True"
                                                                        InvalidDateMessage="" OnSelectionChanged="DateChanged" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="lnkBtnConfig" runat="server" Text="Configuration"></asp:LinkButton>
                                                        <asp:HiddenField runat="server" ID="hidDOJ" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="lnkBtnLeave" runat="server" Text="Edit"></asp:LinkButton>
                                                        <asp:HiddenField runat="server" ID="hidDOR" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="lnkBtnEnCash" runat="server" Text="Edit" Enabled="false"></asp:LinkButton>                                                                                                              
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="lnkBtnInsurance" runat="server" Text="Insurance"></asp:LinkButton>
                                                    </td>
                                                    <td align="center" style="width: 170px">
                                                        <asp:ImageButton ID="imgActiveDeactive" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif"
                                                            CommandName="LOCK" />
                                                        <asp:HiddenField ID="hidIsLockedUser" runat="server" Value='<%#Eval("Is_Locked") %>' />
                                                        <asp:HiddenField ID="hidAssociationId" runat="server" Value='<%#Eval("UsersStaffGroupsAssociationId") %>' />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="trAltItem" runat="server" class="ClsGridAltRow">
                                                    <td align="center">
                                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                                    </td>
                                                    <td class="paddingL">                                                        
                                                        <asp:LinkButton ID="lblStaffName" runat="server" Width="180px" Text='<%#Eval("UserName") %>'></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbStaffGroups" Width="140px" runat="server" CssClass="MidCombo">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtEmployeeNo" CssClass="MidTxtBox" Width="150px" runat="server"
                                                            MaxLength="50" Text='<%#Eval("EmployeeNo") %>' Style="text-align: right" />  
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtAccountNo" CssClass="MidTxtBox" Width="110px" runat="server"
                                                            MaxLength="15" Text='<%#Eval("AccountNo") %>' onblur="extractNumber(this,0,true);"
                                                            onkeyup="extractNumber(this,0,true);" onkeypress="return blockNonNumbers (this, event, false, true);"
                                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false" Style="text-align: right" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtProvidentFundNo" CssClass="MidTxtBox" Width="180px" runat="server"
                                                            MaxLength="30" Text='<%#Eval("ProvidentFundNo") %>' Style="text-align: right" />
                                                    </td>
                                                      <td align="center">
                                                        <asp:TextBox ID="txtUAN" CssClass="MidTxtBox" Width="100px" runat="server"
                                                            MaxLength="30" Text='<%#Eval("UAN") %>' Style="text-align:right" onblur="extractNumber(this,0,true);"
                                                            onkeyup="extractNumber(this,0,true);" onkeypress="return blockNonNumbers (this, event, false, true);"
                                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false"  />
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtPanNo" CssClass="MidTxtBox" runat="server" Width="110px" MaxLength="20"
                                                            onkeydown="return PreventSpecialChars(event);" Text='<%#Eval("PanNo") %>' Style="text-align: right" />
                                                    </td>
                                                    <td align="center" style="width: 130px">
                                                        <table style="width: 130px">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtDateOfJoining" runat="server" MaxLength="11" Text='<%#Eval("DateOfJoining","{0:dd-MMM-yyyy}") %>'
                                                                        CssClass="SmlTxtBox"></asp:TextBox>
                                                                    <rjs:PopCalendar ID="calDateOfJoining" runat="server" Control="txtDateOfJoining"
                                                                        ValidationGroup="Save" Format="dd MMM yyyy" ShowErrorMessage="false" ShowWeekend="True"
                                                                        InvalidDateMessage="" AutoPostBack="True" OnSelectionChanged="DateChanged" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="center" style="width: 130px" id="tdDateOfPermanent" runat="server">
                                                        <table style="width: 130px">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtDateOfPermanent" runat="server" MaxLength="11" Text='<%#Eval("DateOfPermanent","{0:dd-MMM-yyyy}") %>'
                                                                        CssClass="SmlTxtBox"></asp:TextBox>
                                                                    <rjs:PopCalendar ID="calDatePermanent" runat="server" Control="txtDateOfPermanent"
                                                                        ValidationGroup="Save" Format="dd MMM yyyy" AutoPostBack="True" ShowErrorMessage="false"
                                                                        ShowWeekend="True" InvalidDateMessage="" OnSelectionChanged="DateChanged" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="center" style="width: 130px" id="td1" runat="server">
                                                        <table style="width: 130px">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtTransferDate" runat="server" MaxLength="11" Text='<%#Eval("TransferDate","{0:dd-MMM-yyyy}") %>'
                                                                        CssClass="SmlTxtBox"></asp:TextBox>
                                                                    <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtTransferDate"
                                                                        ValidationGroup="Save" Format="dd MMM yyyy" AutoPostBack="True" ShowErrorMessage="false"
                                                                        ShowWeekend="True" InvalidDateMessage="" OnSelectionChanged="DateChanged" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="center" style="width: 130px">
                                                        <table style="width: 130px">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtDateOfResign" runat="server" MaxLength="11" Text='<%#Eval("DateOfResign","{0:dd-MMM-yyyy}") %>'
                                                                        CssClass="SmlTxtBox"></asp:TextBox>
                                                                    <rjs:PopCalendar ID="calDateOfResign" runat="server" Control="txtDateOfResign" ValidationGroup="Save"
                                                                        Format="dd MMM yyyy" ShowErrorMessage="false" ShowWeekend="True" InvalidDateMessage=""
                                                                        AutoPostBack="True" OnSelectionChanged="DateChanged" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="lnkBtnConfig" runat="server" Text="Configuration"></asp:LinkButton>
                                                    </td>
                                                    <td align="center" style="width: 170px">
                                                        <asp:LinkButton ID="lnkBtnLeave" runat="server" Text="Edit"></asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="lnkBtnEnCash" runat="server" Text="Edit" Enabled="false"></asp:LinkButton>                                                      
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="lnkBtnInsurance" runat="server" Text="Insurance"></asp:LinkButton>
                                                    </td>
                                                    <td align="center" style="width: 170px">
                                                        <asp:ImageButton ID="imgActiveDeactive" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif"
                                                            CommandName="LOCK" />
                                                        <asp:HiddenField ID="hidIsLockedUser" runat="server" Value='<%#Eval("Is_Locked") %>' />
                                                        <asp:HiddenField ID="hidAssociationId" runat="server" Value='<%#Eval("UsersStaffGroupsAssociationId") %>' />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:ListView>
                                        <table width="100%" id="tblNote" runat="server" visible="false">
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label2" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="Label3" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="If there exist attendance for a month for a staff member and user is trying to set joining date or resignation date in same month then attendance will be modified according to joining / resignation date."></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:CustomValidator ID="cstvalEmptyEmployeeNo" runat="server" ClientValidationFunction="DuplicateEmployeeNo"
                                            SetFocusOnError="True" Display="None" ErrorMessage="" ValidationGroup="Save"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr id="trNoRecordMsg" runat="server">
                                    <td style="height: 10px;" align="center">
                                        <asp:Label ID="lblNoRecordMsg" runat="server" CssClass="LblNoRecord" Font-Bold="True"
                                            Text="No Record Found." EnableViewState="False" Width="70%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div runat="server" id="divErr">
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trButtons" runat="server" align="center">
                                    <td align="center">
                                        <asp:Button ID="BtnSave" Text="Save" runat="server" CssClass="ClsBtn" BorderWidth="1px" disable-page="true"
                                            OnClick="BtnSave_Click" ValidationGroup="Save" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Back" CssClass="ClsBtn" BorderWidth="1px"
                                            CausesValidation="false" UseSubmitBehavior="false" />
                                        <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="ClsBtn" BorderWidth="1px"
                                            CausesValidation="false" UseSubmitBehavior="false" OnClick="btnExport_Click" />
                                        <asp:HiddenField ID="hidIsConfigured" runat="server" Value="N" />
                                        <asp:HiddenField ID="hidRowCnt" runat="server" />
                                        <asp:HiddenField ID="hidUserRoleId" runat="server" />
                                        <asp:HiddenField ID="hidPageNo" runat="server" Value="1" />
                                        <asp:HiddenField ID="hidUserStaffgroupsAssociationId" runat="server" />
                                        <asp:HiddenField ID="hidRowChangedId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidUserStaffGroupIdMap" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnExport" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        _clientSaveId = "<%=this.BtnSave.ClientID %>";
        _clientbtnCancelId = "<%=this.btnCancel.ClientID %>";
        _clientcmbUserRole = "<%=this.cmbUserRoles.ClientID %>";
        _clientlstvwAssociation = "<%=this.lstvwAssociation.ClientID %>";
        _ClientcstvalEmptyEmployeeNo = "<%=this.cstvalEmptyEmployeeNo.ClientID %>";
        _ClientvalSum = "<%=this.valSum.ClientID %>";
        _clientMessage = "ctl00_MainBody_trErrorMessage";
        _ClientlblErrorMessage = "<%=this.lblErrorMessage.ClientID %>";
        _ClientvlblMessage = "<%=this.lblMessage.ClientID %>";
        _clienthidRowCnt = "<%=this.hidRowCnt.ClientID %>"
        _clientcstDate = "<%=this.cstDate.ClientID %>"
        _clientcstPermanentDateValidations = "<%=this.cstPermanentDateValidations.ClientID %>"
        _clientcstPermanentDateValidations1 = "<%=this.cstPermanentDateValidations1.ClientID %>"
        _clientcstResignationDate = "<%=this.cstResignationDate.ClientID %>"
        _clienthidPageNo = "<%=this.hidPageNo.ClientID %>"
        _clienthidRowChangedId = "<%=this.hidRowChangedId.ClientID %>"
        _clienthidUserStaffgroupsAssociationId = "<%=this.hidUserStaffgroupsAssociationId.ClientID %>"         

        var consolidatedStaffGroupId = <%= this.CONSOLIDATED_STAFFGROUP_ID %>;
         var professionalStaffGroupId = <%= this.PROFESSIONAL_STAFFGROUP_ID %>;

        function IsTextChange(Id,showDash,obj) {           	    	
        	var Ids = document.getElementById(_clienthidUserStaffgroupsAssociationId).value;        	
				document.getElementById(_clienthidUserStaffgroupsAssociationId).value = Ids + "," + Id;
            if(showDash=="Y")
            {
              if (obj.value.trim() == "")
                   obj.value = "-";
            }
        }

		var Page_IsValid = true;
        function CheckSelectedUsers(objBtn) {
			Page_IsValid = true;
            var bResult = true;
            if (CheckSelection(_clientlstvwAssociation, '_ChkSelect')) {
                bResult = true;

                if (typeof (Page_ClientValidate) == 'function') {
                    bResult = Page_ClientValidate();
                }


                if (bResult) {
                    document.getElementById(_clientSaveId).disabled = true;
                    document.getElementById(_clientbtnCancelId).disabled = true;
                    __doPostBack(objBtn.name, '');
                }

            }
            else {
                $get(_ClientvalSum).style.display = "none";
                alert("At least one user should be selected.");
				Page_IsValid = false;
                bResult = false;
            }

            return bResult;
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndReqHandler);
        prm.add_beginRequest(beginRequestHandler)

        function EndReqHandler(sender, args) {
            if (document.getElementById(_clientSaveId) != null)
                document.getElementById(_clientSaveId).disabled = false;
            if (document.getElementById(_clientbtnCancelId) != null)
                document.getElementById(_clientbtnCancelId).disabled = false;
            if (document.getElementById(_clientcmbUserRole) != null)
                document.getElementById(_clientcmbUserRole).disabled = false;    
                           
              SetDateFields();
              AutoSearch();
        }
        function beginRequestHandler(sender, args) {
            if (document.getElementById(_clientSaveId) != null)
                document.getElementById(_clientSaveId).disabled = true;
            if (document.getElementById(_clientbtnCancelId) != null)
                document.getElementById(_clientbtnCancelId).disabled = true;
            if (document.getElementById(_clientcmbUserRole) != null)
                document.getElementById(_clientcmbUserRole).disabled = true;
        }
        SetDateFields();
        function SetDateFields()
        {        
            for(var iRowId = 0; iRowId < $get(_clienthidRowCnt).value; iRowId++)
            {
                if($get(_clientlstvwAssociation + "_ctrl" + iRowId + "_ChkSelect") != null)
                {
                    var value = $get(_clientlstvwAssociation + "_ctrl" + iRowId + "_ChkSelect").checked;
                    var val = !value;

                    var cmbStaffGroup = $get(_clientlstvwAssociation + "_ctrl" + iRowId + "_cmbStaffGroups");
                    if(cmbStaffGroup.value == 0)
                        val = true;

                    if (document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_imgActiveDeactive") != null)
                    document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_imgActiveDeactive").disabled = val; 

                    var isLocked = $get(_clientlstvwAssociation + "_ctrl" + iRowId + "_hidIsLockedUser").value;
                    if(isLocked == 'True')
                    {
                        val = true;
                        value = false;
                    }

                    document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_txtDateOfJoining").disabled = val;
                    document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_txtDateOfPermanent").disabled = val;
                    document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_txtTransferDate").disabled = val;
                    document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_txtDateOfResign").disabled = val;
                    document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_txtEmployeeNo").disabled = val;
                    document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_txtAccountNo").disabled = val;
                    document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_txtProvidentFundNo").disabled = val;
                     document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_txtUAN").disabled = val;
                    document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_txtPanNo").disabled = val;

                    if (document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_lnkBtnConfig") != null)
                        document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_lnkBtnConfig").disabled = val;
                    if (document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_lnkBtnLeave") != null)
                        document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_lnkBtnLeave").disabled = val;
                    if (document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_lnkBtnInsurance") != null)
                        document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_lnkBtnInsurance").disabled = val;
                        
                    document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_cmbStaffGroups").disabled = !value;
                }
            }
        }

        function EnableListViewControls() {
			for(var i = 0; i < $get(_clienthidRowCnt).value - 1; i++) {
				if($get(_clientlstvwAssociation + "_ctrl" + i + "_ChkSelect").checked) {
					var cmbStaffGroup = $get(_clientlstvwAssociation + "_ctrl" + i + "_cmbStaffGroups");
                    var txtsTextBox = $get(_clientlstvwAssociation + "_ctrl" + i + "_txtEmployeeNo");
                    var txtAccountNo = $get(_clientlstvwAssociation + "_ctrl" + i + "_txtAccountNo");
                    var txtPFNo = $get(_clientlstvwAssociation + "_ctrl" + i + "_txtProvidentFundNo");
                    var txtPFNo = $get(_clientlstvwAssociation + "_ctrl" + i + "_txtUAN");
                    var txtPanNo = $get(_clientlstvwAssociation + "_ctrl" + i + "_txtPanNo");
                    
                    cmbStaffGroup.disabled = false;
                    txtsTextBox.disabled = false;
                    txtAccountNo.disabled = false;
                    txtPFNo.disabled = false;
                    txtPanNo.disabled = false;
				}
			}
			return false;
        }


        function CheckAllUncheckAlls() {

            var checkAll = document.getElementById("ctl00_MainBody_lstvwAssociation_ChkSelectAll").checked;
            var chk
            var iRowCount = 0;
            if (iRowCount < 10)
                chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
            else
                chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")

            while (chk != null) {
                chk.checked = checkAll;
                VisibleControls(iRowCount);
                iRowCount = iRowCount + 1;
                if (iRowCount < 10)
                    chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
                else
                    chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
            }
        }

        function ChangeAllStaffGroups() {        
            var cmbAll = document.getElementById("ctl00_MainBody_lstvwAssociation_cmbAllStaffGroups");
            var chk
            var iRowCount = 0;
            if (iRowCount < 10)
                chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
            else
                chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")

            while (chk != null) {
                if (chk.checked) {
                    var cmbStaffGroup = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_cmbStaffGroups")
                    cmbStaffGroup.value = cmbAll.value;

                    VisibleControls(iRowCount);

                }
                iRowCount = iRowCount + 1;
                if (iRowCount < 10)
                    chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
                else
                    chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
            }
        }

        function DisableButtons(objBtn) {

            if (document.getElementById(_clientSaveId) != null)
                document.getElementById(_clientSaveId).disabled = true;
            document.getElementById(_clientbtnCancelId).disabled = true;
            __doPostBack(objBtn.name, '');
        }

        function EnableStaffGroupDropdownsBeforeSave() {
            var iRowCount = 0;
            var chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect");
            while (chk != null) {
                if (chk.checked == true) {
                    var cmbStaffGroup = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_cmbStaffGroups");
                    if (cmbStaffGroup != null)
                        cmbStaffGroup.disabled = false;
                }
                iRowCount++;
                chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect");
            }
        }

        function DuplicateEmployeeNo(aSrc, args) {  
            var chk
            var sDuplicate = false;
            var sEmpty = false;
            var iRowCount = 0;
            var iRowCountIn = 0;
            var sEmptyStaffGroups = "";
            var sEmptyEmployeeNumUser = "";
            var sEmptyAccountNo = "";
            var sEmptyPFNo = "";
            var sEmptyPanNo = "";
            var sPFNo = "";
            var sDuplicateNo = "";
            var sDuplicateAcountNo = "";
            var sDuplicatePFNo = "";
            var sDuplicatePFNo = "";
            
            if ($get(_ClientlblErrorMessage) != null)
                $get(_ClientlblErrorMessage).style.display = "none";
            if ($get(_ClientvlblMessage) != null)
                $get(_ClientvlblMessage).style.display = "none";
            
            var rowNumbers = document.getElementById(_clienthidUserStaffgroupsAssociationId).value;
            var changesRowNumbers = rowNumbers.split(',');

            chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                var sUserName = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_lblStaffName").innerHTML;
                if (chk.checked == true) {
                    var cmbStaffGroup = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_cmbStaffGroups")
                    var txtsTextBox = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_txtEmployeeNo")
                    var txtAccountNo = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_txtAccountNo")
                    var txtPFNo = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_txtProvidentFundNo")
                    var txtUAN = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_txtUAN")
                    var txtPanNo = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_txtPanNo")

                    var sUserName = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_lblStaffName").innerHTML;
                    var i_RowCount = iRowCount + 1

                    if (cmbStaffGroup.value == "0" || cmbStaffGroup.value.trim() == "") {
                        sEmptyStaffGroups = sEmptyStaffGroups + ", " + sUserName;
                        sEmpty = true;
                    }

                    if (Find(changesRowNumbers,iRowCount) && (txtsTextBox.value.trim() == "" || txtAccountNo.value.trim() == "" || txtPFNo.value.trim() == "" || txtUAN.value.trim() == "" || txtPanNo.value.trim() == "")) {
                       
                        if (txtsTextBox.value.trim() == ""){
                            sEmptyEmployeeNumUser = sEmptyEmployeeNumUser + ", " + sUserName
                             sEmpty = true;
                             }
                        if (txtAccountNo.value.trim() == ""){
                            sEmptyAccountNo = sEmptyAccountNo + ", " + sUserName
                             sEmpty = true;
                             }
                        if (cmbStaffGroup.value != professionalStaffGroupId && txtPFNo.value.trim() == ""){
                            sEmptyPFNo = sEmptyPFNo + ", " + sUserName
                             sEmpty = true;
                             }
                        if (cmbStaffGroup.value != professionalStaffGroupId && txtUAN.value.trim() == ""){
                            sEmptyUAN = sEmptyUAN + ", " + sUserName
                             sEmpty = true;
                             }
                        if (cmbStaffGroup.value != professionalStaffGroupId && txtPanNo.value.trim() == ""){
                            sEmptyPanNo = sEmptyPanNo + ", " + sUserName                       
                             sEmpty = true;
                             }
                    }
                }
                iRowCount++;
                chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
            }

            var sConcat = "";
            $get(_ClientcstvalEmptyEmployeeNo).errormessage = "";

            if (sEmpty == true && sEmptyStaffGroups != "") {
                sEmptyStaffGroups = sEmptyStaffGroups.substring(1);
                $get(_ClientcstvalEmptyEmployeeNo).errormessage = $get(_ClientcstvalEmptyEmployeeNo).errormessage + sConcat + "Staff group should be assigned to user(s) : " + sEmptyStaffGroups;
                sConcat = "<br />";
            }

            if (sEmpty == true && sEmptyEmployeeNumUser != "") {
                sEmptyEmployeeNumUser = sEmptyEmployeeNumUser.substring(1);
                $get(_ClientcstvalEmptyEmployeeNo).errormessage = $get(_ClientcstvalEmptyEmployeeNo).errormessage + sConcat + "Employee No. should not be blank for user(s) : " + sEmptyEmployeeNumUser;
                sConcat = "<br />";
            }

            if (sEmpty == true && sEmptyAccountNo != "") {
                sEmptyAccountNo = sEmptyAccountNo.substring(1);
                $get(_ClientcstvalEmptyEmployeeNo).errormessage = $get(_ClientcstvalEmptyEmployeeNo).errormessage + sConcat + "Account No. should not be blank for user(s) : " + sEmptyAccountNo;
                sConcat = "<br />";
            }

            if (sEmpty == true && sEmptyPFNo != "") {
                sEmptyPFNo = sEmptyPFNo.substring(1);
                    $get(_ClientcstvalEmptyEmployeeNo).errormessage = $get(_ClientcstvalEmptyEmployeeNo).errormessage + sConcat + "PF No. should not be blank for user(s) : " + sEmptyPFNo;
                    sConcat = "<br />";
                }

                    
            if (sEmpty == true && sEmptyPanNo != "") {
                sEmptyPanNo = sEmptyPanNo.substring(1);
                    $get(_ClientcstvalEmptyEmployeeNo).errormessage = $get(_ClientcstvalEmptyEmployeeNo).errormessage + sConcat + "Pan No. should not be blank for user(s) : " + sEmptyPanNo;
                    sConcat = "<br />";
                }
          
            if (sEmpty == true || sDuplicate == true) {
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }


        function VisibleControls(iRowId) {
            var IsLocked=document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_hidIsLockedUser");
            var IsUserLocked=IsLocked.value;
            var chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_ChkSelect")
            if (chk.checked == true) {
                var StaffGroup = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_cmbStaffGroups");
                if(StaffGroup.value == 0 )
                    DisableAll(true,iRowId);
                else
                    DisableAll(false,iRowId);
                
                if(document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_hidAssociationId").value == 0)
                   StaffGroup.disabled = false;
            }
            else {
                document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_cmbStaffGroups").disabled = true;
                document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_txtEmployeeNo").disabled = true;
                document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_txtAccountNo").disabled = true;
                document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_txtProvidentFundNo").disabled = true;
                document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_txtUAN").disabled = true;
                document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_txtPanNo").disabled = true;
                
                document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_txtDateOfJoining").disabled = true;
                document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_txtDateOfPermanent").disabled = true;
                document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_txtTransferDate").disabled = true;
                document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_txtDateOfResign").disabled = true;

                if (document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_lnkBtnConfig") != null)
                    document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_lnkBtnConfig").disabled = true;
                if (document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_lnkBtnLeave") != null)
                    document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_lnkBtnLeave").disabled = true;
                if (document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_lnkBtnInsurance") != null)
                    document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_lnkBtnInsurance").disabled = true;
                 if (document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_imgActiveDeactive") != null)
                    document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_imgActiveDeactive").disabled = true; 
               }
               var Ids = document.getElementById(_clienthidUserStaffgroupsAssociationId).value;
               document.getElementById(_clienthidUserStaffgroupsAssociationId).value = Ids + "," + iRowId;
        }

        function OpenPopup(sQueryString, iRowId) {
        
            if (document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_lnkBtnConfig").disabled != true) {

                window.open('UsersEarningsAndDeductions.aspx?' +
                    sQueryString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=500');
            }

            return false;
        }
        function OpenInsurancePopup(sQueryString, iRowId) {

            if (document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_lnkBtnInsurance").disabled != true) {

                window.open('InsuranceDetailsPopup.aspx?' +
                    sQueryString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=760,height=500');
            }

            return false;
        }
        function OpenAllowedLeavesPopup(sQueryString, iRowId) {


            if (document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_lnkBtnLeave").disabled != true) {

                window.open('AllowedYearwiseLeavesPopup.aspx?' +
                    sQueryString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=620,height=530');
            }

            return false;
        }

        function OpenEnCashLeavePopup(sQueryString, iRowId)
        {        
            if (document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowId + "_lnkBtnLeave").disabled != true) {
                window.open('LeaveEnCashmentPopup.aspx?' +
                    sQueryString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=870,height=730');
            }
            return false;
        }


        function ConfirmationMsg() {
			Page_IsValid = true;
            var chk;
            var iRowCount = 0;
            var sMessage = true;
            var bResult = true;

            EnableStaffGroupDropdownsBeforeSave();

            if (typeof (Page_ClientValidate) == 'function') {
                bResult = Page_ClientValidate('Save');
            }
            if (!bResult) {
                Page_IsValid = false;
                return false;
            }

            chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")

            while (chk != null) {
                if (document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_lnkBtnConfig") != null) {
                    if (chk.checked == false) {
                        sMessage = false;
                        break;
                    }
                }
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")

            }

            if (sMessage == false) {
                {
                    if (!window.confirm("This action will delete unchecked association and respective Earning-Deduction configuration, Yearwise Leaves configuration if exists. Are you sure, you want to continue?")) {
                        bResult = false;
						Page_IsValid =false;
                        SelectCheckBoxes();
                    }
                }
            }
            return bResult;
        }

        function SelectCheckBoxes() {
            var chk;
            var iRowCount = 0;
            chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")

            while (chk != null) {
                if (document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_lnkBtnConfig") != null) {
                    {
                        chk.checked = true;
                        VisibleControls(iRowCount);
                    }
                }
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
            }
        }
        function DateValidations(oSrc, args) {
            var lblUserName = "";
            var UserName = "";
            var dtJoiningDate = "";
            var dtResignDate = "";
            var TodayDate = new Date().format("dd-MMM-yyyy")
            var iRowCount = 0;
            var iRowNo = ""
            oSrc.errormessage = "";
            var chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")

            if (chk.checked == true) {
                var iRowCnt = (document.getElementById(_clienthidRowCnt).value) - 1
                while ((iRowCnt) >= 0) {
                    chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_ChkSelect")
                    if (chk.checked == true) {
                        var dtDOJ = "";
                        var dtDOR = "";
                        dtDOJ = _clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfJoining"
                        dtDOR = _clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfResign"
                        
                        dtJoiningDate = document.getElementById(dtDOJ).value;
                        dtResignDate = document.getElementById(dtDOR).value;
                        if (dtJoiningDate != "" && dtResignDate != "") {
                            var dtJD
                            var dtRD
                            lblUserName = document.getElementById(_clientlstvwAssociation + "_ctrl" + (iRowCnt) + "_lblStaffName").innerHTML
                            if (document.all) {
                                dtJD = new Date(dtJoiningDate.replace('-', ' '));
                                dtRD = new Date(dtResignDate.replace('-', ' '));

                            }
                            else {
                                dtJD = new Date(convertdate(document.getElementById(dtDOJ).value));
                                dtRD = new Date(convertdate(document.getElementById(dtDOR).value));
                            }
                            if (dtJD > dtRD) {
                                if (UserName == "")
                                    UserName = lblUserName;
                                else
                                    UserName = lblUserName + ", " + UserName;
                            }
                        }
                    }
                    iRowCnt = iRowCnt - 1
                }
            }
            if (UserName != "") {
                oSrc.errormessage = "Resignation date should be greater than or equal to Joining date for the user(s): " + UserName + ".";
                document.getElementById(_clientcstDate).innerHTML = "Resignation date should be greater than Joining date for the user(s): " + UserName + "." ;
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }

        function PermanentDateValidations(oSrc, args) {
            var lblUserName = "";
            var UserName = "";
            var dtJoiningDate = "";
            var dtPermanentDate = "";
            var TodayDate = new Date().format("dd-MMM-yyyy")
            var iRowNo = ""
            oSrc.errormessage = "";
                var iRowCnt = (document.getElementById(_clienthidRowCnt).value) - 1
                while ((iRowCnt) >= 0) {
                    chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_ChkSelect")
                    if (chk.checked == true) {
                        var dtDOJ = _clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfJoining"
                        var dtDOP = _clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfPermanent"

                        dtJoiningDate = document.getElementById(dtDOJ).value;
                        dtPermanentDate = document.getElementById(dtDOP).value;
                        if (dtJoiningDate != "" && dtPermanentDate != "") {
                            var dtOfJD
                            var dtOfPD
                            lblUserName = document.getElementById(_clientlstvwAssociation + "_ctrl" + (iRowCnt) + "_lblStaffName").innerHTML
                            if (document.all) {
                                dtOfJD = new Date(dtJoiningDate.replace('-', ' '));
                                dtOfPD = new Date(dtPermanentDate.replace('-', ' '));

                            }
                            else {
                                dtOfJD = new Date(convertdate(document.getElementById(dtDOJ).value));
                                dtOfPD = new Date(convertdate(document.getElementById(dtDOP).value));

                            }
                            if (dtOfJD > dtOfPD) {
                                if (UserName == "")
                                    UserName = lblUserName;
                                else
                                    UserName = lblUserName + ", " + UserName;
                            }
                        }
                    }
                    iRowCnt = iRowCnt - 1
            }
                if (UserName != "") {
                    oSrc.errormessage = "Permanent date should be greater than or equal to Joining date for the user(s): " + UserName + ".";
                    document.getElementById(_clientcstPermanentDateValidations).innerHTML = "Permanent date should be greater than Joining date for the user(s): " + UserName + ".";
                
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }


        function TransferDateValidations(oSrc, args) {
            var lblUserName = "";
            var UserName = "";
            var dtJoiningDate = "";
            var dtPermanentDate = "";
            var TodayDate = new Date().format("dd-MMM-yyyy")
            var iRowNo = ""
            oSrc.errormessage = "";
                var iRowCnt = (document.getElementById(_clienthidRowCnt).value) - 1
                while ((iRowCnt) >= 0) {
                    chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_ChkSelect")
                    if (chk.checked == true) {
                        var dtDOJ = _clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfJoining"
                        var dtDOP = _clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtTransferDate"

                        dtJoiningDate = document.getElementById(dtDOJ).value;
                        dtPermanentDate = document.getElementById(dtDOP).value;
                        if (dtJoiningDate != "" && dtPermanentDate != "") {
                            var dtOfJD
                            var dtOfPD
                            lblUserName = document.getElementById(_clientlstvwAssociation + "_ctrl" + (iRowCnt) + "_lblStaffName").innerHTML
                            if (document.all) {
                                dtOfJD = new Date(dtJoiningDate.replace('-', ' '));
                                dtOfPD = new Date(dtPermanentDate.replace('-', ' '));

                            }
                            else {
                                dtOfJD = new Date(convertdate(document.getElementById(dtDOJ).value));
                                dtOfPD = new Date(convertdate(document.getElementById(dtDOP).value));

                            }
                            if (dtOfJD >= dtOfPD) {
                                if (UserName == "")
                                    UserName = lblUserName;
                                else
                                    UserName = lblUserName + ", " + UserName;
                            }
                        }
                    }
                    iRowCnt = iRowCnt - 1
            }
                if (UserName != "") {
                    oSrc.errormessage = "Transfer Date should be greater than Joining Date for the user(s): " + UserName + ".";
                    args.IsValid = false
                    return true
            }
            else {
                args.IsValid = true
                return false
            }
        }

        function TransferDateValidations1(oSrc, args) {
            var lblUserName = "";
            var UserName = "";
            var dtPermanentDate = "";
            var dtTransferDate = "";
            var TodayDate = new Date().format("dd-MMM-yyyy")
            var iRowNo = ""
            oSrc.errormessage = "";
                var iRowCnt = (document.getElementById(_clienthidRowCnt).value) - 1
                while ((iRowCnt) >= 0) {
                    chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_ChkSelect")
                    if (chk.checked == true) {
                        var dtDOP = _clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfPermanent"
                        var dtDOT = _clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtTransferDate"

                        dtPermanentDate = document.getElementById(dtDOP).value;
                        dtTransferDate = document.getElementById(dtDOT).value;
                        if (dtPermanentDate != "" && dtTransferDate != "") {
                            var dtOfJP
                            var dtOfPT
                            lblUserName = document.getElementById(_clientlstvwAssociation + "_ctrl" + (iRowCnt) + "_lblStaffName").innerHTML
                            if (document.all) {
                                dtOfJP = new Date(dtPermanentDate.replace('-', ' '));
                                dtOfPT = new Date(dtTransferDate.replace('-', ' '));

                            }
                            else {
                                dtOfJP = new Date(convertdate(document.getElementById(dtDOP).value));
                                dtOfPT = new Date(convertdate(document.getElementById(dtDOT).value));

                            }
                            if (dtOfJP >= dtOfPT) {
                                if (UserName == "")
                                    UserName = lblUserName;
                                else
                                    UserName = lblUserName + ", " + UserName;
                            }
                        }
                    }
                    iRowCnt = iRowCnt - 1
            }
            
            if (UserName != "") {
                    oSrc.errormessage = "Transfer Date should be greater than Permanent Date for the user(s): " + UserName + ".";
                    args.IsValid = false
                    return true
            }
            else {
                args.IsValid = true
                return false
            }
        }

        function ValidateTransferDate(oSrc, args) {
            var lblUserName = "";
            var UserName = "";
            var dtJoiningDate = "";
            var dtTransferDate = "";
            var TodayDate = new Date().format("dd-MMM-yyyy")
            var iRowNo = ""
            oSrc.errormessage = "";

                var iRowCnt = (document.getElementById(_clienthidRowCnt).value) - 1
                while ((iRowCnt) >= 0)
                {
                    chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_ChkSelect")
                    if (chk.checked == true) 
                    {
                            var dtDOJ = _clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfJoining"
                            var dtDOT = _clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtTransferDate"

                            dtJoiningDate = document.getElementById(dtDOJ).value;
                            dtTransferDate = document.getElementById(dtDOT).value;

                            if (dtJoiningDate == "" && dtTransferDate != "")
                            {   
                                lblUserName = document.getElementById(_clientlstvwAssociation + "_ctrl" + (iRowCnt) + "_lblStaffName").innerHTML

                                if (UserName == "")
                                    UserName = lblUserName;
                                else
                                    UserName = lblUserName + ", " + UserName;
                            }
                    }
                    iRowCnt = iRowCnt - 1
                }

                if (UserName != "")
                {
                    oSrc.errormessage = "Joining date should not be blank if need to set Transfer Date for the user(s): " + UserName + ".";                
                    args.IsValid = false
                    return true
                }
                else
                {
                    args.IsValid = true
                    return false
                }
        }

        function PermanentDateValidations1(oSrc, args) {

            var lblUserName = "";
            var UserName = "";
            var dtJoiningDate = "";
            var dtPermanentDate = "";
            var TodayDate = new Date().format("dd-MMM-yyyy")
            var iRowNo = ""
            oSrc.errormessage = "";
                var iRowCnt = (document.getElementById(_clienthidRowCnt).value) - 1
                while ((iRowCnt) >= 0) {
                    chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_ChkSelect")
                    if (chk.checked == true) {
                        var dtDOJ
                        var dtDOP
                        dtDOJ = _clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfJoining"
                        dtDOP = _clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfPermanent"

                        dtJoiningDate = document.getElementById(dtDOJ).value;
                        dtPermanentDate = document.getElementById(dtDOP).value;
                        lblUserName = document.getElementById(_clientlstvwAssociation + "_ctrl" + (iRowCnt) + "_lblStaffName").innerHTML
                        if (dtJoiningDate == "" && dtPermanentDate != "") {
                            if (UserName == "")
                                UserName = lblUserName;
                            else
                                UserName = lblUserName + ", " + UserName;
                        }
                    }
                    iRowCnt = iRowCnt - 1
            }
                if (UserName != "") {
                    oSrc.errormessage = "Joining date should not be blank for the user(s): " + UserName + ".";
                    document.getElementById(_clientcstPermanentDateValidations1).innerHTML = "Joining date should not be blank for the user(s): " + UserName + ".";

                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }

        function ResignationDateValidations(oSrc, args) {            
            var lblUserName = "";
            var UserNm = "";
            var dtJoiningDate = "";
            var dtPermanentDate = "";
            var dtResignDate = "";
            var TodayDate = new Date().format("dd-MMM-yyyy")
            var iRowNo = ""
           oSrc.errormessage = "";
                var iRowCnt = (document.getElementById(_clienthidRowCnt).value) - 1
                while ((iRowCnt) >= 0) {
                    chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_ChkSelect")
                    if (chk.checked == true) {
                        var dtDOJ
                        var dtDOR
                        var dtDOP
                        dtDOJ = _clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfJoining"
                        dtDOR = _clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfResign"
                        dtDOP = _clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfPermanent"

                        dtJoiningDate = document.getElementById(dtDOJ).value
                        dtResignDate = document.getElementById(dtDOR).value;
                        dtPermanentDate = document.getElementById(dtDOP).value;
                        lblUserName = document.getElementById(_clientlstvwAssociation + "_ctrl" + (iRowCnt) + "_lblStaffName").innerHTML
                        if (dtJoiningDate != "" && dtPermanentDate != "" && dtResignDate != "") {
                            var dtOfRD
                            var dtOfPD
                            lblUserName = document.getElementById(_clientlstvwAssociation + "_ctrl" + (iRowCnt) + "_lblStaffName").innerHTML
                            if (document.all) {
                                dtOfRD = new Date(dtResignDate.replace('-', ' '));
                                dtOfPD = new Date(dtPermanentDate.replace('-', ' '));

                            }
                            else {
                                dtOfRD = new Date(convertdate(document.getElementById(dtDOR).value));
                                dtOfPD = new Date(convertdate(document.getElementById(dtDOP).value));

                            }
                            if (dtOfRD < dtOfPD) 
                                if (UserNm == "")
                                    UserNm = lblUserName;
                                else
                                    UserNm = lblUserName + ", " + UserNm;
                        }
                    }
                    iRowCnt = iRowCnt - 1
            }
            if (UserNm != "") {
                oSrc.errormessage = "Resignation date should be greater than Permanent date for the user(s): " + UserNm + ".";
                document.getElementById(_clientcstResignationDate).innerHTML = "Resignation date should be greater than Permanent date for the user(s): " + UserNm + ".";

                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }

        function ResignDateValidation(oSrc, args) {       
            var lblUserName = "";
            var UserName = "";
            var dtResignationDate = "";
            var dtJoiningDate = "";
            var TodayDate = new Date().format("dd-MMM-yyyy")
            var iRowNo = ""
            oSrc.errormessage = "";
            var iRowCnt = (document.getElementById(_clienthidRowCnt).value) - 1
            while ((iRowCnt) >= 0) {
                chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_ChkSelect")
                 var cmbStaffGroup = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_cmbStaffGroups")
                if (chk.checked == true) {
                    dtResignationDate = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfResign").value;
                    dtJoiningDate = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfJoining").value;
                    lblUserName = document.getElementById(_clientlstvwAssociation + "_ctrl" + (iRowCnt) + "_lblStaffName").innerHTML
                    if (dtResignationDate != "" && dtJoiningDate == "" && !(cmbStaffGroup.value == professionalStaffGroupId || cmbStaffGroup.value == consolidatedStaffGroupId) ) {
                        if (UserName == "")
                            UserName = lblUserName;
                        else
                            UserName = lblUserName + ", " + UserName;
                    }
                }
                iRowCnt = iRowCnt - 1
            }
            if (UserName != "") {
                oSrc.errormessage = "Joining date should not be blank for the user(s): " + UserName + ".";
                document.getElementById("<%=this.cstResigDate.ClientID %>").innerHTML = "Joining date should not be blank for the user(s): " + UserName + ".";

                args.IsValid = false
                return true
            }            
             
            args.IsValid = true
            return false            
        }

        
        function DisableDates(iRowCnt,iStaffGroupId)
        {
             var chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_ChkSelect")
              
             if (chk.checked == true ) {                                      
                     var cmbStaffGroup = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_cmbStaffGroups")                
                    if (cmbStaffGroup.value == professionalStaffGroupId || cmbStaffGroup.value == consolidatedStaffGroupId)  {
                    
                        document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfResign").disabled = true;
                        document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfJoining").disabled = true;
                        document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfPermanent").disabled = true;
                        document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtTransferDate").disabled = true;
                    }
                    else
                    {
                        document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfResign").disabled = false;
                        document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfJoining").disabled = false;
                        document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfPermanent").disabled = false;
                        document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtTransferDate").disabled = false;
                    }
                    DisableAll(cmbStaffGroup.value == 0,iRowCnt);                   
               }
        }

        function DisableAll(val,iRowCnt)
        {
            var cmbStaffGroup = $get(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_cmbStaffGroups");
            if(cmbStaffGroup.value == 0)
                val = true;

            if (document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_imgActiveDeactive") != null)
            document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_imgActiveDeactive").disabled = val; 

            var isLocked = $get(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_hidIsLockedUser").value;
            if(isLocked == 'True')
                val = true;                

            document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfResign").disabled = val;
            document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfJoining").disabled = val;
            document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtDateOfPermanent").disabled = val;
            document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtTransferDate").disabled = val;
            document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtEmployeeNo").disabled = val;
            document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtAccountNo").disabled = val;
            document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtProvidentFundNo").disabled = val;
             document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtUAN").disabled = val;
            document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_txtPanNo").disabled = val;

            if (document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_lnkBtnConfig") != null)
                document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_lnkBtnConfig").disabled = val;
            if (document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_lnkBtnLeave") != null)
                document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_lnkBtnLeave").disabled = val;
            if (document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_lnkBtnInsurance") != null)
                document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCnt + "_lnkBtnInsurance").disabled = val;
        }


        function ConfirmUpdate(iStaffGroupId, iRowCount) {
			Page_IsValid = true;
            DisableDates(iRowCount,iStaffGroupId);
            var bResult = true            
            chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")

            document.getElementById(_clienthidRowChangedId).value = iRowCount;
            if (chk != null) {
                if (document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_lnkBtnConfig") != null) {
                    {
                        cmbStaffGroup = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_cmbStaffGroups")
                        if (cmbStaffGroup.value != iStaffGroupId) {
                            if (cmbStaffGroup.value != "0") {
                                if (!window.confirm("If the staff group is changed, the salary configuration and leaves of the user will get overwritten according to the new staff group. Are you sure you want to continue?")) {
                                    cmbStaffGroup.value = iStaffGroupId;
                                    bResult = false;
								    Page_IsValid = false;                                
                                }
                            }

                            if (bResult) {
                                var Ids = document.getElementById(_clienthidUserStaffgroupsAssociationId).value;
                                document.getElementById(_clienthidUserStaffgroupsAssociationId).value = Ids + "," + iRowCount;
                            }
                        }                       
                    }
                }

            }
            return bResult;
        }

        function Find(arr, obj) {
            for (var i = 0; i < arr.length; i++) {
                if (parseInt(arr[i]) == parseInt( obj)) return true;
            }
            return false;
        }

        //This function is used to display message when page index will be changed.
        function MessageAboutDate(oCmb) {
            var bIsValid
            if (window.confirm("If you change the page then entered details on current page will get lost. Do you want to continue?"))
                bIsValid = true
            else {
                document.getElementById(oCmb).value = document.getElementById(_clienthidPageNo).value
                bIsValid = false
            }
            return bIsValid
           }


           function PreventSpecialChars(e) {
               var k;
               document.all ? k = e.keyCode : k = e.which;
               return ((k > 64 && k < 91) || (k >= 96 && k < 123) || k == 8 || k==39 || k==37 || (k >= 48 && k <= 57) || k == 0 || k == 9 || k==46);
           }

           function OpenSalaryStructurePopup(sQueryString, k)
           {                    
                if(k == 1)
                {
                         window.open('SalaryStructurePopup.aspx?' +sQueryString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=650');
                         return false;
                }
                else
                {
                         window.alert("Please configure Earning and Deduction");
                         return false;
                }
           }

    </script>

    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            AutoSearch();
        });

        function AutoSearch() {
            _slienttxtUserName = '#<%=txtUserName.ClientID%>';
            var SchoolId = "<%=miSchoolId %>";
            var AcademicYearId = "<%=miAcademicYearId %>"
            _clientddlUserType = "<%=this.ddlUserType.ClientID %>"

            BindAutoCompleteEventForStaffWithStatus(SchoolId, AcademicYearId, _slienttxtUserName, _clientcmbUserRole, 1, _clientddlUserType);
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtUserName.ClientID %>");
            bt = document.getElementById("<%=this.btnSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }

        function OpenCarryForwordUserPopup() {
            window.open('UserRejoiningDetailsPopup.aspx', '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=650')
            return false;
        }

	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
