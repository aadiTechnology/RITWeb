<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="StandardwiseFeeConfigurationUI.aspx.cs" Inherits="StdwiseFeeConfigUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">

    <div class="MainBodyDiv" align="center">
        <table width="97%" align="center">
            <tr>
                <td>
                    <table id="LegendTable" runat="server" style="width: 100%;">
                    <tr>
                    <td align="right" colspan ="2">
                                            <span class="ClsMdtStar">*
                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label></span>
                                        </td>
                      </tr>
                    <tr>
                            <td align="right" colspan ="2">
                            <asp:LinkButton ID ="lnkbtnFeeStructureLink"  runat="server"  CssClass="SMSLblSMlBlue" Style="vertical-align: bottom;
                                            padding-left: 10px; font-size: 9pt; font-weight: bold; font-family: Verdana;">Fees Structure</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:ValidationSummary
                                    ID="valSumErrMsg" runat="server" CssClass="LblErrorMsg" ForeColor=""
                                    ShowMessageBox="False" ShowSummary="True" />
                                   
                            </td>
                            <td align ="center"> 
                              <asp:Label ID="lblSuccess" runat="server" ForeColor="Blue" Width="100%" 
                                                EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                             <asp:Label ID="lblError" runat="server" ForeColor="Red" Width="100%" 
                                                EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                            </td>
                            
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <table cellpadding="1" cellspacing="2">
                                    <tr>
                                        <td align="left" colspan="1">
                                            <asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                Font-Bold="True" Text="<%$ Resources:LocalizedResources, Legend%>" EnableViewState="false"></asp:Label></td>
                                        <td align="left" colspan="1">
                                            &nbsp;<asp:Label ID="TextBox1" runat="server" BackColor="#eaeaea" Height="20px"
                                                BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px" EnableViewState="False"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources,FeeTypeNotAssignToStandard%>"
                                                CssClass="ClsTextNormal" EnableViewState="false"></asp:Label></td>
                                        <td align="right" style="width: 5px">
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="TextBox2" runat="server" BackColor="#5dad8e" Height="20px" BorderColor="Black"
                                                BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px" EnableViewState="False"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label></td>
                                        <td align="left">
                                            <asp:Label ID="Label6" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources,TotalFeesConfigurationNotDone%>"
                                                CssClass="ClsTextNormal" EnableViewState="false"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 5px">
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="TextBox4" runat="server" BackColor="#aae2cd" Height="20px" BorderColor="Black"
                                                BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px" EnableViewState="False"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label></td>
                                        <td align="left">
                                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources, UpdateTotalFeesConfiguration%>"
                                                CssClass="ClsTextNormal" EnableViewState="false"></asp:Label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" >
                    <div id="divGridView" class="GridBorder" runat="server" visible="true" style="width: 600pt; overflow-y:hidden; overflow-x=auto">
                        <asp:GridView ID="grdFeeTypes" runat="server" AutoGenerateColumns="False" Height="100%"
                             AllowPaging="False" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                            GridLines="None" DataKeyNames="Standard_Id" EnableViewState="False">
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                            </PagerStyle>
                            <PagerSettings NextPageText="<%$ Resources:LocalizedResources, Next%>" LastPageText="<%$ Resources:LocalizedResources, Last%>" PreviousPageText="<%$ Resources:LocalizedResources, Previous%>"
                                FirstPageText="<%$ Resources:LocalizedResources, First%>" Position="TopAndBottom" Mode="NumericFirstLast"> </PagerSettings>
                            <Columns>
                                <asp:BoundField HeaderText="  " SortExpression="Standard_Id" DataField="Standard_Name">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" Width="10%" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" Width="10%" />
                                </asp:BoundField>
                            </Columns>
                            <RowStyle CssClass="ClsGridRow" />
                            <HeaderStyle CssClass="ClsGridHeader" />
                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                            <EmptyDataRowStyle CssClass="LblNoRecord" />
                        </asp:GridView>
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
            <td>
          <div id="divFeeStructure" runat="server" style=" visibility:hidden; display:none; position: absolute;
                    margin: 0px; padding: 0px; width: 415px; height: 160px; border-width: 0px; left: 5px;
                    top: 0px; line-height: normal; border: solid 2px darkgreen; margin: 100px 0px 0px 50px;
                    background-color: white; z-index: 100">
            <div style="background-color: Transparent; background-image: url(../images/GridHeaderBG.gif);
                        background-repeat: repeat-x; padding: 4px; color: #Black; text-align: right;">
                        <div style="padding: 1px;  font-size: 12px; font-weight: bold;
                            color: #Black; float: left">
                            <asp:Label ID="Label33" runat="server" Text="Add Fees Structure !!!"></asp:Label>
                        </div>
                        <span style="cursor: hand;" onclick="javascript:HidePopup();">
                            <img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
                        </span>
                    </div>
         <div style="padding: 2px; background-color: ThreeDFace WindowFrame; text-align: left;
                        width: 100%; vertical-align: top; color: #333; overflow: auto; height: 115px;"
                        id="PopupInfo">
           <table  style="width: 100%" >
           <tr style="height:20px;">
           <td></td>
           </tr>
                                    <tr id = "trfileuploadcontrol" runat = "server"  >
                                        <td  align="left">
                                             <span class="ClsLabel">File Path :</span>
                                        </td>
                                        <td align="left">                                            
                                             <asp:FileUpload ID="fileUploadItems" runat="server" ToolTip="Only PDF files are allowed" Width="180px" /><span class="ClsMdtStar">*</span>
                                             </td>
                                             <td align="left" style="width:25px">
                                             <asp:ImageButton ID="btnDelete" runat="server"  CausesValidation="false" ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" OnClientClick="return ConfirmDelete()" onclick="btnDelete_Click" /> 
                                             </td>
                                             <td align="left" style="width:25px">
                                             <asp:ImageButton ID="btnView" runat="server"  CausesValidation="false" ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif"   /> 
                                             </td>
                                     </tr>
                                      <tr id = "trfileuploadnote" runat = "server" >
                                        
                                         <td align="left" class="paddingL" colspan="3">
                                                <span class="LblSmlGray">(Supports only .PDF file type. File size should not exceed
                                                   1 MB.)</span>
                                            </td>
                                        
                                        </tr>
                                       <%--  <tr style="height:20px">
                                        <td>
                                        </td>
                                        </tr>--%>
                                     <tr style="width:100%">
                                        <td align="center" colspan=3 class="style1">
                                             <asp:Button ID="btnSave" runat="server" 
                                                                Text="<%$ Resources:LocalizedResources, Save%>" CssClass="ClsBtn"  
                                                               onclick="btnSave_Click" OnClientClick ="if(!IsFileValid())return false;" />
                                        </td>
                                     </tr>
                                </table>
                                </div>
            </td>
            </tr>
            <asp:HiddenField ID="hidTeacherName" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hidCultureInfo" runat="server" />
            <tr id="Tr2" runat="server" enableviewstate ="false">
                <td align="center" colspan="1">
                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back%>" CssClass="ClsBtn" OnClick="btnBack_Click" UseSubmitBehavior="false" />
                </td>
            </tr>
        </table>
    </div>
    
    <script language="javascript" type="text/javascript">
        _ClientfileUploadItems = "<%=this.fileUploadItems.ClientID %>";
        _ClientlblSuccess = "<%=this.lblSuccess.ClientID %>";
        _ClientlblError = "<%=this.lblError.ClientID %>";

        //This function is used to open popun on click on link news.
        function OpenWindow(sfilepath) {
            window.open(sfilepath, '_new', 'scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600');
            return false;
        }

        //This function is used to open popun on click on link news.
        function ShowFeeStructurePopup() {
            document.getElementById(_ClientfileUploadItems).value = '';
            document.getElementById(_ClientlblSuccess).innerHTML = '';
            document.getElementById(_ClientlblError).innerHTML = '' ;
            var x, y, tt_ovr_
            var cssstyle = $get("<%=this.divFeeStructure.ClientID %>").style
            var width = 600
            var height = 380
            var left = parseInt((screen.width / 2) - (width / 2))
            var top = parseInt((screen.height / 2) - (height / 2))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            cssstyle.visibility = "visible"
            cssstyle.display = "block"
        }

        //This function is used to close popup.
        function HidePopup() {
            $get("<%=this.divFeeStructure.ClientID %>").style.visibility = "hidden"
            $get("<%=this.divFeeStructure.ClientID %>").style.display = "none"
            return false
        }

        //This function is used take confirmation about delete.
        function ConfirmDelete() {
            return window.confirm('Are you sure you want to delete current fee structure file?')
        }

        //This function is used to validate is file uploaded by user or not
        function IsFileValid() {
                    if (document.getElementById(_ClientlblSuccess)) {
                document.getElementById(_ClientlblSuccess).innerHTML = "";
                document.getElementById(_ClientlblSuccess).innerText = "";
            }

            if (document.getElementById(_ClientlblError)) {
                document.getElementById(_ClientlblError).innerHTML = "";
                document.getElementById(_ClientlblError).innerText = "";
            }

           

            var lblUFileNameval = "";
            var myImage = document.getElementById(_ClientfileUploadItems).value;
            var fileUpload = $get(_ClientfileUploadItems);
            if (myImage == "") {
                alert('File to be uploaded should be selected.');
                return false;
            }
            else {
                if (myImage.substr(myImage.lastIndexOf('.'), 4).toUpperCase() == ".PDF") {
                            return true;
                }
                else {
                    alert('Invalid file type.');
                    return false;
                }
            }
        }
        
    </script>
  
</asp:Content>

