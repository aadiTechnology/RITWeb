<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="ReturnRenewUI.aspx.cs" Inherits="ReturnRenewUI" EnableEventValidation="false"%>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <center>
        <div class="MainBodyDiv">
            <table width="100%">
                <tr style="width: 100%">
                    <td align="center" style="width: 100%">
                        <asp:Panel ID="pnlInput" runat="server" Width="100%">
                            <asp:UpdatePanel ID="UPanelValSum" runat="server">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td align="left" style="width: 100%">
                                                <asp:Label ID="lblError" runat="server" CssClass="LblErrorMsg" EnableViewState="False"
                                                    Width="100%"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="4" style="height: 18px">
                                                <asp:Label ID="lblMessage" runat="server" CssClass="LblNormalImg" Font-Bold="True"
                                                    ForeColor="Blue" Visible="False" EnableViewState="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <table id="tblReturnRenew" runat="server" align="center" width="100%">
                                <tr>
                                    <td>
                                        <asp:ValidationSummary ID="valsumReturnRenewBook" runat="server" CssClass="LblErrorMsg" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ChildrenAsTriggers="false"
                                            ID="UpdatePanel1">
                                            <ContentTemplate>
                                                <table align="center" width="745pt">
                                                    <tr>
                                                        <td valign="middle" class="ClsBorderlight paddingL" style="width: 200px;">
                                                            <span class="ClsLabel">Book Barcode :</span>
                                                        </td>
                                                        <td valign="middle" align="left">
                                                            <asp:TextBox ID="txtBarcode" runat="server" MaxLength="10" CssClass="MidTxtBox" TabIndex="1"
                                                                OnTextChanged="txtBarcode_TextChanged" AutoPostBack="True"></asp:TextBox>
                                                        </td>
                                                        <td valign="middle" class="ClsBorderlight paddingL" style="width: 170px;">
                                                            <span class="ClsLabel">Accession Number :</span>
                                                        </td>
                                                        <td valign="middle" align="left">
                                                            <asp:TextBox ID="txtBookID" runat="server" MaxLength="10" CssClass="MidTxtBox" TabIndex="2"></asp:TextBox>
                                                        </td>
                                                       <%-- <td rowspan="4" align="center">
                                                            &nbsp;</td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle" class="ClsBorderlight paddingL" style="width: 110px">
                                                            <span class="ClsLabel">User Name :</span>
                                                        </td>
                                                        <td valign="middle" align="left">
                                                            <asp:TextBox ID="txtUserName" runat="server" CssClass="MidTxtBox" MaxLength="50" autocomplete="off"
                                                                TabIndex="3"></asp:TextBox>
                                                        </td>
                                                        <td class="ClsBorderlight paddingL">
                                                            <span class="ClsLabel">Class :</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbClass" runat="server" Width="148px" TabIndex="4">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderlight paddingL"  valign="middle">
                                                            <span>Show Deactivated Users :</span></td>
                                                        <td >
                                                         <asp:CheckBox CssClass="ClsLabel" runat="server" ID="chkShowDeactivatedUser" TabIndex="5"/></td>
                                                        
                                                    </tr>
                                                    <tr align="center">
                                                        
                                                        <td colspan="4">
                                                            <asp:Button ID="btnUserBookSearch" runat="server" CssClass="ClsBtn" 
                                                                EnableViewState="False" OnClick="btnUserBookSearch_Click" TabIndex="6" 
                                                                Text="Search" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnUserBookSearch" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="grdReturnRenewBooks" EventName="RowCommand" />
                                                <asp:AsyncPostBackTrigger ControlID="btnReturnBook" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnBookRemove" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="txtBarcode" EventName="TextChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="btnLateFee" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ChildrenAsTriggers="true"
                                            ID="UpdatePanel3">
                                            <ContentTemplate>
                                                <table width="950px">
                                                    <tr align="right">
                                                        <td width="27%" align="left">
                                                            <table>
                                                                <caption>
                                                                    <tr>
                                                                        <td style="width: 56px">
                                                                            <asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" EnableViewState="false"
                                                                                Font-Bold="True" Text="Legend : "></asp:Label>
                                                                        </td>
                                                                        <td align="center" style="border: 1px solid #000000;" valign="middle">
                                                                            <asp:Label ID="Label1" runat="server" BackColor="Pink" BorderStyle="None" BorderWidth="1px"
                                                                                CssClass="ClsLblLgnd" EnableViewState="False" Font-Bold="False" ForeColor="Black"
                                                                                ReadOnly="True" Text="Late Return" Width="110px"></asp:Label>
                                                                        </td>
                                                                        <td align="center" style="border: 1px solid #000000;" valign="middle">
                                                                            <asp:Label ID="Label4" runat="server" BackColor="Gainsboro" BorderStyle="None" BorderWidth="1px"
                                                                                CssClass="ClsLblLgnd" EnableViewState="False" Font-Bold="False" ForeColor="Red"
                                                                                ReadOnly="True" Text="Deactivated User" Width="110px"></asp:Label>
                                                                        </td>
                                                                        <caption>
                                                                            <img  height="20px" src="../images/spacer.gif" width="20px" />
                                                                        </caption>
                                                                    </tr>
                                                                </caption>
                                                        </td>
                                                    </tr>
                                                </table>
                                                </td> </tr>
                                                <tr>
                                                    <td runat="server" id="trTotalRecId" width="46%" align="center" style="width: 68%">
                                                        <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                                        <span class="LblNormal">To</span>
                                                        <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                        <span class="LblNormal">Out Of</span>
                                                        <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                                        <span class="LblNormal">Records</span> &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Panel ID="pnlGid" runat="server" Width="950px" ScrollBars="Horizontal" CssClass="GridBorder">
                                                            <asp:GridView ID="grdReturnRenewBooks" runat="server" AutoGenerateColumns="False"
                                                                Height="100%" Width="100%" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                                                                GridLines="None" DataKeyNames="Book_No,User_Role_Id,No_Of_Attempt_Renew,Renew_Attempt,Issue_Id,Book_ID,
                                                                SchoolLeft_Date,Is_Locked,Late_Fee_Per_Day,Book_Issued_To,BookReserveUserList,IsForParent,ParentRenewAttempt"
                                                                BackColor="White" EmptyDataText="No record found." AllowPaging="True" AllowSorting="True"
                                                                PageSize="20" DataSourceID="GrdDSobj" OnRowDataBound="grdRetuenRenewBooks_RowDataBound"
                                                                OnRowCreated="grdRetuenRenewBooks_RowCreated" OnSorting="grdRetuenRenewBooks_Sorting"
                                                                OnRowCommand="grdRetuenRenewBooks_RowCommand" OnPageIndexChanging="grdRetuenRenewBooks_PageIndexChanging" TabIndex="7">
                                                                <Columns>
                                                                    <asp:BoundField HeaderText="Accession Number" DataField="Book_No" HtmlEncode="False"
                                                                        SortExpression="Book_No">
                                                                        <ItemStyle HorizontalAlign="Left" CssClass="paddingLSML GridDate" VerticalAlign="Middle"
                                                                            Wrap="False" />
                                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="GridDate" Wrap="False"
                                                                            Width="12%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField HeaderText="Class / Designation" DataField="Class" HtmlEncode="false"
                                                                        SortExpression="StdDivMst.Standard_Id">
                                                                        <ItemStyle HorizontalAlign="Left" CssClass="paddingLSML GridDate" VerticalAlign="Middle"
                                                                            Wrap="false" />
                                                                        <HeaderStyle HorizontalAlign="Left" Wrap="false" CssClass="GridDate" Width="15%"
                                                                            VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Issued_Name" HeaderText="User Name" HtmlEncode="False"
                                                                        SortExpression="Issued_Name">
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Wrap="False" />
                                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML"
                                                                            Width="10%" Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField HeaderText="Book Title" DataField="Book_Title" HtmlEncode="False"
                                                                        SortExpression="Book_Title">
                                                                        <ItemStyle HorizontalAlign="Left" CssClass="paddingLSML" VerticalAlign="Middle" Wrap="false" />
                                                                        <HeaderStyle HorizontalAlign="Left" CssClass="paddingLSML" VerticalAlign="Middle"
                                                                            Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField HeaderText="Author(s)" DataField="Author_Name" HtmlEncode="False"
                                                                        SortExpression="Author_Name">
                                                                        <ItemStyle HorizontalAlign="Left" CssClass="paddingLSML" VerticalAlign="Middle" Wrap="false" />
                                                                        <HeaderStyle HorizontalAlign="Left" CssClass="paddingLSML" VerticalAlign="Middle"
                                                                            Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField HeaderText="Rack/Shelf" DataField="RackShelfNo" HtmlEncode="False"
                                                                        NullDisplayText="-">
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="True" CssClass="GridDate" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="True" CssClass="GridDate" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Issue_Date" HeaderText="Issued Date" HtmlEncode="False"
                                                                        SortExpression="Issue_Date">
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" CssClass="GridDate" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" CssClass="GridDate" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Return_Date" HeaderText="Return Date" HtmlEncode="False"
                                                                        SortExpression="Return_Date">
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" CssClass="GridDate" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" CssClass="GridDate" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Issued To Parent?">
                                                                        <HeaderStyle Width="10%" Wrap="false" />
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="imgBtnForParent" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Return">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnReturn" runat="server" CommandName="RETURN_BOOK" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                ImageUrl="~/RITeSchool/images/book_submit_2.gif" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="GridDate" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Renew">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnRenew" runat="server" CommandName="RENEW_BOOK" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                ImageUrl="~/RITeSchool/images/book_Renew_2.gif" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="GridDate" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Lost Book">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                ImageUrl="~/RITeSchool/images/Bool_Lost_2.gif" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" CssClass="GridDate" />
                                                                    </asp:TemplateField>
                                                                    <asp:HyperLinkField HeaderText="Late Fee" Visible="False">
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="paddingLSML"
                                                                            Width="5%" Wrap="False" />
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:HyperLinkField>
                                                                </Columns>
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
                                                                <RowStyle CssClass="ClsGridAltRow" />
                                                                <HeaderStyle CssClass="ClsGridHeader" />
                                                                <AlternatingRowStyle CssClass="ClsGridRow" />
                                                                <EmptyDataRowStyle CssClass="LblNoRecord" BackColor="#E6EEFC" HorizontalAlign="Center" />
                                                            </asp:GridView>
                                                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                            <asp:HiddenField ID="hidBookNo" runat="server" />
                                                            <asp:HiddenField ID="hidReturnDate" runat="server" />
                                                            <asp:HiddenField ID="hidIssueDate" runat="server" />
                                                            <asp:HiddenField ID="hidRowIndex" runat="server" />
                                                            <asp:HiddenField ID="hidActReturnDate" runat="server" />
                                                            <asp:HiddenField ID="hidReason" runat="server" />
                                                            <asp:HiddenField ID="hidBookId" runat="server" />
                                                            <asp:HiddenField ID="hidBookDetailsID" runat="server" />
                                                            <asp:HiddenField ID="hidLateFeeAmt" runat="server" />
                                                            <asp:HiddenField ID="hidUserId" runat="server" />
                                                            <asp:HiddenField ID="hidCommandName" runat="server" />
                                                            <asp:HiddenField ID="hidRowNo" runat="server" />
                                                            <asp:HiddenField ID="hidReturnBookID" runat="server" />
                                                        </asp:Panel>
                                                        <asp:ObjectDataSource ID="GrdDSobj" runat="server" TypeName="BusinessLogic.IssueReturnBookBL"
                                                            EnablePaging="true" SelectMethod="GetAllIssueBooks" SortParameterName="sortExpression"
                                                            SelectCountMethod="CountRows" EnableCaching="false" OnSelected="GrdDSobj_Selected">
                                                            <SelectParameters>
                                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="Int32" />
                                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                    Type="Int32" />
                                                                <asp:ControlParameter ControlID="txtUserName" Name="asUserName" PropertyName="Text" />
                                                                <asp:ControlParameter ControlID="txtBookID" Name="asBookNo" PropertyName="Text" />
                                                                <asp:ControlParameter ControlID="hidBookDetailsID" Name="aiBookDetailsID" Type="Int32" />
                                                                <asp:ControlParameter ControlID="hidReturnBookID" Name="aiReturnBookID" Type="String" />
                                                                <asp:ControlParameter ControlID="cmbClass" Name="aiStdDivId" Type="Int32" />
                                                                <asp:ControlParameter ControlID="chkShowDeactivatedUser" Name="aiDeactivatedUser" Type="Int32" PropertyName="Checked" />
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                    </td>
                                                </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnUserBookSearch" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="grdReturnRenewBooks" EventName="RowCommand" />
                                                <asp:AsyncPostBackTrigger ControlID="grdReturnRenewBooks" EventName="DataBound" />
                                                <asp:AsyncPostBackTrigger ControlID="btnReturnBook" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnBookRemove" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="txtBarcode" EventName="TextChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="btnLateFee" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divMain" runat="server" class="overlay" style="visibility: hidden; display: none;">
                        </div>
                        <div id="updtpnlPopUp" runat="server" style="visibility: hidden; display: none; position: absolute;
                            margin: 0px; padding: 0px; width: 350px; height: 150px; border-width: 0px; left: 0px;
                            top: 0px; line-height: normal; width: auto; border: solid 1px black; margin: 0px 0px 0px 5px;
                            background-color: white; filter: progid:DXImageTransform.Microsoft.dropshadow(OffX=5, OffY=5, Color=#7D7E7E);">
                            <div class="ReturnRenewPopop">
                                <div style="padding: 1px; font-size: 12px; font-weight: bold; color:Black; float: left;">
                                    Book Return Date</div>
                                <span style="cursor: hand" onclick="javascript:HidePopup();">
                                    <img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
                                </span>
                            </div>
                            <div style="padding: 10px; text-align: left;" class="ClsLabel">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel4" ChildrenAsTriggers="True" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="240px">
                                            <tr>
                                                <td colspan="2" align="left">
                                                    <asp:Label ID="lblSchoolleaving" runat="server" Text="Return date :" CssClass="LblNormal" />
                                                    <asp:TextBox ID="txtReturnDate" Width="100px" CssClass="SmlCombo" runat="server"
                                                        MaxLength="11"></asp:TextBox>
                                                    <rjs:PopCalendar ID="caltxtReturnDate" runat="server" Control="txtReturnDate" ShowErrorMessage="false"
                                                        To-Today="false" Format="dd MMM yyyy" ShowWeekend="True" Separator="-" />
                                                    <span style="color: #ff0000">*</span>
                                                    <asp:CustomValidator ID="custReturnDate" Display="None" runat="server" CssClass="ClsMdtStar"
                                                        ErrorMessage="Return date should not be blank." Visible="true" EnableClientScript="true"
                                                        ClientValidationFunction="IsValidReturnDate"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 130px;">
                                                    <asp:Label ID="lblLateFee" CssClass="LblNormal" Text="Late Fee :" runat="server"></asp:Label>
                                                </td>
                                                <td style="width: 220px;">
                                                    <asp:TextBox ID="txtLateFee" Width="100px" CssClass="SmlCombo" MaxLength="3" runat="server"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                                    ondrop="event.returnValue=false;" > </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <asp:Button ID="btnReturnBook" runat="server" Text="OK" CssClass="ClsBtn" OnClick="btnLateFee_Click" />
                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="false"
                                                        OnClientClick="javascript:HidePopup();return false;" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divMainRemove" runat="server" class="overlay" style="visibility: hidden;
                            display: none;">
                        </div>
                        <div id="updtpnlRemovePopUp" runat="server" style="visibility: hidden; display: none;
                            position: absolute; margin: 0px; padding: 0px; width: 250px; height: 230px; border-width: 0px;
                            left: 0px; top: 0px; line-height: normal; width: auto; border: solid 1px black;
                            margin: 0px 0px 0px 0px; background-color: white; filter: progid:DXImageTransform.Microsoft.dropshadow(OffX=5, OffY=5, Color=#7D7E7E);">
                            <div style="background-color: Transparent; background-image: url(../images/GridHeaderBG.gif);
                                background-repeat: repeat-x; padding: 4px; color: #Black; text-align: right;">
                                <div style="padding: 1px; font-size: 12px; font-weight: bold; color: #Black; float: left;">
                                    Lost Book</div>
                                <span style="cursor: hand" onclick="javascript:HideRemovePopup();">
                                    <img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
                                </span>
                            </div>
                            <div style="padding: 10px; text-align: left;" class="ClsLabel">
                                <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
                                    ID="UpdatePanel2">
                                    <ContentTemplate>
                                        <table width="250px">
                                            <tr align="left">
                                                <td colspan="2">
                                                    <asp:Label ID="lblReasonForLoss" runat="server" Text="Reason for loss of book :"
                                                        Font-Size="9" ForeColor="#000333" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtReason" CssClass="SmlCombo" runat="server" Height="80px" Width="95%"
                                                        TextMode="MultiLine"></asp:TextBox>
                                                    <span style="color: #ff0000">*</span>
                                                    <asp:CustomValidator ID="cstvalBookLost" runat="server" ErrorMessage="Reason for loss of book should not be blank."
                                                        CssClass="ClsMdtStar" Visible="true" EnableClientScript="true" Display="None"
                                                        ClientValidationFunction="validateBookLost"></asp:CustomValidator>
                                                    <asp:CustomValidator ID="cstvalBookLostLength" runat="server" ErrorMessage="Reason for loss of book should not exceed than 500 characters."
                                                        CssClass="ClsMdtStar" Visible="true" EnableClientScript="true" Display="None"
                                                        ClientValidationFunction="validateBookLostLength"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight  paddingL" style="width: 40%">
                                                    <asp:Label ID="lblLateFeeLost" runat="server" Text="Late Fee : "></asp:Label>
                                                </td>
                                                <td style="width: 55%">
                                                    <asp:TextBox ID="txtLateFeeLost" Width="90%" runat="server" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                                    ondrop="event.returnValue=false;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <asp:Button ID="btnBookRemove" runat="server" Text="OK" CssClass="ClsBtn" OnClick="btnLateFee_Click" />
                                                    <asp:Button ID="Button1" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="false"
                                                        OnClientClick="javascript:HideRemovePopup();return false;" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td id="tdLateFee" runat="server">
                        <div id="divMainLateFee" runat="server" class="overlay" style="visibility: hidden;
                            display: none;">
                        </div>
                        <div id="DivLateFeeAmt" runat="server" style="visibility: hidden; display: none;
                            position: absolute; margin: 0px; padding: 0px; width: 250px; height: 110px; border-width: 0px;
                            left: 0px; top: 0px; line-height: normal; width: auto; border: solid 1px black;
                            margin: 0px 0px 0px 0px; background-color: white; filter: progid:DXImageTransform.Microsoft.dropshadow(OffX=5, OffY=5, Color=#7D7E7E);">
                            <div style="background-color: Transparent; background-image: url(../images/GridHeaderBG.gif);
                                background-repeat: repeat-x; padding: 4px; color: #Black; text-align: right;">
                                <div style="padding: 1px; font-size: 12px; font-weight: bold; color: #Black; float: left;">
                                    Late Fee Amount:</div>
                                <span style="cursor: hand" onclick="javascript:HideLateFeePopup();">
                                    <img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
                                </span>
                            </div>
                            <div style="padding: 10px; text-align: left;" class="ClsLabel">
                                <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
                                    ID="UpdatePanel5">
                                    <ContentTemplate>
                                        <table width="250px">
                                            <tr align="center">
                                                <td>
                                                    <asp:Label ID="lblLateFeeAmt" runat="server" Text="Late Fee Amount :" Font-Size="9"
                                                        ForeColor="#000333" />
                                                    <asp:TextBox ID="txtAmt" CssClass="SmlCombo" runat="server" Height="20px" Width="40%" onblur="extractNumber(this,0,false);" 
                                                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                                    ondrop="event.returnValue=false;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="btnLateFee" runat="server" Text="OK" CssClass="ClsBtn" OnClientClick="javascript:HideLateFeePopup();"
                                                        OnClick="btnLateFee_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div runat="server" id="divErr">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2" style="height: 28px">
                        <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="ClsBtn"
                            OnClick="btnBack_Click" Text="Back" UseSubmitBehavior="false" EnableViewState="False" Visible="false"
                            TabIndex="8" />
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <script language="javascript" type="text/javascript">
        _clientLblError = "<%=this.lblError.ClientID %>"
        _clienttxtReturnDate = "<%=this.txtReturnDate.ClientID %>"
        _clientvalsumReturnRenewBook = "<%=this.valsumReturnRenewBook.ClientID %>"
        _clienttxtReason = "<%=this.txtReason.ClientID %>"
        _clientCstValRetDate = "<%=this.custReturnDate.ClientID %>"
        _clienthidIssueDate = "<%=this.hidIssueDate.ClientID %>"
        _clienthidReturnDate = "<%=this.hidReturnDate.ClientID %>"
        _clientgrdReturnRenewBooks = "<%=this.grdReturnRenewBooks.ClientID %>"
        _clienthidRoeIndex = "<%=this.hidRowIndex.ClientID %>"
        _clienttxtAmt = "<%=this.txtAmt.ClientID %>"
        _clienthidUserId = "<%=this.hidUserId.ClientID%>"
        _clienthidCommandName = "<%=this.hidCommandName.ClientID %>"
        _clienthidRowNo = "<%=this.hidRowNo.ClientID %>"
        _clienthidLateFeeAmt = "<%=this.hidLateFeeAmt.ClientID %>"
        _clienttxtLateFee = "<%=this.txtLateFee.ClientID %>"
        _clientlblLateFee = "<%=this.lblLateFee.ClientID %>"
        _clienttxtLateFeeLost = "<%=this.txtLateFeeLost.ClientID %>"
        _clientlblLateFeeLost = "<%=this.lblLateFeeLost.ClientID %>"
        _clientlblMessage = "<%=this.lblMessage.ClientID %>"

        function ShowLateFeePopup(e, sBookNo, iBookId, sLateFee, iUserId, sCommand) {

            if (document.getElementById(_clientlblMessage)) {
                document.getElementById(_clientlblMessage).innerHTML = "";
                document.getElementById(_clientlblMessage).innerText = "";
            }
            var x, y, tt_ovr_
            var cssstyle = $get("<%=this.DivLateFeeAmt.ClientID %>").style
            document.getElementById(_clienttxtReturnDate).value = ''
            var now = new Date()
            $get("<%=this.hidBookNo.ClientID %>").value = sBookNo
            $get("<%=this.hidBookId.ClientID %>").value = iBookId
            var width = 250
            var height = 180
            var left = parseInt((screen.width / 2) - (width / 2))
            var top = parseInt((screen.height / 2) - (height / 2))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            cssstyle.zIndex = Math.max((window.dd && dd.z) ? (dd.z + 2) : 0, 1010)
            cssstyle.visibility = "visible"
            cssstyle.display = "block"
            document.getElementById(_clienttxtAmt).value = sLateFee;
            document.getElementById(_clienthidUserId).value = iUserId;
            document.getElementById(_clienthidCommandName).value = sCommand;

        }
        function HideLateFeePopup() {
            $get("<%=this.DivLateFeeAmt.ClientID %>").style.visibility = "hidden"
            $get("<%=this.DivLateFeeAmt.ClientID %>").style.display = "none"
            var iLateFeeAmt = document.getElementById(_clienttxtAmt).value
            $get("<%=this.hidLateFeeAmt.ClientID %>").value = iLateFeeAmt
            var cssstyleMain = $get("<%=this.DivLateFeeAmt.ClientID %>").style
            cssstyleMain.visibility = "hidden"
            cssstyleMain.display = "none"
            return false
        }

        function ConfirmRenew(RenewMsg, IsConfirm, e, sBookNo, iBookId, sLateFee, iUserId, sCommand, iRowindex) {
            if (document.getElementById(_clientlblMessage)) {
                document.getElementById(_clientlblMessage).innerHTML = "";
                document.getElementById(_clientlblMessage).innerText = "";
            }
            var bResult = true
            if (document.getElementById(_clientLblError) != null)
                document.getElementById(_clientLblError).style.display = "none"

            if (IsConfirm == 'Confirm') {
                if (!window.confirm(RenewMsg)) {
                    bResult = false
                }
                else {
                    document.getElementById(_clienthidRowNo).value = iRowindex;
                    document.getElementById(_clienthidCommandName).value = sCommand;
                    if (sLateFee != 0 && sLateFee != "") {
                        ShowLateFeePopup(e, sBookNo, iBookId, sLateFee, iUserId, sCommand);
                        return false;
                    }
                }

            }
            else if (IsConfirm == 'Alert') {
                alert(RenewMsg)
                bResult = false
            }
            return bResult;
        }

        function HidePopup() {
            $get("<%=this.updtpnlPopUp.ClientID %>").style.visibility = "hidden"
            $get("<%=this.updtpnlPopUp.ClientID %>").style.display = "none"
            var validationResult = true
            if (typeof (Page_ClientValidate) == 'function') {
                validationResult = Page_ClientValidate("")
            }
            if (validationResult == false) {
                return false
            }
            var dtActReturnDate = document.getElementById(_clienttxtReturnDate).value
            $get("<%=this.hidActReturnDate.ClientID %>").value = dtActReturnDate
            var cssstyleMain = $get("<%=this.divMain.ClientID %>").style
            cssstyleMain.visibility = "hidden"
            cssstyleMain.display = "none"
            return false
        }

        function ConfirmReturn(e, sBookNo, iBookId, iUserId, sCommand) {
            if (document.getElementById(_clientlblMessage)) {
                document.getElementById(_clientlblMessage).innerHTML = "";
                document.getElementById(_clientlblMessage).innerText = "";
            }
            var bResult = true
            var validationResult = true
            var iLateFee = 0
            if (typeof (Page_ClientValidate) == 'function') {
                validationResult = Page_ClientValidate("")
            }
            if (validationResult == false) {
                return false
            }

            sBookNo = $get("<%=this.hidBookNo.ClientID %>").value
            var sMsg = "Are you sure you want to Return this book?"
            if (!window.confirm(sMsg)) {
                bResult = false
            }
            else {
                HidePopup()
                iLateFee = document.getElementById(_clienthidLateFeeAmt).value;
                document.getElementById(_clienthidCommandName).value = sCommand;
            }

            return bResult
        }

        function ConfirmRemove(e, sBookNo, iBookId, iUserId, sCommand) {
            if (document.getElementById(_clientlblMessage)) {
                document.getElementById(_clientlblMessage).innerHTML = "";
                document.getElementById(_clientlblMessage).innerText = "";
            }
            var bResult = true
            var validationResult = true
            var iLateFee = 0
            sBookNo = $get("<%=this.hidBookNo.ClientID %>").value
            iBookId = $get("<%=this.hidBookId.ClientID %>").value
            if (typeof (Page_ClientValidate) == 'function') {
                validationResult = Page_ClientValidate("")
            }
            if (validationResult == false) {
                return false
            }
            var sMsg = "Are you sure you want to remove this Book?"
            if (!window.confirm(sMsg)) {
                bResult = false
            }
            else {
                HideRemovePopup();
                iLateFee = document.getElementById(_clienthidLateFeeAmt).value;
                document.getElementById(_clienthidCommandName).value = sCommand;
            }
            return bResult
        }

        function ShowPopup(e, sBookNo, ReturnDate, IssueDate, ServerDate, iLateFee, iRowIndex, iUserId) {
            if (document.getElementById(_clientlblMessage)) {
                document.getElementById(_clientlblMessage).innerHTML = "";
                document.getElementById(_clientlblMessage).innerText = "";
            }
            var x, y, tt_ovr_
            document.getElementById(_clienthidRowNo).value = iRowIndex;
            var cssstyle = $get("<%=this.updtpnlPopUp.ClientID %>").style
            var btnReturn = $get("<%=this.btnReturnBook.ClientID %>")
            var dtReturn = new Date(ReturnDate).format("dd-MMM-yyyy")
            var dtIssue = new Date(IssueDate).format("dd-MMM-yyyy")
            $get("<%=this.hidReturnDate.ClientID %>").value = dtReturn
            $get("<%=this.txtReturnDate.ClientID %>").value = ServerDate
            $get("<%=this.hidIssueDate.ClientID %>").value = dtIssue
            $get("<%=this.hidBookNo.ClientID %>").value = sBookNo
            document.getElementById(_clienthidUserId).value = iUserId;
            var width = 250
            var height = 110
            var left = parseInt((screen.width / 2) - (width / 2))
            var top = parseInt((screen.height / 2) - (height / 2))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            // Override the z-index of the topmost wz_dragdrop.js D&D item
            cssstyle.zIndex = Math.max((window.dd && dd.z) ? (dd.z + 2) : 0, 1010);
            cssstyle.visibility = "visible";
            cssstyle.display = "block";
            document.getElementById(_clienthidLateFeeAmt).value = iLateFee;

            var txtLateFee = document.getElementById(_clienttxtLateFee);
            var lblLateFee = document.getElementById(_clientlblLateFee);

            if (txtLateFee && iLateFee != 0) {
                document.getElementById(_clientlblLateFee).style.display = "";
                document.getElementById(_clienttxtLateFee).value = iLateFee;
            }
            else
                document.getElementById(_clientlblLateFee).style.display = "none";
            if (lblLateFee && iLateFee != 0)
                document.getElementById(_clienttxtLateFee).style.display = "";
            else
                document.getElementById(_clienttxtLateFee).style.display = "none";
        }
        function IsValidReturnDate(oSrc, args) {
            if ($get("<%=this.updtpnlPopUp.ClientID %>").style.visibility == "visible"
                && $get("<%=this.updtpnlRemovePopUp.ClientID %>").style.visibility == "hidden") {
                var ocstValRetDate = document.getElementById(_clientCstValRetDate)
                if (document.getElementById(_clienttxtReturnDate).value == '') {
                    if (ocstValRetDate != null) {
                        ocstValRetDate.innerHTML = 'Return date should not be blank.'
                        ocstValRetDate.errormessage = 'Return date should not be blank.'
                        args.IsValid = false
                        return true
                    }
                }
                else {
                    var dtActReturnDate = document.getElementById(_clienttxtReturnDate).value
                    var dtToday
                    var TodayDate = new Date().format("dd-MMM-yyyy")
                    if (document.all)
                        dtToday = new Date(TodayDate.replace('-', ' '))
                    else
                        dtToday = new Date(convertdate(TodayDate))
                    if (dtActReturnDate.length > 0) {
                        var ReturnDate, dtIssueDate
                        var IssueDate = document.getElementById(_clienthidIssueDate).value
                        if (document.all) {
                            ReturnDate = new Date(dtActReturnDate.replace('-', ' '))
                            dtIssueDate = new Date(IssueDate.replace('-', ' '))
                        }
                        else {
                            ReturnDate = new Date(convertdate(dtActReturnDate))
                            dtIssueDate = new Date(convertdate(IssueDate))
                        }
                        var strIssueDate = getDateString(dtIssueDate)
                        if (ReturnDate < dtIssueDate) {
                            ocstValRetDate.errormessage = "Return date should be greater than issue date. (i.e " + strIssueDate + " )."
                            ocstValRetDate.innerHTML = "Return date should be greater than issue date. (i.e " + strIssueDate + " )."
                            args.IsValid = false
                            return true
                        }
                        if (ReturnDate > dtToday) {
                            ocstValRetDate.errormessage = "Return date should not be future date.";
                            ocstValRetDate.innerHTML = "Return date should not be future date.";
                            args.IsValid = false
                            return true
                        }
                    }
                }
            }
            args.IsValid = true
            return false
        }

        function ShowRemovePopup(e, sBookNo, iBookId, iLateFee, iRowIndex, iUserId) {
            if (document.getElementById(_clientlblMessage)) {
                document.getElementById(_clientlblMessage).innerHTML = "";
                document.getElementById(_clientlblMessage).innerText = "";
            }
            var x, y, tt_ovr_
            var cssstyle = $get("<%=this.updtpnlRemovePopUp.ClientID %>").style
            document.getElementById(_clienttxtReason).value = ''
            $get("<%=this.lblReasonForLoss.ClientID %>").innerHTML = 'Reason for loss of book (' + sBookNo + ') :'
            var now = new Date()
            $get("<%=this.hidBookNo.ClientID %>").value = sBookNo
            $get("<%=this.hidBookId.ClientID %>").value = iBookId
            var width = 250
            var height = 180
            var left = parseInt((screen.width / 2) - (width / 2))
            var top = parseInt((screen.height / 2) - (height / 2))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            cssstyle.zIndex = Math.max((window.dd && dd.z) ? (dd.z + 2) : 0, 1010)
            cssstyle.visibility = "visible"
            cssstyle.display = "block"
            document.getElementById(_clienthidLateFeeAmt).value = iLateFee;
            document.getElementById(_clienthidRowNo).value = iRowIndex;
            document.getElementById(_clienthidUserId).value = iUserId;
            var txtLateFee = document.getElementById(_clienttxtLateFeeLost);
            var lblLateFee = document.getElementById(_clientlblLateFeeLost);

            if (txtLateFee && iLateFee != 0) {
                document.getElementById(_clienttxtLateFeeLost).style.display = "";
                document.getElementById(_clienttxtLateFeeLost).value = iLateFee;
            }
            else
                document.getElementById(_clienttxtLateFeeLost).style.display = "none";
            if (lblLateFee && iLateFee != "")
                document.getElementById(_clientlblLateFeeLost).style.display = "";
            else
                document.getElementById(_clientlblLateFeeLost).style.display = "none";

        }
        function HideRemovePopup() {
            $get("<%=this.updtpnlRemovePopUp.ClientID %>").style.visibility = "hidden"
            $get("<%=this.updtpnlRemovePopUp.ClientID %>").style.display = "none"
            if (document.getElementById(_clientvalsumReturnRenewBook) != null)
                document.getElementById(_clientvalsumReturnRenewBook).style.display = "none"
            var validationResult = true
            if (typeof (Page_ClientValidate) == 'function') {
                validationResult = Page_ClientValidate("")
            }
            if (validationResult == false) {
                return false
            }
            var sReason = document.getElementById(_clienttxtReason).value
            $get("<%=this.hidReason.ClientID %>").value = sReason
            var cssstyleMain = $get("<%=this.divMainRemove.ClientID %>").style
            cssstyleMain.visibility = "hidden"
            cssstyleMain.display = "none"
            return false
        }
        function validateBookLost(oSrc, args) {
            if ($get("<%=this.updtpnlPopUp.ClientID %>").style.visibility == "hidden"
                && $get("<%=this.updtpnlRemovePopUp.ClientID %>").style.visibility == "visible") {
                if (trimAll(document.getElementById(_clienttxtReason).value) == '') {
                    args.IsValid = false
                    return true
                }
            }
            args.IsValid = true
            return false
        }
        function validateBookLostLength(oSrc, args) {

            if ($get("<%=this.updtpnlPopUp.ClientID %>").style.visibility == "hidden"
            && $get("<%=this.updtpnlRemovePopUp.ClientID %>").style.visibility == "visible") {
                if (document.getElementById(_clienttxtReason).value.length > 500) {
                    args.IsValid = false
                    return true
                }
            }
            args.IsValid = true
            return false
        }
    </script>

    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            AutoSearch();
        });
        function AutoSearch() {
            var SchoolId = "<%=miSchoolId %>";
            _clienttxtRegNumber = '#<%=txtUserName.ClientID%>';
            var AcademicYearId = "<%=miAcademicYearId %>"
            BindAutoCompleteEventForUser(SchoolId, AcademicYearId, _clienttxtRegNumber, null, 1, null, null, null);
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        // This function is used to enabled controls once a postback is complete.

        function EndRequestHandler() {
            AutoSearch();
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtUserName.ClientID %>");
            bt = document.getElementById("<%=this.btnUserBookSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }

    </script>

</asp:Content>
