<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="BookBarcodeUI.aspx.cs" Inherits="BookBarcodeUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <center>
        <div class="MainBodyDiv" style="width: 850px">            
            <table width="98%" align="center">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                                Height="20px" Width="100%" CssClass="ClsMdtStar" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td>
                                            <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                                Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ValidationSummary ID="valsumBarcode" runat="server" CssClass="LblErrorMsg" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <table cellpadding="0" cellspacing="2" align="center" width="80%">
                            <tr align="center">
                                <td align="center">
                                    <asp:UpdatePanel ID="upnl3" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table align="center" width="80%">
                                                <tr align="center">
                                                    <td align="center" style="width: 600px;">
                                                        <table align="center" style="width: 600px;">
                                                            <tr>
                                                                <td class="ClsBorderLight " style="width: 525px">
                                                                    <span id="lblBookName" class="ClsLabel">Book Title :</span>
                                                                </td>
                                                                <td style="width: 172px">
                                                                    <asp:TextBox ID="txtBookName" runat="server" CssClass="SmlTxtBox" MaxLength="100"
                                                                        Width="129px"></asp:TextBox><span style="color: red"></span>
                                                                </td>
                                                                <td class="ClsBorderLight">
                                                                    <span id="lblAccessionNumber" class="ClsLabel" style="width: 119px">Accession Number
                                                                        :</span>
                                                                </td>
                                                                <td style="width: 130">
                                                                    <asp:TextBox ID="txtAccessionNumber" runat="server" CssClass="MidTxtBox" MaxLength="100"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="ClsBorderLight" style="width: 525px">
                                                                    <span id="lblAuthorName" class="ClsLabel">Author :</span>
                                                                </td>
                                                                <td style="width: 172px">
                                                                    <asp:TextBox ID="txtAuthorName" runat="server" CssClass="SmlTxtBox" MaxLength="100"
                                                                        Width="129px"></asp:TextBox><span style="color: red"></span>
                                                                </td>
                                                                <td class="ClsBorderLight" style="width: 102px">
                                                                    <span id="lblMediaType" class="ClsLabel">Media Type :</span>
                                                                </td>
                                                                <td style="width: 211px">
                                                                    <div>
                                                                        <asp:RadioButton ID="optAll" runat="server" CssClass="ClsLabel" Text="All" GroupName="GrpMediaType"
                                                                            AutoPostBack="True" OnCheckedChanged="optAll_CheckedChanged" />
                                                                        <asp:RadioButton ID="optPrintable" runat="server" CssClass="ClsLabel" Text="Printable"
                                                                            GroupName="GrpMediaType" AutoPostBack="True" OnCheckedChanged="optPrintable_CheckedChanged" />
                                                                        <asp:RadioButton ID="optNonPrintable" runat="server" CssClass="ClsLabel" Text="NonPrintable"
                                                                            GroupName="GrpMediaType" AutoPostBack="True" OnCheckedChanged="optNonPrintable_CheckedChanged" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="ClsBorderLight" style="width: 525px">
                                                                    <span id="lblPublisher" class="ClsLabel">Publisher :</span>
                                                                </td>
                                                                <td style="width: 172px">
                                                                    <asp:TextBox ID="txtPublisher" runat="server" CssClass="SmlTxtBox" MaxLength="100"
                                                                        Width="129px"></asp:TextBox><span style="color: red"></span>
                                                                </td>
                                                                <td class="ClsBorderLight" style="width: 102px">
                                                                    <span id="Span9" class="ClsLabel">Category :</span>
                                                                </td>
                                                                <td align="left" style="padding-right: 15px;">
                                                                    <div>
                                                                        <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:DropDownList ID="cmbMainCategory" runat="server" CssClass="MidTxtBox">
                                                                                </asp:DropDownList>
                                                                            </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:AsyncPostBackTrigger ControlID="optAll" EventName="CheckedChanged" />
                                                                                <asp:AsyncPostBackTrigger ControlID="optPrintable" EventName="CheckedChanged" />
                                                                                <asp:AsyncPostBackTrigger ControlID="optNonPrintable" EventName="CheckedChanged" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="ClsBorderLight" style="width: 525px">
                                                                    <span id="lblDescription" class="ClsLabel">Description :</span>
                                                                </td>
                                                                <td style="width: 172px">
                                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="MidTxtBox" MaxLength="100"
                                                                        Width="129px"></asp:TextBox><span style="color: red"></span>
                                                                </td>
                                                                <td class="ClsBorderLight" style="width: 102px">
                                                                    <span id="Span3" class="ClsLabel">Standard :</span>
                                                                </td>
                                                                <td align="left" style="padding-right: 15px;">
                                                                    <div>
                                                                        <asp:DropDownList ID="cmbStandard" runat="server" CssClass="MidTxtBox">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            
                                                           
                                                            <tr>
                                                                <td class="ClsBorderLight" style="width: 525px">
                                                                    <span id="Span1" class="ClsLabel">Display Records From :</span>
                                                                </td>
                                                                <td style="width: 172px">
                                                                    <asp:TextBox ID="txtDisplayFrom" runat="server" CssClass="SmlTxtBox" onblur="extractNumber(this,0,false);"
                                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                                        onkeyup="extractNumber(this,0,false);" onpaste="event.returnValue=false" MaxLength="6"
                                                                        Width="129px"></asp:TextBox>
                                                                </td>
                                                                <td class="ClsBorderLight" style="width: 102px">
                                                                    <span id="Span2" class="ClsLabel">To :</span>
                                                                </td>
                                                               <td align="left" style="padding-right: 15px;">
                                                                    <asp:TextBox ID="txtDisplayTo" runat="server" CssClass="MidTxtBox" onblur="extractNumber(this,0,false);"
                                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                                        onkeyup="extractNumber(this,0,false);" onpaste="event.returnValue=false" MaxLength="6"></asp:TextBox>
                                                                        
                                                                </td>
                                                                 
                                                            </tr>
                                                             <tr>
                                                                <td class="ClsBorderLight" style="width: 525px">
                                                                    <span id="Span4" class="ClsLabel">Accession From Number  :</span>
                                                                </td>
                                                                <td style="width: 172px">
                                                                 <asp:TextBox ID="txtPrefix" runat="server" CssClass="SmlTxtBox" 
                                                                        Width="60px" placeholder="Prefix" MaxLength="5"></asp:TextBox>
                                                                    <asp:TextBox ID="txtAccessionFromNumber" runat="server" CssClass="SmlTxtBox" onblur="extractNumber(this,0,false);"
                                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                                        onkeyup="extractNumber(this,0,false);" onpaste="event.returnValue=false" MaxLength="5"
                                                                        Width="60px"></asp:TextBox>
                                                                        
                                                                </td>
                                                                
                                                                <td class="ClsBorderLight" style="width: 102px">
                                                                    <span id="Span5" class="ClsLabel">To :</span>
                                                                </td>
                                                                <td align="left" style="padding-right: 15px;">
                                                                    <asp:TextBox ID="txtAccessionTo" runat="server" CssClass="MidTxtBox" Width="90px" onblur="extractNumber(this,0,false);"
                                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                                        onkeyup="extractNumber(this,0,false);" onpaste="event.returnValue=false" MaxLength="5"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr >
                                                          
                                                             <td align="left" colspan="1" class="ClsBorderlight " style="width: 30%; background-color: #ffffc4;">
                                                <asp:Label ID="Label" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note 1 :"
                                                    CssClass="LblNrmlB" EnableViewState="False"></asp:Label>
                                            </td><td align="left" colspan="4" class="ClsBorderlight" style="padding-left: 5px; width: 70%">
                                                <asp:Label ID="Label4" runat="server" BorderWidth="0px" Text="Display record fields - Displays records 
                                                                    according to serial count."
                                                    CssClass="LblSmlV" EnableViewState="False"></asp:Label>
                                            </td>
                                                                
                                                    </tr>
                                                    <tr >
                                                          
                                                             <td align="left" colspan="1" class="ClsBorderlight " style="width: 30%; background-color: #ffffc4;">
                                                <asp:Label ID="Label1" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note 2 :"
                                                    CssClass="LblNrmlB" EnableViewState="False"></asp:Label>
                                            </td><td align="left" colspan="4" class="ClsBorderlight" style="padding-left: 5px; width: 70%">
                                                <asp:Label ID="Label2" runat="server" BorderWidth="0px" Text=" Accession fields - Displays records according to accessions present ."
                                                    CssClass="LblSmlV" EnableViewState="False"></asp:Label>
                                            </td>
                                                                
                                                    </tr>
                                                            <tr>
                                                           
                                                                <td align="center" colspan="4">
                                                                
                                                                    <asp:Button ID="btnSearch" runat="server" CssClass="ClsBtn" Font-Bold="True" Text="Search"
                                                                        OnClick="btnSearch_Click" />
                                                                    <asp:Button ID="btnClear" runat="server" CssClass="ClsBtn" Font-Bold="True" Text="Clear"
                                                                        OnClick="btnClear_Click" CausesValidation="False" UseSubmitBehavior="true" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td colspan="2">
                                            <table align="center" width="200px" id="tblBookCount" runat="server" visible="false">
                                                <tr>
                                                    <td align="center">
                                                        <span class="ClsLabel" style="font-weight: bold;">Total Book Count : </span>
                                                        <asp:Label ID="lblBookCount" runat="server" Text="Book Count" CssClass="ClsLabel"
                                                            Style="font-weight: bold;" EnableViewState="true"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td align="center" colspan="2">
                                            <asp:Panel ID="pnlBooks" runat="server" Height="300px" Visible="false">
                                                <div id="DivBookDetailsContainer" runat="server" visible="true" class="GridBorder"
                                                    style="width: 100%; height: 300px; overflow: scroll;">
                                                    <asp:ListView ID="lstvwBookMaster" runat="server" DataKeyNames="Book_Title" OnSorting="lstvwBookMaster_Sorting"
                                                        OnDataBound="lstvwBookMaster_DataBound">
                                                        <LayoutTemplate>
                                                            <table align="center" width="100%" runat="server" id="tblShiftInfo" style="color: #333333"
                                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="left" class="paddingLSML">
                                                                        Book Title
                                                                    </th>
                                                                    <th align="left" class="paddingLSML">
                                                                        Accession No.
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                <td align="left" class="paddingLSML">
                                                                    <asp:Label ID="lblBookName" runat="server" Text='<%# Eval(" Book_Title") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingLSML">
                                                                    <asp:Label ID="lblAuthor" runat="server" Text='<%# Eval(" Book_No") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                <td align="left" class="paddingLSML">
                                                                    <asp:Label ID="lblBookName" runat="server" Text='<%# Eval(" Book_Title") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingLSML">
                                                                    <asp:Label ID="lblAuthor" runat="server" Text='<%# Eval(" Book_No") %>'></asp:Label>
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
                                                    <asp:HiddenField ID="hidSubListViewCnt" runat="server" />
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td align="right" width="50%">
                                            <asp:Button UseSubmitBehavior="false" ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn"
                                                CausesValidation="False" />
                                        </td>
                                        <td align="left">
                                            <asp:Button ID="btnGenerateBarcode" runat="server" CssClass="ClsBtn" Font-Bold="True"
                                                Enabled="false" CausesValidation="false" Text="Generate Barcode" Width="118px" />
                                            <asp:CompareValidator ID="cmpValRecords" runat="server" Display="None" ErrorMessage="The value of the 'To' record range should be greater than value of 'From' record range."
                                                CssClass="clsLbl" ControlToCompare="txtDisplayTo" ControlToValidate="txtDisplayFrom"
                                                Operator="LessThanEqual" Type="Integer"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hidMediaType" runat="server" />
            <asp:HiddenField ID="hidBookSortExpression" runat="server" Value="Book_Title" />            
        </div>
    </center>
    
</asp:Content>
