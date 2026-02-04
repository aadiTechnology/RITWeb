<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    AutoEventWireup="true" CodeFile="FeedbackDetailsUI.aspx.cs" Inherits="FeedbackListUI" %>
    
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        &nbsp;<table width="97%">
            <tr runat="server" id="trCombo">
                <td align="left">
                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td style="width: 30%">
                            <asp:UpdatePanel ID="UPanelInput" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                            <ContentTemplate>
                                <table width="100%">
                                 <tr>
                                     <td align="right" colspan="6">
                                       <span class="ClsMdtStar">* Mandatory Fields</span>                                       
                                     </td>
                                   </tr>
                                <tr>
                                     <td align="left" colspan="6">
                                         <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="lblNormal"  HeaderText="Please fix following error(s):"
                                              ShowMessageBox="False" ShowSummary="True" />
                                         <asp:CustomValidator ID="cstForm" Display="None" runat="server" CssClass="ClsMdtStar"
                                            Visible="true" ErrorMessage="" ClientValidationFunction="ValidateControls"></asp:CustomValidator>                                           
                                     </td>
                                   </tr>
                                   <tr>
                                        <td colspan="6" align="center" >
                                             <asp:Label ID="lblDelete"  runat="server" EnableViewState="false" 
                                                CssClass="ClsLabelUpdate" style="color:Blue;"> </asp:Label>
                                        </td>
                                   </tr>
                                    <tr>
                                        <td class="ClsBorderlight" colspan="1" style="width: 15%;">
                                                <span class="ClsLabel" id="lblUserRole">User Role : </span>
                                        </td>
                                        <td colspan="1" style="width: 20%;">
                                            <asp:DropDownList ID="ddlUserRole" runat="server" AutoPostBack="true" Width="132px" TabIndex="1">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="ClsBorderlight" colspan="1" style="width: 15%;">
                                                <span class="ClsLabel" id="lblFeedbackType">Feedback Type : </span>
                                        </td>
                                        <td colspan="1" style="width: 20%;">
                                            <asp:DropDownList ID="ddlFeedbackType" runat="server" AutoPostBack="true" Width="132px" TabIndex="2">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="ClsBorderlight" colspan="1" style="width: 15%;">
                                                 <span class="ClsLabel" id="Span1">Feedback For : </span>
                                        </td>
                                        <td colspan="1" style="width: 20%;">
                                            <asp:DropDownList ID="ddlFeedbackFor" runat="server" AutoPostBack="true" Width="132px" TabIndex="2">
                                                <asp:ListItem Text="--All--" Value="0" Selected ="True"></asp:ListItem>
                                                <asp:ListItem Text="School" Value="School" ></asp:ListItem>
                                                <asp:ListItem Text="Software" Value="Software"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" align="center">
                                                <table width="100%">
                                                  <tr>
                                                  <td width="100px">
                                                  </td>
                                                    <td class="ClsBorderlight" align="center">
                                                            <span class="ClsLabel" id="Span2">Start Date : </span></td>
                                                    <td align="left" valign="top">
                                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                            TabIndex="3"></asp:TextBox>
                                                        <rjs:PopCalendar ID="cFromDate" runat="server" Control="txtFromDate" Format="dd MMM yyyy"
                                                            ShowWeekend="True" Enabled="true" ShowErrorMessage="false" 
                                                            InvalidDateMessage="Please select valid from date." 
                                                            ControlFocusOnError="True" />
                                                            <asp:Label ID="lblFromDateMandMark" runat="server" CssClass="ClsMdtStar" 
                                                        Height="14px" Text="*" Width="14px"></asp:Label>
                                                    </td>
                                                    <td class="ClsBorderlight" align="center">
                                                            <span class="ClsLabel" id="Span3">End Date : </span></td>
                                                    <td align="left" valign="top">
                                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="SmlTxtBox" MaxLength="11" TabIndex="3"></asp:TextBox>
                                                        <rjs:PopCalendar ID="cToDate" runat="server" Control="txtToDate" Format="dd MMM yyyy"
                                                            ShowWeekend="True" Enabled="true" ShowErrorMessage="false" 
                                                            InvalidDateMessage="Please select valid to date."/>
                                                        <asp:Label ID="lblToDateMandMark" runat="server" CssClass="ClsMdtStar" 
                                                            Height="14px" Text="*" Width="14px" EnableViewState="false"></asp:Label></td>
                                                     <td width="100px">
                                                     </td>      
                                                      </tr>
                                                 </table>
                                            </td>
                                        </tr>
                                        <tr>
                                        <td colspan="6" style="width: 15%;" align="center">
                                            <asp:Button ID="btnShow" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                CssClass="ClsBtnSml" Text="Show" Visible="True" OnClick="btnShow_Click" TabIndex="3" />
                                        </td>
                                    </tr>
                                </table>
                                </ContentTemplate>
                                <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click"/>
                                
                                    <asp:AsyncPostBackTrigger ControlID="grdUsersFeedback" EventName="RowCommand" />
                                
                                </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                             <asp:UpdatePanel ID="UpanelGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <asp:Panel ID="pnlUserFeedbackGrid" runat="server">
                                    <table id="Table1" runat="server" width="100%">
                                        <tr runat="server" id="trTotalRec" align="center" visible="false">
                                            <td>
                                                <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                                <span class="LblNormal">To</span>
                                                <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                 <span class="LblNormal">Out Of</span>
                                                <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                                <span class="LblNormal">Records</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" valign="top">
                                                <asp:GridView CssClass="GridBorder" ID="grdUsersFeedback" runat="server" AllowPaging="True" 
                                                    AutoGenerateColumns="False" OnRowCommand="grdUsersFeedback_RowCommand" AllowSorting="True"
                                                    OnRowDataBound="grdUsersFeedback_RowDatabound" EmptyDataText="No record found."
                                                    Width="100%" PageSize="20" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                                                    GridLines="None" DataKeyNames="Feedback_Id" OnSorting="grdUsersFeedback_Sorting" OnRowCreated="grdUsersFeedback_RowCreated"
                                                    OnPageIndexChanging="grdUsersFeedback_PageIndexChanging" TabIndex="4" >
                                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                    </PagerStyle>
                                                    <Columns>
                                                        <asp:BoundField DataField="Feedback_Date" HeaderText="Date" SortExpression="Feedback_Date"
                                                            DataFormatString="{0:dd MMM yyyy}">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"/>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"
                                                                Wrap="False" Width="12%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblUser" runat="server" Text="UserName(Email)"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("User_Name") %>'></asp:Label><br />
                                                                <asp:Label ID="lblEmail" runat="server" Text='<%# "(" + Eval("Email_Address") + ")" %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="23%" HorizontalAlign="Left" CssClass="paddingLSML" VerticalAlign="Middle" />
                                                            <HeaderStyle Width="23%" HorizontalAlign="Left" CssClass="paddingLSML" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Feedback" HeaderText="Feedback">
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Width="51%" Wrap="true" />
                                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML"
                                                                Wrap="True" Width="60%" />
                                                        </asp:BoundField>
                                                        <asp:ButtonField ButtonType="Image" CommandName="Delete_FeedbackDetails" HeaderText="Delete"
                                                            Text="Delete" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"/>
                                                        </asp:ButtonField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <RowStyle CssClass="ClsGridRow" />
                                                    <HeaderStyle CssClass="ClsGridHeader" />
                                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                    <PagerTemplate>
                                                        <table width="100%" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td width="70%" align="left" class="ClsBorderPager" valign="middle">
                                                                    <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                    <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" CssClass="LblNormal"
                                                                        OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td width="30%" align="right" class="ClsBorderPager" valign="middle">
                                                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </PagerTemplate>
                                                </asp:GridView>
                                                <asp:ObjectDataSource TypeName="BusinessLogic.FeedbackDetailsBL" EnablePaging="true"
                                                    ID="GrdDSobj" runat="server" SelectMethod="GetUserFeedbackDetails" SortParameterName="sortExpression"
                                                    SelectCountMethod="GettFeedbackCount" EnableCaching="false" OnSelected="GrdDSobj_Selected">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                        <asp:ControlParameter Name="aiUserRoleId" Type="int32" ControlID="ddlUserRole" PropertyName="SelectedValue" />
                                                        <asp:ControlParameter Name="aiFeedbackTypeId" Type="int32" ControlID="ddlFeedbackType"
                                                            PropertyName="SelectedValue" />
                                                        <asp:ControlParameter Name="asFeedBackFor" Type="String" ControlID="ddlFeedbackFor"
                                                            PropertyName="SelectedValue" />
                                                        <asp:ControlParameter Name="asStartDate" Type="String" ControlID = "txtFromDate" PropertyName="Text" />
                                                        <asp:ControlParameter Name="asEndDate" Type="String" ControlID="txtToDate" PropertyName="Text" />
                                                        <asp:ControlParameter Name="sortDirection" Type="String" ControlID="hidSortDirection"
                                                            PropertyName="Value" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:HiddenField ID="hidSortDirection" runat="server" />
                                <asp:HiddenField ID="hidSortExpression" runat="server" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="grdUsersFeedback" EventName="RowCommand" />
                                    <asp:AsyncPostBackTrigger ControlID="grdUsersFeedback" EventName="SelectedIndexChanged" />
                                </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">                            
                                <table border="0" cellpadding="0" align="center">
                                    <tbody>
                                        <tr>
                                            <td style="height: 5px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="height: 20px">
                                                <asp:Button ID="btnBack" runat="server" BorderStyle="Solid" BorderWidth="1px" CausesValidation="false"
                                                    CssClass="ClsBtnSml" Text="Back" Visible="True" TabIndex="5" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">

        _clienttxtFromDateId = "<%=this.txtFromDate.ClientID %>"
        _clientcFromDateId = "<%=this.cFromDate.ClientID %>"
        _clienttxtToDateId = "<%=this.txtToDate.ClientID %>"
        _clienttcToDateId = "<%=this.cToDate.ClientID %>"
        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this Feedback details?')) {
                bResult = false
            }
            return bResult
        }
        function ValidateControls(oSrc, args) {
            if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtFromDateId).value) == "") {
                oSrc.errormessage = "Start date should not be blank."
                args.IsValid = false
                return true
            }
            else {
                if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtToDateId).value) == "") {
                    oSrc.errormessage = "End date should not be blank."
                    args.IsValid = false
                    return true
                }
                else {
                    var fromDate
                    var toDate
                    if (document.all) {
                        fromDate = new Date((document.getElementById(_clienttxtFromDateId).value).replace('-', ' '))
                        toDate = new Date((document.getElementById(_clienttxtToDateId).value).replace('-', ' '))
                    }
                    else {
                        fromDate = new Date(convertdate(document.getElementById(_clienttxtFromDateId).value))
                        toDate = new Date(convertdate(document.getElementById(_clienttxtToDateId).value))
                    }
                    if (fromDate > toDate) {
                        oSrc.errormessage = "End date should be greater than start date."
                        args.IsValid = false
                        return true
                    } 
                } 
            }
            args.IsValid = true
            return false
        }
    </script>

</asp:Content>
